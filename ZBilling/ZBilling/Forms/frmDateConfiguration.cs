using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZBilling.Class;
using Microsoft.VisualBasic;

namespace ZBilling.Forms
{
    public partial class frmDateConfiguration : Form
    {
        clsFunctiion cf = new clsFunctiion();

        public string DBPath;
        public string LoginUser;
        public IniFile IniPath;

        public frmDateConfiguration()
        {
            InitializeComponent();
        }

        private void LoadEmailInfo()
        {
            try
            {
                textBox8.Text = IniPath.Read("email_host", "Email");
                textBox9.Text = IniPath.Read("username", "Email");
                textBox10.Text = IniPath.Read("password", "Email");
                textBox11.Text = IniPath.Read("bodymessage", "Email");
            }
            catch
            {
            }
        }

        private void LoadDateSettings()
        {
            try
            {
                cf.DbLocation = DBPath;
                string Query = "Select top 1 * from tblSettings where isActive = 1 order by sysid desc";
                DataTable dtrecords = cf.GetRecords(Query);
                if (dtrecords.Rows.Count > 0)
                {
                    if (dtrecords.Rows[0]["CurrentYear"] != null)
                    {
                        textBox1.Text = dtrecords.Rows[0]["CurrentYear"].ToString();
                    }
                    if (dtrecords.Rows[0]["CurrentMonth"] != null)
                    {
                        textBox2.Text = dtrecords.Rows[0]["CurrentMonth"].ToString();
                    }
                    if (dtrecords.Rows[0]["MonthDue"] != null)
                    {
                        textBox3.Text = dtrecords.Rows[0]["MonthDue"].ToString();
                    }
                    if (dtrecords.Rows[0]["BillingCoverDays"] != null)
                    {
                        textBox4.Text = dtrecords.Rows[0]["BillingCoverDays"].ToString();
                    }
                    if (dtrecords.Rows[0]["TransStartDate"] != null)
                    {
                        textBox5.Text = dtrecords.Rows[0]["TransStartDate"].ToString();
                    }
                    if (dtrecords.Rows[0]["TransEndDate"] != null)
                    {
                        textBox6.Text = dtrecords.Rows[0]["TransEndDate"].ToString();
                    }
                    label5.Text = dtrecords.Rows[0]["sysid"].ToString();
                    textBox7.Text = dtrecords.Rows[0]["monthlyinterestrate"].ToString();
                    label9.Text = label5.Text;
                }
            }
            catch
            {
            }
        }

        private void LoadRecords()
        {
            LoadDateSettings();
        }

        private void frmDateConfiguration_Load(object sender, EventArgs e)
        {   
            LoadRecords();
            LoadEmailInfo();
        }

        private void LoadInterestRateHistory()
        {
            try
            {
                string Query = "select a.Module,a.Description,u.Lastname,u.Firstname,a.isActive from tblAuditTrail a left join tblUsers u on a.UserID = u.sysid order by a.sysid desc";
                DataTable dtRecords = cf.GetRecords(Query);
                if (dtRecords.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dtRecords;
                }
            }
            catch
            {
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!cf.isIntegerValid(textBox1.Text) || string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Error: Wrong Year. Please enter a valid year", "Invalid Year", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Text = DateTime.Now.ToString("yyyy");
                textBox1.Focus();
                return;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!cf.isIntegerValid(textBox1.Text) || string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Error: Wrong Month. Please enter a valid month", "Invalid Month", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox2.Text = DateTime.Now.ToString("mm");
                textBox2.Focus();
                return;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (!cf.isIntegerValid(textBox4.Text) || string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Error: Wrong Billing Days. Please enter a valid number", "Invalid Billing Day Cover", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox4.Text = "0";
                textBox4.Focus();
                return;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (!cf.isIntegerValid(textBox1.Text) || string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Error: Wrong Month Due. Please enter a valid number", "Invalid Month Due", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox3.Text = "0";
                textBox3.Focus();
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try {

                string Query = "Select top 1 * from tblSettings where isActive = 1 order by sysid desc";
                string SaveQuery = string.Empty;
                DataTable dtrecords = cf.GetRecords(Query);
                if (dtrecords.Rows.Count > 0)
                {
                    SaveQuery = "Update tblsettings set CurrentYear=" + textBox1.Text + ",CurrentMonth=" + textBox2.Text + ",MonthDue=" + textBox3.Text + ",BillingCoverDays=" + textBox4.Text + ",TransStartDate=" + textBox5.Text + ",TransEndDate=" +
                     textBox6.Text + " where sysid=" + label5.Text;
                    cf.ExecuteNonQuery(SaveQuery);
                }else
                {
                    SaveQuery = "Insert into tblsettings (CurrentYear,CurrentMonth,MonthDue,BillingCoverDays,TransStartDate,TransEndDate)Values(" + textBox1.Text + "," + textBox2.Text + "," + textBox3.Text + "," + textBox4.Text + "," + textBox5.Text + "," + textBox6.Text + ")";
                    cf.ExecuteNonQuery(SaveQuery);
                }

                MessageBox.Show("Done Saving Record", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!cf.isIntegerValid(textBox7.Text.Replace(".", "")))
            {
                MessageBox.Show("Failed to update the Monthly Interest Rate. Please check your value", "Unable to update", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (!string.IsNullOrEmpty(textBox7.Text))
            {
                string Query = "Update tblSettings set monthlyinterestrate = "+ textBox7.Text +" where isActive = 1 and sysid = " + label9.Text;
                if (!cf.ExecuteNonQuery(Query))
                {
                    MessageBox.Show("Failed to update the Monthly Interest Rate. Please check your data", "Unable to update", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    MessageBox.Show("Done", "Update sucessfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void textBox10_MouseHover(object sender, EventArgs e)
        {
            textBox10.PasswordChar = '\0';
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool isvalid = false;
            bool ContinueSending = false;
            string Email = string.Empty;
            while (!isvalid)
            {
                Email = Microsoft.VisualBasic.Interaction.InputBox("Enter email to send message", "Test Email", "", -1, -1);
                if (!cf.IsValidEmail(Email))
                {
                    isvalid = false;
                }
                else
                {
                    isvalid = true;
                }
            }
            if (string.IsNullOrEmpty(textBox8.Text) || string.IsNullOrEmpty(textBox9.Text) || string.IsNullOrEmpty(textBox10.Text))
            {
                MessageBox.Show("Error: Please check your host email.", "Invalid Host Email Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBox11.Text))
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to send without body/mesage?", "No Body/Message to send", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    ContinueSending = true;
                }
            }
            else
            {
                ContinueSending = true;
            }
            if (ContinueSending)
            {
                if (cf.SendEmail(string.Empty, Email, textBox9.Text, textBox10.Text, textBox8.Text, textBox11.Text))
                {
                    MessageBox.Show("Successfully sent to " + Email, "Password Sent", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox8.Text) || string.IsNullOrEmpty(textBox9.Text) || string.IsNullOrEmpty(textBox10.Text))
            {
                MessageBox.Show("Error: Please check your host email.", "Invalid Host Email Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            bool ContinueSending = false;
            if (string.IsNullOrEmpty(textBox11.Text))
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to send without body/mesage?", "No Body/Message to send", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    ContinueSending = true;
                }
            }
            else
            {
                ContinueSending = true;
            }
            if (ContinueSending)
            {
                IniPath.Write("email_host",textBox8.Text, "Email");
                IniPath.Write("username",textBox9.Text, "Email");
                IniPath.Write("password", textBox10.Text, "Email");
                IniPath.Write("bodymessage",textBox11.Text, "Email");

                MessageBox.Show("Email Setting saved.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
