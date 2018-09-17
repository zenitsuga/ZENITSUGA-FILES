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
    public partial class frmCustomerProfile : Form
    {
        public string DBPath;
        public string LoginUser;

        clsFunctiion cf = new clsFunctiion();

        DataTable dtRooms = new DataTable();
        DataView dvCustomer;

        DataTable dtRecords = new DataTable();
        DataView dvRecords;
        
        public frmCustomerProfile()
        {
            InitializeComponent();
        }


        private void clearEntry()
        {
            textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = textBox6.Text = string.Empty;
        }

        public void LoadCustomerInfo()
        {
            try
            {
                clearEntry();
                //dataGridView1.DataSource = null;
                string SQLStatement = "Select * from tblCustomerTenant where isActive = 1 order by LastName,FirstName asc"; cf.DbLocation = DBPath;
                DataTable dtRecords = cf.GetRecords(SQLStatement);
                dataGridView2.DataSource = dtRecords;
                dvCustomer = new DataView(dtRecords);
                dataGridView2.Refresh();
            }
            catch
            {
            }
        }

        public void SaveCustomerInfo()
        {
            try
            {
                cf.DbLocation = DBPath;
                int UserID = cf.GetSysID("tblUsers", " where username='" + LoginUser + "'");
                string SQLStatement = string.Empty;

                if (lblIDNumber.Text == "0")
                {
                    SQLStatement = "Insert into tblCustomerTenant (LastName,FirstName,MiddleName,Address,ContactNumber,isActive,PrimaryKey,UserID)values('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox4.Text + "','" + textBox3.Text + "','" + textBox6.Text + "'," + (checkBox1.Checked == true ? 1 : 0) + ",'" + textBox1.Text + "_" + textBox2.Text + "_" + textBox4.Text + "'," + UserID + ")";
                }
                else
                {
                    SQLStatement = "update tblCustomerTenant set LastName='" + textBox1.Text + "',FirstName='" + textBox2.Text + "',MiddleName='" + textBox4.Text + "',Address='" + textBox3.Text + "',ContactNumber='" + textBox6.Text + "',isActive="+ (checkBox1.Checked == true ? 1 : 0) + ",PrimaryKey='" + textBox1.Text + "_" + textBox2.Text + "_" + textBox4.Text + "',UserID=" +UserID+ " where sysID=" + lblIDNumber.Text;
                }
                if (cf.ExecuteNonQuery(SQLStatement))
                {
                    MessageBox.Show("Done Customer saving.", "Customer(Owner) Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error: Cannot save customer Info.", "Customer(Owner) Saving Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                LoadCustomerInfo();
            }
            catch
            {
            }
        }

        

        private void LoadRecords()
        {
            try
            {
                lblIDNumber.Text = "0";
                textBox1.Text = string.Empty;
                textBox2.Text = string.Empty;
                textBox3.Text = string.Empty;
                textBox4.Text = string.Empty;

                //dtRooms = dtRoomInfo();
                //comboBox1.DataSource = GetRoomType(dtRooms);
                //comboBox1.DisplayMember = "RoomType";
                LoadCustomerInfo();
            }
            catch
            {
            }
        }

        private void frmCustomerProfile_Load(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //if (lblIDNumber.Text == "0")
            //{
            //    MessageBox.Show("Error: Cannot assign room. Please save first your Customer Info.", "Customer Info not exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            //if (comboBox1.Text == string.Empty || comboBox2.Text == string.Empty)
            //{
            //    MessageBox.Show("Error: Cannot assign room. Please save first your Customer Info.", "Invalid Room Details", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            //SaveRoomAssignment(lblIDNumber.Text);     
        }

        private int GetCustomerID(string PrimaryKey)
        {
            int result = 0;
            try
            {
                cf.DbLocation = DBPath;
                string Query = "Select sysid from tblCustomerTenant where primaryKey='" + PrimaryKey + "'";
                DataTable dtResult = cf.GetRecords(Query);
                foreach (DataRow dr in dtResult.Rows)
                {
                    result = int.Parse(dr[0].ToString());
                    break;
                }
            }
            catch
            {
            }
            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty && textBox2.Text == string.Empty)
            {
                MessageBox.Show("Error: Please check your Customer Name. Please provided", "Invalid Customer name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBox1.Text == string.Empty)
            {
                MessageBox.Show("Error: Please check your Customer Name. Please provided Owner Name", "Invalid Customer name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SaveCustomerInfo();
            lblIDNumber.Text = GetCustomerID(textBox1.Text + "_" + textBox2.Text).ToString();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            //comboBox2.DataSource = GetRoomNumber(comboBox1.Text);
            //comboBox2.DisplayMember = "RoomName";
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dgr in dataGridView2.SelectedRows)
            {
                lblIDNumber.Text = dgr.Cells[0].Value.ToString();
                textBox1.Text = dgr.Cells[1].Value.ToString();
                textBox2.Text = dgr.Cells[2].Value.ToString();
                textBox4.Text = dgr.Cells[3].Value.ToString();
                textBox3.Text = dgr.Cells[4].Value.ToString();
                textBox6.Text = dgr.Cells[5].Value.ToString();
                //LoadRoomAssignment(lblIDNumber.Text);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                frmRooms rm = new frmRooms();
                rm.MdiParent = Form1.ActiveForm;
                rm.DBPath = DBPath;
                rm.LoginUser = LoginUser;
                rm.Show();

                LoadRecords();
            }
            catch
            {
            }
            
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (dvCustomer.ToTable().Rows.Count > 0)
            {
                dvCustomer.RowFilter = "LastName LIKE '" + textBox5.Text + "%'";
                if (dvCustomer.ToTable().Rows.Count == 0)
                {
                    dvCustomer.RowFilter = "FirstName LIKE '" + textBox5.Text + "%'";
                    if (dvCustomer.ToTable().Rows.Count == 0)
                    {
                        dvCustomer.RowFilter = "MiddleName LIKE '" + textBox5.Text + "%'";
                    }
                }
                dataGridView2.DataSource = null;
                dataGridView2.DataSource = dvCustomer;
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar))
            {
                MessageBox.Show("Please enter a numeric value.", "Allow numbers only", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox6.Text = string.Empty;
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            try
            {
                dvRecords.RowFilter = comboBox1.Text + " LiKE '" + textBox7.Text + "%'";
                //string criteria = (!string.IsNullOrEmpty(comboBox1.Text) || !string.IsNullOrEmpty(comboBox1.Text)) ? comboBox1.Text + " LIKE '" + textBox7.Text + "%' and " : string.Empty;
                //string Query = "Select (Case when OwnerID is NULL then FALSE else TRUE end) as HasOwner,LastName,FirstName,MiddleName,Address,ContactNumber from tblTenant" + criteria + " OwnerID = " + lblIDNumber.Text;
                //DataTable dtResult = cf.GetRecords(Query);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dvRecords.ToTable();
                dataGridView1.Refresh();

                if (dataGridView1.Rows.Count == 0)
                {
                    LoadTenant(lblIDNumber.Text);
                }
            }catch  
            {

            }
            
        }

        private void lblIDNumber_TextChanged(object sender, EventArgs e)
        {
            LoadTenant(lblIDNumber.Text);
        }
        private void LoadTenant(string SysID)
        {
            try
            {
                //string Query = "SELECT * FROM tblTenant where OwnerID = " + SysID + " order by lastname asc";
                string Query = "Select  t.sysid,case when (isnull(c.Lastname,'') + ',' + isnull(c.Firstname,'')) = '' " +
                               " then '' else (isnull(c.Lastname,'') + ',' + isnull(c.Firstname,'')) end as 'Owner', " +
                               " t.Lastname,t.firstname,t.Middlename,t.Address,t.ContactNumber from tblTenant t  " +
                               " left join tblCustomerTenant c on t.OwnerID=c.sysid where t.isActive = 1 and t.OwnerID="                                + SysID + " order by LastName,FirstName asc";
                dtRecords = cf.GetRecords(Query);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dtRecords;
                dvRecords = new DataView(dtRecords);
                dataGridView1.Refresh();
            }
            catch
            {
            }
        }

        private void deleteRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr =
                MessageBox.Show("Are you sure you want to delete this records?","Confirm to delete",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if(dr == DialogResult.Yes)
            {
                //Check tenant records
                if (dataGridView1.Rows.Count > 0)
                {
                    MessageBox.Show("Error: You cannnot delete this record. There still tenant attached on this owner.", "Delete tenant first.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                RemoveOwner(lblIDNumber.Text);
            }
        }

        private void RemoveOwner(string sysID)
        {
            try
            {
                string Query = "Delete from tblCustomerTenant where sysid=" + sysID;
                cf.ExecuteNonQuery(Query);
                LoadRecords();
            }
            catch
            {
            }
        }

        private void dataGridView2_MouseDown(object sender, MouseEventArgs e)
        {
            if (dataGridView2.Rows.Count > 0)
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
        }
    }
}
