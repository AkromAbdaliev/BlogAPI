
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.Entities
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required int AuthorId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
