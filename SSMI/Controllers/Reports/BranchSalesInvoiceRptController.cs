using FUSIONPRO_PX.Helper;
using FUSIONPRO_PX.Data;
using FUSIONPRO_PX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data.SqlClient;
using System.Configuration;

namespace FUSIONPRO_PX.Controllers.Reports
{
    [SessionExpire]
    public class BranchSalesInvoiceRptController : Controller
    {
        // GET: BranchSalesInvoiceRpt
        ApplicationDbContext context = new ApplicationDbContext();
        FUSIONPRO_PXEntities db = new FUSIONPRO_PXEntities();

        [Authorize(Roles = "BranchSalesInvoiceReports")]
        public ActionResult Index()
        {
            if (Convert.ToInt32(System.Web.HttpContext.Current.Session["compyid"]) == 0 || Convert.ToInt32(System.Web.HttpContext.Current.Session["BRNCHID"]) == 0) { return RedirectToAction("Login", "Account"); }
            if (string.IsNullOrEmpty(Session["SDATE"] as string))
            {
                Session["SDATE"] = DateTime.Now.ToString("dd-MM-yyyy");
                Session["EDATE"] = DateTime.Now.ToString("dd-MM-yyyy");
            }
            else
            {
                if (Request.Form.Get("from") != null)
                {
                    Session["SDATE"] = Request.Form.Get("from");
                    Session["EDATE"] = Request.Form.Get("to");
                }
            }
            DateTime sd = Convert.ToDateTime(System.Web.HttpContext.Current.Session["SDATE"]).Date;
            DateTime ed = Convert.ToDateTime(System.Web.HttpContext.Current.Session["EDATE"]).Date;
            //ViewBag.BRNCHID = new SelectList(context.branchmasters.Where(x => x.BRNCHID > 0).OrderBy(x => x.BRNCHID), "BRNCHID", "BRNCHNAME");
            string brnchname = "";
            if (Request.Form.Get("F_BRNCHNAME") == null)
            {   //brnchname = Session["F_BRNCHNAME"].ToString();
                brnchname = Session["F_BRNCHNAME"].ToString();
            }
            else
            { brnchname = Request.Form.Get("F_BRNCHNAME"); }

            //var brnchname = Request.Form.Get("BRNCHID");// Session["BRNCHNAME"].ToString();

            if (System.Web.HttpContext.Current.Session["Group"].ToString() == "Manager" || System.Web.HttpContext.Current.Session["Group"].ToString() == "Admin")
            {
                //ViewBag.BRNCHID = new SelectList(context.branchmasters.OrderBy(x => x.BRNCHNAME), "BRNCHNAME", "BRNCHNAME", brnchname);// System.Web.HttpContext.Current.Session["BRNCHNAME"]);
                ViewBag.BRNCHID = new SelectList(context.branchmasters.OrderBy(x => x.BRNCHNAME), "BRNCHID", "BRNCHNAME", brnchname);// System.Web.HttpContext.Current.Session["BRNCHNAME"]);
                //var F_BRNCHID = context.Database.SqlQuery<int>("select BRNCHID from BranchMaster where BRNCHNAME='" + brnchname + "'").ToList();
                var F_BRNCHID = context.Database.SqlQuery<int>("select BRNCHID from BranchMaster where BRNCHNAME = '" + brnchname + "'").ToList();
                if (F_BRNCHID.Count > 0)
                {
                    ViewBag.F_BRNCHID = F_BRNCHID[0];
                    ViewBag.F_BRNCHNAME = brnchname;
                    Session["F_BRNCHID"] = F_BRNCHID[0];
                    Session["F_BRNCHNAME"] = brnchname;
                }
                else
                {
                    ViewBag.F_BRNCHID = 0;
                    ViewBag.F_BRNCHNAME = "";
                    Session["F_BRNCHID"] = 0;
                    Session["F_BRNCHNAME"] = "";
                }


            }
            else

            {
                //ViewBag.BRNCHID = new SelectList(context.branchmasters.OrderBy(x => x.BRNCHNAME).Where(x => x.BRNCHNAME == brnchname), "BRNCHNAME", "BRNCHNAME", System.Web.HttpContext.Current.Session["BRNCHNAME"]);
                //ViewBag.BRNCHID = new SelectList(context.branchmasters.OrderBy(x => x.BRNCHNAME).Where(x => x.BRNCHNAME == brnchname), "BRNCHID", "BRNCHNAME", System.Web.HttpContext.Current.Session["F_BRNCHID"]);
                ViewBag.BRNCHID = new SelectList(context.branchmasters.OrderBy(x => x.BRNCHNAME).Where(x => x.BRNCHNAME == brnchname), "BRNCHID", "BRNCHNAME", System.Web.HttpContext.Current.Session["F_BRNCHID"]);
                var F_BRNCHID = context.Database.SqlQuery<BranchMaster>("select * from BranchMaster where BRNCHNAME= '" + brnchname + "'").ToList();
                //ViewBag.F_BRNCHNAME = brnchname;
                //ViewBag.F_BRNCHID = Session["F_BRNCHID"];
                if (F_BRNCHID.Count > 0)
                {
                    ViewBag.F_BRNCHID = Convert.ToInt32(F_BRNCHID[0].BRNCHID);
                    ViewBag.F_BRNCHNAME = F_BRNCHID[0].BRNCHNAME; //brnchname;
                    Session["F_BRNCHID"] = Convert.ToInt32(F_BRNCHID[0].BRNCHID);
                    Session["F_BRNCHNAME"] = F_BRNCHID[0].BRNCHNAME; //brnchname;
                }
                else
                {
                    ViewBag.F_BRNCHID = 0;
                    ViewBag.F_BRNCHNAME = "";
                    Session["F_BRNCHID"] = 0;
                    Session["F_BRNCHNAME"] = "";
                }
                //ViewBag.BRNCHID = new SelectList(context.branchmasters.OrderBy(x => x.BRNCHNAME).Where(x => x.BRNCHNAME == Session["F_BRNCHNAME"].ToString()), "BRNCHID", "BRNCHNAME", System.Web.HttpContext.Current.Session["F_BRNCHID"]);
            }

            ViewBag.CATEID = new SelectList(context.suppliermasters.OrderBy(x => x.CATENAME), "CATEID", "CATENAME");
            ViewBag.MTRLGID = new SelectList(context.materialgroupmasters.OrderBy(x => x.MTRLGDESC), "MTRLGID", "MTRLGDESC");
            ViewBag.MTRLID = new SelectList(context.materialmasters.OrderBy(x => x.MTRLDESC), "MTRLID", "MTRLDESC");

            return View();

        }

