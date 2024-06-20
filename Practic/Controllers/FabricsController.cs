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
    public class FabricsController : Controller
    {
        private readonly PracticdataContext _context;

        public FabricsController(PracticdataContext context)
        {
            _context = context;
        }

        // GET: Fabrics
        public async Task<IActionResult> Index()
        {
            var practicdataContext = _context.Fabrics.Include(f => f.ForWhat).Include(f => f.SetviceName);
            return View(await practicdataContext.ToListAsync());
        }

        // GET: Fabrics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabric = await _context.Fabrics
                .Include(f => f.ForWhat)
                .Include(f => f.SetviceName)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fabric == null)
            {
                return NotFound();
            }

            return View(fabric);
        }

        // GET: Fabrics/Create
        public IActionResult Create()
        {
            ViewData["ForWhatId"] = new SelectList(_context.ForWhats, "Id", "TypeofWork");
            ViewData["SetviceNameId"] = new SelectList(_context.ServiceNames, "Id", "NameofService");
            return View();
        }

        // POST: Fabrics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,SetviceNameId,ForWhatId,CodeTnved,CodeOkrb")] Fabric fabric)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fabric);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ForWhatId"] = new SelectList(_context.ForWhats, "Id", "TypeofWork", fabric.ForWhatId);
            ViewData["SetviceNameId"] = new SelectList(_context.ServiceNames, "Id", "NameofService", fabric.SetviceNameId);
            return View(fabric);
        }

        // GET: Fabrics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabric = await _context.Fabrics.FindAsync(id);
            if (fabric == null)
            {
                return NotFound();
            }
            ViewData["ForWhatId"] = new SelectList(_context.ForWhats, "Id", "TypeofWork", fabric.ForWhatId);
            ViewData["SetviceNameId"] = new SelectList(_context.ServiceNames, "Id", "NameofService", fabric.SetviceNameId);
            return View(fabric);
        }

        // POST: Fabrics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SetviceNameId,ForWhatId,CodeTnved,CodeOkrb")] Fabric fabric)
        {
            if (id != fabric.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fabric);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FabricExists(fabric.Id))
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
            ViewData["ForWhatId"] = new SelectList(_context.ForWhats, "Id", "TypeofWork", fabric.ForWhatId);
            ViewData["SetviceNameId"] = new SelectList(_context.ServiceNames, "Id", "NameofService", fabric.SetviceNameId);
            return View(fabric);
        }

        // GET: Fabrics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabric = await _context.Fabrics
                .Include(f => f.ForWhat)
                .Include(f => f.SetviceName)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fabric == null)
            {
                return NotFound();
            }

            return View(fabric);
        }

        // POST: Fabrics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fabric = await _context.Fabrics.FindAsync(id);
            if (fabric != null)
            {
                _context.Fabrics.Remove(fabric);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FabricExists(int id)
        {
            return _context.Fabrics.Any(e => e.Id == id);
        }
    }
}
