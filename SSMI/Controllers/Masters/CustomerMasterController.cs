using SSMI.Helper;
using SSMI.Models;
using System.Linq;
using System.Web.Mvc;

namespace SSMI.Controllers.Masters
{
    [SessionExpire]
    public class CustomerMasterController : Controller
    {
        [Authorize(Roles = "CustomerMasterIndex")]
        public ActionResult Index()
        {
            var context = new ApplicationDbContext();
            var data = context.Database.SqlQuery<LedgerMasterNew>("select * from LedgerMaster").ToList();
            return View(data);
        }

        [Authorize(Roles = "CustomerMasterIndex")]
        public ActionResult Details(string id)
        {
            var context = new ApplicationDbContext();
            var data = context.Database.SqlQuery<LedgerMasterNew>("select * from LedgerMaster where Guid = @p0", id).FirstOrDefault();
            return PartialView("_Details", data);
        }
    }
}
