using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace projectMVC.Models;

/*
*   Represents a user entity with authentication details and validation.
*/
public class User
{
    /*
    *   The unique identifier of the user.
    */
    [Key]
    public int Id { get; set; }

    /*
    *   The user's full name. This field is required.
    */
    [Display(Name = "Name")]
    [Required(ErrorMessage = "El campo Name es obligatorio")]
    public string Name { get; set; }

    /*
    *   The user's email address. This field is required and must be a valid email format.
    */
    [Display(Name = "Email")]
    [Required(ErrorMessage = "El campo Email es obligatorio")]
    [EmailAddress(ErrorMessage = "El campo Email ingresado no es válido")]
    public string Email { get; set; }

    /*
    *   The user's password. Must contain at least one uppercase letter, one lowercase letter, one number,
    *   and be between 6 and 20 characters long.
    */
    [Display(Name = "Password")]
    [Required(ErrorMessage = "El campo Password es obligatorio")]
    [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{6,20}$",
    ErrorMessage = "La contraseña debe tener al menos 1 número, una mayúscula, y un largo de 6 y 20 capracteres")]
    public string Password { get; set; }

    /*
    *   The status of the user account (e.g., active, inactive).
    */
    public int Status { get; set; }

    /*
    *   A token used for verification or password reset processes.
    */
    public string? Token { get; set; }
}
