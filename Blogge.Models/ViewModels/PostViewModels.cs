using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using Blogge.Models.EntityModels;


namespace Blogge.Models.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class CreatePostViewModel
    {
        [Required, StringLength(50)]
        [DataType(DataType.Text)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required, StringLength(100)]
        [DataType(DataType.Text)]
        [Display(Name = "Subitle")]
        public string SubTitle { get; set; }

        [Display(Name = "PostPicture")]
        public HttpPostedFileBase file { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Content")]
        [Required]
        public string Content { get; set; }
    }


    [ExcludeFromCodeCoverage]
    public class BigPostViewModel
    {
        public SinglePostViewModel PostModel { get; set; }
        public AllCommentsViewModel Comments { get; set; }
        public AddCommentViewModel AddComment { get; set; }
        public SidebarContentViewModel SideBar { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class HomepageViewModel
    {
        public SidebarContentViewModel SideBar { get; set; }
        public List<SinglePostViewModel> LastPosts { get; set; }
        [DefaultValue(5)]
        public int PostCount { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class SinglePostViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Subitle")]
        public string SubTitle { get; set; }
        [Display(Name = "Author")]
        public string Author { get; set; }
        [Display(Name = "AuthorId")]
        public string AuthorId { get; set; }
        [Display(Name = "Content")]
        public string Content { get; set; }
        [Display(Name = "AuthorAvatar")]
        public string AuthorAvatar { get; set; }
        [Display(Name = "PostImage")]
        public string PostImage { get; set; }
        [Display(Name = "PostedAt")]
        public DateTime PostedAt { get; set; }
        [Display(Name = "Rank")]
        public int Rank { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class EditPostViewModel
    {
        [Display(Name = "id")]
        public int Id { get; set; }

        [Display(Name = "Title")]
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Subtitle")]
        [StringLength(100)]
        public string SubTitle { get; set; }

        [Display(Name = "Content")]
        [Required]
        public string Content { get; set; }

        [Display(Name = "PostImage")]
        public HttpPostedFileBase PostImage { get; set; }

    }

    [ExcludeFromCodeCoverage]
    public class SearchResultViewModel
    {
        public List<Post> SearchResults { get; set; }
        public SidebarContentViewModel SideBar { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class SidebarContentViewModel
    {
        [Display(Name = "PostList")]
        public List<SinglePostViewModel> LastPosts { get; set; }
        public List<SinglePostViewModel> LastCommentedPosts { get; set; }
        public List<SinglePostViewModel> TrendingPosts { get; set; }

        public SearchViewModel SearchModel { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class SearchViewModel
    {
        [MinLength(1)]
        public string Search { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class AddCommentViewModel
    {
        [Display(Name = "Add Your Comment")]
        [Required, StringLength(150)]
        public string Content { get; set; }
        public int PostId { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class AllCommentsViewModel
    {
        [Display(Name = "CommentList")]
        public List<CommentViewModel> CommentList { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class CommentViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime PostedAt { get; set; }

        public int PostId { get; set; }

        public string Author { get; set; }

        public string AuthorId { get; set; }

        public string AuthorAvatar { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class EditCommentViewModel
    {
        [Display(Name = "id")]
        public int Id { get; set; }

        [Display(Name = "Content")]
        [Required, StringLength(150)]
        public string Content { get; set; }

        [Display(Name = "CallbackId")]
        public int CallbackId { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ReportCommentViewModel
    {
        [Display(Name = "Report")]
        [Required, StringLength(150)]
        public string Content { get; set; }

        public int CommentId { get; set; }

        public int CallbackPostId { get; set; }
    }
}
