using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projectMVC.Models;
using projectMVC.Data;
using projectMVC.Pagination;
using Slugify;
using System.IO;
using projectMVC.Controllers;


namespace projectMVC.Controllers;

/*
*   This controller manages operations related to entities such as categories, products, and their images.
*   It interacts with the database context and the web host environment.
*/
public class EntityController : Controller
{
    /*
    *   The database context used for accessing and managing data in the application.
    */
    private readonly Context context;
    /*
    *   Provides information about the web hosting environment.
    */
    private readonly IWebHostEnvironment hostingEnv;

    /*
    *   Stores the CSS class for flash messages shown to the user.
    */
    [TempData]
    public string FlashClass { get; set; }
    /*
    *   Stores the content of flash messages shown to the user.
    */
    [TempData]
    public string FlashMessage { get; set; }

    /*
    *   Constructor that receives the database context and hosting environment instances.
    *
    *   @param context The application's database context.
    *   @param hostingEnv The web hosting environment.
    */
    public EntityController(Context context, IWebHostEnvironment hostingEnv)
    {
        this.context = context;
        this.hostingEnv = hostingEnv;
    }

    /*
    *   Displays the default view for the entity controller.
    *
    *   @return An IActionResult with the default view.
    */
    [Route("/entity")]
    public IActionResult Index()
    {
        return View();
    }

    /*
    *   Retrieves and displays a list of all categories ordered by ID in descending order.
    *
    *   @return An IActionResult containing the categories list view.
    */
    [Route("/entity/categories")]
    public async Task<IActionResult> ListCategories()
    {
        var categories = await context.Categories.OrderByDescending(c => c.Id).ToListAsync();
        return View(categories);
    }

    /*
    *   Displays the view to add a new category.
    *
    *   @return An IActionResult with the add category view.
    */
    [Route("/entity/categories/add")]
    public IActionResult AddCategories()
    {
        return View();
    }

