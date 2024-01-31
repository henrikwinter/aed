
//using DextraLib.Oracle;
using Dextra.Database;
using DextraLib.GeneralDao;
using DextraLib.MsSql;
using System;
using System.Web;

namespace Xapp.Db
{

    public static partial class Sqlstore
    {
        public static SqlStrings Person = SqlBuilder<Person>.Build();
        public static SqlStrings Persons = SqlBuilder<Persons>.Build();

    }

    public class OrphanPersons
    {
        public decimal? Id_Persons { get; set; }
        public decimal Id_Organization { get; set; }
        public string Usedname { get; set; }
    }

    public class Persons : Commonfields
    {
        public decimal? Id_Persons { get; set; }
        public string Bid_Persons { get; set; }
        public string Usedname { get; set; }
        public string Email { get; set; }
        public string Birthfirstname { get; set; }
        public string Birthlastname { get; set; }
        public DateTime Birthdate { get; set; }
        public string Placeofbirth { get; set; }
        public string Motherfirstname { get; set; }
        public string Motherlastname { get; set; }
        public string Xmldata { get; set; }
        public decimal? Id_Parent { get; set; }
        public string Userid { get; set; }
        public string Passwordhas { get; set; }

        public string Userpreferences { get; set; }

        public Persons(decimal? iId_Persons, string iBid_Persons, string iUsedname, string iEmail, string iBirthfirstname, string iBirthlastname, DateTime iBirthdate, string iPlaceofbirth, string iMotherfirstname, string iMotherlastname, string iXmldata, decimal iId_Parent, string iUserid)
        {

            Id_Persons = iId_Persons;
            Bid_Persons = iBid_Persons;
            Usedname = iUsedname;
            Email = iEmail;
            Birthfirstname = iBirthfirstname;
            Birthlastname = iBirthlastname;
            Birthdate = iBirthdate;
            Placeofbirth = iPlaceofbirth;
            Motherfirstname = iMotherfirstname;
            Motherlastname = iMotherlastname;
            Xmldata = iXmldata;
            Id_Parent = iId_Parent;
            Userid = iUserid;
        }
        public Persons()
        {

        }

        public Persons ShallowCopy()
        {
            return (Persons)this.MemberwiseClone();
        }

        public Persons(HttpRequestBase rRequest)
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

    public class Person : Commonfields
    {
        public decimal? Id_Person { get; set; }

        public decimal? Cid_Person { get; set; }
        public string Bid_Person { get; set; }
        public string Usedname { get; set; }
        public string Email { get; set; }
        public string Birthfirstname { get; set; }
        public string Birthlastname { get; set; }
        public DateTime Birthdate { get; set; }
        public string Placeofbirth { get; set; }
        public string Motherfirstname { get; set; }
        public string Motherlastname { get; set; }
        public string Xmldata { get; set; }
        public decimal? Id_Parent { get; set; }
        public string Userid { get; set; }

        public string Passwordhas { get; set; }

        public string Userpreferences { get; set; }
        public Person(int id, string uname, string pofd)
        {
            Id_Person = id;
            Usedname = uname;
            Placeofbirth = pofd;
        }

        public Person()
        {
            Id_Person = 0;
        }
    }
}