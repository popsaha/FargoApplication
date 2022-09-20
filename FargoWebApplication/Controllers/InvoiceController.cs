using Fargo_Application.App_Start;
using Fargo_DataAccessLayers;
using Fargo_Models;
using FargoWebApplication.Filter;
using FargoWebApplication.Manager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FargoWebApplication.Controllers
{
    [UserAuthorization] 
    public class InvoiceController : Controller
    {
        // GET: Invoice
        public ActionResult Settlement()
        {
            InvoiceModel invoiceModel = new InvoiceModel();
            try
            {
                var SessionInformation = (LoginModel)Session["SessionInformation"];
                string FromDate = System.DateTime.Now.AddDays(-7).ToString("dd-MM-yyyy");
                string ToDate = System.DateTime.Now.ToString("dd-MM-yyyy");

                invoiceModel.USER_ID = SessionInformation.USER_ID;
                invoiceModel.FROM_DATE = FromDate;
                invoiceModel.TO_DATE = ToDate;

                List<InvoiceModel> LstCashierReprintRequest = InvoiceManager.LstCashierReprintRequest(invoiceModel);
                ViewBag.FromDate = FromDate;
                ViewBag.ToDate = ToDate;
                ViewBag.USER_ID = SessionInformation.USER_ID;
                ViewBag.LstCashierReprintRequest = LstCashierReprintRequest;
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return View(invoiceModel);
        }

        [HttpPost]
        public ActionResult Settlement(InvoiceModel invoiceModel)
        {
            try
            {
                var SessionInformation = (LoginModel)Session["SessionInformation"];
                string FromDate = System.DateTime.Now.AddDays(-7).ToString("dd-MM-yyyy");
                string ToDate = System.DateTime.Now.ToString("dd-MM-yyyy");

                invoiceModel.USER_ID = SessionInformation.USER_ID;
                invoiceModel.FROM_DATE = invoiceModel.FROM_DATE;
                invoiceModel.TO_DATE = invoiceModel.TO_DATE;

                List<InvoiceModel> LstCashierReprintRequest = InvoiceManager.LstCashierReprintRequest(invoiceModel);
                ViewBag.FromDate = FromDate;
                ViewBag.ToDate = ToDate;
                ViewBag.USER_ID = SessionInformation.USER_ID;
                ViewBag.LstCashierReprintRequest = LstCashierReprintRequest;
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return View(invoiceModel);
        }

      
        [HttpPost]
        public JsonResult InvoicePrintSettlement(InvoiceModel invoiceModel)
        {
            int result = 0;
            try
            {
                var SessionInformation = (LoginModel)Session["SessionInformation"];
                invoiceModel.USER_ID = SessionInformation.USER_ID;
                result = InvoiceManager.InvoicePrintSettlement(invoiceModel);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}