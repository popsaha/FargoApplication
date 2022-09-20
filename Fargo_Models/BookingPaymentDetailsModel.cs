using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
    public class BookingPaymentDetailsModel
    {
        public long BOOKING_PAYMENT_DETAILS_ID { get; set; }
        public Nullable<long> BOOKING_TRANSACTION_ID { get; set; }
        public string REFERENCE_NUMBER { get; set; }
        public string TRACKING_NUMBER { get; set; }
        public string PAYMENT_MODE { get; set; }


        public double AMOUNT { get; set; }
        public long TAX_ID { get; set; }
        public double TAX_RATE { get; set; }
        public double TAX_AMOUNT { get; set; }
        public double TOTAL_AMOUNT { get; set; }
        public string STATUS { get; set; }
        public string DESCRIPTION { get; set; }
        public string DATA_SOURCE { get; set; }
        public Nullable<bool> IS_ACTIVE { get; set; }
        public Nullable<long> CREATED_BY { get; set; }
        public Nullable<System.DateTime> CREATED_ON { get; set; }
        public Nullable<long> MODIFIED_BY { get; set; }
        public Nullable<System.DateTime> MODIFIED_ON { get; set; }
        public Nullable<long> DELETED_BY { get; set; }
        public Nullable<System.DateTime> DELETED_ON { get; set; }
        
    }

    public class BookingPaymentResponseModel
    {
        public long BOOKING_PAYMENT_DETAILS_ID { get; set; }
        public long BOOKING_TRANSACTION_ID { get; set; }
        public string PAYMENT_MODE { get; set; }
        public double AMOUNT { get; set; }

        //[+][13-09-2022][for MPESA modification][snehashish]
        //public string MERCHANT_REQUEST_ID { get; set; }
        //public string CHECKOUT_REQUEST_ID { get; set; }
        //[-][13-09-2022][for MPESA modification][snehashish]
    }
}
