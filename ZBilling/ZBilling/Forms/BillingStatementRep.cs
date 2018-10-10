using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Data.SqlClient;
using ZBilling.Class;

namespace ZBilling.Forms
{
    public partial class BillingStatementRep : Form
    {
        public string DBPath;

        Database db = new Database();

        public DataTable dtRecords;

        public BillingStatementRep()
        {
            InitializeComponent();
        }

        private void BillingStatementRep_Load(object sender, EventArgs e)
        {
            db.DBPath = DBPath;
            DataSet dsTrans = new DataSet();
            dsTrans.Tables.Add(dtRecords);
            DataSet1 DSTransaction = (DataSet1)dsTrans;
            ReportDataSource datasource = new ReportDataSource("Transaction", DSTransaction.Tables[0]);
            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(datasource);
            this.reportViewer1.RefreshReport();
        }
        private DataSet1 GetData()
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                SqlCommand cmd = new SqlCommand("select * from transactiondetails");
                cmd.Connection = new SqlConnection(db.GetConnString(DBPath));
                sda.SelectCommand = cmd;
                using (DataSet1 dsCustomers = new DataSet1())
                {
                    sda.Fill(dsCustomers, "DataTable1");
                    return dsCustomers;
                }
            }
        }
    }
}
