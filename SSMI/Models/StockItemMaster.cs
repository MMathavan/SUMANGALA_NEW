using System.ComponentModel.DataAnnotations.Schema;

namespace SSMI.Models
{
    [Table("StockItemMaster")]
    public class StockItemMasterNew
    {
        public string Guid { get; set; }
        public string StockItemParent { get; set; }
        public string StockItemName { get; set; }
        public string BaseUnit { get; set; }
        public string HSNCode { get; set; }
        public string HSNDescription { get; set; }
        public decimal CgstExprn { get; set; }
        public decimal SgstExprn { get; set; }
        public decimal IgstExprn { get; set; }
        public string AlterId { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDateTime { get; set; }
        public int DispStatus { get; set; }
        public int MDispStatus { get; set; }
        public string Denominator { get; set; }
        public string Conversion { get; set; }
        public string AlterUnitName { get; set; }
    }
}
