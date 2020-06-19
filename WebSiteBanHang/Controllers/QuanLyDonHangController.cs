using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    public class QuanLyDonHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: QuanLyDonHang
        public ActionResult Index()
        {

            return View(db.DonDatHangs.Where(n => n.TinhTrangGiaoHang == false && n.DaHuy == false).OrderByDescending(n => n.NgayDat));
        }


        [HttpGet]
        public ActionResult DuyetDonHang(int? id)
        {
            //Lấy sp cần chỉnh sửa dựa vào id
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            DonDatHang dh = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);
            if (dh == null)
            {
                return HttpNotFound();
            }
            var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            return View(dh);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult DuyetDonHang(DonDatHang model)
        {

            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            model.TinhTrangGiaoHang = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult HuyDonHang(int? id)
        {
            //Lấy sp cần chỉnh sửa dựa vào id
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            DonDatHang dh = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);
            if (dh == null)
            {
                return HttpNotFound();
            }

            return View(dh);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult HuyDonHang(DonDatHang model)
        {

            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            model.DaHuy = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}