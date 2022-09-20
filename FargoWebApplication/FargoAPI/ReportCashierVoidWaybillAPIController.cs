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
    public class ReportCashierVoidWaybillAPIController : ApiController
    {

        [HttpGet]
        [Route("api/ReportCashierVoidWaybillAPI/ReportCashiervoidwaybill")]
        public IHttpActionResult ReportCashiervoidwaybill(string FROM_DATE, long CASHIER_ID,string TO_DATE,string PAGE_NUMBER)
        {
            List<ReportCashierVoidWaybillModel> LstReportCashierVoidWaybillModel = new List<ReportCashierVoidWaybillModel>();
            try
            {
                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    LstReportCashierVoidWaybillModel = ReportCashierVoidWaybillManager.ReportCashierVoidWaybills(FROM_DATE, CASHIER_ID, TO_DATE, PAGE_NUMBER);
                    if (LstReportCashierVoidWaybillModel.Count == 0)
                    {
                        responseModel.Status = "Success";
                        responseModel.Message = "No record found.";
                        responseModel.Description = "No record found.";
                        return Ok(LstReportCashierVoidWaybillModel);
                    }
                    else
                    {
                        responseModel.Status = "Success";
                        responseModel.Message = " record found.";
                        responseModel.Description = LstReportCashierVoidWaybillModel.Count + " record's found.";

                        return Ok(LstReportCashierVoidWaybillModel);
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
