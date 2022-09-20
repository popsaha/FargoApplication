using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using context = System.Web.HttpContext;
using System.Data.SqlClient;
using Fargo_DataAccessLayers;
using System.Data;

namespace Fargo_Application.App_Start
{
    public static class ExceptionLogging
    {
        private static String ErrorLineNo, ErrorMessage, ExceptionType, ExceptionURL, hostIp, ErrorLocation, HostAdd;

        public static string SendErrorToText(Exception exception)
        {
            string ErrorMessage = "";
            var Line = Environment.NewLine + Environment.NewLine;

            ErrorLineNo = exception.StackTrace.Substring(exception.StackTrace.Length - 8, 8);
            ErrorMessage = exception.GetType().Name.ToString();
            ExceptionType = exception.GetType().ToString();
            ExceptionURL = context.Current.Request.Url.ToString();
            ErrorLocation = exception.Message.ToString();
            try
            {
                string FilePath = context.Current.Server.MapPath("~/LogFiles/");  //Text File Path
                if (!Directory.Exists(FilePath))
                {
                    Directory.CreateDirectory(FilePath);
                }
                FilePath = FilePath + DateTime.Today.ToString("dd_MMM_yyyy") + ".txt";   //Text File Name
                if (!File.Exists(FilePath))
                {
                    File.Create(FilePath).Dispose();
                }
                using (StreamWriter streamWriter = File.AppendText(FilePath))
                {
                    string ErrorBody = "Log Written Date:" + " " + DateTime.Now.ToString() + Line + "Error Line No :" + " " + ErrorLineNo + Line + "Error Message:" + " " + ErrorMessage + Line + "Exception Type:" + " " + ExceptionType + Line + "Error Location :" + " " + ErrorLocation + Line + " Error Page Url:" + " " + ExceptionURL + Line + "User Host IP:" + " " + hostIp + Line;
                    streamWriter.Write("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    streamWriter.Write("-------------------------------------------------------------------------------------");
                    streamWriter.Write(Line);
                    streamWriter.Write(ErrorBody);
                    streamWriter.Write("--------------------------------*End*------------------------------------------");
                    streamWriter.Write(Line);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                SendErrorToDatabase(exception);
            }
            catch (Exception _exception)
            {
                ErrorMessage = _exception.ToString();
            }
            return ErrorMessage;
        }


        public static string SendErrorToDatabase(Exception exception)
        {
            string ErrorMessage = string.Empty; int result = 0;
            try
            {
                string EXCEPTION_LINE_NO = exception.StackTrace.Substring(exception.StackTrace.Length - 3, 3);
                string EXCEPTION_MESSAGE = exception.GetType().Name.ToString();
                string EXCEPTION_TYPE = exception.GetType().ToString();
                string EXCEPTION_URL = context.Current.Request.Url.ToString();
                string EXCEPTION_LOCATION = exception.Message.ToString();
                string USER_HOST_IP = string.Empty;

                SqlParameter sp1 = new SqlParameter("@EXCEPTION_LINE_NO", EXCEPTION_LINE_NO);
                SqlParameter sp2 = new SqlParameter("@EXCEPTION_MESSAGE", EXCEPTION_MESSAGE);
                SqlParameter sp3 = new SqlParameter("@EXCEPTION_TYPE", EXCEPTION_TYPE);
                SqlParameter sp4 = new SqlParameter("@EXCEPTION_URL", EXCEPTION_URL);
                SqlParameter sp5 = new SqlParameter("@EXCEPTION_LOCATION", EXCEPTION_LOCATION);
                SqlParameter sp6 = new SqlParameter("@USER_HOST_IP", USER_HOST_IP);
                SqlParameter sp7 = new SqlParameter("@FLAG", "1");

                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spExceptionLog", sp1, sp2, sp3, sp4, sp5, sp6, sp7);
            }
            catch (Exception _exception)
            {
                ErrorMessage = _exception.ToString();
            }
            return ErrorMessage;
        }
    }
}