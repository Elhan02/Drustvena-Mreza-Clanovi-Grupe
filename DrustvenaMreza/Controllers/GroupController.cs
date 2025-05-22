using DrustvenaMreza.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrustvenaMreza.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        public class ValuesController : ControllerBase
        {
            GroupRepository groupRepository = new GroupRepository();

            [HttpGet]
            public ActionResult<List<Group>> GetAll()
            {
                List<Group> groups = GroupRepository.Data.Values.ToList();
                return Ok(groups);
            }

            [HttpGet("{id}")]
            public ActionResult<Group> GetById(int id)
            {
                if (!GroupRepository.Data.ContainsKey(id))
                {
                    return NotFound();
                }
                return Ok(GroupRepository.Data[id]);
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

            [HttpPut("{id}")]
            public ActionResult<Group> UpdateGroup(int id, [FromBody] Group uGroup)
            {
                if (String.IsNullOrWhiteSpace(uGroup.Name))
                {
                    return BadRequest();
                }

                if (!GroupRepository.Data.ContainsKey(id))
                {
                    return NotFound();
                }

                Group group = GroupRepository.Data[id];
                group.Name = uGroup.Name;
                groupRepository.Save();

                return Ok(group);
            }

            [HttpDelete("{id}")]
            public ActionResult DeleteGroup(int id)
            {
                if (!GroupRepository.Data.ContainsKey(id))
                {
                    return NotFound();
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
}
