using Npgsql;
using System.Data;

namespace VanadoShared.DatabaseAccess
{
    public interface IPostgresDataAccess
    {
        NpgsqlConnection CreateConnection();
        Task<IEnumerable<T>> LoadDataAsync<T, U>(string sqlOrProcedure, U parameters, CommandType commandType = CommandType.Text);
        Task SaveDataAsync<T>(string sqlOrProcedure, T parameters, CommandType commandType = CommandType.Text);
    }
}