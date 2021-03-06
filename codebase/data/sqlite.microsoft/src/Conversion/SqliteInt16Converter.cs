﻿using System.Data;

using SqliteType = Microsoft.Data.Sqlite.SqliteType;


namespace Axle.Data.Sqlite.Microsoft.Conversion
{
    #if NETSTANDARD2_0_OR_NEWER || NETFRAMEWORK
    [System.Serializable]
    #endif
    internal sealed class SqliteInt16Converter : SqliteSameTypeConverter<short?>
    {
        public SqliteInt16Converter() : base(DbType.Int16, SqliteType.Integer, false) { }
    }
}