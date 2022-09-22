using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
    public class CreditCustomerModel
    {
        public long CUSTOMER_ID { get; set; }
        public string CUSTOMER_CODE { get; set; }
        public string CUSTOMER_PIN { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public Nullable<long> COUNTRY_ID { get; set; }
        public Nullable<long> STATE_ID { get; set; }
        public string CITY { get; set; }
        public string ADDRESS { get; set; }
        public string PIN_CODE { get; set; }
        public string COMPANY { get; set; }
        public string PHONE_NO { get; set; }
        public string EMAIL_ID { get; set; }
        public string PASSWORD { get; set; }
        public string CUSTOMER_COMMISSION { get; set; }
        public double MPESA_AMOUNT { get; set; }
        public string DATA_SOURCE { get; set; }
    }

    public class CreditEntryModel
    {
        public long CREDIT_ENTRY_ID { get; set; }
        public long USER_ID { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string FROM_DATE { get; set; }
        public string TO_DATE { get; set; }
        public long CUSTOMER_ID { get; set; }
        public double CREDIT_ENTRY_AMOUNT { get; set; }
        public string PAYMENT_MODE { get; set; }
        public long REFERENCE_NO { get; set; }
        public string BANK_NAME { get; set; }
        public string PAYMENT_STATUS { get; set; }
        public long CASHIER_ID { get; set; }
        public long STORE_ID { get; set; }
        public string CASHIER_NAME { get; set; }
        public string STORE_NAME { get; set; }
        public string ENTRY_DATE { get; set; }
        public long MANAGER_ID { get; set; }
        public bool IS_MANAGER_APPROVED { get; set; }
        public string MANAGER_REMARK { get; set; }
        public string STATUS { get; set; }
        public BookingMPesaTransactionModel CREDIT_MPESA_TRANSACTION { get; set; }
    }
}
