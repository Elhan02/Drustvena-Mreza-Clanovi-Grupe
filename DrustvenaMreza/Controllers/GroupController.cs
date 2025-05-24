using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                List<Group> groups = GroupRepository.Data.Values.ToList();
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
    }
}
