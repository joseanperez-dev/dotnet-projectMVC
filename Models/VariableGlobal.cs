using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace projectMVC.Models;

/*
*   Represents a global variable entity for application-wide configuration or settings.
*/
public class VariableGlobal
{
    /*
    *   The unique identifier of the global variable.
    */
    [Key]
    public int Id { get; set; }

    /*
    *   The name of the global variable.
    */
    public string Nombre { get; set; }

    /*
    *   The value associated with the global variable.
    */
    public string Valor { get; set; }
}
