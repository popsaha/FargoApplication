using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Fargo_DataAccessLayers;
using Fargo_Models;
using Fargo_Application.App_Start;

namespace FargoWebApplication.Manager
{
    public class TaxManager
    {
        public static List<TaxModel> LstTaxes()
        {
            List<TaxModel> LstTaxes = new List<TaxModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@FLAG", "1");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spTax", sp1);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        TaxModel taxModel = new TaxModel();

                        taxModel.TAX_ID = Convert.ToInt64(sqlDataReader["TAX_ID"].ToString());
                        taxModel.TAX_RATE = Convert.ToDouble(sqlDataReader["TAX_RATE"].ToString());
                        taxModel.TAX_NAME = sqlDataReader["TAX_NAME"].ToString();
                        taxModel.DESCRIPTION = sqlDataReader["DESCRIPTION"].ToString();
                        taxModel.TAX_GROUP_NAME = sqlDataReader["TAX_GROUP_NAME"].ToString();

                        LstTaxes.Add(taxModel);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstTaxes;
        }

        public static TaxModel DefaultTax()
        {

            TaxModel taxModel = new TaxModel();
            try
            {
                SqlParameter sp1 = new SqlParameter("@FLAG", "2");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spTax", sp1);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        taxModel.TAX_ID = Convert.ToInt64(sqlDataReader["TAX_ID"].ToString());
                        taxModel.TAX_RATE = Convert.ToDouble(sqlDataReader["TAX_RATE"].ToString());
                        taxModel.TAX_NAME = sqlDataReader["TAX_NAME"].ToString();
                        taxModel.DESCRIPTION = sqlDataReader["DESCRIPTION"].ToString();
                        taxModel.TAX_GROUP_NAME = sqlDataReader["TAX_GROUP_NAME"].ToString();
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return taxModel;
        }
    }
}