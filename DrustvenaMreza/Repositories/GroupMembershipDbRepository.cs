using DrustvenaMreza.Models;
using Microsoft.Data.Sqlite;

namespace DrustvenaMreza.Repositories
{
    public class GroupMembershipDbRepository
    {
        private readonly string connectionString;

        public GroupMembershipDbRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:SQLiteConnection"];
        }

        public int AddUserToGroup(int groupId, int userId)
        {
            try
            {

                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"INSERT INTO GroupMemberships (UserId, GroupId)
                                 Values (@UserId, @GroupId)";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@GroupId", groupId);
                command.ExecuteNonQuery();

                using SqliteCommand countCommand = new SqliteCommand("SELECT COUNT(*) FROM GroupMemberships WHERE GroupId = @groupId;", connection);
                countCommand.Parameters.AddWithValue("@groupId", groupId);
                int countRows = Convert.ToInt32(countCommand.ExecuteScalar());
                return countRows;
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Error while converting data from database." + ex.Message);
                throw;
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

        public bool RemoveFromGroup(int groupId, int userId)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"DELETE FROM GroupMemberships
                                 WHERE GroupId = @GroupId AND UserId = @UserId";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@GroupId", groupId);
                
                int targetRows = command.ExecuteNonQuery();
                return targetRows > 0; 
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
