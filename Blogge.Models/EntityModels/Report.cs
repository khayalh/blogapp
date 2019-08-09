using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Blogge.Models.EntityModels
{
    [ExcludeFromCodeCoverage]
    public class Report
    {
        [Required]
        public int Id { get; set; }

        public virtual Comment Comment { get; set; }

        public int CommentId { get; set; }

        [StringLength(150)]
        public string ReportText { get; set; }

        public string SenderName { get; set; }

        [DefaultValue("false")]
        public bool IsDeleted { get; set; }
    }
}
