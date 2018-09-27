using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using clsLic;
using System.Data;
using System.Data.SqlClient;

namespace ZBilling.Class
{
    public class clsFunctiion
    {   
        Database db;
        IniFile inif;
        
        public string DbLocation;

        public DataTable SetMenu(int Role)
        {
            DataTable dtResult = new DataTable();
            try
            {
                if (Role > 0)
                {
                    string Query = "Select ModuleName from tblUserAccess where Role = " + Role + " ";
                    DataTable dtQueryResult = new DataTable();
                    dtQueryResult = db.ExecuteQuery(Query);
                    if (dtQueryResult.Rows.Count > 0)
                    {
                        dtResult = dtQueryResult;
                    }
                }
            }
            catch
            {
            }
            return dtResult;
        }

        public string ConstructConnString(string ServerName, string DatabaseName, string Username, string Password)
        {
            string result = string.Empty;
            try
            {
                result = "Server = "+ServerName+"; Database = "+DatabaseName+"; User Id = "+Username+";" +
                          "Password = "+Password+";";
            }
            catch
            {
            }
            return result;
        }

        public bool IsConnected(string DBPath,ref string ErrMsg)
        {
            bool result = false;
            try
            {
                db = new Database();
                db.SQLConnString = DBPath;
                result = db.testConnection(DBPath, ref ErrMsg);

                if (!string.IsNullOrEmpty(ErrMsg))
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            catch(Exception ex)
            {
                ErrMsg = ex.Message.ToString();
            }
            return result;
        }
        public bool ValidateUser(string Username, string Password, ref string UserRole)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(DbLocation))
                {
                    string sqlquery = "select u.username,u.password,r.Role from tblUsers u " +
                                      "  left join tblUserRole r on u.Role = r.sysID " +
                                      "  where Username ='" + Username + "' and  " +
                                      "  password = '" + Password + "' and u.isActive = 1";

                    db.DBPath = DbLocation;
                    DataTable dtUserResult = db.ExecuteQuery(sqlquery);

                    if (dtUserResult.Rows.Count > 0)
                    {
                        UserRole = dtUserResult.Rows[0]["Role"].ToString();
                        result = true;
                    }
                }
            }
            catch
            {
            }
            return result;
        }
        public bool CheckDatabaseStatus(string DatabaseLocation)
        {
            bool result = false;
            try
            {
                db = new Database();
                string ErrMsg = string.Empty;
                result = db.testConnection(DatabaseLocation, ref ErrMsg);
            }
            catch
            {
            }
            return result;
        }
        public bool ValidateLicense(string License,ref int DaysRemain)
        {
            string keys = "zbln-3asd-sqoy19";
            bool result  = false;
            try{
                string LicenseDecrypt = clsLic.CryptoEngine.Decrypt(License,keys);   
                if(isDateValid(LicenseDecrypt))
                {
                    DateTime licEnd = DateTime.Parse(LicenseDecrypt);
                    DaysRemain =licEnd.Subtract(DateTime.Now).Days;
                    if (DaysRemain > 0)
                    {
                        result = true;
                    }
                }
            }catch
            {}
            return result;
        }
        public bool isDateValid(string Date)
        {
            bool result = false;
            try
            {
               DateTime dt =  DateTime.Parse(Date);
               result = true;
            }
            catch
            {
            }
            return result;
        }
        public bool isRoomSizeValid(string RoomSize)
        {
            bool result = false;
            try
            {
                double resdouble = double.Parse(RoomSize);
                result = true;  
            }
            catch
            {
            }
            return result;
        }
        public bool isIntegerValid(string IntergerVal)
        {
            bool result = false;
            try
            {
                int res = int.Parse(IntergerVal);
                result = true;
            }
            catch
            {
            }
            return result;
        }
        public bool CheckDueDateTranasactionCreated(string DueDate)
        {
            bool result = false;
            try
            {
                DateTime DueDateStart = DateTime.Parse(DueDate);
                DateTime DueDateEnd = DueDateStart;
                string Query = "select count(*) from tblTransactionDetails td " +
                               " Left Join tblTransaction t on td.ReferenceID = t.sysid " +
                               " where t.DueDate between '"+ DueDateStart +"' and '"+ DueDateEnd +"'";
                DataTable dtResult = GetRecords(Query);
                if (dtResult.Rows.Count > 0)
                {
                    if (isIntegerValid(dtResult.Rows[0][0].ToString()))
                    {
                        int Count = int.Parse(dtResult.Rows[0][0].ToString());
                        if (Count == 0)
                        {
                            result = true;
                        }
                    }
                }
            }
            catch
            {
            }
            return result;
        }

        public bool checkRoomName(string RoomName,ref int RoomCount)
        {
            bool result = false;
            try
            {
                string Query = "Select * from tblRooms where RoomName='" + RoomName + "' order by sysID desc";
                DataTable dtRecords = GetRecords(Query);
                if (dtRecords.Rows.Count == 0)
                {
                    result = true;
                    RoomCount = 0;
                }
                else if(dtRecords.Rows.Count > 0)
                {
                    RoomCount = dtRecords.Rows.Count;
                }
            }
            catch
            {
            }
            return result;
        }
        public string FixMoneyValue(string moneyVal)
        {
            string result = string.Format("{0:C}", moneyVal).Replace("$", "");
            try
            {
                result = string.Format("{0:C}", moneyVal).Replace("$", "");
            }
            catch
            {
            }
            return result;
        }
        public void PreviousRoomDisable(string RoomName)
        {
            try
            {
                string Query = "Update tblRooms set isActive = 0 where RoomName = '" + RoomName + "'";
                ExecuteNonQuery(Query);
            }
            catch
            {
            }
        }

        public bool CheckMonthlyRate(string RoomNumber,ref string Amount)
        {
            bool result = false;
            try
            {
                string Query = "SELECT top 1 MonthlyDue from tblrooms " +
                               "where RoomName = '"+ RoomNumber +"' and isActive = 1 " +
                               "order by sysid desc ";
                DataTable dtResult = GetRecords(Query);
                if (dtResult.Rows.Count > 0)
                {
                    Amount = string.Format("{0:C}", dtResult.Rows[0][0].ToString().Replace("$", ""));
                }
            }
            catch
            {
            }
            return result;
        }

        public string GetFieldValue(string sysID, string TableName, string FieldOutput)
        {
            string result = string.Empty;
            try
            {
                string query = "Select " + (string.IsNullOrEmpty(FieldOutput) ? "*" : FieldOutput) + " from " + TableName + " where sysID = " + sysID;

                foreach (DataRow dr in GetRecords(query).Rows)
                {
                    result = dr[0].ToString();
                }
            } 
            catch
            {
            }
            return result;
        }
        public int GetSysID(string TableName, string Criteria)
        {
            int result = 0;
            try
            {
                db = new Database();
                string DataResult = string.Empty;
                db.DBPath = DbLocation;
                DataResult = db.ExecuteQuery("Select sysID from " + TableName + " " + Criteria).Rows[0][0].ToString();
                if (!string.IsNullOrEmpty(DataResult))
                {
                    if (isIntegerValid(DataResult))
                    {
                        result = int.Parse(DataResult);
                    }
                }
            }
            catch
            {
            }
            return result;
        }
        public bool DeleteRecord(string tableName, string criteria)
        {
            bool result = false;
            try
            {
                string Query = "Delete from " + tableName + criteria;
                db.DBPath = DbLocation;
                result = db.ExecuteNonQuery(Query);
            }
            catch
            {
            }
            return result;
        }

        public string GetRecordValue(string FieldOutput, string tablename, string sysID)
        {
            string result = string.Empty;
            try
            {
                string query = "Select " + FieldOutput + " from " + tablename + " where sysID =" + sysID;
                DataTable dtResult = GetRecords(query);
                result = dtResult.Rows[0][0].ToString();
            }
            catch
            {
            }
            return result;
        }

        public DataTable GetRecords(string SQLStatement)
        {
            DataTable dtResult = new DataTable();
            try
            {
                db = new Database();
                db.DBPath = DbLocation; 
                dtResult = db.ExecuteQuery(SQLStatement);
            }
            catch
            {
            }
            return dtResult;
        }
        public bool ExecuteNonQuery(string SQLStatement)
        {
            bool result = false;
            try
            {
                db.DBPath = DbLocation;
                result = db.ExecuteNonQuery(SQLStatement);
            }
            catch
            {
            }
            return result;  
        }

        public bool InsertRecords(string TableName,List<string> FieldColumn, List<string> FieldValue,List<string> FieldInteger)
        {
            bool result = false;
            try
            {
                string FC = string.Empty;
                string FV = string.Empty;
                string FI = string.Empty;

                if (FieldColumn.Count > 0 && FieldValue.Count > 0)
                {
                    foreach(string StrFC in FieldColumn)
                    {
                        FC += StrFC + ",";
                    }

                    FC = FC.Substring(0, FC.Length - 1);

                    foreach (string StrFV in FieldValue)
                    {
                        FV += "'" + StrFV + "',";
                    }

                    FV = FieldInteger.Count > 0 ? FV : FV.Substring(0, FV.Length - 1);

                    foreach (string StrFI in FieldInteger)
                    {
                        FI += StrFI + ",";
                    }

                    FV = FV + FI.Substring(0,FI.Length -1);

                    string Query = "Insert into " + TableName + "(" + FC + ")values(" + FV + ")";

                    db.DBPath = DbLocation;
                    result = db.ExecuteNonQuery(Query);
                }
            }
            catch
            {
            }
            return result;
        }
        public bool UpdateRecords(string TableName,List<string> SetFieldandVal,string Criteria)
        {
            bool result=false;
            try
            {
                string SV = string.Empty;
                
                if (SetFieldandVal.Count > 0)
                {
                    foreach (string strSV in SetFieldandVal)
                    {
                        SV += strSV + ",";
                    }

                    SV = SV.Substring(0, SV.Length - 1);

                    string Query = "UPDATE " + TableName + " SET " + SV + " " + Criteria;

                    db.DBPath = DbLocation;
                    result = db.ExecuteNonQuery(Query);
                }
            }
            catch
            {
            }
            return result;
        }
        public DataTable RetrieveRecords(string TableName, string Fields,string Criteria, string OrderBy, string GroupBy)
        {
            DataTable dtRecords = new DataTable();
            try
            {
                db = new Database();
                string Query = "select " + Fields + " from  " +  TableName 
                                + (string.IsNullOrEmpty(Criteria) ? string.Empty: " where " + Criteria)
                                + (string.IsNullOrEmpty(OrderBy) ? string.Empty: " Order by " + OrderBy)
                                + (string.IsNullOrEmpty(GroupBy) ? string.Empty: " Group by " + GroupBy);
                db.DBPath = DbLocation;
                dtRecords = db.ExecuteQuery(Query);
            }
            catch
            {
            }
            return dtRecords;
        }
        public DataTable LoadUserRole()
        {
            DataTable dtRecords = new DataTable();
            try
            {   
                dtRecords = RetrieveRecords("tblUserrole", "sysID,Role", " isActive = 1", " Role ASC", "");
            }
            catch
            {
            }
            return dtRecords;
        }
        public bool isRecordExists(string Tablename, string CriteriaCol, string Value2Find)
        {
            bool result = false;
            try
            {
                string Query = "Select * from " + Tablename + CriteriaCol + Value2Find;
                DataTable dtResult = db.ExecuteQuery(Query);
                if (dtResult.Rows.Count > 0)
                {
                    result = true;
                }
            }
            catch
            {
            }
            return result;
        }
        public double ComputeLastOutStandingForBalance(string CustomerID)
        {
            double Result = 0.00;
            try
            {
                string Query  = "SELECT (OpeningBalance + OutstandingBalance) as NewOpeningBalance " +
                                "FROM tblTransaction where isactive = 1 and CustomerID =" + CustomerID + " order by sysid desc";
                DataTable dtResult = GetRecords(Query);
                if (dtResult.Rows.Count > 0)
                {
                    Result = double.Parse(dtResult.Rows[0][0].ToString());
                }
            }
            catch
            {
            }
            return Result;
        }
        public string GetLastID(string TableName)
        {
            db = new Database();
            string result = string.Empty;
            try
            {
                db.DBPath = DbLocation;
                string Query = "select top 1 sysID from " + TableName + " order by sysid desc";
                DataTable dtRecords = db.ExecuteQuery(Query);
                if (dtRecords.Rows.Count > 0)
                {
                    result = (int.Parse(dtRecords.Rows[0][0].ToString()) + 1).ToString();
                }
                else
                {
                    result = (1).ToString();
                }
            }
            catch
            {
            }
            return result;
        }

        public DataTable GetTransactionSummary(string year,string CustID)
        {
            DataTable dtResult = new DataTable();
            try
            {
                dtResult = GetRecords("SELECT t.DateTransaction,t.TransactionNo,t.openingBalance,t.OutStandingBalance,u.Username,t.Datetimestamp " +
                          "FROM [Billing].[dbo].[tblTransaction] t left join tblUsers u on t.Userid = u.sysID " +
                          "where customerID = "+ CustID +" and Year(DateTransaction) = '"+ year +"' " +
                          "order by t.sysid ASC");

            }
            catch
            {
            }
            return dtResult;
        }

        public string CheckAmountByType(string TypeAccount, string Amount)
        {
            string result = Amount;
            try
            {
                string TypeCode = GetRecords("Select AccountType from tblTransactionType where TransCode='" + TypeAccount + "'").Rows[0][0].ToString();
                if (TypeCode == "2")
                {
                    result = "-" + Amount;
                }
            }
            catch
            {
            }
            return result;
        }

        public bool InsertTransaction(string TransactionNo, string Description, string DateTrans, string CustomerID, string RoomID, string OpeningBal, string OutstandingBal,string UserID,string DueDate,string CustomerType)
        {
            bool result = false;
            try
            {
                string Query = "Insert into tblTransaction(TransactionNo,Description,DateTransaction,CustomerID,RoomID,OpeningBalance,OutstandingBalance,userID,DueDate) values ('" + TransactionNo + "','Transaction Code: "+ TransactionNo +"','"+ DateTrans +"'," + CustomerID + ","+ RoomID + "," + OpeningBal.Replace(",","") + "," + OutstandingBal.Replace(",","") + "," + UserID + ",'"+ DueDate  +"')" ;

                if (!string.IsNullOrEmpty(CustomerType))
                {
                    Query = "Insert into tblTransaction(TransactionNo,Description,DateTransaction,tenantID,RoomID,OpeningBalance,OutstandingBalance,userID,DueDate) values ('" + TransactionNo + "','Transaction Code: " + TransactionNo + "','" + DateTrans + "'," + CustomerID + "," + RoomID + "," + OpeningBal.Replace(",","") + "," + OutstandingBal.Replace(",","") + "," + UserID + ",'" + DueDate + "')";
                }

                result = ExecuteNonQuery(Query);
            }
            catch
            {

            }
            return result;
        }
        public int CheckCustomer(string CustomerName)
        {
            int result = 0;
            try
            {
                string query = "Select sysID from tblCustomerTenant where OwnerName = '" + CustomerName + "' and isActive = 1";
                string ID = GetRecords(query).Rows[0][0].ToString();
                result = int.Parse(ID);
            }
            catch
            {
            }
            return result;
        }
        public List<string> GetProfileInfo(string Field)
        {
            List<string> Result = new List<string>();
            try
            {
                DataTable dtResult = GetRecords("select " + Field + " from tblCustomerTenant where isActive = 1 order by  "+ Field + " Asc");

                Result.Clear();

                foreach (DataRow dr in dtResult.Rows)
                {
                    Result.Add(dr[0].ToString());
                }
            }
            catch
            {
            }
            return Result;
        }
    }
}
