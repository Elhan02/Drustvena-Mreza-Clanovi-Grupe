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
    }
}
