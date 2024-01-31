//using AspNet.Identity.Oracle;
using AspNet.Identity.Oracle;
using Dextra.Common;
using Dextra.Database;
using Dextra.Flowbase;
using Dextra.Toolsspace;
using DextraLib.GeneralDao;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Xapp.Db;
using Xapp.Models;
using System.Text.RegularExpressions;
using Dextra.Xforms;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace Xapp.Controllers
{
    [Authorize]
    public class AdminController : BaseController
    {
        /* Move basecontroller
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
        */
        [PersistField]
        List<Persons> Records_Persons = new List<Persons>();

        AjaxResultCode Error = new AjaxResultCode();

        class functhier
        {
            public string Id { get; set; }
            public string Parent { get; set; }
            public string Type { get; set; }

            public string ItemName { get; set; }
            public string Desc { get; set; }
            public string GroupName { get; set; }
            public string Flag { get; set; }
            public string Param { get; set; }
            public string icon { get; set; }


            public functhier()
            {

            }
            public functhier(int id, int parent, string type, string name, string desc, string group, string flag,string param,  string ico )
            {
                Id = id.ToString();
                Parent = parent.ToString();
                Type = type;
                ItemName = name;
                Desc = desc;
                GroupName = group;
                Flag = flag;
                Param = param;
                string path = @"../Content/icons/famsilkicons/";
                if (string.IsNullOrEmpty(ico)) icon = path + @"user.png"; else icon = path + ico;
            }

        }

        public JsonResult GetFunctionhier()
        {
            int id = 1;
            int idcurrole = 0;
            List<functhier> fh = new List<functhier>();
            List<Functions> All = MvcApplication.Functionroles.Functions;
            foreach (Functions item in All)
            {
                fh.Add(new functhier(id, 0, "Funct", item.Functionname, item.FunctioDesc, item.FunctioGroup, item.Flag, item.Param, "page_red.png"));
                idcurrole = id;

                List<XrefRole> rols = MvcApplication.Functionroles.XrefFunc.Where(f => f.Function == item.Functionname).ToList<XrefRole>();
                foreach (XrefRole rol in rols)
                {
                    id++;
                    fh.Add(new functhier(id, idcurrole, "Role", rol.Role, "", rol.Mode, "", "", "package.png"));
                }
                id++;
            }
            return Json(new { Rows = fh, Error = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetcombosourceFRoles(string curfunct)
        {
            List<Combosource> retv = new List<Combosource>();
            if (string.IsNullOrEmpty(curfunct))
            {
                return Json(new { retv = retv }, JsonRequestBehavior.AllowGet);
            }

            //foreach (string r in MvcApplication.Functionroles.XrefFunc.Select(c => c.Role).Distinct())
            //{
            //    retv.Add(new Combosource(r, r));
           // }

            foreach (Dxroles r in Rolemanager.dxRoles)
            {
                retv.Add(new Combosource(r.Name,r.Name));
            }


            List<XrefRole> curfs = MvcApplication.Functionroles.XrefFunc.Where(t => t.Function == curfunct ).ToList<XrefRole>();
            foreach (XrefRole t in curfs)
            {
                Combosource match = retv.FirstOrDefault(d => d.Typename == t.Role);
                if (match != null)
                {
                    retv.Remove(match);
                }
            }

            return Json(new { retv = retv }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ModifyFNode(FormCollection form)
        {
            bool save = false;
            string xmlstring = System.IO.File.ReadAllText(Tools.Getpath(C.FunctionFile, C.c_XmlFiles));
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlstring);
            XmlNodeList itemNodes = xml.SelectNodes("/Functions/function");
            foreach (XmlNode itemNode in itemNodes)
            {
                if (itemNode["name"].InnerText == form["OrigFunctionName"])
                {
                    string oper = form["Oper"];
                    switch (oper)
                    {
                        case "EditFunction":
                            // itemNode["name"].InnerText = form["FlowName"];
                            itemNode["desc"].InnerText = form["FunctionDesc"];
                            itemNode["group"].InnerText = form["FunctionGroup"];
                            itemNode["param"].InnerText = form["Functionparam"];
                            save = true;
                            break;
                        case "AddRole":

                            XmlElement elem = xml.CreateElement("role");
                            XmlElement elem1 = xml.CreateElement("name");
                            elem1.InnerText = form["Rolefname"];
                            XmlElement elem2 = xml.CreateElement("mode");
                            elem2.InnerText = form["Mode"];
                            elem.AppendChild(elem1);
                            elem.AppendChild(elem2);
                            XmlNode apptr = itemNode["roles"];
                            apptr.AppendChild(elem);
                            save = true;
                            break;
                        case "DelRole":
                            XmlNodeList roles = itemNode.SelectNodes("roles/role");
                            foreach (XmlNode tre in roles)
                            {
                                if (tre["name"].InnerText == form["Rolefname"])
                                {
                                    itemNode["roles"].RemoveChild(tre);
                                }
                            }
                            save = true;
                            break;

                        default:
                            break;
                    }
                }
            }
            if (save)
            {
                xml.Save(Tools.Getpath(C.FunctionFile, C.c_XmlFiles));
                MvcApplication.Functionroles = new LoadRoleManagedFunctions(C.FunctionFile);
            }

            return Json(new { Error = "" }, "text/html");
        }


       class flowshier
       {
            public string Id { get; set; }
            public string  Parent { get; set; }
            public string Type { get; set; }

            public string ItemName { get; set; }
            public string Desc { get; set; }
            public string  GroupName { get; set; }
            public string Flag { get; set; }
            public string Template { get; set; }
            public string icon { get; set; }

            public flowshier(int id, int parent, string type, string name, string desc,string group, string flag,string template,string ico=null)
            {
                Id = id.ToString();
                Parent = parent.ToString();
                Type = type;
                ItemName = name;
                Desc = desc;
                GroupName = group;
                Flag = flag;
                Template = template;
                string path = @"../Content/icons/famsilkicons/";
                if (string.IsNullOrEmpty(ico)) icon =path+ @"user.png"; else icon = path + ico;
            }

            public flowshier()
            {

            }
        }

        [HttpPost]
        public JsonResult ModifyNode(FormCollection form)
        {
            bool save = false;
            string xmlstring = System.IO.File.ReadAllText(Tools.Getpath(C.FlowFile, C.c_XmlFiles));
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlstring);
            XmlNodeList steps = null;
            XmlNodeList itemNodes = xml.SelectNodes("/Flows/flow");
            foreach (XmlNode itemNode in itemNodes)
            {
                if (itemNode["name"].InnerText == form["OrigFlowName"])
                {
                    string oper = form["Oper"];
                    switch (oper)
                    {
                        case "EditFlow":
                           // itemNode["name"].InnerText = form["FlowName"];
                            itemNode["desc"].InnerText = form["FlowDesc"];
                            itemNode["group"].InnerText = form["FlowGroup"];
                            itemNode["template"].InnerText = form["FlowTemplate"];
                            save = true;
                            break;
                        case "EditStep":
                            steps = itemNode.SelectNodes("steps/step");
                            foreach(XmlNode stepNode in steps)
                            {
                                if (stepNode["name"].InnerText == form["OrigStepName"])
                                {
                                   // stepNode["name"].InnerText = form["StepName"];
                                    stepNode["desc"].InnerText = form["StepDesc"];
                                    stepNode["flag"].InnerText = form["Flag"];
                                    try { stepNode["template"].InnerText = form["StepTemplate"]; } catch { } 
                                    save = true;
                                }
                            }
                            break;
                        case "AddTransaction":
                            steps = itemNode.SelectNodes("steps/step");
                            foreach (XmlNode stepNode in steps)
                            {
                                if (stepNode["name"].InnerText == form["OrigStepName"])
                                {
                                    //XmlDocument doc = new XmlDocument();
                                    XmlElement elem = xml.CreateElement("transition");
                                    XmlElement elem1 = xml.CreateElement("tostep");
                                    elem1.InnerText = form["ToStep"];
                                    XmlElement elem2 = xml.CreateElement("postback");
                                    elem2.InnerText = form["Postback"];
                                    elem.AppendChild(elem1);
                                    elem.AppendChild(elem2);
                                    XmlNode apptr = stepNode["transitions"];
                                    apptr.AppendChild(elem);

                                   save = true;
                                }
                            }

                            break;
                        case "DelTransaction":
                            steps = itemNode.SelectNodes("steps/step");
                            foreach (XmlNode stepNode in steps)
                            {
                                if (stepNode["name"].InnerText == form["OrigStepName"])
                                {
                                    XmlNodeList trans = stepNode.SelectNodes("transitions/transition");
                                    foreach(XmlNode tre in trans)
                                    {
                                        if (tre["tostep"].InnerText == form["ToStep"])
                                        {
                                            stepNode["transitions"].RemoveChild(tre);
                                        }
                                    }
                                    save = true;
                                }
                            }
                            break;
                        // --
                        case "AddRole":
                            steps = itemNode.SelectNodes("steps/step");
                            foreach (XmlNode stepNode in steps)
                            {
                                if (stepNode["name"].InnerText == form["OrigStepName"])
                                {
                                    //XmlDocument doc = new XmlDocument();
                                    XmlElement elem = xml.CreateElement("role");
                                    XmlElement elem1 = xml.CreateElement("name");
                                    elem1.InnerText = form["Rolename"];
                                    XmlElement elem2 = xml.CreateElement("mode");
                                    elem2.InnerText = form["Mode"];
                                    elem.AppendChild(elem1);
                                    elem.AppendChild(elem2);
                                    XmlNode apptr = stepNode["roles"];
                                    apptr.AppendChild(elem);

                                    save = true;
                                }
                            }

                            break;
                        case "DelRole":
                            steps = itemNode.SelectNodes("steps/step");
                            foreach (XmlNode stepNode in steps)
                            {
                                if (stepNode["name"].InnerText == form["OrigStepName"])
                                {
                                    XmlNodeList trans = stepNode.SelectNodes("roles/role");
                                    foreach (XmlNode tre in trans)
                                    {
                                        if (tre["name"].InnerText == form["Rolename"])
                                        {
                                            stepNode["roles"].RemoveChild(tre);
                                        }
                                    }
                                    save = true;
                                }
                            }
                            break;


                        default:
                            break;
                    }

                }

            }

            if (save)
            {
               // xml.PreserveWhitespace = true;
                xml.Save(Tools.Getpath(C.FlowFile, C.c_XmlFiles));
                MvcApplication.Flowroles = new LoadRoleManagedFlows(C.FlowFile);
            }

            return Json(new {  Error = "" }, "text/html");
        }
        public JsonResult GetFlowshier()
        {
            int id = 1;

            int idcurflow = 0;
            int idcurstep = 0;
            int idcurtrans = 0;
            int idcurrole = 0;

            List<flowshier> fh = new List<flowshier>();
            List<FlowStep> All = MvcApplication.Flowroles.Flowsteps;

            List<FlowStep> Flows = All.GroupBy(o => o.Flowname).Select(o => o.FirstOrDefault()).ToList<FlowStep>();
            foreach (FlowStep item in Flows)
            {
                fh.Add(new flowshier(id, 0, "Flow", item.Flowname, item.FlowDesc,item.FlowGroup,item.Flag,item.Flowtemplate, "page_red.png"));
                idcurflow = id;
                id++;
                List<FlowStep> res = All.Where(f => f.Flowname == item.Flowname).ToList<FlowStep>();
                foreach (FlowStep fitem in res)
                {
                    fh.Add(new flowshier(id, idcurflow, "Step", fitem.Stepname, fitem.Desc,fitem.FlowGroup,fitem.Flag,fitem.Flowtemplate, "user_edit.png"));
                    idcurstep = id;
                    id++;
                    fh.Add(new flowshier(id, idcurstep, "tHead", "transitions", "","","","", "folder_database.png"));
                    idcurtrans = id;
                    foreach (FlowstepTransition tr in fitem.Transitions)
                    {
                        id++;
                        fh.Add(new flowshier(id, idcurtrans, "Transition", tr.Tostepname, tr.Fromstepname, tr.Postback,"","", "shield.png"));
                    }
                    id++;
                    fh.Add(new flowshier(id, idcurstep, "rHead", "roles", "","","","", "chart_organisation.png"));
                    idcurrole = id;
                    List<XrefRole> rols = MvcApplication.Flowroles.XrefFlows.Where(f => f.Flowname == item.Flowname && f.Stepname == fitem.Stepname).ToList<XrefRole>();
                    foreach (XrefRole rol in rols)
                    {
                        id++;
                        fh.Add(new flowshier(id, idcurrole, "Role", rol.Role, "", rol.Mode, "","", "package.png"));
                    }
                    id++;
                }
            }

            return Json(new { Rows = fh, Error = "" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetcombosourceRoles(string filter, string curstep)
        {
            List<Combosource> retv = new List<Combosource>();
            if (string.IsNullOrEmpty(filter) || string.IsNullOrEmpty(curstep))
            {
                return Json(new { retv = retv }, JsonRequestBehavior.AllowGet);
            }

            //foreach(string r in MvcApplication.Flowroles.XrefFlows.Select(c => c.Role).Distinct())
            //{
            //    retv.Add(new Combosource( r, r));
            //}

            foreach (Dxroles r in Rolemanager.dxRoles)
            {
                retv.Add(new Combosource(r.Name, r.Name));
            }


            List<XrefRole> curfs = MvcApplication.Flowroles.XrefFlows.Where(t => t.Stepname == curstep && t.Flowname == filter).ToList<XrefRole>();
            foreach (XrefRole t in curfs)
            {
                Combosource match = retv.FirstOrDefault(d => d.Typename == t.Role);
                if (match != null)
                {
                    retv.Remove(match);
                }
            }

            return Json(new { retv = retv }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetcombosourceCursteps(string filter,string curstep)
        {
            List<Combosource> retv = new List<Combosource>();
            if (string.IsNullOrEmpty(filter) || string.IsNullOrEmpty(curstep))
            {
                return Json(new { retv = retv }, JsonRequestBehavior.AllowGet);
            }

            FlowStep curfs = new FlowStep();
            List<FlowStep> All = MvcApplication.Flowroles.Flowsteps;

            List<FlowStep> Flows = All.GroupBy(o => o.Flowname).Select(o => o.FirstOrDefault()).ToList<FlowStep>();
            foreach (FlowStep item in Flows)
            {
                if (item.Flowname == filter)
                {
                    List<FlowStep> res = All.Where(f => f.Flowname == item.Flowname).ToList<FlowStep>();
                    foreach (FlowStep fitem in res)
                    {
                        if (fitem.Stepname == curstep) curfs = fitem;
                        retv.Add(new Combosource(fitem.Stepname, fitem.Stepname));
                    }
                }
            }

            foreach (FlowstepTransition t in curfs.Transitions)
            {
                Combosource match = retv.FirstOrDefault(d => d.Typename == t.Tostepname);
                if (match != null)
                {
                    retv.Remove(match);
                }
            }

            return Json(new { retv = retv }, JsonRequestBehavior.AllowGet);
        }



 
        public class Usersview
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string OldPassword { get; set; }

            public Usersview()
            {

            }
        }

 

        // GET: Admin
        public ActionResult Index()
        {
 

            return View();
        }

        // --------------------------------------------------
        class AdminItemsForListbox
        {
            public string Key { get; set; }
            public string Value { get; set; }
            public bool Checked { get; set; }
            public AdminItemsForListbox()
            {

            }

        }

        public JsonResult GetSystemRoles(string Userid)
        {

            if (string.IsNullOrEmpty(Userid)) return Json(new { retv = "NoUser" }, JsonRequestBehavior.AllowGet);
            List<AdminItemsForListbox> roles = new List<AdminItemsForListbox>();

            ApplicationUser user = UserManager.FindByName(Userid);

            IList<string> aroles = UserManager.GetRoles(user.Id);

            Dao<AdminItemsForListbox> rda = new Dao<AdminItemsForListbox>(Sdb);
            rda.SqlSelect(@"select Id as Key,Name as Value from ROLES", new object[] { });
            if (rda.Result.GetSate(DaoResult.ResCountZeroOrMore))
            {
                roles = rda.Result.GetRes<AdminItemsForListbox>();
                foreach (AdminItemsForListbox r in roles)
                {
                    if (aroles.Contains(r.Value)) r.Checked = true; else r.Checked = false;
                }
            }
            return Json(new { retv = roles }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SetSystemRole(string Userid, string role, string roleid, string cmd = "Add")
        {
            ApplicationUser user = UserManager.FindByName(Userid);
            if (user == null) return Json(new { retv = "NoUser" }, JsonRequestBehavior.AllowGet);
            if (!UserManager.IsInRole(user.Id, role) && cmd == "Add")
            {
                UserManager.AddToRole(user.Id, role);
            }
            else if (UserManager.IsInRole(user.Id, role) && cmd != "Add")
            {
                var userResult = UserManager.RemoveFromRole(user.Id, role);
                Dao dd = new Dao(Sdb);
                dd.ExecuteNonQuery("delete from USERROLES where USERID=:0 and ROLEID=:1", new object[] { user.Id, roleid });
            }
            return Json(new { retv = "Ok" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserOrggroup(string Userid)
        {

            if (string.IsNullOrEmpty(Userid)) return Json(new { retv = "NoUser" }, JsonRequestBehavior.AllowGet);
            List<AdminItemsForListbox> roles = new List<AdminItemsForListbox>();
            List<Dxxrefuserorggroup> retv = new List<Dxxrefuserorggroup>();
            Dao<Dxxrefuserorggroup> dao = new Dao<Dxxrefuserorggroup>(Sdb);
            dao.SqlSelect("select * from DXXREFUSERORGGROUP t where T.USERNAME=:0", new object[] { Userid });
            if (dao.Result.GetSate(2))
            {
                retv = dao.Result.GetRes<Dxxrefuserorggroup>();
            }

            Dao<AdminItemsForListbox> rda = new Dao<AdminItemsForListbox>(Sdb);
            rda.SqlSelect(@"select to_char(Id) as Key,GROUPNAME as Value from Dxorggroup", new object[] { });
            if (rda.Result.GetSate(DaoResult.ResCountZeroOrMore))
            {
                roles = rda.Result.GetRes<AdminItemsForListbox>();
                foreach (AdminItemsForListbox r in roles)
                {
                    if (retv.Find(g => g.Groupname==r.Value)!=null) r.Checked = true; else r.Checked = false;
                }
            }
            return Json(new { retv = roles }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SetUserOrggroup(string Userid, string role, string roleid, string cmd = "Add")
        {
            bool found = false;
            Dao<Dxxrefuserorggroup> dd = new Dao<Dxxrefuserorggroup>(Sdb);
            Dxxrefuserorggroup cur = new Dxxrefuserorggroup();
            if ( string.IsNullOrEmpty(Userid)) return Json(new { retv = "NoUser" }, JsonRequestBehavior.AllowGet);
            
            dd.SqlSelect("select * from DXXREFUSERORGGROUP  where username=:0 and groupname=:1", new object[] { Userid , role });
            if (dd.Result.GetSate(DaoResult.ResCountZeroOrMore))
            {
                if (dd.Result.Count == 0)
                {
                    found = false;
                }
                else
                {
                    found = true;
                    cur = dd.Result.GetFirst<Dxxrefuserorggroup>();
                }

                if (!found && cmd == "Add")
                {
                    Dxxrefuserorggroup x = new Dxxrefuserorggroup(null, Userid, role, "");
                    dd.SqlInsert(x);
                }
                else if (found && cmd != "Add")
                {
                    dd.SqlDelete(cur);
                }

            }
            else
            {

            }

            return Json(new { retv = "Ok" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserUsergroup(string Userid)
        {

            if (string.IsNullOrEmpty(Userid)) return Json(new { retv = "NoUser" }, JsonRequestBehavior.AllowGet);
            List<AdminItemsForListbox> roles = new List<AdminItemsForListbox>();
            List<Dxxrefuserusergroup> retv = new List<Dxxrefuserusergroup>();
            Dao<Dxxrefuserusergroup> dao = new Dao<Dxxrefuserusergroup>(Sdb);
            dao.SqlSelect("select * from Dxxrefuserusergroup t where T.USERNAME=:0", new object[] { Userid });
            if (dao.Result.GetSate(2))
            {
                retv = dao.Result.GetRes<Dxxrefuserusergroup>();
            }

            Dao<AdminItemsForListbox> rda = new Dao<AdminItemsForListbox>(Sdb);
            rda.SqlSelect(@"select to_char(Id) as Key,GROUPNAME as Value from DXUSERGROUP", new object[] { });
            if (rda.Result.GetSate(DaoResult.ResCountZeroOrMore))
            {
                roles = rda.Result.GetRes<AdminItemsForListbox>();
                foreach (AdminItemsForListbox r in roles)
                {
                    if (retv.Find(g => g.Groupname == r.Value) != null) r.Checked = true; else r.Checked = false;
                }
            }
            return Json(new { retv = roles }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SetUserUsergroup(string Userid, string role, string roleid, string cmd = "Add")
        {
            bool found = false;
            Dao<Dxxrefuserusergroup> dd = new Dao<Dxxrefuserusergroup>(Sdb);
            Dxxrefuserusergroup cur = new Dxxrefuserusergroup();
            if (string.IsNullOrEmpty(Userid)) return Json(new { retv = "NoUser" }, JsonRequestBehavior.AllowGet);

            dd.SqlSelect("select * from Dxxrefuserusergroup  where username=:0 and groupname=:1", new object[] { Userid, role });
            if (dd.Result.GetSate(DaoResult.ResCountZeroOrMore))
            {
                if (dd.Result.Count == 0)
                {
                    found = false;
                }
                else
                {
                    found = true;
                    cur = dd.Result.GetFirst<Dxxrefuserusergroup>();
                }

                if (!found && cmd == "Add")
                {
                    Dxxrefuserusergroup x = new Dxxrefuserusergroup(null, Userid, role);
                    dd.SqlInsert(x);
                }
                else if (found && cmd != "Add")
                {
                    dd.SqlDelete(cur);
                }

            }
            else
            {

            }

            return Json(new { retv = "Ok" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUsergroup()
        {
            List<Dxusergroup> retv = new List<Dxusergroup>();
            Dao<Dxusergroup> dao = new Dao<Dxusergroup>(Sdb);
            dao.SqlSelect("select * from Dxusergroup ", new object[] { });
            if (dao.Result.GetSate(2))
            {
                retv = dao.Result.GetRes<Dxusergroup>();
            }
            return Json(new { retv = retv }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserGroupRoles(string Usergroup)
        {

            if (string.IsNullOrEmpty(Usergroup)) return Json(new { retv = "NoUsergroup" }, JsonRequestBehavior.AllowGet);
            List<AdminItemsForListbox> roles = new List<AdminItemsForListbox>();
            List<Dxxrefusergrouprole> retv = new List<Dxxrefusergrouprole>();
            Dao<Dxxrefusergrouprole> dao = new Dao<Dxxrefusergrouprole>(Sdb);
            dao.SqlSelect("select * from Dxxrefusergrouprole t where T.GROUPNAME=:0", new object[] { Usergroup });
            if (dao.Result.GetSate(2))
            {
                retv = dao.Result.GetRes<Dxxrefusergrouprole>();
            }

            Dao<AdminItemsForListbox> rda = new Dao<AdminItemsForListbox>(Sdb);
            rda.SqlSelect(@"select to_char(Id) as Key,NAME as Value from DXROLES", new object[] { });
            if (rda.Result.GetSate(DaoResult.ResCountZeroOrMore))
            {
                roles = rda.Result.GetRes<AdminItemsForListbox>();
                foreach (AdminItemsForListbox r in roles)
                {
                    if (retv.Find(g => g.Rolename == r.Value) != null) r.Checked = true; else r.Checked = false;
                }
            }
            return Json(new { retv = roles }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SetUserGroupRoles(string Usergroup, string role,  string cmd = "Add")
        {
            bool found = false;
            Dao<Dxxrefusergrouprole> dd = new Dao<Dxxrefusergrouprole>(Sdb);
            Dxxrefusergrouprole cur = new Dxxrefusergrouprole();
            if (string.IsNullOrEmpty(Usergroup)) return Json(new { retv = "NoUsergroup" }, JsonRequestBehavior.AllowGet);

            dd.SqlSelect("select * from Dxxrefusergrouprole  where groupname=:0 and ROLENAME=:1", new object[] { Usergroup, role });
            if (dd.Result.GetSate(DaoResult.ResCountZeroOrMore))
            {
                if (dd.Result.Count == 0)
                {
                    found = false;
                }
                else
                {
                    found = true;
                    cur = dd.Result.GetFirst<Dxxrefusergrouprole>();
                }

                if (!found && cmd == "Add")
                {
                    Dxxrefusergrouprole x = new Dxxrefusergrouprole(null, role, Usergroup);
                    dd.SqlInsert(x);
                }
                else if (found && cmd != "Add")
                {
                    dd.SqlDelete(cur);
                }

            }
            else
            {

            }

            return Json(new { retv = "Ok" }, JsonRequestBehavior.AllowGet);
        }






        [HttpPost]
        public JsonResult InsertXform_Vw_Users(FormCollection forms)
        {
            XformProcess<Usersview> xp = new XformProcess<Usersview>(Sdb, Request.Params);
            Usersview u = xp.Entity;    //new Usersview(Request);
            string role = "Admin";
            var user = new ApplicationUser() { UserName = u.Username, Email = u.Email };
            //create new user in database
            //var newUser = UserManager.Create(user,u.Password);
            var result = UserManager.CreateAsync(user, u.Password);
            if(result.Result.Succeeded)
            //if (newUser.Succeeded)
            {
                using (var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext())))
                    if (!rm.RoleExists("Proba"))
                    {
                        var roleResult = rm.Create(new IdentityRole(role));
                        if (!roleResult.Succeeded)
                        {

                        }
                    }
                
                var userResult = UserManager.AddToRole(user.Id, role);
                if (!userResult.Succeeded)
                {

                }
                
            }
            else
            {
                Error.Errorcode = 11;
                Error.Errormessage = ((string[])result.Result.Errors)[0];
            }

            return Json(new { Entity =u, Error = Error }, "text/html");
        }
        public JsonResult DeleteXform_Vw_Users(string Id_Entity, string cmd = null)
        {

            ApplicationUser user = UserManager.FindByName(Id_Entity);
            var rolesForUser = UserManager.GetRoles(user.Id);
            if (rolesForUser.Count() > 0)
            {
                foreach (var item in rolesForUser.ToList())
                {
                    var result = UserManager.RemoveFromRole(user.Id, item);
                }
            }

            UserManager.Delete(user);
            Dao d = new Dao(Sdb);
            d.ExecuteNonQuery("update persons set userid=null where userid=:0", new object[] { Id_Entity });

            return Json(new { Error = Error }, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetRecords_Vw_Users(decimal Id, string recordtype)
        public ActionResult GetRecords_Vw_Users(string filter)
        {
            List<Vw_Users> res = new List<Vw_Users>();
            Dao<Vw_Users> t = new Dao<Vw_Users>(Sdb);
            //t.SqlSelect("select * from Vw_Users where Id_Vw_Users>=:0 ", new object[] { Id });
            t.SqlSelect("select * from Vw_Users where Username like :0 ", new object[] { filter+"%" });
            if (t.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                res = t.Result.GetRes<Vw_Users>();
            }

            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd.", Formatting = Newtonsoft.Json.Formatting.Indented };
            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(res, settings),
                ContentEncoding = Encoding.UTF8
            };
        }

        public JsonResult Update_PersonsUserid(decimal Id_Entity,string Userid, string LangLayout)
        {
            Dao<Persons> p = new Dao<Persons>(Sdb);
            p.SqlSelectId(Id_Entity);
            if (p.Result.GetSate(DaoResult.ResCountOne))
            {
                Persons pe = p.Result.GetFirst<Persons>();
                pe.Userid = Userid;
                pe.Assignment = LangLayout;
                if (string.IsNullOrEmpty(Userid)) { pe.Userid = null; pe.Assignment = null; }
                p.SqlUpdate(pe);
            } else
            {
                Error.Errorcode = 11;
                Error.Errormessage = p.Result.Message;
            }

            return Json(new { Error = Error }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //     public async Task<ActionResult>
        public async Task<JsonResult> PasswordReset(FormCollection form)
        {
            XformProcess<Usersview> xp = new XformProcess<Usersview>(Sdb, Request.Params);
            Usersview u = xp.Entity;    //new Usersview(Request);


            string userid = form["Username"]; //u.Username;
            ApplicationUser user = UserManager.FindByName(userid);
            string id = user.Id;

            string password1 = u.OldPassword;
            string password2 = u.Password;
            if (password1 == password2)
            {


                try
                {
                    //Dao test = new Dao(Sdb);
                    //test.SqlUpdate(@"Update Users set UserName =:0, PasswordHash =:1, Email =:2 WHERE Id =:3", new object[] {"Malac","888888","xxx@xmail.hu", "ddc11d03-197d-4615-970a-75f68f8769a4" });

                    user.PasswordHash = UserManager.PasswordHasher.HashPassword(password1);
                    var result1 = await UserManager.UpdateAsync(user);

                    //UserManager.RemovePassword(id);
                    //UserManager.AddPassword(id, password1);
                }
                catch (Exception e)
                {
                    Error.Errorcode = 11;
                    Error.Errormessage = "xxx";
                }
            }
            return Json(new { Error = Error }, "text/html");
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

        [HttpPost]
        public JsonResult InsertXform_Persons(FormCollection forms)
        {
            /// If id <==> Id_parent hierarchy make trigger Id_Ord
            XformProcess<Persons> xp = new XformProcess<Persons>(Sdb, Request.Params);
            //xp.Entity.Itemtype = xp.xform.RootName;
            xp.Entity.Id_Parent = (decimal)xp.Id_Parent;
            //xp.dao.SqlSelect("select * from persons t where FindPerson(T.ID_PERSONS, :0 ,:1 ,:2 ,:3 ,:4 ,:5 ) >0 ", new object[] { xp.Entity.Birthfirstname, xp.Entity.Birthlastname, xp.Entity.Motherfirstname, xp.Entity.Motherlastname, xp.Entity.Placeofbirth, xp.Entity.Birthdate.ToString("yyyy.MM.dd.") });
            if (xp.dao.Result.GetSate(DaoResult.ResCountZero))
            {
                //xp.Entity.Bid_Persons = "NEW";
                xp.Entity.Id_Parent = (decimal)xp.Id_Parent;
                xp.Insert();
            }
            else
            {
                Error.Errorcode = 1;
                Error.Errormessage = "Alreadyexist";
            }

            return Json(new { Entity = xp.Entity, Error = Error }, "text/html");
        }
        [HttpPost]
        public JsonResult SaveXform_Persons(FormCollection forms)
        {
            XformProcess<Persons> xp = new XformProcess<Persons>(Sdb, Request.Params);
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
            return Json(new { Entity = xp.Entity, Error = Error }, "text/html");
        }
        [HttpPost]
        public JsonResult DeleteXform_Persons(FormCollection forms)
        {
            XformProcess<Persons> xp = new XformProcess<Persons>(Sdb, Request.Params);
            xp.Del();
            return Json(new { Entity = xp.Entity, Error = Error }, "text/html");
        }

        public JsonResult DeleteXform_Persons(decimal Id_Persons, string cmd = null)
        {
            XformProcess<Persons> xp = new XformProcess<Persons>(Sdb);
            if (cmd != null && cmd == "close") // onlyclose
            {
                xp.Close(Id_Persons);
            }
            else
            {
                xp.Del(Id_Persons);
            }
            return Json(new { Error = Error }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetXform_Persons(decimal Id_Entity, string Id_Xform = "formid")
        {
            XformProcess<Persons> xp = new XformProcess<Persons>(Sdb);
            string[] htt = xp.Get(Id_Entity, Id_Xform);
            return Json(new { Entity = xp.Entity, Xform = htt[0], XformView = htt[1], Error = Error }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update_Appselector(decimal id_person, string appselector)
        {


            string[] tok = appselector.Split('.');
            UserPref.Appselector = tok[2];
            UserPref.Preflayout = tok[1];
            UserPref.Preflang = tok[0];

            string output = UserPref.Ser();

            Dao<Persons> pd = new Dao<Persons>(Sdb);
            pd.SqlSelectId(id_person);
            if (pd.Result.GetSate(DaoResult.ResCountOne))
            {
                Persons p = pd.Result.GetFirst<Persons>();
                p.Assignment = appselector;
                p.Userpreferences = output;
                pd.SqlUpdate(p);
            }
            return Json(new { ok="", Error = Error }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetRecords_Persons(decimal Id, string recordtype)
        {
            List<Persons> res = new List<Persons>();
            Dao<Persons> t = new Dao<Persons>(Sdb);
            t.SqlSelect("select * from Persons where Id_Parent=:0 ", new object[] { Id });
            if (t.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                res = t.Result.GetRes<Persons>();
            }

            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd.", Formatting = Newtonsoft.Json.Formatting.Indented };
            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(res, settings),
                ContentEncoding = Encoding.UTF8
            };
        }
        public ActionResult GetHierarchy_Persons(decimal Id, string recordtype)
        {
            List<Persons> htre = new List<Persons>();
            Dao<Persons> t = new Dao<Persons>(Sdb);
            t.SqlSelect("select * from Persons where Id_Parent is null", new object[] { });
            if (t.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                Id = (decimal)t.Result.GetFirst<Persons>().Id_Persons;

                Hierarchy<Persons> tree = new Hierarchy<Persons>(Sdb);
                htre = (List<Persons>)tree.GetHierarchy(Id, "Id_Parent");
            }
            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd.", Formatting = Newtonsoft.Json.Formatting.Indented };
            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(htre, settings),
                ContentEncoding = Encoding.UTF8
            };

        }
        public JsonResult MoveHierarchyItem_Persons(int Id, int NewParentId, string dropPos)
        {

            Dao<Persons> dao = new Dao<Persons>(Sdb);
        ///???????? Ez a procedura csak organization ra van    dao.MoveRecord(Id, NewParentId, dropPos, "Id_Parent", "reorder", "OrgItem");
            decimal? reorderid= dao.MoveRecordNew(Id, NewParentId, dropPos, "Id_Parent");
            Reorder_Persons(Id);
            return Json(new { Entity = "", Error = Error }, JsonRequestBehavior.AllowGet);
        }

        public void Reorder_Persons(decimal Id)
        {
            List<Persons> work = new List<Persons>();
            Dao<Persons> rodao = new Dao<Persons>(Sdb);
            rodao.SqlSelect("select * from Persons where Id_Persons=:0", new object[] { Id });
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
               // p.Id_Ord = newordid++;
                rodao.SqlUpdate(p);
            }
        }
        [HttpPost]
        public JsonResult SaveXform_SearchPersons(FormCollection forms)
        {
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

            return Json(new { sql = sql, Entity = xp.Entity, Error = Error }, "text/html");
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


    }
}
 
 
