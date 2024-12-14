using PagedList;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.WebPages;

namespace VoThanhLy_B2014583.Controllers
{
    public class ProductsController : Controller
    {
        private DataBase db = new DataBase();

        // GET: Products
        public ActionResult Index(int? page, string searchString, int? categoryId, int? supplierId, decimal? minPrice, decimal? maxPrice, int? minSl, int? maxSl)
        {
            // Lấy danh sách sản phẩm ban đầu
            var products = db.Products.AsQueryable();

            // Tìm kiếm theo tên sản phẩm
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.ProductName.Contains(searchString));
                ViewBag.SearchString = searchString;
            }

            // Lọc theo loại sản phẩm
            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryID == categoryId);
                ViewBag.CategoryId = categoryId;
            }

            // Lọc theo nhà cung cấp
            if (supplierId.HasValue)
            {
                products = products.Where(p => p.SupplierID == supplierId);
                ViewBag.SupplierId = supplierId;
            }

            // Lọc theo giá
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.UnitPrice >= minPrice);
                ViewBag.MinPrice = minPrice;
            }
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.UnitPrice <= maxPrice);
                ViewBag.MaxPrice = maxPrice;
            }

            // Lọc số lượng
            if (minSl.HasValue)
            {
                products = products.Where(p => p.UnitsOnOrder >= minSl);
                ViewBag.MinSl = minSl;
            }
            if (maxSl.HasValue)
            {
                products = products.Where(p => p.UnitsOnOrder <= maxSl);
                ViewBag.MaxSl = maxSl;
            }

            // Sắp xếp theo tên sản phẩm
            products = products.OrderByDescending(p => p.ProductID);


            // Phân trang
            int pageSize = 15; // Số sản phẩm mỗi trang
            int pageNumber = page ?? 1; // Trang hiện tại
            var pagedProducts = products.ToPagedList(pageNumber, pageSize);

            // Truyền dữ liệu vào View
            return View(pagedProducts);
        }


        public ActionResult ResultSearch(string searchString)
        {
            if (searchString == null || searchString.Trim() == "")
            {
                return View("Index");
            }
            else
            {
                ViewBag.searchString = searchString;
                var products = db.Products.Include(p => p.Category).Include(p => p.Supplier).Where(p => p.ProductName.ToUpper().Contains(searchString.ToUpper()));
                return View(products.ToList());
            }

        }



        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", product.SupplierID);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", product.SupplierID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", product.SupplierID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
