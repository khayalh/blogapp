
using System.Web.Mvc;
using Blogge.Interfaces.Builders.Model;
using Blogge.Models.ViewModels;

namespace Blogge.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private readonly IPostBuilder _postBuilder;

        public HomeController(IPostBuilder postBuilder)
        {
            _postBuilder = postBuilder;
        }

        [ValidateInput(false)]
        public ActionResult Index()
        {
            var model = _postBuilder.BuildHomepageViewModel();
            return View(model);
        }

        [ValidateInput(false)]
        [ActionName("CustomIndex")]
        public ActionResult Index(int postCount)
        {
            var model = _postBuilder.BuildHomepageViewModel(postCount);
            return View("Index", model);
        }

        [ValidateInput(false)]
        [ActionName("Search")]
        [HttpGet]
        public ActionResult Index(SearchViewModel searchModel)
        {
            var model = _postBuilder.BuildHomepageViewModel(searchModel.Search);
            return View("Index", model);
        }
    }
}