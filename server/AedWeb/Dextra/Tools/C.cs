using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dextra.Toolsspace
{
    public static class C
    {
        public const string ApdataFilesDir = "~/App_Data/Files";
        public const string LangDir = "~/App_Data/Files/Lang";
        public const string SchemaDir = "~/App_Data/Files/Schemes";
        public const string LangFile = "lang_hu.csv";
        public const string LangFileOut = "lang_hu_out.csv";

        public const string EnumFile = "enum_hu.csv";
        public const string EnumFileOut = "enum_hu_out.csv";

        //public const string FlowFile = "rolbased.xml";
        public const string FlowFile = "Flows.xml";
        public const string FunctionFile = "Functions.xml";
        public const string GlobalSchemaFile = "Glob.xsd";
        public const string AppSchemaFile = "App.xsd";

        public const string c_HtmlXformTemplate = "~/App_Data/Files/Html/XformTemplate";
        public const string c_HtmlTemplate = "~/App_Data/Files/HtmlTemplates";
        public const string c_HtmlTemplateFlow = "~/App_Data/Files/HtmlTemplates/Flow";
        public const string c_HtmlTemplateOrg = "~/App_Data/Files/HtmlTemplates/Org";
        public const string c_XmlFiles = "~/App_Data/Files/Xml";
        public const string c_XmlCodesFiles = "~/App_Data/Files/Xml/Codes";
        public const string c_Temp = "~/Files/Temp";
        public const string c_Tempu = "Files/Temp/";

        public const string c_FlowstepDataXsdFile = "~/App_Data/Files/Schemes/Core/General/FlowstepData-1.2.xsd";


        public const string c_FlowstepDataDefRoot = "GeneralStep";
        public const string c_FlowstepDataStartRoot = "StartStep";
        public const string c_FlowstepDataNamespace = "Xszenyor.FlowDatas.";

        //public const string c_abs_apppath = @"c:/Work3/WebApp";
        //public const string c_abs_reppath = @"c:/Work3/WebApp/Files/Html/Templates";

        public const string c_CssImgPath = @"Content/images";

        public const string c_rel_apppath = @"../";
        public const string c_rel_reppath = @"../Files/Html/Templates";

        public const string c_Flow_Startprefix = @"Start";
        public const string c_Flow_Endprefix = @"End";

        public const string c_Flow_PostBoolTrue = @"true";

        public const string c_Flow_Cancelcmd = @"Cancel";
        
        //dxAuthorize
        public const string c_role_OrgRead      = @"OrgRead";
        public const string c_role_OrgCreate    = @"OrgCreate"; 
        public const string c_role_OrgWrite     = @"OrgWrite";
        public const string c_role_OrgDelete    = @"OrgDelete";

        public const string c_role_OrganizationRead = @"DevOrgRead";
        public const string c_role_OrganizationCreate = @"DevOrgCreate";
        public const string c_role_OrganizationWrite = @"DevOrgWrite";
        public const string c_role_OrganizationDelete = @"DevOrgDelete";




        public const string c_role_StatusRead   = @"StatusRead";
        public const string c_role_StatusCreate = @"StatusCreate";
        public const string c_role_StatusWrite  = @"StatusWrite";
        public const string c_role_StatusDelete = @"StatusDelete";

        public const string c_role_RequirementsRead   = @"RequirementsRead";
        public const string c_role_RequirementsCreate = @"RequirementsCreate";
        public const string c_role_RequirementsWrite  = @"RequirementsWrite";
        public const string c_role_RequirementsDelete = @"RequirementsDelete";



        public const string c_role_PersonsRead = @"PersonsRead";
        public const string c_role_PersonsCreate = @"PersonsCreate";
        public const string c_role_PersonsWrite = @"PersonsWrite";
        public const string c_role_PersonsDelete = @"PersonsDelete";

        public const string c_role_AnimalRead =   @"AnimalRead";
        public const string c_role_AnimalCreate = @"AnimalCreate";
        public const string c_role_AnimalWrite =  @"AnimalWrite";
        public const string c_role_AnimalDelete = @"AnimalDelete";

        public const string c_role_AedRead = @"AedRead";

    }
}