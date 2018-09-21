using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using z.ERP.Services;
using z.Extensions;
using z.SSO;
using z.SSO.Model;

namespace z.ERP.Console
{
    public class FormBase : Form
    {
        public FormBase()
        {
            CheckForIllegalCrossThreadCalls = false;
            this.SetBounds((Screen.GetBounds(this).Width / 2) - (this.Width / 2),
                             (Screen.GetBounds(this).Height / 2) - (this.Height / 2),
                             this.Width, this.Height, BoundsSpecified.Location);
            service = new ServiceBase();
        }

        protected void ButtonClick(Button button, Action act, Action<Exception> error = null)
        {
            button.Click += (object sender, EventArgs e) =>
            {
                Thread t = new Thread(() =>
                  {
                      button.Enabled = false;
                      try
                      {
                          act();
                      }
                      catch (Exception ex)
                      {
                          error?.Invoke(ex);
                          MessageBox.Show(ex.InnerMessage());
                      }
                      finally
                      {
                          button.Enabled = true;
                      }
                  });
                t.IsBackground = true;
                t.Start();
            };
        }

        protected ServiceBase service
        {
            get;
            set;
        }

        /// <summary>
        /// 当前登录对象
        /// </summary>
        protected Employee employee
        {
            get
            {
                return UserApplication.GetUser<Employee>();
            }
        }
    }
}
