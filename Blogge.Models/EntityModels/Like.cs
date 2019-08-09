using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Blogge.Models.Enums;

namespace Blogge.Models.EntityModels
{
    [ExcludeFromCodeCoverage]
    public class Like
    {
        [Required]
       
        public int Id { get; set; }

        public  string UserID { get; set; }

        public  int? PostId { get; set; }

        public RatingType RatingType { get; set; }

        public virtual Post Post { get; set; }

        public virtual ApplicationUser User { get; set; }

    }
}
