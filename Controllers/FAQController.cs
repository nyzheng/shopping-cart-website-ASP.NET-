using Microsoft.AspNetCore.Mvc;

namespace Shopping.Controllers
{
    public class FAQController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
