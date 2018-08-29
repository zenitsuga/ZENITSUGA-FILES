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
        
        public frmCustomerProfile()
        {
            InitializeComponent();
        }

        private void LoadRoomAssignment(string CustomerID)
        {
            try
            {
                dataGridView1.DataSource = null;
                string SQLStatement = "Select * from tblRoomAssignment where CustomerID = '" + CustomerID + "'";
                cf.DbLocation = DBPath;
                dataGridView1.DataSource = cf.GetRecords(SQLStatement);
                dataGridView1.Refresh();
            }
            catch
            {
            }
        }

        public void LoadCustomerInfo()
        {
            try
            {
                dataGridView1.DataSource = null;
                string SQLStatement = "Select * from tblCustomerTenant where isActive = 1 order by OwnerName,TenantName asc"; cf.DbLocation = DBPath;
                dataGridView2.DataSource = cf.GetRecords(SQLStatement);
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
                string SQLStatement = "Insert into tblCustomerTenant (OwnerName,TenantName,Address,ContactNumber,isActive,PrimaryKey,UserID)values('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" +textBox4.Text + "'," + (checkBox1.Checked == true ? 1:0) + ",'" +textBox1.Text + "_" +textBox2.Text + "',"+ UserID +")";

                if (cf.ExecuteNonQuery(SQLStatement))
                {
                    MessageBox.Show("Done Customer saving.", "Customer Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error: Cannot save customer Info.", "Customer Saving Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                LoadCustomerInfo();
            }
            catch
            {
            }
        }

        public void SaveRoomAssignment(string CustID)
        {
            try
            {
                cf.DbLocation = DBPath;
                int UserID = cf.GetSysID("tblUsers", " where username='" + LoginUser + "'");
                string SQLStatement = "Insert into tblRoomAssignment(RoomNumber,CustomerID,DateTransferred,Remarks,UserID)values(" + comboBox2.Text + "," + CustID + ",'" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" + textBox5.Text + "',"+ UserID +")";
                if (cf.ExecuteNonQuery(SQLStatement))
                {
                    MessageBox.Show("Done Room assigning.", "Room assigning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecords();
                }
                else
                {
                    MessageBox.Show("Error: Cannot assign room", "Room Assignment Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                LoadRoomAssignment(lblIDNumber.Text);
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

                dtRooms = dtRoomInfo();
                comboBox1.DataSource = GetRoomType(dtRooms);
                comboBox1.DisplayMember = "RoomType";
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
            if (lblIDNumber.Text == "0")
            {
                MessageBox.Show("Error: Cannot assign room. Please save first your Customer Info.", "Customer Info not exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBox1.Text == string.Empty || comboBox2.Text == string.Empty)
            {
                MessageBox.Show("Error: Cannot assign room. Please save first your Customer Info.", "Invalid Room Details", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SaveRoomAssignment(lblIDNumber.Text);     
        }

        private DataTable GetRoomNumber(string Roomtype)
        {
            DataTable dtResult = new DataTable();
            try
            {
                dtRooms = dtRoomInfo();
                DataView dv = dtRooms.DefaultView;
                dv.RowFilter = "RoomType='" + Roomtype + "'";
                dtResult = dv.ToTable();
            }
            catch
            {
            }
            return dtResult;
        }

        private DataTable GetRoomType(DataTable dtRooms)
        {
            DataTable dtResult = new DataTable();
            try
            {
                DataView dv = dtRooms.DefaultView;
                dtResult = dv.ToTable(true, "RoomType");
            }
            catch
            {
            }
            return dtResult;
        }

        private DataTable dtRoomInfo()
         {
            DataTable dtResult = new DataTable();
            try
            {
                cf.DbLocation = DBPath;
                string SQLStatement = "Select * from tblRooms where isActive = 1";
                dtResult = cf.GetRecords(SQLStatement);
            }
            catch
            {
            }
            return dtResult;
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
            comboBox2.DataSource = GetRoomNumber(comboBox1.Text);
            comboBox2.DisplayMember = "RoomName";
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dgr in dataGridView2.SelectedRows)
            {
                lblIDNumber.Text = dgr.Cells[0].Value.ToString();
                textBox1.Text = dgr.Cells[1].Value.ToString();
                textBox2.Text = dgr.Cells[2].Value.ToString();
                textBox3.Text = dgr.Cells[3].Value.ToString();
                textBox4.Text = dgr.Cells[4].Value.ToString();
                LoadRoomAssignment(lblIDNumber.Text);
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
        
    }
}
