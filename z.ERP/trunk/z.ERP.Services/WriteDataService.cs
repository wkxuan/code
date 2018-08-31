using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.ERP.Entities;
using z.ERP.Entities.Enum;
using z.ERP.Entities.Procedures;
using z.Exceptions;
using z.Extensions;

namespace z.ERP.Services
{
    public class WriteDataService: ServiceBase
    {
        internal WriteDataService()
        {

        }

        public void CanRcl(WRITEDATAEntity WRITEDATA)
        {
            //1:判断表里面有记录就提示不让在做日处理          
            var sql = " SELECT 1 FROM RCL_HOST";
            DataTable dt = DbHelper.ExecuteTable(sql);
            if (dt.Rows.Count != 0) {
                throw new LogicException($"日处理正在进行中!");
            }

            List<BRANCHEntity> fdListrcl = DbHelper.SelectList(new BRANCHEntity() { STATUS = "1" });
            var boll = false;
            foreach (var fd in fdListrcl) {
                WRITEDATAEntity data = DbHelper.Select(new WRITEDATAEntity() { RQ = WRITEDATA.RQ, BRANCHID = fd.ID });
                if (data!=null && (data.STATUS != 日处理步骤.成功.ToString()))
                {
                    boll = true;
                    break;
                }
            }
            if (boll) {
                throw new LogicException($"当前日期的日处理已经完成!");
            }

            RCL_HOSTEntity host = new RCL_HOSTEntity();
            //2:插入互斥日结表
            host.HOSTNAME= Environment.MachineName;
            using (var Tran = DbHelper.BeginTransaction())
            {
                DbHelper.Save(host);
                Tran.Commit();
            }

            //循环分店信息:
            List<BRANCHEntity> fdList = DbHelper.SelectList(new BRANCHEntity() { STATUS = "1" });

            foreach (var fd in fdList) {

                WRITEDATAEntity data = new WRITEDATAEntity();
                WRITEDATAEntity data1 = DbHelper.Select(new WRITEDATAEntity() { RQ = WRITEDATA.RQ, BRANCHID = fd.ID });
                if (data1 != null) {
                    data.STATUS = data1.STATUS;
                }

                if (data.STATUS.IsEmpty())
                {
                    data.STATUS = "1";
                }

                if (data.STATUS == "0")
                {
                    break;
                }
                else {
                    while (data.STATUS.ToInt() <= 9) {
                        switch (data.STATUS.ToInt())
                        {
                            case 1:
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    UPDATE_SALE_GOODS_RATE update_sale_goods_rate = new UPDATE_SALE_GOODS_RATE()
                                    {
                                        in_RQ = WRITEDATA.RQ,
                                        in_BRANCHID = fd.ID
                                    };
                                    DbHelper.ExecuteProcedure(update_sale_goods_rate);
                                    WRITEDATAEntity writedata = new WRITEDATAEntity();
                                    writedata.RQ = WRITEDATA.RQ;
                                    writedata.BRANCHID = fd.ID;
                                    writedata.STATUS = "1";
                                    UpdateSatus(writedata);
                                    Tran.Commit();
                                }
                                break;
                            case 2:
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    WRITE_GOODS_SUMMARY write_goods_summary = new WRITE_GOODS_SUMMARY()
                                    {
                                        in_RQ = WRITEDATA.RQ,
                                        in_BRANCHID = fd.ID
                                    };
                                    DbHelper.ExecuteProcedure(write_goods_summary);
                                    WRITEDATAEntity writedata = new WRITEDATAEntity();
                                    writedata.RQ = WRITEDATA.RQ;
                                    writedata.BRANCHID = fd.ID;
                                    writedata.STATUS = "2";
                                    UpdateSatus(writedata);
                                    Tran.Commit();
                                }
                                break;
                            case 3:
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    WRITE_CONTRACT_SUMMARY write_contract_summary = new WRITE_CONTRACT_SUMMARY()
                                    {
                                        in_RQ = WRITEDATA.RQ,
                                        in_BRANCHID = fd.ID
                                    };
                                    DbHelper.ExecuteProcedure(write_contract_summary);
                                    WRITEDATAEntity writedata = new WRITEDATAEntity();
                                    writedata.RQ = WRITEDATA.RQ;
                                    writedata.BRANCHID = fd.ID;
                                    writedata.STATUS = "3";
                                    UpdateSatus(writedata);
                                    Tran.Commit();
                                }
                                break;
                            case 4:
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    WRITE_CONTRACT_PAY_RATE write_contract_pay_rate = new WRITE_CONTRACT_PAY_RATE()
                                    {
                                        in_RQ = WRITEDATA.RQ,
                                        in_BRANCHID = fd.ID
                                    };
                                    DbHelper.ExecuteProcedure(write_contract_pay_rate);
                                    WRITEDATAEntity writedata = new WRITEDATAEntity();
                                    writedata.RQ = WRITEDATA.RQ;
                                    writedata.BRANCHID = fd.ID;
                                    writedata.STATUS = "4";
                                    UpdateSatus(writedata);
                                    Tran.Commit();
                                }
                                break;
                            case 5:
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    WRITE_HIS_SALE write_his_sale = new WRITE_HIS_SALE()
                                    {
                                        in_RQ = WRITEDATA.RQ,
                                        in_BRANCHID = fd.ID
                                    };
                                    DbHelper.ExecuteProcedure(write_his_sale);
                                    WRITEDATAEntity writedata = new WRITEDATAEntity();
                                    writedata.RQ = WRITEDATA.RQ;
                                    writedata.BRANCHID = fd.ID;
                                    writedata.STATUS = "5";
                                    UpdateSatus(writedata);
                                    Tran.Commit();
                                }
                                break;
                            case 6:
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    WRITE_BILL write_bill = new WRITE_BILL()
                                    {
                                        in_RQ = WRITEDATA.RQ,
                                        in_BRANCHID = fd.ID,
                                        in_USERID = employee.Id
                                    };
                                    DbHelper.ExecuteProcedure(write_bill);
                                    WRITEDATAEntity writedata = new WRITEDATAEntity();
                                    writedata.RQ = WRITEDATA.RQ;
                                    writedata.BRANCHID = fd.ID;
                                    writedata.STATUS = "6";
                                    UpdateSatus(writedata);
                                    Tran.Commit();
                                }
                                break;
                            case 7:
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    WRITE_BILLFROMSALE write_billfromsale = new WRITE_BILLFROMSALE()
                                    {
                                        in_RQ = WRITEDATA.RQ,
                                        in_BRANCHID = fd.ID,
                                        in_USERID = employee.Id
                                    };
                                    DbHelper.ExecuteProcedure(write_billfromsale);
                                    WRITEDATAEntity writedata = new WRITEDATAEntity();
                                    writedata.RQ = WRITEDATA.RQ;
                                    writedata.BRANCHID = fd.ID;
                                    writedata.STATUS = "7";
                                    UpdateSatus(writedata);
                                    Tran.Commit();
                                }
                                break;
                            case 8:
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    WRITE_CONTRACT_UPDATE write_contract_update = new WRITE_CONTRACT_UPDATE()
                                    {
                                        in_RQ = WRITEDATA.RQ,
                                        in_BRANCHID = fd.ID,
                                        in_USERID = employee.Id
                                    };
                                    DbHelper.ExecuteProcedure(write_contract_update);
                                    WRITEDATAEntity writedata = new WRITEDATAEntity();
                                    writedata.RQ = WRITEDATA.RQ;
                                    writedata.BRANCHID = fd.ID;
                                    writedata.STATUS = "8";
                                    UpdateSatus(writedata);
                                    Tran.Commit();
                                }
                                break;
                            case 9:
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    WRITE_BILL_NOTICE write_bill_notice = new WRITE_BILL_NOTICE()
                                    {
                                        in_RQ = WRITEDATA.RQ,
                                        in_BRANCHID = fd.ID,
                                        in_USERID = employee.Id
                                    };
                                    DbHelper.ExecuteProcedure(write_bill_notice);
                                    WRITEDATAEntity writedata = new WRITEDATAEntity();
                                    writedata.RQ = WRITEDATA.RQ;
                                    writedata.BRANCHID = fd.ID;
                                    writedata.STATUS = "9";
                                    UpdateSatus(writedata);
                                    Tran.Commit();
                                }
                                break;
                            default:
                                break;
                        }
                        using (var Tran = DbHelper.BeginTransaction())
                        {
                            WRITEDATAEntity writedata = new WRITEDATAEntity();
                            writedata.RQ = WRITEDATA.RQ;
                            writedata.BRANCHID = fd.ID;
                            writedata.STATUS = "0";
                            UpdateSatus(writedata);
                            Tran.Commit();
                        }
                    }
                }
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                DbHelper.Delete(host);
                Tran.Commit();
            }
        }

        public void UpdateSatus(WRITEDATAEntity data) {
            DbHelper.Update(data);
        }
    }
}
