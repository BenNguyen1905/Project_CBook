﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBook.Models;
using CBook.Models.Process;
using System.Web.Script.Serialization;

namespace CBook.Controllers
{
    public class CartController : Controller
    {
        //khởi tạo dữ liệu
        CsK23T1cEntities db = new CsK23T1cEntities();

        //tạo 1 chuỗi hằng để gán session
        private const string CartSession = "CartSession";

        // GET: Cart/ : trang giỏ hàng
       

        //GET : /Cart/CartHeader : đếm sổ sản phẩm trong giỏ hàng
        //PartialView : CartHeader
        public ActionResult CartHeader()
        {
            var cart = Session[CartSession];
            var list = new List<CartModel>();
            if (cart != null)
            {
                list = (List<CartModel>)cart;
            }

            return PartialView(list);
        }

        //Xóa 1 sản phẩm trong giỏ hàng
        public JsonResult Delete(int id)
        {
            var sessionCart = (List<CartModel>)Session[CartSession];
            //xóa những giá trị mà có mã sách giống với id
            sessionCart.RemoveAll(x => x.sach.MaSach == id);
            //gán lại giá trị cho session
            Session[CartSession] = sessionCart;

            return Json(new
            {
                status = true
            });
        }

        //Xóa tất cả các sản phẩm trong giỏ hàng
        public JsonResult DeleteAll()
        {
            Session[CartSession] = null;
            return Json(new
            {
                status = true
            });
        }

        //Cập nhật giỏ hàng
        public JsonResult Update(string cartModel)
        {
            //tạo 1 đối tượng dạng json
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartModel>>(cartModel);

            //ép kiểu từ session
            var sessionCart = (List<CartModel>)Session[CartSession];

            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.Single(x => x.sach.MaSach == item.sach.MaSach);
                if (jsonItem != null)
                {
                    item.Quantity = jsonItem.Quantity;
                }
            }
            //cập nhật lại session
            Session[CartSession] = sessionCart;

            return Json(new
            {
                status = true
            });
        }

        //GET : /Cart/AddItem/?id=?&quantity=1 : thêm sản phẩm vào giỏ hàng
        public ActionResult AddItem(int id, int quantity)
        {
            //lấy mã sách và gán đối tượng
            var sach = new AdminProcess().GetIdBook(id);

            //lấy giỏ hàng từ session
            var cart = Session[CartSession];

            //nếu đã có sản phẩm trong giỏ hàng
            if (cart != null)
            {
                var list = (List<CartModel>)cart;
                if (list.Exists(x => x.sach.MaSach == id))
                {

                    foreach (var item in list)
                    {
                        if (item.sach.MaSach == id)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    //tạo mới đối tượng cart item
                    var item = new CartModel();
                    item.sach = sach;
                    item.Quantity = quantity;
                    list.Add(item);
                }

                //Gán vào session
                Session[CartSession] = list;
            }
            else
            {
                //tạo mới giỏ hàng
                var item = new CartModel();
                item.sach = sach;
                item.Quantity = quantity;
                var list = new List<CartModel>();
                list.Add(item);

                //gán vào session
                Session[CartSession] = list;
            }

            return RedirectToAction("Index");
        }

        //Thông tin khách hàng
        [HttpGet]
        [ChildActionOnly]
        public PartialViewResult UserInfo()
        {
            //lấy dữ liệu từ session
            var model = Session["User"];

            if (ModelState.IsValid)
            {
                //tìm tên tài khoản
                var result = db.KhachHangs.SingleOrDefault(x => x.TaiKhoan == model);

                //trả về dữ liệu tương ứng
                return PartialView(result);
            }

            return PartialView();
        }

        [HttpGet]
        public ActionResult Payment()
        {
            //kiểm tra đăng nhập
            if (Session["User"] == null || Session["User"].ToString() == "")
            {
                return RedirectToAction("LoginPage", "User");
            }

            var cart = Session[CartSession];
            var list = new List<CartModel>();
            var sl = 0;
            decimal? total = 0;
            if (cart != null)
            {
                list = (List<CartModel>)cart;
                sl = list.Sum(x => x.Quantity);
                total = list.Sum(x => x.Total);
            }
            ViewBag.Quantity = sl;
            ViewBag.Total = total;
            return View(list);
        }

        [HttpPost]
        public ActionResult Payment(int MaKH)
        {
            var order = new DonDatHang();
            order.NgayDat = DateTime.Now;
            order.NgayGiao = DateTime.Now.AddDays(3);
            order.TinhTrang = true; //đã nhận hàng
            order.MaKH = MaKH;

            try
            {
                //thêm dữ liệu vào đơn đặt hàng
                var result1 = new OrderProcess().Insert(order);

                var cart = (List<CartModel>)Session[CartSession];
                var result2 = new OderDetailProcess();
                decimal? total = 0;
                foreach (var item in cart)
                {
                    var orderDetail = new ChiTietDDH();
                    orderDetail.MaSach = item.sach.MaSach;
                    orderDetail.MaDDH = result1;
                    orderDetail.SoLuong = item.Quantity;
                    orderDetail.DonGia = item.sach.GiaBan;
                    result2.Insert(orderDetail);

                    total = cart.Sum(x => x.Total);
                }
            }
            catch (Exception)
            {
                return Redirect("~/Cart/Error");
            }

            return Redirect("~/Cart/Success");
        }

        public ActionResult Success()
        {
            return View();
        }

    }
}