using SSMI.Helper;
using SSMI.Models;
using System.Linq;
using System.Web.Mvc;

namespace SSMI.Controllers.Masters
{
    [SessionExpire]
    public class UnitMasterController : Controller
    {
        [Authorize(Roles = "UnitMasterIndex")]
        public ActionResult Index()
        {
            var context = new ApplicationDbContext();
            var data = context.Database.SqlQuery<UnitMasterTally>("select * from UnitMaster").ToList();
            return View(data);
        }
    }
}
