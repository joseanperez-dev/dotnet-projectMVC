using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace projectMVC.Models;

/*
*   Represents a thematic category entity used to classify movies or other content.
*/
public class Tematica
{
    /*
    *   The unique identifier of the thematic category.
    */
    [Key]
    public int Id { get; set; }

    /*
    *   The name of the thematic category. This field is required.
    */
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; }

    /*
    *   A URL-friendly slug generated from the category name.
    */
    public string? Slug { get; set; }
}
