using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace DrustvenaMreza.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupController : ControllerBase
    {
            GroupRepository groupRepository = new GroupRepository();

            [HttpGet]
            public ActionResult<List<Group>> GetAll()
            {
                List<Group> groups = GetAllFromDatabase();
                return Ok(groups);
            }

            [HttpPost]
            public ActionResult<Group> CreateGroup([FromBody] Group newGroup)
            {
                if (String.IsNullOrWhiteSpace(newGroup.Name) || String.IsNullOrWhiteSpace(newGroup.DateCreated.ToString()))
                {
                    return BadRequest();
                }
                newGroup.Id = GetMaxId(GroupRepository.Data.Keys.ToList());
                GroupRepository.Data[newGroup.Id] = newGroup;
                groupRepository.Save();

                return Ok(newGroup);

            }

            [HttpDelete("{id}")]
            public ActionResult DeleteGroup(int id)
            {
                if (!GroupRepository.Data.ContainsKey(id))
                {
                    return NotFound();
                }
                foreach (User user in UserRepository.data.Values)
                {
                    if (user.Groups.Contains(GroupRepository.Data[id]))
                    {
                        user.Groups.Remove(GroupRepository.Data[id]);
                    }
                }
                GroupRepository.Data.Remove(id);
                groupRepository.Save();
                return NoContent();
            }

            private int GetMaxId(List<int> ids)
            {
                int maxId = 0;
                foreach (int id in ids)
                {
                    if (id > maxId)
                    {
                        maxId = id;
                    }
                }
                return maxId + 1;
            }

        private List<Group> GetAllFromDatabase()
        {
            List<Group> groups = new List<Group>();

            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=database/data.db");
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
    }
}
