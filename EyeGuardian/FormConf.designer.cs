namespace EyeGuardian
{
    partial class FormConf
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
            this.chkBox_RestNow = new System.Windows.Forms.CheckBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkBox_TimeSetDef = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dTP_Time2Work = new System.Windows.Forms.DateTimePicker();
            this.dTP_Time2Rest = new System.Windows.Forms.DateTimePicker();
            this.chkBox_WorkNow = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkBox_RestNow
            // 
            this.chkBox_RestNow.AutoSize = true;
            this.chkBox_RestNow.Checked = true;
            this.chkBox_RestNow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBox_RestNow.Location = new System.Drawing.Point(18, 26);
            this.chkBox_RestNow.Name = "chkBox_RestNow";
            this.chkBox_RestNow.Size = new System.Drawing.Size(180, 17);
            this.chkBox_RestNow.TabIndex = 0;
            this.chkBox_RestNow.Text = "Rest immediately on double-click";
            this.chkBox_RestNow.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.saveButton.Location = new System.Drawing.Point(157, 248);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(76, 248);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Rest Period:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Work Period:";
            // 
            // chkBox_TimeSetDef
            // 
            this.chkBox_TimeSetDef.AutoSize = true;
            this.chkBox_TimeSetDef.Checked = true;
            this.chkBox_TimeSetDef.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBox_TimeSetDef.Location = new System.Drawing.Point(18, 28);
            this.chkBox_TimeSetDef.Name = "chkBox_TimeSetDef";
            this.chkBox_TimeSetDef.Size = new System.Drawing.Size(125, 17);
            this.chkBox_TimeSetDef.TabIndex = 9;
            this.chkBox_TimeSetDef.Text = "Keep default settings";
            this.chkBox_TimeSetDef.UseVisualStyleBackColor = true;
            this.chkBox_TimeSetDef.CheckedChanged += new System.EventHandler(this.chkBox_TimeSetDef_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dTP_Time2Work);
            this.groupBox1.Controls.Add(this.dTP_Time2Rest);
            this.groupBox1.Controls.Add(this.chkBox_TimeSetDef);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 118);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(221, 113);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Time Setting";
            // 
            // dTP_Time2Work
            // 
            this.dTP_Time2Work.CustomFormat = "HH:mm:ss";
            this.dTP_Time2Work.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTP_Time2Work.Location = new System.Drawing.Point(88, 55);
            this.dTP_Time2Work.Name = "dTP_Time2Work";
            this.dTP_Time2Work.ShowUpDown = true;
            this.dTP_Time2Work.Size = new System.Drawing.Size(68, 20);
            this.dTP_Time2Work.TabIndex = 14;
            this.dTP_Time2Work.Value = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            // 
            // dTP_Time2Rest
            // 
            this.dTP_Time2Rest.CustomFormat = "HH:mm:ss";
            this.dTP_Time2Rest.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTP_Time2Rest.Location = new System.Drawing.Point(88, 79);
            this.dTP_Time2Rest.Name = "dTP_Time2Rest";
            this.dTP_Time2Rest.ShowUpDown = true;
            this.dTP_Time2Rest.Size = new System.Drawing.Size(68, 20);
            this.dTP_Time2Rest.TabIndex = 13;
            this.dTP_Time2Rest.Value = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            // 
            // chkBox_WorkNow
            // 
            this.chkBox_WorkNow.AutoSize = true;
            this.chkBox_WorkNow.Location = new System.Drawing.Point(18, 47);
            this.chkBox_WorkNow.Name = "chkBox_WorkNow";
            this.chkBox_WorkNow.Size = new System.Drawing.Size(184, 17);
            this.chkBox_WorkNow.TabIndex = 11;
            this.chkBox_WorkNow.Text = "Work immediately on double-click";
            this.chkBox_WorkNow.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkBox_WorkNow);
            this.groupBox2.Controls.Add(this.chkBox_RestNow);
            this.groupBox2.Location = new System.Drawing.Point(12, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(220, 73);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Double-Click Behavior";
            // 
            // FormConf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(249, 283);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormConf";
            this.Text = "Configuration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SaveSettings);
            this.Shown += new System.EventHandler(this.LoadSettings);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkBox_RestNow;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkBox_TimeSetDef;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkBox_WorkNow;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DateTimePicker dTP_Time2Rest;
        private System.Windows.Forms.DateTimePicker dTP_Time2Work;
    }
}

