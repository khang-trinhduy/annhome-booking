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

        public IActionResult Switch()
        {
            return RedirectToAction("Index", "Admin", new { });
        }

        [HttpPost]
        public IActionResult Home(Sale sale)
        {
            int count = 0;
            var meetings = _context.appoinment
       .Where(b => b.sale.Contains(sale.email))
       .ToList();
            string idl = "";
            List<string> name = new List<string>();
            foreach (Appoinment appoinment in meetings)
            {
                idl += " " + Convert.ToString(appoinment.Contract);
                name.Insert(count, (appoinment.Customer));
                count++;
            }
            ViewBag.id = idl.Trim().Split(" ");
            ViewBag.meetings = name;
            TempData["email"] = sale.email;
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
        public async Task<IActionResult> AddMeeting(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            //Sale sale = new Sale();
            //var temp = await _context.sale.FirstOrDefaultAsync(s => s.ID == id);
            //sale = temp;
            //TempData["email"] = sale.email;
            return RedirectToAction("Create","Appoinments",new { id });
        }

       // public IActionResult Meetings(int? id)
       // {
       //     if (id == null)
       //     {
       //         return View("~/Views/Shared/Error.cshtml");
       //     }
       //     Sale tmp = new Sale();
       //     foreach (Sale sale in sales)
       //     {
       //         if (sale.ID == id)
       //         {
       //             tmp = sale;
       //         }
       //     }
       //     var meetings = _context.appoinment
       //.Where(b => b.sale.Contains(tmp.email))
       //.ToList();
       //     return View("~/Views/Sales/Meetings.cshtml", meetings);
       // }

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
                if (sale.email.Equals(a.sale))
                {
                    TempData["name"] = sale.name;
                }
            }
            
            return RedirectToAction("Views", "Appoinments", new { id });
        }


    }
}