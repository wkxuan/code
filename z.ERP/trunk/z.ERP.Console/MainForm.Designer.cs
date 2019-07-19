namespace z.ERP.Console
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btn_rcl = new System.Windows.Forms.Button();
            this.WriteRq = new System.Windows.Forms.DateTimePicker();
            this.LogText = new System.Windows.Forms.RichTextBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_rcl
            // 
            this.btn_rcl.Location = new System.Drawing.Point(779, 15);
            this.btn_rcl.Margin = new System.Windows.Forms.Padding(4);
            this.btn_rcl.Name = "btn_rcl";
            this.btn_rcl.Size = new System.Drawing.Size(100, 29);
            this.btn_rcl.TabIndex = 0;
            this.btn_rcl.Text = "开始执行";
            this.btn_rcl.UseVisualStyleBackColor = true;
            this.btn_rcl.Click += new System.EventHandler(this.btn_rcl_Click);
            // 
            // WriteRq
            // 
            this.WriteRq.Location = new System.Drawing.Point(147, 17);
            this.WriteRq.Margin = new System.Windows.Forms.Padding(4);
            this.WriteRq.Name = "WriteRq";
            this.WriteRq.Size = new System.Drawing.Size(183, 25);
            this.WriteRq.TabIndex = 1;
            // 
            // LogText
            // 
            this.LogText.Location = new System.Drawing.Point(62, 52);
            this.LogText.Margin = new System.Windows.Forms.Padding(4);
            this.LogText.Name = "LogText";
            this.LogText.Size = new System.Drawing.Size(817, 403);
            this.LogText.TabIndex = 2;
            this.LogText.Text = "";
            this.LogText.TextChanged += new System.EventHandler(this.LogText_TextChanged);
            // 
            // timer
            // 
            this.timer.Interval = 3600000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "选择日期";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(962, 484);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LogText);
            this.Controls.Add(this.WriteRq);
            this.Controls.Add(this.btn_rcl);
            this.Location = new System.Drawing.Point(0, 0);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "云商日处理程序";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_rcl;
        private System.Windows.Forms.DateTimePicker WriteRq;
        private System.Windows.Forms.RichTextBox LogText;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label label1;
    }
}