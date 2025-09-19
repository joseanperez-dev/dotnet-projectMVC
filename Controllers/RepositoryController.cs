using projectMVC.Data;
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


namespace projectMVC.Controllers;


/*
*   This controller manages repository-based operations for Thematic categories and Movies, including
*   CRUD features and image management for movies using repository pattern.
*/
public class RepositoryController : Controller
{
    /*
    *   Provides access to the web host environment for file system operations.
    */
    private readonly IWebHostEnvironment hostEnv;

    /*
    *   Repository for managing 'Tematica' entities (themes).
    */
    private readonly TematicaRepository tematicaRepo;
    /*
    *   Repository for managing 'Pelicula' entities (movies).
    */
    private readonly PeliculaRepository peliculaRepo;
    /*
    *   Repository for managing 'PeliculaFoto' entities (movie images).
    */
    private readonly PeliculaFotoRepository peliFotoRepo;

    /*
    *   Stores the CSS class for flash messages to display to the user.
    */
    [TempData]
    public string FlashClass { get; set; }
    /*
    *   Stores the message text for flash messages to display to the user.
    */
    [TempData]
    public string FlashMessage { get; set; }

    /*
    *   Constructor that initializes the repositories and hosting environment.
    *
    *   @param hostEnv Provides the web hosting environment.
    *   @param context The application's database context for repository instantiation.
    */
    public RepositoryController(IWebHostEnvironment hostEnv, Context context)
    {
        this.hostEnv = hostEnv;
        this.tematicaRepo = new TematicaRepository(context);
        this.peliculaRepo = new PeliculaRepository(context);
        this.peliFotoRepo = new PeliculaFotoRepository(context);
    }

    /*
    *   Displays the default view for the repository controller.
    *
    *   @return An IActionResult with the default repository view.
    */
    [Route("/repository")]
    public IActionResult Index()
    {
        return View();
    }

    /*
    *   Retrieves and displays a list of all thematic categories (tematicas).
    *
    *   @return An IActionResult containing the thematic categories view.
    */
    [Route("/repository/tematicas")]
    public async Task<IActionResult> ListTematicas()
    {
        var tematicas = tematicaRepo.GetAll();
        return View(tematicas);
    }

    /*
    *   Shows the view to add a new thematic category.
    *
    *   @return An IActionResult with the add thematic category view.
    */
    [Route("/repository/tematicas/add")]
    public IActionResult AddTematica()
    {
        return View();
    }

