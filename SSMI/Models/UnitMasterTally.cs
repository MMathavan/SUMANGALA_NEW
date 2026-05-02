using System.ComponentModel.DataAnnotations.Schema;

namespace SSMI.Models
{
    [Table("UnitMaster")]
    public class UnitMasterTally
    {
        public string Guid { get; set; }
        public string UnitName { get; set; }
        public string UnitDecimal { get; set; }
        public string AlterId { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDateTime { get; set; }
        public int DispStatus { get; set; }
        public int MDispStatus { get; set; }
        public string AlterUnitName { get; set; }
    }
}
