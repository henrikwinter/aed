using Dextra.Common;
using Dextra.Database;
using Dextra.Toolsspace;
using DextraLib.GeneralDao;
using System;
using System.Web;

namespace Xapp.Db
{

    public static partial class Sqlstore
    {
        public static Sqlstring Proba = new Sqlstring("Alma", "Korte","");

    }

   // [DbObjectTypeAttribute("Commonfields")]
    public class Commonfields
    {

       // [DbColumn("Recordvalidfrom", ExtraInfo = "{0}", ExtraInfoBind = "chkcurdate({0})")]
        public DateTime? Recordvalidfrom { get; set; }
        public DateTime? Recordvalidto { get; set; }
        public DateTime? Datavalidfrom { get; set; }
        public DateTime? Datavalidto { get; set; }
        public string Status { get; set; }
        public string Creator { get; set; }
        public string Modifiers { get; set; }
        public string Attributum { get; set; }
        public decimal? Orggroup { get; set; }
        public string Property { get; set; }

        //[IgnoreKeyAttribute]
        [DbColumnFunctionAttribute("ORA_ROWSCN")]
        public decimal? Xrowid { get; set; }
        public string Assignment { get; set; }



        public Commonfields()
        {
            InitCommonFields();
        }


        public Commonfields(string attributum, DateTime? datavalidfrom, DateTime? datavalidto)
        {
            Attributum = attributum;
            Datavalidfrom = datavalidfrom;
            Datavalidto = Datavalidto;

        }
        public void InitCommonFields()
        {

            //Recordvalidfrom = null;// DateTime.Now;
            //Recordvalidto = null;
            //Datavalidfrom = null; 
            //Datavalidto = null;
            //Status = null;
            //Attributum = null;
            //Creator = null;
            //Modifiers = CTools.GetCurrentUserValue("name");  
            //Orggroup = decimal.Parse(CTools.GetCurrentUserValue("group"));   


        }
        public void CloseCommonFields(DateTime? recordvalidto,DateTime? datavalidto,string status = null, string attributum = null, string modifiers = null)
        {
            Recordvalidto = recordvalidto;
            Datavalidto = datavalidto;
            Status = status;
            Attributum = attributum;
            Modifiers = HttpContext.Current.User.Identity.Name;// modifiers;
        }


        public Commonfields GetCommonFields()
        {
            Commonfields retv = new Commonfields();

            retv.Recordvalidfrom = Recordvalidfrom;
            retv.Recordvalidto = Recordvalidto;
            retv.Datavalidfrom = Datavalidfrom;
            retv.Datavalidto = Datavalidto;
            retv.Status = Status;
            retv.Attributum = Attributum;
            retv.Creator = Creator;
            retv.Modifiers = Modifiers;

            return retv;
        }

        public void SetCommonFields(Commonfields c)
        {
            Recordvalidfrom = c.Recordvalidfrom;
            Recordvalidto = c.Recordvalidto;
            Datavalidfrom = c.Datavalidfrom;
            Datavalidto = c.Datavalidto;
            Status = c.Status;
            Attributum = c.Attributum;
            Creator = c.Creator;
            Modifiers = c.Modifiers;
        }

        public void InitCommonFieldsForAdd(DbContext db)
        {
            DateTime cr = db.Dbsysdate.GetDbSydate();  // new DateTime(1600, 1, 1, 1, 1, 1);

            if (Recordvalidfrom == null) Recordvalidfrom = cr; // DateTime.Now;
            Recordvalidto = null;
            if (Datavalidfrom == null) Datavalidfrom = cr; // DateTime.Now;
            Datavalidto = null;
            //Status = null;
            //Attributum = null;
            try
            {
                Creator = HttpContext.Current.User.Identity.Name;  // DTools.GetCurrentUserValue("name"); // Membership.GetUser().UserName;
            } catch { Creator = "Sys"; }
            
            //Modifiers = null;
            try
            {
                Orggroup = (decimal)db.GeneralVariables["orggroup"];
            }
            catch
            {
                Orggroup = 0;
            }


        }


        public void InitCommonFieldsForAddDesp(DbSysDate db)
        {
            DateTime cr = db.GetDbSydate();  // new DateTime(1600, 1, 1, 1, 1, 1);

            if (Recordvalidfrom == null) Recordvalidfrom = cr; // DateTime.Now;
            Recordvalidto = null;
            if (Datavalidfrom == null) Datavalidfrom = cr; // DateTime.Now;
            Datavalidto = null;
            //Status = null;
            //Attributum = null;
            try
            {
                Creator = HttpContext.Current.User.Identity.Name;  // DTools.GetCurrentUserValue("name"); // Membership.GetUser().UserName;
            } catch
            {
                Creator = "Sys";
            }

            //Modifiers = null;
            try
            {
                Orggroup = 0;
            }
            catch
            {
                Orggroup = 0;
            }


        }

        // In work.....
        public int CheckTechFields(string user = "", DateTime? ak = null, DateTime? rk = null)
        {
            if (ak != null) { Datavalidfrom = ak; }
            if (Datavalidfrom == null) { Datavalidfrom = DateTime.Now; }
            if (rk != null) { Recordvalidfrom = rk; }
            if (Recordvalidfrom == null) { Recordvalidfrom = DateTime.Now; }

            if (!String.IsNullOrEmpty(user)) { Creator = user; }
            //if (String.IsNullOrEmpty(Creator)) { Creator = CTools.GetCurrentUserValue("name"); } //Membership.GetUser().UserName;
            //Orggroup = decimal.Parse(CTools.GetCurrentUserValue("group"));    // use.OrgGroup;
            return 0;
        }



    }

}