using System.Data;
using System.Linq;
using z.MVC5.Results;
using z.ERP.Entities;
using System.Collections.Generic;
using z.Extensions;
using System;
using z.ERP.Entities.Enum;
using z.Exceptions;
using z.ERP.Model.Vue;



namespace z.ERP.Services
{
    public class UserService : ServiceBase
    {
        internal UserService()
        {
        }
        public DataGridResult GetRole(SearchItem item)
        {
            string sql = $@"select A.ROLEID,A.ROLECODE,A.ROLENAME,B.ORGID,B.ORGCODE,B.ORGNAME FROM ROLE A,ORG B WHERE A.ORGID=B.ORGID";
            item.HasKey("ROLECODE,", a => sql += $" and A.ROLECODE = '{a}'");
            item.HasKey("ROLENAME", a => sql += $" and A.ROLENAME like '%{a}%'");
            sql += " ORDER BY  A.ROLECODE";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetRoleElement(SearchItem item)
        {
            string sql = $@"select A.* from ROLE A where 1=1 ";
            item.HasKey("ROLEID", a => sql += $" and A.ROLEID = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public Tuple<dynamic, dynamic, DataTable> GetRoleElement(ROLEEntity Data)
        {
            //此处校验一次只能查询一个单号,校验单号必须存在
            string sql = $@"SELECT A.*  FROM ROLE A  WHERE 1=1 ";
            if (!Data.ROLEID.IsEmpty())
                sql += (" AND ROLEID= " + Data.ROLEID);
            DataTable role = DbHelper.ExecuteTable(sql);

            string sqlLoginRoleMenu = $@"select A.*  FROM USERMODULE A,ROLE_MENU B where  A.MODULECODE like B.MODULECODE||'%' ";
            //if (!employee.Id.IsEmpty())
                sqlLoginRoleMenu += (" AND ROLEID= 1");
            DataTable loginRoleMenu = DbHelper.ExecuteTable(sqlLoginRoleMenu);

            string sqlSelectRoleMenu = $@"select A.*  FROM USERMODULE A,ROLE_MENU B where  A.MODULECODE like B.MODULECODE||'%'  ";
            if (!Data.ROLEID.IsEmpty())
                sqlSelectRoleMenu += (" AND ROLEID= " + Data.ROLEID);
            DataTable selectRoleMenu = DbHelper.ExecuteTable(sqlSelectRoleMenu);

            List<USERMODULEEntity> p = DbHelper.SelectList(new USERMODULEEntity()).OrderBy(a => a.MODULECODE).ToList();
            UIResult tree = new UIResult(TreeModel.Create(p,
                a => a.MODULECODE,
                a => new TreeModel()
                {
                    code = a.MODULECODE,
                    title = a.MODULENAME,
                    expand = true,
                    selected = (selectRoleMenu.Select(" MODULECODE='" +a.MODULECODE + "' and MENUID=" + a.MENUID).Length > 0)
                    //disabled = (loginRoleMenu.Select(" MODULECODE='" + a.MODULECODE + "' and MENUID=" + a.MENUID).Length==0)
                })?.ToArray());

            string sqlitem2 = $@"select A.TRIMID,A.NAME,
                                (select count(1) from ROLE_FEE B WHERE A.TRIMID=B.TRIMID and B.ROLEID=1) as DISABLED,
                                (select count(1) from ROLE_FEE B WHERE A.TRIMID=B.TRIMID and B.ROLEID=2) as CHECKED
                                from FEESUBJECT A where 1=1 order by A.TRIMID ";
            DataTable sfxm = DbHelper.ExecuteTable(sqlitem2);

            return new Tuple<dynamic, dynamic, DataTable>(role.ToOneLine(), tree, sfxm);
        }
        public string SaveRole(ROLEEntity SaveData,string Key)
        {
            var v = GetVerify(SaveData);
            if (SaveData.ROLEID.IsEmpty())
                SaveData.ROLEID = NewINC("ROLE");
            v.IsUnique(a => a.ROLEID);
            v.IsUnique(a => a.ROLECODE);
            v.Require(a => a.ROLENAME);
            v.Require(a => a.ORGID);
            v.Verify();
            using (var Tran = DbHelper.BeginTransaction())
            {
                int i = 0;
                string code = "";
                SaveData.ROLE_MENU?.ForEach(rolemenu =>
                {
                    GetVerify(rolemenu).Require(a => a.ROLEID);
                    //code=TreeModel.GetNewKey(rolemenu, a => a.MODULECODE, Key, Key);
                    SaveData.ROLE_MENU[i].MODULECODE = code;
                });
                SaveData.ROLE_FEE?.ForEach(rolefee =>
                {
                    GetVerify(rolefee).Require(a => a.ROLEID);
                });
                DbHelper.Save(SaveData);

                Tran.Commit();
            }
            return SaveData.ROLEID;
        }

    }
}
