using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using z.ERP.Entities;
using z.ERP.Entities.Enum;
using z.ERP.Entities.Procedures;
using z.Exceptions;
using z.Extensions;
using z.MVC5.Results;
using z.SSO.Model;

namespace z.ERP.Services
{
    public class HtglService : ServiceBase
    {
        internal HtglService()
        {
        }
        public DataGridResult GetContract(SearchItem item)
        {
            string sql = $@"SELECT A.*,B.NAME,C.NAME MERNAME,D.SHOPDM,E.BRANDNAME" +
                " FROM CONTRACT A,BRANCH B,MERCHANT C,CONTRACT_SHOPXX D,CONTRACT_BRANDXX E " +
                " WHERE A.BRANCHID=B.ID AND A.MERCHANTID=C.MERCHANTID AND A.CONTRACTID=D.CONTRACTID" +
                " AND A.CONTRACTID=E.CONTRACTID(+) ";
            sql += " AND B.ID IN (" + GetPermissionSql(PermissionType.Branch) + ") ";    //分店权限 by：DZK
            item.HasKey("MERCHANTID", a => sql += $" and C.MERCHANTID  LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and C.NAME  LIKE '%{a}%'");
            item.HasKey("CONTRACTID", a => sql += $" and A.CONTRACTID = '{a}'");
            item.HasKey("STYLE", a => sql += $" and A.STYLE = '{a}'");
            item.HasKey("HTLX", a => sql += $" and A.HTLX = {a}");
            item.HasKey("BRANCHID", a => sql += $" and A.BRANCHID = '{a}'");
            item.HasKey("STATUS", a => sql += $" and A.STATUS = {a}");
            item.HasKey("SIGNER_NAME", a => sql += $" and A.SIGNER_NAME  LIKE '%{a}%'");
            item.HasKey("REPORTER_NAME", a => sql += $" and A.REPORTER_NAME  LIKE '%{a}%'");
            item.HasKey("VERIFY_NAME", a => sql += $" and A.VERIFY_NAME  LIKE '%{a}%'");
            item.HasKey("SHOPDM", a => sql += $" and exists(select 1 from CONTRACT_SHOP P,SHOP U where  P.SHOPID=U.SHOPID and P.CONTRACTID=A.CONTRACTID and UPPER(U.CODE) LIKE '{a.ToUpper()}%')");
            item.HasKey("BRANDNAME", a => sql += $" and exists(select 1 from CONTRACT_BRAND P,BRAND U where  P.BRANDID=U.ID and P.CONTRACTID=A.CONTRACTID and UPPER(U.NAME) LIKE '{a.ToUpper()}%')");
            sql += " ORDER BY  D.SHOPDM";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<合同状态>("STATUS", "STATUSMC");
            dt.NewEnumColumns<核算方式>("STYLE", "STYLEMC");
            return new DataGridResult(dt, count);
        }
        public string GetContractOutput(SearchItem item)
        {
            string sql = $@"SELECT A.*,B.NAME,C.NAME MERNAME,D.SHOPDM,E.BRANDNAME,to_char(A.REPORTER_TIME,'yyyy-mm-dd') DJRQ,to_char(A.VERIFY_TIME,'yyyy-mm-dd') SHRQ"
                         + " FROM CONTRACT A,BRANCH B,MERCHANT C,CONTRACT_SHOPXX D,CONTRACT_BRANDXX E "
                         + " WHERE A.BRANCHID=B.ID AND A.MERCHANTID=C.MERCHANTID AND A.CONTRACTID=D.CONTRACTID AND A.CONTRACTID=E.CONTRACTID ";
            sql += " AND B.ID IN (" + GetPermissionSql(PermissionType.Branch) + ") ";    //分店权限 by：DZK
            item.HasKey("MERCHANTID", a => sql += $" and C.MERCHANTID  LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and C.NAME  LIKE '%{a}%'");
            item.HasKey("CONTRACTID", a => sql += $" and A.CONTRACTID = '{a}'");
            item.HasKey("STYLE", a => sql += $" and A.STYLE = '{a}'");
            // item.HasArrayKey("HTLX", a => sql += $" and A.HTLX in ( { a.SuperJoin(",", b => "'" + b + "'") } ) ");
            item.HasKey("HTLX", a => sql += $" and A.HTLX = {a}");
            item.HasKey("BRANCHID", a => sql += $" and A.BRANCHID = '{a}'");
            item.HasKey("STATUS", a => sql += $" and A.STATUS = {a}");
            item.HasKey("SIGNER", a => sql += $" and A.SIGNER = {a}");
            item.HasKey("REPORTER", a => sql += $" and A.SIGNER = {a}");
            item.HasKey("VERIFY", a => sql += $" and A.SIGNER = {a}");
            item.HasKey("SHOPDM", a => sql += $" and exists(select 1 from CONTRACT_SHOP P,SHOP U where  P.SHOPID=U.SHOPID and P.CONTRACTID=A.CONTRACTID and UPPER(U.CODE) LIKE '{a.ToUpper()}%')");
            item.HasKey("BRANDNAME", a => sql += $" and exists(select 1 from CONTRACT_BRAND P,BRAND U where  P.BRANDID=U.ID and P.CONTRACTID=A.CONTRACTID and UPPER(U.NAME) LIKE '{a.ToUpper()}%')");
            sql += " ORDER BY  D.SHOPDM";

            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.NewEnumColumns<合同状态>("STATUS", "STATUSMC");
            dt.NewEnumColumns<核算方式>("STYLE", "STYLEMC");
            dt.TableName = "ContractList";
            return GetExport("租约列表导出", a =>
            {
                a.SetTable(dt);
            });
        }
        public string SaveContract(CONTRACTEntity SaveData)
        {
            var v = GetVerify(SaveData);

            if (SaveData.STYLE.ToInt() != (int)核算方式.多经点位)
            {
                ORGEntity org = DbHelper.Select(new ORGEntity() { ORGID = SaveData.ORGID });
                if (org.BRANCHID != SaveData.BRANCHID)
                    throw new LogicException($"请核对招商部门与分店之间的关系!");
                //选择是扣点的时候,不应该有保底数据
                if ((SaveData.OPERATERULE.ToInt() == (int)联营合同合作方式.扣点)
                    && (SaveData.STYLE.ToInt() == (int)核算方式.联营合同))
                {
                    foreach (var rent in SaveData.CONTRACT_RENT)
                    {
                        if (rent.RENTS.ToDecimal() != 0)
                            throw new LogicException($"扣点形式的合同不应该有保底值!");
                        if (rent.RENTS_JSKL.ToDecimal() != 0)
                            throw new LogicException($"扣点形式的合同不应该有保底扣率!");
                    }
                }
            }
            // 合同号规则:核算方式(1租赁2联营)+门店(2位)+5位流水  变更合同 9开头
            if (SaveData.CONTRACTID.IsEmpty())
            {
                //合同类型判断
                if (SaveData.CONTRACT_OLD.IsEmpty())
                    SaveData.HTLX = ((int)合同类型.原始合同).ToString();
                else
                    SaveData.HTLX = ((int)合同类型.变更合同).ToString();

                string tblname;
                int leadCode;
                if (SaveData.HTLX == "1")
                    leadCode = (SaveData.STYLE + SaveData.BRANCHID.PadLeft(2, '0')).ToInt();
                else
                    leadCode = ("9" + SaveData.BRANCHID.PadLeft(2, '0')).ToInt();

                tblname = "CONTRACT_" + leadCode.ToString();

                SaveData.CONTRACTID = (NewINC(tblname).ToInt() + leadCode * 100000).ToString();
                SaveData.STATUS = ((int)合同状态.未审核).ToString();
            }
            else
            {
                //从界面上传回来,后面优化
                CONTRACTEntity con = DbHelper.Select(SaveData);
                if (con.STATUS == ((int)合同状态.审核).ToString())
                    throw new LogicException($"租约({SaveData.CONTRACTID})已经审核!");
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

            var dcl = new BILLSTATUSEntity
            {
                BILLID = SaveData.CONTRACTID,
                MENUID = "10600202",
                BRABCHID = SaveData.BRANCHID,
                URL = "HTGL/ZLHT/HtEdit/"
            };
            InsertDclRw(dcl);

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
            sql += " (select NAME from LATEFEERULE LA where LA.ID=A.ZNID_RENT) LATEFEERULENAME,";
            sql += " F.NAME AS OPERATERULENAME  FROM CONTRACT A,MERCHANT B,";
            sql += " BRANCH C, ORG D,CONTRACT_UPDATE E,OPERATIONRULE F WHERE A.MERCHANTID=B.MERCHANTID AND A.CONTRACTID=E.CONTRACTID(+) ";
            sql += " AND A.BRANCHID=C.ID AND A.ORGID=D.ORGID AND A.OPERATERULE=F.ID ";
            sql += " AND C.ID IN (" + GetPermissionSql(PermissionType.Branch) + ")";    //分店权限 by：DZK
            sql += " AND A.CONTRACTID= " + Data.CONTRACTID;
            DataTable contract = DbHelper.ExecuteTable(sql);
            if (!contract.IsNotNull())
            {
                throw new LogicException("找不到租约!");
            }
            contract.NewEnumColumns<核算方式>("STYLE", "STYLEMC");
            contract.NewEnumColumns<合同状态>("STATUS", "STATUSMC");
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

            string sqlCost = $@"SELECT A.*,B.NAME,C.NAME FEERULENAME,D.NAME LATEFEERULENAME,B.TYPE 
                                  FROM CONTRACT_COST A,FEESUBJECT B,FEERULE C,LATEFEERULE D";
            sqlCost += " WHERE A.TERMID=B.TRIMID AND A.FEERULEID=C.ID(+) AND A.ZNGZID=D.ID(+) ";
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
        public Tuple<dynamic, DataTable, DataTable> GetContractDjdwElement(CONTRACTEntity Data)
        {
            if (Data.CONTRACTID.IsEmpty())
            {
                throw new LogicException("请确认租约编号!");
            }
            string sql = $@"SELECT A.*,B.NAME MERNAME,C.NAME FDNAME,D.ORGNAME,E.CONTRACTID_OLD,E.JHRQ,";
            sql += " (select NAME from FEERULE L where L.ID=A.FEERULE_RENT) FEERULE_RENTNAME,";
            sql += " (select NAME from LATEFEERULE LA where LA.ID=A.ZNID_RENT) LATEFEERULENAME,";
            sql += " F.NAME AS OPERATERULENAME  FROM CONTRACT A,MERCHANT B,";
            sql += "   BRANCH C, ORG D,CONTRACT_UPDATE E,OPERATIONRULE F WHERE A.MERCHANTID=B.MERCHANTID AND A.CONTRACTID=E.CONTRACTID(+) ";
            sql += " AND A.BRANCHID=C.ID AND A.ORGID=D.ORGID(+) AND A.OPERATERULE=F.ID(+) ";
            sql += " AND C.ID IN (" + GetPermissionSql(PermissionType.Branch) + ")";    //分店权限 by：DZK
            sql += (" AND A.CONTRACTID= " + Data.CONTRACTID);
            DataTable contract = DbHelper.ExecuteTable(sql);
            if (!contract.IsNotNull())
            {
                throw new LogicException("找不到租约!");
            }

            contract.NewEnumColumns<合同状态>("STATUS", "STATUSMC");

            string sqlshop = $@"SELECT B.SHOPID,C.CODE,D.CATEGORYCODE,D.CATEGORYID," +
                           " D.CATEGORYNAME,B.AREA,B.AREA_RENTABLE" +
                           " FROM CONTRACT A,CONTRACT_SHOP B,SHOP C,CATEGORY D" +
                           " WHERE A.CONTRACTID=B.CONTRACTID AND B.SHOPID=C.SHOPID AND B.CATEGORYID=D.CATEGORYID";
            sqlshop += (" and A.CONTRACTID= " + Data.CONTRACTID);
            DataTable contract_shop = DbHelper.ExecuteTable(sqlshop);

            string sqlCost = $@"SELECT A.*,B.NAME";
            sqlCost += "    FROM CONTRACT_COST_DJDW A,FEESUBJECT B";
            sqlCost += " WHERE A.TREMID=B.TRIMID  ";
            sqlCost += (" AND CONTRACTID= " + Data.CONTRACTID);
            sqlCost += " ORDER BY TREMID";
            DataTable contract_cost = DbHelper.ExecuteTable(sqlCost);

            return new Tuple<dynamic, DataTable, DataTable>(
                contract.ToOneLine(),
                contract_shop,
                contract_cost
                );
        }
        public List<CONTRACT_RENTITEMEntity> LyYdfj(List<CONTRACT_RENTEntity> Data, CONTRACTEntity ContractData)
        {

            //当月度分解没定义的时候抛出异常提示待完善
            List<CONTRACT_RENTITEMEntity> zjfjList = new List<CONTRACT_RENTITEMEntity>();

            CONFIGEntity configBzybj = new CONFIGEntity();
            configBzybj = DbHelper.Select(new CONFIGEntity() { ID = "1004" });

            if (!"012".Contains(configBzybj.CUR_VAL))
                throw new LogicException("参数1004(不足月时月金额算法)设置有误");

            CONFIGEntity configBzyts = new CONFIGEntity();
            configBzyts = DbHelper.Select(new CONFIGEntity() { ID = "1003" });

            if (configBzyts.CUR_VAL.ToInt() <= 0)
                throw new LogicException("参数1003(不足月天数设定)设置有误");

            CONFIGEntity configBlxsw = new CONFIGEntity();
            configBlxsw = DbHelper.Select(new CONFIGEntity() { ID = "1002" });

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

                    if (zjfjTs != allTs) //不足月时金额算法
                    {
                        if (configBzybj.CUR_VAL.ToInt() == 0)   //0(月金额 / 当月总天数) * 实际天数;
                        {
                            zjfj.RENTS = (Math.Round(je / allTs * zjfjTs, configBlxsw.CUR_VAL.ToInt(),
                                MidpointRounding.AwayFromZero)).ToString();
                        }
                        else if (configBzybj.CUR_VAL.ToInt() == 1)  //1:(月金额 / 1003参数设置的天数)*实际天数;
                        {
                            zjfj.RENTS = (Math.Round(je / (configBzyts.CUR_VAL).ToDouble() * zjfjTs,
                            configBlxsw.CUR_VAL.ToInt(), MidpointRounding.AwayFromZero)).ToString();
                        }
                        else  //2:(月金额 * 12 / 365) * 实际天数
                        {
                            zjfj.RENTS = (Math.Round((je * 12 / 365) * zjfjTs,
                            configBlxsw.CUR_VAL.ToInt(), MidpointRounding.AwayFromZero)).ToString();
                        }

                    }
                    else
                    {
                        zjfj.RENTS = Convert.ToString(je);
                    }
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

            CONFIGEntity configBzybj = new CONFIGEntity();
            configBzybj = DbHelper.Select(new CONFIGEntity() { ID = "1004" });

            if (!"012".Contains(configBzybj.CUR_VAL))
                throw new LogicException("参数1004(不足月时月金额算法)设置有误");

            CONFIGEntity configBzyts = new CONFIGEntity();
            configBzyts = DbHelper.Select(new CONFIGEntity() { ID = "1003" });

            if (configBzyts.CUR_VAL.ToInt() <= 0)
                throw new LogicException("参数1003(不足月天数设定)设置有误");

            CONFIGEntity configBlxsw = new CONFIGEntity();
            configBlxsw = DbHelper.Select(new CONFIGEntity() { ID = "1002" });


            //季度分解日期生成
            CONFIGEntity configJDFJGZ = new CONFIGEntity();
            configJDFJGZ = DbHelper.Select(new CONFIGEntity() { ID = "1005" });

            //PAY_CYCLE缴费周期
            //ADVANCE_CYCLE 提前周期
            //FEE_DAY 出单日

            //先计算出来每个年月对应的生成日期
            DateTime dt = ContractData.CONT_START.ToDateTime();
            var ym = dt.Year * 100 + dt.Month;
            if (configJDFJGZ.CUR_VAL.ToInt() == 0)        //添加月份生成参数 0，按自然季度 ，1 顺延季度    BY:DZK  20190717
            {
                switch (feeRule.PAY_CYCLE.ToInt())
                {
                    case 3:
                        switch (dt.Month)
                        {
                            case 1:
                            case 2:
                            case 3:
                                ym = dt.Year * 100 + 1;
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
                    case 6:
                        switch (dt.Month)
                        {
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                                ym = dt.Year * 100 + 1;
                                break;
                            case 7:
                            case 8:
                            case 9:
                            case 10:
                            case 11:
                            case 12:
                                ym = dt.Year * 100 + 7;
                                break;
                        }
                        break;
                    case 12:
                        switch (dt.Month)
                        {
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                            case 8:
                            case 9:
                            case 10:
                            case 11:
                            case 12:
                                ym = dt.Year * 100 + 1;
                                break;
                        }
                        break;
                    default:
                        ym = dt.Year * 100 + dt.Month;
                        break;
                }
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
                if (feeRule.FEE_DAY.ToInt() == -1)
                {
                    PERIODEntity PerioYm = new PERIODEntity();
                    PerioYm = DbHelper.Select(new PERIODEntity() { YEARMONTH = (scn * 100 + scy).ToString() });
                    if (PerioYm == null)
                        throw new LogicException($"请定义{scn}年的财务月区间!");
                    zjfj.CREATEDATE = PerioYm.DATE_END;
                }
                else
                {
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
                    double zts = Math.Abs((per.DATE_END.ToDateTime() - per.DATE_START.ToDateTime()).Days) + 1;

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
                            zjfj.RENTS = Math.Round(je * zjfjTs, configBlxsw.CUR_VAL.ToInt(), MidpointRounding.AwayFromZero).ToString();
                            break;
                        case 2://月租金
                            if (zjfjTs != zts) //不足月时金额算法
                            {

                                if (configBzybj.CUR_VAL.ToInt() == 0)   //0(月金额/当月总天数)*实际天数;
                                {
                                    zjfj.RENTS = (Math.Round(je / zts * zjfjTs, configBlxsw.CUR_VAL.ToInt(),
                                        MidpointRounding.AwayFromZero)).ToString();
                                }
                                else if (configBzybj.CUR_VAL.ToInt() == 1)  //1:(月金额/1003参数设置的天数)*实际天数;
                                {
                                    zjfj.RENTS = (Math.Round(je / (configBzyts.CUR_VAL).ToDouble() * zjfjTs,
                                        configBlxsw.CUR_VAL.ToInt(), MidpointRounding.AwayFromZero)).ToString();
                                }
                                else       //2:(月金额*12/365)*实际天数
                                {
                                    zjfj.RENTS = (Math.Round((je * 12 / 365) * zjfjTs,
                                        configBlxsw.CUR_VAL.ToInt(), MidpointRounding.AwayFromZero)).ToString();

                                }

                            }
                            else
                            {
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
                    throw new LogicException($"租约({Data.CONTRACTID})已经不是未审核不能删除!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                foreach (var con in DeleteData)
                {
                    var dcl = new BILLSTATUSEntity
                    {
                        BILLID = con.CONTRACTID,
                        MENUID = "10600202",
                        BRABCHID = con.BRANCHID,
                        URL = "HTGL/ZLHT/HtEdit/"
                    };
                    DelDclRw(dcl);

                    DbHelper.Delete(con);
                }
                Tran.Commit();
            }
        }
        //租约审核
        public string ExecData(CONTRACTEntity Data)
        {
            if (Data.STATUS != ((int)普通单据状态.未审核).ToString())
                throw new LogicException($"租约({Data.CONTRACTID})已经已审核不能重复审核!");

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
            var dcl = new BILLSTATUSEntity
            {
                BILLID = Data.CONTRACTID,
                MENUID = "10600202",
                BRABCHID = Data.BRANCHID,
                URL = "HTGL/ZLHT/HtEdit/"
            };
            DelDclRw(dcl);

            return Data.CONTRACTID;
        }
        //租约启动
        public string StartUp(CONTRACTEntity Data)
        {
            using (var Tran = DbHelper.BeginTransaction())
            {
                if (Data.HTLX == ((int)合同类型.变更合同).ToString())
                {
                    EXEC_CONTRACT_UPDATE exec_contractbg = new EXEC_CONTRACT_UPDATE()
                    {
                        in_CONTRACTID = Data.CONTRACTID,
                        in_USERID = employee.Id
                    };
                    DbHelper.ExecuteProcedure(exec_contractbg);
                }
                else
                {
                    EXEC_CONTRACT_STARTUP exec_contract = new EXEC_CONTRACT_STARTUP()
                    {
                        V_CONTRACTID = Data.CONTRACTID,
                        V_USERID = employee.Id
                    };

                    DbHelper.ExecuteProcedure(exec_contract);
                }
                Tran.Commit();
            }

            return Data.CONTRACTID;
        }
        //租约终止
        public string Stop(CONTRACTEntity Data)
        {
            using (var Tran = DbHelper.BeginTransaction())
            {
                EXEC_CONTRACT_STOP exec_contract = new EXEC_CONTRACT_STOP()
                {
                    V_CONTRACTID = Data.CONTRACTID,
                    V_USERID = employee.Id
                };

                DbHelper.ExecuteProcedure(exec_contract);

                Tran.Commit();
            }
            return Data.CONTRACTID;
        }
        public DataGridResult GetFreeShopList(SearchItem item)
        {
            string sql = $@"select L.* ,B.NAME BRANCHNAME,M.NAME MERCHANTNAME,M.MERCHANTID " +
                " from FREESHOP L,BRANCH B,CONTRACT C,MERCHANT M " +
                "  where L.BRANCHID =B.ID and L.CONTRACTID=C.CONTRACTID " +
                "  and C.MERCHANTID=M.MERCHANTID";
            sql += " AND B.ID IN (" + GetPermissionSql(PermissionType.Branch) + ") ";    //分店权限 by：DZK
            item.HasKey("BRANCHID", a => sql += $" and B.ID = {a}");
            item.HasKey("BILLID", a => sql += $" and L.BILLID = {a}");
            item.HasKey("CONTRACTID", a => sql += $" and L.CONTRACTID={a}");
            item.HasKey("MERCHANTID", a => sql += $" and L.MERCHANTID={a}");
            item.HasDateKey("FREEDATE_START", a => sql += $" and L.FREEDATE>={a}");
            item.HasDateKey("FREEDATE_END", a => sql += $" and L.FREEDATE<={a}");
            item.HasKey("STATUS", a => sql += $" and L.STATUS={a}");
            item.HasKey("REPORTER", a => sql += $" and L.REPORTER={a}");
            item.HasDateKey("REPORTER_TIME_START", a => sql += $" and L.REPORTER_TIME>={a}");
            item.HasDateKey("REPORTER_TIME_END", a => sql += $" and L.REPORTER_TIME<={a}");
            item.HasKey("VERIFY", a => sql += $" and L.VERIFY={a}");
            item.HasDateKey("VERIFY_TIME_START", a => sql += $" and L.VERIFY_TIME>={a}");
            item.HasDateKey("VERIFY_TIME_END", a => sql += $" and L.VERIFY_TIME<={a}");
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
            var dcl = new BILLSTATUSEntity
            {
                BILLID = SaveData.BILLID,
                MENUID = "10600302",
                BRABCHID = SaveData.BRANCHID,
                URL = "HTGL/FREESHOP/FreeShopEdit/"
            };
            InsertDclRw(dcl);

            return SaveData.BILLID;
        }
        public void DeleteFreeShop(List<FREESHOPEntity> DeleteData)
        {
            foreach (var item in DeleteData)
            {
                FREESHOPEntity Data = DbHelper.Select(item);
                if (Data.STATUS != ((int)退铺单状态.未审核).ToString())
                {
                    throw new LogicException("不是未审核状态,不能删除!");
                }
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                foreach (var item in DeleteData)
                {
                    var dcl = new BILLSTATUSEntity
                    {
                        BILLID = item.BILLID,
                        MENUID = "10600302",
                        BRABCHID = item.BRANCHID,
                        URL = "HTGL/FREESHOP/FreeShopEdit/"
                    };
                    DelDclRw(dcl);

                    DbHelper.Delete(item);
                }
                Tran.Commit();
            }
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
            if (!dt.IsNotNull())
            {
                throw new LogicException("找不到此单据!");
            }
            dt.NewEnumColumns<退铺单状态>("STATUS", "STATUSMC");

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
        public string ExecFreeShop(FREESHOPEntity Data)
        {
            FREESHOPEntity freeShop = DbHelper.Select(Data);
            if (freeShop.STATUS != ((int)退铺单状态.未审核).ToString())
            {
                throw new LogicException("不是未审核状态,不能审核!");
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
            var dcl = new BILLSTATUSEntity
            {
                BILLID = freeShop.BILLID,
                MENUID = "10600302",
                BRABCHID = freeShop.BRANCHID,
                URL = "HTGL/FREESHOP/FreeShopEdit/"
            };
            DelDclRw(dcl);

            return freeShop.BILLID;
        }
        public string StopFreeShop(FREESHOPEntity Data)
        {
            FREESHOPEntity freeShop = DbHelper.Select(Data);
            if (Convert.ToDateTime(freeShop.FREEDATE) > Convert.ToDateTime(DateTime.Now.ToShortString()))
            {
                throw new LogicException("退铺日期大于当前日期不能合同终止!");
            }
            if (freeShop.STATUS != ((int)退铺单状态.审核).ToString())
            {
                throw new LogicException("不是审核状态,不能终止!");
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
        public string BackOoutFreeShop(FREESHOPEntity Data)
        {
            FREESHOPEntity freeShop = DbHelper.Select(Data);
            if (freeShop.STATUS != ((int)退铺单状态.审核).ToString())
            {
                throw new LogicException("不是审核状态,不能退铺!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                string sql1 = @" update SHOP set RENT_STATUS = 1
                                  where SHOPID in (select SHOPID from FREESHOPITEM where BILLID = {0})";
                string sql2 = @" update FREESHOP set STATUS = 3 WHERE BILLID = {0}";
                DbHelper.ExecuteNonQuery(string.Format(sql1,freeShop.BILLID));
                DbHelper.ExecuteNonQuery(string.Format(sql2, freeShop.BILLID));
                Tran.Commit();
            }
            return freeShop.BILLID;
        }
        public string checkHtBgData(CONTRACTEntity Data)
        {
            const string sqlContract = " select CONTRACTID from CONTRACT " +
                                       " where CONTRACT_OLD={0} and STATUS={1} and HTLX={2}";
            var sql = string.Format(sqlContract, Data.CONTRACTID, (int)合同状态.审核, (int)合同类型.变更合同);
            DataTable dtcount = DbHelper.ExecuteTable(sql);
            if (dtcount.Rows.Count > 0)
            {
                return dtcount.Rows[0][0].ToString();
            }
            return "";
        }
    }
}
