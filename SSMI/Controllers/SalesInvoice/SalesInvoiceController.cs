using SSMI.Helper;
using SSMI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SSMI.Controllers.SalesInvoice
{
    [SessionExpire]
    [Authorize]
    public class SalesInvoiceController : Controller
    {
        private readonly ApplicationDbContext context = new ApplicationDbContext();

        [Authorize(Roles = "SalesInvoiceIndex")]
        public ActionResult Index()
        {
            if (Convert.ToInt32(System.Web.HttpContext.Current.Session["compyid"]) == 0 || Session["CUSRID"].ToString() == "")
            {
                return RedirectToAction("Login", "Account");
            }

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

            return View();
        }

        public JsonResult GetAjaxData(JQueryDataTableParamModel param)
        {
            var sd = Convert.ToDateTime(Session["SDATE"]).Date;
            var ed = Convert.ToDateTime(Session["EDATE"]).Date;

            var q = context.tally_voucher.AsQueryable();
            q = q.Where(v => v.RegstrId == 2);
            q = q.Where(v => v.VoucherDate.HasValue && v.VoucherDate.Value >= sd && v.VoucherDate.Value <= ed);

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                var s = param.sSearch;
                q = q.Where(v => (v.VoucherNumber ?? "").Contains(s) || (v.PartyName ?? "").Contains(s));
            }

            var sortCol = Convert.ToInt32(Request["iSortCol_0"] ?? "0");
            var sortDir = (Request["sSortDir_0"] ?? "desc").ToLower();

            if (sortCol == 0)
                q = sortDir == "asc" ? q.OrderBy(x => x.VoucherDate) : q.OrderByDescending(x => x.VoucherDate);
            else if (sortCol == 1)
                q = sortDir == "asc" ? q.OrderBy(x => x.VoucherNumber) : q.OrderByDescending(x => x.VoucherNumber);
            else if (sortCol == 2)
                q = sortDir == "asc" ? q.OrderBy(x => x.PartyName) : q.OrderByDescending(x => x.PartyName);
            else if (sortCol == 3)
                q = sortDir == "asc" ? q.OrderBy(x => x.TotalAmount) : q.OrderByDescending(x => x.TotalAmount);
            else
                q = q.OrderByDescending(x => x.VoucherId);

            if (param.iDisplayLength > 0)
                q = q.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            var data = q.ToList();
            var aaData = data.Select(d => new
            {
                VoucherDate = d.VoucherDate.HasValue ? d.VoucherDate.Value.ToString("dd/MM/yyyy") : "",
                VoucherNumber = d.VoucherNumber,
                PartyName = d.PartyName,
                TotalAmount = d.TotalAmount.HasValue ? d.TotalAmount.Value.ToString() : "",
                DispStatus = d.SyncStatus == 1 ? "Synced" : "Not Synced",
                SyncStatus = d.SyncStatus,
                VoucherId = d.VoucherId.ToString()
            }).ToArray();

            return new JsonResult
            {
                Data = new { data = aaData },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        [Authorize(Roles = "SalesInvoiceIndex")]
        public ActionResult Details(long id)
        {
            var voucher = context.tally_voucher.FirstOrDefault(x => x.VoucherId == id);
            if (voucher == null)
            {
                return PartialView("_Details", new SalesInvoiceDetailsViewModel
                {
                    Voucher = new Tally_Voucher(),
                    InventoryLines = new List<Tally_Voucher_Inventory>(),
                    LedgerLines = new List<Tally_Voucher_Ledger>()
                });
            }

            var inv = context.tally_voucher_inventory.Where(x => x.VoucherId == id).ToList();
            var led = context.tally_voucher_ledger.Where(x => x.VoucherId == id).ToList();

            var vm = new SalesInvoiceDetailsViewModel
            {
                Voucher = voucher,
                InventoryLines = inv,
                LedgerLines = led
            };

            return PartialView("_Details", vm);
        }
    }
}
