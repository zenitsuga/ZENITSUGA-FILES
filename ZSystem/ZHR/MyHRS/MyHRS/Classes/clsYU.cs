
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using Licensing;

namespace MyHRS.Classes
{
    public class clsYU
    {   
        string m_ExePath;
        string m_LogName;

        public bool isValidLicense()
        {
            bool result = false;
            try
            {
                
            }
            catch
            {
            }
            return result;
        }

        public void Control_TextboxError(TextBox tb)
        {
            try
            {
                tb.BackColor = Color.Red;
            }
            catch
            {
            }
        }

        public void LogWrite(string logMessage,string ModuleName,string TypeLog)
        {
            m_ExePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Logs";

            if (!Directory.Exists(m_ExePath))
            {
                Directory.CreateDirectory(m_ExePath);
            }

            if (TypeLog == "Error")
            {
                m_LogName = "Err_" + DateTime.Now.ToString("MMddyyyy");
            }
            else
            {
                m_LogName = "Log_" + DateTime.Now.ToString("MMddyyyy");
            }
            try
            {
                using (StreamWriter w = File.AppendText(m_ExePath + "\\" + m_LogName))
                {
                    Log(TypeLog,logMessage,ModuleName, w);
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        public void Log(string LogType,string logMessage,string ModuleName, TextWriter txtWriter)
        {
            try
            {
                txtWriter.WriteLine("{0}:\t{1}:\t{2}:\t{3}", LogType, DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss"), ModuleName, logMessage);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
