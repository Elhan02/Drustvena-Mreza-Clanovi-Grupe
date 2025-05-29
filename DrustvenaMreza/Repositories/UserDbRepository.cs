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

                command.Parameters.AddWithValue("id", id);

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
    }
}
