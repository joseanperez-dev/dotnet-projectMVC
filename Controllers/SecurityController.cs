using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using projectMVC.Data;
using projectMVC.Models;
using projectMVC.Repositories;
using projectMVC.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;


namespace projectMVC.Controllers;


/*
*   This controller manages security-related actions for user registration, login, logout,
*   password reset, and protected pages. It uses repository pattern for user management and
*   integrates ASP.NET Core authentication and authorization.
*/
public class SecurityController : Controller
{
    /*
    *   Repository for managing User entities.
    */
    private UserRepository userRepo;

    /*
    *   Stores the CSS class for flash messages shown to the user.
    */
    [TempData]
    public string FlashClass { get; set; }
    /*
    *   Stores the text for flash messages shown to the user.
    */
    [TempData]
    public string FlashMessage { get; set; }


    /*
    *   Constructor that initializes the user repository.
    *
    *   @param context The application's database context.
    */
    public SecurityController(Context context)
    {
        userRepo = new UserRepository(context);
    }

    /*
    *   Displays the default view for the security controller.
    *
    *   @return An IActionResult with the default security view.
    */
    [Route("/security")]
    public IActionResult Index()
    {
        return View();
    }

    /*
    *   Shows the user registration form.
    *
    *   @return An IActionResult with the registration view.
    */
    [Route("/security/register")]
    public IActionResult Register()
    {
        return View();
    }

    /*
    *   Handles the HTTP POST request to register a new user.
    *
    *   @param user The user data submitted via the registration form.
    *   @return Redirects on success or error; shows view if invalid.
    */
    [HttpPost]
    [Route("/security/register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(User user)
    {
        if (ModelState.IsValid)
        {
            User userInDbWithEmail = await userRepo.GetUserByEmail(user.Email);
            if (userInDbWithEmail == null)
            {
                string code = Utils.GenerateToken();
                User newUser = new User()
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = Utils.CreatePassword(user.Password),
                    Token = code,
                    Status = 0
                };
                userRepo.Add(newUser);
                string url = $"http://localhost:5124/security/verfy/{code}";
                //Utils.SendEmail(user.Email, "Verificación", $"<a href='{url}'>{url}</a>");
                FlashClass = "success";
                FlashMessage = "Usuario registrado exitosamente";
                //return RedirectToAction(nameof(Register));
                return RedirectToAction("SecurityRegister", "Security", new { token = code });
            }
            else
            {
                FlashClass = "danger";
                FlashMessage = "El Email ingresado ya existe en la base de datos";
                return RedirectToAction(nameof(Register));
            }
        }
        return View();
    }

    /*
    *   Verifies a user's account using a token from their email.
    *
    *   @param token The verification token.
    *   @return Redirects to the registration view if successful; otherwise, NotFound.
    */
    [Route("/security/verify/{token}")]
    public async Task<IActionResult> SecurityRegister(string token)
    {
        if (token == null)
        {
            return NotFound();
        }
        User userUpdate = await userRepo.GetUserByToken(token);
        if (userUpdate == null)
        {
            return NotFound();
        }
        userUpdate.Token = "";
        userUpdate.Status = 1;
        userRepo.Update(userUpdate);
        FlashClass = "success";
        FlashMessage = "Cuenta verificada correctamente";
        return RedirectToAction(nameof(Register));
    }

    /*
    *   Shows the user login form.
    *
    *   @return An IActionResult with the login view.
    */
    [Route("/security/login")]
    public IActionResult Login()
    {
        return View();
    }

    /*
    *   Handles the HTTP POST request for user login.
    *
    *   @param model The user credentials submitted via the login form.
    *   @return Redirects on success or error; shows view if invalid.
    */
    [HttpPost]
    [Route("/security/login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(User model)
    {
        if (ModelState.IsValid)
        {
            User user = await userRepo.GetUser(model.Email, Utils.CreatePassword(model.Password));
            if (user == null)
            {
                FlashClass = "danger";
                FlashMessage = "Las credenciales ingresadas no son válidas";
                return RedirectToAction(nameof(Login));
            }
            List<Claim> claims = new List<Claim>()
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("Name", user.Name)
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
            return RedirectToAction(nameof(ProtectedPage));
        }
        return View();
    }

    /*
    *   Displays a protected page that requires authentication.
    *
    *   @return An IActionResult with the protected view.
    */
    //[Authorize]
    [Route("/security/page")]
    public IActionResult ProtectedPage()
    {
        return View();
    }

    /*
    *   Logs the current user out of the application.
    *
    *   @return Redirects to the login view after logout.
    */
    [Route("/security/logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        FlashClass = "success";
        FlashMessage = "Se ha cerrado la sesión existosamente";
        return RedirectToAction(nameof(Login));
    }

    /*
    *   Shows the view to reset a user's password.
    *
    *   @return An IActionResult with the reset password form.
    */
    [Route("/security/reset")]
    public IActionResult ResetPassword()
    {
        return View();
    }

    /*
    *   Handles the HTTP POST request to reset a user's password.
    *
    *   @param model The user data submitted for password reset.
    *   @return Redirects or shows the view depending on validity.
    */
    [HttpPost]
    [Route("/security/reset")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(User model)
    {
        if (ModelState.IsValid)
        {
            User user = await userRepo.GetUserActiveByEmail(model.Email);
            if (user == null)
            {
                FlashClass = "danger";
                FlashMessage = "Las credenciales ingresadas no son válidas o el usuario no existe";
                return RedirectToAction(nameof(Login));
            }
            user.Password = Utils.CreatePassword(model.Password);
            userRepo.Update(user);
            FlashClass = "success";
            FlashMessage = "Se ha cambiado correctamente la contraseña";
            return RedirectToAction(nameof(ResetPassword));
        }
        return View();
    }

    /*
    *   Displays the password reset page via verification token.
    *
    *   @param token The reset token sent to the user's email.
    *   @return The view with user info or NotFound.
    */
    [Route("/security/reset/{token}")]
    public async Task<IActionResult> ResetPasswordPage(string token)
    {
        if (token == null)
        {
            return NotFound();
        }
        User user = await userRepo.GetUserActiveByToken(token);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    /*
    *   Handles the HTTP POST request to reset user's password using a verification token.
    *
    *   @param token The reset token sent to the user's email.
    *   @param model The user data provided on the change password form.
    *   @return Redirects after successful reset or shows view with errors.
    */
    [HttpPost]
    [Route("/security/reset/{token}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPasswordPage(string token, User model)
    {
        if (token == null)
        {
            return NotFound();
        }
        User user = await userRepo.GetUserActiveByToken(token);
        if (user == null)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            user.Token = "";
            user.Password = Utils.CreatePassword(model.Password);
            userRepo.Update(user);
            FlashClass = "success";
            FlashMessage = "Se ha cambiado correctamente la contraseña";
            return RedirectToAction(nameof(ResetPasswordPage));
        }
        return View(user);
    }
}
