using Fargo_Application.App_Start;
using Fargo_Models;
using FargoWebApplication.Filter;
using FargoWebApplication.Manager;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace FargoWebApplication.FargoAPI
{
    [BasicAuthentication]
    public class CreditCustomerAPIController : ApiController
    {
        #region [GET_API] - for list of Credit Customers
        [HttpGet]
        [Route("api/CreditCustomerAPI/LstCreditCustomers")]
        public IHttpActionResult LstCreditCustomers()
        {
            List<CreditCustomerModel> LstCreditCustomers = new List<CreditCustomerModel>();
            try
            {
                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    LstCreditCustomers = CreditCustomerManager.LstCreditCustomers();

                    if (LstCreditCustomers.Count == 0)
                    {
                        responseModel.Status = "Success";
                        responseModel.Message = "No record found.";
                        responseModel.Description = "No record found.";
                        return Ok(responseModel);
                    }
                    return Ok(LstCreditCustomers);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                return InternalServerError();
            }
        }
        #endregion

        #region [POST_API] - for credit entry in credit customer's a/c
        [HttpPost]
        [Route("api/CreditCustomerAPI/CreditEntry")]

        public HttpResponseMessage CreditEntry([FromBody] CreditEntryModel creditEntryModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    BookingResponseModel bookingResponseModel = new BookingResponseModel();
                    double MPesaAmount = 0;
                    if (BookingTransactionMasterManager.IsMPesaTransaction("Credit",null,creditEntryModel, out MPesaAmount))
                    {
                    string MerchantRequestID = string.Empty; string CheckoutRequestID = string.Empty;
                    if (creditEntryModel != null)
                    {
                        if (creditEntryModel.CREDIT_MPESA_TRANSACTION != null)
                        {
                            MerchantRequestID = creditEntryModel.CREDIT_MPESA_TRANSACTION.MERCHANT_REQUEST_ID;
                            CheckoutRequestID = creditEntryModel.CREDIT_MPESA_TRANSACTION.CHECKOUT_REQUEST_ID;

                            if (!BookingTransactionMasterManager.IsMPesaValidResponse(MPesaAmount, MerchantRequestID, CheckoutRequestID, out bookingResponseModel))
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, bookingResponseModel);
                            }
                        }
                    }
                   }
                    int result = CreditCustomerManager.CreditEntryAmount(creditEntryModel);
                    if (result > 0)
                    {
                        responseModel.Status = "Success";
                        responseModel.Message = "Request sent successfully.";
                        responseModel.Description = "Request sent successfully.";
                        return Request.CreateResponse(HttpStatusCode.Created, responseModel);
                    }
                    else
                    {
                        responseModel.Status = "Failed";
                        responseModel.Message = "Request not sent.";
                        responseModel.Description = "Something went wrong. Please try again.";
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
        #endregion
    }
}
