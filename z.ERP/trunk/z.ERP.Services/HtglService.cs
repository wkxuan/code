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
using z.ERP.Entities.Procedures;

namespace z.ERP.Services
{
    public class HtglService : ServiceBase
    {
        internal HtglService()
        {
        }
        public DataGridResult GetContract(SearchItem item)
        {
            string sql = $@"SELECT A.*,B.NAME,C.NAME MERNAME FROM CONTRACT A,BRANCH B,MERCHANT C " +
                         " WHERE A.BRANCHID=B.ID AND A.MERCHANTID=C.MERCHANTID ";

            item.HasKey("MERCHANTID", a => sql += $" and C.MERCHANTID  LIKE '%{a}%'");
            item.HasKey("NAME", a => sql += $" and C.NAME  LIKE '%{a}%'");
            item.HasKey("CONTRACTID", a => sql += $" and A.CONTRACTID = '{a}'");
            item.HasArrayKey("STYLE", a => sql += $" and A.STYLE in ( { a.SuperJoin(",", b => "'" + b + "'") } ) ");
            item.HasArrayKey("HTLX", a => sql += $" and A.HTLX in ( { a.SuperJoin(",", b => "'" + b + "'") } ) ");
            item.HasKey("BRANCHID", a => sql += $" and A.BRANCHID = '{a}'");
            sql += " ORDER BY  A.CONTRACTID DESC";
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
                    throw new LogicException($"租约({SaveData.CONTRACTID})已经审核!");
                }
                SaveData.VERIFY = con.VERIFY;
                SaveData.VERIFY_NAME = con.VERIFY_NAME;
                SaveData.VERIFY_TIME = con.VERIFY_TIME;
            }
            if (SaveData.CONTRACT_OLD.IsEmpty())
            {
                SaveData.HTLX = ((int)合同类型.原始合同).ToString();
            }
            else
            {
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


        public Tuple<dynamic, DataTable, DataTable, CONTRACTEntity, CONTRACT_RENTEntity, DataTable, DataTable> GetContractElement(CONTRACTEntity Data)
        {
            //只显示本表数据的用module
            //要显示本表之外的数据用DataTable
            if (Data.CONTRACTID.IsEmpty())
            {
                throw new LogicException("请确认租约编号!");
            }
            string sql = $@"SELECT A.*,B.NAME MERNAME,C.NAME FDNAME,D.ORGNAME  FROM CONTRACT A,MERCHANT B,";
            sql += "   BRANCH C, ORG D WHERE A.MERCHANTID=B.MERCHANTID ";
            sql += " AND A.BRANCHID=C.ID AND A.ORGID=D.ORGID ";
            sql += (" AND CONTRACTID= " + Data.CONTRACTID);
            DataTable contract = DbHelper.ExecuteTable(sql);
            if (!contract.IsNotNull())
            {
                throw new LogicException("找不到租约!");
            }

            contract.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            contract.NewEnumColumns<联营合同合作方式>("OPERATERULE", "OPERATERULEMC");
            contract.NewEnumColumns<合作方式>("OPERATERULE", "OPERATERULENAME");
            




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
            ContractParm.CONTRACT_GROUP = DbHelper.SelectList(new CONTRACT_GROUPEntity() { CONTRACTID = Data.CONTRACTID }).ToList();

            ContractParm.CONTRACT_RENT = DbHelper.SelectList(new CONTRACT_RENTEntity() { CONTRACTID = Data.CONTRACTID }).ToList();

            ContractParm.CONTJSKL = DbHelper.SelectList(new CONTJSKLEntity() { CONTRACTID = Data.CONTRACTID }).ToList();


            CONTRACT_RENTEntity ContractRentParm = new CONTRACT_RENTEntity();
            ContractRentParm.CONTRACT_RENTITEM = DbHelper.SelectList(new CONTRACT_RENTITEMEntity() { CONTRACTID = Data.CONTRACTID }).ToList();


            string sqlPay = $@"SELECT A.*,B.NAME,C.NAME TERMNAME FROM CONTRACT_PAY A,PAY B,FEESUBJECT C WHERE A.PAYID=B.PAYID AND A.TERMID=C.TRIMID ";
            sqlPay += (" AND A.CONTRACTID= " + Data.CONTRACTID);
            sqlPay += " ORDER BY A.PAYID,A.STARTDATE";
            DataTable contract_pay = DbHelper.ExecuteTable(sqlPay);


            string sqlCost = $@"SELECT A.*,B.NAME FROM CONTRACT_COST A,FEESUBJECT B WHERE A.TERMID=B.TRIMID ";
            sqlCost += (" AND CONTRACTID= " + Data.CONTRACTID);
            sqlCost += " ORDER BY TERMID,STARTDATE";
            DataTable contract_cost = DbHelper.ExecuteTable(sqlCost);

            contract_cost.NewEnumColumns<月费用收费方式>("SFFS", "SFFSMC");


            return new Tuple<dynamic, DataTable, DataTable, CONTRACTEntity, CONTRACT_RENTEntity, DataTable, DataTable>(
                contract.ToOneLine(),
                contract_brand,
                contract_shop,
                ContractParm,
                ContractRentParm,
                contract_pay,
                contract_cost
                );
        }

        public List<CONTRACT_RENTITEMEntity> LyYdfj(List<CONTRACT_RENTEntity> Data, CONTRACTEntity ContractData)
        {

            //当月度分解没定义的时候抛出异常提示待完善
            List<CONTRACT_RENTITEMEntity> zjfjList = new List<CONTRACT_RENTITEMEntity>();

            foreach (var ydfj in Data)
            {
                List<PERIODEntity> Period = new List<PERIODEntity>();
                Period = DbHelper.SelectList(new PERIODEntity()).
                    Where(a => (a.DATE_START.ToDateTime() <= ydfj.ENDDATE.ToDateTime())
                  && (a.DATE_END.ToDateTime() >= ydfj.STARTDATE.ToDateTime())).OrderBy(b => b.YEARMONTH).ToList();

                foreach (var per in Period)
                {
                    CONTRACT_RENTITEMEntity zjfj = new CONTRACT_RENTITEMEntity();

                    double allTs = Math.Abs((per.DATE_END.ToDateTime() - per.DATE_START.ToDateTime()).Days) + 1;


                    if ((per.DATE_START.ToDateTime() < ContractData.CONT_START.ToDateTime())
                        || (per.DATE_START.ToDateTime() < ydfj.STARTDATE.ToDateTime()))
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

                    double zjfjTs = Math.Abs((zjfj.ENDDATE.ToDateTime() - zjfj.STARTDATE.ToDateTime()).Days) + 1;

                    zjfj.INX = ydfj.INX;
                    var je = Convert.ToDouble(ydfj.RENTS);
                    var zzJe = Math.Round(je * (zjfjTs / allTs), 2, MidpointRounding.AwayFromZero);
                    zjfj.RENTS = Convert.ToString(zzJe);
                    zjfj.CREATEDATE = per.DATE_END;
                    zjfj.YEARMONTH = per.YEARMONTH;
                    zjfjList.Add(zjfj);
                }
            };
            return zjfjList;
        }

        public List<CONTRACT_RENTITEMEntity> zlYdFj(List<CONTRACT_RENTEntity> Data, CONTRACTEntity ContractData)
        {
            List<CONTRACT_RENTITEMEntity> zjfjList = new List<CONTRACT_RENTITEMEntity>();
            //当月度分解没定义的时候抛出异常提示待完善

            FEERULEEntity feeRule = new FEERULEEntity();
            feeRule = DbHelper.Select(new FEERULEEntity() { ID = ContractData.FEERULE_RENT });

            //PAY_CYCLE缴费周期
            //ADVANCE_CYCLE 提前周期
            //FEE_DAY 出单日

            //先计算出来每个年月对应的生成日期
            DateTime dt = ContractData.CONT_START.ToDateTime();
            //var ym = Convert.ToInt32(dt.Year.ToString()) * 100 + Convert.ToInt32(dt.Month.ToString());
            var ym = dt.Year * 100 + dt.Month;

            List<PERIODEntity> Perio = new List<PERIODEntity>();
            Perio = DbHelper.SelectList(new PERIODEntity()).
                Where(a => (a.DATE_START.ToDateTime() <= ContractData.CONT_END.ToDateTime())
                && (a.DATE_END.ToDateTime() >= ContractData.CONT_START.ToDateTime())).OrderBy(b => b.YEARMONTH).ToList();

            List<CONTRACT_RENTITEMEntity> zjfjListGd = new List<CONTRACT_RENTITEMEntity>();
            foreach (var per in Perio)
            {
                CONTRACT_RENTITEMEntity zjfj = new CONTRACT_RENTITEMEntity();

                var scny = 0;
                //可以通过日期上加月份处理
                if ((ym.ToString().Substring(4, 2).ToInt() - feeRule.ADVANCE_CYCLE.ToInt()) <= 0)
                {
                    scny = (ym.ToString().Substring(0, 4).ToInt() - 1) * 100 +
                        ((ym.ToString().Substring(4, 2).ToInt() + 12 - feeRule.ADVANCE_CYCLE.ToInt()));
                }
                else
                {
                    scny = ym - feeRule.ADVANCE_CYCLE.ToInt();
                }

                var ymLast = 0;
                if ((ym.ToString().Substring(4, 2).ToInt() + feeRule.PAY_CYCLE.ToInt()) > 12)
                {
                    ymLast = (ym.ToString().Substring(0, 4).ToInt() + 1) * 100
                        + ((ym.ToString().Substring(4, 2).ToInt() + feeRule.PAY_CYCLE.ToInt()) - 12);
                }
                else
                {
                    ymLast = ym + feeRule.PAY_CYCLE.ToInt();
                }


                var scn = scny.ToString().Substring(0, 4).ToInt();
                var scy = scny.ToString().Substring(4, 2).ToInt();


                zjfj.YEARMONTH = per.YEARMONTH;
                zjfj.CREATEDATE = (new DateTime(scn, scy, feeRule.FEE_DAY.ToInt())).ToString().ToDateTime().ToString();
                zjfjListGd.Add(zjfj);

                if ((per.YEARMONTH.ToString().Substring(4, 2).ToInt() + 1) > 12)
                {
                    per.YEARMONTH = ((per.YEARMONTH.ToString().Substring(0, 4).ToInt() + 1) * 100
                        + ((per.YEARMONTH.ToString().Substring(4, 2).ToInt() + 1) - 12)).ToString();
                }
                else
                {
                    per.YEARMONTH = (per.YEARMONTH.ToInt() + 1).ToString();
                }


                if (per.YEARMONTH.ToInt() == ymLast)
                {
                    ym = ymLast;
                };
            }

            foreach (var ydfj in Data)
            {
                List<PERIODEntity> Period = new List<PERIODEntity>();

                Period = DbHelper.SelectList(new PERIODEntity()).
                    Where(a => (a.DATE_START.ToDateTime() <= ydfj.ENDDATE.ToDateTime())
                  && (a.DATE_END.ToDateTime() >= ydfj.STARTDATE.ToDateTime())).OrderBy(b => b.YEARMONTH).ToList();

                foreach (var per in Period)
                {

                    CONTRACT_RENTITEMEntity zjfj = new CONTRACT_RENTITEMEntity();
                    if ((per.DATE_START.ToDateTime() < ContractData.CONT_START.ToDateTime())
                        || (per.DATE_START.ToDateTime() < ydfj.STARTDATE.ToDateTime()))
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

                    double zjfjTs = Math.Abs((zjfj.ENDDATE.ToDateTime() - zjfj.STARTDATE.ToDateTime()).Days) + 1;

                    zjfj.INX = ydfj.INX;
                    var je = Convert.ToDouble(ydfj.RENTS);
                    switch (ydfj.DJLX.ToInt())
                    {
                        case 1://日租金
                            zjfj.RENTS = Math.Round(je * zjfjTs, 2, MidpointRounding.AwayFromZero).ToString();
                            break;
                        case 2://月租金
                            zjfj.RENTS = je.ToString();
                            break;
                    };
                    foreach (var scrq in zjfjListGd)
                    {
                        if (per.YEARMONTH.ToInt() == scrq.YEARMONTH.ToInt())
                        {
                            zjfj.CREATEDATE = scrq.CREATEDATE;
                        }
                    }


                    zjfj.YEARMONTH = per.YEARMONTH;
                    zjfjList.Add(zjfj);

                }

            }
            return zjfjList;
        }


        public void DeleteContract(List<CONTRACTEntity> DeleteData)
        {
            foreach (var con in DeleteData)
            {
                CONTRACTEntity Data = DbHelper.Select(con);
                if (Data.STATUS != ((int)普通单据状态.未审核).ToString())
                {
                    throw new LogicException($"租约({Data.CONTRACTID})已经不是未审核不能删除!");
                }
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                foreach (var con in DeleteData)
                {
                    DbHelper.Delete(con);
                }
                Tran.Commit();
            }
        }

        public string ExecData(CONTRACTEntity Data)
        {
            CONTRACTEntity con = DbHelper.Select(Data);

            if (con.STATUS != ((int)普通单据状态.未审核).ToString())
            {
                throw new LogicException($"租约({Data.CONTRACTID})已经不是未审核不能继续审核!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                EXEC_CONTRACT exec_contract = new EXEC_CONTRACT()
                {
                    V_CONTRACTID = Data.CONTRACTID,
                    V_USERID = employee.Id
                };
                DbHelper.ExecuteProcedure(exec_contract);
                Tran.Commit();
            }
          
            return con.CONTRACTID;
        }
    }
}
