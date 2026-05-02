using DocumentFormat.OpenXml.Office.Word;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SSMI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SSMI.Controllers.API
{
    public class SSMIAPIController : Controller
    {
        // GET: SSMIAPI
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult stockgroupDetails()
        {
            SqlDataReader reader = null;
            string _connStr = ConfigurationManager.ConnectionStrings["SSMI_DefaultConnection"].ConnectionString;
            SqlConnection myConnection = new SqlConnection(_connStr);

            var strsql = "";
            strsql = "Select * from StockGroupMaster Where MDispStatus = 0";

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = strsql;// "Select * from StateMaster Where StateId = " + id;
            sqlCmd.Connection = myConnection;
            myConnection.Open();
            reader = sqlCmd.ExecuteReader();

            List<StockGroupMaster> stockgrouplist = new List<StockGroupMaster>();
            while (reader.Read())
            {
                stockgrouplist.Add(new StockGroupMaster
                {
                    Guid = reader["Guid"].ToString(),
                    StockGroupParent = reader["StockGroupParent"].ToString(),
                    StockGroupName = reader["StockGroupName"].ToString(),
                    HSNCode = reader["HSNCode"].ToString(),
                    CgstExprn = Convert.ToDecimal(reader["CgstExprn"]),
                    SgstExprn = Convert.ToDecimal(reader["SgstExprn"]),
                    IgstExprn = Convert.ToDecimal(reader["IgstExprn"]),
                    AlterId = reader["AlterId"].ToString(),
                });
            }


            var collectionWrapper = new
            {

                stockgroup = stockgrouplist

            };

            var output = JsonConvert.SerializeObject(collectionWrapper);

            myConnection.Close();
            return Content(output);




        }

        public ActionResult stockitemDetails()
        {
            SqlDataReader reader = null;
            string _connStr = ConfigurationManager.ConnectionStrings["SSMI_DefaultConnection"].ConnectionString;
            SqlConnection myConnection = new SqlConnection(_connStr);

            var strsql = "";
            strsql = "Select * from StockItemMaster Where MDispStatus = 0";

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = strsql;// "Select * from StateMaster Where StateId = " + id;
            sqlCmd.Connection = myConnection;
            myConnection.Open();
            reader = sqlCmd.ExecuteReader();

            List<StockItemMaster> stockitemlist = new List<StockItemMaster>();
            while (reader.Read())
            {
                stockitemlist.Add(new StockItemMaster
                {
                    Guid = reader["Guid"].ToString(),
                    StockItemParent = reader["StockItemParent"].ToString(),
                    StockItemName = reader["StockItemName"].ToString(),
                    BaseUnit = reader["BaseUnit"].ToString(),
                    HSNCode = reader["HSNCode"].ToString(),
                    CgstExprn = Convert.ToDecimal(reader["CgstExprn"]),
                    SgstExprn = Convert.ToDecimal(reader["SgstExprn"]),
                    IgstExprn = Convert.ToDecimal(reader["IgstExprn"]),
                    AlterUnitName = reader["AlterUnitName"].ToString(),
                    Denominator = reader["Denominator"].ToString(),
                    Conversion = reader["Conversion"].ToString(),
                    AlterId = reader["AlterId"].ToString()
                });
            }


            var collectionWrapper = new
            {

                stockitem = stockitemlist

            };

            var output = JsonConvert.SerializeObject(collectionWrapper);

            myConnection.Close();
            return Content(output);




        }

        public ActionResult ledgerdetails()
        {
            SqlDataReader reader = null;
            string _connStr = ConfigurationManager.ConnectionStrings["SSMI_DefaultConnection"].ConnectionString;
            SqlConnection myConnection = new SqlConnection(_connStr);

            var strsql = "";
            //strsql = "Select * from LedgerMaster Where MDispStatus = 0";
            strsql = "Select * from LedgerMaster Where MDispStatus = 0 And  (LegerGroupName LIKE 'Sundry Debtors%')";

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = strsql;// "Select * from StateMaster Where StateId = " + id;
            sqlCmd.Connection = myConnection;
            myConnection.Open();
            reader = sqlCmd.ExecuteReader();

            List<LedgerMaster> ledgerlist = new List<LedgerMaster>();
            while (reader.Read())
            {
                ledgerlist.Add(new LedgerMaster
                {
                    Guid = reader["Guid"].ToString(),
                    LegerGroupName = reader["LegerGroupName"].ToString(),
                    LegerName = reader["LegerName"].ToString(),
                    Address1 = reader["Address1"].ToString(),
                    Address2 = reader["Address1"].ToString(),
                    Address3 = reader["Address3"].ToString(),
                    Address4 = reader["Address4"].ToString(),
                    Address5 = reader["Address5"].ToString(),
                    Address6 = reader["Address6"].ToString(),
                    StateName = reader["StateName"].ToString(),
                    CountryName = reader["CountryName"].ToString(),
                    Pincode = reader["Pincode"].ToString(),
                    EmailId = reader["EmailId"].ToString(),
                    Website = reader["Website"].ToString(),
                    PanNo = reader["PanNo"].ToString(),
                    GSTinNo = reader["GSTinNo"].ToString(),
                    LedgerContactName = reader["LedgerContactName"].ToString(),
                    LedgerContactNo = reader["LedgerContactNo"].ToString(),
                    CreditPeriod = reader["CreditPeriod"].ToString(),
                    CreditLimit = reader["CreditLimit"].ToString(),
                    //OpeningBalance = reader["OpeningBalance"].ToString(),
                    AlterId = reader["AlterId"].ToString()
                });
            }


            var collectionWrapper = new
            {

                Ledger = ledgerlist

            };

            var output = JsonConvert.SerializeObject(collectionWrapper);

            myConnection.Close();
            return Content(output);

        }

        public ActionResult ledgerclosingdetails()
        {
            SqlDataReader reader = null;
            string _connStr = ConfigurationManager.ConnectionStrings["SSMI_DefaultConnection"].ConnectionString;
            SqlConnection myConnection = new SqlConnection(_connStr);

            var strsql = "";
            //strsql = "Select * from LedgerMaster Where MDispStatus = 0";
            strsql = "Select * from Ledger_Closing_Balance Where MDispStatus = 0";

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = strsql;// "Select * from StateMaster Where StateId = " + id;
            sqlCmd.Connection = myConnection;
            myConnection.Open();
            reader = sqlCmd.ExecuteReader();

            List<LedgerClosingDetails> ledgerclosinglist = new List<LedgerClosingDetails>();
            while (reader.Read())
            {
                ledgerclosinglist.Add(new LedgerClosingDetails
                {
                    Guid = reader["Guid"].ToString(),
                    LegerGroupName = reader["LegerGroupName"].ToString(),
                    LegerName = reader["LegerName"].ToString(),
                    ClosingBalance = reader["ClosingBalance"].ToString()
                });
            }


            var collectionWrapper = new
            {

                Ledger = ledgerclosinglist

            };

            var output = JsonConvert.SerializeObject(collectionWrapper);

            myConnection.Close();
            return Content(output);

        }

        public ActionResult unitDetails ()
        {
            SqlDataReader reader = null;
            string _connStr = ConfigurationManager.ConnectionStrings["SSMI_DefaultConnection"].ConnectionString;
            SqlConnection myConnection = new SqlConnection(_connStr);

            var strsql = "";
            strsql = "Select * from UnitMaster Where MDispStatus = 0";

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = strsql;// "Select * from StateMaster Where StateId = " + id;
            sqlCmd.Connection = myConnection;
            myConnection.Open();
            reader = sqlCmd.ExecuteReader();

            List<StockUnitMaster> unitlist = new List<StockUnitMaster>();
            while (reader.Read())
            {
                unitlist.Add(new StockUnitMaster
                {
                    Guid = reader["Guid"].ToString(),
                    UnitName = reader["UnitName"].ToString(),
                    UnitDecimal = reader["UnitDecimal"].ToString(),
                    AlternateUnitName = reader["AlterUnitName"].ToString(),
                    AlterId = reader["AlterId"].ToString()
                });
            }


            var collectionWrapper = new
            {

                stockunit = unitlist

            };

            var output = JsonConvert.SerializeObject(collectionWrapper);

            myConnection.Close();
            return Content(output);


        }

        public ActionResult stockgroupstatusupdate()
        {
            try
            {
                string _connStr = ConfigurationManager
                                    .ConnectionStrings["SSMI_DefaultConnection"]
                                    .ConnectionString;

                using (SqlConnection myConnection = new SqlConnection(_connStr))
                using (SqlCommand sqlCmd = new SqlCommand("pr_Stock_Group_Master_Status_Update", myConnection))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    myConnection.Open();
                    sqlCmd.ExecuteNonQuery();
                }

                return Json(new
                {
                    success = true,
                    message = "Stock group status updated successfully"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult stockitemstatusupdate()
        {
            try
            {
                string _connStr = ConfigurationManager
                                    .ConnectionStrings["SSMI_DefaultConnection"]
                                    .ConnectionString;

                using (SqlConnection myConnection = new SqlConnection(_connStr))
                using (SqlCommand sqlCmd = new SqlCommand("pr_Stock_Item_Master_Status_Update", myConnection))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    myConnection.Open();
                    sqlCmd.ExecuteNonQuery();
                }

                return Json(new
                {
                    success = true,
                    message = "Stock Item status updated successfully"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult stockunitstatusupdate()
        {
            try
            {
                string _connStr = ConfigurationManager
                                    .ConnectionStrings["SSMI_DefaultConnection"]
                                    .ConnectionString;

                using (SqlConnection myConnection = new SqlConnection(_connStr))
                using (SqlCommand sqlCmd = new SqlCommand("pr_Stock_Group_Master_Status_Update", myConnection))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    myConnection.Open();
                    sqlCmd.ExecuteNonQuery();
                }

                return Json(new
                {
                    success = true,
                    message = "Stock group status updated successfully"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ledgerstatusupdate()
        {
            try
            {
                string _connStr = ConfigurationManager
                                    .ConnectionStrings["SSMI_DefaultConnection"]
                                    .ConnectionString;

                using (SqlConnection myConnection = new SqlConnection(_connStr))
                using (SqlCommand sqlCmd = new SqlCommand("pr_Stock_Ledger_Master_Status_Update", myConnection))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    myConnection.Open();
                    sqlCmd.ExecuteNonQuery();
                }

                return Json(new
                {
                    success = true,
                    message = "Ledger status updated successfully"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        //Sales Invoice Json
        public ActionResult deletesalesinvoicedetails()
        {
            SqlDataReader reader = null;
            string _connStr = ConfigurationManager.ConnectionStrings["SSMI_DefaultConnection"].ConnectionString;
            SqlConnection myConnection = new SqlConnection(_connStr);

            var strsql = "";
            //strsql = "Select * from LedgerMaster Where MDispStatus = 0";
            strsql = "Select * from Tally_Voucher Where RegstrId = 2 And SyncStatus = 0";

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = strsql;// "Select * from StateMaster Where StateId = " + id;
            sqlCmd.Connection = myConnection;
            myConnection.Open();
            reader = sqlCmd.ExecuteReader();

            List<LedgerMaster> ledgerlist = new List<LedgerMaster>();
            while (reader.Read())
            {
                ledgerlist.Add(new LedgerMaster
                {
                    Guid = reader["Guid"].ToString(),
                    LegerGroupName = reader["LegerGroupName"].ToString(),
                    LegerName = reader["LegerName"].ToString(),
                    Address1 = reader["Address1"].ToString(),
                    Address2 = reader["Address1"].ToString(),
                    Address3 = reader["Address3"].ToString(),
                    Address4 = reader["Address4"].ToString(),
                    Address5 = reader["Address5"].ToString(),
                    Address6 = reader["Address6"].ToString(),
                    StateName = reader["StateName"].ToString(),
                    CountryName = reader["CountryName"].ToString(),
                    Pincode = reader["Pincode"].ToString(),
                    EmailId = reader["EmailId"].ToString(),
                    Website = reader["Website"].ToString(),
                    PanNo = reader["PanNo"].ToString(),
                    GSTinNo = reader["GSTinNo"].ToString(),
                    LedgerContactName = reader["LedgerContactName"].ToString(),
                    LedgerContactNo = reader["LedgerContactNo"].ToString(),
                    CreditPeriod = reader["CreditPeriod"].ToString(),
                    //OpeningBalance = reader["OpeningBalance"].ToString(),
                    AlterId = reader["AlterId"].ToString()
                });
            }


            var collectionWrapper = new
            {

                Ledger = ledgerlist

            };

            var output = JsonConvert.SerializeObject(collectionWrapper);

            myConnection.Close();
            return Content(output);

        }

        public ActionResult SalesInvoiceDetails()
        {
            string connStr = ConfigurationManager.ConnectionStrings["SSMI_DefaultConnection"].ConnectionString;

            List<SalesInvoiceRoot> invoiceList = new List<SalesInvoiceRoot>();

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                // =====================
                // GET ALL VOUCHERS
                // =====================
                SqlCommand cmd = new SqlCommand(@"
                                SELECT * 
                                FROM Tally_Voucher 
                                WHERE RegstrId = 2 AND SyncStatus = 0
                            ", con);

                SqlDataReader reader = cmd.ExecuteReader();

                List<dynamic> vouchers = new List<dynamic>();

                while (reader.Read())
                {
                    vouchers.Add(new
                    {
                        VoucherId = reader["VoucherId"],
                        //OrderID = reader["OrderID"].ToString(),
                        Guid = reader["RemoteId"].ToString(),
                        CustomerCode = "000",// reader["CustomerCode"].ToString(),
                        CustomerName = reader["PartyName"].ToString(),
                        Address1 = reader["Party_Addr1"].ToString(),
                        Address2 = reader["Party_Addr2"].ToString(),
                        Address3 = reader["Party_Addr3"].ToString(),
                        Address4 = reader["Party_Addr4"].ToString(),
                        Address5 = reader["Party_Addr5"].ToString(),
                        BasicBuyerAddress1 = reader["Party_Desp_Addr1"].ToString(),
                        BasicBuyerAddress2 = reader["Party_Desp_Addr2"].ToString(),
                        BasicBuyerAddress3 = reader["Party_Desp_Addr3"].ToString(),
                        BasicBuyerAddress4 = reader["Party_Desp_Addr4"].ToString(),
                        BasicBuyerAddress5 = reader["Party_Desp_Addr5"].ToString(),
                        StateName = reader["Party_StateDesc"].ToString(),
                        Narration = reader["Narration"].ToString(),
                        CountryName = reader["Party_Country"].ToString(),
                        GSTIN = reader["PartyGSTIN"].ToString(),
                        PlaceOfSupply = reader["PlaceOfSupply"].ToString(),
                        VoucherNo = reader["VoucherNumber"].ToString(),
                        Pincode = reader["Party_Pincode"].ToString(),
                        VoucherDate = Convert.ToDateTime(reader["VoucherDate"]).ToString("yyyy-MM-dd"),
                        BasicOrderRef = reader["BasicOrderRef"].ToString(),
                        BasicDueDateOfPymt = reader["BasicDueDateOfPymt"].ToString(),
                        TotalAmount = reader["TotalAmount"].ToString()
                    });
                }

                reader.Close();


                // =====================
                // LOOP EACH VOUCHER
                // =====================
                foreach (var v in vouchers)
                {
                    SalesInvoiceRoot invoice = new SalesInvoiceRoot();

                    #region CUSTOMER INFO

                    invoice.customerInfo = new CustomerInfo
                    {
                        orderID = "1234567",
                        uid = v.Guid,
                        customerCode = v.CustomerCode,
                        customerName = v.CustomerName,
                        address1 = v.Address1,
                        address2 = v.Address2,
                        address3 = v.Address3,
                        address4 = v.Address4,
                        address5 = v.Address5,
                        basicbuyeraddress1 = v.BasicBuyerAddress1,
                        basicbuyeraddress2 = v.BasicBuyerAddress2,
                        basicbuyeraddress3 = v.BasicBuyerAddress3,
                        basicbuyeraddress4 = v.BasicBuyerAddress4,
                        basicbuyeraddress5 = v.BasicBuyerAddress5,
                        date = Convert.ToDateTime(v.VoucherDate),
                        statename = v.StateName,
                        narration = v.Narration,
                        countryofresidence = v.CountryName,
                        partygstin = v.GSTIN,
                        placeofsupply = v.PlaceOfSupply,
                        partyname = v.CustomerName,
                        vouchernumber = v.VoucherNo,
                        partypincode = v.Pincode,
                        consigneegstin = v.GSTIN,
                        consigneestatename = v.StateName,
                        consigneecountryname = v.CountryName,
                        basicorderref = v.BasicOrderRef,
                        basicduedateofpymt = v.BasicDueDateOfPymt
                    };

                    #endregion


                    #region MATERIAL ITEMS

                    List<MaterialItem> itemList = new List<MaterialItem>();

                    SqlCommand itemCmd = new SqlCommand(@"
                                        SELECT * 
                                        FROM Tally_Voucher_Inventory 
                                        WHERE VoucherId = @VoucherId
                                    ", con);

                    itemCmd.Parameters.AddWithValue("@VoucherId", v.VoucherId);

                    SqlDataReader itemReader = itemCmd.ExecuteReader();

                    while (itemReader.Read())
                    {
                        itemList.Add(new MaterialItem
                        {
                            stockitemname = itemReader["StockItemName"].ToString(),
                            gsthsnname = itemReader["HSNCode"].ToString(),
                            rate = Convert.ToDecimal(itemReader["Rate"]),
                            amount = Convert.ToDecimal(itemReader["Amount"]),
                            actualqty = Convert.ToDecimal(itemReader["Quantity"]),
                            billedqty = Convert.ToDecimal(itemReader["Quantity"]),
                            godownname = itemReader["GodownName"].ToString(),
                            batchname = itemReader["BatchName"].ToString(),
                            destinationgodownname = itemReader["GodownName"].ToString(),
                            orderno = itemReader["OrderNo"].ToString(),
                            orderduedate = itemReader["OrderDueDate"].ToString(),
                            cgstexprn = Convert.ToDecimal(itemReader["CGSTExprn"]),
                            sgstexprn = Convert.ToDecimal(itemReader["SGSTExprn"]),
                            igstexprn = Convert.ToDecimal(itemReader["IGSTExprn"])
                        });
                    }

                    itemReader.Close();

                    invoice.materialItemsList = itemList;

                    #endregion

                    #region LEDGER DETAILS

                    List<Tally_Ledger_Details> ldgrList = new List<Tally_Ledger_Details>();

                    SqlCommand ldgrCmd = new SqlCommand(@"
                                        SELECT * 
                                        FROM VW_TALLY_VOUCHER_LEDGER 
                                        WHERE VoucherId = @VoucherId
                                    ", con);
                    ldgrCmd.Parameters.AddWithValue("@VoucherId", v.VoucherId);

                    SqlDataReader ldgrReader = ldgrCmd.ExecuteReader();

                    while (ldgrReader.Read())
                    {
                        bool isPartyLedger = Convert.ToBoolean(ldgrReader["IsPartyLedger"]);
                        int isTaxLedger = Convert.ToInt32(ldgrReader["IsTaxLedger"]);
                        
                        if (isPartyLedger == false && isTaxLedger == 0)
                        {

                        }
                        else
                        {
                            ldgrList.Add(new Tally_Ledger_Details
                            {
                                LdgrName = ldgrReader["LedgerName"].ToString(),
                                IsPartyLedger = ldgrReader["IsPartyLedger"].ToString(),
                                amount = Convert.ToDecimal(ldgrReader["Amount"])
                            });
                        }

                    }

                    ldgrReader.Close();

                    invoice.tallyLedgerList = ldgrList;

                    #endregion

                    #region LOADING CHAGES DETAILS

                    LoadingCharges loadList = new LoadingCharges();

                    SqlCommand loadCmd = new SqlCommand(@"
                                        SELECT * 
                                        FROM VW_TALLY_VOUCHER_LEDGER 
                                        WHERE VoucherId = @VoucherId
                                    ", con);
                    loadCmd.Parameters.AddWithValue("@VoucherId", v.VoucherId);

                    SqlDataReader loadReader = loadCmd.ExecuteReader();

                    while (loadReader.Read())
                    {
                        bool isPartyLedger = Convert.ToBoolean(loadReader["IsPartyLedger"]);
                        int isTaxLedger = Convert.ToInt32(loadReader["IsTaxLedger"]);

                        if (isPartyLedger == false && isTaxLedger == 0)
                        {
                            loadList = new LoadingCharges
                            {
                                ldgrname = loadReader["LedgerName"].ToString(),
                                amount = Convert.ToDecimal(loadReader["Amount"]),
                                cgstexprn = Convert.ToDecimal(loadReader["CGSTExprn"]),
                                sgstexprn = Convert.ToDecimal(loadReader["SGSTExprn"]),
                                igstexprn = Convert.ToDecimal(loadReader["IGSTExprn"]),
                                total = Math.Round(Convert.ToDecimal(loadReader["TotalAmount"]),2)
                            };
                        }


                    }

                    loadReader.Close();

                    invoice.loadingCharges = loadList;

                    #endregion

                    //roff

                    #region ROUNDING OFF DETAILS

                    decimal roffamt = 0;
                    SqlCommand roffCmd = new SqlCommand(@"
                                        SELECT * 
                                        FROM Tally_Voucher_Ledger 
                                        WHERE LedgerName = 'Rounding Off' And VoucherId = @VoucherId
                                    ", con);
                    roffCmd.Parameters.AddWithValue("@VoucherId", v.VoucherId);

                    SqlDataReader roffReader = roffCmd.ExecuteReader();

                    while (roffReader.Read())
                    {
                        roffamt = Convert.ToDecimal(roffReader["Amount"]);

                    }

                    roffReader.Close();

                    invoice.roundOff = Convert.ToDecimal(roffamt);

                    #endregion



                    #region GRAND TOTAL

                    //decimal itemTotal = itemList.Sum(x => x.amount);

                    invoice.grandTotal = Convert.ToDecimal(v.TotalAmount);//  invoice.grandTotal;

                    #endregion

                    invoiceList.Add(invoice);
                }
            }
            //var collectionWrapper = new
            //{

            //    stockunit = unitlist

            //};

            //var output = JsonConvert.SerializeObject(collectionWrapper);
            // =====================
            // WRAP FINAL OUTPUT
            // =====================
            var  wrapper = new 
            {
                salesInvoices = invoiceList
            };

            string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);

            return Content(json, "application/json");
        }


        //end

        // 🔹 MAIN ACTION
        public async Task<ActionResult> SyncSalesOrder()
        {
            var order = await GetOrderFromApi();

            if (order != null && order.customerInfo != null)
            {
                SaveOrder(order);

                return Json(new
                {
                    status = true,
                    message = "Order synced successfully"
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                status = false,
                message = "Invalid order data"
            }, JsonRequestBehavior.AllowGet);
        }


        public async Task<SalesOrderData> GetOrderFromApi()
        {
            string json = "";// "... your JSON ...";
            json = "{\r\n  \"customerInfo\": {\r\n    \"orderID\":\"1014700SO2503509\",\r\n    \"uid\": \"8d572f80-aaa4-11d9-9f54-008048130853-000a625c\",\r\n    \"customerCode\": \"1000\",\r\n    \"customerName\": \"Kumar Agencies\",\r\n    \"address1\": \"No 12, Anna Nagar\",\r\n    \"address2\": \"West Extension\",\r\n    \"address3\": \"Near Bus Stand\",\r\n    \"address4\": \"Chennai\",\r\n    \"address5\": \"Tamil Nadu\",\r\n    \"basicbuyeraddress1\": \"24/4B, SUNDARAJANPATTI\",\r\n    \"basicbuyeraddress2\": \"MADURAI\",\r\n    \"basicbuyeraddress3\": \"625107\",\r\n    \"basicbuyeraddress4\": \"TAMILNADU\",\r\n    \"basicbuyeraddress5\": \"\",\r\n    \"date\": \"2025-04-15\",\r\n    \"statename\": \"Tamil Nadu\",\r\n    \"narration\": \"Ashok\",\r\n    \"countryofresidence\": \"India\",\r\n    \"partygstin\": \"33ASNPR1514M1ZH\",\r\n    \"placeofsupply\": \"Tamil Nadu\",\r\n    \"partyname\": \"SRI RENGA STEELS\",\r\n    \"vouchernumber\": \"4743/SSPL/25-26\",\r\n    \"partypincode\": \"625007\",\r\n    \"consigneegstin\": \"33ASNPR1514M1ZH\",\r\n    \"consigneestatename\": \"Tamil Nadu\",\r\n    \"consigneecountryname\": \"India\",\r\n    \"basicorderref\": \"550 D CRS\",\r\n    \"basicduedateofpymt\": \"10 DAYS\"\r\n  },\r\n\r\n  \"materialItemsList\": [\r\n    {\r\n      \"stockitemname\": \"BD TMT Bar 8MM 550D CRS\",\r\n      \"gsthsnname\": \"72141090\",\r\n      \"rate\": 46300.00,\r\n      \"amount\": 138900.00,\r\n      \"actualqty\": 3.0,\r\n      \"billedqty\": 3.0,\r\n      \"godownname\": \"Pondicherry\",\r\n      \"batchname\": \"Primary Batch\",\r\n      \"destinationgodownname\": \"Pondicherry\",\r\n      \"indentno\": \"\",\r\n      \"orderno\": \"4743/SSPL/25-26\",\r\n      \"orderduedate\": \"2025-12-16\",\r\n      \"CGST\": 9,\r\n      \"SGST_UTGST\": 9,\r\n      \"IGST\": 18\r\n    },\r\n    {\r\n      \"stockitemname\": \"BD TMT Bar 10MM 550D CRS\",\r\n      \"gsthsnname\": \"72141090\",\r\n      \"rate\": 45800.00,\r\n      \"amount\": 137400.00,\r\n      \"actualqty\": 3.0,\r\n      \"billedqty\": 3.0,\r\n      \"godownname\": \"Pondicherry\",\r\n      \"batchname\": \"Primary Batch\",\r\n      \"destinationgodownname\": \"Pondicherry\",\r\n      \"indentno\": \"\",\r\n      \"orderno\": \"4743/SSPL/25-26\",\r\n      \"orderduedate\": \"2025-12-16\",\r\n      \"CGST\": 9,\r\n      \"SGST_UTGST\": 9,\r\n      \"IGST\": 18\r\n    },\r\n    {\r\n      \"stockitemname\": \"BD TMT Bar 12MM 550D CRS\",\r\n      \"gsthsnname\": \"72141090\",\r\n      \"rate\": 45800.00,\r\n      \"amount\": 91600.00,\r\n      \"actualqty\": 2.0,\r\n      \"billedqty\": 2.0,\r\n      \"godownname\": \"Pondicherry\",\r\n      \"batchname\": \"Primary Batch\",\r\n      \"destinationgodownname\": \"Pondicherry\",\r\n      \"indentno\": \"\",\r\n      \"orderno\": \"4743/SSPL/25-26\",\r\n      \"orderduedate\": \"2025-12-16\",\r\n      \"CGST\": 9,\r\n      \"SGST_UTGST\": 9,\r\n      \"IGST\": 18\r\n    },\r\n    {\r\n      \"stockitemname\": \"BD TMT Bar 16MM 550D CRS\",\r\n      \"gsthsnname\": \"72141090\",\r\n      \"rate\": 45800.00,\r\n      \"amount\": 137400.00,\r\n      \"actualqty\": 3.0,\r\n      \"billedqty\": 3.0,\r\n      \"godownname\": \"Pondicherry\",\r\n      \"batchname\": \"Primary Batch\",\r\n      \"destinationgodownname\": \"Pondicherry\",\r\n      \"indentno\": \"\",\r\n      \"orderno\": \"4743/SSPL/25-26\",\r\n      \"orderduedate\": \"2025-12-16\",\r\n      \"CGST\": 9,\r\n      \"SGST_UTGST\": 9,\r\n      \"IGST\": 18\r\n    },\r\n    {\r\n      \"stockitemname\": \"BD TMT Bar 20MM 550D CRS\",\r\n      \"gsthsnname\": \"72141090\",\r\n      \"rate\": 45800.00,\r\n      \"amount\": 91600.00,\r\n      \"actualqty\": 2.0,\r\n      \"billedqty\": 2.0,\r\n      \"godownname\": \"Pondicherry\",\r\n      \"batchname\": \"Primary Batch\",\r\n      \"destinationgodownname\": \"Pondicherry\",\r\n      \"indentno\": \"\",\r\n      \"orderno\": \"4743/SSPL/25-26\",\r\n      \"orderduedate\": \"2025-12-16\",\r\n      \"CGST\": 9,\r\n      \"SGST_UTGST\": 9,\r\n      \"IGST\": 18\r\n    }\r\n  ],\r\n\r\n  \"loadingCharges\": {\r\n    \"loadingAmount\": 3900.00,\r\n    \"IGST\": 18,\r\n    \"CGST\": 9,\r\n    \"SGST_UTGST\": 9,\r\n\t\"total\":\"\"\r\n  },\r\n\r\n  \"transportationCharges\": {\r\n    \"transportationAmount\": 500.00,\r\n    \"IGST\": 18,\r\n    \"CGST\": 9,\r\n    \"SGST_UTGST\": 9,\r\n\t\"total\":\"\"\r\n  },\r\n\r\n  \"grandTotal\": 707764\r\n}";
            //JObject obj = JObject.Parse(json);

            //json = JsonConvert.SerializeObject(json);
            return JsonConvert.DeserializeObject<SalesOrderData>(json);
        }

        public async Task<ApiResponse> old_GetOrderFromApi()
        {
            using (HttpClient client = new HttpClient())
            {
                //var response = await client.PostAsync(
                //    "http://172.107.33.128/mvdswebapi/api/docfile",
                //    new StringContent("", Encoding.UTF8, "application/json")
                //);

                string json = "";// await response.Content.ReadAsStringAsync();

                json = "{\r\n  \"customerInfo\": {\r\n    \"orderID\":\"1014700SO2503509\",\r\n    \"uid\": \"8d572f80-aaa4-11d9-9f54-008048130853-000a625c\",\r\n    \"customerCode\": \"1000\",\r\n    \"customerName\": \"Kumar Agencies\",\r\n    \"address1\": \"No 12, Anna Nagar\",\r\n    \"address2\": \"West Extension\",\r\n    \"address3\": \"Near Bus Stand\",\r\n    \"address4\": \"Chennai\",\r\n    \"address5\": \"Tamil Nadu\",\r\n    \"basicbuyeraddress1\": \"24/4B, SUNDARAJANPATTI\",\r\n    \"basicbuyeraddress2\": \"MADURAI\",\r\n    \"basicbuyeraddress3\": \"625107\",\r\n    \"basicbuyeraddress4\": \"TAMILNADU\",\r\n    \"basicbuyeraddress5\": \"\",\r\n    \"date\": \"2025-04-15\",\r\n    \"statename\": \"Tamil Nadu\",\r\n    \"narration\": \"Ashok\",\r\n    \"countryofresidence\": \"India\",\r\n    \"partygstin\": \"33ASNPR1514M1ZH\",\r\n    \"placeofsupply\": \"Tamil Nadu\",\r\n    \"partyname\": \"SRI RENGA STEELS\",\r\n    \"vouchernumber\": \"4743/SSPL/25-26\",\r\n    \"partypincode\": \"625007\",\r\n    \"consigneegstin\": \"33ASNPR1514M1ZH\",\r\n    \"consigneestatename\": \"Tamil Nadu\",\r\n    \"consigneecountryname\": \"India\",\r\n    \"basicorderref\": \"550 D CRS\",\r\n    \"basicduedateofpymt\": \"10 DAYS\"\r\n  },\r\n\r\n  \"materialItemsList\": [\r\n    {\r\n      \"stockitemname\": \"BD TMT Bar 8MM 550D CRS\",\r\n      \"gsthsnname\": \"72141090\",\r\n      \"rate\": 46300.00,\r\n      \"amount\": 138900.00,\r\n      \"actualqty\": 3.0,\r\n      \"billedqty\": 3.0,\r\n      \"godownname\": \"Pondicherry\",\r\n      \"batchname\": \"Primary Batch\",\r\n      \"destinationgodownname\": \"Pondicherry\",\r\n      \"indentno\": \"\",\r\n      \"orderno\": \"4743/SSPL/25-26\",\r\n      \"orderduedate\": \"2025-12-16\",\r\n      \"CGST\": 9,\r\n      \"SGST_UTGST\": 9,\r\n      \"IGST\": 18\r\n    },\r\n    {\r\n      \"stockitemname\": \"BD TMT Bar 10MM 550D CRS\",\r\n      \"gsthsnname\": \"72141090\",\r\n      \"rate\": 45800.00,\r\n      \"amount\": 137400.00,\r\n      \"actualqty\": 3.0,\r\n      \"billedqty\": 3.0,\r\n      \"godownname\": \"Pondicherry\",\r\n      \"batchname\": \"Primary Batch\",\r\n      \"destinationgodownname\": \"Pondicherry\",\r\n      \"indentno\": \"\",\r\n      \"orderno\": \"4743/SSPL/25-26\",\r\n      \"orderduedate\": \"2025-12-16\",\r\n      \"CGST\": 9,\r\n      \"SGST_UTGST\": 9,\r\n      \"IGST\": 18\r\n    },\r\n    {\r\n      \"stockitemname\": \"BD TMT Bar 12MM 550D CRS\",\r\n      \"gsthsnname\": \"72141090\",\r\n      \"rate\": 45800.00,\r\n      \"amount\": 91600.00,\r\n      \"actualqty\": 2.0,\r\n      \"billedqty\": 2.0,\r\n      \"godownname\": \"Pondicherry\",\r\n      \"batchname\": \"Primary Batch\",\r\n      \"destinationgodownname\": \"Pondicherry\",\r\n      \"indentno\": \"\",\r\n      \"orderno\": \"4743/SSPL/25-26\",\r\n      \"orderduedate\": \"2025-12-16\",\r\n      \"CGST\": 9,\r\n      \"SGST_UTGST\": 9,\r\n      \"IGST\": 18\r\n    },\r\n    {\r\n      \"stockitemname\": \"BD TMT Bar 16MM 550D CRS\",\r\n      \"gsthsnname\": \"72141090\",\r\n      \"rate\": 45800.00,\r\n      \"amount\": 137400.00,\r\n      \"actualqty\": 3.0,\r\n      \"billedqty\": 3.0,\r\n      \"godownname\": \"Pondicherry\",\r\n      \"batchname\": \"Primary Batch\",\r\n      \"destinationgodownname\": \"Pondicherry\",\r\n      \"indentno\": \"\",\r\n      \"orderno\": \"4743/SSPL/25-26\",\r\n      \"orderduedate\": \"2025-12-16\",\r\n      \"CGST\": 9,\r\n      \"SGST_UTGST\": 9,\r\n      \"IGST\": 18\r\n    },\r\n    {\r\n      \"stockitemname\": \"BD TMT Bar 20MM 550D CRS\",\r\n      \"gsthsnname\": \"72141090\",\r\n      \"rate\": 45800.00,\r\n      \"amount\": 91600.00,\r\n      \"actualqty\": 2.0,\r\n      \"billedqty\": 2.0,\r\n      \"godownname\": \"Pondicherry\",\r\n      \"batchname\": \"Primary Batch\",\r\n      \"destinationgodownname\": \"Pondicherry\",\r\n      \"indentno\": \"\",\r\n      \"orderno\": \"4743/SSPL/25-26\",\r\n      \"orderduedate\": \"2025-12-16\",\r\n      \"CGST\": 9,\r\n      \"SGST_UTGST\": 9,\r\n      \"IGST\": 18\r\n    }\r\n  ],\r\n\r\n  \"loadingCharges\": {\r\n    \"loadingAmount\": 3900.00,\r\n    \"IGST\": 18,\r\n    \"CGST\": 9,\r\n    \"SGST_UTGST\": 9,\r\n\t\"total\":\"\"\r\n  },\r\n\r\n  \"transportationCharges\": {\r\n    \"transportationAmount\": 500.00,\r\n    \"IGST\": 18,\r\n    \"CGST\": 9,\r\n    \"SGST_UTGST\": 9,\r\n\t\"total\":\"\"\r\n  },\r\n\r\n  \"grandTotal\": 707764\r\n}";

                try
                {
                    return JsonConvert.DeserializeObject<ApiResponse>(json);
                }
                catch (Exception ex)
                {
                    throw new Exception("JSON Deserialize Error: " + ex.Message + "\n\nJSON:\n" + json);
                }
            }
        }

        public void SaveOrder(SalesOrderData order)
        {
            string _connStr = ConfigurationManager
                .ConnectionStrings["SSMI_DefaultConnection"].ConnectionString;

            using (SqlConnection con = new SqlConnection(_connStr))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    //SqlCommand del = new SqlCommand(
                    //    "DELETE FROM SalesOrderItems WHERE OrderNo=@OrderNo; " +
                    //    "DELETE FROM SalesOrderHeader WHERE OrderNo=@OrderNo",
                    //    con, tran);

                    SqlCommand del = new SqlCommand(
                                "DELETE FROM Tally_Voucher WHERE VoucherNumber = @OrderNo; ",
                                con, tran);

                    del.Parameters.AddWithValue("@OrderNo", order.customerInfo.vouchernumber);
                    del.ExecuteNonQuery();

                    using (SqlCommand hdr = new SqlCommand(@"
                            INSERT INTO Tally_Voucher
                            (RemoteId, VoucherType, VoucherNumber, VoucherDate, PartyName, Party_Addr1, 
                             Party_Addr2, Party_Addr3, Party_Addr4, Party_Addr5, Party_StateDesc, 
                             Party_Pincode, Party_Country, Party_Desp_Addr1, Party_Desp_Addr2, 
                             Party_Desp_Addr3, Party_Desp_Addr4, Party_Desp_Addr5, Party_Desp_StateDesc, 
                             Party_Desp_Pincode, Party_Desp_Country, PartyGSTIN, PlaceOfSupply, 
                             BilltoPlace, ShiptoPlace, BasicOrderRef, BasicShipVesselNo, 
                             BasicBuyersSalesTaxNo, BasicDueDateOfPymt, Narration, TotalAmount, IRN, 
                             IRNAckNo, IRNAckDate, CreatedOn, RegstrId)
                            OUTPUT INSERTED.VoucherId
                            VALUES (@RemoteId, @VoucherType, @VoucherNumber, @VoucherDate, @PartyName, @Party_Addr1, 
                             @Party_Addr2, @Party_Addr3, @Party_Addr4, @Party_Addr5, @Party_StateDesc, 
                             @Party_Pincode, @Party_Country, @Party_Desp_Addr1, @Party_Desp_Addr2, 
                             @Party_Desp_Addr3, @Party_Desp_Addr4, @Party_Desp_Addr5, @Party_Desp_StateDesc, 
                             @Party_Desp_Pincode, @Party_Desp_Country, @PartyGSTIN, @PlaceOfSupply, 
                             @BilltoPlace, @ShiptoPlace, @BasicOrderRef, @BasicShipVesselNo, 
                             @BasicBuyersSalesTaxNo, @BasicDueDateOfPymt, @Narration, @TotalAmount, @IRN, 
                             @IRNAckNo, @IRNAckDate, @CreatedOn, @RegstrId)", con, tran))
                    {
                        hdr.Parameters.AddWithValue("@RemoteId", order.customerInfo.uid);
                        hdr.Parameters.AddWithValue("@VoucherType", "Sales Order");
                        hdr.Parameters.AddWithValue("@VoucherNumber", order.customerInfo.vouchernumber);
                        hdr.Parameters.AddWithValue("@VoucherDate", order.customerInfo.date);
                        hdr.Parameters.AddWithValue("@PartyName", order.customerInfo.partyname);
                        hdr.Parameters.AddWithValue("@Party_Addr1", order.customerInfo.address1);
                        hdr.Parameters.AddWithValue("@Party_Addr2", order.customerInfo.address2);
                        hdr.Parameters.AddWithValue("@Party_Addr3", order.customerInfo.address3);
                        hdr.Parameters.AddWithValue("@Party_Addr4", order.customerInfo.address4);
                        hdr.Parameters.AddWithValue("@Party_Addr5", order.customerInfo.address5);
                        hdr.Parameters.AddWithValue("@Party_StateDesc", order.customerInfo.statename);
                        hdr.Parameters.AddWithValue("@Party_Pincode", order.customerInfo.partypincode);
                        hdr.Parameters.AddWithValue("@Party_Country", order.customerInfo.consigneecountryname);
                        hdr.Parameters.AddWithValue("@Party_Desp_Addr1", order.customerInfo.basicbuyeraddress1);
                        hdr.Parameters.AddWithValue("@Party_Desp_Addr2", order.customerInfo.basicbuyeraddress2);
                        hdr.Parameters.AddWithValue("@Party_Desp_Addr3", order.customerInfo.basicbuyeraddress3);
                        hdr.Parameters.AddWithValue("@Party_Desp_Addr4", order.customerInfo.basicbuyeraddress4);
                        hdr.Parameters.AddWithValue("@Party_Desp_Addr5", order.customerInfo.basicbuyeraddress5);
                        hdr.Parameters.AddWithValue("@Party_Desp_StateDesc", order.customerInfo.consigneestatename);
                        hdr.Parameters.AddWithValue("@Party_Desp_Pincode", order.customerInfo.partypincode);
                        hdr.Parameters.AddWithValue("@Party_Desp_Country", order.customerInfo.consigneecountryname);
                        hdr.Parameters.AddWithValue("@PartyGSTIN", order.customerInfo.partygstin);
                        hdr.Parameters.AddWithValue("@PlaceOfSupply", order.customerInfo.placeofsupply);
                        hdr.Parameters.AddWithValue("@BilltoPlace", order.customerInfo.placeofsupply);
                        hdr.Parameters.AddWithValue("@ShiptoPlace", order.customerInfo.placeofsupply);

                        hdr.Parameters.AddWithValue("@BasicOrderRef", order.customerInfo.basicorderref);
                        hdr.Parameters.AddWithValue("@BasicShipVesselNo", "-");
                        hdr.Parameters.AddWithValue("@BasicBuyersSalesTaxNo", "-");
                        hdr.Parameters.AddWithValue("@BasicDueDateOfPymt", order.customerInfo.basicduedateofpymt);
                        hdr.Parameters.AddWithValue("@Narration", order.customerInfo.narration );
                        hdr.Parameters.AddWithValue("@TotalAmount", order.GrandTotal);
                        hdr.Parameters.AddWithValue("@IRN", "-");
                        hdr.Parameters.AddWithValue("@IRNAckNo", "-");
                        hdr.Parameters.AddWithValue("@IRNAckDate", DBNull.Value);
                        hdr.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
                        hdr.Parameters.AddWithValue("@RegstrId", 1);

                        long voucherId = (long)hdr.ExecuteScalar();

                        //Stock Item List
                        foreach (var item in order.materialItemsList)
                        {
                            SqlCommand itm = new SqlCommand(@"
                            INSERT INTO Tally_Voucher_Inventory
                            (VoucherId, StockItemName, HSNCode, Quantity, Rate, UnitCode, 
                             Amount, GodownName, BatchName, OrderNo, OrderDueDate, LedgerName, 
                             ClassRate, CGSTExprn, SGSTExprn, IGSTExprn)
                            VALUES (@VoucherId,@StockItemName,@HSNCode,@Quantity,@Rate,
                            @UnitCode,@Amount,@GodownName,@BatchName,@OrderNo,@OrderDueDate,
                            @LedgerName,@ClassRate,@CGSTExprn,@SGSTExprn,@IGSTExprn)", con, tran);

                            itm.Parameters.AddWithValue("@VoucherId", voucherId);
                            itm.Parameters.AddWithValue("@StockItemName", item.stockitemname);
                            itm.Parameters.AddWithValue("@HSNCode", item.gsthsnname);
                            itm.Parameters.AddWithValue("@Quantity", item.billedqty);
                            itm.Parameters.AddWithValue("@Rate", item.rate);
                            itm.Parameters.AddWithValue("@UnitCode", item.unitcode);
                            itm.Parameters.AddWithValue("@Amount", item.amount);
                            itm.Parameters.AddWithValue("@GodownName", item.godownname);
                            itm.Parameters.AddWithValue("@BatchName", item.batchname);
                            itm.Parameters.AddWithValue("@OrderNo", item.orderno);
                            itm.Parameters.AddWithValue("@OrderDueDate", item.orderduedate);
                            itm.Parameters.AddWithValue("@LedgerName", order.customerInfo.partygstin);
                            itm.Parameters.AddWithValue("@ClassRate", item.classrate);
                            itm.Parameters.AddWithValue("@CGSTExprn", item.cgstexprn);
                            itm.Parameters.AddWithValue("@SGSTExprn", item.sgstexprn);
                            itm.Parameters.AddWithValue("@IGSTExprn", item.igstexprn);

                            itm.ExecuteNonQuery();
                        }

                        //Loading Charges
                        //foreach (var lc_item in order.loadingchargesList)
                        //{
                            SqlCommand lcl = new SqlCommand(@"
                            INSERT INTO Tally_Voucher_Ledger
                            (VoucherId, LedgerName, IsPartyLedger, Amount)
                            VALUES (@VoucherId,@LedgerName,@IsPartyLedger,@Amount)", con, tran);

                            lcl.Parameters.AddWithValue("@VoucherId", voucherId);
                            lcl.Parameters.AddWithValue("@LedgerName", order.loadingCharges.ldgrname);
                            lcl.Parameters.AddWithValue("@IsPartyLedger", 0); //0 - "No" 1 - "Yes"
                        lcl.Parameters.AddWithValue("@Amount", order.loadingCharges.amount);
                            lcl.ExecuteNonQuery();
                        //}

                        //Transport Charges
                        //foreach (var tp_item in order.transportchargesList)
                        //{
                            SqlCommand tpc = new SqlCommand(@"
                            INSERT INTO Tally_Voucher_Ledger
                            (VoucherId, LedgerName, IsPartyLedger, Amount)
                            VALUES (@VoucherId,@LedgerName,@IsPartyLedger,@Amount)", con, tran);

                            tpc.Parameters.AddWithValue("@VoucherId", voucherId);
                            tpc.Parameters.AddWithValue("@LedgerName", order.transportationCharges.ldgrname);
                            tpc.Parameters.AddWithValue("@IsPartyLedger", 0); //0 - "No" 1 - "Yes"
                            tpc.Parameters.AddWithValue("@Amount", order.transportationCharges.amount);
                            tpc.ExecuteNonQuery();
                        //}

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


        //public void SaveOrder(ApiResponse api)
        //{
        //    SqlDataReader reader = null;
        //    string _connStr = ConfigurationManager.ConnectionStrings["SSMI_DefaultConnection"].ConnectionString;
        //    SqlConnection myConnection = new SqlConnection(_connStr);

        //    var order = api.data.First(); // ✅ get first order

        //    using (SqlConnection con = new SqlConnection(_connStr))
        //    {
        //        con.Open();
        //        SqlTransaction tran = con.BeginTransaction();

        //        try
        //        {
        //            SqlCommand del = new SqlCommand(
        //                "DELETE FROM SalesOrderItems WHERE OrderNo=@OrderNo; DELETE FROM SalesOrderHeader WHERE OrderNo=@OrderNo",
        //                con, tran);

        //            del.Parameters.AddWithValue("@OrderNo", order.customerInfo.vouchernumber);
        //            del.ExecuteNonQuery();

        //            SqlCommand hdr = new SqlCommand(@"
        //        INSERT INTO SalesOrderHeader
        //        (OrderNo, CustomerCode, CustomerName, OrderDate, GSTIN, GrandTotal)
        //        VALUES (@OrderNo,@Code,@Name,@Date,@GST,@Total)",
        //                con, tran);

        //            hdr.Parameters.AddWithValue("@OrderNo", order.customerInfo.vouchernumber);
        //            hdr.Parameters.AddWithValue("@Code", order.customerInfo.customerCode);
        //            hdr.Parameters.AddWithValue("@Name", order.customerInfo.customerName);
        //            hdr.Parameters.AddWithValue("@Date", order.customerInfo.date);
        //            hdr.Parameters.AddWithValue("@GST", order.customerInfo.partygstin);
        //            hdr.Parameters.AddWithValue("@Total", order.GrandTotal);

        //            hdr.ExecuteNonQuery();

        //            foreach (var item in order.materialItemsList)
        //            {
        //                SqlCommand itm = new SqlCommand(@"
        //            INSERT INTO SalesOrderItems
        //            (OrderNo, ItemName, HSN, Rate, Qty, Amount)
        //            VALUES (@OrderNo,@Item,@HSN,@Rate,@Qty,@Amt)",
        //                    con, tran);

        //                itm.Parameters.AddWithValue("@OrderNo", order.customerInfo.vouchernumber);
        //                itm.Parameters.AddWithValue("@Item", item.stockitemname);
        //                itm.Parameters.AddWithValue("@HSN", item.gsthsnname);
        //                itm.Parameters.AddWithValue("@Rate", item.rate);
        //                itm.Parameters.AddWithValue("@Qty", item.actualqty);
        //                itm.Parameters.AddWithValue("@Amt", item.amount);

        //                itm.ExecuteNonQuery();
        //            }

        //            tran.Commit();
        //        }
        //        catch
        //        {
        //            tran.Rollback();
        //            throw;
        //        }
        //    }
        //}



    }
}