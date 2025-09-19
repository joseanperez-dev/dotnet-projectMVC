using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using projectMVC.Models;


namespace projectMVC.Controllers;


/*
*   This controller manages error-related actions of the application.
*   It handles custom error pages for specific HTTP error codes.
*/
public class ErrorController : Controller
{
    /*
    *   Displays the custom view for 404 (Not Found) errors.
    *
    *   @return An IActionResult which shows the 404 error page.
    */
    [Route("error/404")]
    public IActionResult Error404()
    {
        return View();
    }
}
