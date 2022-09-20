using Fargo_Application.App_Start;
using Fargo_Models;
using FargoWebApplication.Filter;
using FargoWebApplication.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace FargoWebApplication.FargoAPI
{
    [BasicAuthentication]
    public class DayIntransactionAPIController : ApiController
    {
  
        [HttpPost]
        [Route("api/DayIntransactionAPI/DayIntransaction")]

        public HttpResponseMessage DayIntransaction([FromBody] DayIntransactionModel dayintransactionmodrl)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    string Message = DayInTransactionManager.DAY_IN_AMOUNT_Transaction(dayintransactionmodrl);
                    if (!string.IsNullOrEmpty(Message))
                    {
                        responseModel.Status = "Success";
                        responseModel.Message = Message.ToString();
                        responseModel.Description = Message.ToString();
                        return Request.CreateResponse(HttpStatusCode.OK, responseModel);
                    }
                    else
                    {
                        responseModel.Status = "Failed";
                        responseModel.Message = "Internal server error.";
                        responseModel.Description = "Internal server error.";
                        
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


        [HttpPost]
        [Route("api/DayIntransactionAPI/DayEndtransaction")]

        public HttpResponseMessage DayEndtransaction([FromBody] DayIntransactionModel dayintransactionmodrl)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    int result = DayInTransactionManager.DAY_END_AMOUNT_Transaction(dayintransactionmodrl);
                    if (result > 0)
                    {
                        responseModel.Status = "Success";
                        responseModel.Message = "Request sent to Manager.";
                        responseModel.Description = "Request sent to Manager.";
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
    }
}
