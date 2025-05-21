using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DrustvenaMreza.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        UserRepository userRepository = new UserRepository();

        [HttpGet]
        public ActionResult<List<User>> GetAll()
        {
            List<User> Users = UserRepository.data.Values.ToList();
            return Ok(Users);
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

        public int CreateNewId(List<int> identificators)
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

