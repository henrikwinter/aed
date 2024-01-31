using Xapp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Dextra.Toolsspace
{
    public class CustAttr
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string SqlType { get; set; }
        public string Comment { get; set; }
        public string Annotation { get; set; }
        public string Nullable { get; set; }

        public CustAttr()
        {

        }

        public CustAttr(string[] inp)
        {
            Name = string.IsNullOrEmpty(inp[0]) ? "Error" : Tools.InitCapitalConvert(inp[0]);
            string temptype= "string";
            try { temptype = string.IsNullOrEmpty(inp[1]) ? "string" : inp[1]; } catch { }
                
            if (Generator.conv.ContainsKey(temptype.ToLower()))
            {
                Type = Generator.conv[temptype.ToLower()];
            }
            else
            {
                Type = temptype.ToLower();
            }
            SqlType = Generator.sqlconv[Type.ToLower()];

            if (SqlType == "VARCHAR2")
            {
                try
                {
                    if (!string.IsNullOrEmpty(inp[2]))
                    {
                        SqlType += "(" + inp[2] + ")";
                        inp[2] = "";
                    } else
                    {
                        SqlType += "(50)";
                    }
                    
                }
                catch
                {
                    SqlType += "(50)";
                }

            }
            try { Nullable = string.IsNullOrEmpty(inp[2]) ? "" : "?"; } catch { Nullable = ""; }
            try { Comment = string.IsNullOrEmpty(inp[3]) ? "" : @" //" + inp[3]; } catch { Comment = ""; }
            try { Annotation = string.IsNullOrEmpty(inp[4]) ? "" : "[" + inp[4] + "]\r\n"; } catch { Annotation = ""; }
        }
    }
    public class CustEntity
    {
        public string Name { get; set; }
        public List<CustAttr> Attribs { get; set; }

        public string RenderedforClass { get; set; }
        public string RenderedforJs { get; set; }
        public string RenderedforJsCol { get; set; }
        public string RenderedforSql { get; set; }

        public string RenderedAjaxf { get; set; }
        public string RenderedJavascript { get; set; }
        public string RenderedHtml { get; set; }


        public CustEntity()
        {
            Attribs = new List<CustAttr>();
        }

        public CustEntity(string name)
        {
            Name = name;
            Attribs = new List<CustAttr>();
        }

    }

    public static class Generator
    {
        public const string Classtxt = @" public {0}()
        {{
        }}
        public {0} ShallowCopy()
        {{
            return ({0})this.MemberwiseClone();
        }}
        public {0}(HttpRequestBase rRequest)
        {{
            var type = this.GetType();
            foreach (string key in rRequest.Form.Keys)
            {{
                var property = type.GetProperty(key);
                if (property != null)
                {{
                    try
                    {{
                        var convertedValue = Convert.ChangeType(rRequest.Form[key], property.PropertyType);
                        property.SetValue(this, convertedValue);
                    }}
                    catch
                    {{
                    }}
                }}
            }}
        }}";

        public static Dictionary<string, string> conv = new Dictionary<string, string>()
            {
                {"string","string" },
                {"varchar","string" },
                {"varchar2","string" },
                {"decimal","decimal" },
                {"number","decimal" },
                {"date","DateTime" },
                {"datetime","DateTime" },
                {"bool","bool" },
                {"clob","string" }
            };
        public static Dictionary<string, string> sqlconv = new Dictionary<string, string>()
            {
                {"string","VARCHAR2" },
                {"decimal","NUMBER" },
                {"datetime","DATE" }
            };




        public static List<CustEntity> Genratorprc(string Entitydesc, string dir= "Work/")
        {

            string workpath = MvcApplication.GetPhysicalPath(dir);

            Entitydesc = Entitydesc.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace(" ", "");
            List<CustEntity> ent = new List<CustEntity>();

            MatchCollection mc = Regex.Matches(Entitydesc, @"\((.*?)\)");
            foreach (Match m in mc)
            {
                MatchCollection mc1 = Regex.Matches(Entitydesc, m.Value.Replace("(", @"\(").Replace(")", @"\)") + @"\{(.*?)\}");
                string enname = Tools.InitCapitalConvert(m.Value.Trim('(', ')'));
                CustEntity ce = new CustEntity(enname);
                ce.RenderedforClass = string.Format(@"public class {0}  {{", enname);
                ce.RenderedforJs = string.Format(@"var {0}_datafields = [", enname);
                ce.RenderedforJsCol = string.Format(@"var {0}_columns = [", enname);
                ce.RenderedforSql= string.Format(@"CREATE TABLE {0} (", enname);

                MatchCollection mc2 = Regex.Matches(mc1[0].Value, @"\[(.*?)\]");
                foreach (Match m2 in mc2)
                {
                    string[] attr = m2.Value.Trim('[', ']').Split(',');
                    CustAttr a = new CustAttr(attr);
                    ce.RenderedforClass += "\r\n" + Generator.renderClassproperty(a);
                    ce.RenderedforJs += "\r\n" + Generator.renderJsproperty(a);
                    ce.RenderedforJsCol += "\r\n" + Generator.renderJspropertyColumn(a);
                    ce.RenderedforSql += "\r\n" + Generator.renderSqlColumn(a);
                    ce.Attribs.Add(a);

                }

                GenattrSrc(enname, mc2);
                GenentSrc(enname);

                string w1 = string.Format(Generator.Classtxt, enname);
                ce.RenderedforClass += w1 + "\r\n}";
                ce.RenderedforSql = ce.RenderedforSql.Substring(0, ce.RenderedforSql.LastIndexOf(','))+ "\r\n)";
                ce.RenderedforJs = ce.RenderedforJs.Substring(0, ce.RenderedforJs.LastIndexOf(',')) + "\r\n];";
                ce.RenderedforJsCol = ce.RenderedforJsCol.Substring(0, ce.RenderedforJsCol.LastIndexOf(',')) + "\r\n];";

                try { Directory.CreateDirectory(MvcApplication.GetPhysicalPath(workpath + enname)); } catch (Exception ex) { }
                //System.IO.File.WriteAllText(workpath + enname+ "/"+enname + "_Class.cs", ce.RenderedforClass);
                System.IO.File.WriteAllText(workpath + enname + "/" + "Class.cs", ce.RenderedforClass);
                System.IO.File.WriteAllText(workpath + enname + "/"+  "CreateSql.sql", ce.RenderedforSql);

                System.IO.File.WriteAllText(workpath + enname + "/" + "CreateMsSql.sql", ce.RenderedforSql.Replace("VARCHAR2", "varchar").Replace("NUMBER", "decimal").Replace("DATE", "DateTime"));

                System.IO.File.WriteAllText(workpath + enname + "/" + "Js_Data.js", ce.RenderedforJs);
                System.IO.File.WriteAllText(workpath + enname + "/" + "Js_Column.js", ce.RenderedforJsCol);

                ent.Add(ce);

            }

            return ent;
        }

        static string[] Procfilename(string file)
        {
            string[] retval = { "", "" };
            FileInfo fi = new FileInfo(file);
            string ext = fi.Extension;
            string[] tok = ext.Split('_');
            retval[1] = "out";
            if (tok.Length == 3)
            {
                if (!string.IsNullOrEmpty(tok[2])) retval[1] = tok[2];
            }
            string result =Regex.Replace(Path.GetFileNameWithoutExtension(fi.Name), "Template", "", RegexOptions.IgnoreCase).TrimStart('_');

            retval[0] = result; // Path.GetFileNameWithoutExtension(fi.Name);
            return retval;

        }


        static void GenattrSrc(string enname, MatchCollection mc2, string dir = "Work/")
        {
            string workpath = MvcApplication.GetPhysicalPath(dir);
            string[] filePaths = Directory.GetFiles(workpath, "*.attr_template_*");
            string entpath = workpath + enname + "/";
            string Rendered = "";

            try { Directory.CreateDirectory(MvcApplication.GetPhysicalPath(entpath)); } catch (Exception ex) { }
            foreach (string file in filePaths)
            {
                Rendered = "";
                string[] f = Procfilename(file);
                foreach (Match m2 in mc2)
                {
                    string[] attr = m2.Value.Trim('[', ']').Split(',');
                    CustAttr inp = new CustAttr(attr);
                    string format = System.IO.File.ReadAllText(file);  // @"{4}public {1}{2} {0} {{ get; set; }} {3}";
                    Rendered+= string.Format(format, inp.Name, inp.Type, inp.Nullable, inp.Comment, inp.Annotation);
                }

                System.IO.File.WriteAllText(entpath + f[0] + "."+f[1], Rendered);
            }
        }
        static void  GenentSrc(string enname, string dir = "Work/")
        {
            string workpath = MvcApplication.GetPhysicalPath(dir);
            string[] filePaths = Directory.GetFiles(workpath, "*.ent_template_*");
            string entpath = workpath + enname + "/";
            try { Directory.CreateDirectory(MvcApplication.GetPhysicalPath(entpath)); } catch (Exception ex) { }
            foreach (string file in filePaths)
            {
                string[] f = Procfilename(file);
                string template = System.IO.File.ReadAllText(file);
                string src=template.Replace("§EnName§", enname);
                System.IO.File.WriteAllText(entpath + f[0] + "."+f[1], src);
            }
        }

        public static string renderClassproperty(CustAttr inp)
        {
            string format = @"{4}public {1}{2} {0} {{ get; set; }} {3}";
            return string.Format(format, inp.Name, inp.Type, inp.Nullable, inp.Comment, inp.Annotation);
        }
        public static string renderSqlColumn(CustAttr inp)
        {
            string format = @"{0}  {1},";
            return string.Format(format, inp.Name, inp.SqlType);
        }

        public static string renderJsproperty(CustAttr inp)
        {
            string format = @"  {{ name: '{0}', type: '{1}' }}, {2}";
            return string.Format(format, inp.Name, inp.Type, inp.Comment);
        }
        public static string renderJspropertyColumn(CustAttr inp)
        {
            string format = @"{{ text: '§{0}§', datafield: '{0}' }}, {1}";
            return string.Format(format, inp.Name, inp.Comment);
        }

    }
 
}