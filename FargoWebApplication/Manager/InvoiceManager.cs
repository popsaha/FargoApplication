using Fargo_Application.App_Start;
using Fargo_DataAccessLayers;
using Fargo_Models;
using FargoWebApplication.Filter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace FargoWebApplication.Manager
{
    public class InvoiceManager
    {
        public static int ReprintInvoiceRequest(InvoiceModel invoiceModel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@CASHIER_ID", invoiceModel.USER_ID);
                SqlParameter sp2 = new SqlParameter("@TRANSACTION_ID", invoiceModel.TRANSACTION_ID);
                SqlParameter sp3 = new SqlParameter("@FLAG", "1");
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spInvoice", sp1, sp2, sp3);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }


        public static List<InvoiceModel> LstCashierReprintRequest(InvoiceModel _invoiceModel)
        {
            List<InvoiceModel> LstCashierReprintRequest = new List<InvoiceModel>();
            try
            {

                SqlParameter sp1 = new SqlParameter("@MANAGER_ID", _invoiceModel.USER_ID);
                SqlParameter sp2 = new SqlParameter("@FROM_DATE", string.IsNullOrEmpty(_invoiceModel.FROM_DATE) ? "01-01-2022" : ConvertDateFormat.ConvertMMDDYYYY(_invoiceModel.FROM_DATE));
                SqlParameter sp3 = new SqlParameter("@TO_DATE", string.IsNullOrEmpty(_invoiceModel.TO_DATE) ? "01-01-2022" : ConvertDateFormat.ConvertMMDDYYYY(_invoiceModel.TO_DATE));
                SqlParameter sp4 = new SqlParameter("@FLAG", "2");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spInvoice", sp1, sp2, sp3, sp4);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        InvoiceModel invoiceModel = new InvoiceModel();
                        invoiceModel.REPRINT_INVOICE_RECEIPT_ID = Convert.ToInt64(sqlDataReader["REPRINT_INVOICE_RECEIPT_ID"].ToString());
                        invoiceModel.TRANSACTION_ID = sqlDataReader["TRANSACTION_ID"].ToString();
                        invoiceModel.CASHIER_NAME = sqlDataReader["CASHIER_NAME"].ToString();
                        invoiceModel.CASHIER_REQUESTED_ON = sqlDataReader["CASHIER_REQUESTED_ON"].ToString();
                        invoiceModel.STATUS = sqlDataReader["STATUS"].ToString();
                        LstCashierReprintRequest.Add(invoiceModel);
                    }
                }
                
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstCashierReprintRequest;
        }

        public static int InvoicePrintSettlement(InvoiceModel invoiceModel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@MANAGER_ID", invoiceModel.USER_ID);
                SqlParameter sp2 = new SqlParameter("@IS_MANAGER_APPROVED", invoiceModel.IS_MANAGER_APPROVED);
                SqlParameter sp3 = new SqlParameter("@MANAGER_REMARK", invoiceModel.MANAGER_REMARK);
                SqlParameter sp4 = new SqlParameter("@REPRINT_INVOICE_RECEIPT_ID", invoiceModel.REPRINT_INVOICE_RECEIPT_ID);
                SqlParameter sp5 = new SqlParameter("@FLAG", "3");
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spInvoice", sp1, sp2, sp3, sp4, sp5);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }



        public static List<InvoiceModel> LstReprintInvoiceResponse(string USER_ID, string PAGE_NUMBER, out bool IS_NEXT)
        {
            List<InvoiceModel> LstReprintInvoiceResponse = new List<InvoiceModel>();
            IS_NEXT = false;
            try
            {
                
                SqlParameter sp1 = new SqlParameter("@CASHIER_ID", USER_ID);
                SqlParameter sp2 = new SqlParameter("@PAGE_NUMBER", PAGE_NUMBER);
                SqlParameter sp3 = new SqlParameter("@FLAG", "4");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spInvoice", sp1, sp2, sp3);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        InvoiceModel invoiceModel = new InvoiceModel();
                        invoiceModel.REPRINT_INVOICE_RECEIPT_ID = Convert.ToInt64(sqlDataReader["REPRINT_INVOICE_RECEIPT_ID"].ToString());
                        invoiceModel.USER_ID = Convert.ToInt64(sqlDataReader["CASHIER_ID"].ToString());                      
                        invoiceModel.CASHIER_ID = Convert.ToInt64(sqlDataReader["CASHIER_ID"].ToString());                       
                        invoiceModel.TRANSACTION_ID = sqlDataReader["TRANSACTION_ID"].ToString();
                        invoiceModel.CASHIER_NAME = sqlDataReader["CASHIER_NAME"].ToString();
                        invoiceModel.CASHIER_REQUESTED_ON = sqlDataReader["CASHIER_REQUESTED_ON"].ToString();
                        invoiceModel.STATUS = sqlDataReader["STATUS"].ToString();
                        IS_NEXT = Convert.ToBoolean(sqlDataReader["IS_NEXT"].ToString());
                        LstReprintInvoiceResponse.Add(invoiceModel);
                    }
                }

            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstReprintInvoiceResponse;
        }

        public static ETRFileLocationModel ReprintInvoice(string USER_ID, string REPRINT_INVOICE_RECEIPT_ID)
        {
            ETRFileLocationModel ETRFileLocation = new ETRFileLocationModel();
            try
            {
                SqlParameter sp1 = new SqlParameter("@REPRINT_INVOICE_RECEIPT_ID", REPRINT_INVOICE_RECEIPT_ID);
                SqlParameter sp2 = new SqlParameter("@CASHIER_ID", USER_ID);
                SqlParameter sp3 = new SqlParameter("@FLAG", "5");
                DataTable dataTable = clsDataAccess.ExecuteDataTable(CommandType.StoredProcedure, "spInvoice", sp1, sp2, sp3);
                if (dataTable != null && dataTable.Rows.Count>0)
                {
                    ETRFileLocation.TransactionId = dataTable.Rows[0]["TRANSACTION_ID"].ToString();
                    ETRFileLocation.FileLocation = dataTable.Rows[0]["FILE_LOCATION"].ToString();
                    ETRFileLocation.Status = "Success";
                    ETRFileLocation.Message = "File found for TransactionId " + dataTable.Rows[0]["TRANSACTION_ID"].ToString();
                }
                else
                {
                    ETRFileLocation.TransactionId = null;
                    ETRFileLocation.FileLocation = null;
                    ETRFileLocation.Status = "Success";
                    ETRFileLocation.Message = "File not found for TransactionId " + dataTable.Rows[0]["TRANSACTION_ID"].ToString();
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return ETRFileLocation;
        }
    }
}