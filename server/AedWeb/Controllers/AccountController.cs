using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Xapp.Models;
using Dextra.Common;
using Dextra.Flowbase;
using Dextra.Database;
using Xapp.Db;
using DextraLib.GeneralDao;
using Dextra.Toolsspace;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using Dextra.Report;
using System.Xml.Serialization;
using System.IO;

namespace Xapp.Controllers
{


    [Serializable]
    public class SimpleUserProp
    {

        public string PasswordHas { get; set; }
        public string Status { get; set; }
        public string Regkey { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }

        public string RegType { get; set; }

        public List<string> Access { get; set; }

        public SimpleUserProp()
        {
            Access = new List<string>();
        }
    }

    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManagerx;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManagerx = userManager;
            SignInManager = signInManager;
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

        public ApplicationUserManager UserManagerx
        {
            get
            {
                return _userManagerx ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManagerx = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        void setUserpref(string Userpreferences)
        {
            if (Userpreferences == null)
            {
                UserPref.Preflang = "hu";
                UserPref.Preflayout = "Default";
                UserPref.Appselector = "Default";
            }
            else
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserPreferenc));
                StringReader stringReader = new StringReader(Userpreferences);
                UserPref = (UserPreferenc)xmlSerializer.Deserialize(stringReader);
            }
            Preflang = UserPref.Preflang;
        }


        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            MvcApplication.AuditlLog.Info("Try " + model.Email + " -> " + model.Password);
            SignInStatus result = new SignInStatus();
            try
            {
                if (model.Email.Contains("@"))
                {
                    Dao<Person> pdao = new Dao<Person>(Sdb);
                    pdao.SqlSelect(@"select * from persons t  where T.USERID=:0 ", new object[] { model.Email });
                    if (pdao.Result.GetSate(DaoResult.ResCountOne))
                    {
                        Person p = new Person();
                        p = pdao.Result.GetFirst<Person>();
                        //string pswhaskey = ConfigurationManager.AppSettings["SimpleUserHasKey"];

                        XmlSerializer serializer1 = new XmlSerializer(typeof(SimpleUserProp));
                        StringReader stringReader = new StringReader(p.Xmldata);
                        SimpleUserProp spu = (SimpleUserProp)serializer1.Deserialize(stringReader);
                        setUserpref(p.Userpreferences);

                        if (SecurePasswordHasherHelper.Verify(model.Password, p.Passwordhas))
                        {
                            //new System.Collections.Generic.Mscorlib_CollectionDebugView<string>(spu.Access).Items[0]
                            //string cApp = spu.Access[0];
                            string pswValue = ConfigurationManager.AppSettings["SimpleUserPsw"];

                            MvcApplication.AuditlLog.Info("Succes " + model.Email + " -> " + model.Password);
                            SimpleUserName = model.Email;
                            model.Email = "SimpleUser" + UserPref.Simpleselector;
                            model.Password = pswValue;

                            //return RedirectToLocal(returnUrl);
                            result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                            if(result== SignInStatus.Success)
                            {
                                
                                MvcApplication.AuditlLog.Info("Succes " + model.Email + " -> " + model.Password);
                                return RedirectToLocal(returnUrl);

                            } else
                            {
                                ModelState.AddModelError("", "Invalid login attempt.");
                                return View(model);

                            }
                        } else
                        {
                            ModelState.AddModelError("", "Invalid login attempt.");
                            return View(model);
                        }
                    }
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);

                }
                else
                {
                    result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                    switch (result)
                    {
                        case SignInStatus.Success:
                            Dao<Vw_Users> uda = new Dao<Vw_Users>(Sdb);
                            uda.SqlSelect("select * from VW_USERS where USERNAME=:0 ", new object[] { model.Email });
                            if (uda.Result.GetSate(DaoResult.ResCountOne))
                            {
                                Vw_Users vu = uda.Result.GetFirst<Vw_Users>();
                                setUserpref(vu.Userpreferences);
                            }
                            else
                            {
                                setUserpref(null);
                            }
                            MvcApplication.AuditlLog.Info("Succes " + model.Email + " -> " + model.Password);
                            return RedirectToLocal(returnUrl);
                        case SignInStatus.LockedOut:
                            return View("Lockout");
                        case SignInStatus.RequiresVerification:
                            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                        case SignInStatus.Failure:
                        default:
                            ModelState.AddModelError("", "Invalid login attempt.");
                            return View(model);
                    }
                }



            }
            catch (Exception e)
            {
                string test = e.Message;
                return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {

            RegisterViewModel model = new RegisterViewModel();


            return View(model);
        }


        // [Obsolete("Do not use this in Production code!!!", true)]
        void NEVER_EAT_POISON_Disable_CertificateValidation()
        {
            // Disabling certificate validation can expose you to a man-in-the-middle attack
            // which may allow your encrypted message to be read by an attacker
            // https://stackoverflow.com/a/14907718/740639
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (
                    object s,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors
                )
                {
                    return true;
                };
        }


        [AllowAnonymous]
        public ActionResult ConfirmRegister()
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
            string regtype = "frmEmploy";
            List<string> serr = new List<string>();
            Dao<Persons> pdao = new Dao<Persons>(Sdb);
            if (model.formselectorg == "frmGuest" && string.IsNullOrEmpty(model.formselectore))
            {
                model.Password = model.Password_g;
                model.ConfirmPassword = model.ConfirmPassword_g;
                model.Email = model.Email_g;
                model.Question = "App1";
                model.FirstName = model.NickName;
                model.LastName = "";
                regtype = "frmGuest";

            }

            if (model.formselectorp == "frmPerson" && string.IsNullOrEmpty(model.formselectore))
            {


                model.Password = model.Password_p;
                model.ConfirmPassword = model.ConfirmPassword_p;
                model.Email = model.Email_p;
                regtype = "frmPerson";

                if (model.Password != model.ConfirmPassword)
                {
                    serr.Add("Password is not identical");
                    ViewBag.RegErrMsg = serr.ToArray<string>();
                    return View(model);

                }
                // ---- spec !!! meg kell irni
                MvcApplication.AuditlLog.Info("Try-reg " + model.Email + " -> " + model.Password);
                pdao.SqlSelect(@"select * from persons t  where T.BID_PERSONS=:0 ", new object[] { model.PersonName });
                if (pdao.Result.GetSate(DaoResult.ResCountOne))  //&& 1==2)
                {

                    Persons p = new Persons();
                    p = pdao.Result.GetFirst<Persons>();
                    if (p.Status == "confirmed")
                    {
                        serr.Add("Allready exist");
                        ViewBag.RegErrMsg = serr.ToArray<string>();
                        return View(model);
                    }

                    string hashed_password = SecurePasswordHasherHelper.Hash(model.Password);
                    p.Userid = model.PersonName;
                    p.Passwordhas = hashed_password;
                    p.Status = "confirmed";
                    pdao.SqlUpdate(p);
                    MvcApplication.AuditlLog.Info("Try-regSucces " + model.Email + " -> " + model.Password);
                    return RedirectToAction("Login", "Account");


                }
                else
                {
                    serr.Add("Not identical");
                    MvcApplication.AuditlLog.Info("Try-regFailed " + model.Email + " -> " + model.Password);

                    ViewBag.RegErrMsg = serr.ToArray<string>();
                    //AddErrors(result);
                    // If we got this far, something failed, redisplay form
                    return View(model);
                }
            }



            MvcApplication.AuditlLog.Info("Try-reg " + model.Email + " -> " + model.Password);
            pdao.SqlSelect(@"select * from persons t  where T.USERID=:0 ", new object[] { model.Email });

            if (pdao.Result.GetSate(DaoResult.ResCountOneOrMore))  //&& 1==2)
            {
                serr.Add("Already exist");
            }
            else
            {
                //var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                //var result = await UserManagerx.CreateAsync(user, model.Password);
                //if (result.Succeeded)
                if (regtype == "frmEmploy")
                {
                    bool validdomain = false;
                    string cApp = model.Question;
                    string cdomain = model.Email.Split('@')[1];
                    try
                    {
                        StreamReader reader = new StreamReader(Tools.Getpath("registeredemaildomain.txt", C.ApdataFilesDir));
                        while (reader.Peek() >= 0)
                        {
                            string line = reader.ReadLine();
                            if (line == cApp + "," + cdomain)
                            {
                                validdomain = true;
                                break;
                            }

                        }
                        reader.Close();

                    }
                    catch { }

                    if (!validdomain)
                    {
                        serr.Add("EmailDomainNotEnabled");
                        ViewBag.RegErrMsg = serr.ToArray<string>();
                        return View(model);
                    }

                }


                if (model.Password != model.ConfirmPassword)
                {
                    serr.Add("Password is not identical");
                    ViewBag.RegErrMsg = serr.ToArray<string>();
                    return View(model);

                }

                try
                {
                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    //string pswhaskey = ConfigurationManager.AppSettings["SimpleUserHasKey"];
                    string hashed_password = SecurePasswordHasherHelper.Hash(model.Password);

                    string pwd = Guid.NewGuid().ToString();


                    SimpleUserProp spu = new SimpleUserProp();
                    spu.PasswordHas = hashed_password;
                    spu.Regkey = pwd;
                    spu.Status = "register";
                    spu.Company = model.Answer;
                    spu.Phone = model.Phonenum;
                    spu.RegType = regtype;


                    switch (model.Question)
                    {
                        case "App1":
                            spu.Access.Add("App1");
                            break;
                        case "App2":
                            spu.Access.Add("App2");
                            break;
                        case "App3":
                            spu.Access.Add("App3");
                            break;
                        default:
                            serr.Add("Invalid Appselect");
                            ViewBag.RegErrMsg = serr.ToArray<string>();
                            return View(model);

                    }

                    Persons p = new Persons();
                    p.Userid = model.Email;
                    p.Passwordhas = hashed_password;
                    p.Status = "register";
                    p.Attributum = pwd;
                    p.Id_Persons = null;
                    p.Bid_Persons = "NEW";
                    p.Usedname = model.FirstName + " " + model.LastName;
                    p.Birthfirstname = model.FirstName;
                    p.Birthlastname = model.LastName;
                    //p.Usedname = model.Email.Split('@')[0];
                    //p.Birthfirstname = p.Usedname;
                    //p.Birthlastname = p.Usedname;
                    p.Birthdate = DateTime.Now;
                    p.InitCommonFieldsForAdd(Sdb);

                    XmlSerializer serializer = new XmlSerializer(typeof(SimpleUserProp));
                    using (StringWriter textWriter = new StringWriter())
                    {
                        serializer.Serialize(textWriter, spu);
                        p.Xmldata = textWriter.ToString();
                    }



                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { v1 = p.Userid, v2 = pwd, v3 = pdao.Result.Lastid }, protocol: Request.Url.Scheme);


                    SimpleDocument doc = new SimpleDocument(Sdb, "confirmemail1.html");
                    doc.Glob.txtIfCantsee = Langue.Translate("txtIfCantsee", "Email", Preflang);
                    doc.Glob.txtViewInbrowser = Langue.Translate("txtViewInbrowser", "Email", Preflang);
                    doc.Glob.IntroHeader = Langue.Translate("IntroHeader", "Email", Preflang);
                    string nametmp = model.NickName;
                    if (string.IsNullOrEmpty(model.NickName)) nametmp = model.FirstName + " " + model.LastName + " ";
                    doc.Glob.IntroSubHeader = string.Format(Langue.Translate("IntroSubHeader", "Email", Preflang), nametmp);
                    doc.Glob.IntroTxt = Langue.Translate("IntroTxt", "Email", Preflang);
                    doc.Glob.btnConfiLink = string.Format(@"<a style=""color:#FFFFFF;text-decoration:none;font-family:Helvetica,Arial,sans-serif;font-size:20px;line-height:135%;"" href=""{0}"" target=""_blank"">{1}</a>", callbackUrl, Langue.Translate("ConfirmLinkTxt", "Email", Preflang));
                    //doc.Glob.btnConfiLink = callbackUrl;
                    doc.Glob.LeftColumn = Langue.Translate("LeftColumn", "Email", Preflang);
                    doc.Glob.LeftColumnTxt = Langue.Translate("LeftColumnTxt", "Email", Preflang);
                    doc.Glob.RightColumn = Langue.Translate("RightColumn", "Email", Preflang);
                    doc.Glob.RightColumnTxt = Langue.Translate("RightColumnTxt", "Email", Preflang);
                    doc.Glob.EmptyRow = string.Format(Langue.Translate("EmptyRow", "Email", Preflang), model.Email);
                    doc.Glob.txtIfdoNotWant = Langue.Translate("txtIfdoNotWant", "Email", Preflang);

                    string curtempl = doc.Render();

                    if (model.Email == "test@mail.hu")
                    {
                        return RedirectToAction("ConfirmRegister", "Account");
                    }



                    decimal lastid = pdao.Result.Lastid;
                    SmtpClient client = new SmtpClient("mail.dextraline.hu");
                    //If you need to authenticate
                    client.Credentials = new NetworkCredential("sfhw@dextraline.hu", "dezoxiribonukleinsav053832da");
                    client.EnableSsl = true;
                    client.Port = 587;
                    //client.UseDefaultCredentials = true;
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.IsBodyHtml = true;
                    mailMessage.From = new MailAddress("info@dextraline.hu");
                    //mailMessage.To.Add("h.winter@svn.dextraline.hu");
                    mailMessage.To.Add(model.Email);
                    mailMessage.Subject = Langue.Translate("Subject", "Email", Preflang);
                    mailMessage.Body = curtempl;  //"Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>";


                    NEVER_EAT_POISON_Disable_CertificateValidation();
                    client.Send(mailMessage);

                    pdao.SqlInsert(p);
                    if (pdao.Result.Error)
                    {
                        serr.Add("DbError [" + pdao.Result.ErrorCode.ToString() + "]");
                        ViewBag.RegErrMsg = serr.ToArray<string>();
                        return View(model);
                    }



                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    //string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    MvcApplication.AuditlLog.Info("Try-regSucces " + model.Email + " -> " + model.Password);
                    return RedirectToAction("ConfirmRegister", "Account");
                }
                catch (Exception e)
                {
                    serr.Add("SmtpError [" + e.Message + "]");
                    ViewBag.RegErrMsg = serr.ToArray<string>();
                    return View(model);

                }
                //AddErrors(result);
            }
            //IdentityResult result=new IdentityResult("Hibacska");
            //ModelState.AddModelError("xFirstName", "asasasasasa");

            MvcApplication.AuditlLog.Info("Try-regFailed " + model.Email + " -> " + model.Password);

            ViewBag.RegErrMsg = serr.ToArray<string>();
            //AddErrors(result);
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string v1, string v2, decimal v3)
        {
            string userId = v1;
            string code = v2;
            ViewBag.Message = "";
            if (userId == null || code == null)
            {
                return View("Error");
            }

            Dao<Persons> pdao = new Dao<Persons>(Sdb);
            //pdao.SqlSelect(@"select * from persons t  where T.USERID=:0  and t.ID_PERSONS=:1 and t.Status=:2 ", new object[] { userId, v3, "register" });
            pdao.SqlSelect(@"select * from persons t  where T.USERID=:0  and  t.Status=:1 ", new object[] { userId, "register" });
            if (pdao.Result.GetSate(DaoResult.ResCountOne))
            {
                Persons p = new Persons();
                p = pdao.Result.GetFirst<Persons>();
                if (p.Attributum == code)
                {

                    XmlSerializer serializer1 = new XmlSerializer(typeof(SimpleUserProp));
                    StringReader stringReader = new StringReader(p.Xmldata);
                    SimpleUserProp spu = (SimpleUserProp)serializer1.Deserialize(stringReader);
                    spu.Status = "confirmed";


                    XmlSerializer serializer = new XmlSerializer(typeof(SimpleUserProp));
                    using (StringWriter textWriter = new StringWriter())
                    {
                        serializer.Serialize(textWriter, spu);
                        p.Xmldata = textWriter.ToString();
                    }


                    p.Status = "confirmed";
                    ViewBag.Message = "ConfirmEmail";
                    pdao.SqlUpdate(p);
                    return View("ConfirmEmail");
                }
            }
            else
            {
                return View("ConfirmEmail");
            }
            //var result = await UserManagerx.ConfirmEmailAsync(userId, code);
            //return View(result.Succeeded ? "ConfirmEmail" : "Error");
            return View("ConfirmEmail");
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
                var user = await UserManagerx.FindByNameAsync(model.Email);
                if (user == null || !(await UserManagerx.IsEmailConfirmedAsync(user.Id)))
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
            var user = await UserManagerx.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManagerx.ResetPasswordAsync(user.Id, model.Code, model.Password);
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
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManagerx.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

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
                var result = await UserManagerx.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManagerx.AddLoginAsync(user.Id, info.Login);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Rolemanager = null; // new DxRoleManager(Sdb, "Guest");
            setUserpref(null);
            MvcApplication.AuditlLog.Info("Logoff " + User.Identity.Name + " ");
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManagerx != null)
                {
                    _userManagerx.Dispose();
                    _userManagerx = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

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
            return RedirectToAction("Index", "Home");
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