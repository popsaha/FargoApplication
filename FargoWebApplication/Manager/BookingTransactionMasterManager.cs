using Fargo_Application.App_Start;
using Fargo_DataAccessLayers;
using Fargo_Models;
using FargoWebApplication.Filter;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Web;
using System.Web.Hosting;

namespace FargoWebApplication.Manager
{
    public class BookingTransactionMasterManager
    {

        public static int SubmitBookingTransaction(BookingTransactionMasterModel bookingTransactionMaster, out string TransactionId, string MerchantRequestID, string CheckoutRequestID)

        public DbFargoApplicationEntities _db = new DbFargoApplicationEntities();
        public static List<BookingTransactionMasterModel> TransactionReport(BOOKING_TRANSACTION_MASTER _BOOKING_TRANSACTION_MASTER)
        {
            List<BookingTransactionMasterModel> LstBookingTransactionMaster = new List<BookingTransactionMasterModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@DATE", _BOOKING_TRANSACTION_MASTER.DATE);
                SqlParameter sp2 = new SqlParameter("@STORE_ID", _BOOKING_TRANSACTION_MASTER.STORE_ID);
                SqlParameter sp3 = new SqlParameter("@FLAG", "1");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spReport", sp1, sp2, sp3);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        BookingTransactionMasterModel bookingTransactionMaster = new BookingTransactionMasterModel();

                        bookingTransactionMaster.IMEI_NUMBER = sqlDataReader["IMEI_NUMBER"].ToString();
                        //bookingTransactionMaster.TRACKING_NUMBER = sqlDataReader["TRACKING_NUMBER"].ToString();
                        bookingTransactionMaster.CUSTOMER_NAME = sqlDataReader["CUSTOMER_NAME"].ToString();
                        bookingTransactionMaster.CUSTOMER_CONTACT = sqlDataReader["CUSTOMER_CONTACT"].ToString();
                        // bookingTransactionMaster.COURIER_ADDRESS = sqlDataReader["COURIER_ADDRESS"].ToString();
                        bookingTransactionMaster.TOTAL_AMOUNT = Convert.ToDouble(sqlDataReader["TOTAL_AMOUNT"].ToString());
                        //bookingTransactionMaster.PAYMENT_MODE = sqlDataReader["PAYMENT_MODE"].ToString();
                        //bookingTransactionMaster.REFERENCE_NUMBER = sqlDataReader["REFERENCE_NUMBER"].ToString();
                        bookingTransactionMaster.DATE = sqlDataReader["DATE"].ToString();
                        bookingTransactionMaster.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();

                        LstBookingTransactionMaster.Add(bookingTransactionMaster);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstBookingTransactionMaster;
        }

        public static int SubmitBookingTransaction(BookingTransactionMasterModel bookingTransactionMaster, string DomainName, out string TransactionId, out bool IS_DUPLICATE_WAYBILL_FOUND, out bool IsValidWayBillNumber, string MerchantRequestID, string CheckoutRequestID)

