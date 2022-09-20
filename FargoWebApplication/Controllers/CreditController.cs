using Fargo_Application.App_Start;
using Fargo_DataAccessLayers;
using Fargo_Models;
using FargoWebApplication.Filter;
using FargoWebApplication.Manager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FargoWebApplication.Controllers
{
    [UserAuthorization] 
    public class CreditController : Controller
    {
        // GET: Credit
        public ActionResult Index()
        {
            CreditEntryModel creditEntryModel = new CreditEntryModel();
            var SessionInformation = (LoginModel)Session["SessionInformation"];
            string FromDate = System.DateTime.Now.AddDays(-7).ToString("dd-MM-yyyy");
            string ToDate = System.DateTime.Now.ToString("dd-MM-yyyy");

            creditEntryModel.USER_ID = SessionInformation.USER_ID;
            creditEntryModel.FROM_DATE = FromDate;
            creditEntryModel.TO_DATE = ToDate;

            List<CreditEntryModel> LstCreditEntry = CreditCustomerManager.LstCreditEntry(creditEntryModel);
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;
            ViewBag.USER_ID = SessionInformation.USER_ID;
            ViewBag.LstCreditEntry = LstCreditEntry;

            return View(creditEntryModel);
        }

        [HttpPost]
        public JsonResult ActionCreditEntry(CreditEntryModel creditEntryModel)
        {
            int result = CreditCustomerManager.ActionCreditEntry(creditEntryModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Index(CreditEntryModel creditEntryModel)
        {
            var SessionInformation = (LoginModel)Session["SessionInformation"];
            string FromDate = creditEntryModel.FROM_DATE;
            string ToDate = creditEntryModel.TO_DATE;

            creditEntryModel.USER_ID = SessionInformation.USER_ID;
            creditEntryModel.FROM_DATE = FromDate;
            creditEntryModel.TO_DATE = ToDate;

            List<CreditEntryModel> LstCreditEntry = CreditCustomerManager.LstCreditEntry(creditEntryModel);
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;
            ViewBag.USER_ID = SessionInformation.USER_ID;
            ViewBag.LstCreditEntry = LstCreditEntry;

            return View(creditEntryModel);
        }

        public ActionResult Report()
        {
            CreditEntryModel creditEntryModel = new CreditEntryModel();
            var SessionInformation = (LoginModel)Session["SessionInformation"];
            string FromDate = System.DateTime.Now.AddDays(-7).ToString("dd-MM-yyyy");
            string ToDate = System.DateTime.Now.ToString("dd-MM-yyyy");

            creditEntryModel.USER_ID = SessionInformation.USER_ID;
            creditEntryModel.FROM_DATE = FromDate;
            creditEntryModel.TO_DATE = ToDate;

            List<CreditEntryModel> LstCreditEntry = CreditCustomerManager.CreditEntryReport(creditEntryModel);
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;
            ViewBag.USER_ID = SessionInformation.USER_ID;
            ViewBag.LstCreditEntry = LstCreditEntry;

            return View(creditEntryModel);
        }

        [HttpPost]
        public ActionResult Report(CreditEntryModel creditEntryModel)
        {
            var SessionInformation = (LoginModel)Session["SessionInformation"];
            string FromDate = creditEntryModel.FROM_DATE;
            string ToDate = creditEntryModel.TO_DATE;

            creditEntryModel.USER_ID = SessionInformation.USER_ID;
            creditEntryModel.FROM_DATE = FromDate;
            creditEntryModel.TO_DATE = ToDate;

            List<CreditEntryModel> LstCreditEntry = CreditCustomerManager.CreditEntryReport(creditEntryModel);
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;
            ViewBag.USER_ID = SessionInformation.USER_ID;
            ViewBag.LstCreditEntry = LstCreditEntry;

            return View(creditEntryModel);
        }
    }
}