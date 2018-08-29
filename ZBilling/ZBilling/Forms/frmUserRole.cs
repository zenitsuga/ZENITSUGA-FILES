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
    public partial class frmUserRole : Form
    {
        clsFunctiion cf = new clsFunctiion();
        public string DBPath;

        public frmUserRole()
        {
            InitializeComponent();
        }

        private void frmUserRole_Load(object sender, EventArgs e)
        {
            LoadRecords();
        }
        private void DeleteRecords()
        {
            try
            {
                string items = string.Empty;

                items = dataGridView1.SelectedRows[0].Cells["Role"].Value.ToString();

                if(string.IsNullOrEmpty(items))
                {
                    MessageBox.Show("Error: No item to delete. Please select one", "Error in Deleting Record: tblUserRole", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }else{
                    DialogResult dr = MessageBox.Show("Are you sure you want to delete " + items + "?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        cf.DbLocation = DBPath;
                        if (!cf.DeleteRecord("tblUserRole", " where Role = '" + items + "'"))
                        {
                                MessageBox.Show("Error: Deleting records error. Please check your entry", "Delete Record Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        MessageBox.Show("Done", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBox1.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message.ToString(), "Unexpected Error on Deleting Record: tblUserRole", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void InsertRecords()
        {
            try
            {
                cf.DbLocation = DBPath;
                List<string>FC = new List<string>();
                FC.Add("Role");
                FC.Add("isActive");
                List<string>FV = new List<string>();
                FV.Add(textBox1.Text);
                List<string> FI = new List<string>();
                FI.Add(checkBox1.Checked ? "1" : "0");
                if (!cf.InsertRecords("tblUserRole", FC, FV,FI))
                {
                    MessageBox.Show("Error: Saving records error. Please check your entry", "Saving Record Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                MessageBox.Show("Done", "Save Record : UserRole", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Text = string.Empty;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message.ToString(), "Unexpected Error on Saving Record: tblUserRole", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadRecords()
        {
            try
            {
                DataTable dtLoadUserRole = new DataTable();
                cf.DbLocation = DBPath;
                dtLoadUserRole = cf.RetrieveRecords("tblUserRole", "*", " isActive = 1", " Role Asc", "");

                dataGridView1.Refresh();
                if (dtLoadUserRole.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dtLoadUserRole;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }catch(Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message.ToString(), "Unexpected Error on Load Record: tblUserRole", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Error: Cannot add blank field. Please check your entry.", "Invalid Entry : User Role", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            InsertRecords();
            LoadRecords();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DeleteRecords();
            LoadRecords();
        }
    }
}
