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
                                  pf.domain||  ab.url url
                               from menutree aa, menu ab,platform pf
                              where  aa.menuid = ab.id(+)  and ab.platformid=pf.id(+)";
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
            return new DataGridResult(DbHelper.ExecuteTable(sql), 0);
        }


        public UIResult GetMenuNew(MENUTREEEntity data)
        {
            List<MENUTREEModule> MENU_GROUPList = new List<MENUTREEModule>();

            //子系统要多传递参数回来
            string sqlgroup = @" SELECT MODULECODE ID,MODULENAME NAME FROM USERMODULE WHERE LENGTH(MODULECODE)=4 ";
            if (int.Parse(employee.Id) > 0)
            {
                sqlgroup += @" and (MODULECODE in (
                                        SELECT DISTINCT SUBSTR(MODULECODE,1,4) FROM USERMODULE A,ROLE_MENU B,USER_ROLE C
                                        WHERE A.MENUID=B.MENUID AND B.ROLEID=C.ROLEID
                                        AND C.USERID=" + employee.Id;
            }

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
                                  pf.domain||  ab.url url
                               from usermodule aa, menu ab,platform pf
                              where  aa.menuid = ab.id and LENGTH(aa.MODULECODE)=6 and ab.platformid=pf.id(+) and aa.modulecode like  '" + menuGr.ID+"%'";
                    if (int.Parse(employee.Id) > 0)
                    {
                        sql += @" and (aa.menuid in (
                                                   select a.id
                                                     from menu a,
                                                           ROLE_MENU     b,
                                                           USER_ROLE c
                                                    where a.id = b.menuid
                                                      and b.roleid = c.roleid
                                                      and c.userid = " + employee.Id + @") or
                                    aa.menuid is null)";
                    }

                    sql += " order by aa.modulecode  ";
                    DataTable menu = DbHelper.ExecuteTable(sql);
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
                throw new Exception("用户不存在或已停用");
            if (salt(e.USERID, password) == e.PASSWORD)
            {
                return e.ToObj(a => new User() { Id = e.USERID, Name = e.USERNAME });
            }
            else
            {
                throw new Exception("密码错误");
            }
        }

        string salt(string userid, string pass)
        {
            return (userid + LoginSalt + pass).ToMD5();
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


    }
}
