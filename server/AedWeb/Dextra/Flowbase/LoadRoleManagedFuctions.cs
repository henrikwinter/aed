using Dextra.Toolsspace;
using System.Collections.Generic;
using System.Xml;

namespace Dextra.Flowbase
{
    public class Functions
    {
        public string Functionname { get; set; }
        public string FunctioDesc { get; set; }
        public string FunctioGroup { get; set; }

        public string Controller { get; set; }
        public string Action { get; set; }
        public string Flag { get; set; }
        public string Param { get; set; }

        public List<string> Subroles = new List<string>();
        public Functions()
        {

        }
        public Functions(string name, string desc,string group, string contoller, string action, string flag, string param)
        {
            Functionname = name;
            FunctioDesc = desc;
            FunctioGroup = group;
            Controller = contoller;
            Action = action;
            Flag = flag;
            Param = param;
            Subroles = new List<string>();
        }
    }

    public class FlowStep
    {
        public List<FlowstepTransition> Transitions;
        public decimal? Id_Flow { get; set; }
        public string Flowname { get; set; }
        public string FlowDesc { get; set; }
        public string FlowGroup{ get; set; }
        public string Flowtemplate { get; set; }
        public string Stepname { get; set; }
        public string Controller { get; set; }
        public string Action  { get; set; }
        public string Desc { get; set; }
        public string Flag { get; set; }
        public string Param { get; set; }

        public bool Error { get; set; }
        public string ErrorMessage { get; set; }

        public List<string> Subroles = new List<string>();

        public FlowStep()
        {
            Subroles = new List<string>();
        }


        public FlowStep(FlowStep fs,decimal? id_flow)
        {
            Id_Flow = id_flow;
            Transitions = new List<FlowstepTransition>();
            Transitions.AddRange(fs.Transitions);
            Flowname =  fs.Flowname;
            FlowDesc = fs.FlowDesc;
            FlowGroup = fs.FlowGroup;
            Flowtemplate = fs.Flowtemplate;
            Stepname = fs.Stepname;
            Controller =fs.Controller;
            Action = fs.Action;
            Desc = fs.Desc;
            Flag = fs.Flag;
            Param =fs.Param;
            Subroles = new List<string>();

        }
        public FlowStep(string flowname,string stepname)
        {
            Transitions = new List<FlowstepTransition>();
            Flowname = flowname;
            Stepname = stepname;
            Subroles = new List<string>();
        }
        public FlowStep(string flowname, string flowdesc, string group, string template,string stepname, string contoller, string action, string desc,string flag,string param)
        {
            Transitions = new List<FlowstepTransition>();
            Flowname = flowname;
            FlowDesc = flowdesc;
            FlowGroup = group;
            Flowtemplate = template;
            Stepname = stepname;
            Controller = contoller;
            Action = action;
            Desc = desc;
            Flag = flag;
            Param = param;
            Subroles = new List<string>();
        }


    }
    public class FlowstepTransition
    {
        public string Tostepname { get; set; }
        public string Fromstepname { get; set; }
        public string Postback { get; set; }
        public bool UserHasContinue { get; set; }

        public FlowstepTransition()
        {

        }
        public FlowstepTransition(string to, string from, string p)
        {
            Tostepname = to;
            Fromstepname = from;
            Postback = p;
        }


    }

    public class RenderSartableFlows
    {
        public string Groupname { get; set; }
        public List<FlowStep> startables = new List<FlowStep>();
        public RenderSartableFlows()
        {

        }
        public RenderSartableFlows(string key, List<FlowStep> s)
        {
            Groupname = key;
            startables = new List<FlowStep>();
            startables.AddRange(s);

        }
    }

    public class RenderSartableFunctions
    {
        public string Groupname { get; set; }
        public List<Functions> startables = new List<Functions>();
        public RenderSartableFunctions()
        {

        }
        public RenderSartableFunctions(string key, List<Functions> s)
        {
            Groupname = key;
            startables = new List<Functions>();
            startables.AddRange(s);

        }
    }


    public class XrefRole
    {

        public string Role { get; set; }
        public string Function { get; set; }
        public string Flowname { get; set; }
        public string Stepname { get; set; }
        public string Mode { get; set; }

      //  public string Group { get; set; }

