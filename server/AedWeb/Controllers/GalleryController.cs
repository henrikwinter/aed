using Dextra.Common;
using Dextra.Database;
using Dextra.Toolsspace;
using DextraLib.GeneralDao;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.Serialization;
using Xapp.Db;
using Xapp.Models;

namespace Xapp.Controllers
{

    public class Imfile
    {
        public string root { get; set; }
        public string filename { get; set; }
        public string filenameNoExtension { get; set; }
        public string extension { get; set; }
        public string origname { get; set; }

        public Imfile(string path)
        {
            try
            {
                extension = Path.GetExtension(path);
                filename = Path.GetFileName(path);
                filenameNoExtension = Path.GetFileNameWithoutExtension(path);
                root = Path.GetPathRoot(path);
                origname = path;

            }
            catch { }
        }
    }


    public static class GalleryHelper
    {


        public static string BuildDatastring(GalleryElement e)
        {
            if (e.UsersAccesDescribe == null) e.UsersAccesDescribe = MakeAllUsersDescribeString();
            return e.Filename + "|" + e.UsersAccesDescribe + "|" + e.Title + "|" + e.ThumbnailFielname + "|" + e.Description + "|" + e.Flag + "|" + e.Property + "|\r\n";
        }
        public static string getToken(string[] t, int i, string def)
        {
            if (t.ElementAtOrDefault(i) == null)
            {
                return def;
            }
            return t[i];

        }
        public static GalleryElement GetDatafromdatastring(string datastring, int? i = null)
        {
            GalleryElement retv = new GalleryElement();

            string[] tokens = new string[] { null };
            tokens = datastring.Split(new[] { "|" }, StringSplitOptions.None);
            retv.Id = i;
            retv.Filename = getToken(tokens, 0, "error");
            retv.UsersAccesDescribe = getToken(tokens, 1, MakeAllUsersDescribeString());
            retv.Title = getToken(tokens, 2, "Title");
            retv.ThumbnailFielname = getToken(tokens, 3, retv.Filename);
            retv.Description = getToken(tokens, 4, "Description");
            retv.Flag = getToken(tokens, 5, "");
            retv.Property = getToken(tokens, 6, "");

            return retv;
        }
        public static string MakeAllUsersDescribeString(string cuser = null)
        {
            //ApplicationDbContext context = new ApplicationDbContext();
            List<ApplicationUser> users = new List<ApplicationUser>();// context.Users.ToList();

            List<Vw_Users> temp = new List<Vw_Users>();
            Dao<Vw_Users> aludao = new Dao<Vw_Users>(MvcApplication.Adb);
            aludao.SqlSelect(@"SELECT * from VW_USERs t", new object[] { });
            if (aludao.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                temp = aludao.Result.GetRes<Vw_Users>();
            }
            foreach(Vw_Users item in temp)
            {
                ApplicationUser a = new ApplicationUser();
                a.UserName = item.Username;
                users.Add(a);
            }


            // UsersContext u = new UsersContext();
            //List<UserProfile> users = u.UserProfiles.ToList();
            string avusers = "";

            foreach (ApplicationUser i in users)
            {
                if (i.UserName == cuser)
                {
                    avusers += "+" + i.UserName + ".";
                }
                else
                {
                    avusers += "-" + i.UserName + ".";
                }

            }
            return avusers;
        }
        public static bool FindImageInDatfile(List<string> list, ref GalleryElement e)
        {
            if (list == null) return false;
            foreach (string l in list)
            {
                string[] t = l.Split(new[] { "|" }, StringSplitOptions.None);
                if (t[0].IndexOf(e.Filename) >= 0)
                {
                    e = GetDatafromdatastring(l);
                    return true;
                }
            }
            return false;
        }
        public static bool CheckAccesUser(string userID, string UsersAccesDescribe)
        {
            if (userID.Equals("Sfhw"))
            {
                return true;
            }
            if (UsersAccesDescribe.IndexOf("+All.") >= 0 && UsersAccesDescribe.IndexOf("-" + userID + ".") < 0)
            {
                return true;
            }

            if (UsersAccesDescribe.IndexOf("-All.") >= 0 && UsersAccesDescribe.IndexOf("+" + userID + ".") >= 0)
            {
                return true;
            }


            if (UsersAccesDescribe.IndexOf("+" + userID + ".") >= 0)
            {
                return true;
            }
            return false;
        }

