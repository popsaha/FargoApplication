using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
    public class LotModel
    {
        public long LOT_ID { get; set; }
        public List<StoreModel> LST_STORE { get; set; }
        public string LOT_NAME { get; set; }     
        public string LOT_CODE { get; set; }      
        public string DESCRIPTION { get; set; }
        public string FROM_TRACKING_NUMBER { get; set; }
        public string TO_TRACKING_NUMBER { get; set; }
        public string CURRENT_TRACKING_NUMBER { get; set; }
        public long USER_ID { get; set; }
        public long STORE_ID { get; set; }
        public string STORE_NAME { get; set; }
    }
}
