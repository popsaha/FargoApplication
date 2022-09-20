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
    public class DayInEndTransactionManager
    {

        public static DayInEndTransactionModel DayInEndTransaction(long CASHIER_ID, string DATE)
        {
            DataTable dt = new DataTable();
            DayInEndTransactionModel dayInEndTransactionModel = new DayInEndTransactionModel();
            SqlParameter sp1 = new SqlParameter("@CASHIER_ID", CASHIER_ID);
            SqlParameter sp3 = new SqlParameter("@DATE", ConvertDateFormat.ConvertMMDDYYYY(DATE));
            SqlParameter sp4 = new SqlParameter("@FLAG", '3');
            try
            {
                dt = clsDataAccess.ExecuteDataTable(CommandType.StoredProcedure, "spDayInEndTransaction", sp1,  sp3, sp4);
                if (dt.Rows.Count > 0 && dt.Rows != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dayInEndTransactionModel.CREATED_ON = dt.Rows[i]["CREATED_ON"].ToString();
                        dayInEndTransactionModel.TOTAL_DAY_IN_AMOUNT = Convert.ToDouble(dt.Rows[i]["TOTAL_DAY_IN_AMOUNT"]);
                        dayInEndTransactionModel.TOTAL_CANCEL_AMOUNT = Convert.ToDouble(dt.Rows[i]["TOTAL_CANCEL_AMOUNT"]);
                        dayInEndTransactionModel.TOTAL_MPESA_AMOUNT = Convert.ToDouble(dt.Rows[i]["TOTAL_MPESA_AMOUNT"]);
                        dayInEndTransactionModel.TOTAL_CASH_AMOUNT = Convert.ToDouble(dt.Rows[i]["TOTAL_CASH_AMOUNT"]);
                        dayInEndTransactionModel.EXPECTED_CASH_AMOUNT = (Convert.ToDouble(dt.Rows[i]["TOTAL_DAY_IN_AMOUNT"]) + Convert.ToDouble(dt.Rows[i]["TOTAL_CASH_AMOUNT"])) - Convert.ToDouble(dt.Rows[i]["TOTAL_CANCEL_AMOUNT"]);
                        dayInEndTransactionModel.DAY_IN_END_TRANSACTION_ID = Convert.ToInt64(dt.Rows[i]["DAY_IN_END_TRANSACTION_ID"]);
                    }
                };
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return dayInEndTransactionModel;
        }

    }
}