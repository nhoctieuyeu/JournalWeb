using JournalWeb.Data;
using JournalWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
                .Include(x => x.Medias)
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
            int? moodLevel,
            string moodLabel,
            List<IFormFile> medias)
        {
            var userId = CheckLogin();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Auth");

            if (string.IsNullOrWhiteSpace(noiDung))
            {
                ViewBag.Loi = "Nội dung không được để trống";
                return View();
            }

            var uploadedFiles = new List<(string FilePath, string Ext)>();

            if (medias != null && medias.Any(f => f != null && f.Length > 0))
            {
                var allow = new[] { ".jpg", ".jpeg", ".png", ".mp4", ".mov", ".webm" };
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(folder);

                foreach (var media in medias.Where(f => f != null && f.Length > 0))
                {
                    var ext = Path.GetExtension(media.FileName).ToLower();

                    if (!allow.Contains(ext))
                    {
                        ViewBag.Loi = "Chỉ cho phép ảnh hoặc video ngắn (.jpg, .jpeg, .png, .mp4, .mov, .webm)";
                        return View();
                    }

                    var fileName = Guid.NewGuid() + ext;
                    var fullPath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        media.CopyTo(stream);
                    }

                    uploadedFiles.Add(("/uploads/" + fileName, ext));
                }
            }

            var finalNoiDung = noiDung.Trim();
            string safeMoodLabel = null;
            if (!string.IsNullOrWhiteSpace(moodLabel))
                safeMoodLabel = moodLabel.Trim().Replace("|", "/").Replace("]", "").Replace("[", "");

            string camXuc = null;
            if (moodLevel.HasValue && moodLevel.Value >= 0 && moodLevel.Value <= 6 && !string.IsNullOrWhiteSpace(safeMoodLabel))
                camXuc = $"{moodLevel.Value}|{safeMoodLabel}";
            else if (!string.IsNullOrWhiteSpace(safeMoodLabel))
                camXuc = safeMoodLabel;

            var nk = new NhatKy
            {
                NguoiDungId = userId.Value,
                TieuDe = tieuDe,
                NoiDung = finalNoiDung,
                NgayViet = ngayViet,
                NgayTao = DateTime.Now
            };

            // Tương thích ngược: nếu model NhatKy có các trường mood mới thì gán, không có thì bỏ qua.
            var nhatKyType = typeof(NhatKy);
            nhatKyType.GetProperty("CamXuc")?.SetValue(nk, camXuc);
            nhatKyType.GetProperty("IsRiengTu")?.SetValue(nk, false);
            nhatKyType.GetProperty("NgayCapNhat")?.SetValue(nk, DateTime.Now);

            _context.NhatKy.Add(nk);
            _context.SaveChanges();

            // ===== LƯU MEDIA RIÊNG =====
            if (uploadedFiles.Any())
            {
                foreach (var file in uploadedFiles)
                {
                    var mediaEntity = new NhatKyMedia
                    {
                        NhatKyId = nk.NhatKyId,
                        DuongDanFile = file.FilePath,
                        LoaiMedia = file.Ext == ".mp4" || file.Ext == ".mov" || file.Ext == ".webm"
                            ? "video"
                            : "image",
                        NgayTao = DateTime.Now
                    };

                    _context.NhatKyMedia.Add(mediaEntity);
                }

                _context.SaveChanges();
            }


            return RedirectToAction("Index");
        }
    }
}
