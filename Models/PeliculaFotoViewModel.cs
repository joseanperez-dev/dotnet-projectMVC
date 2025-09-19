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
*   ViewModel for managing movie images and their collection in views.
*   Contains a single movie photo entity and a collection of photos.
*/
public class PeliculaFotoViewModel
{
    /*
    *   The individual movie photo entity being added or edited.
    */
    public PeliculaFoto PeliculaFoto { get; set; }

    /*
    *   A collection of movie photos, typically for listing in views.
    */
    public IEnumerable<PeliculaFoto>? PeliculasFotos { get; set; }
}
