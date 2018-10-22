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
        public AdminController(BookingFormContext context)
        {
            _context = context;
            StreamReader r = new StreamReader("admins.json");
            string json = r.ReadToEnd();
            admins = JsonConvert.DeserializeObject<List<Admin>>(json);
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
                    return View("Admin", modal);
                }
            }
            return View("Error");
        }
    }
}