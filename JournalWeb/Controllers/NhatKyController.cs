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

        private int? CheckLogin() => HttpContext.Session.GetInt32("NguoiDungId");

        public IActionResult Index()
        {
            var userId = CheckLogin();
            if (!userId.HasValue) return RedirectToAction("Login", "Auth");
            if (HttpContext.Session.GetString("PinVerified") != "true")
                return RedirectToAction("VerifyPinLogin", "Auth");

            var moodColorMap = LoadMoodColorMap();

            var list = _context.NhatKy
                .Include(x => x.Medias)
                .Where(x => x.NguoiDungId == userId)
                .OrderByDescending(x => x.NgayTao)
                .ThenByDescending(x => x.NhatKyId)
                .ToList();

            var cards = list.Select(x => BuildCardVm(x, moodColorMap)).ToList();

            var grouped = cards
                .GroupBy(x => new { Year = (x.NgayViet ?? DateTime.Now).Year, Month = (x.NgayViet ?? DateTime.Now).Month })
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

        public IActionResult Create()
        {
            if (!CheckLogin().HasValue) return RedirectToAction("Login", "Auth");
            return View();
        }

        [HttpPost]
        public IActionResult Create(string tieuDe, string noiDung, DateTime ngayViet, int? moodLevel, string moodLabel, List<IFormFile> medias)
        {
            var userId = CheckLogin();
            if (!userId.HasValue) return RedirectToAction("Login", "Auth");

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
                    using var stream = new FileStream(fullPath, FileMode.Create);
                    media.CopyTo(stream);
                    uploadedFiles.Add(("/uploads/" + fileName, ext));
                }
            }

            var finalNoiDung = noiDung.Trim();
            var safeMoodLabel = string.IsNullOrWhiteSpace(moodLabel)
                ? null
                : moodLabel.Trim().Replace("|", "/").Replace("]", "").Replace("[", "");

            string camXuc = null;
            if (moodLevel.HasValue && moodLevel.Value >= 1 && moodLevel.Value <= 7 && !string.IsNullOrWhiteSpace(safeMoodLabel))
                camXuc = $"{moodLevel.Value}|{safeMoodLabel}";
            else if (!string.IsNullOrWhiteSpace(safeMoodLabel))
                camXuc = safeMoodLabel;

            if (!string.IsNullOrWhiteSpace(camXuc))
            {
                var splitIndex = camXuc.IndexOf('|');
                if (splitIndex > -1)
                {
                    var lv = camXuc.Substring(0, splitIndex);
                    var label = camXuc.Substring(splitIndex + 1);
                    finalNoiDung = $"[[MOOD|{lv}|{label}]]\n{finalNoiDung}";
                }
                else
                {
                    finalNoiDung = $"[[MOOD|4|{camXuc}]]\n{finalNoiDung}";
                }
            }

            var nk = new NhatKy
            {
                NguoiDungId = userId.Value,
                MucDoId = moodLevel,
                TieuDe = tieuDe,
                NoiDung = finalNoiDung,
                NgayTao = ngayViet == default ? DateTime.Now : ngayViet,
                IsRiengTu = true
            };

            _context.NhatKy.Add(nk);
            _context.SaveChanges();

            if (uploadedFiles.Any())
            {
                foreach (var file in uploadedFiles)
                {
                    _context.NhatKyMedia.Add(new NhatKyMedia
                    {
                        NhatKyId = nk.NhatKyId,
                        DuongDanFile = file.FilePath,
                        LoaiMedia = file.Ext == ".mp4" || file.Ext == ".mov" || file.Ext == ".webm" ? "video" : "image",
                        ThoiLuong = null,
                        NgayTao = DateTime.Now
                    });
                }
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        private JournalCardVm BuildCardVm(NhatKy item, Dictionary<int, string> moodColorMap)
        {
            var content = item.NoiDung ?? string.Empty;
            var moodLevel = item.MucDoId ?? 4;
            var moodLabel = string.Empty;

            if (content.StartsWith("[[MOOD|", StringComparison.Ordinal))
            {
                var end = content.IndexOf("]]", StringComparison.Ordinal);
                if (end > 0)
                {
                    var token = content.Substring(7, end - 7);
                    var split = token.IndexOf('|');
                    if (split > -1)
                    {
                        if (int.TryParse(token.Substring(0, split), out var lv)) moodLevel = lv;
                        moodLabel = token.Substring(split + 1);
                        content = content.Substring(end + 2).Trim();
                    }
                }
            }

            var moodColor = moodColorMap.TryGetValue(moodLevel, out var c)
                ? c
                : "linear-gradient(180deg, #F8EAD9 0%, #F7C66B 100%)";

            return new JournalCardVm
            {
                NhatKyId = item.NhatKyId,
                TieuDe = item.TieuDe,
                NoiDungTomTat = content,
                MoodLabel = string.IsNullOrWhiteSpace(moodLabel) ? ("Mức " + moodLevel) : moodLabel,
                MoodLevel = moodLevel,
                MoodColor = moodColor,
                MoodTopic = GuessMoodTopic(content),
                NgayViet = item.NgayTao,
                DisplayDateLine = BuildDisplayDateLine(item.NgayTao),
                Medias = item.Medias?.OrderBy(x => x.MediaId).ToList() ?? new List<NhatKyMedia>()
            };
        }

        private Dictionary<int, string> LoadMoodColorMap()
        {
            var map = new Dictionary<int, string>();
            try
            {
                var conn = _context.Database.GetDbConnection();
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT MucDoId, MauNenGradient FROM MucDoCamXuc";
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0) && !reader.IsDBNull(1))
                        map[Convert.ToInt32(reader.GetValue(0))] = Convert.ToString(reader.GetValue(1))?.Trim();
                }
            }
            catch { }
            return map;
        }

        private string BuildMonthTitle(int month, int year)
        {
            var now = DateTime.Now;
            return year == now.Year ? $"Tháng {month}" : $"tháng {month} năm {year}";
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

        private string GuessMoodTopic(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return "Gia đình";
            var lower = content.ToLower();
            if (lower.Contains("công việc") || lower.Contains("dự án")) return "Công việc";
            if (lower.Contains("gia đình") || lower.Contains("nhà")) return "Gia đình";
            if (lower.Contains("sức khỏe") || lower.Contains("bệnh")) return "Sức khỏe";
            return "Các hoạt động khác";
        }
    }
}
