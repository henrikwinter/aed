using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace Dextra.Toolsspace
{
    public static class P
    {
        public static string LangDir { get; set; }
        public static string AltDir { get; set; }
        public static string c_XmlCodes { get; set; }

        public static string GalleryDir { get; set; }
        public static string GalleryRDir { get; set; }

        public static string BmFtpDir { get; set; }

        public static string PreferdLangDefault { get; set; }
        static P()
        {
            LangDir = "~/App_Data/Files/Xml";
            AltDir = "~/Vacak";
            c_XmlCodes = "~/Files/Xml/Codes";
            GalleryDir = "~/Gallery/";
            GalleryRDir = "Gallery";
            PreferdLangDefault = "hu"; // "Hu.Default.Default";
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                string fp = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Files/Xml"), "test.xml");
                xmlDoc.Load(fp);
                XmlNodeList itemNodes = xmlDoc.SelectNodes("/Var");
                foreach (XmlNode itemNode in itemNodes)
                {
                    XmlNode node = itemNode.SelectSingleNode("LangDir");
                    if ((node != null))
                    {
                        LangDir = node.InnerText;
                    }

                }

            } catch 
            {

            }




        }
    }
}