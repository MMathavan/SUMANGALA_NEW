using SSMI.Data;
using SSMI.Helper;
using SSMI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSMI.Controllers.Masters
{
    [SessionExpire]
    public class EmployeeMasterController : Controller
    {
        // GET: EmployeeMaster
        ApplicationDbContext context = new ApplicationDbContext();
        [Authorize(Roles = "EmployeeMasterIndex")]
        public ActionResult Index()
        {
            return View(context.employeemasters.ToList());
        }

        public JsonResult GetAjaxData(JQueryDataTableParamModel param)
        {
            using (var e = new SSMIEntities())
            {
                var totalRowsCount = new System.Data.Entity.Core.Objects.ObjectParameter("TotalRowsCount", typeof(int));
                var filteredRowsCount = new System.Data.Entity.Core.Objects.ObjectParameter("FilteredRowsCount", typeof(int));

                var data = e.pr_SearchEmployeeMaster(param.sSearch,
                                                Convert.ToInt32(Request["iSortCol_0"]),
                                                Request["sSortDir_0"],
                                                param.iDisplayStart,
                                                param.iDisplayStart + param.iDisplayLength,
                                                totalRowsCount,
                                                filteredRowsCount);
                var aaData = data.Select(d => new { CATENAME = d.CATENAME, CATEID = d.CATEID.ToString() }).ToArray();
                return Json(new
                {
                    //sEcho = param.sEcho,
                    data = aaData
                    //iTotalRecords = Convert.ToInt32(totalRowsCount.Value),
                    //iTotalDisplayRecords = Convert.ToInt32(filteredRowsCount.Value)
                }, JsonRequestBehavior.AllowGet);
            }
        }


        [Authorize(Roles = "EmployeeMasterEdit")]
        public void Edit(int id)
        {
            Response.Redirect(@Url.Action("Form", "EmployeeMaster") + "/" + id);
        }


        //form start

        [Authorize(Roles = "EmployeeMasterCreate")]
        public ActionResult Form(int? id = 0)
        {
            if (Convert.ToInt32(System.Web.HttpContext.Current.Session["compyid"]) == 0) { return RedirectToAction("Login", "Account"); }
            EmployeeMaster tab = new EmployeeMaster();

            List<SelectListItem> selectedDISPSTATUS = new List<SelectListItem>();
            SelectListItem selectedItem = new SelectListItem { Text = "Disabled", Value = "1", Selected = false };
            selectedDISPSTATUS.Add(selectedItem);
            selectedItem = new SelectListItem { Text = "Enabled", Value = "0", Selected = true };
            selectedDISPSTATUS.Add(selectedItem);
            ViewBag.DISPSTATUS = selectedDISPSTATUS;

            List<SelectListItem> selectedCATEGTYPE = new List<SelectListItem>();
            SelectListItem selectedcategtype = new SelectListItem { Text = "Male", Value = "0", Selected = false };
            selectedCATEGTYPE.Add(selectedcategtype);
            selectedcategtype = new SelectListItem { Text = "Female", Value = "1", Selected = false };
            selectedCATEGTYPE.Add(selectedcategtype);
            ViewBag.CATEGTYPE = selectedCATEGTYPE;

            //ViewBag.STATEID = new SelectList(context.statemasters.Where(x => x.DISPSTATUS == 0).OrderBy(x => x.STATEDESC), "STATEID ", "STATEDESC");
            ViewBag.LOCTID = new SelectList("");

            tab.CATEID = 0;
            if (id != 0)
            {
                tab = context.employeemasters.Find(id);
                List<SelectListItem> selectedDISPSTATUS1 = new List<SelectListItem>();
                if (Convert.ToInt32(tab.DISPSTATUS) == 1)
                {
                    SelectListItem selectedItem31 = new SelectListItem { Text = "Disabled", Value = "1", Selected = true };
                    selectedDISPSTATUS1.Add(selectedItem31);
                    selectedItem31 = new SelectListItem { Text = "Enabled", Value = "0", Selected = false };
                    selectedDISPSTATUS1.Add(selectedItem31);
                    ViewBag.DISPSTATUS = selectedDISPSTATUS1;
                }

                //List<SelectListItem> selectedCATESTYPE1 = new List<SelectListItem>();
                //if (Convert.ToInt32(tab.CATESTYPE) == 1)
                //{
                //    SelectListItem selectedstype = new SelectListItem { Text = "Random", Value = "1", Selected = true };
                //    selectedCATESTYPE1.Add(selectedstype);
                //    selectedstype = new SelectListItem { Text = "Authorised", Value = "0", Selected = false };
                //    selectedCATESTYPE1.Add(selectedstype);
                //    ViewBag.CATESTYPE = selectedCATESTYPE1;
                //}

                //ViewBag.LOCTID = new SelectList(context.locationmasters.Where(x => x.DISPSTATUS == 0).OrderBy(x => x.LOCTDESC), "LOCTID", "LOCTDESC", tab.LOCTID);
                //ViewBag.STATEID = new SelectList(context.statemasters.Where(x => x.DISPSTATUS == 0).OrderBy(x => x.STATEDESC), "STATEID", "STATEDESC", tab.STATEID);

            }
            return View(tab);
        }//...........end of form
         //for end


        public void savedata(EmployeeMaster tab)
        {
            if (Session["CUSRID"] != null)
                tab.CUSRID = Session["CUSRID"].ToString();
            else tab.CUSRID = "0";
            tab.LMUSRID = 1;
            tab.PRCSDATE = DateTime.Now;
            //tab.CATEDNAME = tab.CATENAME;

            string stypdesc = "";

            var Query = context.Database.SqlQuery<string>("select COMPCODE from COMPANYMASTER where COMPID = 1").ToList();
            if (Query.Count() != 0) { stypdesc = Query[0]; }

            string adesc = (tab.CATENAME.Substring(0, 3)).ToUpper();

            if ((tab.CATEID).ToString() != "0")
            {
                //// Response.Write("update mode");
                //if (tab.CATENO == 0)
                //{
                //    //if (tab.CATESTYPE == 0) { stypdesc = "AS"; } else { stypdesc = "RS"; }

                //    tab.CATENO = Convert.ToInt32(Autonumber.employeeautonum("Employeermaster", "CATENO", tab.STATEID.ToString()).ToString());
                //    int ano = tab.CATENO;
                //    string prfx = stypdesc + string.Format("{0:D3}", ano);
                //    tab.CATECODE = prfx.ToString();
                //}
                //else
                //{
                //    int ano = tab.CATENO;
                //    string prfx = stypdesc + string.Format("{0:D3}", ano);
                //    tab.CATECODE = prfx.ToString();

                //}

                context.Entry(tab).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            else
            {
                //string stypdesc = "";
                //string adesc = (tab.CATENAME.Substring(0, 3)).ToUpper();

                //if (tab.CATESTYPE == 0) { stypdesc = "AS"; } else { stypdesc = "RS"; }

                //tab.CATENO = Convert.ToInt32(Autonumber.employeeautonum("Employeemaster", "CATENO", tab.STATEID.ToString()).ToString());
                //int ano = tab.CATENO;
                //string prfx = stypdesc + string.Format("{0:D3}", ano);
                //tab.CATECODE = prfx.ToString();

                context.employeemasters.Add(tab);
                context.SaveChanges();
            }
            Response.Redirect("Index");
        }//...........end of save function

        //------------------------Delete Record----------//
        [Authorize(Roles = "EmployeeMasterDelete")]
        public void Del()
        {
            String id = Request.Form.Get("id");
            String fld = Request.Form.Get("fld");
            String temp = Delete_fun.delete_check1(fld, id);
            if (temp.Equals("PROCEED"))
            {
                EmployeeMaster employeemasters = context.employeemasters.Find(Convert.ToInt32(id));
                context.employeemasters.Remove(employeemasters);
                context.SaveChanges();
                Response.Write("Deleted Successfully ...");
            }

            else
                Response.Write(temp);
        }//End of Delete

        //public JsonResult GetLocationDetails(int id)
        //{
        //    var result = (from r in context.locationmasters.Where(x => x.STATEID == id)
        //                  select new { r.LOCTID, r.LOCTDESC }).OrderBy(x => x.LOCTDESC);
        //    return Json(result, JsonRequestBehavior.AllowGet);

        //}//..end

    }
}