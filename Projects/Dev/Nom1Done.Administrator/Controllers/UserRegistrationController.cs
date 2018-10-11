using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UPRD.DTO;
using UPRD.Services.Interface;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Nom1Done.Admin.App_Start;
using UPRD.Services;

namespace Nom1Done.Admin.Controllers
{
    public class UserRegistrationController : Controller
    {
        public UserRegistrationController()
        {
        }

        private IUprdUserRegistrationService userRegistrationService;
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        public UserRegistrationController(IUprdUserRegistrationService userService)
        {
            this.userRegistrationService = userService;
        }
        // GET: UserRegistration
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetData()
        {
            var detail = userRegistrationService.GetUserList();
            return Json(new { data = detail }, JsonRequestBehavior.AllowGet);
        }

        // Add/Edit User Records
        [HttpGet]
        public ActionResult AddOrEditData(string id)
        {
            var data = userRegistrationService.GetShipperList();
            if (data != null)
                ViewBag.ShipperList = new SelectList(data, "DUNS", "Name");
            else
                ViewBag.ShipperList = new SelectList(Enumerable.Empty<SelectListItem>());

            if (string.IsNullOrEmpty(id))
                return PartialView("_AddOrEditData", new UserRegistrationDTO());
            else
            {
                var userdata = userRegistrationService.GetUsersListById(id);
                if (data != null)
                    ViewBag.ShipperList = new SelectList(data, "DUNS", "Name", userdata.ShipperDuns);
                return PartialView("_AddOrEditData", userdata);
            }
        }
        // Insert new record 
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterUser(UserRegistrationDTO model)
        {
            string msg = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(model.Id))
                    {
                        var user = UserManager.FindById(model.Id);
                        user.UserName = model.Email;
                        user.Email = model.Email;
                        user.ShipperDuns = model.ShipperDuns;

                        var result = await UserManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                            msg = "Updated Data Successfully.";
                        }
                        else
                            msg = "Something went Wrong!!";
                    }
                    else
                    {
                        var user = new UPRD.Data.ApplicationUser { UserName = model.Email, Email = model.Email, ShipperDuns = model.ShipperDuns, UserType = "C", IsEnabled = true };
                        var result = await UserManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                            msg = "Registered User Successfully.";
                        }
                        else
                            msg = "Something went Wrong!!";
                    }
                }
                else
                    msg = "Something went Wrong!!";
            }
            catch (Exception ex)
            {

            }
            //var errors = ModelState.Values.SelectMany(v => v.Errors);
            return Json(new { success = true, message = msg }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> DeleteUser(string userId)
        {
            string msg = null;
            if (string.IsNullOrEmpty(userId))
                msg = "Something went Wrong!!";
            else
            {
                var user = UserManager.FindById(userId);
                if (user.IsEnabled != null)
                {
                    if (user.IsEnabled == true)
                        user.IsEnabled = false;
                    else
                        user.IsEnabled = true;
                }
                else
                    user.IsEnabled = true;
                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    if (user.IsEnabled == true)
                        msg = "User Activated Successfully.";
                    else
                        msg = "User De-Activated Successfully.";
                }
            }
            return Json(new { success = true, message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(string userId)
        {
            string msg = null;
            if (string.IsNullOrEmpty(userId))
                msg = "Something went Wrong!!";
            else
            {
                var user = UserManager.FindById(userId);
                var result = UserManager.DeleteAsync(user);
                //var result = userRegistrationService.DeleteUserById(userId);
                msg = "Deleted Successfully.";
            }
            return Json(new { success = true, message = msg }, JsonRequestBehavior.AllowGet);
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
    }
}