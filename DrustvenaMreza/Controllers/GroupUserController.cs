using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DrustvenaMreza.Controllers
{
    [Route("api/groups/{groupId}/users")]
    [ApiController]
    public class GroupUserController : ControllerBase
    {
        private GroupDbRepository GroupRepo { get; set; }
        private UserDbRepository UserRepo { get; set; }
        private GroupMembershipDbRepository MembershipRepo { get; set; }

        public GroupUserController(IConfiguration configuration)
        {
            GroupRepo = new GroupDbRepository(configuration);
            UserRepo = new UserDbRepository(configuration);
            MembershipRepo = new GroupMembershipDbRepository(configuration);
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
            try
            {
                Group? group = GroupRepo.GetById(groupId);
                User? user = UserRepo.GetById(userId);

                if (group == null)
                {
                    return NotFound("Group not found.");
                }
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                int countRows = MembershipRepo.AddUserToGroup(groupId, userId);
                group.Users.Add(user);

                object result = new
                {
                    Data = group,
                    Total = countRows
                };

                return Ok(result);
            }
            catch (SqliteException ex)
            {
                if (ex.SqliteErrorCode == 19)
                {
                    return Problem("User is already in group.");
                }
                return Problem("An error occurred while adding user to group.");
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while adding user to group.");
            }
        }

        [HttpDelete("{userId}")]
        public ActionResult GroupUserDelete(int groupId, int userId)
        {
            try
            {
                Group? group = GroupRepo.GetById(groupId);
                User? user = UserRepo.GetById(userId);

                if (group == null)
                {
                    return NotFound("Group not found.");
                }
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                bool isDeleted = MembershipRepo.RemoveFromGroup(groupId, userId);

                if (!isDeleted)
                {
                    return NotFound("User is not a part of group");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while removing user from group.");
            }
        }
    }
}
