using System.Data;
using z.MVC5.Results;
using z.ERP.Entities;
using System.Collections.Generic;
using z.Extensions;
using System;
using z.ERP.Entities.Enum;
using z.Context;
using z.SSO;
using z.SSO.Model;
using System.Linq;
using z.Encryption;

namespace z.ERP.Services
{
    public class HomeService : ServiceBase
    {
        protected const string LoginSalt = "z.SSO.LoginSalt.1";
        internal HomeService()
        {
        }

        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns></returns>
        public DataGridResult GetMenu(SearchItem item)
        {
            string sql = @" select aa.id,
                                    aa.pid,
                                    ab.id menuid,
                                    aa.name,
                                    ab.url url,
                                    ab.platformid
                               from menutree aa, menu ab
                              where  aa.menuid = ab.id(+)  ";
            if (int.Parse(employee.Id) > 0)
            {
                sql += @" and (aa.menuid in (
                                                   select a.id
                                                     from menu a,
                                                           menuqx        b,
                                                           USER_ROLE c
                                                    where a.id = b.menuid
                                                      and b.roleid = c.roleid
                                                      and c.userid = " + employee.Id + @") or
                                    aa.menuid is null)";
            }

            sql += " order by aa.id  ";
            DataTable dt = DbHelper.ExecuteTable(sql);
            List<PLATFORMEntity> PlatFormList = DbHelper.SelectList(new PLATFORMEntity());
            foreach (DataRow dr in dt.Rows)
            {
                string url = dr["URL"].ToString();
                dr["PLATFORMID"].ToString().TryToInt(PlatFormId =>
              {
                  var pt = PlatFormList.Where(a => a.ID == PlatFormId.ToString()).FirstOrDefault(a => url.IsRegexMatch(a.MATCH));
                  if (pt != null)
                      dr["URL"] = pt.DOMAIN + url;
              });
            }
            return new DataGridResult(dt, 0);
        }


        public UIResult GetMenuNew(MENUEntity data, string host)
        {
            List<MENUTREEModule> MENU_GROUPList = new List<MENUTREEModule>();
            List<PLATFORMEntity> PlatFormList = DbHelper
                .SelectList(new PLATFORMEntity() { ID = data.PLATFORMID })
                .GroupBy(a => a.ID)
                .Select(a =>
                {
                    var pt = a.FirstOrDefault(b => host.IsRegexMatch(b.MATCH));
                    if (pt == null)
                        pt = a.First();
                    return new PLATFORMEntity()
                    {
                        ID = a.Key,
                        DOMAIN = pt.DOMAIN
                    };
                }).ToList();

            //子系统要多传递参数回来
            string sqlgroup = @" SELECT MODULECODE ID,MODULENAME NAME,ICON FROM USERMODULE WHERE LENGTH(MODULECODE)=4  and ENABLE_FLAG=1";
            if (int.Parse(employee.Id) > 0)
            {
                sqlgroup += @" and MODULECODE in (
                                        SELECT DISTINCT SUBSTR(A.MODULECODE,1,4) FROM USERMODULE A,ROLE_MENU B,USER_ROLE C
                                        WHERE A.MENUID=B.MENUID AND B.ROLEID=C.ROLEID
                                        AND C.USERID=" + employee.Id + ")";
            }
            if (int.Parse(data.PLATFORMID)==1)
                sqlgroup += @" and MODULECODE like '02%'";
            if (int.Parse(data.PLATFORMID) == 2)
                sqlgroup += @" and MODULECODE like '05%'";
            sqlgroup += @" ORDER BY MODULECODE";
            DataTable menuGroup = DbHelper.ExecuteTable(sqlgroup);


            if (menuGroup.IsNotNull())
            {
                MENU_GROUPList = menuGroup.ToList<MENUTREEModule>();

                foreach (var menuGr in MENU_GROUPList)
                {
                    string sql = @" select aa.moduleid id,
                                    ab.id menuid,
                                    aa.modulename name,
                                    ab.url url,
                                    ab.platformid
                               from usermodule aa, menu ab
                              where  aa.menuid = ab.id and LENGTH(aa.MODULECODE)=6 and aa.ENABLE_FLAG=1
                                and aa.modulecode like  '" + menuGr.ID + "%'";
                    if (int.Parse(employee.Id) > 0)
                    {
                        sql += @" and aa.menuid in (
                                                   select a.id
                                                     from menu a,
                                                           ROLE_MENU     b,
                                                           USER_ROLE c
                                                    where a.id = b.menuid
                                                      and b.roleid = c.roleid
                                                      and c.userid = " + employee.Id + @" or
                                    aa.menuid is null)";
                    }

                    sql += " order by aa.modulecode  ";
                    DataTable menu = DbHelper.ExecuteTable(sql);
                    foreach (DataRow dr in menu.Rows)
                    {
                        string url = dr["URL"].ToString();
                        dr["PLATFORMID"].ToString().TryToInt(PlatFormId =>
                        {
                            var pt = PlatFormList.FirstOrDefault(a => a.ID == PlatFormId.ToString());
                            if (pt != null)
                                dr["URL"] = pt.DOMAIN + url;
                        });
                    }
                    menuGr.MENUList = menu.ToList<MENUEntity>();

                };
            };
            return new UIResult(new
            {
                MENU = MENU_GROUPList
            });
        }

        public User GetUserById(string id)
        {
            return DbHelper.Select(new SYSUSEREntity(id))?.ToObj(a => new User() { Id = a.USERID, Name = a.USERNAME });
        }


        public User GetUserByCode(string code, string password)
        {
            var e = DbHelper.SelectList(new SYSUSEREntity() { USERCODE = code })?
                        .Where(a => a.USER_FLAG.ToInt() == (int)用户标记.正常)
                        .FirstOrDefault();
            if (e == null)
                throw new Exception($"用户{code}不存在或已停用");
            if (salt(e.USERID, password) == e.PASSWORD)
            {
                return e.ToObj(a => new User() { Id = e.USERID, Name = e.USERNAME });
            }
            else
            {
                throw new Exception($"用户{code}密码错误");
            }
        }

        string salt(string userid, string pass)
        {
            return MD5Encryption.Encrypt(userid + LoginSalt + pass);
        }

        public string[] GetPermissionByUserId(string userid)
        {

            return DbHelper.ExecuteTable($@"SELECT A.MENUID FROM USERMODULE A, MENU B where A.MENUID = B.ID
 and exists(select 1 from USER_ROLE A1, ROLE_MENU B1, USERMODULE C1 where A1.USERID = '{userid}'
and A1.ROLEID = B1.ROLEID and B1.MENUID = C1.MENUID and C1.MENUID = A.MENUID )")
                                                  .ToList<string>().ToArray();
            //return DbHelper.ExecuteTable($@"select b.menuid
            //                                              from USER_ROLE a
            //                                              join menuqx b
            //                                                on a.roleid = b.roleid
            //                                             where a.userid = '{userid}'")
            //                                             .ToList<string>().ToArray();
        }

        public void ChangePs(SYSUSEREntity data)
        {

            SYSUSEREntity sysuser = DbHelper.Select(new SYSUSEREntity() { USERID = employee.Id });
            sysuser.PASSWORD = salt(sysuser.USERID, data.PASSWORD);
            DbHelper.Save(sysuser);
        }


        public string SaveSysUser(SYSUSEREntity data)
        {
            var v = GetVerify(data);
            if (data.USERID.IsEmpty())
            {
                data.USERID = NewINC("SYSUSER");
                data.PASSWORD = salt(data.USERID, data.PASSWORD);
            }
            else
            {
                if (data.PASSWORD.IsEmpty())
                {
                    SYSUSEREntity sysuser = DbHelper.Select(new SYSUSEREntity() { USERID = data.USERID });
                    data.PASSWORD = sysuser.PASSWORD;
                }
                else
                    data.PASSWORD = salt(data.USERID, data.PASSWORD);
            }
            v.Require(a => a.USERID);
            v.IsUnique(a => a.USERID);
            v.Require(a => a.USERCODE);
            v.IsUnique(a => a.USERCODE);
            v.Require(a => a.USERNAME);
            v.IsUnique(a => a.USERNAME);
            v.Require(a => a.VOID_FLAG);
            v.Verify();

            using (var Tran = DbHelper.BeginTransaction())
            {
                data.USER_ROLE?.ForEach(menu =>
                {
                    GetVerify(menu).Require(a => a.ROLEID);
                });

                DbHelper.Save(data);
                Tran.Commit();
            }
            return data.USERID;
        }
    }
}
