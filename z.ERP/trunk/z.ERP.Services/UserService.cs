using System.Data;
using z.MVC5.Results;
using z.ERP.Entities;
using z.Extensions;
using System;
using z.SSO.Model;
using z.ERP.Entities.Enum;
using System.Collections.Generic;
using z.ERP.Model.Vue;
using System.Linq;

namespace z.ERP.Services
{
    public class UserService : ServiceBase
    {
        internal UserService()
        {
        }
        public DataGridResult GetUser(SearchItem item)
        {
            string sql = $@"select A.USERID,A.USERCODE,A.USERNAME,A.SHOPID,A.USER_TYPE,B.ORGID,B.ORGCODE,B.ORGNAME,C.CODE SHOPCODE,C.NAME SHOPNAME
             from SYSUSER A,ORG B,SHOP C 
             where A.SHOPID=C.SHOPID(+) and A.ORGID=B.ORGID(+) ";  //and A.ORGID in (" + GetPermissionSql(PermissionType.Org) + ")";
            item.HasKey("USERCODE", a => sql += $" and A.USERCODE like '%{a}%'");
            item.HasKey("USERNAME", a => sql += $" and A.USERNAME like '%{a}%'");
            item.HasKey("USER_TYPE", a => sql += $" and A.USER_TYPE in ({a})");
            sql += " ORDER BY  A.USERCODE";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<用户类型>("USER_TYPE", "USER_TYPEMC");
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetUserElement(SearchItem item)
        {
            string sql = $@"select A.*,B.ORGIDCASCADER,C.CODE SHOPCODE,C.NAME SHOPNAME ";
            sql += " from SYSUSER A,ORG B,SHOP C where A.ORGID=B.ORGID(+) and A.SHOPID=C.SHOPID(+)  ";
            item.HasKey("USERID", a => sql += $" and A.USERID = '{a}'");
            item.HasKey("USERCODE", a => sql += $" and A.USERCODE like '%{a}%'");
            item.HasKey("USERNAME", a => sql += $" and A.USERNAME like '%{a}%'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public Tuple<dynamic, DataTable> GetUserElement(SYSUSEREntity Data)
        {
            string sql = $@"select A.USERID,A.USERCODE,A.USERNAME,A.USER_TYPE,A.ORGID,to_char(A.USER_FLAG) USER_FLAG,";
             sql += " to_char(A.VOID_FLAG) VOID_FLAG,B.ORGIDCASCADER from SYSUSER A,ORG B where A.ORGID=B.ORGID(+)  ";
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
            string sql = $@"select A.ROLEID,A.ROLECODE,A.ROLENAME,B.ORGID,B.ORGCODE,B.ORGNAME,1 STATUS 
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
        public Tuple<dynamic, DataTable, TreeModel[], DataTable, DataTable, TreeModel[]> GetRoleElement(ROLEEntity Data)
        {
            string sql = $@"SELECT A.*,B.ORGIDCASCADER  FROM ROLE A,ORG B  WHERE A.ORGID=B.ORGID ";
            if (!Data.ROLEID.IsEmpty())
                sql += (" AND ROLEID= " + Data.ROLEID);
            DataTable role = DbHelper.ExecuteTable(sql);


            //string sqlMenu = $@" SELECT MODULECODE,MENUID FROM ROLE_MENU WHERE 1=1";
            //if (!Data.ROLEID.IsEmpty())
            //    sqlMenu += (" AND ROLEID= " + Data.ROLEID);
            //DataTable module = DbHelper.ExecuteTable(sqlMenu);
            //更改权限列表为树 by：DZK
            string sql1 = @" SELECT NVL(U.MENUID,0) MENUID,U.MODULECODE,U.MODULENAME,R.ROLEID IsChecked FROM USERMODULE U
                    LEFT JOIN ROLE_MENU R ON U.MODULECODE=R.MODULECODE  AND U.MENUID=R.MENUID AND R.ROLEID = " + Data.ROLEID + @"                    
	                    ORDER BY U.MODULECODE";
            List<USERMODULEEntity> um = DbHelper.ExecuteTable(sql1).ToList<USERMODULEEntity>(); ;
            var module = TreeModel.Create(um,
                a => a.MODULECODE,
                a => new TreeModel()
                {
                    value = a.MENUID,
                    @checked = !a.IsChecked.IsNullValue(),
                    code = a.MODULECODE,
                    title = a.MODULENAME,
                    expand = false
                })?.ToArray();


            string sqlFee = $@" SELECT TRIMID FROM  ROLE_FEE WHERE 1=1";
            if (!Data.ROLEID.IsEmpty())
                sqlFee += (" AND ROLEID= " + Data.ROLEID);
            DataTable fee = DbHelper.ExecuteTable(sqlFee);



            string sqlYt = $@" SELECT YTID ID FROM  ROLE_YT WHERE 1=1";
            if (!Data.ROLEID.IsEmpty())
                sqlYt += (" AND ROLEID= " + Data.ROLEID);
            DataTable yt = DbHelper.ExecuteTable(sqlYt);


            string sqlRegion = $@" SELECT REGIONID FROM  ROLE_REGION WHERE 1=1";
            if (!Data.ROLEID.IsEmpty())
                sqlRegion += (" AND ROLEID= " + Data.ROLEID);
            DataTable region = DbHelper.ExecuteTable(sqlRegion);

            //List<GOODS_KINDEntity> p = DbHelper.SelectList(new GOODS_KINDEntity()).OrderBy(a => a.CODE).ToList();
            string sqlYt2 = "select G.CATEGORYID,G.CATEGORYCODE,G.CATEGORYNAME,Y.YTID LEVEL_LAST from CATEGORY G,ROLE_YT Y where G.CATEGORYID =Y.YTID(+) ";
            sqlYt2 += (" AND Y.ROLEID(+)= " + Data.ROLEID);

            List<CATEGORYEntity> p =  DbHelper.ExecuteTable(sqlYt2).ToList<CATEGORYEntity>();

            var ytTreeData = TreeModel.Create(p,
                a => a.CATEGORYCODE,
                a => new TreeModel()
                {
                    value  = a.CATEGORYID,
                    @checked = !a.LEVEL_LAST.IsNullValue(),
                    code = a.CATEGORYCODE,
                    title = a.CATEGORYCODE + " " + a.CATEGORYNAME,
                    expand = false
                })?.ToArray();

            return new Tuple<dynamic, DataTable, TreeModel[], DataTable, DataTable, TreeModel[]>(role.ToOneLine(), fee, module, yt, region, ytTreeData);
        }


        public Tuple<dynamic, DataTable, TreeModel[], TreeModel[], DataTable> GetRoleInit()
        {

            var org = DataService.GetTreeOrg();

            //string sql1 = $@"SELECT * FROM ( ";
            //sql1 += " SELECT MENUID,MODULECODE,MODULENAME MENUNAME,'' AS BUTONNAME";
            //sql1 += " FROM USERMODULE WHERE NOT MENUID IS NULL AND LENGTH(MODULECODE)=6 ";
            //sql1 += " UNION";
            //sql1 += " SELECT MENUID,MODULECODE,'' AS MENUNAME,MODULENAME AS BUTONNAME";
            //sql1 += " FROM USERMODULE WHERE NOT MENUID IS NULL AND LENGTH(MODULECODE) = 8 ";
            //sql1 += " ) ORDER BY MODULECODE ";
            //DataTable module = DbHelper.ExecuteTable(sql1);

            //更改权限列表为树 by：DZK
            string sql1 = @" (select NVL(U.MENUID,0) MENUID,U.MODULECODE,U.MODULENAME FROM USERMODULE U,MENU M
                        WHERE U.MENUID=M.ID 
                        UNION ALL 
                        select  NVL(MENUID,0),MODULECODE,MODULENAME FROM USERMODULE 
                        WHERE MENUID is null or MENUID =0) 
	                        ORDER BY MODULECODE";
            List<USERMODULEEntity> um = DbHelper.ExecuteTable(sql1).ToList<USERMODULEEntity>(); ;
            var module = TreeModel.Create(um,
                a => a.MODULECODE,
                a => new TreeModel()
                {
                    value = a.MENUID,
                    code = a.MODULECODE,
                    title = a.MODULENAME,
                    expand = false
                })?.ToArray();


            string sqlitem2 = $@"select A.TRIMID,A.NAME from FEESUBJECT A  order by A.TRIMID";
            DataTable fee = DbHelper.ExecuteTable(sqlitem2);

            string sqlitemRegion = $@"select A.REGIONID,A.NAME from REGION A  order by A.REGIONID";
            DataTable region = DbHelper.ExecuteTable(sqlitemRegion);

            List<CATEGORYEntity> p = DbHelper.SelectList(new CATEGORYEntity()).OrderBy(a => a.CATEGORYCODE).ToList();
            var ytTreeData = TreeModel.Create(p,
                a => a.CATEGORYCODE,
                a => new TreeModel()
                {
                    value = a.CATEGORYID,
                    code = a.CATEGORYCODE,
                    title = a.CATEGORYCODE + " " + a.CATEGORYNAME,
                    expand = false
                })?.ToArray();

            return new Tuple<dynamic, DataTable, TreeModel[], TreeModel[], DataTable>(org.Item1, fee, module, ytTreeData, region);
        }


        
        public string SaveRole(ROLEEntity SaveData)
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

    }
}
