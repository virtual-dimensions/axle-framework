﻿using System.Data;

#if NETFRAMEWORK
using SqliteType = Axle.Data.Sqlite.SqliteType;
#else
using SqliteType = Microsoft.Data.Sqlite.SqliteType;
#endif


namespace Axle.Data.Sqlite.Conversion
{
    #if NETSTANDARD2_0_OR_NEWER || NETFRAMEWORK
    [System.Serializable]
    #endif
    internal sealed class SqliteDoubleConverter : SqliteSameTypeConverter<double?>
    {
        #if NETFRAMEWORK
        public SqliteDoubleConverter() : base(DbType.Double, SqliteType.Double, true) { }
        #else
        public SqliteDoubleConverter() : base(DbType.Double, SqliteType.Real, true) { }
        #endif
    }
}