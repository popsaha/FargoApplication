using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Fargo_DataAccessLayers;
using Fargo_Models;
using Fargo_Application.App_Start;


namespace FargoWebApplication.Manager
{
    public class ETRTransactionManager
    {
        public static ETRFileLocationModel ETRFileLocation(string TRANSACTION_ID)
        {
            ETRFileLocationModel ETRFileLocation = new ETRFileLocationModel();
            try
            {
                SqlParameter sp1 = new SqlParameter("@TRANSACTION_ID", TRANSACTION_ID);
                SqlParameter sp2 = new SqlParameter("@FLAG", "2");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spETRTransaction", sp1, sp2);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        ETRFileLocation.TransactionId = sqlDataReader["TRANSACTION_ID"].ToString();
                        ETRFileLocation.FileLocation = sqlDataReader["FILE_LOCATION"].ToString();
                        ETRFileLocation.Status = "Success";
                        ETRFileLocation.Message = "File found for TransactionId " + TRANSACTION_ID;
                    }
                }
                else
                {
                    ETRFileLocation.TransactionId = null;
                    ETRFileLocation.FileLocation = null;
                    ETRFileLocation.Status = "Success";
                    ETRFileLocation.Message = "File not found for TransactionId " + TRANSACTION_ID;          
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return ETRFileLocation;
        }

        public static List<ETRInvoiceModel> LstTransactionId(string PAGE_NUMBER, out bool IsNext)
        {
            List<ETRInvoiceModel> LstTransactionId = new List<ETRInvoiceModel>();
            IsNext = false;
            try
            {
                SqlParameter sp1 = new SqlParameter("@PAGE_NUMBER", PAGE_NUMBER);
                SqlParameter sp2 = new SqlParameter("@FLAG", "3");
                DataTable dataTable = clsDataAccess.ExecuteDataTable(CommandType.StoredProcedure, "spETRTransaction", sp1, sp2);
                if (dataTable != null && dataTable.Rows.Count>0)
                {
                    IsNext = Convert.ToBoolean(dataTable.Rows[0]["IS_NEXT"].ToString());
                    LstTransactionId = (from DataRow dataRow in dataTable.Rows
                                        select new ETRInvoiceModel()
                                        {
                                            TRANSACTION_ID = dataRow["TRANSACTION_ID"].ToString()
                                        }).ToList();
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstTransactionId;
        }
    }
}