    /*
    *   Handles the HTTP POST request to add a new category.
    *
    *   @param model The category data submitted by the user.
    *   @return Redirects to the add category view if successful; otherwise, returns the same view.
    */
    [HttpPost]
    [Route("/entity/categories/add")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddCategories(Category model)
    {
        if (ModelState.IsValid)
        {
            Category category = new Category
            {
                Name = model.Name
                //, Slug = new SlugHelper().GenerateSlug(model.Name)
            };
            context.Add(category);
            await context.SaveChangesAsync();
            FlashClass = "success";
            FlashMessage = "Se creó el registro exitosamente";
            return RedirectToAction(nameof(AddCategories));
        }
        return View();
    }

    /*
    *   Displays the view to edit an existing category.
    *
    *   @param id The ID of the category to edit.
    *   @return The edit category view if found; otherwise, NotFound.
    */
    [Route("/entity/categories/edit/{id}")]
    public IActionResult EditCategories(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var category = context.Categories.Find(id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    /*
    *   Handles the HTTP POST request to update a category.
    *
    *   @param model The updated category data.
    *   @param id The ID of the category to update.
    *   @return Redirects to the edit view if successful; otherwise, returns the same view if not valid or not found.
    */
    [HttpPost]
    [Route("/entity/categories/edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCategories(Category model, int? id)
    {
        if (ModelState.IsValid)
        {
            Category categoryUpdate = context.Categories.Find(id);
            categoryUpdate.Name = model.Name;
            context.Update(categoryUpdate);
            await context.SaveChangesAsync();
            FlashClass = "success";
            FlashMessage = "Se modificó el registro exitosamente";
            return RedirectToAction(nameof(EditCategories));
        }
        if (id == null)
        {
            return NotFound();
        }
        var category = context.Categories.Find(id);
        if (category == null)
        {
            return NotFound();
        }
        return View();
    }

    /*
    *   Deletes a category from the database.
    *
    *   @param id The ID of the category to delete.
    *   @return Redirects to the categories list view if successful; otherwise, NotFound.
    */
    [Route("/entity/categories/delete/{id}")]
    public async Task<IActionResult> DeleteCategories(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var category = context.Categories.Find(id);
        if (category == null)
        {
            return NotFound();
        }
        context.Categories.Remove(category);
        await context.SaveChangesAsync();
        FlashClass = "success";
        FlashMessage = "Se eliminó el registro exitosamente";
        return RedirectToAction(nameof(ListCategories));
    }

    /*
    *   Retrieves and displays a list of all products including their categories, ordered by product ID descending.
    *
    *   @return An IActionResult containing the products list view.
    */
    [Route("/entity/products")]
    public async Task<IActionResult> ListProducts()
    {
        var products = await context.Products.Include(x => x.Category).OrderByDescending(c => c.Id).ToListAsync();
        return View(products);
    }

    /*
    *   Retrieves and displays a paginated list of products, optionally filtered by a search string.
    *
    *   @param currentFilter The current search filter.
    *   @param searchString A string to filter product names.
    *   @param pageNumber The page number to display.
    *   @return An IActionResult with the paginated products view.
    */
    [Route("/entity/products-pagination")]
    public async Task<IActionResult> ListPaginatedProducts(string currentFilter, string searchString, int? pageNumber)
    {
        if (searchString != null)
        {
            pageNumber = 1;
        }
        else
        {
            searchString = currentFilter;
        }
        ViewData["CurrentFilter"] = searchString;
        ViewData["pageNumber"] = pageNumber;
        var products = from p in context.Products select p;
        products = products.Include(x => x.Category).OrderByDescending(c => c.Id);
        int pageSize = 2;
        return View(await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), pageNumber ?? 1, pageSize));
    }

    /*
    *   Retrieves and displays a paginated list of products filtered by category ID.
    *
    *   @param category_id The ID of the category to filter by.
    *   @param currentFilter The current search filter.
    *   @param searchString A string to filter product names.
    *   @param pageNumber The page number to display.
    *   @return An IActionResult with the paginated products view or NotFound if category_id is missing.
    */
    [Route("/entity/productsOrderByCategory/{category_id}")]
    public async Task<IActionResult> OrderByCategory(int category_id, string currentFilter, string searchString, int? pageNumber)
    {
        if (category_id == null)
        {
            return NotFound();
        }
        if (searchString != null)
        {
            pageNumber = 1;
        }
        else
        {
            searchString = currentFilter;
        }
        ViewData["CurrentFilter"] = searchString;
        ViewData["pageNumber"] = pageNumber;
        ViewData["category"] = context.Categories.Find(category_id).Name;
        ViewData["category_id"] = category_id;
        var products = from p in context.Products select p;
        products = products.Include(x => x.Category).OrderByDescending(c => c.Id).Where(p => p.CategoryId == category_id);
        int pageSize = 2;
        return View(await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), pageNumber ?? 1, pageSize));
    }

    /*
    *   Searches for products by name containing the specified search query and displays paginated results.
    *
    *   @param searcher The search query string.
    *   @param currentFilter The current filter.
    *   @param searchString The current search string.
    *   @param pageNumber The current page number.
    *   @return An IActionResult with the search results view.
    */
    [Route("/entity/searchProduct")]
    public async Task<IActionResult> SearchProduct([FromQuery(Name = "searcher")] string searcher, string currentFilter, string searchString, int? pageNumber)
    {
        if (searchString != null)
        {
            pageNumber = 1;
        }
        else
        {
            searchString = currentFilter;
        }
        ViewData["CurrentFilter"] = searchString;
        ViewData["pageNumber"] = pageNumber;
        ViewData["searcher"] = searcher;
        var products = from p in context.Products where p.Name.Contains(searcher) select p;
        products = products.OrderByDescending(s => s.Id).Include(x => x.Category);
        int pageSize = 5;
        return View(await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), pageNumber ?? 1, pageSize));
    }

    /*
    *   Displays the view to add a new product, including a list of categories.
    *
    *   @return An IActionResult with the add products view.
    */
    [Route("/entity/products/add")]
    public async Task<IActionResult> AddProducts()
    {
        ViewModel model = new ViewModel();
        model.Product = new Product();
        model.Categories = context.Categories.Select(c => new SelectListItem()
        {
            Text = c.Name,
            Value = c.Id.ToString()
        });
        return View(model);
    }

    /*
    *   Handles the HTTP POST request to add a new product.
    *
    *   @param viewModel The view model containing product data and categories.
    *   @return Redirects to the add products view if successful; otherwise, returns the view with current data.
    */
    [HttpPost]
    [Route("/entity/products/add")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddProducts(ViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            Product product = new Product
            {
                Name = viewModel.Product.Name,
                Slug = new SlugHelper().GenerateSlug(viewModel.Product.Name),
                Descripction = viewModel.Product.Descripction,
                Prize = viewModel.Product.Prize,
                Stock = viewModel.Product.Stock,
                Date = DateTime.Now,
                Category = context.Categories.Find(viewModel.Product.CategoryId)
            };
            context.Add(product);
            await context.SaveChangesAsync();
            FlashClass = "success";
            FlashMessage = "Se creó el registro exitosamente";
            return RedirectToAction(nameof(AddProducts));
        }
        ViewModel model = new ViewModel();
        model.Categories = context.Categories.Select(i => new SelectListItem()
        {
            Text = i.Name,
            Value = i.Id.ToString()
        });
        return View(model);
    }

    /*
    *   Displays the view to edit an existing product, including the list of categories.
    *
    *   @param id The ID of the product to edit.
    *   @return The edit products view if found; otherwise, NotFound.
    */
    [Route("/entity/products/edit/{id}")]
    public IActionResult EditProducts(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        ViewModel model = new ViewModel();
        var product = context.Products.Find(id);
        if (product == null)
        {
            return NotFound();
        }
        model.Product = product;
        model.Categories = context.Categories.Select(c => new SelectListItem()
        {
            Text = c.Name,
            Value = c.Id.ToString()
        });
        return View(model);
    }

    /*
    *   Handles the HTTP POST request to update a product.
    *
    *   @param viewModel The updated product data and categories.
    *   @param id The product ID.
    *   @return Redirects to the edit products view if successful; otherwise, returns the view with current data.
    */
    [HttpPost]
    [Route("/entity/products/edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProducts(ViewModel viewModel, int? id)
    {
        if (ModelState.IsValid)
        {
            Product productUpdate = context.Products.Find(id);
            productUpdate.Name = viewModel.Product.Name;
            productUpdate.Slug = new SlugHelper().GenerateSlug(viewModel.Product.Name);
            productUpdate.Descripction = viewModel.Product.Descripction;
            productUpdate.Prize = viewModel.Product.Prize;
            productUpdate.Stock = viewModel.Product.Stock;
            productUpdate.Category = context.Categories.Find(viewModel.Product.CategoryId);
            context.Update(productUpdate);
            await context.SaveChangesAsync();
            FlashClass = "success";
            FlashMessage = "Se modificó el registro exitosamente";
            return RedirectToAction(nameof(EditProducts));
        }
        if (id == null)
        {
            return NotFound();
        }
        var product = context.Products.Find(id);
        if (product == null)
        {
            return NotFound();
        }
        ViewModel model = new ViewModel();
        model.Categories = context.Categories.Select(i => new SelectListItem()
        {
            Text = i.Name,
            Value = i.Id.ToString()
        });
        return View(model);
    }

    /*
    *   Deletes a product from the database.
    *
    *   @param id The ID of the product to delete.
    *   @return Redirects to paginated product list if successful; otherwise, NotFound.
    */
    [Route("/entity/products/delete/{id}")]
    public async Task<IActionResult> DeleteProducts(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var product = context.Products.Find(id);
        if (product == null)
        {
            return NotFound();
        }
        context.Products.Remove(product);
        await context.SaveChangesAsync();
        FlashClass = "success";
        FlashMessage = "Se eliminó el registro exitosamente";
        return RedirectToAction("ListPaginatedProducts", "entity", new { pageNumber = "1" });
    }

    /*
    *   Retrieves and displays a list of images for a specific product.
    *
    *   @param id The ID of the product.
    *   @return The product images view with the associated product and images.
    */
    [Route("/entity/products-images/{id}")]
    public async Task<IActionResult> ListProductsImages(int id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var product = context.Products.Find(id);
        if (product == null)
        {
            return NotFound();
        }
        ProductsImagesViewModel model = new ProductsImagesViewModel();
        model.ProductImage = new ProductImage();
        ViewBag.Name = product.Name;
        ViewBag.Id = id;
        model.ProductsImages = await context.ProductsImages.Where(p => p.ProductId == id).ToListAsync();
        return View(model);
    }

    /*
    *   Handles the HTTP POST request to upload a new image for a product.
    *
    *   @param id The ID of the product.
    *   @param viewModel The view model containing the image and product information.
    *   @return Redirects to the product images view if successful; otherwise, returns the view with current data.
    */
    [HttpPost]
    [Route("/entity/products-images/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ListProductsImages(int id, ProductsImagesViewModel viewModel)
    {
        Console.WriteLine(viewModel.ProductImage.Product == null);
        if (ModelState.IsValid)
        {
            string principalPath = hostingEnv.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            Console.WriteLine("HOLA DESDE ENTITY");
            Console.WriteLine(files.Count);
            long timeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            string fileName = "image_" + timeStamp;
            Console.WriteLine(fileName);
            //string fileName = Guid.NewGuid().ToString();
            var uploads = Path.Combine(principalPath, @"uploads/products");
            var extension = Path.GetExtension(files[0].FileName);
            Console.WriteLine($"archivo = {fileName} | extension = {extension}");
            string filePath = Path.Combine(uploads, fileName + extension);
            if (!System.IO.File.Exists(filePath))
            {
                using (var fileStreams = new FileStream(filePath, FileMode.Create))
                {
                    files[0].CopyTo(fileStreams);
                }
                ProductImage productImage = new ProductImage
                {
                    Name = fileName + extension,
                    Product = context.Products.Find(viewModel.ProductImage.ProductId)
                };
                context.ProductsImages.Add(productImage);
                await context.SaveChangesAsync();
                FlashClass = "success";
                FlashMessage = "Se creó el registro exitosamente";
                return RedirectToAction(nameof(ListProductsImages));
            }
        }
        if (id == null)
        {
            return NotFound();
        }
        var product = context.Products.Find(id);
        if (product == null)
        {
            return NotFound();
        }
        ProductsImagesViewModel model = new ProductsImagesViewModel();
        model.ProductImage = new ProductImage();
        ViewBag.Name = product.Name;
        ViewBag.Id = id;
        model.ProductsImages = await context.ProductsImages.Where(p => p.ProductId == id).ToListAsync();
        return View(model);
    }

    /*
    *   Deletes an image associated with a product from both the database and file system.
    *
    *   @param id The ID of the product image to delete.
    *   @return Redirects to the product images view of the related product if successful; otherwise, NotFound.
    */
    [Route("/entity/products-images/delete/{id}")]
    public async Task<IActionResult> DeleteProductsImages(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var productImage = context.ProductsImages.Find(id);
        if (productImage == null)
        {
            return NotFound();
        }
        var productId = productImage.ProductId;
        var image = productImage.Name;
        context.ProductsImages.Remove(productImage);
        await context.SaveChangesAsync();
        string principalPath = hostingEnv.WebRootPath;
        var imagePath = Path.Combine(principalPath, @"uploads/products/" + image);
        if (System.IO.File.Exists(imagePath))
        {
            System.IO.File.Delete(imagePath);
        }
        FlashClass = "success";
        FlashMessage = "Se eliminó el registro exitosamente";
        return RedirectToAction("ListProductsImages", "entity", new
        {
            id = productId
        });
    }
}
