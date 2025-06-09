using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrustvenaMreza.Controllers
{
    [Route("api/users/{userId}/posts")]
    [ApiController]
    public class UserPostController : ControllerBase
    {
        private PostDBRepository postDBRepository { get; set; }
        private UserDbRepository userDbRepository { get; set; }

        public UserPostController(IConfiguration configuration)
        {
            this.postDBRepository = new PostDBRepository(configuration);
            this.userDbRepository = new UserDbRepository(configuration);
        }

        [HttpPost]
        public ActionResult<Post> Create(int userId, Post post)
        {

            if (post == null || string.IsNullOrWhiteSpace(post.Content) || post.Date == null)
            {
                return BadRequest();
            }
            try
            {
                User user = userDbRepository.GetById(userId);
                if (user == null)
                {
                    return NotFound();
                }
                post.UserId = userId;
                Post newPost = postDBRepository.Create(post);
                return Ok(newPost);
            }
            catch (Exception ex)
            {
                return Problem("An error occured while creating post.");
            }
        }
    }
}
