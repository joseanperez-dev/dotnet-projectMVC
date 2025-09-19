using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using projectMVC.Models;

namespace projectMVC.Controllers
{
    /*
     *    HomeController handles the main routes of the application.
     *    It inherits from Controller, providing functionality to return views and content.
     */
    public class HomeController : Controller
    {
        /*
         *    Handles the root URL "/" of the application.
         *    Returns the default view associated with the Index action.
         */
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
