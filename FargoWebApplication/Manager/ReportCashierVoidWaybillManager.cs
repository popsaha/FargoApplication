using Fargo_Application.App_Start;
using Fargo_DataAccessLayers;
using Fargo_Models;
using FargoWebApplication.Filter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FargoWebApplication.Manager
{
    public class ReportCashierVoidWaybillManager
    {

        public static List<ReportCashierVoidWaybillModel> ReportCashierVoidWaybills(string FROM_DATE, long CASHIER_ID, string TO_DATE,string PAGE_NUMBER)
        {
            DataTable dt = new DataTable();
           
            List<ReportCashierVoidWaybillModel> lstreportCashierVoidWaybillMod = new List<ReportCashierVoidWaybillModel>();
          

            SqlParameter sp1 = new SqlParameter("@CASHIER_ID", CASHIER_ID);
            SqlParameter sp2 = new SqlParameter("@FROM_DATE", ConvertDateFormat.ConvertMMDDYYYY(FROM_DATE));
            SqlParameter sp3 = new SqlParameter("@TO_DATE", ConvertDateFormat.ConvertMMDDYYYY(TO_DATE));
            SqlParameter sp4 = new SqlParameter("@PAGE_NUMBER", PAGE_NUMBER);
            
            try
            {
                dt = clsDataAccess.ExecuteDataTable(CommandType.StoredProcedure, "spReportCashierVoidWaybill", sp1, sp2, sp3,sp4);
                if (dt.Rows.Count>0 && dt.Rows != null)
                {
                    for(int i =0;i<dt.Rows.Count;i++)
                    {
                        ReportCashierVoidWaybillModel reportCashierVoidWaybillModel = new ReportCashierVoidWaybillModel();
                        reportCashierVoidWaybillModel.PAGE_NUMBER = Convert.ToInt64(dt.Rows[i]["PAGE_NUMBER"].ToString());
                        reportCashierVoidWaybillModel.CREATED_ON = Convert.ToDateTime(dt.Rows[i]["CREATED_ON"]);
                        reportCashierVoidWaybillModel.CASHIER_ID = Convert.ToInt64(dt.Rows[i]["CASHIER_ID"].ToString());
                        reportCashierVoidWaybillModel.WAYBILL_NUMBER = dt.Rows[i]["WAYBILL_NUMBER"].ToString();
                        reportCashierVoidWaybillModel.VOID_TRACKING_TRANSACTION_ID = Convert.ToInt64(dt.Rows[i]["VOID_TRACKING_TRANSACTION_ID"]);
                        reportCashierVoidWaybillModel.STORE_ID = Convert.ToInt64(dt.Rows[i]["STORE_ID"]);
                        reportCashierVoidWaybillModel.STORE_NAME = dt.Rows[i]["STORE_NAME"].ToString();
                        reportCashierVoidWaybillModel.CANCELLATION_REASON = dt.Rows[i]["CANCELLATION_REASON"].ToString();
                        reportCashierVoidWaybillModel.CASHIER_NAME = dt.Rows[i]["CASHIER_NAME"].ToString();
                        reportCashierVoidWaybillModel.STATUS = dt.Rows[i]["STATUS"].ToString();
                        reportCashierVoidWaybillModel.REQUESTED_DATE = dt.Rows[i]["REQUESTED_DATE"].ToString();
                        reportCashierVoidWaybillModel.RESPONDED_DATE = dt.Rows[i]["RESPONDED_DATE"].ToString();
                        reportCashierVoidWaybillModel.IS_NEXT = Convert.ToInt64(dt.Rows[i]["IS_NEXT"]);
                        lstreportCashierVoidWaybillMod.Add(reportCashierVoidWaybillModel);
                    }
                    

                   
                };
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return lstreportCashierVoidWaybillMod;
        }
    }
}