using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace ProgramGrabber.Class
{
    public class clsFunctions
    {
        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public string GetMachineID()
        {
            string result = string.Empty;
            try
            {
                ManagementObjectSearcher searcher =
   new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    result = queryObj["ProcessorID"].ToString();
                }
            }
            catch
            {
            }
            return result;
        }
    }
}
