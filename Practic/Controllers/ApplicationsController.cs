using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practic;
using Practic.Models;

namespace Practic.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly PracticdataContext _context;

        public ApplicationsController(PracticdataContext context)
        {
            _context = context;
        }

        // GET: Applications
        public async Task<IActionResult> Index()
        {
            var practicdataContext = _context.Applications.Include(a => a.Currency).Include(a => a.Fabric).Include(a => a.Firm);
            return View(await practicdataContext.ToListAsync());
        }

        // GET: Applications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .Include(a => a.Currency)
                .Include(a => a.Fabric)
                .Include(a => a.Firm)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // GET: Applications/Create
        public IActionResult Create()
        {
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "NameofCurrency");
            ViewData["FabricId"] = new SelectList(_context.Fabrics, "Id", "Name");
            ViewData["FirmId"] = new SelectList(_context.Firms, "Id", "NameofFirm");
            return View();
        }

        // POST: Applications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FabricId,ShortDescription,FirmId,CurrencyId,Price,Quantity")] Application application)
        {
            if (ModelState.IsValid)
            {
                _context.Add(application);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "NameofCurrency", application.CurrencyId);
            ViewData["FabricId"] = new SelectList(_context.Fabrics, "Id", "Name", application.FabricId);
            ViewData["FirmId"] = new SelectList(_context.Firms, "Id", "NameofFirm", application.FirmId);
            return View(application);
        }

        // GET: Applications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Applications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "NameofCurrency", application.CurrencyId);
            ViewData["FabricId"] = new SelectList(_context.Fabrics, "Id", "Name", application.FabricId);
            ViewData["FirmId"] = new SelectList(_context.Firms, "Id", "NameofFirm", application.FirmId);
            return View(application);
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FabricId,ShortDescription,FirmId,CurrencyId,Price,Quantity")] Application application)
        {
            if (id != application.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(application);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationExists(application.Id))
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
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "NameofCurrency", application.CurrencyId);
            ViewData["FabricId"] = new SelectList(_context.Fabrics, "Id", "Name", application.FabricId);
            ViewData["FirmId"] = new SelectList(_context.Firms, "Id", "NameofFirm", application.FirmId);
            return View(application);
        }

        // GET: Applications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .Include(a => a.Currency)
                .Include(a => a.Fabric)
                .Include(a => a.Firm)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application != null)
            {
                _context.Applications.Remove(application);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationExists(int id)
        {
            return _context.Applications.Any(e => e.Id == id);
        }
    }
}
