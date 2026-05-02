using System.ComponentModel.DataAnnotations.Schema;

namespace SSMI.Models
{
    [Table("StockGroupMaster")]
    public class StockGroupMasterNew
    {
        public string Guid { get; set; }
        public string StockGroupParent { get; set; }
        public string StockGroupName { get; set; }
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
    }
}
