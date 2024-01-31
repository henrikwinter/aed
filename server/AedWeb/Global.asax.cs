using Dextra.Common.SignalR.Hubs;
using Dextra.Database;
using Dextra.Flowbase;
using Dextra.Toolsspace;
using DextraLib.XForm;
using log4net;
using Xapp.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Xml.Schema;
using System.Text;

using Microsoft.VisualBasic.FileIO;

namespace Xapp
{

    public class LangContent
    {
        public string HuValue { get; set; }
        public string EnValue { get; set; }
        public string OthValue { get; set; }
        public LangContent()
        {

        }
        public LangContent(string hv, string ev, string ov)
        {
            HuValue = hv;
            EnValue = ev;
            OthValue = ov;

        }
    }

    public class MvcApplication : System.Web.HttpApplication
    {

        public static bool ErrorInstart = false;

        public static GlobalDictionary Gdict = new GlobalDictionary();

        public static Dictionary<string, XmlSchema> XformSchemes = new Dictionary<string, XmlSchema>();

        public static XSchema Globschema = new XSchema();
        public static XmlSchema Gschema = new XmlSchema();

        public static XSchema Appschema = new XSchema();
        public static XmlSchema Aschema = new XmlSchema();


        public static DbContext Adb = new DbContext();

        // public static Dictionary<string, string> LangDictionary = new Dictionary<string, string>();

        public static Dictionary<string, LangContent> LangDictionaryM = new Dictionary<string, LangContent>();

        public static Dictionary<string, LangContent> EnumDictionaryM = new Dictionary<string, LangContent>();

        public static ILog GeneralLog = log4net.LogManager.GetLogger("General");
        public static ILog AuditlLog = log4net.LogManager.GetLogger("Audit");

        public static SignalHub Signal;

        public static string ApplicationPath;


        public static LoadRoleManagedFlows Flowroles = new LoadRoleManagedFlows();
        public static LoadRoleManagedFunctions Functionroles = new LoadRoleManagedFunctions();

        public static void LoadRoles()
        {
            Flowroles = new LoadRoleManagedFlows(C.FlowFile);
            Functionroles = new LoadRoleManagedFunctions(C.FunctionFile);
        }



        public static string GetPhysicalPath(string path)
        {
            return Path.Combine(HttpRuntime.AppDomainAppPath, path);
        }

        public static void LoadGlobalSchema()
        {
            XformSchemes.Clear();

            Globschema = new XSchema(Tools.Getpath(C.GlobalSchemaFile, C.SchemaDir));
            Gschema = Globschema.schema;
            Appschema = new XSchema(Tools.Getpath(C.AppSchemaFile, C.SchemaDir));
            Aschema = Appschema.schema;
            XformSchemes.Add("Aschema", Aschema);
            XformSchemes.Add("Gschema", Gschema);


        }


        public static void SaveLangDictionary()
        {

            string filepath = Tools.Getpath(C.LangFileOut, C.LangDir);
            File.Delete(filepath);
            using (StreamWriter outputFile = new StreamWriter(filepath, false, Encoding.UTF8))
            {
                foreach (var item in LangDictionaryM)
                {
                    string oline = Langue.escapecsv(item.Key) + "," + Langue.escapecsv(item.Value.HuValue) + "," + Langue.escapecsv(item.Value.EnValue) + "," + Langue.escapecsv(item.Value.OthValue);

                    outputFile.WriteLine(oline);
                }
            }
        }


