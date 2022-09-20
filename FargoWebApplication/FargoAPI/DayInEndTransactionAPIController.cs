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
    public class DayInEndTransactionAPIController : ApiController
    {
        [HttpGet]
        [Route("api/DayInEndTransactionAPI/DayInEndTransaction")]

        public IHttpActionResult DayInEndTransaction(long CASHIER_ID,string DATE)
        {
            DayInEndTransactionModel dayInEndTransactionModel = new DayInEndTransactionModel();
            ResponseModel responseModel = new ResponseModel();
            try
            {
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    dayInEndTransactionModel = DayInEndTransactionManager.DayInEndTransaction(CASHIER_ID, DATE);
                    if (dayInEndTransactionModel.DAY_IN_END_TRANSACTION_ID < 1)
                    {
                        responseModel.Status = "Success";
                        responseModel.Message = "No record found.";
                        responseModel.Description = "No record found.";
                        return Ok(dayInEndTransactionModel);                        
                    }
                    else
                    {
                        responseModel.Status = "Success";
                        responseModel.Message = " record found.";
                        responseModel.Description =   "1 record found.";
                        return Ok(dayInEndTransactionModel);
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
