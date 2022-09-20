using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
    public class SettlementModel
    {
        public long STORE_ID { get; set; }
        public string FROM_DATE { get; set; }
        public string TO_DATE { get; set; }
        public string SEARCH_DATE { get; set; }
        public string DATE { get; set; }
        public long USER_ID { get; set; }
        public long DAY_IN_END_TRANSACTION_ID { get; set; }
        public long CASHIER_ID { get; set; }
        public string DAY_IN_TIME { get; set; }
        public double TOTAL_DAY_IN_AMOUNT { get; set; }
        public double TOTAL_CASH_AMOUNT { get; set; }
        public double TOTAL_MPESA_AMOUNT { get; set; }
        public double TOTAL_CREDIT_AMOUNT { get; set; }
        public int NO_OF_CASH_TRANSACTION { get; set; }
        public int NO_OF_MPESA_TRANSACTION { get; set; }
        public int NO_OF_CREDIT_TRANSACTION { get; set; }
        public double TOTAL_DAY_END_AMOUNT { get; set; }
        public string DAY_END_TIME { get; set; }
        public long MANAGER_ID { get; set; }
        public string IS_MANAGER_APPROVED { get; set; }
        public string MANAGER_REMARK { get; set; }
        public string STATUS { get; set; }
        public string STORE_NAME { get; set; }
        public string CASHIER_NAME { get; set; }
        public string MANAGER_NAME { get; set; }
    }
}
