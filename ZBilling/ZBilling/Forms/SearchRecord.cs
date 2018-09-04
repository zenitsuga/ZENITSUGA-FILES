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
    public partial class SearchRecord : Form
    {
        public string DBPath;

        public object Result;
        public string TableName;
        public string Criteria;
        public string FieldOutput;

        DataView dvResult;
        clsFunctiion cf = new clsFunctiion();

        public SearchRecord()
        {
            InitializeComponent();
        }

        private void LoadRecords()
        {
            try
            {
                string Query = "Select " + FieldOutput + " from " + TableName + Criteria;

                dataGridView1.DataSource = null;
                DataTable dtrecords = new DataTable();
                cf.DbLocation = DBPath;
                dtrecords = cf.GetRecords(Query);
                dataGridView1.DataSource = dtrecords;
                dataGridView1.Refresh();

                comboBox1.Items.Clear();
                foreach (string str in FieldOutput.Split(','))
                {
                    comboBox1.Items.Add(str);
                }
                if (dtrecords.Rows.Count > 0)
                {
                    dvResult = new DataView(dtrecords);
                }
            }
            catch
            {
            }
        }

        private void SearchRecord_Load(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBox1.Text))
            {
                if (dvResult.ToTable().Rows.Count > 0)
                {
                    dvResult.RowFilter = string.Format("CONVERT(" +comboBox1.Text + ",System.String) like '" + textBox1.Text + "%'");
                    
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = dvResult;
                    dataGridView1.Refresh();
                }
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to insert this records?", "Search Found", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    TextBox tb = new TextBox();
                    Result = tb;
                    tb.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                    string FieldOut = "OwnerName + (Case when TenantName != '' then ' \\ ' else '' end) + TenantName as 'CustomerName'";
                    tb.Tag = cf.GetRecordValue(FieldOut, "tblCustomerTenant", tb.Text );
                    this.Close();
                }
            }
        }

        private void selectRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to insert this records?", "Search Found", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    TextBox tb = new TextBox();
                    Result = tb;
                    tb.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                    string FieldOut = "OwnerName + (Case when TenantName != '' then ' \\ ' else '' end) + TenantName as 'CustomerName'";
                    tb.Tag = cf.GetRecordValue(FieldOut, "tblCustomerTenant", tb.Text);
                    this.Close();
                }
            }
        }

        private void loadAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                LoadRecords();
            }
            catch
            {
            }
        }
    }
}
