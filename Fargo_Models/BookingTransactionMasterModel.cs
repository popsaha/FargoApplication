using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
   public class BookingTransactionMasterModel
    {
        public long BOOKING_TRANSACTION_ID { get; set; }
        public long USER_ID { get; set; }
        public long CASHIER_ID { get; set; }
        public long STORE_ID { get; set; }
        public string MATERIAL_CODE { get; set; }
        public string IMEI_NUMBER { get; set; }
        public long CUSTOMER_ID { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string CUSTOMER_CONTACT { get; set; }
        public string CUSTOMER_PIN  { get; set; }
        public double AMOUNT { get; set; }
        public string TRANSACTION_ID { get; set; }
        public double INVOICE_AMOUNT { get; set; }
        public double TAX_ID { get; set; }
        public double TAX_RATE { get; set; }
        public double TAX_AMOUNT { get; set; }
        public double TOTAL_AMOUNT { get; set; }
        public string FROM_DATE { get; set; }
        public string TO_DATE { get; set; }
        public string DATE { get; set; }
        public string TIME { get; set; }
        public string STORE_NAME { get; set; }
        public string CASHIER_NAME { get; set; }
        public string MANAGER_NAME { get; set; }
        public string REQUESTED_ON { get; set; }
        public string RESPONDED_ON { get; set; }
        public Nullable<bool> IS_NEXT { get; set; }
        public string PAGE_NUMBER { get; set; }
        public string CUSTOMER_TYPE { get; set; }
        public string ETR_STATUS { get; set; }
        public List<BookingOrderDetailsModel> BOOKING_ORDER_DETAILS { get; set; }
        public List<BookingPaymentDetailsModel> BOOKING_PAYMENT_DETAILS { get; set; }
        public List<BookingOrderResponseModel> BOOKING_ORDER_RESPONSE { get; set; }
        public List<BookingPaymentResponseModel> BOOKING_PAYMENT_RESPONSE { get; set; }
        public List<CancelTransactionModel> CANCEL_TRANSACTION_MODEL { get; set; }
        public BookingMPesaTransactionModel BOOKING_MPESA_TRANSACTION { get; set; }
    }

   public class BookingResponseModel
   {
       public string Status { get; set; }
       public string Message { get; set; }
       public string Description { get; set; }
       public string TransactionId { get; set; }
   }



   public class BookingTransactionResponseModel
   {
       public string Status { get; set; }
       public string Message { get; set; }
       public string Description { get; set; }
       public Nullable<bool> IsNext { get; set; }
       public List<BookingTransactionMasterModel> Data { get; set; }
   }

   public class ETRTransactionModel 
   {

       public ETRTransactionBuyerModel buyer { get; set; }
       public string cashier1 { get; set; }
       public string currencyCode { get; set; }
       public double discountAmount { get; set; }
       public string invoiceDocumentReference { get; set; }
       public string issueDate { get; set; }
       public string issueTime { get; set; }
       public List<ETRTransactionItemModel> items { get; set; }
       public string posID { get; set; }
       public string registrationID { get; set; }
       public ETRTransactionTaxModel tax { get; set; }
       public double taxExclusiveAmount { get; set; }
       public double taxInclusiveAmount { get; set; }
       public string transactionID { get; set; }       
       public long transactionTypeCode { get; set; }
   }

   public class ETRTransactionBuyerModel 
   {
       public string registrationName { get; set; }
       public string taxIdentificationNumber { get; set; }
   }

   public class ETRTransactionItemModel
   {
       public string code { get; set; }
       public string description { get; set; }
       public double discount { get; set; }
       public string hs { get; set; }
       public double invoicedQuantity { get; set; }
       public double price { get; set; }
       public string taxCode { get; set; }
       public double total { get; set; }
   }

   public class ETRTransactionTaxModel
   {
       public double vatNetAmount { get; set; }
       public double vatTaxAmount { get; set; }
   }

   public class ETRTransactionResponseModel
   {
       public string transactionID { get; set; }
       public ETRTransactionSignatureResponseModel signature { get; set; }
       public string qr { get; set; }
       public bool isDuplicate { get; set; }
       public string success { get; set; }
       public string errorCode { get; set; }
       public string errorMessage { get; set; }
   }

   public class ETRFileLocationModel
   {
       public string Status { get; set; }
       public string Message { get; set; }
       public string TransactionId { get; set; }
       public string FileLocation { get; set; }     
   }
   public class ETRTransactionSignatureResponseModel
   {
       public string cuNumber { get; set; }
       public string timestamp { get; set; }
       public string fiscalTransactionNumber { get; set; }
   }

   public class CancelTransactionModel
   {
       public long CANCEL_BOOKING_TRANSACTION_ID { get; set; }
       public long STORE_ID { get; set; }
       public long CASHIER_ID { get; set; }
       public string TRANSACTION_ID { get; set; }
       public double CANCEL_AMOUNT { get; set; }
       public long CANCELLATION_ID { get; set; }
       public bool IS_NEXT { get; set; }
       public string PAGE_NUMBER { get; set; }
       public string STATUS { get; set; }
       public string REASON { get; set; }
       public string DATE { get; set; }
       public List<BookingOrderResponseModel> WAYBILL_DETAILS { get; set; }
       public long USER_ID { get; set; }
       public long BOOKING_TRANSACTION_ID { get; set; }
       public long WAYBILL_NO { get; set; }
       public double TOTAL_AMOUNT { get; set; }
       public string CASHIER_NAME { get; set; }
       public string STORE_NAME { get; set; }
       public string FROM_DATE { get; set; }
       public string TO_DATE { get; set; }
       public bool IS_MANAGER_APPROVED { get; set; }
       public string MANAGER_REMARK { get; set; }
   }

   public class CancelTransactionResponseModel
   {
       public string Status { get; set; }
       public string Message { get; set; }
       public string Description { get; set; }
       public bool IsNext { get; set; }
       public List<CancelTransactionModel> Data { get; set; }
   }
}
