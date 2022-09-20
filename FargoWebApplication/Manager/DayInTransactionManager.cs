using Fargo_Application.App_Start;
using Fargo_DataAccessLayers;
using Fargo_Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FargoWebApplication.Manager
{
    public class DayInTransactionManager
    {
        public static string DAY_IN_AMOUNT_Transaction(DayIntransactionModel dayintransactionmodel)
        {
            string Message = string.Empty;
            try
            {
                SqlParameter sp1 = new SqlParameter("@STORE_ID", dayintransactionmodel.STORE_ID);
                SqlParameter sp2 = new SqlParameter("@CASHIER_ID", dayintransactionmodel.CASHIER_ID);
                SqlParameter sp3 = new SqlParameter("@TOTAL_DAY_IN_AMOUNT", dayintransactionmodel.TOTAL_DAY_IN_AMOUNT);
                SqlParameter sp4 = new SqlParameter("@FLAG", "1");

                DataTable dataTable = clsDataAccess.ExecuteDataTable(CommandType.StoredProcedure, "spDayInEndTransaction", sp1, sp2, sp3, sp4);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    Message = dataTable.Rows[0][0].ToString();
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return Message;
        }



        public static int DAY_END_AMOUNT_Transaction(DayIntransactionModel dayintransactionmodel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@STORE_ID", dayintransactionmodel.STORE_ID);
                SqlParameter sp2 = new SqlParameter("@CASHIER_ID", dayintransactionmodel.CASHIER_ID);

                SqlParameter sp3 = new SqlParameter("@TOTAL_DAY_END_AMOUNT", dayintransactionmodel.TOTAL_DAY_END_AMOUNT);


                SqlParameter sp4 = new SqlParameter("@FLAG", "2");


                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spDayInEndTransaction", sp1, sp2, sp3, sp4);

            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }
    }
}