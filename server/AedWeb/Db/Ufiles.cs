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
        public static SqlStrings Ufiles = SqlBuilder<Ufiles>.Build();

    }

    public class Ufiles : Commonfields
    {

        public decimal? Id_Ufiles { get; set; }

        public decimal? Id_Flows { get; set; }
        public string Datatype { get; set; }
        public string Filetype { get; set; }
        public string Mimetype { get; set; }
        public decimal? Filesize { get; set; }
        public byte[] Bdata { get; set; }
        public string Cdata { get; set; }

        public Ufiles()
        {
        }
        public Ufiles(decimal? id, string datatype, string filetype, string mimetype, decimal? filesize, byte[] data, string info)
        {
            Id_Ufiles = id;
            Datatype = datatype;
            Filetype = filetype;
            Mimetype = mimetype;
            Filesize = filesize;
            Bdata = data;
            Cdata = info;

        }
        public Persons ShallowCopy()
        {
            return (Persons)this.MemberwiseClone();
        }

        public Ufiles(HttpRequestBase rRequest)
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