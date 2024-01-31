using Dextra.Common;
using Dextra.Database;
using Dextra.Toolsspace;
using DextraLib.GeneralDao;
using Xapp.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using Xapp.FlowDatas;
using System.Web.Routing;
using Dextra.Xforms;
using Dextra.Report;
using DextraLib.XForm;
using System.Text.RegularExpressions;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;

namespace Xapp.Controllers
{

    //[dxAuthorize]
    public class PersonsController : BaseController
    {
        [PersistField]
        public OrgPersonModel Model = null;

        AjaxResultCode Error = new AjaxResultCode();
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (Model == null)
            {
                Model = new OrgPersonModel(Sdb, Rolemanager.AvailableRoles, Rolemanager.CurrentUser);
            }

        }

        // GET: Persons
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Persons()
        {
            return View();
        }



        public ActionResult StartSelect(string Flowname, string Stepname)
        {
            BackUrl = Request.UrlReferrer.OriginalString;
            Model.Getflowstep(Flowname, Stepname);
            return View("Select", Model);
        }

        public ActionResult Select(decimal Id_Flow, bool c = false)
        {
            if (!c) BackUrl = Request.UrlReferrer.OriginalString;
            Model.Getflowstep(Id_Flow);
            return View("Select", Model);
        }

        [HttpPost]
        public ActionResult Select(FormCollection forms)
        {
            UpdateModel(Model.FlowModel.PostedFlowData);
            UpdateModel(Model.ViewModel.Person.ClientPart);

            if (Model.FlowModel._Check()) return Redirect(BackUrl);
            // -- business process
            Model.FlowModel.Complett = true;
            var test = Model.ViewModel.Person.ClientPart.io_SelectedPersonId.ToString();
            string desc = test;

            return Model.Set(BackUrl, View("Select", Model), desc);
        }


        public ActionResult Propertyes(decimal Id_Flow, bool c = false)
        {
            if (!c) BackUrl = Request.UrlReferrer.OriginalString;
            Model.Getflowstep(Id_Flow);



            // TestDocument ds = new TestDocument(Tools.Getpath("DocprobaPdf.html", C.c_HtmlTemplate), Tools.Getpath("DocProcess.html", C.c_HtmlTemplate), "/Index ", getBaseUrl());
            string resume = "";// ds.Render();

            //string retforPdf = ds.ProcTextAreas(ds.RenderdedReport, Request.Params);
            //string retforHtml = ds.ProcTextAreas(ds.RenderdedReport, Request.Params, false);
            ViewBag.Testhtml = resume;



            return View("Propertyes", Model);
        }

        [HttpPost]
        public ActionResult Propertyes(FormCollection forms)
        {
            UpdateModel(Model.FlowModel.PostedFlowData);
            UpdateModel(Model.ViewModel.Org.ClientPart);

            if (Model.FlowModel._Check()) return Redirect(BackUrl);
            // -- business process
            Model.FlowModel.Complett = true;
            var test = Model.ViewModel.Org.ClientPart.io_SelectedOrgId.ToString();
            string desc = test;

            return Model.Set(BackUrl, View("Propertyes", Model), desc);
        }

        public ActionResult Status(decimal Id_Flow, bool c = false)
        {
            if (!c) BackUrl = Request.UrlReferrer.OriginalString;
            Model.Getflowstep(Id_Flow);
            return View("Status", Model);
        }

        [HttpPost]
        public ActionResult Status(FormCollection forms)
        {
            UpdateModel(Model.FlowModel.PostedFlowData);
            UpdateModel(Model.ViewModel.Org.ClientPart);

            if (Model.FlowModel._Check()) return Redirect(BackUrl);
            // -- business process
            Model.FlowModel.Complett = true;
            var test = Model.ViewModel.Org.ClientPart.io_SelectedOrgId.ToString();
            string desc = test;

            return Model.Set(BackUrl, View("Status", Model), desc);
        }

