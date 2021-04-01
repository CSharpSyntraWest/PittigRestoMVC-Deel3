using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PittigRestoMVC.Data;
using PittigRestoMVC.Models;
using PittigRestoMVC.Models.ViewModels;

namespace PittigRestoMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuItemsController : Controller
    {
        
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        [BindProperty]
        public MenuItemViewModel MenuItemVM { get; set; }
        public MenuItemsController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            MenuItemVM = new MenuItemViewModel()
            {
                Category = _context.Category,//.OrderBy(c => c.Name),
                MenuItem = new MenuItem()
            };
        }

        // GET: Admin/MenuItems
        public async Task<IActionResult> Index()
        {

            var applicationDbContext = _context.MenuItem.Include(m => m.Category).Include(m => m.SubCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/MenuItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _context.MenuItem
                .Include(m => m.Category)
                .Include(m => m.SubCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        // GET: Admin/MenuItems/Create
        public IActionResult Create()
        {           
            return View(MenuItemVM);
        }

        // POST: Admin/MenuItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()// MenuItemViewModel menuItemVM)
        {
            MenuItemVM.MenuItem.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());
            if (!ModelState.IsValid)
            {
                return View(MenuItemVM);
            }
               _context.Add(MenuItemVM.MenuItem);
                await _context.SaveChangesAsync();
            //nog uploaded image uit Form naar disk schrijven:
            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var menuItemFromDb = await _context.MenuItem.FindAsync(MenuItemVM.MenuItem.Id);


            if (files.Count > 0)
            {
   
                //uploaded image naar wwwroot/images schrijven:
                var uploadsPath = System.IO.Path.Combine(webRootPath, "images");
                var extension = System.IO.Path.GetExtension(files[0].FileName);//bv "Kir.png" => extension= ".png"

                //fileStream openen om naar disk te schrijven
                using (var fileStream = new FileStream(uploadsPath + "\\" + MenuItemVM.MenuItem.Id + extension, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                menuItemFromDb.Image = @"\images\" + MenuItemVM.MenuItem.Id + extension;
            }
            else
            {
                //TO DO;
                //Default food image nemen
                //var defaultImgPath = webRootPath + @"\images\default_food.png";
                var defaultImgPath = Path.Combine(webRootPath, "images", "default_food.png");
                var destImgPath = Path.Combine(webRootPath, "images", MenuItemVM.MenuItem.Id + ".png");
                System.IO.File.Copy(defaultImgPath, destImgPath);
                menuItemFromDb.Image = @"\images\" + MenuItemVM.MenuItem.Id + ".png";
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Admin/MenuItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MenuItemVM.MenuItem = await _context.MenuItem.Include(m => m.Category)
                .Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);

            if (MenuItemVM.MenuItem == null)
            {
                return NotFound();
            }

            MenuItemVM.SubCategory = await _context.SubCategory
                .Where(s => s.CategoryId == MenuItemVM.MenuItem.CategoryId).ToListAsync();

         
            return View(MenuItemVM);
        }

        // POST: Admin/MenuItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPOST(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id != MenuItemVM.MenuItem.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
               //?TO DO Subcategorieën invullen
                return View(MenuItemVM);
            }
            //testen of er een image upload is in Request.Form
            //In dit geval moet de bestaande image worden uitgeveegd en de nieuwe image op disk schrijven
            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var menuItemFromDb = await _context.MenuItem.FindAsync(MenuItemVM.MenuItem.Id);
            if (files.Count > 0)
            {
                var uploads = Path.Combine(webRootPath, "images");
                var extension_new = Path.GetExtension(files[0].FileName);
                //Veeg de bestaande image weg:
                var imagePath = Path.Combine(webRootPath, menuItemFromDb.Image.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
                //Schrijven van nieuwe image op disk (dezelfde manier als bij Create)
                //we will upload the new file
                using (var filesStream = new FileStream(Path.Combine(uploads, MenuItemVM.MenuItem.Id + extension_new), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                menuItemFromDb.Image = @"\images\" + MenuItemVM.MenuItem.Id + extension_new;
            }
            menuItemFromDb.Name = MenuItemVM.MenuItem.Name;
            menuItemFromDb.Description = MenuItemVM.MenuItem.Description;
            menuItemFromDb.Price = MenuItemVM.MenuItem.Price;
            menuItemFromDb.Spicyness = MenuItemVM.MenuItem.Spicyness;
            menuItemFromDb.CategoryId = MenuItemVM.MenuItem.CategoryId;
            menuItemFromDb.SubCategoryId = MenuItemVM.MenuItem.SubCategoryId;
            await _context.SaveChangesAsync();
                
               
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/MenuItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _context.MenuItem
                .Include(m => m.Category)
                .Include(m => m.SubCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        // POST: Admin/MenuItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuItem = await _context.MenuItem.FindAsync(id);
            _context.MenuItem.Remove(menuItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuItemExists(int id)
        {
            return _context.MenuItem.Any(e => e.Id == id);
        }
    }
}
