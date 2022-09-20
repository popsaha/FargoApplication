using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
   public class DayInEndTransactionModel
    {
       public long DAY_IN_END_TRANSACTION_ID { get; set; }
        public double TOTAL_DAY_IN_AMOUNT { get; set; }     
        public double TOTAL_MPESA_AMOUNT  { get; set; }   
        public double TOTAL_CASH_AMOUNT { get; set; }
        public double EXPECTED_CASH_AMOUNT  { get; set; }
        public double TOTAL_CANCEL_AMOUNT { get; set; }
        public string CREATED_ON { get; set; }
    }
}
