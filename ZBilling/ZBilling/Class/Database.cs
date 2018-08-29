using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace ZBilling.Class
{
    public class Database
    {
        public string DBPath;
        public string SQLConnString;

        public string GetConnString(string DBPath)
        {
            IniFile ini = new IniFile(DBPath);
            string result = string.Empty;
            try
            {
                if (DBPath.Contains(".ini"))
                {
                    result = "Server = " + ini.Read("ServerName", "Database") + "; Database = " + ini.Read("DatabaseName", "Database") + "; User Id = " + ini.Read("UserName", "Database") + ";" +
                             "Password = " + ini.Read("Password", "Database") + "; ";
                }
            }
            catch
            {
            }
            return result;
        }

        public string SQLConnBuilder(string DBPath)
        {
            string result = string.Empty;
            try
            {
                string ConnViaIni = string.Empty;
                if (DBPath.ToLower().Contains("server"))
                {
                    ConnViaIni = DBPath;
                }
                else
                {
                    ConnViaIni = GetConnString(DBPath);
                }
                    SQLConnString = ConnViaIni;//"Data Source=" + DBPath + ";Version=3;New=True;Compress=True;";
                result = SQLConnString;
            }
            catch
            {
            }
            return result;
        }
        //public bool testConnection(string DbPath,ref string ErrMsg)
        //{
        //    bool result = false;
        //    try
        //    {
        //        //string sqlConstr = "Data Source=" + DbPath + ";Version=3;New=True;Compress=True;"; 
        //        string sqlConstr = SQLConnBuilder(DbPath);
        //        SQLiteConnection sconn = new SQLiteConnection(sqlConstr);
        //        sconn.Open();
        //        result = sconn.State == System.Data.ConnectionState.Open ? true:false;
        //    }
        //    catch(Exception ex)
        //    {
        //        ErrMsg = ex.Message.ToString();
        //    }
        //    return result;
        //}
        public bool testConnection(string ConnectionString, ref string ErrMsg)
        {
            bool result = false;
            try
            {
                //string sqlConstr = "Data Source=" + DbPath + ";Version=3;New=True;Compress=True;"; 
                SQLConnString = ConnectionString;
                DBPath = ConnectionString;
                string sqlConstr = SQLConnBuilder(DBPath);//SQLConnBuilder(ConnectionString);
                //SQLiteConnection sconn = new SQLiteConnection(sqlConstr);
                SqlConnection sconn = new SqlConnection(sqlConstr);
                sconn.Open();
                result = sconn.State == System.Data.ConnectionState.Open ? true : false;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message.ToString();
            }
            return result;
        }
        public bool ExecuteNonQuery(string SQLStatement)
        {
            bool result = false;
            try
            {
                //string SqlConnstr = SQLConnBuilder(DBPath);
                string sqlConstr = SQLConnBuilder(DBPath);
                SqlConnection sconn = new SqlConnection(sqlConstr);
                sconn.Open();
                if (sconn.State == ConnectionState.Open)
                {
                    SqlCommand sqlcom = new SqlCommand(SQLStatement, sconn);
                    sqlcom.ExecuteReader();
                    result = true;
                }
            }
            catch
            {
            }
            return result;
        }
        public DataTable ExecuteQuery(string SQLStatement)
        {
            DataTable dtResult = new DataTable();
            try
            {
                string SqlConnstr = SQLConnBuilder(DBPath);
                SqlConnection sconn = new SqlConnection(SqlConnstr);
                sconn.Open();
                if (sconn.State == ConnectionState.Open)
                {
                    SqlDataAdapter sda = new SqlDataAdapter(SQLStatement, sconn);
                    sda.Fill(dtResult);
                }
            }
            catch
            {
            }
            return dtResult;
        }
    }
}
