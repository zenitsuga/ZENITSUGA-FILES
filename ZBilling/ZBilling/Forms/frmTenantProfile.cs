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
    public partial class frmTenantProfile : Form
    {
        clsFunctiion cf = new clsFunctiion();
        public string DBPath;
        public string LoginUser;

        DataView dvRecords;

        public frmTenantProfile()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowOwner();
        }

        private void ShowOwner()
        {
            try{
                SearchRecord sr = new SearchRecord();
                sr.DBPath = DBPath;
                sr.TableName = "tblCustomerTenant";
                sr.Criteria = " where isActive = 1";
                sr.FieldOutput = "sysid,LastName,FirstName,MiddleName,Address,ContactNumber,(LastName + ',' + Firstname ++ ' ' +  MiddleName)as TagOutput";
                sr.ShowDialog();
                if (sr.Result != null)
                {
                    TextBox tb = (TextBox)sr.Result;
                    string sysID = tb.Text;
                    textBox5.Text = tb.Tag.ToString();
                    lblOwnerID.Text = sysID;
                }
            }
            catch{
            }
        }

        private void searchOwnerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOwner();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveTenantInfo();
            textBox1.Focus();
        }
        public void SaveTenantInfo()
        {
            try
            {
                //Validation Check
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("Error: Cannot save Tenant details. Please check your entry", "Saving Tenant Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                cf.DbLocation = DBPath;
                int UserID = cf.GetSysID("tblUsers", " where username='" + LoginUser + "'");
                string SQLStatement = string.Empty;

                if (lblTenantID.Text == "0")
                {
                    SQLStatement = "Insert into tblTenant (LastName,FirstName,MiddleName,Address,ContactNumber,isActive,PrimaryKey,UserID,ownerID)values('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + textBox6.Text + "'," + (checkBox1.Checked == true ? 1 : 0) + ",'" + textBox1.Text + "_" + textBox2.Text + "_" + textBox3.Text + "'," + UserID +"," + lblOwnerID.Text + ")";
                }
                else
                {
                    SQLStatement = "update tblTenant set LastName='" + textBox1.Text + "',FirstName='" + textBox2.Text + "',MiddleName='" + textBox3.Text + "',Address='" + textBox4.Text + "',ContactNumber='" + textBox6.Text + "',isActive=" + (checkBox1.Checked == true ? 1 : 0) + ",PrimaryKey='" + textBox1.Text + "_" + textBox2.Text + "_" + textBox3.Text + "',UserID=" + UserID + ",ownerID =" + lblOwnerID.Text +" where sysID=" + lblTenantID.Text;
                }
                if (cf.ExecuteNonQuery(SQLStatement))
                {
                    MessageBox.Show("Done Customer saving.", "Customer(Tenant) Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearEntry();
                }
                else
                {
                    MessageBox.Show("Error: Cannot save customer Info.", "Customer(Tenant) Saving Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                LoadTenantInfo();
            }
            catch
            {
            }
        }
        public void LoadTenantInfo()
        {
            try
            {
                ClearEntry();
                
                //dataGridView1.DataSource = null;
                string SQLStatement = "Select  t.sysid,case when (isnull(c.Lastname,'') + ',' + isnull(c.Firstname,'')) = ',' " +
                                      "then '' else (isnull(c.Lastname,'') + ',' + isnull(c.Firstname,'')) end as " +                                                               "'Owner',t.Lastname,t.firstname,t.Middlename,t.Address,t.ContactNumber from " +
                                      " tblTenant t left join tblCustomerTenant c on t.OwnerID=c.sysid where t.isActive = 1 " +
                                      " order by LastName,FirstName asc"; 
                cf.DbLocation = DBPath;
                DataTable dtRecords = cf.GetRecords(SQLStatement);
                dvRecords = new DataView(dtRecords);
                dataGridView1.DataSource = dtRecords;
                dataGridView1.Refresh();
            }
            catch
            {
            }
        }
        private void ClearEntry()
        {
            textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = textBox6.Text = string.Empty;
            lblOwnerID.Text = "0";
            lblTenantID.Text = "0";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lblOwnerID.Text != "0")
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to remove this Owner?", "Remove Assigned Owner", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    textBox5.Text = string.Empty;
                    lblOwnerID.Text = "0";
                }
            }
        }

        private void frmTenantProfile_Load(object sender, EventArgs e)
        {
            LoadTenantInfo();
        }
        private void GetOwnerDetails(string Sysid)
        {
            try
            {
                textBox5.Text = string.Empty;
                string Query = "Select (isnull(LastName,'') + ',' + isnull(Firstname,'') + ' ' + isnull(middlename,'')) as ownername from tblCustomerTenant where sysID = " + Sysid;

                textBox5.Text = cf.GetRecords(Query).Rows[0][0].ToString();

            }
            catch
            {
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string sysID = "0";
            sysID = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            lblTenantID.Text = sysID;
            lblOwnerID.Text = dataGridView1.SelectedRows[0].Cells["Owner"].Value.ToString();
            //GetOwnerDetails(sysID);
            textBox5.Text = dataGridView1.SelectedRows[0].Cells["Owner"].Value.ToString();
            textBox1.Text = dataGridView1.SelectedRows[0].Cells["LastName"].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells["FirstName"].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells["MiddleName"].Value.ToString();
            textBox6.Text = dataGridView1.SelectedRows[0].Cells["ContactNumber"].Value.ToString();
            textBox4.Text = dataGridView1.SelectedRows[0].Cells["Address"].Value.ToString();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string sysID = "0";
            sysID = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            lblTenantID.Text = sysID;
            lblOwnerID.Text = dataGridView1.SelectedRows[0].Cells["Owner"].Value.ToString();
            //GetOwnerDetails(lblOwnerID.Text);
            textBox5.Text = dataGridView1.SelectedRows[0].Cells["Owner"].Value.ToString();
            textBox1.Text = dataGridView1.SelectedRows[0].Cells["LastName"].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells["FirstName"].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells["MiddleName"].Value.ToString();
            textBox6.Text = dataGridView1.SelectedRows[0].Cells["ContactNumber"].Value.ToString();
            textBox4.Text = dataGridView1.SelectedRows[0].Cells["Address"].Value.ToString();
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    {
                        var relativeMousePosition = dataGridView1.PointToClient(Cursor.Position);
                        contextMenuStrip1.Show(dataGridView1, relativeMousePosition);
                    }
                    break;
            }
        }

        private void deleteRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr =
                MessageBox.Show("Are you sure you want to delete this records?", "Confirm to delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                RemoveTenant(lblTenantID.Text);
            }
            LoadTenantInfo();
        }
        private void RemoveTenant(string sysID)
        {
            try
            {
                string Query = "Delete from tblTenant where sysid=" + sysID;
                cf.ExecuteNonQuery(Query);
                LoadTenantInfo();
            }
            catch
            {
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ClearEntry();
            textBox1.Focus();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox7.Text != string.Empty)
            {
                if (!string.IsNullOrEmpty(comboBox1.Text))
                {
                    if (dvRecords.ToTable().Rows.Count > 0)
                    {
                        dvRecords.RowFilter = comboBox1.Text + " Like '" + textBox7.Text + "%'";
                    }
                }
            }
            else
            {
                dvRecords.RowFilter = null;
            }
            dataGridView1.DataSource = dvRecords;
        }
    }
}