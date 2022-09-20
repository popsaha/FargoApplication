using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
    public class TransactionCancelModel
    {        
        public string TRANSACTION_ID { get; set; }
        public long BOOKING_TRANSACTION_ID { get; set; }
        public string WAYBILL_NO { get; set; }
        public double TOTAL_AMOUNT { get; set; }
    }

    public class TransactionCancelResponseModel 
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public string TRANSACTION_ID { get; set; }
        public double TOTAL_AMOUNT { get; set; }
        public List<TransactionCancelModel> WAYBILL_INFO { get; set; }
    }
    public class CancelTransactionByWaybillModel
    {
        public string TRANSACTION_ID { get; set; }
        public long CASHIER_ID { get; set; }
        public long BOOKING_TRANSACTION_ID { get; set; }
        public string WAYBILL_NO { get; set; }
        public double TOTAL_AMOUNT { get; set; }
        public DateTime DATE { get; set; }

    }

    public class CancelTransactionByWaybillResponseModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public string TRANSACTION_ID { get; set; }
        public double TOTAL_AMOUNT { get; set; }
        public CancelTransactionByWaybillModel WAYBILL_INFO { get; set; }
    }
}
