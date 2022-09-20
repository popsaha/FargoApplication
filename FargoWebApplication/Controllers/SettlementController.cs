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
    public class SettlementController : Controller
    {
        // GET: Settlement
        public ActionResult Index()
        {
            var SessionInformation = (LoginModel)Session["SessionInformation"];
            List<SettlementModel> LstSettlement = null;
            LstSettlement = SettlementManager.SettlementInfo(SessionInformation.USER_ID);

            if (LstSettlement != null && LstSettlement.Count > 0)
                ViewBag.IsData = "1";
            else
                ViewBag.IsData = "0";

            ViewBag.LstSettlement = LstSettlement;
            ViewBag.USER_ID = SessionInformation.USER_ID;
            return View();
        }


        [HttpPost]
        public ActionResult Index(SettlementModel settlementModel)
        {
            var SessionInformation = (LoginModel)Session["SessionInformation"];
            settlementModel.USER_ID = SessionInformation.USER_ID;
            List<SettlementModel>  LstSettlement = SettlementManager.SettlementInfo(settlementModel);

            if (LstSettlement != null && LstSettlement.Count > 0)
                ViewBag.IsData = "1";
            else
                ViewBag.IsData = "0";

            ViewBag.LstSettlement = LstSettlement;
            return View();
        }

        [HttpPost]
        public JsonResult SettlementInfo(string Id)
        {
            SettlementModel settlementModel = SettlementManager.SettlementInfo(Id);
            return Json(settlementModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Approval(SettlementModel settlementModel)
        {
            int result = SettlementManager.Approval(settlementModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LstDaySettlement(SettlementModel settlementModel)
        {
            CashierSettlementModel cashierSettlementModel = new CashierSettlementModel();
            DataSet dataSet= CashierSettlementManager.LstDaySettlement(settlementModel, out cashierSettlementModel);
            return Json(cashierSettlementModel, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Report()
        {
            var SessionInformation = (LoginModel)Session["SessionInformation"];
            ViewBag.USER_ID = SessionInformation.USER_ID;
            return View();
        }


        [HttpPost]
        public ActionResult Report(SettlementModel settlementModel)
        {
            var SessionInformation = (LoginModel)Session["SessionInformation"];
            settlementModel.USER_ID = SessionInformation.USER_ID;
            List<SettlementModel> LstSettlement = SettlementManager.SettlementInfo(settlementModel);

            if (LstSettlement != null && LstSettlement.Count > 0)
                ViewBag.IsData = "1";
            else
                ViewBag.IsData = "0";

            ViewBag.LstSettlement = LstSettlement;
            return View();
        }


        public ActionResult Void()
        {
            VoidTrackingTransactionModel voidTrackingTransactionModel = new VoidTrackingTransactionModel();
            var SessionInformation = (LoginModel)Session["SessionInformation"];
            string FromDate = System.DateTime.Now.AddDays(-7).ToString("dd-MM-yyyy");
            string ToDate = System.DateTime.Now.ToString("dd-MM-yyyy");

            voidTrackingTransactionModel.USER_ID = SessionInformation.USER_ID;
            voidTrackingTransactionModel.FROM_DATE = FromDate;
            voidTrackingTransactionModel.TO_DATE = ToDate;

            List<VoidTrackingTransactionModel> LstVoidSettlementInfo = SettlementManager.VoidSettlementInfo(voidTrackingTransactionModel);
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;
            ViewBag.USER_ID = SessionInformation.USER_ID;
            ViewBag.LstVoidSettlementInfo = LstVoidSettlementInfo;

            return View(voidTrackingTransactionModel);
        }

        [HttpPost]
        public ActionResult Void(VoidTrackingTransactionModel voidTrackingTransactionModel)
        {
            var SessionInformation = (LoginModel)Session["SessionInformation"];
            string FromDate = voidTrackingTransactionModel.FROM_DATE;
            string ToDate = voidTrackingTransactionModel.TO_DATE;

            voidTrackingTransactionModel.USER_ID = SessionInformation.USER_ID;
            voidTrackingTransactionModel.FROM_DATE = FromDate;
            voidTrackingTransactionModel.TO_DATE = ToDate;

            List<VoidTrackingTransactionModel> LstVoidSettlementInfo = SettlementManager.VoidSettlementInfo(voidTrackingTransactionModel);
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;
            ViewBag.USER_ID = SessionInformation.USER_ID;
            ViewBag.LstVoidSettlementInfo = LstVoidSettlementInfo;

            return View(voidTrackingTransactionModel);
        }

        [HttpPost]
        public JsonResult VoidSettlement(VoidTrackingTransactionModel voidTrackingTransactionModel)
        {
            int result = SettlementManager.VoidSettlement(voidTrackingTransactionModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult VoidReport()
        {
            VoidTrackingTransactionModel voidTrackingTransactionModel = new VoidTrackingTransactionModel();
            var SessionInformation = (LoginModel)Session["SessionInformation"];
            string FromDate = string.Empty;
            string ToDate = string.Empty;

            voidTrackingTransactionModel.USER_ID = SessionInformation.USER_ID;
            voidTrackingTransactionModel.FROM_DATE = FromDate;
            voidTrackingTransactionModel.TO_DATE = ToDate;

            List<VoidTrackingTransactionModel> LstVoidSettlementInfo = SettlementManager.VoidSettlementReport(voidTrackingTransactionModel);
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;
            ViewBag.USER_ID = SessionInformation.USER_ID;
            ViewBag.LstVoidSettlementInfo = LstVoidSettlementInfo;

            return View(voidTrackingTransactionModel);
        }

        [HttpPost]
        public ActionResult VoidReport(VoidTrackingTransactionModel voidTrackingTransactionModel)
        {
            var SessionInformation = (LoginModel)Session["SessionInformation"];
            string FromDate = voidTrackingTransactionModel.FROM_DATE;
            string ToDate = voidTrackingTransactionModel.TO_DATE;

            voidTrackingTransactionModel.USER_ID = SessionInformation.USER_ID;
            voidTrackingTransactionModel.FROM_DATE = FromDate;
            voidTrackingTransactionModel.TO_DATE = ToDate;

            List<VoidTrackingTransactionModel> LstVoidSettlementInfo = SettlementManager.VoidSettlementReport(voidTrackingTransactionModel);
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;
            ViewBag.USER_ID = SessionInformation.USER_ID;
            ViewBag.LstVoidSettlementInfo = LstVoidSettlementInfo;

            return View(voidTrackingTransactionModel);
        }

        public ActionResult Test()
        {
            return View(); 
        }




        public ActionResult Transaction()
        {
                CancelTransactionModel cancelTransactionModel = new CancelTransactionModel();
                var SessionInformation = (LoginModel)Session["SessionInformation"];
                string FromDate = System.DateTime.Now.AddDays(-7).ToString("dd-MM-yyyy");
                string ToDate = System.DateTime.Now.ToString("dd-MM-yyyy");

                cancelTransactionModel.USER_ID = SessionInformation.USER_ID;
                cancelTransactionModel.FROM_DATE = FromDate;
                cancelTransactionModel.TO_DATE = ToDate;

                List<CancelTransactionModel> LstCancelTransactions = TransactionCancelManager.LstCancelTransactions(cancelTransactionModel);
                ViewBag.FromDate = FromDate;
                ViewBag.ToDate = ToDate;
                ViewBag.USER_ID = SessionInformation.USER_ID;
                ViewBag.LstCancelTransactions = LstCancelTransactions;
                return View(cancelTransactionModel);
        }

        [HttpPost]
        public JsonResult CancelTransactionSettlement(CancelTransactionModel cancelTransactionModel)
        {
            var SessionInformation = (LoginModel)Session["SessionInformation"];
            cancelTransactionModel.USER_ID = SessionInformation.USER_ID;
            int result = TransactionCancelManager.CancelTransactionSettlement(cancelTransactionModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Transaction(CancelTransactionModel cancelTransactionModel)
        {
            var SessionInformation = (LoginModel)Session["SessionInformation"];
            string FromDate = cancelTransactionModel.FROM_DATE;
            string ToDate = cancelTransactionModel.TO_DATE;

            cancelTransactionModel.USER_ID = SessionInformation.USER_ID;
            cancelTransactionModel.FROM_DATE = FromDate;
            cancelTransactionModel.TO_DATE = ToDate;

            List<CancelTransactionModel> LstCancelTransactions = TransactionCancelManager.LstCancelTransactions(cancelTransactionModel);
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;
            ViewBag.USER_ID = SessionInformation.USER_ID;
            ViewBag.LstCancelTransactions = LstCancelTransactions;
            return View(cancelTransactionModel);
        }

        public ActionResult CancelReport()
        {
            CancelTransactionModel cancelTransactionModel = new CancelTransactionModel();
            var SessionInformation = (LoginModel)Session["SessionInformation"];
            string FromDate = System.DateTime.Now.AddDays(-7).ToString("dd-MM-yyyy");
            string ToDate = System.DateTime.Now.ToString("dd-MM-yyyy");

            cancelTransactionModel.USER_ID = SessionInformation.USER_ID;
            cancelTransactionModel.FROM_DATE = FromDate;
            cancelTransactionModel.TO_DATE = ToDate;

            List<CancelTransactionModel> LstCancelTransactions = TransactionCancelManager.CancelTransactionReport(cancelTransactionModel);
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;
            ViewBag.USER_ID = SessionInformation.USER_ID;
            ViewBag.LstCancelTransactions = LstCancelTransactions;
            return View(cancelTransactionModel);
        }

        [HttpPost]
        public ActionResult CancelReport(CancelTransactionModel cancelTransactionModel)
        {
            var SessionInformation = (LoginModel)Session["SessionInformation"];
            string FromDate = cancelTransactionModel.FROM_DATE;
            string ToDate = cancelTransactionModel.TO_DATE;

            cancelTransactionModel.USER_ID = SessionInformation.USER_ID;
            cancelTransactionModel.FROM_DATE = FromDate;
            cancelTransactionModel.TO_DATE = ToDate;

            List<CancelTransactionModel> LstCancelTransactions = TransactionCancelManager.CancelTransactionReport(cancelTransactionModel);
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;
            ViewBag.USER_ID = SessionInformation.USER_ID;
            ViewBag.LstCancelTransactions = LstCancelTransactions;
            return View(cancelTransactionModel);
        }

    }
}