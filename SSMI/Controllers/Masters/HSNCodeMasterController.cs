using ATIMES_ERP.Data;
using ATIMES_ERP.Helper;
using ATIMES_ERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ATIMES_ERP.Controllers.Masters
{
    [SessionExpire]
    public class HSNCodeMasterController : Controller
    {
        // GET: HSNCodeMaster
        ApplicationDbContext context = new ApplicationDbContext();
        [Authorize(Roles = "HSNCodeMasterIndex")]
        public ActionResult Index()
        {
            return View(context.HSNCodeMaster.ToList());//Loading Grid
        }

        public JsonResult GetAjaxData(JQueryDataTableParamModel param)
        {
            using (var e = new ATIMESEntities())
            {
                var totalRowsCount = new System.Data.Entity.Core.Objects.ObjectParameter("TotalRowsCount", typeof(int));
                var filteredRowsCount = new System.Data.Entity.Core.Objects.ObjectParameter("FilteredRowsCount", typeof(int));
                var data = e.pr_SearchHSNCodeMaster(param.sSearch,
                                                Convert.ToInt32(Request["iSortCol_0"]),
                                                Request["sSortDir_0"],
                                                param.iDisplayStart,
                                                param.iDisplayStart + param.iDisplayLength,
                                                totalRowsCount,
                                                filteredRowsCount);

                //var aaData = data.Select(d => new string[] { d.HSNCODE, d.HSNDESC, d.TAXEXPRN.ToString(), d.CGSTEXPRN.ToString(), d.SGSTEXPRN.ToString(), d.IGSTEXPRN.ToString(), d.DISPSTATUS, d.HSNID.ToString() }).ToArray();
                var aaData = data.Select(d => new { HSNCODE = d.HSNCODE, HSNDESC = d.HSNDESC, TAXEXPRN = d.TAXEXPRN.ToString(), CGSTEXPRN = d.CGSTEXPRN.ToString(), SGSTEXPRN = d.SGSTEXPRN.ToString(), IGSTEXPRN = d.IGSTEXPRN.ToString(), DISPSTATUS = d.DISPSTATUS.ToString(), HSNID = d.HSNID.ToString() }).ToArray();
                return Json(new
                {
                    //sEcho = param.sEcho,
                    data = aaData
                    //iTotalRecords = Convert.ToInt32(totalRowsCount.Value),
                    //iTotalDisplayRecords = Convert.ToInt32(filteredRowsCount.Value)
                }, JsonRequestBehavior.AllowGet);
            }
        }

        //-------------Initializing Form-------------//
        [Authorize(Roles = "HSNCodeMasterCreate")]
        public ActionResult Form(int? id = 0)
        {
            if (Convert.ToInt32(System.Web.HttpContext.Current.Session["compyid"]) == 0) { return RedirectToAction("Login", "Account"); }
            HSNCodeMaster tab = new HSNCodeMaster();
            tab.HSNID = 0;
            tab.PRCSDATE = DateTime.Now;
            //-----------------------------DISPSTATUS -----------------------------//
            List<SelectListItem> selectedDISPSTATUS = new List<SelectListItem>();
            SelectListItem selectedItem = new SelectListItem { Text = "Disabled", Value = "1", Selected = false };
            selectedDISPSTATUS.Add(selectedItem);
            selectedItem = new SelectListItem { Text = "Enabled", Value = "0", Selected = true };
            selectedDISPSTATUS.Add(selectedItem);
            ViewBag.DISPSTATUS = selectedDISPSTATUS;
            // IMP
            if (id == -1)
                ViewBag.msg = "<div class='msg'>Record Successfully Saved</div>";


            if (id != 0 && id != -1)  // IMP
            {
                tab = context.HSNCodeMaster.Find(id);
                //------------------------------Selected values in Dropdown List--------------------------------//
                List<SelectListItem> selectedDISPSTATUS1 = new List<SelectListItem>();
                if (Convert.ToInt32(tab.DISPSTATUS) == 1)
                {
                    SelectListItem selectedItem31 = new SelectListItem { Text = "Disabled", Value = "1", Selected = true };
                    selectedDISPSTATUS1.Add(selectedItem31);
                    selectedItem31 = new SelectListItem { Text = "Enabled", Value = "0", Selected = false };
                    selectedDISPSTATUS1.Add(selectedItem31);
                    ViewBag.DISPSTATUS = selectedDISPSTATUS1;
                }
            }
            return View(tab);
        }//End of Form


        //---------------------Insert or Modify data------------------//
        public void savedata(HSNCodeMaster tab)
        {
            tab.CUSRID = Session["CUSRID"].ToString();
            tab.LMUSRID = 1;
            tab.PRCSDATE = DateTime.Now;
            tab.IGSTEXPRN = tab.TAXEXPRN;

            var s = tab.HSNDESC;//...ProperCase
            //s = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());//end
            //tab.CFDESC = s;
            if ((tab.HSNID).ToString() != "0")
            {
                context.Entry(tab).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            else
            {
                context.HSNCodeMaster.Add(tab);
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
        [Authorize(Roles = "HSNCodeMasterDelete")]
        public void Del()
        {
            String id = Request.Form.Get("id");
            String fld = Request.Form.Get("fld");
            String temp = Delete_fun.delete_check1(fld, id);
            if (temp.Equals("PROCEED"))
            {
                HSNCodeMaster HSNCodeMaster = context.HSNCodeMaster.Find(Convert.ToInt32(id));
                context.HSNCodeMaster.Remove(HSNCodeMaster);
                context.SaveChanges();
                Response.Write("Deleted Successfully ...");
            }
            else
                Response.Write(temp);
        }//End of Delete

    }
}