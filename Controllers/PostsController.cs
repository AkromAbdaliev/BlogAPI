using BlogAPI.Data;
using BlogAPI.Models.Dtos;
using BlogAPI.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PostsController : ControllerBase
	{
		private readonly ApplicationDbContext _dbContext;
		public PostsController(ApplicationDbContext dbContext) 
		{
			_dbContext = dbContext;
		}


		[HttpGet]
		public async Task<IActionResult> GetAllBlogPosts()  
		{
			var posts =  await _dbContext.BlogPosts.ToListAsync();
			return Ok(posts);
		}

		[HttpGet]
		[Route("{id:guid}")]
		[ActionName("GetBlogPostById")]
		public async Task<IActionResult> GetBlogPostById(Guid id) 
		{
			 var post = await _dbContext.BlogPosts
					.FirstOrDefaultAsync(x => x.Id == id);

			return post == null ? NotFound() : Ok(post);
		}

		[HttpPost]
		public async Task<IActionResult> PostBlogPost(AddBlogPostRequest addBlogPost)
		{
			// Check if the data is valid
			if (!ModelState.IsValid)
			{
				// Return 400 Bad Request with details about what failed validation
				return BadRequest(ModelState);
			}

			//convert Dto to entity
			var post = new Post()
			{
				Title = addBlogPost.Title,
				Content = addBlogPost.Content,
				AuthorId = addBlogPost.AuthorId,
				CreatedDate = DateTime.UtcNow
			};

			post.Id = Guid.NewGuid();
			await _dbContext.BlogPosts.AddAsync(post);
			await _dbContext.SaveChangesAsync();

			return CreatedAtAction(nameof(GetBlogPostById), new { id = post.Id }, post);
		}
		
		[HttpPut]
		[Route("{id:guid}")] 
		public async Task<IActionResult> PutBlogPost([FromRoute] Guid id, UpdateBlogPostRequest updateBlogPostRequest)
		{
			// Check if exists
			var existingPost =  await _dbContext.BlogPosts.FindAsync(id);
			
			if (existingPost != null) 
			{
				existingPost.Title = updateBlogPostRequest.Title;
				existingPost.Content = updateBlogPostRequest.Content;
				existingPost.AuthorId = updateBlogPostRequest.AuthorId;
				existingPost.CreatedDate = DateTime.UtcNow;

				await _dbContext.SaveChangesAsync();
				return NoContent();
			}
			
			return NotFound();
		}

		[HttpDelete]
		[Route("{id:guid}")]
		public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
		{
			var existingPost = await _dbContext.BlogPosts.FindAsync(id);

            if (existingPost != null)
            {
				_dbContext.Remove(existingPost);
				await _dbContext.SaveChangesAsync();
				return NoContent();
            }
			return NotFound();

        }

	}
}
