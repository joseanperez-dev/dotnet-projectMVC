using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectMVC.Models;

/*
*   Represents a product entity with details for database storage and display.
*/
public class Product
{
    /*
    *   The unique identifier of the product.
    */
    [Key]
    public int Id { get; set; }

    /*
    *   The name of the product. This field is required.
    */
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string? Name { get; set; }

    /*
    *   A URL-friendly version of the product name.
    */
    public string? Slug { get; set; }

    /*
    *   A description of the product.
    */
    [Display(Name = "Descripction")]
    public string Descripction { get; set; }

    /*
    *   The price of the product. Must be between 1 and 1,000,000.
    */
    [Range(1, 1000000, ErrorMessage = "El precio no es valido, debe ser mayor o igual a 1")]
    public int Prize { get; set; }

    /*
    *   The quantity of product in stock.
    */
    public int Stock { get; set; }

    /*
    *   The date associated with the product record (e.g., creation date).
    */
    public DateTime? Date { get; set; }

    /*
    *   The foreign key ID of the category this product belongs to. This field is required.
    */
    [Display(Name = "Categoría")]
    [Required(ErrorMessage = "La categría es obligatoria")]
    public int? CategoryId { get; set; }

    /*
    *   Navigation property to the associated category entity.
    */
    [ForeignKey("CategoryId")]
    public virtual Category? Category { get; set; }
}
