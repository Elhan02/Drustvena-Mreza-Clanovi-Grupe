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
            public GroupDbRepository GroupDataBase { get; set; }

            public GroupController()
            {
                GroupDataBase = new GroupDbRepository();
            }

            [HttpGet]
            public ActionResult<List<Group>> GetAll()
            {
                List<Group> groups = GroupDataBase.GetAll();
                return Ok(groups);
            }

            [HttpGet("{id}")]
            public ActionResult<Group> GetById(int id)
            {
                if (GroupDataBase.GetById(id) == null)
                {
                    return NotFound();
                }
                return Ok(GroupDataBase.GetById(id));
        }

            [HttpPost]
            public ActionResult<Group> CreateGroup([FromBody] Group newGroup)
            {
                if (String.IsNullOrWhiteSpace(newGroup.Name) || String.IsNullOrWhiteSpace(newGroup.DateCreated.ToString()))
                {
                    return BadRequest();
                }
                int rowNumber = GroupDataBase.Create(newGroup);
                newGroup.Id = rowNumber;

                return Ok(newGroup);

            }

            [HttpPut("{id}")]
            public ActionResult<Group> UpdateGroup(int id, [FromBody] Group uGroup)
            {
                if (String.IsNullOrWhiteSpace(uGroup.Name))
                {
                    return BadRequest();
                }

                if (GroupDataBase.Update(id, uGroup) == 0)
                {
                    return NotFound();
                }

                uGroup.Id = id;
                return Ok(uGroup);
            }

        [HttpDelete("{id}")]
            public ActionResult DeleteGroup(int id)
            {
                if (GroupDataBase.Delete(id) == 0)
                {
                    return NotFound();
                }
                
                return NoContent();
            }
    }
}
