using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoThanhLy_B2014583.Models;

namespace VoThanhLy_B2014583.Controllers
{
    public class ThongKeKhachHangMuaSanPhamController : Controller
    {
        // GET: ThongKeKhachHangMuaSanPham
        private DataBase db = new DataBase();
        public ActionResult Index(int? page, string searchKeyword, string sortOrder, int? minOrders, int? maxOrders)
        {
            // Lấy danh sách khách hàng từ cơ sở dữ liệu
            var khachHang = db.Customers.AsQueryable();

            // Tìm kiếm theo tên khách hàng
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                khachHang = khachHang.Where(k => k.CompanyName.ToLower().Contains(searchKeyword.ToLower()) || k.ContactName.ToLower().Contains(searchKeyword.ToLower()));
            }

            // Chuẩn bị dữ liệu cho ViewModel
            var ThongKeKhachHangMuaSanPham = khachHang.ToList().Select(k => new ThongKeKhachHangMuaSanPham_Index_ViewModel
            {
                CustomerID = k.CustomerID,
                CompanyName = k.CompanyName,
                ContactName = k.ContactName,
                ContactTitle = k.ContactTitle,
                Address = k.Address,
                City = k.City,
                Region = k.Region,
                PostalCode = k.PostalCode,
                Country = k.Country,
                Phone = k.Phone,
                Fax = k.Fax,
                SoLuongDonHangDaDat = db.Orders.Count(o => o.CustomerID == k.CustomerID)
            });

            // Lọc theo số đơn hàng trong khoảng [minOrders, maxOrders]
            if (minOrders.HasValue)
            {
                ThongKeKhachHangMuaSanPham = ThongKeKhachHangMuaSanPham.Where(k => k.SoLuongDonHangDaDat >= minOrders.Value);
            }

            if (maxOrders.HasValue)
            {
                ThongKeKhachHangMuaSanPham = ThongKeKhachHangMuaSanPham.Where(k => k.SoLuongDonHangDaDat <= maxOrders.Value);
            }

            // Sắp xếp theo số đơn hàng
            switch (sortOrder)
            {
                case "orders_asc":
                    ThongKeKhachHangMuaSanPham = ThongKeKhachHangMuaSanPham.OrderBy(k => k.SoLuongDonHangDaDat);
                    break;
                case "orders_desc":
                    ThongKeKhachHangMuaSanPham = ThongKeKhachHangMuaSanPham.OrderByDescending(k => k.SoLuongDonHangDaDat);
                    break;
            }

            // Phân trang
            int pageSize = 10; // Số khách hàng mỗi trang
            int pageNumber = page ?? 1; // Trang hiện tại
            var pagedThongKeKhachHangMuaSanPham = ThongKeKhachHangMuaSanPham.ToPagedList(pageNumber, pageSize);

            // Truyền các tham số lọc và sắp xếp vào ViewBag để sử dụng lại trong View
            ViewBag.SearchKeyword = searchKeyword;
            ViewBag.SortOrder = sortOrder;
            ViewBag.MinOrders = minOrders;
            ViewBag.MaxOrders = maxOrders;

            return View(pagedThongKeKhachHangMuaSanPham);
        }


    }
}