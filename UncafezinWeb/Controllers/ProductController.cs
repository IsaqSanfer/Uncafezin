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

public class ProductController : Controller
{
    private readonly AppDbContext _context;

    public ProductController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Product
    public async Task<IActionResult> Index()
    {
        IEnumerable<Product> products = await _context.Product.Include(p => p.Category).ToListAsync();
        _context.Dispose();

        return View(products);
    }

    // GET: Product/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        ProductViewModel model = new ProductViewModel();
        model.CategoryList = CategoryList();

        if (id != null)
        {
            var product = await _context.Product.Include(p => p.Category).FirstOrDefaultAsync(m => m.Code == id);
            model.Code = product.Code;
            model.Description = product.Description;
            model.Quantity = product.Quantity;
            model.Price = product.Price;
            model.CodeCategory = product.CodeCategory;
        }

        return View(model);
    }

    // GET: Product/Create
    public IActionResult Create()
    {
        ViewData["CodeCategory"] = new SelectList(_context.Category, "Code", "Code");
        return View();
    }

    // POST: Product/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            Product oProduct = new Product()
            {
                Code = model.Code,
                Description = model.Description,
                Quantity = model.Quantity,
                Price = model.Price,
                CodeCategory = (int)model.CodeCategory
            };


            if (model.Code == 0)
            {
                await _context.AddAsync(oProduct);
            }
            else
            {
                _context.Entry(oProduct).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }
        else
        {
            model.CategoryList = CategoryList();
            return View(model);
        }
        return RedirectToAction(nameof(Index));
    }

    // GET: Product/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Product.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        ViewData["CodeCategory"] = new SelectList(_context.Category, "Code", "Code", product.CodeCategory);
        return View(product);
    }

    // POST: Product/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Code,Description,Quantity,Price,CodeCategory")] Product product)
    {
        if (id != product.Code)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Code))
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
        ViewData["CodeCategory"] = new SelectList(_context.Category, "Code", "Code", product.CodeCategory);
        return View(product);
    }

    // GET: Product/Delete/5
    public IActionResult Delete(int? id)
    {        
        var product = new Product();
        _context.Attach(product);
        _context.Remove(product);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    // POST: Product/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _context.Product.FindAsync(id);
        if (product != null)
        {
            _context.Product.Remove(product);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProductExists(int id)
    {
        return _context.Product.Any(e => e.Code == id);
    }

    private IEnumerable<SelectListItem> CategoryList()
    {
        List<SelectListItem> list = new List<SelectListItem>();

        list.Add(new SelectListItem()
        {
            Value = String.Empty,
            Text = String.Empty,
        });

        foreach (var item in _context.Category.ToList())
        {
            list.Add(new SelectListItem()
            {
                Value = String.Empty,
                Text = String.Empty,
            });
        }

        return list;
    }
}
