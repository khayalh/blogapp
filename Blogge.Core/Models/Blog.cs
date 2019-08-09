
using System.ComponentModel.DataAnnotations;

namespace Blogge.Core.Models
{
    public class Post
    {
        [Required]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Title { get; set; }

        [Required, StringLength(50)]
        public string Author { get; set; }

        [Required]
        public string Content { get; set; }
    }
}