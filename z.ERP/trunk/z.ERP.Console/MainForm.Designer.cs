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
            this.SuspendLayout();
            // 
            // btn_rcl
            // 
            this.btn_rcl.Location = new System.Drawing.Point(607, 12);
            this.btn_rcl.Name = "btn_rcl";
            this.btn_rcl.Size = new System.Drawing.Size(75, 23);
            this.btn_rcl.TabIndex = 0;
            this.btn_rcl.Text = "日处理";
            this.btn_rcl.UseVisualStyleBackColor = true;
            // 
            // WriteRq
            // 
            this.WriteRq.Location = new System.Drawing.Point(68, 14);
            this.WriteRq.Name = "WriteRq";
            this.WriteRq.Size = new System.Drawing.Size(200, 21);
            this.WriteRq.TabIndex = 1;
            // 
            // LogText
            // 
            this.LogText.Location = new System.Drawing.Point(68, 52);
            this.LogText.Name = "LogText";
            this.LogText.Size = new System.Drawing.Size(614, 323);
            this.LogText.TabIndex = 2;
            this.LogText.Text = "";
            this.LogText.TextChanged += new System.EventHandler(this.LogText_TextChanged);
            // 
            // timer
            // 
            this.timer.Interval = 60000;

            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 387);
            this.Controls.Add(this.LogText);
            this.Controls.Add(this.WriteRq);
            this.Controls.Add(this.btn_rcl);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_rcl;
        private System.Windows.Forms.DateTimePicker WriteRq;
        private System.Windows.Forms.RichTextBox LogText;
        private System.Windows.Forms.Timer timer;
    }
}