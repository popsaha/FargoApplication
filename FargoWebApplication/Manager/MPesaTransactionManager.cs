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
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace FargoWebApplication.Manager
{
    public class MPesaTransactionManager
    {
        // customer will send the request for mpesa transaction
        public static DataTable MPesaValidation(MPesaValidation mPesaValidation)
        {
            DataTable dataTable = new DataTable();
            try
            {
                SqlParameter sp1 = new SqlParameter("@CUSTOMER_NAME", mPesaValidation.CUSTOMER_NAME);
                SqlParameter sp2 = new SqlParameter("@CUSTOMER_MOBILE", mPesaValidation.CUSTOMER_MOBILE);
                SqlParameter sp3 = new SqlParameter("@MPESA_AMOUNT", mPesaValidation.MPESA_AMOUNT);
                SqlParameter sp4 = new SqlParameter("@CREATED_BY", mPesaValidation.USER_ID);
                SqlParameter sp5 = new SqlParameter("@FLAG", "1");
                dataTable = clsDataAccess.ExecuteDataTable(CommandType.StoredProcedure, "spMPesaTransaction", sp1, sp2, sp3, sp4, sp5);
                
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return dataTable;
        }

        // Step 1: generate a ACCESS TOKEN
        public static string GenerateAccessToken(string BasicAuthenticationCredentials)
        {
            string accessToken = string.Empty;
            try
            {
                string JSONResponse = "{";
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string URL = String.Format("https://sandbox.safaricom.co.ke/oauth/v1/generate?grant_type=client_credentials");
                WebRequest webRequest = WebRequest.Create(URL);
                webRequest.Method = "GET";
                webRequest.Headers["Authorization"] = "Basic " + BasicAuthenticationCredentials;
                webRequest.ContentType = "application/json";
                HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                string responseString = streamReader.ReadLine();
                while (responseString != null)
                {
                    Console.WriteLine(responseString);
                    responseString = streamReader.ReadLine();
                    JSONResponse = JSONResponse + responseString;
                }               
                GenerateTokenModel generateTokenModel = JsonConvert.DeserializeObject<GenerateTokenModel>(JSONResponse);
                accessToken = generateTokenModel.access_token;
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return accessToken;
        }

        //Step 2: Initiate the request to Safaricom
        public static int MPesaProcess(string MPESA_TRANSACTION_ID, string CUSTOMER_MOBILE, double MPESA_AMOUNT, string TIMESTAMP, string accessToken, string BusinessShortCode, string PasswordKey, out MPesaProcessResponseModel mPesaProcessResponseModel)
        {
            int result=0;
            mPesaProcessResponseModel = new MPesaProcessResponseModel();
            try
            {
                string JSONResponse = "{";
                MPesaProcessModel mPesaProcessModel = new MPesaProcessModel();
                mPesaProcessModel.BusinessShortCode = BusinessShortCode;
                mPesaProcessModel.Password = EncryptDecryptString.EncodeBase64(BusinessShortCode + PasswordKey + TIMESTAMP);
                mPesaProcessModel.Timestamp = TIMESTAMP;
                mPesaProcessModel.TransactionType = "CustomerPayBillOnline";
                mPesaProcessModel.Amount = MPESA_AMOUNT;
                //mPesaProcessModel.PartyA = "254708374149";
                mPesaProcessModel.PartyA = "254" + CUSTOMER_MOBILE; //The phone number sending money.
                mPesaProcessModel.PartyB = BusinessShortCode; //The organization receiving the funds.
                //mPesaProcessModel.PhoneNumber = "254708374149";
                mPesaProcessModel.PhoneNumber = "254" + CUSTOMER_MOBILE; //The Mobile Number to receive the STK Pin Prompt; This number can be the same as PartyA value above.
                

                // CALLBACK URL to receive the response from MPESA 
                mPesaProcessModel.CallBackURL = "https://fargo.speed18.com/api/MPesaTransactionAPI/MPesaTransactionRequest";

                //for testing callback API
                //mPesaProcessModel.CallBackURL = "https://fargo.speed18.com/api/MPesaTransactionAPI/MPesaTransactionTestRequest";

                mPesaProcessModel.AccountReference = "CompanyXLTD";
                mPesaProcessModel.TransactionDesc = "Payment of X";

                string JSONString = JsonConvert.SerializeObject(mPesaProcessModel);

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string URL = String.Format("https://sandbox.safaricom.co.ke/mpesa/stkpush/v1/processrequest");
                WebRequest webRequest = WebRequest.Create(URL);
                webRequest.Method = "POST";
                webRequest.Headers["Authorization"] = "Bearer " + accessToken;
                webRequest.ContentType = "application/json";

                using (var stramWriter= new StreamWriter(webRequest.GetRequestStream()))
                {
                    stramWriter.Write(JSONString);
                    stramWriter.Flush();
                    stramWriter.Close();
                    HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

                    StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                    string responseString = streamReader.ReadLine();
                    while (responseString != null)
                    {
                        responseString = streamReader.ReadLine();
                        JSONResponse = JSONResponse + responseString;
                    } 
                }
                string Base64EndcodedJSONResponse = EncryptDecryptString.EncodeBase64(JSONResponse);
                MPesaTransactionResponseModel mPesaTransactionResponseModel = JsonConvert.DeserializeObject<MPesaTransactionResponseModel>(JSONResponse);


                if (mPesaTransactionResponseModel.ResponseCode == "0")
                {
                    mPesaProcessResponseModel.Status = "Processing";
                    mPesaProcessResponseModel.MerchantRequestID = mPesaTransactionResponseModel.MerchantRequestID;
                    mPesaProcessResponseModel.CheckoutRequestID = mPesaTransactionResponseModel.CheckoutRequestID;
                    mPesaProcessResponseModel.ResponseCode = mPesaTransactionResponseModel.ResponseCode;
                    mPesaProcessResponseModel.CustomerMessage = mPesaTransactionResponseModel.CustomerMessage;
                    mPesaProcessResponseModel.ResponseDescription = mPesaTransactionResponseModel.ResponseDescription;
                    
                }
                else
                {
                    mPesaProcessResponseModel.Status = "Failed";
                    mPesaProcessResponseModel.MerchantRequestID = mPesaTransactionResponseModel.MerchantRequestID;
                    mPesaProcessResponseModel.CheckoutRequestID = mPesaTransactionResponseModel.CheckoutRequestID;
                    mPesaProcessResponseModel.ResponseCode = mPesaTransactionResponseModel.ResponseCode;
                    mPesaProcessResponseModel.CustomerMessage = mPesaTransactionResponseModel.CustomerMessage;
                    mPesaProcessResponseModel.ResponseDescription = mPesaTransactionResponseModel.ResponseDescription;
                }
                SqlParameter sp1 = new SqlParameter("@FLAG", "2");
                SqlParameter sp2 = new SqlParameter("@MPESA_TRANSACTION_ID", MPESA_TRANSACTION_ID);
                SqlParameter sp3 = new SqlParameter("@MERCHANT_REQUEST_ID", mPesaTransactionResponseModel.MerchantRequestID);
                SqlParameter sp4 = new SqlParameter("@CHECKOUT_REQUEST_ID", mPesaTransactionResponseModel.CheckoutRequestID);
                SqlParameter sp5 = new SqlParameter("@MPESA_PROCESS_RESPONSE", Base64EndcodedJSONResponse);
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spMPesaTransaction", sp1, sp2, sp3, sp4, sp5);
               
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        
        // Step 3a: CALLBACK RESPONSE in JSON from Safaricom
        #region METHOD to capture the JSON sent by MPESA to our CALLBACK API.
        public static int MPesaTransactionTestRequest(string JSONRequest)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@JSONRequest", JSONRequest);
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spMpesaTest", sp1);
                
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }
        #endregion

        //Step 3b: convert CALLBACK RESPONSE into values to insert in table
        #region Method to save the data of CALLBACK RESPONSE JSON to the [MPESA_TRANSACTION] table.
        public static int MPesaTransactionRequest(MpesaCallbackResponseBody mpesaCallbackResponseBody)
        {
            int result = 0;
            try
            {
                string Value1 = string.Empty;
                string Value2 = string.Empty;
                string Value3 = string.Empty;
                string Value4 = string.Empty;
                
                if (mpesaCallbackResponseBody.Body.StkCallback.ResultCode == 0)
                {
                    Value1 = mpesaCallbackResponseBody.Body.StkCallback.CallbackMetadata.Item[0].Value;
                    Value2 = mpesaCallbackResponseBody.Body.StkCallback.CallbackMetadata.Item[1].Value;
                    Value3 = mpesaCallbackResponseBody.Body.StkCallback.CallbackMetadata.Item[2].Value;
                    Value4 = mpesaCallbackResponseBody.Body.StkCallback.CallbackMetadata.Item[3].Value;
                }
                string JSONRequest = JsonConvert.SerializeObject(mpesaCallbackResponseBody);
                string Base64EndcodedJSONRequest = EncryptDecryptString.EncodeBase64(JSONRequest);

                SqlParameter MERCHANT_REQUEST_ID_CALLBACK = new SqlParameter("@MERCHANT_REQUEST_ID", mpesaCallbackResponseBody.Body.StkCallback.MerchantRequestID);
                SqlParameter CHECKOUT_REQUEST_ID_CALLBACK = new SqlParameter("@CHECKOUT_REQUEST_ID", mpesaCallbackResponseBody.Body.StkCallback.CheckoutRequestID);
                SqlParameter MPESA_CALLBACK_REQUEST = new SqlParameter("@MPESA_CALLBACK_REQUEST", Base64EndcodedJSONRequest);
                SqlParameter RESULT_CODE = new SqlParameter("@RESULT_CODE", mpesaCallbackResponseBody.Body.StkCallback.ResultCode);
                SqlParameter RESULT_DESC = new SqlParameter("@RESULT_DESC", mpesaCallbackResponseBody.Body.StkCallback.ResultDesc);
                SqlParameter AMOUNT = new SqlParameter("@AMOUNT", Value1);
                SqlParameter MPESA_RECEIPT_NUMBER = new SqlParameter("@MPESA_RECEIPT_NUMBER", Value2);
                SqlParameter TRANSACTION_DATE = new SqlParameter("@TRANSACTION_DATE", Value3);
                SqlParameter PHONE_NUMBER = new SqlParameter("@PHONE_NUMBER", Value4);
                SqlParameter JSON_REQUEST = new SqlParameter("@JSONRequest", JSONRequest);
                SqlParameter FLAG = new SqlParameter("@FLAG", "3");
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spMPesaTransaction", MERCHANT_REQUEST_ID_CALLBACK, CHECKOUT_REQUEST_ID_CALLBACK, MPESA_CALLBACK_REQUEST, RESULT_CODE, RESULT_DESC, AMOUNT, MPESA_RECEIPT_NUMBER, TRANSACTION_DATE, PHONE_NUMBER, JSON_REQUEST, FLAG);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }
        #endregion

        //[+][13-09-2022][snehashish]
        public static MPesaTransactionModel MPesaTransactionCallbackData(string MPESA_TRANSACTION_ID)
        {
            MPesaTransactionModel mPesaTransactionModel = new MPesaTransactionModel();
            try
            {
                SqlParameter sp1 = new SqlParameter("@MPESA_TRANSACTION_ID", MPESA_TRANSACTION_ID); //MPESA_TRANSACTION_ID
                SqlParameter sp2 = new SqlParameter("@FLAG", "4");
                DataTable dataTable = clsDataAccess.ExecuteDataTable(CommandType.StoredProcedure, "spMPesaTransaction", sp1, sp2);
                if(dataTable != null && dataTable.Rows.Count > 0)
                {
                    mPesaTransactionModel.MerchantRequestID = dataTable.Rows[0]["MERCHANT_REQUEST_ID_CALLBACK"].ToString();
                    mPesaTransactionModel.CheckoutRequestID = dataTable.Rows[0]["CHECKOUT_REQUEST_ID_CALLBACK"].ToString();
                    if(dataTable.Rows[0]["RESULT_CODE"] != null)
                    {
                        mPesaTransactionModel.ResultCode = Convert.ToInt32(dataTable.Rows[0]["RESULT_CODE"]);
                    }                    
                    mPesaTransactionModel.ResultDesc = dataTable.Rows[0]["RESULT_DESC"].ToString();
                    if(mPesaTransactionModel.ResultCode == 0)
                    {                        
                            mPesaTransactionModel.CallbackMetadata.Item[0].Value = dataTable.Rows[0]["AMOUNT"].ToString();
                            mPesaTransactionModel.CallbackMetadata.Item[1].Value = dataTable.Rows[0]["MPESA_RECEIPT_NUMBER"].ToString();
                            mPesaTransactionModel.CallbackMetadata.Item[2].Value = dataTable.Rows[0]["TRANSACTION_DATE"].ToString();
                            mPesaTransactionModel.CallbackMetadata.Item[3].Value = dataTable.Rows[0]["PHONE_NUMBER"].ToString();
                        
                        
                    }
                    
                }

            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return mPesaTransactionModel;
        }
        //[-][13-09-2022][snehashish]

        #region DELETE - used for testing purpose.
        public static int TestAPI()
        {
            int result = 0;
            try
            {
                string TEST_NAME = "TEST_NAME_1";
                string TEST_TYPE = "TEST_TYPE_1";
                SqlParameter sp1 = new SqlParameter("@TEST_NAME", TEST_NAME);
                SqlParameter sp2 = new SqlParameter("@TEST_TYPE", TEST_TYPE);
                SqlParameter sp3 = new SqlParameter("@FLAG", "1");

                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spTestTable", sp1, sp2, sp3);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }
        #endregion


    }
}