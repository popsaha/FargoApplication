using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
    #region Callback API Model
    public class MpesaCallbackResponseBody
    {
        public Body Body { get; set; }
    }
    public class Body
    {
        public MPesaTransactionModel StkCallback { get; set; }
    }
    public class StkCallback
    {
        public MPesaTransactionModel MPesaTransactionModel { get; set; }
    }
    public class MPesaTransactionModel
    {
        public string MerchantRequestID { get; set; }
        public string CheckoutRequestID { get; set; }
        public int ResultCode { get; set; }
        public string ResultDesc { get; set; }
        public CallbackMetadata CallbackMetadata { get; set; } 
    }
    public class CallbackMetadata
    {
        public List<Item> Item { get; set; }
    }
    public class Item
    {
        public string Name { get; set; } 
        public string Value { get; set; } 
    }
    #endregion

    public class MPesaTransactionResponseModel
    {
        public string MerchantRequestID { get; set; }
        public string CheckoutRequestID { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
        public string CustomerMessage { get; set; }
    }

    public class MPesaProcessResponseModel
    {
        public string Status { get; set; }
        public string MerchantRequestID { get; set; }
        public string CheckoutRequestID { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
        public string CustomerMessage { get; set; }
    }
    public class MPesaProcessModel
    {
        public string BusinessShortCode { get; set; }
        public string Password { get; set; }
        public string Timestamp { get; set; }
        public string TransactionType { get; set; }
        public double Amount { get; set; }
        public string PartyA { get; set; }
        public string PartyB { get; set; }
        public string PhoneNumber { get; set; }
        public string CallBackURL { get; set; }
        public string AccountReference { get; set; }
        public string TransactionDesc { get; set; }

    }
    public class GenerateTokenModel
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
    }
   
    public class MPesaValidation
    {
        public long CUSTOMER_ID { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string CUSTOMER_MOBILE { get; set; }
        public string MPESA_AMOUNT { get; set; }
        public string MPESA_CALLBACK_REQUEST { get; set; }
        public string MPESA_PROCESS_RESPONSE { get; set; }
        public long USER_ID { get; set; }
    }
    public class BookingMPesaTransactionModel
    {
        public string MERCHANT_REQUEST_ID { get; set; }
        public string CHECKOUT_REQUEST_ID { get; set; }
    }
}
