using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZBilling.Class;

namespace ZBilling.Forms
{
    public partial class frmCompanyProfile : Form
    {
        IniFile ini;
        public string IniPath;
        public frmCompanyProfile()
        {
            InitializeComponent();
        }

        private void ReadINI()
        {
            try
            {
                if (!string.IsNullOrEmpty(IniPath))
                {
                    ini = new IniFile(IniPath);
                    textBox1.Text = ini.Read("CompanyName", "CompanyProfile");
                    textBox2.Text = ini.Read("CompanyAddress", "CompanyProfile");
                    textBox3.Text = ini.Read("ContactNumber", "CompanyProfile");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Error in writing ini", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void WriteINI()
        {
            try
            {
                if (!string.IsNullOrEmpty(IniPath))
                {
                    ini = new IniFile(IniPath);
                    ini.Write("CompanyName", textBox1.Text, "CompanyProfile");
                    ini.Write("CompanyAddress", textBox2.Text, "CompanyProfile");
                    ini.Write("ContactNumber", textBox3.Text, "CompanyProfile");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Error in writing ini", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmCompanyProfile_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(IniPath))
            {
                ReadINI();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(IniPath))
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to save this information?", "Save Company Profile", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    WriteINI();
                }
            }
        }
    }
}
