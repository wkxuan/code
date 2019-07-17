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

            ButtonClick(btn_rcl, () =>
            {
                LogText.Clear();
                LogText.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "开始");
                WRITEDATAEntity WRITEDATA = new WRITEDATAEntity();
                WRITEDATA.RQ = this.WriteRq.Value.ToShortDateString();

                RCLEntity rcldata = new RCLEntity();
                rcldata.RQ = this.WriteRq.Value.ToShortDateString();

                // service.WriteDataService.CanHyRcl(rcldata, LogText);
                service.WriteDataService.CanRcl(WRITEDATA, LogText);
            });


            this.WriteRq.Value = DateTime.Now;
            this.timer.Enabled = true;
        }

        private void LogText_TextChanged(object sender, EventArgs e)
        {
            LogText.ScrollToCaret();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            timer.Enabled = false;
            this.WriteRq.Value = DateTime.Now;
            try
            {

                ButtonClick(btn_rcl, () =>
                {
                    LogText.Clear();
                    LogText.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "开始");
                    WRITEDATAEntity WRITEDATA = new WRITEDATAEntity();
                    WRITEDATA.RQ = this.WriteRq.Value.ToShortDateString();

                    RCLEntity rcldata = new RCLEntity();
                    rcldata.RQ = this.WriteRq.Value.ToShortDateString();

                    // service.WriteDataService.CanHyRcl(rcldata, LogText);
                    service.WriteDataService.CanRcl(WRITEDATA, LogText);
                });
            }
            finally
            {
                timer.Interval = 60000;        //10 min
                timer.Enabled = true;
            };
        }
    }
}
