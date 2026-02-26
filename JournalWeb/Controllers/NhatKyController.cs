using JournalWeb.Data;
using JournalWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace JournalWeb.Controllers
{
    public class NhatKyController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public NhatKyController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        private int? GetCurrentUserId() => HttpContext.Session.GetInt32("NguoiDungId");
        private bool IsPinVerified() => HttpContext.Session.GetString("PinVerified") == "true";

        // ================= INDEX =================
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue) return RedirectToAction("Login", "Auth");
            if (!IsPinVerified()) return RedirectToAction("VerifyPinLogin", "Auth");

            var nhatKys = await _context.NhatKy
                .Include(x => x.CamXuc)
                .Include(x => x.Medias)
                .Include(x => x.NhatKy_CamXucChiTiets)
                    .ThenInclude(x => x.CamXucChiTiet)
                .Include(x => x.NhatKy_DanhMucs)
                    .ThenInclude(x => x.DanhMuc)
                .Where(x => x.NguoiDungId == userId)
                .OrderByDescending(x => x.NgayViet)
                .ThenByDescending(x => x.NhatKyId)
                .ToListAsync();

            var cards = nhatKys.Select(x => BuildCardVm(x)).ToList();

            var grouped = cards
                .GroupBy(x => new { Year = x.NgayViet.Year, Month = x.NgayViet.Month })
                .OrderByDescending(g => g.Key.Year)
                .ThenByDescending(g => g.Key.Month)
                .Select(g => new JournalMonthGroupVm
                {
                    GroupTitle = BuildMonthTitle(g.Key.Month, g.Key.Year),
                    Items = g.ToList()
                })
                .ToList();

            return View(grouped);
        }

        // ================= CREATE (GET) =================
        public async Task<IActionResult> Create()
        {
            if (!GetCurrentUserId().HasValue) return RedirectToAction("Login", "Auth");
            if (!IsPinVerified()) return RedirectToAction("VerifyPinLogin", "Auth");

            ViewBag.CamXucList = await _context.CamXuc
                .Include(x => x.CamXucChiTiets.OrderBy(ct => ct.ThuTu))
                .OrderBy(x => x.CamXucId)
                .ToListAsync();

            ViewBag.DanhMucList = await _context.DanhMuc.OrderBy(d => d.DanhMucId).ToListAsync();
            return View();
        }

        // ================= CREATE (POST) =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            string tieuDe,
            string noiDung,
            DateTime ngayViet,
            int mucDoId,
            int moodLevel,
            List<int> chiTietIds,
            List<int> danhMucIds,
            List<IFormFile> medias)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue) return RedirectToAction("Login", "Auth");
            if (!IsPinVerified()) return RedirectToAction("VerifyPinLogin", "Auth");

            if (string.IsNullOrWhiteSpace(noiDung))
            {
                ViewBag.Loi = "Nội dung không được để trống";
                return await LoadCreateViewData();
            }

            if (mucDoId <= 0 && moodLevel > 0)
            {
                mucDoId = moodLevel;
            }

            var camXuc = await _context.CamXuc.FindAsync(mucDoId);
            if (camXuc == null)
            {
                ViewBag.Loi = "Vui lòng chọn cảm xúc";
                return await LoadCreateViewData();
            }

            // Xử lý upload media
            var uploadedFiles = new List<(string FilePath, string Ext)>();
            if (medias != null && medias.Any(f => f != null && f.Length > 0))
            {
                var allow = new[] { ".jpg", ".jpeg", ".png", ".mp4", ".mov", ".webm" };
                var uploadFolder = Path.Combine(_env.WebRootPath, "Uploads");
                Directory.CreateDirectory(uploadFolder);

                foreach (var media in medias.Where(f => f != null && f.Length > 0))
                {
                    var ext = Path.GetExtension(media.FileName).ToLower();
                    if (!allow.Contains(ext))
                    {
                        ViewBag.Loi = "Chỉ cho phép ảnh hoặc video ngắn (.jpg, .jpeg, .png, .mp4, .mov, .webm)";
                        return await LoadCreateViewData();
                    }

                    var fileName = $"{Guid.NewGuid()}{ext}";
                    var fullPath = Path.Combine(uploadFolder, fileName);
                    using var stream = new FileStream(fullPath, FileMode.Create);
                    await media.CopyToAsync(stream);
                    uploadedFiles.Add(($"/Uploads/{fileName}", ext));
                }
            }

            // Tạo bài viết
            var nhatKy = new NhatKy
            {
                NguoiDungId = userId.Value,
                MucDoId = mucDoId,
                TieuDe = tieuDe?.Trim(),
                NoiDung = noiDung.Trim(),
                NgayViet = ngayViet == default ? DateTime.Now : ngayViet,
                NgayTao = DateTime.Now,
                IsRiengTu = true
            };

            _context.NhatKy.Add(nhatKy);
            await _context.SaveChangesAsync();

            // Lưu chi tiết cảm xúc
            if (chiTietIds != null && chiTietIds.Any())
            {
                foreach (var chiTietId in chiTietIds.Distinct())
                {
                    _context.NhatKy_CamXucChiTiet.Add(new NhatKy_CamXucChiTiet
                    {
                        NhatKyId = nhatKy.NhatKyId,
                        ChiTietId = chiTietId
                    });
                }
                await _context.SaveChangesAsync();
            }

            // Lưu danh mục
            if (danhMucIds != null && danhMucIds.Any())
            {
                foreach (var danhMucId in danhMucIds.Distinct())
                {
                    _context.NhatKy_DanhMuc.Add(new NhatKy_DanhMuc
                    {
                        NhatKyId = nhatKy.NhatKyId,
                        DanhMucId = danhMucId
                    });
                }
                await _context.SaveChangesAsync();
            }

            // Lưu media
            if (uploadedFiles.Any())
            {
                foreach (var file in uploadedFiles)
                {
                    _context.NhatKyMedia.Add(new NhatKyMedia
                    {
                        NhatKyId = nhatKy.NhatKyId,
                        DuongDanFile = file.FilePath,
                        LoaiMedia = file.Ext == ".mp4" || file.Ext == ".mov" || file.Ext == ".webm" ? "video" : "image",
                        ThoiLuong = null,
                        NgayTao = DateTime.Now
                    });
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        private async Task<IActionResult> LoadCreateViewData()
        {
            ViewBag.CamXucList = await _context.CamXuc
                .Include(x => x.CamXucChiTiets.OrderBy(ct => ct.ThuTu))
                .OrderBy(x => x.CamXucId)
                .ToListAsync();
            ViewBag.DanhMucList = await _context.DanhMuc.OrderBy(d => d.DanhMucId).ToListAsync();
            return View();
        }

        private JournalCardVm BuildCardVm(NhatKy item)
        {
            var camXuc = item.CamXuc ?? new CamXuc();
            var chiTiets = item.NhatKy_CamXucChiTiets?
                .Select(x => x.CamXucChiTiet?.TenChiTiet)
                .Where(x => x != null)
                .ToList() ?? new List<string>();
            var danhMucs = item.NhatKy_DanhMucs?
                .Select(x => x.DanhMuc?.TenDanhMuc)
                .Where(x => x != null)
                .ToList() ?? new List<string>();

            return new JournalCardVm
            {
                NhatKyId = item.NhatKyId,
                TieuDe = item.TieuDe,
                NoiDungTomTat = item.NoiDung,
                MoodLevel = camXuc.CamXucId,
                MoodLabel = camXuc.TenCamXuc ?? "Không xác định",
                MoodColor = camXuc.MaMauGradient ?? "#F7C66B",
                MoodChiTiets = chiTiets,
                DanhMucs = danhMucs,
                NgayViet = item.NgayViet,
                DisplayDateLine = BuildDisplayDateLine(item.NgayViet),
                Medias = item.Medias?.OrderBy(x => x.MediaId).ToList() ?? new List<NhatKyMedia>()
            };
        }

        private string BuildMonthTitle(int month, int year)
        {
            var now = DateTime.Now;
            return year == now.Year ? $"Tháng {month}" : $"Tháng {month} năm {year}";
        }

        private string BuildDisplayDateLine(DateTime dt)
        {
            var thu = dt.DayOfWeek switch
            {
                DayOfWeek.Monday => "Thứ Hai",
                DayOfWeek.Tuesday => "Thứ Ba",
                DayOfWeek.Wednesday => "Thứ Tư",
                DayOfWeek.Thursday => "Thứ Năm",
                DayOfWeek.Friday => "Thứ Sáu",
                DayOfWeek.Saturday => "Thứ Bảy",
                _ => "Chủ Nhật"
            };
            return $"{thu}, ngày {dt.Day} thg {dt.Month}, {dt.Year}";
        }
    }
}
