using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JournalWeb.Data;

namespace JournalWeb.Controllers
{
    public class TestController : Controller
    {
        private readonly AppDbContext _context;

        public TestController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult TestHash()
        {
            var mk = "manman@2025";
            var hash = JournalWeb.Helpers.SecurityHelper.HashPassword(mk);
            return Content($"Hash của '{mk}' là: {hash}");
        }

        public IActionResult Index()
        {
            int dem = _context.NguoiDung.Count();
            return Content("Ket noi DB thanh cong. So nguoi dung = " + dem);
        }
    }
}