        public XrefRole(string role,string flowname,string stepname,string mode)
        {
            Role = role;
            Stepname = stepname;
            Flowname = flowname;
            Mode = mode;

        }
        public XrefRole(string role, string funct, string mode)
        {
            Role = role;
            Function = funct;
            Mode = mode;
           // Group = group;
        }
    }
    public class Role
    {

        public string RoleName { get; set; }
        public string RoleId { get; set; }

        public Role(string name, string role)
        {
            RoleName = name;
            RoleId = role;

        }
    }

    public class LoadRoleManagedFunctions
    {
        public const string XpathFunctions = "Functions";
        public const string XpathXpathFunctionsName = "name";
        public const string XpathXpathFunctionsDesc = "desc";
        public const string XpathXpathFunctionsGroup = "group";
        public const string XpathXpathFunctionsRoles = "roles";
        public const string XpathXpathFunctionsModules = "modules";
        public const string XpathXpathFunctionsSubroles = "subroles";
        public const string XpathXpathFunctionsModule = "module";
        public const string XpathXpathFunctionsRolesName = "name";
        public const string XpathXpathFunctionsRolesMode = "mode";
        public const string XpathFunctionsController = "controller";
        public const string XpathFunctionsAction = "action";
        public const string XpathFunctionsFlag = "flag";
        public const string XpathFunctionsParam = "param";


        public List<Functions> Functions = new List<Functions>();
        public List<XrefRole> XrefFunc = new List<XrefRole>();
        //public List<Functions> Modules = new List<Functions>();
     

        public LoadRoleManagedFunctions()
        {

        }
        public LoadRoleManagedFunctions(string file)
        {
            XrefFunc = new List<XrefRole>();
            Functions cs = new Functions();
            if(System.IO.File.Exists(Toolsspace.Tools.Getpath(file, C.c_XmlFiles)))
            {
                string xmlstring = System.IO.File.ReadAllText(Toolsspace.Tools.Getpath(file, C.c_XmlFiles));
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(xmlstring);
                XmlNodeList functions = xml.GetElementsByTagName(XpathFunctions)[0].ChildNodes;
                foreach (XmlNode n in functions)
                {
                    string functionname = getElementvalue(n, XpathXpathFunctionsName);
                    string functiondesc = getElementvalue(n, XpathXpathFunctionsDesc);

                    string functioncontroller = getElementvalue(n, XpathFunctionsController);
                    string functionaction = getElementvalue(n, XpathFunctionsAction);
                    string functionflag = getElementvalue(n, XpathFunctionsFlag);
                    string functionparam = getElementvalue(n, XpathFunctionsParam);
                    string functiongroup = getElementvalue(n, XpathXpathFunctionsGroup);
                    List<string> fsubroles = new List<string>();
                    try
                    {
                        XmlNodeList subroles = n.SelectSingleNode(XpathXpathFunctionsSubroles).ChildNodes;
                        foreach (XmlNode s in subroles)
                        {
                            fsubroles.Add(s.InnerText);
                        }
                    }
                    catch { }
                    cs = new Functions(functionname, functiondesc, functiongroup, functioncontroller, functionaction, functionflag, functionparam);
                    cs.Subroles.AddRange(fsubroles);
                    XmlNodeList roles = n.SelectSingleNode(XpathXpathFunctionsRoles).ChildNodes;
                    foreach (XmlNode r in roles)
                    {
                        string rolename = getElementvalue(r, XpathXpathFunctionsRolesName);
                        string rolemod = getElementvalue(r, XpathXpathFunctionsRolesMode);
                        XrefFunc.Add(new XrefRole(rolename, functionname, rolemod));
                    }

                    Functions.Add(cs);

                }

            }
        }
       string getElementvalue(XmlNode n, string key)
        {
            if (n == null) return "";
            try
            {
                return n.SelectSingleNode(key).InnerText;
            }
            catch
            {

            }
            return "";
        }

    }


    public class LoadRoleManagedFlows
    {
        public const string XpathFlows = "Flows";
        public const string XpathFlowName = "name";
        public const string XpathFlowDesc = "desc";
        public const string XpathFlowTemplate = "template";
        public const string XpathFlowGroup = "group";
        public const string XpathFlowSteps = "steps";
        public const string XpathFlowStepName = "name";
        public const string XpathFlowStepDesc = "desc";
        public const string XpathFlowStepController = "controller";
        public const string XpathFlowStepAction = "action";
        public const string XpathFlowStepFlag = "flag";
        public const string XpathFlowStepParam = "param";
        public const string XpathFlowStepTemplate = "template";
        public const string XpathFlowStepsTransitions = "transitions";
        public const string XpathFlowStepsRoles = "roles";
        public const string XpathXpathFunctionsSubroles = "subroles";
        public const string XpathFlowStepsRolesName = "name";
        public const string XpathFlowStepsRolesMode = "mode";


