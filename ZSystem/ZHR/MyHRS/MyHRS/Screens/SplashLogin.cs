using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using MyHRS.Classes;

namespace MyHRS.Screens
{
    public partial class SplashLogin : Form
    {
        clsYU YU = new clsYU();
        bool isLogin = false;

        public SplashLogin()
        {
            InitializeComponent();
        }

        #region UI
        private void TextboxControl()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl.GetType().Name == "GroupBox")
                {
                    foreach (Control GBCtrl in ctrl.Controls)
                    {
                        if (GBCtrl.GetType().Name == "Panel")
                        {
                            foreach (Control PnlCtrl in GBCtrl.Controls)
                            {
                                if (PnlCtrl.GetType().Name == "TextBox")
                                {
                                    TextBox tb = (TextBox)PnlCtrl;
                                    tb.Enter += new EventHandler(tb_Enter);
                                    tb.Leave += new EventHandler(tb_Leave);
                                }
                            }
                        }
                    }
                }
            }
        }
        private void LoadLoginBG()
        {
            try
            {
                PBSplashImage.BackgroundImage = Image.FromFile(Environment.CurrentDirectory + "\\Resources\\LoginPIC.jpg");
                PBSplashImage.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch(Exception ex)
            {
                YU.LogWrite(ex.Message.ToString(),"SplashLogin", "Error");        
            }
        }
        #endregion


        public void ControlInitialized()
        {
           TextboxControl();
           LoadLoginBG();
        }

        private void SplashLogin_Load(object sender, EventArgs e)
        {
            ControlInitialized();
            timer1.Enabled = true;
            YU.LogWrite("Start Program","SplashLogin", "Log");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Value += 10;
            if (progressBar1.Value == 100)
            {
                Thread.Sleep(100);
                timer1.Enabled = false;
                this.Height = 310;
            }
        }

        private void tb_Enter(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.BackColor = Color.Yellow;
        }

        private void tb_Leave(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.BackColor = Color.White;
        }

        private void textBox2_MouseHover(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '\0';
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            textBox2.PasswordChar = '*';
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Error: Invalid entry. Please check", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                YU.Control_TextboxError(textBox1);
                textBox1.Focus();
            }
            isLogin = true;
            this.Close();
        }

        private void SplashLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isLogin)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to close this program?", "Exit Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }

        private void databaseConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DatabaseConnection dc = new DatabaseConnection();
            dc.ShowDialog();
        }

    }
}
