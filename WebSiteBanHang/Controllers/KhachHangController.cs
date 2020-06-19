using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    [Authorize(Roles ="QuanTri, QuanLySanPham")]
    public class KhachHangController : Controller
    {
        // GET: KhachHang
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        [Authorize(Roles = "QuanLySanPham")]
        public ActionResult Index()
        {
            //truy van du lieu thong qua cau lenh
            //Doi lstKH se lay toan bo du lieu tu bang KhachHang

            /* - Cach 1: lay du lieu la 1 danh sach khach hang
            var lstKH = from KH in db.KhachHangs select KH; */

            // - Cach 2: dung phuong thuc ho tro san
            var lstKH = db.KhachHangs;
            return View(lstKH);
        }

        [Authorize(Roles = "QuanTri")]
        public ActionResult Index1()
        {
            var lstKH = from KH in db.KhachHangs select KH;
            return View(lstKH);
        }

        public ActionResult TruyVan1DoiTuong()
        {
            //truy van 1 doi tuong bang cau lenh truy van
            //B1: Lấy ra danh sách khách hàng
            var lstKH = from kh in db.KhachHangs where kh.MaKH == 2 select kh;
            /*B2: 
            KhachHang khang = lstKH.FirstOrDefault(); */
            //B2: Lấy đối tượng khách hàng dựa trên phương thức bổ trợ
            KhachHang khang = db.KhachHangs.SingleOrDefault(n => n.MaKH == 2);

            return View(khang);
        }

        public ActionResult SortDuLieu()
        {
            //phuong thuc sap xep du lieu
            List<KhachHang> lstKH = db.KhachHangs.OrderBy(n => n.TenKh).ToList();
            return View(lstKH);
        }

        public ActionResult GroupDuLieu()
        {
            //Group du lieu
            List<ThanhVien> lstKH = db.ThanhViens.OrderByDescending(n => n.TaiKhoan).ToList();
            return View(lstKH);
        }
    }
}