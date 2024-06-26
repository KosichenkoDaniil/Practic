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
    public class ServiceNamesController : Controller
    {
        private readonly PracticdataContext _context;
        private readonly int pageSize = 6;

        public ServiceNamesController(PracticdataContext context)
        {
            _context = context;
        }

        // GET: ServiceNames
        public IActionResult Index(int page = 1)
        {
            var serviceNameView = HttpContext.Session.Get<ServiceNameViewModel>("ServiceNames");
            if (serviceNameView == null)
            {
                serviceNameView = new ServiceNameViewModel();
            }

            IQueryable<Models.ServiceName> serviceNamesDbContext = _context.ServiceNames;
            serviceNamesDbContext = Search(serviceNamesDbContext, serviceNameView.NameofService, serviceNameView.Department);
            var count = serviceNamesDbContext.Count();
            serviceNamesDbContext = serviceNamesDbContext.Skip((page - 1) * pageSize).Take(pageSize);
            ServiceNameViewModel serviceNames = new ServiceNameViewModel
            {
                serviceNames = serviceNamesDbContext,
                PageViewModel = new PageViewModel(count, page, pageSize),
                NameofService = serviceNameView.NameofService,
                Department = serviceNameView.Department
            };
            return View(serviceNames);
        }

        [HttpPost]
        public IActionResult Index(ServiceNameViewModel serviceNameView)
        {
            HttpContext.Session.Set("ServiceNames", serviceNameView);

            return RedirectToAction("Index");
        }

        // GET: ServiceNames/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceName = await _context.ServiceNames
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceName == null)
            {
                return NotFound();
            }

            return View(serviceName);
        }

        // GET: ServiceNames/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceNames/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NameofService,Department")] ServiceName serviceName)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serviceName);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serviceName);
        }

        // GET: ServiceNames/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceName = await _context.ServiceNames.FindAsync(id);
            if (serviceName == null)
            {
                return NotFound();
            }
            return View(serviceName);
        }

        // POST: ServiceNames/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameofService,Department")] ServiceName serviceName)
        {
            if (id != serviceName.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceName);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceNameExists(serviceName.Id))
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
            return View(serviceName);
        }

        // GET: ServiceNames/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceName = await _context.ServiceNames
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceName == null)
            {
                return NotFound();
            }

            return View(serviceName);
        }

        // POST: ServiceNames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConserviceNameed(int id)
        {
            var serviceName = await _context.ServiceNames.FindAsync(id);
            if (serviceName != null)
            {
                _context.ServiceNames.Remove(serviceName);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceNameExists(int id)
        {
            return _context.ServiceNames.Any(e => e.Id == id);
        }

        private IQueryable<Models.ServiceName> Search(IQueryable<Models.ServiceName> serviceNames, string NameofService, string Department)
        {
            serviceNames = serviceNames.Where(o => o.NameofService.Contains(NameofService ?? "")
           && (o.Department.Contains(Department ?? "")));

            return serviceNames;
        }
    }
}
