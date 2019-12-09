﻿using System.Data;
using z.MVC5.Results;
using z.ERP.Entities;
using System.Collections.Generic;
using z.Extensions;
using System;
using z.ERP.Entities.Enum;
using z.SSO.Model;
using System.Linq;
using z.Encryption;
using System.Web;

namespace z.ERP.Services
{
    public class HomeService : ServiceBase
    {
        protected const string LoginSalt = "z.SSO.LoginSalt.1";
        internal HomeService()
        {
        }


        /// <summary>
        /// 获取一个菜单的url
        /// </summary>
        /// <param name="menuid"></param>
        /// <returns></returns>
        public string GetMenuUrl(string menuid)
        {
            if (menuid.IsEmpty())
                return null;
            MENUEntity menu = DbHelper.ExecuteOneObject<MENUEntity>($"select id,url from MENU where id='{menuid}'");
            if (menu == null)
                throw new Exception($"找不到菜单{menuid}");
            List<PLATFORMEntity> allp = DbHelper.SelectList(new PLATFORMEntity() { ID = menu.PLATFORMID });
            PLATFORMEntity ps = allp.FirstOrDefault(a => HttpContext.Current.Request.Url.Host.IsRegexMatch(a.MATCH));
            if (ps == null)
                ps = allp.First();
            return ps.DOMAIN + menu.URL;
        }

