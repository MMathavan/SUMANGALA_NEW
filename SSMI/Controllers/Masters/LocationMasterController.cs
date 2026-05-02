using ATIMES_ERP.Data;
using ATIMES_ERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ATIMES_ERP.Controllers.Masters
{
    [SessionExpire]
    public class LocationMasterController : Controller
    {
        // GET: LocationMaster
        ApplicationDbContext context = new ApplicationDbContext();
        [Authorize(Roles = "LocationMasterIndex")]
        public ActionResult Index()
        {
            return View(context.locationmasters.ToList());//Loading Grid
        }
        //public JsonResult GetAjaxData(JQueryDataTableParamModel param)
        //{
        //    using (var e = new NATIMES_ERPEntities())
        //    {
        //        var totalRowsCount = new System.Data.Entity.Core.Objects.ObjectParameter("TotalRowsCount", typeof(int));
        //        var filteredRowsCount = new System.Data.Entity.Core.Objects.ObjectParameter("FilteredRowsCount", typeof(int));
        //        var data = e.pr_SearchLocationMaster(param.sSearch,
        //                                        Convert.ToInt32(Request["iSortCol_0"]),
        //                                        Request["sSortDir_0"],
        //                                        param.iDisplayStart,
        //                                        param.iDisplayStart + param.iDisplayLength,
        //                                        totalRowsCount,
        //                                        filteredRowsCount);

        //        var aaData = data.Select(d => new { LOCTCODE = d.LOCTCODE, LOCTDESC = d.LOCTDESC, STATEDESC = d.STATEDESC, DISPSTATUS = d.DISPSTATUS.ToString(), LOCTID = d.LOCTID.ToString() }).ToArray();
        //        return Json(new
        //        {
        //            //sEcho = param.sEcho,
        //            data = aaData
        //            //iTotalRecords = Convert.ToInt32(totalRowsCount.Value),
        //            //iTotalDisplayRecords = Convert.ToInt32(filteredRowsCount.Value)
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //----------------------Initializing Form--------------------------//
        [Authorize(Roles = "LocationMasterCreate")]
        public ActionResult Form(int? id = 0)
        {
            LocationMaster tab = new LocationMaster();
            tab.LOCTID = 0;
            List<SelectListItem> selectedDISPSTATUS = new List<SelectListItem>();
            SelectListItem selectedItem = new SelectListItem { Text = "Disabled", Value = "1", Selected = false };
            selectedDISPSTATUS.Add(selectedItem);
            selectedItem = new SelectListItem { Text = "Enabled", Value = "0", Selected = true };
            selectedDISPSTATUS.Add(selectedItem);
            ViewBag.DISPSTATUS = selectedDISPSTATUS;

            ViewBag.STATEID = new SelectList(context.statemasters, "STATEID", "STATEDESC");

            // IMP
            if (id == -1)
                ViewBag.msg = "<div class='msg'>Record Successfully Saved</div>";
            if (id != 0 && id != -1)  // IMP
            {
                tab = context.locationmasters.Find(id);

                List<SelectListItem> selectedDISPSTATUS1 = new List<SelectListItem>();
                if (Convert.ToInt32(tab.DISPSTATUS) == 1)
                {
                    SelectListItem selectedItem31 = new SelectListItem { Text = "Disabled", Value = "1", Selected = true };
                    selectedDISPSTATUS1.Add(selectedItem31);
                    selectedItem31 = new SelectListItem { Text = "Enabled", Value = "0", Selected = false };
                    selectedDISPSTATUS1.Add(selectedItem31);
                    ViewBag.DISPSTATUS = selectedDISPSTATUS1;
                }
                ViewBag.STATEID = new SelectList(context.statemasters, "STATEID", "STATEDESC", tab.STATEID);
            }
            return View(tab);
        }//End of Form
        //--------------------------Insert or Modify data------------------------//
        public void savedata(LocationMaster tab)
        {
            tab.CUSRID = Session["CUSRID"].ToString();
            tab.LMUSRID = 1;
            tab.PRCSDATE = DateTime.Now;
            var s = tab.LOCTDESC;//...ProperCase
            s = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());//end
            tab.LOCTDESC = s;
            if ((tab.LOCTID).ToString() != "0")
            {
                context.Entry(tab).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            else
            {
                context.locationmasters.Add(tab);
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
        }
        //End of savedata
        //------------------------Delete Record----------//
        [Authorize(Roles = "LocationMasterDelete")]
        public void Del()
        {
            String id = Request.Form.Get("id");
            //    String fld = Request.Form.Get("fld");
            //    String temp = Delete_fun.delete_check1(fld, id);
            //    if (temp.Equals("PROCEED"))
            //    {
            LocationMaster locationmasters = context.locationmasters.Find(Convert.ToInt32(id));
            context.locationmasters.Remove(locationmasters);
            context.SaveChanges();
            Response.Write("Deleted Successfully ...");
        }

    }
}