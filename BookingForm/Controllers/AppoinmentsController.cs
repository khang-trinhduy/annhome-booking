using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingForm.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;

namespace BookingForm.Controllers
{
    public class AppoinmentsController : Controller
    {
        private readonly BookingFormContext _context;
        private readonly List<Sale> sales;

        public AppoinmentsController(BookingFormContext context)
        {
            _context = context;
            StreamReader r = new StreamReader("sales.json");
            string json = r.ReadToEnd();
            sales = JsonConvert.DeserializeObject<List<Sale>>(json);
        }
        public static string So_chu(double gNum)
        {
            if (gNum == 0)
                return "Không đồng";

            string lso_chu = "";
            string tach_mod = "";
            string tach_conlai = "";
            double Num = Math.Round(gNum, 0);
            string gN = Convert.ToString(Num);
            int m = Convert.ToInt32(gN.Length / 3);
            int mod = gN.Length - m * 3;
            string dau = "[+]";

            // Dau [+ , - ]
            if (gNum < 0)
                dau = "[-]";
            dau = "";

            // Tach hang lon nhat
            if (mod.Equals(1))
                tach_mod = "00" + Convert.ToString(Num.ToString().Trim().Substring(0, 1)).Trim();
            if (mod.Equals(2))
                tach_mod = "0" + Convert.ToString(Num.ToString().Trim().Substring(0, 2)).Trim();
            if (mod.Equals(0))
                tach_mod = "000";
            // Tach hang con lai sau mod :
            if (Num.ToString().Length > 2)
                tach_conlai = Convert.ToString(Num.ToString().Trim().Substring(mod, Num.ToString().Length - mod)).Trim();

            ///don vi hang mod
            int im = m + 1;
            if (mod > 0)
                lso_chu = Tach(tach_mod).ToString().Trim() + " " + Donvi(im.ToString().Trim());
            /// Tach 3 trong tach_conlai

            int i = m;
            int _m = m;
            int j = 1;
            string tach3 = "";
            string tach3_ = "";

            while (i > 0)
            {
                tach3 = tach_conlai.Trim().Substring(0, 3).Trim();
                tach3_ = tach3;
                lso_chu = lso_chu.Trim() + " " + Tach(tach3.Trim()).Trim();
                m = _m + 1 - j;
                if (!tach3_.Equals("000"))
                    lso_chu = lso_chu.Trim() + " " + Donvi(m.ToString().Trim()).Trim();
                tach_conlai = tach_conlai.Trim().Substring(3, tach_conlai.Trim().Length - 3);

                i = i - 1;
                j = j + 1;
            }
            if (lso_chu.Trim().Substring(0, 1).Equals("k"))
                lso_chu = lso_chu.Trim().Substring(10, lso_chu.Trim().Length - 10).Trim();
            if (lso_chu.Trim().Substring(0, 1).Equals("l"))
                lso_chu = lso_chu.Trim().Substring(2, lso_chu.Trim().Length - 2).Trim();
            if (lso_chu.Trim().Length > 0)
                lso_chu = dau.Trim() + " " + lso_chu.Trim().Substring(0, 1).Trim().ToUpper() + lso_chu.Trim().Substring(1, lso_chu.Trim().Length - 1).Trim() + " đồng chẵn.";

            return lso_chu.ToString().Trim();

        }
        private static string Chu(string gNumber)
        {
            string result = "";
            switch (gNumber)
            {
                case "0":
                    result = "không";
                    break;
                case "1":
                    result = "một";
                    break;
                case "2":
                    result = "hai";
                    break;
                case "3":
                    result = "ba";
                    break;
                case "4":
                    result = "bốn";
                    break;
                case "5":
                    result = "năm";
                    break;
                case "6":
                    result = "sáu";
                    break;
                case "7":
                    result = "bảy";
                    break;
                case "8":
                    result = "tám";
                    break;
                case "9":
                    result = "chín";
                    break;
            }
            return result;
        }
        private static string Donvi(string so)
        {
            string Kdonvi = "";

            if (so.Equals("1"))
                Kdonvi = "";
            if (so.Equals("2"))
                Kdonvi = "nghìn";
            if (so.Equals("3"))
                Kdonvi = "triệu";
            if (so.Equals("4"))
                Kdonvi = "tỷ";
            if (so.Equals("5"))
                Kdonvi = "nghìn tỷ";
            if (so.Equals("6"))
                Kdonvi = "triệu tỷ";
            if (so.Equals("7"))
                Kdonvi = "tỷ tỷ";

            return Kdonvi;
        }
        private static string Tach(string tach3)
        {
            string Ktach = "";
            if (tach3.Equals("000"))
                return "";
            if (tach3.Length == 3)
            {
                string tr = tach3.Trim().Substring(0, 1).ToString().Trim();
                string ch = tach3.Trim().Substring(1, 1).ToString().Trim();
                string dv = tach3.Trim().Substring(2, 1).ToString().Trim();
                if (tr.Equals("0") && ch.Equals("0"))
                    Ktach = " không trăm lẻ " + Chu(dv.ToString().Trim()) + " ";
                if (!tr.Equals("0") && ch.Equals("0") && dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm ";
                if (!tr.Equals("0") && ch.Equals("0") && !dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm lẻ " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (tr.Equals("0") && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm mười " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("0"))
                    Ktach = " không trăm mười ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("5"))
                    Ktach = " không trăm mười lăm ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười " + Chu(dv.Trim()).Trim() + " ";

                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười lăm ";

            }


            return Ktach;

        }
        public async Task<IActionResult> Views(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appoinment = await _context.appoinment
                .FirstOrDefaultAsync(m => m.Contract == id);
            //Random rand = new Random();
            //int rd = rand.Next(100, 151);
            //ViewBag.rd = appoinment.Priority;
            if(appoinment == null)
            {
                appoinment = await _context.appoinment.FirstOrDefaultAsync(m => m.ID == id);
            }
            ViewBag.Money_Letters = So_chu(appoinment.Money);
            string temp = (appoinment.Money).ToString("N", new CultureInfo("is-IS"));
            ViewBag.New_Money = temp.Substring(0, temp.Length - 3) + " VNĐ";
            if (appoinment == null)
            {
                return NotFound();
            }

            return View(appoinment);
        }

        // GET: Appoinments
        public async Task<IActionResult> Index()
        {

            return View(await _context.appoinment.ToListAsync());
        }

        // GET: Appoinments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appoinment = await _context.appoinment
                .FirstOrDefaultAsync(m => m.ID == id);
            if (appoinment == null)
            {
                return NotFound();
            }

            return View(appoinment);
        }

