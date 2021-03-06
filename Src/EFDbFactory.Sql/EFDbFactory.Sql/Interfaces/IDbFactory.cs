﻿using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace EFDbFactory.Sql
{
    public interface IDbFactory : IDisposable
    {
        /// <summary>
        /// Create a Connection with Transaction level
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        Task<IDbFactory> Create(System.Data.IsolationLevel isolationLevel);

        /// <summary>
        /// Create a factory with no transaction
        /// </summary>
        /// <returns></returns>
        Task<IDbFactory> Create();

        /// <summary>
        /// Create a factory with no commit. use for testing
        /// </summary>
        /// <returns></returns>
        Task<IDbFactory> CreateNoCommit();

        /// <summary>
        /// Get your context with porvided sql connection and with transaction if factory is transactional
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T For<T>() where T : EFDbContext;

        /// <summary>
        /// commit transaction when you have done your work. if there is an error in your code the transaction will not be committed. throw InvalidOperationException if the factory is created with no transaction.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        void CommitTransaction();

        /// <summary>
        /// return a sql connection when factory is being created.
        /// </summary>
        SqlConnection Connection { get; }

        /// <summary>
        /// return sql transaction which is being initialized with factory creation
        /// </summary>
        SqlTransaction Transaction { get; }
    }
}
