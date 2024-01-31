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
        public static SqlStrings Vw_Agreementproperty = SqlBuilder<Agreement>.Build();
    }


    public class Vw_Agreementproperty : Commonfields
    {

        public decimal? Id_Agreementproperty { get; set; }
        public decimal Id_Agreement { get; set; }
        public decimal? Id_Flows { get; set; }
        public string Description { get; set; }
        public string Recordtype { get; set; }
        public decimal? Amount { get; set; }
        public string Stable { get; set; }
        public Vw_Agreementproperty()
        {

        }


    }
}