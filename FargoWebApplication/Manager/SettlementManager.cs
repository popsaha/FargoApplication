using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Fargo_DataAccessLayers;
using Fargo_Models;
using Fargo_Application.App_Start;
using FargoWebApplication.Filter;

namespace FargoWebApplication.Manager
{
    public class SettlementManager
    {

        public static List<SettlementModel> SettlementInfo(long MANAGER_ID)
        {
            List<SettlementModel> LstSettlement = new List<SettlementModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@MANAGER_ID", MANAGER_ID);             
                SqlParameter sp2 = new SqlParameter("@FLAG", "1");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spSettlement", sp1,sp2);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        SettlementModel _settlementModel = new SettlementModel();

                        _settlementModel.DAY_IN_END_TRANSACTION_ID = Convert.ToInt64(sqlDataReader["DAY_IN_END_TRANSACTION_ID"].ToString());
                        _settlementModel.STORE_ID = Convert.ToInt64(sqlDataReader["STORE_ID"].ToString());
                        _settlementModel.CASHIER_ID = Convert.ToInt64(sqlDataReader["CASHIER_ID"].ToString());
                        _settlementModel.DAY_IN_TIME = sqlDataReader["DAY_IN_TIME"].ToString();
                        _settlementModel.TOTAL_DAY_IN_AMOUNT = Convert.ToDouble(sqlDataReader["TOTAL_DAY_IN_AMOUNT"].ToString());
                        _settlementModel.TOTAL_CASH_AMOUNT = Convert.ToDouble(sqlDataReader["TOTAL_CASH_AMOUNT"].ToString());
                        _settlementModel.TOTAL_MPESA_AMOUNT =Convert.ToDouble(sqlDataReader["TOTAL_MPESA_AMOUNT"].ToString());
                        _settlementModel.TOTAL_CREDIT_AMOUNT = Convert.ToDouble(sqlDataReader["TOTAL_CREDIT_AMOUNT"].ToString());
                        _settlementModel.NO_OF_CASH_TRANSACTION = Convert.ToInt32(sqlDataReader["NO_OF_CASH_TRANSACTION"].ToString());
                        _settlementModel.NO_OF_MPESA_TRANSACTION = Convert.ToInt32(sqlDataReader["NO_OF_MPESA_TRANSACTION"].ToString());
                        _settlementModel.NO_OF_CREDIT_TRANSACTION = Convert.ToInt32(sqlDataReader["NO_OF_CREDIT_TRANSACTION"].ToString());
                        _settlementModel.TOTAL_DAY_END_AMOUNT = Convert.ToDouble(sqlDataReader["TOTAL_DAY_END_AMOUNT"].ToString());
                        _settlementModel.DAY_END_TIME = sqlDataReader["DAY_END_TIME"].ToString();
                        _settlementModel.MANAGER_ID = Convert.ToInt64(sqlDataReader["MANAGER_ID"].ToString());
                        _settlementModel.STATUS = sqlDataReader["STATUS"].ToString();
                        _settlementModel.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();
                        _settlementModel.CASHIER_NAME = sqlDataReader["CASHIER_NAME"].ToString();
                        _settlementModel.MANAGER_NAME = sqlDataReader["MANAGER_NAME"].ToString();
                        _settlementModel.DATE = sqlDataReader["DATE"].ToString();
                        _settlementModel.SEARCH_DATE = sqlDataReader["SEARCH_DATE"].ToString();

                        LstSettlement.Add(_settlementModel);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstSettlement;
        }
        public static List<SettlementModel> SettlementInfo(SettlementModel settlementModel)
        {
            List<SettlementModel> LstSettlement = new List<SettlementModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@MANAGER_ID",settlementModel.USER_ID);
                SqlParameter sp2 = new SqlParameter("@STORE_ID",settlementModel.STORE_ID);
                SqlParameter sp3 = new SqlParameter("@FROM_DATE", string.IsNullOrEmpty(settlementModel.FROM_DATE) ? ConvertDateFormat.ConvertMMDDYYYY(settlementModel.FROM_DATE) : ConvertDateFormat.ConvertMMDDYYYY(settlementModel.FROM_DATE));
                SqlParameter sp4 = new SqlParameter("@TO_DATE", string.IsNullOrEmpty(settlementModel.TO_DATE) ? ConvertDateFormat.ConvertMMDDYYYY(settlementModel.TO_DATE) : ConvertDateFormat.ConvertMMDDYYYY(settlementModel.TO_DATE));              
                SqlParameter sp5 = new SqlParameter("@FLAG", "2");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spSettlement", sp1,sp2, sp3, sp4,sp5);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        SettlementModel _settlementModel = new SettlementModel();

                        _settlementModel.DAY_IN_END_TRANSACTION_ID = Convert.ToInt64(sqlDataReader["DAY_IN_END_TRANSACTION_ID"].ToString());
                        _settlementModel.STORE_ID = Convert.ToInt64(sqlDataReader["STORE_ID"].ToString());
                        _settlementModel.CASHIER_ID = Convert.ToInt64(sqlDataReader["CASHIER_ID"].ToString());
                        _settlementModel.DAY_IN_TIME = sqlDataReader["DAY_IN_TIME"].ToString();
                        _settlementModel.TOTAL_DAY_IN_AMOUNT = Convert.ToDouble(sqlDataReader["TOTAL_DAY_IN_AMOUNT"].ToString());
                        _settlementModel.TOTAL_CASH_AMOUNT = Convert.ToDouble(sqlDataReader["TOTAL_CASH_AMOUNT"].ToString());
                        _settlementModel.TOTAL_MPESA_AMOUNT =Convert.ToDouble(sqlDataReader["TOTAL_MPESA_AMOUNT"].ToString());
                        _settlementModel.TOTAL_CREDIT_AMOUNT = Convert.ToDouble(sqlDataReader["TOTAL_CREDIT_AMOUNT"].ToString());
                        _settlementModel.NO_OF_CASH_TRANSACTION = Convert.ToInt32(sqlDataReader["NO_OF_CASH_TRANSACTION"].ToString());
                        _settlementModel.NO_OF_MPESA_TRANSACTION = Convert.ToInt32(sqlDataReader["NO_OF_MPESA_TRANSACTION"].ToString());
                        _settlementModel.NO_OF_CREDIT_TRANSACTION = Convert.ToInt32(sqlDataReader["NO_OF_CREDIT_TRANSACTION"].ToString());
                        _settlementModel.TOTAL_DAY_END_AMOUNT = Convert.ToDouble(sqlDataReader["TOTAL_DAY_END_AMOUNT"].ToString());
                        _settlementModel.DAY_END_TIME = sqlDataReader["DAY_END_TIME"].ToString();
                        _settlementModel.MANAGER_ID = Convert.ToInt64(sqlDataReader["MANAGER_ID"].ToString());
                        _settlementModel.STATUS = sqlDataReader["STATUS"].ToString();
                        _settlementModel.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();
                        _settlementModel.CASHIER_NAME = sqlDataReader["CASHIER_NAME"].ToString();
                        _settlementModel.MANAGER_NAME = sqlDataReader["MANAGER_NAME"].ToString();
                        _settlementModel.DATE = sqlDataReader["DATE"].ToString();
                        _settlementModel.SEARCH_DATE = sqlDataReader["SEARCH_DATE"].ToString();
                        _settlementModel.IS_MANAGER_APPROVED = sqlDataReader["IS_MANAGER_APPROVED"].ToString();

                        LstSettlement.Add(_settlementModel);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstSettlement;
        }