        public static bool WriteGdat(List<string> Gdat, string filename)
        {
            FileStream fsOverwrite = new FileStream(filename, FileMode.Create);
            //using (StreamWriter sw = new StreamWriter(filename, false))
            using (StreamWriter sw = new StreamWriter(fsOverwrite))
            {
                foreach (string s in Gdat)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        sw.WriteLine(s.TrimEnd('\r', '\n'));
                    }
                }
            }
            return true;
        }

        public static void MakeThumbnail(GalleryElement ge, string GalleryPath)
        {
            string thumbnailFolder = GalleryPath + "\\th";
            Imfile img = new Imfile(GalleryPath + "\\" + ge.ThumbnailFielname);
            if (img != null && System.IO.File.Exists(img.origname))
            {
                using (FileStream fs = new FileStream(img.origname, FileMode.Open, FileAccess.Read))
                {
                    try
                    {
                        Image origImage = Image.FromStream(fs);
                        var thumbnail = origImage.GetThumbnailImage(90, 120, null, IntPtr.Zero);
                        thumbnail.Save(thumbnailFolder + "\\" + ge.ThumbnailFielname);
                        thumbnail.Dispose();
                        origImage.Dispose();

                    }
                    catch { }
                }
            }

        }
        public static List<string> ReadGdat(string file)
        {
            if (System.IO.File.Exists(file))
            {
                return System.IO.File.ReadAllLines(file).ToList();
            }
            return new List<string>();
        }

        public static void WalkDirectoryTree(System.IO.DirectoryInfo root, ref List<FileInfo> files)
        {
            System.IO.DirectoryInfo[] subDirs = null;

            // First, process all the files directly under this folder
            try
            {
                List<FileInfo> t = root.GetFiles("gdat.txt").ToList<FileInfo>();
                if (t.Count > 0)
                {
                    files.AddRange(t);
                }

            }
            catch (UnauthorizedAccessException e)
            {
                //log.Add(e.Message);
            }

            catch (System.IO.DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            if (files != null)
            {
                //foreach (System.IO.FileInfo fi in files)
                //{
                //Console.WriteLine(fi.FullName);
                //}

                // Now find all the subdirectories under this directory.
                subDirs = root.GetDirectories();

                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory.
                    WalkDirectoryTree(dirInfo, ref files);
                }
            }

        }


    }
    public class GalleryElement
    {

        public int? Id { get; set; }
        public string Filename { get; set; }

        public string UsersAccesDescribe { get; set; }

        public string Title { get; set; }
        public string ThumbnailFielname { get; set; }

        public string Description { get; set; }
        public string Flag { get; set; }

        public string Property { get; set; }
        public string MimeGroup { get; set; }
        public string Filenameo { get; set; }
        public bool Accesable { get; set; }

        public GalleryElement(string filename)
        {
            Filename = filename;
            Title = Path.GetFileNameWithoutExtension(filename);
            Description = "";
            ThumbnailFielname = filename;
            Flag = "";
            Property = "";
        }

        public GalleryElement(string filename, ref List<string> lines, string Userid)
        {
            bool isVideo = false;
            MimeGroup = "immage";
            string mimeType = System.Web.MimeMapping.GetMimeMapping(filename);
            if (mimeType.StartsWith("video", StringComparison.OrdinalIgnoreCase))
            {
                isVideo = true;
                MimeGroup = "video";
            }
            Filename = filename;
            Title = Path.GetFileNameWithoutExtension(filename);
            Filenameo = Path.GetFileNameWithoutExtension(filename);
            Description = Filenameo;
            ThumbnailFielname = filename;
            if (isVideo) ThumbnailFielname = Path.GetFileNameWithoutExtension(filename) + ".jpg";


            Flag = "";
            Property = "";
            UsersAccesDescribe = GalleryHelper.MakeAllUsersDescribeString();
            GalleryElement t = this;
            if (GalleryHelper.FindImageInDatfile(lines, ref t))
            {
                this.Filename = t.Filename;
                this.UsersAccesDescribe = t.UsersAccesDescribe;
                this.Title = t.Title;
                this.Description = t.Description;
                this.ThumbnailFielname = t.ThumbnailFielname;
                if (isVideo) ThumbnailFielname = Path.GetFileNameWithoutExtension(t.ThumbnailFielname) + ".jpg";
                this.Description = t.Description;
                this.Flag = t.Flag;
                this.Property = t.Property;

            }
            else
            {
                lines.Add(GalleryHelper.BuildDatastring(this));
            }
            Accesable = GalleryHelper.CheckAccesUser(Userid, UsersAccesDescribe);
        }

        public GalleryElement()
        {

        }
        public GalleryElement(int id, string name, string title, string descript, string users, string property, string flag)
        {
            Id = id;
            Filename = name;
            Title = title;
            Description = descript;
            UsersAccesDescribe = users;
            Property = property;
            Flag = flag;
        }

    }
    public class DxGallery
    {
        public List<GalleryElement> Galeryelemets { get; set; }
        public string GalleryName { get; set; }
        public string GalleryPath { get; set; }
        public string GalleryColor { get; set; }
        public string GalleryPathView { get; set; }
        public string CurUserId { get; set; }
        public string Folderpic { get; set; }
        public bool isEmpty { get; set; }

        public bool Admin { get; set; }
        public string GalleryTheme { get; set; }

        public List<string> Gdat = new List<string>();
        public DxGallery()
        {

        }
        public DxGallery(string galerycolor, string galleryname)
        {
            GalleryColor = galerycolor;
            
            GalleryPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(P.GalleryDir + galerycolor), galleryname);
            GalleryPathView = P.GalleryRDir + "/" + galerycolor + "/" + galleryname + "/";
            GalleryName = galleryname;
            isEmpty = false;

        }

        public DxGallery(string galerycolor, string galleryname, string userID)
        {
            Galeryelemets = new List<GalleryElement>();
            GalleryColor = galerycolor;
            GalleryPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(P.GalleryDir + galerycolor), galleryname);
            GalleryPathView = P.GalleryRDir+"/" + galerycolor + "/" + galleryname + "/";
            GalleryName = galleryname;
            isEmpty = false;


            Gdat = GalleryHelper.ReadGdat(GalleryPath + "\\gdat.txt");

            string thumbnailFolder = GalleryPath + "\\th";
            if (!Directory.Exists(thumbnailFolder))
            {
                Directory.CreateDirectory(thumbnailFolder);
            }

            CurUserId = userID;

            var set = new HashSet<string> { ".png", ".jpg", ".bmp" };

            if (galerycolor.StartsWith("video", StringComparison.OrdinalIgnoreCase))
            {
                set = new HashSet<string> { ".mp4", ".flv", ".ogv", ".webm" };
            }

            List<string> ListOfMedia = new List<string>(Directory.GetFiles(GalleryPath, "*.*", SearchOption.TopDirectoryOnly).Where(f => set.Contains(new FileInfo(f).Extension, StringComparer.OrdinalIgnoreCase)).OrderBy(f => new FileInfo(f).Name));
            foreach (string file in ListOfMedia)
            {
                string filename = Path.GetFileName(file);
                GalleryElement ge = new GalleryElement(filename, ref Gdat, userID);
                if (ge.Accesable)
                {
                    Galeryelemets.Add(ge);
                }
                if (!System.IO.File.Exists(thumbnailFolder + "\\" + ge.ThumbnailFielname))
                {
                    GalleryHelper.MakeThumbnail(ge, GalleryPath);
                }


            }

            foreach (string l in Gdat.ToList())
            {
                GalleryElement x = GalleryHelper.GetDatafromdatastring(l);
                if (!System.IO.File.Exists(GalleryPath + "\\" + x.Filename))
                {
                    Gdat.Remove(l);
                    System.IO.File.Delete(GalleryPath + "\\th\\" + x.Filename);
                }
            }

            GalleryHelper.WriteGdat(Gdat, GalleryPath + "\\gdat.txt");
            if (Galeryelemets.Count == 0)
            {
                Galeryelemets.Add(new GalleryElement("emptygallery.jpg"));
                isEmpty = true;
            }
        }

    }
    public class Galleryes
    {
        public string Gname { get; set; }
        public bool Admin { get; set; }
        public List<DxGallery> gallerys { get; set; }


        public Galleryes(string galleryColor, string UserName, bool onlydir = false)
        {

            gallerys = new List<DxGallery>();
            try
            {
                string curpath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(P.GalleryDir), galleryColor);
                if ((System.IO.File.GetAttributes(curpath) & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint)
                {
                    foreach (string ffolder in Directory.GetDirectories(curpath))
                    {
                        string folder = Path.GetFileName(ffolder);
                        DxGallery cgallery = new DxGallery();
                        if (onlydir)
                        {
                            cgallery = new DxGallery(galleryColor, folder);
                        }
                        else
                        {
                            cgallery = new DxGallery(galleryColor, folder, UserName);
                            if (cgallery.isEmpty)
                            {
                                cgallery.GalleryPathView = P.GalleryRDir+"/";
                            }

                            if (cgallery.Galeryelemets[0].MimeGroup == "video")
                            {
                                cgallery.Folderpic = "folder.jpg";
                            }
                            else
                            {
                                cgallery.Folderpic = cgallery.Galeryelemets[0].Filename;
                            }

                        }
                        gallerys.Add(cgallery);
                    }
                }
            }
            catch (UnauthorizedAccessException) { }
            {

            }

        }
    }

    public class GalleryController : BaseController
    {

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            
            List<Vw_Users> temuser = GetAllUsers();
            List<ApplicationUser> users = new List<ApplicationUser>();
            foreach(Vw_Users u in temuser)
            {
                ApplicationUser a = new ApplicationUser();
                a.UserName = u.Username;
                users.Add(a);
            }
            var aSerializer = new XmlSerializer(typeof(ApplicationUser));
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
          //  aSerializer.Serialize(sw, users); // pass an instance of A
           // string xmlResult = sw.GetStringBuilder().ToString();
            


        }


        public ActionResult Index()
        {
            Galleryes gcolor = new Galleryes("Black", User.Identity.Name);
            return View(gcolor);
        }
        public ActionResult Galleryes(string galleryColor = "Black")
        {
            Galleryes gcolor = new Galleryes(galleryColor, User.Identity.Name);
            gcolor.Gname = galleryColor;
            gcolor.Admin=Rolemanager.Roles.Contains("AdminRole");

            return View(gcolor);
        }
      //  public ActionResult Gallery(string color, string folder)
      //  {
      //      DxGallery cgallery = new DxGallery(color, folder, User.Identity.Name);
      //      return View(cgallery);
      // }

        public ActionResult Gallery(string color, string folder,string theme)
        {
            DxGallery cgallery = new DxGallery(color, folder, User.Identity.Name);
            cgallery.GalleryTheme = theme;
            cgallery.Admin= Rolemanager.Roles.Contains("AdminRole");
            return View(cgallery);
        }

        public ActionResult Szenyorvideo(string color, string folder)
        {

            DxGallery cgallery = new DxGallery("Video", "Szenyorvideo", User.Identity.Name);
            return View(cgallery);
        }


        public class Adminmodel
        {
            public List<ApplicationUser> Users { get; set; }
            public List<string> Accesmode { get; set; }
            public Adminmodel()
            {
                //UsersContext u = new UsersContext();
                //ApplicationDbContext context = new ApplicationDbContext();
                Users = new List<ApplicationUser>();// context.Users.ToList();

                List<Vw_Users> temp = new List<Vw_Users>();
                Dao<Vw_Users> aludao = new Dao<Vw_Users>(MvcApplication.Adb);
                aludao.SqlSelect(@"SELECT * from VW_USERs t", new object[] { });
                if (aludao.Result.GetSate(DaoResult.ResCountOneOrMore))
                {
                    //Users = aludao.Result.GetRes<ApplicationUser>();
                    temp = aludao.Result.GetRes<Vw_Users>();
                }

                foreach (Vw_Users item in temp)
                {
                    ApplicationUser a = new ApplicationUser();
                    a.UserName = item.Username;
                    Users.Add(a);
                }

                //Users = u.UserProfiles.ToList();
                Accesmode = new List<string>();
                Accesmode.Add("+");
                Accesmode.Add("-");
                Accesmode.Add(" ");
            }

        }

        [Authorize(Users = "Sfhw")]
        public ActionResult AdminGaleryes()
        {
            Adminmodel am = new Adminmodel();
            return View(am);
        }

        public JsonResult NewUser(string userid, string password)
        {
            // WebSecurity.CreateUserAndAccount(userid, password);
            return Json(new { Rows = "", Error = "" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeletUser(string userid)
        {
            // ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(userid); // deletes record from webpages_Membership table
            //  ((SimpleMembershipProvider)Membership.Provider).DeleteUser(userid, true);

            //Membership.DeleteUser(userid);
            return Json(new { Rows = "", Error = "" }, JsonRequestBehavior.AllowGet);
        }



        class Galleryeshier
        {
            public int Id { get; set; }
            public int? Id_Parent { get; set; }
            public string Name { get; set; }
            public Galleryeshier(int i, int? p, string n)
            {
                Id = i;
                Id_Parent = p;
                Name = n;
            }

        }

        [HttpPost]
        public JsonResult Set_Gallerydata(List<GalleryElement> data, string path)
        {
            string GalleryPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/" + path)) + "\\gdat.txt";

            List<string> Gdata = new List<string>();
            foreach (GalleryElement i in data)
            {
                string l = GalleryHelper.BuildDatastring(i);
                Gdata.Add(l);
            }
            GalleryHelper.WriteGdat(Gdata, GalleryPath);

            return Json(new { data = "" }, "text/html");
        }
        public JsonResult Get_Gallerydata(string path)
        {
            string GalleryPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/"), path);
            List<GalleryElement> gdata = new List<GalleryElement>();
            List<string> Gdat = new List<string>();

            if (System.IO.File.Exists(GalleryPath + "\\gdat.txt"))
            {
                Gdat = GalleryHelper.ReadGdat(GalleryPath + "\\gdat.txt");
                int id = 0;
                foreach (string line in Gdat)
                {
                    gdata.Add(GalleryHelper.GetDatafromdatastring(line, id));
                    id++;
                }
            }
            return Json(new { Rows = gdata, Error = "" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Get_Gallery_Hierarchy()
        {
            List<Galleryeshier> gdata = new List<Galleryeshier>();

            int id = 0;
            int? id_parent = null;

            gdata.Add(new Galleryeshier(0, null, "Gallery"));
            gdata.Add(new Galleryeshier(1, 0, "Black"));
            gdata.Add(new Galleryeshier(2, 0, "Red"));
            gdata.Add(new Galleryeshier(3, 0, "Blue"));
            gdata.Add(new Galleryeshier(4, 0, "Green"));
            gdata.Add(new Galleryeshier(5, 0, "Yellow"));
            gdata.Add(new Galleryeshier(6, 0, "Video"));

            id = 10;
            Galleryes g = new Galleryes("Black", User.Identity.Name, true);
            foreach (DxGallery item in g.gallerys)
            {
                gdata.Add(new Galleryeshier(id, 1, item.GalleryName));
                id++;
            }

            g = new Galleryes("Red", User.Identity.Name, true);
            foreach (DxGallery item in g.gallerys)
            {
                gdata.Add(new Galleryeshier(id, 2, item.GalleryName));
                id++;
            }
            g = new Galleryes("Blue", User.Identity.Name, true);
            foreach (DxGallery item in g.gallerys)
            {
                gdata.Add(new Galleryeshier(id, 3, item.GalleryName));
                id++;
            }
            g = new Galleryes("Green", User.Identity.Name, true);
            foreach (DxGallery item in g.gallerys)
            {
                gdata.Add(new Galleryeshier(id, 4, item.GalleryName));
                id++;
            }
            g = new Galleryes("Yellow", User.Identity.Name, true);
            foreach (DxGallery item in g.gallerys)
            {
                gdata.Add(new Galleryeshier(id, 5, item.GalleryName));
                id++;
            }
            g = new Galleryes("Video", User.Identity.Name, true);
            foreach (DxGallery item in g.gallerys)
            {
                gdata.Add(new Galleryeshier(id, 6, item.GalleryName));
                id++;
            }

            return Json(new { Rows = gdata, Error = "" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SetAcces(string path, string ustr, string oper = "+")
        {
            string GalleryPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/"), path);
            DirectoryInfo di = new DirectoryInfo(GalleryPath);
            List<FileInfo> files = new List<FileInfo>();
            GalleryHelper.WalkDirectoryTree(di, ref files);

            List<string> Gdat = new List<string>();
            List<string> NewGdat = new List<string>();
            foreach (FileInfo fi in files)
            {
                NewGdat.Clear();
                Gdat = GalleryHelper.ReadGdat(fi.FullName);
                int i = 0;
                foreach (string l in Gdat)
                {
                    GalleryElement e = GalleryHelper.GetDatafromdatastring(l);
                    string orig = e.UsersAccesDescribe;
                    string origu = e.UsersAccesDescribe;
                    if (origu.Contains("-" + ustr + "."))
                    {
                        origu = origu.Replace("-" + ustr + ".", "");
                    }
                    if (origu.Contains("+" + ustr + "."))
                    {
                        origu = origu.Replace("+" + ustr + ".", "");
                    }
                    string newu = origu + oper + ustr + ".";
                    string l1 = "";
                    if (oper == " ")
                    {
                        l1 = l.Replace(orig, origu);
                    }
                    else
                    {
                        l1 = l.Replace(orig, newu);
                    }

                    NewGdat.Add(l1);
                    i++;
                }
                GalleryHelper.WriteGdat(NewGdat, fi.FullName);
                var a = "";
            }
            return Json(new { Rows = "", Error = "" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DelGalleryfolder(string path)
        {
            string GalleryPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/"), path);
            Directory.Delete(GalleryPath);
            return Json(new { Rows = "", Error = "" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DelGalleryPicture(string path)
        {
            string GalleryPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/"), path);
            System.IO.File.Delete(GalleryPath);
            return Json(new { Rows = "", Error = "" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddGalleryfolder(string path, string name)
        {
            string GalleryPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/"), path);
            if (!Directory.Exists(GalleryPath + "\\" + name))
            {
                Directory.CreateDirectory(GalleryPath + "\\" + name);
            }
            return Json(new { Rows = "", Error = "" }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult UploadImg()
        {
            try
            {

                if (Request.Files.Count > 0)
                {
                    try
                    {
                        var postedFile = Request.Files[0];
                        var image = new WebImage(postedFile.InputStream)
                        {
                            //FileName = postedFile.FileName;
                            FileName = Request.Params["origFileName"]
                        };
                        PrcUploadImg(image);
                    }
                    catch
                    {
                        // The user uploaded a file that wasn't an image or an image format that we don't understand
                        return Json(new { isUploaded = false, message = "File uploaded Error!", newid = 0 });
                    }

                }

                //PrcUploadImg();
                return Json(new { isUploaded = true, message = "File uploaded successfully!" });

            }
            catch
            {
                return Json(new { isUploaded = false, message = "File uploaded Error!", newid = 0 });
            }
        }
        public void PrcUploadImg(WebImage image)
        {



            //WebImage image = WebImage.GetImageFromRequest("img");
            //WebImage image = new WebImage(Request.InputStream);
            //Ufiles tu = null;
            if (image != null)
            {
                // image = CorpPicture(Request, image);
                string mimeType = Request.Files[0].ContentType;
                byte[] fileData = image.GetBytes();
                int fileLength = fileData.Length;
                decimal? Id_Ufiles = null;
                string title = "no";
                string filename = "no";
                string galerycolor = "no";
                string galleryname = "no";

                try
                {
                    title = Request.Form["Titles"];
                    galerycolor = Request.Form["galerycolor"];
                    galleryname = Request.Form["galleryname"];
                    //Id_Ufiles = decimal.Parse(Request.Form["Id_Ufiles"]);
                }
                catch { }
                //  tu = new Ufiles(Id_Ufiles, "Facepic", "FACEPIC", mimeType, fileLength, fileData, "");
                string GalleryPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(P.GalleryDir + galerycolor), galleryname);
                image.Save(GalleryPath + "\\" + image.FileName,null,false);
                string usercontrol = GalleryHelper.MakeAllUsersDescribeString(User.Identity.Name);
                GalleryElement e = new GalleryElement(0, Path.GetFileName(image.FileName), title, "uploaded", usercontrol, "", "");
                List<string> gdat = GalleryHelper.ReadGdat(GalleryPath + "\\gdat.txt");
                gdat.Add(GalleryHelper.BuildDatastring(e));
                GalleryHelper.WriteGdat(gdat, GalleryPath + "\\gdat.txt");
                string thumbnailFolder = GalleryPath + "\\th";
                Imfile img = new Imfile(image.FileName);
                if (img != null && System.IO.File.Exists(img.origname))
                {
                    using (FileStream fs = new FileStream(img.origname, FileMode.Open, FileAccess.Read))
                    {
                        try
                        {
                            Image origImage = Image.FromStream(fs);
                            var thumbnail = origImage.GetThumbnailImage(90, 120, null, IntPtr.Zero);
                            thumbnail.Save(thumbnailFolder + "\\" + Path.GetFileName(image.FileName));
                            thumbnail.Dispose();
                            origImage.Dispose();

                        }
                        catch { }
                    }
                }


            }
            //return tu;
        }
        public static WebImage CorpPicture(HttpRequestBase Request, WebImage image)
        {
            double height = image.Height;
            double width = image.Width;

            double wW = int.Parse(Request.Form["w"]);
            double wH = int.Parse(Request.Form["h"]);

            double ow = int.Parse(Request.Form["ow"]);
            double oh = int.Parse(Request.Form["oh"]);

            double left = int.Parse(Request.Form["x"]);
            double right = int.Parse(Request.Form["x2"]);
            double top = int.Parse(Request.Form["y"]);
            double bottom = int.Parse(Request.Form["y2"]);

            double dw = width / ow;
            double dh = height / oh;

            // x tengely
            int cleft = (int)(left * dw);
            int cright = (int)(width - right * dw);

            // y tengely
            int ctop = (int)(top * dh);
            int cbottom = (int)(height - bottom * dh);



            image = image.Crop(ctop, cleft, cbottom, cright);
            return image;

        }


    }





}