        public const string XpathFlowStepsTransitionsTostep = "tostep";
        public const string XpathFlowStepsTransitionsPostback = "postback";

        // public List<Flowdescr> Flowdescr = new List<Flowbase.Flowdescr>();

        public List<FlowStep> Flowsteps = new List<FlowStep>();



        public List<XrefRole> XrefFlows = new List<XrefRole>();

        

        public List<Role> Roles = new List<Role>();

        public LoadRoleManagedFlows()
        {

        }

        string getElementvalue(XmlNode n, string key)
        {
            if (n == null) return "";
            try
            {
                return n.SelectSingleNode(key).InnerText;
            } catch
            {

            }
            return "";
        }

        public LoadRoleManagedFlows(string file)
        {
            XrefFlows = new List<XrefRole>();
            FlowStep cs = new FlowStep();
            
            if (System.IO.File.Exists(Toolsspace.Tools.Getpath(file, C.c_XmlFiles)))
            {
                string xmlstring = System.IO.File.ReadAllText(Toolsspace.Tools.Getpath(file, C.c_XmlFiles));
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(xmlstring);
                XmlNodeList flows = xml.GetElementsByTagName(XpathFlows)[0].ChildNodes;
                foreach (XmlNode n in flows)
                {
                    string flowname = getElementvalue(n, XpathFlowName);
                    string flowdesc = getElementvalue(n, XpathFlowDesc);
                    string flowgroup = getElementvalue(n, XpathFlowGroup);
                    string flowtemplate = getElementvalue(n, XpathFlowTemplate);
                    XmlNodeList steps = n.SelectSingleNode(XpathFlowSteps).ChildNodes;
                    foreach (XmlNode s in steps)
                    {

                        string stepname = getElementvalue(s, XpathFlowStepName);
                        string stepdesc = getElementvalue(s, XpathFlowStepDesc);
                        string stepcontroller = getElementvalue(s, XpathFlowStepController);
                        string stepaction = getElementvalue(s, XpathFlowStepAction);
                        string stepflag = getElementvalue(s, XpathFlowStepFlag);
                        string stepparam = getElementvalue(s, XpathFlowStepParam);
                        string steptemplate = getElementvalue(s, XpathFlowStepTemplate);
                        if (!string.IsNullOrEmpty(steptemplate)) flowtemplate = steptemplate;
                        cs = new FlowStep(flowname, flowdesc, flowgroup, flowtemplate, stepname, stepcontroller, stepaction, stepdesc, stepflag, stepparam);
                        XmlNodeList trans = s.SelectSingleNode(XpathFlowStepsTransitions).ChildNodes;
                        foreach (XmlNode t in trans)
                        {
                            //string transname = t.InnerText;
                            string tostep = getElementvalue(t, XpathFlowStepsTransitionsTostep);
                            string postbackmode = getElementvalue(t, XpathFlowStepsTransitionsPostback);

                            cs.Transitions.Add(new FlowstepTransition(tostep, stepname, postbackmode));
                        }
                        XmlNodeList roles = s.SelectSingleNode(XpathFlowStepsRoles).ChildNodes;
                        foreach (XmlNode r in roles)
                        {
                            string rolename = getElementvalue(r, XpathFlowStepsRolesName);
                            string rolemod = getElementvalue(r, XpathFlowStepsRolesMode);
                            XrefFlows.Add(new XrefRole(rolename, flowname, stepname, rolemod));
                        }

                        List<string> fsubroles = new List<string>();
                        try
                        {
                            XmlNodeList subroles = n.SelectSingleNode(XpathXpathFunctionsSubroles).ChildNodes;
                            foreach (XmlNode su in subroles)
                            {
                                fsubroles.Add(su.InnerText);
                            }
                        }
                        catch { }

                        cs.Subroles.AddRange(fsubroles);

                        Flowsteps.Add(cs);
                        // Flowdescr.Add(new Flowdescr(cs.Flowname, cs.FlowDesc, cs.FlowGroup, flowtemplate));
                    }

                }

            }


        }

    }

}