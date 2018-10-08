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
    public partial class frmBillingAccount : Form
    {
        public string DBPath;
        public string LoginUser;
        public bool isChoose = false;
        public string AccountCode;
        public string Accounts;
        public string Description;
        public string FixedAmount;


        clsFunctiion cf = new clsFunctiion();
        DataView dv;

        public frmBillingAccount()
        {
            InitializeComponent();
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (textBox3.Text == string.Empty)
                {
                    textBox3.Text = "";
                    textBox3.Focus();
                }
                else if (cf.isIntegerValid(e.KeyChar.ToString()) || e.KeyChar == '.')
                {

                }
                else
                {
                    MessageBox.Show("Error: Please enter monetary value.", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox3.Text = "";
                    textBox3.Focus();
                }
            }

            catch
            { }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.Text == string.Empty)
            {
                textBox3.Text = "0.00";
            }
        }

        private void frmBillingAccount_Load(object sender, EventArgs e)
        {
            cf.DbLocation = DBPath;
            LoadBillingAccount();
        }
        private void LoadBillingAccount()
        {
            string Query = "Select sysid,AccountCode,Accounts,Description,FixedAmount,isActive from tblBillingAccount " +
                           " where isActive = 1 order by AccountCode asc";
            DataTable dtRecords = cf.GetRecords(Query);
            if (dtRecords.Rows.Count > 0)
            {
                dataGridView1.DataSource = dtRecords;
                dataGridView1.Refresh();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to add this Account on your billing?", "Add Transaction to Billing", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                Accounts = dataGridView1["Accounts", index].Value.ToString();
                AccountCode = dataGridView1["AccountCode", index].Value.ToString();
                Description = dataGridView1["Description", index].Value.ToString();
                FixedAmount = textBox3.Text;
                isChoose = true;
                this.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dataGridView1.SelectedRows[0].Index;
            textBox1.Text = dataGridView1["Accounts", index].Value.ToString();
            textBox4.Text = dataGridView1["AccountCode", index].Value.ToString();
            textBox3.Text = dataGridView1["Description", index].Value.ToString();
            textBox3.Text = dataGridView1["FixedAmount", index].Value.ToString();
        }

        
    }
}
