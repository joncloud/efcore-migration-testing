using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests
{
    public static class SqlConnectionExtensions
    {
        public static async Task<int> ExecuteNonQueryAsync(this SqlConnection connection, Action<SqlCommand> fn)
        {
            var command = connection.CreateCommand();
            fn(command);
            return await command.ExecuteNonQueryAsync();
        }

        public static async IAsyncEnumerable<T> ExecuteReaderAsync<T>(this SqlConnection connection, Action<SqlCommand> fn, Func<SqlDataReader, T> read)
        {
            var command = connection.CreateCommand();
            fn(command);
            
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                yield return read(reader);
            }
        }

        public static async Task<T> ExecuteScalarAsync<T>(this SqlConnection connection, Action<SqlCommand> fn)
        {
            var command = connection.CreateCommand();
            fn(command);
            return (T)await command.ExecuteScalarAsync();
        }
    }
}
