using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RPOSWI.Screen;
namespace RPOSWI
{
    public partial class Form1 : Form
    {
        public string ActiveUser;

        public Form1()
        {
            InitializeComponent();
        }

        private void configurationSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowConfig();
        }

        private void ShowConfig()
        {
            try
            {
                ConfigSettings cs = new ConfigSettings();
                cs.UserActive = ActiveUser;
                cs.ShowDialog();
            }catch
            {
            }
        }
    }
}
