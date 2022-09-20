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
    public class LotManager
    {
        public static DataSet LstLotInfo(out List<StoreModel> LstStores, out List<LotModel> LstLotes)
        {
            DataSet dataSet = new DataSet();
            LstStores = null; LstLotes = null;
            try
            {
                SqlParameter sp1 = new SqlParameter("@FLAG", "1");
                dataSet = clsDataAccess.ExecuteDataset(CommandType.StoredProcedure, "spStoreSlot", sp1);
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count > 0)
                    {
                        if (dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count > 0)
                        {
                            LstStores = (from DataRow dataRow in dataSet.Tables[0].Rows
                                         select new StoreModel()
                                         {
                                             STORE_ID = Convert.ToInt64(dataRow["STORE_ID"]),
                                             STORE_NAME = dataRow["STORE_NAME"].ToString()
                                         }).ToList();
                        }
                    }
                    if (dataSet.Tables[1] != null && dataSet.Tables[1].Rows.Count > 0)
                    {
                        LstLotes = (from DataRow dataRow in dataSet.Tables[1].Rows
                                     select new LotModel()
                                     {
                                         LOT_ID = Convert.ToInt64(dataRow["LOT_ID"]),
                                         LST_STORE=(from DataRow _dataRow in dataSet.Tables[0].Rows
                                                     select new StoreModel()
                                                     {
                                                         STORE_ID = Convert.ToInt64(_dataRow["STORE_ID"]),
                                                         STORE_NAME = _dataRow["STORE_NAME"].ToString()
                                                     }).ToList(),
                                         STORE_ID = Convert.ToInt64(dataRow["STORE_ID"]),
                                         LOT_NAME = dataRow["LOT_NAME"].ToString(),
                                         DESCRIPTION = dataRow["DESCRIPTION"].ToString(),
                                         STORE_NAME = dataRow["STORE_NAME"].ToString(),
                                         FROM_TRACKING_NUMBER = dataRow["FROM_TRACKING_NUMBER"].ToString(),
                                         TO_TRACKING_NUMBER = dataRow["TO_TRACKING_NUMBER"].ToString()
                                     }).ToList();
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return dataSet;
        }

        public static int SubmitLotInfo(List<LotModel> LstLotModel, long USER_ID)
        {
            int result = 0; 
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("LOT_NAME");
                dataTable.Columns.Add("DESCRIPTION");
                dataTable.Columns.Add("FROM_TRACKING_NUMBER");
                dataTable.Columns.Add("TO_TRACKING_NUMBER");
                dataTable.Columns.Add("STORE_ID");
                dataTable.Columns.Add("DATA_SOURCE");
                dataTable.Columns.Add("IS_ACTIVE");
                dataTable.Columns.Add("CREATED_BY");
                dataTable.Columns.Add("CREATED_ON");
                dataTable.AcceptChanges();

                foreach (LotModel lotModel in LstLotModel)
                {
                    string LOT_NAME = lotModel.LOT_NAME;
                    string DESCRIPTION = null;
                    string FROM_TRACKING_NUMBER = lotModel.FROM_TRACKING_NUMBER;
                    string TO_TRACKING_NUMBER = lotModel.TO_TRACKING_NUMBER;
                    long STORE_ID = lotModel.STORE_ID;
                    string DATA_SOURCE = "W";
                    bool IS_ACTIVE = true;                    
                    string CREATED_BY = USER_ID.ToString();
                    string CREATED_ON = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");

                    if (STORE_ID > 0 && !string.IsNullOrEmpty(LOT_NAME) && Convert.ToInt64(FROM_TRACKING_NUMBER) > 0 && Convert.ToInt64(TO_TRACKING_NUMBER) > 0)
                    {
                        dataTable.Rows.Add(LOT_NAME, DESCRIPTION, FROM_TRACKING_NUMBER, TO_TRACKING_NUMBER, STORE_ID, DATA_SOURCE, IS_ACTIVE, CREATED_BY, CREATED_ON);
                        dataTable.AcceptChanges();
                    }                   
                }

                SqlParameter sp1 = new SqlParameter("@tblStoreLotMaster", dataTable);
                SqlParameter sp2 = new SqlParameter("@CREATED_BY", USER_ID);
                SqlParameter sp3 = new SqlParameter("@FLAG", "6");
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spStoreSlot", sp1, sp2, sp3);
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        public static int UpdateLotInfo(List<LotModel> LstLotModel, long USER_ID)
        {
            int result = 0;
            try
            {

                SqlParameter sp1 = new SqlParameter("@LOT_ID", LstLotModel[0].LOT_ID);
                SqlParameter sp2 = new SqlParameter("@STORE_ID", LstLotModel[0].STORE_ID);
                SqlParameter sp3 = new SqlParameter("@LOT_NAME", LstLotModel[0].LOT_NAME);
                SqlParameter sp4 = new SqlParameter("@FROM_TRACKING_NUMBER", LstLotModel[0].FROM_TRACKING_NUMBER);
                SqlParameter sp5 = new SqlParameter("@TO_TRACKING_NUMBER", LstLotModel[0].TO_TRACKING_NUMBER);
                SqlParameter sp6 = new SqlParameter("@MODIFIED_BY", USER_ID);
                SqlParameter sp7 = new SqlParameter("@FLAG", "4");

                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spStoreSlot", sp1, sp2, sp3, sp4, sp5, sp6, sp7);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        public static int DeleteLotInfo(long LOT_ID, long USER_ID)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@LOT_ID", LOT_ID);
                SqlParameter sp2 = new SqlParameter("@DELETED_BY", USER_ID);
                SqlParameter sp3 = new SqlParameter("@FLAG", "5");

                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spStoreSlot", sp1, sp2, sp3);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }
    }
}