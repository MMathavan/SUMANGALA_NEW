using ATIMES_ERP.Helper;
using ATIMES_ERP.Data;
using ATIMES_ERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data.SqlClient;
using System.Configuration;

namespace ATIMES_ERP.Controllers.Masters
{
    [SessionExpire]
    public class RateCardMasterController : Controller
    {
        // GET: RateCardMaster
        ApplicationDbContext context = new ApplicationDbContext();
        NATIMES_ERPEntities db = new NATIMES_ERPEntities();

        [Authorize(Roles = "RateCardMasterIndex")]
        public ActionResult Index()
        {
            if (Convert.ToInt32(System.Web.HttpContext.Current.Session["compyid"]) == 0 || Convert.ToInt32(System.Web.HttpContext.Current.Session["BRNCHID"]) == 0) { return RedirectToAction("Login", "Account"); }
            //if (string.IsNullOrEmpty(Session["SDATE"] as string))
            //{
            //    Session["SDATE"] = DateTime.Now.ToString("dd-MM-yyyy");
            //    Session["EDATE"] = DateTime.Now.ToString("dd-MM-yyyy");
            //    //Session["S_BRNCHID"] = 0;
            //}
            //else
            //{
            //    if (Request.Form.Get("from") != null)
            //    {
            //        Session["SDATE"] = Request.Form.Get("from");
            //        Session["EDATE"] = Request.Form.Get("to");
            //        //Session["S_BRNCHID"] = Request.Form.Get("S_BRNCHID");
            //    }
            //}
            //DateTime sd = Convert.ToDateTime(System.Web.HttpContext.Current.Session["SDATE"]).Date;
            //DateTime ed = Convert.ToDateTime(System.Web.HttpContext.Current.Session["EDATE"]).Date;

            if (string.IsNullOrEmpty(Session["F_CATEID"] as string))
            {

                Session["F_CATEID"] = "0";
            }
            else
            {
                var a = Request.Form.Get("CATEID");
                Session["F_CATEID"] = Request.Form.Get("CATEID");

            }


            ViewBag.CATEID = new SelectList(context.statemasters.Where(x => x.STATEID > 0 &&  x.DISPSTATUS == 0).OrderBy(x => x.STATEDESC), "STATEID", "STATEDESC", Convert.ToInt32(Session["F_CATEID"]));

            return View();
        }

        //public JsonResult GetAjaxData(JQueryDataTableParamModel param)
        //{
        //    using (var e = new NATIMES_ERPEntities())
        //    {

        //        var totalRowsCount = new System.Data.Entity.Core.Objects.ObjectParameter("TotalRowsCount", typeof(int));
        //        var filteredRowsCount = new System.Data.Entity.Core.Objects.ObjectParameter("FilteredRowsCount", typeof(int));
        //        var data = e.pr_SearchRateCardMaster(param.sSearch,
        //                                        Convert.ToInt32(Request["iSortCol_0"]),
        //                                        Request["sSortDir_0"],
        //                                        param.iDisplayStart,
        //                                        param.iDisplayStart + param.iDisplayLength,
        //                                        totalRowsCount,
        //                                         filteredRowsCount,
        //                                         Convert.ToInt32(System.Web.HttpContext.Current.Session["compyid"]),
        //                                         Convert.ToInt32(Session["F_CATEID"]));
        //        var aaData = data.Select(d => new { TRANDATE = d.TRANDATE.Value.ToString("dd-MM-yyyy"), MTRLGDESC = d.MTRLGDESC, MTRLDESC = d.MTRLDESC, PRATE = d.PRATE.ToString(), RATECID = d.RATECID.ToString() }).ToArray();
        //        return Json(new
        //        {
        //            //sEcho = param.sEcho,
        //            data = aaData
        //            //iTotalRecords = Convert.ToInt32(totalRowsCount.Value),
        //            //iTotalDisplayRecords = Convert.ToInt32(filteredRowsCount.Value)
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [Authorize(Roles = "RateCardMasterCreate")]
        public ActionResult Form()
        {
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
            ViewBag.STATEID = new SelectList(context.statemasters.Where(x => x.DISPSTATUS == 0).OrderBy(x => x.STATEDESC), "STATEID", "STATEDESC");
            ViewBag.MTRLGID = new SelectList(context.materialgroupmasters.Where(x => x.DISPSTATUS == 0).OrderBy(x => x.MTRLGDESC), "MTRLGID", "MTRLGDESC");
            //ViewBag.Category = new SelectList("");
            //ViewBag.Subcategory = new SelectList("");
            return View();
        }

