using Dextra.Database;
using DextraLib.GeneralDao;
using DextraLib.Oracle;

namespace Xapp.Db
{

    public static partial class Sqlstore
    {
        public static SqlStrings Vw_User = SqlBuilder<Vw_User>.Build();

    }



    public class Vw_User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Groupname { get; set; }
        public string Rolename { get; set; }
        public string Usedname { get; set; }

        public Vw_User()
        {

        }
        public Vw_User(string iId, string iUsername, string iGroupname, string iRolename, string iUsedname)
        {

            Id = iId;
            Username = iUsername;
            Groupname = iGroupname;
            Rolename = iRolename;
            Usedname = iUsedname;
        }
    }

}