        /// <summary>
        /// 获取一个ERP服务域名
        /// </summary>
        /// <param name="menuid"></param>
        /// <returns></returns>
        public string GetErpDomain()
        {
            List<PLATFORMEntity> allp = DbHelper.SelectList(new PLATFORMEntity() { ID = "1" });
            PLATFORMEntity ps = allp.FirstOrDefault(a => HttpContext.Current.Request.Url.Host.IsRegexMatch(a.MATCH));
            if (ps == null)
                ps = allp.First();
            return ps.DOMAIN;
        }
        /// <summary>
        /// 获取配置系统
        /// </summary>
        /// <returns></returns>
        public List<PLATFORMEntity> GetPLATFORM() {
            return DbHelper.SelectList(new PLATFORMEntity()).OrderBy(a=>a.ID).ToList<PLATFORMEntity>();
        }
        /// <summary>
        /// 获取权限菜单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public DataTable GetMenuList(MENUEntity data) {
            string sqlp = "";
            if (int.Parse(employee.Id) > 0) {
                //权限条件
                sqlp = $@"AND U.MODULECODE IN ( SELECT B.MODULECODE FROM USERMODULE A, ROLE_MENU B,USER_ROLE C WHERE A.MODULECODE = B.MODULECODE
                        AND B.ROLEID = C.ROLEID AND C.USERID = '{employee.Id}')";
            }
            //先查询权限菜单再查询菜单父目录
            string sql = $@"SELECT U.*,M.URL,M.PLATFORMID FROM USERMODULE U,MENU M 
                        WHERE U.MENUID=M.ID(+) AND U.ENABLE_FLAG=1 AND LENGTH(U.MODULECODE)>2 AND M.PLATFORMID={data.PLATFORMID} {sqlp} 
                        UNION ALL 
                        SELECT U.*,M.URL,M.PLATFORMID FROM USERMODULE U,MENU M 
                        WHERE U.MENUID=M.ID(+) AND U.ENABLE_FLAG=1 AND LENGTH(U.MODULECODE)>2 AND  
                        U.MODULEID IN ( SELECT U1.PMODULEID FROM USERMODULE U1,MENU M1 WHERE U1.MENUID=M1.ID(+) AND U1.ENABLE_FLAG=1 
                        AND LENGTH(U1.MODULECODE)>2 AND M1.PLATFORMID={data.PLATFORMID} {sqlp}) ";
            string sqls = $@"SELECT * FROM ({sql}) ORDER BY MODULECODE,INX";
            DataTable menuGroup = DbHelper.ExecuteTable(sqls);
            return menuGroup;
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
                              where  aa.menuid = ab.id(+) and ab.STATUS IN (1,98)  ";
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
                                        SELECT DISTINCT SUBSTR(B.MODULECODE,1,4) FROM ROLE_MENU B,USER_ROLE C
                                        WHERE B.ROLEID=C.ROLEID
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
                                and ab.STATUS IN (1,98)
                                and aa.modulecode like  '" + menuGr.ID + "%'";
                    if (int.Parse(employee.Id) > 0)
                    {
                        //因菜单树型权限 按扭权限不全选时 对应菜单未保存到ROLE_MENU  
                        //暂改成 由按扭权单id截取6位关联菜单id   by wangkx 20190705
                        //b.menuid 改为 to_number(substr( to_char(b.menuid),1,6))  
                        // by DZK  20190716   ID取6位引起crm找不到子菜单，根据PLATFORMID 查询
                        if (int.Parse(data.PLATFORMID) == 1)
                        {
                            sql += @" and aa.menuid in (
                                                   select a.id
                                                     from menu a,
                                                           ROLE_MENU b,
                                                           USER_ROLE c
                                                    where a.id = to_number(substr( to_char(b.menuid),1,6))  
                                                      and b.roleid = c.roleid
                                                      and c.userid = " + employee.Id + @" or
                                    aa.menuid is null)";
                        }
                        else
                        {
                            sql += @" and aa.menuid in (
                                                   select a.id
                                                     from menu a,
                                                           ROLE_MENU b,
                                                           USER_ROLE c
                                                    where a.id = b.menuid 
                                                      and b.roleid = c.roleid
                                                      and c.userid = " + employee.Id + @" or
                                    aa.menuid is null)";
                        }
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
                return e.ToObj(a => new User() { Id = e.USERID,Code=e.USERCODE, Name = e.USERNAME });
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

            return DbHelper.ExecuteTable($@"SELECT B.ID FROM MENU B where 1=1
                                               and B.STATUS IN (1,98)
                                               and exists(select 1 from USER_ROLE A1, ROLE_MENU B1 where A1.USERID = '{userid}'
                                               and A1.ROLEID = B1.ROLEID and B1.MENUID = B.ID)")
                                                  .ToList<string>().ToArray();

            //return DbHelper.ExecuteTable($@"SELECT A.MENUID FROM USERMODULE A, MENU B where A.MENUID = B.ID
            //                                   and exists(select 1 from USER_ROLE A1, ROLE_MENU B1, USERMODULE C1 where A1.USERID = '{userid}'
            //                                   and A1.ROLEID = B1.ROLEID and B1.MENUID = C1.MENUID and C1.MENUID = A.MENUID )")
            //                                      .ToList<string>().ToArray();

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
        public DataTable DclrwData()
        {
            var sql = @" select B.MENUID,M.NAME,C.NAME BRANCHMC ,B.URL ,BILLID "
                     + "   from BILLSTATUS B,MENU M,BRANCH C"
                     + "  where B.MENUID=M.ID and B.BRABCHID =C.ID  " 
                     + "    and M.ID in (" + GetPermissionSql(PermissionType.Menu) + ")"      //菜单权限
                     + "    and C.ID in (" + GetPermissionSql(PermissionType.Branch) + ")"    //门店权限
                     + "  order by  B.MENUID ";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt;
        }
        public DataTable NoticeData(int type) {
            string sql = "";
            if (type == 1)
            {
                sql = $@"SELECT N.ID,N.TITLE,N.STATUS,TO_CHAR(VERIFY_TIME,'yyyy-MM-dd') release_time FROM NOTICE N WHERE N.STATUS=2 AND N.TYPE=1 
                    AND exists(select 1 from NOTICE_BRANCH   where NOTICE_BRANCH.NOTICEID=N.ID AND NOTICE_BRANCH.BRANCHID IN 
                    ("+ GetPermissionSql(PermissionType.Branch) + "))  AND not exists(select 1 from READNOTOCELOG   where READNOTOCELOG.NOTICEID=N.ID AND READNOTOCELOG.USERID=" + employee.Id + ") ORDER BY VERIFY_TIME DESC ";
            }
            else {
                sql = @"SELECT N.ID,N.TITLE,N.STATUS,TO_CHAR(VERIFY_TIME,'yyyy-MM-dd') release_time FROM NOTICE N WHERE N.STATUS=2  AND N.TYPE=1 
                  AND exists(select 1 from NOTICE_BRANCH   where NOTICE_BRANCH.NOTICEID=N.ID AND NOTICE_BRANCH.BRANCHID IN 
                    (" + GetPermissionSql(PermissionType.Branch) + "))   AND exists(select 1 from READNOTOCELOG   where READNOTOCELOG.NOTICEID=N.ID AND READNOTOCELOG.USERID=" + employee.Id + ") ORDER BY VERIFY_TIME DESC ";
            }
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt;
        }
        public Tuple<dynamic, int> AlertData()
        {
            var sql = @" select DEF_ALERT.ID,MC,XSSX,SQLSTR, 0 COUNT FROM DEF_ALERT
                          where DEF_ALERT.ID in ("+ GetPermissionSql(PermissionType.Alert) + ") ORDER BY XSSX ";

            var count = 0;
            DataTable dt = DbHelper.ExecuteTable(sql);
            foreach (DataRow item in dt.Rows) {
                var sqlstr = item["SQLSTR"].ToString();
                DataTable dtstr = DbHelper.ExecuteTable(sqlstr);
                item["COUNT"] = dtstr.Rows.Count;
                count += dtstr.Rows.Count;
            }
            return new Tuple<dynamic, int>(dt, count);
        }
    }
}
