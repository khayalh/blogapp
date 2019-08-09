
using Blogge.Models.ViewModels;


namespace Blogge.Interfaces.Builders.Model
{
    public interface IPostBuilder
    {
        SinglePostViewModel BuildPostViewModel(int id);
        SidebarContentViewModel BuildSidebarContentViewModel();
        HomepageViewModel BuildHomepageViewModel(int displayCount);
        HomepageViewModel BuildHomepageViewModel(string searchKey);
        HomepageViewModel BuildHomepageViewModel();
        CreatePostViewModel BuildCreatePostViewModel();
        EditPostViewModel BuildEditPostViewModel(int postId);
        SearchViewModel BuildSearchResultViewModel();
        BigPostViewModel BuildBigPostViewModel(int id);
        EditCommentViewModel BuildEditCommentViewModel(int id);
        ReportCommentViewModel BuildReportCommentViewModel(int commentId, int callbackPostId);
        SearchResultViewModel BuildSearchResultViewModel(string searchKey);
    }
}