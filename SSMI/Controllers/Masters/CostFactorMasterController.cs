using ATIMES_ERP.Helper;
using ATIMES_ERP.Models;
using ATIMES_ERP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ATIMES_ERP.Controllers.Masters
{
    [SessionExpire]
    public class CostFactorMasterController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        //
        // GET: /CostFactorMaster/
        [Authorize(Roles = "CostFactorMasterIndex")]
        public ActionResult Index()
        {
            return View(context.CostFactorMaster.ToList());//Loading Grid
        }
        public JsonResult GetAjaxData(JQueryDataTableParamModel param)
        {
            using (var e = new NATIMES_ERPEntities())
            {
                var totalRowsCount = new System.Data.Entity.Core.Objects.ObjectParameter("TotalRowsCount", typeof(int));
                var filteredRowsCount = new System.Data.Entity.Core.Objects.ObjectParameter("FilteredRowsCount", typeof(int));
                var data = e.pr_SearchCostFactorMaster(param.sSearch,
                                                Convert.ToInt32(Request["iSortCol_0"]),
                                                Request["sSortDir_0"],
                                                param.iDisplayStart,
                                                param.iDisplayStart + param.iDisplayLength,
                                                totalRowsCount,
                                                filteredRowsCount);

                //var aaData = data.Select(d => new string[] { d.CFDESC, d.CFEXPR.ToString(), d.CFMODE, d.DISPSTATUS, d.CFID.ToString() }).ToArray();
                var aaData = data.Select(d => new { CFDESC = d.CFDESC, CFEXPR = d.CFEXPR, CFMODE = d.CFMODE, DISPSTATUS = d.DISPSTATUS.ToString(), CFID = d.CFID.ToString() }).ToArray();
                return Json(new
                {
                    sEcho = param.sEcho,
                    aaData = aaData,
                    iTotalRecords = Convert.ToInt32(totalRowsCount.Value),
                    iTotalDisplayRecords = Convert.ToInt32(filteredRowsCount.Value)
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "CostFactorMasterEdit")]
        public void Edit(int id)
        {
            Response.Redirect(@Url.Action("Form", "CostFactorMaster") + "/" + id);
        }

        //-------------Initializing Form-------------//
        [Authorize(Roles = "CostFactorMasterCreate")]
        public ActionResult Form(int? id = 0)
        {
            if (Convert.ToInt32(System.Web.HttpContext.Current.Session["compyid"]) == 0) { return RedirectToAction("Login", "Account"); }
            CostFactorMaster tab = new CostFactorMaster();
            tab.CFID = 0;
            tab.PRCSDATE = DateTime.Now;
            //------------------------Dropdown List------------------------------------------------//
            ViewBag.DORDRID = new SelectList(context.displayordermasters, "DORDRID", "DORDRDESC");
            ViewBag.ACHEADID = new SelectList(context.accountheadmasters, "ACHEADID", "ACHEADDESC");
            ViewBag.CFOPTN = new SelectList(context.costfactortaxtypes, "CFOPTN", "CFOPTNDESC");
            //-----------------------------DISPSTATUS -----------------------------//
            List<SelectListItem> selectedDISPSTATUS = new List<SelectListItem>();
            SelectListItem selectedItem = new SelectListItem { Text = "Disabled", Value = "1", Selected = false };
            selectedDISPSTATUS.Add(selectedItem);
            selectedItem = new SelectListItem { Text = "Enabled", Value = "0", Selected = true };
            selectedDISPSTATUS.Add(selectedItem);
            ViewBag.DISPSTATUS = selectedDISPSTATUS;
            //-----------------------------CFMODE -----------------------------//
            List<SelectListItem> selectedCFMODE = new List<SelectListItem>();
            SelectListItem selectedItem1 = new SelectListItem { Text = "ADD", Value = "0", Selected = false };
            selectedCFMODE.Add(selectedItem1);
            selectedItem1 = new SelectListItem { Text = "DEDUCT", Value = "1", Selected = false };
            selectedCFMODE.Add(selectedItem1);
            ViewBag.CFMODE = selectedCFMODE;
            //-------------/-----------------CFTYPE --------------------------------//
            List<SelectListItem> selectedCFTYPE = new List<SelectListItem>();
            SelectListItem selectedItem2 = new SelectListItem { Text = "VALUE", Value = "0", Selected = false };
            selectedCFTYPE.Add(selectedItem2);
            selectedItem2 = new SelectListItem { Text = "%", Value = "1", Selected = true };
            selectedCFTYPE.Add(selectedItem2);
            ViewBag.CFTYPE = selectedCFTYPE;
            //--------------------------------CFNATR--------------------------------------//
            List<SelectListItem> selectedCFNATR = new List<SelectListItem>();
            SelectListItem selectedItem3 = new SelectListItem { Text = "INCLUSIVE", Value = "0", Selected = false };
            selectedCFNATR.Add(selectedItem3);
            selectedItem3 = new SelectListItem { Text = "EXCLUSIVE", Value = "1", Selected = true };
            selectedCFNATR.Add(selectedItem3);
            ViewBag.CFNATR = selectedCFNATR;
            // IMP
            if (id == -1)
                ViewBag.msg = "<div class='msg'>Record Successfully Saved</div>";


            if (id != 0 && id != -1)  // IMP
            {
                tab = context.CostFactorMaster.Find(id);
                //------------------------------Selected values in Dropdown List--------------------------------//
                ViewBag.DORDRID = new SelectList(context.displayordermasters, "DORDRID", "DORDRDESC", tab.DORDRID);
                ViewBag.ACHEADID = new SelectList(context.accountheadmasters, "ACHEADID", "ACHEADDESC", tab.ACHEADID);
                ViewBag.CFOPTN = new SelectList(context.costfactortaxtypes, "CFOPTN", "CFOPTNDESC", tab.CFOPTN);
                List<SelectListItem> selectedDISPSTATUS1 = new List<SelectListItem>();
                if (Convert.ToInt32(tab.DISPSTATUS) == 1)
                {
                    SelectListItem selectedItem31 = new SelectListItem { Text = "Disabled", Value = "1", Selected = true };
                    selectedDISPSTATUS1.Add(selectedItem31);
                    selectedItem31 = new SelectListItem { Text = "Enabled", Value = "0", Selected = false };
                    selectedDISPSTATUS1.Add(selectedItem31);
                    ViewBag.DISPSTATUS = selectedDISPSTATUS1;
                }
                else
                {
                    SelectListItem selectedItem31 = new SelectListItem { Text = "Disabled", Value = "1", Selected = false };
                    selectedDISPSTATUS1.Add(selectedItem31);
                    selectedItem31 = new SelectListItem { Text = "Enabled", Value = "0", Selected = true };
                    selectedDISPSTATUS1.Add(selectedItem31);
                    ViewBag.DISPSTATUS = selectedDISPSTATUS1;
                }
                //---------------------------------------------------CFMODE DROPDOWN
                List<SelectListItem> selectedCFMODE1 = new List<SelectListItem>();
                if (Convert.ToInt32(tab.CFMODE) == 0)
                {
                    SelectListItem selectedItem11 = new SelectListItem { Text = "ADD", Value = "0", Selected = true };
                    selectedCFMODE1.Add(selectedItem11);
                    selectedItem11 = new SelectListItem { Text = "DEDUCT", Value = "1", Selected = false };
                    selectedCFMODE1.Add(selectedItem11);
                    ViewBag.CFMODE = selectedCFMODE1;
                }
                else
                {
                    SelectListItem selectedItem11 = new SelectListItem { Text = "ADD", Value = "0", Selected = false };
                    selectedCFMODE1.Add(selectedItem11);
                    selectedItem11 = new SelectListItem { Text = "DEDUCT", Value = "1", Selected = true };
                    selectedCFMODE1.Add(selectedItem11);
                    ViewBag.CFMODE = selectedCFMODE1;
                }
                //----------------------------------------------------CFTYPE DROPDOWN
                List<SelectListItem> selectedCFTYPE1 = new List<SelectListItem>();
                if (Convert.ToInt32(tab.CFTYPE) == 0)
                {
                    SelectListItem selectedItem22 = new SelectListItem { Text = "VALUE", Value = "0", Selected = true };
                    selectedCFTYPE1.Add(selectedItem22);
                    selectedItem22 = new SelectListItem { Text = "%", Value = "1", Selected = false };
                    selectedCFTYPE1.Add(selectedItem22);
                    ViewBag.CFTYPE = selectedCFTYPE1;
                }

                //------------------------------------------------------CFNATR
                List<SelectListItem> selectedCFNATR1 = new List<SelectListItem>();
                if (Convert.ToInt32(tab.CFNATR) == 0)
                {
                    SelectListItem selectedItem32 = new SelectListItem { Text = "INCLUSIVE", Value = "0", Selected = true };
                    selectedCFNATR1.Add(selectedItem32);
                    selectedItem32 = new SelectListItem { Text = "EXCLUSIVE", Value = "1", Selected = false };
                    selectedCFNATR1.Add(selectedItem32);
                    ViewBag.CFNATR = selectedCFNATR1;
                }
            }
            return View(tab);
        }//End of Form
        //---------------------Insert or Modify data------------------//
        public void savedata(CostFactorMaster tab)
        {
            var s = tab.CFDESC;//...ProperCase
            //s = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());//end
            //tab.CFDESC = s;
            if ((tab.CFID).ToString() != "0")
            {
                context.Entry(tab).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            else
            {
                context.CostFactorMaster.Add(tab);
                context.SaveChanges();
            }

            // IMP
            if (Request.Form.Get("continue") == null)
            {
                Response.Redirect("index");
            }
            else
            {
                Response.Redirect("Form/-1");
            }

        }//End of savedata
        //------------------------Delete Record----------//
        [Authorize(Roles = "CostFactorMasterDelete")]
        public void Del()
        {
            String id = Request.Form.Get("id");
            String fld = Request.Form.Get("fld");
            String temp = Delete_fun.delete_check1(fld, id);
            if (temp.Equals("PROCEED"))
            {
                CostFactorMaster CostFactorMaster = context.CostFactorMaster.Find(Convert.ToInt32(id));
                context.CostFactorMaster.Remove(CostFactorMaster);
                context.SaveChanges();
                Response.Write("Deleted Successfully ...");
            }
            else
                Response.Write(temp);
        }//End of Delete
    }//End of Class
}