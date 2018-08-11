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
            string sql = $@"SELECT A.*,B.NAME,C.NAME MERNAME,D.SHOPDM,E.BRANDNAME FROM CONTRACT A,BRANCH B,MERCHANT C,CONTRACT_SHOPXX D,CONTRACT_BRANDXX E " +
                         " WHERE A.BRANCHID=B.ID AND A.MERCHANTID=C.MERCHANTID AND A.CONTRACTID=D.CONTRACTID AND A.CONTRACTID=E.CONTRACTID ";

            item.HasKey("MERCHANTID", a => sql += $" and C.MERCHANTID  LIKE '%{a}%'");
            item.HasKey("NAME", a => sql += $" and C.NAME  LIKE '%{a}%'");
            item.HasKey("CONTRACTID", a => sql += $" and A.CONTRACTID = '{a}'");
            item.HasKey("STYLE", a => sql += $" and A.STYLE = '{a}'");
            item.HasArrayKey("HTLX", a => sql += $" and A.HTLX in ( { a.SuperJoin(",", b => "'" + b + "'") } ) ");
            item.HasKey("BRANCHID", a => sql += $" and A.BRANCHID = '{a}'");

            item.HasKey("SHOPDM", a => sql += $" and exists(select 1 from CONTRACT_SHOP P,SHOP U where  P.SHOPID=U.SHOPID and P.CONTRACTID=A.CONTRACTID and UPPER(U.CODE) LIKE '{a.ToUpper()}%')");
            item.HasKey("BRANDNAME", a => sql += $" and exists(select 1 from CONTRACT_BRAND P,BRAND U where  P.BRANDID=U.ID and P.CONTRACTID=A.CONTRACTID and UPPER(U.NAME) LIKE '{a.ToUpper()}%')");
            sql += " ORDER BY  D.SHOPDM";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            dt.NewEnumColumns<核算方式>("STYLE", "STYLEMC");
            return new DataGridResult(dt, count);
        }

        public string SaveContract(CONTRACTEntity SaveData)
        {
            var v = GetVerify(SaveData);
            ORGEntity org = DbHelper.Select(new ORGEntity() { ORGID = SaveData.ORGID });
            if (org.BRANCHID != SaveData.BRANCHID) {
                throw new LogicException($"请核对招商部门与分店之间的关系!");
            };
            //选择是扣点的时候,不应该有保底数据
            if ((SaveData.OPERATERULE.ToInt() == (int)联营合同合作方式.扣点)
                && (SaveData.STYLE.ToInt()==(int)核算方式.联营合同)) {
                foreach (var rent in SaveData.CONTRACT_RENT) {
                    if (rent.RENTS.ToDecimal() != 0) {
                        throw new LogicException($"扣点形式的合同不应该有保底值!");
                    }
                    if (rent.RENTS_JSKL.ToDecimal() != 0)
                    {
                        throw new LogicException($"扣点形式的合同不应该有保底扣率!");
                    }
                }
            }

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
            string sql = $@"SELECT A.*,B.NAME MERNAME,C.NAME FDNAME,D.ORGNAME,E.CONTRACTID_OLD,E.JHRQ,";
            sql += " (select NAME from FEERULE L where L.ID=A.FEERULE_RENT) FEERULE_RENTNAME,";
            sql += " F.NAME AS OPERATERULENAME  FROM CONTRACT A,MERCHANT B,";
            sql += "   BRANCH C, ORG D,CONTRACT_UPDATE E,OPERATIONRULE F WHERE A.MERCHANTID=B.MERCHANTID AND A.CONTRACTID=E.CONTRACTID(+) ";
            sql += " AND A.BRANCHID=C.ID AND A.ORGID=D.ORGID AND A.OPERATERULE=F.ID ";
            sql += (" AND A.CONTRACTID= " + Data.CONTRACTID);
            DataTable contract = DbHelper.ExecuteTable(sql);
            if (!contract.IsNotNull())
            {
                throw new LogicException("找不到租约!");
            }

            contract.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            contract.NewEnumColumns<联营合同合作方式>("OPERATERULE", "OPERATERULEMC");

            contract.NewEnumColumns<起始日清算>("QS_START", "QS_STARTMC");
            contract.NewEnumColumns<销售额标记>("TAB_FLAG", "TAB_FLAGMC");




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
            var ym = dt.Year * 100 + dt.Month;
            switch (feeRule.PAY_CYCLE.ToInt()) {
                case 3:
                    switch (dt.Month) {
                        case 1:
                        case 2:
                        case 3:
                            ym = dt.Year * 100 +1;
                            break;
                        case 4:
                        case 5:
                        case 6:
                            ym = dt.Year * 100 + 4;
                            break;
                        case 7:
                        case 8:
                        case 9:
                            ym = dt.Year * 100 + 7;
                            break;
                        case 10:
                        case 11:
                        case 12:
                            ym = dt.Year * 100 + 10;
                            break;
                    }
                    break;
                default:
                    ym = dt.Year * 100 + dt.Month;
                    break;
            }


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
                if (feeRule.FEE_DAY.ToInt() == -1){
                    PERIODEntity PerioYm = new PERIODEntity();
                    PerioYm = DbHelper.Select(new PERIODEntity() { YEARMONTH = (scn*100+ scy).ToString() });
                    zjfj.CREATEDATE = PerioYm.DATE_END;
                }
                else {
                    zjfj.CREATEDATE = (new DateTime(scn, scy, feeRule.FEE_DAY.ToInt())).ToString().ToDateTime().ToString();
                }
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
                    double zts= Math.Abs((per.DATE_END.ToDateTime() - per.DATE_START.ToDateTime()).Days) + 1;

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
                            if (zjfjTs != zts)
                            {
                                //30后期增加系统参数去处理
                                zjfj.RENTS = (Math.Round(je / 30 * zjfjTs, 0, MidpointRounding.AwayFromZero)).ToString();
                            }
                            else {
                                zjfj.RENTS = je.ToString();
                            }
                           
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



        public string ExecHtBgData(CONTRACTEntity Data)
        {
            CONTRACTEntity con = DbHelper.Select(Data);

            if (con.STATUS != ((int)普通单据状态.未审核).ToString())
            {
                throw new LogicException($"租约({Data.CONTRACTID})已经不是未审核不能继续审核!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
               
                //EXEC_CONTRACT exec_contract = new EXEC_CONTRACT()
                //{
                //    V_CONTRACTID = Data.CONTRACTID,
                //    V_USERID = employee.Id
                //};
                //DbHelper.ExecuteProcedure(exec_contract);
                Tran.Commit();
            }

            return con.CONTRACTID;
            throw new LogicException($"租约变更审核过程待完善!");
        }

        public DataGridResult GetFreeShopList(SearchItem item)
        {
            string sql = $@"select L.* ,B.NAME BRANCHNAME,M.NAME MERCHANTNAME,M.MERCHANTID " +
                " from FREESHOP L,BRANCH B,CONTRACT C,MERCHANT M " +
                "  where L.BRANCHID =B.ID and L.CONTRACTID=C.CONTRACTID " +
                "  and C.MERCHANTID=M.MERCHANTID";
            item.HasKey("BILLID", a => sql += $" and L.BILLID = {a}");
            item.HasKey("CONTRACTID", a => sql += $" and L.CONTRACTID={a}");
            item.HasKey("MERCHANTID", a => sql += $" and L.MERCHANTID={a}");
            item.HasKey("FREEDATE_START ", a => sql += $" and L.FREEDATE>={a}");
            item.HasKey("FREEDATE_END", a => sql += $" and L.FREEDATE<={a}");
            item.HasKey("STATUS", a => sql += $" and L.STATUS={a}");
            item.HasKey("REPORTER", a => sql += $" and L.REPORTER={a}");
            item.HasKey("REPORTER_TIME_START", a => sql += $" and L.REPORTER_TIME>={a}");
            item.HasKey("REPORTER_TIME_END", a => sql += $" and L.REPORTER_TIME<={a}");
            item.HasKey("VERIFY", a => sql += $" and L.VERIFY={a}");
            item.HasKey("VERIFY_TIME_START", a => sql += $" and L.VERIFY_TIME>={a}");
            item.HasKey("VERIFY_TIME_END", a => sql += $" and L.VERIFY_TIME<={a}");
            sql += " ORDER BY  L.BILLID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<退铺单状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }
        public string SaveFreeShop(FREESHOPEntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.BILLID.IsEmpty())
                SaveData.BILLID = NewINC("FREESHOP");
            SaveData.STATUS = ((int)退铺单状态.未审核).ToString();
            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            SaveData.VERIFY = employee.Id;
            v.Require(a => a.BILLID);
            v.Require(a => a.BRANCHID);
            v.Require(a => a.CONTRACTID);
            v.Verify();

            using (var Tran = DbHelper.BeginTransaction())
            {
                SaveData.FREESHOPITEM?.ForEach(item =>
                {
                    GetVerify(item).Require(a => a.SHOPID);
                });
                DbHelper.Save(SaveData);
                Tran.Commit();
            }
            return SaveData.BILLID;
        }

        public object GetContractList(CONTRACTEntity Data)
        {
            string sql = $@"select T.MERCHANTID,S.NAME SHMC,T.STYLE,T.BRANCHID,  "
               + " T.REPORTER_NAME,T.REPORTER_TIME "
               + "  from CONTRACT T,MERCHANT S where T.MERCHANTID=S.MERCHANTID ";
            if (!Data.CONTRACTID.IsEmpty())
                sql += (" and T.CONTRACTID= " + Data.CONTRACTID);
            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.NewEnumColumns<核算方式>("STYLE", "STYLEMC");

            string sql_shop = $@"SELECT P.SHOPID,P.CATEGORYID,S.CODE,Y.CATEGORYCODE,Y.CATEGORYNAME,T.BRANCHID "
                + " FROM CONTRACT_SHOP P,SHOP S,CATEGORY Y,CONTRACT T "
                + " WHERE  P.SHOPID=S.SHOPID and P.CATEGORYID=Y.CATEGORYID and P.CONTRACTID = T.CONTRACTID";
            if (!Data.CONTRACTID.IsEmpty())
                sql_shop += (" and P.CONTRACTID= " + Data.CONTRACTID);
            sql_shop += " order by S.CODE";
            DataTable shop = DbHelper.ExecuteTable(sql_shop);
                        
            var result = new
            {
                contract = dt,
                shop = shop,
            };

            return result;
        }

        public object ShowOneFreeShopEdit(FREESHOPEntity Data)
        {
            string sql = $@" SELECT L.*,M.MERCHANTID,M.NAME SHMC FROM FREESHOP L,CONTRACT C,MERCHANT M";
            sql += "  where L.CONTRACTID=C.CONTRACTID and C.MERCHANTID=M.MERCHANTID ";
            if (!Data.BILLID.IsEmpty())
                sql += (" and L.BILLID= " + Data.BILLID);
            DataTable dt = DbHelper.ExecuteTable(sql);            

            string sqlshop = $@"SELECT G.*,S.CODE,Y.CATEGORYCODE,Y.CATEGORYNAME " +
                "  FROM FREESHOPITEM G,SHOP S,CATEGORY Y  " +
                "  WHERE G.SHOPID=S.SHOPID AND S.CATEGORYID= Y.CATEGORYID";
            if (!Data.BILLID.IsEmpty())
                sqlshop += (" and G.BILLID= " + Data.BILLID);
            DataTable dtshop = DbHelper.ExecuteTable(sqlshop);

            var result = new
            {
                freeShop = dt,
                freeShopItem = new dynamic[] {
                   dtshop
                }
            };
            return result;
        }
        
        public Tuple<dynamic, DataTable> GetFreeShopDetail(FREESHOPEntity Data)
        {
            string sql = $@" SELECT L.*,M.MERCHANTID,M.NAME SHMC,B.NAME BRANCHNAME " +
                " FROM FREESHOP L,CONTRACT C,MERCHANT M,BRANCH B ";
            sql += "  where L.CONTRACTID=C.CONTRACTID and C.MERCHANTID=M.MERCHANTID and C.BRANCHID=B.ID ";
            if (!Data.BILLID.IsEmpty())
                sql += (" and L.BILLID= " + Data.BILLID);
            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.NewEnumColumns<退铺单状态>("STATUS", "STATUSMC");

            string sqlshop = $@"SELECT G.*,S.CODE,Y.CATEGORYCODE,Y.CATEGORYNAME " +
                "  FROM FREESHOPITEM G,SHOP S,CATEGORY Y  " +
                "  WHERE G.SHOPID=S.SHOPID AND S.CATEGORYID= Y.CATEGORYID";
            if (!Data.BILLID.IsEmpty())
                sqlshop += (" and G.BILLID= " + Data.BILLID);
            DataTable dtshop = DbHelper.ExecuteTable(sqlshop);

            return new Tuple<dynamic, DataTable>(dt.ToOneLine(), dtshop);
        }
        public string ExecFreeShop(FREESHOPEntity Data)
        {
            FREESHOPEntity freeShop = DbHelper.Select(Data);
            if (freeShop.STATUS == ((int)退铺单状态.审核).ToString())
            {
                throw new LogicException("单据(" + Data.BILLID + ")已经审核不能再次审核!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                freeShop.VERIFY = employee.Id;
                freeShop.VERIFY_NAME = employee.Name;
                freeShop.VERIFY_TIME = DateTime.Now.ToString();
                freeShop.STATUS = ((int)退铺单状态.审核).ToString();
                DbHelper.Save(freeShop);
                Tran.Commit();
            }
            return freeShop.BILLID;
        }
        public string StopFreeShop(FREESHOPEntity Data)
        {
            FREESHOPEntity freeShop = DbHelper.Select(Data);
            if (Convert.ToDateTime(freeShop.FREEDATE) >= Convert.ToDateTime(DateTime.Now.ToShortString()))
            {
                throw new LogicException("退铺日期大于当前日期不能终止合同!");
            }
            if (freeShop.STATUS == ((int)退铺单状态.终止).ToString())
            {
                throw new LogicException("单据(" + Data.BILLID + ")已经终止不能再次终止!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                STOPFREESHOP stopFreeShop = new STOPFREESHOP()
                {
                    P_BILLID = Data.BILLID,
                    P_TERMINATE = employee.Id
                };
                DbHelper.ExecuteProcedure(stopFreeShop);
                Tran.Commit();
            }
            return freeShop.BILLID;
        }


    }
}
