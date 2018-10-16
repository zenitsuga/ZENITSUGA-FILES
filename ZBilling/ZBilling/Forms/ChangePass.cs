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
    public partial class ChangePass : Form
    {
        string keys = "zbln-3asd-sqoy19";

        clsFunctiion cf = new clsFunctiion();
        public string LoginUser;
        public string DBPath;

        public ChangePass()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Error: Cannot change password. Invalid entry.", "Please check", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DialogResult dr = MessageBox.Show("Are you sure you want to change your password?", "Change Password", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                ChangePassword();
            }
        }

        private void ChangePass_Load(object sender, EventArgs e)
        {
            textBox1.Text = LoginUser;
        }
        private void ChangePassword()
        {
            try
            {
                cf.DbLocation = DBPath;
                List<string> SV = new List<string>();
                string Criteria = string.Empty;

                SV.Add("Username = '" + textBox1.Text + "'");
                
                SV.Add("Password = '" + clsLic.CryptoEngine.Encrypt(textBox2.Text, keys) + "'");
                string UserID = cf.GetSysID("tblUsers"," where username ='" + textBox1.Text + "'").ToString();
                Criteria = "where sysID=" + UserID;

                if (!cf.UpdateRecords("tblUsers", SV, Criteria))
                {
                    MessageBox.Show("Error: Saving records error. Please check your entry", "Saving Record Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                MessageBox.Show("Done", "Save Record : Users", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Text = textBox2.Text = string.Empty;
            }
            catch
            {
            }
        }

        private void textBox2_MouseHover(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '\0';
        }

        private void textBox2_MouseLeave(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
        }

    }
}
