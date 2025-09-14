using System.Data;
using Dapper;
using Npgsql;
using Microsoft.Extensions.Configuration;
using System.Dynamic;

namespace VanadoShared.DatabaseAccess
{
    public class PostgresDataAccess : IPostgresDataAccess
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public PostgresDataAccess(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("PostgresConnection");
        }

        public NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        public async Task<IEnumerable<T>> LoadDataAsync<T, U>(string sqlOrProcedure, U parameters, CommandType commandType = CommandType.Text)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            return await connection.QueryAsync<T>(sqlOrProcedure, parameters, commandType: commandType);
        }

        public async Task SaveDataAsync<T>(string sqlOrProcedure, T parameters, CommandType commandType = CommandType.Text)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var convertedParams = ConvertEnumsToStrings(parameters);

            await connection.ExecuteAsync(sqlOrProcedure, convertedParams, commandType: commandType);
        }

        private object ConvertEnumsToStrings<T>(T parameters)
        {
            var expando = new ExpandoObject() as IDictionary<string, object?>;

            foreach (var prop in typeof(T).GetProperties())
            {
                var value = prop.GetValue(parameters);

                if (value != null && prop.PropertyType.IsEnum)
                    expando[prop.Name] = value.ToString().ToLower();
                else
                    expando[prop.Name] = value;
            }

            return expando;
        }
    }
}
