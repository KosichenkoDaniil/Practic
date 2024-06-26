using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practic;
using Practic.Models;
using Practic.ViewModels;
using Practic.Infrastructure;

namespace Practic.Controllers
{
    public class FirmsController : Controller
    {
        private readonly PracticdataContext _context;
        private readonly int pageSize = 6;

        public FirmsController(PracticdataContext context)
        {
            _context = context;
        }

        // GET: Firms
        public IActionResult Index(int page = 1)
        {
            var firmView = HttpContext.Session.Get<FirmViewModel>("Firm");
            if (firmView == null)
            {
                firmView = new FirmViewModel();
            }

            IQueryable<Firm> firmsDbContext = _context.Firms;
            firmsDbContext = Search(firmsDbContext, firmView.NameofFirm, firmView.СountryofFirm);
            var count = firmsDbContext.Count();
            firmsDbContext = firmsDbContext.Skip((page - 1) * pageSize).Take(pageSize);
            FirmViewModel firms = new FirmViewModel
            {
                firms = firmsDbContext,
                PageViewModel = new PageViewModel(count, page, pageSize),
                NameofFirm = firmView.NameofFirm,
                СountryofFirm = firmView.СountryofFirm
            };
            return View(firms);
        }

        [HttpPost]
        public IActionResult Index(FirmViewModel firmView)
        {
            HttpContext.Session.Set("Firm", firmView);

            return RedirectToAction("Index");
        }

        // GET: Firms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var firm = await _context.Firms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (firm == null)
            {
                return NotFound();
            }

            return View(firm);
        }

        // GET: Firms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Firms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,СountryofFirm,NameofFirm")] Firm firm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(firm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(firm);
        }

        // GET: Firms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var firm = await _context.Firms.FindAsync(id);
            if (firm == null)
            {
                return NotFound();
            }
            return View(firm);
        }

        // POST: Firms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,СountryofFirm,NameofFirm")] Firm firm)
        {
            if (id != firm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(firm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FirmExists(firm.Id))
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
            return View(firm);
        }

        // GET: Firms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var firm = await _context.Firms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (firm == null)
            {
                return NotFound();
            }

            return View(firm);
        }

        // POST: Firms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var firm = await _context.Firms.FindAsync(id);
            if (firm != null)
            {
                _context.Firms.Remove(firm);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FirmExists(int id)
        {
            return _context.Firms.Any(e => e.Id == id);
        }

        private IQueryable<Firm> Search(IQueryable<Firm> firms, string NameofFirm, string CountryofFirm)
        {
            firms = firms.Where(o => o.NameofFirm.Contains(NameofFirm ?? "")
           && (o.СountryofFirm.Contains(CountryofFirm ?? "")));

            return firms;
        }
    }
}
