using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace RPOSWI.Classes
{
    public class ConfigurationSettings
    {
        public string ServerName {get;set;}
        public string MachineID { get; set; }
    }

    public class clsFunction
    {
        public ConfigurationSettings LoadConfigSettings()
        {
            ConfigurationSettings cs = new ConfigurationSettings();
            try
            {
                cs.MachineID = GetMachineID();
            }
            catch
            {
            }
            return cs;
        }
        public string GetMachineID()
        {
            string Result = string.Empty;
            try
            {
                ManagementObjectSearcher searcher =
        new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Result = queryObj["ProcessorID"].ToString();
                }
            }
            catch
            {
            }
            return Result;
        }
    }
}
