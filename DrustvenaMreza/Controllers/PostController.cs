using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace DrustvenaMreza.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private PostDBRepository postDBRepository;

        public PostController(IConfiguration configuration)
        {
            this.postDBRepository = new PostDBRepository(configuration);
        }

        [HttpGet]
        public ActionResult<List<Post>> GetAll()
        {
            try
            {
                List<Post> posts = postDBRepository.GetAll();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return Problem("An error occured while fetching posts.");
            }
        }
        [HttpDelete("{postId}")]
        public ActionResult Delete(int postId) 
        {
            try
            {
                bool deletePost = postDBRepository.Delete(postId); ;
                if (!deletePost)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem("An error occured while deleting post.");
            }
        }
    }
}
