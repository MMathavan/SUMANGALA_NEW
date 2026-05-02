using ATIMES_ERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ATIMES_ERP.Data;
using ATIMES_ERP.Helper;

namespace ATIMES_ERP.Controllers.Masters
{
    [SessionExpire]
    public class DesginationMasterController : Controller
    {
        // GET: DesginationMaster
        ApplicationDbContext context = new ApplicationDbContext();
        [Authorize(Roles = "DesignationMasterIndex")]
        public ActionResult Index()
        {
            return View(context.desginationmasters.ToList());
        }


        public JsonResult GetAjaxData(JQueryDataTableParamModel param)
        {
            using (var e = new NATIMES_ERPEntities())
            {
                var totalRowsCount = new System.Data.Entity.Core.Objects.ObjectParameter("TotalRowsCount", typeof(int));
                var filteredRowsCount = new System.Data.Entity.Core.Objects.ObjectParameter("FilteredRowsCount", typeof(int));

                var data = e.pr_SearchDesignationMaster(param.sSearch,
                                                Convert.ToInt32(Request["iSortCol_0"]),
                                                Request["sSortDir_0"],
                                                param.iDisplayStart,
                                                param.iDisplayStart + param.iDisplayLength,
                                                totalRowsCount,
                                                filteredRowsCount);
                var aaData = data.Select(d => new { DSGNCODE = d.DSGNCODE, DSGNDESC = d.DSGNDESC, DISPSTATUS = d.DISPSTATUS.ToString(), DSGNID = d.DSGNID.ToString() }).ToArray();
                return Json(new
                {
                    //sEcho = param.sEcho,
                    data = aaData
                    //iTotalRecords = Convert.ToInt32(totalRowsCount.Value),
                    //iTotalDisplayRecords = Convert.ToInt32(filteredRowsCount.Value)
                }, JsonRequestBehavior.AllowGet);
            }
        }


        //----------------Initializing Form-----------------------//
        [Authorize(Roles = "DesignationMasterCreate")]
        public ActionResult Form(int? id = 0)
        {
            DesignationMaster tab = new DesignationMaster();

            List<SelectListItem> selectedDISPSTATUS = new List<SelectListItem>();
            SelectListItem selectedItem = new SelectListItem { Text = "Disabled", Value = "1", Selected = false };
            selectedDISPSTATUS.Add(selectedItem);
            selectedItem = new SelectListItem { Text = "Enabled", Value = "0", Selected = true };
            selectedDISPSTATUS.Add(selectedItem);
            ViewBag.DISPSTATUS = selectedDISPSTATUS;
            tab.DSGNID = 0;

            // IMP
            if (id == -1)
                ViewBag.msg = "<div class='msg'>Record Successfully Saved</div>";


            if (id != 0 && id != -1)  // IMP  
            {

                tab = context.desginationmasters.Find(id);
                List<SelectListItem> selectedDISPSTATUS1 = new List<SelectListItem>();
                if (Convert.ToInt32(tab.DISPSTATUS) == 1)
                {
                    SelectListItem selectedItem3 = new SelectListItem { Text = "Disabled", Value = "1", Selected = true };
                    selectedDISPSTATUS1.Add(selectedItem3);
                    selectedItem3 = new SelectListItem { Text = "Enabled", Value = "0", Selected = false };
                    selectedDISPSTATUS1.Add(selectedItem3);
                    ViewBag.DISPSTATUS = selectedDISPSTATUS1;
                }
            }
            return View(tab);
        }//End of Form



        public void savedata(DesignationMaster tab)
        {
            tab.CUSRID = Session["CUSRID"].ToString();
            tab.LMUSRID = 1;
            tab.PRCSDATE = DateTime.Now;
            var s = tab.DSGNDESC;//...ProperCase
            s = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());//end
            tab.DSGNDESC = s;

            if ((tab.DSGNID).ToString() != "0")
            {
                context.Entry(tab).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            else
            {
                context.desginationmasters.Add(tab);
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

        }//---------End  

        //------------------------Delete Record----------//
        [Authorize(Roles = "DesignationMasterDelete")]
        public void Del()
        {
            String id = Request.Form.Get("id");
            String fld = Request.Form.Get("fld");
            String temp = Delete_fun.delete_check1(fld, id);
            if (temp.Equals("PROCEED"))
            {
                DesignationMaster designationmasters = context.desginationmasters.Find(Convert.ToInt32(id));
                context.desginationmasters.Remove(designationmasters);
                context.SaveChanges();
                Response.Write("Deleted Successfully ...");
            }

            else
                Response.Write(temp);
        }//End of Delete

        //
    }
}