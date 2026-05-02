using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSMI.Models
{
    [Table("Tally_Voucher_Inventory")]
    public class Tally_Voucher_Inventory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long InventoryId { get; set; }

        public long VoucherId { get; set; }

        public string StockItemName { get; set; }
        public string HSNCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public string UnitCode { get; set; }
        public decimal Amount { get; set; }
        public string GodownName { get; set; }
        public string BatchName { get; set; }
        public string OrderNo { get; set; }
        public string OrderDueDate { get; set; }
        public string LedgerName { get; set; }
        public decimal ClassRate { get; set; }
        public decimal CGSTExprn { get; set; }
        public decimal SGSTExprn { get; set; }
        public decimal IGSTExprn { get; set; }
        public decimal OrderPreClosureQty { get; set; }
        public DateTime? OrderPreClosureDate { get; set; }
        public string OrderClosureReason { get; set; }

        [ForeignKey("VoucherId")]
        public virtual Tally_Voucher Voucher { get; set; }
    }
}
