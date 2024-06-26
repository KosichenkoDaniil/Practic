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
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Practic.Infrastructure;

namespace Practic.Controllers
{
    public class CurrenciesController : Controller
    {
        private readonly PracticdataContext _context;
      //  private readonly int pageSize = 10;

        public CurrenciesController(PracticdataContext context)
        {
            _context = context;
        }

        // GET: Currencies
        public IActionResult Index(int page = 1)
        {
            var currencyView = HttpContext.Session.Get<CurrencyViewModel>("Currencies");
            if (currencyView == null)
            {
                currencyView = new CurrencyViewModel();
            }

            IQueryable<Models.Currency> currenciesDbContext = _context.Currencies;
            currenciesDbContext = Search(currenciesDbContext, currencyView.NameofCurrency, currencyView.CountryofCurrency);
            var count = currenciesDbContext.Count();
         //   currenciesDbContext = currenciesDbContext.Skip((page - 1) * pageSize).Take(pageSize);
            CurrencyViewModel currencies = new CurrencyViewModel
            {
                currencies = currenciesDbContext,
         //       PageViewModel = new PageViewModel(count, page, pageSize),
                NameofCurrency = currencyView.NameofCurrency,
                CountryofCurrency = currencyView.CountryofCurrency
            };
            return View(currencies);
        }

        [HttpPost]
        public IActionResult Index(CurrencyViewModel currencyView)
        {
            HttpContext.Session.Set("Currencies", currencyView);

            return RedirectToAction("Index");
        }

        // GET: Currencies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currency = await _context.Currencies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (currency == null)
            {
                return NotFound();
            }

            return View(currency);
        }

        // GET: Currencies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Currencies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NameofCurrency,CountryofCurrency")] Currency currency)
        {
            if (ModelState.IsValid)
            {
                _context.Add(currency);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(currency);
        }

        // GET: Currencies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currency = await _context.Currencies.FindAsync(id);
            if (currency == null)
            {
                return NotFound();
            }
            return View(currency);
        }

        // POST: Currencies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameofCurrency,CountryofCurrency")] Currency currency)
        {
            if (id != currency.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(currency);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CurrencyExists(currency.Id))
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
            return View(currency);
        }

        // GET: Currencies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currency = await _context.Currencies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (currency == null)
            {
                return NotFound();
            }

            return View(currency);
        }

        // POST: Currencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currency = await _context.Currencies.FindAsync(id);
            if (currency != null)
            {
                _context.Currencies.Remove(currency);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CurrencyExists(int id)
        {
            return _context.Currencies.Any(e => e.Id == id);
        }

        private IQueryable<Models.Currency> Search(IQueryable<Models.Currency> currencies, string NameofCurrency, string CountryofCurrency)
        {
            currencies = currencies.Where(o => o.NameofCurrency.Contains(NameofCurrency ?? "")
           && (o.CountryofCurrency.Contains(CountryofCurrency ?? "")));
            
            return currencies;
        }
    }
}
