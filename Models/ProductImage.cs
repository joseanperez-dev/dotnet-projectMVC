using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectMVC.Models;

/*
*   Represents an image associated with a product.
*/
public class ProductImage
{
    /*
    *   The unique identifier of the product image.
    */
    [Key]
    public int Id { get; set; }

    /*
    *   The URL or file name of the image.
    */
    [DataType(DataType.ImageUrl)]
    [Display(Name = "Foto")]
    public string? Name { get; set; }

    /*
    *   The foreign key ID linking this image to a product.
    */
    public int? ProductId { get; set; }

    /*
    *   Navigation property to the associated product entity.
    */
    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }
}
