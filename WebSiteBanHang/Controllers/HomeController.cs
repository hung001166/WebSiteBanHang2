using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
using System.Web.Security;

namespace WebSiteBanHang.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        QuanLyBanHangEntities db = new QuanLyBanHangEntities(); 
        public ActionResult Index()
        {
            //Lần lượt tạo các ViewBag để lấy list sản phẩm từ database
            //List giày nam, mới
            var lstDTM = db.SanPhams.Where(n=>n.MaLoaiSP==1 && n.DaXoa== 0);
            //Gán vào ViewBag
            ViewBag.ListDTM = lstDTM;

            //List giày nữ, mới
            var lstLT = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.DaXoa == 0);
            //Gán vào ViewBag
            ViewBag.ListLTM = lstLT;

            //List giày trẻ em, mới
            var lstMTB = db.SanPhams.Where(n => n.MaLoaiSP == 3);
            //Gán vào ViewBag
            ViewBag.ListMTB = lstMTB;

            return View();
        }

        public ActionResult MenuPartial()
        {
            //truy van lay ve 1 list SP

            var lstSP = db.SanPhams;
            return PartialView(lstSP); 
        }

        [HttpGet]
        public ActionResult DangKy()
        {

            return View();
        }
        [HttpPost]
        public ActionResult DangKy(ThanhVien tv,FormCollection f)
        {
            //Kiểm tra captcha hợp lệ
            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                ViewBag.ThongBao = "Thêm Thành công!";
                //Thêm khách hàng vào csdl
                
                db.ThanhViens.Add(tv);
                tv.MaLoaiTV = 4;
                db.SaveChanges();
                return View();
            }

            ViewBag.ThongBao = "Sai mã captcha nha =))";
            return View();
        }

        [HttpGet]
        public ActionResult DangKy1()
        {

            return View();
        }

        //Xây dựng action Đăng nhập
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            /* 
            //Kiểm tra tên đăng nhập và mật khảu
            string sTaiKhoan = f["txtTenDangNhap"].ToString();
            string sMatKhau = f["txtMatKhau"].ToString();

            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);
            if (tv != null)
            {
                Session["TaiKhoan"] = tv;
                return RedirectToAction("Index");
            }    

            return RedirectToAction("Index"); */

            string taikhoan = f["txtTenDangNhap"].ToString();
            string matkhau = f["txtMatKhau"].ToString();
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == taikhoan && n.MatKhau == matkhau); // truy vấn đăng nhập ktra thông tin thành viên
            if (tv != null)
            {
                //lấy ra list quyền của thành viên tương ứng với loại thành viên
                var lstQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV);
                //DUyệt list quyền
                string Quyen = "";
                if (tv.MaLoaiTV == 3 || tv.MaLoaiTV==4)
                {
                    Session["TaiKhoan"] = tv;
                    return RedirectToAction("Index");
                }   
                else
                {
                    if (lstQuyen.Count() != 0)
                    {
                        foreach (var item in lstQuyen)
                        {
                            Quyen += item.Quyen.MaQuyen + ",";
                        }
                        Quyen = Quyen.Substring(0, Quyen.Length - 1);//cắt đi dấu phẩy
                        PhanQuyen(tv.TaiKhoan.ToString(), Quyen);
                        Session["TaiKhoan"] = tv;

                        return Content("<script>window.location.reload();</script>");
                    }
                }

                
            }
            return Content("Tài khoản hoặc Mật khẩu không đúng!");
        }

        public void PhanQuyen(string TaiKhoan, string Quyen)
        {
            FormsAuthentication.Initialize();

            var ticket = new FormsAuthenticationTicket(1, //version
                                                    TaiKhoan, //user
                                                    DateTime.Now, //begin
                                                    DateTime.Now.AddHours(3), //timeout
                                                    false, //remember
                                                    Quyen, //permission.."admin" or more than one "DangKy,QuanLyDonHang,QuanLySanPham"
                                                    FormsAuthentication.FormsCookiePath // tự lấy đường dẫn cookie - duy trì đăng nhập, lưu hđ cho đến timeout
                                                    );
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);

        }

        //Tao trang ngan chan phan quyen
        public ActionResult LoiPhanQuyen()
        {

            return View();
        }

        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}