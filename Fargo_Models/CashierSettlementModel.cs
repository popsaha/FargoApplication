using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
    public class CashierSettlementModel
    {

        public List<SettlementDetailModel> SETTLEMENT_DETAIL { get; set; } //= new List<DETAILREPORT>();
        public List<DaySummaryReportModel> SUMMARY_REPORT { get; set; } //= new List<SUMMARYREPORT>();
        public List<MPesaDetailModel> MPESA_DETAIL { get; set; }// = new List<DETAILMPESA>();
        public List<CashDetailModel> CASH_DETAIL { get; set; }// = new List<DETAILCASH>();
        public List<CreditDetailModel> CREDIT_DETAIL { get; set; } //= new List<DETAILCREDIT>();
        public double TOTAL_AMOUNT { get; set; }
        public double NO_OF_TRANSACTIONS { get; set; }
    }

    public class SettlementDetailModel
    {
        public long BOOKING_TRANSACTION_ID { get; set; }
        public string WAYBILL_NUMBER { get; set; }
        public string INVOICE_NUMBER { get; set; }
        public string SAP_NUMBER { get; set; }
        public string TRANSACTION_ID { get; set; }
        public double TOTAL_AMOUNT { get; set; }
        public long CASHIER_ID { get; set; }
        public string PAYMENT_MODE { get; set; }
        public string CASHIER_NAME { get; set; }
    }
    public class DetailReportResponseModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsNext { get; set; }
        public List<SettlementDetailModel> Data { get; set; }
    }
    public class DaySummaryReportModel
    {
        public double NO_OF_CASH_TRANSACTION { get; set; }
        public double TOTAL_CASH_AMOUNT { get; set; }
        public double NO_OF_MPESA_TRANSACTION { get; set; }
        public double TOTAL_MPESA_AMOUNT { get; set; }
        public double NO_OF_CREDIT_TRANSACTION { get; set; }
        public double TOTAL_CREDIT_AMOUNT { get; set; }
    }
    public class DaySummaryReportResponseModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsNext { get; set; }
        public List<DaySummaryReportModel> Data { get; set; }
    }
    public class CashDetailModel
    {
        public long BOOKING_TRANSACTION_ID { get; set; }
        public string WAYBILL_NUMBER { get; set; }
        public string INVOICE_NUMBER { get; set; }
        public string SAP_NUMBER { get; set; }
        public string TRANSACTION_ID { get; set; }
        public double CASH_AMOUNT { get; set; }

    }
    public class CashDetaiReportResponseModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsNext { get; set; }
        public List<CashDetailModel> Data { get; set; }
    }
    public class MPesaDetailModel
    {
        public long BOOKING_TRANSACTION_ID { get; set; }
        public string WAYBILL_NUMBER { get; set; }
        public string INVOICE_NUMBER { get; set; }
        public string SAP_NUMBER { get; set; }
        public string TRANSACTION_ID { get; set; }
        public double MPESA_AMOUNT { get; set; }
    }
    public class MPesaDetailReportResponseModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsNext { get; set; }
        public List<MPesaDetailModel> Data { get; set; }
    }
    public class CreditDetailModel
    {
        public long BOOKING_TRANSACTION_ID { get; set; }
        public string WAYBILL_NUMBER { get; set; }
        public string INVOICE_NUMBER { get; set; }
        public string SAP_NUMBER { get; set; }
        public string TRANSACTION_ID { get; set; }
        public double CREDIT_AMOUNT { get; set; }
    }

    public class CreditDetailReportResponseModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsNext { get; set; }
        public List<CreditDetailModel> Data { get; set; }
    }
}