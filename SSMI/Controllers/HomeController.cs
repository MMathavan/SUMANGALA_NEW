using SSMI.Data;
using SSMI.Filters;
using SSMI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SSMI.Controllers
{
    //[AuthActionFilter]

    public class HomeController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        SSMIEntities db = new SSMIEntities();
        public ActionResult Index()
        {
            ViewBag.CNAME = Session["CUSRID"];
            var bid = Convert.ToInt32(Session["BRNCHID"]);

            if (Request.Form.Get("clear") != null)
            {
                Session.Remove("SDATE");
                Session.Remove("EDATE");
            }
            else if (Request.Form.Get("from") != null)
            {
                Session["SDATE"] = Request.Form.Get("from");
                Session["EDATE"] = Request.Form.Get("to");
            }

            DateTime? sd = null;
            DateTime? ed = null;
            var sdateStr = Session["SDATE"] as string;
            var edateStr = Session["EDATE"] as string;
            if (!string.IsNullOrWhiteSpace(sdateStr) && !string.IsNullOrWhiteSpace(edateStr))
            {
                sd = Convert.ToDateTime(sdateStr).Date;
                ed = Convert.ToDateTime(edateStr).Date;
            }

            ViewBag.ItemGroupCount = context.Database.SqlQuery<int>("select count(*) from StockGroupMaster").FirstOrDefault();
            ViewBag.ItemCount = context.Database.SqlQuery<int>("select count(*) from StockItemMaster").FirstOrDefault();
            ViewBag.UnitCount = context.Database.SqlQuery<int>("select count(*) from UnitMaster").FirstOrDefault();
            ViewBag.CustomerCount = context.Database.SqlQuery<int>("select count(*) from LedgerMaster").FirstOrDefault();

            if (sd.HasValue && ed.HasValue)
            {
                ViewBag.TotalSalesOrder = context.Database.SqlQuery<int>(
                    "select count(*) from Tally_Voucher where RegstrId = @p0 and VoucherDate >= @p1 and VoucherDate <= @p2",
                    1,
                    sd.Value,
                    ed.Value).FirstOrDefault();

                ViewBag.TotalSalesInvoice = context.Database.SqlQuery<int>(
                    "select count(*) from Tally_Voucher where RegstrId = @p0 and VoucherDate >= @p1 and VoucherDate <= @p2",
                    2,
                    sd.Value,
                    ed.Value).FirstOrDefault();

                ViewBag.SalesOrderSync0Count = context.Database.SqlQuery<int>(
                    "select count(*) from Tally_Voucher where RegstrId = @p0 and isnull(SyncStatus,0) = @p1 and VoucherDate >= @p2 and VoucherDate <= @p3",
                    1,
                    0,
                    sd.Value,
                    ed.Value).FirstOrDefault();

                ViewBag.SalesOrderSync1Count = context.Database.SqlQuery<int>(
                    "select count(*) from Tally_Voucher where RegstrId = @p0 and isnull(SyncStatus,0) = @p1 and VoucherDate >= @p2 and VoucherDate <= @p3",
                    1,
                    1,
                    sd.Value,
                    ed.Value).FirstOrDefault();

                ViewBag.SalesInvoiceSync0Count = context.Database.SqlQuery<int>(
                    "select count(*) from Tally_Voucher where RegstrId = @p0 and isnull(SyncStatus,0) = @p1 and VoucherDate >= @p2 and VoucherDate <= @p3",
                    2,
                    0,
                    sd.Value,
                    ed.Value).FirstOrDefault();

                ViewBag.SalesInvoiceSync1Count = context.Database.SqlQuery<int>(
                    "select count(*) from Tally_Voucher where RegstrId = @p0 and isnull(SyncStatus,0) = @p1 and VoucherDate >= @p2 and VoucherDate <= @p3",
                    2,
                    1,
                    sd.Value,
                    ed.Value).FirstOrDefault();
            }
            else
            {
                ViewBag.TotalSalesOrder = context.Database.SqlQuery<int>(
                    "select count(*) from Tally_Voucher where RegstrId = @p0",
                    1).FirstOrDefault();

                ViewBag.TotalSalesInvoice = context.Database.SqlQuery<int>(
                    "select count(*) from Tally_Voucher where RegstrId = @p0",
                    2).FirstOrDefault();

                ViewBag.SalesOrderSync0Count = context.Database.SqlQuery<int>(
                    "select count(*) from Tally_Voucher where RegstrId = @p0 and isnull(SyncStatus,0) = @p1",
                    1,
                    0).FirstOrDefault();

                ViewBag.SalesOrderSync1Count = context.Database.SqlQuery<int>(
                    "select count(*) from Tally_Voucher where RegstrId = @p0 and isnull(SyncStatus,0) = @p1",
                    1,
                    1).FirstOrDefault();

                ViewBag.SalesInvoiceSync0Count = context.Database.SqlQuery<int>(
                    "select count(*) from Tally_Voucher where RegstrId = @p0 and isnull(SyncStatus,0) = @p1",
                    2,
                    0).FirstOrDefault();

                ViewBag.SalesInvoiceSync1Count = context.Database.SqlQuery<int>(
                    "select count(*) from Tally_Voucher where RegstrId = @p0 and isnull(SyncStatus,0) = @p1",
                    2,
                    1).FirstOrDefault();
            }

            //if (bid == 1)
            //{
            //    var rslt = context.Database.SqlQuery<pr_New_Dashboard_Assgn_Result>("pr_New_Dashboard_Assgn @PCompYId = @PCompYId", new SqlParameter("@PCOMPYId", Convert.ToInt32(Session["compyid"]))).ToList();
            //    ViewBag.POAMT = rslt[0].POAMT;
            //    ViewBag.GRNAMT = rslt[0].GRNAMT;
            //    ViewBag.ADVAMT = rslt[0].ADVAMT;
            //    ViewBag.PIAMT = rslt[0].PIAMT;
            //    ViewBag.AMIN = rslt[0].AMIN;
            //    ViewBag.AMRN = rslt[0].AMRN;
            //}
            //else
            //{
            //    //var rslt = context.Database.SqlQuery<pr_New_Branch_Dashboard_Assgn_Result>("pr_New_Dashboard_Assgn @PCompYId = @PCompYId, @PBrnchId = @PBrnchId", new SqlParameter("@PCOMPYId,@PCOMPYId", Convert.ToInt32(Session["compyid"]))).ToList();
            //    var rslt = context.Database.SqlQuery<pr_New_Branch_Dashboard_Assgn_Result>("pr_New_Branch_Dashboard_Assgn @PCompYId = " + Convert.ToInt32(Session["compyid"]) + ", @PBrnchId = " + bid).ToList();
            //    ViewBag.POAMT = rslt[0].POAMT;
            //    ViewBag.GRNAMT = rslt[0].GRNAMT;
            //    ViewBag.ADVAMT = rslt[0].ADVAMT;
            //    ViewBag.PIAMT = rslt[0].PIAMT;
            //    ViewBag.AMIN = rslt[0].AMIN;
            //    ViewBag.AMRN = rslt[0].AMRN;

            //}

            //var Arslt = context.Database.SqlQuery<pr_DB_MainStore_Stock_Value_Result>("pr_DB_MainStore_Stock_Value @PCompYId = @PCompYId, @PTWrdId = @PTWrdId", new SqlParameter("@PCOMPYId", Convert.ToInt32(Session["compyid"])), new SqlParameter("@PTWrdId", 31)).ToList();
            //ViewBag.MVALUE = Arslt[0].AVALUE;

            return View();
        }

        public ActionResult Charts()
        {
            return View();
        }

        public ActionResult Tables()
        {
            return View();
        }

        public ActionResult Forms()
        {
            return View();
        }

        //public JsonResult GetbBranchStockData()
        //{
        //    //List<pr_DB_Branch_Store_Sales_Value_Result> data = new List<pr_DB_Branch_Store_Sales_Value_Result>();
        //    ////Here MyDatabaseEntities  is our dbContext
        //    //using (SSMIEntities dc = new SSMIEntities())
        //    //{
        //    //    data = dc.Database.SqlQuery<pr_DB_Branch_Store_Sales_Value_Result>("pr_DB_Branch_Store_Sales_Value @PCOMPYID='" + System.Web.HttpContext.Current.Session["COMPYID"] + "',@PTWRDID = " + Convert.ToInt32(Session["BRNCHID"])).ToList();
        //    //}

        //    var chartData = new object[data.Count + 1];
        //    //chartData[0] = new object[]{
        //    //    "Value",
        //    //    "Branch"
        //    //    //,"Receipt"
        //    //};

        //    //int j = 0;
        //    //foreach (var i in data)
        //    //{
        //    //    j++;
        //    //    chartData[j] = new object[] { i.BRNCHNAME.ToString(), i.AVALUE };
        //    //}
        //    ////return chartData;
        //    return Json(chartData);
        //}


        //public JsonResult GetMaterialOutwardData()
        //{
        //    List<pr_DB_Main_Store_OutWard_Value_Result> data = new List<pr_DB_Main_Store_OutWard_Value_Result>();
        //    //Here MyDatabaseEntities  is our dbContext
        //    using (SSMIEntities dc = new SSMIEntities())
        //    {
        //        data = dc.Database.SqlQuery<pr_DB_Main_Store_OutWard_Value_Result>("pr_DB_Main_Store_OutWard_Value @PCOMPYID='" + System.Web.HttpContext.Current.Session["COMPYID"] + "'").ToList();
        //    }

        //    var chartData = new object[data.Count + 1];
        //    chartData[0] = new object[]{
        //        "Task",
        //        "Hours per Day"
        //        //,"Receipt"
        //    };

        //    int j = 0;
        //    foreach (var i in data)
        //    {
        //        j++;
        //        chartData[j] = new object[] { i.MTRLDESC.ToString(), i.CQTY };
        //    }
        //    //return chartData;
        //    return Json(chartData);
        //}

        public ActionResult BootstrapElements()
        {
            return View();
        }

        public ActionResult BootstrapGrid()
        {
            return View();
        }

        public ActionResult BlankPage()
        {
            return View();
        }

        public JsonResult GetRequestDetail()
        {
            return Json(new object[0], JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetIssueDetail()
        {
            return Json(new object[0], JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPurchaseDetail()
        {
            return Json(new object[0], JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetRequestDetail()
        //{
        //    Dictionary<string, decimal> dct = new Dictionary<string, decimal>();



        //   // var rslt = context.Database.SqlQuery<VW_NOTOFICATION_MATERIAL_REQUEST_ASSGN>("Select * From VW_NOTOFICATION_MATERIAL_REQUEST_ASSGN Where TCUSRID ='Senthil'").ToList();
        //    var rslt = context.Database.SqlQuery<VW_NOTOFICATION_MATERIAL_REQUEST_ASSGN>("Select * From VW_NOTOFICATION_MATERIAL_REQUEST_ASSGN").ToList();

        //    foreach (var data in rslt)
        //    {
        //       // var reordr = context.Database.SqlQuery<decimal>("select isnull(ROLNQTY,0) as ROLNQTY from materialmaster where MTRLID=" + Convert.ToInt32(data.MTRLID)).ToList();

        //       // var rolnqty = Convert.ToInt32(reordr[0]);
        //       // var clqty = Convert.ToInt32(data.CLQTY);

        //      //  if (clqty < rolnqty)
        //      //  {
        //            dct.Add(data.BRNCHNAME, data.CCOUNT.Value);
        //      //  }
        //    }

        //    var list = (from x in dct
        //                select new { id = x.Key, name = x.Value }).ToList();

        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}


        //public JsonResult GetIssueDetail()
        //{
        //    Dictionary<string, decimal> dct = new Dictionary<string, decimal>();



        //    // var rslt = context.Database.SqlQuery<VW_NOTOFICATION_MATERIAL_REQUEST_ASSGN>("Select * From VW_NOTOFICATION_MATERIAL_REQUEST_ASSGN Where TCUSRID ='Senthil'").ToList();
        //    var rslt = context.Database.SqlQuery<VW_NOTOFICATION_MATERIAL_ISSUE_ASSGN>("Select * From VW_NOTOFICATION_MATERIAL_ISSUE_ASSGN Where BRNCHID =" + Convert.ToInt32(System.Web.HttpContext.Current.Session["F_BRNCHID"])).ToList();

        //    foreach (var data in rslt)
        //    {
        //        // var reordr = context.Database.SqlQuery<decimal>("select isnull(ROLNQTY,0) as ROLNQTY from materialmaster where MTRLID=" + Convert.ToInt32(data.MTRLID)).ToList();

        //        // var rolnqty = Convert.ToInt32(reordr[0]);
        //        // var clqty = Convert.ToInt32(data.CLQTY);

        //        //  if (clqty < rolnqty)
        //        //  {
        //        dct.Add(data.BRNCHNAME, data.CCOUNT.Value);
        //        //  }
        //    }

        //    var list = (from x in dct
        //                select new { id = x.Key, name = x.Value }).ToList();

        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetPurchaseDetail()
        //{
        //    Dictionary<string, decimal> dct = new Dictionary<string, decimal>();



        //    // var rslt = context.Database.SqlQuery<VW_NOTOFICATION_MATERIAL_REQUEST_ASSGN>("Select * From VW_NOTOFICATION_MATERIAL_REQUEST_ASSGN Where TCUSRID ='Senthil'").ToList();
        //    var rslt = context.Database.SqlQuery<VW_NOTOFICATION_PURCHASE_INVOICE_ASSGN>("Select * From VW_NOTOFICATION_PURCHASE_INVOICE_ASSGN").ToList();

        //    foreach (var data in rslt)
        //    {
        //        // var reordr = context.Database.SqlQuery<decimal>("select isnull(ROLNQTY,0) as ROLNQTY from materialmaster where MTRLID=" + Convert.ToInt32(data.MTRLID)).ToList();

        //        // var rolnqty = Convert.ToInt32(reordr[0]);
        //        // var clqty = Convert.ToInt32(data.CLQTY);

        //        //  if (clqty < rolnqty)
        //        //  {
        //        //dct.Add(data.BRNCHNAME, data.CCOUNT.Value);
        //        //  }
        //    }

        //    var list = (from x in dct
        //                select new { id = x.Key, name = x.Value }).ToList();

        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}


        //END CLASS
    }
}