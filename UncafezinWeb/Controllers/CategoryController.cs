using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UncafezinDAL;
using UncafezinEntities;
using UncafezinWeb.Models;

namespace UncafezinWeb.Controllers;

public class CategoryController : Controller
{
    private readonly AppDbContext _context;

    public CategoryController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Category
    public async Task<IActionResult> Index()
    {
        IEnumerable<Category> categories = await _context.Category.ToListAsync();
        _context.Dispose();

        return View(categories);
    }

    // GET: Category/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        CategoryViewModel model = new CategoryViewModel();

        if (id != null)
        {
            var category = await _context.Category.FirstOrDefaultAsync(x => x.Code == id);
            model.Code = category.Code;
            model.Description = category.Description;
        }

        return View(model);
    }

    // GET: Category/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Category/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CategoryViewModel category)
    {
        if (ModelState.IsValid)
        {
            Category oCategory = new Category()
            {
                Code = category.Code,
                Description = category.Description,
            };

            if(category.Code == 0)
            {
                _context.Category.Add(oCategory);
            }
            else
            {
                //alternativa seria chamar edit() neste ponto
                _context.Entry(oCategory).State = EntityState.Modified;
            }
            
            _context.SaveChanges();
        } 
        else
        {
            return View(category);
        }
        
        return RedirectToAction(nameof(Index));
    }

    // GET: Category/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await _context.Category.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    // POST: Category/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Code,Description")] Category category)
    {
        if (id != category.Code)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.Code))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(category);
    }

    // GET: Category/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        //var category = await _context.Category.FirstOrDefaultAsync(x => x.Code == id);
        var category = new Category() { Code = id }; 

        if (category == null)
        {
            return NotFound();
        }
        else
        {
            _context.Attach(category);
            _context.Remove(category);
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }

    // POST: Category/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var category = await _context.Category.FindAsync(id);
        if (category != null)
        {
            _context.Category.Remove(category);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CategoryExists(int id)
    {
        return _context.Category.Any(e => e.Code == id);
    }
}