        public static void LoadLangDictionary(SignalCallback callback = null, bool force = false)
        {
            int cik = 0;
            int start = 0;
            int end = File.ReadAllLines(Tools.Getpath(C.LangFile, C.LangDir)).Length;


            string lf = Tools.Getpath(C.LangFile, C.LangDir);

            string e = Tools.GetEncoding(lf);
            if (e != "UTF-8")
            {
                string valami = Tools.readFileAsUtf8(lf);
                File.WriteAllText(lf, valami);
                force = true;
            }

            if (force)
            {
                // LangDictionary.Clear();
                LangDictionaryM.Clear();

                if (callback != null) callback("Nyelv", "Start", start, end, cik++);


                using (TextFieldParser csvParser = new TextFieldParser(lf))
                {
                    csvParser.CommentTokens = new string[] { "#" };
                    csvParser.SetDelimiters(new string[] { "," });
                    csvParser.HasFieldsEnclosedInQuotes = true;

                    // Skip the row with the column names
                    csvParser.ReadLine();

                    while (!csvParser.EndOfData)
                    {
                        // Read current line fields, pointer moves to the next line.
                        string[] fields = csvParser.ReadFields();
                        try
                        {
                            LangDictionaryM.Add(fields[0].Trim(), new LangContent(fields[1].Trim(), fields[2].Trim(), fields[3].Trim()));
                        }
                        catch { }
                        if (callback != null) callback("Nyelv", "Progress", start, end, cik++);
                    }
                }

                /*
                    StreamReader reader = new StreamReader(lf);
                    while (reader.Peek() >= 0)
                    {
                        string line = reader.ReadLine().Replace(',', ';');
                        try
                        {
                           // LangDictionary.Add(line.Split(';')[0].Trim(), line.Split(';')[1].Trim());
                            LangDictionaryM.Add(line.Split(';')[0].Trim(), new LangContent(line.Split(';')[1].Trim(), line.Split(';')[2].Trim(), line.Split(';')[3].Trim()));
                        }
                        catch
                        {
                        }
                        if (callback != null) callback("Nyelv", "Progress", start, end, cik++);
                    }
                    reader.Close();
                */

                if (callback != null) callback("Nyelv", "End", start, end, cik++);
            }
        }


        public static void SaveEnumDictionary()
        {

            string filepath = Tools.Getpath(C.EnumFileOut, C.LangDir);
            File.Delete(filepath);
            using (StreamWriter outputFile = new StreamWriter(filepath, false, Encoding.UTF8))
            {
                foreach (var item in EnumDictionaryM)
                {
                    //string oline = item.Key + "," + item.Value.HuValue + "," + item.Value.EnValue + "," + item.Value.OthValue;
                    string oline = Langue.escapecsv(item.Key) + "," + Langue.escapecsv(item.Value.HuValue) + "," + Langue.escapecsv(item.Value.EnValue) + "," + Langue.escapecsv(item.Value.OthValue);

                    outputFile.WriteLine(oline);
                }
            }
        }


        public static void LoadEnumDictionary(SignalCallback callback = null, bool force = false)
        {
            int cik = 0;
            int start = 0;
            int end = File.ReadAllLines(Tools.Getpath(C.EnumFile, C.LangDir)).Length;


            string lf = Tools.Getpath(C.EnumFile, C.LangDir);

            string e = Tools.GetEncoding(lf);
            if (e != "UTF-8")
            {
                string valami = Tools.readFileAsUtf8(lf);
                File.WriteAllText(lf, valami);
                force = true;
            }

            if (force)
            {
                // LangDictionary.Clear();
                EnumDictionaryM.Clear();

                if (callback != null) callback("Nyelv", "Start", start, end, cik++);


                using (TextFieldParser csvParser = new TextFieldParser(lf))
                {
                    csvParser.CommentTokens = new string[] { "#" };
                    csvParser.SetDelimiters(new string[] { "," });
                    csvParser.HasFieldsEnclosedInQuotes = true;

                    // Skip the row with the column names
                    csvParser.ReadLine();

                    while (!csvParser.EndOfData)
                    {
                        // Read current line fields, pointer moves to the next line.
                        string[] fields = csvParser.ReadFields();
                        EnumDictionaryM.Add(fields[0].Trim(), new LangContent(fields[1].Trim(), fields[2].Trim(), fields[3].Trim()));
                        if (callback != null) callback("Nyelv", "Progress", start, end, cik++);
                    }
                }


                /*
                StreamReader reader = new StreamReader(lf);
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine().Replace(',', ';');
                    try
                    {
                        // LangDictionary.Add(line.Split(';')[0].Trim(), line.Split(';')[1].Trim());
                        EnumDictionaryM.Add(line.Split(';')[0].Trim(), new LangContent(line.Split(';')[1].Trim(), line.Split(';')[2].Trim(), line.Split(';')[3].Trim()));
                    }
                    catch
                    {
                    }
                    if (callback != null) callback("Nyelv", "Progress", start, end, cik++);
                }
                reader.Close();
                */

                if (callback != null) callback("Nyelv", "End", start, end, cik++);
            }
        }



