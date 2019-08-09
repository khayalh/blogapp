
using System;
using System.Web.Mvc;
using Blogge.Interfaces.Builders.Model;
using Blogge.Interfaces.Facades.Identity;
using Blogge.Interfaces.Repositories;
using Blogge.Interfaces.Validators;
using Blogge.Models.Enums;
using Blogge.Models.ViewModels;

namespace Blogge.Web.Controllers
{
    [HandleError]
    public class PostController : Controller
    {
        private readonly IBlogRepository _postRepository;
        private readonly IIdentityFacade _identityFacade;
        private readonly IPostBuilder _postBuilder;
        private readonly IAccessValidator _accessValidator;
        private readonly IDisplayValidator _displayValidator;


        public PostController(
            IBlogRepository PostRepository,
            IIdentityFacade IdentityFacade,
            IPostBuilder postBuilder,
            IDisplayValidator displayValidator,
            IAccessValidator accessValidator) 
        {
            _postRepository = PostRepository;
            _identityFacade = IdentityFacade;
            _postBuilder = postBuilder;
            _accessValidator = accessValidator;
            _displayValidator = displayValidator;
        }
    
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Create()
        {
            if (_accessValidator.IsRedactor() || _accessValidator.IsAdministration())
            {
                var model = _postBuilder.BuildCreatePostViewModel();
                return View(model);
            }
            return RedirectToAction("Forbidden", "Error");
        }

        [HttpPost]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult AddComment(AddCommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                _postRepository.AddComment(model);
            }
            return RedirectToAction("SinglePost", "Post", new { id = model.PostId });
        }

        [HttpPost]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult Create(CreatePostViewModel p)
        {
            if (_accessValidator.IsRedactor() || _accessValidator.IsAdministration())
            {
                if (ModelState.IsValid)
                {
                    _postRepository.AddPost(p);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "All fields, except image, should be filled!");
                return View(p);
            }

            return RedirectToAction("Unauthorized", "Error");
        }

        [ValidateInput(false)]
        public ActionResult SinglePost(int id)
        {
            if (_postRepository.GetPost(id) == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var model = _postBuilder.BuildBigPostViewModel(id);
            return View(model);
        }

        [Authorize]
        [ValidateInput(false)]
        public ActionResult Edit(int id)
        {
            if (_accessValidator.IsRedactor() || _accessValidator.IsAdministration())
            {
                if (_postRepository.GetPost(id) == null)
                {
                    return RedirectToAction("NotFound", "Error");
                }

                var visible = _displayValidator.IsAvailablePost(id);
                if (!visible)
                {
                    return RedirectToAction("Forbidden", "Error");
                }

                var access = _accessValidator.CanAccessPost(id);

                if (!access)
                {
                    return RedirectToAction("Forbidden", "Error");
                }

                var model = _postBuilder.BuildEditPostViewModel(id);
                return View(model);
            }
            return RedirectToAction("Unauthorized", "Error");
        }

        [Authorize(Roles = "redactor,moderator,admin")]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditPostViewModel post)
        {
            if (ModelState.IsValid)
            {
                _postRepository.EditPost(post);

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "All fields, except image, should be filled!");
            return View(post);
        }

        [Authorize]
        [ValidateInput(false)]
        public ActionResult Delete(int id)
        {
            if (_accessValidator.IsRedactor() || _accessValidator.IsAdministration())
            {

                if (_postRepository.GetPost(id) == null)
                {
                    return RedirectToAction("NotFound", "Error");
                }

                var visible = _displayValidator.IsAvailablePost(id);
                if (!visible)
                {
                    return RedirectToAction("Forbidden", "Error");
                }

                var access = _accessValidator.CanAccessPost(id);
                if (!access)
                {
                    return RedirectToAction("Forbidden", "Error");
                }

                var model = _postBuilder.BuildPostViewModel(
                    Convert.ToInt32(_identityFacade.GetRouteData().Values["id"]));
                return View(model);
            }
            return RedirectToAction("Unauthorized", "Error");
        }

        [Authorize(Roles = "redactor,moderator,admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _postRepository.DeletePost(id);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult RatePost(int id, RatingType ratingType)
        {
            if (_postRepository.GetPost(id) == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var visible = _displayValidator.IsAvailablePost(id);
            if (!visible)
            {
                return RedirectToAction("Forbidden", "Error");
            }

            _postRepository.RatePost(id, _identityFacade.GetUserId(), ratingType);

            return RedirectToAction("SinglePost", "Post", new {  id });
        }

        [Authorize]
        public ActionResult EditComment(int id, int postId)
        {

                if (_postRepository.GetComment(id) == null)
                {
                    return RedirectToAction("NotFound", "Error");
                }

                var visiblePost = _displayValidator.IsAvailablePost(postId);
                if (!visiblePost)
                {
                    return RedirectToAction("Forbidden", "Error");
                }

                var visibleComment = _displayValidator.IsAvailableComment(id);
                if (!visibleComment)
                {
                    return RedirectToAction("Forbidden", "Error");
                }

                var access = _accessValidator.CanAccessComment(id);

                if (!access)
                {
                    return RedirectToAction("Forbidden", "Error");
                }

                var model = _postBuilder.BuildEditCommentViewModel(id);
                return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditComment([Bind(Include = "ID,Content,CallbackId")] EditCommentViewModel comment)
        {
            if (ModelState.IsValid)
            {
                _postRepository.EditComment(comment);

                return RedirectToAction("SinglePost", "Post", new { id = comment.CallbackId });
            }
            ModelState.AddModelError("", "All fields should be filled!");
            return View(comment);
        }

        [Authorize]
        public ActionResult DeleteComment(int id, int postId, int callbackPostId)
        {
            if (_postRepository.GetComment(id) == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            var visiblePost = _displayValidator.IsAvailablePost(postId);
            if (!visiblePost)
            {
                return RedirectToAction("Forbidden", "Error");
            }

            var visibleComment = _displayValidator.IsAvailableComment(id);
            if (!visibleComment)
            {
                return RedirectToAction("Forbidden", "Error");
            }

            var access = _accessValidator.CanAccessComment(id);

            if (!access)
            {
                return RedirectToAction("Forbidden", "Error");
            }

            _postRepository.DeleteComment(id);

            return RedirectToAction("SinglePost", "Post", new { id = callbackPostId });
        }

        [Authorize]
        public ActionResult ReportComment(int id, int postId, int callbackPostId)
        {

            if (_postRepository.GetComment(id) == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            var visiblePost = _displayValidator.IsAvailablePost(postId);
            if (!visiblePost)
            {
                return RedirectToAction("Forbidden", "Error");
            }

            var visibleComment = _displayValidator.IsAvailableComment(id);
            if (!visibleComment)
            {
                return RedirectToAction("Forbidden", "Error");
            }

            var model = _postBuilder.BuildReportCommentViewModel(id, callbackPostId);

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ReportComment(ReportCommentViewModel model, int callbackPostId)
        {
            if (ModelState.IsValid)
            {
                _postRepository.ReportComment(model);

                return RedirectToAction("SinglePost", "Post", new { id = callbackPostId });
            }
            ModelState.AddModelError("", "All fields should be filled!");
            return View(model);
        }
    }
}
