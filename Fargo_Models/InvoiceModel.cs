using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
    public class InvoiceModel
    {
        public long USER_ID { get; set; }
        public long  REPRINT_INVOICE_RECEIPT_ID {get;set;}
        public long BOOKING_TRANSACTION_ID { get; set; }
        public string  TRANSACTION_ID {get;set;}
        public long CASHIER_ID { get; set; }
        public string CASHIER_NAME { get; set; }
        public bool IS_CASHIER_APPROVED { get; set; }
        public string CASHIER_REQUESTED_ON { get; set; }
        public string  CASHIER_REMARK {get;set;}
        public long MANAGER_ID { get; set; }
        public bool  IS_MANAGER_APPROVED {get;set;}
        public string MANAGER_RESPONDED_ON { get; set; }
        public string MANAGER_REMARK { get; set; }
        public bool IS_INVOICE_PRINTED { get; set; }
        public long INVOICE_PRINTED_BY { get; set; }
        public string  INVOICE_PRINTED_ON {get;set;}
        public string STATUS { get; set; }
        public string FROM_DATE { get; set; }
        public string TO_DATE { get; set; }
    }

    public class InvoiceResponseModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public bool IsNext { get; set; }
        public List<InvoiceModel> InvoiceInfo { get; set; }
    
    }
}
