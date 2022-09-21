using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fargo_DataAccessLayers;
using FargoWebApplication.Filter;
using Fargo_Application.App_Start;
using Fargo_Models;
using FargoWebApplication.Manager;
using System.Threading;
using System.Data;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace FargoWebApplication.FargoAPI
{
   
    public class MPesaTransactionAPIController : ApiController
    {
        [BasicAuthentication]
        [HttpPost]
        [Route("api/MPesaTransactionAPI/MPesaValidation")]
        public HttpResponseMessage MPesaValidation([FromBody] MPesaValidation mPesaValidation)
        {          
            try
            {
                int result = 0;
               
                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    MPesaProcessResponseModel mPesaProcessResponseModel = new MPesaProcessResponseModel();
                    DataTable dataTable = MPesaTransactionManager.MPesaValidation(mPesaValidation);
                    if (dataTable != null && dataTable.Rows.Count>0)
                    {
                        string MPESA_TRANSACTION_ID = dataTable.Rows[0]["MPESA_TRANSACTION_ID"].ToString();
                        string CUSTOMER_NAME = dataTable.Rows[0]["CUSTOMER_NAME"].ToString();
                        string CUSTOMER_MOBILE = dataTable.Rows[0]["CUSTOMER_MOBILE"].ToString();
                        double MPESA_AMOUNT = Convert.ToDouble(dataTable.Rows[0]["MPESA_AMOUNT"].ToString());
                        string CREATED_ON = dataTable.Rows[0]["CREATED_ON"].ToString();
                        string TIMESTAMP = dataTable.Rows[0]["TIMESTAMP"].ToString();
                        string MERCHANT_REQUEST_ID = String.Empty;
                        string CHECKOUT_REQUEST_ID = String.Empty;

                        string accessToken = MPesaTransactionManager.GenerateAccessToken();

                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            result = MPesaTransactionManager.MPesaProcess(MPESA_TRANSACTION_ID, CUSTOMER_MOBILE, MPESA_AMOUNT, TIMESTAMP, accessToken, out mPesaProcessResponseModel);
                            if (mPesaProcessResponseModel != null && result > 0)
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, mPesaProcessResponseModel);
                            }
                            else
                            {
                                responseModel.Status = "Failed";
                                responseModel.Message = "MPesa transaction not done.";
                                responseModel.Description = "MPesa transaction not done.";
                                return Request.CreateResponse(HttpStatusCode.InternalServerError,       responseModel);
                            }                           
                        }
                        else
                        {
                            responseModel.Status = "Failed";
                            responseModel.Message = "MPesa transaction not done.";
                            responseModel.Description = "MPesa transaction not done.";
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, responseModel);
                        }
                    }
                    else
                    {
                        responseModel.Status = "Failed";
                        responseModel.Message = "MPesa transaction not done.";
                        responseModel.Description = "MPesa transaction not done.";
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, responseModel);
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, new Exception("Unauthorized, Please try again."));
                }
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message.ToString());
            }
        }

        #region  CALLBACK API.
        [HttpPost]
        [Route("api/MPesaTransactionAPI/MPesaTransactionRequest")]
        public HttpResponseMessage MPesaTransactionRequest()
        {
            int result = 0;
            try
            {
                ResponseModel responseModel = new ResponseModel();
                //callback response
                string JSONRequest = getRawPostData().Result;

                MpesaCallbackResponseBody mpesaCallbackResponseBody = JsonConvert.DeserializeObject<MpesaCallbackResponseBody>(JSONRequest);
                //save callback response to [MPESA_TRANSACTION] table
                result = MPesaTransactionManager.MPesaTransactionRequest(mpesaCallbackResponseBody);
                if (result > 0)
                {
                    responseModel.Status = "Success";
                    responseModel.Message = "Transaction successfully made.";
                    responseModel.Description = "Transaction successfully made.";
                    return Request.CreateResponse(HttpStatusCode.Created, responseModel);
                    
                }
                else
                {
                    responseModel.Status = "Failed";
                    responseModel.Message = "Transaction not done.";
                    responseModel.Description = "Transaction not done.";
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, responseModel);
                }
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message.ToString());
            }
        }
        private async Task<String> getRawPostData()
        {
            using (var contentStream = await this.Request.Content.ReadAsStreamAsync())
            {
                contentStream.Seek(0, SeekOrigin.Begin);
                using (var sr = new StreamReader(contentStream))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        #endregion

        #region  ignore this callback API
        [HttpPost]
        [Route("api/MPesaTransactionAPI/MPesaTransactionTestRequest")]
        public HttpResponseMessage MPesaTransactionTestRequest([FromBody] MPesaTransactionModel mPesaTransactionModel)
        {

            try
            {
                ResponseModel responseModel = new ResponseModel();
                //string Username = Thread.CurrentPrincipal.Identity.Name;
                //if (!string.IsNullOrEmpty(Username))
                //{

                //int result = MPesaTransactionManager.MPesaTransactionRequest(mPesaTransactionModel);
                int result = 0;
                    if (result > 0)
                    {
                        //ResultCode: 0 => The service request is processed successfully.
                        if (mPesaTransactionModel.ResultCode == 0)
                            {
                                MPesaProcessModel mPesaProcessModel = new MPesaProcessModel();
                                if (mPesaTransactionModel.CallbackMetadata.Item[0].Value == (object)mPesaProcessModel.Amount)
                                {
                                    responseModel.Status = "Success";
                                    responseModel.Message = "Transaction successfully made.";
                                    responseModel.Description = mPesaTransactionModel.ResultDesc;
                                    return Request.CreateResponse(HttpStatusCode.Created, responseModel);
                                }
                                else
                                {
                                    responseModel.Status = "Failed";
                                    responseModel.Message = "Transaction not made.";
                                    responseModel.Description = mPesaTransactionModel.ResultDesc;
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, responseModel);
                                }
                            }

                        //ResultCode: 1032 => Request cancelled by user
                        else if (mPesaTransactionModel.ResultCode == 1032)
                            {

                                responseModel.Status = "Failed";
                                responseModel.Message = "Transaction not made.";
                                responseModel.Description = mPesaTransactionModel.ResultDesc;
                                return Request.CreateResponse(HttpStatusCode.BadRequest, responseModel);
                            }

                        //ResultCode: 1037... => any other code means an error occurred or the transaction failed.
                        else
                            {
                                responseModel.Status = "Failed";
                                responseModel.Message = "Transaction not made.";
                                responseModel.Description = "The service request is not processed.";
                                responseModel.Description = mPesaTransactionModel.ResultDesc; ;
                                return Request.CreateResponse(HttpStatusCode.InternalServerError, responseModel);
                            }

                    }
                    else
                    {
                        responseModel.Status = "Failed";
                        responseModel.Message = "Transaction not made.";
                        responseModel.Description = "The service request is not processed.";
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, responseModel);
                    }


                //}
                //else
                //{
                //    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, new Exception("Unauthorized, Please try again."));
                //}
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message.ToString());
            }
        }
        #endregion

        

    }
}
