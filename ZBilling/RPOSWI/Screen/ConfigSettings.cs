using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RPOSWI.Classes;

namespace RPOSWI.Screen
{
    public partial class ConfigSettings : Form
    {
        public string UserActive;

        clsFunction cf = new clsFunction();
        ConfigurationSettings cs;

        public ConfigSettings()
        {
            InitializeComponent();
        }

        private void ConfigSettings_Load(object sender, EventArgs e)
        {
            cs = new ConfigurationSettings();
            cs = cf.LoadConfigSettings();
            if (cs != null)
            {
                txtMachineKey.Text = cs.MachineID;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using(FolderBrowserDialog ofd = new FolderBrowserDialog())
            {
                ofd.ShowNewFolderButton = true;
                ofd.ShowDialog();
                txtQuotationPath.Text = ofd.SelectedPath; 
            }
        }
    }
}
