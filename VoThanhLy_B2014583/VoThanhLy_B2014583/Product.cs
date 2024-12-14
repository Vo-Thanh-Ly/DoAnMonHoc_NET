using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VoThanhLy_B2014583
{
    public partial class Product
    {
        public Product()
        {
            this.Order_Details = new HashSet<Order_Detail>();
        }

        [Key]
        [Required(ErrorMessage = "Mã sản phẩm không được để trống.")]
        [Display(Name = "Mã sản phẩm")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống.")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự.")]
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Nhà cung cấp không được để trống.")]
        [Display(Name = "Nhà cung cấp")]
        public Nullable<int> SupplierID { get; set; }

        [Required(ErrorMessage = "Loại sản phẩm không được để trống.")]
        [Display(Name = "Loại sản phẩm")]
        public Nullable<int> CategoryID { get; set; }

        [Required(ErrorMessage = "Thông tin số lượng mỗi đơn vị không được để trống.")]
        [StringLength(50, ErrorMessage = "Thông tin số lượng mỗi đơn vị không được vượt quá 50 ký tự.")]
        [Display(Name = "Số lượng mỗi đơn vị")]
        public string QuantityPerUnit { get; set; }

        [Required(ErrorMessage = "Đơn giá không được để trống.")]
        [Range(0, double.MaxValue, ErrorMessage = "Đơn giá phải là số không âm.")]
        [Display(Name = "Đơn giá")]
        public Nullable<decimal> UnitPrice { get; set; }

        [Required(ErrorMessage = "Số lượng tồn kho không được để trống.")]
        [Range(0, short.MaxValue, ErrorMessage = "Số lượng tồn kho phải là số không âm.")]
        [Display(Name = "Số lượng tồn kho")]
        public Nullable<short> UnitsInStock { get; set; }

        [Required(ErrorMessage = "Số lượng đặt hàng không được để trống.")]
        [Range(0, short.MaxValue, ErrorMessage = "Số lượng đặt hàng phải là số không âm.")]
        [Display(Name = "Số lượng đặt hàng")]
        public Nullable<short> UnitsOnOrder { get; set; }

        [Required(ErrorMessage = "Mức đặt hàng lại không được để trống.")]
        [Range(0, short.MaxValue, ErrorMessage = "Mức đặt hàng lại phải là số không âm.")]
        [Display(Name = "Mức đặt hàng lại")]
        public Nullable<short> ReorderLevel { get; set; }

        [Required(ErrorMessage = "Thông tin ngừng kinh doanh không được để trống.")]
        [Display(Name = "Ngừng kinh doanh")]
        public bool Discontinued { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Order_Detail> Order_Details { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
