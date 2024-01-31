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
        public static SqlStrings Aed_Devices = SqlBuilder<Aed_Devices>.Build();
    }



    public class Aed_Devices : Commonfields
    {

        public string Id_Device { get; set; }
        public decimal? Id_Persons { get; set; }
        public string Xmldata { get; set; }

        public Aed_Devices()
        {

        }

        public Aed_Devices ShallowCopy()
        {
            return (Aed_Devices)this.MemberwiseClone();
        }

        public Aed_Devices(HttpRequestBase rRequest)
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