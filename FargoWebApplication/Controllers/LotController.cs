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
    public class LotController : Controller
    {
        // GET: Lot
        public ActionResult Index()
        {
            List<StoreModel> LstStores = null; List<LotModel> LstLotes = null;

            DataSet dataSet = LotManager.LstLotInfo(out LstStores, out LstLotes);

            ViewBag.LstStores = LstStores;
            ViewBag.LstLotes = LstLotes;

            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            return View();
        }


        public ActionResult SubmitLotInfo(List<LotModel> LstLotModel)
        {
            try
            {
                var SessionInformation = (LoginModel)Session["SessionInformation"];

                int result = LotManager.SubmitLotInfo(LstLotModel, SessionInformation.USER_ID);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                throw exception;
            }
        }

        public ActionResult UpdateLotInfo(List<LotModel> LstLotModel)
        {
            try
            {
                var SessionInformation = (LoginModel)Session["SessionInformation"];

                int result = LotManager.UpdateLotInfo(LstLotModel, SessionInformation.USER_ID);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                throw exception;
            }
        }


        public ActionResult DeleteLotInfo(long LOT_ID)
        {
            try
            {
                var SessionInformation = (LoginModel)Session["SessionInformation"];

                int result = LotManager.DeleteLotInfo(LOT_ID, SessionInformation.USER_ID);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                throw exception;
            }
        }
    }
}