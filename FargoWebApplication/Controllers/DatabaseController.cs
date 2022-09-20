using Fargo_Application.App_Start;
using FargoWebApplication.Filter;
using FargoWebApplication.Manager;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FargoWebApplication.Controllers
{
    [UserAuthorization]
    public class DatabaseController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = null;
            return View();
        }

        [HttpPost]
        public ActionResult Index(string BackupPath)
        {
            int result = 0;
            try
            {
                string DatabaseName = "DbFargoApplication" + DateTime.Now.ToString("ddMMyyymmss") + ".Bak";
                BackupPath = Path.Combine(Server.MapPath("~/Database/Backup/"), DatabaseName);
                string newPath = @"G:\TBB\Fargo Application\Fargo application with new layout integration\Fargo web application\FargoWebApplication\FargoWebApplication\NewDatabase\NewBackup";
                result = DatabaseManager.Backup(newPath);
                ViewBag.Message = result;
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
            }
            return View();
        }
    

    
        public ActionResult Backup()
        {
            return View();
        }
    
        [HttpPost]
        public ActionResult Backup(string BackupPath)
        {
            int result = 0;
            try
            {
                BackupPath = Server.MapPath("~/Database/Backup/");
                result= DatabaseManager.Backup(BackupPath);
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
            }
            return View();
        }
    }
}