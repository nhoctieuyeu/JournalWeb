using JournalWeb.Data;
using JournalWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace JournalWeb.Controllers
{
    public class NhatKyController : Controller
    {
        private readonly AppDbContext _context;

        public NhatKyController(AppDbContext context)
        {
            _context = context;
        }

        private int? CheckLogin()
        {
            return HttpContext.Session.GetInt32("NguoiDungId");
        }

        // ===== DANH SÁCH =====
        public IActionResult Index()
        {
            var userId = CheckLogin();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Auth");

            if (HttpContext.Session.GetString("PinVerified") != "true")
                return RedirectToAction("VerifyPinLogin", "Auth");

            var list = _context.NhatKy
                .Where(x => x.NguoiDungId == userId)
                .OrderByDescending(x => x.NgayViet)
                .ToList();

            return View(list);
        }

        // ===== FORM VIẾT =====
        public IActionResult Create()
        {
            if (!CheckLogin().HasValue)
                return RedirectToAction("Login", "Auth");

            return View();
        }

        // ===== LƯU =====
        [HttpPost]
        public IActionResult Create(
            string tieuDe,
            string noiDung,
            DateTime ngayViet,
            IFormFile media)
        {
            var userId = CheckLogin();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Auth");

            if (string.IsNullOrWhiteSpace(noiDung))
            {
                ViewBag.Loi = "Nội dung không được để trống";
                return View();
            }

            string filePath = null;
            string ext = null;

            if (media != null && media.Length > 0)
            {
                ext = Path.GetExtension(media.FileName).ToLower();
                var allow = new[] { ".jpg", ".jpeg", ".png", ".mp4", ".mov" };


                if (!allow.Contains(ext))
                {
                    ViewBag.Loi = "Chỉ cho phép ảnh hoặc video";
                    return View();
                }

                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + ext;
                var fullPath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    media.CopyTo(stream);
                }

                filePath = "/uploads/" + fileName;
            }

            var nk = new NhatKy
            {
                NguoiDungId = userId.Value,
                TieuDe = tieuDe,
                NoiDung = noiDung,
                NgayViet = ngayViet,
                NgayTao = DateTime.Now
            };

            _context.NhatKy.Add(nk);
            _context.SaveChanges();

            // ===== LƯU MEDIA RIÊNG =====
            if (filePath != null)
            {
                var mediaEntity = new NhatKyMedia
                {
                    NhatKyId = nk.NhatKyId,
                    DuongDanFile = filePath,
                    LoaiMedia = ext.Contains("mp4") || ext.Contains("mov")
                        ? "video"
                        : "image",
                    NgayTao = DateTime.Now
                };

                _context.NhatKyMedia.Add(mediaEntity);
                _context.SaveChanges();
            }


            return RedirectToAction("Index");
        }
    }
}
