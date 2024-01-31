using Dextra.Database;
using DextraLib.GeneralDao;
using DextraLib.Oracle;
using System;
// dd
namespace Xapp.Db
{

    public static partial class Sqlstore
    {
        public static SqlStrings Vw_Personsproperty = SqlBuilder<Vw_Personsproperty>.Build();

    }

    public class Vw_Personsproperty : Commonfields
    {

        public decimal? Id_Personsproperty { get; set; }
        public decimal Id_Flows { get; set; }
        public decimal Id_Persons { get; set; }
        public string Description { get; set; }
        public string Recordtype { get; set; }

        public DateTime Datefrom { get; set; }
        public DateTime Dateto { get; set; }
        public string Ready { get; set; }
        public decimal? Conductedyear { get; set; }
        public string Stable { get; set; }

        public Vw_Personsproperty()
        {

        }


    }
}