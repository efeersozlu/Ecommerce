using EfeErsozlu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace EfeErsozlu.Controllers
{
    public class HomeController : Controller
    {
        private ETicaretDbEntities db = new ETicaretDbEntities();


        public ActionResult Index(string error)
        {
            if (error != null)
            {

                ViewBag.Err = "Sepetinizde Ürün Bulunmamaktadır.";
            }
            var query = db.Category.Select(c => new { c.CategoryId, c.CategoryName });
            ViewBag.Categories = new SelectList(query.AsEnumerable(), "CategoryId", "CategoryName");

            return View(db.Product.ToList());
        }


        [HttpPost]
        public ActionResult Index(int? categoryid)
        {
            var query = db.Category.Select(c => new { c.CategoryId, c.CategoryName });
            ViewBag.Categories = new SelectList(query.AsEnumerable(), "CategoryId", "CategoryName");
            return View(db.Product.Where(c => c.ProductCategoryId == categoryid).ToList());
        }
        public ActionResult ProductDetails(int? id)
        {
            int total = 0;
            List<Basket> totalB = (from tb in db.Basket where tb.BasketProductId == id && tb.BasketOkay == true select tb).ToList();
            foreach (Basket b in totalB)
            {
                total = total + b.BasketProductNumber;
            }
            ViewBag.total = total;
            Product product = (from p in db.Product where p.ProductId == id select p).FirstOrDefault();
            var ProName = (from p in db.Product where p.ProductId == id select p.ProductName).FirstOrDefault();
            ViewBag.pn = ProName;
            if (product.ProdunctStock <= 0)
            {
                ViewBag.Err = "Stokta Ürün Bulunmuyor.";
            }
            return View(product);

        }
        public ActionResult AddBasket(int? id)
        {

            var product = (from pn in db.Product where pn.ProductId == id select pn.ProductName).FirstOrDefault();
            ViewBag.ProName = product.ToString();
            ViewBag.getDate = DateTime.UtcNow;
            ViewBag.ProId = id;
            return View();
        }
        [HttpPost]
        public ActionResult AddBasket(Basket basket, int id)
        {

            decimal proprice = (from pp in db.Product where pp.ProductId == id select pp.ProductPrice).FirstOrDefault();
            basket.BasketOkay = false;
            basket.BasketProductId = id;
            basket.BarketDate = DateTime.Today;
            int totalpronumber = basket.BasketProductNumber;
            decimal total = proprice * totalpronumber;
            basket.BasketTotalPrice = total;
            var sec = (from t in db.Product where t.ProductId == id select t.ProdunctStock).FirstOrDefault();
            if (totalpronumber > sec)
            {
                return RedirectToAction("AddBasket", new { error = "Stok Aşımı" });
            }
            else if (ModelState.IsValid)
            {

                db.Basket.Add(basket);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Cart()
        {
            List<Basket> basketlist = (from c in db.Basket where c.BasketOkay == false select c).ToList();
            if (basketlist.Count == 0)
            {
                return RedirectToAction("Index", new { error = "Sepetiniz Boş" });
            }

            return View(basketlist);
        }
        public ActionResult BuyProducts()
        {
            List<Basket> basketlist = (from b in db.Basket where b.BasketOkay == false select b).ToList();
            if (basketlist.Count == 0)
            {
                return RedirectToAction("Index", new { error = "Sepetiniz Boş" });
            }
            else
            {
                foreach (Basket b in basketlist)
                {
                    Product product = (from p in db.Product where p.ProductId == b.BasketProductId select p).FirstOrDefault();
                    product.ProdunctStock = product.ProdunctStock - b.BasketProductNumber;
                    b.BasketOkay = true;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult DeleteProduct(int id)
        {
            Basket sil = (from q in db.Basket where q.BasketProductId == id select q).FirstOrDefault();
            db.Basket.Remove(sil);
            db.SaveChanges();
            return RedirectToAction("Cart");
        }
    }
}