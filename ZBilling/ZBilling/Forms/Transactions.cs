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
    public partial class Transactions : Form
    {
        public string DBPath;
        clsFunctiion cf = new clsFunctiion();

        public string Userlogin;

        string customerType = string.Empty;

        DataTable dtTransDetails;
        int GetIDTransaction;

        bool formIsLoad;

        string TransactionID;

        public Transactions()
        {
            InitializeComponent();
        }

        private void DataTableDetails()
        {
            try
            {
                dtTransDetails = new DataTable();
                dtTransDetails.Columns.Add("ReferenceID");
                dtTransDetails.Columns.Add("Accounts");
                dtTransDetails.Columns.Add("Amount");
                dtTransDetails.Columns.Add("Remarks");
            }catch
            {
            }
        }

        private void ComputeSummary()
        {
            double Openbal = 0.00;
            double Outbal = 0.00;
            double TotalBal = 0;

            if (dataGridView2.Rows.Count > 0)
            {
                foreach (DataGridViewRow dgr in dataGridView2.Rows)
                {
                    double OpeningBal = AmountChecker(dgr.Cells["OpeningBalance"].Value.ToString());
                    double OutStandingBal = AmountChecker(dgr.Cells["OutstandingBalance"].Value.ToString());

                    Openbal += OpeningBal;
                    Outbal += OutStandingBal;
                }
                TotalBal = Openbal - Outbal;

                textBox9.Text = dataGridView2.Rows.Count.ToString();
                textBox10.Text = string.Format("{0:C}", TotalBal.ToString()).Replace("$", "");
            }
        }

        private double AmountChecker(string Amount)
        {
            double amountResult = 0.00;
            try
            {
                if (!string.IsNullOrEmpty(Amount))
                {
                    amountResult = double.Parse(Amount);
                }
            }
            catch
            {
            }
            return amountResult;
        }

        private void Transactions_Load(object sender, EventArgs e)
        {
            loadNewTrans();        
        }

        private void loadNewTrans()
        {
            try
            {
                dataGridView1.DataSource = null;
                textBox5.Text = "0.00";
                textBox6.Text = "0.00";
                textBox7.Text = "0.00";
                textBox2.Text = string.Empty;
                label11.Text = "0";
                cf.DbLocation = DBPath;
                GetIDTransaction = int.Parse(cf.GetLastID("tblTransaction"));
                TransactionID = DateTime.Now.ToString("MMMyyyy") + "-" + GetIDTransaction.ToString().PadLeft(4, '0');
                textBox4.Text = DateTime.Now.ToString("yyyy-MM-dd");
                textBox1.Text = TransactionID;
                Textbox("OwnerName", textBox2);
                Textbox("TenantName", textBox3);
                cf.DbLocation = DBPath;
                comboBox2.DataSource = cf.GetRecords("Select sysid,TransCode from tblTransactionType where isActive = 1 order by TransCode asc");
                comboBox2.DisplayMember = "TransCode";
                comboBox2.ValueMember = "sysID";
                comboBox3.DataSource = cf.GetRecords("Select Distinct YEAR(DateTransaction) as YEAR from tblTransaction where isActive=1");
                comboBox3.DisplayMember = "YEAR";
                formIsLoad = true;
                if (comboBox3.Items.Count == 0)
                {
                    comboBox3.Text = (DateTime.Now.Year.ToString());
                    //comboBox3.Items.Add(DateTime.Now.Year.ToString());
                }
                DataTableDetails();
                textBox5.Text = string.Format("{0:C}", cf.ComputeLastOutStandingForBalance(label11.Text)).Replace("$", "");
            }
            catch
            {
            }
        }

        private void ComputeOutStanding()
        {
            double Total = 0.00;
            foreach (DataGridViewRow dgr in dataGridView1.Rows)
            {
                string Amount = dgr.Cells["Amount"].Value.ToString();
                if (isNumberOnly(Amount))
                {
                    Total += double.Parse(Amount);
                }
            }

            textBox6.Text = string.Format("{0:C}", Total.ToString()).Replace("$", "");
        }

        private void Textbox(string TypeQuery,TextBox tb)
        {
            try
            {
                AutoCompleteStringCollection autocoll = new AutoCompleteStringCollection();
                cf.DbLocation = DBPath;
                foreach (string strProf in cf.GetProfileInfo(TypeQuery))
                {
                    autocoll.Add(strProf);
                }

                tb.AutoCompleteMode = AutoCompleteMode.Append;
                tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                tb.AutoCompleteCustomSource = autocoll;
            }
            catch
            {
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmFetchRecords ffr = new FrmFetchRecords();
            ffr.DBLocation = DBPath;
            ffr.TableName = "tblCustomerTenant";
            ffr.ShowDialog();
        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {
            string CustID = customerType == "Owner" ? "CustomerID" : "TenantID";
            string sysid = customerType == "Owner" ? label11.Text : label12.Text;

            comboBox1.DataSource = cf.GetRecords("Select RoomNumber from tblRoomAssignment where "+ CustID +" =" + sysid);
            comboBox1.DisplayMember = "RoomNumber";
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (formIsLoad)
            {
                label11.Text = cf.CheckCustomer(textBox2.Text).ToString();
                if (label11.Text == "0")
                {
                    //MessageBox.Show("Error: Customer not found. Please check", "Invalid Customer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //textBox2.Focus();
                }
                dataGridView2.DataSource = null;
                dataGridView2.DataSource = cf.GetTransactionSummary(comboBox3.Text, label11.Text);
                ComputeSummary();
                textBox5.Text = string.Format("{0:C}", cf.ComputeLastOutStandingForBalance(label11.Text)).Replace("$", "");
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private bool isNumberOnly(string Checker)
        {
            bool result = false;
            try
            {
                double.Parse(Checker);
                result = true;
            }
            catch
            {
            }
            return result;
        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if(!isNumberOnly(textBox7.Text))
                {
                    MessageBox.Show("Error: Invalid Amount. Please input a money value", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox7.Text = string.Empty;
                    textBox7.Focus();
                    return;
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Error: Cannot saved 0 transaction. Please add atleast one.", "Invalid Transaction", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DialogResult dr = MessageBox.Show("This will save your transaction. Would you like to continue?", "Save Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string userID = cf.GetSysID("tblusers","where username ='" + Userlogin + "'").ToString();

                if (cf.InsertTransaction(TransactionID, string.Empty, textBox4.Text, label11.Text, comboBox1.Text, FixedNegaAmount(textBox5.Text), FixedNegaAmount(textBox6.Text), userID))
                {
                    MessageBox.Show("Transaction Saved", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dataGridView1.DataSource = null;
                    button3.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Error Saving Transaction", "Please check your transaction", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    button3.Enabled = false;
                }

                dataGridView2.DataSource = null;
                dataGridView2.DataSource = cf.GetTransactionSummary(comboBox3.Text, label11.Text);
                ComputeSummary();
            }

        }

        private string FixedNegaAmount(string Amount)
        {
            string Result = Amount;
            try
            {
                if (Amount.Contains("("))
                {
                   Amount = Amount.Replace("(", "-").Replace(")","");
                }
            }
            catch
            {
            }
            return Result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to add this?", "Add Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                DataRow drow = dtTransDetails.NewRow();
                drow[0] = TransactionID;
                drow[1] = comboBox2.Text;
                drow[2] = cf.CheckAmountByType(comboBox2.Text,textBox7.Text);
                drow[3] = textBox3.Text;
                dtTransDetails.Rows.Add(drow);
                dataGridView1.DataSource = dtTransDetails;

            }
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            if (!isNumberOnly(textBox7.Text))
            {
                MessageBox.Show("Error: Invalid Amount. Please input a money value", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox7.Text = string.Empty;
                textBox7.Focus();
                return;
            }
        }

        private void textBox7_Enter(object sender, EventArgs e)
        {
            
        }

        private void textBox7_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!isNumberOnly(textBox7.Text))
                {
                    MessageBox.Show("Error: Invalid Amount. Please input a money value", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox7.Text = string.Empty;
                    textBox7.Focus();
                    return;
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (label11.Text == "0" && label12.Text == "0")
            {
                MessageBox.Show("Error: Please select first customer.","Invalid customer",MessageBoxButtons.OK,MessageBoxIcon.Error);
                button4.Focus();
                return;
            }

            if (textBox7.Text == string.Empty)
            {
                MessageBox.Show("Error: Cannot add transaction.Please check", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult dr = MessageBox.Show("Are you sure you want to add this?", "Add Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                DataRow drow = dtTransDetails.NewRow();
                drow[0] = TransactionID;
                drow[1] = comboBox2.Text;
                drow[2] = cf.CheckAmountByType(comboBox2.Text, textBox7.Text);
                drow[3] = textBox3.Text;
                dtTransDetails.Rows.Add(drow);
                dataGridView1.DataSource = dtTransDetails;
                ComputeOutStanding();
                textBox3.Text = string.Empty;
                textBox7.Text = string.Empty;
            }
        }

        private void Transactions_FormClosing(object sender, FormClosingEventArgs e)
        {
            formIsLoad = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to start a new transaction?",
                "New Transaction???", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                loadNewTrans();
            }
        }

        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
           
        }

        private void CheckSummary()
        {
            try
            {
                dataGridView2.DataSource = null;
                dataGridView2.DataSource = cf.GetTransactionSummary(comboBox3.Text, label11.Text);
                ComputeSummary();
            }
            catch
            {
            }
        }

        private void comboBox3_Enter(object sender, EventArgs e)
        {
            CheckSummary();
        }

        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            CheckSummary();
        }
        
        private void comboBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                CheckSummary();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FrmTransactionReference tr = new FrmTransactionReference();
            tr.DBPath = DBPath;
            tr.LoginUser = Userlogin;
            tr.ShowDialog();
            if (tr.CustomerType == "Owner")
            {
                textBox2.Text = tr.customerName;
                label11.Text = tr.customerID;

                textBox8.Text = string.Empty;
                label12.Text = "0";
            }
            else if(tr.CustomerType == "Tenant")
            {
                textBox8.Text = tr.customerName;
                label12.Text = tr.customerID;

                textBox2.Text = string.Empty;
                label11.Text = "0";
            }
            customerType = tr.CustomerType;
        }
    }
}
