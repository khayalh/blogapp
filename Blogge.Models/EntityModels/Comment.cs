using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Blogge.Models.EntityModels
{
    [ExcludeFromCodeCoverage]
    public class Comment 
    {
        [Required]
        public int Id { get; set; }

        [StringLength(150)]
        public string Content { get; set; }

        public DateTime PostedAt { get; set; }

        public string Author { get; set; }

        public string AuthorId { get; set; }

        public int PostId { get; set; }

        [DefaultValue("false")]
        public bool Blocked { get; set; }
        [DefaultValue("false")]
        public bool IsDeleted { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Post Post { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
