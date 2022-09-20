using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
   public class ReportCashierVoidWaybillModel
    {
        public long PAGE_NUMBER { get; set; }
        public DateTime CREATED_ON { get; set; }
        public long VOID_TRACKING_TRANSACTION_ID { get; set; }
        public long STORE_ID { get; set; }
        public string STORE_NAME { get; set; }
        public long CASHIER_ID { get; set; }
        public string CASHIER_NAME { get; set; }

        public string WAYBILL_NUMBER { get; set; }
        public string CANCELLATION_REASON { get; set; }
        public string STATUS { get; set; }
        public string REQUESTED_DATE { get; set; }
        public string RESPONDED_DATE { get; set; }
        public long IS_NEXT { get; set; }
    }
}
