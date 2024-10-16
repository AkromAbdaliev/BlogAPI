namespace BlogAPI.Models.Dtos
{
	public class AddBlogPostRequest
	{
		public required string Title { get; set; }
		public required string Content { get; set; }
		public required int AuthorId { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}
