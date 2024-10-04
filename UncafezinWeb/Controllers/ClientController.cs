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

public class ClientController : Controller
{
    private readonly AppDbContext _context;

    public ClientController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Client
    public async Task<IActionResult> Index()
    {
        return View(await _context.Client.ToListAsync());
    }

    // GET: Client/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        ClientViewModel model = new ClientViewModel();

        if (id != null)
        {
            var client = await _context.Client.FirstOrDefaultAsync(m => m.Code == id);
            model.Code = client.Code;
            model.Name = client.Name;
            model.CNPJ_CPF = client.CNPJ_CPF;
            model.Email = client.Email;
            model.Cellphone = client.Cellphone;
        }

        return View(model);
    }

    // GET: Client/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Client/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ClientViewModel model)
    {
        if (ModelState.IsValid)
        {
            Client oClient = new Client()
            {
                Code = model.Code,
                Name = model.Name,
                CNPJ_CPF = model.CNPJ_CPF,
                Email = model.Email,
                Cellphone = model.Cellphone,
            };

            if (model.Code == 0)
            {
                await _context.Client.AddAsync(oClient);
            }
            else
            {
                _context.Entry(oClient).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }
        else
        {
            return View(model);
        }
        
        return RedirectToAction(nameof(Index));
    }

    // GET: Client/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var client = await _context.Client.FindAsync(id);
        if (client == null)
        {
            return NotFound();
        }
        return View(client);
    }

    // POST: Client/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Code,Name,CNPJ_CPF,Email,Cellphone")] Client client)
    {
        if (id != client.Code)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(client);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(client.Code))
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
        return View(client);
    }

    // GET: Client/Delete/5
    public IActionResult Delete(int id)
    {
        var client = new Client() { Code = id };
        _context.Attach(client);
        _context.Remove(client);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    // POST: Client/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var client = await _context.Client.FindAsync(id);
        if (client != null)
        {
            _context.Client.Remove(client);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ClientExists(int id)
    {
        return _context.Client.Any(e => e.Code == id);
    }
}