        public void savedata(FormCollection frm)
        {
            try
            {
                RateCardMaster tab = new RateCardMaster();
                string[] cateid = frm.GetValues("MSTATEID");
                string[] mtrlid = frm.GetValues("MTRLID");
                string[] purchaserate = frm.GetValues("ITEMPRATE");
                //string[] salesrate = frm.GetValues("ITEMSRATE");
                tab.PRCSDATE = DateTime.Now;
                tab.TARIFFMID = 2;
                tab.RATETYPE = 0;
                tab.CATEID = Convert.ToInt32(cateid[0]);// Convert.ToInt32(frm.GetValues("CATEID"));
                tab.TRANDATE = Convert.ToDateTime(Request.Form.Get("TRANDATE"));
                tab.CUSRID = Session["CUSRID"].ToString();
                tab.COMPYID = Convert.ToInt32(Session["compyid"]);

                for (int i = 0; i < mtrlid.Count(); i++)
                {
                    if (purchaserate[i] != "0.00")//if (purchaserate[i] != "0.00" || salesrate[i] != "0.00")
                    {
                        tab.ITEMID = Convert.ToInt32(mtrlid[i]);
                        tab.PRATE = Convert.ToDecimal(purchaserate[i]);
                        tab.SRATE = Convert.ToDecimal(purchaserate[i]);

                        context.ratecardmasters.Add(tab); context.SaveChanges();
                    }
                }

                Response.Write("Saved Successfully");
            }
            catch (Exception e) { Response.Write(e.Message); }
        }

        //public string DisplayMaterialList(string id)
        //{
        //    var param = id.Split(';');
        //    var mtrlgid = Convert.ToInt32(param[0]);
        //    var cateid = Convert.ToInt32(param[1]);

        //    //string catequery = "", subcatequery = "";

        //    //int? itemtid;
        //    //if (param[1] != "") { itemtid = Convert.ToInt32(param[1]); catequery = "@ItemTId=" + itemtid + ""; } else { itemtid = null; catequery = "@ItemTId=null"; }

        //    //int? itemgid;
        //    //if (param[2] != "") { itemgid = Convert.ToInt32(param[2]); subcatequery = "@ItemGId=" + itemgid + ""; } else { itemgid = null; subcatequery = "@ItemGId=null"; }


        //    var html = "";

        //    //var rawquery = "PR_RATECARD_CTRL_ASSGN @MtrlGId = " + mtrlgid + "" ;// + "," + catequery + "," + subcatequery + "";
        //    var rawquery = "PR_RATECARD_CTRL_ASSGN @MtrlGId = " + mtrlgid + ", @CateId = " + cateid + "";
        //    var query = db.Database.SqlQuery<PR_RATECARD_CTRL_ASSGN_Result>(rawquery).ToList();

        //    foreach (var data in query)
        //    {
        //        html = html + "<tr><td class='col-lg-4'><input type='text' name='MTRLGID' value=" + data.MTRLGID + " class='hide' /><input type='text' name='MSTATEID' value=" + cateid + " class='hide' />" + data.MTRLGDESC + " </td>";
        //        html = html + "<td class='col-lg-4'><input type='text' name='MTRLID' value=" + data.MTRLID + " class='hide' />" + data.MTRLDESC + " </td>";
        //        html = html + "<td class='col-lg-2'><input type='text' name='ITEMPRATE' value='" + data.PRATE + "' class=' form-control ITEMPRATE' /></td>";
        //        html = html + "</tr>";
        //    }


        //    /* var sql = "select * from ItemMaster where ITEMBRNDID=" + brandid+"";
        //     List<ItemMaster> querylist = new List<ItemMaster>();
        //     if (param[1] != "")
        //     { sql = sql + " and ITEMTID=" + Convert.ToInt32(param[1]) + "";}
        //     if (param[2] != "")
        //     {sql = sql + " and ITEMGID=" + Convert.ToInt32(param[2]) + "";}

        //     querylist = context.Database.SqlQuery<ItemMaster>(sql).ToList();
        //     foreach(var data in querylist)
        //     {
        //         html = html + "<tr><td><input type='text' name='ITEMID' value=" + data.ITEMID + " class='hide' />" + data.ITEMMODEL + " </td><td><input type='text' name='ITEMPRATE' value=0 class='ITEMPRATE' /></td><td><input type='text' name='ITEMSRATE' value=0 class='ITEMSRATE' /></td></tr>";
        //     }*/

        //    return html;
        //}

        [Authorize(Roles = "RateCardMasterDelete")]
        public void Del()
        {
            String id = Request.Form.Get("id");
            RateCardMaster ratecardmasters = context.ratecardmasters.Find(Convert.ToInt32(id));
            context.ratecardmasters.Remove(ratecardmasters);
            context.SaveChanges();
            Response.Write("Deleted Successfully ...");
        }

        //end
    }
}