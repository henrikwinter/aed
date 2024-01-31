using Dextra.Common;
using Dextra.Toolsspace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Xapp.Controllers
{
    public class DevController : BaseController
    {
        // GET: Dev
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult prcGenerator(FormCollection form)
        {
            AjaxResultCode Error = new AjaxResultCode();
            string path = "Work/";
            string Entitydesc = form["txtEntityDesc"];
            List<CustEntity> ent = Generator.Genratorprc(Entitydesc, path);

            return Json(new { Out = "", Error = Error }, "text/html");
        }

    }
}