        protected void Session_Start(object sender, EventArgs e)
        {
            Session["sid"] = Session.SessionID;

        }

        public DbSysDate Dbsysdate { get; set; }

        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            Signal = new SignalHub();

            var exceptionLogger = new LogExceptionFilterAttribute();
            GlobalFilters.Filters.Add(exceptionLogger);


            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
           // GlobalFilters.Filters.Add(new MyExceptionHandler());

            try
            {
                LoadGlobalSchema();
                LoadLangDictionary(null, true);
                LoadEnumDictionary(null, true);
                LoadRoles();



                string DbSourceLocal = WebConfigurationManager.ConnectionStrings["LocalConnection"].ConnectionString;
                string DbSource = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                Adb = new DbContext(DbSource);
                Dbsysdate = new DbSysDate(Adb);
                Adb.SetDate("dtfrom", Dbsysdate.Sysdbdate);
                throw new Exception("Stop");
            }
            catch (Exception ex)
            {
                /// ??? mi a f kellene
                //throw new Exception("Stop");

            }


            ApplicationPath = Path.Combine(HttpRuntime.AppDomainAppPath, "");

        }


        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    Response.Filter = null;
                }
                catch { }

                Exception serverException = Server.GetLastError();
                //WebErrorHandler errorHandler = null;

                //Try to log the inner Exception since that's what
                //contains the 'real' error. 
                if (serverException.InnerException != null)
                    serverException = serverException.InnerException;

                // Custom logging and notification for this application 
                //AppUtils.LogAndNotify(new WebErrorHandler(serverException));


                //if (App.Configuration.DebugMode == DebugModes.DeveloperErrorMessage)
                if (1==1)
                    {
                    Response.TrySkipIisCustomErrors = true;
                    Server.ClearError();
                    Response.StatusCode = 500;
                    //MessageDisplay.DisplayMessage("Application Error", "<pre style='font: normal 8pt Arial'>" + ErrorDetail + "</pre>");
                    return;
                }
                else if (1==6)
                {
                    Response.TrySkipIisCustomErrors = true;
                    string StockMessage = "The Server Administrator has been notified and the error logged.<p>"; // +
                            //"Please continue on by either clicking the back button or by returning to the home page.<p>" +
                            //"<center><b><a href='" + App.Configuration.WebLogHomeUrl + "'>Web Log Home Page</a></b></center>";

                    // Handle some stock errors that may require special error pages
                    var HEx = serverException as HttpException;
                    if (HEx != null)
                    {
                        int HttpCode = HEx.GetHttpCode();
                        Server.ClearError();

                        if (HttpCode == 404) // Page Not Found 
                        {
                            Response.StatusCode = 404;
                            //MessageDisplay.DisplayMessage("Page not found","You've accessed an invalid page on this Web server. " +StockMessage);
                            return;
                        }
                        if (HttpCode == 401) // Access Denied 
                        {
                            Response.StatusCode = 401;
                            //MessageDisplay.DisplayMessage("Access Denied","You've accessed a resource that requires a valid login. " +StockMessage);
                            return;
                        }
                    }

                    Server.ClearError();
                    Response.StatusCode = 500;

                    // generate a custom error page 
                    //MessageDisplay.DisplayMessage("Application Error","We're sorry, but an unhandled error occurred on the server. " +StockMessage);

                    return;
                }

                // default behavior
                return;
            }
            catch (Exception ex)
            {
                // default error handling 
                //if (App.Configuration.DebugMode == DebugModes.Default)
                if(1==1)
                {
                    // and display an error message
                    Response.TrySkipIisCustomErrors = true;

                    // Just throw the exception
                    throw ex;
                }
                else
                {

                    Server.ClearError();
                    Response.TrySkipIisCustomErrors = true;
                    Response.StatusCode = 200;
                    // MessageDisplay.DisplayMessage("Application Error Handler Failed","The application Error Handler failed with an exception." +(App.Configuration.DebugMode == DebugModes.DeveloperErrorMessage ? "<pre>" + ex.ToString() + "</pre>" : ""));
                }

            }
        }


        protected void Application_Error()
        {
            HttpContext httpContext = HttpContext.Current;
            if (httpContext != null)
            {

                /* When the request is ajax the system can automatically handle a mistake with a JSON response. 
                   Then overwrites the default response */
                //bool uplf = IsMaxRequestExceededException(this.Server.GetLastError());



                ///++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++   ez itt proba session timeout eseten nem megy ay error pagera !??
                ///
                if (1 != 1)
                {
                    var error = Server.GetLastError();
                    var code = (error is HttpException) ? (error as HttpException).GetHttpCode() : 500;

                    if (code != 404)
                    {
                        // Generate email with error details and send to administrator
                        // I'm using RazorMail which can be downloaded from the Nuget Gallery
                        // I also have an extension method on type Exception that creates a string representation
                        //var email = new RazorMailMessage("Website Error");
                        //email.To.Add("errors@wduffy.co.uk");
                        //email.Templates.Add(error.GetAsHtml(new HttpRequestWrapper(Request)));
                        //Kernel.Get<IRazorMailSender>().Send(email);
                    }


                    Response.Clear();
                    Server.ClearError();

                    string path = Request.Path;
                    Context.RewritePath(string.Format("~/Errors/Http{0}", code), false);
                    IHttpHandler httpHandler = new MvcHttpHandler();
                    httpHandler.ProcessRequest(Context);
                    Context.RewritePath(path, false);

                }
                ///++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++   ez itt proba


                if (((MvcHandler)httpContext.CurrentHandler) != null && ((MvcHandler)httpContext.CurrentHandler).RequestContext != null)
                {
                    RequestContext requestContext = ((MvcHandler)httpContext.CurrentHandler).RequestContext;
                    if (requestContext.HttpContext.Request.IsAjaxRequest())
                    {
                        httpContext.Response.Clear();
                        string controllerName = requestContext.RouteData.GetRequiredString("controller");
                        IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();
                        IController controller = factory.CreateController(requestContext, controllerName);
                        ControllerContext controllerContext = new ControllerContext(requestContext, (ControllerBase)controller);

                        JsonResult jsonResult = new JsonResult
                        {
                            Data = new { success = false, serverError = "500" },
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                        jsonResult.ExecuteResult(controllerContext);
                        httpContext.Response.End();
                    }
                    else
                    {
                        Exception exception = httpContext.Server.GetLastError();
                        if (exception is TestException) httpContext.Response.Redirect("~/Error/TestException");
                        else if (exception is HardException) httpContext.Response.Redirect("~/Error/HardException");
                        else if (IsMaxRequestExceededException(this.Server.GetLastError())) httpContext.Response.Redirect("~/Error/Largefile");
                        // erre szok visitani
                        try
                        {
                            httpContext.Response.Redirect("~/Error/General");
                        }
                        catch
                        {
                            var error = Server.GetLastError();
                            var code = (error is HttpException) ? (error as HttpException).GetHttpCode() : 500;

                            Response.Clear();
                           // httpContext.Response.ClearHeaders();
                          //  httpContext.Response.ClearContent();
                            Server.ClearError();

                            string path = Request.Path;
                            //Context.RewritePath(string.Format("~/Errors/Http{0}", code), false);
                            Context.RewritePath("~/Error/General", false);
                            IHttpHandler httpHandler = new MvcHttpHandler();
                            httpHandler.ProcessRequest(Context);
                            Context.RewritePath(path, false);
                        }

                    }
                }
            }
        }


        const int TimedOutExceptionCode = -2147467259;
        private static bool IsMaxRequestExceededException(Exception e)
        {
            // unhandled errors = caught at global.ascx level
            // http exception = caught at page level

            Exception main;
            var unhandled = e as HttpUnhandledException;

            if (unhandled != null && unhandled.ErrorCode == TimedOutExceptionCode)
            {
                main = unhandled.InnerException;
            }
            else
            {
                main = e;
            }


            var http = main as HttpException;

            if (http != null && http.ErrorCode == TimedOutExceptionCode)
            {
                // hack: no real method of identifying if the error is max request exceeded as 
                // it is treated as a timeout exception
                if (http.StackTrace.Contains("GetEntireRawContent"))
                {
                    // MAX REQUEST HAS BEEN EXCEEDED
                    return true;
                }
            }

            return false;
        }

        protected void Application_EndRequest()
        {
            if (Context.Items["AjaxPermissionDenied"] is bool)
            {
                Context.Response.StatusCode = 401;
                Context.Response.End();
            }
        }

    }


}

