using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    [Authorize(Roles = "QuanTri")]
    public class PhanQuyenController : Controller
    {
        
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: PhanQuyen
        public ActionResult Index()
        {
            return View(db.LoaiThanhViens.OrderByDescending(n => n.MaLoaiTV));
        }

        [HttpGet]
        public ActionResult PhanQuyen(int? id)
        {
            //Lấy MaLoaiTV cần chỉnh sửa dựa vào id
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            LoaiThanhVien ltv = db.LoaiThanhViens.SingleOrDefault(n => n.MaLoaiTV == id);
            if (ltv == null)
            {
                return HttpNotFound();
            }
            //Lay ra ds quyen (de load ra checkbox)
            ViewBag.MaQuyen = db.Quyens;
            //lay ra ds quyen cua loai thanh vien
            ViewBag.LoaiTVQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == id && n.Quyen.TrangThai == true);
            return View(ltv);
        }

        [HttpPost]
        public ActionResult PhanQuyen(int? MaLTV, IEnumerable<LoaiThanhVien_Quyen> lstPhanQuyen)
        {
            //TH1: Neu da tien hanh phan quyen nhung muon phan quyen lai
            //B1: Xoa nhung quyen cua thuoc loai TV do
            var lstDaPhanQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == MaLTV);
            if (lstDaPhanQuyen!= null)
            {
                db.LoaiThanhVien_Quyen.RemoveRange(lstDaPhanQuyen);
                db.SaveChanges();
            }

            //Ktra list danh sach quyen duoc check
            foreach (var item in lstPhanQuyen)
            {
                if (item.MaQuyen != null)
                {
                    //neu duoc check thi insert du lieu vao bang pahn quyen
                    db.LoaiThanhVien_Quyen.Add(item);
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}