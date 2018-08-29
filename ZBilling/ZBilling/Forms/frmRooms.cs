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
    public partial class frmRooms : Form
    {
        clsFunctiion cf = new clsFunctiion();

        public string DBPath;
        public string LoginUser;

        public frmRooms()
        {
            InitializeComponent();
        }

        private void frmRooms_Load(object sender, EventArgs e)
        {
            GetRoomType();
            LoadRecords();
        }
        private void GetRoomType()
        {
            try
            {
                cf.DbLocation = DBPath;
                string Query = "Select Distinct RoomType from tblrooms where isActive = 1 order by RoomType asc";
                comboBox1.DataSource = cf.GetRecords(Query);
                comboBox1.DisplayMember = "RoomType";
                comboBox1.ValueMember = "RoomType";
            }catch
            {
            }
        }
        private void LoadRecords()
        {
            try
            {
                cf.DbLocation = DBPath;
                string Query = "Select r.sysId as 'ID',r.RoomType,r.RoomName,u.Username,r.Description,r.SizeSQM as 'Size(SQM)' from tblRooms r left join tblUsers u on r.UserID = u.sysID where r.isActive = 1";
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = cf.GetRecords(Query);
                dataGridView1.Refresh();
            }
            catch
            {
            }
        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {
            GetRoomType();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox1.Text) || string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Error: Cannot save entry. Please check your details.", "Invalid Room Details", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }

            try
            {
                int UID = cf.GetSysID("tblUsers"," where Username = '"+ LoginUser +"'");
                int isActive = checkBox1.Checked ? 1:2;
                string Query = "Insert into tblRooms(RoomType,RoomName,UserID,Description,SizeSQM,isActive)values('" + comboBox1.Text + "','" + textBox1.Text +"'," + UID + ",'" + textBox2.Text +"','" + textBox3.Text + "',"+ isActive +")";

                if (cf.ExecuteNonQuery(Query))
                {
                    MessageBox.Show("Record Saved.", "Room saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error: Cannot save entry. Please check your details.", "Invalid Room Details", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                LoadRecords();
            }catch
            {
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
               DialogResult dr =  MessageBox.Show("Are you sure you want to delete these items?", "Delete Items", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
               if (dr == DialogResult.Yes)
               {
                   foreach (DataGridViewRow dgr in dataGridView1.SelectedRows)
                   {
                       string sysid = dgr.Cells[0].Value.ToString();
                       string Query = "Delete from tblRooms where sysid=" + sysid;
                       if (cf.ExecuteNonQuery(Query))
                       {
                           MessageBox.Show("Done deleting records", "Delete successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                       }
                       else
                       {
                           MessageBox.Show("Error deleting records", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                       }
                   }
                   LoadRecords();
               }
            }
            else
            {
                MessageBox.Show("Error: Cannot delete entry. Please select from the list.", "No Room to delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.Text == string.Empty)
            {
                textBox3.Text = "0";
            }
            else
            {
                if (!cf.isRoomSizeValid(textBox3.Text))
                {
                    MessageBox.Show("Error: Invalid Room size", "Room Size Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox3.Focus();
                    return;
                }
            }
        }
        
    }
}
