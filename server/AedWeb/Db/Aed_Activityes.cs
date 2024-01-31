using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dextra.Database;
using DextraLib.GeneralDao;
using Xapp.Db;

namespace Xapp.Db
{
    public static partial class Sqlstore
    {
        public static SqlStrings Aed_Activityes = SqlBuilder<Aed_Activityes>.Build();
        public static SqlStrings Vw_Activity = SqlBuilder<Vw_Activity>.Build();
    }


    public class Vw_Activity
    {

       public string Id_Device { get; set; }
        public string Category { get; set; }
        public string Recordtype { get; set; }
        public DateTime Pdate { get; set; }
        public decimal? Id_Persons { get; set; }
        public string Status { get; set; }
        public string Gaccount { get; set; }
        public string Coord { get; set; }
        public string Ip { get; set; }
        public string Patientgender { get; set; }
        public string Atientage { get; set; }
        public string Mstate { get; set; }
        public string Data { get; set; }
        public Vw_Activity()
        {

        }

        public Vw_Activity ShallowCopy()
        {
            return (Vw_Activity)this.MemberwiseClone();
        }

        public Vw_Activity(HttpRequestBase rRequest)
        {

            var type = this.GetType();

            foreach (string key in rRequest.Form.Keys)
            {
                var property = type.GetProperty(key);
                if (property != null)
                {
                    try
                    {
                        var convertedValue = Convert.ChangeType(rRequest.Form[key], property.PropertyType);
                        property.SetValue(this, convertedValue);

                    }
                    catch
                    {

                    }
                }
            }

        }
    }


    public class Aed_Activityes : Commonfields
    {

        public decimal? Id_Aed_Activityes { get; set; }
        public string Id_Device { get; set; }
        public string Category { get; set; }
        public string Recordtype { get; set; }
        public string Xmldata { get; set; }
        public DateTime Pdate { get; set; }

        public Aed_Activityes()
        {
            Id_Aed_Activityes = 0;
        }

        public Aed_Activityes ShallowCopy()
        {
            return (Aed_Activityes)this.MemberwiseClone();
        }

        public Aed_Activityes(HttpRequestBase rRequest)
        {

            var type = this.GetType();

            foreach (string key in rRequest.Form.Keys)
            {
                var property = type.GetProperty(key);
                if (property != null)
                {
                    try
                    {
                        var convertedValue = Convert.ChangeType(rRequest.Form[key], property.PropertyType);
                        property.SetValue(this, convertedValue);

                    }
                    catch
                    {

                    }
                }
            }

        }

    }
}