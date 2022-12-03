using Microsoft.AspNetCore.Mvc;

namespace BuyerAPI.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
