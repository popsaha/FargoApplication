using Fargo_Application.App_Start;
using Fargo_Models;
using FargoWebApplication.Filter;
using FargoWebApplication.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace FargoWebApplication.FargoAPI
{
    [BasicAuthentication]
    public class CancelTransactionAPIController : ApiController
    {
        [HttpGet]
        [Route("api/CancelTransactionAPI/LstTransactionByTransactionId")]
        public IHttpActionResult LstTransactionByTransactionId(long CASHIER_ID, string TRANSACTION_ID)
        {
            List<TransactionCancelModel> LstTransactionCancelModel = new List<TransactionCancelModel>();
            TransactionCancelResponseModel transactionCancelResponseModel = new TransactionCancelResponseModel();
            try
            {
                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    LstTransactionCancelModel = TransactionCancelManager.LstTransactionByTransactionId(CASHIER_ID, TRANSACTION_ID);
                    if (LstTransactionCancelModel.Count == 0)
                    {
                        responseModel.Status = "Success";
                        responseModel.Message = "No record found.";
                        responseModel.Description = "No record found.";
                        return Ok(responseModel);
                    }
                    else
                    {
                        transactionCancelResponseModel.Status = "Success";
                        transactionCancelResponseModel.Message = "Record found.";
                        transactionCancelResponseModel.Description = LstTransactionCancelModel.Count + " record's found.";                        
                        transactionCancelResponseModel.TRANSACTION_ID = TRANSACTION_ID.ToString();
                        transactionCancelResponseModel.TOTAL_AMOUNT = LstTransactionCancelModel[0].TOTAL_AMOUNT;
                        transactionCancelResponseModel.WAYBILL_INFO = LstTransactionCancelModel;

                        return Ok(transactionCancelResponseModel);
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                return InternalServerError();
            }
        }


        [HttpGet]
        [Route("api/CancelTransactionAPI/TransactionByWaybillNo")]
        public IHttpActionResult TransactionByWaybillNo(long CASHIER_ID, string WAYBILL_NO)
        {
            CancelTransactionByWaybillModel cancelTransactionByWaybillModel = new CancelTransactionByWaybillModel();
            CancelTransactionByWaybillResponseModel cancelTransactionByWaybillResponseModel = new CancelTransactionByWaybillResponseModel();
            try
            {
                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    cancelTransactionByWaybillModel = TransactionCancelManager.TransactionByWaybillNo(CASHIER_ID, WAYBILL_NO);
                    if (cancelTransactionByWaybillModel == null || cancelTransactionByWaybillModel.BOOKING_TRANSACTION_ID<1)
                    {
                        responseModel.Status = "Success";
                        responseModel.Message = "No record found.";
                        responseModel.Description = "No record found.";
                        return Ok(responseModel);
                    }
                    else
                    {
                        cancelTransactionByWaybillResponseModel.Status = "Success";
                        cancelTransactionByWaybillResponseModel.Message = "Record found.";
                        cancelTransactionByWaybillResponseModel.Description = "1 record found.";
                        cancelTransactionByWaybillResponseModel.WAYBILL_INFO = cancelTransactionByWaybillModel;
                        cancelTransactionByWaybillResponseModel.TRANSACTION_ID = cancelTransactionByWaybillModel.TRANSACTION_ID.ToString();
                        cancelTransactionByWaybillResponseModel.TOTAL_AMOUNT = cancelTransactionByWaybillModel.TOTAL_AMOUNT;

                        return Ok(cancelTransactionByWaybillResponseModel);
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                return InternalServerError();
            }
        }
    }
}
