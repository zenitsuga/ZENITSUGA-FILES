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
    public partial class frmUsers : Form
    {
        clsFunctiion cf = new clsFunctiion();
        public string DBPath;

        public frmUsers()
        {
            InitializeComponent();
        }

        private void LoadSettings()
        {
            try
            {
                cf.DbLocation = DBPath;
                comboBox1.DataSource = cf.LoadUserRole();
                comboBox1.DisplayMember = "Role";
                comboBox1.ValueMember = "sysID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message.ToString(), "Unexpected Error on Load Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmUsers_Load(object sender, EventArgs e)
        {
            LoadSettings();
            LoadRecords();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Error: Invalid User. Please check your entry", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (cf.isRecordExists("tblUsers", " where Username=", "'" + textBox1.Text + "'"))
            {
                UpdateRecords();
            }
            else
            {
                InsertRecords();
            }
            LoadRecords();
        }

        private void DeleteRecords()
        {
            try
            {
                string items = string.Empty;

                items = dataGridView1.SelectedRows[0].Cells["Username"].Value.ToString();

                if (string.IsNullOrEmpty(items))
                {
                    MessageBox.Show("Error: No item to delete. Please select one", "Error in Deleting Record: tblUsers", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    DialogResult dr = MessageBox.Show("Are you sure you want to delete " + items + "?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        cf.DbLocation = DBPath;
                        if (!cf.DeleteRecord("tblUser", " where Username = '" + items + "'"))
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
                MessageBox.Show("Error:" + ex.Message.ToString(), "Unexpected Error on Deleting Record: tblUser", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateRecords()
        {
            try
            {
                cf.DbLocation = DBPath;
                List<string> SV = new List<string>();
                string Criteria = string.Empty;

                SV.Add("Username = '" + textBox1.Text + "'");
                SV.Add("Password = '" + textBox2.Text + "'");
                SV.Add("Role = " + (checkBox1.Checked ? "1" : "0"));
                SV.Add("AllowUserAccess = " + (checkBox2.Checked ? "1" : "0"));

                Criteria = "sysID=" + dataGridView1.SelectedRows[0].Cells["sysID"].Value.ToString();

                if (!cf.UpdateRecords("tblUsers",SV,Criteria))
                {
                    MessageBox.Show("Error: Saving records error. Please check your entry", "Saving Record Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                MessageBox.Show("Done", "Save Record : Users", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Text = string.Empty;

            }
            catch
            {
            }
        }

        private void InsertRecords()
        {
            try
            {
                cf.DbLocation = DBPath;
                List<string> FC = new List<string>();
                FC.Add("Username");
                FC.Add("Password");
                FC.Add("Role");
                FC.Add("isActive");
                FC.Add("AllowUserAccess");
                List<string> FV = new List<string>();
                FV.Add(textBox1.Text);
                FV.Add(textBox1.Text);
                
                List<string> FI = new List<string>();
                FV.Add(comboBox1.ValueMember);
                FI.Add(checkBox1.Checked ? "1" : "0");
                FI.Add(checkBox2.Checked ? "1" : "0");
                if (!cf.InsertRecords("tblUsers", FC, FV, FI))
                {
                    MessageBox.Show("Error: Saving records error. Please check your entry", "Saving Record Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                MessageBox.Show("Done", "Save Record : Users", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message.ToString(), "Unexpected Error on Saving Record: tblUserRole", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadRecords()
        {
            try
            {
                DataTable dtLoadUser = new DataTable();
                cf.DbLocation = DBPath;
                dtLoadUser = cf.RetrieveRecords("tblUsers", "*", " isActive = 1", " Username Asc", "");

                dataGridView1.Refresh();
                if (dtLoadUser.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dtLoadUser;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message.ToString(), "Unexpected Error on Load Record: tblUser", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DeleteRecords();
        }

    }
}