    /*
    *   Handles the HTTP POST request to add a new thematic category.
    *
    *   @param model The thematic category data submitted by the user.
    *   @return Redirects to the add tematica view on success; otherwise, shows the same view.
    */
    [HttpPost]
    [Route("/repository/tematicas/add")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddTematica(Tematica model)
    {
        if (ModelState.IsValid)
        {
            Tematica tematica = new Tematica
            {
                Nombre = model.Nombre,
                Slug = new SlugHelper().GenerateSlug(model.Nombre)
            };
            tematicaRepo.Add(tematica);
            FlashClass = "success";
            FlashMessage = "Se creó el registro exitosamente";
            return RedirectToAction(nameof(AddTematica));
        }
        return View();
    }

    /*
    *   Shows the view to edit an existing thematic category.
    *
    *   @param id The ID of the thematic category to edit.
    *   @return The edit view if the category exists; otherwise, NotFound.
    */
    [Route("/repository/tematicas/edit/{id}")]
    public IActionResult EditTematica(int id)
    {
        if (id == null)
        {
            return NotFound();
        }
        Tematica tematica = tematicaRepo.GetById(id);
        if (tematica == null)
        {
            return NotFound();
        }
        return View(tematica);
    }

    /*
    *   Handles the HTTP POST request to update a thematic category.
    *
    *   @param model The updated data for the thematic category.
    *   @param id The ID of the thematic category to update.
    *   @return Redirects on success; otherwise, shows the view with errors.
    */
    [HttpPost]
    [Route("/repository/tematicas/edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditTematica(Tematica model, int id)
    {
        if (ModelState.IsValid)
        {
            Tematica tematicaUpdate = tematicaRepo.GetById(id);
            tematicaUpdate.Nombre = model.Nombre;
            tematicaUpdate.Slug = new SlugHelper().GenerateSlug(model.Nombre);
            tematicaRepo.Update(tematicaUpdate);
            FlashClass = "success";
            FlashMessage = "Se modificó el registro exitosamente";
            return RedirectToAction(nameof(EditTematica));
        }
        if (id == null) return NotFound();
        Tematica tematica = tematicaRepo.GetById(id);
        if (tematica == null) return NotFound();
        return View();
    }

    /*
    *   Deletes a thematic category from the repository.
    *
    *   @param id The ID of the thematic category to delete.
    *   @return Redirects to the tematicas list if successful; otherwise, NotFound.
    */
    [Route("/repository/tematicas/delete/{id}")]
    public async Task<IActionResult> DeleteTematica(int id)
    {
        if (id == null)
        {
            return NotFound();
        }
        tematicaRepo.Delete(id);
        FlashClass = "success";
        FlashMessage = "Se eliminó el registro exitosamente";
        return RedirectToAction(nameof(ListTematicas));
    }

    /*
    *   Shows the view to add a new movie, including the list of available thematic categories.
    *
    *   @return An IActionResult with the add movie view.
    */
    [Route("/repository/peliculas/add")]
    public IActionResult AddPelicula()
    {
        PeliculaAddEditViewModel model = new PeliculaAddEditViewModel();
        model.Pelicula = new Pelicula();
        model.Tematicas = tematicaRepo.GetAll().Select(i => new SelectListItem()
        {
            Text = i.Nombre,
            Value = i.Id.ToString()
        });
        return View(model);
    }

    /*
    *   Handles the HTTP POST request to add a new movie.
    *
    *   @param viewModel The view model containing the movie data and categories.
    *   @return Redirects to the add movie view if successful; otherwise, shows the view with validation errors.
    */
    [HttpPost]
    [Route("/repository/peliculas/add")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPelicula(PeliculaAddEditViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            Pelicula pelicula = new Pelicula
            {
                Nombre = viewModel.Pelicula.Nombre,
                Slug = new SlugHelper().GenerateSlug(viewModel.Pelicula.Nombre),
                Descripcion = viewModel.Pelicula.Descripcion,
                Fecha = DateTime.Now,
                Tematica = tematicaRepo.GetById(viewModel.Pelicula.TematicaId)
            };
            peliculaRepo.Add(pelicula);
            FlashClass = "success";
            FlashMessage = "Se creó el registro exitosamente";
            return RedirectToAction(nameof(AddPelicula));
        }
        PeliculaAddEditViewModel model = new PeliculaAddEditViewModel();
        model.Pelicula = new Pelicula();
        model.Tematicas = tematicaRepo.GetAll().Select(i => new SelectListItem()
        {
            Text = i.Nombre,
            Value = i.Id.ToString()
        });
        return View(model);
    }

    /*
    *   Shows the view to edit an existing movie and its categories.
    *
    *   @param id The ID of the movie to edit.
    *   @return The edit movie view if found; otherwise, NotFound.
    */
    [Route("/repository/peliculas/edit/{id}")]
    public IActionResult EditPelicula(int id)
    {
        if (id == null)
        {
            return NotFound();
        }
        Pelicula pelicula = peliculaRepo.GetById(id);
        if (pelicula == null)
        {
            return NotFound();
        }
        PeliculaAddEditViewModel model = new PeliculaAddEditViewModel();
        model.Pelicula = pelicula;
        model.Tematicas = tematicaRepo.GetAll().Select(i => new SelectListItem()
        {
            Text = i.Nombre,
            Value = i.Id.ToString()
        });
        return View(model);
    }

    /*
    *   Handles the HTTP POST request to update a movie.
    *
    *   @param viewModel The view model containing updated movie data and categories.
    *   @return Redirects on success; otherwise, shows the view with errors.
    */
    [HttpPost]
    [Route("/repository/peliculas/edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPelicula(PeliculaAddEditViewModel viewModel)
    {
        Pelicula pelicula = viewModel.Pelicula;
        if (pelicula.Tematica != null)
        {
            Console.WriteLine(pelicula.Tematica);
        }
        else
        {
            Console.WriteLine("ADIOS");
        }
        if (ModelState.IsValid)
        {
            Pelicula peliculaUpdate = peliculaRepo.GetById(pelicula.Id);
            peliculaUpdate.Nombre = pelicula.Nombre;
            peliculaUpdate.Slug = new SlugHelper().GenerateSlug(pelicula.Nombre);
            peliculaUpdate.Descripcion = pelicula.Descripcion;
            Console.WriteLine(pelicula.TematicaId);
            peliculaUpdate.Tematica = tematicaRepo.GetById(pelicula.TematicaId);
            peliculaRepo.Update(peliculaUpdate);
            FlashClass = "success";
            FlashMessage = "Se actualizó el registro exitosamente";
            return RedirectToAction(nameof(EditPelicula));
        }
        PeliculaAddEditViewModel model = new PeliculaAddEditViewModel();
        model.Pelicula = peliculaRepo.GetById(pelicula.Id);
        model.Tematicas = tematicaRepo.GetAll().Select(i => new SelectListItem()
        {
            Text = i.Nombre,
            Value = i.Id.ToString()
        });
        return View(model);
    }

    /*
    *   Deletes a movie from the repository.
    *
    *   @param id The ID of the movie to delete.
    *   @return Redirects to the movies list if successful; otherwise, NotFound.
    */
    [Route("/repository/peliculas/delete/{id}")]
    public async Task<IActionResult> DeletePelicula(int id)
    {
        if (id == null)
        {
            return NotFound();
        }
        peliculaRepo.Delete(id);
        FlashClass = "success";
        FlashMessage = "Se eliminó el registro exitosamente";
        return RedirectToAction(nameof(ListPeliculas));
    }

    /*
    *   Retrieves and displays a paginated list of all movies.
    *
    *   @param page The page number for pagination.
    *   @return An IActionResult with the paginated movies view.
    */
    [Route("/repository/peliculas")]
    public IActionResult ListPeliculas(int page = 1)
    {
        int pageSize = 3;
        int listSize = peliculaRepo.GetAll().Count();
        int pages = listSize / pageSize;
        int rest = listSize % pageSize;
        var pagedPeliculas = peliculaRepo.GetPagedPeliculas(page, pageSize);
        PeliculaViewModel viewModel = new PeliculaViewModel
        {
            Peliculas = pagedPeliculas,
            PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItems = pagedPeliculas.Count()
            }
        };
        pages = (rest >= 1) ? pages + 1 : pages;
        ViewBag.Pages = pages;
        return View(viewModel);
    }

    /*
    *   Retrieves and displays a paginated list of movies filtered by thematic category.
    *
    *   @param tematicaId The ID of the thematic category.
    *   @param page The page number for pagination.
    *   @return An IActionResult with the paginated view.
    */
    [Route("/repository/peliculas/tematica/{tematicaId}")]
    public IActionResult ListPeliculasPorTematica(int tematicaId, int page = 1)
    {
        int pageSize = 3;
        var peliculasPorTematica = peliculaRepo.GetAllByTematica(tematicaId);
        int listSize = peliculasPorTematica.Count();
        int pages = listSize / pageSize;
        int rest = listSize % pageSize;
        var pagedPeliculasPorTematica = peliculaRepo.GetPagedPeliculasPorTematica(tematicaId, page, pageSize);
        foreach (var pagedPelicula in pagedPeliculasPorTematica)
        {
            Console.WriteLine(pagedPelicula.Nombre + " (" + pagedPelicula.Tematica.Nombre + ")");
        }
        PeliculaViewModel viewModel = new PeliculaViewModel
        {
            Peliculas = pagedPeliculasPorTematica,
            PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItems = pagedPeliculasPorTematica.Count()
            }
        };
        pages = (rest >= 1) ? pages + 1 : pages;
        ViewBag.Pages = pages;
        ViewBag.TematicaId = tematicaId;
        ViewBag.TematicaNombre = tematicaRepo.GetById(tematicaId).Nombre;
        return View(viewModel);
    }

    /*
    *   Retrieves and displays a paginated list of movies filtered by a search key.
    *
    *   @param searchKey The search query for movies.
    *   @param page The page number for pagination.
    *   @return An IActionResult with the view of search results or all movies if searchKey is null.
    */
    [Route("/repository/peliculas/search")]
    public IActionResult ListPeliculasPorBuscador([FromQuery(Name = "searcher")] string searchKey, int page = 1)
    {
        if (searchKey != null)
        {
            int pageSize = 3;
            var peliculas = peliculaRepo.GetAllBuscador(searchKey);
            int listSize = peliculas.Count();
            int pages = listSize / pageSize;
            int rest = listSize % pageSize;
            var pagedPeliculas = peliculaRepo.GetPagedPeliculasBuscador(searchKey, page, pageSize);
            foreach (var pagedPelicula in pagedPeliculas)
            {
                Console.WriteLine(pagedPelicula.Nombre);
            }
            PeliculaViewModel viewModel = new PeliculaViewModel
            {
                Peliculas = pagedPeliculas,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = pagedPeliculas.Count()
                }
            };
            pages = (rest >= 1) ? pages + 1 : pages;
            ViewBag.Pages = pages;
            ViewBag.SearchKey = searchKey;
            return View(viewModel);
        }
        else
        {
            return RedirectToAction(nameof(ListPeliculas));
        }
    }

    /*
    *   Retrieves and displays all images associated with a specific movie.
    *
    *   @param peliculaId The ID of the movie.
    *   @return The movie images view model.
    */
    [Route("/repository/peliculas/fotos/{peliculaId}")]
    public IActionResult ListPeliculasFotos(int peliculaId)
    {
        if (peliculaId == null)
        {
            return NotFound();
        }
        Pelicula pelicula = peliculaRepo.GetById(peliculaId);
        if (pelicula == null)
        {
            return NotFound();
        }
        ViewBag.Nombre = pelicula.Nombre;
        ViewBag.Id = peliculaId;
        PeliculaFoto peliculaFoto = new PeliculaFoto();
        var peliculasFotos = peliFotoRepo.GetFotosByPelicula(peliculaId);
        PeliculaFotoViewModel viewModel = new PeliculaFotoViewModel
        {
            PeliculaFoto = peliculaFoto,
            PeliculasFotos = peliculasFotos
        };
        return View(viewModel);
    }

    /*
    *   Handles the HTTP POST request to upload a new image for a movie.
    *
    *   @param id The ID of the movie.
    *   @param viewModel The view model containing the image and related movie info.
    *   @return Redirects back to the images view if successful; otherwise, shows the same view.
    */
    [HttpPost]
    [Route("/repository/peliculas/fotos/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ListPeliculasFotos(int id, PeliculaFotoViewModel viewModel)
    {
        Console.WriteLine("LLEGO HASTA AQUI");
        Console.WriteLine(viewModel.PeliculaFoto.PeliculaId);
        Console.WriteLine(viewModel.PeliculaFoto.Pelicula == null);

        if (ModelState.IsValid)
        {
            string principalPath = hostEnv.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            Console.WriteLine("HOLA");
            Console.WriteLine(files.Count);
            long timeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            string fileName = "image_" + timeStamp;
            Console.WriteLine(fileName);
            var uploads = Path.Combine(principalPath, @"uploads/peliculas");
            Console.WriteLine("Uploads: " + uploads);
            var extension = Path.GetExtension(files[0].FileName);
            Console.WriteLine($"archivo = {fileName} | extension = {extension}");
            string filePath = Path.Combine(uploads, fileName + extension);
            if (!System.IO.File.Exists(filePath))
            {
                Console.WriteLine("Entro aquí! " + !System.IO.File.Exists(filePath));
                using (var fileStreams = new FileStream(filePath, FileMode.Create))
                {
                    files[0].CopyTo(fileStreams);
                }
                PeliculaFoto peliculaFoto = new PeliculaFoto
                {
                    Nombre = fileName + extension,
                    Pelicula = peliculaRepo.GetById(viewModel.PeliculaFoto.PeliculaId)
                };
                peliFotoRepo.Add(peliculaFoto);
                FlashClass = "success";
                FlashMessage = "Se creó el registro exitosamente";
                return RedirectToAction(nameof(ListPeliculasFotos));
            }
        }
        if (id == null)
        {
            return NotFound();
        }
        PeliculaFotoViewModel model = new PeliculaFotoViewModel();
        model.PeliculaFoto = new PeliculaFoto();
        Pelicula pelicula = peliculaRepo.GetById(id);
        if (pelicula == null)
        {

        }
        ViewBag.Nombre = pelicula.Nombre;
        ViewBag.Id = id;
        model.PeliculasFotos = peliFotoRepo.GetFotosByPelicula(pelicula.Id);
        return View(model);
    }

    /*
    *   Deletes an image associated with a movie from both the repository and file system.
    *
    *   @param id The ID of the movie image to delete.
    *   @return Redirects to the images view for the related movie if successful; otherwise, NotFound.
    */
    [Route("/repository/peliculas/fotos/delete/{id}")]
    public async Task<IActionResult> DeleteProductsImages(int id)
    {
        if (id == null)
        {
            return NotFound();
        }
        PeliculaFoto peliculaFoto = peliFotoRepo.GetById(id);
        if (peliculaFoto == null)
        {
            return NotFound();
        }
        int peliculaId = peliculaFoto.PeliculaId;
        string fotoNombre = peliculaFoto.Nombre;
        peliFotoRepo.Delete(peliculaFoto.Id);
        string principalPath = hostEnv.WebRootPath;
        var imagePath = Path.Combine(principalPath, @"uploads/peliculas/" + fotoNombre);
        if (System.IO.File.Exists(imagePath))
        {
            System.IO.File.Delete(imagePath);
        }
        FlashClass = "success";
        FlashMessage = "Se eliminó el registro exitosamente";
        return RedirectToAction("ListPeliculasFotos", "repository", new
        {
            id = peliculaId
        });
    }
}
