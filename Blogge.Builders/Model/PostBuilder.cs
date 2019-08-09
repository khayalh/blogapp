using Blogge.Extensions.Providers;
using Blogge.Interfaces.Builders.Model;
using Blogge.Interfaces.Facades.Identity;
using Blogge.Interfaces.Repositories;
using Blogge.Models;
using Blogge.Models.ViewModels;
using System.Linq;
using System.Collections.Generic;
using Blogge.Builders.Settings;
using Blogge.Interfaces.Converters;
using Blogge.Interfaces.Validators;
using Blogge.Models.EntityModels;

namespace Blogge.Builders.Model
{
    public class PostBuilder : IPostBuilder
    {
        private readonly IBlogRepository _postRepository;
        private readonly IIdentityFacade _identityFacade;
        private readonly IDisplayValidator _displayValidator;
        private readonly IImageRepository _imageRepository;
        private readonly IDataConverter _dataConverter;

        public PostBuilder(IBlogRepository postRepository,
            IIdentityFacade identityFacade,
            IDisplayValidator displayValidator,
            IImageRepository imageRepository,
            IDataConverter dataConverter)

        {
            _postRepository = postRepository;
            _identityFacade = identityFacade;
            _displayValidator = displayValidator;
            _imageRepository = imageRepository;
            _dataConverter = dataConverter;
        }

        public BigPostViewModel BuildBigPostViewModel(int id)
        {
            var model = new BigPostViewModel()
            {
                PostModel = BuildPostViewModel(id),
                AddComment = BuildAddCommentsViewModel(id),
                Comments = BuildAllCommentsViewModel(id),
                SideBar = BuildSidebarContentViewModel()
            };

            return model;
        }

        public HomepageViewModel BuildHomepageViewModel()
        {
            var model = new HomepageViewModel()
            {
                SideBar = BuildSidebarContentViewModel()
            };

            model.LastPosts = model.SideBar.LastPosts;
            model.PostCount = model.LastPosts.Count();

            return model;
        }
        public HomepageViewModel BuildHomepageViewModel(int displayCount)
        {
            var lastPosts = _postRepository.GetPosts()
                .Where(x=>x.IsDeleted == false)
                .OrderByDescending(x => x.PostedAt)
                .Take(displayCount).ToList();

            var model = new HomepageViewModel()
            {
                SideBar = BuildSidebarContentViewModel(),
                LastPosts = BuildListOfPostsViewModel(lastPosts),
                PostCount = lastPosts.Count()
            };

            return model;
        }

        public HomepageViewModel BuildHomepageViewModel(string searchResult)
        {
            var result = _postRepository.SearchPosts(searchResult);

            var model = new HomepageViewModel()
            {
                SideBar = BuildSidebarContentViewModel(),
                LastPosts = BuildListOfPostsViewModel(result),
                PostCount = result.Count()
            };

            return model;
        }

        public AllCommentsViewModel BuildAllCommentsViewModel(int id)
        {
            var post = _postRepository.GetPost(id);
            var model = new AllCommentsViewModel() { CommentList = new List<CommentViewModel>() };
            foreach (var comment in post.Comments)
            {
                if (_displayValidator.IsAvailableComment(comment))
                {
                    model.CommentList.Add(BuildCommentViewModel(comment.Id));
                }
            }
            model.CommentList.Reverse();
            return model;
        }

        public CommentViewModel BuildCommentViewModel(int id)
        {
            var comment = _postRepository.GetComment(id);
            var model = new CommentViewModel()
            {
                Author = comment.Author,
                AuthorId = comment.AuthorId,
                Content = comment.Content,
                PostId = comment.Post.Id,
                Id = comment.Id,
                PostedAt = comment.PostedAt,
                AuthorAvatar = _imageRepository.GetImageInString(comment.AuthorId)
            };

            return model;
        }

        public AddCommentViewModel BuildAddCommentsViewModel(int id)
        {
            var model = new AddCommentViewModel()
            {
                PostId = id
            };

            return model;
        }

