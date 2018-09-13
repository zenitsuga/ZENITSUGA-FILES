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
                dtTransDetails.Columns.Add("Description");
                dtTransDetails.Columns.Add("Amount");
                dtTransDetails.Columns.Add("Remarks");

                dataGridView1.DataSource = dtTransDetails;
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
                AutoLoadInfo(textBox11, textBox12, textBox13);
            }
            catch
            {
            }
        }
        private void AutoLoadInfo(TextBox tbCUStart,TextBox tbCUEnd,TextBox tbDueDate)
        {
            try
            {
                string Query = "select top 1 * from tblSettings where isActive=1 order by sysid desc";
                DataTable dtRecords = cf.GetRecords(Query);
                if (dtRecords.Rows.Count > 0)
                {
                    tbDueDate.Text = dtRecords.Rows[0]["CurrentYear"].ToString() + "-" + dtRecords.Rows[0]["CurrentMonth"].ToString() + "-" + dtRecords.Rows[0]["MonthDue"].ToString();
                    int DaysBeforeCutoff = int.Parse(dtRecords.Rows[0]["BillingCoverDays"] != null ? dtRecords.Rows[0]["BillingCoverDays"].ToString() : "30");
                    tbCUStart.Text = DateTime.Parse(tbDueDate.Text).AddDays(-DaysBeforeCutoff).ToString("yyyy-MM-dd");
                    tbCUEnd.Text = DateTime.Parse(tbCUStart.Text).AddDays(DaysBeforeCutoff).ToString("yyyy-MM-dd");
                }
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
        private void LoadRoom()
        {
            try
            {
                string CustID = customerType == "Owner" ? "CustomerID" : "TenantID";
                string sysid = customerType == "Owner" ? label11.Text : label12.Text;

                comboBox1.DataSource = cf.GetRecords("Select RoomNumber from tblRoomAssignment where " + CustID + " =" + sysid);
                comboBox1.DisplayMember = "RoomNumber";
            }
            catch
            {
            }
        }
        private void comboBox1_Enter(object sender, EventArgs e)
        {
            LoadRoom();
            ComputeSummary();
            ComputeOutStanding();
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
            frmBillingAccount ba = new frmBillingAccount();
            ba.ShowDialog();
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

        private DataTable AddTransaction(string TransactionID,string Account,string Description,string Amount,string Remarks)
        {
            try
            {
                DataRow drow = dtTransDetails.NewRow();
                drow[0] = TransactionID;
                drow[1] = Account;
                drow[2] = Description;
                drow[3] = Amount;
                drow[4] = string.Empty;
                dtTransDetails.Rows.Add(drow);
            }
            catch
            {
            }
            return dtTransDetails;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to add this?", "Add Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                
                //dataGridView1.DataSource = AddTransaction(TransactionID,;

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
                //DataRow drow = dtTransDetails.NewRow();
                //drow[0] = TransactionID;
                //drow[1] = comboBox2.Text;
                //drow[2] = cf.CheckAmountByType(comboBox2.Text, textBox7.Text);
                //drow[3] = textBox3.Text;
                //dtTransDetails.Rows.Add(drow);
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

            }
            customerType = tr.CustomerType;
            dataGridView1.DataSource = null;
            LoadRoom();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            ComputeOutStanding();
        }

        private void LoadTransactionDetails(string DueDate)
        {
            try
            {
                dtTransDetails = new DataTable();
                DataTableDetails();
                DateTime DueDateStart = DateTime.Parse(DueDate);
                DateTime DueDateEnd = DueDateStart;
                string Query = "select * from tblTransactionDetails td " +
                               " Left Join tblTransaction t on td.ReferenceID = t.sysid " +
                               " where t.DueDate between '" + DueDateStart + "' and '" + DueDateEnd + "'";
                DataTable dtResult = cf.GetRecords(Query);
                if (dtResult.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtResult.Rows)
                    {
                        DataRow drow = dtTransDetails.NewRow();
                        drow["ReferenceID"] = dr["ReferenceID"].ToString();
                        drow["Accounts"] = dr["Accounts"].ToString();
                        drow["Description"] = dr["Description"].ToString();
                        drow["Amount"] = dr["Amount"].ToString();
                        drow["Remarks"] = dr["Remarks"].ToString();
                        dtTransDetails.Rows.Add(drow);
                    }
                    dataGridView1.DataSource = dtTransDetails;
                    ComputeOutStanding();
                    ComputeSummary();
                }
            }
            catch
            {
            }
        }

        private void InsertMonthlyDue()
        {
            try
            {
                string Amount = "0.00";
                DataRow drow = dtTransDetails.NewRow();
                drow[0] = TransactionID;
                drow[1] = "MODUE";
                drow[2] = "Monthly Due for " + DateTime.Now.ToString("MMMM");
                cf.CheckMonthlyRate(comboBox1.Text, ref Amount);
                drow[3] = Amount;
                drow[4] = "Auto computed";

                if (Amount == "0.00")
                {
                    MessageBox.Show("Error: Amount Due is 0.00. Cannot saved the transaction. Please check", "Amount is 0.00", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                dtTransDetails.Rows.Add(drow);
                dataGridView1.DataSource = dtTransDetails;
            }
            catch
            {
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(textBox13.Text))
                {
                    MessageBox.Show("Error: Cannot saved transaction. Please check your due date.", "Invalid DueDate", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("Error: Cannot saved 0 transaction. Please add atleast one.", "Invalid Transaction", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DialogResult dr = MessageBox.Show("This will save your transaction. Would you like to continue?", "Save Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    string userID = cf.GetSysID("tblusers", "where username ='" + Userlogin + "'").ToString();
                    
                    if (cf.InsertTransaction(TransactionID, string.Empty, textBox4.Text, label11.Text, comboBox1.Text, FixedNegaAmount(textBox5.Text), FixedNegaAmount(textBox6.Text), userID, textBox13.Text))
                    {
                            if (SaveBillingDetails())
                            {
                        
                                MessageBox.Show("Transaction Saved", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                dataGridView1.DataSource = null;
                                button3.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show("Error Saving Transaction", "Please check your transaction details", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                DeleteBillingDetails(TransactionID);
                                button3.Enabled = false;
                                return;
                            }
                    }
                    else
                    {
                        MessageBox.Show("Error Saving Transaction", "Please check your transaction", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        button3.Enabled = false;
                    }
                    ComputeSummary();
                }
            }
            catch
            {
                MessageBox.Show("Error in Saving Billing.Please check", "Saving Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteBillingDetails(string RefID)
        {
            try
            {
                int refid = cf.GetSysID("tblTransaction", " where TransactionNo='" + RefID + "'");
                string Query = "Delete from tblTransactionDetails where referenceID=" + refid;
                cf.ExecuteNonQuery(Query);
                Query = "Delete from tblTransaction where sysID=" + refid;
                cf.ExecuteNonQuery(Query);
            }
            catch
            {
            }
        }

        private bool SaveBillingDetails()
        {
            bool result = false;
            try
            {
                foreach (DataGridViewRow dgr in dataGridView1.Rows)
                {
                    string userID = cf.GetSysID("tblusers", "where username ='" + Userlogin + "'").ToString();
                    string RefID = dgr.Cells["ReferenceID"].Value.ToString();
                    string Account = dgr.Cells["Accounts"].Value.ToString();
                    string Description = dgr.Cells["Description"].Value.ToString();
                    string Amount = dgr.Cells["Amount"].Value.ToString();
                    string Remarks = dgr.Cells["Remarks"].Value.ToString();

                    int billid = 0;
                    int refid = 0;

                    refid = cf.GetSysID("tblTransaction", " where TransactionNo='" + RefID + "'");
                    billid = cf.GetSysID("tblBillingAccount"," where AccountCode = '" + Account + "'");

                    if (!string.IsNullOrEmpty(RefID) || refid != 0)
                    {
                        if (!string.IsNullOrEmpty(Account) || billid != 0 )
                        {
                            string Query = "Insert into tblTransactionDetails (ReferenceID,Accounts,Description,Amount,UserID,isActive,Remarks)values (" + refid + "," + billid + ",'" + Description + "'," + Amount + "," + userID + ",1,'" + Remarks + "')";

                            result = cf.ExecuteNonQuery(Query);
                        }else
                        {
                            MessageBox.Show("Error in Account Name. Please check","Failed to save entry",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            return false; 
                        }
                    }else
                    {
                        MessageBox.Show("Error in Reference ID. Please check","Failed to save entry",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            return false;
                    }
                }
            }
            catch
            {
            }
            return result;
        }

        private void label12_TextChanged(object sender, EventArgs e)
        {
            string Query = "Select c.SysID,c.Lastname + ',' + c.Firstname as 'CustomerName' from tblCustomerTenant c left Join tblTenant t on t.OwnerID = c.sysid where t.sysId = " + label12.Text;
            DataTable dtResult = cf.GetRecords(Query);
            if (dtResult.Rows.Count > 0)
            {
                textBox2.Text = dtResult.Rows[0]["CustomerName"].ToString();
                label11.Text = dtResult.Rows[0]["SysID"].ToString();
            }
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            try{
                if (!string.IsNullOrEmpty(textBox13.Text))
                {
                    if (cf.CheckDueDateTranasactionCreated(textBox13.Text))
                    {
                        if (DateTime.Now.Subtract(DateTime.Parse(textBox13.Text)).Days > 0)
                        {
                            if (!checkBox1.Checked)
                            {
                                DialogResult dr = MessageBox.Show("Warning: Due date for this month was reach. Would you like to check override cut-off to generate billing for this month.", "Transaction OverDue", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (dr == DialogResult.Yes)
                                {
                                    checkBox1.Checked = true;

                                }
                            }
                        }
                        InsertMonthlyDue();
                    }
                }
                LoadTransactionDetails(textBox13.Text);
            }catch
                {
                }
        }
    }
}
