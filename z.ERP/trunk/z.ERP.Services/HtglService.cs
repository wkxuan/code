using System.Data;
using z.MVC5.Results;
using z.ERP.Entities;
using System.Collections.Generic;
using z.Extensions;
using System;
using z.ERP.Entities.Enum;
using z.Exceptions;
using System.Linq;
using z.Extensiont;

namespace z.ERP.Services
{
    public class HtglService: ServiceBase
    {
        internal HtglService()
        {
        }
        public DataGridResult GetContract(SearchItem item)
        {
            string sql = $@"SELECT A.*,B.NAME,C.NAME MERNAME FROM CONTRACT A,BRANCH B,MERCHANT C "+
                         " WHERE A.BRANCHID=B.ID AND A.MERCHANTID=C.MERCHANTID ";
            item.HasKey("CONTRACTID", a => sql += $" and A.CONTRACTID = '{a}'");
            item.HasArrayKey("STYLE", a => sql += $" and A.STYLE in ( { a.SuperJoin(",", b => "'" + b + "'") } ) ");
            sql += " ORDER BY   A.CONTRACTID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }

        public string SaveContract(CONTRACTEntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.CONTRACTID.IsEmpty())
            {
                SaveData.CONTRACTID = NewINC("CONTRACT");
                SaveData.STATUS = ((int)合同状态.未审核).ToString();
            }
            else
            {
                //从界面上传回来,后面优化
                CONTRACTEntity con = DbHelper.Select(SaveData);
                if (con.STATUS == ((int)合同状态.审核).ToString())
                {
                    throw new LogicException("租约(" + SaveData.CONTRACTID + ")已经审核!");
                }
                SaveData.VERIFY = con.VERIFY;
                SaveData.VERIFY_NAME = con.VERIFY_NAME;
                SaveData.VERIFY_TIME = con.VERIFY_TIME;
            }
            SaveData.STYLE= ((int)核算方式.联营合同).ToString();
            if (SaveData.CONTRACT_OLD.IsEmpty())
            {
                SaveData.HTLX = ((int)合同类型.原始合同).ToString();
            }
            else {
                SaveData.HTLX = ((int)合同类型.变更合同).ToString();
            }

            SaveData.QZRQ = SaveData.CONT_START;

            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();

            using (var Tran = DbHelper.BeginTransaction())
            {
                DbHelper.Save(SaveData);

                Tran.Commit();
            }
            return SaveData.CONTRACTID;
        }


        public Tuple<dynamic, DataTable,DataTable> GetContractElement(CONTRACTEntity Data)
        {
            if (Data.CONTRACTID.IsEmpty())
            {
                throw new LogicException("请确认租约编号!");
            }
            string sql = $@"SELECT * FROM CONTRACT WHERE 1=1 ";
            sql += (" AND CONTRACTID= " + Data.CONTRACTID);
            DataTable contract = DbHelper.ExecuteTable(sql);

            contract.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");


            string sqlitem = $@"SELECT B.BRANDID,C.NAME " +
                             " FROM  CONTRACT A,CONTRACT_BRAND B,BRAND C " +
                             " where A.CONTRACTID = B.CONTRACTID AND B.BRANDID=C.ID  ";
            sqlitem += (" and A.CONTRACTID= " + Data.CONTRACTID);
            DataTable contract_brand = DbHelper.ExecuteTable(sqlitem);

            string sqlshop = $@"SELECT B.SHOPID,C.CODE,D.CATEGORYCODE,D.CATEGORYID," +
                           " D.CATEGORYNAME,B.AREA,B.AREA_RENTABLE" +
                           " FROM CONTRACT A,CONTRACT_SHOP B,SHOP C,CATEGORY D" +
                           " WHERE A.CONTRACTID=B.CONTRACTID AND B.SHOPID=C.SHOPID AND B.CATEGORYID=D.CATEGORYID";
            sqlshop += (" and A.CONTRACTID= " + Data.CONTRACTID);
            DataTable contract_shop = DbHelper.ExecuteTable(sqlshop);
            return new Tuple<dynamic, DataTable, DataTable>(contract.ToOneLine(), contract_brand, contract_shop);
        }
    }
}
