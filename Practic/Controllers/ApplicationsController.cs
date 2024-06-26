using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using Practic;
using Practic.Models;
using Practic.ViewModels;
using Practic.Infrastructure;
using static System.Net.Mime.MediaTypeNames;

namespace Practic.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly PracticdataContext _context;
        //private readonly int pageSize = 10;

        public ApplicationsController(PracticdataContext context)
        {
            _context = context;
        }

        // GET: Applications
        public IActionResult Index(int page = 1)
        {
            var applicationView = HttpContext.Session.Get<ApplicationViewModel>("Application");
            if (applicationView == null)
            {
                applicationView = new ApplicationViewModel();
            }

            IQueryable<Models.Application> applicationsDbContext = _context.Applications.Include(o => o.Fabric).Include(o => o.Currency).Include(o => o.Firm);
            applicationsDbContext = Search(applicationsDbContext, applicationView.Name, applicationView.ShortDescription, applicationView.NameofFirm, applicationView.NameofCurrency
    , applicationView.Price, applicationView.Quantity);
            var count = applicationsDbContext.Count();
           // applicationsDbContext = applicationsDbContext.Skip((page - 1) * pageSize).Take(pageSize);
            ApplicationViewModel applications = new ApplicationViewModel
            {
                applications = applicationsDbContext,
              //  PageViewModel = new PageViewModel(count, page, pageSize),
                Name = applicationView.Name,
                ShortDescription = applicationView.ShortDescription,
                NameofFirm = applicationView.NameofFirm,
                NameofCurrency = applicationView.NameofCurrency,
                Price = applicationView.Price,
                Quantity = applicationView.Quantity
            };
            return View(applications);
        }

        [HttpPost]
        public IActionResult Index(ApplicationViewModel applicationView)
        {
            HttpContext.Session.Set("Application", applicationView);

            return RedirectToAction("Index");
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
        public async Task<IActionResult> Create([Bind("Id,FabricId,ShortDescription,FirmId,CurrencyId,Price,Quantity")] Models.Application application)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,FabricId,ShortDescription,FirmId,CurrencyId,Price,Quantity")] Models.Application application)
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

        private IQueryable<Models.Application> Search(IQueryable<Models.Application> applications, string Name, string ShortDescription,
           string NameofFirm, string NameofCurrency, decimal Price, int Quantity)
        {
            applications = applications.Where(o => o.Fabric.Name.Contains(Name ?? "")
           && (o.ShortDescription.Contains(ShortDescription ?? ""))
           && (o.Firm.NameofFirm.Contains(NameofFirm ?? ""))
           && (o.Currency.NameofCurrency.Contains(NameofCurrency ?? ""))
           && (o.Price == Price || Price == 0)
           && (o.Quantity == Quantity || Quantity == 0));

            return applications;
        }
    }
}
