using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSMI.Models
{
    public class ApiResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<SalesOrderData> data { get; set; }   // ✅ LIST
    }

    public class SalesOrderData
    {
        public CustomerInfo customerInfo { get; set; }
        public List<MaterialItem> materialItemsList { get; set; }
        public LoadingCharges loadingCharges { get; set; }
        public TransportCharges transportationCharges { get; set; }
        public CGST cgst { get; set; }
        public SGST sgst { get; set; }
        public IGST igst { get; set; }

        [JsonProperty("grandTotal")]
        public decimal GrandTotal { get; set; }

        [JsonProperty("roundOff")]
        public decimal RoundOff { get; set; }
    }

    public class CustomerInfo
    {
        public string orderID { get; set; }
        public string uid { get; set; }
        public string customerCode { get; set; }
        public string customerName { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string address4 { get; set; }
        public string address5 { get; set; }
        public string basicbuyeraddress1 { get; set; }
        public string basicbuyeraddress2 { get; set; }
        public string basicbuyeraddress3 { get; set; }
        public string basicbuyeraddress4 { get; set; }
        public string basicbuyeraddress5 { get; set; }
        public DateTime date { get; set; }
        public string statename { get; set; }
        public string narration { get; set; }
        public string countryofresidence { get; set; }
        public string partygstin { get; set; }
        public string placeofsupply { get; set; }
        public string partyname { get; set; }
        public string vouchernumber { get; set; }
        public string partypincode { get; set; }
        public string consigneegstin { get; set; }
        public string consigneestatename { get; set; }
        public string consigneecountryname { get; set; }
        public string basicorderref { get; set; }
        public string basicduedateofpymt { get; set; }

    }

    public class MaterialItem
    {
        public string stockitemname { get; set; }
        public string gsthsnname { get; set; }
        public decimal rate { get; set; }
        public decimal amount { get; set; }
        public string unitcode { get; set; } = "MT";
        public decimal actualqty { get; set; }
        public decimal billedqty { get; set; }
        public string godownname { get; set; }
        public string batchname { get; set; }
        public string destinationgodownname { get; set; }
        public string indentno { get; set; }
        public string orderno { get; set; }
        public string orderduedate { get; set; }
        public decimal classrate { get; set; } = 100;

        [JsonProperty("CGST")]
        public decimal? cgstexprn { get; set; }

        [JsonProperty("SGST_UTGST")]
        public decimal? sgstexprn { get; set; }

        [JsonProperty("IGST")]
        public decimal? igstexprn { get; set; }
    }

    public class LoadingCharges
    {
        public string ldgrname { get; set; } = "Loading charges -TMT";

        [JsonProperty("loadingAmount")]
        public decimal? amount { get; set; }

        [JsonProperty("CGST")]
        public decimal? cgstexprn { get; set; }

        [JsonProperty("SGST_UTGST")]
        public decimal? sgstexprn { get; set; }

        [JsonProperty("IGST")]
        public decimal? igstexprn { get; set; }
        public decimal? total { get; set; }
    }

    public class TransportCharges
    {
        public string ldgrname { get; set; } = "Transport Charges";

        [JsonProperty("transportationAmount")]
        public decimal? amount { get; set; }

        [JsonProperty("CGST")]
        public decimal? cgstexprn { get; set; }

        [JsonProperty("SGST_UTGST")]
        public decimal? sgstexprn { get; set; }

        [JsonProperty("IGST")]
        public decimal? igstexprn { get; set; }
        public decimal? total { get; set; }
    }

    public class CGST
    {
        [JsonProperty("ldgrname")]
        public string ldgrname { get; set; } = "CGST @ 9%";

        [JsonProperty("amount")]
        public decimal? amount { get; set; }

        [JsonProperty("cgstexprn")]
        public decimal? cgstexprn { get; set; }

    }

    public class SGST
    {
        [JsonProperty("ldgrname")]
        public string ldgrname { get; set; } = "SGST @ 9%";

        [JsonProperty("amount")]
        public decimal? amount { get; set; }

        [JsonProperty("sgstexprn")]
        public decimal? sgstexprn { get; set; }

    }

    public class IGST
    {
        [JsonProperty("ldgrname")]
        public string ldgrname { get; set; } = "IGST @ 18%";

        [JsonProperty("amount")]
        public decimal? amount { get; set; }

        [JsonProperty("igstexprn")]
        public decimal? igstexprn { get; set; }

    }

    public class Tally_Ledger_Details
    {
        public string LdgrName { get; set; }
        public string IsPartyLedger { get; set; }
        public decimal? amount { get; set; }

    }

    public class PreCloseSalesOrderData
    {
        public CustomerInfo customerInfo { get; set; }
        public List<PreCloseMaterialItem> materialItemsList { get; set; }
        public LoadingCharges loadingCharges { get; set; }
        public TransportCharges transportationCharges { get; set; }
        public CGST cgst { get; set; }
        public SGST sgst { get; set; }
        public IGST igst { get; set; }

        [JsonProperty("grandTotal")]
        public decimal GrandTotal { get; set; }

        [JsonProperty("roundOff")]
        public decimal RoundOff { get; set; }
    }

    public class PreCloseMaterialItem
    {
        public string stockitemname { get; set; }
        public string gsthsnname { get; set; }
        public decimal rate { get; set; }
        public decimal amount { get; set; }
        public string unitcode { get; set; } = "MT";
        public decimal actualqty { get; set; }
        public decimal billedqty { get; set; }
        public string godownname { get; set; }
        public string batchname { get; set; }
        public string destinationgodownname { get; set; }
        public string indentno { get; set; }
        public string orderno { get; set; }
        public string orderduedate { get; set; }
        public decimal classrate { get; set; } = 100;

        [JsonProperty("CGST")]
        public decimal? cgstexprn { get; set; }

        [JsonProperty("SGST_UTGST")]
        public decimal? sgstexprn { get; set; }

        [JsonProperty("IGST")]
        public decimal? igstexprn { get; set; }

        [JsonProperty("orderpreclosureqty")]
        public decimal OrderPreClosureQty { get; set; }

        [JsonProperty("orderpreclosuredate")]
        public string OrderPreClosureDate { get; set; }

        [JsonProperty("orderclosurereason")]
        public string OrderClosureReason { get; set; }
    }

}