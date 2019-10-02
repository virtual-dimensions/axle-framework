﻿using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Axle.Verification;

namespace Axle.Data.DataSources
{
    internal sealed class DataSourceCommandBuilder : ICommandBuilder, ICommandBuilderResult
    {
        private readonly IDbServiceProvider _provider;
        private readonly IDataSource _dataSource;
        private readonly string _commandText;
        private readonly CommandType _commandType;
        private int? _commandTimeout;
        private readonly ICollection<IDataParameter> _parameters;

        public DataSourceCommandBuilder(
            IDbServiceProvider provider,
            IDataSource dataSource,
            CommandType commandType, 
            string commandText)
        {
            _provider = provider;
            _dataSource = dataSource;
            _commandType = commandType;
            _commandText = commandText;
            _parameters = new List<IDataParameter>();
        }

        public ICommandBuilder SetTimeout(int timeout)
        {
            _commandTimeout = ComparableVerifier.IsGreaterThan(Verifier.VerifyArgument(timeout, nameof(timeout)), 0);
            return this;
        }

        private DbCommand CreateCommandInstance(string text, IDataSourceConnection connection)
        {
            var command = _provider.CreateCommand(
                text,
                _commandType,
                _commandTimeout,
                connection.WrappedInstance,
                connection.CurrentTransaction?.WrappedInstance);
            foreach (var parameter in _parameters)
            {
                command.Parameters.Add(parameter);
            }
            return command;
        }

        DataSourceCommand ICommandBuilderResult.Build()
        {
            return new DataSourceCommand(_dataSource, _provider, _commandText, CreateCommandInstance);
        }
    }
}