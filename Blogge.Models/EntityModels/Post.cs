
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Blogge.Models.EntityModels
{
    [ExcludeFromCodeCoverage]
    public class Post 
    {
        [Required]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Title { get; set; }

        [Required, StringLength(100)]
        public string SubTitle { get; set; }

        public string Author { get; set; }

        public string AuthorId { get; set; }

        [Required]
       
        public string Content { get; set; }

        public DateTime PostedAt { get; set; }

        public int Rank { get; set; }

        [DefaultValue("false")]
        public bool Blocked { get; set; }
        [DefaultValue("false")]
        public bool IsDeleted { get; set; }

        public virtual PostPicture postPicture {get; set;}

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Like> Likes { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}