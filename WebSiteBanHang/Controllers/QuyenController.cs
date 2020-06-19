using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    [Authorize(Roles = "QuanTri")]
    public class QuyenController : Controller
    {
        // GET: Quyen
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            return View(db.Quyens.Where(n => n.TrangThai == true).OrderBy(n => n.TenQuyen));
        }

        [HttpGet]
        public ActionResult ThemQuyen()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemQuyen(Quyen quyen)
        {
            if (ModelState.IsValid)
            {
                db.Quyens.Add(quyen);
                quyen.TrangThai = true;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult SuaQuyen(string id)
        {
            //Lấy quyền cần chỉnh sửa dựa vào id
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            Quyen q = db.Quyens.SingleOrDefault(n => n.MaQuyen == id);
            if (q == null)
            {
                return HttpNotFound();
            }

            return View(q);
        }
        
        [HttpPost]
        public ActionResult SuaQuyen(Quyen model)
        {

            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            model.TrangThai = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult XoaQuyen(string id)
        {
            //Lấy quyền cần chỉnh sửa dựa vào id
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            Quyen q = db.Quyens.SingleOrDefault(n => n.MaQuyen == id);
            if (q == null)
            {
                return HttpNotFound();
            }

            return View(q);
        }

        [HttpPost]
        public ActionResult XoaQuyen(Quyen model)
        {

            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            model.TrangThai = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}