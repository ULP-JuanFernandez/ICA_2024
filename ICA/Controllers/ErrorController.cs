using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICA.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{code}")]
        public IActionResult Index(int code)
        {
            switch (code)
            {
                case 404:
                    return View("404");
                case 405:
                    return View("405");
                default:
                    return View("Error"); 
            }
        }
    }
}
