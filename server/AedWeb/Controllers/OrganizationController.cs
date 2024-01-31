using Dextra.Common;
using Dextra.Database;
using Dextra.Flowbase;
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
using System.Web.Mvc;
using System.Web.Routing;
using Xapp;
using Xapp.Controllers;
using Xapp.Db;
using Xapp.FlowDatas;
using static Xapp.Controllers.AjaxController;

namespace Xapp.Controllers
{
    [dxAuthorize]
  //  [CheckSessionTimeOutAttribute]
    public class OrganizationController : BaseController
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

        // GET: Organization
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Organization()
        {
            Model.FlowModel.BackUrl = Request.UrlReferrer.OriginalString;
            return View(Model);
        }

        #region Organization


        [HttpPost]
        [dxAuthorize(C.c_role_OrgCreate)]
        public JsonResult InsertXform_Organization(FormCollection forms)
        {
            XformProcess<Organization> xp = new XformProcess<Organization>(Sdb, Request.Params);

            xp.Entity.Recordtype = forms["Recordtype"];// "OrgItem";

            xp.Entity.Id_Organization = null;
            xp.Entity.Id_Parent = (decimal)xp.Id_Parent;
            xp.Entity.Bid_Organization = "NEW";
            xp.Entity.Status = "Active";
            xp.Insert();

            return Json(new { Entity = xp.Entity, Error = xp.Error }, "text/html");
        }

        [HttpPost]
        [dxAuthorize(C.c_role_OrgWrite)]
        public JsonResult SaveXform_Organization(FormCollection forms)
        {
            XformProcess<Organization> xp = new XformProcess<Organization>(Sdb, Request.Params);
        
            xp.Entity.Id_Organization = (decimal)xp.Id_Entity;
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
        
        [dxAuthorize(C.c_role_OrgDelete)]
        public JsonResult DeleteXform_Organization(decimal Id_Entity, string cmd = null)
        {
            XformProcess<Organization> xp = new XformProcess<Organization>(Sdb);
            if (cmd != null && cmd == "close") // onlyclose
            {
                xp.Close(Id_Entity);
            }
            else
            {
                xp.Del(Id_Entity);
            }
            return Json(new { Error = xp.Error }, JsonRequestBehavior.AllowGet);
        }

        [dxAuthorize(C.c_role_OrgRead)]
        public JsonResult GetXform_Organization(decimal Id_Entity, string Id_Xform = "formid")
        {
            XformProcess<Organization> xp = new XformProcess<Organization>(Sdb);
            string[] htt = xp.Get(Id_Entity, Id_Xform);
            return Json(new { Xform = htt[0], XformView = htt[1],Root=xp.xform.RootName, Error = xp.Error }, JsonRequestBehavior.AllowGet);
        }


        [dxAuthorize(C.c_role_OrgRead)]
        public ActionResult GetRecords_Organization(decimal Id, string recordtype)
        {
            List<Organizationclient> res = new List<Organizationclient>();
            List<Organizationclient> ressingle = new List<Organizationclient>();
            Dao<Organizationclient> t = new Dao<Organizationclient>(Sdb);
            t.SqlSelect("select X.ID_PERSON,P.USEDNAME,o.* from organization o ,Mxrf_Persons_Org x, persons p where O.ID_ORGANIZATION=X.ID_ORGANIZATION(+) and P.ID_PERSONS(+)=X.ID_PERSON and o.Id_Parent=:0 and o.recordtype=:1", new object[] { Id, recordtype });
            if (t.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                res = t.Result.GetRes<Organizationclient>();
            }

            var xxx=res.Select(x => x.Id_Organization).Distinct();
            foreach(decimal x in xxx)
            {
                //Organizationclient to = res.Find(z => z.Id_Organization == x);
                List<Organizationclient> tol = res.FindAll(d => d.Id_Organization == x);
                Organizationclient to = tol[0];
                if (tol.Count > 1) to.Usedname += "(*)";
                ressingle.Add(to);
            }

            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd.", Formatting = Newtonsoft.Json.Formatting.Indented };
            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(ressingle, settings),
                ContentEncoding = Encoding.UTF8
            };
        }

