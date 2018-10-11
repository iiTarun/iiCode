using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Web;
using System.Threading.Tasks;
using Nom1Done.Data;
using Nom1Done.Models;
using Nom1Done.Service;
using Nom1Done.Service.Interface;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System;
using System.Linq;
using System.Net.Mail;
using System.Configuration;
using Nom1Done.DTO;
using RestSharp;
using Nom1Done.CustomSerialization;

namespace Nom1Done.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IDashboardService dashboardService;
        private readonly IPipelineService pipelineService;
        private readonly IPasswordHistoryService passwordHistoryService;
        List<Claim> claims = null;
        static string apiBaseUrl = ConfigurationManager.AppSettings.Get("BaseUrlOfUprdApi");
        private readonly RestClient pipelines = new RestClient(apiBaseUrl + "/api/UserPipelineMapping/");

        public AccountController(IDashboardService dashboardService, IPipelineService pipelineService, IPasswordHistoryService passwordHistoryService) : base(pipelineService)
        {
            this.dashboardService = dashboardService;
            this.pipelineService = pipelineService;
            this.passwordHistoryService = passwordHistoryService;
        }


        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
         {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                SignInStatus result = new SignInStatus();
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (model.Email == null || model.Password == null)
                {
                    ModelState.AddModelError("", "Email and Password can't be left blank.");
                }
                else
                {
                    var user = SignInManager.UserManager.FindByEmail(model.Email);                    
                    if (user != null)
                    {
                        var role = UserManager.GetRoles(user.Id);
                        var shipper = dashboardService.GetShipperByUser(user.Id);
                        var ShipperDetails = dashboardService.GetItemByUser(user.Id);
                        var request = new RestRequest(string.Format("HasPipelines?userID=" + user.Id + "&ShipperID=" + shipper.ID), Method.GET) { RequestFormat = DataFormat.Json };
                        request.JsonSerializer = NewtonsoftJsonSerializer.Default;
                        var response = pipelines.Execute<bool>(request);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK && !response.Data)
                        {
                            ModelState.AddModelError("", "You Don't have Access to NatGasHub.");
                            return View(model);
                        }
                        DateTime LastModifiesDate = passwordHistoryService.GetLastModifiedDateByUser(user.Id);
                        if (DateTime.Now > LastModifiesDate.AddDays(90))
                        {
                            ViewBag.HiddenUserId = user.Id;
                            ViewBag.Expired = "Password Expired";
                            return View(model);
                        }
                        claims = new List<Claim>();
                        claims.Add(new Claim("UserName", shipper.FirstName + " " + shipper.LastName));
                        claims.Add(new Claim("ShipperCompanyId", shipper.ID + ""));
                        claims.Add(new Claim("CompanyId", shipper.ID + ""));
                        claims.Add(new Claim("ShipperDetails", ShipperDetails.Name));
                        claims.Add(new Claim("ShipperDuns", ShipperDetails.DUNS + ""));
                        claims.Add(new Claim("UserId", user.Id + ""));
                        claims.Add(new Claim("Roles", role[0].ToString() + ""));
                        // claims.Add(new Claim("FirstSelectedPipeIdByUser", PipeId.ToString()));

                        result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                        switch (result)
                        {
                            case SignInStatus.Success:
                                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                                Session.Abandon();
                                var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                                var authenticationManager = Request.GetOwinContext().Authentication;
                                authenticationManager.SignIn(identity);

                                return RedirectToLocal(returnUrl);
                            case SignInStatus.LockedOut:
                                return View("Lockout");
                            case SignInStatus.RequiresVerification:  
                                
                                return RedirectToAction("VerifyCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe, userId = user.Id });
                            case SignInStatus.Failure:                              
                                ModelState.AddModelError("", "Invalid login attempt.");
                                return View(model);
                            default:
                                ModelState.AddModelError("", "Invalid login attempt.");
                                return View(model);
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "Email not registered with NatGasHub");
                        return View(model);
                    }
                }
                return View(model);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string returnUrl, bool rememberMe,string userId)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            string id = string.Empty;
            var user = await UserManager.FindByIdAsync(userId);
            await SendOTPToEmail(user, UserManager,userId);
            return View(new VerifyCodeViewModel { ReturnUrl = returnUrl, RememberMe = rememberMe });
        }


        #region Verify OTP
        public async Task<JsonResult> VerifyOTP(string code,string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    return Json(new { result = "Empty" });
                }
                var verified = await UserManager.VerifyTwoFactorTokenAsync(userId, "Email Code", code);
                if (!verified)
                    return Json(new { result = "Failure" });
                else
                {
                    var shipper = dashboardService.GetShipperByUser(userId);
                    var ShipperDetails = dashboardService.GetItemByUser(userId);
                    var role = UserManager.GetRoles(userId);
                    claims = new List<Claim>();
                    claims.Add(new Claim("UserName", shipper.FirstName + " " + shipper.LastName));
                    claims.Add(new Claim("ShipperCompanyId", shipper.ID + ""));
                    claims.Add(new Claim("CompanyId", shipper.ID + ""));
                    claims.Add(new Claim("ShipperDetails", ShipperDetails.Name));
                    claims.Add(new Claim("ShipperDuns", ShipperDetails.DUNS + ""));
                    claims.Add(new Claim("UserId", userId + ""));
                    claims.Add(new Claim("Roles", role[0].ToString() + ""));
                    var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                    var authenticationManager = Request.GetOwinContext().Authentication;
                    authenticationManager.SignIn(identity);

                    return Json(new { result = "Success" });
                }
            }
            catch(Exception ex)
            {
                return Json(new { result = "Error" });
            }   
        }
        #endregion


        #region Resend OTP
        public async Task<JsonResult> ResendOtp(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            var IsEmailResend = await SendOTPToEmail(user, UserManager, userId);
            if (IsEmailResend)
                return Json(new { result = "Success" });
            else
                return Json(new { result = "Failure" });
        }
        #endregion

        #region Change Password
        [HttpGet]
        public PartialViewResult ChangePasswordPartial()
        {

            return PartialView("~/Views/Account/_ChangePaswordPartial.cshtml");

        }
        #endregion

        #region Change Password POST
        [HttpPost]
        public JsonResult ChangePassword(ChangePasswordDTO model)
        {
            try
            {
                string UserId = string.Empty;
                if (!string.IsNullOrEmpty(model.UserId))
                {
                    UserId = model.UserId;
                }
                else
                {
                    UserId = GetLoggedInUserId();
                }
                var user = UserManager.FindById(UserId);
                if (UserManager.CheckPassword(user, model.CurrentPassword))
                {
                    var IsPasswordUpdated = UserManager.ChangePassword(UserId, model.CurrentPassword, model.NewPassword);
                    if (IsPasswordUpdated.Succeeded)
                    {
                        var IsTableUpdated = passwordHistoryService.UpdateLastModifiedDateByUser(UserId);
                        if (IsTableUpdated)
                        {
                            Response.Cookies[".AspNet.ApplicationCookie"].Expires = DateTime.Now.AddDays(-1);
                            return Json(new { result = "Success" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                            return Json(new { result = "Unable to send Email" });

                    }
                    return Json(new { result = "Not Updated" });
                }
                else
                {
                    return Json(new { result = "Not Matched" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = "Failure" }, JsonRequestBehavior.AllowGet);
            }


        }
        #endregion

        #region Enabling/Disabling Two Factor Authantication in Identity              
        public JsonResult SetTwoFactorAuthentication(bool EnableTwoFactAuth)
        {
            if(!string.IsNullOrEmpty(GetLoggedInUserId()))
            {
                var identityResult = UserManager.SetTwoFactorEnabled(GetLoggedInUserId(), EnableTwoFactAuth);
                if (identityResult.Succeeded)
                    return Json(new { result = "Success" });
                else
                    return Json(new { result = "Failure" });
            }
            else
            {
                return Json(new { result = "Expired" });
            }
           
        }
        #endregion

        [HttpPost]
        #region Reset Password
        public async Task<ActionResult> ForgetPassword(string email)
        {
            try
            {
                var NewPassword = System.Web.Security.Membership.GeneratePassword(7, 1);
                string userId = null, userEmail;
                var userModel = UserManager.FindByEmail(email);
                if (userModel != null)
                {
                    userId = userModel.Id;
                    userEmail = userModel.Email;
                    string resetToken = UserManager.GeneratePasswordResetToken(userId);
                    var ValidatedPassword = ValidatePassword(NewPassword);
                    IdentityResult result = UserManager.ResetPassword(userId, resetToken, ValidatedPassword);
                    if (result.Succeeded)
                    {
                        var IsTableUpdated = passwordHistoryService.UpdateLastModifiedDateByUser(userId);
                        if (IsTableUpdated)
                        {
                            var IsEmailSend = await SendNewPasswordToEmail(ValidatedPassword, userEmail);
                            if (IsEmailSend)
                                return Json(new { result = "Success" });
                        }
                        else
                        {
                            return Json(new { result = "Table Not Updated", JsonRequestBehavior.AllowGet });
                        }
                    }
                    else
                    {
                        return Json(new { result = "Password Not Valid", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { result = "Not Exist", JsonRequestBehavior.AllowGet });
                }
                return Json(new { result = "Error" });
            }
            catch (Exception ex)
            {
                return Json(new { result = ex.Message });
            }
        }

        private string ValidatePassword(string NewPassword)
        {
            Random rand = new Random();
            List<int> nonSelectedIndexes = new List<int>(Enumerable.Range(0, NewPassword.Length));
            if (!NewPassword.Any(x => char.IsDigit(x)))
            {   //does not contain digit
                char[] pass = NewPassword.ToCharArray();
                int pos = nonSelectedIndexes[rand.Next(nonSelectedIndexes.Count)];
                nonSelectedIndexes.Remove(pos);
                pass[pos] = Convert.ToChar(rand.Next(10) + '0');
                NewPassword = new string(pass);
            }
            if (!NewPassword.Any(x => char.IsLower(x)))
            { //does not contain lower
                char[] pass = NewPassword.ToCharArray();
                int pos = nonSelectedIndexes[rand.Next(nonSelectedIndexes.Count)];
                nonSelectedIndexes.Remove(pos);
                pass[pos] = Convert.ToChar(rand.Next(26) + 'a');
                NewPassword = new string(pass);
            }

            if (!NewPassword.Any(x => char.IsUpper(x)))
            { //does not contain upper
                char[] pass = NewPassword.ToCharArray();
                int pos = nonSelectedIndexes[rand.Next(nonSelectedIndexes.Count)];
                nonSelectedIndexes.Remove(pos);
                pass[pos] = Convert.ToChar(rand.Next(26) + 'A');
                NewPassword = new string(pass);
            }
            return NewPassword;
        }
        #endregion

        #region Email for New Password
        public async Task<bool> SendNewPasswordToEmail(string NewPassword, string Email)
        {
            string Body = BuildResetPasswordEmailBody(NewPassword);
            string subject = "New Password for NatGasHub";
            string[] recipients;
            if (ConfigurationManager.AppSettings.Get("Environment") == "Local")
            {
                recipients = new string[] { "neha.sharma@fifthnote.co", "gagandeep.singh@fifthnote.co" };
            }
            else
            {
                recipients = new string[] { Email };
            }
            string from = ConfigurationManager.AppSettings.Get("EmailIdForAlert");
            #region Send Email
            var isSend = EmailandSMSservice.SendGmail(subject, Body, recipients, from);
            if (isSend)
                return true;
            else
                return false;
            #endregion
        }

        private static string BuildResetPasswordEmailBody(string newPassword)
        {
            string emailbody = "<div style=\" display: inline-block; width:100%;height:100px;\"><div style=\" display: inline-block;width:10%;height:20px;\"></div><div class=\"content\" style=\" display: inline-block;width:75%;height:600px; background:#fff;\"><div style=\"width:100%;height:80px; background:#ffff;\"><img src=\"http://test.natgashub.com/Assets/OrangeNom1DoneLogo.jpg\" alt =\"NatGasHub\" height=\"50px\" width=\"70px\"/><br/><br/>";
            emailbody += string.Format("Your NatGasHub new Password: {0}", newPassword);
            emailbody = emailbody + "</div><div style=\"width:100%;height:50px;<div style=\" display: inline-block;width:10%;height:20px;\"></div></div>";
            return emailbody;
        }
        #endregion


        #region Email for OTP
        private async Task<bool> SendOTPToEmail(ApplicationUser user, ApplicationUserManager userManager,string userId)
        {
            try
            {
                string code = await UserManager.GenerateTwoFactorTokenAsync(userId, "Email Code");
                //string validity = DateTime.Now.AddMinutes(12).ToString("hh:mm tt");
                string Body = BuildOTPEmail(code);
                string subject = "Your NatGasHub 2 Factor verification code";
                string[] recipients;
                if (ConfigurationManager.AppSettings.Get("Environment") == "Local")
                {
                    recipients = new string[] { "neha.sharma@fifthnote.co", "gagandeep.singh@fifthnote.co" };
                }
                else
                {
                    recipients = new string[] { userManager.GetEmail(userId) };
                }
                string from = ConfigurationManager.AppSettings.Get("EmailIdForAlert");
                #region Send Email
                var isSend = EmailandSMSservice.SendGmail(subject, Body, recipients, from);
                if (isSend)
                    return true;
                else
                    return false;
                #endregion
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        private static string BuildOTPEmail(string code)
        {
            string emailbody = "<div style=\" display: inline-block; width:100%;height:100px;\"><div style=\" display: inline-block;width:10%;height:20px;\"></div><div class=\"content\" style=\" display: inline-block;width:75%;height:600px; background:#fff;\"><div style=\"width:100%;height:80px; background:#ffff;\"><img src=\"http://test.natgashub.com/Assets/OrangeNom1DoneLogo.jpg\" alt =\"NatGasHub\" height=\"50px\" width=\"70px\"/><br/><br/>";
            emailbody += string.Format("Your NatGasHub 2 Factor verification code is {0}.  This is valid for the next 15 mins.", code);
            emailbody = emailbody + "</div><div style=\"width:100%;height:50px;<div style=\" display: inline-block;width:10%;height:20px;\"></div></div>";
            return emailbody;
        }
        #endregion






        //
        // POST: /Account/VerifyCode
        //[HttpPost]
        //[AllowAnonymous]
        ////[ValidateAntiForgeryToken]
        //public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // The following code protects for brute force attacks against the two factor codes. 
        //    // If a user enters incorrect codes for a specified amount of time then the user account 
        //    // will be locked out for a specified amount of time. 
        //    // You can configure the account lockout settings in IdentityConfig
        //    //var result = await SignInManager.TwoFactorSignInAsync(User.Identity.GetUserId(), model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
        //    //var result=await UserManager.VerifyTwoFactorTokenAsync(User.Identity.GetUserId(),model.Provider,model.co)
        //    //switch (result)
        //    //{
        //    //    case SignInStatus.Success:
        //    //        return RedirectToLocal(model.ReturnUrl);
        //    //    case SignInStatus.LockedOut:
        //    //        return View("Lockout");
        //    //    case SignInStatus.Failure:
        //    //    default:
        //    //        ModelState.AddModelError("", "Invalid code.");
        //    //        return View(model);
        //    //}

        //    var verified = await UserManager.VerifyTwoFactorTokenAsync(GetLoggedInUserId(), "Email Code", model.Code);
        //    if (!verified)
        //        ViewBag.OTPErrorMsg = "{code} is not a valid OTP, please verify and try again.";
        //    else
        //        return RedirectToAction("Index", "Dashboard");             
        //    return View(model);
        //}

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        //[AllowAnonymous]
        //public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        //{
        //    var userId = await SignInManager.GetVerifiedUserIdAsync();
        //    if (userId == null)
        //    {
        //        return View("Error");
        //    }
        //    var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
        //    var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
        //    return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        //}

        //
        // POST: /Account/SendCode
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> SendCode(SendCodeViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }

        //    // Generate the token and send it
        //    if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
        //    {
        //        return View("Error");
        //    }
        //    return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        //}

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Abandon();
            //Session["ShipperCompanyId"] = null;
            //Session["CompanyId"] = null;
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            //Response.Cache.SetNoStore();
            return RedirectToAction("Login", "Account");
        }


        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        if (_userManager != null)
        //        {
        //            _userManager.Dispose();
        //            _userManager = null;
        //        }

        //        if (_signInManager != null)
        //        {
        //            _signInManager.Dispose();
        //            _signInManager = null;
        //        }
        //    }

        //    base.Dispose(disposing);
        //}

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Dashboard");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}