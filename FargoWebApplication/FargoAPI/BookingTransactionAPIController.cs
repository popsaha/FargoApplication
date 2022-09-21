using Fargo_Application.App_Start;
using Fargo_DataAccessLayers;
using Fargo_Models;
using FargoWebApplication.Filter;
using FargoWebApplication.Manager;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace FargoWebApplication.FargoAPI
{
    [BasicAuthentication]
    public class BookingTransactionAPIController : ApiController
    {
        DbFargoApplicationEntities _db = new DbFargoApplicationEntities();

        [HttpPost]
        [Route("api/BookingTransactionAPI/BookingTransactionMaster")]
        public HttpResponseMessage BookingTransactionMaster([FromBody] BookingTransactionMasterModel bookingTransactionMaster)
        {
            string TransactionId = string.Empty;
            BookingResponseModel bookingResponseModel = new BookingResponseModel();
            try
            {
                string Username = Thread.CurrentPrincipal.Identity.Name;                
                if (!string.IsNullOrEmpty(Username))
                {
                    string MerchantRequestID = string.Empty; string CheckoutRequestID = string.Empty;
                    if (bookingTransactionMaster != null)
                    {
                        if (bookingTransactionMaster.BOOKING_ORDER_DETAILS != null)
                        {
                            if (bookingTransactionMaster.BOOKING_PAYMENT_DETAILS != null)
                            {
                                if (BookingTransactionMasterManager.IsValidWaybillNumber(bookingTransactionMaster))
                                {
                                    if (!BookingTransactionMasterManager.IsDuplicateWaybillFound(bookingTransactionMaster))
                                    {
                                        if (bookingTransactionMaster.CUSTOMER_ID > 0)
                                        {
                                            if (BookingTransactionMasterManager.HasNetBalance(bookingTransactionMaster))
                                            {
                                                int result = BookingTransactionMasterManager.SubmitBookingTransaction(bookingTransactionMaster, out TransactionId,MerchantRequestID, CheckoutRequestID);
                                                if (result > 0)
                                                {
                                                    bookingResponseModel.TransactionId = TransactionId;
                                                    bookingResponseModel.Status = "Success";
                                                    bookingResponseModel.Message = "Transaction booked successfully.";
                                                    bookingResponseModel.Description = "Transaction booked successfully.";
                                                    return Request.CreateResponse(HttpStatusCode.Created, bookingResponseModel);
                                                }
                                                else
                                                {
                                                    bookingResponseModel.TransactionId = null;
                                                    bookingResponseModel.Status = "Failed";
                                                    bookingResponseModel.Message = "Transaction not done.";
                                                    bookingResponseModel.Description = "Something went wrong. Please try again.";
                                                    return Request.CreateResponse(HttpStatusCode.InternalServerError, bookingResponseModel);
                                                }
                                            }
                                            else
                                            {
                                                bookingResponseModel.TransactionId = null;
                                                bookingResponseModel.Status = "Failed";
                                                bookingResponseModel.Message = "Insufficient account balance for customer.";
                                                bookingResponseModel.Description = "Insufficient account balance for customer.";
                                                return Request.CreateResponse(HttpStatusCode.BadRequest, bookingResponseModel);
                                            }
                                        }
                                        else
                                        {
                                            double MPesaAmount = 0;
                                            if (BookingTransactionMasterManager.IsMPesaTransaction("Booking", bookingTransactionMaster, null, out MPesaAmount))
                                            {
                                                if (bookingTransactionMaster.BOOKING_MPESA_TRANSACTION != null)
                                                {
                                                    MerchantRequestID = bookingTransactionMaster.BOOKING_MPESA_TRANSACTION.MERCHANT_REQUEST_ID;
                                                    CheckoutRequestID = bookingTransactionMaster.BOOKING_MPESA_TRANSACTION.CHECKOUT_REQUEST_ID;

                                                    if (!BookingTransactionMasterManager.IsMPesaValidResponse(MPesaAmount, MerchantRequestID, CheckoutRequestID, out bookingResponseModel))
                                                    {
                                                        return Request.CreateResponse(HttpStatusCode.BadRequest, bookingResponseModel);
                                                    }
                                                    else
                                                    {
                                                        int result = BookingTransactionMasterManager.SubmitBookingTransaction(bookingTransactionMaster, out TransactionId, MerchantRequestID, CheckoutRequestID);
                                                        if (result > 0)
                                                        {
                                                            bookingResponseModel.TransactionId = TransactionId;
                                                            bookingResponseModel.Status = "Success";
                                                            bookingResponseModel.Message = "Transaction booked successfully.";
                                                            bookingResponseModel.Description = "Transaction booked successfully.";
                                                            return Request.CreateResponse(HttpStatusCode.Created, bookingResponseModel);
                                                        }
                                                        else
                                                        {
                                                            bookingResponseModel.TransactionId = null;
                                                            bookingResponseModel.Status = "Failed";
                                                            bookingResponseModel.Message = "Transaction not done.";
                                                            bookingResponseModel.Description = "Something went wrong. Please try again.";
                                                            return Request.CreateResponse(HttpStatusCode.InternalServerError, bookingResponseModel);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    bookingResponseModel.TransactionId = null;
                                                    bookingResponseModel.Status = "Failed";
                                                    bookingResponseModel.Message = "Transaction not done.";
                                                    bookingResponseModel.Description = "MPesa merchant & checkout information is missing.";
                                                    return Request.CreateResponse(HttpStatusCode.BadRequest, bookingResponseModel);
                                                }
                                            }
                                            else
                                            {
                                                int result = BookingTransactionMasterManager.SubmitBookingTransaction(bookingTransactionMaster, out TransactionId, MerchantRequestID, CheckoutRequestID);
                                                if (result > 0)
                                                {
                                                    bookingResponseModel.TransactionId = TransactionId;
                                                    bookingResponseModel.Status = "Success";
                                                    bookingResponseModel.Message = "Transaction booked successfully.";
                                                    bookingResponseModel.Description = "Transaction booked successfully.";
                                                    return Request.CreateResponse(HttpStatusCode.Created, bookingResponseModel);
                                                }
                                                else
                                                {
                                                    bookingResponseModel.TransactionId = null;
                                                    bookingResponseModel.Status = "Failed";
                                                    bookingResponseModel.Message = "Transaction not done.";
                                                    bookingResponseModel.Description = "Something went wrong. Please try again.";
                                                    return Request.CreateResponse(HttpStatusCode.InternalServerError, bookingResponseModel);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        bookingResponseModel.TransactionId = null;
                                        bookingResponseModel.Status = "Failed";
                                        bookingResponseModel.Message = "Transaction not done.";
                                        bookingResponseModel.Description = "Duplicate waybill found.";
                                        return Request.CreateResponse(HttpStatusCode.InternalServerError, bookingResponseModel);
                                    }
                                }
                                else
                                {
                                    bookingResponseModel.TransactionId = null;
                                    bookingResponseModel.Status = "Failed";
                                    bookingResponseModel.Message = "Transaction not done.";
                                    bookingResponseModel.Description = "Invalid waybill found.";
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, bookingResponseModel);
                                }
                            }
                            else
                            {
                                bookingResponseModel.TransactionId = null;
                                bookingResponseModel.Status = "Failed";
                                bookingResponseModel.Message = "Transaction not done.";
                                bookingResponseModel.Description = "Payment information is missing";
                                return Request.CreateResponse(HttpStatusCode.BadRequest, bookingResponseModel);
                            }                            
                        }
                        else
                        {
                            bookingResponseModel.TransactionId = null;
                            bookingResponseModel.Status = "Failed";
                            bookingResponseModel.Message = "Transaction not done.";
                            bookingResponseModel.Description = "Waybill information is missing.";
                            return Request.CreateResponse(HttpStatusCode.BadRequest, bookingResponseModel);
                        }
                    }
                    else
                    {
                        bookingResponseModel.TransactionId = null;
                        bookingResponseModel.Status = "Failed";
                        bookingResponseModel.Message = "Transaction not done.";
                        bookingResponseModel.Description = "Required information is missing.";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, bookingResponseModel);
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, new Exception("Unauthorized, Please try again."));
                }
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message.ToString());
            }
        }


        [HttpGet]
        [Route("api/BookingTransactionAPI/ReportBookingTransaction")]
        public IHttpActionResult ReportBookingTransaction(long USER_ID, string FROM_DATE, string TO_DATE, string PAGE_NUMBER)
        {
            BookingTransactionResponseModel LstBookingTransaction = new BookingTransactionResponseModel();
            BookingTransactionMasterModel bookingTransactionMasterModel = new BookingTransactionMasterModel();
            try
            {
                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    bookingTransactionMasterModel.USER_ID = USER_ID;
                    bookingTransactionMasterModel.FROM_DATE = FROM_DATE;
                    bookingTransactionMasterModel.TO_DATE = TO_DATE;
                    bookingTransactionMasterModel.PAGE_NUMBER = PAGE_NUMBER;

                    LstBookingTransaction.Data = BookingTransactionMasterManager.ReportBookingTransaction(bookingTransactionMasterModel);

                    if (LstBookingTransaction.Data.Count == 0)
                    {
                        LstBookingTransaction.Status = "Success";
                        LstBookingTransaction.Message = "No record found.";
                        LstBookingTransaction.Description = "No record found.";
                        LstBookingTransaction.IsNext = false;
                        return Ok(LstBookingTransaction);
                    }
                    else
                    {
                        LstBookingTransaction.Status = "Success";
                        LstBookingTransaction.Message = "Record's found.";
                        LstBookingTransaction.Description = LstBookingTransaction.Data.Count + " record's found.";
                        LstBookingTransaction.IsNext = LstBookingTransaction.Data[0].IS_NEXT;
                        return Ok(LstBookingTransaction);
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


        [HttpPost]
        [Route("api/BookingTransactionAPI/CancelTransactionRequest")]
        public HttpResponseMessage CancelTransactionRequest(CancelTransactionModel cancelTransactionModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {

                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    int result = 0;
                    if (BookingTransactionMasterManager.IsValidCancelTransactionRequest(cancelTransactionModel.TRANSACTION_ID))
                    {
                        result = BookingTransactionMasterManager.CancelTransactionRequest(cancelTransactionModel);
                        if (result > 0)
                        {
                            responseModel.Status = "Success";
                            responseModel.Message = "Request sent to manager for cancellation.";
                            responseModel.Description = "Request sent to manager for cancellation.";
                            return Request.CreateResponse(HttpStatusCode.Created, responseModel);
                        }
                        else
                        {
                            responseModel.Status = "Failed";
                            responseModel.Message = "Cancellation request not sent.";
                            responseModel.Description = "Something went wrong. Please try again.";
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, responseModel);
                        }
                    }
                    else
                    {
                        responseModel.Status = "Failed";
                        responseModel.Message = "Invalid cancel request.";
                        responseModel.Description = "Cancellation request is already sent for same transaction.";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, responseModel);
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, new Exception("Unauthorized, Please try again."));
                }
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message.ToString());
            }
        }




        [HttpGet]
        [Route("api/BookingTransactionAPI/ReportCancelTransaction")]
        public IHttpActionResult ReportCancelTransaction(long USER_ID, string FROM_DATE, string TO_DATE, string PAGE_NUMBER)
        {
            CancelTransactionResponseModel LstCancelTransaction = new CancelTransactionResponseModel();
            CancelTransactionModel cancelTransactionModel = new CancelTransactionModel();
            try
            {
                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    LstCancelTransaction.Data = BookingTransactionMasterManager.ReportCancelTransaction(USER_ID, FROM_DATE, TO_DATE, PAGE_NUMBER);

                    if (LstCancelTransaction.Data.Count == 0)
                    {
                        LstCancelTransaction.Status = "Success";
                        LstCancelTransaction.Message = "No record found.";
                        LstCancelTransaction.Description = "No record found.";
                        LstCancelTransaction.IsNext = false;
                        return Ok(LstCancelTransaction);
                    }
                    else
                    {
                        LstCancelTransaction.Status = "Success";
                        LstCancelTransaction.Message = "Record's found.";
                        LstCancelTransaction.Description = LstCancelTransaction.Data.Count + " record's found.";
                        LstCancelTransaction.IsNext = LstCancelTransaction.Data[0].IS_NEXT;
                        return Ok(LstCancelTransaction);
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
        [Route("api/BookingTransactionAPI/ReportBookingTransactionOnId")]
        public IHttpActionResult ReportBookingTransactionOnId(string BOOKING_TRANSACTION_ID)
        {
            BookingTransactionMasterModel bookingTransactionMasterModel = new BookingTransactionMasterModel();
            try
            {
                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    bookingTransactionMasterModel = BookingTransactionMasterManager.ReportBookingTransactionOnId(BOOKING_TRANSACTION_ID);

                    if (bookingTransactionMasterModel.BOOKING_TRANSACTION_ID < 1)
                    {
                        responseModel.Status = "Success";
                        responseModel.Message = "No record found.";
                        responseModel.Description = "No record found.";
                        return Ok(responseModel);
                    }
                    else
                    {
                        return Ok(bookingTransactionMasterModel);
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
