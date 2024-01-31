using Dextra.Common;
using Dextra.Database;
using Dextra.Flowbase;
using Dextra.Toolsspace;
using DextraLib.GeneralDao;
using DextraLib.Report;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using Xapp.Db;
using Xapp.FlowDatas;
using Xapp.Models;
using Dextra.Xforms;

namespace Xapp.Controllers
{

    public class FlostepDocument : DocumentSourceOld
    {

        public object Data { get; set; }

        public FlostepDocument(string template) :base(template)
        {

        }

    }
    [Authorize]
    public class FlowController : BaseController
    {
        //[PersistField]SimpleFlowModel
        public SimpleFlowModel Model = null;


        //public FlowManager<FlowModeldata> FlowMan = new FlowManager<FlowModeldata>();
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (Model == null || Model.FlowModel.CurrentUser!= Rolemanager.CurrentUser)
            {
                Model = new SimpleFlowModel(Sdb, Rolemanager.AvailableRoles, Rolemanager.CurrentUser);
            }
        }

        public JsonResult GetRowdetails(decimal id_flow)
        {
            Model.Getflowstep(id_flow);
            string template = "";
            try {
                template = System.IO.File.ReadAllText(Tools.Getpath(Model.FlowModel.CurrentFlowstep.Flowtemplate, C.c_HtmlTemplateFlow), Encoding.UTF8);
            } catch { template = @"<h1>Nofile</h1>"; }
            
            AjaxResultCode Error = new AjaxResultCode();
            
            string result = Model.FlowModel.dbflow.Pvariables;
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);
            string typename = C.c_FlowstepDataNamespace + xml.DocumentElement.Name;
            object item = Activator.CreateInstance(Type.GetType(typename));
            item= Tools.Deserialize(item, Model.FlowModel.dbflow.Pvariables);

            FlostepDocument doc = new FlostepDocument(template);
            doc.Data = item;
            string rendered = "";

            try { rendered = doc.Render(); } catch { rendered = @"<h1>Error</h1>"; }
            return Json(new { rendered = rendered, Error = Error }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OpenFlow(decimal Id_Flow)
        {
            Model.Getflowstep(Id_Flow);
            return RedirectToAction(Model.FlowModel.CurrentFlowstep.Action, Model.FlowModel.CurrentFlowstep.Controller, new  { Id_Flow = Id_Flow,c=true });
        }

        public ActionResult GetFlows()
        {
            AjaxResultCode Error = new AjaxResultCode();
            List<Flow> result = new List<Flow>();
            Dao<Flow> fdao = new Dao<Flow>(Sdb);
            string w = Model.FlowModel.getAvilableFlowsteps();
            fdao.SqlSelect("select /*A+*/ * from flows f where F.FLOWNAME||F.STEPNAME in ("+w+")", new object[] { });
            //T.Id_Flow,T.Id_Parrentflow,T.Bid_Flow,T.Flowname,T.Stepname,T.Controller,T.Action,T.Title,T.Flowhistory,T.Pvariables,T.Recordvalidfrom 
            if (fdao.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                result = fdao.Result.GetRes<Flow>();
            }

            XmlDocument tempx = new XmlDocument();
            foreach (Flow i in result)
            {
                try
                {
                    XrefRole wa = MvcApplication.Flowroles.XrefFlows.Find(r => r.Flowname == i.Flowname && r.Stepname == i.Stepname);
                    i.Attributum = wa.Mode;

                    tempx.LoadXml(i.Flowhistory);
                    string describe = "";
                    i.Flowname = Langue.Translate(i.Flowname, "FLOWS", Preflang);
                    i.Title = Langue.Translate(i.Title, "FLOWS", Preflang);
                    i.Stepname = Langue.Translate(i.Stepname, "FLOWS", Preflang);
                    XmlNode titleNode = tempx.SelectSingleNode("//Flow/Action/StartStep/Describe");

                    if (titleNode != null)
                    {
                        string temp = Langue.Translate(titleNode.InnerText, "FLOWS", Preflang);
                        describe = temp;
                        i.Title += describe;
                    }



                }
                catch {
                    int r = 22;
                }
            }

            JsonSerializerSettings settings = new JsonSerializerSettings{DateFormatString = "yyyy.MM.dd.",Formatting = Newtonsoft.Json.Formatting.Indented};
            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(result, settings),
                ContentEncoding = Encoding.UTF8
            };
            //return Json(new { result =ttt , Error = Error }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFlowsXml()
        {
            AjaxResultCode Error = new AjaxResultCode();
            string result = "";
            Xform xflows = new Xform("Flows");
            xflows.InitDefaults();

         //   result = MvcHtmlString.Create(xflows.RenderWithTemplate("Xform1")).ToString();
            return Json(new { result = result, Error = Error }, JsonRequestBehavior.AllowGet);
        }


        // GET: Flow
        public ActionResult Index()
        {
            FlowViewModel model = new FlowViewModel();
            model.Startableflows = Model.FlowModel.StartableFlowSteps();
            return View(model);
        }
    }
}
