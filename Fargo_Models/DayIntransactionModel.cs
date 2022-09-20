using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
   public class DayIntransactionModel
    {
        public long STORE_ID { get; set; }
        public long CASHIER_ID { get; set; }
        public decimal TOTAL_DAY_IN_AMOUNT { get; set; }
        public decimal TOTAL_DAY_END_AMOUNT { get; set; }
       
    }
}
