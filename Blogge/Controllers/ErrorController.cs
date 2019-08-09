
using System.Web.Mvc;

namespace Blogge.Web.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        [HandleError]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NotFound()
        {
            int status = 404;
            return View("error", status);
        }
        public ActionResult Forbidden()
        {
            int status = 403;
            return View("error", status);
        }
        public ActionResult Unauthorized()
        {
            int status = 401;
            return View("error", status);
        }
        public ActionResult ServerError()
        {
            int status = 500;
            return View("error", status);
        }
        public ActionResult BadRequest()
        {
            int status = 400;
            return View("error", status);
        }
        //default 
        public ActionResult Error()
        {
            int status = 400;
            return View("error", status);
        }
    }
}