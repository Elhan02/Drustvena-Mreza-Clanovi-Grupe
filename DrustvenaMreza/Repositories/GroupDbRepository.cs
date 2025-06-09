using DrustvenaMreza.Models;
using Microsoft.Data.Sqlite;
using System.Xml.Linq;

namespace DrustvenaMreza.Repositories
{
    public class GroupDbRepository
    {
        private readonly string connectionString;

        public GroupDbRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:SQLiteConnection"];
        }

        public int CountAll()
        {
            try

            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT COUNT(*) FROM Groups;";
                using SqliteCommand command = new SqliteCommand(query, connection);

                int countRows = Convert.ToInt32(command.ExecuteScalar());
                return countRows;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine("Error while connecting with database or executing SQL command: " + ex.Message);
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Error while converting data from database: " + ex.Message);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Error, connection is not open or oppened more times: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
                throw;
            }
        }

        public List<Group> GetPaged(int page, int pageSize)
        {
            List<Group> groups = new List<Group>();

            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT * FROM Groups LIMIT @Limit OFFSET @Offset ";
                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@Limit", pageSize);
                command.Parameters.AddWithValue("@Offset", pageSize * (page - 1));

                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string name = reader["Name"].ToString();
                    DateTime creationDate = Convert.ToDateTime(reader["CreationDate"]);

                    groups.Add(new Group(id, name, creationDate));

                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine("Error while connecting with database or executing SQL command: " + ex.Message);
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Error while converting data from database: " + ex.Message);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Error, connection is not open or oppened more times: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
                throw;
            }
            return groups;
        }

        public Group GetById(int id)
        {

            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"SELECT  g.Id, g.Name, g.CreationDate, u.Id AS UserId, u.Username AS Username, 
                               u.Name AS Firstname, u.Surname AS Lastname, u.Birthday AS UserBirthDate
                               FROM Groups g
                               LEFT JOIN GroupMemberships gm ON g.Id = gm.GroupId
                               LEFT JOIN Users u ON gm.UserId = u.Id
                               WHERE g.Id = @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                using SqliteDataReader reader = command.ExecuteReader();

                Group group = null;

                while (reader.Read())
                {
                    if (group == null)
                    {
                        group = new Group
                        (
                            Convert.ToInt32(reader["Id"]),
                            reader["Name"].ToString(),
                            Convert.ToDateTime(reader["CreationDate"])
                        );
                    }
                    if (reader["UserId"] != DBNull.Value)
                    {
                        User newUser = new User
                        {
                            Id = Convert.ToInt32(reader["UserId"]),
                            UserName = Convert.ToString(reader["Username"]),
                            Name = Convert.ToString(reader["Firstname"]),
                            Lastname = Convert.ToString(reader["Lastname"]),
                            Birthdate = Convert.ToDateTime(reader["UserBirthDate"])
                        };
                        group.Users.Add(newUser);
                    }

                }
                return group;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine("Error while connecting with database or executing SQL command: " + ex.Message);
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Error while converting data from database: " + ex.Message);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Error, connection is not open or oppened more times: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
                throw;
            }
            return null;
        }

        public int Create(Group newGroup)
        {
            int lastId = 0;
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "INSERT INTO Groups (Name, CreationDate) VALUES (@Name, @CreationDate); SELECT LAST_INSERT_ROWID();";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Name", newGroup.Name);
                command.Parameters.AddWithValue("@CreationDate", newGroup.DateCreated.ToString("yyyy-MM-dd"));

                lastId = Convert.ToInt32(command.ExecuteScalar());
            }
            catch (SqliteException ex)
            {
                Console.WriteLine("Error while connecting with database or executing SQL command: " + ex.Message);
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Error while converting data from database: " + ex.Message);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Error, connection is not open or oppened more times: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
                throw;
            }
            return lastId;
        }

        public Group Update(Group group)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "UPDATE Groups SET Name = @Name WHERE Id = @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Name", group.Name);
                command.Parameters.AddWithValue("@Id", group.Id);
                
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0 ? group : null;
                
            }
            catch (SqliteException ex)
            {
                Console.WriteLine("Error while connecting with database or executing SQL command: " + ex.Message);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Error, connection is not open or oppened more times: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
                throw;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "DELETE FROM Groups WHERE Id = @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine("Error while connecting with database or executing SQL command: " + ex.Message);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Error, connection is not open or oppened more times: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
                throw;
            }
        }

    }
}
