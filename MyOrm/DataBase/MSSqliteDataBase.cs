using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace MyOrm.DataBase
{
    class MSSqliteDataBase : IDataBase
    {
        private const string connStr = "data source=db.db";
        public DbConnection GetConnection()
        {
            var conn = new SqliteConnection(connStr);
            SQLitePCL.Batteries.Init();
            conn.Open();
            return conn;
        }

        public DbDataReader GetDataReader()
        {
            throw new NotImplementedException();
        }

        public DbCommand GetCommand(bool openTranscation)
        {
            var conn = this.GetConnection();
            var comm = conn.CreateCommand();
            if (openTranscation)
            {
                var trans = conn.BeginTransaction();
                comm.Transaction = trans;
            }
            return comm;
        }

        public DbDataAdapter GetDataAdapter()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
