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

        public IniFile inif;
        string keys = "zbln-3asd-sqoy19";

        public string UserRole;
        public string DBPath;

        DataView dvRecords;

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
            textBox5.Text = cf.GetDefaultUserPassword(inif);
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
            if (cf.isRecordExists("tblUsers", " where Username=", "'" + textBox2.Text + "'"))
            {
                UpdateRecords(false);
            }
            else
            {
                InsertRecords();
            }
            LoadRecords();
            label8.Text = "0";
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
            label8.Text = "0";
        }
        private void UpdateRecords(bool isDefaultPassword)
        {
            try
            {
                cf.DbLocation = DBPath;
                List<string> SV = new List<string>();
                string Criteria = string.Empty;
                
                SV.Add("Username = '" + textBox2.Text + "'");
                if (isDefaultPassword)
                {
                    SV.Add("Password = '" + clsLic.CryptoEngine.Encrypt(textBox5.Text, keys) + "'");
                }
                else
                {
                    SV.Add("Password = '" + clsLic.CryptoEngine.Encrypt(textBox1.Text, keys) + "'");
                }
                SV.Add("Fullname = '" + textBox4.Text + "'");
                //SV.Add("EmailAddress = '" + textBox5.Text + "'");
                SV.Add("Role = " + (comboBox1.Text.ToLower().Contains("admin") ? "1" : "0"));
                SV.Add("AllowAccessUser = " + (checkBox2.Checked ? "1" : "0"));

                Criteria = "where sysID=" + dataGridView1.SelectedRows[0].Cells["sysID"].Value.ToString();

                if (!cf.UpdateRecords("tblUsers",SV,Criteria))
                {
                    MessageBox.Show("Error: Saving records error. Please check your entry", "Saving Record Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                MessageBox.Show("Done", "Save Record : Users", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Text = string.Empty;
                label8.Text = "0";
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
                FC.Add("Fullname");
                //FC.Add("EmailAddress");
                FC.Add("Role");
                FC.Add("isActive");
                FC.Add("AllowAccessUser");
                List<string> FV = new List<string>();
                FV.Add(textBox2.Text);
                FV.Add(clsLic.CryptoEngine.Encrypt(textBox1.Text, keys));
                FV.Add(textBox4.Text);
                //FV.Add(textBox5.Text);
                List<string> FI = new List<string>();
                int UserRole = cf.GetSysID("tblUserRole", " where Role='" + comboBox1.Text + "'");
                FI.Add(UserRole.ToString());
                FI.Add((comboBox1.Text.ToLower().Contains("admin") ? "1" : "0"));
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
                dvRecords = dtLoadUser.DefaultView;
                dataGridView1.Refresh();
                if (dtLoadUser.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dvRecords;
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

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try{
                if (!string.IsNullOrEmpty(textBox3.Text))
                {
                    dvRecords.RowFilter = "Username like '" + textBox3.Text + "%'";
                }
                else
                {
                    dvRecords.RowFilter = null;
                }
                dataGridView1.DataSource = dvRecords;
            }catch
            {
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                label8.Text = dataGridView1["sysid", dataGridView1.CurrentCell.RowIndex].Value.ToString();
                textBox2.Text = dataGridView1["username", dataGridView1.CurrentCell.RowIndex].Value.ToString();

                if (!UserRole.ToString().Contains("admin"))
                {
                    textBox1.PasswordChar = '*';
                }
                else
                {
                    textBox1.PasswordChar = '\0';
                }
                textBox1.Text = clsLic.CryptoEngine.Decrypt(dataGridView1["password", dataGridView1.CurrentCell.RowIndex].Value.ToString(), keys);
                textBox4.Text = dataGridView1["fullname", dataGridView1.CurrentCell.RowIndex].Value.ToString();
                textBox5.Text = cf.GetDefaultUserPassword(inif); //dataGridView1["EmailAddress", dataGridView1.CurrentCell.RowIndex].Value.ToString();
                textBox1.PasswordChar = '*';
            }
            catch
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(textBox5.Text))
            //{
            //    MessageBox.Show("Error: Please provide email address.", "No Email Address Define.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            //if (label8.Text == "0")
            //{
            //    MessageBox.Show("Error: Please choose user to retrieve.", "No Username Define.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            //if (!cf.IsValidEmail(textBox5.Text))
            //{
            //    MessageBox.Show("Error: Please check email address.", "Invalid Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            if (label8.Text != "0")
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to defaul the password on this user:" + textBox2.Text, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    UpdateRecords(true);
                    LoadRecords();
                }
            }
        }

        private void textBox1_MouseHover(object sender, EventArgs e)
        {
            textBox1.PasswordChar = '\0';
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            textBox1.PasswordChar = '*';
        }

    }
}