using Microsoft.Data.SqlClient;
using System.Data;

namespace LabWork7
{
    internal class DataAccessLayer
    {
        //  5.1.1
        private static string _server = @"mssql";
        private static string _database = "ispp3407";
        private static string _login = "ispp3407";
        private static string _password = "3407";

        //  5.1.1
        public static string ConnectionString
        {
            get
            {
                var builder = new SqlConnectionStringBuilder
                {
                    DataSource = _server,
                    InitialCatalog = _database,
                    UserID = _login,
                    Password = _password,
                    TrustServerCertificate = true,
                };
                return builder.ConnectionString;
            }
        }

        ////  5.1.2
        public static void SetConnectionSettings(string server, string database, string login, string password)
        {
            if (!string.IsNullOrWhiteSpace(server)) _server = server;
            if (!string.IsNullOrWhiteSpace(database)) _database = database;
            if (!string.IsNullOrWhiteSpace(login)) _login = login;
            if (!string.IsNullOrWhiteSpace(password)) _password = password;
        }

        // 5.1.3
        public static async Task<bool> CanConnectAsync()
        {
            try
            {
                using var conn = new SqlConnection(ConnectionString);
                await conn.OpenAsync();
                return conn.State == ConnectionState.Open;
            }
            catch
            {
                return false;
            }
        }

        //// 5.2.1
        public static async Task<int> ExecuteNonQueryAsync(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new ArgumentException("sql is null or empty", sql);
            using var conn = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync();
        }

        ////  5.2.2
        public static async Task<object?> ExecuteScalarAsync(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql)) 
                throw new ArgumentException("sql is null or empty", sql);
            using var conn = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            await conn.OpenAsync();
            return await cmd.ExecuteScalarAsync();
        }

        //// 5.3.1
        public static async Task<int> UpdateTicketPriceAsync(int sessionId, decimal newPrice)
        {
            const string sql = "UPDATE Session SET Price = @Price WHERE SessionId = @Id";
            using var conn = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Price", newPrice);
            cmd.Parameters.AddWithValue("@Id", sessionId);
            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync();
        }

        // 5.4.2
        public static async Task<int> UploadPosterAsync(int filmId, string filePath)
        {
            if (!File.Exists(filePath)) 
                throw new FileNotFoundException("File not found", filePath);
            byte[] data = await File.ReadAllBytesAsync(filePath);
            const string sql = "UPDATE Film SET Poster = @Data WHERE FilmId = @FilmId";
            using var conn = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Data", data);
            cmd.Parameters.AddWithValue("@FilmId", filmId);
            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync();
        }

        // 5.4.3
        public static async Task<bool> SavePosterToFileAsync(int filmId, string outputFilePath)
        {
            const string sql = "SELECT Poster FROM Film WHERE FilmId = @FilmId";
            using var conn = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@FilmId", filmId);
            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            if (result == null || result == DBNull.Value)
                return false;
            byte[] data = (byte[])result;
            await File.WriteAllBytesAsync(outputFilePath, data);
            return true;
        }

        // 5.5.1
        public static async Task<DataTable> GetUpcomingFilmsAsync()
        {
            const string sql = "SELECT FilmId, Title, RentalStart FROM Film WHERE RentalStart >= CAST(GETDATE() AS date) ORDER BY RentalStart";
            using var conn = new SqlConnection(ConnectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            var dt = new DataTable();
            dt.Load(reader);
            return dt;
        }
    }

}
