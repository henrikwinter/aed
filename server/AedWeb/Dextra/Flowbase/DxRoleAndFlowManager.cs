//using Dextra.Database;
using Dextra.Database;
using Dextra.Toolsspace;
using DextraLib.GeneralDao;
using Xapp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Xapp.Db;

namespace Dextra.Flowbase
{
    public class Orgroup
    {

        public string OrgroupName { get; set; }
        public decimal Number { get; set; }

        public Orgroup(string name, decimal n)
        {
            OrgroupName = name;
            Number = n;

        }
    }

    [Serializable]
    public class BaseFlowData
    {
        public string  Errormsgs { get; set; }
        public bool Error { get; set; }
        public BaseFlowData()
        {
            Errormsgs = "";
            Error = false;
        }

    }

    [Serializable]
    public class PostedFlowdata
    {
        public string ToFlowName { get; set; }
        public string ToStepname { get; set; }
        public string Continoue { get; set; }
        public string History { get; set; }

        public string Errormsgs { get; set; }
        public bool Error { get; set; }

        public PostedFlowdata()
        {
            Errormsgs = "";
            Error = false;
        }
    }

    public class DxFlowManager
    {
        public DxFlowManager()
        {

        }


        public PostedFlowdata PostedFlowData { get; set; }

        public string CurrentUser { get; set; }
        public string  BackUrl { get; set; }

        public bool Complett { get; set; }

        List<Vw_User> AvailableRoles = new List<Vw_User>();
        public DxFlowManager(DbContext db,List<Vw_User> roles,string currentUser)
        {
            AvailableRoles = roles;
            CurrentUser = currentUser;
            Fdb = db;

            PostedFlowData = new PostedFlowdata();
        }
        public FlowStep CurrentFlowstep { get; set; }

        public Flow dbflow = new Flow();
        DbContext Fdb = null;

        public bool Erroroccure { get; set; }


        //-------------------------------------------------------------------------------------------


        public List<RenderSartableFlows> StartableFlowSteps()
        {
            List<FlowStep> retvalue = new List<FlowStep>();

            List<FlowStep> startables = MvcApplication.Flowroles.Flowsteps.Where(f => f.Flag == C.c_Flow_Startprefix).ToList<FlowStep>();
            foreach (FlowStep item in startables)
            {
                List<XrefRole> res = MvcApplication.Flowroles.XrefFlows.Where(f => f.Flowname == item.Flowname && f.Stepname == item.Stepname && isRole(f.Role)).ToList<XrefRole>();
                if (res.Count > 0) retvalue.Add(item);
            }

            List<RenderSartableFlows> ret1 = new List<RenderSartableFlows>();
            var result1 = retvalue.Where(f => f.Flag == C.c_Flow_Startprefix).GroupBy(test => test.FlowGroup);
            foreach (var it in result1)
            {
                ret1.Add(new RenderSartableFlows(it.Key, it.Distinct<FlowStep>().ToList()));
            }

            return ret1;
        }


        // Ez csak a Start stepnel kell
        public void _Getflowstep(string flowname, string stepname)
        {
            dbflow = new Flow();
            CurrentFlowstep = new FlowStep( MvcApplication.Flowroles.Flowsteps.FirstOrDefault(f => f.Flowname == flowname && f.Stepname == stepname),null);
            SetTransitionFlag(CurrentFlowstep);
        }
        public bool _Check()
        {
            return PostedFlowData.Continoue == C.c_Flow_Cancelcmd;
        }

        public void _Getflowstep(decimal flowid)
        {

            Dao<Flow> fdao = new Dao<Flow>(Fdb);
            fdao.SqlSelect("Select /*A+*/ * from Flows where Id_Flow=:0", new object[] { flowid });
            if (fdao.Result.GetSate(DaoResult.ResCountOne))
            {
                dbflow = fdao.Result.GetFirst<Flow>();
                CurrentFlowstep =new FlowStep( MvcApplication.Flowroles.Flowsteps.FirstOrDefault(n => n.Flowname == dbflow.Flowname && n.Stepname == dbflow.Stepname), flowid);
                SetTransitionFlag(CurrentFlowstep);
            }
            else
            {
                CurrentFlowstep = null;
            }

        }

