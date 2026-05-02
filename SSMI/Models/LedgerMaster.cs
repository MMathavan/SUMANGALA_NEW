using System.ComponentModel.DataAnnotations.Schema;

namespace SSMI.Models
{
    [Table("LedgerMaster")]
    public class LedgerMasterNew
    {
        public string Guid { get; set; }
        public string LegerGroupName { get; set; }
        public string LegerName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address5 { get; set; }
        public string Address6 { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public string Pincode { get; set; }
        public string EmailId { get; set; }
        public string Website { get; set; }
        public string PanNo { get; set; }
        public string GSTinNo { get; set; }
        public string LedgerContactName { get; set; }
        public string LedgerContactNo { get; set; }
        public string CreditPeriod { get; set; }
        public string OpeningBalance { get; set; }
        public string AlterId { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDateTime { get; set; }
        public int DispStatus { get; set; }
        public int MDispStatus { get; set; }
        public string CreditLimit { get; set; }
    }
}
