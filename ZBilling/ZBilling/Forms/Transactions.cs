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
        DataView dvTrans;

        int GetIDTransaction;

        bool formIsLoad;
        public bool isValidDueDate;

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

        private void ComputerPreviousBilling(string RoomNumber)
        {
            try
            {
                DateTime DueDate = DateTime.Parse(textBox13.Text);

                DateTime firstDayOfMonth = new DateTime(DueDate.Year, DueDate.Month, 1);
                DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                string PrevBillCode = string.Empty;
                string PDueInterest = string.Empty;
                DataTable dtRecords = cf.GetRecords("Select TOP 1 AccountCode from tblBillingAccount where isActive = 1 and Accounts = 'UnpaidDue' order by sysid desc");
                PrevBillCode = dtRecords.Rows[0]["AccountCode"] != null ? dtRecords.Rows[0]["AccountCode"].ToString() : "PREVDUE";
                PDueInterest = "IMRDUE";

                string Query = "select * from tblTransactionDetails td " +
                               " inner join tblTransaction t on td.ReferenceID = t.sysid " +
                               " full outer Join tblpayment p on p.PaymentReference = t.TransactionNo " +
                               " where t.RoomID = " + RoomNumber + 
                               " and t.DueDate not between '"+ firstDayOfMonth  + "' and '" + lastDayOfMonth + "'" +
                               " order by td.sysid asc";

                DataTable dtrecords = cf.GetRecords(Query);
                if(dtrecords.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtrecords.Rows)
                    {
                        DataRow drow = dtTransDetails.NewRow();
                        drow["ReferenceID"] = dr["TransactionNo"].ToString();
                        drow["Accounts"] = PrevBillCode;
                        drow["Description"] = dr["Description"].ToString().Contains("Unpaid") ?  dr["Description"].ToString():"(Unpaid) " + dr["Description"].ToString();
                        drow["Amount"] = string.Format("{0:C}", double.Parse(dr["Amount"].ToString())).Replace("$", "");
                        drow["Remarks"] = "Auto computed";
                        dtTransDetails.Rows.Add(drow);
                        //---- Interest Due
                        DataRow drowID = dtTransDetails.NewRow();
                        drowID["ReferenceID"] = dr["TransactionNo"].ToString();
                        drowID["Accounts"] = PDueInterest;
                        drowID["Description"] = "Interest (" + getInterestRate() + ") : " + (dr["Description"].ToString().Contains("Unpaid") ? dr["Description"].ToString() : "(Unpaid) " + dr["Description"].ToString());
                        double IntAmount = double.Parse(dr["Amount"].ToString()) * getInterestRate();
                        drowID["Amount"] = string.Format("{0:C}", IntAmount).Replace("$", "");
                        drowID["Remarks"] = "Auto computed";
                        dtTransDetails.Rows.Add(drowID);
                    }
                }
            }
            catch
            {
            }
        }

        private double getInterestRate()
        {
            double result = 0;
            try
            {
                string Query = "select top 1 isnull(MonthlyInterestRate,0) as MIR from tblsettings where isActive = 1 order by sysid desc";
                DataTable dtresult = cf.GetRecords(Query);
                if (dtresult.Rows.Count > 0)
                {
                    result = double.Parse(dtresult.Rows[0][0].ToString());
                }
            }
            catch
            {

            }
            return result;
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
            isValidDueDate = false;
            loadNewTrans(ref isValidDueDate);
            DataTable dtResult = dtTransactionRecords();
            if (dtResult.Rows.Count > 0)
            {
                dtDistinctRecord(dtResult, "DueDate",comboBox4,typeof(DateTime));
                dvTrans = dtResult.DefaultView;        
            }

        }

        private void dtDistinctRecord(DataTable dtRecords, string FilterField, ComboBox cb,Type type)
        {   
            try
            {
                cb.DataSource = null;
                if(type.Name == "int")
                {
                var Test = (from row in dtRecords.AsEnumerable()
                            select row.Field<int>(FilterField)).Distinct();
                    if (Test.Count() > 0)
                    {
                        cb.Items.Clear();
                        foreach (int strDR in Test)
                        {
                            cb.Items.Add(strDR);
                        }
                    }
                }
                else if (type.Name == "DateTime")
                {
                var Test = (from row in dtRecords.AsEnumerable()
                            select row.Field<DateTime>(FilterField)).Distinct();
                    if (Test.Count() > 0)
                    {
                        cb.Items.Clear();
                        foreach (DateTime strDR in Test)
                        {
                            cb.Items.Add(strDR.ToString("yyyy"));
                        }
                    }
                }

                
            }
            catch
            {
            }
        }

        private DataTable dtTransactionRecords()
        {
            string Query = "Select t.transactionNo,t.RoomID,t.DueDate from tbltransactiondetails td " +
                           " left join tbltransaction t on td.referenceID = t.sysid " +
                           " where t.DueDate between '"+ textBox13.Text +"' and '"+ textBox13.Text +" 23:59:59' ";
            DataTable dtResult = new DataTable();
            try
            {
                dtResult = cf.GetRecords(Query);
            }
            catch
            {
            }
            return dtResult;
        }

        private void loadNewTrans(ref bool isValidDueDate)
        {
            try
            {
                comboBox1.DataSource = null;
                textBox2.Text = textBox8.Text = string.Empty;
                label11.Text = label12.Text = "0";
                LoadRoom();
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
                CheckDueDate();
                textBox5.Text = string.Format("{0:C}", cf.ComputeLastOutStandingForBalance(label11.Text)).Replace("$", "");
                AutoLoadInfo(textBox11, textBox12, textBox13);

                if (DueDateVsCurrent())
                {
                    isValidDueDate = true;   
                }
                comboBox4.Items.Clear();
                string StartDate = cf.GetRecordValue("TransStartDate", "tblSettings", "1");
                string EndDate = cf.GetRecordValue("TransEndDate", "tblSettings", "1");
                int startYear = string.IsNullOrEmpty(StartDate) ? 2015:int.Parse(cf.isIntegerValid(StartDate) ? StartDate:"2015");
                int endYear = string.IsNullOrEmpty(EndDate) ? DateTime.Now.Year : int.Parse(cf.isIntegerValid(EndDate) ? EndDate : DateTime.Now.Year.ToString());
                for (int cnt = startYear; cnt <= endYear; cnt++)
                {
                    comboBox4.Items.Add(cnt.ToString());
                }
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
        private void ComputePrevBilling(DateTime DueDate)
        {
            try
            {

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

            textBox6.Text = string.Format("{0:C}", double.Parse(Total.ToString())).Replace("$", "");
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
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                return;
            }
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
                textBox2.Text = textBox8.Text = string.Empty;
                label11.Text = label12.Text = "0";
                
                loadNewTrans(ref isValidDueDate);
                if (!isValidDueDate)
                {
                    this.Close();
                }
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
            CheckDueDate();
            ComputeOutStanding();
        }

        private void LoadTransactionDetails(string DueDate)
        {
            try
            {
                dtTransDetails = new DataTable();
                dataGridView1.DataSource = null;
                if (dataGridView1.Rows.Count == 0)
                {
                    DataTableDetails();
                }
                string CustID = !string.IsNullOrEmpty(label11.Text) ? " and CustomerID = " + label11.Text : " and TenantID =" + label12.Text;   
                DateTime DueDateStart = DateTime.Parse(DueDate);
                DateTime DueDateEnd = DueDateStart;
                string Query = "select b.AccountCode,* from tblTransactionDetails td " +
                               " Left Join tblTransaction t on td.ReferenceID = t.sysid " +
                               " Left Join tblBillingAccount b on td.Accounts = b.sysid " +
                               " where td.isPaid = 0 and t.DueDate between '" + DueDateStart + "' and '" + DueDateEnd + "' " + CustID;
                DataTable dtResult = cf.GetRecords(Query);
                if (dtResult.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtResult.Rows)
                    {
                        DataRow drow = dtTransDetails.NewRow();
                        drow["ReferenceID"] = dr["TransactionNo"].ToString();
                        drow["Accounts"] = dr["AccountCode"].ToString();
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
                if (string.IsNullOrEmpty(textBox2.Text))
                {
                    return;
                }
                if (cf.isIntegerValid(comboBox1.Text))
                {
                    string DUECode = string.Empty;
                    DataTable dtRecords = cf.GetRecords("Select TOP 1 AccountCode from tblBillingAccount where isActive = 1 and Accounts = 'MonthlyDue' order by sysid desc");
                    DUECode = dtRecords.Rows[0]["AccountCode"] != null ? dtRecords.Rows[0]["AccountCode"].ToString() : "MODUE";
                    string Description = "Monthly Due for " + DateTime.Now.ToString("MMMM") + " " + DateTime.Now.ToString("yyyy");
                    string QueryModue = "Select td.* from tbltransactiondetails td " +
                                        " left join tbltransaction t on td.referenceID = t.sysid " + 
                                        " where t.roomid = "+ comboBox1.Text +" and  td.description = '" + Description + "'";
                    DataTable dtQuery = cf.GetRecords(QueryModue);

                    if (dtQuery.Rows.Count == 0)
                    {
                        string Amount = "0.00";
                        DataRow drow = dtTransDetails.NewRow();
                        drow[0] = TransactionID;
                        drow[1] = DUECode;
                        drow[2] = "Monthly Due for " + DateTime.Now.ToString("MMMM") + " " + DateTime.Now.ToString("yyyy");
                        cf.CheckMonthlyRate(comboBox1.Text, ref Amount);
                        drow[3] = Amount;
                        drow[4] = "Auto computed";

                        if (Amount == "0.00")
                        {
                            MessageBox.Show("Error: Amount Due is 0.00. Cannot saved the transaction. Please check", "Amount is 0.00", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        dtTransDetails.Rows.Add(drow);

                        string PDAmount = "0.00";
                        DataRow PDdrow = dtTransDetails.NewRow();
                        PDdrow[0] = TransactionID;
                        PDdrow[1] = DUECode;
                        PDdrow[2] = "Interest Monthly Due for " + DateTime.Now.ToString("MMMM") + " " + DateTime.Now.ToString("yyyy");
                        cf.CheckMonthlyRate(comboBox1.Text, ref Amount);
                        PDdrow[3] = Amount;
                        PDdrow[4] = "Auto computed";

                    }
                    dataGridView1.DataSource = dtTransDetails;
                }
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

                    int TransactionCount = cf.GetRecords("select * from tbltransaction where transactionNo = '" + textBox1.Text + "'").Rows.Count;
                    if (TransactionCount > 0)
                    {
                        string updateQuery = "update tblTransaction set OpeningBalance = " + FixedNegaAmount(textBox5.Text) + ",OutstandingBalance=" + FixedNegaAmount(textBox6.Text) + " where transactionNo = '" + textBox1.Text + "'";
                        if (cf.ExecuteNonQuery(updateQuery))
                        {
                            DeleteBillingDetails(textBox1.Text);
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
                    }
                    else
                    {
                        string CustomerType = label12.Text != "0" ? "TenantID" : string.Empty;
                        string CustID = label12.Text != "0" ? label12.Text : label11.Text; 
                    if (cf.InsertTransaction(TransactionID, string.Empty, textBox4.Text, CustID, comboBox1.Text, FixedNegaAmount(textBox5.Text), FixedNegaAmount(textBox6.Text), userID, textBox13.Text,CustomerType))
                    {
                        DeleteBillingDetails(TransactionID);
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
                }
                    ComputeSummary();
                }
            }
            catch
            {
                MessageBox.Show("Error in Saving Billing.Please check", "Saving Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ComputeOutStanding();
            loadNewTrans(ref isValidDueDate);
            if (!isValidDueDate)
            {
                this.Close();
            }
        }

        private void DeleteBillingDetails(string RefID)
        {
            try
            {
                int refid = cf.GetSysID("tblTransaction", " where TransactionNo='" + RefID + "'");
                string Query = "Delete from tblTransactionDetails where referenceID=" + refid;
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

                    if (!Description.Contains("Unpaid"))
                    {
                        if (!string.IsNullOrEmpty(RefID) || refid != 0)
                        {
                            if (!string.IsNullOrEmpty(Account) || billid != 0)
                            {
                                string Query = "Insert into tblTransactionDetails (ReferenceID,Accounts,Description,Amount,UserID,isActive,Remarks)values (" + refid + "," + billid + ",'" + Description + "'," + Amount + "," + userID + ",1,'" + Remarks + "')";

                                result = cf.ExecuteNonQuery(Query);
                            }
                            else
                            {
                                MessageBox.Show("Error in Account Name. Please check", "Failed to save entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error in Reference ID. Please check", "Failed to save entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
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

        private bool CheckExistTranasactionDueDate(string duedate,string RoomID,string CustID)
        {
            bool result = false;
            try
            {
                if (string.IsNullOrEmpty(CustID))
                {
                    return false;
                }
                string Query = "select * from tbltransaction where duedate = '"+ duedate +"' and roomID = " + RoomID + " " + (string.IsNullOrEmpty(CustID) ? string.Empty:" " + CustID);
                DataTable dtRecords = cf.GetRecords(Query);
                if (dtRecords.Rows.Count > 0)
                {
                    result = true;
                }
            }
            catch
            {
            }
            return result;
        }

        private bool DueDateVsCurrent()
        {
            bool result = false;
            try
            {
                string Query = "Select top 1 CurrentYear,CurrentMonth from tblSettings where isActive = 1 order by sysid desc";
                DataTable dtRecords = cf.GetRecords(Query);

                int CurrentYear = DateTime.Now.Year;
                int CurentMonth = DateTime.Now.Month;

                if (dtRecords.Rows.Count > 0)
                {
                    int DataYear = 0;
                    int DataMonth = 0;
                    
                    if(dtRecords.Rows[0]["CurrentYear"] != null)
                    {
                        if(cf.isIntegerValid(dtRecords.Rows[0]["CurrentYear"].ToString()))
                        {
                            DataYear = int.Parse(dtRecords.Rows[0]["CurrentYear"].ToString());
                        }
                    }

                    if(dtRecords.Rows[0]["CurrentMonth"] != null)
                    {
                        if(cf.isIntegerValid(dtRecords.Rows[0]["CurrentMonth"].ToString()))
                        {
                            DataMonth = int.Parse(dtRecords.Rows[0]["CurrentMonth"].ToString());
                        }
                    }

                    if (CurrentYear != DataYear)
                    {
                        result = false;
                    }
                    else
                    {
                        if (CurentMonth == DataMonth)
                        {
                            result = true;
                        }
                    }
                }
            }
            catch
            {
            }
            return result;
        }

        private void CheckDueDate()
        {
            try
            {
                string duedate = textBox13.Text;
                string CustID = label11.Text != "0" ? " and CustomerID=" + label11.Text: string.Empty;
                CustID = label12.Text != "0" ? " and TenantID=" + label12.Text : CustID;

                if (CheckExistTranasactionDueDate(duedate, comboBox1.Text, CustID))
                {
                    string Query = "select * from tbltransaction where duedate = '" + textBox13.Text + "' and roomID = " + comboBox1.Text + (string.IsNullOrEmpty(CustID) ? string.Empty : CustID);
                    DataTable dtRecords = cf.GetRecords(Query);
                    if (dtRecords.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtRecords.Rows)
                        {
                            textBox1.Text = dr["TransactionNo"].ToString();
                            textBox5.Text = dr["OpeningBalance"].ToString();
                            textBox6.Text = dr["OutstandingBalance"].ToString();
                        }
                    }
                }

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
                    }
                }
                LoadTransactionDetails(textBox13.Text);
                if (!string.IsNullOrEmpty(textBox13.Text))
                {
                    if (dataGridView1.Rows.Count == 0)
                    {
                        InsertMonthlyDue();
                    }
                    if (cf.isIntegerValid(comboBox1.Text))
                    {
                        ComputerPreviousBilling(comboBox1.Text);
                    }
                }
                ComputeOutStanding();
                ComputeSummary();
            }
            catch
            {
            }
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                CheckDueDate();
            }
        }

        private void dataGridView1_Enter(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                CheckDueDate();
                ComputeOutStanding();
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(textBox2.Text))
            {
                ComputeOutStanding();
            }
        }

        private void comboBox4_SelectedValueChanged(object sender, EventArgs e)
        {
            DateTime dtParseStart = DateTime.Parse(comboBox4.Text + "/01/01");
            DateTime dtParseEnd = DateTime.Parse(comboBox4.Text + "/12/30");

            string Query = "select TransactionNo,RoomID,DueDate from tblTransaction where dateTransaction between '" + comboBox4.Text + 
                           "/01/01'" + " and '" + comboBox4.Text + "/12/30" + "' and isActive = 1";

            dvTrans = cf.GetRecords(Query).DefaultView;

            if (dvTrans != null)
            {
            
                dvTrans.RowFilter = " DueDate > #" + dtParseStart.ToString("MM/dd/yyyy") + "# and DueDate < #" + dtParseEnd.ToString("MM/dd/yyyy") + "#";
                dataGridView3.DataSource = dvTrans.ToTable();
                dataGridView3.Columns["DueDate"].Visible = false;
            }
        }

        private void GetPrevRecordViaTransNo(string TransNo)
        {
            try
            {
                string Query = "select t.CustomerID,t.TenantID,t.RoomID,t.OpeningBalance,t.OutStandingBalance,t.DueDate, " + 
                               "case when t.CustomerID > 0 then " + 
                               " isnull(ct.lastname,'') + ',' + isnull(ct.Firstname,'') " +  
                               " else " +
                               " isnull(tt.lastname,'') + ',' + isnull(tt.firstname,'') " +
                               " end as 'Name' " +
                               " from tblTransaction t left join tblTenant tt on t.tenantID = tt.sysid " +
                               " left join tblCustomerTenant ct on t.CustomerID = ct.sysid " +
                               " where t.TransactionNo = '" + TransNo +"' and t.isActive = 1";
                DataTable dtresult = cf.GetRecords(Query);
                textBox2.Text = textBox8.Text = string.Empty;
                if (dtresult.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtresult.Rows)
                    {
                        label11.Text = dr["CustomerID"].ToString();
                        label12.Text = dr["TenantID"].ToString();
                        textBox2.Text = label11.Text != "-1" ? dr["Name"].ToString():string.Empty;
                        textBox8.Text = textBox2.Text == "" ? dr["Name"].ToString() : string.Empty;
                        textBox5.Text = cf.FixMoneyValue(dr["OpeningBalance"].ToString());
                        textBox6.Text = cf.FixMoneyValue(dr["OutStandingBalance"].ToString());
                        textBox13.Text = cf.isDateValid(dr["DueDate"].ToString()) ? DateTime.Parse(dr["DueDate"].ToString()).ToString("yyyy-MM-dd"):string.Empty;
                        customerType = label11.Text != "-1" ? "Owner" : "Tenant";
                        LoadRoom();
                       
                    }
                }
            }
            catch
            {
            }
        }

        private void dataGridView3_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string TransNo = dataGridView3[0, dataGridView3.CurrentCell.RowIndex].Value.ToString(); 
                textBox1.Text = TransNo;
                GetPrevRecordViaTransNo(TransNo);
            }
            catch
            {
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to create a new transaction? Current record will be lost.", "Create New Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                isValidDueDate = false;
                loadNewTrans(ref isValidDueDate);
            }
        }

        private void comboBox5_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime dtParseStart = DateTime.Parse(comboBox4.Text + "/01/01");
                DateTime dtParseEnd = DateTime.Parse(comboBox4.Text + "/12/31");
                dvTrans.RowFilter = " DueDate > #" + dtParseStart.ToString("MM/dd/yyyy") + "# and DueDate < #" + dtParseEnd.ToString("MM/dd/yyyy") + "# " + (string.IsNullOrEmpty(comboBox5.Text) ? string.Empty: "and TransactionNo like '" + comboBox5.Text.Substring(0, 3) + "%'");
                dataGridView3.DataSource = dvTrans.ToTable();
                dataGridView3.Columns["DueDate"].Visible = false;
            }
            catch
            {
            }
        }
    }
}