        [dxAuthorize(C.c_role_OrgRead)]
        public ActionResult GetHierarchy_Organization(decimal Id, decimal level, string recordtype,string selecdate="")
        {

            List<Organizationclient> ret = new List<Organizationclient>();
            Dao<Organizationclient> t = new Dao<Organizationclient>(Sdb);
            if (!string.IsNullOrEmpty(selecdate))
            {
                try
                {
                    Sdb.GeneralVariables["dtfrom"] = DateTime.ParseExact(selecdate, "yyyy.MM.dd.", CultureInfo.InvariantCulture);
                } catch { }
            }
            

            if (Id == 0) t.SqlSelect("SELECT /*A+.OwnJoj*/ T.* FROM ORGANIZATION T LEFT JOIN ORGANIZATION O ON T.ID_PARENT = O.ID_ORGANIZATION WHERE O.ID_ORGANIZATION IS NULL AND T.RECORDTYPE=:0  ORDER BY T.ID_ORD ", new object[] { recordtype }); //and t.status='Active'
            else t.SqlSelect("select /*A+*/ T.* from Organization T where T.Id_Organization=:0 and T.recordtype=:1  ", new object[] { Id,recordtype }); //and t.status='Active'
            if (t.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
               // ret.AddRange(t.Result.GetRes<Organizationclient>());

                foreach (Organizationclient item in t.Result.GetRes<Organizationclient>())
                {
                    Dao<Organizationclient> t1 = new Dao<Organizationclient>(Sdb);
                    t1.SqlSelect(@"SELECT /*A+*/ T.*,extractvalue(xmltype(T.XMLDATA),'//@ComplexType') as Orgtype FROM ORGANIZATION T where T.RECORDTYPe=:2 and level<:0   CONNECT BY PRIOR ID_ORGANIZATION = ID_PARENT START WITH T.ID_ORGANIZATION=:1 ORDER SIBLINGS BY ID_ORD  ", new object[] { level, item.Id_Organization, recordtype });
                    if (t1.Result.GetSate(DaoResult.ResCountOneOrMore))
                    {
                        ret.AddRange(t1.Result.GetRes<Organizationclient>());
                    }
                }

            }
            if (ret.Count == 0)
            {
                if (t.Result.Count == 0)
                {
                    if(Id==0 && string.IsNullOrEmpty(selecdate))
                    {
                        Organization or = new Organization();
                        or.Id_Organization = null;
                        or.Bid_Organization = "NEW";
                        or.Id_Parent = null;
                        or.Name = "Root";
                        or.Recordtype = "OrgItem";
                        or.InitCommonFieldsForAdd(Sdb);
                        Dao<Organization> ida = new Dao<Organization>(Sdb);
                        ida.SqlInsert(or);

                        Organizationclient temp = new Organizationclient(or);
                        temp.Id_Organization = ida.Result.Lastid;


                        ret.Add(temp);
                    }

                } else
                {
                    ret.AddRange(t.Result.GetRes<Organizationclient>());
                }
            }
            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd.", Formatting = Newtonsoft.Json.Formatting.Indented };
            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(ret, settings),
                ContentEncoding = Encoding.UTF8
            };

        }

        [dxAuthorize(C.c_role_OrgWrite)]
        public JsonResult MoveHierarchyItem_Organization(int Id, int NewParentId, string dropPos)
        {

            Dao<Organization> dao = new Dao<Organization>(Sdb);
            // ez most ideiglenese
            //dao.MoveRecord(Id, NewParentId, dropPos, "Id_Parent", "reorder");

            // ez lesz jo
            decimal? reorderid = dao.MoveRecordNew(Id, NewParentId, dropPos, "Id_Parent");
            dao.ReorderOrg(reorderid, "OrgItem");

            //Reorder_Organization(Id);

            return Json(new { Entity = "", Error = Error }, JsonRequestBehavior.AllowGet);
        }
        public void Reorder_Organization(decimal Id)
        {
            List<Organization> work = new List<Organization>();
            Dao<Organization> rodao = new Dao<Organization>(Sdb);
            rodao.SqlSelect("select * from Organization where Id_Organization=:0", new object[] { Id });
            if (rodao.Result.GetSate(DaoResult.ResCountOneOrMore))
            {

                decimal Id_Parent = (decimal)rodao.Result.GetFirst<Organization>().Id_Parent;
                rodao.SqlSelect("select * from Organization where Id_Parent=:0 order by Id_Ord", new object[] { Id_Parent });
                if (rodao.Result.GetSate(DaoResult.ResCountOneOrMore))
                {
                    work = rodao.Result.GetRes<Organization>();
                }
            }
            decimal newordid = 0;
            foreach (Organization p in work)
            {
                p.Id_Ord = newordid++;
                rodao.SqlUpdate(p);
            }
        }
        // ------ Special functions
       
        class ChangedDates
        {
            public DateTime? Datavalidfrom { get; set; }
            public ChangedDates()
            {

            }
        }
        [dxAuthorize(C.c_role_OrgRead)]
        public ActionResult GetValidDates()
        {
            List<ChangedDates> res = new List<ChangedDates>();
            string sql = @"select /*A+USGr*/ distinct(trunc(T.DATAVALIDFROM)) as DATAVALIDFROM from organization T ";

            Dao<ChangedDates> opdao = new Dao<ChangedDates>(Sdb);
            opdao.SqlSelect(sql, new object[] { });
            if (opdao.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                res = opdao.Result.GetRes<ChangedDates>();
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
        public ActionResult GetOrphanPersons()
        {
            List<OrphanPersons> res = new List<OrphanPersons>();
            string sql = @"
                    select X.ID_ORGANIZATION,P.ID_PERSONS,P.USEDNAME 
                    from Mxrf_Persons_Org x, persons p where 
                     P.ID_PERSONS(+)=X.ID_PERSON
                    and X.ID_ORGANIZATION in
                    (
                    select 
                    X.ID_ORGANIZATION
                    from Mxrf_Persons_Org x
                    minus
                    select 
                    O.ID_ORGANIZATION
                    from ORGANIZATION o where O.RECORDTYPE='StatusItem'
                    ) ";

            Dao<OrphanPersons> opdao = new Dao<OrphanPersons>(Sdb);
            opdao.SqlSelect(sql, new object[] { });
            if (opdao.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                res = opdao.Result.GetRes<OrphanPersons>();
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
        public ActionResult GetRecords_Orphanstatus()
        {
            List<Organizationclient> res = new List<Organizationclient>();
            string sql = @"
                    select 
                    *
                    from ORGANIZATION o where O.ID_PARENT in
                    (
                    select
                    T.ID_PARENT as id
                    FROM ORGANIZATION T where T.RECORDTYPe='StatusItem' 
                    minus
                    select
                    T.ID_ORGANIZATION as id
                    FROM ORGANIZATION T where T.RECORDTYPe='OrgItem'
                    ) ";
            Dao<Organizationclient> t = new Dao<Organizationclient>(Sdb);
            t.SqlSelect(sql, new object[] { });
            if (t.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                res = t.Result.GetRes<Organizationclient>();
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
        public JsonResult GetXform_AttachedPersons(decimal Id, string Id_Xform = "formid")
        {

            List<string> retval = new List<string>();

            Dao<Persons> t = new Dao<Persons>(Sdb);
            t.SqlSelect("SELECT T.*  FROM   Mxrf_Persons_Org x, persons T WHERE  T.ID_PERSONS(+) = X.ID_PERSON   and X.ID_ORGANIZATION=:0 ", new object[] { Id }); // 44 test
            if (t.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                foreach(Persons item in t.Result.GetRes<Persons>())
                {
                    Xform xform = new Xform(item.Xmldata,null);
                    XformGenralDocument1 ds = new XformGenralDocument1(xform.ElementAppinfoDocTemplate, xform);
                    ds.Render();
                    retval.Add(ds.Rendered.Replace("GetPersonPic/", "GetPersonPic/" +  item.Id_Persons.ToString() ));
                }
            }
            return Json(new { Forms = retval, Error = Error }, JsonRequestBehavior.AllowGet);
        }

        [dxAuthorize(C.c_role_OrgWrite)]
        public ActionResult MultipleStatusrecord( decimal Id,decimal Copynum)
        {
            Organization status = new Organization();
            Dao<Organization> t = new Dao<Organization>(Sdb);
            t.SqlSelectId(Id);
            if (t.Result.GetSate(DaoResult.ResCountOne))
            {
                status = t.Result.GetFirst<Organization>();
                string name = status.Name;
                for (int i = 0; i < Copynum; i++)
                {
                    status.Id_Organization = null;
                    status.Bid_Organization = "NEW";
                    status.Name =name+ string.Format("-({0})", i);
                    status.InitCommonFieldsForAdd(Sdb);
                    t.SqlInsert(status);
                }

            }


            return Json(new {  Error = Error }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Status
        [HttpPost]
        [dxAuthorize(C.c_role_OrgCreate)]
        public JsonResult InsertXform_Orgstatus(FormCollection forms)
        {
            if (forms["Recordtype"] == "OrgItem") return Json(new { Entity = "", Error = Error }, "text/html");
            return InsertXform_Organization(forms);
        }

        [HttpPost]
        [dxAuthorize(C.c_role_OrgWrite)]
        public JsonResult SaveXform_Orgstatus(FormCollection forms)
        {
            return SaveXform_Organization(forms);
        }

        [dxAuthorize(C.c_role_OrgDelete)]
        public JsonResult DeleteXform_Orgstatus(decimal Id_Entity, string cmd = null)
        {
            return DeleteXform_Organization(Id_Entity, cmd);
        }

        [dxAuthorize(C.c_role_OrgRead)]
        public JsonResult GetXform_Orgstatus(decimal Id_Entity, string Id_Xform = "formid")
        {
            return GetXform_Organization(Id_Entity, Id_Xform );
        }

        #endregion

        #region Statusrequirements
        [HttpPost]
        [dxAuthorize(C.c_role_RequirementsCreate)]
        public JsonResult InsertXform_Statusrequirements(FormCollection forms)
        {
            /// If id <==> Id_parent hierarchy make trigger Id_Ord
            XformProcess<Statusrequirements> xp = new XformProcess<Statusrequirements>(Sdb, Request.Params);
            xp.Entity.Recordtype = forms["Recordtype"];
            xp.Entity.Itemtype = xp.xform.RootName;
            xp.Entity.Id_Organization = (decimal)xp.Id_Parent;
            xp.Entity.Id_Statusrequirements = null;
            xp.Insert();
            return Json(new { Entity = xp.Entity, Error = xp.Error }, "text/html");
        }

        [HttpPost]
        [dxAuthorize(C.c_role_RequirementsWrite)]
        public JsonResult SaveXform_Statusrequirements(FormCollection forms)
        {
            XformProcess<Statusrequirements> xp = new XformProcess<Statusrequirements>(Sdb, Request.Params);
            xp.Entity.Recordtype = forms["Recordtype"];
            xp.Entity.Itemtype = xp.xform.RootName;
            xp.Entity.Id_Statusrequirements = (decimal)xp.Id_Entity;
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
        [dxAuthorize(C.c_role_RequirementsDelete)]
        public JsonResult DeleteXform_Statusrequirements(FormCollection forms)
        {
            XformProcess<Statusrequirements> xp = new XformProcess<Statusrequirements>(Sdb, Request.Params);
            xp.Del();
            return Json(new { Entity = xp.Entity, Error = xp.Error }, "text/html");
        }

        [dxAuthorize(C.c_role_RequirementsDelete)]
        public JsonResult DeleteXform_Statusrequirements(decimal Id_Statusrequirements, string cmd = null)
        {
            XformProcess<Statusrequirements> xp = new XformProcess<Statusrequirements>(Sdb);
            if (cmd != null && cmd == "close") // onlyclose
            {
                xp.Close(Id_Statusrequirements);
            }
            else
            {
                xp.Del(Id_Statusrequirements);
            }
            return Json(new { Error = xp.Error }, JsonRequestBehavior.AllowGet);
        }

        [dxAuthorize(C.c_role_RequirementsRead)]
        public JsonResult GetXform_Statusrequirements(decimal Id_Entity, string Id_Xform = "formid")
        {
            XformProcess<Statusrequirements> xp = new XformProcess<Statusrequirements>(Sdb);
            string[] htt = xp.Get(Id_Entity, Id_Xform);
            return Json(new { Entity = xp.Entity, Xform = htt[0], XformView = htt[1], Root = xp.xform.RootName, Error = xp.Error }, JsonRequestBehavior.AllowGet);
        }

        [dxAuthorize(C.c_role_RequirementsRead)]
        public ActionResult GetRecords_Statusrequirements(decimal Id, string recordtype)
        {
            List<Statusrequirements> res = new List<Statusrequirements>();
            Dao<Statusrequirements> t = new Dao<Statusrequirements>(Sdb);
            t.SqlSelect("select * from Statusrequirements where Id_Organization=:0 ", new object[] { Id });
            if (t.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                res = t.Result.GetRes<Statusrequirements>();
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
    }
}