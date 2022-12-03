using Microsoft.AspNetCore.Mvc;

namespace BuyerAPI.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
