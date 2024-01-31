using Dextra.Common;
using Dextra.Database;
using Dextra.Toolsspace;
using DextraLib.GeneralDao;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Xapp.Db;
using Xapp.FlowDatas;

namespace Xapp.Controllers
{
    public class AedController : BaseController
    {

        [PersistField]
        public AedModel Model = null;

        AjaxResultCode Error = new AjaxResultCode();
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (Model == null)
            {
                Model = new AedModel(Sdb, Rolemanager.AvailableRoles, Rolemanager.CurrentUser);
            }

        }

        // GET: Aed
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Help()
        {
            return View();
        }
        public ActionResult News()
        {
            return View();
        }
        public ActionResult Statistics()
        {

            //string url = @"https://maps.googleapis.com/maps/api/directions/json?origin=75+9th+Ave+New+York,+NY&destination=MetLife+Stadium+1+MetLife+Stadium+Dr+East+Rutherford,+NJ+07073&key=AIzaSyAJ7pUSB9P9OUn13Y3_rQs8-KMHOZDcfr0";

            //WebRequest request = WebRequest.Create(url);

            //WebResponse response = request.GetResponse();

            //Stream data = response.GetResponseStream();

            //StreamReader reader = new StreamReader(data);

            //// json-formatted string from maps api
            //string responseFromServer = reader.ReadToEnd();

            //response.Close();
            return View();
        }

        public class chart1data{
            public DateTime Actdate { get; set; }

            public decimal Connect_Val { get; set; }
            public decimal Mesurement_Val { get; set; }
            public decimal Pulse_Val { get; set; }
            public decimal Resume_Val { get; set; }

        }

        public ActionResult Getchart1Data()
        {

            string sql = @"
                        select 
                        T1.ACTDATE,
                        T1.VALUE as connect_val,T2.VALUE as pulse_val,T3.VALUE as mesurement_val,T4.VALUE as resume_val
                        from 
                        vw_activity_stat1 t1,
                        vw_activity_stat1 t2,
                        vw_activity_stat1 t3,
                        vw_activity_stat1 t4
                        where T1.ACTDATE=T2.ACTDATE and T2.ACTDATE=T3.ACTDATE  and T3.ACTDATE=T4.ACTDATE
                        and T1.RECORDTYPE='Connect'
                        and T2.RECORDTYPE='Pulse'
                        and T3.RECORDTYPE='Mesurement'
                        and T4.RECORDTYPE='Resume'
";
            List<chart1data> res = new List<chart1data>();
            Dao<chart1data> cadao = new Dao<chart1data>(Sdb);
            cadao.SqlSelect(sql, new object[] { });
            if (cadao.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                res = cadao.Result.GetRes<chart1data>();
            }

            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd.", Formatting = Newtonsoft.Json.Formatting.Indented };
            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(res, settings),
                ContentEncoding = Encoding.UTF8
            };

        }

        //[dxAuthorize(C.c_role_AedRead)]
        public ActionResult GetRecords_Aed_VwActivityes(decimal Id, string recordtype)
        {
            List<Vw_Activity> res = new List<Vw_Activity>();
            Dao<Vw_Activity> t = new Dao<Vw_Activity>(Sdb);
            t.SqlSelect("select  t.* from Vw_Activity t ", new object[] { }); //  where rownum<11

            if (t.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                res = t.Result.GetRes<Vw_Activity>();
            }

            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd.", Formatting = Newtonsoft.Json.Formatting.Indented };
            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(res, settings),
                ContentEncoding = Encoding.UTF8
            };
        }



        //[dxAuthorize(C.c_role_AedRead)]
        public ActionResult GetRecords_Aed_Activityes(decimal Id, string recordtype)
        {
            List<Aed_Activityes> res = new List<Aed_Activityes>();
            Dao<Aed_Activityes> t = new Dao<Aed_Activityes>(Sdb);
            t.SqlSelect("select  t.* from Aed_Activityes t where rownum<11 ", new object[] { });

            if (t.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                res = t.Result.GetRes<Aed_Activityes>();
            }

            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd.", Formatting = Newtonsoft.Json.Formatting.Indented };
            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(res, settings),
                ContentEncoding = Encoding.UTF8
            };
        }

    }
}