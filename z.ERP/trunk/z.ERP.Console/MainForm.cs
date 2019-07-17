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
                var msg = string.Empty;
                var e = employee;
                WRITEDATAEntity WRITEDATA = new WRITEDATAEntity();
                WRITEDATA.RQ = this.WriteRq.Value.ToShortDateString();

                RCLEntity rcldata = new RCLEntity();
                rcldata.RQ = this.WriteRq.Value.ToShortDateString();

                // service.WriteDataService.CanHyRcl(rcldata, LogText);
                service.WriteDataService.CanRcl(WRITEDATA, LogText);
            });



            TimerTick(timer, () =>
            {
                LogText.Clear();
                LogText.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + "开始");
                var msg = string.Empty;
                var e = employee;
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


    }
}
