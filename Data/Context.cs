using projectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace projectMVC.Data;

/*
*   This class represents the database context of the application.
*   It inherits from DbContext and defines the entity sets (tables) in the database.
*/
public class Context : DbContext
{
    /*
    *   Constructor that receives DbContextOptions and configures the database connection.
    *
    *   @param options Options used to configure the DbContext
    */
    public Context(DbContextOptions<Context> options) : base(options)
    {

    }

    /*
    *    Represents the Categories table in the database.
    */
    public DbSet<Category> Categories { get; set; }
    /*
    *    Represents the Products table in the database.
    */
    public DbSet<Product> Products { get; set; }

    /*
    *    Represents the ProductImages table in the database,
    *    containing images associated with products.
    */
    public DbSet<ProductImage> ProductsImages { get; set; }


    // For these tables I used a database in Spanish, so everything related to them is in Spanish too to avoid confusion.
    /*
    *    Represents the Tematicas table,
    *    likely used to categorize movies by themes.
    */
    public DbSet<Tematica> Tematicas { get; set; }

    /*
    *    Represents the Peliculas table in the database.
    */
    public DbSet<Pelicula> Peliculas { get; set; }

    /*
    *    Represents the PeliculasFotos table,
    *    containing images associated with movies.
    */
    public DbSet<PeliculaFoto> PeliculasFotos { get; set; }

    /*
    *    Represents the VariablesGlobales table,
    *    used to store global configuration or application settings.
    */
    public DbSet<VariableGlobal> VariablesGlobales { get; set; }

    /*
    *    Represents the Users table,
    *    containing user account information.
    */
    public DbSet<User> Users { get; set; }
}