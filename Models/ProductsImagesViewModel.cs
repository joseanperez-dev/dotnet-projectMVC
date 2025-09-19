using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace projectMVC.Models;

/*
*   ViewModel used to manage product images in views.
*   Contains a single product image entity and a collection of images.
*/
public class ProductsImagesViewModel
{
    /*
    *   The individual product image being added or edited.
    */
    public ProductImage ProductImage { get; set; }

    /*
    *   A collection of product images, typically for display in lists.
    */
    public IEnumerable<ProductImage>? ProductsImages { get; set; }
}
