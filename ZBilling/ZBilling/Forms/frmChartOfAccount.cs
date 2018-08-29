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
    public partial class frmChartOfAccount : Form
    {
        clsFunctiion cf = new clsFunctiion();
        public string DBPath;
        public string LoginUser;

        public frmChartOfAccount()
        {
            InitializeComponent();
        }

        private void frmChartOfAccount_Load(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void LoadRecords()
        {
            try
            {
                cf.DbLocation = DBPath;
                DataTable dtResult = cf.GetRecords( "Select t.sysID,t.TransCode,t.Description,u.Username,case when " +                                                      "t.accounttype = 1 then 'Debit' else 'Credit' end AccountType from "+                                                     "tblTransactionType t left join tblUsers u on t.UserID=u.sysID"); 

                dataGridView1.DataSource = dtResult;
            }
            catch
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Error: Please check your entry", "Invalid Account", MessageBoxButtons.OK,MessageBoxIcon.Error); 
                return;
            }
            int typeOfAccount = comboBox1.Text == "Debit (+)" ? 1 : 2;

            List<string> FC = new List<string>();
            FC.Add("TransCode");
            FC.Add("Description");
            FC.Add("AccountType");
            FC.Add("userID");

            List<string>FV = new List<string>();
            FV.Add(textBox1.Text);
            FV.Add(textBox2.Text);
            FV.Add(typeOfAccount.ToString());
            cf.DbLocation = DBPath;
            
            List<string> IC = new List<string>();
            IC.Add(cf.GetSysID("tblUsers", " where Username='" + LoginUser + "'").ToString());

            if (cf.InsertRecords("tbltransactionType", FC, FV, IC))
            {
                MessageBox.Show("Record Saved.");
            }
            else
            {
                MessageBox.Show("Error: Cannot save record. Please check your entry","Cannot saved", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            LoadRecords();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to delete selected Item(s)?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    foreach (DataGridViewRow dgr in dataGridView1.SelectedRows)
                    {
                        string TCode = dgr.Cells["TransCode"].Value.ToString();
                        cf.DeleteRecord("tblTransactionType", " where TransCode='" + TCode + "'");
                    }
                }
                LoadRecords();
            }
        }
    }
}
