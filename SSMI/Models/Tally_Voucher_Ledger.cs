using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSMI.Models
{
    [Table("Tally_Voucher_Ledger")]
    public class Tally_Voucher_Ledger
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LedgerId { get; set; }

        public long VoucherId { get; set; }

        public string LedgerName { get; set; }
        public bool IsPartyLedger { get; set; }
        public decimal Amount { get; set; }
        public short IsTaxLedger { get; set; }

        [ForeignKey("VoucherId")]
        public virtual Tally_Voucher Voucher { get; set; }
    }
}
