using Dextra.Database;
using DextraLib.GeneralDao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xapp.Db
{

    public static partial class Sqlstore
    {
        public static SqlStrings Mxrf_Animal_Ufiles = SqlBuilder<Mxrf_Animal_Ufiles>.Build();


    }

    public class Mxrf_Animal_Ufiles
    {

        public decimal? Id_Mxrf_Animal_Ufiles { get; set; }
        public decimal Id_Animal { get; set; }
        public decimal Id_Ufiles { get; set; }

        public Mxrf_Animal_Ufiles()
        {
        }

        public Mxrf_Animal_Ufiles(decimal idanimal, decimal idufiles)
        {
            Id_Animal = idanimal;
            Id_Ufiles = idufiles;

        }
        public Mxrf_Animal_Ufiles ShallowCopy()
        {
            return (Mxrf_Animal_Ufiles)this.MemberwiseClone();
        }
        public Mxrf_Animal_Ufiles(HttpRequestBase rRequest)
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