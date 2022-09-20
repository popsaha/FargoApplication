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
using System.Configuration;

namespace FargoWebApplication.FargoAPI
{
    [BasicAuthentication]
    public class InvoiceAPIController : ApiController
    {
        DbFargoApplicationEntities _db = new DbFargoApplicationEntities();

        [HttpPost]
        [Route("api/InvoiceAPI/ReprintInvoiceRequest")]
        public HttpResponseMessage ReprintInvoiceRequest([FromBody] InvoiceModel invoiceModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    int result = InvoiceManager.ReprintInvoiceRequest(invoiceModel);
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

        [HttpGet]
        [Route("api/InvoiceAPI/LstReprintInvoiceResponse")]
        public IHttpActionResult LstReprintInvoiceResponse(string USER_ID, string PAGE_NUMBER)
        {
            InvoiceResponseModel invoiceResponseModel = new InvoiceResponseModel();
            try
            {
                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    bool IS_NEXT = false;
                    List<InvoiceModel> LstInvoiceModel = InvoiceManager.LstReprintInvoiceResponse(USER_ID, PAGE_NUMBER, out IS_NEXT);
                    if (LstInvoiceModel.Count == 0)
                    {
                        responseModel.Status = "Success";
                        responseModel.Message = "No record found.";
                        responseModel.Description = "No record found.";
                        return Ok(responseModel);
                    }
                    else
                    {
                        invoiceResponseModel.InvoiceInfo = LstInvoiceModel;
                        invoiceResponseModel.IsNext = IS_NEXT;
                        invoiceResponseModel.Status = "Success";
                        invoiceResponseModel.Message = "Record found.";
                        invoiceResponseModel.Description = LstInvoiceModel.Count + " record's found.";

                        return Ok(invoiceResponseModel);
                    }
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

        [HttpGet]
        [Route("api/InvoiceAPI/ReprintInvoice")]
        public IHttpActionResult ReprintInvoice(string USER_ID, string REPRINT_INVOICE_RECEIPT_ID)
        {
            ETRFileLocationModel ETRFileLocation = new ETRFileLocationModel();
            try
            {
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    ETRFileLocation = InvoiceManager.ReprintInvoice(USER_ID, REPRINT_INVOICE_RECEIPT_ID);
                    return Ok(ETRFileLocation);
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
    }
}
