using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Fargo_DataAccessLayers;
using Fargo_Models;
using Fargo_Application.App_Start;
using System.IO;


namespace FargoWebApplication.Manager
{
    public class DatabaseManager
    {
        public static int Backup(string BackupPath)
        {
            int result = 0;
            try
            {
                //if (!System.IO.Directory.Exists(BackupPath))
                //{
                //    System.IO.Directory.CreateDirectory(BackupPath);
                //}
                System.IO.Directory.CreateDirectory(BackupPath);
                string SourceDatabaseName = "DbFargoApplication";
                string Query = "BACKUP DATABASE " + SourceDatabaseName + " TO DISK='" + BackupPath + "\\" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".Bak'";

                result = clsDataAccess.ExecuteNonQuery(CommandType.Text, Query);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }
    }
}