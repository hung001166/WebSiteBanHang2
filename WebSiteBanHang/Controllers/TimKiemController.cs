using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    public class TimKiemController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: TimKiem
        public ActionResult KQTimKiem(string sTuKhoa)
        {
            //tim kiem theo ten sp
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));

            return View(lstSP.OrderBy(n=>n.TenSP ));
        }
    }
}