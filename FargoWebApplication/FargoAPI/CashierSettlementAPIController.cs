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
    public class CashierSettlementAPIController : ApiController
    {
        [HttpGet]
        [Route("api/CashierSettlementAPI/LstCashierSettlement")]
        public IHttpActionResult LstCashierSettlement(string DATE, long CASHIER_ID, int Flag)
        {
            //List<CashierSettlementModel> LstCashierSettlement = new List<CashierSettlementModel>();
            
            try
            {
                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    if (Flag == 1)
                    {
                        DetailReportResponseModel lstCashierSettlement = new DetailReportResponseModel();
                        lstCashierSettlement.Data = CashierSettlementManager.LstDETAILREPORT(DATE, CASHIER_ID);
                        if (lstCashierSettlement.Data.Count == 0)
                        {
                            lstCashierSettlement.Status = "Success";
                            lstCashierSettlement.Message = "No record found.";
                            lstCashierSettlement.Description = "No record found.";
                            lstCashierSettlement.IsNext = false;
                            return Ok(lstCashierSettlement);
                        }
                        else
                        {
                            lstCashierSettlement.Status = "Success";
                            lstCashierSettlement.Message = " Record found.";
                            lstCashierSettlement.IsNext = false;
                            lstCashierSettlement.Description = lstCashierSettlement.Data.Count + " record's found.";

                            return Ok(lstCashierSettlement);
                        }
                    }
                    else if (Flag == 2)
                    {
                        DaySummaryReportResponseModel LstCashierSettlement = new DaySummaryReportResponseModel();
                        LstCashierSettlement.Data = CashierSettlementManager.LstDaySummaryReport(DATE, CASHIER_ID);
                        if (LstCashierSettlement.Data.Count == 0)
                        {
                            LstCashierSettlement.Status = "Success";
                            LstCashierSettlement.Message = "No record found.";
                            LstCashierSettlement.Description = "No record found.";
                            LstCashierSettlement.IsNext = false;
                            return Ok(LstCashierSettlement);
                        }
                        else
                        {
                            LstCashierSettlement.Status = "Success";
                            LstCashierSettlement.Message = " record found.";
                            LstCashierSettlement.IsNext = false;
                            LstCashierSettlement.Description = LstCashierSettlement.Data.Count + " record's found.";

                            return Ok(LstCashierSettlement);
                        }
                    }
                    else if (Flag == 3)
                    {
                        MPesaDetailReportResponseModel LstCashierSettlement = new MPesaDetailReportResponseModel();
                        LstCashierSettlement.Data = CashierSettlementManager.LstMPesaDetailReport(DATE, CASHIER_ID);
                        if (LstCashierSettlement.Data.Count == 0)
                        {
                            LstCashierSettlement.Status = "Success";
                            LstCashierSettlement.Message = "No record found.";
                            LstCashierSettlement.Description = "No record found.";
                            LstCashierSettlement.IsNext = false;
                            return Ok(LstCashierSettlement);
                        }
                        else
                        {
                            LstCashierSettlement.Status = "Success";
                            LstCashierSettlement.Message = " record found.";
                            LstCashierSettlement.IsNext = false;
                            LstCashierSettlement.Description = LstCashierSettlement.Data.Count + " record's found.";

                            return Ok(LstCashierSettlement);
                        }
                    }
                    else if (Flag == 4)
                    {
                        CashDetaiReportResponseModel LstCashierSettlement = new CashDetaiReportResponseModel();
                        LstCashierSettlement.Data = CashierSettlementManager.LstCashDetailReport(DATE, CASHIER_ID);
                        if (LstCashierSettlement.Data.Count == 0)
                        {
                            LstCashierSettlement.Status = "Success";
                            LstCashierSettlement.Message = "No record found.";
                            LstCashierSettlement.Description = "No record found."; LstCashierSettlement.IsNext = false;
                            return Ok(LstCashierSettlement);
                        }
                        else
                        {
                            LstCashierSettlement.Status = "Success";
                            LstCashierSettlement.Message = " record found.";
                            LstCashierSettlement.IsNext = false;
                            LstCashierSettlement.Description = LstCashierSettlement.Data.Count + " record's found.";

                            return Ok(LstCashierSettlement);
                        }
                    }
                    else if (Flag == 5)
                    {
                        CreditDetailReportResponseModel LstCashierSettlement = new CreditDetailReportResponseModel();
                        LstCashierSettlement.Data = CashierSettlementManager.LstCreditDetailReport(DATE, CASHIER_ID);
                        if (LstCashierSettlement.Data.Count == 0)
                        {
                            LstCashierSettlement.Status = "Success";
                            LstCashierSettlement.Message = "No record found.";
                            LstCashierSettlement.Description = "No record found.";
                            LstCashierSettlement.IsNext = false;
                            return Ok(LstCashierSettlement);
                        }
                        else
                        {
                            LstCashierSettlement.Status = "Success";
                            LstCashierSettlement.Message = " record found.";
                            LstCashierSettlement.IsNext = false;
                            LstCashierSettlement.Description = LstCashierSettlement.Data.Count + " record's found.";

                            return Ok(LstCashierSettlement);
                        }
                    }
                    else
                    {
                        responseModel.Status = "Success";
                        responseModel.Message = "No record found.";
                        responseModel.Description = "No record found.";
                        return Ok(responseModel);
                    }

                    //LstCashierSettlement = CashierSettlementManager.LstCashierSettlement(DATE, CASHIER_ID);
                    //if (LstCashierSettlement.Count == 0)
                    //{
                    //    responseModel.Status = "Success";
                    //    responseModel.Message = "No record found.";
                    //    responseModel.Description = "No record found.";
                    //    return Ok(LstCashierSettlement);
                    //}
                    //else
                    //{
                    //    responseModel.Status = "Success";
                    //    responseModel.Message = " record found.";
                    //    responseModel.Description = LstCashierSettlement.Count + " record's found.";

                    //    return Ok(LstCashierSettlement);
                    //}
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
        [Route("api/CashierSettlementAPI/LstDETAILREPORT")]
        public IHttpActionResult LstDETAILREPORT(string DATE, long CASHIER_ID)
        {
            DetailReportResponseModel LstCashierSettlement = new DetailReportResponseModel();
            try
            {
                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    LstCashierSettlement.Data = CashierSettlementManager.LstDETAILREPORT(DATE, CASHIER_ID);
                    if (LstCashierSettlement.Data.Count == 0)
                    {
                        LstCashierSettlement.Status = "Success";
                        LstCashierSettlement.Message = "No record found.";
                        LstCashierSettlement.Description = "No record found.";
                        LstCashierSettlement.IsNext = false;
                        return Ok(LstCashierSettlement);
                    }
                    else
                    {
                        LstCashierSettlement.Status = "Success";
                        LstCashierSettlement.Message = " Record found.";
                        LstCashierSettlement.IsNext = false;
                        LstCashierSettlement.Description = LstCashierSettlement.Data.Count + " record's found.";

                        return Ok(LstCashierSettlement);
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
        [Route("api/CashierSettlementAPI/LstDaySummaryReport")]
        public IHttpActionResult LstDaySummaryReport(string DATE, long CASHIER_ID)
        {
            DaySummaryReportResponseModel LstCashierSettlement = new DaySummaryReportResponseModel();
            try
            {
                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    LstCashierSettlement.Data = CashierSettlementManager.LstDaySummaryReport(DATE, CASHIER_ID);
                    if (LstCashierSettlement.Data.Count == 0)
                    {
                        LstCashierSettlement.Status = "Success";
                        LstCashierSettlement.Message = "No record found.";
                        LstCashierSettlement.Description = "No record found.";
                        LstCashierSettlement.IsNext = false;
                        return Ok(LstCashierSettlement);
                    }
                    else
                    {
                        LstCashierSettlement.Status = "Success";
                        LstCashierSettlement.Message = " record found.";
                        LstCashierSettlement.IsNext = false;
                        LstCashierSettlement.Description = LstCashierSettlement.Data.Count + " record's found.";

                        return Ok(LstCashierSettlement);
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
        [Route("api/CashierSettlementAPI/LstMPesaDetailReport")]
        public IHttpActionResult LstMPesaDetailReport(string DATE, long CASHIER_ID)
        {
            MPesaDetailReportResponseModel LstCashierSettlement = new MPesaDetailReportResponseModel();
            try
            {
                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    LstCashierSettlement.Data = CashierSettlementManager.LstMPesaDetailReport(DATE, CASHIER_ID);
                    if (LstCashierSettlement.Data.Count == 0)
                    {
                        LstCashierSettlement.Status = "Success";
                        LstCashierSettlement.Message = "No record found.";
                        LstCashierSettlement.Description = "No record found.";
                        LstCashierSettlement.IsNext = false;
                        return Ok(LstCashierSettlement);
                    }
                    else
                    {
                        LstCashierSettlement.Status = "Success";
                        LstCashierSettlement.Message = " record found.";
                        LstCashierSettlement.IsNext = false;
                        LstCashierSettlement.Description = LstCashierSettlement.Data.Count + " record's found.";

                        return Ok(LstCashierSettlement);
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
        [Route("api/CashierSettlementAPI/LstCashDetailReport")]
        public IHttpActionResult LstCashDetailReport(string DATE, long CASHIER_ID)
        {
            CashDetaiReportResponseModel LstCashierSettlement = new CashDetaiReportResponseModel();
            try
            {
                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    LstCashierSettlement.Data = CashierSettlementManager.LstCashDetailReport(DATE, CASHIER_ID);
                    if (LstCashierSettlement.Data.Count == 0)
                    {
                        LstCashierSettlement.Status = "Success";
                        LstCashierSettlement.Message = "No record found.";
                        LstCashierSettlement.Description = "No record found."; LstCashierSettlement.IsNext = false;
                        return Ok(LstCashierSettlement);
                    }
                    else
                    {
                        LstCashierSettlement.Status = "Success";
                        LstCashierSettlement.Message = " record found.";
                        LstCashierSettlement.IsNext = false;
                        LstCashierSettlement.Description = LstCashierSettlement.Data.Count + " record's found.";

                        return Ok(LstCashierSettlement);
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
        [Route("api/CashierSettlementAPI/LstCreditDetailReport")]
        public IHttpActionResult LstCreditDetailReport(string DATE, long CASHIER_ID)
        {
            CreditDetailReportResponseModel LstCashierSettlement = new CreditDetailReportResponseModel();
            try
            {
                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    LstCashierSettlement.Data = CashierSettlementManager.LstCreditDetailReport(DATE, CASHIER_ID);
                    if (LstCashierSettlement.Data.Count == 0)
                    {
                        LstCashierSettlement.Status = "Success";
                        LstCashierSettlement.Message = "No record found.";
                        LstCashierSettlement.Description = "No record found.";
                        LstCashierSettlement.IsNext = false;
                        return Ok(LstCashierSettlement);
                    }
                    else
                    {
                        LstCashierSettlement.Status = "Success";
                        LstCashierSettlement.Message = " record found.";
                        LstCashierSettlement.IsNext = false;
                        LstCashierSettlement.Description = LstCashierSettlement.Data.Count + " record's found.";

                        return Ok(LstCashierSettlement);
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
