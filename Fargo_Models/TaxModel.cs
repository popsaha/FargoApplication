using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
    public class TaxModel
    {
        public long TAX_ID{get; set;}
        public string TAX_NAME { get; set; }
        public double TAX_RATE { get; set; }
        public string DESCRIPTION { get; set; }
        public string TAX_GROUP_NAME { get; set; }
    }
}
