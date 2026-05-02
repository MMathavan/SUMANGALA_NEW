using SSMI.Helper;
using SSMI.Models;
using System.Linq;
using System.Web.Mvc;

namespace SSMI.Controllers.Masters
{
    [SessionExpire]
    public class ItemGroupMasterController : Controller
    {
        [Authorize(Roles = "ItemGroupMasterIndex")]
        public ActionResult Index()
        {
            var context = new ApplicationDbContext();
            var data = context.Database.SqlQuery<StockGroupMasterNew>("select * from StockGroupMaster").ToList();
            return View(data);
        }
    }
}