        public void _Setflowstep(string flowname, string stepname,string flowdesc=null)
        {
            CurrentFlowstep =new FlowStep(MvcApplication.Flowroles.Flowsteps.FirstOrDefault(f => f.Flowname == flowname && f.Stepname == stepname),CurrentFlowstep.Id_Flow);
            SetTransitionFlag(CurrentFlowstep);
            dbflow.Flowname = CurrentFlowstep.Flowname;
            dbflow.Stepname = CurrentFlowstep.Stepname;
            dbflow.Controller = CurrentFlowstep.Controller;
            dbflow.Action = CurrentFlowstep.Action;
            if (!string.IsNullOrEmpty(flowdesc))
            {
                string pattern = @"\((.*?)\)";
                dbflow.Title = Regex.Replace(dbflow.Title,pattern, "("+flowdesc+")");
            }
            if (CurrentFlowstep.Flag == C.c_Flow_Endprefix)
            {
                dbflow.Recordvalidto = DateTime.Now;
                dbflow.Datavalidto = DateTime.Now;
                dbflow.Status = "Closed";
            }

            _Setflowstep();
        }


        public  void _Setflowstep()
        {
            Dao<Flow> fdao = new Dao<Flow>(Fdb);
            fdao.SqlUpdate(dbflow);
            Erroroccure = fdao.Result.Error;
        }

        public void _Makeflowstep(string flowname, string stepname,string describe=null)
        {
            CurrentFlowstep =new FlowStep( MvcApplication.Flowroles.Flowsteps.FirstOrDefault(f => f.Flowname == flowname && f.Stepname == stepname),null);
            SetTransitionFlag(CurrentFlowstep);
            if (!string.IsNullOrEmpty(describe))
            {
                string test = @"{0} ({1})";
                if (describe.StartsWith("|"))
                {
                    CurrentFlowstep.Desc = string.Format(test, CurrentFlowstep.FlowDesc, describe);
                }
                else
                {
                    CurrentFlowstep.Desc = string.Format(test, CurrentFlowstep.FlowDesc, describe);
                }
            }
            dbflow.Flowname = CurrentFlowstep.Flowname;
            dbflow.Stepname = CurrentFlowstep.Stepname;
            dbflow.Controller = CurrentFlowstep.Controller;
            dbflow.Action = CurrentFlowstep.Action;
            dbflow.Title = CurrentFlowstep.Desc;
            _Makeflowstep();
        }
        public  void _Makeflowstep()
        {
            Dao<Flow> fdao = new Dao<Flow>(Fdb);
            dbflow.InitCommonFieldsForAdd(Fdb);
            dbflow.Id_Flow = null;
            fdao.SqlInsert(dbflow);
            dbflow.Id_Flow = fdao.Result.Lastid;
            CurrentFlowstep.Id_Flow= fdao.Result.Lastid;
        }


        public ActionResult _Set(string Backurl,string flowdesc=null)
        {
            ActionResult ret;
            string describe = AddHistory(HttpUtility.HtmlDecode(PostedFlowData.History), flowdesc);
            if (!string.IsNullOrEmpty(flowdesc)) describe = flowdesc;
              
            if (CurrentFlowstep.Id_Flow == null)
            {
                
                _Makeflowstep(CurrentFlowstep.Flowname, PostedFlowData.ToStepname, describe);
            }
            else
            {
                _Setflowstep(CurrentFlowstep.Flowname, PostedFlowData.ToStepname, describe);
            }
            if ( CurrentFlowstep.Flag == C.c_Flow_Endprefix) ret = new RedirectResult(Backurl);

            if (PostedFlowData.Continoue == C.c_Flow_PostBoolTrue)
            {
                ret = new RedirectResult("~/" + CurrentFlowstep.Controller + "/" + CurrentFlowstep.Action + "?Id_Flow=" + CurrentFlowstep.Id_Flow.ToString() + "&c=true");

            }
            else
            {
                ret = new RedirectResult(Backurl);
            }
            return ret;
        }

        public  void End()
        {
          
            dbflow.Flowname = CurrentFlowstep.Flowname;
            dbflow.Stepname = CurrentFlowstep.Stepname;
            dbflow.Controller = CurrentFlowstep.Controller;
            dbflow.Action = CurrentFlowstep.Action;
            //dbflow.Title = CurrentFlowstep.Desc;
            dbflow.Recordvalidto = DateTime.Now;
            dbflow.Datavalidto= DateTime.Now;
            dbflow.Status = "Closed";
            _Setflowstep();
        }


        public bool isRole(string rolename)
        {
            List<Vw_User> t = AvailableRoles.Where(r => r.Rolename == rolename).ToList<Vw_User>();
            if (t.Count>0) return true;
            return false;
        }


        public bool isAcces(string flowname)
        {

            List<XrefRole> res = MvcApplication.Functionroles.XrefFunc.Where(c => c.Flowname == flowname).ToList<XrefRole>();
            foreach (XrefRole i in res)
            {
                if (isRole(i.Role)) return true;
            }
            return false;
        }


