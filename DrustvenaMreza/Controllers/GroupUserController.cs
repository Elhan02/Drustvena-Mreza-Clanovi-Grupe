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
        GroupRepository groupRepository = new GroupRepository();
        UserRepository userRepository = new UserRepository();

        [HttpGet]
        public ActionResult<List<User>> GetAll(int groupId)
        {
            if (!GroupRepository.Data.ContainsKey(groupId))
            {
                return NotFound();
            }
            List<User> users = UserRepository.data.Values.ToList();
            List<User> groupUser = new List<User>();
            foreach (User user in users)
            {
                foreach (Group group in user.Groups)
                {
                    if (group.Id == groupId)
                    {
                        groupUser.Add(user);
                    }
                }
            }
            return Ok(groupUser);
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

            user.Groups.Add(group);
            userRepository.Save();

            return Ok(user);
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

            if (user.Groups.Contains(group))
            {
                user.Groups.Remove(group);
            }
            userRepository.Save();
            
            return NoContent();
        }
        



    }
}
