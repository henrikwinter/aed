
//using DextraLib.Oracle;
using Dextra.Database;
using Dextra.Toolsspace;
using DextraLib.GeneralDao;
using DextraLib.MsSql;
using System;
using System.Web;


namespace Xapp.Db
{

    public static partial class Sqlstore
    {
        public static SqlStrings Animal = SqlBuilder<Animal>.Build();


    }

    public class Animal : Commonfields
    {
        public decimal? Id_Animal { get; set; }
        public string Bid_Animal { get; set; }
        public decimal? Id_Parent { get; set; }
        public string Enar { get; set; }
        public DateTime Birthdate { get; set; }
        public string Name { get; set; }
        public string Root { get; set; }


        public void Translate(string Preflang)
        {
            Root = Langue.EnumTranslate(Root, Root, "DbAnimalRoot", Preflang);
        }


        public string Recordtype { get; set; }
        public string Xmldata { get; set; }
        public decimal? Id_Parentorg { get; set; }
        public string Gender { get; set; }
        public string Placeofbirth { get; set; }
        public string Enar_Parentmale { get; set; }
        public string Enar_Parentfemale { get; set; }
        public string Item { get; set; }
        public Animal()
        {
        }
        public Animal ShallowCopy()
        {
            return (Animal)this.MemberwiseClone();
        }
        public Animal(HttpRequestBase rRequest)
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