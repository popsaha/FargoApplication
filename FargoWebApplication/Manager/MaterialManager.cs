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
    public class MaterialManager
    {
        public static List<MaterialModel> LstMaterials()
        {
            List<MaterialModel> LstMaterials = new List<MaterialModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@FLAG", "1");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spMaterials", sp1);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        MaterialModel materialModel = new MaterialModel();

                        materialModel.MATERIAL_ID = Convert.ToInt64(sqlDataReader["MATERIAL_ID"].ToString());
                        materialModel.MATERIAL_CODE = sqlDataReader["MATERIAL_CODE"].ToString();

                        LstMaterials.Add(materialModel);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstMaterials;
        }

        
    }
}