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
    public partial class Form1 : Form
    {
        public string DBPath;
        public string Amount;

        public string AmountTendered;
        public string PaymentMethod;
        public bool IsPaid;

        clsFunctiion cf = new clsFunctiion();

        public Form1()
        {
            InitializeComponent();
        }

        private void LoadPaymentMethod()
        {
            try
            {
                string Query = "select sysid,PaymentMethod from tblPaymentMethod where isActive = 1 order by PaymentMethod asc";
                DataTable dtRecords = cf.GetRecords(Query);

                if (dtRecords.Rows.Count > 0)
                {
                    comboBox1.DataSource = dtRecords;
                    comboBox1.DisplayMember = "PaymentMethod";
                    comboBox1.ValueMember = "sysid";
                }
            }
            catch
            {
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cf.DbLocation = DBPath;
            textBox1.Text = string.Format("{0:C}", Amount).Replace("$", "");
            LoadPaymentMethod();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == 13)
            {
                button1.PerformClick();
            }

            if (!cf.isIntegerValid(e.KeyChar.ToString()) && e.KeyChar.ToString() != "." && e.KeyChar.ToString() != "\b")
            {
                MessageBox.Show("Error: Please enter monetary value. ", "Invalid Payment Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox2.Text = string.Empty;
                return;
            }
            else
            {   
            }
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //if (string.IsNullOrEmpty(textBox2.Text))
                //{
                //    textBox2.Text = "0.00";
                //}
                if (textBox2.Text != string.Empty)
                {
                    textBox3.Text = string.Format("{0:C}", double.Parse(textBox2.Text) - double.Parse(textBox1.Text)).Replace("$", "");
                }
                else
                {
                    textBox3.Text = string.Format("{0:C}", double.Parse("0.00") - double.Parse(textBox1.Text)).Replace("$", "");
                }
            }
            catch
            {
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "0.00")
            {
                textBox2.Text = string.Empty;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            try
            {
                if (textBox2.Text == string.Empty)
                {
                    textBox2.Text = "0.00";
                }
            }
            catch
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.Contains("("))
            {
                MessageBox.Show("Error: Cannot save payment. Please check your amount", "Payment Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            PaymentMethod = comboBox1.SelectedValue.ToString();
            AmountTendered = string.Format("{0:C}", textBox2.Text).Replace("$", "");
            IsPaid = true;
            this.Close();
        }
    }
}
