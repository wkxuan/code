using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
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
            string sql = $@"SELECT A.*,B.NAME,C.NAME MERNAME,CI.SHOPCODESTR SHOPDM,CI.BRANDNAMESTR BRANDNAME" +
                " FROM CONTRACT A,BRANCH B,MERCHANT C,CONTRACT_INFO CI " +
                " WHERE A.BRANCHID=B.ID AND A.MERCHANTID=C.MERCHANTID" +
                " AND A.CONTRACTID=CI.CONTRACTID(+) ";
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
            sql += " ORDER BY  A.REPORTER_TIME DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<合同状态>("STATUS", "STATUSMC");
            dt.NewEnumColumns<核算方式>("STYLE", "STYLEMC");
            return new DataGridResult(dt, count);
        }
        public string GetContractOutput(SearchItem item)
        {
            string sql = $@"SELECT A.*,B.NAME,C.NAME MERNAME,CI.SHOPCODESTR SHOPDM,CI.BRANDNAMESTR BRANDNAME,to_char(A.REPORTER_TIME,'yyyy-mm-dd') DJRQ,to_char(A.VERIFY_TIME,'yyyy-mm-dd') SHRQ"
                         + " FROM CONTRACT A,BRANCH B,MERCHANT C,CONTRACT_INFO CI "
                         + " WHERE A.BRANCHID=B.ID AND A.MERCHANTID=C.MERCHANTID AND A.CONTRACTID=CI.CONTRACTID(+) ";
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
            sql += " ORDER BY  A.REPORTER_TIME DESC";

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

            if (SaveData.STYLE.ToInt() == (int)核算方式.租赁合同)
            {
                OPERATIONRULEEntity oprule = DbHelper.Select(new OPERATIONRULEEntity() { ID = SaveData.OPERATERULE });

                if (oprule.PROCESSTYPE != "2" && SaveData.STANDARD == "2")
                {
                    throw new LogicException("合作方式不是纯租类型,月周期方式不能选择合同月");
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

            var dcl = new BILLSTATUSEntity
            {
                BILLID = SaveData.CONTRACTID,
                MENUID = "10600202",
                BRABCHID = SaveData.BRANCHID,
                URL = "HTGL/ZLHT/HtEdit/"
            };

            using (var Tran = DbHelper.BeginTransaction())
            {
                SaveData.CONTRACT_PAY?.ForEach(item =>
                {
                    GetVerify(item).Require(a => a.PAYID);
                    GetVerify(item).Require(a => a.TERMID);
                    GetVerify(item).Require(a => a.KL);
                    if (!item.KL.IsEmpty())
                    {
                        item.KL = (item.KL.ToDouble() / 100).ToString();
                    }
                });

                DbHelper.Save(SaveData);
                InsertDclRw(dcl);

                Tran.Commit();
            }

            //更新CONTRACT_INFO 表数据
            using (var Tran = DbHelper.BeginTransaction())
            {
                MAKE_CONTRACT_INFO proc = new MAKE_CONTRACT_INFO()
                {
                    in_CONTRACTID = SaveData.CONTRACTID
                };
                DbHelper.ExecuteProcedure(proc);
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

            string sqlPay = $@"SELECT A.CONTRACTID,A.PAYID,A.TERMID,A.STARTDATE,A.ENDDATE,A.KL*100 KL,A.ZNGZID,B.NAME,C.NAME TERMNAME 
                                 FROM CONTRACT_PAY A,PAY B,FEESUBJECT C WHERE A.PAYID=B.PAYID AND A.TERMID=C.TRIMID ";
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

        /// <summary>
        /// 根据合同开始结束日期生成合同月明细
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        public List<PERIODEntity> createHTY(DateTime begin, DateTime end)
        {
            List<PERIODEntity> Perio = new List<PERIODEntity>();

            DateTime datetmp = begin;
            int day = datetmp.Day;
            int leftDay = 0;

            if (day >= 29)
            {
                leftDay = DateTime.DaysInMonth(datetmp.Year, datetmp.Month) - day + 1;
            }


            while (datetmp < end)
            {
                PERIODEntity per = new PERIODEntity();
                per.YEARMONTH = datetmp.Year.ToString() + datetmp.Month.ToString().PadLeft(2, '0');
                per.DATE_START = datetmp.ToString();

                if (leftDay > 0)
                {  //下个月月末 向前推leftDay 天
                    datetmp = datetmp.AddMonths(1).AddDays(1 - datetmp.AddMonths(1).Day).AddMonths(1).AddDays(-1 - leftDay);
                }
                else
                    datetmp = datetmp.AddMonths(1).AddDays(-1);  //下个月

                per.DATE_END = datetmp.ToString();

                Perio.Add(per);

                datetmp = datetmp.AddDays(1);
            }

            return Perio;
        }


        public List<CONTRACT_RENTITEMEntity> zlYdFj(List<CONTRACT_RENTEntity> Data, CONTRACTEntity ContractData)
        {
            // ContractData.OPERATERULE
            // ContractData.STANDARD

            OPERATIONRULEEntity oprule = DbHelper.Select(new OPERATIONRULEEntity() { ID = ContractData.OPERATERULE });

            if (oprule.PROCESSTYPE != "2" && ContractData.STANDARD == "2")
            {
                throw new LogicException("合作方式不是纯租类型,月周期方式不能选择合同月");
            }

            List<CONTRACT_RENTITEMEntity> zjfjList = new List<CONTRACT_RENTITEMEntity>();
            //当月度分解没定义的时候抛出异常提示待完善

            FEERULEEntity feeRule = new FEERULEEntity();
            feeRule = DbHelper.Select(new FEERULEEntity() { ID = ContractData.FEERULE_RENT });

            CONFIGEntity configBzybj = new CONFIGEntity();
            configBzybj = DbHelper.Select(new CONFIGEntity() { ID = "1004" });
            if (configBzybj == null)
                throw new LogicException("未设置参数1004(不足月时月金额算法)");

            if (!"012".Contains(configBzybj.CUR_VAL))
                throw new LogicException("参数1004(不足月时月金额算法)设置有误");

            CONFIGEntity configBzyts = new CONFIGEntity();
            configBzyts = DbHelper.Select(new CONFIGEntity() { ID = "1003" });
            if (configBzyts == null)
                throw new LogicException("未设置参数1003(不足月天数设定)");

            if (configBzyts.CUR_VAL.ToInt() <= 0)
                throw new LogicException("参数1003(不足月天数设定)设置有误");

            CONFIGEntity configBlxsw = new CONFIGEntity();
            configBlxsw = DbHelper.Select(new CONFIGEntity() { ID = "1002" });
            if (configBlxsw == null)
                throw new LogicException("未设置参数1002(提成租金精度)");


            //季度分解日期生成
            CONFIGEntity configJDFJGZ = new CONFIGEntity();
            configJDFJGZ = DbHelper.Select(new CONFIGEntity() { ID = "1005" });

            if (configJDFJGZ == null)
                throw new LogicException("未设置参数1005(季度缴费生成方式)");


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

            if (ContractData.STANDARD == "2")  //合同月
            {
                Perio = createHTY(ContractData.CONT_START.ToDateTime(), ContractData.CONT_END.ToDateTime());
            }
            else
            {
                Perio = DbHelper.SelectList(new PERIODEntity()).
                Where(a => (a.DATE_START.ToDateTime() <= ContractData.CONT_END.ToDateTime())
                && (a.DATE_END.ToDateTime() >= ContractData.CONT_START.ToDateTime())).OrderBy(b => b.YEARMONTH).ToList();
            }

            List<PERIODEntity> PerioTMP = Perio.DeepClone();



            List<CONTRACT_RENTITEMEntity> zjfjListGd = new List<CONTRACT_RENTITEMEntity>();
            foreach (var per in Perio)
            {
                CONTRACT_RENTITEMEntity zjfj = new CONTRACT_RENTITEMEntity();

                var scny = 0;
                int iCYCLE = feeRule.ADVANCE_CYCLE.ToInt();

                if (feeRule.ADVANCE_CYCLE.ToInt() == -1)
                    iCYCLE = 0;

                //可以通过日期上加月份处理
                if ((ym.ToString().Substring(4, 2).ToInt() - iCYCLE) <= 0)
                {
                    scny = (ym.ToString().Substring(0, 4).ToInt() - 1) * 100 +
                        ((ym.ToString().Substring(4, 2).ToInt() + 12 - iCYCLE));
                }
                else
                {
                    scny = ym - iCYCLE;
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
                    if (ContractData.STANDARD == "2")  //合同月
                    {
                        foreach (var pertmp in PerioTMP)
                        {
                            if (scn * 100 + scy == int.Parse(pertmp.YEARMONTH))
                                zjfj.CREATEDATE = pertmp.DATE_END;
                            break;
                        }
                    }
                    else
                    {
                        PERIODEntity PerioYm = new PERIODEntity();
                        PerioYm = DbHelper.Select(new PERIODEntity() { YEARMONTH = (scn * 100 + scy).ToString() });
                        if (PerioYm == null)
                            throw new LogicException($"请定义{scn}年的财务月区间!");
                        zjfj.CREATEDATE = PerioYm.DATE_END;
                    }

                }
                else
                {
                    if (feeRule.ADVANCE_CYCLE.ToInt() == -1)  // 提前多少天出单
                    {
                        foreach (var pertmp in PerioTMP)
                        {
                            if (scn * 100 + scy == int.Parse(pertmp.YEARMONTH))
                            {
                                zjfj.CREATEDATE = pertmp.DATE_START.ToDateTime().AddDays(feeRule.FEE_DAY.ToInt() * -1).ToString();
                                break;
                            }

                        }

                    }
                    else
                    {
                        zjfj.CREATEDATE = (new DateTime(scn, scy, feeRule.FEE_DAY.ToInt())).ToString().ToDateTime().ToString();
                    }

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

                if (ContractData.STANDARD == "2")  //合同月
                {
                    Period = createHTY(ydfj.STARTDATE.ToDateTime(), ydfj.ENDDATE.ToDateTime());
                }
                else
                {
                    Period = DbHelper.SelectList(new PERIODEntity()).
                        Where(a => (a.DATE_START.ToDateTime() <= ydfj.ENDDATE.ToDateTime())
                        && (a.DATE_END.ToDateTime() >= ydfj.STARTDATE.ToDateTime())).OrderBy(b => b.YEARMONTH).ToList();
                }

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
                    //  var je = Convert.ToDouble(ydfj.RENTS);
                    var je = Convert.ToDouble(ydfj.PRICE) * Convert.ToDouble(ContractData.AREAR);
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
            {
                SaveData.BILLID = NewINC("FREESHOP");
                SaveData.REPORTER = employee.Id;
                SaveData.REPORTER_NAME = employee.Name;
                SaveData.REPORTER_TIME = DateTime.Now.ToString();
            }
            else {
                FREESHOPEntity data = DbHelper.Select(new FREESHOPEntity() { BILLID = SaveData.BILLID });

                if (data == null)
                {
                    throw new LogicException("该单据不存在!");
                }

                if (data.STATUS != ((int)退铺单状态.未审核).ToString())
                {
                    throw new LogicException("该单据不是未审核状态，不能修改!");
                }
            }
            SaveData.STATUS = ((int)退铺单状态.未审核).ToString();
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
                throw new LogicException("退铺日期未到,不能终止合同!");
            }
            if (freeShop.STATUS == ((int)退铺单状态.终止合同).ToString())
            {
                throw new LogicException("已经终止合同,不重复操作!");
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
                DbHelper.ExecuteNonQuery(string.Format(sql1, freeShop.BILLID));
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
        #region 获取导出合同信息
        public CONTRACTOUTPUTEntity GetContractOutPut(CONTRACTEntity Data)
        {
            CONTRACTOUTPUTEntity Coutput = new CONTRACTOUTPUTEntity();
            string sqlC = CONTRACTINFOSQL(Data.CONTRACTID);   //合同信息
            DataTable dt = DbHelper.ExecuteTable(sqlC);
            if (dt.Rows.Count > 0)
            {
                Coutput.CONTRACTID = Data.CONTRACTID;
                Coutput.BRANCHNAME = dt.Rows[0]["BRANCHNAME"].ToString();
                Coutput.BRANCHNAME1 = dt.Rows[0]["BRANCHNAME"].ToString();
                Coutput.BRANCHNAME2 = dt.Rows[0]["BRANCHNAME"].ToString();
                Coutput.BRANCHNAME3 = dt.Rows[0]["BRANCHNAME"].ToString();
                Coutput.MERCHANTNAME = dt.Rows[0]["MERCHANTNAME"].ToString();
                Coutput.BRANCHADDRESS = dt.Rows[0]["BRANCHADDRESS"].ToString();
                Coutput.AREAR = dt.Rows[0]["AREAR"].ToString();
                Coutput.CONT_START = dt.Rows[0]["CONT_START"].ToString().ToDateTime().ToString("yyyy年MM月dd日");
                Coutput.CONT_END = dt.Rows[0]["CONT_END"].ToString().ToDateTime().ToString("yyyy年MM月dd日");
                Coutput.CONT_START1 = dt.Rows[0]["CONT_START"].ToString().ToDateTime().ToString("yyyy年MM月dd日");
                Coutput.CONT_END1 = dt.Rows[0]["CONT_END"].ToString().ToDateTime().ToString("yyyy年MM月dd日");
                Coutput.CONT_DAYS = dt.Rows[0]["CONT_DAYS"].ToString();
                Coutput.FREE_BEGIN = dt.Rows[0]["FREE_BEGIN"].ToString()=="" ?"":dt.Rows[0]["FREE_BEGIN"].ToString().ToDateTime().ToString("yyyy年MM月dd日");
                Coutput.FREE_END = dt.Rows[0]["FREE_END"].ToString()=="" ? "":dt.Rows[0]["FREE_END"].ToString().ToDateTime().ToString("yyyy年MM月dd日");
                Coutput.FREEDAYS = dt.Rows[0]["FREEDAYS"].ToString()==""? "0": dt.Rows[0]["FREEDAYS"].ToString();
                Coutput.FEERULE_RENT = dt.Rows[0]["FEERULE_RENT"].ToString();
            }
            string sqlB = CONTRACT_BRANDINFOSQL(Data.CONTRACTID);   //品牌信息
            DataTable dt1 = DbHelper.ExecuteTable(sqlB);
            if (dt1.Rows.Count > 0)
            {
                Coutput.BRANDNAME = dt1.Rows[0]["BRANDNAME"].ToString();
                if (dt1.Rows.Count > 1)
                {
                    List<string> list = new List<string>();
                    for (var i = 1; i < dt1.Rows.Count; i++)
                    {
                        list.Add(dt1.Rows[i]["BRANDNAME"].ToString());
                    }
                    Coutput.BRANDNAME2 = string.Join(",", list);
                }
            }
            string sqlS = CONTRACT_SHOPINFOSQL(Data.CONTRACTID);   //商铺信息
            DataTable dt2 = DbHelper.ExecuteTable(sqlS);
            if (dt2.Rows.Count > 0)
            {
                Coutput.CATEGORYNAME = dt2.Rows[0]["CATEGORYNAME"].ToString();
                List<string> list = new List<string>();
                for (var i = 0; i < dt2.Rows.Count; i++)
                {
                    list.Add(dt2.Rows[i]["SHOPCODE"].ToString());
                }
                Coutput.SHOPCODE = string.Join(",", list);
            }
            string sqlRENT = CONTRACT_RENTINFOSQL(Data.CONTRACTID);   //租金信息
            DataTable dt3 = DbHelper.ExecuteTable(sqlRENT);
            if (dt3.Rows.Count > 0)
            {              
                decimal AMOUNT = 0;
                foreach (DataRow item in dt3.Rows)
                {
                    if (item["PRICE"].ToString().ToDecimal()>0) {
                        Coutput.RZJ_PRICE = item["PRICE"].ToString();
                    }
                    AMOUNT += item["SUMRENTS"].ToString().ToDecimal();
                }
                Coutput.SUMRENTS = AMOUNT.ToString();
            }
            string sqlCOST = CONTRACT_COSTINFOSQL(Data.CONTRACTID);   //收费项目信息
            DataTable dt4 = DbHelper.ExecuteTable(sqlCOST);
            if (dt4.Rows.Count > 0)
            {
                foreach (DataRow item in dt4.Rows)
                {
                    if (item["NAME"].ToString().Contains("保证金"))
                    {
                        Coutput.BZJAMOUNT = item["COST"].ToString();
                        Coutput.BZJAMOUNT_DX = ConvertToChinese(Coutput.BZJAMOUNT.ToDecimal());
                    }
                    if (item["NAME"].ToString().Contains("市场管理费"))
                    {
                        Coutput.WYF_PRICE = item["PRICE"].ToString()=="" ? "0": item["PRICE"].ToString();
                        Coutput.SUMWYF = item["COST"].ToString() == "" ? "0" : item["COST"].ToString(); 
                        Coutput.SUMWYF_RENTS = (Coutput.SUMWYF.ToDecimal() + Coutput.SUMRENTS.ToDecimal()).ToString();
                    }
                }
                if (string.IsNullOrEmpty(Coutput.WYF_PRICE)) {
                    Coutput.WYF_PRICE = "0";
                    Coutput.SUMWYF = "0";
                    Coutput.SUMWYF_RENTS = Coutput.SUMRENTS;
                }
            }
            return Coutput;
        }
        public string CONTRACTINFOSQL(string CONTRACTID)
        {
            return $@"SELECT  C.CONTRACTID,B.NAME BRANCHNAME,M.NAME MERCHANTNAME,B.ADDRESS BRANCHADDRESS,C.AREAR,C.CONT_START,C.CONT_END,((C.CONT_END+1)-C.CONT_START) CONT_DAYS,
                    C.FREE_BEGIN,C.FREE_END,((C.FREE_END+1)-C.FREE_BEGIN) FREEDAYS,F.NAME FEERULE_RENT 
                    FROM CONTRACT C,BRANCH B,MERCHANT M,FEERULE F 
                    WHERE C.BRANCHID=B.ID AND C.MERCHANTID=M.MERCHANTID AND C.FEERULE_RENT=F.ID  AND C.CONTRACTID={CONTRACTID}";
        }
        public string CONTRACT_BRANDINFOSQL(string CONTRACTID)
        {
            return $@"SELECT B.NAME BRANDNAME
                    FROM CONTRACT_BRAND CB,BRAND B
                    WHERE CB.BRANDID=B.ID AND CB.CONTRACTID={CONTRACTID}";
        }
        public string CONTRACT_SHOPINFOSQL(string CONTRACTID)
        {
            return $@"SELECT S.CODE SHOPCODE ,C.CATEGORYNAME
                    FROM CONTRACT_SHOP CS,SHOP S,CATEGORY C
                    WHERE CS.SHOPID=S.SHOPID AND CS.CATEGORYID=C.CATEGORYID AND CS.CONTRACTID={CONTRACTID}";
        }
        public string CONTRACT_RENTINFOSQL(string CONTRACTID)
        {
            return $@"SELECT CR.PRICE, CR.SUMRENTS
                    FROM CONTRACT_RENT CR WHERE CR.CONTRACTID={CONTRACTID}";
        }
        public string CONTRACT_COSTINFOSQL(string CONTRACTID)
        {
            return $@"SELECT CS.PRICE,CS.COST ,F.NAME
                    FROM CONTRACT_COST CS,FEESUBJECT F
                    WHERE  CS.TERMID=F.TRIMID AND CS.CONTRACTID={CONTRACTID}";
        }
        /// <summary>
        /// 数字转换成大写
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ConvertToChinese(decimal number)
        {
            var s = number.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            var d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            var r = Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());
            return r;
        }
        #endregion
    }
}
