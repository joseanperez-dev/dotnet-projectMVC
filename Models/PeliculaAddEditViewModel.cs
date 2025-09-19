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
*   ViewModel used for adding or editing a movie.
*   Contains the movie entity and a list of thematic categories for selection.
*/
public class PeliculaAddEditViewModel
{
    /*
    *   The movie entity being added or edited.
    */
    public Pelicula Pelicula { get; set; }
    
    /*
    *   List of thematic categories to populate a dropdown or selection list in the view.
    */
    public IEnumerable<SelectListItem>? Tematicas { get; set; }
}
