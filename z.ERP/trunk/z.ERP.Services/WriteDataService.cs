using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using z.ERP.Entities;
using z.ERP.Entities.Enum;
using z.ERP.Entities.Procedures;
using z.Exceptions;
using z.Extensions;

namespace z.ERP.Services
{
    public class WriteDataService : ServiceBase
    {
        internal WriteDataService()
        {

        }

        public void CanRcl(WRITEDATAEntity WRITEDATA, RichTextBox LogData)
        {
            if (employee.Id.ToInt() < 0)
            {
                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "请用操作员登陆做日处理!");
                return;
            }
            //1:判断表里面有记录就提示不让在做日处理          
            var sql = " SELECT 1 FROM RCL_HOST";
            DataTable dt = DbHelper.ExecuteTable(sql);
            if (dt.Rows.Count != 0)
            {
                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "日处理正在进行中!");
                return;
            }



            List<BRANCHEntity> fdListrcl = DbHelper.SelectList(new BRANCHEntity() { STATUS = "1" });
            var boll = false;
            foreach (var fd in fdListrcl)
            {
                WRITEDATAEntity data = DbHelper.Select(new WRITEDATAEntity() { RQ = WRITEDATA.RQ, BRANCHID = fd.ID });
                if (data == null)
                {
                    boll = true;
                    break;
                }
                else
                {
                    if (data.STATUS != "0")
                    {
                        boll = true;
                        break;
                    }
                }
            }
            if (!boll)
            {
                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "当前日期的日处理已经完成!");
                return;
            }

            RCL_HOSTEntity host = new RCL_HOSTEntity();
            //3:插入互斥日结表
            host.HOSTNAME = Environment.MachineName;
            DbHelper.Save(host);
            bool errorProc = false;
            //循环分店信息:
            List<BRANCHEntity> fdList = DbHelper.SelectList(new BRANCHEntity() { STATUS = "1" });

            foreach (var fd in fdList)
            {

                WRITEDATAEntity data = new WRITEDATAEntity();
                WRITEDATAEntity data1 = DbHelper.Select(new WRITEDATAEntity() { RQ = WRITEDATA.RQ, BRANCHID = fd.ID });
                if (data1 != null)
                {
                    data.STATUS = data1.STATUS;
                }

                if (data.STATUS.IsEmpty())
                {
                    data.STATUS = "1";
                    WRITEDATAEntity writedata = new WRITEDATAEntity();
                    writedata.RQ = WRITEDATA.RQ;
                    writedata.BRANCHID = fd.ID;
                    writedata.STATUS = "1";
                    DbHelper.Insert(writedata);
                }

                if (data.STATUS == "0")
                {
                    break;
                }
                else
                {

                    while (data.STATUS.ToInt() <= 9)
                    {
                        #region
                        switch (data.STATUS.ToInt())
                        {
                            case 1:
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "更新交易商品税率");
                                try
                                {
                                    using (var Tran = DbHelper.BeginTransaction())
                                    {
                                        UPDATE_SALE_GOODS_RATE update_sale_goods_rate = new UPDATE_SALE_GOODS_RATE()
                                        {
                                            in_RQ = WRITEDATA.RQ.ToDateTime(),
                                            in_BRANCHID = fd.ID
                                        };
                                        DbHelper.ExecuteProcedure(update_sale_goods_rate);
                                        WRITEDATAEntity writedata = new WRITEDATAEntity();
                                        writedata.RQ = WRITEDATA.RQ;
                                        writedata.BRANCHID = fd.ID;
                                        writedata.STATUS = "2";
                                        UpdateSatus(writedata);
                                        Tran.Commit();
                                        data.STATUS = writedata.STATUS;
                                    }

                                }
                                catch (Exception e)
                                {
                                    DbHelper.Delete(host);
                                    errorProc = true;
                                    LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                                }
                                break;
                            case 2:
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "汇总商品销售表");
                                try
                                {
                                    using (var Tran = DbHelper.BeginTransaction())
                                    {
                                        WRITE_GOODS_SUMMARY write_goods_summary = new WRITE_GOODS_SUMMARY()
                                        {
                                            in_RQ = WRITEDATA.RQ.ToDateTime(),
                                            in_BRANCHID = fd.ID
                                        };
                                        DbHelper.ExecuteProcedure(write_goods_summary);
                                        WRITEDATAEntity writedata = new WRITEDATAEntity();
                                        writedata.RQ = WRITEDATA.RQ;
                                        writedata.BRANCHID = fd.ID;
                                        writedata.STATUS = "3";
                                        UpdateSatus(writedata);
                                        Tran.Commit();
                                        data.STATUS = writedata.STATUS;
                                    }

                                }
                                catch (Exception e)
                                {
                                    DbHelper.Delete(host);
                                    errorProc = true;
                                    LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                                }
                                break;
                            case 3:
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "汇总统计维度表");
                                try
                                {
                                    using (var Tran = DbHelper.BeginTransaction())
                                    {
                                        WRITE_CONTRACT_SUMMARY write_contract_summary = new WRITE_CONTRACT_SUMMARY()
                                        {
                                            in_RQ = WRITEDATA.RQ.ToDateTime(),
                                            in_BRANCHID = fd.ID
                                        };
                                        DbHelper.ExecuteProcedure(write_contract_summary);
                                        WRITEDATAEntity writedata = new WRITEDATAEntity();
                                        writedata.RQ = WRITEDATA.RQ;
                                        writedata.BRANCHID = fd.ID;
                                        writedata.STATUS = "4";
                                        UpdateSatus(writedata);
                                        Tran.Commit();
                                        data.STATUS = writedata.STATUS;
                                    }

                                }
                                catch (Exception e)
                                {
                                    DbHelper.Delete(host);
                                    errorProc = true;
                                    LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                                }

                                break;
                            case 4:
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "汇总手续费表");
                                try
                                {
                                    using (var Tran = DbHelper.BeginTransaction())
                                    {
                                        WRITE_CONTRACT_PAY_RATE write_contract_pay_rate = new WRITE_CONTRACT_PAY_RATE()
                                        {
                                            in_RQ = WRITEDATA.RQ.ToDateTime(),
                                            in_BRANCHID = fd.ID
                                        };
                                        DbHelper.ExecuteProcedure(write_contract_pay_rate);
                                        WRITEDATAEntity writedata = new WRITEDATAEntity();
                                        writedata.RQ = WRITEDATA.RQ;
                                        writedata.BRANCHID = fd.ID;
                                        writedata.STATUS = "5";
                                        UpdateSatus(writedata);
                                        Tran.Commit();
                                        data.STATUS = writedata.STATUS;
                                    }

                                }
                                catch (Exception e)
                                {
                                    DbHelper.Delete(host);
                                    errorProc = true;
                                    LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                                }

                                break;
                            case 5:
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "转移交易数据");
                                try
                                {
                                    using (var Tran = DbHelper.BeginTransaction())
                                    {
                                        WRITE_HIS_SALE write_his_sale = new WRITE_HIS_SALE()
                                        {
                                            in_RQ = WRITEDATA.RQ.ToDateTime(),
                                            in_BRANCHID = fd.ID
                                        };
                                        DbHelper.ExecuteProcedure(write_his_sale);
                                        WRITEDATAEntity writedata = new WRITEDATAEntity();
                                        writedata.RQ = WRITEDATA.RQ;
                                        writedata.BRANCHID = fd.ID;
                                        writedata.STATUS = "6";
                                        UpdateSatus(writedata);
                                        Tran.Commit();
                                        data.STATUS = writedata.STATUS;
                                    }
                                }
                                catch (Exception e)
                                {
                                    DbHelper.Delete(host);
                                    errorProc = true;
                                    LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                                }

                                break;
                            case 6:
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "生成租金每月收费项目账单");
                                try
                                {
                                    using (var Tran = DbHelper.BeginTransaction())
                                    {
                                        WRITE_BILL write_bill = new WRITE_BILL()
                                        {
                                            in_RQ = WRITEDATA.RQ.ToDateTime(),
                                            in_BRANCHID = fd.ID,
                                            in_USERID = employee.Id
                                        };
                                        DbHelper.ExecuteProcedure(write_bill);
                                        WRITEDATAEntity writedata = new WRITEDATAEntity();
                                        writedata.RQ = WRITEDATA.RQ;
                                        writedata.BRANCHID = fd.ID;
                                        writedata.STATUS = "7";
                                        UpdateSatus(writedata);
                                        Tran.Commit();
                                        data.STATUS = writedata.STATUS;
                                    }
                                }
                                catch (Exception e)
                                {
                                    DbHelper.Delete(host);
                                    errorProc = true;
                                    LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                                }

                                break;
                            case 7:
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "生成销售相关账单");
                                try
                                {
                                    using (var Tran = DbHelper.BeginTransaction())
                                    {
                                        WRITE_BILLFROMSALE write_billfromsale = new WRITE_BILLFROMSALE()
                                        {
                                            in_RQ = WRITEDATA.RQ.ToDateTime(),
                                            in_BRANCHID = fd.ID,
                                            in_USERID = employee.Id
                                        };
                                        DbHelper.ExecuteProcedure(write_billfromsale);
                                        WRITEDATAEntity writedata = new WRITEDATAEntity();
                                        writedata.RQ = WRITEDATA.RQ;
                                        writedata.BRANCHID = fd.ID;
                                        writedata.STATUS = "8";
                                        UpdateSatus(writedata);
                                        Tran.Commit();
                                        data.STATUS = writedata.STATUS;
                                    }
                                }
                                catch (Exception e)
                                {
                                    DbHelper.Delete(host);
                                    errorProc = true;
                                    LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                                }

                                break;
                            case 8:
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "租约变更启动 ");
                                try
                                {
                                    using (var Tran = DbHelper.BeginTransaction())
                                    {
                                        WRITE_CONTRACT_UPDATE write_contract_update = new WRITE_CONTRACT_UPDATE()
                                        {
                                            in_RQ = WRITEDATA.RQ.ToDateTime(),
                                            in_BRANCHID = fd.ID,
                                            in_USERID = employee.Id
                                        };
                                        DbHelper.ExecuteProcedure(write_contract_update);
                                        WRITEDATAEntity writedata = new WRITEDATAEntity();
                                        writedata.RQ = WRITEDATA.RQ;
                                        writedata.BRANCHID = fd.ID;
                                        writedata.STATUS = "9";
                                        UpdateSatus(writedata);
                                        Tran.Commit();
                                        data.STATUS = writedata.STATUS;
                                    }
                                }
                                catch (Exception e)
                                {
                                    DbHelper.Delete(host);
                                    errorProc = true;
                                    LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                                }

                                break;
                            case 9:
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "生成缴费通知单");
                                try
                                {
                                    using (var Tran = DbHelper.BeginTransaction())
                                    {
                                        WRITE_BILL_NOTICE write_bill_notice = new WRITE_BILL_NOTICE()
                                        {
                                            in_RQ = WRITEDATA.RQ.ToDateTime(),
                                            in_BRANCHID = fd.ID,
                                            in_USERID = employee.Id
                                        };
                                        DbHelper.ExecuteProcedure(write_bill_notice);
                                        WRITEDATAEntity writedata = new WRITEDATAEntity();
                                        writedata.RQ = WRITEDATA.RQ;
                                        writedata.BRANCHID = fd.ID;
                                        writedata.STATUS = "10";
                                        UpdateSatus(writedata);
                                        Tran.Commit();
                                        data.STATUS = writedata.STATUS;
                                    }
                                }
                                catch (Exception e)
                                {
                                    DbHelper.Delete(host);
                                    errorProc = true;
                                    LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                                }

                                break;
                            default:
                                break;
                        }
                        #endregion
                        if (errorProc)
                        {
                            break;
                        }
                    }
                }
                if (!errorProc)
                {
                    using (var Tran = DbHelper.BeginTransaction())
                    {
                        WRITEDATAEntity writedata = new WRITEDATAEntity();
                        writedata.RQ = WRITEDATA.RQ;
                        writedata.BRANCHID = fd.ID;
                        writedata.STATUS = "0";
                        UpdateSatus(writedata);
                        Tran.Commit();
                    }
                    LogData.AppendText(DateTime.Now.ToString("\r\n" + "yyyy-MM-dd HH:mm:ss") + ":分店" + fd.ID + "成功结束!");
                }

            }
            DbHelper.Delete(host);
        }


        public void CanHyRcl(RCLEntity RCLDATA, RichTextBox LogData)
        {
            if (employee.Id.ToInt() < 0)
            {
                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "请用操作员登陆做日处理!");
                return;
            }
            //1:判断表里面有记录就提示不让在做日处理          
            var sql = " SELECT 1 FROM RCL_HOST";
            DataTable dt = DbHelper.ExecuteTable(sql);
            if (dt.Rows.Count != 0)
            {
                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "日处理正在进行中!");
                return;
            }


            var boll = false;
            RCLEntity rcl = DbHelper.Select(new RCLEntity() { RQ = RCLDATA.RQ });
            if (rcl == null)
            {
                boll = true;
            }
            else
            {
                if (rcl.STATUS != "0")
                {
                    boll = true;
                }
            }
            if (!boll)
            {
                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "当前日期的日处理已经完成!");
                return;
            }

            RCL_HOSTEntity host = new RCL_HOSTEntity();
            //3:插入互斥日结表
            host.HOSTNAME = Environment.MachineName;
            DbHelper.Save(host);

            RCLEntity data = new RCLEntity();
            RCLEntity data1 = DbHelper.Select(new RCLEntity() { RQ = RCLDATA.RQ });
            if (data1 != null)
            {
                data.STATUS = data1.STATUS;
            }

            if (data.STATUS.IsEmpty())
            {
                data.STATUS = "1";
                RCLEntity rcldata = new RCLEntity();
                rcldata.RQ = RCLDATA.RQ;
                rcldata.STATUS = "1";
                rcldata.PROC_KSSJ = DateTime.Now.ToString();
                rcldata.OPERATOR_ID = employee.Id;
                DbHelper.Insert(rcldata);
            }
            bool errorProc = false;
            if (data.STATUS == "0")
            {
                return;
            }
            else
            {

                while (data.STATUS.ToInt() <= 32)
                {
                    #region

                    switch (data.STATUS.ToInt())
                    {
                        case 1:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "启动促销单据");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HYXF_QD_HYCXDJ proc1 = new HYXF_QD_HYCXDJ()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),
                                        pZXR = employee.Id
                                    };
                                    DbHelper.ExecuteProcedure(proc1);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "2";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }

                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }
                            break;
                        case 2:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "启动优惠券促销活动单据");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HYK_YHQ_QD_CXDJ proc2 = new HYK_YHQ_QD_CXDJ()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),
                                        pZXR = employee.Id
                                    };
                                    DbHelper.ExecuteProcedure(proc2);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "3";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }

                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }
                            break;
                        case 3:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "更新记账日期");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HYK_PROC_UPDATECRMJZRQ proc = new HYK_PROC_UPDATECRMJZRQ()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),
                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "4";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }

                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }

                            break;
                        case 4:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "金额账日报表");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HYK_PROC_HYK_CZK_RBB proc = new HYK_PROC_HYK_CZK_RBB()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "5";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }

                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }

                            break;
                        case 5:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "库存卡保管帐");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HYK_PROC_HYK_KCCZKBGZ proc = new HYK_PROC_HYK_KCCZKBGZ()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),
                                        pNY = (RCLDATA.RQ.ToDateTime().Year * 100 + RCLDATA.RQ.ToDateTime().Month).ToString()
                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "6";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }

                            break;
                        case 6:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "金额帐消费日报表");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HYK_PROC_HYK_XFRBB proc = new HYK_PROC_HYK_XFRBB()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "7";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }

                            break;
                        case 7:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "优惠券帐消费日报表");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HYK_PROC_HYK_YHQ_XFRBB proc = new HYK_PROC_HYK_YHQ_XFRBB()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "8";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }

                            break;
                        case 8:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "会员卡转为睡眠状态 ");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HYKGL_ZX_HYK_SMZTGZ proc = new HYKGL_ZX_HYK_SMZTGZ()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "9";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }

                            break;
                        case 9:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "会员卡警告等级变动");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HYKGL_ZX_HYK_JGDJGZ proc = new HYKGL_ZX_HYK_JGDJGZ()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "10";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }

                            break;
                        case 10:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "会员卡有效期延长规则");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HYXF_ZX_HYK_YXQYCGZ proc = new HYXF_ZX_HYK_YXQYCGZ()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "11";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }

                            break;
                        case 11:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "会员卡状态变动处理");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HYXF_ZX_HYK_ZTBG proc = new HYXF_ZX_HYK_ZTBG()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "12";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }

                            break;
                        case 12:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "有效期到期冻结");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HYKGL_ZX_HYK_YXQDQDJ proc = new HYKGL_ZX_HYK_YXQDQDJ()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "13";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }

                            break;
                        case 13:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "积分处理");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HYK_PROC_HYK_JFFLGZ proc = new HYK_PROC_HYK_JFFLGZ()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "14";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }

                            break;
                        case 14:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "优惠券帐日报表");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HYK_PROC_HYK_YHQ_RBB proc = new HYK_PROC_HYK_YHQ_RBB()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "15";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }

                            break;
                        case 15:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "积分返还礼品进销存报表");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HYK_PROC_HYK_JFFHLPJXC proc = new HYK_PROC_HYK_JFFHLPJXC()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "16";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }

                            break;
                        case 16:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "汇总储值卡消费明细");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HYK_PROC_CZK_XFHZ proc = new HYK_PROC_CZK_XFHZ()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "17";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }

                            break;
                        case 17:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "移动后台不刷卡历史记录到消费记录");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HYK_MOVE_HYXF_BSK proc = new HYK_MOVE_HYXF_BSK()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "18";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }

                            break;
                        case 18:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "汇总会员卡消费明细");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    WRITE_HYK_XFMX proc = new WRITE_HYK_XFMX()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "19";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }

                            break;
                        case 19:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "会员卡积分日报表");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HYK_PROC_HYK_JFRBB proc = new HYK_PROC_HYK_JFRBB()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "20";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }

                            break;
                        case 20:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "会员卡等级升迁");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HYXF_ZX_HYK_DJSQGZ proc = new HYXF_ZX_HYK_DJSQGZ()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "21";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }

                            break;
                        case 21:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "汇总柜组消费积分");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    WRITE_HYK_GZXFJFR proc = new WRITE_HYK_GZXFJFR()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "22";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }

                            break;
                        case 22:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "储值卡日报表");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HYK_PROC_MZK_CZK_RBB proc = new HYK_PROC_MZK_CZK_RBB()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "23";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }

                            break;
                        case 23:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "库存储值卡保管帐");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HYK_PROC_MZK_KCCZKBGZ proc = new HYK_PROC_MZK_KCCZKBGZ()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "24";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }

                            break;
                        case 24:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "储值卡消费日报表");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HYK_PROC_MZK_XFRBB proc = new HYK_PROC_MZK_XFRBB()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "25";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }

                            break;
                        case 25:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "按商品汇总促销优惠券金额");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HYK_PROC_SP_CXHZ proc = new HYK_PROC_SP_CXHZ()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "26";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }

                            break;


                        case 26:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "按部门汇总促销优惠券金额");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HZ_YHQ_CXHDBYBM proc = new HZ_YHQ_CXHDBYBM()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "27";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }
                            break;
                        case 27:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "按合同汇总促销优惠券金额");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HZ_YHQ_CXHDBYHT proc = new HZ_YHQ_CXHDBYHT()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "28";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }
                            break;
                        case 28:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "汇总满百减折数据");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HZ_MBJZ_CXHDSJ proc = new HZ_MBJZ_CXHDSJ()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "29";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }
                            break;
                        case 29:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "将消费数据移动到会员消费纪录中");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HYK_PROC_MOVE_XFJL proc = new HYK_PROC_MOVE_XFJL()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "30";
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }
                            break;
                        case 30:
                            LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "将消费数据删除");
                            try
                            {
                                using (var Tran = DbHelper.BeginTransaction())
                                {
                                    HYK_PROC_DEL_XFJL proc = new HYK_PROC_DEL_XFJL()
                                    {
                                        pRCLRQ = RCLDATA.RQ.ToDateTime(),

                                    };
                                    DbHelper.ExecuteProcedure(proc);
                                    RCLEntity rcldata = new RCLEntity();
                                    rcldata.RQ = RCLDATA.RQ;
                                    rcldata.STATUS = "0";
                                    rcldata.PROC_JSSJ = DateTime.Now.ToString();
                                    UpdateSatusvip(rcldata);
                                    Tran.Commit();
                                    data.STATUS = rcldata.STATUS;
                                }
                            }
                            catch (Exception e)
                            {
                                DbHelper.Delete(host);
                                errorProc = true;
                                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + e.Message);
                            }
                            break;
                        default:
                            break;

                    }
                    #endregion
                    if (errorProc)
                    {
                        break;
                    }
                }
            }
            DbHelper.Delete(host);
            if (!errorProc)
            {
                LogData.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "vip日结成功结束!");
            }

        }

        public void UpdateSatus(WRITEDATAEntity data)
        {
            DbHelper.Update(data);
        }

        public void UpdateSatusvip(RCLEntity data)
        {
            DbHelper.Update(data);
        }
    }
}
