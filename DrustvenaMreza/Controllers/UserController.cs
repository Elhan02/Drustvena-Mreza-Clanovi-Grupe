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

        public UserController()
        {
            DbRepository = new UserDbRepository();
        }

        [HttpGet]
        public ActionResult<List<User>> GetAll()
        {
            List<User> Users = DbRepository.GetAllFromDatabase();
            return Ok(Users);
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            if (DbRepository.GetById(id) == null)
            {
                return NotFound();
            }
            return Ok(DbRepository.GetById(id));
        }

        [HttpPost]
        public ActionResult<User> Create([FromBody] User nUser)
        {
            if (string.IsNullOrWhiteSpace(nUser.UserName) || string.IsNullOrWhiteSpace(nUser.Name) ||
                string.IsNullOrWhiteSpace(nUser.Lastname) || string.IsNullOrWhiteSpace(nUser.Birthdate.ToString("yyyy-MM-dd")))
            {
                return BadRequest();
            }
            int rowId = DbRepository.Create(nUser);
            nUser.Id = rowId;
            return Ok(nUser);
        }

        [HttpPut("{id}")]
        public ActionResult<User> Update(int id, [FromBody] User uUser)
        {
            if (string.IsNullOrWhiteSpace(uUser.UserName) || string.IsNullOrWhiteSpace(uUser.Name) || string.IsNullOrWhiteSpace(uUser.Lastname) || string.IsNullOrWhiteSpace(uUser.Birthdate.ToString("yyyy-MM-dd")))
            {
                return BadRequest();
            }
            if (DbRepository.Update(id, uUser) == 0)
            {
                return NotFound();
            }
            uUser.Id = id;
            return Ok(uUser);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (DbRepository.Delete(id)==0)
            {
                return NotFound();
            }

            return NoContent();
        }        
    }
}

