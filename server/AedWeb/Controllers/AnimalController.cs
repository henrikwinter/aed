using Dextra.Common;
using Dextra.Database;
using Dextra.Report;
using Dextra.Toolsspace;
using Dextra.Xforms;
using DextraLib.GeneralDao;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Xapp.Db;
using Xapp.FlowDatas;

namespace Xapp.Controllers
{
    public class AnimalController : BaseController
    {

        [PersistField]
        public AnimalModel Model = null;

        AjaxResultCode Error = new AjaxResultCode();
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (Model == null)
            {
                Model = new AnimalModel(Sdb, Rolemanager.AvailableRoles, Rolemanager.CurrentUser);
            }

        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Animal()
        {
            ViewBag.Message = "";
            return View();
        }

        
        public ActionResult Propertyes(int id_animal)
        {
            ViewBag.Message = "";
            //if (!c) BackUrl = Request.UrlReferrer.OriginalString;
            Model.Getflowstep(1);
            Model.ViewModel.Animal.ClientPart.io_SelectedAnimalId = id_animal;
            return View("Propertyes", Model);
        }

        #region Aimal
        [HttpPost]
        [dxAuthorize(C.c_role_AnimalCreate)]
        public JsonResult InsertXform_Animals(FormCollection forms)
        {
            XformProcess<Animal> xp = new XformProcess<Animal>(Sdb, Request.Params);

            //xp.Entity.Recordtype = forms["Recordtype"];  Sample!
            xp.Entity.Root = xp.xform.RootName;
            xp.Entity.Id_Animal = null;
           // xp.Entity.Id_Parent = (decimal)xp.Id_Parent;
            xp.Entity.Bid_Animal = "NEW";
            xp.Entity.Status = "Active";
            xp.Insert();
            return Json(new { Entity = xp.Entity, Error = xp.Error }, "text/html");
        }