        {
            string DomainName = ConfigurationManager.AppSettings["DomainName"].ToString();
            string ETR_URL = ConfigurationManager.AppSettings["ETR_URL"].ToString();
            string SAP_URL = ConfigurationManager.AppSettings["SAP_URL"].ToString();
            string SAPAuthorizationHeader = ConfigurationManager.AppSettings["SAPAuthorizationHeader"].ToString();
            string ETRAccessKey = ConfigurationManager.AppSettings["ETRAccessKey"].ToString();
            string ETRClientId = ConfigurationManager.AppSettings["ETRClientId"].ToString();

            int result = 0; TransactionId = string.Empty;
            double? totalCashAmount = 0; double? totalMPesaAmount = 0; double? totalCreditAmount = 0;
            int NoOfCashTransaction = 0; int NoOfMPesaTransaction = 0; int NoOfCreditTransaction = 0;

            IS_DUPLICATE_WAYBILL_FOUND = false;
            IsValidWayBillNumber = true;


            double totalAmount = bookingTransactionMaster.TOTAL_AMOUNT;
            double taxId = bookingTransactionMaster.TAX_ID;
            double taxRate = bookingTransactionMaster.TAX_RATE;
            double invoiceAmount = (totalAmount * 100) / (100 + taxRate);
            double taxAmount = totalAmount - invoiceAmount;

            bookingTransactionMaster.TAX_ID = taxId;
            bookingTransactionMaster.TAX_RATE = taxRate;
            bookingTransactionMaster.INVOICE_AMOUNT = Math.Round(invoiceAmount, 2);
            bookingTransactionMaster.TAX_AMOUNT = Math.Round(taxAmount, 2);

            try
            {
                string NoOfWaybills = string.Empty;
                string WAYBILL = string.Empty;
                int Count = 0;
                if (bookingTransactionMaster.BOOKING_ORDER_DETAILS != null)
                {
                    if (bookingTransactionMaster.BOOKING_ORDER_DETAILS.Count > 0)
                    {
                        foreach (BookingOrderDetailsModel bookingOrderDetailsModel in bookingTransactionMaster.BOOKING_ORDER_DETAILS)
                        {
                            Count = Count + 1;
                            if (!string.IsNullOrEmpty(bookingOrderDetailsModel.TRACKING_NUMBER))
                            {
                                NoOfWaybills += "'" + bookingOrderDetailsModel.TRACKING_NUMBER + "',";
                                WAYBILL += "<tr><td colspan='2'>&nbsp;&nbsp;&nbsp;" + Count + ". Waybill No:&nbsp;&nbsp;&nbsp;" + bookingOrderDetailsModel.TRACKING_NUMBER + "</td></tr>";
                                IsValidWayBillNumber = IsValidWaybillNumber(bookingOrderDetailsModel.TRACKING_NUMBER);
                                if (!IsValidWayBillNumber)
                                { return 0; }
                            }
                            else
                            {
                                IsValidWayBillNumber = IsValidWaybillNumber(bookingOrderDetailsModel.TRACKING_NUMBER);
                                if (!IsValidWayBillNumber)
                                { return 0; }
                            }
                        }
                    }
                    else
                    {
                        IsValidWayBillNumber = IsValidWaybillNumber("");
                        if (!IsValidWayBillNumber)
                        { return 0; }
                    }
                }
                else
                {
                    IsValidWayBillNumber = IsValidWaybillNumber("");
                    if (!IsValidWayBillNumber)
                    { return 0; }
                }
                string WAYBILL_QUERY = string.Empty;
                NoOfWaybills = NoOfWaybills.TrimEnd(',');
                if (string.IsNullOrEmpty(NoOfWaybills))
                {
                    NoOfWaybills = "FCL00000000";
                    WAYBILL_QUERY = "SELECT * FROM BOOKING_ORDER_DETAILS WHERE TRACKING_NUMBER IN ('" + NoOfWaybills + "')";
                }
                else
                {
                    WAYBILL_QUERY = "SELECT * FROM BOOKING_ORDER_DETAILS WHERE TRACKING_NUMBER IN (" + NoOfWaybills + ")";
                }
                SqlParameter sp1 = new SqlParameter("@USER_ID", bookingTransactionMaster.USER_ID);
                SqlParameter sp2 = new SqlParameter("@CASHIER_ID", bookingTransactionMaster.CASHIER_ID);
                SqlParameter sp3 = new SqlParameter("@STORE_ID", bookingTransactionMaster.STORE_ID);
                SqlParameter sp4 = new SqlParameter("@CUSTOMER_ID", bookingTransactionMaster.CUSTOMER_ID);
                SqlParameter sp5 = new SqlParameter("@IMEI_NUMBER", bookingTransactionMaster.IMEI_NUMBER);
                SqlParameter sp6 = new SqlParameter("@CUSTOMER_NAME", bookingTransactionMaster.CUSTOMER_NAME);
                SqlParameter sp7 = new SqlParameter("@CUSTOMER_CONTACT", bookingTransactionMaster.CUSTOMER_CONTACT);
                SqlParameter sp8 = new SqlParameter("@CUSTOMER_PIN", bookingTransactionMaster.CUSTOMER_PIN);
                SqlParameter sp9 = new SqlParameter("@INVOICE_AMOUNT", bookingTransactionMaster.INVOICE_AMOUNT);
                SqlParameter sp10 = new SqlParameter("@TAX_ID", bookingTransactionMaster.TAX_ID);
                SqlParameter sp11 = new SqlParameter("@TAX_RATE", bookingTransactionMaster.TAX_RATE);
                SqlParameter sp12 = new SqlParameter("@TAX_AMOUNT", bookingTransactionMaster.TAX_AMOUNT);
                SqlParameter sp13 = new SqlParameter("@TOTAL_AMOUNT", bookingTransactionMaster.TOTAL_AMOUNT);
                SqlParameter sp14 = new SqlParameter("@MATERIAL_CODE", bookingTransactionMaster.MATERIAL_CODE);
                SqlParameter sp15 = new SqlParameter("@DESCRIPTION", null);
                SqlParameter sp16 = new SqlParameter("@CREATED_BY", bookingTransactionMaster.USER_ID);
                SqlParameter sp17 = new SqlParameter("@CREATED_ON", DateTime.Now);
                SqlParameter sp18 = new SqlParameter("@FLAG", "1");


                string transactionQuery = string.Empty;
                transactionQuery += " DECLARE @NAME VARCHAR(100)='';";
                transactionQuery += " DECLARE @POS_ID VARCHAR(100)='';";
                transactionQuery += " DECLARE @REGISTRATION_ID VARCHAR(100)='';";
                transactionQuery += " DECLARE @STORE_CODE VARCHAR(50)='';";
                transactionQuery += " IF EXISTS(" + WAYBILL_QUERY + ")";
                transactionQuery += " BEGIN";
                transactionQuery += " SELECT '0' AS 'BOOKING_TRANSACTION_ID', null AS 'NAME', null AS 'TRANSACTION_DATE', DATEADD(MINUTE, 330,GETUTCDATE()) AS 'BOOKING_DATETIME', '' AS 'POS_ID', '' AS 'REGISTRATION_ID', '' AS 'STORE_CODE', '1' AS IS_DUPLICATE_WAYBILL_FOUND;";
                transactionQuery += " END";
                transactionQuery += " ELSE";
                transactionQuery += " BEGIN";
                transactionQuery += " SET @NAME=(SELECT FIRST_NAME+' '+LAST_NAME FROM USER_MASTER WHERE USER_ID=@USER_ID);";
                transactionQuery += " SELECT @POS_ID=POS_ID, @REGISTRATION_ID=REFERENCE_ID, @STORE_CODE= STORE_CODE FROM STORE_MASTER WHERE STORE_ID=@STORE_ID;";
                transactionQuery += " INSERT INTO BOOKING_TRANSACTION_MASTER(USER_ID, CASHIER_ID, STORE_ID, IMEI_NUMBER, CUSTOMER_ID, CUSTOMER_NAME, CUSTOMER_CONTACT, CUSTOMER_PIN, INVOICE_AMOUNT, TAX_ID, TAX_RATE, TAX_AMOUNT, TOTAL_AMOUNT, MATERIAL_CODE, DESCRIPTION, DATA_SOURCE, IS_ACTIVE, CREATED_BY, CREATED_ON)";
                transactionQuery += " VALUES(@USER_ID, @CASHIER_ID, @STORE_ID, @IMEI_NUMBER, @CUSTOMER_ID, @CUSTOMER_NAME, @CUSTOMER_CONTACT, @CUSTOMER_PIN, ROUND(@INVOICE_AMOUNT, 2), @TAX_ID, @TAX_RATE, ROUND(@TAX_AMOUNT, 2), @TOTAL_AMOUNT,  @MATERIAL_CODE, @DESCRIPTION, 'M', '1', @CREATED_BY, DATEADD(MINUTE, 330,GETUTCDATE()));";
                transactionQuery += " SELECT @@IDENTITY AS 'BOOKING_TRANSACTION_ID', @NAME AS 'NAME', CONVERT(VARCHAR,DATEADD(MINUTE, 330,GETUTCDATE()),107) AS 'TRANSACTION_DATE', DATEADD(MINUTE, 330,GETUTCDATE()) AS 'BOOKING_DATETIME', @POS_ID AS 'POS_ID', @REGISTRATION_ID AS 'REGISTRATION_ID', '0' AS IS_DUPLICATE_WAYBILL_FOUND, @STORE_CODE AS 'STORE_CODE';";
                transactionQuery += " END";

                DataTable dataTable = clsDataAccess.ExecuteDataTable(CommandType.Text, transactionQuery, sp1, sp2, sp3, sp4, sp5, sp6, sp7, sp8, sp9, sp10, sp11, sp12, sp13, sp14, sp15, sp16, sp17, sp18);
                if (dataTable != null)
                {
                    IS_DUPLICATE_WAYBILL_FOUND = dataTable.Rows[0]["IS_DUPLICATE_WAYBILL_FOUND"].ToString() == "1" ? true : false;
                    if (!IS_DUPLICATE_WAYBILL_FOUND)
                    {
                        string BOOKING_TRANSACTION_ID = dataTable.Rows[0]["BOOKING_TRANSACTION_ID"].ToString();
                        string NAME = dataTable.Rows[0]["NAME"].ToString();
                        string TRANSACTION_DATE = dataTable.Rows[0]["TRANSACTION_DATE"].ToString();
                        string POS_ID = dataTable.Rows[0]["POS_ID"].ToString();
                        string REGISTRATION_ID = dataTable.Rows[0]["REGISTRATION_ID"].ToString();
                        string STORE_CODE = dataTable.Rows[0]["STORE_CODE"].ToString();
                        DateTime BOOKING_DATETIME = Convert.ToDateTime(dataTable.Rows[0]["BOOKING_DATETIME"].ToString());

                        DataTable _DTPayment = DTPayment(bookingTransactionMaster.BOOKING_PAYMENT_DETAILS, BOOKING_TRANSACTION_ID, bookingTransactionMaster.USER_ID.ToString(), bookingTransactionMaster.CUSTOMER_ID, bookingTransactionMaster.TOTAL_AMOUNT, out totalCashAmount, out totalMPesaAmount, out totalCreditAmount, out NoOfCashTransaction, out NoOfMPesaTransaction, out NoOfCreditTransaction, MerchantRequestID, CheckoutRequestID);
                        DataTable _DTOrder = DTOrder(bookingTransactionMaster.BOOKING_ORDER_DETAILS, BOOKING_TRANSACTION_ID, bookingTransactionMaster.USER_ID.ToString());

                        SqlParameter _sp1 = new SqlParameter("@BOOKING_TRANSACTION_ID", BOOKING_TRANSACTION_ID);
                        SqlParameter _sp2 = new SqlParameter("@tblPayment", _DTPayment);
                        SqlParameter _sp3 = new SqlParameter("@tblOrder", _DTOrder);
                        SqlParameter _sp4 = new SqlParameter("@TOTAL_AMOUNT", bookingTransactionMaster.TOTAL_AMOUNT);
                        SqlParameter _sp5 = new SqlParameter("@CUSTOMER_ID", bookingTransactionMaster.CUSTOMER_ID);
                        SqlParameter _sp6 = new SqlParameter("@TOTAL_CASH_AMOUNT", totalCashAmount);
                        SqlParameter _sp7 = new SqlParameter("@TOTAL_MPESA_AMOUNT", totalMPesaAmount);
                        SqlParameter _sp8 = new SqlParameter("@TOTAL_CREDIT_AMOUNT", totalCreditAmount);
                        SqlParameter _sp9 = new SqlParameter("@NO_OF_CASH_TRANSACTION", NoOfCashTransaction);
                        SqlParameter _sp10 = new SqlParameter("@NO_OF_MPESA_TRANSACTION", NoOfMPesaTransaction);
                        SqlParameter _sp11 = new SqlParameter("@NO_OF_CREDIT_TRANSACTION", NoOfCreditTransaction);
                        SqlParameter _sp12 = new SqlParameter("@CASHIER_ID", bookingTransactionMaster.CASHIER_ID);
                        SqlParameter _sp13 = new SqlParameter("@STORE_ID", bookingTransactionMaster.STORE_ID);
                        SqlParameter _sp14 = new SqlParameter("@MERCHANT_REQUEST_ID", MerchantRequestID);
                        SqlParameter _sp15 = new SqlParameter("@CHECKOUT_REQUEST_ID", CheckoutRequestID);
                        SqlParameter _sp16 = new SqlParameter("@FLAG", "2");

                        //it_output-pos_id = 'F0001'.                         "'pos01'."for testing in dev/QA
                        //it_output-registration_id = '84KRA0510500001'.      "'91KRA0030010073'.  "for testing in dev/QA
                        result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spBookingTransaction", _sp1, _sp2, _sp3, _sp4, _sp5, _sp6, _sp7, _sp8, _sp9, _sp10, _sp11, _sp12, _sp13, _sp14, _sp15, _sp16);
                        if (result > 0)
                        {
                            string _transactionId = "";
                            Int32 _initialTransactionId = 100000000;
                            _transactionId = (_initialTransactionId + Convert.ToInt32(BOOKING_TRANSACTION_ID)).ToString();
                            TransactionId = _transactionId;

                            ETRTransactionBuyerModel _ETRTransactionBuyerModel = new ETRTransactionBuyerModel()
                            {
                                registrationName = "Domestic Customer",
                                taxIdentificationNumber = "PSK12121989"
                            };
                            List<ETRTransactionItemModel> _ETRTransactionItemModel = new List<ETRTransactionItemModel>()
                        {
                            new  ETRTransactionItemModel
                            {
                                code = "10000262",
                                description = "Air freight",
                                discount = 500.00,
                                hs="",
                                invoicedQuantity=1,
                                price = 5220.46,
                                taxCode = "A",
                                total = 4720.46
                            }
                        };
                            ETRTransactionTaxModel _ETRTransactionTaxModel = new ETRTransactionTaxModel()
                            {
                                vatNetAmount = 4069.36,
                                vatTaxAmount = 651.10
                            };
                            ETRTransactionModel _ETRTransactionModel = new ETRTransactionModel()
                            {
                                buyer = _ETRTransactionBuyerModel,
                                cashier1 = "",
                                currencyCode = "KSH",
                                discountAmount = 0,
                                invoiceDocumentReference = "",
                                issueDate = "2022-07-15",
                                issueTime = "11:08:13",
                                items = _ETRTransactionItemModel,
                                posID = POS_ID,
                                registrationID = REGISTRATION_ID,
                                tax = _ETRTransactionTaxModel,
                                taxExclusiveAmount = 4069.36,
                                taxInclusiveAmount = 4720.46,
                                transactionID = _transactionId,
                                transactionTypeCode = 1
                            };

                            string JSONResponse = "{";
                            string JSONString = JsonConvert.SerializeObject(_ETRTransactionModel);

                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            ServicePointManager.ServerCertificateValidationCallback = new
                                                                                    RemoteCertificateValidationCallback
                                                                                    (
                                                                                       delegate { return true; }
                                                                                    );

                            string URL = String.Format("https://52.168.16.149:8010/api/v2.0/transaction/new");
                            WebRequest webRequest = WebRequest.Create(URL);
                            webRequest.Method = "POST";
                            webRequest.Headers["clientid"] = "OiZqm01q9S51y5J";
                            webRequest.Headers["accessKey"] = "3ZixXmuHFk7qyXO+2sfxPxFmEROn4m13mir+gRjFFfk=";
                            webRequest.ContentType = "application/json";

                            using (var stramWriter = new StreamWriter(webRequest.GetRequestStream()))
                            {
                                stramWriter.Write(JSONString);
                                stramWriter.Flush();
                                stramWriter.Close();
                                HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

                                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                                string responseString = streamReader.ReadLine();
                                JSONResponse = responseString;
                            }
                            string Base64EndcodedJSONResponse = EncryptDecryptString.EncodeBase64(JSONResponse);
                            ETRTransactionResponseModel _ETRTransactionResponseModel = JsonConvert.DeserializeObject<ETRTransactionResponseModel>(JSONResponse);

                            string FileLocation = DomainName + "Invoices/" + "INVOICE_" + _ETRTransactionResponseModel.transactionID + ".pdf";
                            SqlParameter sp1_ = new SqlParameter("@BOOKING_TRANSACTION_ID", BOOKING_TRANSACTION_ID);
                            SqlParameter sp2_ = new SqlParameter("@TRANSACTION_ID", _ETRTransactionResponseModel.transactionID);
                            SqlParameter sp3_ = new SqlParameter("@CU_NUMBER", _ETRTransactionResponseModel.signature == null ? null : _ETRTransactionResponseModel.signature.cuNumber);
                            SqlParameter sp4_ = new SqlParameter("@TIMESTAMP", _ETRTransactionResponseModel.signature == null ? null : _ETRTransactionResponseModel.signature.timestamp);
                            SqlParameter sp5_ = new SqlParameter("@FISCAL_TRANSACTION_NUMBER", _ETRTransactionResponseModel.signature == null ? null : _ETRTransactionResponseModel.signature.fiscalTransactionNumber);
                            SqlParameter sp6_ = new SqlParameter("@QR", _ETRTransactionResponseModel.qr);
                            SqlParameter sp7_ = new SqlParameter("@IS_DUPLICATE", _ETRTransactionResponseModel.isDuplicate == null ? false : _ETRTransactionResponseModel.isDuplicate);
                            SqlParameter sp8_ = new SqlParameter("@SUCCESS", _ETRTransactionResponseModel.success == null ? "" : _ETRTransactionResponseModel.success);
                            SqlParameter sp9_ = new SqlParameter("@ERROR_CODE", _ETRTransactionResponseModel.errorCode == null ? "" : _ETRTransactionResponseModel.errorCode);
                            SqlParameter sp10_ = new SqlParameter("@ERROR_MESSAGE", _ETRTransactionResponseModel.errorMessage == null ? "" : _ETRTransactionResponseModel.errorMessage);
                            SqlParameter sp11_ = new SqlParameter("@FILE_LOCATION", FileLocation);
                            SqlParameter sp12_ = new SqlParameter("@CREATED_BY", bookingTransactionMaster.USER_ID);
                            SqlParameter sp13_ = new SqlParameter("@FLAG", "1");
                            int ETRResult = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spETRTransaction", sp1_, sp2_, sp3_, sp4_, sp5_, sp6_, sp7_, sp8_, sp9_, sp10_, sp11_, sp12_, sp13_);
                            if (ETRResult > 0)
                            {
                                string TRANSACTION_ID = _ETRTransactionResponseModel.transactionID;
                                TRANSACTION_DATE = TRANSACTION_DATE.ToString();
                                string TRANSACTION_BY = NAME;
                                string CUSTOMER_PIN = bookingTransactionMaster.CUSTOMER_PIN;
                                string CUSTOMER_MOBILE = bookingTransactionMaster.CUSTOMER_CONTACT;
                                string PAYMENT_BY = string.Empty;
                                if (bookingTransactionMaster.CUSTOMER_ID > 0)
                                {
                                    PAYMENT_BY = "CREDIT";
                                }
                                else
                                {
                                    PAYMENT_BY = string.Join("/", bookingTransactionMaster.BOOKING_PAYMENT_DETAILS.Select(x => x.PAYMENT_MODE).ToArray());
                                }

                                double INVOICE_AMOUNT = bookingTransactionMaster.INVOICE_AMOUNT;
                                double TAX_RATE = bookingTransactionMaster.TAX_RATE;
                                double TAX_AMOUNT = bookingTransactionMaster.TAX_AMOUNT;
                                double AMOUNT_DUE = 0;
                                double TOTAL_AMOUNT = bookingTransactionMaster.TOTAL_AMOUNT;
                                string CU_NUMBER = _ETRTransactionResponseModel.signature == null ? "" : _ETRTransactionResponseModel.signature.cuNumber;
                                string FISCAL_TRANSACTION_NUMBER = _ETRTransactionResponseModel.signature == null ? "" : _ETRTransactionResponseModel.signature.fiscalTransactionNumber;
                                string QR = _ETRTransactionResponseModel.qr;


                                DynamicExportToPDF(TRANSACTION_ID, TRANSACTION_DATE, TRANSACTION_BY, CUSTOMER_PIN, CUSTOMER_MOBILE, PAYMENT_BY, WAYBILL, INVOICE_AMOUNT, TAX_RATE, TAX_AMOUNT, AMOUNT_DUE, TOTAL_AMOUNT, CU_NUMBER, FISCAL_TRANSACTION_NUMBER, QR, DomainName);
                                int _result = 0;


                            #region FOR SAP INTEGRATION REGARDING BOOKING TRANSACTION.
                            try
                            {
                                _result = SAPBookingTransactionIntegration(bookingTransactionMaster.USER_ID, BOOKING_TRANSACTION_ID, TRANSACTION_ID, "KES", DateTime.Now.ToString("MMddyyyy"), bookingTransactionMaster.BOOKING_ORDER_DETAILS.Count(), bookingTransactionMaster.TOTAL_AMOUNT, bookingTransactionMaster.MATERIAL_CODE, STORE_CODE, _ETRTransactionResponseModel.signature.cuNumber, _ETRTransactionResponseModel.signature.fiscalTransactionNumber, bookingTransactionMaster.BOOKING_ORDER_DETAILS.Count().ToString(), SAP_URL, SAPAuthorizationHeader);
                            }
                            catch (Exception exception)
                            {

                                #region FOR SAP INTEGRATION REGARDING BOOKING TRANSACTION.
                                try
                                {
                                    _result = SAPBookingTransactionIntegration(bookingTransactionMaster.USER_ID, BOOKING_TRANSACTION_ID, TRANSACTION_ID, "USD", DateTime.Now.ToString("MMddyyyy"), bookingTransactionMaster.BOOKING_ORDER_DETAILS.Count(), bookingTransactionMaster.TOTAL_AMOUNT, bookingTransactionMaster.MATERIAL_CODE, STORE_CODE, _ETRTransactionResponseModel.signature.cuNumber, _ETRTransactionResponseModel.signature.fiscalTransactionNumber, NoOfWaybills);
                                }
                                catch (Exception exception)
                                {


                                }
                                #endregion
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }


        public static int SAPBookingTransactionIntegration(long USER_ID, string BOOKING_TRANSACTION_ID, string TRANSACTION_ID, string CURRENCY, string DATE, double QUANTITY, double PRICE, string MATERIAL_CODE, string STORE_CODE, string CU_NUMBER, string FISCAL_TRANSACTION_NUMBER, string MATERIAL_TEXT, string SAP_URL, string SAPAuthorizationHeader)
        {
            int result = 0;
            try
            {
                SAPFreightwareHeaderModel _SAPFreightwareHeaderModel = new SAPFreightwareHeaderModel()
                {
                    SoldtoParty = STORE_CODE,
                    DocumentType = "ZRC", //FIXED
                    CUNumber = CU_NUMBER,
                    CUInvoiceNumber = FISCAL_TRANSACTION_NUMBER,
                    MpesaReferenceNumber = "",
                    AppNumber = TRANSACTION_ID,
                    Plant = "2100", //FIXED
                    Date = DATE,
                    Currency = CURRENCY

                };
                List<SAPFreightwareItemModel> _SAPFreightwareItemModel = new List<SAPFreightwareItemModel>()
                {
                    new SAPFreightwareItemModel()
                    {
                        ItemPosition ="10",
                        Material = MATERIAL_CODE,
                        MaterialText = MATERIAL_TEXT,
                        Price = PRICE,
                        Quantity = QUANTITY,
                        UOM = "EA"
                    }
                };

                SalesOrder_Request salesOrder_Request = new SalesOrder_Request()
                {
                    Header = _SAPFreightwareHeaderModel,
                    Item = _SAPFreightwareItemModel
                };

                string JSONResponse = string.Empty;
                string JSONString = JsonConvert.SerializeObject(salesOrder_Request);
                ServicePointManager.Expect100Continue = true;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = new
                                                                        RemoteCertificateValidationCallback
                                                                        (
                                                                           delegate { return true; }
                                                                        );

                string URL = String.Format("http://fc-podev.fargocourier.co.ke:50000/RESTAdapter/webserver/mobileapp/salesorder");
                WebRequest webRequest = WebRequest.Create(URL);
                webRequest.Method = "POST";
                webRequest.Headers["Authorization"] = "Basic ZnJlaWdoX3VzZXI6RmFyZ29AMjAyMg==";
                webRequest.ContentType = "application/json";

                using (var stramWriter = new StreamWriter(webRequest.GetRequestStream()))
                {
                    stramWriter.Write(JSONString);
                    stramWriter.Flush();
                    stramWriter.Close();
                    HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

                    StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                    string responseString = streamReader.ReadLine();
                    JSONResponse = responseString;
                }
                string Base64EndcodedJSONResponse = EncryptDecryptString.EncodeBase64(JSONResponse);
                string JSONRequest = JsonConvert.SerializeObject(salesOrder_Request);
                string Base64EndcodedJSONRequest = EncryptDecryptString.EncodeBase64(JSONRequest);

                SqlParameter sp1 = new SqlParameter("@CASHIER_ID", USER_ID);
                SqlParameter sp2 = new SqlParameter("@BOOKING_TRANSACTION_ID", BOOKING_TRANSACTION_ID);
                SqlParameter sp3 = new SqlParameter("@TRANSACTION_ID", TRANSACTION_ID);
                SqlParameter sp4 = new SqlParameter("@SOLD_TO_PARTY", salesOrder_Request.Header.SoldtoParty);
                SqlParameter sp5 = new SqlParameter("@DOCUMENT_TYPE", salesOrder_Request.Header.DocumentType);
                SqlParameter sp6 = new SqlParameter("@CU_NUMBER", salesOrder_Request.Header.CUNumber);
                SqlParameter sp7 = new SqlParameter("@CU_INVOICE_NUMBER", salesOrder_Request.Header.CUInvoiceNumber);
                SqlParameter sp8 = new SqlParameter("@MPESA_REFERENCE_NUMBER", salesOrder_Request.Header.MpesaReferenceNumber);
                SqlParameter sp9 = new SqlParameter("@APP_NUMBER", salesOrder_Request.Header.AppNumber);
                SqlParameter sp10 = new SqlParameter("@PLANT", salesOrder_Request.Header.Plant);
                SqlParameter sp11 = new SqlParameter("@DATE", salesOrder_Request.Header.Date);
                SqlParameter sp12 = new SqlParameter("@CURRENCY", salesOrder_Request.Header.Currency);
                SqlParameter sp13 = new SqlParameter("@ITEM_POSITION", salesOrder_Request.Item[0].ItemPosition);
                SqlParameter sp14 = new SqlParameter("@MATERIAL", salesOrder_Request.Item[0].Material);
                SqlParameter sp15 = new SqlParameter("@MATERIAL_TEXT", salesOrder_Request.Item[0].MaterialText);
                SqlParameter sp16 = new SqlParameter("@PRICE", salesOrder_Request.Item[0].Price);
                SqlParameter sp17 = new SqlParameter("@QUANTITY", salesOrder_Request.Item[0].Quantity);
                SqlParameter sp18 = new SqlParameter("@UOM", salesOrder_Request.Item[0].UOM);
                SqlParameter sp19 = new SqlParameter("@BASE64_REQUEST", Base64EndcodedJSONRequest);
                SqlParameter sp20 = new SqlParameter("@BASE64_RESPONSE", Base64EndcodedJSONResponse);
                SqlParameter sp21 = new SqlParameter("@FLAG", "1");

                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spSAPBookingTransaction", sp1, sp2, sp3, sp4, sp5, sp6, sp7, sp8, sp9, sp10, sp11, sp12, sp13, sp14, sp15, sp16, sp17, sp18, sp19, sp20, sp21);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }




        private static DataTable DTPayment(List<BookingPaymentDetailsModel> _BOOKING_PAYMENT_DETAILS, string BOOKING_TRANSACTION_ID, string USER_ID, long CUSTOMER_ID, double? TOTAL_AMOUNT, out double? totalCashAmount, out double? totalMPesaAmount, out double? totalCreditAmount, out int NoOfCashTransaction, out int NoOfMPesaTransaction, out int NoOfCreditTransaction, string MERCHANT_REQUEST_ID, string CHECKOUT_REQUEST_ID)
        {
            totalCashAmount = 0; totalMPesaAmount = 0; totalCreditAmount = 0;
            NoOfCashTransaction = 0; NoOfMPesaTransaction = 0; NoOfCreditTransaction = 0;

            DataTable _DTPayment = new DataTable();
            _DTPayment.Columns.Add("BOOKING_TRANSACTION_ID");
            _DTPayment.Columns.Add("USER_ID");
            _DTPayment.Columns.Add("CASHIER_ID");
            _DTPayment.Columns.Add("PAYMENT_MODE");
            _DTPayment.Columns.Add("AMOUNT");
            _DTPayment.Columns.Add("MERCHANT_REQUEST_ID");
            _DTPayment.Columns.Add("CHECKOUT_REQUEST_ID");
            _DTPayment.Columns.Add("DESCRIPTION");
            _DTPayment.Columns.Add("DATA_SOURCE");
            _DTPayment.Columns.Add("IS_ACTIVE");
            _DTPayment.Columns.Add("CREATED_BY");
            _DTPayment.Columns.Add("CREATED_ON");
            _DTPayment.AcceptChanges();
            try
            {
                if (CUSTOMER_ID > 0)
                {
                    totalCreditAmount = TOTAL_AMOUNT;
                    NoOfCreditTransaction = 1;

                    _DTPayment.Rows.Add(BOOKING_TRANSACTION_ID, USER_ID, USER_ID, "CREDIT", TOTAL_AMOUNT, null, null, "Full payment made via Credit.", "M", "1", USER_ID, DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss"));
                    _DTPayment.AcceptChanges();
                }
                else
                {
                    foreach (BookingPaymentDetailsModel BookingPaymentDetails in _BOOKING_PAYMENT_DETAILS)
                    {
                        if (BookingPaymentDetails.AMOUNT > 0)
                        {
                            string PaymentMode = string.Empty;
                            if (BookingPaymentDetails.PAYMENT_MODE.ToUpper() == "CASH")
                            {
                                totalCashAmount += BookingPaymentDetails.AMOUNT;
                                NoOfCashTransaction = NoOfCashTransaction + 1;
                                PaymentMode = "Cash";
                            }
                            if (BookingPaymentDetails.PAYMENT_MODE.ToUpper() == "MPESA")
                            {
                                totalMPesaAmount += BookingPaymentDetails.AMOUNT;
                                NoOfMPesaTransaction = NoOfMPesaTransaction + 1;
                                PaymentMode = "MPesa";
                            }
                            _DTPayment.Rows.Add(BOOKING_TRANSACTION_ID, USER_ID, USER_ID, BookingPaymentDetails.PAYMENT_MODE, BookingPaymentDetails.AMOUNT, MERCHANT_REQUEST_ID, CHECKOUT_REQUEST_ID, "Partial payment made via " + PaymentMode + ".", "M", "1", USER_ID, DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss"));
                            _DTPayment.AcceptChanges();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return _DTPayment;
        }

        private static DataTable DTOrder(List<BookingOrderDetailsModel> _BOOKING_ORDER_DETAILS, string BOOKING_TRANSACTION_ID, string USER_ID)
        {
            DataTable _DTOrder = new DataTable();
            _DTOrder.Columns.Add("BOOKING_TRANSACTION_ID");
            _DTOrder.Columns.Add("USER_ID");
            _DTOrder.Columns.Add("CASHIER_ID");
            _DTOrder.Columns.Add("TRACKING_NUMBER");
            _DTOrder.Columns.Add("IS_CANCELLED");
            _DTOrder.Columns.Add("CANCELLED_BY");
            _DTOrder.Columns.Add("CANCELLED_ON");
            _DTOrder.Columns.Add("CANCELLATION_ID");
            _DTOrder.Columns.Add("DESCRIPTION");
            _DTOrder.Columns.Add("DATA_SOURCE");
            _DTOrder.Columns.Add("IS_ACTIVE");
            _DTOrder.Columns.Add("CREATED_BY");
            _DTOrder.Columns.Add("CREATED_ON");
            _DTOrder.AcceptChanges();
            try
            {
                foreach (BookingOrderDetailsModel BookingOrderDetails in _BOOKING_ORDER_DETAILS)
                {
                    _DTOrder.Rows.Add(BOOKING_TRANSACTION_ID, USER_ID, USER_ID, BookingOrderDetails.TRACKING_NUMBER, "0", null, null, null, null, "M", "1", USER_ID, DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss"));
                    _DTOrder.AcceptChanges();
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return _DTOrder;
        }

        public static List<StoreModel> LstStores()
        {
            List<StoreModel> LstStores = new List<StoreModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@FLAG", "1");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spReportBookingTransaction", sp1);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        StoreModel storeModel = new StoreModel();
                        storeModel.STORE_ID = Convert.ToInt64(sqlDataReader["STORE_ID"].ToString());
                        storeModel.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();
                        LstStores.Add(storeModel);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstStores;
        }

        public static List<BookingTransactionMasterModel> BookingTransactionReport(BookingTransactionMasterModel bookingTransactionModel)
        {
            List<BookingTransactionMasterModel> LstBookingTransactionReport = new List<BookingTransactionMasterModel>();
            try
            {
                string fromDate = "01-01-2022"; string toDate = DateTime.Now.AddDays(1).ToString("MM-dd-yyyy");
                SqlParameter sp1 = null; SqlParameter sp2 = null; SqlParameter sp3 = null; SqlParameter sp4 = null;

                if (string.IsNullOrEmpty(bookingTransactionModel.FROM_DATE) && string.IsNullOrEmpty(bookingTransactionModel.TO_DATE))
                {
                    sp1 = new SqlParameter("@STORE_ID", bookingTransactionModel.STORE_ID);
                    sp2 = new SqlParameter("@FROM_DATE", fromDate);
                    sp3 = new SqlParameter("@TO_DATE", toDate);
                    sp4 = new SqlParameter("@FLAG", bookingTransactionModel.STORE_ID == null ? "4" : "3");
                }
                else
                {
                    sp1 = new SqlParameter("@STORE_ID", bookingTransactionModel.STORE_ID);
                    sp2 = new SqlParameter("@FROM_DATE", ConvertDateFormat.ConvertMMDDYYYY(bookingTransactionModel.FROM_DATE));
                    sp3 = new SqlParameter("@TO_DATE", string.IsNullOrEmpty(bookingTransactionModel.TO_DATE) ? ConvertDateFormat.ConvertMMDDYYYY(bookingTransactionModel.FROM_DATE) : ConvertDateFormat.ConvertMMDDYYYY(bookingTransactionModel.TO_DATE));
                    sp4 = new SqlParameter("@FLAG", bookingTransactionModel.STORE_ID == null ? "4" : "3");
                }
                DataSet dataSet = clsDataAccess.ExecuteDataset(CommandType.StoredProcedure, "spReportBookingTransaction", sp1, sp2, sp3, sp4);
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count > 0)
                    {

                        BookingTransactionMasterModel bookingTransactionMaster = new BookingTransactionMasterModel();

                        LstBookingTransactionReport = (from DataRow dataRow in dataSet.Tables[0].Rows
                                                       select new BookingTransactionMasterModel()
                                                       {

                                                           BOOKING_TRANSACTION_ID = Convert.ToInt64(dataRow["BOOKING_TRANSACTION_ID"].ToString()),
                                                           CASHIER_NAME = dataRow["FIRST_NAME"].ToString() + " " + dataRow["LAST_NAME"].ToString(),
                                                           IMEI_NUMBER = dataRow["IMEI_NUMBER"].ToString(),
                                                           CUSTOMER_NAME = dataRow["CUSTOMER_NAME"].ToString(),
                                                           CUSTOMER_CONTACT = dataRow["CUSTOMER_CONTACT"].ToString(),
                                                           TOTAL_AMOUNT = Convert.ToDouble(dataRow["TOTAL_AMOUNT"].ToString()),
                                                           STORE_NAME = dataRow["STORE_NAME"].ToString(),
                                                           DATE = dataRow["ENTRY_DATE"].ToString(),
                                                           TRANSACTION_ID = dataRow["TRANSACTION_ID"].ToString(),
                                                           ETR_STATUS = dataRow["ETR_STATUS"].ToString(),
                                                           BOOKING_PAYMENT_RESPONSE = (from DataRow paymentDataRow in dataSet.Tables[1].Rows
                                                                                       where Convert.ToInt64(paymentDataRow["BOOKING_TRANSACTION_ID"].ToString()) == Convert.ToInt64(dataRow["BOOKING_TRANSACTION_ID"].ToString())
                                                                                       select new BookingPaymentResponseModel()
                                                                                       {
                                                                                           BOOKING_PAYMENT_DETAILS_ID = Convert.ToInt64(paymentDataRow["BOOKING_PAYMENT_DETAILS_ID"].ToString()),
                                                                                           BOOKING_TRANSACTION_ID = Convert.ToInt64(paymentDataRow["BOOKING_TRANSACTION_ID"].ToString()),
                                                                                           AMOUNT = Convert.ToDouble(paymentDataRow["AMOUNT"].ToString()),
                                                                                           PAYMENT_MODE = paymentDataRow["PAYMENT_MODE"].ToString(),
                                                                                       }).ToList(),
                                                           BOOKING_ORDER_RESPONSE = (from DataRow orderDataRow in dataSet.Tables[2].Rows
                                                                                     where Convert.ToInt64(orderDataRow["BOOKING_TRANSACTION_ID"].ToString()) == Convert.ToInt64(dataRow["BOOKING_TRANSACTION_ID"].ToString())
                                                                                     select new BookingOrderResponseModel()
                                                                                     {
                                                                                         BOOKING_ORDER_DETAILS_ID = Convert.ToInt64(orderDataRow["BOOKING_ORDER_DETAILS_ID"].ToString()),
                                                                                         BOOKING_TRANSACTION_ID = Convert.ToInt64(orderDataRow["BOOKING_TRANSACTION_ID"].ToString()),
                                                                                         TRACKING_NUMBER = orderDataRow["TRACKING_NUMBER"].ToString()
                                                                                     }).ToList(),
                                                           CANCEL_TRANSACTION_MODEL = (from DataRow cancelDataRow in dataSet.Tables[3].Rows
                                                                                       where Convert.ToInt64(cancelDataRow["BOOKING_TRANSACTION_ID"].ToString()) == Convert.ToInt64(dataRow["BOOKING_TRANSACTION_ID"].ToString())
                                                                                       select new CancelTransactionModel()
                                                                                       {
                                                                                           BOOKING_TRANSACTION_ID = Convert.ToInt64(cancelDataRow["BOOKING_TRANSACTION_ID"].ToString()),
                                                                                           CANCEL_AMOUNT = Convert.ToDouble(cancelDataRow["CANCEL_AMOUNT"].ToString()),
                                                                                       }).ToList()
                                                       }).ToList();

                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstBookingTransactionReport;
        }

        public static List<BookingTransactionMasterModel> ReportBookingTransaction(BookingTransactionMasterModel bookingTransactionModel)
        {
            List<BookingTransactionMasterModel> LstBookingTransactionReport = new List<BookingTransactionMasterModel>();
            string LstBookingTransactionId = string.Empty;
            try
            {

                SqlParameter sp1 = new SqlParameter("@CASHIER_ID", bookingTransactionModel.USER_ID);
                SqlParameter sp2 = new SqlParameter("@FROM_DATE", ConvertDateFormat.ConvertMMDDYYYY(bookingTransactionModel.FROM_DATE));
                SqlParameter sp3 = new SqlParameter("@TO_DATE", ConvertDateFormat.ConvertMMDDYYYY(bookingTransactionModel.TO_DATE));
                SqlParameter sp4 = new SqlParameter("@PAGE_NUMBER", ConvertDateFormat.ConvertMMDDYYYY(bookingTransactionModel.PAGE_NUMBER));
                SqlParameter sp5 = new SqlParameter("@FLAG", "1");

                DataSet dataSet = clsDataAccess.ExecuteDataset(CommandType.StoredProcedure, "spBookingMobileReport", sp1, sp2, sp3, sp4, sp5);
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count > 0)
                    {
                        LstBookingTransactionReport = (from DataRow dataRow in dataSet.Tables[0].Rows
                                                       select new BookingTransactionMasterModel()
                                                       {
                                                           BOOKING_TRANSACTION_ID = Convert.ToInt64(dataRow["BOOKING_TRANSACTION_ID"].ToString()),
                                                           CASHIER_NAME = dataRow["FIRST_NAME"].ToString() + " " + dataRow["LAST_NAME"].ToString(),
                                                           IMEI_NUMBER = dataRow["IMEI_NUMBER"].ToString(),
                                                           CUSTOMER_NAME = dataRow["CUSTOMER_NAME"].ToString(),
                                                           CUSTOMER_CONTACT = dataRow["CUSTOMER_CONTACT"].ToString(),
                                                           TOTAL_AMOUNT = Convert.ToDouble(dataRow["TOTAL_AMOUNT"].ToString()),
                                                           STORE_NAME = dataRow["STORE_NAME"].ToString(),
                                                           CUSTOMER_TYPE = dataRow["CUSTOMER_TYPE"].ToString(),
                                                           DATE = ConvertDateFormat.ToUniversalIso8601(Convert.ToDateTime(dataRow["ENTRY_DATE"].ToString())),
                                                           TRANSACTION_ID = dataRow["TRANSACTION_ID"].ToString(),
                                                           IS_NEXT = Convert.ToBoolean(dataRow["IS_NEXT"].ToString()),
                                                           BOOKING_PAYMENT_RESPONSE = (from DataRow paymentDataRow in dataSet.Tables[1].Rows
                                                                                       where Convert.ToInt64(paymentDataRow["BOOKING_TRANSACTION_ID"].ToString()) == Convert.ToInt64(dataRow["BOOKING_TRANSACTION_ID"].ToString())
                                                                                       select new BookingPaymentResponseModel()
                                                                                       {
                                                                                           BOOKING_PAYMENT_DETAILS_ID = Convert.ToInt64(paymentDataRow["BOOKING_PAYMENT_DETAILS_ID"].ToString()),
                                                                                           BOOKING_TRANSACTION_ID = Convert.ToInt64(paymentDataRow["BOOKING_TRANSACTION_ID"].ToString()),
                                                                                           PAYMENT_MODE = paymentDataRow["PAYMENT_MODE"].ToString(),
                                                                                           AMOUNT = Convert.ToDouble(paymentDataRow["AMOUNT"].ToString())
                                                                                       }).ToList(),
                                                           BOOKING_ORDER_RESPONSE = (from DataRow orderDataRow in dataSet.Tables[2].Rows
                                                                                     where Convert.ToInt64(orderDataRow["BOOKING_TRANSACTION_ID"].ToString()) == Convert.ToInt64(dataRow["BOOKING_TRANSACTION_ID"].ToString())
                                                                                     select new BookingOrderResponseModel()
                                                                                     {
                                                                                         BOOKING_ORDER_DETAILS_ID = Convert.ToInt64(orderDataRow["BOOKING_ORDER_DETAILS_ID"].ToString()),
                                                                                         BOOKING_TRANSACTION_ID = Convert.ToInt64(orderDataRow["BOOKING_TRANSACTION_ID"].ToString()),
                                                                                         TRACKING_NUMBER = orderDataRow["TRACKING_NUMBER"].ToString()
                                                                                     }).ToList()

                                                       })
                                                       .ToList();
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstBookingTransactionReport;
        }

        public static int CancelTransactionRequest(CancelTransactionModel cancelTransactionModel)
        {
            int result = 0;
            string QUERY = string.Empty;
            try
            {
                SqlParameter sp1 = new SqlParameter("@TRANSACTION_ID", cancelTransactionModel.TRANSACTION_ID);
                SqlParameter sp2 = new SqlParameter("@CASHIER_ID", cancelTransactionModel.CASHIER_ID);
                SqlParameter sp3 = new SqlParameter("@STORE_ID", cancelTransactionModel.STORE_ID);
                SqlParameter sp4 = new SqlParameter("@CANCELLATION_ID", cancelTransactionModel.CANCELLATION_ID);
                SqlParameter sp5 = new SqlParameter("@CANCEL_AMOUNT", cancelTransactionModel.CANCEL_AMOUNT);
                SqlParameter sp6 = new SqlParameter("@FLAG", "3");
                DataTable dataTable = clsDataAccess.ExecuteDataTable(CommandType.StoredProcedure, "spCancelTransaction", sp1, sp2, sp3, sp4, sp5, sp6);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    string BOOKING_TRANSACTION_ID = dataTable.Rows[0]["BOOKING_TRANSACTION_ID"].ToString();
                    string CANCEL_BOOKING_TRANSACTION_ID = dataTable.Rows[0]["CANCEL_BOOKING_TRANSACTION_ID"].ToString();
                    string DATE_TIME = dataTable.Rows[0]["DATE_TIME"].ToString();

                    foreach (BookingOrderResponseModel bookingOrderResponseModel in cancelTransactionModel.WAYBILL_DETAILS)
                    {
                        if (!string.IsNullOrEmpty(bookingOrderResponseModel.TRACKING_NUMBER))
                        {
                            string WAYBILL_NUMBER = bookingOrderResponseModel.TRACKING_NUMBER;
                            string CANCELLED_BY = cancelTransactionModel.CASHIER_ID == 0 ? "0" : cancelTransactionModel.CASHIER_ID.ToString();
                            string STORE_ID = cancelTransactionModel.STORE_ID == 0 ? "0" : cancelTransactionModel.STORE_ID.ToString();
                            string CANCELLATION_ID = cancelTransactionModel.CANCELLATION_ID == 0 ? "0" : cancelTransactionModel.CANCELLATION_ID.ToString();
                            double CANCEL_AMOUNT = cancelTransactionModel.CANCEL_AMOUNT == 0 ? 0 : cancelTransactionModel.CANCEL_AMOUNT;

                            QUERY += " UPDATE BOOKING_ORDER_DETAILS SET IS_CANCELLED='1', CANCELLED_BY='" + CANCELLED_BY + "', CANCELLED_ON='" + DATE_TIME + "', CANCELLATION_ID='" + CANCELLATION_ID + "', CANCEL_BOOKING_TRANSACTION_ID='" + CANCEL_BOOKING_TRANSACTION_ID + "' WHERE TRACKING_NUMBER='" + WAYBILL_NUMBER + "' AND BOOKING_TRANSACTION_ID='" + BOOKING_TRANSACTION_ID + "';" + System.Environment.NewLine;
                        }
                    }
                    if (!string.IsNullOrEmpty(QUERY))
                    {
                        result = clsDataAccess.ExecuteNonQuery(CommandType.Text, QUERY);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        public static List<CancelTransactionModel> ReportCancelTransaction(long USER_ID, string FROM_DATE, string TO_DATE, string PAGE_NUMBER)
        {
            List<CancelTransactionModel> LstCancelTransaction = new List<CancelTransactionModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@CASHIER_ID", USER_ID);
                SqlParameter sp2 = new SqlParameter("@FROM_DATE", ConvertDateFormat.ConvertMMDDYYYY(FROM_DATE));
                SqlParameter sp3 = new SqlParameter("@TO_DATE", ConvertDateFormat.ConvertMMDDYYYY(TO_DATE));
                SqlParameter sp4 = new SqlParameter("@PAGE_NUMBER", PAGE_NUMBER);
                SqlParameter sp5 = new SqlParameter("@FLAG", "4");

                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spCancelTransaction", sp1, sp2, sp3, sp4, sp5);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        CancelTransactionModel cancelTransactionModel = new CancelTransactionModel();

                        cancelTransactionModel.CANCEL_BOOKING_TRANSACTION_ID = Convert.ToInt64(sqlDataReader["CANCEL_BOOKING_TRANSACTION_ID"].ToString());
                        cancelTransactionModel.TRANSACTION_ID = sqlDataReader["TRANSACTION_ID"].ToString();
                        cancelTransactionModel.CANCEL_AMOUNT = Convert.ToDouble(sqlDataReader["CANCEL_AMOUNT"].ToString());
                        cancelTransactionModel.DATE = ConvertDateFormat.ToUniversalIso8601(Convert.ToDateTime(sqlDataReader["DATE"].ToString()));
                        cancelTransactionModel.REASON = sqlDataReader["REASON"].ToString();
                        cancelTransactionModel.IS_NEXT = Convert.ToBoolean(sqlDataReader["IS_NEXT"].ToString());
                        cancelTransactionModel.STATUS = sqlDataReader["STATUS"].ToString();

                        LstCancelTransaction.Add(cancelTransactionModel);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstCancelTransaction;

        }

        public static BookingTransactionMasterModel ReportBookingTransactionOnId(string BOOKING_TRANSACTION_ID)
        {
            BookingTransactionMasterModel bookingTransactionMasterModel = new BookingTransactionMasterModel();
            string LstBookingTransactionId = string.Empty;
            try
            {
                SqlParameter sp1 = new SqlParameter("@BOOKING_TRANSACTION_ID", BOOKING_TRANSACTION_ID);
                SqlParameter sp2 = new SqlParameter("@FLAG", "2");
                DataSet dataSet = clsDataAccess.ExecuteDataset(CommandType.StoredProcedure, "spBookingMobileReport", sp1, sp2);
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count > 0)
                    {
                        bookingTransactionMasterModel = (from DataRow dataRow in dataSet.Tables[0].Rows
                                                         select new BookingTransactionMasterModel()
                                                         {
                                                             BOOKING_TRANSACTION_ID = Convert.ToInt64(dataRow["BOOKING_TRANSACTION_ID"].ToString()),
                                                             CASHIER_NAME = dataRow["FIRST_NAME"].ToString() + " " + dataRow["LAST_NAME"].ToString(),
                                                             IMEI_NUMBER = dataRow["IMEI_NUMBER"].ToString(),
                                                             CUSTOMER_NAME = dataRow["CUSTOMER_NAME"].ToString(),
                                                             CUSTOMER_CONTACT = dataRow["CUSTOMER_CONTACT"].ToString(),
                                                             TOTAL_AMOUNT = Convert.ToDouble(dataRow["TOTAL_AMOUNT"].ToString()),
                                                             STORE_NAME = dataRow["STORE_NAME"].ToString(),
                                                             CUSTOMER_TYPE = dataRow["CUSTOMER_TYPE"].ToString(),
                                                             TRANSACTION_ID = dataRow["TRANSACTION_ID"].ToString(),
                                                             DATE = ConvertDateFormat.ToUniversalIso8601(Convert.ToDateTime(dataRow["ENTRY_DATE"].ToString())),
                                                             BOOKING_PAYMENT_RESPONSE = (from DataRow paymentDataRow in dataSet.Tables[1].Rows
                                                                                         where Convert.ToInt64(paymentDataRow["BOOKING_TRANSACTION_ID"].ToString()) == Convert.ToInt64(dataRow["BOOKING_TRANSACTION_ID"].ToString())
                                                                                         select new BookingPaymentResponseModel()
                                                                                         {
                                                                                             BOOKING_PAYMENT_DETAILS_ID = Convert.ToInt64(paymentDataRow["BOOKING_PAYMENT_DETAILS_ID"].ToString()),
                                                                                             BOOKING_TRANSACTION_ID = Convert.ToInt64(paymentDataRow["BOOKING_TRANSACTION_ID"].ToString()),
                                                                                             PAYMENT_MODE = paymentDataRow["PAYMENT_MODE"].ToString(),
                                                                                             AMOUNT = Convert.ToDouble(paymentDataRow["AMOUNT"].ToString())
                                                                                         }).ToList(),
                                                             BOOKING_ORDER_RESPONSE = (from DataRow orderDataRow in dataSet.Tables[2].Rows
                                                                                       where Convert.ToInt64(orderDataRow["BOOKING_TRANSACTION_ID"].ToString()) == Convert.ToInt64(dataRow["BOOKING_TRANSACTION_ID"].ToString())
                                                                                       select new BookingOrderResponseModel()
                                                                                       {
                                                                                           BOOKING_ORDER_DETAILS_ID = Convert.ToInt64(orderDataRow["BOOKING_ORDER_DETAILS_ID"].ToString()),
                                                                                           BOOKING_TRANSACTION_ID = Convert.ToInt64(orderDataRow["BOOKING_TRANSACTION_ID"].ToString()),
                                                                                           TRACKING_NUMBER = orderDataRow["TRACKING_NUMBER"].ToString()
                                                                                       }).ToList()

                                                         }).SingleOrDefault();
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return bookingTransactionMasterModel;
        }

        public static void DynamicExportToPDF(string TRANSACTION_ID, string TRANSACTION_DATE, string TRANSACTION_BY, string CUSTOMER_PIN, string CUSTOMER_MOBILE, string PAYMENT_BY, string WAYBILL, double INVOICE_AMOUNT, double TAX_RATE, double TAX_AMOUNT, double AMOUNT_DUE, double TOTAL_AMOUNT, string CU_NUMBER, string FISCAL_TRANSACTION_NUMBER, string QR, string DOMAIN_NAME)
        {
            string stringPDF = string.Empty;
            string QRFileName = string.Empty;
            try
            {
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/DynamicView/BookingTransactionPDF.html")))
                {
                    stringPDF = reader.ReadToEnd();
                }
                GenerateQRCode(QR, TRANSACTION_ID, DOMAIN_NAME, out QRFileName);
                string QR_PATH = DOMAIN_NAME + "Invoices/QRCodes/" + QRFileName;
                stringPDF = stringPDF.Replace("{TRANSACTION_ID}", TRANSACTION_ID.ToString());
                stringPDF = stringPDF.Replace("{TRANSACTION_DATE}", TRANSACTION_DATE.ToString());
                stringPDF = stringPDF.Replace("{TRANSACTION_BY}", TRANSACTION_BY.ToString());
                stringPDF = stringPDF.Replace("{CUSTOMER_PIN}", CUSTOMER_PIN);
                stringPDF = stringPDF.Replace("{CUSTOMER_MOBILE}", CUSTOMER_MOBILE.ToString());
                stringPDF = stringPDF.Replace("{PAYMENT_BY}", PAYMENT_BY.ToString());
                stringPDF = stringPDF.Replace("{WAYBILL}", WAYBILL.ToString());
                stringPDF = stringPDF.Replace("{INVOICE_AMOUNT}", INVOICE_AMOUNT.ToString());
                stringPDF = stringPDF.Replace("{TAX_RATE}", TAX_RATE.ToString());
                stringPDF = stringPDF.Replace("{TAX_AMOUNT}", TAX_AMOUNT.ToString());
                stringPDF = stringPDF.Replace("{AMOUNT_DUE}", AMOUNT_DUE.ToString());
                stringPDF = stringPDF.Replace("{TOTAL_AMOUNT}", TOTAL_AMOUNT.ToString());
                stringPDF = stringPDF.Replace("{CU_NUMBER}", CU_NUMBER.ToString());
                stringPDF = stringPDF.Replace("{FISCAL_TRANSACTION_NUMBER}", FISCAL_TRANSACTION_NUMBER.ToString());
                stringPDF = stringPDF.Replace("{QR_PATH}", QR_PATH.ToString());

                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    var pgSize = new iTextSharp.text.Rectangle(150, 420);
                    string ExistingFilePath = Path.Combine(HostingEnvironment.MapPath("~/Invoices/"), "INVOICE_" + TRANSACTION_ID + ".pdf");
                    if (File.Exists(ExistingFilePath))
                    {
                        File.Delete(ExistingFilePath);
                    }
                    string FilePath = HostingEnvironment.MapPath("~/Invoices/");
                    StringReader stringReader = new StringReader(stringPDF);
                    //Document document = new Document(PageSize.A8, 0f, 0f, 0f, 0f);
                    Document document = new Document(pgSize, 0f, 0f, 5, 0f);
                    if (document.IsOpen() == false)
                    {
                        document.Open();
                    }
                    PdfWriter pdfWriter = PdfWriter.GetInstance(document, new FileStream(FilePath + "\\" + "INVOICE_" + TRANSACTION_ID + ".pdf", FileMode.Create));
                    document.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(pdfWriter, document, stringReader);
                    document.Close();
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
        }

        public static string GenerateQRCode(string QRText, string TransactionId, string DOMAIN_NAME, out string QRFileName)
        {
            string ImageURL = string.Empty;
            QRFileName = string.Empty;
            try
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(QRText, QRCodeGenerator.ECCLevel.Q);
                System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
                imgBarCode.Height = 100;
                imgBarCode.Width = 100;
                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] byteImage = ms.ToArray();
                        //ImageURL = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                        ImageURL = Convert.ToBase64String(byteImage);
                    }
                    imgBarCode.ImageUrl = ImageURL;
                    byte[] FromBase64StringToImageByte = Convert.FromBase64String(ImageURL);
                    bool IsPhysicalQRPathExists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Invoices/QRCodes"));

                    if (IsPhysicalQRPathExists)
                    {

                        QRFileName = "QR_" + TransactionId + ".png";
                        string PhysicalQRPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Invoices/QRCodes"), Path.GetFileName(QRFileName));
                        bool IsFileExists = System.IO.Directory.Exists(Path.Combine(HttpContext.Current.Server.MapPath("~/Invoices/QRCodes"), Path.GetFileName(QRFileName)));
                        if (IsFileExists)
                        {
                            File.Delete(PhysicalQRPath);
                        }
                        System.IO.File.WriteAllBytes(PhysicalQRPath, FromBase64StringToImageByte);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return ImageURL;
        }

        public static bool IsValidWaybillNumber(string WAYBILL_NO)
        {
            bool IsValidWaybillNumber = true;
            try
            {
                double Digit = 0;
                if (!string.IsNullOrEmpty(WAYBILL_NO))
                {

                    if (WAYBILL_NO.Contains("FCLIN"))
                    {
                        string initCode = WAYBILL_NO.Substring(0, 5);
                        string initDigit = WAYBILL_NO.Substring(5, 8);
                        IsValidWaybillNumber = double.TryParse(initDigit, out Digit);
                        if (!IsValidWaybillNumber)
                        {
                            IsValidWaybillNumber = false;
                        }
                    }
                    else if (WAYBILL_NO.Contains("FCLCA"))
                    {
                        string initCode = WAYBILL_NO.Substring(0, 5);
                        string initDigit = WAYBILL_NO.Substring(5, 8);
                        IsValidWaybillNumber = double.TryParse(initDigit, out Digit);
                        if (!IsValidWaybillNumber)
                        {
                            IsValidWaybillNumber = false;
                        }
                    }
                    else if (WAYBILL_NO.Contains("FCL"))
                    {
                        string initCode = WAYBILL_NO.Substring(0, 3);
                        string initDigit = WAYBILL_NO.Substring(3, 8);
                        IsValidWaybillNumber = double.TryParse(initDigit, out Digit);
                        if (!IsValidWaybillNumber)
                        {
                            IsValidWaybillNumber = false;
                        }
                    }
                    else
                    {
                        IsValidWaybillNumber = false;
                    }
                }
                else
                {
                    IsValidWaybillNumber = false;
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
                IsValidWaybillNumber = false;
            };
            return IsValidWaybillNumber;
        }

        public static bool HasNetBalance(BookingTransactionMasterModel bookingTransactionMaster)
        {
            bool HasNetBalance = true;
            try
            {
                SqlParameter sp1 = new SqlParameter("@CUSTOMER_ID", bookingTransactionMaster.CUSTOMER_ID);
                SqlParameter sp2 = new SqlParameter("@FLAG", "3");
                DataTable dataTable = clsDataAccess.ExecuteDataTable(CommandType.StoredProcedure, "spCreditEntryInsert", sp1, sp2);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    double NET_AMOUNT = Convert.ToDouble(dataTable.Rows[0][0].ToString());
                    if (bookingTransactionMaster.TOTAL_AMOUNT <= NET_AMOUNT)
                        HasNetBalance = true;
                    else
                        HasNetBalance = false;
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return HasNetBalance;
        }

        #region CHECKING FOR MPESA PAYMENT TRANSACTION
        public static bool IsMPesaTransaction(string Type, BookingTransactionMasterModel bookingTransactionMaster, CreditEntryModel creditEntryModel, out double MPesaAmount)
        {
            bool IsMPesaTransaction = false;
            MPesaAmount = 0;
            try
            {
                if (Type.Equals("Booking"))
                {
                    if (bookingTransactionMaster != null)
                    {
                        if (bookingTransactionMaster.BOOKING_PAYMENT_DETAILS != null)
                        {
                            foreach (BookingPaymentDetailsModel bookingPaymentDetailsModel in bookingTransactionMaster.BOOKING_PAYMENT_DETAILS)
                            {
                                if (!string.IsNullOrEmpty(bookingPaymentDetailsModel.PAYMENT_MODE))
                                {
                                    if ((bookingPaymentDetailsModel.PAYMENT_MODE.ToUpper().Equals("MPESA")) && (bookingPaymentDetailsModel.AMOUNT > 0))
                                    {
                                        MPesaAmount = bookingPaymentDetailsModel.AMOUNT;
                                        IsMPesaTransaction = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (Type.Equals("Credit"))
                {
                    if (creditEntryModel != null)
                    {
                        if (creditEntryModel.CREDIT_MPESA_TRANSACTION != null)
                        {
                            if (!string.IsNullOrEmpty(creditEntryModel.PAYMENT_MODE))
                            {
                                if (creditEntryModel.PAYMENT_MODE.ToUpper().Equals("MPESA") && (creditEntryModel.CREDIT_ENTRY_AMOUNT > 0))
                                {
                                    MPesaAmount = creditEntryModel.CREDIT_ENTRY_AMOUNT;
                                    IsMPesaTransaction = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return IsMPesaTransaction;
        }
        #endregion


        public static bool IsMPesaValidResponse(double MPesaAmount, string MerchantRequestID, string CheckoutRequestID, out BookingResponseModel bookingResponseModel)
        {
            bool IsMPesaValidResponse = false; bookingResponseModel = new BookingResponseModel();
            try
            {
                if (!string.IsNullOrEmpty(MerchantRequestID) && !string.IsNullOrEmpty(CheckoutRequestID))
                {
                    SqlParameter sp1 = new SqlParameter("@MERCHANT_REQUEST_ID", MerchantRequestID);
                    SqlParameter sp2 = new SqlParameter("@CHECKOUT_REQUEST_ID", CheckoutRequestID);
                    SqlParameter sp3 = new SqlParameter("@FLAG", "5");
                    DataTable dataTable = clsDataAccess.ExecuteDataTable(CommandType.StoredProcedure, "spMPesaTransaction", sp1, sp2, sp3);
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        string STATUS = dataTable.Rows[0][0].ToString();
                        string RESULT_CODE = dataTable.Rows[0]["RESULT_CODE"].ToString();
                        string RESULT_DESC = dataTable.Rows[0]["RESULT_DESC"].ToString();
                        string AMOUNT = dataTable.Rows[0]["AMOUNT"].ToString();
                        string MPESA_RECEIPT_NUMBER = dataTable.Rows[0]["MPESA_RECEIPT_NUMBER"].ToString();
                        string TRANSACTION_DATE = dataTable.Rows[0]["TRANSACTION_DATE"].ToString();
                        string PHONE_NUMBER = dataTable.Rows[0]["PHONE_NUMBER"].ToString();

                        if (!string.IsNullOrEmpty(RESULT_CODE))
                        {
                            //if ((RESULT_CODE == "0") && (MPesaAmount == Convert.ToDouble(AMOUNT)))
                            if ((RESULT_CODE == "0"))
                            {
                                IsMPesaValidResponse = true;
                                bookingResponseModel.TransactionId = null;
                                bookingResponseModel.Status = "Success";
                                bookingResponseModel.Message = "MPesa transaction is successful";
                                bookingResponseModel.Description = RESULT_DESC.ToString();
                            }
                            else if (RESULT_CODE == "1032") //M-Pesa declined by user
                            {
                                IsMPesaValidResponse = false;
                                bookingResponseModel.TransactionId = null;
                                bookingResponseModel.Status = "Declined";
                                bookingResponseModel.Message = "MPesa transaction is not done.";
                                bookingResponseModel.Description = RESULT_DESC.ToString();
                            }
                            else //could be any RESULT_CODE other than 0 or 1032; Mpesa unsuccessful
                            {
                                IsMPesaValidResponse = false;
                                bookingResponseModel.TransactionId = null;
                                bookingResponseModel.Status = "Failed";
                                bookingResponseModel.Message = "MPesa transaction is not done.";
                                bookingResponseModel.Description = RESULT_DESC.ToString();
                            }
                        }
                        //when callback request still doesnot came.
                        else
                        {
                            IsMPesaValidResponse = false;
                            bookingResponseModel.TransactionId = null;
                            bookingResponseModel.Status = "Processing";
                            bookingResponseModel.Message = "MPesa transaction is in process.";
                            bookingResponseModel.Description = "Please try again";
                        }
                    }
                    else
                    {
                        IsMPesaValidResponse = false;
                        bookingResponseModel.TransactionId = null;
                        bookingResponseModel.Status = "Invalid";
                        bookingResponseModel.Message = "Invalid MPesa transaction.";
                        bookingResponseModel.Description = "Invalid MPesa transaction.";
                    }
                }
                else
                {
                    IsMPesaValidResponse = true;
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return IsMPesaValidResponse;
        }


        public static bool IsValidCancelTransactionRequest(string TransactionId)
        {
            bool IsValidCancelTransactionRequest = false;
            try
            {
                SqlParameter sp1 = new SqlParameter("@TRANSACTION_ID", TransactionId);
                SqlParameter sp2 = new SqlParameter("@FLAG", "8");
                DataTable dataTable = clsDataAccess.ExecuteDataTable(CommandType.StoredProcedure, "spCancelTransaction", sp1, sp2);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    string BOOKING_TRANSACTION_ID = dataTable.Rows[0]["BOOKING_TRANSACTION_ID"].ToString();
                    string TRANSACTION_ID = dataTable.Rows[0]["TRANSACTION_ID"].ToString();
                    string STATUS = dataTable.Rows[0]["STATUS"].ToString();
                    bool IS_MANAGER_APPROVED = Convert.ToBoolean(dataTable.Rows[0]["IS_MANAGER_APPROVED"].ToString());
                    if (STATUS.Equals("C") && !IS_MANAGER_APPROVED)
                    {
                        IsValidCancelTransactionRequest = true;
                    }
                }
                else
                {
                    IsValidCancelTransactionRequest = true;
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return IsValidCancelTransactionRequest;
        }



        public static bool IsValidWaybillNumber(BookingTransactionMasterModel bookingTransactionMasterModel)
        {
            bool IsValidWaybillNumber = false;
            try
            {
                double Digit = 0;
                foreach (BookingOrderDetailsModel bookingOrderDetailsModel in bookingTransactionMasterModel.BOOKING_ORDER_DETAILS)
                {
                    string WAYBILL_NO = bookingOrderDetailsModel.TRACKING_NUMBER;
                    if (!string.IsNullOrEmpty(WAYBILL_NO))
                    {
                        if (WAYBILL_NO.Contains("FCLIN"))
                        {
                            string initCode = WAYBILL_NO.Substring(0, 5);
                            string initDigit = WAYBILL_NO.Substring(5, 8);
                            IsValidWaybillNumber = double.TryParse(initDigit, out Digit);
                            if (!IsValidWaybillNumber) break;
                        }
                        else if (WAYBILL_NO.Contains("FCLCA"))
                        {
                            string initCode = WAYBILL_NO.Substring(0, 5);
                            string initDigit = WAYBILL_NO.Substring(5, 8);
                            IsValidWaybillNumber = double.TryParse(initDigit, out Digit);
                            if (!IsValidWaybillNumber) break;
                        }
                        else if (WAYBILL_NO.Contains("FCL"))
                        {
                            string initCode = WAYBILL_NO.Substring(0, 3);
                            string initDigit = WAYBILL_NO.Substring(3, 8);
                            IsValidWaybillNumber = double.TryParse(initDigit, out Digit);
                            if (!IsValidWaybillNumber) break;
                        }
                        else
                        {
                            IsValidWaybillNumber = false;
                        }
                    }
                    else
                    {
                        IsValidWaybillNumber = false; break;
                    }
                }

            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            };
            return IsValidWaybillNumber;
        }

        public static bool IsDuplicateWaybillFound(BookingTransactionMasterModel bookingTransactionMasterModel)
        {
            bool IsDuplicateWaybillFound = true;
            try
            {
                string LstWaybills = string.Empty;
                foreach (BookingOrderDetailsModel bookingOrderDetailsModel in bookingTransactionMasterModel.BOOKING_ORDER_DETAILS)
                {
                    string WAYBILL_NO = bookingOrderDetailsModel.TRACKING_NUMBER;
                    if (string.IsNullOrEmpty(WAYBILL_NO))
                    {
                        break;
                    }
                    else
                    {
                        LstWaybills += "'" + WAYBILL_NO + "',";
                    }
                }
                LstWaybills = LstWaybills.TrimEnd(',');
                string Query = "SELECT * FROM BOOKING_ORDER_DETAILS WHERE TRACKING_NUMBER IN (" + LstWaybills + ")";
                DataTable dataTable = clsDataAccess.ExecuteDataTable(CommandType.Text, Query);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    IsDuplicateWaybillFound = true;
                }
                else
                {
                    IsDuplicateWaybillFound = false;
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            };
            return IsDuplicateWaybillFound;
        }


    }
}