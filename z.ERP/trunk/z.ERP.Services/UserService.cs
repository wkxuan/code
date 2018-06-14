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
using z.SSO.Model;



namespace z.ERP.Services
{
    public class UserService : ServiceBase
    {
        internal UserService()
        {
        }
        public DataGridResult GetUser(SearchItem item)
        {
            string sql = $@"select A.USERID,A.USERCODE,A.USERNAME FROM SYSUSER A WHERE  1=1";
            item.HasKey("USERCODE,", a => sql += $" and A.USERCODE = '{a}'");
            item.HasKey("USERNAME", a => sql += $" and A.USERNAME like '%{a}%'");
            sql += " ORDER BY  A.USERCODE";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetUserElement(SearchItem item)
        {
            string sql = $@"select A.*,B.ORGIDCASCADER from SYSUSER A,ORG B where A.ORGID=B.ORGID(+)  ";
            item.HasKey("USERID", a => sql += $" and A.USERID = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public Tuple<dynamic, DataTable> GetUserElement(SYSUSEREntity Data)
        {
            string sql = $@"select A.USERID,A.USERCODE,A.USERNAME,A.USER_TYPE,A.ORGID,A.USER_FLAG,A.VOID_FLAG,";
             sql += " B.ORGIDCASCADER from SYSUSER A,ORG B where A.ORGID=B.ORGID(+)  ";
            if (!Data.USERID.IsEmpty())
            {
                sql += " and A.USERID = " + Data.USERID;
            }
            DataTable user = DbHelper.ExecuteTable(sql);
            string sqlUserRole = $@"select A.USERID,B.ROLEID,B.ROLECODE,B.ROLENAME,C.ORGNAME from USER_ROLE A,ROLE B,ORG C
                        where A.ROLEID=B.ROLEID and B.ORGID=C.ORGID(+) ";
            if (!Data.USERID.IsEmpty())
            {
                sqlUserRole += " and A.USERID = " + Data.USERID;
            }
            DataTable userrole = DbHelper.ExecuteTable(sqlUserRole);
            return new Tuple<dynamic, DataTable>(user.ToOneLine(), userrole);
        }
        public DataGridResult GetRole(SearchItem item)
        {
            string sql = $@"select A.ROLEID,A.ROLECODE,A.ROLENAME,B.ORGID,B.ORGCODE,B.ORGNAME 
                         FROM ROLE A,ORG B WHERE A.ORGID=B.ORGID and A.ORGID in (" + GetPermissionSql(PermissionType.Org) + ")";
            item.HasKey("ROLECODE,", a => sql += $" and A.ROLECODE = '{a}'");
            item.HasKey("ROLENAME", a => sql += $" and A.ROLENAME like '%{a}%'");
            item.HasKey("ORGCODE,", a => sql += $" and B.ORGCODE in like '{a}%'");
            sql += " ORDER BY  A.ROLECODE ";
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
        public Tuple<dynamic, DataTable, DataTable> GetRoleElement(ROLEEntity Data)
        {
            string sql = $@"SELECT A.*,B.ORGIDCASCADER  FROM ROLE A,ORG B  WHERE A.ORGID=B.ORGID ";
            if (!Data.ROLEID.IsEmpty())
                sql += (" AND ROLEID= " + Data.ROLEID);
            DataTable role = DbHelper.ExecuteTable(sql);


            string sql1 = $@"SELECT MENUID,MODULENAME,MODULECODE  FROM USERMODULE WHERE NOT MENUID IS NULL AND LENGTH(MODULECODE)=6 ";
            DataTable module = DbHelper.ExecuteTable(sql1);


            string sqlitem2 = $@"select A.TRIMID,A.NAME from FEESUBJECT A  order by A.TRIMID";
            DataTable fee = DbHelper.ExecuteTable(sqlitem2);

            return new Tuple<dynamic, DataTable, DataTable>(role.ToOneLine(), fee, module);
        }
        public string SaveRole(ROLEEntity SaveData, string Key)
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
                DbHelper.Save(SaveData);
                Tran.Commit();
            }
            return SaveData.ROLEID;
        }


        public DataGridResult Get_UserModule_Button(SearchItem item)
        {
            string sql = $@" SELECT MENUID,MODULENAME,MODULECODE  FROM USERMODULE WHERE NOT MENUID IS NULL";
            item.HasKey("MODULECODE", a => sql += $" and MODULECODE like '%{a}%' AND MODULECODE<>{a}");
            sql += " ORDER BY  MODULECODE";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

    }
}
