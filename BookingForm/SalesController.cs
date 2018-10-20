using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingForm.Models;

namespace BookingForm
{
    public class SalesController : Controller
    {
        private readonly BookingFormContext _context;

        public SalesController(BookingFormContext context)
        {
            _context = context;
        }

        // GET: Sales
        public async Task<IActionResult> Index()
        {
            List<int> temp = new List<int>();
            List<Sale> sales = await _context.sale.ToListAsync();
            int count = 0;
            foreach (var sale in sales)
            {
                var meetings = _context.appoinment.Where(b => b.sale.Contains(sale.email)).ToList();
                temp.Insert(count, meetings.Count);
                count++;
            }

            ViewBag.lc = temp;
            return View(await _context.sale.ToListAsync());
        }

        // GET: Sales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.sale
                .FirstOrDefaultAsync(m => m.ID == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // GET: Sales/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,name,phone,email,pass")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sale);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sale);
        }

        public async Task<IActionResult> Meetings(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var sale = await _context.sale.FindAsync(id);
            var meetings = _context.appoinment
        .Where(b => b.sale.Contains(sale.email))
        .ToList();
            return View(meetings);
        }

        public IActionResult AppDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return RedirectToAction("Details", "Appoinments", new { id });
        }

        // GET: Sales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.sale.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            return View(sale);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,name,phone,email,pass")] Sale sale)
        {
            if (id != sale.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleExists(sale.ID))
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
            return View(sale);
        }

        // GET: Sales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.sale
                .FirstOrDefaultAsync(m => m.ID == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sale = await _context.sale.FindAsync(id);
            _context.sale.Remove(sale);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaleExists(int id)
        {
            return _context.sale.Any(e => e.ID == id);
        }
    }
}
