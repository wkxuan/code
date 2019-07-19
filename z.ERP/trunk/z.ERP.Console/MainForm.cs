using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using z.ERP.Entities;

namespace z.ERP.Console
{
    public partial class MainForm : FormBase
    {
        public MainForm()
        {
            InitializeComponent();

            this.WriteRq.Value = DateTime.Now.AddDays(-1);
            this.timer.Enabled = true;

            ButtonClick(btn_rcl, () =>
            {
                //LogText.Clear();
                LogText.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "开始");
                var msg = string.Empty;
                var e = employee;
                WRITEDATAEntity WRITEDATA = new WRITEDATAEntity();
                WRITEDATA.RQ = this.WriteRq.Value.ToShortDateString();

                RCLEntity rcldata = new RCLEntity();
                rcldata.RQ = this.WriteRq.Value.ToShortDateString();

                // service.WriteDataService.CanHyRcl(rcldata, LogText);
                service.WriteDataService.CanRcl(WRITEDATA, LogText);
            });



            //TimerTick(timer, () =>
            //{
            //    LogText.Clear();
            //    LogText.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "开始");
            //    var msg = string.Empty;
            //    var e = employee;
            //    WRITEDATAEntity WRITEDATA = new WRITEDATAEntity();
            //    WRITEDATA.RQ = this.WriteRq.Value.ToShortDateString();

            //    RCLEntity rcldata = new RCLEntity();
            //    rcldata.RQ = this.WriteRq.Value.ToShortDateString();

            //    // service.WriteDataService.CanHyRcl(rcldata, LogText);
            //    service.WriteDataService.CanRcl(WRITEDATA, LogText);
            //    this.timer.Enabled = true;
            //});

        }

        private void LogText_TextChanged(object sender, EventArgs e)
        {
            LogText.ScrollToCaret();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Enabled = false;

            if (this.WriteRq.Value < DateTime.Now.AddDays(-1))
                this.WriteRq.Value = this.WriteRq.Value.AddDays(1);
            else
                this.WriteRq.Value = this.WriteRq.Value.AddDays(-1);

            try
                {
                    //LogText.Clear();
                    LogText.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "日处理开始...");

                    WRITEDATAEntity WRITEDATA = new WRITEDATAEntity();
                    WRITEDATA.RQ = this.WriteRq.Value.ToShortDateString();

                    RCLEntity rcldata = new RCLEntity();
                    rcldata.RQ = this.WriteRq.Value.ToShortDateString();

                    // service.WriteDataService.CanHyRcl(rcldata, LogText);
                    service.WriteDataService.CanRcl(WRITEDATA, LogText);
                }
                finally
                {
                    timer.Interval = 3600000;        //60 min

                    timer.Enabled = true;
                };
        }

        private void btn_rcl_Click(object sender, EventArgs e)
        {

        }
    }
}