        public ActionResult Contract(decimal Id_Flow, bool c = false)
        {
            if (!c) BackUrl = Request.UrlReferrer.OriginalString;
            Model.Getflowstep(Id_Flow);

            Dao<Agreement> adao = new Dao<Agreement>(Sdb);
            adao.SqlSelect(@"select * from agreement a where A.ID_FLOWS=:0 and A.ID_PERSONS=:1 and A.STATUS='InDeveloped'", new object[] { Id_Flow, Model.ViewModel.Person.ClientPart.io_SelectedPersonId });
            if (!adao.Result.GetSate(DaoResult.ResCountZero))
            {
                Model.ViewModel.Person.ClientPart.io_AgreemetId = adao.Result.GetFirst<Agreement>().Id_Agreement;
            }
            else
            {
                Dao dao = new Dao(Sdb);
                dao.ExecuteNonQuery(@"INSERT INTO MXRF_PERSONS_ORG (ID_PERSON,ID_ORGANIZATION,ID_FLOWS,RECORDVALIDFROM,DATAVALIDFROM,STATUS,CREATOR,ORGGROUP) VALUES  (:0,:1,:2,sysdate,sysdate,'InDeveleoped',:userid,:orggroup) returning ID_MXRF_PERSONS_ORG into :lastid ",
                    new object[] { Model.ViewModel.Person.ClientPart.io_SelectedPersonId, Model.ViewModel.Org.ClientPart.io_SelectedOrgId, Id_Flow });
                if (!dao.Result.Error)
                {
                    Agreement agr = new Agreement();
                    List<SchemaRoots> result = XformUtil.GetXformRootsFromScheme(MvcApplication.XformSchemes["Gschema"],false, "Agreement");

                    string root = "def";
                    if (result.Count > 0) root = result[0].value;
                    Xform temp = new Xform(root);
                    agr.Id_Agreement = null;
                    agr.Id_Persons = Model.ViewModel.Person.ClientPart.io_SelectedPersonId;
                    agr.Id_Mxrf_Persons_Org = dao.Result.Lastid;
                    agr.Id_Flows = Id_Flow;
                    agr.Xmldata = temp.GetXmlStringFromXform();
                    agr.Bid_Agreement = "NEW";
                    agr.Status = "InDeveloped";
                    agr.InitCommonFieldsForAdd(Sdb);
                    adao.SqlInsert(agr);
                    Model.ViewModel.Person.ClientPart.io_AgreemetId = adao.Result.Lastid;
                }
            }

            return View("Contract", Model);
        }

        [HttpPost]
        public ActionResult Contract(FormCollection forms)
        {
            UpdateModel(Model.FlowModel.PostedFlowData);
            UpdateModel(Model.ViewModel.Org.ClientPart);

            if (Model.FlowModel._Check()) return Redirect(BackUrl);
            // -- business process
            Model.FlowModel.Complett = true;
            var test = Model.ViewModel.Org.ClientPart.io_SelectedOrgId.ToString();
            string desc = test;

            return Model.Set(BackUrl, View("Contract", Model), desc);
        }

        #region Agreement
        [HttpPost]

        public JsonResult InsertXform_Agreement(FormCollection forms)
        {
            XformProcess<Agreement> xp = new XformProcess<Agreement>(Sdb, Request.Params);

            //xp.Entity.Recordtype = forms["Recordtype"];  Sample!
            decimal? Id_Flows = null;
            try { Id_Flows = decimal.Parse(forms["Id_Flows"]); } catch { }
            decimal? Id_Organization = null;
            try { Id_Organization = decimal.Parse(forms["Id_Organization"]); } catch { }



            Dao dao = new Dao(Sdb);
            dao.ExecuteNonQuery(@"INSERT INTO MXRF_PERSONS_ORG (ID_PERSON,ID_ORGANIZATION,ID_FLOWS,RECORDVALIDFROM,DATAVALIDFROM,STATUS,CREATOR,ORGGROUP) VALUES  (:0,:1,:2,sysdate,sysdate,'InDeveleoped',:userid,:orggroup) returning ID_MXRF_PERSONS_ORG into :lastid ",
                new object[] { xp.Id_Parent, Id_Organization, Id_Flows });
            if (!dao.Result.Error)
            {
                xp.Entity.Id_Agreement = null;
                xp.Entity.Id_Persons = (decimal)xp.Id_Parent;
                xp.Entity.Id_Mxrf_Persons_Org = dao.Result.Lastid;
                xp.Entity.Id_Flows = Id_Flows;
                xp.Entity.Bid_Agreement = "NEW";
                xp.Entity.Status = "InDeveloped";
                xp.Insert();

            }

            return Json(new { Entity = xp.Entity, Error = xp.Error }, "text/html");
        }

        [HttpPost]

        public JsonResult SaveXform_Agreement(FormCollection forms)
        {
            XformProcess<Agreement> xp = new XformProcess<Agreement>(Sdb, Request.Params);

            //xp.Entity.Itemtype = xp.xform.RootName; Sample!

            xp.Entity.Id_Agreement = (decimal)xp.Id_Entity;
            if (forms["cmd"] != null && forms["cmd"] == "close")
            {
                xp.CloseAndSave();
            }
            else
            {
                xp.Save();
            }
            return Json(new { Entity = xp.Entity, Error = xp.Error }, "text/html");
        }

