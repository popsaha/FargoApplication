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
    public class TransactionCancelManager
    {

        public static List<TransactionCancelModel> LstTransactionByTransactionId(long CASHIER_ID, string TRANSACTION_ID)
        {
            List<TransactionCancelModel> LstTransactionCancelModel = new List<TransactionCancelModel>();
            SqlParameter sp1 = new SqlParameter("@CASHIER_ID", CASHIER_ID);
            SqlParameter sp2 = new SqlParameter("@TRANSACTION_ID", TRANSACTION_ID);
            SqlParameter sp3 = new SqlParameter("@FLAG", '1');  
            try
            {
                DataTable dataTable = clsDataAccess.ExecuteDataTable(CommandType.StoredProcedure, "spCancelTransaction", sp1, sp2, sp3);
                if (dataTable.Rows.Count > 0 && dataTable.Rows != null)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        TransactionCancelModel transactionCancelModel = new TransactionCancelModel();

                        transactionCancelModel.TRANSACTION_ID = dataTable.Rows[i]["TRANSACTION_ID"].ToString();
                        transactionCancelModel.BOOKING_TRANSACTION_ID = Convert.ToInt64(dataTable.Rows[i]["BOOKING_TRANSACTION_ID"]);
                        transactionCancelModel.WAYBILL_NO = dataTable.Rows[i]["WAYBILL_NO"].ToString();
                        transactionCancelModel.TOTAL_AMOUNT = Convert.ToDouble(dataTable.Rows[i]["TOTAL_AMOUNT"]);

                        LstTransactionCancelModel.Add(transactionCancelModel);
                    }
                };
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstTransactionCancelModel;
        }


        public static CancelTransactionByWaybillModel TransactionByWaybillNo(long CASHIER_ID, string WAYBILL_NO)
        {
            CancelTransactionByWaybillModel cancelTransactionByWaybillModel = new CancelTransactionByWaybillModel();
            SqlParameter sp1 = new SqlParameter("@CASHIER_ID", CASHIER_ID);
            SqlParameter sp2 = new SqlParameter("@WAYBILL_NO", WAYBILL_NO);
            SqlParameter sp3 = new SqlParameter("@FLAG", '2');
            try
            {
                DataTable dataTable = clsDataAccess.ExecuteDataTable(CommandType.StoredProcedure, "spCancelTransaction", sp1, sp2, sp3);
                if (dataTable.Rows.Count > 0 && dataTable.Rows != null)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        cancelTransactionByWaybillModel.TRANSACTION_ID = dataTable.Rows[i]["TRANSACTION_ID"].ToString();
                        cancelTransactionByWaybillModel.BOOKING_TRANSACTION_ID = Convert.ToInt64(dataTable.Rows[i]["BOOKING_TRANSACTION_ID"]);
                        cancelTransactionByWaybillModel.WAYBILL_NO = dataTable.Rows[i]["WAYBILL_NO"].ToString();
                        cancelTransactionByWaybillModel.TOTAL_AMOUNT = Convert.ToDouble(dataTable.Rows[i]["TOTAL_AMOUNT"]);
                        cancelTransactionByWaybillModel.CASHIER_ID = Convert.ToInt64(dataTable.Rows[i]["CASHIER_ID"].ToString());
                        cancelTransactionByWaybillModel.DATE = Convert.ToDateTime(dataTable.Rows[i]["DATE"]);
                    }
                };
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return cancelTransactionByWaybillModel;
        }

        public static List<CancelTransactionModel> LstCancelTransactions(CancelTransactionModel _cancelTransactionModel)
        {
            List<CancelTransactionModel> LstCancelTransactions = new List<CancelTransactionModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@MANAGER_ID", _cancelTransactionModel.USER_ID);
                SqlParameter sp2 = new SqlParameter("@FROM_DATE", string.IsNullOrEmpty(_cancelTransactionModel.FROM_DATE) ? "01-01-2022" : ConvertDateFormat.ConvertMMDDYYYY(_cancelTransactionModel.FROM_DATE));
                SqlParameter sp3 = new SqlParameter("@TO_DATE", string.IsNullOrEmpty(_cancelTransactionModel.TO_DATE) ? "01-01-2022" : ConvertDateFormat.ConvertMMDDYYYY(_cancelTransactionModel.TO_DATE));
                SqlParameter sp4 = new SqlParameter("@FLAG", "5");

                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spCancelTransaction", sp1, sp2, sp3, sp4);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        CancelTransactionModel cancelTransactionModel = new CancelTransactionModel();

                        cancelTransactionModel.CANCEL_BOOKING_TRANSACTION_ID = Convert.ToInt64(sqlDataReader["CANCEL_BOOKING_TRANSACTION_ID"].ToString());
                        cancelTransactionModel.CASHIER_NAME = sqlDataReader["CASHIER_NAME"].ToString();
                        cancelTransactionModel.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();
                        cancelTransactionModel.TRANSACTION_ID = sqlDataReader["TRANSACTION_ID"].ToString();
                        cancelTransactionModel.CANCEL_AMOUNT = Convert.ToInt64(sqlDataReader["CANCEL_AMOUNT"].ToString());
                        cancelTransactionModel.DATE = sqlDataReader["DATE"].ToString();
                        cancelTransactionModel.REASON = sqlDataReader["REASON"].ToString();
                        cancelTransactionModel.STATUS = sqlDataReader["STATUS"].ToString();

                        LstCancelTransactions.Add(cancelTransactionModel);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstCancelTransactions;
        }

        public static int CancelTransactionSettlement(CancelTransactionModel cancelTransactionModel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@MANAGER_ID", cancelTransactionModel.USER_ID);
                SqlParameter sp2 = new SqlParameter("@IS_MANAGER_APPROVED", cancelTransactionModel.IS_MANAGER_APPROVED);
                SqlParameter sp3 = new SqlParameter("@MANAGER_REMARK", cancelTransactionModel.MANAGER_REMARK);
                SqlParameter sp4 = new SqlParameter("@CANCEL_BOOKING_TRANSACTION_ID", cancelTransactionModel.CANCEL_BOOKING_TRANSACTION_ID);
                SqlParameter sp5 = new SqlParameter("@FLAG", "6");
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spCancelTransaction", sp1, sp2, sp3, sp4, sp5);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }



        public static List<CancelTransactionModel> CancelTransactionReport(CancelTransactionModel _cancelTransactionModel)
        {
            List<CancelTransactionModel> LstCancelTransactions = new List<CancelTransactionModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@MANAGER_ID", _cancelTransactionModel.USER_ID);
                SqlParameter sp2 = new SqlParameter("@FROM_DATE", string.IsNullOrEmpty(_cancelTransactionModel.FROM_DATE) ? "01-01-2022" : ConvertDateFormat.ConvertMMDDYYYY(_cancelTransactionModel.FROM_DATE));
                SqlParameter sp3 = new SqlParameter("@TO_DATE", string.IsNullOrEmpty(_cancelTransactionModel.TO_DATE) ? "01-01-2022" : ConvertDateFormat.ConvertMMDDYYYY(_cancelTransactionModel.TO_DATE));
                SqlParameter sp4 = new SqlParameter("@FLAG", "7");

                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spCancelTransaction", sp1, sp2, sp3, sp4);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        CancelTransactionModel cancelTransactionModel = new CancelTransactionModel();

                        cancelTransactionModel.CANCEL_BOOKING_TRANSACTION_ID = Convert.ToInt64(sqlDataReader["CANCEL_BOOKING_TRANSACTION_ID"].ToString());
                        cancelTransactionModel.CASHIER_NAME = sqlDataReader["CASHIER_NAME"].ToString();
                        cancelTransactionModel.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();
                        cancelTransactionModel.TRANSACTION_ID = sqlDataReader["TRANSACTION_ID"].ToString();
                        cancelTransactionModel.CANCEL_AMOUNT = Convert.ToInt64(sqlDataReader["CANCEL_AMOUNT"].ToString());
                        cancelTransactionModel.DATE = sqlDataReader["DATE"].ToString();
                        cancelTransactionModel.REASON = sqlDataReader["REASON"].ToString();
                        cancelTransactionModel.STATUS = sqlDataReader["STATUS"].ToString();

                        LstCancelTransactions.Add(cancelTransactionModel);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstCancelTransactions;
        }

    }


}