using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace projectMVC.Models;

/*
*   ViewModel for managing product data and associated categories in views.
*   Contains a single product entity and a list of categories for selection.
*/
public class ViewModel
{
    /*
    *   The product entity being added or edited.
    */
    public Product Product { get; set; }

    /*
    *   List of categories to populate a dropdown or selection list in the view.
    */
    public IEnumerable<SelectListItem>? Categories { get; set; }
}

