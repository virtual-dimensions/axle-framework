﻿using System.Data;

namespace Axle.Data.DataSources
{
    /// <summary>
    /// An interface representing a data source object. The data source object is responsible for
    /// interacting with an underlying ADO.NET implementation and exposing means for 
    /// connecting to a database and executing queries.
    /// </summary>
    public interface IDataSource : IDbParameterBuilder
    {
        /// <summary>
        /// Opens a new connection to the datasource.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="IDataSourceConnection"/> representing the open connection.
        /// </returns>
        IDataSourceConnection OpenConnection();

        IDataSourceCommand GetCommand(string commandText, CommandType commandType, BuildCommandCallback buildCommandCallback);

        IDataSourceCommand GetScript(string bundle, string scriptPath, CommandType commandType, BuildCommandCallback buildCommandCallback);

        /// <summary>
        /// Gets a string value representing the SQL dialect used by the represented data source.
        /// </summary>
        string DialectName { get; }
        /// <summary>
        /// Gets the connection string value that is used to establish a connection to the data source.
        /// </summary>
        string ConnectionString { get; }
    }
}