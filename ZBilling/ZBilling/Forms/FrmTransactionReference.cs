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
    public partial class FrmTransactionReference : Form
    {
        public string DBPath;
        public string LoginUser;

        public string CustomerType;
        public string customerName;
        public string customerID;

        clsFunctiion cf = new clsFunctiion();
        DataTable dtLoadOwner = new DataTable();
        DataTable dtLoadTenant = new DataTable();

        DataView dvOwner;
        DataView dvTenant;
        
        public FrmTransactionReference()
        {
            InitializeComponent();
        }

        private void FrmTransactionReference_Load(object sender, EventArgs e)
        {
            try
            {
                string TableName = "tblCustomerTenant";
                string Query = "select sysid,lastname,firstname,middlename,address from " + TableName + " where isActive =1 order by lastname asc";
                cf.DbLocation = DBPath;
                dtLoadOwner = cf.GetRecords(Query);
                dvOwner = new DataView(dtLoadOwner);
                TableName = "tblTenant";
                Query = "select sysid,lastname,firstname,middlename,address from " + TableName + " where isActive =1 order by lastname asc";
                dtLoadTenant = cf.GetRecords(Query);
                dvTenant = new DataView(dtLoadTenant);
            }
            catch
            {
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {   
            if (radioButton1.Checked)
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dvOwner.ToTable();
                dataGridView1.Refresh();

                if (dataGridView1.Rows.Count == 0)
                {
                    string TableName = "tblCustomerTenant";
                    string Query = "select sysid,lastname,firstname,middlename,address from " + TableName + " where isActive =1 order by lastname asc";
                    cf.DbLocation = DBPath;
                    dtLoadOwner = cf.GetRecords(Query);

                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = dtLoadOwner;
                    dataGridView1.Refresh();
                }
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dvTenant.ToTable();
                dataGridView1.Refresh();

                if (dataGridView1.Rows.Count == 0)
                {
                    string TableName = "tblTenant";
                    string Query = "select sysid,lastname,firstname,middlename,address from " + TableName + " where isActive =1 order by lastname asc";
                    cf.DbLocation = DBPath;
                    dtLoadTenant = cf.GetRecords(Query);

                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = dtLoadTenant;
                    dataGridView1.Refresh();
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataView dvFilter = new DataView();
                if (radioButton1.Checked)
                {
                    dvFilter = dvOwner;
                }
                else if (radioButton2.Checked)
                {
                    dvFilter = dvTenant;
                }
                string Filter = "Lastname like '" + textBox1.Text + "%'";
                dvFilter.RowFilter = Filter;
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dvFilter.ToTable();
                dataGridView1.Refresh();
            }
            catch
            {
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            customerName = dataGridView1["Lastname",e.RowIndex].Value.ToString() + "," + dataGridView1["Firstname",e.RowIndex].Value.ToString();

            DialogResult dr = MessageBox.Show("Are you sure you want to insert this customer?", "Transaction for " + customerName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                CustomerType = radioButton1.Checked ? "Owner" : "Tenant";
                customerID = dataGridView1["sysid", e.RowIndex].Value.ToString();
            }
            this.Close();
        }
    }
}
