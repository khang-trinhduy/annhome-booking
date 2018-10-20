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
    public class LoginController : Controller
    {
        private readonly BookingFormContext _context;
        private readonly List<Sale> sales;
        public LoginController(BookingFormContext context)
        {
            _context = context;
            StreamReader r = new StreamReader("sales.json");
            string json = r.ReadToEnd();
            sales = JsonConvert.DeserializeObject<List<Sale>>(json);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Home(Sale sale)
        {
            foreach (Sale s in sales)
            {
                if (sale.email.Equals(s.email) && sale.pass.Equals(s.pass))
                {
                    return View(s);
                }
            }
            return View("Error");
        }

        [HttpGet]
        public IActionResult AddMeeting()
        {
            return RedirectToAction("Create","Appoinments", new { id = "name" });
        }

        public IActionResult Meetings(int? id)
        {
            if (id == null)
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            Sale tmp = new Sale();
            foreach (Sale sale in sales)
            {
                if (sale.ID == id)
                {
                    tmp = sale;
                }
            }
            var meetings = _context.appoinment
       .Where(b => b.sale.Contains(tmp.email))
       .ToList();
            return View("~/Views/Sales/Meetings.cshtml", meetings);
        }

        public async Task<IActionResult> AppDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return RedirectToAction("Details", "Appoinments", new { id });
        }
    }
}