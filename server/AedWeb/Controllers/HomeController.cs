using Dextra.Common;
using Dextra.Database;
using Dextra.Flowbase;
using Xapp.Db;
using Xapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.IO;
using Dextra.Xforms;
using Dextra.Toolsspace;
using HtmlAgilityPack;
using System.Xml;
using DextraLib.Report;
using Dextra.Report;

namespace Xapp.Controllers
{

    
    
    public class HomeController : BaseController
    {


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Testproc()
        {

            string template = "proba.html"; // form["template"];
            template = Tools.Getpath(template, C.c_HtmlTemplate);

            TestDocument ds = new TestDocument(template, "", "", "");
            string curtempl = ds.Render();
            //curtempl = ds.Rendered;
            string retforHtml = ds.ProcTextAreas(curtempl, Request.Params); //, false);
            return Content(retforHtml);
        }

        public ActionResult ReportTest()
        {
            //TestDocument ds = new TestDocument(Tools.Getpath("proba.html", C.c_HtmlTemplate), "", "", "");
            string ret = "";//ds.Render("/Home/Testproc");

            return Content(ret);
        }

       // [dxAuthorize]
        public ActionResult Index()
        {
            // Langue.processlang();
            return View();
        }

        [dxAuthorize]
        public ActionResult DownloadDocsIndex(string path = "")
        {
            string dpath = Server.MapPath("~/App_Data/Files/Ftp/"+ViewBag.Appselector);
            string realPath;
            realPath = Server.MapPath("~/App_Data/Files/Ftp/" + ViewBag.Appselector+"/" + path);
            if (System.IO.Directory.Exists(realPath))
            {
                List<FileModel> fileListModel = new List<FileModel>();
                IEnumerable<string> fileList = Directory.EnumerateFiles(realPath, "*.doc").OrderByDescending(filename => filename); 
                foreach (string file in fileList)
                {
                    FileInfo f = new FileInfo(file);
                    FileModel fileModel = new FileModel();
                    try
                    {
                        FileInfo fn = new FileInfo(file.Replace(".doc", ".nfo"));

                        StreamReader sr = fn.OpenText();
                        string s = "";
                        while ((s = sr.ReadLine()) != null)
                        {
                            fileModel.FileComment = MvcHtmlString.Create(fileModel.FileComment + s);
                        }
                        sr.Close();
                    }
                    catch { }



                    fileModel.FileName = Path.GetFileName(file);
                    fileModel.FilePath = file;
                    fileModel.FileAccessed = f.LastAccessTime;
                    fileModel.FileSizeText = (f.Length < 1024) ? f.Length.ToString() + " B" : f.Length / 1024 + " KB";
                    fileListModel.Add(fileModel);

                }
                //fileListModel = (List<FileModel>)from str in fileListModel orderby str.FileName descending select str;
                ExplorerModel explorerModel = new ExplorerModel(null, fileListModel);
                return View(explorerModel);
            } else
            {
                return Content(path + " is not a valid file or directory.");
            }

        }

        [dxAuthorize]
        public ActionResult DownloadsIndex(string path="")
        {

            string dpath = Server.MapPath("~/App_Data/Files/Ftp/" + ViewBag.Appselector);
            string realPath;
            realPath = Server.MapPath("~/App_Data/Files/Ftp/" + ViewBag.Appselector + "/" + path);


            //realPath = @"C:\Work\DotNetDevelope\Dextra\Xszenyor\Xszenyor\App_Data\" + path;


            // or realPath = "FullPath of the folder on server" 

            if (System.IO.File.Exists(realPath))
            {
                return base.File(realPath, "application/octet-stream");
            }
            else if (System.IO.Directory.Exists(realPath))
            {

                Uri url = Request.Url;
                //Every path needs to end with slash
                if (url.ToString().Last() != '/')
                {
                    //Response.Redirect("/DownloadsIndex/" + path + "/");
                }

                List<FileModel> fileListModel = new List<FileModel>();

                List<DirModel> dirListModel = new List<DirModel>();

                IEnumerable<string> dirList = Directory.EnumerateDirectories(realPath);
                foreach (string dir in dirList)
                {
                    DirectoryInfo d = new DirectoryInfo(dir);

                    DirModel dirModel = new DirModel();

                    dirModel.DirName = Path.GetFileName(dir);
                    dirModel.DirPath =dir;
                    dirModel.DirPath = dirModel.DirPath.Replace(dpath, "");
                    dirModel.DirAccessed = d.LastAccessTime;

                    dirListModel.Add(dirModel);
                }

                IEnumerable<string> fileList = Directory.EnumerateFiles(realPath,"*.zip").OrderByDescending(filename => filename); 
                foreach (string file in fileList)
                {
                    FileInfo f = new FileInfo(file);
                    FileModel fileModel = new FileModel();
                    try
                    {
                        FileInfo fn = new FileInfo(file.Replace(".zip", ".nfo"));

                        StreamReader sr = fn.OpenText();
                        string s = "";
                        while ((s = sr.ReadLine()) != null)
                        {
                            fileModel.FileComment = MvcHtmlString.Create(fileModel.FileComment + s);
                        }
                        sr.Close();
                    }
                    catch { }

             

                    fileModel.FileName = Path.GetFileName(file);
                    fileModel.FilePath = file;
                    fileModel.FileAccessed = f.LastAccessTime;
                    fileModel.FileSizeText = (f.Length < 1024) ? f.Length.ToString() + " B" : f.Length / 1024 + " KB";
                    fileListModel.Add(fileModel);

                }

                
                //var sort = from str in fileListModel orderby str.FileName descending select str;

                ExplorerModel explorerModel = new ExplorerModel(dirListModel, fileListModel);
                explorerModel.Currentdirectory = realPath.Replace(dpath, "");
                
                DirectoryInfo parentDir = Directory.GetParent(realPath);
                explorerModel.Parentpath = parentDir.FullName.Replace(dpath,"");

                return View(explorerModel);
            }
            else
            {
                return Content(path + " is not a valid file or directory.");
            }


            //return View();
        }

