using Dextra.Database.Local;
//using Dextra.Database;
using Dextra.Report;
using DextraLib.GeneralDao;

using System;
using System.Reflection;

namespace Dextra.Xforms
{

    /// <summary>
    ///  SZAROS A DAO es LOCALDAO 
    /// </summary>
    /// <typeparam name="T"></typeparam>

    public class XfpDao<T> {

        public DaoResult Result { get; set; }

        Dextra.Database.Dao<T> dao { get; set; }
        Dao<T> ldao { get; set; }
        bool local = false;

        public XfpDao(object dbcontext )
        {
            Type t = dbcontext.GetType();
            if (t== typeof(Dextra.Database.DbContext))
            {
                local = true;
            } else
            {
                local = false;
            }

        }

        public object SqlSelect(string sql, object[] par, string cmd = "")
        {
            if (local) return ldao.SqlSelect(sql, par, cmd);
            return dao.SqlSelect(sql, par, cmd);
        }
        public object SqlSelectId(object id)
        {
            if (local) return ldao.SqlSelectId(id);
            return dao.SqlSelectId(id);

        }
        public object SqlInsert(T e)
        {
            if (local) return ldao.SqlInsert(e);
            return dao.SqlInsert(e);

        }
        public object SqlUpdate(T e)
        {
            if (local) return ldao.SqlUpdate(e);
            return dao.SqlUpdate(e); 

        }
        public object SqlUpdate(T e, string Where, object[] par, string cmd = "Update")
        {
            if (local) return ldao.SqlUpdate(e, Where, par, cmd );
            return dao.SqlUpdate(e, Where, par, cmd);

        }
        public object SqlDelete(T e)
        {
            if (local) return ldao.SqlDelete(e);
            return dao.SqlDelete(e);

        }
        public object SqlDelete(T e, string Where, object[] par)
        {
            if (local) return ldao.SqlDelete(e,Where,par);
            return dao.SqlDelete(e, Where, par);

        }
        public string MoveRecord(int id, int newpid, string drp, string foreginkeyname, string reorderproc, decimal? xrowid = null)
        {
            //if (local) return ldao.MoveRecord(id, newpid, drp, foreginkeyname, reorderproc, xrowid);
            decimal? reorderid = dao.MoveRecordNew(id, newpid, drp, foreginkeyname);
            return ""; // Ez itt elegciki dao.MoveRecord(id, newpid, drp, foreginkeyname, reorderproc, xrowid);

        }
        public void ClearResult()
        {
            if (local) ldao.ClearResult();
            else dao.ClearResult();
        }
    }


}