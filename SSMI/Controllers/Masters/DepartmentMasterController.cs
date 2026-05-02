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
    public class DepartmentMasterController : Controller
    {
        // GET: DepartmentMaster
        ApplicationDbContext context = new ApplicationDbContext();

        [Authorize(Roles = "DepartmentMasterIndex")]
        public ActionResult Index()
        {
            return View(context.departmentmasters.ToList());
        }


        public JsonResult GetAjaxData(JQueryDataTableParamModel param)
        {
            using (var e = new NATIMES_ERPEntities())
            {
                var totalRowsCount = new System.Data.Entity.Core.Objects.ObjectParameter("TotalRowsCount", typeof(int));
                var filteredRowsCount = new System.Data.Entity.Core.Objects.ObjectParameter("FilteredRowsCount", typeof(int));

                var data = e.pr_SearchDepartmentMaster(param.sSearch,
                                                Convert.ToInt32(Request["iSortCol_0"]),
                                                Request["sSortDir_0"],
                                                param.iDisplayStart,
                                                param.iDisplayStart + param.iDisplayLength,
                                                totalRowsCount,
                                                filteredRowsCount);
                //var aaData = data.Select(d => new string[] { d.DEPTCODE, d.DEPTDESC, d.DISPSTATUS, d.DEPTID.ToString() }).ToArray();
                var aaData = data.Select(d => new { DEPTCODE = d.DEPTCODE, DEPTDESC = d.DEPTDESC, DISPSTATUS = d.DISPSTATUS.ToString(), DEPTID = d.DEPTID.ToString() }).ToArray();
                return Json(new
                {
                    //sEcho = param.sEcho,
                    data = aaData
                    //iTotalRecords = Convert.ToInt32(totalRowsCount.Value),
                    //iTotalDisplayRecords = Convert.ToInt32(filteredRowsCount.Value)
                }, JsonRequestBehavior.AllowGet);
            }
        }


        [Authorize(Roles = "DepartmentMasterEdit")]
        public void Edit(int id)
        {
            Response.Redirect(@Url.Action("Form", "DepartmentMaster") + "/" + id);
        }



        [Authorize(Roles = "DepartmentMasterCreate")]
        public ActionResult Form(int? id = 0)
        {
            DepartmentMaster tab = new DepartmentMaster();
            List<SelectListItem> selectedDISPSTATUS = new List<SelectListItem>();
            SelectListItem selectedItem = new SelectListItem { Text = "Disabled", Value = "1", Selected = false };
            selectedDISPSTATUS.Add(selectedItem);
            selectedItem = new SelectListItem { Text = "Enabled", Value = "0", Selected = true };
            selectedDISPSTATUS.Add(selectedItem);
            ViewBag.DISPSTATUS = selectedDISPSTATUS;
            tab.DEPTID = 0;
            if (id == -1)
                ViewBag.msg = "<div class='msg'>Record Successfully Saved</div>";
            if (id != 0 && id != -1)  // IMP
            {
                tab = context.departmentmasters.Find(id);
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
        }//...........end of form
        //.............insert and update
        public void savedata(DepartmentMaster tab)
        {
            if (Session["CUSRID"] != null) tab.CUSRID = Session["CUSRID"].ToString(); else tab.CUSRID = "0";
            tab.LMUSRID = 1;
            tab.PRCSDATE = DateTime.Now;
            if ((tab.DEPTID).ToString() != "0")
            {
                context.Entry(tab).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            else
            {
                context.departmentmasters.Add(tab);
                context.SaveChanges();
            }
            if (Request.Form.Get("continue") == null)
            {
                Response.Redirect("index");
            }
            else
            {
                Response.Redirect("Form/-1");
            }
        }//...........end of save function
         //................delete function

        [Authorize(Roles = "DepartmentMasterDelete")]
        public void Del()
        {
            String id = Request.Form.Get("id");
            String fld = Request.Form.Get("fld");
            String temp = Delete_fun.delete_check1(fld, id);
            if (temp.Equals("PROCEED"))
            {
                DepartmentMaster departmentmasters = context.departmentmasters.Find(Convert.ToInt32(id));
                context.departmentmasters.Remove(departmentmasters);
                context.SaveChanges();
                Response.Write("Deleted Successfully ...");
            }
            else
                Response.Write(temp);
        }//..........end of delete function
    }//..............end of class
}//..................end of namespaceespace