using Dextra.Common;
using Dextra.Toolsspace;
using Dextra.Xforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Web.Routing;
using Xapp.Db;
using System.Web;
using Newtonsoft.Json;
using System.Text;
using DextraLib.XForm;
//using Dextra.Database.Local;
using DextraLib.GeneralDao;
using Dextra.Flowbase;
using System.Web.Helpers;
using Dextra.Database;
using System.Xml;
using System.Xml.Schema;
using static DextraLib.XForm.XformUtil;

namespace Xapp.Controllers
{


    public class Combosource
    {
        public string Typename { get; set; }
        public string Display { get; set; }
        public Combosource()
        {


        }
        public Combosource(string t, string d)
        {
            Typename = t;
            Display = d;

        }
    }

    [Authorize]
    public class AjaxController : BaseController
    {
        AjaxResultCode Error = new AjaxResultCode();

        // GET: Ajax
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

        [AllowAnonymous]
        public JsonResult RegisterSessionSignalId(string id)
        {

            SignalRId = id;
            List<RenderSartableFunctions> ret = new List<RenderSartableFunctions>();
            AjaxResultCode Error = new AjaxResultCode();
            SignalRId = id;
            try {
                DxFunctionManager FuncMan = new DxFunctionManager(Sdb, Rolemanager.AvailableRoles, Rolemanager.CurrentUser);

                ret = FuncMan.StartableFunctions("Navbar");

            }
            catch { }
            
            return Json(new { Ret = ret, Error = Error }, JsonRequestBehavior.AllowGet);
        }
        public void ReloadLang_ProgresBarrSetting(string msg, string command, int start, int end, int percent)
        {
            MvcApplication.Signal.ProgresBarrSetting(SignalRId, msg, command, start, end, percent);
            System.Threading.Thread.Sleep(200);
            string user = Rolemanager.CurrentUser;
        }
        public JsonResult LoadLang(string id)
        {
            AjaxResultCode Error = new AjaxResultCode();
            MvcApplication.LoadLangDictionary(ReloadLang_ProgresBarrSetting,true);
            // MvcApplication.LoadRoles();
            return Json(new { Ret = "Ok", Error = Error }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadEnum(string id)
        {
            AjaxResultCode Error = new AjaxResultCode();
            MvcApplication.LoadEnumDictionary(ReloadLang_ProgresBarrSetting, true);
            // MvcApplication.LoadRoles();
            return Json(new { Ret = "Ok", Error = Error }, JsonRequestBehavior.AllowGet);
        }




        public JsonResult GetProcFlowext(string Flowname, string Stepname, string Id_Flow)
        {
            AjaxResultCode Error = new AjaxResultCode();
            string root = Flowname + Stepname;
            XmlSchemaElement RootItems = MvcApplication.Gschema.Elements.Values.OfType<XmlSchemaElement>().FirstOrDefault(Elements => Elements.Name == root);
            if (RootItems == null)
            {
                if (string.IsNullOrEmpty(Id_Flow))
                {
                    root = C.c_FlowstepDataStartRoot;
                }
                else
                {
                    root = C.c_FlowstepDataDefRoot;
                }

            }
            Xform xform = new Xform(root);
            xform.InitDefaults();
            MvcHtmlString ret = MvcHtmlString.Create(xform.DefRender.Render_simple("Xform", Preflang));
            return Json(new { ret = ret.ToHtmlString(), Error = Error }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GenerateHistoryxml()
        {
            Xform workxform = new Xform(Request.Params);
            
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(workxform.GetXmlStringFromXform());
            XmlElement root = xmlDoc.DocumentElement;
            XmlNodeList nodes = root.SelectNodes("//" + workxform.RootName);
            string xml = xmlDoc.InnerXml;
            return Json(new { xml = xml }, "text/html");
        }

        public JsonResult GetProcFlowHist(decimal id_flow)
        {
            string res = "";
            Dao<Flow> fd = new Dao<Flow>(Sdb);
            fd.SqlSelectId(id_flow);
            if (fd.Result.GetSate(DaoResult.ResCountOne))
            {
                Flow fl = fd.Result.GetFirst<Flow>();
                res = Flowutils.RenderHistory(fl.Flowhistory);
            }
            return Json(new { frm = res }, JsonRequestBehavior.AllowGet);

        }




        ///--------------------------------------------------

        public JsonResult NewXformId(string root, string refroot, string formid, string template = null, string selector = "Gschema")
        {
            AjaxResultCode Error = new AjaxResultCode();
            string retv = "";
            try
            {
                Dictionary<string, string> Refroot = XformUtil.StringToDictionary(refroot);
                Xform temp = new Xform(root, Refroot, selector);
                temp.InitDefaults();
                temp.DefRender.SimpleIdMode = true;
                retv = temp.DefRender.Render(formid, template,Preflang);
            }
            catch (Exception e)
            {
                Error.Errorcode = 10;
                Error.Errormessage = e.Message;
            }
            return Json(new { Xform = retv, Error = Error }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult NewXform(string root, string refroot, string formid, string template = null, string selector = "Gschema")
        {
            AjaxResultCode Error = new AjaxResultCode();
            string retv = "";
            try
            {
                Dictionary<string, string> Refroot = XformUtil.StringToDictionary(refroot);
                Xform temp = new Xform(root, Refroot, selector);
                temp.InitDefaults();
                retv = temp.DefRender.Render(formid, template, Preflang);
            }
            catch (Exception e)
            {
                Error.Errorcode = 10;
                Error.Errormessage = e.Message;
            }
            return Json(new { Xform = retv, Error = Error }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult NewXformPost(FormCollection forms)
        {
            AjaxResultCode Error = new AjaxResultCode();
            string retv = "";
            try
            {
                Dictionary<string, string> Refroot = XformUtil.StringToDictionary(forms["refroot"]);
                Xform temp = new Xform(forms["root"], Refroot, string.IsNullOrEmpty(forms["selector"]) ? "Gschema" : forms["selector"]);
                temp.InitDefaults();
                //string template = forms["template"];
                retv = temp.DefRender.Render(forms["formid"], forms["template"], Preflang);
            }
            catch (Exception e)
            {
                Error.Errorcode = 10;
                Error.Errormessage = e.Message;
            }
            return Json(new { Xform = retv, Error = Error }, "text/html");
        }
        public JsonResult Xform_RenderPart(string part)
        {
            AjaxResultCode Error = new AjaxResultCode();
            string retv = "";
            string rules = "";
            try
            {
                string[] p = part.Trim('_').Split('_');
                string root = p[0];
                int ocidx = int.Parse(p[p.Length - 1]);
                Xform xform2 = new Xform(root);
                retv = xform2.DefRender.PartialRender(XformUtil.fromName(part), ocidx);
                rules = xform2.DefRender.Jqrulesjson.TrimEnd(',');
            }
            catch { Error.Errorcode = 11; Error.Errormessage = "Hiba"; }
            return Json(new { ret = retv, rules = rules, Error = Error }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Xform_ChangeRoot(FormCollection forms, string template = null, string selector = "Gschema")
        {
            string ret = "";
            AjaxResultCode Error = new AjaxResultCode();
            string formid = forms["formid"];
            try
            {
                Xform workxform = new Xform(Request.Params, selector);
                workxform.ChangeRoot(forms[DextraLib.XForm.Xconstans.HtmlXfromRootId], forms[DextraLib.XForm.Xconstans.HtmlXfromRefRootId], selector);
                ret = workxform.DefRender.Render(formid, template, Preflang);

            }
            catch (Exception e)
            {
                var t = e;
            }

            return Json(new { Xform = ret, Error = Error }, "text/html");

        }
        [HttpPost]
        public JsonResult Xform_ChangeRootPost(FormCollection forms)
        {
            string ret = "";
            AjaxResultCode Error = new AjaxResultCode();
            string formid = forms["formid"];
            string template = forms["template"];
            try
            {
                Xform workxform = new Xform();
                if (forms["XformRoot"] == null || forms["XformRoot"] == "Empty")
                {
                    Dictionary<string, string> Refroot = XformUtil.StringToDictionary(forms[Xconstans.HtmlXfromRefRootId]);
                    workxform = new Xform(forms[Xconstans.HtmlXfromRootId], Refroot, string.IsNullOrEmpty(forms["selector"]) ? "Gschema" : forms["selector"]);
                    workxform.InitDefaults();

                } else
                {
                    workxform = new Xform(Request.Params, string.IsNullOrEmpty(forms["selector"]) ? "Gschema" : forms["selector"]);
                    workxform.ChangeRoot(forms[Xconstans.HtmlXfromRootId], forms[Xconstans.HtmlXfromRefRootId], string.IsNullOrEmpty(forms["selector"]) ? "Gschema" : forms["selector"]);
                }
                ret = workxform.DefRender.Render(formid, template,Preflang);
            }
            catch
            {

            }
            return Json(new { Xform = ret, Error = Error }, "text/html");

        }

        [Obsolete] // nem hasznalom
        public JsonResult BuildRootSelectorWithComplexType(string filter, string selector = "Gschema")
        {
            AjaxResultCode Error = new AjaxResultCode();
            List<SchemaRoots> result = XformUtil.BuildRootSelectorWithComplexType(MvcApplication.XformSchemes[selector], filter);


            foreach (SchemaRoots item in result)
            {


                //item.value = Langue.Translate(item.value, "XSENUM", Preflang);//.Split('.')[0].ToLower());
                item.Label = Langue.Translate(item.Label, "XSENUM", Preflang);//.Split('.')[0].ToLower());
            }


            return Json(new { Res = result, Error = Error }, JsonRequestBehavior.AllowGet);
        }

        [Obsolete]  // nem hasznalom
        public JsonResult BuildRootSelectorWithAllElement(string filter, string selector = "Gschema")
        {
            AjaxResultCode Error = new AjaxResultCode();
            List<SchemaRoots> result = XformUtil.BuildRootSelectorWithAllElement(MvcApplication.XformSchemes[selector], filter);

            foreach (SchemaRoots item in result)
            {
                //item.value = Langue.Translate(item.value, "XSENUM", Preflang);//.Split('.')[0].ToLower());
                item.Label = Langue.Translate(item.Label, "XSENUM", Preflang);//.Split('.')[0].ToLower());
            }

            return Json(new { Res = result, Error = Error }, JsonRequestBehavior.AllowGet);
        }

        // -----------------------------------------------------------------------------------------------------

        // Menu vagy combo tovabbiak a dextrahelp.txt-ben

        public JsonResult BuildRootSelectorComboWithComplexType(string filter, string selector = "Gschema")
        {

            XSchema Appschemel = MvcApplication.Appschema;   //  .Appscheme;
            List<SchemaRoots> res = XformUtil.GetXformRootsFromScheme(MvcApplication.XformSchemes[selector],false, filter);
            //res = res.FindAll(r => r.parentid != 0);
            foreach (SchemaRoots item in res)
            {
                // item.value = Langue.Translate(item.value, "XSENUM", Preflang);//.Split('.')[0].ToLower());
                item.Label = Langue.Translate(item.Label, "XSENUM", Preflang);//.Split('.')[0].ToLower());
            }
            return Json(new { Res = res }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult BuildRootSelectorMenuWithComplexType(string filter, string selector = "Gschema")
        {

           XSchema Appschemel = MvcApplication.Appschema;   //  .Appscheme;
           List<SchemaRoots> res = XformUtil.GetXformRootsFromScheme(MvcApplication.XformSchemes[selector],true, filter);

            foreach (SchemaRoots item in res)
            {
               // item.value = Langue.Translate(item.value, "XSENUM", Preflang);//.Split('.')[0].ToLower());
                item.Label = Langue.Translate(item.Label, "XSENUM", Preflang);//.Split('.')[0].ToLower());
            }

            return Json(new { Res = res }, JsonRequestBehavior.AllowGet);
        }



        /// --------------------------  In Work
        /// 

        public ActionResult GetXml(string filename)
        {
            byte[] content = null;
            MvcApplication.GeneralLog.Info("IconGetXml:" + Server.MapPath("~/App_Data/Files/Xml/Codes/" + filename));
            //return File(content, "");
            return File(Server.MapPath("~/App_Data/Files/Xml/Codes/" + filename), "application/xml");
        }

        public JsonResult GetDictionary(string group)
        {
            AjaxResultCode Error = new AjaxResultCode();
            string res = "";
            return Json(new { Rows = res, Error = Error }, JsonRequestBehavior.AllowGet);
        }

        public class ComboRow
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
        public JsonResult Xform_ComboSource(string xmlfilename, string xmldataroot, string filter, string cValue)
        {
            List<ComboRow> citems = new List<ComboRow>();
            AjaxResultCode Error = new AjaxResultCode();

            string option = "";
            string options = "";


            XDocument XMLDoc = new XDocument();
            try { XMLDoc = XDocument.Load(Tools.GetXformXmlComboPath(xmlfilename)); } catch { }

            if (XMLDoc.Root != null)
            {
                if (string.IsNullOrEmpty(filter))
                {
                    citems = (from r in XMLDoc.Root.Element(XName.Get(xmldataroot)).Elements(XName.Get("group"))
                              select new ComboRow
                              {
                                  Key = (string)r.Element("typecode"),
                                  Value = (string)r.Element("typename")
                              }).ToList();

                }
                else
                {
                    citems = (from r in XMLDoc.Root.Element(XName.Get(xmldataroot)).Elements(XName.Get("group"))
                              where ((string)r.Attribute("Filter") == filter || !r.HasAttributes)
                              select new ComboRow
                              {
                                  Key = (string)r.Element("typecode"),
                                  Value = (string)r.Element("typename")
                              }).ToList();

                }
            }

            foreach (ComboRow i in citems)
            {
                option = "<option value=\"" + i.Key + "\" selected= >" + i.Value + "</option>";
                if (i.Key == cValue) { option = option.Replace("selected=", "selected=\"selected\""); } else { option = option.Replace("selected=", " "); }
                options += option;
            }

            return Json(new { ret = options, Error = Error }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult prcGenerator(FormCollection form)
        {
            string path = "Work/";
            string Entitydesc = form["txtEntityDesc"];
            List<CustEntity> ent = Generator.Genratorprc(Entitydesc, path);

            /*

            foreach (CustEntity item in ent)
            {
                path = "Work/" + item.Name + "/";
                try  { Directory.CreateDirectory(MvcApplication.GetPhysicalPath(path)); } catch (Exception ex)  {  }

                System.IO.File.WriteAllText(MvcApplication.GetPhysicalPath(path) + item.Name+ "_Class.cs", item.RenderedforClass);
                System.IO.File.WriteAllText(MvcApplication.GetPhysicalPath(path) + item.Name + "_Javafortree.js", item.RenderedJavascript);
                System.IO.File.WriteAllText(MvcApplication.GetPhysicalPath(path) + item.Name + "_AjaxController.cs", item.RenderedAjaxf);
                System.IO.File.WriteAllText(MvcApplication.GetPhysicalPath(path) + item.Name + "_Sql.sql", item.RenderedforSql);
                System.IO.File.WriteAllText(MvcApplication.GetPhysicalPath(path) + item.Name + "_Html.html", item.RenderedHtml);
            }
            */
            return Json(new { Out = "", Error = Error }, "text/html");
        }

        // ----- New Image Upload
        public decimal? UploadPicture(HttpRequestBase request)
        {
            decimal? Id_Ufiles = null;
            Ufiles tu = new Ufiles();
            if (request.Files.Count > 0)
            {
                var postedFile = request.Files[0];
                var image = new WebImage(postedFile.InputStream)
                {
                    FileName = request.Params["origFileName"]
                };

                if (image != null)
                {
                    string mimeType = request.Files[0].ContentType;
                    byte[] fileData = image.GetBytes();
                    int fileLength = fileData.Length;
                    string Titles = "no";
                    try {Titles = request.Form["Titles"];} catch { }
                    string Picturetype = "";
                    try { Picturetype = request.Form["Picturetype"]; } catch { }
                    tu = new Ufiles(Id_Ufiles, Picturetype, Picturetype, mimeType, fileLength, fileData, Titles);
                    tu.InitCommonFieldsForAdd(Sdb);
                    Dao<Ufiles> udao = new Dao<Ufiles>(Sdb);
                    udao.SqlInsert(tu);
                    Id_Ufiles = udao.Result.Lastid;
                }
            }
            return Id_Ufiles;
        }
        [HttpPost] ActionResult UploadImg()
        {
            decimal? Id_Ufiles = UploadPicture(Request);
            return Json(new { isUploaded = true, message = "File uploaded successfully!" });
        }

        [HttpPost]
        public ActionResult UploadPersonPic()
        {
            decimal? Id_Persons = null;
            try { Id_Persons =decimal.Parse(Request.Form["Id_Persons"]); } catch { }

            Dao w = new Dao(Sdb);
            Dao<Mxrf_Persons_Ufiles> mxdao = new Dao<Mxrf_Persons_Ufiles>(Sdb);
            mxdao.SqlSelect("select U.ID_UFILES,X.ID_MXRF_PERSONS_UFILES from MXRF_PERSONS_UFILES x,ufiles u where U.ID_UFILES=X.ID_UFILES and  x.ID_PERSONS =:0 and U.FILETYPE='FACEPIC'", new object[] { Id_Persons });
            if(mxdao.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                foreach(Mxrf_Persons_Ufiles r in mxdao.Result.GetRes<Mxrf_Persons_Ufiles>())
                {
                    w.ExecuteNonQuery("delete from MXRF_PERSONS_UFILES where ID_MXRF_PERSONS_UFILES=:0 ", new object[] { r.Id_Mxrf_Persons_Ufiles });
                    w.ExecuteNonQuery("delete from ufiles where ID_UFILES=:0 ", new object[] { r.Id_Ufiles });
                }
            }

            decimal? Id_Ufiles = UploadPicture(Request);
            mxdao.SqlInsert(new Mxrf_Persons_Ufiles((decimal)Id_Persons,(decimal) Id_Ufiles));
            Sdb.Commit();
            return Json(new { isUploaded = true, message = "File uploaded successfully!" });
        }

        [HttpPost]
        public ActionResult UploadAnimalPic()
        {
            decimal? Id_Animal = null;
            try { Id_Animal = decimal.Parse(Request.Form["Id_Animal"]); } catch { }

            Dao w = new Dao(Sdb);
            Dao<Mxrf_Animal_Ufiles> mxdao = new Dao<Mxrf_Animal_Ufiles>(Sdb);
            mxdao.SqlSelect("select U.ID_UFILES,X.ID_MXRF_ANIMAL_UFILES from MXRF_ANIMAL_UFILES x,ufiles u where U.ID_UFILES=X.ID_UFILES and  x.ID_ANIMAL =:0 and U.FILETYPE='FACEPIC'", new object[] { Id_Animal });
            if (mxdao.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                foreach (Mxrf_Animal_Ufiles r in mxdao.Result.GetRes<Mxrf_Animal_Ufiles>())
                {
                    w.ExecuteNonQuery("delete from MXRF_ANIMAL_UFILES where ID_MXRF_ANIMAL_UFILES=:0 ", new object[] { r.Id_Mxrf_Animal_Ufiles });
                    w.ExecuteNonQuery("delete from ufiles where ID_UFILES=:0 ", new object[] { r.Id_Ufiles });
                }
            }

            decimal? Id_Ufiles = UploadPicture(Request);
            mxdao.SqlInsert(new Mxrf_Animal_Ufiles((decimal)Id_Animal, (decimal)Id_Ufiles));
            Sdb.Commit();
            return Json(new { isUploaded = true, message = "File uploaded successfully!" });
        }

        public ActionResult GetAnimalPic(decimal? id, string type = "FACEPIC")
        {

            Dao<Ufiles> uf = new Dao<Ufiles>(Sdb);
            uf.SqlSelect(@"select T.Id_Ufiles,T.Datatype,T.Filetype,T.Mimetype,T.Filesize,T.Bdata,T.Cdata from ufiles T, MXRF_Animal_UFILES x where T.ID_UFILES=X.ID_UFILES and T.FILETYPE=:1 and X.ID_Animal=:0  and x.datavalidto is null", new object[] { id, type });
            if (uf.Result.GetSate(2))
            {
                Ufiles u = uf.Result.GetFirst<Ufiles>();
                if (u != null)
                {
                    if (u.Mimetype == "encodedjpg")
                    {
                        byte[] Bdata = null;
                        string Mimetype = "image/jpg";

                        string arckep = "";
                        arckep = u.Cdata;
                        int NumberChars = arckep.Length;
                        Bdata = new byte[NumberChars / 2];
                        for (int i = 0; i < NumberChars; i += 2)
                            Bdata[i / 2] = Convert.ToByte(arckep.Substring(i, 2), 16);

                        return File(Bdata, Mimetype);


                    }
                    else
                    {
                        return File(u.Bdata, u.Mimetype);
                    }

                }
                return File(Server.MapPath("~/Content/images/empty.gif"), "image/jpg");
            }


            return File(Server.MapPath("~/Content/images/empty.gif"), "image/jpg");
        }


        public ActionResult GetPersonPicBid(string bid, string type = "FACEPIC")
        {
            Dao<Persons> pd = new Dao<Persons>(Sdb);
            decimal? id = 0;
            pd.SqlSelect("select * from Persons T where t.Bid_Persons=:0 ", new object[] { bid });
            if (pd.Result.GetSate(DaoResult.ResCountOne))
            {
                id = pd.Result.GetFirst<Persons>().Id_Persons;
            }
            return GetPersonPic(id, "FACEPIC");
        }

        public ActionResult GetPersonPic(decimal? id, string type = "FACEPIC")
        {

            Dao<Ufiles> uf = new Dao<Ufiles>(Sdb);
            uf.SqlSelect(@"select T.Id_Ufiles,T.Datatype,T.Filetype,T.Mimetype,T.Filesize,T.Bdata,T.Cdata from ufiles T, MXRF_PERSONS_UFILES x where T.ID_UFILES=X.ID_UFILES and T.FILETYPE=:1 and X.ID_PERSONS=:0  and x.datavalidto is null", new object[] { id, type });
            if (uf.Result.GetSate(2))
            {
                Ufiles u = uf.Result.GetFirst<Ufiles>();
                if (u != null)
                {
                    if (u.Mimetype == "encodedjpg")
                    {
                        byte[] Bdata = null;
                        string Mimetype = "image/jpg";

                        string arckep = "";
                        arckep = u.Cdata;
                        int NumberChars = arckep.Length;
                        Bdata = new byte[NumberChars / 2];
                        for (int i = 0; i < NumberChars; i += 2)
                            Bdata[i / 2] = Convert.ToByte(arckep.Substring(i, 2), 16);

                        return File(Bdata, Mimetype);


                    }
                    else
                    {
                        return File(u.Bdata, u.Mimetype);
                    }

                }
                return File(Server.MapPath("~/Content/images/empty.gif"), "image/jpg");
            }


            return File(Server.MapPath("~/Content/images/empty.gif"), "image/jpg");
        }

    }
}