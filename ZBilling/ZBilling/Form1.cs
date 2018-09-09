using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZBilling.Forms;
using System.IO;
using ZBilling.Class;
namespace ZBilling
{
    public partial class Form1 : Form
    {
        clsFunctiion cf = new clsFunctiion();
        public bool isDatabaseConnected;
        bool isValidLicense = false;
        bool isSuccessLogin;
        int roleUser = 0;
        
        string IniFileSettings;

        string DatabasePath;

        int daysremain = 0;

        IniFile iniF;

        public string CompanyName;

        public Form1()
        {
            InitializeComponent();
        }

        public void CheckCompanyName()
        {
            try
            {
                CompanyName = iniF.Read("CompanyName", "CompanyProfile");
                if (!string.IsNullOrEmpty(CompanyName))
                    this.Text = this.Text.Replace("[CompanyName]", CompanyName);
            }
            catch
            {

            }
        }

        public void ReadIniFile(string IniPath)
        {
            iniF = new IniFile(IniPath);
            try
            {
                iniF = new IniFile(IniFileSettings);
                CheckCompanyName();
                //DatabasePath = cf.IsConnected(IniFileSettings); //iniF.Read("DatabaseLocation", "Database");
                string ErrMsg = string.Empty;
                isDatabaseConnected = cf.IsConnected(IniFileSettings, ref ErrMsg);
                if (!string.IsNullOrEmpty(ErrMsg))
                {
                    MessageBox.Show("Error: Please check your database setting.", "Database cannot connect", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string LicenseCode = iniF.Read("LicenseCode","License");
                
                isValidLicense = cf.ValidateLicense(LicenseCode, ref daysremain);

                if (!isValidLicense)
                {
                    DialogResult dr = MessageBox.Show("Error: Invalid License please provide. Would you like to open your settings?", "Invalid License", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                    if (dr == DialogResult.Yes)
                    {
                        Settings setting = new Settings();
                        setting.ShowDialog();
                        IniFileSettings = setting.IniFileSettings;                        
                        isDatabaseConnected = setting.isDatabaseConnected;                        
                    }
                    else
                    {
                        Application.Exit();
                        return;
                    }
                    
                }
                

                if (daysremain <= 30 && daysremain != 0)
                {
                    DialogResult dr = MessageBox.Show("Error: You have only ("+ daysremain + ") days to use the application. Would you like to continue?", "Invalid License", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                    if (dr == DialogResult.No)
                    {
                        Application.Exit();
                        return;
                    }

                }
                else if (daysremain == 0)
                {
                    DialogResult dr = MessageBox.Show("Error: You have only (" + daysremain + ") days to use the application. Would you like to enter a new license?", "Invalid License", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (dr == DialogResult.No)
                    {
                        Application.Exit();
                        return;
                    }
                }

                isValidLicense = true;
            }
            catch
            {
            }   
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings sets = new Settings();
            sets.MdiParent = this;
            sets.Show();
            IniFileSettings = sets.IniFileSettings;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to close the application?", "Confirm to exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckAndLoadSettings())
            {
                if (loginToolStripMenuItem.Text == "Login" || loginToolStripMenuItem.Text == "&Login")
                {
                    frmLogin fl = new frmLogin();
                    fl.MdiParent = this;
                    fl.DBPath = DatabasePath;
                    fl.MSMainMenu = menuStrip1;
                    fl.TSMainMenu = toolStrip1;
                    fl.Show();
                    fl.UserLoggedIn = tssUserlogin;
                    fl.UserRole = tssUserRole;
                    isSuccessLogin = fl.isSuccessLogin;
                    ShowHideMenu();
                }
                else
                {
                    DialogResult dr = MessageBox.Show("Are you sure you want to logout?", "Confirm logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        isSuccessLogin = false;
                        tssUserlogin.Text = "Unknown";
                        ShowHideMenu();
                        tssUserRole.Text = "Role: N/A";
                        toolStripButton1.Text = "Login";
                        loginToolStripMenuItem.Text = "Login";
                    }
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (CheckAndLoadSettings())
            {
                if (toolStripButton1.Text == "Login" || toolStripButton1.Text == "&Login")
                {
                    frmLogin fl = new frmLogin();
                    fl.MdiParent = this;
                    fl.DBPath = DatabasePath;
                    fl.MSMainMenu = menuStrip1;
                    fl.TSMainMenu = toolStrip1;
                    fl.Show();
                    fl.UserLoggedIn = tssUserlogin;
                    fl.UserRole = tssUserRole;
                    isSuccessLogin = fl.isSuccessLogin;
                    ShowHideMenu();
                }
                else
                {
                    DialogResult dr = MessageBox.Show("Are you sure you want to logout?", "Confirm logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        isSuccessLogin = false;
                        tssUserlogin.Text = "Unknown";
                        ShowHideMenu(); 
                        tssUserRole.Text = "Role: N/A";
                        toolStripButton1.Text = "Login";
                        loginToolStripMenuItem.Text = "Login";
                    }
                }
            }
        }

        private void ShowHideMenu()
        {
            try
            {
                string GetRole = tssUserRole.Text.Replace("Role:","").Trim();
                cf.DbLocation = DatabasePath;
                int RoleID = cf.GetSysID("tblUserRole", " where Role ='" + GetRole + "'");
                DataTable dtSetMenuActivation = cf.SetMenu(RoleID);
                ActivateMenuByRole(dtSetMenuActivation, false);
            }
            catch
            {
            }
        }

        private void ActivateMenuByRole(DataTable dtRoles, bool status)
        {
            try
            {
                if (dtRoles.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtRoles.Rows)
                    {
                        foreach (ToolStripItem TS_Items in toolStrip1.Items)
                        {
                            if (TS_Items.Name.Contains(dr[0].ToString()))
                            {
                                TS_Items.Visible = status;
                            }
                        }
                        foreach (ToolStripMenuItem MS_Items in menuStrip1.Items)
                        {  
                            if (MS_Items.Name.Contains(dr[0].ToString()))
                            {
                                MS_Items.Visible = status;
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Settings sets = new Settings();
            sets.MdiParent = this;
            sets.Show();
            IniFileSettings = sets.IniFileSettings;
            isDatabaseConnected = sets.isDatabaseConnected;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ReadSettings(int ReadCounter)
        {
            try
            {
                string IniPath = Environment.CurrentDirectory + "\\ResourceFolder\\DefaultSettings.txt";
                if (File.Exists(IniPath))
                {
                    IniFileSettings = File.ReadAllText(IniPath);
                    if (string.IsNullOrEmpty(IniFileSettings))
                    {
                        if (ReadCounter == 0)
                        {
                            DialogResult dr = MessageBox.Show("File setting location not yet defined. Would you like to set it first?", "DefaultSettings.txt is empty", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                            if (dr == DialogResult.Yes)
                            {
                                Settings setting = new Settings();
                                setting.ShowDialog(); 
                                IniFileSettings = setting.IniFileSettings;
                                CheckCompanyName();
                                isDatabaseConnected = setting.isDatabaseConnected;
                            }
                        }
                        else
                        {
                            Settings setting = new Settings();
                            setting.ShowDialog();
                            IniFileSettings = setting.IniFileSettings;
                                              CheckCompanyName();
                            isDatabaseConnected = setting.isDatabaseConnected;
                        }

                        if (isDatabaseConnected)
                        {
                            IniPath = IniFileSettings;
                        }
                    }
                    

                    //if (File.Exists(IniPath))
                    //{
                    //    File.WriteAllText(IniPath, string.Empty, Encoding.UTF8);
                    //    File.AppendAllText(IniPath, IniFileSettings, Encoding.UTF8);
                    //}
                    ReadIniFile(IniPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Writing Ini Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tmrToday.Enabled = true;
            int readcounter = 1;
                    ReadSettings(readcounter);
                    if (!string.IsNullOrEmpty(IniFileSettings))
                    {
                        //ReadIniFile(IniFileSettings);
                    }
                    else
                    {
                        MessageBox.Show("Error: Check your Application Settings.", "Setting is Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        readcounter++; ;
                }
                    CheckAndLoadSettings();
        }
        private bool CheckAndLoadSettings()
        {
            bool result = false;

            if (string.IsNullOrEmpty(IniFileSettings))
            {
                MessageBox.Show("Error: Check your Application Settings.", "Setting is Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Settings setting = new Settings();                
                setting.ShowDialog();
                IniFileSettings = setting.IniFileSettings;
                isDatabaseConnected = setting.isDatabaseConnected;
                result = false;
            }
            else
            {
                DatabasePath = IniFileSettings;
                result = true;
            }
            return result;
        }

        private void tmrToday_Tick(object sender, EventArgs e)
        {
            tssToday.Text = "Today is " + DateTime.Now.ToString("MMMMM dd, yyyy hh:mm:ss");
            tssDatabaseStatus.Text = isDatabaseConnected ? "Connected" : "Disconnected";
            tssLicenseStatus.Text = isValidLicense ? "Valid" : "Invalid";
            if(daysremain < 30)
            tssLicenseStatus.ToolTipText = "Valid until " + DateTime.Now.AddDays(double.Parse(daysremain.ToString()));
        }

        private void tssUserlogin_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tssUserlogin.Text))
            {
                toolStripButton1.Text = "Login";
                loginToolStripMenuItem.Text = "Login";
            }
            else
            {
                toolStripButton1.Text = "Logout";
                loginToolStripMenuItem.Text = "Logout";
            }
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckAndLoadSettings())
            {
                frmUserRole fur = new frmUserRole();
                fur.MdiParent = this;
                fur.Show();
            }
        }

        private void userRoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckAndLoadSettings())
            {
                frmUserRole fur = new frmUserRole();
                fur.MdiParent = this;
                fur.DBPath = DatabasePath;
                fur.Show();
            }
        }

        private void usersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (CheckAndLoadSettings())
            {
                frmUsers fu = new frmUsers();
                fu.MdiParent = this;
                fu.DBPath = DatabasePath;
                fu.Show();
            }
        }

        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckAndLoadSettings())
            {
                frmUsers fu = new frmUsers();
                fu.MdiParent = this;
                fu.DBPath = DatabasePath;
                fu.Show();
            }
        }

        private void LoadButtonPerUser()
        {
 
        }

        private void tssTransaction_Click(object sender, EventArgs e)
        {
            if (CheckAndLoadSettings())
            {
                Transactions trans = new Transactions();
                trans.MdiParent = this;
                trans.DBPath = DatabasePath;
                trans.Userlogin = tssUserlogin.Text;
                trans.Show();
            }
        }

        private void createTransactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckAndLoadSettings())
            {
                Transactions trans = new Transactions();
                trans.MdiParent = this;
                trans.DBPath = DatabasePath;
                trans.Show();
            }
        }

        private void chartOfAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckAndLoadSettings())
            {
                frmChartOfAccount COA = new frmChartOfAccount();
                COA.MdiParent = this;
                COA.DBPath = DatabasePath;
                COA.LoginUser = tssUserlogin.Text;
                COA.Show();
            }
        }

        private void customerProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckAndLoadSettings())
            {
                frmCustomerProfile cp = new frmCustomerProfile();
                cp.MdiParent = this;
                cp.DBPath = DatabasePath;
                cp.LoginUser = tssUserlogin.Text;
                cp.Show();
            }
        }

        private void roomsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckAndLoadSettings())
            {
                frmRooms rm = new frmRooms();
                rm.MdiParent = this;
                rm.DBPath = DatabasePath;
                rm.LoginUser = tssUserlogin.Text;
                rm.Show();
            }
        }

        private void roomsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (CheckAndLoadSettings())
            {
                frmRooms rm = new frmRooms();
                rm.MdiParent = this;
                rm.DBPath = DatabasePath;
                rm.LoginUser = tssUserlogin.Text;
                rm.Show();
            }
        }

        private void roomAssignmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckAndLoadSettings())
            {
                frmRoomAssignment  ra = new frmRoomAssignment();
                ra.MdiParent = this;
                ra.DBPath = DatabasePath;
                ra.LoginUser = tssUserlogin.Text;
                ra.Show();
            }
        }

        private void tenantProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckAndLoadSettings())
            {
                frmTenantProfile cp = new frmTenantProfile();
                cp.MdiParent = this;
                cp.DBPath = DatabasePath;
                cp.LoginUser = tssUserlogin.Text;
                cp.Show();
            }
        }
    }
}
