using Dextra.Database;
using DextraLib.GeneralDao;
using DextraLib.Oracle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xapp.Db
{
    public static partial class Sqlstore
    {
        public static SqlStrings Dxxrefuserusergroup = SqlBuilder<Dxxrefuserusergroup>.Build();

    }


    public class Dxxrefuserusergroup
    {
        [PrimaryKeyAttribute]
        public decimal? Id { get; set; }
        public string Username { get; set; }
        public string Groupname { get; set; }

        public Dxxrefuserusergroup()
        {

        }
        public Dxxrefuserusergroup(decimal? iId, string iUsername, string iGroupname)
        {

            Id = iId;
            Username = iUsername;
            Groupname = iGroupname;
        }

    }
}