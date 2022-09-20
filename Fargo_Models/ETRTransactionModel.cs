using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
    public class ETRInvoiceModel
    {
        public string TRANSACTION_ID { get; set; }
    }

    public class ETRInvoiceResponseModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public bool IsNext { get; set; }
        public List<ETRInvoiceModel> Data { get; set; }
    }
}
