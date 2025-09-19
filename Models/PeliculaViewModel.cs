using projectMVC.Models;
using projectMVC.Repositories;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Slugify;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace projectMVC.Models;

/*
*   ViewModel representing a paged collection of movies along with paging information.
*/
public class PeliculaViewModel
{
    /*
    *   The collection of movie entities for the current page.
    */
    public IEnumerable<Pelicula> Peliculas { get; set; }

    /*
    *   Paging metadata including total items, items per page, and current page.
    */
    public PagingInfo PagingInfo { get; set; }
}

/*
*   Holds pagination metadata for use in views and logic.
*/
public class PagingInfo
{
    /*
    *   The total number of items across all pages.
    */
    public int TotalItems { get; set; }

    /*
    *   The number of items displayed per page.
    */
    public int ItemsPerPage { get; set; }

    /*
    *   The current page number being viewed.
    */
    public int CurrentPage { get; set; }

    /*
    *   The total number of pages calculated based on items and page size.
    */
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / ItemsPerPage);
}
