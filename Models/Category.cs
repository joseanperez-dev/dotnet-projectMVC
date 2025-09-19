using System;
using System.ComponentModel.DataAnnotations;

namespace projectMVC.Models;

/*
*   Represents a category entity with an ID and a required name.
*/
public class Category
{
    /*
    *   The unique identifier of the category.
    */
    [Key]
    public int Id { get; set; }

    /*
    *   The name of the category. This field is required.
    */
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Name { get; set; }
}