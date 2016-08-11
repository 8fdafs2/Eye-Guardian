using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace EyeGuardian
{
    public partial class FormConf : Form
    {
        public FormConf()
        {
            InitializeComponent();

            dTP_Time2Rest.Validated += new EventHandler(dTPValidate);
            dTP_Time2Work.Validated += new EventHandler(dTPValidate);

            chkBox_TimeSetDef.Checked = Properties.Settings.Default.blDefTimeSetting;

            TimeSetDef();
        }

        private void dTPValidate(object sender, EventArgs e)
        {
            DateTimePicker oDTP = (DateTimePicker)sender;

            if (TimeLib.DateTime2Millisecond(oDTP.Value) < 1000)
            {
                oDTP.Value = TimeLib.Millisecond2DateTime(1000);
            }
        }

        private void LoadSettings(object sender, EventArgs e)
        {
            chkBox_RestNow.Checked = Properties.Settings.Default.blRestOnDoubleClick;
            chkBox_WorkNow.Checked = Properties.Settings.Default.blWorkOnDoubleClick;

            chkBox_TimeSetDef.Checked = Properties.Settings.Default.blDefTimeSetting;

            dTP_Time2Rest.Value = TimeLib.Millisecond2DateTime(Properties.Settings.Default.iMillisecond2Rest);
            dTP_Time2Work.Value = TimeLib.Millisecond2DateTime(Properties.Settings.Default.iMillisecond2Work);
        }

        private void SaveSettings(object sender, FormClosingEventArgs e)
        {
            // If the user clicked "Save"
            if (this.DialogResult == DialogResult.OK)
            {
                Properties.Settings.Default.blRestOnDoubleClick = chkBox_RestNow.Checked;
                Properties.Settings.Default.blWorkOnDoubleClick = chkBox_WorkNow.Checked;

                Properties.Settings.Default.blDefTimeSetting = chkBox_TimeSetDef.Checked;

                int iMillisecond2Rest = TimeLib.DateTime2Millisecond(dTP_Time2Rest.Value);
                int iMillisecond2Work = TimeLib.DateTime2Millisecond(dTP_Time2Work.Value);

                bool blTime2Rest_Changed = false;
                bool blTime2Work_Changed = false;

                if (Properties.Settings.Default.iMillisecond2Rest != iMillisecond2Rest)
                {
                    blTime2Rest_Changed = true;
                    Properties.Settings.Default.iMillisecond2Rest = iMillisecond2Rest;
                }

                if (Properties.Settings.Default.iMillisecond2Work != iMillisecond2Work)
                {
                    blTime2Work_Changed = true;
                    Properties.Settings.Default.iMillisecond2Work = iMillisecond2Work;
                }

                Properties.Settings.Default.Save();

                if (Program.blResting && blTime2Rest_Changed)
                {
                    Program.RestNow(true);
                    Debug.WriteLine("Resting");
                }
                else if (!Program.blResting && blTime2Work_Changed)
                {
                    Program.WorkNow(true);
                    Debug.WriteLine("Working");
                }

            }
        }

        private void chkBox_TimeSetDef_CheckedChanged(object sender, EventArgs e)
        {
            TimeSetDef();
        }

        private void TimeSetDef()
        {
            if (chkBox_TimeSetDef.Checked)
            {
                dTP_Time2Rest.Value = TimeLib.Millisecond2DateTime(Properties.Settings.Default.Properties.GetDefault<int>("iMillisecond2Rest"));
                dTP_Time2Work.Value = TimeLib.Millisecond2DateTime(Properties.Settings.Default.Properties.GetDefault<int>("iMillisecond2Work"));

                dTP_Time2Rest.Enabled = false;
                dTP_Time2Work.Enabled = false;
            }
            else
            {
                dTP_Time2Rest.Enabled = true;
                dTP_Time2Work.Enabled = true;
            }
        }
    }
}