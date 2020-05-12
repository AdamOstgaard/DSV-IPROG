using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace IPROG.Uppgifter.uppg3_1_2
{
    public class GuestBookDbConnector 
    {
        private readonly string _connectionString;
        private static readonly Regex SanitizeRegex;

        static GuestBookDbConnector()
        {
            const string pattern = "<.*>";

            SanitizeRegex = new Regex(pattern, RegexOptions.Compiled);
        }

        /// <summary>
        /// Creates a new <see cref="GuestBookDbConnector"/> instance with the provided connection string.
        /// </summary>
        /// <param name="connectionString">Connection string for DB access</param>
        public GuestBookDbConnector(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Creates a table named guests if it does not already exist.
        /// </summary>
        /// <returns></returns>
        public async Task InitializeTablesAsync()
        {
            using (var dbConnection = new MySqlConnection(_connectionString))
            using (var command = dbConnection.CreateCommand())
            {
                dbConnection.Open();
                command.CommandText = "CREATE TABLE IF NOT EXISTS guests ("
                    + "id INT NOT NULL PRIMARY KEY AUTO_INCREMENT, "
                    + "name VARCHAR(50), "
                    + "email VARCHAR(50), "
                    + "homepage VARCHAR(50), "
                    + "comment VARCHAR(100)"
                    + ")";

                await command.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// Gets all guests from the database asynchronously.
        /// </summary>
        /// <returns>A task with the guests as result</returns>
        public async Task<IEnumerable<Guest>> GetGuestsAsync()
        {
            using (var dbConnection = new MySqlConnection(_connectionString))
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM guests";

                dbConnection.Open();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        return Enumerable.Empty<Guest>();
                    }

                    var guests = new List<Guest>();

                    while (await reader.ReadAsync())
                    {
                        var name = (string)reader["name"];
                        var email = (string)reader["email"];
                        var phone = (string)reader["homepage"];
                        var comment = (string)reader["comment"];

                        guests.Add(new Guest(name, email, phone, comment));
                    }

                    return guests;
                }
            }
        }

        /// <summary>
        /// Adds a guest to the database asynchronously.
        /// </summary>
        /// <param name="guest">The guest to add</param>
        public async Task AddGuestAsync(Guest guest)
        {
            using (var dbConnection = new MySqlConnection(_connectionString))
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = "INSERT INTO guests " +
                    " (name, email, homepage, comment) " +
                    "VALUES (@name, @email, @homepage, @comment)";

                command.Parameters.AddRange(new[] {
                    CreateSanitizedParameter("@name", guest.Name),
                    CreateSanitizedParameter("@email", guest.Email),
                    CreateSanitizedParameter("@homepage", guest.Homepage),
                    CreateSanitizedParameter("@comment", guest.Comment)
                });

                dbConnection.Open();

                await command.ExecuteNonQueryAsync();
            }
        }

        public override string ToString()
        {
            var sb = new MySqlConnectionStringBuilder(_connectionString);
            return $"GuestbookDB connected to {sb.Server} on port {sb.Port}.";
        }

        /// <summary>
        /// Sanitizes the input to maker sure it does not contain any Html.
        /// </summary>
        /// <param name="input">Data to be sanitized</param>
        /// <returns>A safe sanitized string.</returns>
        private static string SanitizeInput(string input)
            => SanitizeRegex.Replace(input, "censur");
        
        /// <summary>
        /// Sanitizes the input and creates a SqlParameter.
        /// </summary>
        /// <param name="name">Name of the new parameter.</param>
        /// <param name="data">Data to be sanitized.</param>
        /// <returns>A sanitized <see cref="MySqlParameter"/></returns>
        private static MySqlParameter CreateSanitizedParameter(string name, string data)
            => new MySqlParameter(name, SanitizeInput(data));
    }
}
