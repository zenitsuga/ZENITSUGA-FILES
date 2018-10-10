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
namespace ZBilling.Forms
{
    public partial class Settings : Form
    {
        clsFunctiion cf = new clsFunctiion();
        Database db = new Database();
        IniFile ini;

        public string CompanyName;
        public bool isDatabaseConnected;
        public string IniFileSettings;
        private bool isValidLicense = false;

        public Settings()
        {
            InitializeComponent();
        }

        private void ReadSettings()
        {
            try
            {
                string IniPath = IniFileSettings; //Environment.CurrentDirectory + "\\ResourceFolder\\DefaultSettings.txt";
                
                if (File.Exists(IniPath))
                {
                    IniFileSettings = string.IsNullOrEmpty(File.ReadAllText(IniPath)) ? IniFileSettings:IniPath;
                    if (string.IsNullOrEmpty(IniFileSettings))
                    {
                        DialogResult dr = MessageBox.Show("File setting location not yet defined. Would you like to set it first?", "Settings is empty", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        if (dr == DialogResult.Yes)
                        {
                            button6.PerformClick();
                        }
                    }
                    else
                    {
                        ReadDefaultIni();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Writing Ini Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveSettings()
        {
            try
            {
                if (!string.IsNullOrEmpty(IniFileSettings))
                {
                    if (File.Exists(IniFileSettings))
                    {
                        ini = new IniFile(IniFileSettings);
                        if (!string.IsNullOrEmpty(textBox1.Text))
                        {
                            if (textBox1.Text.Contains(";"))
                            {
                                string ServerName = (textBox1.Text.Split(';')[0].ToString()).Split('=')[1].ToString();
                                ini.Write("ServerName", ServerName, "Database");
                                string DatabaseName = (textBox1.Text.Split(';')[1].ToString()).Split('=')[1].ToString();
                                ini.Write("DatabaseName", DatabaseName, "Database");
                                string UserName = (textBox1.Text.Split(';')[2].ToString()).Split('=')[1].ToString();
                                ini.Write("UserName", UserName, "Database");
                                string Password = (textBox1.Text.Split(';')[3].ToString()).Split('=')[1].ToString();
                                ini.Write("Password", Password, "Database");
                            }
                        }

                        ini.Write("MainBackground", textBox2.Text, "Application");
                        ini.Write("HelpFileLocation", textBox3.Text, "Application");
                        ini.Write("LicenseCode", textBox4.Text, "License");

                        MessageBox.Show("Configuration Saved", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Ini file settings not yet defined.", "Load ini first", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Writing Ini Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Settings_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = this.Text.Replace("[CompanyName]", CompanyName);
                IniFileSettings = Environment.CurrentDirectory + "\\Settings.ini";
                if (!File.Exists(IniFileSettings))
                {
                    MessageBox.Show("Error: Please check your Settings.ini for database settings", "File Not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                ReadSettings();
                validateDatabase();

                ValidatedLicense();
                if (isValidLicense && isDatabaseConnected)
                {
                    this.Close();
                }
            }
            catch
            {
            }
        }

        private void validateDatabase()
        {
            try
            {
                string ErrMsg = string.Empty;
                if (!cf.IsConnected(textBox1.Text, ref ErrMsg))
                {
                    MessageBox.Show("Error: Database is Invalid", "Check your database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    label2.Text = "DISCONNECTED";
                    label2.ForeColor = Color.Red;
                    isDatabaseConnected = false;
                    textBox1.Focus();
                }
                else
                {
                    label2.Text = "CONNECTED";
                    label2.ForeColor = Color.Black;
                    isDatabaseConnected = true;
                }
            }
            catch
            {
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            ValidatedLicense();
            if (isValidLicense)
            {
                DialogResult dr = MessageBox.Show("Do you want to save this settings?", "Confirm to Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    validateDatabase();
                    SaveSettings();
                }
            }
            else
            {
                //MessageBox.Show("Error: Please check your license.","Invalid License",MessageBoxButtons.OK,MessageBoxIcon.Error);
                textBox4.Focus();
                return;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.CurrentDirectory;
            ofd.Title = "Location of Configuration file (.ini)";
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Filter = "Setting File (*.ini)|*.ini";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
               IniFileSettings = ofd.FileName;
               SaveDefaultIni();
               ValidatedLicense();
            }
        }

        private void ReadDefaultIni()
        {
            try
            {
                if (!string.IsNullOrEmpty(IniFileSettings))
                {
                    ini = new IniFile(IniFileSettings);
                    string ConnString = cf.ConstructConnString(ini.Read("ServerName","Database"), ini.Read("DatabaseName","Database"), ini.Read("Username","Database"), ini.Read("Password","Database"));
                    textBox1.Text = ConnString;
                    textBox2.Text = ini.Read("MainBackground", "Application");
                    textBox3.Text = ini.Read("HelpFileLocation", "Application");
                    textBox4.Text = ini.Read("LicenseCode", "License");
                }
            }
            catch
            {
            }
        }

        private void SaveDefaultIni()
        {
            try
            {
                //string DefaultIniPath = Environment.CurrentDirectory + "\\ResourceFolder\\DefaultSettings.txt";
                
                //if (!File.Exists(DefaultIniPath))
                //{
                //    File.CreateText(DefaultIniPath);
                //}

                //File.AppendAllText(DefaultIniPath, IniFileSettings, Encoding.UTF8);
                ReadDefaultIni();
                MessageBox.Show("Configuration Path Loaded", "Configuration is set", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Saving Default Ini Path", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Locate your database file(.db)";
                ofd.InitialDirectory = Environment.CurrentDirectory;
                ofd.CheckFileExists = true;
                ofd.CheckPathExists = true;
                ofd.Filter = "Database File (*.db)|*.db";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                   textBox1.Text = ofd.FileName;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Locate your Image file";
                ofd.InitialDirectory = Environment.CurrentDirectory;
                ofd.CheckFileExists = true;
                ofd.CheckPathExists = true;
                ofd.Filter = "jpeg File (*.jpg)|*.jpg|png File (*.png)|*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    textBox2.Text = ofd.FileName;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Locate your Help file";
                ofd.InitialDirectory = Environment.CurrentDirectory;
                ofd.CheckFileExists = true;
                ofd.CheckPathExists = true;
                ofd.Filter = "Help File (*.htm)|*.htm|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    textBox3.Text = ofd.FileName;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            frmCompanyProfile cp = new frmCompanyProfile();
            cp.IniPath = IniFileSettings;
            cp.ShowDialog();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Error: Cannot connect. Please provide your database", "Invalid Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Focus();
                return;
            }
            string ErrMsg = string.Empty;
            if (!cf.IsConnected(textBox1.Text, ref ErrMsg))
            {
                MessageBox.Show("Error: " + ErrMsg, "Disconnected Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                label2.Text = "DISCONNECTED";
                textBox1.Focus();
                return;
            }
            else
            {
                label2.Text = "CONNECTED";
            }
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            ValidatedLicense();
        }
        private void ValidatedLicense()
        {
            try
            {
                if (string.IsNullOrEmpty(textBox4.Text))
                {
                    MessageBox.Show("Error: Invalid License Key. Enter a new one.", "License Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblLicenseStatus.Text = "Invalid";
                    lblLicenseStatus.ForeColor = Color.Red;
                    textBox4.Focus();
                    isValidLicense = false;
                    return;
                }
                int daysremain = 0;
                bool isValid = cf.ValidateLicense(textBox4.Text, ref daysremain);

                if (!isValid)
                {
                    lblLicenseStatus.Text = "Invalid";
                    lblLicenseStatus.ForeColor = Color.Red;
                    isValidLicense = false;
                    textBox4.Focus();
                }
                else
                {
                    if (daysremain <= 30)
                    {
                        lblLicenseStatus.Text = "You only have " + daysremain + " day(s) remaining";
                        lblLicenseStatus.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblLicenseStatus.Text = "Valid";
                        lblLicenseStatus.ForeColor = Color.Black;
                    }
                    isValidLicense = true;
                }
            }
            catch
            {
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            validateDatabase();
        }
    }
}
