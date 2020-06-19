using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    public class GioHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();  

        //Lay gio Hang
        public List<ItemGioHang> LayGioHang()
        {
            //Gio hang da ton tai
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                //Neu  session gio hang chua ton tai thi khoi tai gio hang
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;
                
            }
            return lstGioHang;
        }

        //THem gio hang thong thuong (load lai trang)
        public ActionResult ThemGioHang(int MaSP, string strURL)
        {
            //Ktra sp co ton tai trong CSDL ko
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //Trang duong dan khong hop le
                Response.StatusCode = 404;
                return null;
            }
            //lay gio hang
            List<ItemGioHang> lstGioHang = LayGioHang();

            // Neu sp da ton tai trong gio hang
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);

            if (spCheck != null)
            {
                //ktra số lượng tồn trc khi cho khách mua hàng
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return View("ThongBao");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);
            }
           
            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }

            lstGioHang.Add(itemGH);
            return Redirect(strURL);
        }

        //Tính tổng số lượng
        public double TinhTongSoLuong()
        {
            //lay gio hang
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.SoLuong);
        }
        //Tinhs tong tien
        public decimal TinhTongTien()
        {
            //lay gio hang
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.ThanhTien);
        }

        public ActionResult GioHangPartial()
        {
            if (TinhTongSoLuong() == 0 )
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }


        // GET: GioHang
        public ActionResult XemGioHang()
        {
            //Lay gio hang
            List<ItemGioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }


        //Chinh sua gio hang
        [HttpGet]
        public ActionResult SuaGioHang(int MaSP)
        {
            //Ktra session gio hang ton tai chua
            if (Session["GioHang"]==null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Ktra sp co ton tai trong CSDL ko
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //Trang duong dan khong hop le
                Response.StatusCode = 404;
                return null;
            }
            //lay list gio hang tu session
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Ktra sp đó có tồn tại trong giỏ hàng hay không
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck==null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Lay list gio hang tao giao dien
            ViewBag.GioHang = lstGioHang;

            //Neu ton tai roi 
            return View(spCheck);
        }
 
        //Xu ly cap nhat gio hang
        [HttpPost]
        public ActionResult CapNhatGioHang(ItemGioHang itemGH)
        {
            //ktra so luong ton
            SanPham spCheck = db.SanPhams.Single(n => n.MaSP == itemGH.MaSP);
            if (spCheck.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            //cap nhat so luong trong session gio hang
            //B1: lay List<GioHang> tu session["Giohang"]
            List<ItemGioHang> lstGH = LayGioHang();
            //B2: Lay sp can cap nhat tu trong List<GioHang> ra
            ItemGioHang itemGHUpdate = lstGH.Find(n => n.MaSP == itemGH.MaSP);
            //B3: Cap nhat lai so luong  va thanh tien
            itemGHUpdate.SoLuong = itemGH.SoLuong;
            itemGHUpdate.ThanhTien = itemGHUpdate.SoLuong * itemGHUpdate.DonGia;

            //return RedirectToAction("SuaGioHang", new { @MaSP = itemGH.MaSP });
            return RedirectToAction("XemGioHang");
        }

        public ActionResult XoaGioHang(int MaSP)
        {
            //Ktra session gio hang ton tai chua
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Ktra sp co ton tai trong CSDL ko
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //Trang duong dan khong hop le
                Response.StatusCode = 404;
                return null;
            }
            //lay list gio hang tu session
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Ktra sp đó có tồn tại trong giỏ hàng hay không
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Xoa item trong gio hang
            lstGioHang.Remove(spCheck); 

            return RedirectToAction("XemGioHang");
        }

        //Xay dung chuc nag dat hang
        public ActionResult DatHang(KhachHang kh) // kh: biến get dữ liệu từ form sang, truyền vào biến khang
        {
            //Ktra session gio hang ton tai chua
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Đối với KH thông thường
            KhachHang khang = new KhachHang();
            if (Session["TaiKhoan"] == null)
            {
                //Thêm khách hàng vào bảng khách hàng cho KH vãng lai (chưa có TK)
                khang = kh;
                db.KhachHangs.Add(khang);
                db.SaveChanges();
            }    
            else
            {
                //Đối với KH là thành viên (có TK)
                ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
                khang.TenKh = tv.HoTen;
                khang.DiaChi = tv.DiaChi;
                khang.Email = tv.Email;
                khang.SoDienThoai = tv.SoDienThoai;
                db.KhachHangs.Add(khang);
                db.SaveChanges();
            }    

            //them don hang
            DonDatHang ddh = new DonDatHang();
            ddh.MaKH = khang.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            ddh.DaHuy = false;
            db.DonDatHangs.Add(ddh);
            db.SaveChanges();

            //Them chi tiet don hang
            List<ItemGioHang> lstGH = LayGioHang();
            foreach (var item in lstGH)
            {
                ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
                ctdh.MaDDH = ddh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.DonGia;
                db.ChiTietDonDatHangs.Add(ctdh);
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang");

        }
    }
}