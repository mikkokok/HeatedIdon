using Microsoft.AspNetCore.Mvc;

namespace HeatedIdon.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        [Route("[controller]")]
        [HttpGet]
        public ActionResult Index()
        {
            return new RedirectResult("~/");
        }
    }
}