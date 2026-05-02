using DocumentFormat.OpenXml.Drawing.Charts;
using Newtonsoft.Json;
using SSMI.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
//using System.Web.Mvc;

namespace SSMI.Controllers.API
{
    [RoutePrefix("api/syncpreclosesalesorder")]
    public class SyncPreCloseSalesOrderController : ApiController
    {
        // GET: SyncPreCloseSalesOrder
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [HttpPost]
        [Route("save")]
        public IHttpActionResult SavePreCloseSalesOrder()
        {
            try
            {
                var rawJson = Request.Content.ReadAsStringAsync().Result;

                var order = GetOrderFromApi(rawJson);

                PreCloseSaveOrder(order);

                return Ok(new
                {
                    Status = true,
                    Message = "Sales Order Pre Closed successfully"
                });
            }
            catch (Newtonsoft.Json.JsonReaderException ex)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    Status = false,
                    Message = ex.Message   // 👈 EXACT error message only
                });
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    Status = false,
                    Message = "An unexpected error occurred."
                });
            }
        }

        public PreCloseSalesOrderData GetOrderFromApi(string rawJson)
        {
            return JsonConvert.DeserializeObject<PreCloseSalesOrderData>(rawJson);
        }

        public void PreCloseSaveOrder(PreCloseSalesOrderData order)
        {
            string _connStr = ConfigurationManager
                .ConnectionStrings["SSMI_DefaultConnection"].ConnectionString;

            using (SqlConnection con = new SqlConnection(_connStr))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();

                try
                {

                    using (SqlCommand hdr = new SqlCommand(@"
                            INSERT INTO Tally_Voucher
                            (RemoteId, VoucherType, VoucherNumber, VoucherDate, PartyName, Party_Addr1, 
                             Party_Addr2, Party_Addr3, Party_Addr4, Party_Addr5, Party_StateDesc, 
                             Party_Pincode, Party_Country, Party_Desp_Addr1, Party_Desp_Addr2, 
                             Party_Desp_Addr3, Party_Desp_Addr4, Party_Desp_Addr5, Party_Desp_StateDesc, 
                             Party_Desp_Pincode, Party_Desp_Country, PartyGSTIN, PlaceOfSupply, 
                             BilltoPlace, ShiptoPlace, BasicOrderRef, BasicShipVesselNo, 
                             BasicBuyersSalesTaxNo, BasicDueDateOfPymt, Narration, TotalAmount, RoundoffAmt, IRN, 
                             IRNAckNo, IRNAckDate, CreatedOn, RegstrId)
                            OUTPUT INSERTED.VoucherId
                            VALUES (@RemoteId, @VoucherType, @VoucherNumber, @VoucherDate, @PartyName, @Party_Addr1, 
                             @Party_Addr2, @Party_Addr3, @Party_Addr4, @Party_Addr5, @Party_StateDesc, 
                             @Party_Pincode, @Party_Country, @Party_Desp_Addr1, @Party_Desp_Addr2, 
                             @Party_Desp_Addr3, @Party_Desp_Addr4, @Party_Desp_Addr5, @Party_Desp_StateDesc, 
                             @Party_Desp_Pincode, @Party_Desp_Country, @PartyGSTIN, @PlaceOfSupply, 
                             @BilltoPlace, @ShiptoPlace, @BasicOrderRef, @BasicShipVesselNo, 
                             @BasicBuyersSalesTaxNo, @BasicDueDateOfPymt, @Narration, @TotalAmount, @RoundoffAmt, @IRN, 
                             @IRNAckNo, @IRNAckDate, @CreatedOn, @RegstrId)", con, tran))
                    {
                        hdr.Parameters.AddWithValue("@RemoteId", "asa");// order.customerInfo.uid);
                        hdr.Parameters.AddWithValue("@VoucherType", "Sales Order");
                        hdr.Parameters.AddWithValue("@VoucherNumber", order.customerInfo.vouchernumber);
                        hdr.Parameters.AddWithValue("@VoucherDate", order.customerInfo.date);
                        hdr.Parameters.AddWithValue("@PartyName", order.customerInfo.customerName);

                        hdr.Parameters.AddWithValue("@Party_Addr1", string.IsNullOrEmpty(order.customerInfo.address1) ? (object)DBNull.Value : order.customerInfo.address1);
                        hdr.Parameters.AddWithValue("@Party_Addr2", string.IsNullOrEmpty(order.customerInfo.address2) ? (object)DBNull.Value : order.customerInfo.address2);
                        hdr.Parameters.AddWithValue("@Party_Addr3", string.IsNullOrEmpty(order.customerInfo.address3) ? (object)DBNull.Value : order.customerInfo.address3);
                        hdr.Parameters.AddWithValue("@Party_Addr4", string.IsNullOrEmpty(order.customerInfo.address4) ? (object)DBNull.Value : order.customerInfo.address4);
                        hdr.Parameters.AddWithValue("@Party_Addr5", string.IsNullOrEmpty(order.customerInfo.address5) ? (object)DBNull.Value : order.customerInfo.address5);
                        //hdr.Parameters.AddWithValue("@Party_Addr1", order.customerInfo.address1);
                        //hdr.Parameters.AddWithValue("@Party_Addr2", order.customerInfo.address2);
                        //hdr.Parameters.AddWithValue("@Party_Addr3", order.customerInfo.address3);
                        //hdr.Parameters.AddWithValue("@Party_Addr4", order.customerInfo.address4);
                        //hdr.Parameters.AddWithValue("@Party_Addr5", order.customerInfo.address5);
                        hdr.Parameters.AddWithValue("@Party_StateDesc", order.customerInfo.statename);
                        hdr.Parameters.AddWithValue("@Party_Pincode", order.customerInfo.partypincode);
                        hdr.Parameters.AddWithValue("@Party_Country", "India");// order.customerInfo.consigneecountryname);
                        //hdr.Parameters.AddWithValue("@Party_Desp_Addr1", order.customerInfo.basicbuyeraddress1);
                        //hdr.Parameters.AddWithValue("@Party_Desp_Addr2", order.customerInfo.basicbuyeraddress2);
                        //hdr.Parameters.AddWithValue("@Party_Desp_Addr3", order.customerInfo.basicbuyeraddress3);
                        //hdr.Parameters.AddWithValue("@Party_Desp_Addr4", order.customerInfo.basicbuyeraddress4);
                        //hdr.Parameters.AddWithValue("@Party_Desp_Addr5", order.customerInfo.basicbuyeraddress5);
                        hdr.Parameters.AddWithValue("@Party_Desp_Addr1", string.IsNullOrEmpty(order.customerInfo.basicbuyeraddress1) ? (object)DBNull.Value : order.customerInfo.basicbuyeraddress1);
                        hdr.Parameters.AddWithValue("@Party_Desp_Addr2", string.IsNullOrEmpty(order.customerInfo.basicbuyeraddress2) ? (object)DBNull.Value : order.customerInfo.basicbuyeraddress2);
                        hdr.Parameters.AddWithValue("@Party_Desp_Addr3", string.IsNullOrEmpty(order.customerInfo.basicbuyeraddress3) ? (object)DBNull.Value : order.customerInfo.basicbuyeraddress3);
                        hdr.Parameters.AddWithValue("@Party_Desp_Addr4", string.IsNullOrEmpty(order.customerInfo.basicbuyeraddress4) ? (object)DBNull.Value : order.customerInfo.basicbuyeraddress4);
                        hdr.Parameters.AddWithValue("@Party_Desp_Addr5", string.IsNullOrEmpty(order.customerInfo.basicbuyeraddress5) ? (object)DBNull.Value : order.customerInfo.basicbuyeraddress5);
                        hdr.Parameters.AddWithValue("@Party_Desp_StateDesc", order.customerInfo.statename);
                        hdr.Parameters.AddWithValue("@Party_Desp_Pincode", order.customerInfo.partypincode);
                        hdr.Parameters.AddWithValue("@Party_Desp_Country", "India");//order.customerInfo.consigneecountryname);
                        //hdr.Parameters.AddWithValue("@PartyGSTIN", order.customerInfo.partygstin);
                        hdr.Parameters.AddWithValue("@PartyGSTIN", string.IsNullOrEmpty(order.customerInfo.partygstin) ? (object)DBNull.Value : order.customerInfo.partygstin);
                        hdr.Parameters.AddWithValue("@PlaceOfSupply", order.customerInfo.placeofsupply);
                        hdr.Parameters.AddWithValue("@BilltoPlace", order.customerInfo.placeofsupply);
                        hdr.Parameters.AddWithValue("@ShiptoPlace", order.customerInfo.placeofsupply);

                        hdr.Parameters.AddWithValue("@BasicOrderRef", order.customerInfo.basicorderref);
                        hdr.Parameters.AddWithValue("@BasicShipVesselNo", "-");
                        hdr.Parameters.AddWithValue("@BasicBuyersSalesTaxNo", "-");
                        hdr.Parameters.AddWithValue("@BasicDueDateOfPymt", order.customerInfo.basicduedateofpymt);
                        //hdr.Parameters.AddWithValue("@Narration", order.customerInfo.narration);
                        hdr.Parameters.AddWithValue("@Narration", string.IsNullOrEmpty(order.customerInfo.narration) ? (object)DBNull.Value : order.customerInfo.narration);
                        hdr.Parameters.AddWithValue("@TotalAmount", order.GrandTotal);
                        hdr.Parameters.AddWithValue("@RoundoffAmt", order.RoundOff);
                        hdr.Parameters.AddWithValue("@IRN", "-");
                        hdr.Parameters.AddWithValue("@IRNAckNo", "-");
                        hdr.Parameters.AddWithValue("@IRNAckDate", DBNull.Value);
                        hdr.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
                        hdr.Parameters.AddWithValue("@RegstrId", 3);

                        long voucherId = (long)hdr.ExecuteScalar();

                        //Stock Item List
                        foreach (var item in order.materialItemsList)
                        {

                            var preqty = string.IsNullOrEmpty(item.OrderPreClosureQty.ToString()) ? 0m : Convert.ToDecimal(item.OrderPreClosureQty);

                            SqlCommand itm = new SqlCommand(@"
                            INSERT INTO Tally_Voucher_Inventory
                            (VoucherId, StockItemName, HSNCode, Quantity, Rate, UnitCode, 
                             Amount, GodownName, BatchName, OrderNo, OrderDueDate, LedgerName, 
                             ClassRate, CGSTExprn, SGSTExprn, IGSTExprn, 
                             OrderPreClosureQty, OrderPreClosureDate, OrderClosureReason)
                            VALUES (@VoucherId,@StockItemName,@HSNCode,@Quantity,@Rate,
                            @UnitCode,@Amount,@GodownName,@BatchName,@OrderNo,@OrderDueDate,
                            @LedgerName,@ClassRate,@CGSTExprn,@SGSTExprn,@IGSTExprn,
                            @OrderPreClosureQty, @OrderPreClosureDate, @OrderClosureReason)", con, tran);

                            itm.Parameters.AddWithValue("@VoucherId", voucherId);
                            itm.Parameters.AddWithValue("@StockItemName", item.stockitemname);
                            //itm.Parameters.AddWithValue("@HSNCode", item.gsthsnname);
                            //itm.Parameters.AddWithValue("@HSNCode",string.IsNullOrEmpty(item.gsthsnname)? (object)DBNull.Value: item.gsthsnname);
                            itm.Parameters.AddWithValue("@HSNCode", string.IsNullOrWhiteSpace(item.gsthsnname) ? "-" : item.gsthsnname);
                            itm.Parameters.AddWithValue("@Quantity", item.billedqty);
                            itm.Parameters.AddWithValue("@Rate", item.rate);
                            itm.Parameters.AddWithValue("@UnitCode", item.unitcode);
                            itm.Parameters.AddWithValue("@Amount", item.amount);
                            itm.Parameters.AddWithValue("@GodownName", item.godownname);
                            itm.Parameters.AddWithValue("@BatchName", item.batchname);
                            itm.Parameters.AddWithValue("@OrderNo", item.orderno);
                            //itm.Parameters.AddWithValue("@OrderDueDate", item.orderduedate);
                            itm.Parameters.AddWithValue("@OrderDueDate", string.IsNullOrEmpty(item.orderduedate) ? (object)DBNull.Value : item.orderduedate);
                            //itm.Parameters.AddWithValue("@LedgerName", order.customerInfo.partygstin);
                            itm.Parameters.AddWithValue("@LedgerName", string.IsNullOrWhiteSpace(order.customerInfo.partygstin) ? "-" : order.customerInfo.partygstin);
                            itm.Parameters.AddWithValue("@ClassRate", item.classrate);
                            itm.Parameters.AddWithValue("@CGSTExprn", item.cgstexprn);
                            itm.Parameters.AddWithValue("@SGSTExprn", item.sgstexprn);
                            itm.Parameters.AddWithValue("@IGSTExprn", item.igstexprn);

                            itm.Parameters.AddWithValue("@OrderPreClosureQty", preqty);
                            itm.Parameters.AddWithValue("@OrderPreClosureDate", string.IsNullOrEmpty(item.OrderPreClosureDate) ? (object)DBNull.Value : item.OrderPreClosureDate);
                            itm.Parameters.AddWithValue("@OrderClosureReason", string.IsNullOrEmpty(item.OrderClosureReason) ? (object)DBNull.Value : item.OrderClosureReason);

                            itm.ExecuteNonQuery();
                        }

                        //Party Ledger Details
                        //foreach (var lc_item in order.loadingchargesList)
                        //{
                        SqlCommand pld = new SqlCommand(@"
                            INSERT INTO Tally_Voucher_Ledger
                            (VoucherId, LedgerName, IsPartyLedger, IsTaxLedger, Amount)
                            VALUES (@VoucherId,@LedgerName,@IsPartyLedger,@IsTaxLedger,@Amount)", con, tran);

                        pld.Parameters.AddWithValue("@VoucherId", voucherId);
                        //pld.Parameters.AddWithValue("@LedgerName", order.loadingCharges.ldgrname);
                        //pld.Parameters.AddWithValue("@LedgerName",order.loadingCharges?.ldgrname == null? (object)DBNull.Value: order.loadingCharges.ldgrname);
                        pld.Parameters.AddWithValue("@LedgerName", string.IsNullOrWhiteSpace(order.customerInfo.partyname) ? "-" : order.customerInfo.partyname);
                        pld.Parameters.AddWithValue("@IsPartyLedger", 1); //0 - "No" 1 - "Yes"
                        pld.Parameters.AddWithValue("@IsTaxLedger", 0);
                        pld.Parameters.AddWithValue("@Amount", order.GrandTotal);
                        pld.ExecuteNonQuery();
                        //}


                        //Loading Charges
                        //foreach (var lc_item in order.loadingchargesList)
                        //{
                        if (order.loadingCharges.amount > 0)
                        {
                            SqlCommand lcl = new SqlCommand(@"
                            INSERT INTO Tally_Voucher_Ledger
                            (VoucherId, LedgerName, IsPartyLedger, IsTaxLedger, Amount)
                            VALUES (@VoucherId,@LedgerName,@IsPartyLedger,@IsTaxLedger,@Amount)", con, tran);

                            lcl.Parameters.AddWithValue("@VoucherId", voucherId);
                            //lcl.Parameters.AddWithValue("@LedgerName", order.loadingCharges.ldgrname);
                            //lcl.Parameters.AddWithValue("@LedgerName",order.loadingCharges?.ldgrname == null? (object)DBNull.Value: order.loadingCharges.ldgrname);
                            lcl.Parameters.AddWithValue("@LedgerName", string.IsNullOrWhiteSpace(order.loadingCharges?.ldgrname) ? "-" : order.loadingCharges.ldgrname);
                            lcl.Parameters.AddWithValue("@IsPartyLedger", 0); //0 - "No" 1 - "Yes"
                            lcl.Parameters.AddWithValue("@IsTaxLedger", 0);
                            lcl.Parameters.AddWithValue("@Amount", order.loadingCharges.amount);
                            lcl.ExecuteNonQuery();
                        }
                        //}

                        //Transport Charges
                        //foreach (var tp_item in order.transportchargesList)
                        //{
                        //if (order.transportationCharges.amount > 0)
                        //{
                        //    SqlCommand tpc = new SqlCommand(@"
                        //    INSERT INTO Tally_Voucher_Ledger
                        //    (VoucherId, LedgerName, IsPartyLedger, Amount)
                        //    VALUES (@VoucherId,@LedgerName,@IsPartyLedger,@Amount)", con, tran);

                        //    tpc.Parameters.AddWithValue("@VoucherId", voucherId);
                        //    //tpc.Parameters.AddWithValue("@LedgerName", order.transportationCharges.ldgrname);
                        //    tpc.Parameters.AddWithValue("@LedgerName", string.IsNullOrWhiteSpace(order.transportationCharges?.ldgrname) ? "-" : order.transportationCharges.ldgrname);
                        //    tpc.Parameters.AddWithValue("@IsPartyLedger", 0); //0 - "No" 1 - "Yes"
                        //    tpc.Parameters.AddWithValue("@Amount", order.transportationCharges.amount);
                        //    tpc.ExecuteNonQuery();
                        //}
                        //}

                        decimal cgstamt = 0;
                        if (order.cgst.amount != null)
                        {
                            cgstamt = Convert.ToDecimal(order.cgst.amount);
                        }
                        //CGST
                        if (cgstamt > 0)//if (order.cgst.amount > 0)
                        {
                            SqlCommand lcl = new SqlCommand(@"
                            INSERT INTO Tally_Voucher_Ledger
                            (VoucherId, LedgerName, IsPartyLedger, IsTaxLedger, Amount)
                            VALUES (@VoucherId,@LedgerName,@IsPartyLedger,@IsTaxLedger,@Amount)", con, tran);

                            lcl.Parameters.AddWithValue("@VoucherId", voucherId);
                            lcl.Parameters.AddWithValue("@LedgerName", string.IsNullOrWhiteSpace(order.cgst?.ldgrname) ? "-" : order.cgst.ldgrname);
                            lcl.Parameters.AddWithValue("@IsPartyLedger", 0); //0 - "No" 1 - "Yes"
                            lcl.Parameters.AddWithValue("@IsTaxLedger", 1);
                            lcl.Parameters.AddWithValue("@Amount", order.cgst.amount);
                            lcl.ExecuteNonQuery();
                        }

                        //SGST
                        if (order.sgst.amount > 0)
                        {
                            SqlCommand lcl = new SqlCommand(@"
                            INSERT INTO Tally_Voucher_Ledger
                            (VoucherId, LedgerName, IsPartyLedger, IsTaxLedger, Amount)
                            VALUES (@VoucherId,@LedgerName,@IsPartyLedger,@IsTaxLedger,@Amount)", con, tran);

                            lcl.Parameters.AddWithValue("@VoucherId", voucherId);
                            lcl.Parameters.AddWithValue("@LedgerName", string.IsNullOrWhiteSpace(order.sgst?.ldgrname) ? "-" : order.sgst.ldgrname);
                            lcl.Parameters.AddWithValue("@IsPartyLedger", 0); //0 - "No" 1 - "Yes"
                            lcl.Parameters.AddWithValue("@IsTaxLedger", 1);
                            lcl.Parameters.AddWithValue("@Amount", order.sgst.amount);
                            lcl.ExecuteNonQuery();
                        }

                        //IGST
                        if (order.igst.amount > 0)
                        {
                            SqlCommand lcl = new SqlCommand(@"
                            INSERT INTO Tally_Voucher_Ledger
                            (VoucherId, LedgerName, IsPartyLedger, IsTaxLedger, Amount)
                            VALUES (@VoucherId,@LedgerName,@IsPartyLedger,@IsTaxLedger,@Amount)", con, tran);

                            lcl.Parameters.AddWithValue("@VoucherId", voucherId);
                            lcl.Parameters.AddWithValue("@LedgerName", string.IsNullOrWhiteSpace(order.igst?.ldgrname) ? "-" : order.igst.ldgrname);
                            lcl.Parameters.AddWithValue("@IsPartyLedger", 0); //0 - "No" 1 - "Yes"
                            lcl.Parameters.AddWithValue("@IsTaxLedger", 0);
                            lcl.Parameters.AddWithValue("@IsTaxLedger", 1);
                            lcl.Parameters.AddWithValue("@Amount", order.igst.amount);
                            lcl.ExecuteNonQuery();
                        }

                        //Rounding Off
                        if (order.RoundOff != 0)
                        {
                            SqlCommand tpc = new SqlCommand(@"
                            INSERT INTO Tally_Voucher_Ledger
                            (VoucherId, LedgerName, IsPartyLedger, IsTaxLedger, Amount)
                            VALUES (@VoucherId,@LedgerName,@IsPartyLedger,@IsTaxLedger,@Amount)", con, tran);

                            tpc.Parameters.AddWithValue("@VoucherId", voucherId);
                            //tpc.Parameters.AddWithValue("@LedgerName", order.transportationCharges.ldgrname);
                            tpc.Parameters.AddWithValue("@LedgerName", "Rounding Off");
                            tpc.Parameters.AddWithValue("@IsPartyLedger", 0); //0 - "No" 1 - "Yes"
                            tpc.Parameters.AddWithValue("@IsTaxLedger", 2);
                            tpc.Parameters.AddWithValue("@Amount", order.RoundOff);
                            tpc.ExecuteNonQuery();
                        }


                        //hdr.ExecuteNonQuery();
                        tran.Commit();
                    }



                    // tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

    }
}