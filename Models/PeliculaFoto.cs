using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectMVC.Models;

/*
*   Represents a image associated with a movie (Pelicula).
*/
public class PeliculaFoto
{
    /*
    *   The unique identifier of the movie photo.
    */
    [Key]
    public int Id { get; set; }
    
    /*
    *   The file name or URL of the image.
    */
    [Display(Name = "Nombre")]
    [DataType(DataType.ImageUrl)]
    public string? Nombre { get; set; }

    /*
    *   The foreign key ID of the associated movie.
    */
    public int PeliculaId { get; set; }

    /*
    *   Navigation property to the associated movie entity.
    */
    [ForeignKey("PeliculaId")]
    public virtual Pelicula? Pelicula { get; set; }
}