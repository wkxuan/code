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
            return DbHelper.ExecuteTable($@"select b.menuid
                                                          from USER_ROLE a
                                                          join menuqx b
                                                            on a.roleid = b.roleid
                                                         where a.userid = '{userid}'")
                                                         .ToList<string>().ToArray();
        }

        public void ChangePs(SYSUSEREntity data)
        {

            SYSUSEREntity sysuser = DbHelper.Select(new SYSUSEREntity() { USERID = employee.Id });
            sysuser.PASSWORD = salt(sysuser.USERID, data.PASSWORD);
            DbHelper.Save(sysuser);
        }


    }
}