        public static SettlementModel SettlementInfo(string DAY_IN_END_TRANSACTION_ID)
        {
            SettlementModel _settlementModel = new SettlementModel();
            try
            {
                SqlParameter sp1 = new SqlParameter("@DAY_IN_END_TRANSACTION_ID", DAY_IN_END_TRANSACTION_ID);
                SqlParameter sp2 = new SqlParameter("@FLAG", "3");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spSettlement", sp1, sp2);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        _settlementModel.DAY_IN_END_TRANSACTION_ID = Convert.ToInt64(sqlDataReader["DAY_IN_END_TRANSACTION_ID"].ToString());
                        _settlementModel.STORE_ID = Convert.ToInt64(sqlDataReader["STORE_ID"].ToString());
                        _settlementModel.CASHIER_ID = Convert.ToInt64(sqlDataReader["CASHIER_ID"].ToString());
                        _settlementModel.DAY_IN_TIME = sqlDataReader["DAY_IN_TIME"].ToString();
                        _settlementModel.TOTAL_DAY_IN_AMOUNT = Convert.ToDouble(sqlDataReader["TOTAL_DAY_IN_AMOUNT"].ToString());
                        _settlementModel.TOTAL_CASH_AMOUNT = Convert.ToDouble(sqlDataReader["TOTAL_CASH_AMOUNT"].ToString());
                        _settlementModel.TOTAL_MPESA_AMOUNT = Convert.ToDouble(sqlDataReader["TOTAL_MPESA_AMOUNT"].ToString());
                        _settlementModel.TOTAL_CREDIT_AMOUNT = Convert.ToDouble(sqlDataReader["TOTAL_CREDIT_AMOUNT"].ToString());
                        _settlementModel.NO_OF_CASH_TRANSACTION = Convert.ToInt32(sqlDataReader["NO_OF_CASH_TRANSACTION"].ToString());
                        _settlementModel.NO_OF_MPESA_TRANSACTION = Convert.ToInt32(sqlDataReader["NO_OF_MPESA_TRANSACTION"].ToString());
                        _settlementModel.NO_OF_CREDIT_TRANSACTION = Convert.ToInt32(sqlDataReader["NO_OF_CREDIT_TRANSACTION"].ToString());
                        _settlementModel.TOTAL_DAY_END_AMOUNT = Convert.ToDouble(sqlDataReader["TOTAL_DAY_END_AMOUNT"].ToString());
                        _settlementModel.DAY_END_TIME = sqlDataReader["DAY_END_TIME"].ToString();
                        _settlementModel.MANAGER_ID = Convert.ToInt64(sqlDataReader["MANAGER_ID"].ToString());
                        _settlementModel.STATUS = sqlDataReader["STATUS"].ToString();
                        _settlementModel.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();
                        _settlementModel.CASHIER_NAME = sqlDataReader["CASHIER_NAME"].ToString();
                        _settlementModel.MANAGER_NAME = sqlDataReader["MANAGER_NAME"].ToString();
                        _settlementModel.DATE = sqlDataReader["DATE"].ToString();
                        _settlementModel.SEARCH_DATE = sqlDataReader["SEARCH_DATE"].ToString();
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return _settlementModel;
        }

        public static int Approval(SettlementModel settlementModel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@MANAGER_ID", settlementModel.MANAGER_ID);
                SqlParameter sp2 = new SqlParameter("@IS_MANAGER_APPROVED", settlementModel.IS_MANAGER_APPROVED);
                SqlParameter sp3 = new SqlParameter("@MANAGER_REMARK", settlementModel.MANAGER_REMARK);
                SqlParameter sp4 = new SqlParameter("@DAY_IN_END_TRANSACTION_ID", settlementModel.DAY_IN_END_TRANSACTION_ID);
                SqlParameter sp5 = new SqlParameter("@FLAG", "4");
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spSettlement", sp1, sp2, sp3, sp4, sp5);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        public static int VoidSettlement(VoidTrackingTransactionModel voidTrackingTransactionModel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@MANAGER_ID", voidTrackingTransactionModel.MANAGER_ID);
                SqlParameter sp2 = new SqlParameter("@IS_MANAGER_APPROVED", voidTrackingTransactionModel.IS_MANAGER_APPROVED);
                SqlParameter sp3 = new SqlParameter("@MANAGER_REMARK", voidTrackingTransactionModel.MANAGER_REMARK);
                SqlParameter sp4 = new SqlParameter("@VOID_TRACKING_TRANSACTION_ID", voidTrackingTransactionModel.VOID_TRACKING_TRANSACTION_ID);
                SqlParameter sp5 = new SqlParameter("@FLAG", "1");
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spVoidSettlement", sp1, sp2, sp3, sp4, sp5);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        public static List<VoidTrackingTransactionModel> VoidSettlementInfo(VoidTrackingTransactionModel voidTrackingTransactionModel)
        {
            List<VoidTrackingTransactionModel> LstVoidTransactionRequest = new List<VoidTrackingTransactionModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@MANAGER_ID", voidTrackingTransactionModel.USER_ID);
                SqlParameter sp2 = new SqlParameter("@FROM_DATE", !string.IsNullOrEmpty(voidTrackingTransactionModel.FROM_DATE)?ConvertDateFormat.ConvertMMDDYYYY(voidTrackingTransactionModel.FROM_DATE):"01-01-2000");
                SqlParameter sp3 = new SqlParameter("@TO_DATE", !string.IsNullOrEmpty(voidTrackingTransactionModel.TO_DATE) ? ConvertDateFormat.ConvertMMDDYYYY(voidTrackingTransactionModel.TO_DATE) : "01-01-2000");
                SqlParameter sp4 = new SqlParameter("@FLAG", "2");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spVoidSettlement", sp1, sp2, sp3, sp4);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        VoidTrackingTransactionModel voidTrackingTransaction = new VoidTrackingTransactionModel();

                        voidTrackingTransaction.VOID_TRACKING_TRANSACTION_ID = Convert.ToInt64(sqlDataReader["VOID_TRACKING_TRANSACTION_ID"].ToString());
                        voidTrackingTransaction.STORE_ID = Convert.ToInt64(sqlDataReader["STORE_ID"].ToString());
                        voidTrackingTransaction.CASHIER_ID = Convert.ToInt64(sqlDataReader["CASHIER_ID"].ToString());
                        voidTrackingTransaction.MANAGER_ID = Convert.ToInt64(sqlDataReader["MANAGER_ID"].ToString());
                        voidTrackingTransaction.IS_CASHIER_APPROVED = Convert.ToBoolean(sqlDataReader["IS_CASHIER_APPROVED"].ToString());
                        voidTrackingTransaction.IS_MANAGER_APPROVED = Convert.ToBoolean(sqlDataReader["IS_MANAGER_APPROVED"].ToString());
                        voidTrackingTransaction.CASHIER_NAME = sqlDataReader["CASHIER_NAME"].ToString();
                        voidTrackingTransaction.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();
                        voidTrackingTransaction.TRACKING_NUMBER = sqlDataReader["TRACKING_NUMBER"].ToString();
                        voidTrackingTransaction.CASHIER_REMARK = sqlDataReader["CASHIER_REMARK"].ToString();
                        voidTrackingTransaction.MANAGER_NAME = sqlDataReader["MANAGER_NAME"].ToString();
                        voidTrackingTransaction.MANAGER_REMARK = sqlDataReader["MANAGER_REMARK"].ToString();
                        voidTrackingTransaction.PAGE_NUMBER = sqlDataReader["MANAGER_STATUS"].ToString();
                        voidTrackingTransaction.STATUS = sqlDataReader["STATUS"].ToString();
                        voidTrackingTransaction.CASHIER_APPROVED = sqlDataReader["CASHIER_APPROVED"].ToString();
                        voidTrackingTransaction.MANAGER_APPROVED = sqlDataReader["MANAGER_APPROVED"].ToString();
                        voidTrackingTransaction.REQUESTED_DATE = sqlDataReader["REQUESTED_DATE"].ToString();
                        voidTrackingTransaction.MANAGER_RESPONDED_ON = sqlDataReader["MANAGER_RESPONDED_ON"].ToString();
                        voidTrackingTransaction.CREDIT_NOTE_AMOUNT = Convert.ToDouble(sqlDataReader["CREDIT_NOTE_AMOUNT"].ToString());
                        voidTrackingTransaction.REASON = sqlDataReader["REASON"].ToString();

                        LstVoidTransactionRequest.Add(voidTrackingTransaction);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstVoidTransactionRequest;
        }

        public static List<VoidTrackingTransactionModel> VoidSettlementReport(VoidTrackingTransactionModel voidTrackingTransactionModel)
        {
            List<VoidTrackingTransactionModel> LstVoidTransactionRequest = new List<VoidTrackingTransactionModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@MANAGER_ID", voidTrackingTransactionModel.USER_ID);
                SqlParameter sp2 = new SqlParameter("@FROM_DATE", !string.IsNullOrEmpty(voidTrackingTransactionModel.FROM_DATE) ? ConvertDateFormat.ConvertMMDDYYYY(voidTrackingTransactionModel.FROM_DATE) : "01-01-2000");
                SqlParameter sp3 = new SqlParameter("@TO_DATE", !string.IsNullOrEmpty(voidTrackingTransactionModel.TO_DATE) ? ConvertDateFormat.ConvertMMDDYYYY(voidTrackingTransactionModel.TO_DATE) : "01-01-2000");
                SqlParameter sp4 = new SqlParameter("@FLAG", "3");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spVoidSettlement", sp1, sp2, sp3, sp4);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        VoidTrackingTransactionModel voidTrackingTransaction = new VoidTrackingTransactionModel();

                        voidTrackingTransaction.VOID_TRACKING_TRANSACTION_ID = Convert.ToInt64(sqlDataReader["VOID_TRACKING_TRANSACTION_ID"].ToString());
                        voidTrackingTransaction.STORE_ID = Convert.ToInt64(sqlDataReader["STORE_ID"].ToString());
                        voidTrackingTransaction.CASHIER_ID = Convert.ToInt64(sqlDataReader["CASHIER_ID"].ToString());
                        voidTrackingTransaction.MANAGER_ID = Convert.ToInt64(sqlDataReader["MANAGER_ID"].ToString());
                        voidTrackingTransaction.IS_CASHIER_APPROVED = Convert.ToBoolean(sqlDataReader["IS_CASHIER_APPROVED"].ToString());
                        voidTrackingTransaction.IS_MANAGER_APPROVED = Convert.ToBoolean(sqlDataReader["IS_MANAGER_APPROVED"].ToString());
                        voidTrackingTransaction.CASHIER_NAME = sqlDataReader["CASHIER_NAME"].ToString();
                        voidTrackingTransaction.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();
                        voidTrackingTransaction.TRACKING_NUMBER = sqlDataReader["TRACKING_NUMBER"].ToString();
                        voidTrackingTransaction.CASHIER_REMARK = sqlDataReader["CASHIER_REMARK"].ToString();
                        voidTrackingTransaction.MANAGER_NAME = sqlDataReader["MANAGER_NAME"].ToString();
                        voidTrackingTransaction.MANAGER_REMARK = sqlDataReader["MANAGER_REMARK"].ToString();
                        voidTrackingTransaction.PAGE_NUMBER = sqlDataReader["MANAGER_STATUS"].ToString();
                        voidTrackingTransaction.STATUS = sqlDataReader["STATUS"].ToString();
                        voidTrackingTransaction.CASHIER_APPROVED = sqlDataReader["CASHIER_APPROVED"].ToString();
                        voidTrackingTransaction.MANAGER_APPROVED = sqlDataReader["MANAGER_APPROVED"].ToString();
                        voidTrackingTransaction.REQUESTED_DATE = sqlDataReader["REQUESTED_DATE"].ToString();
                        voidTrackingTransaction.MANAGER_RESPONDED_ON = sqlDataReader["MANAGER_RESPONDED_ON"].ToString();
                        voidTrackingTransaction.CREDIT_NOTE_AMOUNT = Convert.ToDouble(sqlDataReader["CREDIT_NOTE_AMOUNT"].ToString());
                        voidTrackingTransaction.REASON = sqlDataReader["REASON"].ToString();

                        LstVoidTransactionRequest.Add(voidTrackingTransaction);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstVoidTransactionRequest;
        }
   
    }
}