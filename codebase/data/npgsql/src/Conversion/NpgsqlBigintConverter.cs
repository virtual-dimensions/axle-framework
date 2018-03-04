﻿using System.Data;

using NpgsqlTypes;


namespace Axle.Data.Npgsql.Conversion
{
    internal sealed class NpgsqlBigintConverter : NpgsqlSameTypeConverter<long?>
    {
        public NpgsqlBigintConverter() : base(DbType.Int64, NpgsqlDbType.Bigint) { }
    }
}