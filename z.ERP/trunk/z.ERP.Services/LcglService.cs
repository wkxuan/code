using System;
using System.Collections.Generic;
using System.Data;
using z.ERP.Entities;
using z.ERP.Entities.Enum;
using z.ERP.Entities.Procedures;
using z.Exceptions;
using z.Extensions;
using z.MVC5.Results;
using z.SSO.Model;

namespace z.ERP.Services
{
    public class LcglService : ServiceBase
    {
        internal LcglService()
        {
        }
        #region 审批流定义
        /// <summary>
        /// 模板定义
        /// </summary>
        /// <returns></returns>
        public DataTable GetApprovalData(string branchid) {
            string sql = $@"SELECT A.APPRID,A.NAME,0 STATUS FROM APPROVAL A ORDER BY APPRID";
            DataTable dt= DbHelper.ExecuteTable(sql);
            string sql1 = $@"SELECT A.APPRID,A.NAME,NVL(AB.STATUS ,0) STATUS FROM APPROVAL A,APPROVAL_BRANCH AB WHERE A.APPRID=AB.APPRID(+) AND AB.BRANCHID={branchid}";
            DataTable dt1= DbHelper.ExecuteTable(sql1);
            if (dt1.Rows.Count > 0)
            {
                foreach (DataRow item in dt1.Rows)
                {
                    foreach (DataRow items in dt.Rows)
                    {
                        if (item["APPRID"].ToString() == items["APPRID"].ToString())
                        {
                            items["STATUS"] = item["STATUS"].ToString();
                        }                        
                    }
                }
            }           
            return dt;
        }

        public string Switchchange(string branchid, string apprid) {
            APPROVAL_BRANCHEntity ab = DbHelper.Select(new APPROVAL_BRANCHEntity() { APPRID = apprid, BRANCHID = branchid });
            if (ab.STATUS == "2")
            {
                ab.STATUS = "1";
            }
            else {
                ab.STATUS = "2";
            }
            DbHelper.Save(ab);
            return ab.STATUS;
        }
        public Tuple<DataTable,DataTable> ShowDetail(string branchid, string apprid) {
            string sql = $@"SELECT * FROM APPROVAL_NODE WHERE BRANCHID={branchid} AND APPRID={apprid} ORDER BY NODE_INX";
            DataTable dt = DbHelper.ExecuteTable(sql);
            string sql1 = $@"SELECT A.*,R.ROLENAME OPER_NAME FROM APPROVAL_NODE_OPER A,APPROVAL_NODE B,ROLE R 
                                WHERE A.APPR_NODE_ID=B.APPR_NODE_ID AND R.ROLEID=A.OPER_DATA AND A.OPER_TYPE=1 AND B.BRANCHID={branchid} AND B.APPRID={apprid}
                                UNION ALL
                            SELECT A.*,U.USERNAME OPER_NAME FROM APPROVAL_NODE_OPER A,APPROVAL_NODE B,SYSUSER U 
                                WHERE A.APPR_NODE_ID=B.APPR_NODE_ID AND U.USERID=A.OPER_DATA AND A.OPER_TYPE=2 AND B.BRANCHID={branchid} AND B.APPRID={apprid}";
            DataTable dt1 = DbHelper.ExecuteTable(sql1);
            return new Tuple<DataTable, DataTable>(dt,dt1);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="SaveData">门店模板表</param>
        /// <param name="SaveDataDetail">节点明细</param>
        /// <param name="SaveDataOper">节点权限</param>
        /// <returns></returns>
        public string SaveApprovalData(APPROVAL_BRANCHEntity SaveData, List<APPROVAL_NODEEntity> SaveDataDetail, List<APPROVAL_NODE_OPEREntity> SaveDataOper) {
            using (var Tran = DbHelper.BeginTransaction())
            {
                APPROVAL_BRANCHEntity ab = DbHelper.Select(new APPROVAL_BRANCHEntity() { APPRID = SaveData.APPRID, BRANCHID = SaveData.BRANCHID });
                if (ab == null)
                {
                    SaveData.STATUS = "2";
                    DbHelper.Save(SaveData);    //门店模板表保存
                }
                else {
                    string sql = $@"DELETE FROM APPROVAL_NODE WHERE APPR_NODE_ID LIKE '" + SaveData.BRANCHID + DY10(SaveData.APPRID) + "%'";
                    string sql1 = $@"DELETE FROM APPROVAL_NODE_OPER WHERE APPR_NODE_ID LIKE '" + SaveData.BRANCHID + DY10(SaveData.APPRID) + "%'";
                    DbHelper.ExecuteNonQuery(sql);
                    DbHelper.ExecuteNonQuery(sql1);
                }               
                foreach (var item in SaveDataDetail)
                {
                    var v = GetVerify(item);
                    v.IsUnique(a => a.APPR_NODE_ID);
                    v.Require(a => a.APPR_NODE_ID);
                    v.Require(a => a.APPRID);
                    v.Require(a => a.BRANCHID);
                    v.Require(a => a.NODE_TITLE);
                    v.Require(a => a.NODE_INX);
                    v.Require(a => a.NEXT_APPR_NODE_ID);
                    v.Verify();
                    DbHelper.Save(item);  //节点保存
                }
                foreach (var itemo in SaveDataOper) {
                    DbHelper.Save(itemo);
                }
                Tran.Commit();
            }
            return SaveData.STATUS;
        }
        public string DY10(string I) {
            if (Convert.ToInt32(I) < 10)
            {
                return "0" + I;
            }
            else
            {
                return I;
            }
        }   
        #endregion
    }
}

