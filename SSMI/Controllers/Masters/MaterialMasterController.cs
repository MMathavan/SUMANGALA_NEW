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
    public class MaterialMasterController : Controller
    {
        // GET: MaterialMaster
        ApplicationDbContext context = new ApplicationDbContext();

        [Authorize(Roles = "MaterialMasterIndex")]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAjaxData(JQueryDataTableParamModel param)
        {
            using (var e = new NATIMES_ERPEntities())
            {
                var totalRowsCount = new System.Data.Entity.Core.Objects.ObjectParameter("TotalRowsCount", typeof(int));
                var filteredRowsCount = new System.Data.Entity.Core.Objects.ObjectParameter("FilteredRowsCount", typeof(int));

                var data = e.pr_SearchMaterialMaster(param.sSearch,
                                                Convert.ToInt32(Request["iSortCol_0"]),
                                                Request["sSortDir_0"],
                                                param.iDisplayStart,
                                                param.iDisplayStart + param.iDisplayLength,
                                                totalRowsCount,
                                                filteredRowsCount);
                var aaData = data.Select(d => new { MTRLCODE = d.MTRLCODE, MTRLGDESC = d.MTRLGDESC, MTRLDESC = d.MTRLDESC, PACKMDESC = d.PACKMDESC, UNITCODE = d.UNITCODE, HSNCODE = d.HSNCODE,  DISPSTATUS = d.DISPSTATUS.ToString(), MTRLID = d.MTRLID.ToString() }).ToArray();
                return Json(new
                {
                    //sEcho = param.sEcho,
                    data = aaData
                    //iTotalRecords = Convert.ToInt32(totalRowsCount.Value),
                    //iTotalDisplayRecords = Convert.ToInt32(filteredRowsCount.Value)
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "MaterialMasterEdit")]
        public void Edit(int id)
        {
            Response.Redirect(@Url.Action("Form", "MaterialMaster") + "/" + id);
        }


        //----------------Initializing Form-----------------------//
        [Authorize(Roles = "MaterialMasterCreate")]
        public ActionResult Form(int? id = 0)
        {
            MaterialMaster tab = new MaterialMaster();
            List<SelectListItem> selectedDISPSTATUS = new List<SelectListItem>();
            SelectListItem selectedItem = new SelectListItem { Text = "Disabled", Value = "1", Selected = false };
            selectedDISPSTATUS.Add(selectedItem);
            selectedItem = new SelectListItem { Text = "Enabled", Value = "0", Selected = true };
            selectedDISPSTATUS.Add(selectedItem);
            ViewBag.DISPSTATUS = selectedDISPSTATUS;

            ViewBag.MTRLGID = new SelectList(context.materialgroupmasters.Where(x => x.DISPSTATUS == 0).OrderBy(x => x.MTRLGDESC), "MTRLGID ", "MTRLGDESC");
            ViewBag.MTRLTID = new SelectList(context.materialtypemasters.Where(x => x.DISPSTATUS == 0).OrderBy(x => x.MTRLTDESC), "MTRLTID ", "MTRLTDESC");
            ViewBag.PACKMID = new SelectList(context.packingmasters.Where(x => x.DISPSTATUS == 0).OrderBy(x => x.PACKMDESC), "PACKMID ", "PACKMDESC", 1);
            ViewBag.UNITID = new SelectList(context.unitmasters.Where(x => x.DISPSTATUS == 0).OrderBy(x => x.UNITCODE), "UNITID ", "UNITCODE");
            ViewBag.HSNID = new SelectList(context.HSNCodeMaster.Where(x => x.DISPSTATUS == 0).OrderBy(x => x.HSNCODE), "HSNID ", "HSNCODE");

            tab.MTRLID = 0;

            // IMP
            if (id == -1)
                ViewBag.msg = "<div class='msg'>Record Successfully Saved</div>";


            if (id != 0 && id != -1)  // IMP  
            {

                tab = context.materialmasters.Find(id);
                List<SelectListItem> selectedDISPSTATUS1 = new List<SelectListItem>();
                if (Convert.ToInt32(tab.DISPSTATUS) == 1)
                {
                    SelectListItem selectedItem3 = new SelectListItem { Text = "Disabled", Value = "1", Selected = true };
                    selectedDISPSTATUS1.Add(selectedItem3);
                    selectedItem3 = new SelectListItem { Text = "Enabled", Value = "0", Selected = false };
                    selectedDISPSTATUS1.Add(selectedItem3);
                    ViewBag.DISPSTATUS = selectedDISPSTATUS1;
                }

                ViewBag.MTRLGID = new SelectList(context.materialgroupmasters.Where(x => x.DISPSTATUS == 0).OrderBy(x => x.MTRLGDESC), "MTRLGID ", "MTRLGDESC", tab.MTRLGID);
                ViewBag.MTRLTID = new SelectList(context.materialtypemasters.Where(x => x.DISPSTATUS == 0).OrderBy(x => x.MTRLTDESC), "MTRLTID ", "MTRLTDESC", tab.MTRLTID);
                ViewBag.PACKMID = new SelectList(context.packingmasters.Where(x => x.DISPSTATUS == 0).OrderBy(x => x.PACKMDESC), "PACKMID ", "PACKMDESC", tab.PACKMID);
                ViewBag.UNITID = new SelectList(context.unitmasters.Where(x => x.DISPSTATUS == 0).OrderBy(x => x.UNITCODE), "UNITID ", "UNITCODE", tab.UNITID);
                ViewBag.HSNID = new SelectList(context.HSNCodeMaster.Where(x => x.DISPSTATUS == 0).OrderBy(x => x.HSNCODE), "HSNID ", "HSNCODE", tab.HSNID);


            }
            return View(tab);
        }//End of Form
         //-------------------Insert or Modify data-------------//

        public void savedata(MaterialMaster tab)
        {
            int ccount = 0;
            var s = tab.MTRLDESC;//...ProperCase
            s = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());//end
            tab.MTRLDESC = s;
            //tab.MTRLGCODE = s.Substring(0, 3);
            tab.CUSRID = Session["CUSRID"].ToString();
            tab.LMUSRID = 1;
            tab.PRCSDATE = DateTime.Now;
            if ((tab.MTRLID).ToString() != "0")
            {
                context.Entry(tab).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            else
            {
                var Query = context.Database.SqlQuery<int>("select COUNT(*) from MATERIALMASTER where DISPSTATUS =0").ToList();
                if (Query.Count() != 0) { ccount = Query[0] + 1; }

                string prfx =  "M/" + string.Format("{0:D4}", ccount);
                tab.MTRLCODE = prfx.ToString();
                context.materialmasters.Add(tab);
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
        [Authorize(Roles = "MaterialMasterDelete")]
        public void Del()
        {
            String id = Request.Form.Get("id");
            String fld = Request.Form.Get("fld");
            String temp = Delete_fun.delete_check1(fld, id);
            if (temp.Equals("PROCEED"))
            {
                MaterialMaster materialmasters = context.materialmasters.Find(Convert.ToInt32(id));
                context.materialmasters.Remove(materialmasters);
                context.SaveChanges();
                Response.Write("Deleted Successfully ...");
            }
            else
                Response.Write(temp);
        }//..........end of delete function

        //
    }
}