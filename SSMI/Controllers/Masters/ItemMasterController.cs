using SSMI.Helper;
using SSMI.Models;
using System.Linq;
using System.Web.Mvc;

namespace SSMI.Controllers.Masters
{
    [SessionExpire]
    public class ItemMasterController : Controller
    {
        [Authorize(Roles = "ItemMasterIndex")]
        public ActionResult Index()
        {
            var context = new ApplicationDbContext();
            var data = context.Database.SqlQuery<StockItemMasterNew>("select * from StockItemMaster").ToList();
            return View(data);
        }
    }
}
