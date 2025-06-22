using DrustvenaMreza.Models;
using Microsoft.Data.Sqlite;

namespace DrustvenaMreza.Repositories
{
    public class UserDbRepository
    {
        private readonly string connectionString;

        public UserDbRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:SQLiteConnection"];
        }

        public List<User> GetPaged(int page, int pageSize)
        {
            List<User> users = new List<User>();
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT * FROM Users LIMIT @LIMIT OFFSET @OFFSET";
                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@LIMIT", pageSize);
                command.Parameters.AddWithValue("@OFFSET", pageSize * (page - 1));

                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string username = Convert.ToString(reader["Username"]);
                    string name = Convert.ToString(reader["Name"]);
                    string lastname = Convert.ToString(reader["Surname"]);
                    DateTime birthdate = Convert.ToDateTime(reader["Birthday"]);
                    users.Add(new User(id, username, name, lastname, birthdate));

                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"An error occured while connecting or executing invalid SQL statements: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"An error occurred while converting data from the database: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Connection is not open or is open more than one time: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
            return users;
        }

        public User GetById(int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT * FROM Users WHERE Id=@Id";
                using SqliteCommand command = new SqliteCommand(@query, connection);

                command.Parameters.AddWithValue("@Id", id);

                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int UserId = Convert.ToInt32(reader["Id"]);
                    string username = Convert.ToString(reader["Username"]);
                    string name = Convert.ToString(reader["Name"]);
                    string lastname = Convert.ToString(reader["Surname"]);
                    DateTime birthdate = Convert.ToDateTime(reader["Birthday"]);
                    User newUser = new User(UserId, username, name, lastname, birthdate);
                    return newUser;
                }
                return null;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"An error occured while connecting or executing invalid SQL statements: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"An error occurred while converting data from the database: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Connection is not open or is open more than one time: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
        }

        public User Create(User nUser)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"INSERT INTO Users (Username, Name, Surname, Birthday) VALUES(@Username, @Name, @Surname,@Birthday); SELECT LAST_INSERT_ROWID();";
                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@Username", nUser.UserName);
                command.Parameters.AddWithValue("@Name", nUser.Name);
                command.Parameters.AddWithValue("@Surname", nUser.Lastname);
                command.Parameters.AddWithValue("@Birthday", nUser.Birthdate.ToString("yyyy-MM-dd"));

                nUser.Id = Convert.ToInt32(command.ExecuteScalar());
                return nUser;

            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"An error occured while connecting or executing invalid SQL statements: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"An error occurred while converting data from the database: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Connection is not open or is open more than one time: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
        }
        public User Update(User uUser)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "UPDATE Users SET  Username = @Username, Name = @Name, Surname = @Surname, Birthday = @Birthday WHERE Id = @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@Id", uUser.Id);
                command.Parameters.AddWithValue("@Username", uUser.UserName);
                command.Parameters.AddWithValue("@Name", uUser.Name);
                command.Parameters.AddWithValue("@Surname", uUser.Lastname);
                command.Parameters.AddWithValue("@Birthday", uUser.Birthdate.ToString("yyyy-MM-dd"));

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0 ? uUser : null;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"An error occured while connecting or executing invalid SQL statements: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"An error occurred while converting data from the database: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Connection is not open or is open more than one time: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "DELETE FROM Users WHERE Id=@Id";
                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@Id", id);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"An error occured while connecting or executing invalid SQL statements: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"An error occurred while converting data from the database: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Connection is not open or is open more than one time: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
        }

        public int CountAll()
        {
            try

            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT COUNT(*) FROM Users;";
                using SqliteCommand command = new SqliteCommand(query, connection);

                int countRows = Convert.ToInt32(command.ExecuteScalar());
                return countRows;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"An error occured while connecting or executing invalid SQL statements: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"An error occurred while converting data from the database: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Connection is not open or is open more than one time: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
        }
    }
}
