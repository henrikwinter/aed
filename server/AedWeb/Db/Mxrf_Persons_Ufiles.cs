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
        public static SqlStrings Mxrf_Persons_Ufiles = SqlBuilder<Mxrf_Persons_Ufiles>.Build();


    }

    public class Mxrf_Persons_Ufiles : Commonfields
    {
        public decimal? Id_Mxrf_Persons_Ufiles { get; set; }
        public decimal Id_Persons { get; set; }
        public decimal Id_Ufiles { get; set; }

        public Mxrf_Persons_Ufiles()
        {
        }

        public Mxrf_Persons_Ufiles(decimal idperson, decimal idufiles)
        {
            Id_Persons = idperson;
            Id_Ufiles = idufiles;

        }
        public Mxrf_Persons_Ufiles ShallowCopy()
        {
            return (Mxrf_Persons_Ufiles)this.MemberwiseClone();
        }
        public Mxrf_Persons_Ufiles(HttpRequestBase rRequest)
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