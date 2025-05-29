using DrustvenaMreza.Models;
using Microsoft.Data.Sqlite;

namespace DrustvenaMreza.Repositories
{
    public class GroupDbRepository
    {
        private const string path = "Data Source=database/data.db";

        public List<Group> GetAll()
        {
            List<Group> groups = new List<Group>();

            try
            {
                using SqliteConnection connection = new SqliteConnection(path);
                connection.Open();

                string query = "SELECT * FROM Groups";
                using SqliteCommand command = new SqliteCommand(query, connection);

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
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Error while converting data from database: " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Error, connection is not open or oppened more times: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }
            return groups;
        }

        public Group GetById(int id)
        {

            try
            {
                using SqliteConnection connection = new SqliteConnection(path);
                connection.Open();

                string query = "SELECT * FROM Groups WHERE Id = @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("Id", id);

                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int groupId = Convert.ToInt32(reader["Id"]);
                    string name = reader["Name"].ToString();
                    DateTime creationDate = Convert.ToDateTime(reader["CreationDate"]);

                    return new Group(groupId, name, creationDate);

                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine("Error while connecting with database or executing SQL command: " + ex.Message);
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Error while converting data from database: " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Error, connection is not open or oppened more times: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }
            return null;
        }

    }
}