        public string getAvilableFlowsteps()
        {
            List<string> retv = new List<string>();
            List<XrefRole> work = MvcApplication.Flowroles.XrefFlows.Where(f => isRole(f.Role)).ToList<XrefRole>();
            foreach(XrefRole item in work)
            {
                if (!retv.Contains(item.Flowname + item.Stepname)) retv.Add(item.Flowname + item.Stepname);
            }
            return "'" +string.Join("','", retv.ToArray())+"'";
        }

        public string AddHistory(string ExtHistory,string udata=null)
        {
            if (string.IsNullOrEmpty(dbflow.Flowhistory))
            {
                dbflow.Flowhistory = @"<?xml version=""1.0"" encoding=""UTF-8""?><Flow xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""  xsi:noNamespaceSchemaLocation=""" + C.c_FlowstepDataXsdFile + @"""></Flow>";
            }

            XElement xudata = new XElement("Udata");
            if (!string.IsNullOrEmpty(udata))
            {
                xudata.Value = udata;
            }


            TextReader tr = new StringReader(dbflow.Flowhistory);
            XDocument doc = XDocument.Load(tr);
            string describe = "";
            if (!string.IsNullOrEmpty(ExtHistory))
            {
                XElement act =  XElement.Parse(ExtHistory);
                try
                {
                    describe = act.Elements().FirstOrDefault(x => x.Name == "Describe").Value;
                } catch { }

                act.RemoveAttributes();

                doc.Element("Flow").Add(
                  new XElement("Action",
                    new XElement("transitionName", CurrentFlowstep.Stepname),
                    new XElement("userName", CurrentUser),
                    new XElement("date", DateTime.Now.ToString("yyyy.MM.dd. HH:mm:ss")),                                                    //DateTime.Now.ToString("o"))
                    //XElement.Parse(action)
                    act,
                    xudata
                        ));

            }
            else
            {
                doc.Element("Flow").Add(
                  new XElement("Action",
                    new XElement("transitionName", CurrentFlowstep.Stepname),
                    new XElement("userName", CurrentUser),
                    new XElement("date", DateTime.Now.ToString("o")),
                    xudata
                        ));
            }

            
            dbflow.Flowhistory = doc.ToString();
            return describe; // doc.ToString();

        }

        void SetTransitionFlag(FlowStep CurrentFlowstep)
        {
            foreach (FlowstepTransition t in CurrentFlowstep.Transitions)
            {
                var te = MvcApplication.Flowroles.XrefFlows.Where(r => r.Flowname == CurrentFlowstep.Flowname && r.Stepname == t.Tostepname && isRole(r.Role));
                if (te == null) t.UserHasContinue = false;
                else
                {
                    FlowStep ts= MvcApplication.Flowroles.Flowsteps.FirstOrDefault(d => d.Flowname == CurrentFlowstep.Flowname && d.Stepname == t.Tostepname);
                    if (ts!=null && ts.Flag == "End")
                    {
                        t.UserHasContinue = false;
                    } else
                    {
                        t.UserHasContinue = true;
                    }

                }

            }
        }

    }

    public class DxRoleManager
    {
        public string CurrentUser { get; set; }
        public Orgroup DefaultOrgroup { get; set; }

        List<Orgroup> VisisbleOrgoups = new List<Orgroup>();

        List<Orgroup> AvailableOrgoups = new List<Orgroup>();

        public List<Vw_User> AvailableRoles = new List<Vw_User>();
        public List<string> Roles = new List<string>();

        public List<string> SubRoles = new List<string>();

        public string Preferedlang { get; set; }
        public List<Dxroles> dxRoles { get; set; }

        DbContext Fdb = null;
        public DxRoleManager()
        {

        }
        public DxRoleManager(DbContext db, string username,bool f)
        {
            CurrentUser = username;
            Dao<Vw_User> dao = new Dao<Vw_User>(db);
            dao.SqlSelect("select * from Vw_User where username=:0 ", new object[] { username });
            if (dao.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                AvailableRoles = dao.Result.GetRes<Vw_User>();
            }

            List<string> subr = new List<string>();
            foreach (Vw_User r in AvailableRoles)
            {
                Roles.Add(r.Rolename);
                List<XrefRole> c = MvcApplication.Functionroles.XrefFunc.FindAll(x => x.Role == r.Rolename);
                foreach (XrefRole x in c)
                {
                    List<Functions> f2 = MvcApplication.Functionroles.Functions.FindAll(z => z.Functionname == x.Function);
                    foreach (Functions fi in f2)
                    {
                        subr.AddRange(fi.Subroles);
                    }
                }
            }

            SubRoles.AddRange(subr.Distinct());
        }

        public DxRoleManager(DbContext db, string username)
        {



            CurrentUser = username;
            Dao<Vw_User> dao = new Dao<Vw_User>(db);
            dao.SqlSelect("select * from Vw_User where username=:0 ", new object[] { username });
            if (dao.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                AvailableRoles = dao.Result.GetRes<Vw_User>();
            }

            List<string> subr = new List<string>();
            foreach(Vw_User r in AvailableRoles)
            {
                Roles.Add(r.Rolename);
                List<XrefRole> c = MvcApplication.Functionroles.XrefFunc.FindAll(x => x.Role == r.Rolename);
                foreach(XrefRole x in c)
                {
                    List<Functions> f = MvcApplication.Functionroles.Functions.FindAll(z => z.Functionname == x.Function);
                    foreach(Functions fi in  f)
                    {
                        subr.AddRange(fi.Subroles);
                    }
                }

                List<XrefRole> c1 = MvcApplication.Flowroles.XrefFlows.FindAll(x => x.Role == r.Rolename);
                foreach (XrefRole x in c)
                {
                    List<FlowStep> f = MvcApplication.Flowroles.Flowsteps.FindAll(z => z.Flowname == x.Flowname);
                    foreach (FlowStep fi in f)
                    {
                        subr.AddRange(fi.Subroles);
                    }
                }

            }

            SubRoles.AddRange(subr.Distinct());

            DefaultOrgroup = new Orgroup("", 0);
            List<Vw_User_Orggroup> temp = new List<Vw_User_Orggroup>();
            Dao<Vw_User_Orggroup> daoo = new Dao<Vw_User_Orggroup>(db);
            daoo.SqlSelect("select * from Vw_User_Orggroup where username=:0 ", new object[] { username });
            if (daoo.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                try
                {
                    temp = daoo.Result.GetRes<Vw_User_Orggroup>();
                    foreach (Vw_User_Orggroup item in temp)
                    {
                        if (item.Flag == "DefWrite")
                        {
                            DefaultOrgroup = new Orgroup(item.Groupname, item.Groupvalue);
                        }
                        else if (item.Flag == "Read")
                        {
                            VisisbleOrgoups.Add(new Orgroup(item.Groupname, item.Groupvalue));
                        }
                        else if (item.Flag == "Write")
                        {
                            AvailableOrgoups.Add(new Orgroup(item.Groupname, item.Groupvalue));
                        }

                    }

                }
                catch (Exception e)
                {
                    var ee = e;
                }
            }


            
            Dao<Dxroles> dxroles = new Dao<Dxroles>(db);
            dxroles.SqlSelect(@"select * from DXROLES d ", new object[] { });
            if (dxroles.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                dxRoles = dxroles.Result.GetRes<Dxroles>();
            }


        }
    }

  
    public class DxFunctionManager
    {

        public string CurrentUser { get; set; }
        List<Vw_User> AvailableRoles = new List<Vw_User>();
        public DxFunctionManager(DbContext db, List<Vw_User> roles, string currentUser)
        {
            AvailableRoles = roles;
            CurrentUser = currentUser;
        }
        public DxFunctionManager()
        {

        }

        public List<RenderSartableFunctions> StartableFunctions(string filter=null)
        {
            List<Functions> workfunctions = MvcApplication.Functionroles.Functions.Where(f => f.FunctioGroup != "Navbar" && f.FunctioGroup != "Hide").ToList<Functions>();
            if (filter == "Navbar")
            {
                workfunctions = MvcApplication.Functionroles.Functions.Where(f => f.FunctioGroup == "Navbar").ToList<Functions>();

            }
            List<Functions> retvalue = new List<Functions>();
            foreach (Functions item in workfunctions)
            {
                List<XrefRole> res = MvcApplication.Functionroles.XrefFunc.Where(f => f.Function == item.Functionname && isRole(f.Role) ).ToList<XrefRole>();
               
                if (res.Count > 0) retvalue.Add(item);

            }

            List<RenderSartableFunctions> ret1 = new List<RenderSartableFunctions>();
            var result1 = retvalue.GroupBy(test => test.FunctioGroup);
            foreach (var it in result1)
            {
                ret1.Add(new RenderSartableFunctions(it.Key, it.Distinct<Functions>().ToList()));
            }

            return ret1;
        }
        public bool isRole(string rolename)
        {
            List<Vw_User> t = AvailableRoles.Where(r => r.Rolename == rolename).ToList<Vw_User>();
            if (t.Count > 0) return true;
            return false;
        }


        public bool isAcces(string functionname)
        {

            List<XrefRole> res = MvcApplication.Functionroles.XrefFunc.Where(c => c.Function == functionname).ToList<XrefRole>();
            foreach(XrefRole i in res)
            {
                if (isRole(i.Role)) return true;
            }
            return false;
        }

    }

}