        public JsonResult GetCustomerList()
        {
            var result = (from r in context.customermasters
                          select new { r.CATEID, r.CATENAME }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }//...end

        public JsonResult GetMaterialGroupList()
        {
            var result = (from r in context.materialgroupmasters
                          select new { r.MTRLGID, r.MTRLGDESC }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }//...end

        public JsonResult GetMaterialList()
        {
            var result = (from r in context.materialmasters
                          select new { r.MTRLID, r.MTRLDESC }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }//...end

        //
        public void showrpt()
        {

            var brnchid = Convert.ToInt32(Request.Form.Get("F_BRNCHID"));
            var rpttype = Convert.ToInt32(Request.Form.Get("rpttype"));

            var afromDate = Request.Form.Get("from").Split('-');
            var fromDate = afromDate[2] + "-" + afromDate[1] + "-" + afromDate[0];

            var atoDate = Request.Form.Get("to").Split('-');
            var toDate = atoDate[2] + "-" + atoDate[1] + "-" + atoDate[0];

            var strPath = ConfigurationManager.AppSettings["Reporturl"];

            var rptURL = ""; var rptquery = ""; var strHead = "";

            var SDATE = afromDate[0] + "-" + afromDate[1] + "-" + afromDate[2];
            var EDATE = atoDate[0] + "-" + atoDate[1] + "-" + atoDate[2];

            context.Database.ExecuteSqlCommand("DELETE FROM TMPRPT_IDS WHERE KUSRID='" + Session["CUSRID"].ToString() + "'");
            var sql = "INSERT INTO TMPRPT_IDS(KUSRID, OPTNSTR, RPTID)";


            switch (rpttype)
            {
                case 1:
                    var gentype = Convert.ToInt32(Request.Form.Get("gentype"));
                    switch (gentype)
                    {
                        case 0:
                            strHead = "Consolidated Sales Invoice Report From " + SDATE + " Till " + EDATE + "";
                            if (brnchid == 0)
                            {
                                sql = sql + " SELECT  '" + Session["CUSRID"].ToString() + "', 'order' , TRANDID FROM VW_TRANSACTION_DETAIL_ASSGN WHERE REGSTRID = 15 AND DISPSTATUS IN (0) AND TRANDATE BETWEEN '" + fromDate + "' AND '" + toDate + "' AND BRNCHID >= " + brnchid + "";
                            }
                            else
                            {
                                sql = sql + " SELECT  '" + Session["CUSRID"].ToString() + "', 'order' , TRANDID FROM VW_TRANSACTION_DETAIL_ASSGN WHERE REGSTRID = 15 AND DISPSTATUS IN (0) AND TRANDATE BETWEEN '" + fromDate + "' AND '" + toDate + "' AND BRNCHID = " + brnchid + "";
                            }

                            rptURL = strPath + "\\Branch_Sales_Invoice_Register.Rpt";
                            rptquery = "{VW_BRANCH_SALES_INVOICE_REGISTER_RPT.KUSRID}='" + Session["CUSRID"].ToString() + "'";
                            break;
                        case 1:
                            strHead = "Sales Invoice Detail Report From " + SDATE + " Till " + EDATE + "";
                            if (brnchid == 0)
                            {
                                sql = sql + " SELECT  '" + Session["CUSRID"].ToString() + "', 'order' , TRANDID FROM VW_TRANSACTION_DETAIL_ASSGN WHERE REGSTRID = 15 AND DISPSTATUS IN (0) AND TRANDATE BETWEEN '" + fromDate + "' AND '" + toDate + "' AND BRNCHID >= " + brnchid + "";
                            }
                            else
                            {
                                sql = sql + " SELECT  '" + Session["CUSRID"].ToString() + "', 'order' , TRANDID FROM VW_TRANSACTION_DETAIL_ASSGN WHERE REGSTRID = 15 AND DISPSTATUS IN (0) AND TRANDATE BETWEEN '" + fromDate + "' AND '" + toDate + "' AND BRNCHID = " + brnchid + "";
                            }

                            rptURL = strPath + "\\Branch_Sales_Invoice_Register_Detail.Rpt";
                            rptquery = "{VW_BRANCH_SALES_INVOICE_REGISTER_DETAIL_RPT.KUSRID}='" + Session["CUSRID"].ToString() + "'";
                            break;
                        case 2:
                            strHead = "Pending Sales Invoice Detail Report From " + SDATE + " Till " + EDATE + "";
                            if (brnchid == 0)
                            {
                                sql = sql + " SELECT  '" + Session["CUSRID"].ToString() + "', 'order' , TRANDID FROM VW_TRANSACTION_DETAIL_ASSGN WHERE REGSTRID = 15 AND DISPSTATUS IN (0) AND TRANDATE BETWEEN '" + fromDate + "' AND '" + toDate + "' AND BRNCHID >= " + brnchid + "";
                            }
                            else
                            {
                                sql = sql + " SELECT  '" + Session["CUSRID"].ToString() + "', 'order' , TRANDID FROM VW_TRANSACTION_DETAIL_ASSGN WHERE REGSTRID = 15 AND DISPSTATUS IN (0) AND TRANDATE BETWEEN '" + fromDate + "' AND '" + toDate + "' AND BRNCHID = " + brnchid + "";
                            }

                            rptURL = strPath + "\\Branch_Sales_Invoice_Pending_Register_Detail.Rpt";
                            rptquery = "{VW_BRANCH_SALES_INVOICE_PENDING_REGISTER_DETAIL_RPT.KUSRID}='" + Session["CUSRID"].ToString() + "'";
                            break;
                    }
                    break;
                case 2:
                    var supplrtype = Convert.ToInt32(Request.Form.Get("custtype"));
                    switch (supplrtype)
                    {
                        case 0:
                            strHead = "Customer Wise Consolidated Sales Invoice Report From " + SDATE + " Till " + EDATE + "";
                            if (brnchid == 0)
                            {
                                sql = sql + " SELECT  '" + Session["CUSRID"].ToString() + "', 'order' , TRANDID FROM VW_TRANSACTION_DETAIL_ASSGN WHERE REGSTRID = 15 AND DISPSTATUS IN (0) AND TRANDATE BETWEEN '" + fromDate + "' AND '" + toDate + "' AND BRNCHID >= " + brnchid + " AND TRANREFID IN (" + Request.Form.Get("CATEID") + ")";
                            }
                            else
                            {
                                sql = sql + " SELECT  '" + Session["CUSRID"].ToString() + "', 'order' , TRANDID FROM VW_TRANSACTION_DETAIL_ASSGN WHERE REGSTRID = 15 AND DISPSTATUS IN (0) AND TRANDATE BETWEEN '" + fromDate + "' AND '" + toDate + "' AND BRNCHID = " + brnchid + " AND TRANREFID IN (" + Request.Form.Get("CATEID") + ")";
                            }

                            rptURL = strPath + "\\Branch_Sales_Invoice_Customer_Register.Rpt";
                            rptquery = "{VW_BRANCH_SALES_INVOICE_REGISTER_RPT.KUSRID}='" + Session["CUSRID"].ToString() + "'";
                            break;
                        case 1:
                            strHead = "Customer Wise Sales Invoice Detailed Report From " + SDATE + " Till " + EDATE + "";
                            if (brnchid == 0)
                            {
                                sql = sql + " SELECT  '" + Session["CUSRID"].ToString() + "', 'order' , TRANDID FROM VW_TRANSACTION_DETAIL_ASSGN WHERE REGSTRID = 15 AND DISPSTATUS IN (0) AND TRANDATE BETWEEN '" + fromDate + "' AND '" + toDate + "' AND BRNCHID >= " + brnchid + " AND TRANREFID IN (" + Request.Form.Get("CATEID") + ")";
                            }
                            else
                            {
                                sql = sql + " SELECT  '" + Session["CUSRID"].ToString() + "', 'order' , TRANDID FROM VW_TRANSACTION_DETAIL_ASSGN WHERE REGSTRID = 15 AND DISPSTATUS IN (0) AND TRANDATE BETWEEN '" + fromDate + "' AND '" + toDate + "' AND BRNCHID = " + brnchid + " AND TRANREFID IN (" + Request.Form.Get("CATEID") + ")";
                            }

                            rptURL = strPath + "\\Branch_Sales_Invoice_Customer_Register_Detail.Rpt";
                            rptquery = "{VW_BRANCH_SALES_INVOICE_REGISTER_DETAIL_RPT.KUSRID}='" + Session["CUSRID"].ToString() + "'";
                            break;
                        case 2:
                            strHead = "Customer Wise Consolidated Sales Invoice Report From " + SDATE + " Till " + EDATE + "";
                            if (brnchid == 0)
                            {
                                sql = sql + " SELECT  '" + Session["CUSRID"].ToString() + "', 'order' , TRANDID FROM VW_TRANSACTION_DETAIL_ASSGN WHERE REGSTRID = 15 AND DISPSTATUS IN (0) AND TRANDATE BETWEEN '" + fromDate + "' AND '" + toDate + "' AND BRNCHID >= " + brnchid + " AND TRANREFID IN (" + Request.Form.Get("CATEID") + ")";
                            }
                            else
                            {
                                sql = sql + " SELECT  '" + Session["CUSRID"].ToString() + "', 'order' , TRANDID FROM VW_TRANSACTION_DETAIL_ASSGN WHERE REGSTRID = 15 AND DISPSTATUS IN (0) AND TRANDATE BETWEEN '" + fromDate + "' AND '" + toDate + "' AND BRNCHID = " + brnchid + " AND TRANREFID IN (" + Request.Form.Get("CATEID") + ")";
                            }

                            rptURL = strPath + "\\Branch_Sales_Invoice_Customer_Register_C01.Rpt";
                            rptquery = "{VW_BRANCH_SALES_INVOICE_REGISTER_RPT.KUSRID}='" + Session["CUSRID"].ToString() + "'";
                            break;
                    }
                    break;
                case 3:
                    var mgrouptype = Convert.ToInt32(Request.Form.Get("mgrouptype"));
                    switch (mgrouptype)
                    {
                        case 0:
                            strHead = "Material Group Wise Sales Invoice Detailed Report From " + SDATE + " Till " + EDATE + "";
                            if (brnchid == 0)
                            {
                                sql = sql + " SELECT  '" + Session["CUSRID"].ToString() + "', 'order' , TRANDID FROM VW_TRANSACTION_DETAIL_ASSGN WHERE REGSTRID = 15 AND DISPSTATUS IN (0) AND TRANDATE BETWEEN '" + fromDate + "' AND '" + toDate + "' AND BRNCHID >= " + brnchid + " AND TRANDREFGID IN (" + Request.Form.Get("MTRLGID") + ")";
                            }
                            else
                            {
                                sql = sql + " SELECT  '" + Session["CUSRID"].ToString() + "', 'order' , TRANDID FROM VW_TRANSACTION_DETAIL_ASSGN WHERE REGSTRID = 15 AND DISPSTATUS IN (0) AND TRANDATE BETWEEN '" + fromDate + "' AND '" + toDate + "' AND BRNCHID = " + brnchid + " AND TRANDREFGID IN (" + Request.Form.Get("MTRLGID") + ")";
                            }

                            rptURL = strPath + "\\Branch_Sales_Invoice_Material_Group_Register_Detail.Rpt";
                            rptquery = "{VW_BRANCH_SALES_INVOICE_REGISTER_DETAIL_RPT.KUSRID}='" + Session["CUSRID"].ToString() + "'";
                            break;
                        case 1:
                            strHead = "Material Group Wise Sales Invoice Consolidated Report From " + SDATE + " Till " + EDATE + "";
                            if (brnchid == 0)
                            {
                                sql = sql + " SELECT  '" + Session["CUSRID"].ToString() + "', 'order' , TRANDID FROM VW_TRANSACTION_DETAIL_ASSGN WHERE REGSTRID = 15 AND DISPSTATUS IN (0) AND TRANDATE BETWEEN '" + fromDate + "' AND '" + toDate + "' AND BRNCHID >= " + brnchid + " AND TRANDREFGID IN (" + Request.Form.Get("MTRLGID") + ")";
                            }
                            else
                            {
                                sql = sql + " SELECT  '" + Session["CUSRID"].ToString() + "', 'order' , TRANDID FROM VW_TRANSACTION_DETAIL_ASSGN WHERE REGSTRID = 15 AND DISPSTATUS IN (0) AND TRANDATE BETWEEN '" + fromDate + "' AND '" + toDate + "' AND BRNCHID = " + brnchid + " AND TRANDREFGID IN (" + Request.Form.Get("MTRLGID") + ")";
                            }

                            rptURL = strPath + "\\Branch_Sales_Invoice_Material_Group_Register_Detail_C01.Rpt";
                            rptquery = "{VW_BRANCH_SALES_INVOICE_REGISTER_DETAIL_RPT.KUSRID}='" + Session["CUSRID"].ToString() + "'";
                            break;
                    }
                    break;
                case 4:
                    var mtrldesctype = Convert.ToInt32(Request.Form.Get("mtrldesctype"));
                    switch (mtrldesctype)
                    {
                        case 0:
                            strHead = "Material Wise Sales Invoice Detailed Report From " + SDATE + " Till " + EDATE + "";
                            if (brnchid == 0)
                            {
                                sql = sql + " SELECT  '" + Session["CUSRID"].ToString() + "', 'order' , TRANDID FROM VW_TRANSACTION_DETAIL_ASSGN WHERE REGSTRID = 15 AND DISPSTATUS IN (0) AND TRANDATE BETWEEN '" + fromDate + "' AND '" + toDate + "' AND BRNCHID >= " + brnchid + " AND TRANDREFID IN (" + Request.Form.Get("MTRLID") + ")";
                            }
                            else
                            {
                                sql = sql + " SELECT  '" + Session["CUSRID"].ToString() + "', 'order' , TRANDID FROM VW_TRANSACTION_DETAIL_ASSGN WHERE REGSTRID = 15 AND DISPSTATUS IN (0) AND TRANDATE BETWEEN '" + fromDate + "' AND '" + toDate + "' AND BRNCHID = " + brnchid + " AND TRANDREFID IN (" + Request.Form.Get("MTRLID") + ")";
                            }

                            rptURL = strPath + "\\Branch_Sales_Invoice_Material_Register_Detail.Rpt";
                            rptquery = "{VW_BRANCH_SALES_INVOICE_REGISTER_DETAIL_RPT.KUSRID}='" + Session["CUSRID"].ToString() + "'";
                            break;
                        case 1:
                            strHead = "Material Wise Sales Invoice Consolidated Report From " + SDATE + " Till " + EDATE + "";
                            if (brnchid == 0)
                            {
                                sql = sql + " SELECT  '" + Session["CUSRID"].ToString() + "', 'order' , TRANDID FROM VW_TRANSACTION_DETAIL_ASSGN WHERE REGSTRID = 15 AND DISPSTATUS IN (0) AND TRANDATE BETWEEN '" + fromDate + "' AND '" + toDate + "' AND BRNCHID >= " + brnchid + " AND TRANDREFID IN (" + Request.Form.Get("MTRLID") + ")";
                            }
                            else
                            {
                                sql = sql + " SELECT  '" + Session["CUSRID"].ToString() + "', 'order' , TRANDID FROM VW_TRANSACTION_DETAIL_ASSGN WHERE REGSTRID = 15 AND DISPSTATUS IN (0) AND TRANDATE BETWEEN '" + fromDate + "' AND '" + toDate + "' AND BRNCHID = " + brnchid + " AND TRANDREFID IN (" + Request.Form.Get("MTRLID") + ")";
                            }

                            rptURL = strPath + "\\Branch_Sales_Invoice_Material_Register_Detail_C01.Rpt";
                            rptquery = "{VW_BRANCH_SALES_INVOICE_REGISTER_DETAIL_RPT.KUSRID}='" + Session["CUSRID"].ToString() + "'";
                            break;
                    }
                    break;
                default:
                    //rptURL = "E:\\SIMSReports\\GMR_Register_SubCategory_Wise_Detail_Rpt.rpt";
                    break;
            }


            context.Database.ExecuteSqlCommand(sql);

            Response.Write(sql);
            //Response.End();


            ReportDocument cryRpt = new ReportDocument();
            TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            Tables CrTables;

            cryRpt.Load(rptURL);

            cryRpt.RecordSelectionFormula = rptquery;
            string paramName = "@FHEAD";

            for (int i = 0; i < cryRpt.DataDefinition.FormulaFields.Count; i++)
                if (cryRpt.DataDefinition.FormulaFields[i].FormulaName == "{" + paramName + "}")
                    cryRpt.DataDefinition.FormulaFields[i].Text = "'" + strHead + "'";
            String constring = ConfigurationManager.ConnectionStrings["Fusionpro_PX_DefaultConnection"].ConnectionString;
            SqlConnectionStringBuilder stringbuilder = new SqlConnectionStringBuilder(constring);
            crConnectionInfo.ServerName = stringbuilder.DataSource;
            crConnectionInfo.DatabaseName = stringbuilder.InitialCatalog;
            crConnectionInfo.UserID = stringbuilder.UserID;
            crConnectionInfo.Password = stringbuilder.Password;
            CrTables = cryRpt.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            {
                crtableLogoninfo = CrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                CrTable.ApplyLogOnInfo(crtableLogoninfo);

            }
            if ((Request.Form.Get("prnttype") == "1"))
            {
                cryRpt.ExportToHttpResponse(ExportFormatType.Excel, System.Web.HttpContext.Current.Response, false, "");
            }
            if ((Request.Form.Get("prnttype") == "2"))
            {
                cryRpt.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "");
            }


            cryRpt.Dispose(); cryRpt.Close();

        }
        //


    }//end
}