using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DextraLib.MySql;

using System.Text.RegularExpressions;

namespace Dextra.Database
{
    public class MProcParam : MyProcParam
    {
        public MProcParam() : base()
        {

        }

        public MProcParam(string name, object value, string dir = "I") : base(name, value, dir = "I")
        {

        }

    }



    public class MDbContext : MyDbContext
    {
        public MDbContext()
        {
        }

        public MDbContext(string connectionstring, string Userid = "Gad") : base(connectionstring, Userid = "Gad")
        {
        }

        public MDbContext(string connectionstring, Dictionary<string, object> defaultvariables) : base(connectionstring, defaultvariables)
        {
        }

    }

    public class MDao : MyDao
    {


        [Obsolete]
        public object SqlNonQuery(string sql, object[] par)
        {
            return ExecuteNonQuery(sql, par);
        }

        [Obsolete]
        public object SqlQuery(string sql, object[] par)
        {
            return ExecuteReader(sql, par);
        }

        [Obsolete]
        public void SqlProcedure(string procname, ref List<ProcParam> pars, MDbContext db = null)
        {
            List<MyProcParam> p1 = new List<MyProcParam>();
            foreach (ProcParam p in pars)
            {
                p1.Add(new MyProcParam(p.Name, p.Value, p.Dir));
            }

            base.SqlProcedure(procname, ref p1, db);
        }


        public object SqlSelect(string sql, object[] par)
        {
            //sql = OraDao.OraDaoHelper.ConvertStairsToColumns(sql, Dbcontext.Connection);
           // sql = MyDaoHelper.ConvertStairsToColumns(sql,null);
            return base.ExecuteReader(sql, par);
        }




        public MDao(MDbContext db)
            : base(db)    //new DextraLib.OraDao.OraDbContext(db.ConnectString, db.GeneralVariables))
        {

        }

        #region dispose
        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~MDao()
        {
            Dispose(false);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {

            }
            _disposed = true;
        }
        #endregion
    }

    public class MDao<T> : MyDao<T>
    {
        // public SqlStrings Sqlstring = new SqlStrings();


        public object SqlDeleteId(decimal id)
        {

            string sql = "";
            sql = Sqlstring.Delete + " " + Sqlstring.PrKeyColumns + "=:0 ";
            ExecuteNonQuery(sql, new object[] { id });
            return Result;
        }
        [Obsolete]
        public object SqlNonQuery(string sql, object[] par)
        {
            return ExecuteNonQuery(sql, par);
        }
        [Obsolete]
        public object SqlNonQuery(string sql, object data, object[] par = null)
        {
            return ExecuteNonQueryFromClass(sql, data, par);
        }





        // -----------------------------------------------------------

        public void SqlProcedure(string procname, ref List<ProcParam> pars, MyDbContext db = null)
        {
            List<MyProcParam> p1 = new List<MyProcParam>();
            foreach (ProcParam p in pars)
            {
                p1.Add(new MyProcParam(p.Name, p.Value, p.Dir));
            }

            base.SqlProcedure(procname, ref p1, db);

        }

        public object SqlSelectId(object id)
        {
            string sql = Sqlstring.Select + " where " + Sqlstring.PrKeyColumns + "=:0 ";
            return base.ExecuteReaderToClass(sql, new object[] { id });
        }

        public object SqlSelect(string sql, object[] par, string cmd = "")
        {
            sql = Sqlstring.ReplaceColls(sql, cmd);
            return base.ExecuteReaderToClass(sql, par);
        }
        public object SqlInsert(T e)
        {
            string lsql = Sqlstring.GetSqlCmd("InsertExtra");
            e = (T)ExecuteNonQueryFromClass(lsql, e);
            return e;
        }
        public object SqlUpdate(T e, string cmd = "UpdatePk")
        {
            string lsql = "";
            lsql = Sqlstring.GetSqlCmd(cmd);
            ExecuteNonQueryFromClass(lsql, e);
            return Result;
        }
        public object SqlUpdate(T e, string Where, object[] par, string cmd = "Update")
        {
            string lsql = "";
            lsql = Regex.Replace(Sqlstring.GetSqlCmd(cmd), "WHERE", "", RegexOptions.IgnoreCase);
            lsql = lsql + " where " + Regex.Replace(Where, "WHERE", "", RegexOptions.IgnoreCase);

            ExecuteNonQueryFromClass(lsql, e, par);
            return Result;
        }


        public object SqlDelete(T e)
        {
            string lsql = "";
            lsql = Sqlstring.GetSqlCmd("DeletePk");
            ExecuteNonQueryFromClass(lsql, e);
            return Result;
        }
        public object SqlDelete(T e, string Where, object[] par)
        {
            string cmd = "Delete"; // ez itt nem nagyon kell
            string lsql = "";

            lsql = Regex.Replace(Sqlstring.GetSqlCmd(cmd), "WHERE", "", RegexOptions.IgnoreCase);
            lsql = lsql + " where " + Regex.Replace(Where, "WHERE", "", RegexOptions.IgnoreCase);

            ExecuteNonQueryFromClass(lsql, e);
            return Result;
        }
        public void ClearResult()
        {
            Result.Clear();
        }
        public MDao(MDbContext db) : base(db)  //new DextraLib.OraDao.OraDbContext(db.ConnectString,db.GeneralVariables))
        {
            // typeParameterType = typeof(T);
            //  Sqlstring = DaoHelper.WriteColumnMappings<T>();
        }

        #region dispose
        bool _disposed;


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~MDao()
        {
            Dispose(false);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {

            }
            _disposed = true;
        }
        #endregion
    }

}