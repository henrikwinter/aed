using Dextra.Common;
using Dextra.Database;

using DextraLib.XForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Xapp.Db;
using Xapp.Models;
using Dextra.Xforms;

namespace Xapp.Controllers
{

    public class Mst_HupaOrggroup
    {
        public decimal? Id_Mst_Hupaorggroup { get; set; }
        public string Hupaorggroupname { get; set; }
        public Mst_HupaOrggroup()
        {

        }



    }

    [Authorize]
    public class QueryController : BaseController
    {
        string BackUrl = null;

        class Titlearr
        {

            public string Text { get; set; }
            public string Width { get; set; }
            public string Format { get; set; }
            public string Hidden { get; set; }
            public Titlearr(string val)
            {
                val = Regex.Replace(val, @"\r\n?|\n", "");
                val = val.Replace("{", "").Replace("}", "");

                string[] l = val.Split(',');
                foreach (string i in l)
                {
                    string[] ll = i.Split(':');
                    string key = ll[0].Replace("\"", "").Trim();
                    string value = ll[1].Replace("\"", "").Trim();
                    switch (key)
                    {
                        case "Text":
                            Text = value;
                            break;
                        case "Width":
                            Width = value;
                            break;
                        case "Format":
                            Format = value;
                            break;
                        case "Hidden":
                            Hidden = value;
                            break;

                        default:
                            break;
                    }

                }

            }


            public Titlearr(string text, string width, string format, string hidden)
            {
                Text = text;
                Width = width;
                Format = format;
                Hidden = hidden;
            }


        }
        public JsonResult InitQueryesData(string root,string formid)
        {

            Xform qxf = new Xform(root);

            string xformHtml = qxf.DefRender.Render(formid, "gschema");

            

            string[] rows = qxf.Elements[0].Appinfo["Tablerow"].Split('|');
            List<Titlearr> ta = new List<Titlearr>();
            foreach (string row in rows)
            {
                ta.Add(new Titlearr(row));
            }
            return Json(new { ta, xformHtml,root }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ListDivided_UserFromHupaOrggroup()
        {
            List<Mst_HupaOrggroup> b = new List<Mst_HupaOrggroup>();
            using (Dao<Mst_HupaOrggroup> d = new Dao<Mst_HupaOrggroup>(Sdb))
            {
                string sql = @"SELECT T.ID_MST_HUPAORGGROUP, T.HUPAORGGROUPNAME,'' as ORGGROUPDESC from  MST_HUPAORGGROUP T   WHERE  ( AccesHupa(:userid,T.ID_MST_HUPAORGGROUP ) > 0 )  order by T.HUPAORGGROUPNAME";
                d.SqlSelect(sql, new object[] { });
                if (d.Result.GetSate(0))
                {
                    b = d.Result.GetRes<Mst_HupaOrggroup>();
                }
            }

            return Json(new { Rows = b }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RunQuery()
        {

            List<Queryes> retval = new List<Queryes>();
            Xform qxf = new Xform(Request.Params);

            string QsQl = qxf.Elements[0].Appinfo["QuerySql"];
            QsQl = Regex.Replace(QsQl, @"\t\r\n?|\n", "");

            string SelectedOrgs = "";
            string SelectedOrg = "";
            try
            {
                SelectedOrgs = Request.Params["SelectedOrgs"];
                SelectedOrg = Request.Params["SelectedOrg"];
                string query = string.Format("select * from a_table where something in({0})", SelectedOrgs);
            }
            catch { }

            if (QsQl.IndexOf("in(orglist)") != -1)   //, StringComparison.OrdinalIgnoreCase
            {
                if (!string.IsNullOrEmpty(SelectedOrgs))
                {
                    string insql = " in (" + SelectedOrgs + ") ";
                    QsQl = QsQl.Replace("in(orglist)", insql);

                }
                else
                {
                    QsQl = QsQl.Replace("in(orglist)", "");
                }

            }


            //List<XformElement> inputs = qxf.Elements.FindAll(x => x.RenderType == "Input");
            List<SchemaWalkerResult> inputs = qxf.AllElements.FindAll(x => x.Itemtype == "Input");
            object[] sparams = new object[] { };

            foreach (SchemaWalkerResult elem in inputs)
            {
                if (elem.Value == null)
                {
                    elem.Value = "";
                }
                Array.Resize(ref sparams, sparams.Length + 1);
                sparams[sparams.Length - 1] = elem.Value;

            }

            using (Dao<Queryes> qda = new Dao<Queryes>(Sdb))
            {
                qda.SqlSelect(QsQl, sparams);
                if (qda.Result.GetSate(0))
                {
                    retval = qda.Result.GetRes<Queryes>();
                }
            }

            return Json(new { retval }, JsonRequestBehavior.AllowGet); ;
        }

        public ActionResult Queryes()
        {
            BackUrl = Request.UrlReferrer.OriginalString;
            QueryModel Qm = new QueryModel();
            Qm.BackUrl= Request.UrlReferrer.OriginalString;
            return View("Queryes", Qm);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ExportData()
        {
            string Mimetype = "image/jpg";
            var test = Request.Params;
            string a = Request.Params["content"];
            string fname = Request.Params["fname"];
            string d = Request.Params["__RequestVerificationToken"];
            byte[] data = Convert.FromBase64String(a);
            string contentType = MimeMapping.GetMimeMapping(fname);
            //byte[] array = Encoding.ASCII.GetBytes(a);
            //System.IO.File.WriteAllBytes(@"D:\chart.jpg", data);
            //return Json(new { a }, JsonRequestBehavior.AllowGet);
            return File(data, Mimetype);
        }

    }
}