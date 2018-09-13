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
    public partial class frmBillingAccount : Form
    {
        clsFunctiion cf = new clsFunctiion();
        public frmBillingAccount()
        {
            InitializeComponent();
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (textBox3.Text == string.Empty)
                {
                    textBox3.Text = "";
                    textBox3.Focus();
                }
                else if (cf.isIntegerValid(e.KeyChar.ToString()) || e.KeyChar == '.')
                {

                }
                else
                {
                    MessageBox.Show("Error: Please enter monetary value.", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox3.Text = "";
                    textBox3.Focus();
                }
            }

            catch
            { }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.Text == string.Empty)
            {
                textBox3.Text = "0.00";
            }
        }
    }
}
