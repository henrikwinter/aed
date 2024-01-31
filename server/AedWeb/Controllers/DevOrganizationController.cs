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
using Xapp;
using Xapp.Db;
using Xapp.FlowDatas;

namespace Xapp.Controllers
{
    public class DevOrganizationController : BaseController
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
        public ActionResult Index()
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
            UpdateModel(Model.ViewModel.Org.ClientPart);

            if (Model.FlowModel._Check()) return Redirect(BackUrl);
            // -- business process

            Model.FlowModel.Complett = true;
            var test = Model.ViewModel.Org.ClientPart.io_SelectedOrgId.ToString();
            string desc = test;
            Sdb.StartTransaction();
            ActionResult ret = Model.Set(BackUrl, View("Select", Model), desc);

            List<DextraLib.Oracle.OraProcParam> par = new List<DextraLib.Oracle.OraProcParam>();
            ProcParam p1 = new ProcParam("CurentId_Flow", (decimal)Model.FlowModel.CurrentFlowstep.Id_Flow);
            par.Add(p1);
            ProcParam p2 = new ProcParam("Id_Organization_Root", Model.ViewModel.Org.ClientPart.io_SelectedOrgId, "IO");
            par.Add(p2);
            ProcParam p3 = new ProcParam("Userid", Model.FlowModel.CurrentUser);
            par.Add(p3);
            Dao<Organization> p = new Dao<Organization>(Sdb);
            p.SqlProcedure("CpyorgNew", ref par, Sdb);

