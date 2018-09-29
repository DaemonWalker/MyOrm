using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace MyOrm.DataBase
{
    interface IDataBase : IDisposable
    {
        DbConnection GetConnection();
        DbDataReader GetDataReader();
        DbCommand GetCommand(bool openTranscation);
        DbDataAdapter GetDataAdapter();

    }
}
