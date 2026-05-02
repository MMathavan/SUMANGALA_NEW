using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using SSMI.Data;
using SSMI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SSMI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        ApplicationDbContext _db = new ApplicationDbContext();

        private class EmployeeLookupRow
        {
            public int CATEID { get; set; }
            public string CATENAME { get; set; }
        }

        private class BranchLookupRow
        {
            public int BRNCHID { get; set; }
            public string BRNCHNAME { get; set; }
        }

        public AccountController()
                   : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        IAuthenticationManager Authentication
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        // GET: Account
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            SSMIEntities db = new SSMIEntities();
            ViewBag.ReturnUrl = returnUrl;
            Session["DEPTID"] = "";
            Session["DEPTNAME"] = "";
            Session["CUSRID"] = "";
            Session["EMPLNAME"] = "";
            Session["EMPLID"] = "";
            Session["F_BRNCHID"] = "";
            Session["F_BRNCHNAME"] = "";
            Session["F_DBRNCHID"] = "";
            Session["F_DEPTNAME"] = "";
            Session["BRNCHCTYPE"] = "";
            Session["COMPID"] = "";
            Session["S_BRNCHID"] = "";
            Session["Group"] = "";
            Session["STATEID"] = "";
            Session["EMP_STATEID"] = "";
            Session["EMP_LOCTID"] = "";
            Session["grntranrefid"] = "0";
            try
            {
                ViewBag.COMPID = new SelectList(context.companymasters, "COMPID", "COMPNAME");
            }
            catch
            {
                ViewBag.DbError = "Database connection is not available.";
                ViewBag.COMPID = new SelectList(Enumerable.Empty<SelectListItem>(), "Value", "Text");
            }
            Session["LDATE"] = DateTime.Now.ToString("dd-MM-yyyy");
            Session["GYrDesc"] = (DateTime.Now.Year - 1) + " - " + (DateTime.Now.Year);
            try
            {
                ViewBag.COMPYID = new SelectList(context.VW_ACCOUNTING_YEAR_DETAIL_ASSGN.OrderByDescending(m => m.YRDESC), "COMPYID", "YRDESC");
            }
            catch
            {
                ViewBag.DbError = "Database connection is not available.";
                ViewBag.COMPYID = new SelectList(Enumerable.Empty<SelectListItem>(), "Value", "Text");
            }
           
            //return View(new LoginViewModel());
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            SSMIEntities db = new SSMIEntities();

            try
            {
                ViewBag.COMPID = new SelectList(context.companymasters, "COMPID", "COMPNAME");
            }
            catch
            {
                ViewBag.DbError = "Database connection is not available.";
                ViewBag.COMPID = new SelectList(Enumerable.Empty<SelectListItem>(), "Value", "Text");
            }

            try
            {
                ViewBag.COMPYID = new SelectList(context.VW_ACCOUNTING_YEAR_DETAIL_ASSGN.OrderByDescending(m => m.YRDESC), "COMPYID", "YRDESC");
            }
            catch
            {
                ViewBag.DbError = "Database connection is not available.";
                ViewBag.COMPYID = new SelectList(Enumerable.Empty<SelectListItem>(), "Value", "Text");
            }

            var brnchctype = 0;// context.Database.SqlQuery<Int16>("Select BRNCHCTYPE From BranchMaster Where BRNCHID = '" + user.BrnchId + "'").ToList();
            var stateid = 1;// context.Database.SqlQuery<Int32>("Select STATEID From BranchMaster Where BRNCHID = '" + user.BrnchId + "'").ToList();
                            //var deptid = "2";// context.Database.SqlQuery<Int32>("Select DEPTID From BRANCHDEPARTMENTMASTER Where DBRNCHID = '" + user.DBrnchId + "'").ToList();
                            //var deptdesc = "ADMIN";// context.Database.SqlQuery<string>("Select DDEPTDESC From BRANCHDEPARTMENTMASTER Where DBRNCHID = '" + user.DBrnchId + "'").ToList();
                            //var uid = user.Id;

            var userchk = context.Database.SqlQuery<int>("Select CateId From View_User_Diable_Chk_For_Login Where UserName = '" + model.UserName.Trim() + "' And DispStatus = 0").ToList();
            if (userchk.Count > 0)
            {
                if (ModelState.IsValid)
                {
                    var user = await UserManager.FindAsync(model.UserName, model.Password);
                    if (user != null)
                    {

                        context.Database.ExecuteSqlCommand("Update AspNetUsers Set NPassword = '" + model.Password + "' where id ='" + user.Id + "'");

                        Session["compyid"] = model.COMPYID;
                        Session["CUSRID"] = model.UserName;
                        Session["COMPID"] = model.COMPID;
                        Session["BRNCHNAME"] = user.UBrnchName;
                        Session["BRNCHID"] = user.BrnchId;
                        Session["F_BRNCHID"] = user.BrnchId;
                        Session["F_BRNCHNAME"] = user.UBrnchName;
                        Session["F_DBRNCHID"] = user.DBrnchId;
                        Session["F_DEPTNAME"] = "ADMIN";// user.DeptName;
                        Session["S_BRNCHID"] = user.BrnchId;
                        Session["BRNCHCTYPE"] = 0;// brnchctype[0];// model.BRNCHCTYPE;
                        Session["DEPTID"] = "2";// deptid[0];
                        Session["DEPTNAME"] = "ADMIN";// deptdesc[0];
                        Session["grntranrefid"] = "0";
                        Session["STATEID"] = 1;// stateid[0];
                        Session["EMPLID"] = user.DBrnchId;

                        //

                        Session["LDATE"] = Request.Form.Get("LDATE"); var COMPID = Request.Form.Get("COMPID");
                        DateTime TmpDate = Convert.ToDateTime(Request.Form.Get("LDATE")).Date;
                        var LMNTH = TmpDate.Month; var LYR = TmpDate.Year; var PFYear = 0; var PTYear = 0; var PFDATE = ""; var PTDATE = ""; var GYrDesc = "";

                        if (LMNTH >= 4)
                        {// Response.Write(LMNTH + ".." + LYR + "..." + Session["LDATE"]); Response.End(); 
                            PFYear = LYR;
                            PTYear = LYR + 1;
                            PFDATE = "01/04/" + PFYear; PTDATE = "31/03/" + PTYear;
                            GYrDesc = PFYear + " - " + PTYear;


                        }
                        else
                        { //Response.Write("ELSE" + LMNTH + ".." + LYR + "..." + Session["LDATE"]); Response.End(); 
                            PFYear = LYR - 1;
                            PTYear = LYR;
                            PFDATE = "01/04/" + PFYear; PTDATE = "31/03/" + PTYear;
                            GYrDesc = PFYear + " - " + PTYear;
                        }

                        List<int> Max_YrId = new List<int>();
                        var ACCYR_QRY = context.Database.SqlQuery<PR_ACCOUNTINGYEAR_ID_CHK_Result>("PR_ACCOUNTINGYEAR_ID_CHK @PFYear=" + Convert.ToInt32(PFYear) + ",@PTYear=" + Convert.ToInt32(PTYear) + "").ToList();
                        if (ACCYR_QRY.Count == 0)
                        {
                            context.Database.ExecuteSqlCommand("INSERT INTO AccountingYear (  YrDesc, FDate, TDate, CUSRID, PRCSDATE ) VALUES  ( '" + GYrDesc + "', '" + Convert.ToDateTime(PFDATE).ToString("MM/dd/yyyy") + "', '" + Convert.ToDateTime(PTDATE).ToString("MM/dd/yyyy") + "', '" + Session["CUSRID"] + "', '" + DateTime.Now.ToString("MM-dd-yyyy") + "')");
                            Max_YrId = context.Database.SqlQuery<Int32>("SELECT MAX(YRID) FROM AccountingYear").ToList();
                        }
                        else
                        {
                            var ROW = ACCYR_QRY.Count - 1;
                            Max_YrId.Add(ACCYR_QRY[ROW].YRID);
                        }

                        var GCID = context.Database.SqlQuery<int>("select MAX(COMPYID) from CompanyAccountingDetail").ToList();

                        var COMPDTL_QRY = context.Database.SqlQuery<PR_COMPANYACCOUNTINGDETAIL_ID_CHK_Result>("PR_COMPANYACCOUNTINGDETAIL_ID_CHK @PCompId=" + COMPID + ",@PYrId=" + Convert.ToInt32(Max_YrId[0]) + "").ToList();
                        if (COMPDTL_QRY.Count == 0)
                        {
                            context.Database.ExecuteSqlCommand("INSERT INTO CompanyAccountingDetail ( CompId, YrId,  CUSRID, PRCSDATE ) VALUES  ( " + COMPID + ", " + Convert.ToInt32(Max_YrId[0]) + ",  '" + Session["CUSRID"] + "', '" + DateTime.Now.ToString("MM-dd-yyyy") + "')");
                            // GCID = context.Database.SqlQuery<int>("select MAX(COMPYID) from CompanyAccountingDetail").ToList();
                            System.Web.HttpContext.Current.Session["compyid"] = Convert.ToInt32(GCID[0] + 1);
                        }
                        else
                        {
                            System.Web.HttpContext.Current.Session["compyid"] = Convert.ToInt32(COMPDTL_QRY[0].COMPYID);
                        }

                        Session["GYrDesc"] = GYrDesc;


                        //var sql = context.Database.SqlQuery<int>("select GroupId from ApplicationUserGroups inner join AspNetUsers on AspNetUsers.Id=ApplicationUserGroups.UserId where AspNetUsers.UserName='" + model.UserName + "'").ToList();

                        //if (sql[0].Equals(1)) { Session["Group"] = "Admin"; }
                        //if (sql[0].Equals(2)) { Session["Group"] = "SuperAdmin"; }
                        //if (sql[0].Equals(4)) { Session["Group"] = "Users"; }
                        //if (sql[0].Equals(3)) { Session["Group"] = "Manager"; }

                        var sql = context.Database.SqlQuery<VW_USER_DETAILS>("select * from VW_USER_DETAILS Where UserName='" + model.UserName + "'").ToList();
                        if (sql.Count == 0)
                        {
                            Session["Group"] = "";
                        }
                        else
                        {
                            if (sql.Count > 1)
                            { Session["Group"] = sql[1].GroupName; }
                            else
                            { Session["Group"] = sql[0].GroupName; }

                        }

                        //var aa = Session["EMPLID"].ToString();
                        //var emplid = 0;
                        //if (aa != "") { emplid = Convert.ToInt32(Session["EMPLID"]); }
                        //var rsql = context.Database.SqlQuery<EmployeeMaster>("select * from EmployeeMaster Where CATEID = '" + emplid + "'").ToList();
                        //if (rsql.Count > 0)
                        //{
                        //    Session["EMP_STATEID"] = rsql[0].STATEID;
                        //    Session["EMP_LOCTID"] = rsql[0].LOCTID;
                        //}
                        //else
                        //{
                        //    Session["EMP_STATEID"] = "0";
                        //    Session["EMP_LOCTID"] = "0";
                        //}

                        Session["EXCLPATH"] = "D:\\SACT_EXCEL\\" + Session["CUSRID"];



                    }

                    if (user != null)
                    {
                        await SignInAsync(user, model.RememberMe);
                        Session["MyMenu"] = "";
                        context.Database.ExecuteSqlCommand("delete from menurolemaster where Roles='" + model.UserName + "'");
                        context.Database.ExecuteSqlCommand("EXEC pr_USER_MENU_DETAIL_ASSGN @PKUSRID='" + model.UserName + "'");
                        return RedirectToLocal(returnUrl);
                        //return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError("", "Invalid username or password.");

                    return View(model);

                }
            }
            else
            {
                ModelState.AddModelError("", "User Name Not Exists.");
            }


            return View(model);
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}
            //var data = new Data();
            //var users = data.users();

            //if (users.Any(p => p.user == model.UserName && p.password == model.Password))
            //{
            //    var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, model.UserName),}, DefaultAuthenticationTypes.ApplicationCookie);

            //    Authentication.SignIn(new AuthenticationProperties
            //    {
            //        IsPersistent = model.RememberMe
            //    }, identity);

            //    return RedirectToAction("Index", "Home");
            //}
            //else
            //{
            //    ModelState.AddModelError("", "Invalid login attempt.");
            //    return View(model);
            //}
        }


        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }


        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Authentication.SignOut();
            return RedirectToAction("Login", "Account");
        }




        //   [Authorize(Roles = "Admin, CanEditUser")]
        public ActionResult Register()
        {
            var emplList = _db.Database.SqlQuery<EmployeeLookupRow>(
                "select CATEID, CATENAME from EmployeeMaster where isnull(DISPSTATUS,0) = 0 order by CATENAME").ToList();
            ViewBag.EMPLID = new SelectList(emplList, "CATEID", "CATENAME");
           // ViewBag.DBRNCHID = new SelectList("");
            return View();
        }

        [HttpPost]
        // [Authorize(Roles = "Admin, CanEditUser")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(AccountViewModels.RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = model.GetUser();
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Account");
                }

            }

            var emplList = _db.Database.SqlQuery<EmployeeLookupRow>(
                "select CATEID, CATENAME from EmployeeMaster where isnull(DISPSTATUS,0) = 0 order by CATENAME").ToList();
            ViewBag.EMPLID = new SelectList(emplList, "CATEID", "CATENAME");

            // If we got this far, something failed, redisplay form
            return View(model);
        }



        //    [Authorize(Roles = "Admin, CanEditUser")]
        public ActionResult UserGroups(string id)
        {
            var user = _db.Users.First(u => u.UserName == id);
            var model = new AccountViewModels.SelectUserGroupsViewModel(user);
            return View(model);
        }


        [HttpPost]
        // [Authorize(Roles = "Admin, CanEditUser")]
        [ValidateAntiForgeryToken]
        public ActionResult UserGroups(AccountViewModels.SelectUserGroupsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var idManager = new IdentityManager();
                var user = _db.Users.First(u => u.UserName == model.UserName);
                idManager.ClearUserGroups(user.Id);
                foreach (var group in model.Groups)
                {
                    if (group.Selected)
                    {
                        idManager.AddUserToGroup(user.Id, group.GroupId);
                    }
                }
                return RedirectToAction("index");
            }
            return View();
        }


        // [Authorize(Roles = "Admin, CanEditRole, CanEditGroup, User")]
        public ActionResult UserPermissions(string id)
        {
            var user = _db.Users.First(u => u.UserName == id);
            var model = new AccountViewModels.UserPermissionsViewModel(user);
            return View(model);
        }


        // [Authorize(Roles = "Admin, CanEditUser")]
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        //  [Authorize(Roles = "Admin, CanEditUser")]
        public async Task<ActionResult> Manage(AccountViewModels.ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //  [Authorize(Roles = "Admin, CanEditUser")]
        public ActionResult Index()
        {
            var users = _db.Users.OrderBy(X => X.UserName);
            var model = new List<AccountViewModels.EditUserViewModel>();
            foreach (var user in users)
            {
                var u = new AccountViewModels.EditUserViewModel(user);
                // if(u.UserName==Session["CUSRID"].ToString())
                model.Add(u);
            }
            return View(model);
        }

       // [Authorize(Roles = "UserPasswordChange")]
        public ActionResult CIndex()
        {
            var uname = Session["CUSRID"];
            var users = _db.Users.Where(X => X.UserName == uname).OrderBy(X => X.UserName);
            var model = new List<AccountViewModels.EditUserViewModel>();
            foreach (var user in users)
            {
                var u = new AccountViewModels.EditUserViewModel(user);
                // if(u.UserName==Session["CUSRID"].ToString())
                model.Add(u);
            }
            return View(model);
        }


        //   [Authorize(Roles = "Admin, CanEditUser")]
        public ActionResult Edit(string id, ManageMessageId? Message = null)
        {
            var user = _db.Users.First(u => u.UserName == id);
            int brnchid = user.BrnchId;
            int dbrnchid = user.DBrnchId;
            var model = new AccountViewModels.EditUserViewModel(user);

            var branchList = _db.Database.SqlQuery<BranchLookupRow>(
                "select distinct BrnchId as BRNCHID, UBrnchName as BRNCHNAME from AspNetUsers where UBrnchName is not null and ltrim(rtrim(UBrnchName)) <> '' order by UBrnchName").ToList();
            ViewBag.BRNCHID = new SelectList(branchList, "BRNCHID", "BRNCHNAME", brnchid);

            var deptList = _db.Database.SqlQuery<EmployeeLookupRow>(
                "select CATEID, CATENAME from EmployeeMaster where isnull(DISPSTATUS,0) = 0 order by CATENAME").ToList();
            ViewBag.DBRNCHID = new SelectList(deptList, "CATEID", "CATENAME", dbrnchid);
            //ViewBag.DBRNCHID = new SelectList(_db.branchdepartmentmasters, "DBRNCHID", "DDEPTDESC", dbrnchid);
            // ViewBag.DBRNCHID = new SelectList(_db.branchdepartmentmasters.Where(x => x.BRNCHID == model.BrnchId), "DBRNCHID", "DDEPTDESC", model.DBrnchId);
            ViewBag.MessageId = Message;
            return View(model);
        }


        [HttpPost]
        //   [Authorize(Roles = "Admin, CanEditUser")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AccountViewModels.EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var branchList = _db.Database.SqlQuery<BranchLookupRow>(
                    "select distinct BrnchId as BRNCHID, UBrnchName as BRNCHNAME from AspNetUsers where UBrnchName is not null and ltrim(rtrim(UBrnchName)) <> '' order by UBrnchName").ToList();
                ViewBag.BRNCHID = new SelectList(branchList, "BRNCHID", "BRNCHNAME", model.BrnchId);

                var deptList = _db.Database.SqlQuery<EmployeeLookupRow>(
                    "select CATEID, CATENAME from EmployeeMaster where isnull(DISPSTATUS,0) = 0 order by CATENAME").ToList();
                ViewBag.DBRNCHID = new SelectList(deptList, "CATEID", "CATENAME", model.DBrnchId);
                //ViewBag.DBRNCHID = new SelectList(_db.branchdepartmentmasters.Where(x => x.BRNCHID == model.BrnchId), "DBRNCHID", "DDEPTDESC", model.DBrnchId);

                //    ViewBag.Subjects = new SelectList(_odb.SUBJ_MSTR.Where(o => o.TYPE == "4" && o.TYPE == "5").OrderBy(o => o.SUBJ_NAME), "SUBJ_ID", "SUBJ_VAL");

                var user = _db.Users.First(u => u.UserName == model.UserName);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.BrnchId = model.BrnchId;
                user.UBrnchName = model.UBrnchName;
                user.DBrnchId = model.DBrnchId;
                //user.DeptName = model.DeptName;

                //      user
                _db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var branchList2 = _db.Database.SqlQuery<BranchLookupRow>(
                "select distinct BrnchId as BRNCHID, UBrnchName as BRNCHNAME from AspNetUsers where UBrnchName is not null and ltrim(rtrim(UBrnchName)) <> '' order by UBrnchName").ToList();
            ViewBag.BRNCHID = new SelectList(branchList2, "BRNCHID", "BRNCHNAME", model.BrnchId);

            var deptList2 = _db.Database.SqlQuery<EmployeeLookupRow>(
                "select CATEID, CATENAME from EmployeeMaster where isnull(DISPSTATUS,0) = 0 order by CATENAME").ToList();
            ViewBag.DBRNCHID = new SelectList(deptList2, "CATEID", "CATENAME", model.DBrnchId);

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [Authorize(Roles = "Admin, CanEditUser")]
        public ActionResult Delete(string id = null)
        {
            var user = _db.Users.First(u => u.UserName == id);
            var model = new AccountViewModels.EditUserViewModel(user);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditUser")]
        public ActionResult DeleteConfirmed(string id)
        {
            var user = _db.Users.First(u => u.UserName == id);
            _db.Users.Remove(user);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        //public JsonResult BranchDepartment(int id)
        //{
        //    var result = _db.Database.SqlQuery<BranchDepartmentMaster>("select * FROM BranchDepartmentMaster where BRNCHID =" + id + " ORDER BY DDEPTDESC").ToList();
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}



    }
}