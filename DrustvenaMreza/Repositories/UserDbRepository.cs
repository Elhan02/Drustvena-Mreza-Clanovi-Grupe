using DrustvenaMreza.Models;
using Microsoft.Data.Sqlite;

namespace DrustvenaMreza.Repositories
{
    public class UserDbRepository
    {
        private const string datapath = "Data Source=database/data.db";
        public List<User> GetAllFromDatabase()
        {
            List<User> users = new List<User>();
            try
            {
                using SqliteConnection connection = new SqliteConnection(datapath);
                connection.Open();

                string query = "SELECT * FROM Users";
                using SqliteCommand command = new SqliteCommand(query, connection);

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
                Console.WriteLine($"Greska se dogodila pri konekciji ili izvrsavanju neispravnih SQL naredbi: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greska se dogodila pri konverziji podataka iz baze podataka: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena vise puta: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neocekivana greska:{ex.Message}");
            }
            return users;
        }

        public User GetById(int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(datapath);
                connection.Open();

                string query = "SELECT * FROM Users WHERE Id=@Id";
                using SqliteCommand command = new SqliteCommand(@query, connection);

                command.Parameters.AddWithValue("Id", id);

                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int UserId = Convert.ToInt32(reader["Id"]);
                    string username = Convert.ToString(reader["Username"]);
                    string name = Convert.ToString(reader["Name"]);
                    string lastname = Convert.ToString(reader["Surname"]);
                    DateTime birthdate = Convert.ToDateTime(reader["Birthday"]);
                    return new User(UserId, username, name, lastname, birthdate);
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greska se desila pri konekciji ili pri izvrsavanju neispravnih SQL naredbi:{ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greska se dogodila pri konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija je otvorena vise puta ili nije otvorena: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neocekivana greska: {ex.Message}");
            }
            return null;
        }

        public int Create(User nUser)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(datapath);
                connection.Open();

                string query = @"INSERT INTO Users (Username, Name, Surname, Birthday) VALUES(@Username, @Name, @Surname,@Birthday); SELECT LAST_INSERT_ROWID();";
                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@Username", nUser.UserName);
                command.Parameters.AddWithValue("@Name", nUser.Name);
                command.Parameters.AddWithValue("@Surname", nUser.Lastname);
                command.Parameters.AddWithValue("@Birthday", nUser.Birthdate.ToString("yyyy-MM-dd"));

                int lastInsertedRowId = Convert.ToInt32(command.ExecuteScalar());
                return lastInsertedRowId;

            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greska se dogodila pri konekciji ili pri izvrsavanju nesipravnih SQL naredbi: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greska se desila pri konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena vise puta: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neocekivana greska: {ex.Message}");
            }
            return 0;
        }
        public int Update(int id, User uUser)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(datapath);
                connection.Open();

                string query = "UPDATE Users SET Username = @Username, Name = @Name, Surname = @Surname, Birthday = @Birthday WHERE Id = @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Username", uUser.UserName);
                command.Parameters.AddWithValue("@Name", uUser.Name);
                command.Parameters.AddWithValue("@Surname", uUser.Lastname);
                command.Parameters.AddWithValue("@Birthday", uUser.Birthdate.ToString("yyyy-MM-dd"));

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greska se dogodila pri konekciji ili pri izvrsavanju nesipravnih SQL naredbi: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greska se dogodila pri konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je vise puta otvorena: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neocekivana greska{ex.Message}");
            }
            return 0;
        }

        public int Delete(int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(datapath);
                connection.Open();

                string query = "DELETE FROM Users WHERE Id=@Id";
                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@Id", id);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greska se dogodila pri konekciji ili pri izvrsavanju nesipravnih SQL naredbi: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greska se dogodila pri konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je vise puta otvorena: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neocekivana greska{ex.Message}");
            }
            return 0;
        }
    }
}
