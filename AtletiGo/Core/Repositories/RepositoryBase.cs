using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using static Dapper.SqlMapper;

namespace AtletiGo.Core.Repositories
{
    public class RepositoryBase : IRepositoryBase
    {
        private readonly string _connString;

        public RepositoryBase(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("Postgre");
        }

        public IEnumerable<T> Query<T>(string sql, object obj = null) where T : class
        {
            using var db = new NpgsqlConnection(_connString);

            return db.Query<T>(sql, obj);
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            using var db = new NpgsqlConnection(_connString);

            return db.GetList<T>();
        }

        public IEnumerable<T> GetAll<T>(object obj = null) where T : class
        {
            using var db = new NpgsqlConnection(_connString);

            return db.GetList<T>(obj);
        }

        public T GetById<T>(Guid codigo) where T : class
        {
            using var db = new NpgsqlConnection(_connString);

            return db.Get<T>(codigo);
        }

        public void Insert<T>(T entity)
        {
            using var db = new NpgsqlConnection(_connString);

            db.Insert<Guid, T>(entity);
        }

        public void Update<T>(T entity)
        {
            using var db = new NpgsqlConnection(_connString);

            db.Update<T>(entity);
        }

        public void Delete<T>(T entity)
        {
            using var db = new NpgsqlConnection(_connString);

            db.Delete(entity);
        }
    }
}