        public JsonResult DeleteXform_Agreement(decimal Id_Entity, string cmd = null)
        {
            XformProcess<Agreement> xp = new XformProcess<Agreement>(Sdb);
            if (cmd != null && cmd == "close")
            {
                xp.Close(Id_Entity);
            }
            else
            {
                xp.Del(Id_Entity);
            }

            return Json(new { Error = xp.Error }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetXform_Agreement(decimal Id_Entity, string Id_Xform = "formid")
        {
            XformProcess<Agreement> xp = new XformProcess<Agreement>(Sdb);
            //string[] htt = xp.Get(Id_Entity, Id_Xform);  !!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            string[] htt = xp.GetNewVer(Id_Entity, "AllRender", null, Id_Xform);
            return Json(new { Xform = htt[0], XformView = htt[1], Loaded = xp.Loaded, Error = xp.Error }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult TestAgrdoc(FormCollection form)
        {

            string template = form["template"];
            template = Tools.Getpath(template, C.c_HtmlTemplate);
            AgreementDocument agrdoc = new AgreementDocument(Sdb, template, decimal.Parse(form["Id_Flows"]));
            string curtempl = agrdoc.Render();
            string retforHtml = "";
            try
            {
                retforHtml = agrdoc.ProcTextAreas(curtempl, Request.Params);
            }
            catch (Exception ex)
            {

            }

            retforHtml = Regex.Replace(retforHtml, @"<dr:remove>.*</dr:remove>", "");

            Html2Pdf test = new Html2Pdf(retforHtml, getBaseUrl());
            Model.ViewModel.PdfAgrement = test.Bdata;
            MvcApplication.Signal.DownloadFile(SignalRId, "Persons/GetPdf", "TestDoc.pdf");

            //HttpContext.Response.AddHeader("Content-Type", "application/pdf");
            //HttpContext.Response.AddHeader("Content-Disposition", String.Format("attachment; filename=AutoOutlines.pdf; size={0}",test.Bdata.Length.ToString()));
            //HttpContext.Response.BinaryWrite(test.Bdata);
            //HttpContext.Response.End();
            //return null;
            //return File(test.Bdata, "application/pdf","proba.pdf");

            //var a = 11;
            string curtempl1 = System.IO.File.ReadAllText(Tools.Getpath("Close.html", "~/App_Data/Files/HtmlTemplates"));
            retforHtml = curtempl1.Replace(@"src=""~/", @"src=""" + Tools.GetBaseUrl() + "/");
            return Content(retforHtml);
        }

        public ActionResult GetPdf(string filename)
        {
            //HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            //response.Content = new StreamContent(new MemoryStream(Model.ViewModel.PdfAgrement));
            //response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            //response.Content.Headers.ContentLength = Model.ViewModel.PdfAgrement.Length;
            //ContentDispositionHeaderValue contentDisposition = null;
            //if (ContentDispositionHeaderValue.TryParse("inline; filename=" + "Test" + ".pdf", out contentDisposition))
            //{
            //    response.Content.Headers.ContentDisposition = contentDisposition;
            //}

            //return response;

            return File(Model.ViewModel.PdfAgrement, "application/pdf");
        }

        public JsonResult GetAgrementDoc(decimal Id_Flows)
        {
            Dictionary<string, string> h = new Dictionary<string, string>     {
                            { "Id_Flows",Id_Flows.ToString() },
                            { "Valami", "Mas" }
                    };


            AgreementDocument agrdoc = new AgreementDocument(Sdb, "agreement.html", Id_Flows,h);
            string ret = agrdoc.Render("Persons/TestAgrdoc");
            return Json(new { Agrdoc = ret ,Error = Error }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region AgreementProperty

        [HttpPost]
        public JsonResult InsertXform_Vw_Agreementproperty(FormCollection forms)
        {
            string table = forms["table"];

            decimal? Id_Flows = null;
            try { Id_Flows = decimal.Parse(forms["Id_Flows"]); } catch { }

            switch (table)
            {
                case "Agreement_Elements":
                    XformProcess<Agreement_Elements> xp = new XformProcess<Agreement_Elements>(Sdb, Request.Params);
                    
                    xp.Entity.Id_Flows = Id_Flows;
                    xp.Entity.Id_Agreement_Elements = null;
                    xp.Entity.Id_Agreement = (decimal)xp.Id_Parent;
                    xp.Entity.Status = "Active";

                    xp.Entity.Root = xp.xform.RootName;
                    xp.Entity.Complextype = xp.xform.TypeName;

                    xp.Insert();
                    return Json(new { Entity = xp.Entity, Error = xp.Error }, "text/html");
                case "Agreement_Pays":
                    XformProcess<Agreement_Pays> xp1 = new XformProcess<Agreement_Pays>(Sdb, Request.Params);
                    xp1.Entity.Id_Flows = Id_Flows;
                    xp1.Entity.Id_Agreement_Pays = null;
                    xp1.Entity.Id_Agreement = (decimal)xp1.Id_Parent;
                    xp1.Entity.Status = "Active";

                    xp1.Entity.Root = xp1.xform.RootName;
                    xp1.Entity.Complextype = xp1.xform.TypeName;

                    xp1.Insert();
                    return Json(new { Entity = xp1.Entity, Error = xp1.Error }, "text/html");
                default:
                    break;
            }
            return Json(new { Entity = "", Error = "Error" }, "text/html");
        }

        [HttpPost]
        public JsonResult SaveXform_Vw_Agreementproperty(FormCollection forms)
        {


            string table = forms["table"];
            switch (table)
            {
                case "Agreement_Elements":
                    XformProcess<Agreement_Elements> xp = new XformProcess<Agreement_Elements>(Sdb, Request.Params);
                    xp.Entity.Id_Agreement_Elements = (decimal)xp.Id_Entity;

                    xp.Entity.Root = xp.xform.RootName;
                    xp.Entity.Complextype = xp.xform.TypeName;

                    if (forms["cmd"] != null && forms["cmd"] == "close")
                    {
                        xp.CloseAndSave();
                    }
                    else
                    {
                        xp.Save();
                    }
                    return Json(new { Entity = xp.Entity, Error = xp.Error }, "text/html");

                case "Agreement_Pays":
                    XformProcess<Agreement_Pays> xp1 = new XformProcess<Agreement_Pays>(Sdb, Request.Params);
                    xp1.Entity.Id_Agreement_Pays = (decimal)xp1.Id_Entity;

                    xp1.Entity.Root = xp1.xform.RootName;
                    xp1.Entity.Complextype = xp1.xform.TypeName;


                    if (forms["cmd"] != null && forms["cmd"] == "close")
                    {
                        xp1.CloseAndSave();
                    }
                    else
                    {
                        xp1.Save();
                    }
                    return Json(new { Entity = xp1.Entity, Error = xp1.Error }, "text/html");
                default:
                    break;
            }
            return Json(new { Entity = "", Error = "Error" }, "text/html");
        }

        public JsonResult DeleteXform_Vw_Agreementproperty(decimal Id_Entity, string table, string cmd = null)
        {
            //string table = "";
            switch (table)
            {
                case "Agreement_Elements":
                    XformProcess<Agreement_Elements> xq = new XformProcess<Agreement_Elements>(Sdb);
                    if (cmd != null && cmd == "close") xq.Close(Id_Entity);
                    else xq.Del(Id_Entity);
                    return Json(new { Error = xq.Error }, JsonRequestBehavior.AllowGet);
                case "Agreement_Pays":
                    XformProcess<Agreement_Pays> xo = new XformProcess<Agreement_Pays>(Sdb);
                    if (cmd != null && cmd == "close") xo.Close(Id_Entity);
                    else xo.Del(Id_Entity);
                    return Json(new { Error = xo.Error }, JsonRequestBehavior.AllowGet);
                default:
                    break;
            }
            return Json(new { Error = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetXform_Vw_Agreementproperty(decimal Id_Entity, string table, string Id_Xform = "formid")
        {
            // string table = "";
            string[] htt;
            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd.", Formatting = Newtonsoft.Json.Formatting.Indented };
            object ro = new { Entity = "", Xform = "", XformView = "", Error = "" };
            switch (table)
            {
                case "Agreement_Elements":
                    XformProcess<Agreement_Elements> xq = new XformProcess<Agreement_Elements>(Sdb);
                    htt = xq.Get(Id_Entity, Id_Xform);
                    // ro = new { Entity = xq.Entity, Xform = htt[0], XformView = htt[1], Error = xq.Error };
                    return Json(new { Entity = xq.Entity, Xform = htt[0], XformView = htt[1], Error = xq.Error }, JsonRequestBehavior.AllowGet);
                    break;
                case "Agreement_Pays":
                    XformProcess<Agreement_Pays> xo = new XformProcess<Agreement_Pays>(Sdb);
                    htt = xo.Get(Id_Entity, Id_Xform);
                    //ro = new { Entity = xo.Entity, Xform = htt[0], XformView = htt[1], Error = xo.Error };
                    return Json(new { Entity = xo.Entity, Xform = htt[0], XformView = htt[1], Error = xo.Error }, JsonRequestBehavior.AllowGet);
                    break;
                default:
                    break;
            }

            return Json(new { Entity = "", Xform = "", XformView = "", Error = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRecords_Vw_Agreementproperty(decimal Id, string recordtype)
        {
            List<Vw_Agreementproperty> res = new List<Vw_Agreementproperty>();
            Dao<Vw_Agreementproperty> t = new Dao<Vw_Agreementproperty>(Sdb);
            t.SqlSelect("select * from Vw_Agreementproperty where ID_AGREEMENT=:0 ", new object[] { Id });
            if (t.Result.GetSate(DaoResult.ResCountOneOrMore))
            {

                res = t.Result.GetRes<Vw_Agreementproperty>();
            }

            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd.", Formatting = Newtonsoft.Json.Formatting.Indented };
            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(res, settings),
                ContentEncoding = Encoding.UTF8
            };
        }


        #endregion

        #region Propertyes

        public JsonResult InsertXform_Vw_Personsproperty(FormCollection forms)
        {
            string table = forms["table"];
            decimal? Id_Flows = null;
            decimal? ParentParentId = null;
            try { Id_Flows = decimal.Parse(forms["Id_Flows"]); } catch { }
            try { ParentParentId = decimal.Parse(forms["Id_ParentId_Parent"]); } catch { }
            switch (table)
            {
                case "Qualifications":
                    XformProcess<Qualifications> xq = new XformProcess<Qualifications>(Sdb, Request.Params);
                    xq.Entity.Id_Qualifications = null;
                    xq.Entity.Id_Flows = Id_Flows;
                    xq.Entity.Id_Persons = ParentParentId;
                    xq.Entity.Recordtype = xq.xform.RootName;
                    xq.Insert();
                    return Json(new { Entity = xq.Entity, Error = xq.Error }, "text/html");
                case "Othersdata":
                    XformProcess<Othersdata> xo = new XformProcess<Othersdata>(Sdb, Request.Params);
                    xo.Entity.Id_Othersdata = null;
                    xo.Entity.Id_Flows = Id_Flows;
                    xo.Entity.Id_Persons = ParentParentId;
                    xo.Entity.Recordtype = xo.xform.RootName;
                    xo.Insert();
                    return Json(new { Entity = xo.Entity, Error = xo.Error }, "text/html");
                case "Activityes":
                    XformProcess<Activityes> xa = new XformProcess<Activityes>(Sdb, Request.Params);
                    xa.Entity.Id_Activityes = null;
                    xa.Entity.Id_Flows = Id_Flows;
                    xa.Entity.Id_Persons = ParentParentId;
                    xa.Entity.Recordtype = xa.xform.RootName;
                    xa.Insert();
                    return Json(new { Entity = xa.Entity, Error = xa.Error }, "text/html");
                default:
                    break;
            }
            return Json(new { Entity = "", Error = "Error" }, "text/html");
        }
        public JsonResult SaveXform_Vw_Personsproperty(FormCollection forms)
        {
            string table = forms["table"];
            switch (table)
            {
                case "Qualifications":
                    XformProcess<Qualifications> xq = new XformProcess<Qualifications>(Sdb, Request.Params);
                    xq.Entity.Id_Qualifications = (decimal?)xq.Id_Entity;
                    if (forms["cmd"] != null && forms["cmd"] == "close") xq.CloseAndSave();
                    else xq.Save();
                    return Json(new { Entity = xq.Entity, Error = xq.Error }, "text/html");
                case "Othersdata":
                    XformProcess<Othersdata> xo = new XformProcess<Othersdata>(Sdb, Request.Params);
                    xo.Entity.Id_Othersdata = (decimal)xo.Id_Entity;
                    if (forms["cmd"] != null && forms["cmd"] == "close") xo.CloseAndSave();
                    else xo.Save();
                    return Json(new { Entity = xo.Entity, Error = xo.Error }, "text/html");
                case "Activityes":
                    XformProcess<Activityes> xa = new XformProcess<Activityes>(Sdb, Request.Params);
                    xa.Entity.Id_Activityes = (decimal)xa.Id_Entity;
                    if (forms["cmd"] != null && forms["cmd"] == "close") xa.CloseAndSave();
                    else xa.Save();
                    return Json(new { Entity = xa.Entity, Error = xa.Error }, "text/html");
                default:
                    break;
            }
            return Json(new { Entity = "", Error = "Error" }, "text/html");
        }
        public JsonResult DeleteXform_Vw_Personsproperty(decimal Id_Entity, string table, string cmd = null)
        {
            //string table = "";
            switch (table)
            {
                case "Qualifications":
                    XformProcess<Qualifications> xq = new XformProcess<Qualifications>(Sdb);
                    if (cmd != null && cmd == "close") xq.Close(Id_Entity);
                    else xq.Del(Id_Entity);
                    return Json(new { Error = xq.Error }, JsonRequestBehavior.AllowGet);
                case "Othersdata":
                    XformProcess<Othersdata> xo = new XformProcess<Othersdata>(Sdb);
                    if (cmd != null && cmd == "close") xo.Close(Id_Entity);
                    else xo.Del(Id_Entity);
                    return Json(new { Error = xo.Error }, JsonRequestBehavior.AllowGet);
                case "Activityes":
                    XformProcess<Activityes> xa = new XformProcess<Activityes>(Sdb);
                    if (cmd != null && cmd == "close") xa.Close(Id_Entity);
                    else xa.Del(Id_Entity);
                    return Json(new { Error = xa.Error }, JsonRequestBehavior.AllowGet);
                default:
                    break;
            }
            return Json(new { Error = "" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetXform_Vw_Personsproperty(decimal Id_Entity, string table, string Id_Xform = "formid")
        {
            // string table = "";
            string[] htt;
            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd.", Formatting = Newtonsoft.Json.Formatting.Indented };
            object ro = new { Entity = "", Xform = "", XformView = "", Error = "" };
            switch (table)
            {
                case "Qualifications":
                    XformProcess<Qualifications> xq = new XformProcess<Qualifications>(Sdb);
                    htt = xq.Get(Id_Entity, Id_Xform);
                    // ro = new { Entity = xq.Entity, Xform = htt[0], XformView = htt[1], Error = xq.Error };
                    return Json(new { Entity = xq.Entity, Xform = htt[0], XformView = htt[1], Error = xq.Error }, JsonRequestBehavior.AllowGet);
                    break;
                case "Othersdata":
                    XformProcess<Othersdata> xo = new XformProcess<Othersdata>(Sdb);
                    htt = xo.Get(Id_Entity, Id_Xform);
                    //ro = new { Entity = xo.Entity, Xform = htt[0], XformView = htt[1], Error = xo.Error };
                    return Json(new { Entity = xo.Entity, Xform = htt[0], XformView = htt[1], Error = xo.Error }, JsonRequestBehavior.AllowGet);
                    break;
                case "Activityes":
                    XformProcess<Activityes> xa = new XformProcess<Activityes>(Sdb);
                    htt = xa.Get(Id_Entity, Id_Xform);
                    //ro = new { Entity = xa.Entity, Xform = htt[0], XformView = htt[1], Error = xa.Error };
                    return Json(new { Entity = xa.Entity, Xform = htt[0], XformView = htt[1], Error = xa.Error }, JsonRequestBehavior.AllowGet);
                    break;
                default:
                    break;
            }

            //return new ContentResult { ContentType = "application/json", Content = JsonConvert.SerializeObject(ro, settings), ContentEncoding = Encoding.UTF8 };
            return Json(new { Entity = "", Xform = "", XformView = "", Error = "" }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetRecords_Vw_Personsproperty(decimal Id, string recordtype)
        {
            List<Vw_Personsproperty> res = new List<Vw_Personsproperty>();
            Dao<Vw_Personsproperty> t = new Dao<Vw_Personsproperty>(Sdb);
            t.SqlSelect("select * from Vw_Personsproperty where id_flows=:0 ", new object[] { Id });
            if (t.Result.GetSate(DaoResult.ResCountOneOrMore))
            {

                res = t.Result.GetRes<Vw_Personsproperty>();
            }

            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd.", Formatting = Newtonsoft.Json.Formatting.Indented };
            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(res, settings),
                ContentEncoding = Encoding.UTF8
            };
        }


        [dxAuthorize(C.c_role_OrgRead)]
        public JsonResult GetXform_PersonsCard(decimal Id, string Id_Xform = "formid")
        {

            List<string> retval = new List<string>();
            string personname = "";
            Dao<Persons> t = new Dao<Persons>(Sdb);
            t.SqlSelect("SELECT T.*  FROM  persons T WHERE  T.ID_PERSONS=:0 ", new object[] { Id }); // 44 test
            if (t.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                if (string.IsNullOrEmpty(Id_Xform))
                {
                    personname = t.Result.GetFirst<Persons>().Usedname;
                }
                else
                {
                    foreach (Persons item in t.Result.GetRes<Persons>())
                    {
                        personname = item.Usedname;
                        Xform xform = new Xform(item.Xmldata, null);
                        XformGenralDocument1 ds = new XformGenralDocument1(xform.ElementAppinfoDocTemplate, xform);
                        ds.Render();
                        retval.Add(ds.Rendered.Replace("GetPersonPic/", "GetPersonPic/" + item.Id_Persons.ToString()));
                    }

                }
            }
            return Json(new { Forms = retval, Personname = personname, Error = Error }, JsonRequestBehavior.AllowGet);
        }

        public class PropInfoResult
        {
            public int Numberofyear { get; set; }
            public string MaxQualifications { get; set; }
            public List<string> Invokedsum { get; set; }

            public PropInfoResult()
            {

            }
        }

        public JsonResult GetPropertyesInfo(decimal Id, string root, string formid)
        {
            List<string> retval = new List<string>();
            Dao qd = new Dao(Sdb);

            Xform work = new Xform(root, null, "Aschema");
            work.SetElementValueById("Id_person", Id.ToString());

            string sql = work.Elements[0].Appinfo["sql"];
            object[] para = Tools.BuildparamfromXform(ref sql, work);
            qd.SqlSelect(sql, para);
            int count = Tools.SetXformelementFromDaoresult(ref work, qd);




            string[] outs = new string[] { "", "" };
            // outs[0] = work.DefRender.Render(formid);
            XformGenralDocument1 ds = new XformGenralDocument1(work.ElementAppinfoDocTemplate, work, count);
            ds.Render();
            //outs[1] = ds.RenderdedReport;
            outs[1] = ds.Rendered;


            return Json(new { Forms = outs, Error = Error }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PersonsAjax
        [HttpPost]
        [dxAuthorize(C.c_role_PersonsCreate)]
        public JsonResult InsertXform_Persons(FormCollection forms)
        {
            // return InsertXform_Persons(forms);

            XformProcess<Persons> xp = new XformProcess<Persons>(Sdb, Request.Params);

            //xp.Entity.Recordtype = forms["Recordtype"];
            //xp.Entity.Itemtype = xp.xform.RootName;
            //xp.Entity.Id_Parent = (decimal)xp.Id_ParentEntity;

            xp.Entity.Bid_Persons = "NEW";
            xp.Entity.Id_Persons = null;
            xp.Insert();
            return Json(new { Entity = xp.Entity, Error = xp.Error }, "text/html");
        }

        [HttpPost]
        [dxAuthorize(C.c_role_PersonsWrite)]
        public JsonResult SaveXform_Persons(FormCollection forms)
        {
            // return SaveXform_Persons(forms);

            XformProcess<Persons> xp = new XformProcess<Persons>(Sdb, Request.Params);

            //xp.Entity.Recordtype = forms["Recordtype"];
            //xp.Entity.Itemtype = xp.xform.RootName;

            xp.Entity.Id_Persons = (decimal)xp.Id_Entity;
            if (forms["cmd"] != null && forms["cmd"] == "close")
            {
                xp.CloseAndSave();
            }
            else
            {
                xp.Save();
            }

            return Json(new { Entity = xp.Entity, Error = xp.Error }, "text/html");
        }

        [HttpPost]
        [dxAuthorize(C.c_role_PersonsDelete)]
        public JsonResult DeleteXform_Persons(FormCollection forms)
        {
            XformProcess<Persons> xp = new XformProcess<Persons>(Sdb, Request.Params);
            xp.Del();
            return Json(new { Entity = xp.Entity, Error = xp.Error }, "text/html");
        }

        [dxAuthorize(C.c_role_PersonsDelete)]
        public JsonResult DeleteXform_Persons(decimal Id_Persons, string cmd = null)
        {
            XformProcess<Persons> xp = new XformProcess<Persons>(Sdb);
            if (cmd != null && cmd == "close")
            {
                xp.Close(Id_Persons);
            }
            else
            {
                xp.Del(Id_Persons);
            }

            return Json(new { Error = xp.Error }, JsonRequestBehavior.AllowGet);
        }

        [dxAuthorize(C.c_role_PersonsRead)]
        public JsonResult GetXform_Persons(decimal Id_Entity, string Id_Xform = "formid")
        {
            XformProcess<Persons> xp = new XformProcess<Persons>(Sdb);
            string[] htt = xp.Get(Id_Entity, Id_Xform);
            return Json(new { Entity = xp.Entity, Xform = htt[0], XformView = htt[1], Error = xp.Error }, JsonRequestBehavior.AllowGet);
        }

        [dxAuthorize(C.c_role_PersonsRead)]
        public ActionResult GetRecords_Persons(decimal Id, string recordtype)
        {
            List<Persons> res = new List<Persons>();
            Dao<Persons> t = new Dao<Persons>(Sdb);
            if (recordtype == "first")
            {
                t.SqlSelect("select /*A+*/ t.* from Persons t where rownum<11 ", new object[] { });
            } else
            {
                t.SqlSelect("select t.* from Persons t where Id_Persons=:0 ", new object[] { Id });
            }
           
            if (t.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                Records_Persons.Clear();
                Records_Persons = t.Result.GetRes<Persons>();
            }

            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd.", Formatting = Newtonsoft.Json.Formatting.Indented };
            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(res, settings),
                ContentEncoding = Encoding.UTF8
            };
        }

        [dxAuthorize(C.c_role_PersonsRead)]
        public ActionResult GetHierarchy_Persons(decimal Id, decimal level, string recordtype)
        {
            List<Persons> htre = new List<Persons>();
            Dao<Persons> t = new Dao<Persons>(Sdb);
            t.SqlSelect("select * from Persons where T.RECORDTYPe=:2 and level<:0 CONNECT BY PRIOR ID_Persons = ID_PARENT START WITH T.ID_Persons=:1 ORDER SIBLINGS BY ID_ORD ", new object[] { level, Id, recordtype });
            if (t.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                Id = (decimal)t.Result.GetFirst<Persons>().Id_Persons;

                //Hierarchy<Persons> tree = new Hierarchy<Persons>(Sdb);
                //htre = (List<Persons>)tree.GetHierarchy(Id, "Id_Parent");
            }
            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd.", Formatting = Newtonsoft.Json.Formatting.Indented };
            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(htre, settings),
                ContentEncoding = Encoding.UTF8
            };

        }

        [dxAuthorize(C.c_role_PersonsWrite)]
        public JsonResult MoveHierarchyItem_Persons(int Id, int NewParentId, string dropPos)
        {

            Dao<Persons> dao = new Dao<Persons>(Sdb);
            dao.MoveRecord(Id, NewParentId, dropPos, "Id_Parent", "reorder");
            Reorder_Persons(Id);
            return Json(new { Entity = "", Error = Error }, JsonRequestBehavior.AllowGet);
        }

        public void Reorder_Persons(decimal Id)
        {
            List<Persons> work = new List<Persons>();
            Dao<Persons> rodao = new Dao<Persons>(Sdb);
            rodao.SqlSelect("select * from Persons where Id_Parent=:0  order by Id_Ord", new object[] { Id });
            if (rodao.Result.GetSate(DaoResult.ResCountOneOrMore))
            {

                decimal Id_Parent = (decimal)rodao.Result.GetFirst<Persons>().Id_Parent;
                rodao.SqlSelect("select * from Persons where Id_Parent=:0 order by Id_Ord", new object[] { Id_Parent });
                if (rodao.Result.GetSate(DaoResult.ResCountOneOrMore))
                {
                    work = rodao.Result.GetRes<Persons>();
                }
            }
            decimal newordid = 0;
            foreach (Persons p in work)
            {
                //p.Id_Ord = newordid++;
                rodao.SqlUpdate(p);
            }
        }

        [HttpPost]
        public JsonResult SaveXform_SearchPersons(FormCollection forms)
        {
            if (forms["selecdate"] != null)
            {
                try
                {
                    string selecdate = forms["selecdate"];
                    Sdb.GeneralVariables["dtfrom"] = DateTime.ParseExact(selecdate, "yyyy.MM.dd.", CultureInfo.InvariantCulture);
                }
                catch { }

            }
            else
            {
                Sdb.GeneralVariables["dtfrom"] = Sdb.Dbsysdate.Sysdbdate;
            }


            XformProcess<SearchPersons> xp = new XformProcess<SearchPersons>(Sdb, Request.Params);
            string sql = xp.xform.Elements[0].Appinfo["sql"];

            Dao<Persons> pdao = new Dao<Persons>(Sdb);
            object[] para = Tools.BuildparamfromClass(ref sql, xp.Entity);
            pdao.SqlSelect(sql, para);

            //pdao.SqlSelect("select * from Persons where upper(usedname) like 'V%' ",new object[] { });
            Records_Persons.Clear();
            if (pdao.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                Records_Persons = pdao.Result.GetRes<Persons>();
            }

            return Json(new { sql = sql, Entity = xp.Entity, Error = xp.Error }, "text/html");
        }

        public ActionResult GetRecords_Persons_Session(string opt)
        {
            List<Persons> retv = new List<Persons>();
            if (Records_Persons.Count > 0) retv.AddRange(Records_Persons);
            //else retv.Add(new Persons());
            if (string.IsNullOrEmpty(opt))
            {
                Records_Persons.Clear();
            }

            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd.", Formatting = Newtonsoft.Json.Formatting.Indented };
            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(retv, settings),
                ContentEncoding = Encoding.UTF8
            };
        }


        class ChangedDates
        {
            public DateTime? Datavalidfrom { get; set; }
            public ChangedDates()
            {
                //Datavalidfrom = DateTime.Now;
            }
            public ChangedDates(DateTime d)
            {
                Datavalidfrom = d;
            }

        }
        [dxAuthorize(C.c_role_OrgRead)]
        public ActionResult GetValidDates()
        {
            List<ChangedDates> res = new List<ChangedDates>();
            string sql = @"select /*A+USGr*/ distinct(trunc(T.DATAVALIDFROM)) as DATAVALIDFROM from persons T  where T.DATAVALIDTO is not null order by 1";

            Dao<ChangedDates> opdao = new Dao<ChangedDates>(Sdb);
            opdao.SqlSelect(sql, new object[] { });
            if (opdao.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                res = opdao.Result.GetRes<ChangedDates>();
            }
            res.Add(new ChangedDates(Sdb.Dbsysdate.Sysdbdate));
            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd.", Formatting = Newtonsoft.Json.Formatting.Indented };
            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(res, settings),
                ContentEncoding = Encoding.UTF8
            };
        }


        public class SearchPersons
        {
            public string Bid_Persons { get; set; }
            public string Usedname { get; set; }
            public string Email { get; set; }
            public string Birthfirstname { get; set; }
            public string Birthlastname { get; set; }
            public DateTime Birthdate { get; set; }
            public string Placeofbirth { get; set; }
            public string Motherfirstname { get; set; }
            public string Motherlastname { get; set; }
            public SearchPersons()
            {
            }
            public SearchPersons ShallowCopy()
            {
                return (SearchPersons)this.MemberwiseClone();
            }
            public SearchPersons(HttpRequestBase rRequest)
            {
                var type = this.GetType();
                foreach (string key in rRequest.Form.Keys)
                {
                    var property = type.GetProperty(key);
                    if (property != null)
                    {
                        try
                        {
                            var convertedValue = Convert.ChangeType(rRequest.Form[key], property.PropertyType);
                            property.SetValue(this, convertedValue);
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        [PersistField]
        List<Persons> Records_Persons = new List<Persons>();



        [PersistField]
        List<Organization> Records_Organization = new List<Organization>();
        [HttpPost]
        public JsonResult SaveXform_SearchOrganization(FormCollection forms)
        {
            XformProcess<Organization> xp = new XformProcess<Organization>(Sdb, Request.Params);
            string sql = xp.xform.Elements[0].Appinfo["sql"];

            Dao<Organization> pdao = new Dao<Organization>(Sdb);
            object[] para = Tools.BuildparamfromClass(ref sql, xp.Entity);
            pdao.SqlSelect(sql, para);

            //pdao.SqlSelect("select * from Persons where upper(usedname) like 'V%' ",new object[] { });
            Records_Organization.Clear();
            if (pdao.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                Records_Organization = pdao.Result.GetRes<Organization>();
            }

            return Json(new { sql = sql, Entity = xp.Entity, Error = Error }, "text/html");
        }

        public ActionResult GetRecords_Organization_Session(string opt)
        {
            List<Organization> retv = new List<Organization>();
            if (Records_Organization.Count > 0) retv.AddRange(Records_Organization);
            //else retv.Add(new Persons());
            if (string.IsNullOrEmpty(opt))
            {
                Records_Organization.Clear();
            }

            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd.", Formatting = Newtonsoft.Json.Formatting.Indented };
            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(retv, settings),
                ContentEncoding = Encoding.UTF8
            };
        }

        #endregion
    }
}
