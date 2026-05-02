using System.Collections.Generic;

namespace SSMI.Models
{
    public class SalesInvoiceDetailsViewModel
    {
        public Tally_Voucher Voucher { get; set; }
        public List<Tally_Voucher_Inventory> InventoryLines { get; set; }
        public List<Tally_Voucher_Ledger> LedgerLines { get; set; }
    }
}
