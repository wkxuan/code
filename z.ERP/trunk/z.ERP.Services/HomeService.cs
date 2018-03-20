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

namespace z.ERP.Services
{
    public class HomeService : ServiceBase
    {
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

        public void ChangePs(SYSUSEREntity data) {

            SYSUSEREntity sysuser = DbHelper.Select(new SYSUSEREntity(){ USERID = employee.Id }); 
            ERPUserHelper userHelper = new ERPUserHelper();
            sysuser.PASSWORD = userHelper.salt(sysuser, data.PASSWORD);
            DbHelper.Save(sysuser);
        }
    }
}
