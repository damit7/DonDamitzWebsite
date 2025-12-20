using System.Data;
using Microsoft.Data.SqlClient;
using DonDamitzWebsite.Models;

namespace DonDamitzWebsite.Data
{
    /// <summary>
    /// Data Access Layer for Contact Messages using ADO.NET
    /// Handles all database operations for the ContactMessages table
    /// </summary>
    public class ContactRepository
    {
        private readonly string _connectionString;

        public ContactRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'DefaultConnection' not found");
        }

        /// <summary>
        /// Inserts a new contact message into the database
        /// </summary>
        /// <param name="message">The contact message to save</param>
        /// <returns>The ID of the newly created message</returns>
        public async Task<int> CreateAsync(ContactMessage message)
        {
            const string sql = @"
                INSERT INTO ContactMessages (Name, Email, Subject, Message, SubmittedOn, IPAddress)
                VALUES (@Name, @Email, @Subject, @Message, @SubmittedOn, @IPAddress);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            // Add parameters to prevent SQL injection
            command.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = message.Name;
            command.Parameters.Add("@Email", SqlDbType.NVarChar, 255).Value = message.Email;
            command.Parameters.Add("@Subject", SqlDbType.NVarChar, 200).Value = message.Subject;
            command.Parameters.Add("@Message", SqlDbType.NVarChar, -1).Value = message.Message; // -1 for NVARCHAR(MAX)
            command.Parameters.Add("@SubmittedOn", SqlDbType.DateTime2).Value = message.SubmittedOn;
            command.Parameters.Add("@IPAddress", SqlDbType.NVarChar, 50).Value = (object?)message.IPAddress ?? DBNull.Value;

            await connection.OpenAsync();
            var newId = (int)await command.ExecuteScalarAsync();

            return newId;
        }

        /// <summary>
        /// Retrieves a contact message by ID
        /// </summary>
        /// <param name="id">The message ID</param>
        /// <returns>The contact message or null if not found</returns>
        public async Task<ContactMessage?> GetByIdAsync(int id)
        {
            const string sql = @"
                SELECT Id, Name, Email, Subject, Message, SubmittedOn, IPAddress
                FROM ContactMessages
                WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapReaderToContactMessage(reader);
            }

            return null;
        }

        /// <summary>
        /// Gets all contact messages ordered by submission date (newest first)
        /// </summary>
        /// <returns>List of all contact messages</returns>
        public async Task<List<ContactMessage>> GetAllAsync()
        {
            const string sql = @"
                SELECT Id, Name, Email, Subject, Message, SubmittedOn, IPAddress
                FROM ContactMessages
                ORDER BY SubmittedOn DESC";

            var messages = new List<ContactMessage>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                messages.Add(MapReaderToContactMessage(reader));
            }

            return messages;
        }

        /// <summary>
        /// Counts the number of messages submitted from a specific IP address within a time period
        /// Used for rate limiting
        /// </summary>
        /// <param name="ipAddress">The IP address to check</param>
        /// <param name="minutesAgo">Time period in minutes</param>
        /// <returns>Count of messages from that IP</returns>
        public async Task<int> GetMessageCountByIPAsync(string ipAddress, int minutesAgo)
        {
            const string sql = @"
                SELECT COUNT(*)
                FROM ContactMessages
                WHERE IPAddress = @IPAddress
                AND SubmittedOn >= DATEADD(MINUTE, @MinutesAgo, GETDATE())";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@IPAddress", SqlDbType.NVarChar, 50).Value = ipAddress;
            command.Parameters.Add("@MinutesAgo", SqlDbType.Int).Value = -minutesAgo; // Negative for DATEADD

            await connection.OpenAsync();
            var count = (int)await command.ExecuteScalarAsync();

            return count;
        }

        /// <summary>
        /// Maps a SqlDataReader row to a ContactMessage object
        /// </summary>
        private static ContactMessage MapReaderToContactMessage(SqlDataReader reader)
        {
            return new ContactMessage
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Subject = reader.GetString(reader.GetOrdinal("Subject")),
                Message = reader.GetString(reader.GetOrdinal("Message")),
                SubmittedOn = reader.GetDateTime(reader.GetOrdinal("SubmittedOn")),
                IPAddress = reader.IsDBNull(reader.GetOrdinal("IPAddress"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("IPAddress"))
            };
        }

        /// <summary>
        /// Tests the database connection
        /// </summary>
        /// <returns>True if connection is successful</returns>
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}