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
    public partial class FrmFetchRecords : Form
    {
        clsFunctiion cf = new clsFunctiion();

        public string DBLocation;
        public string TableName;
        
        public FrmFetchRecords()
        {
            InitializeComponent();
        }

        private void FrmFetchRecords_Load(object sender, EventArgs e)
        {
            LoadColumn();
            LoadRecords(string.Empty);
        }

        private void LoadColumn()
        {
            string Query = "SELECT Column_Name " +
                            "FROM INFORMATION_SCHEMA.COLUMNS " +
                            "WHERE TABLE_NAME = '" + TableName + "'";
            try
            {
                DataTable dtResult = new DataTable();
                cf.DbLocation = DBLocation;
                dtResult = cf.GetRecords(Query);
                if (dtResult.Rows.Count > 0)
                {
                    comboBox1.DataSource = dtResult;
                    comboBox1.DisplayMember = "Column_Name";
                    comboBox1.Refresh();
                }
            }
            catch
            {
            }
        }

        private void LoadRecords(string Criteria)
        {
            try
            {
                string QueryString = "Select * from " + TableName + (string.IsNullOrEmpty(Criteria) ? "": " where " + Criteria);
                cf.DbLocation = DBLocation;
                dataGridView1.DataSource = cf.GetRecords(QueryString);
                dataGridView1.Refresh();
            }
            catch
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(comboBox1.Text) && !string.IsNullOrEmpty(textBox1.Text))
            {
                LoadRecords(comboBox1.Text + " like '%" + textBox1.Text + "%'");
            }

        }
    }
}
