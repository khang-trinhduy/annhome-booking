using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookingForm.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BookingForm.Controllers
{
    public class AdminController : Controller
    {
        private readonly BookingFormContext _context;
        private readonly List<Admin> admins;
        private readonly List<Sale> sales;
        private static Admin administrator;
        public AdminController(BookingFormContext context)
        {
            _context = context;
            StreamReader r = new StreamReader("admins.json");
            string json = r.ReadToEnd();
            admins = JsonConvert.DeserializeObject<List<Admin>>(json);
            StreamReader s = new StreamReader("sales.json");
            string jsonn = s.ReadToEnd();
            sales = JsonConvert.DeserializeObject<List<Sale>>(jsonn);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Home(Admin admin)
        {
            var meetings = await _context.appoinment.ToListAsync();
            var meeting = _context.appoinment.First();
            foreach (Admin s in admins)
            {
                if (admin.email.Equals(s.email) && admin.password.Equals(s.password))
                {
                    admin.name = s.name;
                    admin.phone = s.phone;
                    AdminModal modal = new AdminModal();
                    modal.admin = admin;
                    modal.appoinment = meeting;
                    modal.appoinments = meetings;
                    administrator = admin;
                    return View(modal);
                }
            }
            return View("Error");
        }
        public async Task<IActionResult> AppDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Appoinment a = await _context.appoinment
       .FirstOrDefaultAsync(b => b.Contract == id);
            if (a == null)
            {
                return NotFound();
            }
            foreach (Sale sale in sales)
            {
                if (sale.email == a.sale)
                    TempData["name"] = sale.name;
            }
            return RedirectToAction("Views", "Appoinments", new { id });
        }

        public async Task<IActionResult> Confirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Appoinment a = await _context.appoinment
       .FirstOrDefaultAsync(b => b.Contract == id);
            if (a == null)
            {
                return NotFound();
            }
            foreach (Sale sale in sales)
            {
                if (sale.email == a.sale)
                {
                    TempData["name"] = sale.name;
                    break;
                }
            }
            return View(a);
        }

        public async Task<AdminModal> GetModal(Admin admin)
        {
            var meetings = await _context.appoinment.ToListAsync();
            var meeting = _context.appoinment.First();
            AdminModal modal = new AdminModal();
            modal.admin = admin;
            modal.appoinment = meeting;
            modal.appoinments = meetings;
            return modal;
        }

        public async Task<IActionResult> Confirmed(Appoinment a)
        {
            if (a.Contract == null)
            {
                return NotFound();
            }
            Appoinment tmp = await _context.appoinment.FirstOrDefaultAsync(m => m.Contract == a.Contract);
            tmp.dTime = a.dTime;
            tmp.Price = a.Price;
            tmp.Confirm = true;
            DateTime first = DateTime.ParseExact(tmp.cTime, "ddMMyyyy HH:mm:ss.FFFFFFF",
                                       System.Globalization.CultureInfo.InvariantCulture);
            DateTime second = DateTime.ParseExact(tmp.dTime, "ddMMyyyy HH:mm:ss.FFFFFFF",
                                       System.Globalization.CultureInfo.InvariantCulture);
            TimeSpan span = second.Subtract(first);
            if(span.TotalHours <= 1)
            {
                //ok

            }
            else
            {

                tmp.Contract = _context.appoinment.Max(c => c.Contract) + 1;
                tmp.Priority = _context.appoinment.Max(c => c.Priority) + 1;
            }
            //string[] new_day = Convert.ToString(day).Split(" ")[0].Split("/");
            //string new_time = Convert.ToString(hh) + Convert.ToString(MM) + Convert.ToString(ss) + Convert.ToString(FF);

            await _context.SaveChangesAsync();
            AdminModal modal = await GetModal(administrator);
            return View("Home", modal);
        }
    }
}