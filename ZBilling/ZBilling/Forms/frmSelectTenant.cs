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
    public partial class frmSelectTenant : Form
    {
        public string DBPath;
        clsFunctiion cf = new clsFunctiion();

        public string OwnerID;
        public string SelectedTenantID;

        DataTable dtRecords;
        DataView dvRecords;

        public frmSelectTenant()
        {
            InitializeComponent();
        }

        public DataTable GetRecords()
        {
            DataTable dtResult = new DataTable();
            try
            {
                string Query = "Select sysid,Lastname,FirstName,Middlename from tblTenant where ownerid =" + OwnerID;
                dtResult = cf.GetRecords(Query);
            }
            catch
            {
            }
            return dtResult;
        }

        private void frmSelectTenant_Load(object sender, EventArgs e)
        {
            try
            {
                cf.DbLocation = DBPath;
                dataGridView1.DataSource = GetRecords();
            }
            catch
            {
            }
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if(dataGridView1.Rows.Count > 0)
                {
                    //dataGridView1.CurrentCell = dataGridView1[0, dataGridView1.CurrentCell.RowIndex - 1];
                    dataGridView1.CurrentRow.Selected = true;
                    SelectedTenantID = dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString();
                    this.Close();
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                //dataGridView1.CurrentCell = dataGridView1[0, dataGridView1.CurrentCell.RowIndex - 1];
                dataGridView1.CurrentRow.Selected = true;
                SelectedTenantID = dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString();
                this.Close();
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                //dataGridView1.CurrentCell = dataGridView1[0, dataGridView1.CurrentCell.RowIndex - 1];
                dataGridView1.CurrentRow.Selected = true;
                SelectedTenantID = dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString();
                this.Close();
            }
        }
    }
}
