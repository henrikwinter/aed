using Dextra.Common;
using log4net;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Xapp.Models;

namespace Xapp.Controllers
{

    public class MyExceptionHandler : ActionFilterAttribute, IExceptionFilter

    {

        public void OnException(ExceptionContext filterContext)

        {

            Exception e = filterContext.Exception;

            filterContext.ExceptionHandled = true;

            filterContext.Result = new ViewResult()

            {

                ViewName = "Index"

            };

        }

    }



    public class LogExceptionFilterAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            // Log the exception here with your logging framework of choice.
            ErrorLogService.LogError(filterContext.Exception);

        }
    }

    public static class ErrorLogService
    {
        public static ILog Log = log4net.LogManager.GetLogger(typeof(ErrorLogService));
        public static void LogError(Exception ex)
        {
            //Email developers, call fire department, log to database etc.
            Log.Error(ex.Message, ex);
        }
    }



    public class TestException : Exception
    {
        public TestException(string m)
            : base(m)
        {


        }
    }
    public class HardException : Exception
    {
        public HardException(string m)
            : base(m)
        {


        }
    }

    public class NotFoundModel : HandleErrorInfo
    {
        public NotFoundModel(Exception exception, string controllerName, string actionName)
            : base(exception, controllerName, actionName)
        {

        }
        public string RequestedUrl { get; set; }
        public string ReferrerUrl { get; set; }
    }


    public class ErrorController : BaseController
    {

        public ErrorController()
        {

        }

        // GET: Error
        public ActionResult Index()
        {
            //return View();
            return View("Index", new ErrorModel() { ErrorTitle = "General Error", ExceptionDetail = null });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public ActionResult General(Exception exception)
        {
            MvcApplication.GeneralLog.Debug("General ....", exception);
            // ErrMsgs.Add(exception.ToString());
            if (Request.IsAjaxRequest())
            {
                //Lets return Json
                Response.ClearContent();
                Response.Write(exception.Message);
                Response.StatusCode = 410;
                return new HttpStatusCodeResult(410, exception.Message);
                //return Json(new { Error = exception.Message });
            }
            return View("Index", new ErrorModel() { ErrorTitle = "General Error", ExceptionDetail = exception });
        }


        // Largefile
        public ActionResult Largefile(Exception exception)
        {
            Response.ClearContent();
            Server.ClearError();
            string json = new JavaScriptSerializer().Serialize(new { isUploaded = false, message = "Error!", newid = 0 });
            Response.Write(json);
            Response.StatusCode = 200;
            return new HttpStatusCodeResult(200, exception.Message);
           //return Json(new { isUploaded = false, message = "File uploaded Error!", newid = 0 });
        }

        public ActionResult TestException(Exception exception)
        {
            MvcApplication.GeneralLog.Debug("Test ....", exception);
            // ErrMsgs.Add(exception.ToString());
            if (Request.IsAjaxRequest())
            {
                Response.ClearContent();
                Response.Write(exception.Message);
                Response.StatusCode = 410;
                return new HttpStatusCodeResult(410, exception.Message);

            }

            return View("Index", new ErrorModel() { ErrorTitle = "TestException Error", ExceptionDetail = exception });
        }


        public ActionResult HardException(Exception exception)
        {
            MvcApplication.GeneralLog.Debug("Hard ....", exception);
            //   ErrMsgs.Add(exception.ToString());
            if (Request.IsAjaxRequest())
            {
                Response.ClearContent();
                Response.Write(exception.Message);
                Response.StatusCode = 410;
                return new HttpStatusCodeResult(410, exception.Message);

            }
            return View("Index", new ErrorModel() { ErrorTitle = "HardException Error", ExceptionDetail = exception });
        }



        public ActionResult Http404(Exception exception)
        {
            if (Request.IsAjaxRequest())
            {
                Response.ClearContent();
                Response.Write(exception.Message);
                Response.StatusCode = 410;
                return new HttpStatusCodeResult(410, exception.Message);

            }
            return View("Index", new ErrorModel() { ErrorTitle = "Not found", ExceptionDetail = exception });
            //return View();

        }

        public ActionResult Http403(Exception exception)
        {
            if (Request.IsAjaxRequest())
            {
                Response.ClearContent();
                Response.Write(exception.Message);
                Response.StatusCode = 410;
                return new HttpStatusCodeResult(410, exception.Message);

            }
            return View("Index", new ErrorModel() { ErrorTitle = "Forbidden", ExceptionDetail = exception });

        }

        public ActionResult NotFound(string url)
        {
            var originalUri = url ?? Request.QueryString["aspxerrorpath"] ?? Request.Url.OriginalString;
            var controllerName = (string)RouteData.Values["controller"];
            var actionName = (string)RouteData.Values["action"];
            var model = new NotFoundModel(new HttpException(404, "Failed to find page"), controllerName, actionName) { RequestedUrl = originalUri, ReferrerUrl = Request.UrlReferrer == null ? "" : Request.UrlReferrer.OriginalString };
            Response.StatusCode = 404;
            return View("Index", model);
        }
        //protected override void HandleUnknownAction(string actionName)
        //{
        //    var name = GetViewName(ControllerContext, "~/Views/Error/{0}.cshtml", actionName, "~/Views/Error/Error.cshtml", "~/Views/Error/General.cshtml", "~/Views/Shared/Error.cshtml");
        //    var controllerName = (string)RouteData.Values["controller"];
        //    //var model = new HandleErrorInfo(Server.GetLastError(), controllerName, actionName);
        //    //var result = new ViewResult { ViewName = name, ViewData = new ViewDataDictionary<HandleErrorInfo>(model), };
        //    Response.StatusCode = 501;
        // //   result.ExecuteResult(ControllerContext);
        //}

        protected string GetViewName(ControllerContext context, params string[] names)
        {
            foreach (var name in names)
            {
                var result = ViewEngines.Engines.FindView(ControllerContext, name, null);
                if (result.View != null) return name;
            }
            return null;
        }

    }
}