        [HttpPost]
        [dxAuthorize(C.c_role_AnimalWrite)]
        public JsonResult SaveXform_Animals(FormCollection forms)
        {
            XformProcess<Animal> xp = new XformProcess<Animal>(Sdb, Request.Params);

            //xp.Entity.Itemtype = xp.xform.RootName; Sample!

            xp.Entity.Id_Animal = (decimal)xp.Id_Entity;
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
        [dxAuthorize(C.c_role_AnimalDelete)]
        public JsonResult DeleteXform_Animals(FormCollection forms)
        {
            XformProcess<Animal> xp = new XformProcess<Animal>(Sdb, Request.Params);
            xp.Del();
            return Json(new { Entity = xp.Entity, Error = xp.Error }, "text/html");
        }


        public JsonResult DeleteXform_Animals(decimal Id_Entity, string cmd = null)
        {
            XformProcess<Animal> xp = new XformProcess<Animal>(Sdb);
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

        [dxAuthorize(C.c_role_AnimalRead)]
        public JsonResult GetXform_Animals(decimal Id_Entity, string Id_Xform = "formid")
        {
            XformProcess<Animal> xp = new XformProcess<Animal>(Sdb);
            //xp.Curlang = Preflang;
            string[] htt = xp.Get(Id_Entity, Id_Xform);
            return Json(new { Xform = htt[0], XformView = htt[1], Error = xp.Error }, JsonRequestBehavior.AllowGet);
        }

        [dxAuthorize(C.c_role_AnimalRead)]
        public ActionResult GetRecords_Animals(decimal Id, string recordtype)
        {
            List<Animal> res = new List<Animal>();
            Dao<Animal> t = new Dao<Animal>(Sdb);
            if (recordtype == "first")
            {
                t.SqlSelect("select /*A+*/ t.* from Animal t where rownum<11 ", new object[] { });
            }
            else
            {
                t.SqlSelect("select * from Animal where Id_Animal=:0 ", new object[] { Id });
            }

            if (t.Result.GetSate(DaoResult.ResCountOneOrMore))
            {

                Records_Animal.Clear();
                Records_Animal = t.Result.GetRes<Animal>();
                //res = t.Result.GetRes<Animal>();
            }

            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd.", Formatting = Newtonsoft.Json.Formatting.Indented };
            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(res, settings),
                ContentEncoding = Encoding.UTF8
            };
        }

        public class SearchAnimal
        {
            public decimal? Id_Animal { get; set; }
            public string Bid_Animal { get; set; }
            public decimal? Id_Parent { get; set; }
            public string Enar { get; set; }
            public DateTime Birthdate { get; set; }
            public string Birthdatestring { get; set; }
            public string Name { get; set; }

            public SearchAnimal()
            {
            }
            public SearchAnimal ShallowCopy()
            {
                return (SearchAnimal)this.MemberwiseClone();
            }
            public SearchAnimal(HttpRequestBase rRequest)
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
        List<Animal> Records_Animal = new List<Animal>();


        [HttpPost]
        public JsonResult SaveXform_SearchAnimals(FormCollection forms)
        {
            if (forms["selecdate"] != null)
            {
                try
                {
                    string selecdate = forms["selecdate"];
                    Sdb.GeneralVariables["dtfrom"] = DateTime.ParseExact(selecdate, "yyyy.MM.dd.", CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    var e = ex;
                }

            }
            else
            {
                Sdb.GeneralVariables["dtfrom"] = Sdb.Dbsysdate.Sysdbdate;
            }


            XformProcess<SearchAnimal> xp = new XformProcess<SearchAnimal>(Sdb, Request.Params);
            string sql = xp.xform.Elements[0].Appinfo["sql"];

            Dao<Animal> pdao = new Dao<Animal>(Sdb);
            object[] para = Tools.BuildparamfromClass(ref sql, xp.Entity);
            pdao.SqlSelect(sql, para);

            //pdao.SqlSelect("select * from Persons where upper(usedname) like 'V%' ",new object[] { });
            Records_Animal.Clear();
            if (pdao.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                try
                {
                    Records_Animal = pdao.Result.GetRes<Animal>();
                } catch (Exception ex)
                {
                    var e = ex;
                }
                
            }
            foreach(Animal a in Records_Animal)
            {
                a.Translate(Preflang);
            }

            return Json(new { sql = sql, Entity = xp.Entity, Error = xp.Error }, "text/html");
        }


        public ActionResult GetRecords_Animals_Session(string opt)
        {
            List<Animal> retv = new List<Animal>();
            if (Records_Animal.Count > 0) retv.AddRange(Records_Animal);
            //else retv.Add(new Persons());
            if (string.IsNullOrEmpty(opt))
            {
                Records_Animal.Clear();
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


        #region Propertyes

        public JsonResult InsertXform_Vw_Animalproperty(FormCollection forms)
        {
            string table = forms["table"];
            decimal? Id_Flows = null;
            decimal? ParentParentId = null;
            try { Id_Flows = decimal.Parse(forms["Id_Flows"]); } catch { }
            try { ParentParentId = decimal.Parse(forms["Id_ParentId_Parent"]); } catch { }
            switch (table)
            {
                case "Animalothersdata":
                    XformProcess<Animalothersdata> xo = new XformProcess<Animalothersdata>(Sdb, Request.Params);
                    xo.Entity.Id_Animalothersdata = null;
                    xo.Entity.Id_Flows = Id_Flows;
                    xo.Entity.Id_Animal = ParentParentId;
                    xo.Entity.Recordtype = xo.xform.RootName;
                    xo.Insert();
                    return Json(new { Entity = xo.Entity, Error = xo.Error }, "text/html");
                case "Animalhealtevents":
                    XformProcess<Animalhealtevents> xa = new XformProcess<Animalhealtevents>(Sdb, Request.Params);
                    xa.Entity.Id_Animalhealtevents = null;
                    xa.Entity.Id_Flows = Id_Flows;
                    xa.Entity.Id_Animal = ParentParentId;
                    xa.Entity.Recordtype = xa.xform.RootName;
                    xa.Insert();
                    return Json(new { Entity = xa.Entity, Error = xa.Error }, "text/html");
                default:
                    break;
            }
            return Json(new { Entity = "", Error = "Error" }, "text/html");
        }
        public JsonResult SaveXform_Vw_Animalproperty(FormCollection forms)
        {
            string table = forms["table"];
            switch (table)
            {

                case "Animalothersdata":
                    XformProcess<Animalothersdata> xo = new XformProcess<Animalothersdata>(Sdb, Request.Params);
                    xo.Entity.Id_Animalothersdata = (decimal)xo.Id_Entity;
                    if (forms["cmd"] != null && forms["cmd"] == "close") xo.CloseAndSave();
                    else xo.Save();
                    return Json(new { Entity = xo.Entity, Error = xo.Error }, "text/html");
                case "Animalhealtevents":
                    XformProcess<Animalhealtevents> xa = new XformProcess<Animalhealtevents>(Sdb, Request.Params);
                    xa.Entity.Id_Animalhealtevents = (decimal)xa.Id_Entity;
                    if (forms["cmd"] != null && forms["cmd"] == "close") xa.CloseAndSave();
                    else xa.Save();
                    return Json(new { Entity = xa.Entity, Error = xa.Error }, "text/html");
                default:
                    break;
            }
            return Json(new { Entity = "", Error = "Error" }, "text/html");
        }
        public JsonResult DeleteXform_Vw_Animalproperty(decimal Id_Entity, string table, string cmd = null)
        {
            //string table = "";
            switch (table)
            {

                case "Animalothersdata":
                    XformProcess<Animalothersdata> xo = new XformProcess<Animalothersdata>(Sdb);
                    if (cmd != null && cmd == "close") xo.Close(Id_Entity);
                    else xo.Del(Id_Entity);
                    return Json(new { Error = xo.Error }, JsonRequestBehavior.AllowGet);
                case "Animalhealtevents":
                    XformProcess<Animalhealtevents> xa = new XformProcess<Animalhealtevents>(Sdb);
                    if (cmd != null && cmd == "close") xa.Close(Id_Entity);
                    else xa.Del(Id_Entity);
                    return Json(new { Error = xa.Error }, JsonRequestBehavior.AllowGet);
                default:
                    break;
            }
            return Json(new { Error = "" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetXform_Vw_Animalproperty(decimal Id_Entity, string table, string Id_Xform = "formid")
        {
            // string table = "";
            string[] htt;
            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd.", Formatting = Newtonsoft.Json.Formatting.Indented };
            object ro = new { Entity = "", Xform = "", XformView = "", Error = "" };
            switch (table)
            {

                case "Animalothersdata":
                    XformProcess<Animalothersdata> xo = new XformProcess<Animalothersdata>(Sdb);
                    htt = xo.Get(Id_Entity, Id_Xform);
                    //ro = new { Entity = xo.Entity, Xform = htt[0], XformView = htt[1], Error = xo.Error };
                    return Json(new { Entity = xo.Entity, Xform = htt[0], XformView = htt[1], Error = xo.Error }, JsonRequestBehavior.AllowGet);
                    break;
                case "Animalhealtevents":
                    XformProcess<Animalhealtevents> xa = new XformProcess<Animalhealtevents>(Sdb);
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
        public ActionResult GetRecords_Vw_Animalproperty(decimal Id, string recordtype)
        {
            List<Vw_Animalproperty> res = new List<Vw_Animalproperty>();
            Dao<Vw_Animalproperty> t = new Dao<Vw_Animalproperty>(Sdb);
            t.SqlSelect("select * from Vw_Animalproperty where id_animal=:0 ", new object[] { Id });
            if (t.Result.GetSate(DaoResult.ResCountOneOrMore))
            {

                res = t.Result.GetRes<Vw_Animalproperty>();
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
        public JsonResult GetXform_AnimalCard(decimal Id, string Id_Xform = "formid")
        {

            List<string> retval = new List<string>();
            string animalname = "";
            Dao<Animal> t = new Dao<Animal>(Sdb);
            t.SqlSelect("SELECT T.*  FROM  Animal T WHERE  T.ID_Animal=:0 ", new object[] { Id }); // 44 test
            if (t.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                if (string.IsNullOrEmpty(Id_Xform))
                {
                    animalname = t.Result.GetFirst<Animal>().Name;
                }
                else
                {
                    foreach (Animal item in t.Result.GetRes<Animal>())
                    {
                        animalname = item.Name;
                        Xform xform = new Xform(item.Xmldata, null);
                        XformGenralDocument1 ds = new XformGenralDocument1(xform.ElementAppinfoDocTemplate, xform);
                        ds.Render();
                        retval.Add(ds.Rendered.Replace("GetAnimalPic/", "GetAnimalPic/" + item.Id_Animal.ToString()));
                    }

                }
            }
            return Json(new { Forms = retval, Animalname = animalname, Error = Error }, JsonRequestBehavior.AllowGet);
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
            work.SetElementValueById("Id_animal", Id.ToString());

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


    }


}