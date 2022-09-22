using Fargo_Application.App_Start;
using Fargo_DataAccessLayers;
using Fargo_Models;
using FargoWebApplication.Filter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace FargoWebApplication.Manager
{
    public class CreditCustomerManager
    {
        #region Method for getting the list of Credit Customers
        public static List<CreditCustomerModel> LstCreditCustomers()
        {
            List<CreditCustomerModel> LstCreditCustomers = new List<CreditCustomerModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@FLAG", "1");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spCreditCustomer", sp1);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        CreditCustomerModel creditCustomer = new CreditCustomerModel();

                        creditCustomer.CUSTOMER_ID = Convert.ToInt64(sqlDataReader["CUSTOMER_ID"].ToString());
                        creditCustomer.CUSTOMER_CODE = sqlDataReader["CUSTOMER_CODE"].ToString();
                        creditCustomer.CUSTOMER_NAME = sqlDataReader["CUSTOMER_NAME"].ToString();
                        creditCustomer.COMPANY = sqlDataReader["COMPANY"].ToString();
                        creditCustomer.CUSTOMER_PIN = sqlDataReader["CUSTOMER_PIN"].ToString();
                        creditCustomer.PIN_CODE = sqlDataReader["PIN_CODE"].ToString();
                        creditCustomer.COUNTRY_ID = Convert.ToInt64(sqlDataReader["COUNTRY_ID"].ToString());
                        creditCustomer.STATE_ID = Convert.ToInt64(sqlDataReader["STATE_ID"].ToString());
                        creditCustomer.CITY = sqlDataReader["CITY"].ToString();
                        creditCustomer.ADDRESS = sqlDataReader["ADDRESS"].ToString();
                        creditCustomer.PHONE_NO = sqlDataReader["PHONE_NO"].ToString();
                        creditCustomer.EMAIL_ID = sqlDataReader["EMAIL_ID"].ToString();
                        creditCustomer.PASSWORD = sqlDataReader["PASSWORD"].ToString();
                        creditCustomer.CUSTOMER_COMMISSION = sqlDataReader["CUSTOMER_COMMISSION"].ToString();
                        creditCustomer.DATA_SOURCE = sqlDataReader["DATA_SOURCE"].ToString();

                        LstCreditCustomers.Add(creditCustomer);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstCreditCustomers;
        }
        #endregion

        #region Method for Credit Entry of Credit Customers
        public static int CreditEntryAmount(CreditEntryModel creditEntryModel)
        {
            int result = 0;
            try
            {
                SqlParameter customerId = new SqlParameter("@CUSTOMER_ID", creditEntryModel.CUSTOMER_ID);
                SqlParameter creditEntryAmount = new SqlParameter("@CREDIT_ENTRY_AMOUNT", creditEntryModel.CREDIT_ENTRY_AMOUNT);
                SqlParameter paymentMode = new SqlParameter("@PAYMENT_MODE", creditEntryModel.PAYMENT_MODE);
                if (creditEntryModel.PAYMENT_MODE == "CASH")
                {
                    creditEntryModel.PAYMENT_STATUS = "COMPLETED";
                }
                else
                {
                    creditEntryModel.PAYMENT_STATUS = "PENDING";
                }
                SqlParameter paymentStatus = new SqlParameter("@PAYMENT_STATUS", creditEntryModel.PAYMENT_STATUS);
                SqlParameter referenceNo = new SqlParameter("@REFERENCE_NO", creditEntryModel.REFERENCE_NO);
                SqlParameter bankName = new SqlParameter("@BANK_NAME", creditEntryModel.BANK_NAME);
                SqlParameter storeId = new SqlParameter("@STORE_ID", creditEntryModel.STORE_ID);
                SqlParameter cashierId = new SqlParameter("@CASHIER_ID", creditEntryModel.CASHIER_ID);

                SqlParameter flag = new SqlParameter("@FLAG", "1");

                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spCreditEntryInsert", customerId, creditEntryAmount, paymentMode, paymentStatus, referenceNo, bankName, storeId, cashierId, flag);

            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        #endregion

        public static List<CreditEntryModel> LstCreditEntry(CreditEntryModel creditEntryModel)
        {
            List<CreditEntryModel> LstCreditEntry = new List<CreditEntryModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@USER_ID", creditEntryModel.USER_ID);
                SqlParameter sp2 = new SqlParameter("@FROM_DATE", string.IsNullOrEmpty(creditEntryModel.FROM_DATE) ? ConvertDateFormat.ConvertMMDDYYYY(creditEntryModel.FROM_DATE) : ConvertDateFormat.ConvertMMDDYYYY(creditEntryModel.FROM_DATE));
                SqlParameter sp3 = new SqlParameter("@TO_DATE", string.IsNullOrEmpty(creditEntryModel.TO_DATE) ? ConvertDateFormat.ConvertMMDDYYYY(creditEntryModel.TO_DATE) : ConvertDateFormat.ConvertMMDDYYYY(creditEntryModel.TO_DATE));
                SqlParameter sp4 = new SqlParameter("@FLAG", "1");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spCreditEntry", sp1, sp2, sp3, sp4);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        CreditEntryModel _creditEntryModel = new CreditEntryModel();

                        _creditEntryModel.CREDIT_ENTRY_ID = Convert.ToInt64(sqlDataReader["CREDIT_ENTRY_ID"].ToString());
                        _creditEntryModel.ENTRY_DATE = sqlDataReader["ENTRY_DATE"].ToString();
                        _creditEntryModel.CUSTOMER_NAME = sqlDataReader["CUSTOMER_NAME"].ToString();
                        _creditEntryModel.CREDIT_ENTRY_AMOUNT = Convert.ToDouble(sqlDataReader["CREDIT_ENTRY_AMOUNT"].ToString());
                        _creditEntryModel.PAYMENT_MODE = sqlDataReader["PAYMENT_MODE"].ToString();
                        _creditEntryModel.REFERENCE_NO = Convert.ToInt64(sqlDataReader["REFERENCE_NO"].ToString());
                        _creditEntryModel.BANK_NAME = sqlDataReader["BANK_NAME"].ToString();
                        _creditEntryModel.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();
                        _creditEntryModel.CASHIER_NAME = sqlDataReader["CASHIER_NAME"].ToString();

                        LstCreditEntry.Add(_creditEntryModel);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstCreditEntry;
        }

        public static int ActionCreditEntry(CreditEntryModel creditEntryModel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@MANAGER_ID", creditEntryModel.MANAGER_ID);
                SqlParameter sp2 = new SqlParameter("@IS_MANAGER_APPROVED", creditEntryModel.IS_MANAGER_APPROVED);
                SqlParameter sp3 = new SqlParameter("@MANAGER_REMARK", creditEntryModel.MANAGER_REMARK);
                SqlParameter sp4 = new SqlParameter("@CREDIT_ENTRY_ID", creditEntryModel.CREDIT_ENTRY_ID);
                SqlParameter sp5 = new SqlParameter("@CREDIT_ENTRY_AMOUNT", creditEntryModel.CREDIT_ENTRY_AMOUNT);
                SqlParameter sp6 = new SqlParameter("@FLAG", "2");
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spCreditEntry", sp1, sp2, sp3, sp4, sp5, sp6);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }


        public static List<CreditEntryModel> CreditEntryReport(CreditEntryModel creditEntryModel)
        {
            List<CreditEntryModel> LstCreditEntry = new List<CreditEntryModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@USER_ID", creditEntryModel.USER_ID);
                SqlParameter sp2 = new SqlParameter("@FROM_DATE", string.IsNullOrEmpty(creditEntryModel.FROM_DATE) ? ConvertDateFormat.ConvertMMDDYYYY(creditEntryModel.FROM_DATE) : ConvertDateFormat.ConvertMMDDYYYY(creditEntryModel.FROM_DATE));
                SqlParameter sp3 = new SqlParameter("@TO_DATE", string.IsNullOrEmpty(creditEntryModel.TO_DATE) ? ConvertDateFormat.ConvertMMDDYYYY(creditEntryModel.TO_DATE) : ConvertDateFormat.ConvertMMDDYYYY(creditEntryModel.TO_DATE));
                SqlParameter sp4 = new SqlParameter("@FLAG", "3");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spCreditEntry", sp1, sp2, sp3, sp4);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        CreditEntryModel _creditEntryModel = new CreditEntryModel();

                        _creditEntryModel.CREDIT_ENTRY_ID = Convert.ToInt64(sqlDataReader["CREDIT_ENTRY_ID"].ToString());
                        _creditEntryModel.ENTRY_DATE = sqlDataReader["ENTRY_DATE"].ToString();
                        _creditEntryModel.CUSTOMER_NAME = sqlDataReader["CUSTOMER_NAME"].ToString();
                        _creditEntryModel.CREDIT_ENTRY_AMOUNT = Convert.ToDouble(sqlDataReader["CREDIT_ENTRY_AMOUNT"].ToString());
                        _creditEntryModel.PAYMENT_MODE = sqlDataReader["PAYMENT_MODE"].ToString();
                        _creditEntryModel.REFERENCE_NO = Convert.ToInt64(sqlDataReader["REFERENCE_NO"].ToString());
                        _creditEntryModel.BANK_NAME = sqlDataReader["BANK_NAME"].ToString();
                        _creditEntryModel.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();
                        _creditEntryModel.CASHIER_NAME = sqlDataReader["CASHIER_NAME"].ToString();
                        _creditEntryModel.STATUS = sqlDataReader["STATUS"].ToString();

                        LstCreditEntry.Add(_creditEntryModel);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstCreditEntry;
        }

    }
}