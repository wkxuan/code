using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using z.ERP.Entities;
using z.ERP.Entities.Enum;
using z.ERP.Model.Tree;
using z.ERP.Model.Vue;
using z.Extensions;
using z.MVC5.Results;
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
            string sql = $@"select A.USERID,A.USERCODE,A.USERNAME,A.SHOPID,A.USER_TYPE,B.ORGID,B.ORGCODE,B.ORGNAME,C.CODE SHOPCODE,C.NAME SHOPNAME
                              from SYSUSER A,ORG B,SHOP C 
                             where A.SHOPID=C.SHOPID(+) and A.ORGID=B.ORGID(+) 
                               and A.ORGID in (" + GetPermissionSql(PermissionType.Org) + ")";   //部门权限
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
            string sql = @"select A.USERID,A.USERCODE,A.USERNAME,A.USER_TYPE,A.ORGID,A.USER_FLAG,
                                  A.VOID_FLAG,B.ORGIDCASCADER,C.CODE SHOPCODE 
                             from SYSUSER A,ORG B,SHOP C 
                            where A.ORGID=B.ORGID(+) 
                              and A.SHOPID=C.SHOPID(+) ";
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
                              FROM ROLE A,ORG B WHERE A.ORGID=B.ORGID 
                               and A.ORGID in (" + GetPermissionSql(PermissionType.Org) + ")";  //部门权限
            item.HasKey("ROLECODE", a => sql += $" and A.ROLECODE = '{a}'");
            item.HasKey("ROLENAME", a => sql += $" and A.ROLENAME like '%{a}%'");
            item.HasKey("ORGCODE", a => sql += $" and B.ORGCODE like '{a}%'");
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
        public Tuple<dynamic, List<TreeEntity>, TreeModel[], List<TreeEntity>, DataTable, DataTable, DataTable> GetRoleInit(ROLEEntity Data)
        {
            string sqlRole = $@"SELECT A.*,B.ORGIDCASCADER  FROM ROLE A,ORG B  WHERE A.ORGID=B.ORGID ";
            if (!Data.ROLEID.IsEmpty())
                sqlRole += " AND ROLEID= " + Data.ROLEID;
            DataTable role = DbHelper.ExecuteTable(sqlRole);

            string sqlMenu = @"(select NVL(U.MENUID,0) MENUID,U.MODULECODE,U.MODULENAME,nvl(substr(MODULECODE,0,LENGTH(MODULECODE)-2),0) parentid  
                               from USERMODULE U,MENU M 
                              where U.MENUID=M.ID  AND U.ENABLE_FLAG=1  and length(to_char(m.id)) <8
                          union all
                             select NVL(m.id,0) MENUID,U.MODULECODE|| lpad(to_char(mod(m.id,100)),2,'0') MODULECODE,m.name MODULENAME,U.MODULECODE parentid  
                               from USERMODULE U,MENU M 
                              where U.ENABLE_FLAG=1 AND M.PLATFORMID =1 and length(to_char(m.id)) =8
                                    and u.menuid = trunc(m.id/100)
                          union all 
                             select NVL(MENUID,0),MODULECODE,MODULENAME,nvl(substr(MODULECODE,0,LENGTH(MODULECODE)-2),0) parentid 
                               from USERMODULE 
                              where ENABLE_FLAG=1 and (MENUID is null or MENUID =0))
                           order by MODULECODE,MENUID";

            string sql1 =@"select Z.MENUID,Z.MODULECODE,Z.MODULENAME,Z.PARENTID,R.ROLEID IsChecked
                             from ({0}) Z left join ROLE_MENU R ON Z.MENUID = R.MENUID AND Z.MODULECODE = R.MODULECODE AND R.ROLEID ={1}
                         order by MODULECODE,MENUID";

            var resData = new List<USERMODULEEntity>();
            if (!Data.ROLEID.IsEmpty())
            {
                resData = DbHelper.ExecuteTable(string.Format(sql1, sqlMenu, Data.ROLEID)).ToList<USERMODULEEntity>();
            }
            else
            {
                resData = DbHelper.ExecuteTable(sqlMenu).ToList<USERMODULEEntity>();
            }
            List<TreeEntity> treeList = new List<TreeEntity>();
            foreach (var item in resData)
            {
                TreeEntity node = new TreeEntity();
                node.value = item.MENUID;
                node.code = item.MODULECODE;
                node.@checked = !item.IsChecked.IsNullValue();
                node.title = item.MODULENAME;
                node.expand = false;
                node.parentId = item.PARENTID;
                treeList.Add(node);
            }
            var module = treeList.ToTree();

            string sqlitem2 = $@"select A.TRIMID,A.NAME from FEESUBJECT A  order by A.TRIMID";
            DataTable fee = DbHelper.ExecuteTable(sqlitem2);
            fee.Columns.Add("_checked", typeof(bool));
            for (var i=0;i<fee.Rows.Count;i++)
            {
                if (!Data.ROLEID.IsEmpty())
                {
                    string sqlFee = "select 1 from ROLE_FEE where ROLEID= {0} and TRIMID={1}";
                    var feeDt = DbHelper.ExecuteTable(string.Format(sqlFee, Data.ROLEID, fee.Rows[i]["TRIMID"]));
                    fee.Rows[i]["_checked"] = feeDt.Rows.Count > 0 ? true : false;
                }else
                {
                    fee.Rows[i]["_checked"] = false;
                }             
            }

            //区域数据
            var regionTreeData = regionQxData(Data);

            //业态数据
            string sqlYt = @"select G.CATEGORYID,G.CATEGORYCODE,G.CATEGORYNAME 
                             from CATEGORY G where 1=1";

            string SqlyTQx = GetFullYtQX("G");
            if (SqlyTQx != "")
            {
                sqlYt += " and " + SqlyTQx;
            }
            var ytData = DbHelper.ExecuteTable(sqlYt).ToList<CATEGORYEntity>();          
            var ytTreeData = TreeModel.Create(ytData, a => a.CATEGORYCODE,
                a => new TreeModel()
                {
                    value = a.CATEGORYID,
                    @checked = ytCheck(Data.ROLEID, a.CATEGORYID),
                    code = a.CATEGORYCODE,
                    title = a.CATEGORYCODE + " " + a.CATEGORYNAME,
                    expand = false
                })?.ToArray();

            //门店
            string sqlbranch = $@"select B.ID BRANCHID,B.NAME from BRANCH B where 1=1 "
                               + "   and B.ID IN ("+GetPermissionSql(PermissionType.Branch)+")"                                   
                               + " order by B.ID";

            DataTable branch = DbHelper.ExecuteTable(sqlbranch);
            branch.Columns.Add("_checked", typeof(bool));
            for (var i = 0; i < branch.Rows.Count; i++)
            {
                if (!Data.ROLEID.IsEmpty())
                {
                    string sqlBranch = "select 1 from ROLE_BRANCH WHERE ROLEID={0} and BRANCHID={1} ";
                    var branchDt = DbHelper.ExecuteTable(string.Format(sqlBranch, Data.ROLEID, branch.Rows[i]["BRANCHID"]));
                    branch.Rows[i]["_checked"] = branchDt.Rows.Count > 0 ? true : false;
                }
                else
                {
                    branch.Rows[i]["_checked"] = false;
                }
            }

            //预警
            string sqlalert = $@"select B.ID ALERTID,B.MC NAME from DEF_ALERT B where 1=1 "
                            + "     and B.ID IN (" + GetPermissionSql(PermissionType.Alert) + ")"
                            + "   order by B.ID";
            DataTable alert = DbHelper.ExecuteTable(sqlalert);
            alert.Columns.Add("_checked", typeof(bool));
            for (var i = 0; i < alert.Rows.Count; i++)
            {
                if (!Data.ROLEID.IsEmpty())
                {
                    string sqlAlert = "select B.ID ALERTID,B.MC NAME from DEF_ALERT B,ROLE_ALERT R WHERE B.ID=R.ALERTID and R.ROLEID={0} and R.ALERTID={1}";
                    var alertDt = DbHelper.ExecuteTable(string.Format(sqlAlert, Data.ROLEID, alert.Rows[i]["ALERTID"]));
                    alert.Rows[i]["_checked"] = alertDt.Rows.Count > 0 ? true : false;
                }
                else
                {
                    alert.Rows[i]["_checked"] = false;
                }
            }
            return new Tuple<dynamic, List<TreeEntity>, TreeModel[], List<TreeEntity>, DataTable, DataTable, DataTable>(role.ToOneLine(), module, ytTreeData, regionTreeData, fee, branch, alert);
        }
        public List<TreeEntity> regionQxData(ROLEEntity Data)
        {
            var treeList = new List<TreeEntity>();

            string sqlRegion = $@"SELECT REGIONID,CODE,NAME FROM REGION WHERE 1=1";
            sqlRegion += " AND REGIONID IN (" + GetPermissionSql(PermissionType.Region) + ")";

            var region = DbHelper.ExecuteTable(sqlRegion).ToList<REGIONEntity>();
            foreach (var item in region)
            {
                TreeEntity node = new TreeEntity();
                node.value = item.REGIONID;
                node.code = item.CODE;
                node.title = item.NAME;
                node.expand = false;
                node.children = floorQxData(Data,item.REGIONID);
                treeList.Add(node);
            }
            return treeList;
        }
        public List<TreeEntity> floorQxData(ROLEEntity Data,string regionid)
        {
            var treeList = new List<TreeEntity>();

            string sqlFloor = $@"SELECT ID,CODE,NAME FROM FLOOR
                                  WHERE REGIONID=" + regionid;
            sqlFloor += " AND ID IN (" + GetPermissionSql(PermissionType.Floor) + ")";
            sqlFloor += " ORDER BY ID ASC";
            var floor = DbHelper.ExecuteTable(sqlFloor).ToList<FLOOREntity>();

           
            if (!Data.ROLEID.IsEmpty())
            {
                const string floorQx = @"select 1 from ROLE_FLOOR where roleid={0} and floorid={1}";

                foreach (var item in floor)
                {
                    bool flag = false;
                    if(DbHelper.ExecuteTable(string.Format(floorQx, Data.ROLEID, item.ID)).Rows.Count > 0){
                        flag = true;
                    }
                    TreeEntity node = new TreeEntity();
                    node.value = item.ID;
                    node.code = item.CODE;
                    node.title = item.NAME;
                    node.expand = false;
                    node.@checked = flag;
                    node.parentId = regionid;
                    treeList.Add(node);
                }
            }
            else
            {
                foreach (var item in floor)
                {
                    TreeEntity node = new TreeEntity();
                    node.value = item.ID;
                    node.code = item.CODE;
                    node.title = item.NAME;
                    node.expand = false;
                    node.@checked = false;
                    node.parentId = regionid;
                    treeList.Add(node);
                }
            }          
            return treeList;
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
            if (SaveData.VOID_FLAG!="1") {
                SaveData.VOID_FLAG = "2";       //标记有null数据， 不为1得全部置为2 
            }
            v.Verify();
            using (var Tran = DbHelper.BeginTransaction())
            {
                DbHelper.Save(SaveData);
                Tran.Commit();
            }
            return SaveData.ROLEID;
        }
        public bool ytCheck(string roleid,string id)
        {
            if (roleid.IsEmpty())
                return false;
            const string selStr = @"select 1 from ROLE_YT where ROLEID={0} and YTID={1}";
            if (DbHelper.ExecuteTable(string.Format(selStr, roleid, id)).Rows.Count > 0)
                return true;
            else
                return false;
        }
    }
}
