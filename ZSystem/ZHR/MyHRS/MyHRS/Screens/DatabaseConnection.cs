using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MyHRS.Classes;

namespace MyHRS.Screens
{
    public partial class DatabaseConnection : Form
    {
        clsYU yu = new clsYU();

        public DatabaseConnection()
        {
            InitializeComponent();
        }

        private void InitializedForm()
        {
            try
            {
                pbDBImage.Image = Image.FromFile(Environment.CurrentDirectory + "\\Resources\\Database.png");
                pbStopNGo.Image = Image.FromFile(Environment.CurrentDirectory + "\\Resources\\Stop.png");
            }
            catch(Exception ex)
            {
                yu.LogWrite(ex.Message.ToString(),"DatabaseConnection", "Error");        
            }
        }

        private void DatabaseConnection_Load(object sender, EventArgs e)
        {
            yu.LogWrite("Database Connection Open","DatabaseConnection", "Log");
            InitializedForm();
        }
    }
}
