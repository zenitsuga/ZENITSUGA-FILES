using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;
using ProgramGrabber.Class;

namespace ProgramGrabber
{
    public partial class Form1 : Form
    {
        clsFunctions cf = new clsFunctions();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = cf.GetMachineID();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Validate Email
            if (!cf.IsValidEmail(textBox2.Text))
            {
                MessageBox.Show("Error: Invalid email format. Please check", "Email Address error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("Registration successfully. Kindly wait the verification key on your email.", "Machine Registered", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