        // GET: Appoinments/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: Appoinments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Customer,Gender,Address,Phone,Email,Job,WorkPlace,Cmnd,Day,Place,Money,Purpose,Requires,Price,Details,DType,Cash,NCH1,NCH2,NCH3,NMS,NSHH,NSH,HKTT,sale,password,Contract, Priority")] Appoinment appoinment)
        {
            //if (ModelState.IsValid)
            //{
            foreach (Sale sale in sales)
            {
                if(sale.email == appoinment.sale)
                {
                    TempData["name"] = sale.name;
                    break;
                }

            }
            if (!check(appoinment.sale, appoinment.password))
            {
                return View(appoinment);
            }
            
            else {
                string tmp = await Escalating();
                if(tmp != "")
                {
                    string []n = tmp.Split(" ");
                    appoinment.Contract = Convert.ToInt32(n[0]);
                    appoinment.Priority = Convert.ToInt32(n[1]);
                    ViewBag.contract = appoinment.Contract;
                    ViewBag.priority = appoinment.Priority;
                }
                appoinment.cTime = DateTime.Now.ToString("ddMMyyyy HH:mm:ss.FFFFFFF");
                appoinment.Confirm = false;
                _context.Add(appoinment);
                foreach (Sale sale in sales)
                {
                    if (sale.email.Equals(appoinment.sale))
                    {
                        ViewBag.sale = sale;
                    }
                } 
                
                await _context.SaveChangesAsync();
                return View("Confirm");
            } 
        }

        public IActionResult Home(Sale sale)
        {
            return RedirectToAction("Home", "Login", new { sale });
        }

        public async Task<string> Escalating()
        {
            var tmp = await _context.appoinment.ToListAsync();
            string output = "";
            if(tmp.Count <= 0)
            {
                return "";
            }
            List<int> contracts = new List<int>();
            List<int> priors = new List<int>();
            foreach (Appoinment a in await _context.appoinment.ToListAsync())
            {
                try
                {
                    contracts.Add(a.Contract);
                    priors.Add(a.Priority);
                }
                catch(Exception e)
                {
                    break;
                }
            }
            if(contracts.Count == 0)
            {
                Random rd = new Random();
                int c = rd.Next(52, 120);
                int p = rd.Next(100, 176);
                output += Convert.ToString(c) + " " + Convert.ToString(p);
            }
            else
            {
                Random rd = new Random();
                int c = rd.Next(52, 120);
                int p = rd.Next(100, 176);
                output += Convert.ToString(c) + " " + Convert.ToString(p);
            }
            return output;
        }

        public bool check(string email, string pass)
        {
            foreach (Sale sale in sales)
            {
                if (email.Equals(sale.email)&&pass.Equals(sale.pass))
                    
                    return true;
            }
            return false;
        }

        // GET: Appoinments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appoinment = await _context.appoinment.FindAsync(id);
            if (appoinment == null)
            {
                return NotFound();
            }
            return View(appoinment);
        }

        // POST: Appoinments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Customer,Gender,Address,Phone,Email,Job,WorkPlace,Cmnd,Day,Place,Money,Purpose,Requires,Price,Details,DType,Cash,NCH1,NCH2,NCH3,NMS,NSHH,NSH,HKTT,sale,password,Contract, Priority, Confirm")] Appoinment appoinment)
        {
            if (id != appoinment.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appoinment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppoinmentExists(appoinment.ID))
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
            return View(appoinment);
        }

        // GET: Appoinments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appoinment = await _context.appoinment
                .FirstOrDefaultAsync(m => m.ID == id);
            if (appoinment == null)
            {
                return NotFound();
            }

            return View(appoinment);
        }

        // POST: Appoinments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appoinment = await _context.appoinment.FindAsync(id);
            _context.appoinment.Remove(appoinment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppoinmentExists(int id)
        {
            return _context.appoinment.Any(e => e.ID == id);
        }
    }
}
