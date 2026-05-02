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
    public class PackingMasterController : Controller
    {
        // GET: PackingMaster
        ApplicationDbContext context = new ApplicationDbContext();

        [Authorize(Roles = "PackingMasterIndex")]
        public ActionResult Index()
        {
            return View(context.packingmasters.ToList());//---Loading Grid
        }


        //public JsonResult GetAjaxData(JQueryDataTableParamModel param)
        //{
        //    using (var e = new NATIMES_ERPEntities())
        //    {
        //        var totalRowsCount = new System.Data.Entity.Core.Objects.ObjectParameter("TotalRowsCount", typeof(int));
        //        var filteredRowsCount = new System.Data.Entity.Core.Objects.ObjectParameter("FilteredRowsCount", typeof(int));

        //        var data = e.pr_SearchPackingMaster(param.sSearch,
        //                                        Convert.ToInt32(Request["iSortCol_0"]),
        //                                        Request["sSortDir_0"],
        //                                        param.iDisplayStart,
        //                                        param.iDisplayStart + param.iDisplayLength,
        //                                        totalRowsCount,
        //                                        filteredRowsCount);
        //        var aaData = data.Select(d => new { PACKMCODE = d.PACKMCODE, PACKMDESC = d.PACKMDESC, PACKMNOU = d.PACKMNOU.ToString(), DISPSTATUS = d.DISPSTATUS.ToString(), PACKMID = d.PACKMID.ToString() }).ToArray();
        //        return Json(new
        //        {
        //            //sEcho = param.sEcho,
        //            data = aaData
        //            //iTotalRecords = Convert.ToInt32(totalRowsCount.Value),
        //            //iTotalDisplayRecords = Convert.ToInt32(filteredRowsCount.Value)
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [Authorize(Roles = "PackingMasterEdit")]
        public void Edit(int id)
        {
            Response.Redirect(@Url.Action("Form", "PackingMaster") + "/" + id);
        }


        //----------------Initializing Form-----------------------//
        [Authorize(Roles = "PackingMasterCreate")]
        public ActionResult Form(int? id = 0)
        {
            PackingMaster tab = new PackingMaster();
            List<SelectListItem> selectedDISPSTATUS = new List<SelectListItem>();
            SelectListItem selectedItem = new SelectListItem { Text = "Disabled", Value = "1", Selected = false };
            selectedDISPSTATUS.Add(selectedItem);
            selectedItem = new SelectListItem { Text = "Enabled", Value = "0", Selected = true };
            selectedDISPSTATUS.Add(selectedItem);
            ViewBag.DISPSTATUS = selectedDISPSTATUS;
            tab.PACKMID = 0;

            // IMP
            if (id == -1)
                ViewBag.msg = "<div class='msg'>Record Successfully Saved</div>";


            if (id != 0 && id != -1)  // IMP  
            {

                tab = context.packingmasters.Find(id);
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
         //-------------------Insert or Modify data-------------//

        public void savedata(PackingMaster tab)
        {

            var s = tab.PACKMDESC;//...ProperCase
            s = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());//end
            tab.PACKMDESC = s;
            //tab.MTRLGCODE = s.Substring(0, 3);
            tab.CUSRID = Session["CUSRID"].ToString();
            tab.LMUSRID = 1;
            tab.PRCSDATE = DateTime.Now;
            if ((tab.PACKMID).ToString() != "0")
            {
                context.Entry(tab).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            else
            {
                context.packingmasters.Add(tab);
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




        }//--End of Savedata
         //---------------Delete Record-------------//
        [Authorize(Roles = "PackingMasterDelete")]
        public void Del()
        {
            String id = Request.Form.Get("id");
            String fld = Request.Form.Get("fld");
            String temp = Delete_fun.delete_check1(fld, id);
            if (temp.Equals("PROCEED"))
            {
                PackingMaster packingmasters = context.packingmasters.Find(Convert.ToInt32(id));
                context.packingmasters.Remove(packingmasters);
                context.SaveChanges();
                Response.Write("Deleted Successfully ...");
            }
            else
                Response.Write(temp);
        }//..........end of delete function


        //end
    }
}