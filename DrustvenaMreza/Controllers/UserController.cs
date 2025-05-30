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
        public UserDbRepository DbRepository { get; set; }

        public UserController(IConfiguration configuration)
        {
            DbRepository = new UserDbRepository(configuration);
        }

        [HttpGet]
        public ActionResult GetAll()
        {

            try
            {
                List<User> Users = DbRepository.GetAllFromDatabase();
                return Ok(Users);
            }
            catch (Exception ex)
            {
                return Problem("An error occured while fetching users.");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            try
            {
                User user = DbRepository.GetById(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return Problem("An error occured while fetching user.");
            }
        }

        [HttpPost]
        public ActionResult<User> Create([FromBody] User nUser)
        {
            if (string.IsNullOrWhiteSpace(nUser.UserName) || string.IsNullOrWhiteSpace(nUser.Name) ||
                string.IsNullOrWhiteSpace(nUser.Lastname) || string.IsNullOrWhiteSpace(nUser.Birthdate.ToString("yyyy-MM-dd")))
            {
                return BadRequest();
            }

            try
            {
                User user = DbRepository.Create(nUser);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return Problem("An error occured while creating  new user");
            }
        }

        [HttpPut("{id}")]
        public ActionResult<User> Update(int id, [FromBody] User uUser)
        {
            if (string.IsNullOrWhiteSpace(uUser.UserName) || string.IsNullOrWhiteSpace(uUser.Name) || string.IsNullOrWhiteSpace(uUser.Lastname) || string.IsNullOrWhiteSpace(uUser.Birthdate.ToString("yyyy-MM-dd")))
            {
                return BadRequest();
            }
            try
            {
                uUser.Id = id;
                User user = DbRepository.Update(uUser);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return Problem("An error occured while updating user.");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                bool deletedUser = DbRepository.Delete(id);
                if (deletedUser)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem("An error occured while removing user.");
            }
        }
    }
}

