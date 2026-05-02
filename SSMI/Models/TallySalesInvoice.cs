using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSMI.Models
{
    public class TallySalesInvoice
    {
        public List<SalesInvoiceRoot> salesInvoices { get; set; }
    }

    public class SalesInvoiceRoot
    {
        public CustomerInfo customerInfo { get; set; }
        public List<MaterialItem> materialItemsList { get; set; }
        public List<Tally_Ledger_Details> tallyLedgerList { get; set; }
        public LoadingCharges loadingCharges { get; set; }
        //public TransportCharges transportationCharges { get; set; }

        public decimal roundOff { get; set; }
        public decimal grandTotal { get; set; }
    }

}