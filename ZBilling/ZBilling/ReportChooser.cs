using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZBilling.Class;
using System.IO;

namespace ZBilling
{
    public partial class ReportChooser : Form
    {
        public string DBPath;
        public string LoginUser;

        DataView dvRecords;

        clsFunctiion cf = new clsFunctiion();

        public ReportChooser()
        {
            InitializeComponent();
        }

        private void ReportChooser_Load(object sender, EventArgs e)
        {
            cf.DbLocation = DBPath;
            LoadReport();
        }

        private void LoadReport()
        {
            try
            {
                string Query = "Select ReportName,Description,ExeLocation,isActive from tblReporting where isActive = 1 order by ReportName asc";
                DataTable dtRecords = cf.GetRecords(Query);
                if (dtRecords.Rows.Count > 0)
                {
                    dvRecords = new DataView(dtRecords);
                    dataGridView1.DataSource = dvRecords;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    if (dataGridView1.Columns["ExeLocation"].Visible)
                    {
                        dataGridView1.Columns["ExeLocation"].Visible = false;
                    }
                    dataGridView1.Refresh();
                }
            }
            catch
            {
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string ReportFilter = "";
                dvRecords.RowFilter  = ReportFilter;
            }
            catch
            {
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string ProgramLocation = dataGridView1["ExeLocation", dataGridView1.CurrentRow.Index].Value.ToString();
            if (!string.IsNullOrEmpty(ProgramLocation))
            {
                bool fileExist = File.Exists(ProgramLocation);
                if (!ProgramLocation.Contains(".exe") || !fileExist)
                {
                    MessageBox.Show("Error: Please check your report file. Cannot generate report", "Executable file not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
    }
}