        public ActionResult Download(string file)
        {
            if (!System.IO.File.Exists(file))
            {
                return HttpNotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(file);
            var response = new FileContentResult(fileBytes, "application/octet-stream")
            {
                FileDownloadName = Path.GetFileName(file)
            };
            return response;
        }


        public ActionResult AccesDenied()
        {
            ViewBag.Message = "";
            return View();
        }

        [dxAuthorize]
        public ActionResult FunctionDash()
        {
            ViewBag.Message = "";
            DxFunctionManager FuncMan = new DxFunctionManager(Sdb, Rolemanager.AvailableRoles, Rolemanager.CurrentUser);
            
            //if (!FuncMan.isAcces("FunctionDash")) return RedirectToAction("AccesDenied", "Home");


            FunctionViewModel model = new FunctionViewModel();
            model.Startableflows = FuncMan.StartableFunctions();


            int maxrocount = 8;
            string rendered = @"";
            
            foreach (RenderSartableFunctions fi in model.Startableflows)
            {
                int fcount = 0;
                rendered += @"<div class=""d-flex flex-row justify-content-center"">"+ Langue.Translate(fi.Groupname, "FunctionNamesGroup", Preflang)+"</div>";
                rendered += @"<div class=""d-flex flex-row justify-content-center"">";
                foreach (Functions fitem in fi.startables)
                {
                    decimal color;
                    string border = fitem.Flag;
                    try
                    {
                        //color = decimal.Parse(fitem.Flag.Replace("#", ""));
                        color =int.Parse(fitem.Flag.Replace("#", ""), System.Globalization.NumberStyles.HexNumber);
                        int  bcolor = (int)(color - (color / 2));
                        border ="#"+ bcolor.ToString();
                       // border= "red";

                    } catch (Exception e)
                    {
                        var t = "";
                    }
                    

                    //rendered += string.Format(@"<div class=""appicons1 rounded"" style=""background-color:{0};border:2px solid {1};"">", fitem.Flag,border);
                    rendered += string.Format(@"<div class=""appicons1 rounded"" style=""background-color:{0};"">", fitem.Flag);
                    rendered += string.Format(@"<a href=""{2}{0}/{1}"" class="""">", fitem.Controller, fitem.Action, Url.Content("~/"));
                    rendered += string.Format(@"<img src=""{1}Content/icons/{0}"" alt=""..."">", fitem.Param, Url.Content("~/"));
                    rendered += string.Format(@"</a>", 1);
                    rendered += string.Format(@" <p>{0}</p>", Langue.Translate(fitem.FunctioDesc, "FunctionNames", Preflang));
                    rendered += string.Format(@"</div>", 1);
                    fcount++;
                    if(fcount % maxrocount == 0)
                    {
                        rendered += @"</div>";
                        rendered += @"<div class=""d-flex flex-row justify-content-center"">";
                    }
                }
                int row = fi.startables.Count / maxrocount;
                if (row == 0) row = 1;
                int plus = (row * maxrocount) -fi.startables.Count ;
                for (int i = 0; i < plus; i++)
                {
                    rendered += string.Format(@"<div class=""appicons1e rounded"" style="""">", "white");
                    //rendered += string.Format(@"<a href=""{2}{0}/{1}"" class="""">", "", "", Url.Content("~/"));
                    rendered += string.Format(@"<img src=""{1}Content/icons/{0}"" alt=""..."">", "Generic.ico", Url.Content("~/"));
                    //rendered += string.Format(@"</a>", 1);
                    //rendered += string.Format(@" <p>{0}</p>", Langue.Translate("emptytxt", "FunctionNames", Preflang));
                    rendered += string.Format(@" <p>{0}</p>","");
                    rendered += string.Format(@"</div>", 1);
                }
                rendered += @"</div>";
            }

            model.Rendered = new MvcHtmlString(rendered);
            return View(model);

        }

        [dxAuthorize]
        public ActionResult SchedulerDash()
        {
            ViewBag.Message = "";
            return View();
        }

        [dxAuthorize]
        public ActionResult ReportDash()
        {
            ViewBag.Message = "";
            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult SessionExpire()
        {
            ViewBag.Message = "";
            return View();
        }

        [Authorize]
        public ActionResult ReloadAccesData()
        {
            BackUrl = Request.UrlReferrer.OriginalString;
            MvcApplication.LoadRoles();
            Rolemanager = new DxRoleManager(Sdb, Rolemanager.CurrentUser);
            return Redirect(BackUrl);
        }

        [Authorize]
        public ActionResult ReloadSchema()
        {
            BackUrl = Request.UrlReferrer.OriginalString;
            MvcApplication.LoadGlobalSchema();
            return Redirect(BackUrl);
        }


        public ActionResult ChangeLang(string lang)
        {
            BackUrl = Request.UrlReferrer.OriginalString;
            //string[] temp = Preflang.Split('.');
            Preflang = lang; // + "." + temp[1] + "." + temp[2];
            UserPref.Preflang = lang;
            Sdb.CurrentLang = Preflang;
            ViewBag.Preferedlang= Preflang;
            MvcApplication.SaveLangDictionary();
            MvcApplication.LoadLangDictionary(null, true);

            MvcApplication.SaveEnumDictionary();
            MvcApplication.LoadEnumDictionary(null, true);

            return Redirect(BackUrl);
        }


        public JsonResult ChangeLangA(string lang)
        {

            return Json(new { ret ="" }, JsonRequestBehavior.AllowGet);
        }


    }
}