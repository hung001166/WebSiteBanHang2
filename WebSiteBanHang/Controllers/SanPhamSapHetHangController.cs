using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    [Authorize(Roles = "QuanTri, QuanLySanPham")]
    public class SanPhamSapHetHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: SanPhamSapHetHang
        public ActionResult Index()
        {
            return View(db.SanPhams.Where(n => n.SoLuongTon <= 3).OrderByDescending(n => n.SoLuongTon));
        }
    }
}