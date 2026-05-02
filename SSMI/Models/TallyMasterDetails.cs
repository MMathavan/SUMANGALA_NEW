using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSMI.Models
{
    public class TallyMasterDetails
    {
    }

    public class StockGroupMaster
    {
        public string Guid { get; set; }
        public string StockGroupParent { get; set; }
        public string StockGroupName { get; set; }
        public string HSNCode { get; set; }
        //public string HSNDescription { get; set; }
        public decimal CgstExprn { get; set; }
        public decimal SgstExprn { get; set; }
        public decimal IgstExprn { get; set; }
        public string AlterId { get; set; }
        //public string CreatedDate { get; set; }
        //public string UpdatedDateTime { get; set; }
        //public int DispStatus { get; set; }
        //public int MDispStatus { get; set; }

    }

    public class StockItemMaster
    {
        public string Guid { get; set; }
        public string StockItemParent { get; set; }
        public string StockItemName { get; set; }
        public string BaseUnit { get; set; }
        public string HSNCode { get; set; }
        //public string HSNDescription { get; set; }
        public decimal CgstExprn { get; set; }
        public decimal SgstExprn { get; set; }
        public decimal IgstExprn { get; set; }
        public string AlterId { get; set; }
        public string Denominator { get; set; }
        public string Conversion { get; set; }
        public string AlterUnitName { get; set; }
        //public string CreatedDate { get; set; }
        //public string UpdatedDateTime { get; set; }
        //public int DispStatus { get; set; }
        //public int MDispStatus { get; set; }

    }

    public class StockUnitMaster
    {
        public string Guid { get; set; }
        public string UnitName { get; set; }
        public string UnitDecimal { get; set; }
        public string AlternateUnitName { get; set; }
        public string AlterId { get; set; }
        //public string CreatedDate { get; set; }
        //public string UpdatedDateTime { get; set; }
        //public int DispStatus { get; set; }
        //public int MDispStatus { get; set; }

    }

    public class LedgerMaster
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
        public string CreditLimit { get; set; }
        //public string OpeningBalance { get; set; }
        public string AlterId { get; set; }
        //public string CreatedDate { get; set; }
        //public string UpdatedDateTime { get; set; }
        //public int DispStatus { get; set; }
        //public int MDispStatus { get; set; }


    }

    public class LedgerClosingDetails
    {
        public string Guid { get; set; }
        public string LegerGroupName { get; set; }
        public string LegerName { get; set; }
        public string ClosingBalance { get; set; }

    }


}