using System;
using System.Collections.Generic;
using Dextra.Toolsspace;
using Xapp;
using DextraLib.XForm;
using System.Xml.Schema;
using System.Linq;
using System.Xml;

namespace Dextra.Xforms
{
    public class XformOld : DextraLib.XFormV0.XForm
    {
        public XformOld() :base()
        {

        }
        public XformOld(string root, Dictionary<string, string> iRefroot = null) : base(MvcApplication.Gschema, root, iRefroot)
        {

            this.XsltPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Files/Schemes/Xslt");
            this.DefRender.LangTrans = Langue.TranslateXform;
            this.DefRender.getpath = Toolsspace.Tools.GetXformXmlComboPath;
        }

        public XformOld(string xformxml, bool force) : base(MvcApplication.Gschema, xformxml, force)
        {
            try
            {
                this.XsltPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Files/Schemes/Xslt");
                this.DefRender.LangTrans = Langue.TranslateXform;
                this.DefRender.getpath= Toolsspace.Tools.GetXformXmlComboPath;
            }
            catch (Exception e)  {

            }
        }

        public XformOld(System.Collections.Specialized.NameValueCollection PostVal, bool force = false) : base(MvcApplication.Gschema, PostVal, force)
        {
            this.XsltPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Files/Schemes/Xslt");
            this.DefRender.LangTrans = Langue.TranslateXform;
            this.DefRender.getpath = Toolsspace.Tools.GetXformXmlComboPath;
        }

        public void ChangeRoot( string NewRoot, string NewRefroot = null)
        {
            base.ChangeRoot(MvcApplication.Gschema, NewRoot, NewRefroot);
            this.DefRender.LangTrans = Langue.TranslateXform;
            this.DefRender.getpath = Toolsspace.Tools.GetXformXmlComboPath;
        }

    }

    public class Xform : XForm
    {
        public DextraLib.XForm.DefRender DefRender;
        public Xform() :base()
        {

        }

        public Xform(string root, Dictionary<string, string> iRefroot = null, string selector = "Gschema") : base(MvcApplication.XformSchemes[selector], root, iRefroot)
        {

            DefRender = new DextraLib.XForm.DefRender(this);
            this.XsltPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Files/Schemes/Xslt");
            this.DefRender.LangTrans = Langue.TranslateXform;
            //this.DefRender.getcurlang =
            this.DefRender.EnumTrans = Langue.EnumTranslateXform;
            this.DefRender.getpath = Toolsspace.Tools.GetXformXmlComboPath;
        }
        public Xform(System.Collections.Specialized.NameValueCollection PostVal, string selector = "Gschema") : base(MvcApplication.XformSchemes[selector], PostVal)
        {
            PostVal.Get("XformRoot");
            DefRender = new DextraLib.XForm.DefRender(this);
            this.XsltPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Files/Schemes/Xslt");
            this.DefRender.LangTrans = Langue.TranslateXform;
            //this.DefRender.langselector = "hu";
            this.DefRender.EnumTrans = Langue.EnumTranslateXform;
            this.DefRender.getpath = Toolsspace.Tools.GetXformXmlComboPath;
        }
        public Xform(string xformxml, string selector) : base(xformxml, MvcApplication.XformSchemes[string.IsNullOrEmpty(selector)? "Gschema" : selector])
        {
            try
            {
                DefRender = new DextraLib.XForm.DefRender(this);
                this.XsltPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Files/Schemes/Xslt");

                this.DefRender.LangTrans = Langue.TranslateXform;
                this.DefRender.EnumTrans = Langue.EnumTranslateXform;
                //this.DefRender.langselector = lang;
                this.DefRender.getpath = Toolsspace.Tools.GetXformXmlComboPath;
            }
            catch (Exception e)
            {

            }
        }

        public void SetLang(string lang)
        {
            this.DefRender.langselector = lang;
        }

        public void ChangeRoot(string NewRoot, string NewRefroot = null, string selector = "Gschema")
        {
            base.ChangeRoot(MvcApplication.XformSchemes[selector], NewRoot, NewRefroot);
            this.DefRender.LangTrans = Langue.TranslateXform;
            this.DefRender.getpath = Toolsspace.Tools.GetXformXmlComboPath;
        }
    }

};