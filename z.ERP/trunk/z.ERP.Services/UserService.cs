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
        public Tuple<dynamic, List<TreeEntity>, TreeModel[], List<TreeEntity>, DataTable, DataTable, DataTable> GetRoleElement(ROLEEntity Data)
        {
            string sql = $@"SELECT A.*,B.ORGIDCASCADER  FROM ROLE A,ORG B  WHERE A.ORGID=B.ORGID ";
            if (!Data.ROLEID.IsEmpty())
                sql += (" AND ROLEID= " + Data.ROLEID);
            DataTable role = DbHelper.ExecuteTable(sql);

            string sql1 = @"select Z.MENUID,Z.MODULECODE,Z.MODULENAME,Z.PARENTID,R.ROLEID IsChecked
                              from (
                            select NVL(U.MENUID, 0) MENUID, U.MODULECODE, U.MODULENAME, nvl(substr(MODULECODE, 0, LENGTH(MODULECODE) - 2), 0) parentid
                              from USERMODULE U, MENU M
                             where U.MENUID = M.ID  AND U.ENABLE_FLAG = 1  and length(to_char(m.id)) < 8
                         union all
                            select NVL(m.id, 0) MENUID, U.MODULECODE || lpad(to_char(mod(m.id, 100)), 2, '0') MODULECODE, m.name MODULENAME, U.MODULECODE parentid
                              from USERMODULE U, MENU M
                             where U.ENABLE_FLAG = 1 AND M.PLATFORMID = 1 and length(to_char(m.id)) = 8
                                   and u.menuid = trunc(m.id / 100)
                         union all
                            select NVL(MENUID, 0), MODULECODE, MODULENAME, nvl(substr(MODULECODE, 0, LENGTH(MODULECODE) - 2), 0) parentid 
                              from USERMODULE
                             where ENABLE_FLAG = 1 and(MENUID is null or MENUID = 0)) Z left join ROLE_MENU R ON Z.MENUID = R.MENUID AND Z.MODULECODE = R.MODULECODE AND R.ROLEID = " + Data.ROLEID + @"
                          order by MODULECODE,MENUID";

            List<USERMODULEEntity> um = DbHelper.ExecuteTable(sql1).ToList<USERMODULEEntity>(); ;
            List<TreeEntity> treeList = new List<TreeEntity>();
            foreach (var item in um)
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

            //费用
            string sqlFee = $@" SELECT TRIMID FROM  ROLE_FEE WHERE 1=1";
            if (!Data.ROLEID.IsEmpty())
                sqlFee += (" AND ROLEID= " + Data.ROLEID);
            DataTable fee = DbHelper.ExecuteTable(sqlFee);


            //业态
            string sqlYt = $@" SELECT YTID ID FROM  ROLE_YT WHERE 1=1";
            if (!Data.ROLEID.IsEmpty())
                sqlYt += (" AND ROLEID= " + Data.ROLEID);
            DataTable yt = DbHelper.ExecuteTable(sqlYt);

            var regionTreeData = regionQxData(Data);

            //业态
            string sqlYt2 = "select G.CATEGORYID,G.CATEGORYCODE,G.CATEGORYNAME,Y.YTID LEVEL_LAST from CATEGORY G,ROLE_YT Y where G.CATEGORYID =Y.YTID(+) ";
            sqlYt2 += " AND Y.ROLEID(+)= " + Data.ROLEID;

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
            //门店
            string sqlbranch = $@"select B.ID BRANCHID,B.NAME from BRANCH B,ROLE_BRANCH R WHERE B.ID=R.BRANCHID ";
            if (!Data.ROLEID.IsEmpty())
                sqlbranch += (" AND ROLEID= " + Data.ROLEID);
            DataTable branch = DbHelper.ExecuteTable(sqlbranch);

            //预警
            string sqlalert = $@"select B.ID ALERTID,B.MC NAME from DEF_ALERT B,ROLE_ALERT R WHERE B.ID=R.ALERTID";
            if (!Data.ROLEID.IsEmpty())
                sqlalert += (" AND ROLEID= " + Data.ROLEID);
            DataTable alert = DbHelper.ExecuteTable(sqlalert);

            return new Tuple<dynamic, List<TreeEntity>, TreeModel[], List<TreeEntity>, DataTable, DataTable, DataTable>(role.ToOneLine(), module, ytTreeData, regionTreeData, fee, branch, alert);
        }
        public Tuple<List<TreeEntity>, TreeModel[], List<TreeEntity>, DataTable, DataTable, DataTable> GetRoleInit()
        {
            string sql1 = @"(select NVL(U.MENUID,0) MENUID,U.MODULECODE,U.MODULENAME,nvl(substr(MODULECODE,0,LENGTH(MODULECODE)-2),0) parentid  
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

            List<USERMODULEEntity> um = DbHelper.ExecuteTable(sql1).ToList<USERMODULEEntity>();
            List<TreeEntity> treeList = new List<TreeEntity>();
            foreach (var item in um)
            {
                TreeEntity node = new TreeEntity();
                node.value = item.MENUID;
                node.code = item.MODULECODE;
                node.title = item.MODULENAME;
                node.expand = false;
                node.parentId = item.PARENTID;
                treeList.Add(node);
            }
            var module=treeList.ToTree();

            //费用
            string sqlitem2 = $@"select A.TRIMID,A.NAME from FEESUBJECT A  order by A.TRIMID";
            DataTable fee = DbHelper.ExecuteTable(sqlitem2);

            var role = new ROLEEntity();
            //区域数据
            var regionTreeData = regionQxData(role);

            //业态树
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

            //门店
            string sqlbranch = $@"select B.ID BRANCHID,B.NAME from BRANCH B where 1=1 "
                               + "   and B.ID IN ("+GetPermissionSql(PermissionType.Branch)+")"                                   
                               + " order by B.ID";
            DataTable branch = DbHelper.ExecuteTable(sqlbranch);
            //预警
            string sqlalert = $@"select B.ID ALERTID,B.MC NAME from DEF_ALERT B where 1=1 "
                            + "     and B.ID IN (" + GetPermissionSql(PermissionType.Alert) + ")"
                            + "   order by B.ID";
            DataTable alert = DbHelper.ExecuteTable(sqlalert);

            return new Tuple<List<TreeEntity>, TreeModel[], List<TreeEntity>, DataTable, DataTable, DataTable>(module, ytTreeData, regionTreeData, fee, branch, alert);
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
    }
}
