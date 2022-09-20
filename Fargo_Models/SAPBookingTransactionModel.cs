using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
    public class SalesOrder_Request
    {
        public SAPFreightwareHeaderModel Header { get; set; }
        public List<SAPFreightwareItemModel> Item { get; set; }
    }

    public class SAPFreightwareHeaderModel
    {
        public string SoldtoParty { get; set; }
        public string DocumentType { get; set; }
        public string CUNumber { get; set; }
        public string CUInvoiceNumber { get; set; }
        public string MpesaReferenceNumber { get; set; }
        public string AppNumber { get; set; }
        public string Plant { get; set; }
        public string Date { get; set; }
        public string Currency { get; set; }
    }

    public class SAPFreightwareItemModel
    {
        public string ItemPosition { get; set; }
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
        public string UOM { get; set; }
    }
}
