using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSMI.Models
{
    [Table("dbo.Tally_Voucher")]
    public class Tally_Voucher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VoucherId { get; set; }

        public string RemoteId { get; set; }
        public string VoucherType { get; set; }
        public string VoucherNumber { get; set; }
        public DateTime? VoucherDate { get; set; }
        public string PartyName { get; set; }
        public string Party_Addr1 { get; set; }
        public string Party_Addr2 { get; set; }
        public string Party_Addr3 { get; set; }
        public string Party_Addr4 { get; set; }
        public string Party_Addr5 { get; set; }
        public string Party_StateDesc { get; set; }
        public string Party_Pincode { get; set; }
        public string Party_Country { get; set; }
        public string Party_Desp_Addr1 { get; set; }
        public string Party_Desp_Addr2 { get; set; }
        public string Party_Desp_Addr3 { get; set; }
        public string Party_Desp_Addr4 { get; set; }
        public string Party_Desp_Addr5 { get; set; }
        public string Party_Desp_StateDesc { get; set; }
        public string Party_Desp_Pincode { get; set; }
        public string Party_Desp_Country { get; set; }
        public string PartyGSTIN { get; set; }
        public string PlaceOfSupply { get; set; }
        public string BilltoPlace { get; set; }
        public string ShiptoPlace { get; set; }
        public string BasicOrderRef { get; set; }
        public string BasicShipVesselNo { get; set; }
        public string BasicBuyersSalesTaxNo { get; set; }
        public string BasicDueDateOfPymt { get; set; }
        public string Narration { get; set; }
        public decimal? TotalAmount { get; set; }
        public string IRN { get; set; }
        public string IRNAckNo { get; set; }
        public DateTime? IRNAckDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int SyncStatus { get; set; }
        public int RegstrId { get; set; }
        public string SyncError { get; set; }
        public DateTime? SyncOn { get; set; }
        public decimal RoundoffAmt { get; set; }
        public int PreSyncStatus { get; set; }
        public DateTime? PreSyncOn { get; set; }
        public string PreSyncError { get; set; }

        public virtual ICollection<Tally_Voucher_Inventory> InventoryLines { get; set; }
        public virtual ICollection<Tally_Voucher_Ledger> LedgerLines { get; set; }
    }

    public class TallyRoot
    {
        [JsonProperty("tallymessage")]
        public List<TallyMessage> TallyMessage { get; set; }
    }

    public class TallyMessage
    {
        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("address")]
        public List<object> Address { get; set; }

        [JsonProperty("basicbuyeraddress")]
        public List<object> BasicBuyerAddress { get; set; }

        [JsonProperty("alteredon")]
        public string AlteredOn { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("guid")]
        public string Guid { get; set; }

        [JsonProperty("partyname")]
        public string PartyName { get; set; }

        [JsonProperty("partyledgername")]
        public string PartyLedgerName { get; set; }

        [JsonProperty("vouchernumber")]
        public string VoucherNumber { get; set; }

        [JsonProperty("vouchertypename")]
        public string VoucherTypeName { get; set; }

        [JsonProperty("narration")]
        public string Narration { get; set; }

        [JsonProperty("partygstin")]
        public string PartyGSTIN { get; set; }

        [JsonProperty("placeofsupply")]
        public string PlaceOfSupply { get; set; }

        [JsonProperty("allinventoryentries")]
        public List<InventoryEntry> AllInventoryEntries { get; set; }

        [JsonProperty("ledgerentries")]
        public List<LedgerEntry> LedgerEntries { get; set; }
    }

    public class Metadata
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("remoteid")]
        public string RemoteId { get; set; }

        [JsonProperty("vchkey")]
        public string VchKey { get; set; }

        [JsonProperty("vchtype")]
        public string VchType { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("objview")]
        public string ObjView { get; set; }
    }

    public class InventoryEntry
    {
        [JsonProperty("stockitemname")]
        public string StockItemName { get; set; }

        [JsonProperty("gsthsnname")]
        public string HSN { get; set; }

        [JsonProperty("rate")]
        public string Rate { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("actualqty")]
        public string ActualQty { get; set; }

        [JsonProperty("billedqty")]
        public string BilledQty { get; set; }

        [JsonProperty("batchallocations")]
        public List<BatchAllocation> BatchAllocations { get; set; }

        [JsonProperty("accountingallocations")]
        public List<AccountingAllocation> AccountingAllocations { get; set; }

        [JsonProperty("ratedetails")]
        public List<RateDetail> RateDetails { get; set; }
    }

    public class BatchAllocation
    {
        [JsonProperty("batchname")]
        public string BatchName { get; set; }

        [JsonProperty("orderno")]
        public string OrderNo { get; set; }

        [JsonProperty("orderduedate")]
        public string OrderDueDate { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("actualqty")]
        public string ActualQty { get; set; }

        [JsonProperty("billedqty")]
        public string BilledQty { get; set; }
    }

    public class AccountingAllocation
    {
        [JsonProperty("ledgername")]
        public string LedgerName { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("classrate")]
        public string ClassRate { get; set; }
    }

    public class RateDetail
    {
        [JsonProperty("gstratedutyhead")]
        public string DutyHead { get; set; }

        [JsonProperty("gstrate")]
        public string Rate { get; set; }

        [JsonProperty("gstratevaluationtype")]
        public string EvaluationType { get; set; }
    }

    public class LedgerEntry
    {
        [JsonProperty("ledgername")]
        public string LedgerName { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("ispartyledger")]
        public bool IsPartyLedger { get; set; }

        [JsonProperty("ratedetails")]
        public List<RateDetail> RateDetails { get; set; }
    }


}