        public SinglePostViewModel BuildPostViewModel(int id)
        {
            var post = _postRepository.GetPost(id);

            var model = new SinglePostViewModel()
            {
                Author = post.Author,
                AuthorAvatar = _imageRepository.GetImageInString(post.AuthorId),
                AuthorId = post.AuthorId,
                Content = post.Content,
                Id = post.Id,
                Title = post.Title,
                PostedAt = post.PostedAt,
                Rank = post.Rank,
                SubTitle = post.SubTitle
            };

            if (post.postPicture != null)
            {
                model.PostImage = _dataConverter.ConvertToString(post.postPicture.PostImageInBytes);
            }
            else
            {
                model.PostImage = string.Empty;
            }
            return model;
        }

        public SidebarContentViewModel BuildSidebarContentViewModel()
        {
            var posts = _postRepository.GetPosts().Where(x => x.IsDeleted == false);

            var lastComments = _postRepository.GetComments()
                .Where(x => x.IsDeleted == false && x.Blocked == false && x.Post.IsDeleted == false && x.Post.Blocked == false)
                .OrderByDescending(x => x.PostedAt)
                .Take(PostDisplayCount.LastCommentedCount)
                .ToList();
          
            var discussing = GetPostsByComments(lastComments);
            var latest = posts.OrderByDescending(x => x.PostedAt).Take(PostDisplayCount.LastPostedCount).ToList();
            var popular = posts.OrderByDescending(x => x.Rank).Take(PostDisplayCount.TrendingCount).ToList();

            var model = new SidebarContentViewModel()
            {
                LastCommentedPosts = BuildListOfPostsViewModel(discussing),
                LastPosts = BuildListOfPostsViewModel(latest),
                TrendingPosts = BuildListOfPostsViewModel(popular),
                SearchModel = BuildSearchResultViewModel()
            };

            return model;
        }

        public SearchViewModel BuildSearchResultViewModel()
        {
            var model = new SearchViewModel();
            return model;
        }

        private List<Post> GetPostsByComments(List<Comment> lastComments)
        {
            var listOfPosts = new List<Post>();
            foreach (var comment in lastComments)
            {
                var post = _postRepository.GetPost(comment.PostId);
                if (listOfPosts.Contains(post))
                {
                    continue;
                }
                listOfPosts.Add(post);
            }
            return listOfPosts;
        }

        public List<SinglePostViewModel> BuildListOfPostsViewModel(List<Post> listOfPosts)
        {
            var listOfModels = new List<SinglePostViewModel>();

            foreach (var post in listOfPosts)
            {
                var model = BuildPostViewModel(post.Id);
                listOfModels.Add(model);
            }
            return listOfModels;

        }

        public CreatePostViewModel BuildCreatePostViewModel()
        {
            var model = new CreatePostViewModel();

            return model;
        }

        public EditPostViewModel BuildEditPostViewModel(int postId)
        {
            var post = _postRepository.GetPost(postId);
            var model = new EditPostViewModel()
            {
                Id = post.Id,
                Content = post.Content,
                SubTitle = post.SubTitle,
                Title = post.Title
                
            };
            return model;
        }

        public EditCommentViewModel BuildEditCommentViewModel(int id)
        {
            var comment = _postRepository.GetComment(id);
            var model = new EditCommentViewModel()
            {
                Id = comment.Id,
                Content = comment.Content,
                CallbackId = comment.PostId
            };
            return model;
        }

        public ReportCommentViewModel BuildReportCommentViewModel(int commentId, int callbackPostId)
        {
            var model = new ReportCommentViewModel()
            {
                CommentId = commentId,
                CallbackPostId = callbackPostId
            };
            return model;
        }

        public SearchResultViewModel BuildSearchResultViewModel(string searchKey)
        {
            var searchResult = _postRepository.SearchPosts(searchKey);

            var model = new SearchResultViewModel()
            {
                SideBar = BuildSidebarContentViewModel(),
                SearchResults = searchResult
            };

            return model;
        }
    }
}
