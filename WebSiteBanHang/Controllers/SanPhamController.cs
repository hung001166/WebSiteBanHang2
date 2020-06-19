using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;
using PagedList;

namespace WebSiteBanHang.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        //Su dung partial View
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult SanPhamMain()
        {

            return View();
        }

        //Tạo 2 partial view sản phẩm 1 và 2 để hiển thị sản phẩm theo 2 style khác nhau
        [ChildActionOnly]
        public ActionResult SanPhamStyle1Partial()
        {

            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult SanPhamStyle2Partial()
        {

            return PartialView();
        }

      
        //Xây dựng 1 action load sản phẩm theo mã loại sản phẩm và mã nhà sản phẩm
        public ActionResult SanPham(int? MaLoaiSP, int? MaNSX, int? page)
        {
            if (MaLoaiSP == null || MaNSX == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }    
            //Load sp theo 2 tiêu chí: Mã loai sp và mã NSX(2 trường này nằm trong bảng sản phẩm) 
            var lstSP = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX);
            if (lstSP.Count()==0)
            {

                //thông báo ko có sản phẩm đó
                return HttpNotFound();
            }
            //thuc hien phan trang
            //tao bien so sp tren trang
            int PageSize = 6;
            //tao bien so 2: So trang hien tai
            int PageNumber = (page ?? 1);
            ViewBag.MaLoaiSP = MaLoaiSP;
            ViewBag.MaNSX = MaNSX;
            return View(lstSP.OrderBy(n=>n.MaSP).ToPagedList(PageNumber, PageSize)  );
        }

        //Xây dựng trang chi tiết sản phẩm
        public ActionResult XemChiTiet(int? id, string tensp)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id && n.DaXoa == 0);
            if (sp==null)
            {
                //thông báo ko có sản phẩm đó
                return HttpNotFound();
            }
            return View(sp);
        }
    }
}