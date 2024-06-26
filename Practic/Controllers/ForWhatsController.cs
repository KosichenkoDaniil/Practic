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
    public class ForWhatsController : Controller
    {
        private readonly PracticdataContext _context;
   //     private readonly int pageSize = 10;

        public ForWhatsController(PracticdataContext context)
        {
            _context = context;
        }

        // GET: ForWhats
        public IActionResult Index(int page = 1)
        {
            var forWhatView = HttpContext.Session.Get<ForWhatViewModel>("ForWhats");
            if (forWhatView == null)
            {
                forWhatView = new ForWhatViewModel();
            }

            IQueryable<Models.ForWhat> forWhatsDbContext = _context.ForWhats;
            forWhatsDbContext = Search(forWhatsDbContext, forWhatView.TypeofWork);
            var count = forWhatsDbContext.Count();
        //    forWhatsDbContext = forWhatsDbContext.Skip((page - 1) * pageSize).Take(pageSize);
            ForWhatViewModel forWhats = new ForWhatViewModel
            {
                forWhats = forWhatsDbContext,
        //        PageViewModel = new PageViewModel(count, page, pageSize),
                TypeofWork = forWhatView.TypeofWork
            };
            return View(forWhats);
        }

        [HttpPost]
        public IActionResult Index(ForWhatViewModel forWhatView)
        {
            HttpContext.Session.Set("ForWhats", forWhatView);

            return RedirectToAction("Index");
        }

        // GET: ForWhats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forWhat = await _context.ForWhats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (forWhat == null)
            {
                return NotFound();
            }

            return View(forWhat);
        }

        // GET: ForWhats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ForWhats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TypeofWork")] ForWhat forWhat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(forWhat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(forWhat);
        }

        // GET: ForWhats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forWhat = await _context.ForWhats.FindAsync(id);
            if (forWhat == null)
            {
                return NotFound();
            }
            return View(forWhat);
        }

        // POST: ForWhats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TypeofWork")] ForWhat forWhat)
        {
            if (id != forWhat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(forWhat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ForWhatExists(forWhat.Id))
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
            return View(forWhat);
        }

        // GET: ForWhats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forWhat = await _context.ForWhats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (forWhat == null)
            {
                return NotFound();
            }

            return View(forWhat);
        }

        // POST: ForWhats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConforWhated(int id)
        {
            var forWhat = await _context.ForWhats.FindAsync(id);
            if (forWhat != null)
            {
                _context.ForWhats.Remove(forWhat);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ForWhatExists(int id)
        {
            return _context.ForWhats.Any(e => e.Id == id);
        }

        private IQueryable<Models.ForWhat> Search(IQueryable<Models.ForWhat> forWhats, string TypeofWork)
        {
            forWhats = forWhats.Where(o => o.TypeofWork.Contains(TypeofWork ?? ""));

            return forWhats;
        }
    }
}
