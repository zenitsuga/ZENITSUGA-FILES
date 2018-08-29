using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Management;

namespace Registration
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TextBox1.Text = GetMachineKey();
        }
        public string GetMachineKey()
        {
            string MachineKey = "Not Found!!!";
            try
            {
                ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard");
                ManagementObjectCollection moc = mos.Get();

                foreach (ManagementObject mo in moc)
                {
                    MachineKey = mo["SerialNumber"].ToString();
                }
                return MachineKey;
            }
            catch
            {
            }
            return MachineKey;
        }
    }
}