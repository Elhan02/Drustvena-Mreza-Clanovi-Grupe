using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace DrustvenaMreza.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        public GroupDbRepository GroupDataBase { get; set; }

        public GroupController(IConfiguration configuration)
        {
            GroupDataBase = new GroupDbRepository(configuration);
        }

        [HttpGet]
        public ActionResult GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page and Page size must be greater than zero.");
            }

            try
            {
                List<Group> groups = GroupDataBase.GetPaged(page, pageSize);
                int countRows = GroupDataBase.CountAll();

                object result = new
                {
                    Data = groups,
                    Total = countRows
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while fetching groups.");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Group> GetById(int id)
        {
            try
            {
                Group? group = GroupDataBase.GetById(id);
                if (group == null)
                {
                    return NotFound($"Group with ID {id} not found.");
                }
                return Ok(group);
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while fetching the group.");
            }
        }

        [HttpPost]
        public ActionResult<Group> CreateGroup([FromBody] Group newGroup)
        {
            if (newGroup == null || String.IsNullOrWhiteSpace(newGroup.Name) || String.IsNullOrWhiteSpace(newGroup.DateCreated.ToString()))
            {
                return BadRequest("Invalid group data");
            }

            try 
            { 
                int rowNumber = GroupDataBase.Create(newGroup);
                newGroup.Id = rowNumber;

                return Ok(newGroup);
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while creating group.");
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Group> UpdateGroup(int id, [FromBody] Group uGroup)
        {
            if (uGroup == null || String.IsNullOrWhiteSpace(uGroup.Name))
            {
                return BadRequest("Invalid group data.");
            }

            try
            {
                uGroup.Id = id;
                Group? group = GroupDataBase.Update(uGroup);
                if (group == null)
                {
                    return NotFound($"Group with ID {id} not found.");
                }
                return Ok(group);
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while updating group.");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteGroup(int id)
        {
            try
            {
                bool isDeleted = GroupDataBase.Delete(id);
                if (isDeleted)
                {
                    return NoContent();
                }

                return NotFound($"Group with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while deleting group.");
            }
        }
    }
}
