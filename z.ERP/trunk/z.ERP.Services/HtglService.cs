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
            dt.NewEnumColumns<核算方式>("STYLE", "STYLEMC");
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


        public Tuple<dynamic, DataTable,DataTable,  CONTRACTEntity, CONTRACT_RENTEntity, DataTable, DataTable > GetContractElement(CONTRACTEntity Data)
        {
            //只显示本表数据的用module
            //要显示本表之外的数据用DataTable
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


            CONTRACTEntity ContractParm = new CONTRACTEntity();

            //全表查询,程序层面过滤
            //ContractParm.CONTRACT_GROUP = DbHelper.SelectList(new CONTRACT_GROUPEntity()).Where(a => a.CONTRACTID==Data.CONTRACTID).ToList();

            //查询数据库的时候已经参数过滤
            ContractParm.CONTRACT_GROUP = DbHelper.SelectList(new CONTRACT_GROUPEntity() { CONTRACTID=Data.CONTRACTID }).ToList();

            ContractParm.CONTRACT_RENT = DbHelper.SelectList(new CONTRACT_RENTEntity() { CONTRACTID = Data.CONTRACTID }).ToList();

            ContractParm.CONTJSKL = DbHelper.SelectList(new CONTJSKLEntity() { CONTRACTID = Data.CONTRACTID }).ToList();


            CONTRACT_RENTEntity ContractRentParm = new CONTRACT_RENTEntity();
            ContractRentParm.CONTRACT_RENTITEM= DbHelper.SelectList(new CONTRACT_RENTITEMEntity() { CONTRACTID = Data.CONTRACTID }).ToList();


            string sqlPay = $@"SELECT * FROM CONTRACT_PAY WHERE 1=1 ";
            sqlPay += (" AND CONTRACTID= " + Data.CONTRACTID);
            sqlPay += " ORDER BY PAYID,STARTDATE";
            DataTable contract_pay = DbHelper.ExecuteTable(sqlPay);


            string sqlCost = $@"SELECT * FROM CONTRACT_COST WHERE 1=1 ";
            sqlCost += (" AND CONTRACTID= " + Data.CONTRACTID);
            sqlCost += " ORDER BY TERMID,STARTDATE";
            DataTable contract_cost = DbHelper.ExecuteTable(sqlCost);


            return new Tuple<dynamic,DataTable, DataTable, CONTRACTEntity, CONTRACT_RENTEntity,DataTable, DataTable>(
                contract.ToOneLine(),
                contract_brand, 
                contract_shop,
                ContractParm,
                ContractRentParm,
                contract_pay,
                contract_cost
                );
        }

        public List<CONTRACT_RENTITEMEntity> LyYdfj(List<CONTRACT_RENTEntity> Data,CONTRACTEntity ContractData) {

            List<CONTRACT_RENTITEMEntity> zjfjList = new List<CONTRACT_RENTITEMEntity>();

            foreach (var ydfj in Data)
            {
                List<PERIODEntity> Period = new List<PERIODEntity>();

                //月度分解的数据量不是太多可以这样处理，先全查出来,然后数据层面去过滤需要的数据
                Period = DbHelper.SelectList(new PERIODEntity()).
                    Where(a => (a.DATE_START.ToDateTime() <= ydfj.ENDDATE.ToDateTime())
                  && (a.DATE_END.ToDateTime() >= ydfj.STARTDATE.ToDateTime())).ToList();

                foreach (var per in Period)
                {
                    CONTRACT_RENTITEMEntity zjfj = new CONTRACT_RENTITEMEntity();

                    if ((per.DATE_START.ToDateTime() < ContractData.CONT_START.ToDateTime())
                        ||(per.DATE_START.ToDateTime()<ydfj.STARTDATE.ToDateTime()))
                    {
                        per.DATE_START = ydfj.STARTDATE;
                    };

                    if ((per.DATE_END.ToDateTime() > ContractData.CONT_END.ToDateTime())
                        || (per.DATE_END.ToDateTime() > ydfj.ENDDATE.ToDateTime()))
                    {
                        per.DATE_END = ydfj.ENDDATE;
                    };

                    zjfj.STARTDATE = per.DATE_START;
                    zjfj.ENDDATE = per.DATE_END;
                    zjfj.INX = ydfj.INX;
                    zjfj.RENTS = ydfj.RENTS;
                    zjfj.CREATEDATE = per.DATE_END;
                    zjfj.YEARMONTH = per.YEARMONTH;
                    zjfjList.Add(zjfj);
                }
            };
            return zjfjList;
        }
    }
}
