using Dextra.Flowbase;
using DextraLib.GeneralDao;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Dextra.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using Xapp.Db;
using Xapp;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Xapp.Models;
using Dextra.Toolsspace;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text;

namespace Dextra.Common
{
    public class AjaxResultCode
    {
        public int Errorcode { get; set; }
        public string Errormessage { get; set; }
        public AjaxResultCode()
        {
            Errorcode = 0;
            Errormessage = "Complett";
        }
    }

    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class dxAuthorize : AuthorizeAttribute
    {

        public dxAuthorize(params string[] roles)
        {
            ReqSubroles = roles.ToList<string>();
        }

        //Custom named parameters for annotation
        public string ResourceKey { get; set; }
        public string OperationKey { get; set; }
        List<string> ReqSubroles = new List<string>();
        List<string> Subroles = new List<string>();

        List<Vw_User> AvailableRoles = new List<Vw_User>();

        bool isRole(string rolename)
        {
            List<Vw_User> t = AvailableRoles.Where(r => r.Rolename == rolename).ToList<Vw_User>();
            if (t.Count > 0) return true;
            return false;
        }
        bool isSubRole()
        {
            int chk = ReqSubroles.Count();
            foreach (string sr in ReqSubroles)
            {
                if (Subroles.Where(r => r == sr).Count() > 0)
                {
                    chk--;
                }
            }
            if (chk == 0) return true;
            return false;
        }

        bool isAcces(string conttroller, string action)
        {
            List<XrefRole> res = new List<XrefRole>();
            string s1 = "";
            try
            {
                s1 = MvcApplication.Functionroles.Functions.Where(f => f.Action == action && f.Controller == conttroller).FirstOrDefault<Functions>().Functionname;
                res = MvcApplication.Functionroles.XrefFunc.Where(c => c.Function == s1).ToList<XrefRole>();
            }
            catch
            {
                try
                {
                    s1 = MvcApplication.Flowroles.Flowsteps.Where(f => f.Action == action && f.Controller == conttroller).FirstOrDefault<FlowStep>().Flowname;
                    res = MvcApplication.Flowroles.XrefFlows.Where(c => c.Function == s1).ToList<XrefRole>();
                }
                catch
                {

                }
            }

            foreach (XrefRole i in res)
            {
                if (isRole(i.Role)) return true;
            }
            return false;
        }



        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                var urlHelper = new UrlHelper(context.RequestContext);
                context.HttpContext.Response.StatusCode = 403;
                context.Result = new JsonResult
                {
                    Data = new
                    {
                        Error = "NotAuthorized",
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                base.HandleUnauthorizedRequest(context);
            }
        }

        //Core authentication, called before each action
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            var t = httpContext.GetOwinContext().Authentication.User;
            var uname = ((System.Security.Claims.ClaimsIdentity)t.Identity).Name;
            //var manager = httpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //if (!string.IsNullOrEmpty(uname))
            //{
            //    ApplicationUser user = manager.FindByName(uname);
            //}

            bool test = base.AuthorizeCore(httpContext);
            string saction = httpContext.Request.RequestContext.RouteData.Values["action"].ToString();
            string scontroller = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
            DxRoleManager Rolemanager = new DxRoleManager(MvcApplication.Adb, uname, true);
            AvailableRoles = Rolemanager.AvailableRoles;
            Subroles = Rolemanager.SubRoles;

            if (!httpContext.Request.IsAjaxRequest())
            {
                return isAcces(scontroller, saction);
            }
            else
            {
                return isSubRole();
            }
        }
    }

    public class UserPreferenc
    {
        public string Preflang { get; set; }
        public string Preflayout { get; set; }
        public string Appselector { get; set; }
        public string Theme { get; set; }
        public string Simpleselector { get; set; }
        public UserPreferenc()
        {

        }
        public UserPreferenc(string lang, string layout, string appselector)
        {
            Preflang = lang;
            Preflayout = layout;
            Appselector = appselector;
            Theme = "None";
        }

        public string Ser()
        {

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserPreferenc));

            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, new UTF8Encoding(false));
            xmlTextWriter.Formatting = System.Xml.Formatting.Indented;

            xmlSerializer.Serialize(xmlTextWriter, this);
            String output = Encoding.UTF8.GetString(memoryStream.ToArray());
            //String output = Encoding.ASCII.GetString(memoryStream.ToArray());
            //output.Trim(new char[] { '\uFEFF' });
            //output.Trim(new char[] { '\uFEFF', '\u200B' });
            return output;
        }
    }



    public class BaseController : Controller
    {

        [PersistField]
        public string BackUrl = null;

        [PersistField]
        public string SignalRId = null;

        [PersistField]
        public DxRoleManager Rolemanager = null;

        [PersistField]
        public DbContext Sdb = null;

        public DbSysDate Dbsysdate { get; set; }

        public string DbSource { get; set; }
        public string DbSourceLocal { get; set; }


        [PersistField]
        public byte[] TempImg = null;
        [PersistField]
        public string TempImgMime = null;

        const BindingFlags FieldBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        [PersistField]
        public string Preflang = null;


        [PersistField]
        public UserPreferenc UserPref = new UserPreferenc();

        [PersistField]
        public string SimpleUserName = null;


        private ApplicationUserManager _userManager;
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



        public BaseController():base()
        {
         
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

        }


        [PersistField]
        public LimitedSizeStack<string> UrlHistory = new LimitedSizeStack<string>(20);

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            // MvcApplication.LoadGlobalSchema();
            //MvcApplication.LoadLangDictionary();


            ViewBag.Message = "";

            // Get persisted Controllervariables 
            foreach (System.Reflection.FieldInfo fi in GetType().GetFields(FieldBindingFlags))
            {
                PersistFieldAttribute attr = PersistFieldAttribute.GetAttribute(fi);
                if (attr != null)
                {
                    switch (attr.Location)
                    {
                        case Location.Session:
                            if (Session != null)
                            {
                                try
                                {
                                    var rest = Session[attr.GetKeyFor(fi)];
                                    if (rest != null) fi.SetValue(this, Session[attr.GetKeyFor(fi)]);

                                }
                                catch (Exception e)
                                {
                                    var ss = e;
                                }
                            }
                            break;
                    }
                }
            }
            string ip = this.Request.ServerVariables["REMOTE_ADDR"];
            MvcApplication.GeneralLog.Info("Init:" + this.ToString() + "-" + User.Identity.Name + " from:" + ip);

            DbSourceLocal = WebConfigurationManager.ConnectionStrings["LocalConnection"].ConnectionString;
            DbSource = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            if (Sdb == null)
            {
                Sdb = new DbContext(DbSource);
                Sdb.errh = ControllerErrHandler;

            }

            string username = string.IsNullOrEmpty(User.Identity.Name) ? "Guest" : User.Identity.Name;


            if (Rolemanager == null || Rolemanager.CurrentUser != username)
            {
                if (!string.IsNullOrEmpty(username))
                {
                    Rolemanager = new DxRoleManager(Sdb, username);
                    Dbsysdate = new DbSysDate(Sdb);
                    Sdb.SetDefaultUser(username);
                    Sdb.SetDefaultOrggroup(Rolemanager.DefaultOrgroup.Number);
                    Sdb.SetDate("dtfrom", Dbsysdate.Sysdbdate);
                    Sdb.CurrentLang = Preflang;
                }

                //ApplicationUser user = UserManager.FindByName(username);
                //if (user != null)
                //{
                //    Rolemanager = new DxRoleManager(Sdb, username);
                //    user.SRoles = new List<string>();
                //    user.SRoles.AddRange( Rolemanager.SubRoles);
                //    Dbsysdate = new DbSysDate(Sdb);
                //    Sdb.SetDefaultUser(username);
                //    Sdb.SetDefaultOrggroup(Rolemanager.DefaultOrgroup.Number);
                //    Sdb.SetDate("dtfrom", Dbsysdate.Sysdbdate);
                //}
            }

        }

        public virtual void ControllerErrHandler(DaoErr e)
        {
            MvcApplication.GeneralLog.Error("Error :" + e.Sqlerrm);
        }
        public virtual void ControllerErrHandler(Exception e)
        {
            MvcApplication.GeneralLog.Error("CtrlError :" + e.Message);
        }


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {


            try
            {
                if (filterContext.IsChildAction || filterContext.HttpContext.Request.IsAjaxRequest())
                {

                }
                else
                {
                    ViewBag.CurentUserId = Rolemanager.CurrentUser;
                    if (Rolemanager.CurrentUser == "Guest")
                    {

                        if (UserPref.Preflang == null) UserPref.Preflang = "hu";
                        UserPref.Preflayout = "Default";
                        UserPref.Appselector = "Default";
                        Preflang = "hu";
                        //if (Preflang == null) Preflang = "hu"; // "hu.Special.Special";
                        //string[] temp = Preflang.Split('.');
                        //Preflang = temp[0] + ".Special.Special";
                    }
                    Preflang = Preflang == null ? P.PreferdLangDefault : Preflang;
                    ViewBag.SimpleUserName = SimpleUserName;
                    ViewBag.Preferedlang = UserPref.Preflang;
                    ViewBag.Layout = UserPref.Preflayout;
                    ViewBag.Theme = UserPref.Theme == null ? "DefaultDark" : UserPref.Theme;
                    ViewBag.Appselector = UserPref.Appselector == null ? "Default" : UserPref.Appselector;


                    if (Request.RequestType == "GET")
                    {
                        string curent = Request.UrlReferrer.OriginalString;
                        // if (Request.Params.AllKeys[0] != "Id_Flow") BackUrl = curent;
                        if (!UrlHistory.test(curent))
                        {
                            UrlHistory.Push(curent);
                        }
                    }

                    if (Request.RequestType == "POST")
                    {

                    }

                }
            }
            catch { }
            base.OnActionExecuting(filterContext);
        }
        protected override void OnActionExecuted(ActionExecutedContext ctx)
        {

            if (Session != null) Session["controllerInstance"] = this;

            // Stor persisted Controllervariables 
            foreach (System.Reflection.FieldInfo fi in GetType().GetFields(FieldBindingFlags))
            {
                PersistFieldAttribute attr = PersistFieldAttribute.GetAttribute(fi);
                if (attr != null)
                {
                    switch (attr.Location)
                    {
                        case Location.Session:
                            if (Session != null)
                                Session[attr.GetKeyFor(fi)] = fi.GetValue(this);
                            break;
                    }
                }
            }

            base.OnActionExecuted(ctx);
        }



        public List<Vw_Users> GetAllUsers()
        {
            List<Vw_Users> res = new List<Vw_Users>();
            using (Dao<Vw_Users> dao = new Dao<Vw_Users>(Sdb))
            {
                dao.SqlSelect(@"SELECT * from VW_USERs t", new object[] { });
                if (dao.Result.GetSate(DaoResult.ResCountOneOrMore))
                {
                    return dao.Result.GetRes<Vw_Users>();
                }
                else
                {
                    return new List<Vw_Users>();
                }
            }
        }



        public string Root
        {
            get
            {
                //var context = HttpContext.Current;
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                if (context != null)
                {
                    var request = context.Request;
                    var scheme = request.Url.Scheme;
                    var server = request.Headers["Host"] ?? string.Format("{0}:{1}", request.Url.Host, request.Url.Port);
                    var host = string.Format("{0}://{1}", scheme, server);
                    var root = host + VirtualPathUtility.ToAbsolute("~");
                    return root;
                }
                return string.Empty;
            }
        }
        public string getBaseUrl()
        {
            return Root;
        }
        public string GetBasePath()
        {
            return System.Web.HttpContext.Current.Server.MapPath("/");
        }

        public string LangCallback()
        {
            return Preflang;
        }

        public class LimitedSizeStack<T> : LinkedList<T>
        {
            private readonly int _maxSize;
            public LimitedSizeStack(int maxSize)
            {
                _maxSize = maxSize;
            }

            public bool test(string s)
            {
                try
                {
                    string v = this.First.Value.ToString();
                    return v == s;
                }
                catch { }
                return false;
            }

            public void Push(T item)
            {
                this.AddFirst(item);

                if (this.Count > _maxSize)
                    this.RemoveLast();
            }

            public T Pop()
            {
                var item = this.First.Value;
                this.RemoveFirst();
                return item;
            }
        }

    }
}