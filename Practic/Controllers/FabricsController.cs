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
    public class FabricsController : Controller
    {
        private readonly PracticdataContext _context;
        private readonly int pageSize = 10;

        public FabricsController(PracticdataContext context)
        {
            _context = context;
        }

        // GET: Fabrics
        public IActionResult Index(int page = 1)
        {
            var fabricView = HttpContext.Session.Get<FabricViewModel>("Fabric");
            if (fabricView == null)
            {
                fabricView = new FabricViewModel();
            }

            IQueryable<Models.Fabric> fabricsDbContext = _context.Fabrics.Include(o => o.SetviceName).Include(o => o.ForWhat);
            fabricsDbContext = Search(fabricsDbContext, fabricView.Name, fabricView.NameofService, fabricView.TypeofWork, fabricView.CodeTnved
    , fabricView.CodeOkrb);
            var count = fabricsDbContext.Count();
            fabricsDbContext = fabricsDbContext.Skip((page - 1) * pageSize).Take(pageSize);
            FabricViewModel fabrics = new FabricViewModel
            {
                fabrics = fabricsDbContext,
                PageViewModel = new PageViewModel(count, page, pageSize),
                Name = fabricView.Name,
                NameofService = fabricView.NameofService,
                TypeofWork = fabricView.TypeofWork,
                CodeTnved = fabricView.CodeTnved,
                CodeOkrb = fabricView.CodeOkrb
            };
            return View(fabrics);
        }

        [HttpPost]
        public IActionResult Index(FabricViewModel fabricView)
        {
            HttpContext.Session.Set("Fabric", fabricView);

            return RedirectToAction("Index");
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

        private IQueryable<Models.Fabric> Search(IQueryable<Models.Fabric> fabrics, string Name, string NameofService,
           string TypeofWork, string CodeTnved, string CodeOkrb)
        {
            fabrics = fabrics.Where(o => o.Name.Contains(Name ?? "")
           && (o.SetviceName.NameofService.Contains(NameofService ?? ""))
           && (o.ForWhat.TypeofWork.Contains(TypeofWork ?? ""))
           && (o.CodeTnved.Contains(CodeTnved ?? ""))
           && (o.CodeOkrb.Contains(CodeOkrb ?? "")));

            return fabrics;
        }
    }
}
