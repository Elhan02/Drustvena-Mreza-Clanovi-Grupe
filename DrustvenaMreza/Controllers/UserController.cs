using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace DrustvenaMreza.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        UserRepository userRepository = new UserRepository();
        GroupRepository groupRepository = new GroupRepository();

        [HttpGet]
        public ActionResult<List<User>> GetAll()
        {
            List<User> Users = GetAllFromDatabase();
            return Ok(Users);
        }

        private List<User> GetAllFromDatabase()
        {
            List<User> users = new List<User>();
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=database/data.db");
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

        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            if (!UserRepository.data.ContainsKey(id))
            {
                return NotFound();
            }
            return Ok(UserRepository.data[id]);
        }

        [HttpPost]
        public ActionResult<User> Create([FromBody] User nUser)
        {
            if (string.IsNullOrWhiteSpace(nUser.UserName) || string.IsNullOrWhiteSpace(nUser.Name) ||
                string.IsNullOrWhiteSpace(nUser.Lastname) || string.IsNullOrWhiteSpace(nUser.Birthdate.ToString("yyyy-MM-dd")))
            {
                return BadRequest();
            }
            nUser.Id = CreateNewId(UserRepository.data.Keys.ToList());
            UserRepository.data[nUser.Id] = nUser;
            userRepository.Save();

            return Ok(nUser);
        }

        [HttpPut("{id}")]
        public ActionResult<User> Update(int id, [FromBody] User uUser)
        {
            if (string.IsNullOrWhiteSpace(uUser.UserName) || string.IsNullOrWhiteSpace(uUser.Name) || string.IsNullOrWhiteSpace(uUser.Lastname) || string.IsNullOrWhiteSpace(uUser.Birthdate.ToString("yyyy-MM-dd")))
            {
                return BadRequest();
            }
            if (!UserRepository.data.ContainsKey(id))
            {
                return NotFound();
            }
            User user = UserRepository.data[id];
            user.UserName = uUser.UserName;
            user.Name = uUser.Name;
            user.Lastname = uUser.Lastname;
            user.Birthdate = uUser.Birthdate;
            userRepository.Save();

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (!UserRepository.data.ContainsKey(id))
            {
                return NotFound();
            }
            UserRepository.data.Remove(id);
            userRepository.Save();

            return NoContent();
        }

        private int CreateNewId(List<int> identificators)
        {
            int maxId = 0;
            foreach (int id in identificators)
            {
                if (maxId < id)
                {
                    maxId = id;
                }
            }
            return maxId + 1;
        }

    }
}

