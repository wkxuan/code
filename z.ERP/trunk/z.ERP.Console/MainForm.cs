﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace z.ERP.Console
{
    public partial class MainForm : FormBase
    {
        public MainForm()
        {
            InitializeComponent();
            ButtonClick(btn_rcl, () =>
            {
                var e = employee;
                var a = service.SpglService.GetKindInit();
            });
        }
    }
}