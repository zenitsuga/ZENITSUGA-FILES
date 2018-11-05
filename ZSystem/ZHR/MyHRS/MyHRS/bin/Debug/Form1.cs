using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MyHRS.Screens;

namespace MyHRS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SplashLogin sp = new SplashLogin();
            sp.ShowDialog();
        }

        private void databaseConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DatabaseConnection dc = new DatabaseConnection();
            dc.ShowDialog();
        }
    }
}
