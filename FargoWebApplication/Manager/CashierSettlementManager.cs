using Fargo_Application.App_Start;
using Fargo_DataAccessLayers;
using Fargo_Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FargoWebApplication.Manager
{
    public class CashierSettlementManager
    {

        public static List<CashierSettlementModel> LstCashierSettlement(string DATE, long CASHIER_ID)
        {
            List<CashierSettlementModel> LstCashierSettlementModel = new List<CashierSettlementModel>();
            CashierSettlementModel cashierSettlementModel = new CashierSettlementModel();
            try
            {
                SqlParameter sp1 = new SqlParameter("@CASHIER_ID", CASHIER_ID);
                SqlParameter sp2 = new SqlParameter("@DATE", DATE);
                SqlParameter sp3 = new SqlParameter("@FLAG", '1');
                SqlParameter sp4 = new SqlParameter("@FLAG", '2');
                SqlParameter sp5 = new SqlParameter("@FLAG", '3');
                SqlParameter sp6 = new SqlParameter("@FLAG", '4');
                SqlParameter sp7 = new SqlParameter("@FLAG", '5');
                
                #region for SETTLEMENT_DETAIL
                List<SettlementDetailModel> LstCashierSettlement = new List<SettlementDetailModel>();
                SqlDataReader sqlDataReader1 = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spCashierSettlement", sp1, sp2, sp3);
                if (sqlDataReader1.HasRows)
                {
                    while (sqlDataReader1.Read())
                    {
                        SettlementDetailModel settlementDetailModel = new SettlementDetailModel();

                        settlementDetailModel.WAYBILL_NUMBER = sqlDataReader1["WAYBILL_NUMBER"].ToString();
                        settlementDetailModel.INVOICE_NUMBER = (sqlDataReader1["INVOICE_NUMBER"].ToString());
                        settlementDetailModel.TRANSACTION_ID = sqlDataReader1["TRANSACTION_ID"].ToString();
                        settlementDetailModel.TOTAL_AMOUNT = Convert.ToDouble(sqlDataReader1["TOTAL_AMOUNT"]);
                        settlementDetailModel.CASHIER_ID = Convert.ToInt64(sqlDataReader1["CASHIER_ID"].ToString());
                        settlementDetailModel.PAYMENT_MODE = sqlDataReader1["PAYMENT_MODE"].ToString();
                        LstCashierSettlement.Add(settlementDetailModel);

                         cashierSettlementModel.SETTLEMENT_DETAIL = LstCashierSettlement;
                    }
                }
                
                #endregion

                #region for Summary report
                List<DaySummaryReportModel> LstDaySummaryReport = new List<DaySummaryReportModel>();
                SqlDataReader sqlDataReader2 = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spCashierSettlement", sp1, sp2, sp4);
                if (sqlDataReader2.HasRows)
                {
                    while (sqlDataReader2.Read())
                    {
                        DaySummaryReportModel daySummaryReportModel = new DaySummaryReportModel();

                        daySummaryReportModel.NO_OF_MPESA_TRANSACTION = Convert.ToInt64(sqlDataReader2["NO_OF_MPESA_TRANSACTION"]);
                        daySummaryReportModel.TOTAL_MPESA_AMOUNT = Convert.ToDouble(sqlDataReader2["TOTAL_MPESA_AMOUNT"]);
                        daySummaryReportModel.NO_OF_CASH_TRANSACTION = Convert.ToInt64(sqlDataReader2["NO_OF_CASH_TRANSACTION"]);
                        daySummaryReportModel.TOTAL_CASH_AMOUNT = Convert.ToDouble(sqlDataReader2["TOTAL_CASH_AMOUNT"]);
                        daySummaryReportModel.NO_OF_CREDIT_TRANSACTION = Convert.ToInt64(sqlDataReader2["NO_OF_CREDIT_TRANSACTION"]);
                        daySummaryReportModel.TOTAL_CREDIT_AMOUNT = Convert.ToDouble(sqlDataReader2["TOTAL_CREDIT_AMOUNT"]);

                        LstDaySummaryReport.Add(daySummaryReportModel);
                        cashierSettlementModel.SUMMARY_REPORT = LstDaySummaryReport;
                    }
                }
              
                #endregion

                #region for Detail MPESA
                List<MPesaDetailModel> LstMPesaDetail = new List<MPesaDetailModel>();
                SqlDataReader SqlDataReader3 = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spCashierSettlement", sp1, sp2, sp5);
                if (SqlDataReader3.HasRows)
                {
                    while (SqlDataReader3.Read())
                    {
                        MPesaDetailModel mPesaDetailModel = new MPesaDetailModel();

                        mPesaDetailModel.BOOKING_TRANSACTION_ID = Convert.ToInt64(SqlDataReader3["BOOKING_TRANSACTION_ID"]);
                        mPesaDetailModel.WAYBILL_NUMBER = SqlDataReader3["WAYBILL_NUMBER"].ToString();
                        mPesaDetailModel.INVOICE_NUMBER = SqlDataReader3["INVOICE_NUMBER"].ToString();
                        mPesaDetailModel.TRANSACTION_ID = SqlDataReader3["TRANSACTION_ID"].ToString();
                        mPesaDetailModel.MPESA_AMOUNT = Convert.ToDouble(SqlDataReader3["MPESA_AMOUNT"]);
                        LstMPesaDetail.Add(mPesaDetailModel);

                        cashierSettlementModel.MPESA_DETAIL = LstMPesaDetail;
                    }
                }

                #endregion

                #region for DETAILCASH
                List<CashDetailModel> LstCashDetailModel = new List<CashDetailModel>();
                SqlDataReader sqlDataReader4 = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spCashierSettlement", sp1, sp2, sp6);
                if (sqlDataReader4.HasRows)
                {
                    while (sqlDataReader4.Read())
                    {
                        CashDetailModel cashDetailModel = new CashDetailModel();

                        cashDetailModel.BOOKING_TRANSACTION_ID = Convert.ToInt64(sqlDataReader4["BOOKING_TRANSACTION_ID"]);
                        cashDetailModel.WAYBILL_NUMBER = sqlDataReader4["WAYBILL_NUMBER"].ToString();
                        cashDetailModel.INVOICE_NUMBER = sqlDataReader4["INVOICE_NUMBER"].ToString();
                        cashDetailModel.TRANSACTION_ID = sqlDataReader4["TRANSACTION_ID"].ToString();
                        cashDetailModel.CASH_AMOUNT = Convert.ToDouble(sqlDataReader4["CASH_AMOUNT"]);
                        LstCashDetailModel.Add(cashDetailModel);

                        cashierSettlementModel.CASH_DETAIL = LstCashDetailModel;
                    }
                }

                #endregion

                #region for Detail Credit
                List<CreditDetailModel> _LstCreditDetailModel = new List<CreditDetailModel>();
                SqlDataReader sqlDataReader5 = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spCashierSettlement", sp1, sp2, sp7);
                if (sqlDataReader5.HasRows)
                {
                    while (sqlDataReader5.Read())
                    {
                        CreditDetailModel creditDetailModel = new CreditDetailModel();

                        creditDetailModel.BOOKING_TRANSACTION_ID = Convert.ToInt64(sqlDataReader5["BOOKING_TRANSACTION_ID"]);
                        creditDetailModel.WAYBILL_NUMBER = sqlDataReader5["WAYBILL_NUMBER"].ToString();
                        creditDetailModel.INVOICE_NUMBER = (sqlDataReader5["INVOICE_NUMBER"]).ToString();
                        creditDetailModel.TRANSACTION_ID = sqlDataReader5["TRANSACTION_ID"].ToString(); ;
                        creditDetailModel.CREDIT_AMOUNT = Convert.ToDouble(sqlDataReader5["CREDIT_AMOUNT"]);
                        _LstCreditDetailModel.Add(creditDetailModel);

                        cashierSettlementModel.CREDIT_DETAIL = _LstCreditDetailModel;
                    }
                }
                #endregion
                LstCashierSettlementModel.Add(cashierSettlementModel);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstCashierSettlementModel;
        }

        public static DataSet LstDaySettlement(SettlementModel settlementModel, out CashierSettlementModel cashierSettlementModel)
        {
            DataSet dataSet = new DataSet();
            cashierSettlementModel = new CashierSettlementModel();
            List<SettlementDetailModel> LstSettlementDetail = new List<SettlementDetailModel>();
            List<DaySummaryReportModel> LstDaySummaryReport = new List<DaySummaryReportModel>();
            List<CashDetailModel> LstCashInfo = new List<CashDetailModel>();
            List<MPesaDetailModel> LstMPesaInfo = new List<MPesaDetailModel>();
            List<CreditDetailModel> LstCreditInfo = new List<CreditDetailModel>();

            SqlParameter sp1 = new SqlParameter("CASHIER_ID", settlementModel.USER_ID);
            SqlParameter sp2 = new SqlParameter("DATE", settlementModel.SEARCH_DATE);
            SqlParameter sp3= new SqlParameter("FLAG","6");
            try
            {
                dataSet = clsDataAccess.ExecuteDataset(CommandType.StoredProcedure, "spCashierSettlement", sp1, sp2, sp3);
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count > 0)
                    {
                        LstSettlementDetail = (from DataRow dataRow in dataSet.Tables[0].Rows
                                               select new SettlementDetailModel()
                                               {
                                                   BOOKING_TRANSACTION_ID = Convert.ToInt64(dataRow["BOOKING_TRANSACTION_ID"].ToString()),
                                                   CASHIER_ID = Convert.ToInt64(dataRow["CASHIER_ID"].ToString()),
                                                   WAYBILL_NUMBER = dataRow["WAYBILL_NUMBER"].ToString(),
                                                   INVOICE_NUMBER = dataRow["INVOICE_NUMBER"].ToString(),
                                                   TRANSACTION_ID = dataRow["TRANSACTION_ID"].ToString(),
                                                   TOTAL_AMOUNT = Convert.ToDouble(dataRow["TOTAL_AMOUNT"].ToString()),                                                 
                                                   PAYMENT_MODE = dataRow["PAYMENT_MODE"].ToString(),
                                                   SAP_NUMBER = dataRow["SAP_NUMBER"].ToString(),
                                                   CASHIER_NAME = dataRow["CASHIER_NAME"].ToString()
                                               }).ToList();

                        LstCashInfo = (from SettlementDetailModel _settlementDetailModel in LstSettlementDetail.Where(x => x.PAYMENT_MODE == "CASH")
                                                                select new CashDetailModel()
                                                                {
                                                                    WAYBILL_NUMBER = _settlementDetailModel.WAYBILL_NUMBER.ToString(),
                                                                    INVOICE_NUMBER = _settlementDetailModel.INVOICE_NUMBER.ToString(),
                                                                    TRANSACTION_ID = _settlementDetailModel.TRANSACTION_ID.ToString(),
                                                                    CASH_AMOUNT = Convert.ToDouble(_settlementDetailModel.TOTAL_AMOUNT),
                                                                    SAP_NUMBER = _settlementDetailModel.SAP_NUMBER.ToString()
                                                                }).ToList();

                       LstMPesaInfo = (from SettlementDetailModel _settlementDetailModel in LstSettlementDetail.Where(x => x.PAYMENT_MODE == "MPESA")
                                                                select new MPesaDetailModel()
                                                                {
                                                                    WAYBILL_NUMBER = _settlementDetailModel.WAYBILL_NUMBER.ToString(),
                                                                    INVOICE_NUMBER = _settlementDetailModel.INVOICE_NUMBER.ToString(),
                                                                    TRANSACTION_ID = _settlementDetailModel.TRANSACTION_ID.ToString(),
                                                                    MPESA_AMOUNT = Convert.ToDouble(_settlementDetailModel.TOTAL_AMOUNT),
                                                                    SAP_NUMBER = _settlementDetailModel.SAP_NUMBER.ToString()
                                                                }).ToList();

                       LstCreditInfo = (from SettlementDetailModel _settlementDetailModel in LstSettlementDetail.Where(x => x.PAYMENT_MODE == "CREDIT")
                                                                   select new CreditDetailModel()
                                                                   {
                                                                       WAYBILL_NUMBER = _settlementDetailModel.WAYBILL_NUMBER.ToString(),
                                                                       INVOICE_NUMBER = _settlementDetailModel.INVOICE_NUMBER.ToString(),
                                                                       TRANSACTION_ID = _settlementDetailModel.TRANSACTION_ID.ToString(),
                                                                       CREDIT_AMOUNT = Convert.ToDouble(_settlementDetailModel.TOTAL_AMOUNT),
                                                                       SAP_NUMBER = _settlementDetailModel.SAP_NUMBER.ToString()
                                                                   }).ToList();
                    }
                    if (dataSet.Tables[1] != null && dataSet.Tables[1].Rows.Count > 0)
                    {
                        LstDaySummaryReport = (from DataRow dataRow in dataSet.Tables[1].Rows
                                     select new DaySummaryReportModel()
                                     {
                                         NO_OF_CASH_TRANSACTION = Convert.ToDouble(dataRow["NO_OF_CASH_TRANSACTION"].ToString()),
                                         TOTAL_CASH_AMOUNT = Convert.ToDouble(dataRow["TOTAL_CASH_AMOUNT"].ToString()), 
                                         NO_OF_MPESA_TRANSACTION = Convert.ToDouble(dataRow["NO_OF_MPESA_TRANSACTION"].ToString()),
                                         TOTAL_MPESA_AMOUNT = Convert.ToDouble(dataRow["TOTAL_MPESA_AMOUNT"].ToString()),
                                         NO_OF_CREDIT_TRANSACTION = Convert.ToDouble(dataRow["NO_OF_CREDIT_TRANSACTION"].ToString()),
                                         TOTAL_CREDIT_AMOUNT = Convert.ToDouble(dataRow["TOTAL_CREDIT_AMOUNT"].ToString()), 
                                     }).ToList();
                    }
                    cashierSettlementModel.SETTLEMENT_DETAIL = LstSettlementDetail;
                    cashierSettlementModel.SUMMARY_REPORT = LstDaySummaryReport;
                    cashierSettlementModel.CASH_DETAIL = LstCashInfo;
                    cashierSettlementModel.MPESA_DETAIL = LstMPesaInfo;
                    cashierSettlementModel.CREDIT_DETAIL = LstCreditInfo;
                    cashierSettlementModel.NO_OF_TRANSACTIONS = LstSettlementDetail.Select(x => x.BOOKING_TRANSACTION_ID).Distinct().Count();
                    cashierSettlementModel.TOTAL_AMOUNT = LstSettlementDetail.Sum(x => x.TOTAL_AMOUNT);
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return dataSet;
        }

        public static List<SettlementDetailModel> LstDETAILREPORT(string DATE, long CASHIER_ID)
        {
            List<SettlementDetailModel> LstCashierSettlement = new List<SettlementDetailModel>();
            //List<CashierSettlementModel> LstCashierSettlementModel = new List<CashierSettlementModel>();
            //CashierSettlementModel cashierSettlementModel = new CashierSettlementModel();
            try
            {
                SqlParameter sp1 = new SqlParameter("@CASHIER_ID", CASHIER_ID);
                SqlParameter sp2 = new SqlParameter("@DATE", DATE);
                SqlParameter sp3 = new SqlParameter("@FLAG", '1');


                #region for SETTLEMENT_DETAIL
                DataTable dt = clsDataAccess.ExecuteDataTable(CommandType.StoredProcedure, "spCashierSettlement", sp1, sp2, sp3);
                SqlDataReader sqlDataReader1 = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spCashierSettlement", sp1, sp2, sp3);
                if (sqlDataReader1.HasRows)
                {
                    while (sqlDataReader1.Read())
                    {
                        SettlementDetailModel settlementDetailModel = new SettlementDetailModel();

                        settlementDetailModel.WAYBILL_NUMBER = sqlDataReader1["WAYBILL_NUMBER"].ToString();
                        settlementDetailModel.INVOICE_NUMBER = (sqlDataReader1["INVOICE_NUMBER"].ToString());
                        settlementDetailModel.TRANSACTION_ID = sqlDataReader1["TRANSACTION_ID"].ToString();
                        settlementDetailModel.TOTAL_AMOUNT = Convert.ToDouble(sqlDataReader1["TOTAL_AMOUNT"]);
                        settlementDetailModel.CASHIER_ID = Convert.ToInt64(sqlDataReader1["CASHIER_ID"].ToString());
                        settlementDetailModel.PAYMENT_MODE = sqlDataReader1["PAYMENT_MODE"].ToString();
                        LstCashierSettlement.Add(settlementDetailModel);

                        //cashierSettlementModel.SETTLEMENT_DETAIL = LstCashierSettlement;
                    }
                }

                #endregion

             
                //LstCashierSettlementModel.Add(cashierSettlementModel);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstCashierSettlement;
        }

        public static List<DaySummaryReportModel> LstDaySummaryReport(string DATE, long CASHIER_ID)
        {
            List<DaySummaryReportModel> LstCashierSettlement = new List<DaySummaryReportModel>();
           
            try
            {
                SqlParameter sp1 = new SqlParameter("@CASHIER_ID", CASHIER_ID);
                SqlParameter sp2 = new SqlParameter("@DATE", DATE);
                SqlParameter sp3 = new SqlParameter("@FLAG", '2');


                #region for Summary report
                
                SqlDataReader sqlDataReader2 = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spCashierSettlement", sp1, sp2, sp3);
                if (sqlDataReader2.HasRows)
                {
                    while (sqlDataReader2.Read())
                    {
                        DaySummaryReportModel daySummaryReportModel = new DaySummaryReportModel();

                        daySummaryReportModel.NO_OF_MPESA_TRANSACTION = Convert.ToInt64(sqlDataReader2["NO_OF_MPESA_TRANSACTION"]);
                        daySummaryReportModel.TOTAL_MPESA_AMOUNT = Convert.ToDouble(sqlDataReader2["TOTAL_MPESA_AMOUNT"]);
                        daySummaryReportModel.NO_OF_CASH_TRANSACTION = Convert.ToInt64(sqlDataReader2["NO_OF_CASH_TRANSACTION"]);
                        daySummaryReportModel.TOTAL_CASH_AMOUNT = Convert.ToDouble(sqlDataReader2["TOTAL_CASH_AMOUNT"]);
                        daySummaryReportModel.NO_OF_CREDIT_TRANSACTION = Convert.ToInt64(sqlDataReader2["NO_OF_CREDIT_TRANSACTION"]);
                        daySummaryReportModel.TOTAL_CREDIT_AMOUNT = Convert.ToDouble(sqlDataReader2["TOTAL_CREDIT_AMOUNT"]);

                        LstCashierSettlement.Add(daySummaryReportModel);
                       
                    }
                }

                #endregion



            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstCashierSettlement;
        }

        public static List<MPesaDetailModel> LstMPesaDetailReport(string DATE, long CASHIER_ID)
        {
            List<MPesaDetailModel> LstCashierSettlement = new List<MPesaDetailModel>();
          
            try
            {
                SqlParameter sp1 = new SqlParameter("@CASHIER_ID", CASHIER_ID);
                SqlParameter sp2 = new SqlParameter("@DATE", DATE);
                SqlParameter sp3 = new SqlParameter("@FLAG", '3');
                #region for Detail MPESA
              
                SqlDataReader SqlDataReader3 = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spCashierSettlement", sp1, sp2, sp3);
                if (SqlDataReader3.HasRows)
                {
                    while (SqlDataReader3.Read())
                    {
                        MPesaDetailModel mPesaDetailModel = new MPesaDetailModel();

                        mPesaDetailModel.BOOKING_TRANSACTION_ID = Convert.ToInt64(SqlDataReader3["BOOKING_TRANSACTION_ID"]);
                        mPesaDetailModel.WAYBILL_NUMBER = SqlDataReader3["WAYBILL_NUMBER"].ToString();
                        mPesaDetailModel.INVOICE_NUMBER = SqlDataReader3["INVOICE_NUMBER"].ToString();
                        mPesaDetailModel.TRANSACTION_ID = SqlDataReader3["TRANSACTION_ID"].ToString();
                        mPesaDetailModel.MPESA_AMOUNT = Convert.ToDouble(SqlDataReader3["MPESA_AMOUNT"]);
                        LstCashierSettlement.Add(mPesaDetailModel);

                        
                    }
                }
                #endregion

            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstCashierSettlement;
        }

        public static List<CashDetailModel> LstCashDetailReport(string DATE, long CASHIER_ID)
        {
            List<CashDetailModel> LstCashierSettlement = new List<CashDetailModel>();

            try
            {
                SqlParameter sp1 = new SqlParameter("@CASHIER_ID", CASHIER_ID);
                SqlParameter sp2 = new SqlParameter("@DATE", DATE);
                SqlParameter sp3 = new SqlParameter("@FLAG", '4');
                #region for DETAILCASH
               
                SqlDataReader sqlDataReader4 = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spCashierSettlement", sp1, sp2, sp3);
                if (sqlDataReader4.HasRows)
                {
                    while (sqlDataReader4.Read())
                    {
                        CashDetailModel cashDetailModel = new CashDetailModel();

                        cashDetailModel.BOOKING_TRANSACTION_ID = Convert.ToInt64(sqlDataReader4["BOOKING_TRANSACTION_ID"]);
                        cashDetailModel.WAYBILL_NUMBER = sqlDataReader4["WAYBILL_NUMBER"].ToString();
                        cashDetailModel.INVOICE_NUMBER = sqlDataReader4["INVOICE_NUMBER"].ToString();
                        cashDetailModel.TRANSACTION_ID = sqlDataReader4["TRANSACTION_ID"].ToString();
                        cashDetailModel.CASH_AMOUNT = Convert.ToDouble(sqlDataReader4["CASH_AMOUNT"]);
                        LstCashierSettlement.Add(cashDetailModel);

                    }
                }

                #endregion

            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstCashierSettlement;
        }

        public static List<CreditDetailModel> LstCreditDetailReport(string DATE, long CASHIER_ID)
        {
            List<CreditDetailModel> LstCashierSettlement = new List<CreditDetailModel>();

            try
            {
                SqlParameter sp1 = new SqlParameter("@CASHIER_ID", CASHIER_ID);
                SqlParameter sp2 = new SqlParameter("@DATE", DATE);
                SqlParameter sp3 = new SqlParameter("@FLAG", '5');
                #region for Detail Credit
               
                SqlDataReader sqlDataReader5 = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spCashierSettlement", sp1, sp2, sp3);
                if (sqlDataReader5.HasRows)
                {
                    while (sqlDataReader5.Read())
                    {
                        CreditDetailModel creditDetailModel = new CreditDetailModel();

                        creditDetailModel.BOOKING_TRANSACTION_ID = Convert.ToInt64(sqlDataReader5["BOOKING_TRANSACTION_ID"]);
                        creditDetailModel.WAYBILL_NUMBER = sqlDataReader5["WAYBILL_NUMBER"].ToString();
                        creditDetailModel.INVOICE_NUMBER = (sqlDataReader5["INVOICE_NUMBER"]).ToString();
                        creditDetailModel.TRANSACTION_ID = sqlDataReader5["TRANSACTION_ID"].ToString(); ;
                        creditDetailModel.CREDIT_AMOUNT = Convert.ToDouble(sqlDataReader5["CREDIT_AMOUNT"]);
                        LstCashierSettlement.Add(creditDetailModel);

                        
                    }
                }
                #endregion

            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstCashierSettlement;
        }
    }
}