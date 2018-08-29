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
    public partial class frmLogin : Form
    {
        clsFunctiion cf = new clsFunctiion();

        public string DBPath;
        public bool isSuccessLogin;
        public ToolStripStatusLabel UserLoggedIn;
        public ToolStripStatusLabel UserRole;
        public MenuStrip MSMainMenu;
        public ToolStrip TSMainMenu;

        public frmLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Role = string.Empty;

            //User Verification

            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Error: Please enter your credential correctly", "Invalid User", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Focus();
                return;
            }

            //User Validation
            cf.DbLocation = DBPath;
            if (!string.IsNullOrEmpty(DBPath))
            {
                if (!cf.ValidateUser(textBox1.Text, textBox2.Text, ref Role))
                {
                    MessageBox.Show("Error: Invalid User. Please check", "User Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Error: Database is not properly defined. You cannot use this application. Please check", "DBConnection Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

            //Role Accessing
                int RoleID = cf.GetSysID("tblUserRole"," where Role ='" + Role + "'");
                DataTable dtSetMenuActivation = cf.SetMenu(RoleID);
                ActivateMenuByRole(dtSetMenuActivation,true);
                UserRole.Text = UserRole == null ? "Role: N/A" : "Role: " + Role;
                UserLoggedIn.Text = UserLoggedIn == null ? "" : textBox1.Text;
                isSuccessLogin = true;
            this.Close();
        }

        private void ActivateMenuByRole(DataTable dtRoles,bool status)
        {
            try
            {
                if (dtRoles.Rows.Count > 0)
                {
                  foreach(DataRow dr in dtRoles.Rows)
                  {
                      MenuModule("toolstrip", dr, status);
                      MenuModule("toolbutton", dr, status);
                      MenuModule("toolsplit", dr, status);
                  }
                }
            }
            catch
            {
            }
        }

        private void MenuModule(string MenuType,DataRow dr,bool status)
        {
            try
            {
                if (MenuType == "toolstrip")
                {
                    foreach (ToolStripItem TS_Items in MSMainMenu.Items)
                    {
                        if (TS_Items.Name.Contains(dr[0].ToString()))
                        {
                            TS_Items.Visible = status;
                        }
                    }
                }
                else if (MenuType == "toolbutton" || MenuType == "toolstrip")
                {
                    foreach (var MS_Items in TSMainMenu.Items)
                    {   
                        if (MS_Items.GetType() == typeof(ToolStripButton))
                        {
                          ToolStripButton tsbtn = (ToolStripButton)MS_Items;
                                if (tsbtn.Name.Contains(dr[0].ToString()))
                                {
                                    tsbtn.Visible = status;
                                }
                        }
                        else
                        {
                            ToolStripSplitButton MI = (ToolStripSplitButton)MS_Items;
                            if (MI.Name.Contains(dr[0].ToString()))
                            {
                                MI.Visible = status;
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if (!cf.CheckDatabaseStatus(DBPath))
            {
                MessageBox.Show("Error: Database is not connected. Please check", "DBConnection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isSuccessLogin = false;
                this.Close();
            }

            if (string.IsNullOrEmpty(DBPath))
            {
                MessageBox.Show("Error: Database is not properly defined. You cannot use this application. Please check", "DBConnection Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isSuccessLogin = false;
                this.Close();
            }
        }
    }
}
