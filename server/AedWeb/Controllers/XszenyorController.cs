using Dextra.Common;
using Dextra.Database;
using Dextra.Report;
using Dextra.Xforms;
using DextraLib.GeneralDao;
using System.Web.Mvc;
using Xapp.Db;

namespace Xapp.Controllers
{
    public class NbszModel1
    {
        public Persons cp { get; set; }

        public Hupatestperson hp { get; set; }
        public MvcHtmlString rendered { get; set; }
        public MvcHtmlString rendered1 { get; set; }
        public NbszModel1()
        {
            cp = new Persons();
            hp = new Hupatestperson();
        }
        public NbszModel1(Persons p)
        {
            cp = p;
        }
    }

    public class XszenyorController : BaseController
    {

        // GET: Xszenyor
        public ActionResult Index()
        {
            NbszModel1 model = new NbszModel1();
            
            string cuser= User.Identity.Name;
            cuser = "Xszenyor";
            //Dao tmp = new Dao(Sdb);
            //tmp.SqlSelect("select Id_Persons from Persons where userid=:0 ", new object[] { cuser });
            //if(tmp.Result.GetSate(DaoResult.ResCountOne))
            //{
            //    cp.Id_Persons = tmp.DynResult.Id_Persons;

            //} else
            //{

            //}Gschema



            //Dao<Persons> cpdao = new Dao<Persons>(Sdb);
            //cpdao.SqlSelect("select * from Persons where userid=:0 ", new object[] {cuser });
            ////cpdao.SqlSelectId(cp.Id_Persons);
            //if (cpdao.Result.GetSate(DaoResult.ResCountOne))
            //{
            //    model.cp = cpdao.Result.GetFirst<Persons>();
            //}

            //Xform xperson = new Xform(model.cp.Xmldata, "Gschema");
            //model.rendered= new  MvcHtmlString(xperson.DefRender.Render("formid", "templateview"));
            //XformGenralDocument1 ds = new XformGenralDocument1(xperson.ElementAppinfoDocTemplate, xperson);
            //ds.CurLang = "Hu";
            //ds.Render();
            //model.rendered1 = new MvcHtmlString( ds.Rendered); //RenderdedReport;

            Dao<Hupatestperson> hpdao = new Dao<Hupatestperson>(Sdb);
            hpdao.SqlSelectId(3604973600);
            if (hpdao.Result.GetSate(DaoResult.ResCountOne))
            {
                model.hp = hpdao.Result.GetFirst<Hupatestperson>();
            }

            

            Xform hperson = new Xform(model.hp.Xmldata, "Gschema");
            model.rendered = new MvcHtmlString(hperson.DefRender.Render("formid", "templateview"));
            XformGenralDocument1 ds = new XformGenralDocument1(hperson.ElementAppinfoDocTemplate, hperson);
            ds.CurLang = "Hu";
            ds.Render();
            model.rendered1 = new MvcHtmlString(ds.Rendered); //RenderdedReport;

            return View(model);
        }
    }
}