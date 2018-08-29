using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RPOSWI.Screen
{
    public partial class ConfigSettings : Form
    {
        public string UserActive;

        public ConfigSettings()
        {
            InitializeComponent();
        }

        private void ConfigSettings_Load(object sender, EventArgs e)
        {
            ShowTab();
        }

        private void ShowTab()
        {
            try
            {
                if (string.IsNullOrEmpty(UserActive))
                {
                    foreach (TabPage tp in tabControl1.TabPages)
                    {
                        tp.
                    }
                }
            }
            catch
            {
            }
        }
    }
}
