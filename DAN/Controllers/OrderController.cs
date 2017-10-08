using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAN.Models;
using System.Data.Entity.Validation;

namespace DAN.Controllers
{
    public class OrderController : Controller
    {
        DANEntities db = new DANEntities();
        [HttpGet]
        public ActionResult Index()
        {
            var model = new OrderViewModel();
            if (Session["Cart"] == null)
                return RedirectToAction("Index", "Cart");
            if (Session["User"] != null)
            {
                model = (OrderViewModel)Session["User"];
                return View(model);
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Code != null)
                {
                    var code = db.Codes.SingleOrDefault(e => e.Code1 == model.Code);
                    if (code == null || code.Expired.Value)
                    {
                        ModelState.AddModelError("Code", "Mã khuyến mãi không hợp lệ hoặc đã được sử dụng!");
                        return View("Index");
                    }
                }
                Session["User"] = model;
                return View(model);
            }

            return View("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Done()
        {
            var user = (OrderViewModel)Session["User"];
            var cart = (List<Product>)Session["Cart"];
            try
            {
                var order = new Order()
                {
                    Prefix = "DAN-",
                    Fullname = user.fullName,
                    Address = user.adress,
                    Phone = user.phone.ToString(),
                    Email = user.email,
                    Code = user.Code ?? "Không sử dụng",
                    CreatOn = DateTime.Now,
                    TotalPrice = cart.Sum(e => e.Quantum * e.SalePrice),
                    PaidInfo = user.PaidInfo
                };
                db.Orders.Add(order);
                db.SaveChanges();
                foreach (var item in cart)
                {
                    db.OrderDetails.Add(new OrderDetail()
                    {
                        Pname = item.Pname,
                        OId = order.Id,
                        Quantum = item.Quantum,
                        Price = item.Quantum * item.SalePrice
                    });
                    db.SaveChanges();
                }
                Session.Remove("Cart");
                return View(order);
            }
            catch
            {
                ModelState.AddModelError("", "Có lỗi xảy ra, Vui lòng thử lại!");
                return View("Index");
            }
        }
        public ActionResult SearchInvoice()
        {
            return View("SearchInvoice");
        }
        [HttpPost]
        public ActionResult SearchInvoice(SearchInvoiceModel model)
        {
            if (ModelState.IsValid)
            {
                var order = db.Orders.SingleOrDefault(e => e.OId == model.OId);
                if (order == null)
                {
                    ModelState.AddModelError("", "Không tìm thấy đơn hàng nào!");
                    return View(model);
                }
                else
                    return RedirectToAction("Invoice", new { OId = model.OId });
            }
            return View(model);
        }
        public ActionResult Invoice(string OId)
        {
            var order = db.Orders.SingleOrDefault(e => e.OId == OId);
            if (order == null)
                return RedirectToAction("SearchInvoice");
            var orderDetail = db.OrderDetails.Where(e => e.OId == order.Id);
            var model = new InvoiceViewModel()
            {
                order = order,
                orderDetail = orderDetail
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Invoice(string key, string OId, string returnUrl)
        {
            switch (key)
            {
                case "status":
                    var order = db.Orders.SingleOrDefault(e => e.OId == OId);
                    order.Status = true;
                    db.SaveChanges();
                    break;
                case "paid":
                    var orders = db.Orders.SingleOrDefault(e => e.OId == OId);
                    orders.Paid = true;
                    db.SaveChanges();
                    break;
            }
            return Redirect(returnUrl);
        }
    }
}