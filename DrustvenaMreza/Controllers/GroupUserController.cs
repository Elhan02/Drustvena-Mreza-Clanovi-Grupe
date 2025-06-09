using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrustvenaMreza.Controllers
{
    [Route("api/groups/{groupId}/users")]
    [ApiController]
    public class GroupUserController : ControllerBase
    {
        private GroupDbRepository GroupRepo { get; set; }
        private UserDbRepository UserRepo { get; set; }

        public GroupUserController(IConfiguration configuration)
        {
            GroupRepo = new GroupDbRepository(configuration);
            UserRepo = new UserDbRepository(configuration);
        }

        GroupRepository groupRepository = new GroupRepository();
        UserRepository userRepository = new UserRepository();

        [HttpGet]
        public ActionResult<List<User>> GetAll(int groupId)
        {
            Group group = GroupRepo.GetById(groupId);
            if (group == null)
            {
                return NotFound();
            }
            return Ok(group);
        }

        [HttpPut("{userId}")]
        public ActionResult<User> GroupUserAdd(int groupId, int userId)
        {
            if (!GroupRepository.Data.ContainsKey(groupId))
            {
                return NotFound("Group not found.");
            }
            if (!UserRepository.data.ContainsKey(userId))
            {
                return NotFound("User not found.");
            }
            Group group = GroupRepository.Data[groupId];
            User user = UserRepository.data[userId];

            if (group.Users.Contains(user))
            {
                return Conflict();
            }

            group.Users.Add(user);
            userRepository.Save();

            return Ok(group);
        }

        [HttpDelete("{userId}")]
        public ActionResult GroupUserDelete(int groupId, int userId)
        {
            if (!GroupRepository.Data.ContainsKey(groupId))
            {
                return NotFound("Group not found.");
            }
            if (!UserRepository.data.ContainsKey(userId))
            {
                return NotFound("User not found.");
            }
            Group group = GroupRepository.Data[groupId];
            User user = UserRepository.data[userId];

            if (group.Users.Contains(user))
            {
                group.Users.Remove(user);
            }
            userRepository.Save();
            
            return NoContent();
        }
    }
}
