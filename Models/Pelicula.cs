using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectMVC.Models;

/*
*   Represents a movie (Pelicula) entity with details for database and display.
*/
public class Pelicula
{
    /*
    *   The unique identifier of the movie.
    */
    [Key]
    public int Id { get; set; }

    /*
    *   The name of the movie. This field is required.
    */
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; }

    /*
    *   A URL-friendly version of the movie name.
    */
    public string? Slug { get; set; }

    /*
    *   Description of the movie. This field is required.
    */
    [Display(Name = "Descripción")]
    [Required(ErrorMessage = "La descripción es obligatoria")]
    public string Descripcion { get; set; }

    /*
    *   The release or relevant date of the movie.
    */
    [Display(Name = "Fecha")]
    public DateTime Fecha { get; set; }

    /*
    *   The foreign key for the associated "Tematica" (theme).
    */
    [Display(Name = "Temática")]
    [Required(ErrorMessage = "La temática es obligatoria")]
    public int TematicaId { get; set; }

    /*
    *   Navigation property for the associated Tematica entity.
    */
    [ForeignKey("TematicaId")]
    public virtual Tematica? Tematica { get; set; }
}
