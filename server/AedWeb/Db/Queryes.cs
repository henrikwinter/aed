using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xapp.Db
{
    public class Queryes
    {

        public decimal? Id { get; set; }
        public string Col1 { get; set; }
        public string Col2 { get; set; }
        public string Col3 { get; set; }
        public string Col4 { get; set; }
        public string Col5 { get; set; }
        public string Col6 { get; set; }
        public string Col7 { get; set; }
        public string Col8 { get; set; }
        public string Col9 { get; set; }



        public Queryes()
        {

        }


        public Queryes ShallowCopy()
        {
            return (Queryes)this.MemberwiseClone();
        }

        public Queryes(HttpRequestBase rRequest)
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