            Model.FlowModel.Complett = true;
            if (p.Result.Error)
            {
                Model.Getflowstep("DelopeOrganization", "Select");
                Model.FlowModel.CurrentFlowstep.Error = true; Model.FlowModel.CurrentFlowstep.ErrorMessage = p.Result.Message;
                Sdb.RollBack();
                return View("Select", Model);
                //MvcApplication.Signal.ServerError(SignalRId, p.Result.Message, "Test");
            }
            Sdb.Commit();
            //
            return ret;
        }



        public ActionResult Develope(decimal Id_Flow, bool c = false)
        {
            if (!c) BackUrl = Request.UrlReferrer.OriginalString;
            Model.Getflowstep(Id_Flow);
            return View("Develope", Model);
        }

        [HttpPost]
        public ActionResult Develope(FormCollection forms)
        {
            UpdateModel(Model.FlowModel.PostedFlowData);

            if (Model.FlowModel._Check()) return Redirect(BackUrl);
            // -- business process

            Model.FlowModel.Complett = true;
            //
            return Model.Set(BackUrl, View("Develope", Model));
        }

        public ActionResult Discard(decimal Id_Flow, bool c = false)
        {
            if (!c) BackUrl = Request.UrlReferrer.OriginalString;
            Model.Getflowstep(Id_Flow);
            return View("Discard", Model);
        }

        [HttpPost]
        public ActionResult Discard(FormCollection forms)
        {
            UpdateModel(Model.FlowModel.PostedFlowData);

            if (Model.FlowModel._Check()) return Redirect(BackUrl);
            // -- business process

            List<DextraLib.Oracle.OraProcParam> par = new List<DextraLib.Oracle.OraProcParam>();
            ProcParam p1 = new ProcParam("CurentId_Flow", (decimal)Model.FlowModel.CurrentFlowstep.Id_Flow);
            Dao<Organization> p = new Dao<Organization>(Sdb);
            par.Add(p1);
            p.SqlProcedure("RollbackCpyOrg", ref par, Sdb);

            Model.FlowModel.Complett = true;
            if (p.Result.Error)
            {
                Model.FlowModel.CurrentFlowstep.Error = true; Model.FlowModel.CurrentFlowstep.ErrorMessage = p.Result.Message;
                return View("Discard", Model);
                //MvcApplication.Signal.ServerError(SignalRId, p.Result.Message, "Test");
            }
            //

            return Model.Set(BackUrl, View("Discard", Model));
        }

        public ActionResult Check(decimal Id_Flow, bool c = false)
        {
            if (!c) BackUrl = Request.UrlReferrer.OriginalString;
            Model.Getflowstep(Id_Flow);
            return View("Check", Model);
        }

        [HttpPost]
        public ActionResult Check(FormCollection forms)
        {
            UpdateModel(Model.FlowModel.PostedFlowData);
            try
            {
                UpdateModel(Model.ViewModel.Org.ClientPart);
            } catch (Exception e){
                var x = e;
            }
            

            if (Model.FlowModel._Check()) return Redirect(BackUrl);
            // -- business process

            Model.FlowModel.Complett = true;
            //

            if(Model.FlowModel.PostedFlowData.ToStepname== "Activate")
            {
                if(Model.ViewModel.Org.ClientPart.Activate_Date< Sdb.Dbsysdate.Sysdbdate)
                {
                    Model.FlowModel.CurrentFlowstep.Error = true; Model.FlowModel.CurrentFlowstep.ErrorMessage ="Activate date wrong!";
                    return View("Check", Model);
                }
                List<DextraLib.Oracle.OraProcParam> par = new List<DextraLib.Oracle.OraProcParam>();
                ProcParam p1 = new ProcParam("CurentId_Flow", (decimal)Model.FlowModel.CurrentFlowstep.Id_Flow);
                par.Add(p1);
                ProcParam p2 = new ProcParam("Activate_date", Model.ViewModel.Org.ClientPart.Activate_Date);
                par.Add(p2);
                Dao<Organization> p = new Dao<Organization>(Sdb);
                p.SqlProcedure("ActivateOrgNew", ref par, Sdb);
            }

            return Model.Set(BackUrl, View("Check", Model));
        }

        // ---
        public ActionResult Activate(decimal Id_Flow, bool c = false)
        {
            if (!c) BackUrl = Request.UrlReferrer.OriginalString;
            Model.Getflowstep(Id_Flow);
            return View("Activate", Model);
        }

        [HttpPost]
        public ActionResult Activate(FormCollection forms)
        {
            UpdateModel(Model.FlowModel.PostedFlowData);

            if (Model.FlowModel._Check()) return Redirect(BackUrl);
            // -- business process

            Model.FlowModel.Complett = true;
            //
            return Model.Set(BackUrl, View("Activate", Model));
        }





        [dxAuthorize(C.c_role_OrganizationRead)]
        public JsonResult Get_InProgressDevOrganization()
        {
            List<Flow> ret = new List<Flow>();
            Dao<Flow> fd = new Dao<Flow>(Sdb);
            fd.SqlSelect(@"select f.* from flows f where f.id_flow in ( select /*A+*/ distinct(o.id_flows) as idflows from organization o where o.status='Developed' )", new object[] { });
            if (fd.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                ret = fd.Result.GetRes<Flow>();
            }

            return Json(new { ret=ret, Error = Error }, JsonRequestBehavior.AllowGet);
        }


        #region Checkprocess
        public class DevCheckelement
        {
            public decimal? Id_Persons { get; set; }
            public decimal Id_Organization { get; set; }
            public decimal? Id_Mxrf_Persons_Org { get; set; }
            public string Usedname { get; set; }
            public string Attributum { get; set; }
            public string Assignment { get; set; }
            public string Checkstate { get; set; }
            public string Neworglongname { get; set; }
            public string Origorglongname { get; set; }
            public string Shortname { get; set; }
        }
        public ActionResult GetCheckList(decimal Id_Flow,decimal mode=0)
        {
            List<DevCheckelement> res = new List<DevCheckelement>();
            string sql = @"select 
                O.ID_ORGANIZATION,X.ID_PERSON as id_persons, nvl(O.ATTRIBUTUM,'-na-') as ATTRIBUTUM,Devorg_Chek(O.ID_ORGANIZATION,o.ID_FLOWS) as checkstate,longorgname(O.ID_ORGANIZATION ) as NewOrglongname,longorgname(O.CPYID ) as OrigOrglongname,O.SHORTNAME,
                X.ID_MXRF_PERSONS_ORG, o.Assignment, PersonName(X.ID_PERSON ) as usedname 
                from organization o,MXRF_PERSONS_ORG x
                where O.ID_FLOWS=:0
                and O.CPYID=X.ID_ORGANIZATION  and ( Devorg_Chek(O.ID_ORGANIZATION,o.ID_FLOWS)<>'Original' or O.ATTRIBUTUM is not null ) ";

            string sqlend = @"
                SELECT    PersonName(X.ID_PERSON ) as usedname ,longorgname(O.ID_ORGANIZATION ) as OrigOrglongname
                FROM   MXRF_PERSONS_ORG x ,PERSONS p,ORGANIZATION o
                WHERE   X.ID_FLOWS = :0 AND X.ID_ORGANIZATION IS NULL
                and X.ID_PERSON=P.ID_PERSONS and X.CPYID=O.ID_ORGANIZATION ";


            Dao<DevCheckelement> dao = new Dao<DevCheckelement>(Sdb);
            if(mode==0) dao.SqlSelect(sql, new object[] { Id_Flow });
            else dao.SqlSelect(sqlend, new object[] { Id_Flow });
            if (dao.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                res = dao.Result.GetRes<DevCheckelement>();
            }

            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd.", Formatting = Newtonsoft.Json.Formatting.Indented };
            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(res, settings),
                ContentEncoding = Encoding.UTF8
            };
        }

        public JsonResult UpdateCheckAction(decimal Id_Status,string value)
        {
            Dao dao = new Dao(Sdb);
            dao.SqlUpdate("update organization t set t.Assignment=:0 where t.Id_Organization=:1 ", new object[] {  value, Id_Status });

            return Json(new { Error = Error }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Spec
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

        [dxAuthorize(C.c_role_OrgWrite)]
        public ActionResult MultipleStatusrecord(decimal Id, decimal Copynum)
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
                    status.Name = name + string.Format("-({0})", i);
                    status.InitCommonFieldsForAdd(Sdb);
                    t.SqlInsert(status);
                }

            }


            return Json(new { Error = Error }, JsonRequestBehavior.AllowGet);
        }

        [dxAuthorize(C.c_role_OrgRead)]
        public JsonResult GetXform_AttachedPersons(decimal Id, string Id_Xform = "formid")
        {

            List<string> retval = new List<string>();

            Dao<Persons> t = new Dao<Persons>(Sdb);
            //t.SqlSelect("SELECT T.*  FROM   Mxrf_Persons_Org x, persons T WHERE  T.ID_PERSONS(+) = X.ID_PERSON   and X.ID_ORGANIZATION=:0 ", new object[] { Id }); // 44 test
            t.SqlSelect("select T.* from organization o,organization o1,  Mxrf_Persons_Org x, persons T where o.ID_ORGANIZATION =O1.CPYID and O1.ID_ORGANIZATION=:0 and O.ID_ORGANIZATION=X.ID_ORGANIZATION and T.ID_PERSONS(+) = X.ID_PERSON ", new object[] { Id });
            if (t.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                foreach (Persons item in t.Result.GetRes<Persons>())
                {
                    Xform xform = new Xform(item.Xmldata, null);
                    XformGenralDocument1 ds = new XformGenralDocument1(xform.ElementAppinfoDocTemplate, xform);
                    ds.Render();
                    //retval.Add(ds.RenderdedReport.Replace("GetPersonPic/", "GetPersonPic/" + item.Id_Persons.ToString()));
                    retval.Add(ds.Rendered.Replace("GetPersonPic/", "GetPersonPic/" + item.Id_Persons.ToString()));
                }
            }
            return Json(new { Forms = retval, Error = Error }, JsonRequestBehavior.AllowGet);
        }

        [dxAuthorize(C.c_role_OrganizationRead)]
        public ActionResult GetRecords_Orphanstatus(decimal Id)
        {
            List<Organizationclient> ret = new List<Organizationclient>();
            string sql = @"SELECT * FROM   organization l
                           WHERE L.ID_FLOWS = :0 AND L.RECORDTYPE = 'StatusItem' AND L.ID_PARENT NOT IN
                                          (SELECT ID_ORGANIZATION
                                                 FROM   (SELECT   *
                                                           FROM   organization T
                                                          WHERE   1 = 1 AND T.ID_FLOWS = :0
                                                                  AND NVL (t.attributum, 'not') NOT IN
                                                                           ('Deleted'))
                                           CONNECT BY   PRIOR ID_ORGANIZATION = ID_PARENT
                                           START WITH   ID_ORGANIZATION in
                                                (
                                                    select ID_ORGANIZATION from organization t where T.ID_FLOWS=:0 and T.ID_PARENT is null                         
                                                ))";

            Dao<Organizationclient> orpd = new Dao<Organizationclient>(Sdb);
            orpd.SqlSelect(sql, new object[] { Id });
            if (orpd.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                ret = orpd.Result.GetRes<Organizationclient>();
            }

            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd.", Formatting = Newtonsoft.Json.Formatting.Indented };
            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(ret, settings),
                ContentEncoding = Encoding.UTF8
            };

        }

        [dxAuthorize(C.c_role_OrganizationWrite)]
        public JsonResult SetOrphanStatusInTree(decimal Id_Status, string value)
        {
            Dao od = new Dao(Sdb);
            od.SqlUpdate("update organization t set t.property=:1,t.id_parent=null  where t.id_organization=:0 ", new object[] { Id_Status, value });

            return Json(new { Entity = "", Error = Error }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Organization
        // --- Organization

        /// <summary>
        /// Uj szervezet rekord tervezési statusszal és flow-ID vel. Parent ID a tervezési parent rekordhoz. 
        /// </summary>

        [HttpPost]
        [dxAuthorize(C.c_role_OrganizationCreate)]
        public JsonResult InsertXform_Organization(FormCollection forms)
        {
            XformProcess<Organization> xp = new XformProcess<Organization>(Sdb, Request.Params);

            bool inspos = !string.IsNullOrEmpty(forms["chkInsertPosition"]);
            decimal? ParentParentId = null;
            decimal? Id_Flows = null;
            try { ParentParentId = decimal.Parse(forms["Id_ParentId_Parent"]); } catch { inspos = false; }
            try { Id_Flows = decimal.Parse(forms["Id_Flows"]); } catch { }


            if (inspos && ParentParentId != 0) xp.Entity.Id_Parent = (decimal)ParentParentId;
            else xp.Entity.Id_Parent = (decimal)xp.Id_Parent;
            //// ------------------------- flow!!!
            xp.Entity.Id_Flows = Id_Flows;

            xp.Entity.InitCommonFieldsForAdd(Sdb);
            xp.Entity.Datavalidto = Sdb.Dbsysdate.Sysdbdate;
            xp.Entity.Recordtype = "OrgItem";
            xp.Entity.Status = "Developed";
            xp.Entity.Attributum = "New";
            xp.Entity.Bid_Organization = "NEW";
            xp.Entity.Id_Organization = null;
            //xp.Insert();
            xp.dao.SqlInsert(xp.Entity);
            return Json(new { Entity = xp.Entity, Error = xp.Error }, "text/html");
        }

        [HttpPost]
        [dxAuthorize(C.c_role_OrganizationWrite)]
        public JsonResult SaveXform_Organization(FormCollection forms)
        {
            XformProcess<Organization> xp = new XformProcess<Organization>(Sdb, Request.Params);
            xp.Entity.Attributum = "Modifyed";

            xp.Entity.Id_Organization = (decimal)xp.Id_Entity;
            xp.Save();

            return Json(new { Entity = xp.Entity, Error = xp.Error }, "text/html");
        }

        [dxAuthorize(C.c_role_OrganizationDelete)]
        public JsonResult DeleteXform_Organization(decimal Id_Organization, string cmd = null)
        {
            XformProcess<Organization> xp = new XformProcess<Organization>(Sdb);

            xp.dao.SqlSelectId(Id_Organization);
            if (xp.dao.Result.GetSate(DaoResult.ResCountOne))
            {
                xp.Entity=xp.dao.Result.GetFirst<Organization>();
                xp.Loaded = true;
                if (xp.Entity.Cpyid == null)
                {
                    xp.Del(Id_Organization);
                } else
                {
                    xp.Entity.Attributum = "Deleted";
                    xp.Save();
                }
            }

            return Json(new { Error = xp.Error }, JsonRequestBehavior.AllowGet);
        }

        [dxAuthorize(C.c_role_OrganizationRead)]
        public JsonResult GetXform_Organization(decimal Id_Entity, string Id_Xform = "formid")
        {
            XformProcess<Organization> xp = new XformProcess<Organization>(Sdb);
            string[] htt = xp.Get(Id_Entity, Id_Xform);
            return Json(new { Entity = xp.Entity, Xform = htt[0], XformView = htt[1], Error = xp.Error }, JsonRequestBehavior.AllowGet);
        }


        [dxAuthorize(C.c_role_OrganizationRead)]
        public ActionResult GetHierarchy_Organization(decimal Id, decimal level, string recordtype)
        {
            List<Organizationclient> ret = new List<Organizationclient>();
            Dao<Organizationclient> t = new Dao<Organizationclient>(Sdb);
            t.SqlSelect("select * from organization t where T.ID_FLOWS=:0 and T.ID_PARENT is null  and  T.RECORDTYPe=:1", new object[] { Id, recordtype });
            if (t.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                foreach (Organizationclient item in t.Result.GetRes<Organizationclient>())
                {
                    Dao<Organizationclient> t1 = new Dao<Organizationclient>(Sdb);
                    t1.SqlSelect(@"select * from ( 
                        SELECT  T.*,extractvalue(xmltype(T.XMLDATA),'//@ComplexType') as Orgtype FROM ORGANIZATION T 
                        where (T.RECORDTYPe=:2  or t.property='InTree') and  nvl(t.attributum,'not') not in('Deleted') 
                    ) where  level<:0
                    CONNECT BY PRIOR ID_ORGANIZATION = ID_PARENT START WITH ID_ORGANIZATION=:1 ORDER SIBLINGS BY ID_ORD  ", new object[] { level, item.Id_Organization, recordtype });
                    if (t1.Result.GetSate(DaoResult.ResCountOneOrMore))
                    {
                        ret.AddRange(t1.Result.GetRes<Organizationclient>());
                    }
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


        [dxAuthorize(C.c_role_OrgRead)]
        public ActionResult PreGetHierarchy_Organization(decimal Id, decimal level, string recordtype)
        {

            List<Organizationclient> ret = new List<Organizationclient>();
            Dao<Organizationclient> t = new Dao<Organizationclient>(Sdb);

            if (Id == 0) t.SqlSelect("SELECT /*A+.OwnJoj*/ T.* FROM ORGANIZATION T LEFT JOIN ORGANIZATION O ON T.ID_PARENT = O.ID_ORGANIZATION WHERE O.ID_ORGANIZATION IS NULL AND T.RECORDTYPE=:0 and t.status='Active' ORDER BY T.ID_ORD ", new object[] { recordtype });
            else t.SqlSelect("select /*A+*/ T.* from Organization T where T.Id_Organization=:0 and T.recordtype=:1 and t.status='Active' ", new object[] { Id, recordtype });
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
            if (ret.Count == 0) ret.AddRange(t.Result.GetRes<Organizationclient>());
            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd.", Formatting = Newtonsoft.Json.Formatting.Indented };
            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(ret, settings),
                ContentEncoding = Encoding.UTF8
            };

        }


        [dxAuthorize(C.c_role_OrganizationWrite)]
        public JsonResult MoveHierarchyItem_Organization(int Id, int NewParentId, string dropPos)
        {

            Dao<Organization> dao = new Dao<Organization>(Sdb);
            dao.SqlSelectId(Id);
            if (dao.Result.GetSate(DaoResult.ResCountOne))
            {
                Organization to = dao.Result.GetFirst<Organization>();
                decimal? reorderid = dao.MoveRecordNew(Id, NewParentId, dropPos, "Id_Parent");
                dao.ReorderOrg(reorderid, to.Recordtype);
                to.Property = null;
                dao.SqlUpdate(to);

            }

            // ez lesz jo
            //Dao od = new Dao(Sdb);
            //od.SqlUpdate("update organization t set t.property=null,t.ID_ORD=0 where t.id_organization=:0 ", new object[] { Id });
            //Reorder_Organization(Id);
            return Json(new { Entity = "", Error = Error }, JsonRequestBehavior.AllowGet);
        }

        public void Reorder_Organization(decimal Id)
        {
            // ????? Ez jo???
            List<Organization> work = new List<Organization>();
            Dao<Organization> rodao = new Dao<Organization>(Sdb);
            rodao.SqlSelect("select * from Organization where Id_Parent=:0 and RECORDTYPE=:1 order by Id_Ord", new object[] { Id,"OrgItem" });
            if (rodao.Result.GetSate(DaoResult.ResCountOneOrMore))
            {

                decimal Id_Parent = (decimal)rodao.Result.GetFirst<Organization>().Id_Parent;
                rodao.SqlSelect("select * from Organization where Id_Parent=:0  and RECORDTYPE=:1 order by Id_Ord", new object[] { Id_Parent, "OrgItem" });
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


        #endregion

        #region Status
        // -- Status
        [HttpPost]
        [dxAuthorize(C.c_role_StatusCreate)]
        public JsonResult InsertXform_Status(FormCollection forms)
        {
            XformProcess<Organization> xp = new XformProcess<Organization>(Sdb, Request.Params);

            bool inspos = !string.IsNullOrEmpty(forms["chkInsertPosition"]);
            decimal? ParentParentId = null;
            decimal? Id_Flows = null;
            try { ParentParentId = decimal.Parse(forms["Id_ParentId_Parent"]); } catch { inspos = false; }
            try { Id_Flows = decimal.Parse(forms["Id_Flows"]); } catch { }



             xp.Entity.Id_Parent = (decimal)ParentParentId;
            //// ------------------------- flow!!!
            xp.Entity.Id_Flows = Id_Flows;


            xp.Entity.InitCommonFieldsForAdd(Sdb);
            xp.Entity.Datavalidto = Sdb.Dbsysdate.Sysdbdate;
            xp.Entity.Status = "Developed";
            xp.Entity.Attributum = "New";
            xp.Entity.Recordtype = "StatusItem";
            xp.Entity.Bid_Organization = "NEW";
            xp.Entity.Id_Organization = null;

            //xp.Insert();
            xp.dao.SqlInsert(xp.Entity);

            return Json(new { Entity = xp.Entity, Error = xp.Error }, "text/html");

        }

        [HttpPost]
        [dxAuthorize(C.c_role_StatusWrite)]
        public JsonResult SaveXform_Status(FormCollection forms)
        {
            XformProcess<Organization> xp = new XformProcess<Organization>(Sdb, Request.Params);
            xp.Entity.Attributum = "Modifyed";

            xp.Entity.Id_Organization = (decimal)xp.Id_Entity;
            xp.Save();
            return Json(new { Entity = xp.Entity, Error = xp.Error }, "text/html");
        }

        [dxAuthorize(C.c_role_StatusDelete)]
        public JsonResult DeleteXform_Status(decimal Id_Status, string cmd = null)
        {
            XformProcess<Organization> xp = new XformProcess<Organization>(Sdb);

            xp.dao.SqlSelectId(Id_Status);
            if (xp.dao.Result.GetSate(DaoResult.ResCountOne))
            {
                xp.Loaded = true;
                xp.Entity = xp.dao.Result.GetFirst<Organization>();
                if (xp.Entity.Cpyid == null)
                {
                    xp.Del(Id_Status);
                }
                else
                {
                    xp.Entity.Attributum = "Deleted";
                    xp.Save();
                }
            }

            return Json(new { Error = xp.Error }, JsonRequestBehavior.AllowGet);
        }

        [dxAuthorize(C.c_role_StatusRead)]
        public JsonResult GetXform_Status(decimal Id_Entity, string Id_Xform = "formid")
        {
            XformProcess<Organization> xp = new XformProcess<Organization>(Sdb);
            string[] htt = xp.Get(Id_Entity, Id_Xform);
            return Json(new { Entity = xp.Entity, Xform = htt[0], XformView = htt[1], Error = xp.Error }, JsonRequestBehavior.AllowGet);
        }


        [dxAuthorize(C.c_role_StatusRead)]
        public ActionResult GetRecords_Status(decimal Id, string recordtype)
        {
            List<Organizationclient> res = new List<Organizationclient>();
            List<Organizationclient> ressingle = new List<Organizationclient>();
            Dao<Organizationclient> t = new Dao<Organizationclient>(Sdb);
            //t.SqlSelect("select * from Organization where Id_Parent=:0 and recordtype=:1 ", new object[] { Id, recordtype });
            t.SqlSelect("select X.ID_PERSON,P.USEDNAME,o.* from organization o ,Mxrf_Persons_Org x, persons p where O.CPYID=X.ID_ORGANIZATION(+) and P.ID_PERSONS(+)=X.ID_PERSON and o.Id_Parent=:0 and o.recordtype=:1 and  nvl(o.attributum,'not') not in('Deleted')  ", new object[] { Id, recordtype });
            if (t.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                res = t.Result.GetRes<Organizationclient>();
            }

            var xxx = res.Select(x => x.Id_Organization).Distinct();
            foreach (decimal x in xxx)
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

        [dxAuthorize(C.c_role_OrganizationWrite)]
        public JsonResult SetStatusInTree(decimal Id_Status, string value)
        {
            Dao od = new Dao(Sdb);
            od.SqlUpdate("update organization t set t.property=:1 where t.id_organization=:0 ", new object[] { Id_Status, value });

            return Json(new { Entity = "", Error = Error }, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region Statusrequirements
        [HttpPost]
        [dxAuthorize(C.c_role_RequirementsCreate)]
        public JsonResult InsertXform_Statusrequirements(FormCollection forms)
        {
            /// If id <==> Id_parent hierarchy make trigger Id_Ord
            XformProcess<Statusrequirements> xp = new XformProcess<Statusrequirements>(Sdb, Request.Params);

            decimal? Id_Flows = null;
            try { Id_Flows = decimal.Parse(forms["Id_Flows"]); } catch { }

            xp.Entity.InitCommonFieldsForAdd(Sdb);
            xp.Entity.Datavalidto = Sdb.Dbsysdate.Sysdbdate;
            xp.Entity.Status = "Developed";
            xp.Entity.Attributum = "New";

            xp.Entity.Recordtype = forms["Recordtype"];
            xp.Entity.Itemtype = xp.xform.RootName;
            xp.Entity.Id_Flows =(decimal) Id_Flows;
            xp.Entity.Id_Organization = (decimal)xp.Id_Parent;
            xp.Entity.Id_Statusrequirements = null;
            //xp.Insert();
            xp.dao.SqlInsert(xp.Entity);
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

            xp.dao.SqlSelectId(Id_Statusrequirements);
            if (xp.dao.Result.GetSate(DaoResult.ResCountOne))
            {
                xp.Loaded = true;
                xp.Entity = xp.dao.Result.GetFirst<Statusrequirements>();
                if (xp.Entity.Cpyid == null)
                {
                    xp.Del(Id_Statusrequirements);
                }
                else
                {
                    xp.Entity.Attributum = "Deleted";
                    xp.Save();
                }
            }
            return Json(new { Error = xp.Error }, JsonRequestBehavior.AllowGet);
        }

        [dxAuthorize(C.c_role_RequirementsRead)]
        public JsonResult GetXform_Statusrequirements(decimal Id_Entity, string Id_Xform = "formid")
        {
            XformProcess<Statusrequirements> xp = new XformProcess<Statusrequirements>(Sdb);
            string[] htt = xp.Get(Id_Entity, Id_Xform);
            return Json(new { Entity = xp.Entity, Xform = htt[0], XformView = htt[1], Error = xp.Error }, JsonRequestBehavior.AllowGet);
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