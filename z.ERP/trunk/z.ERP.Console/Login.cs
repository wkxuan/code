using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using z.SSO;

namespace z.ERP.Console
{
    public partial class Login : FormBase
    {
        public Login()
        {
            InitializeComponent();
            ButtonClick(btn_login, () =>
             {
                 string LoginName = tb_name.Text;
                 string PassWord = tb_psw.Text;
                 UserApplication.Login(LoginName, PassWord);
                 this.Hide();
                 Application.Run(new MainForm());
                 this.Close();
             });
        }

    }
}
