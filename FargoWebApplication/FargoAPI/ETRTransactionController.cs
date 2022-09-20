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

namespace FargoWebApplication.FargoAPI
{
    [BasicAuthentication]
    public class ETRTransactionController : ApiController
    {
        [HttpGet]
        [Route("api/ETRTransaction/ETRFileLocation")]
        public IHttpActionResult ETRFileLocation(string TRANSACTION_ID)
        {
            ETRFileLocationModel ETRFileLocation = new ETRFileLocationModel();
            try
            {
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    ETRFileLocation = ETRTransactionManager.ETRFileLocation(TRANSACTION_ID);
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


        [HttpGet]
        [Route("api/ETRTransaction/LstTransactionId")]
        public IHttpActionResult LstTransactionId(string PAGE_NUMBER)
        {
            ETRInvoiceResponseModel LstTransactionResponse = new ETRInvoiceResponseModel();
            try
            {
                bool IsNext = false;
                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    LstTransactionResponse.Data = ETRTransactionManager.LstTransactionId(PAGE_NUMBER, out IsNext);
                    if (LstTransactionResponse.Data.Count == 0)
                    {
                        LstTransactionResponse.Status = "Success";
                        LstTransactionResponse.Message = "No record found.";
                        LstTransactionResponse.Description = "No record found.";
                        LstTransactionResponse.IsNext = false;
                        return Ok(LstTransactionResponse);
                    }
                    else
                    {
                        LstTransactionResponse.Status = "Success";
                        LstTransactionResponse.Message = "Record's found.";
                        LstTransactionResponse.Description = LstTransactionResponse.Data.Count + " record's found.";
                        LstTransactionResponse.IsNext = IsNext;
                        return Ok(LstTransactionResponse);
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
    }
}
