using EfeErsozlu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EfeErsozlu.Controllers
{
    public class ApiController : Controller
    {
        private ETicaretDbEntities db = new ETicaretDbEntities();
        // GET: Api
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult Category(ApiRegistration apiModel)
        {
            ApiRegistration api = db.ApiRegistration.Where(x => x.ApiKey == apiModel.ApiKey && x.TcNo == apiModel.TcNo).FirstOrDefault();
            var categoryList = db.Category.Select(x=>x.CategoryName).ToList();

            if (api != null)
            {
                if(categoryList != null)
                {
                    return Json(new { sonuc = "1", categories = categoryList });
                }
                else
                {
                    return Json(new { sonuc = "-2", mesaj = "Hiçbir kategori bulunmamaktadır." });
                }
                
            }
            else
            {
                return Json(new { sonuc = "-1", mesaj = "Yanlış Api-key veya TC no" });
            }
        }

        [HttpPost]
        public JsonResult Product(ApiRegistration apiModel)
        {
            ApiRegistration api = db.ApiRegistration.Where(x => x.ApiKey == apiModel.ApiKey && x.TcNo == apiModel.TcNo).FirstOrDefault();
            var ProductList = db.Product.Select(x=> new {x.ProductName, x.ProdunctStock, x.ProductPrice }).ToList();

            if (api != null)
            {
                if (ProductList != null)
                {
                    return Json(new { sonuc = "1", products = ProductList });
                }
                else
                {
                    return Json(new { sonuc = "-2", mesaj = "Hiçbir ürün bulunmamaktadır." });
                }

            }
            else
            {
                return Json(new { sonuc = "-1", mesaj = "Yanlış Api-key veya TC no" });
            }
        }

        [HttpPost]
        public JsonResult Basket(ApiRegistration apiModel)
        {
            ApiRegistration api = db.ApiRegistration.Where(x => x.ApiKey == apiModel.ApiKey && x.TcNo == apiModel.TcNo).FirstOrDefault();
            var BasketList = db.Basket.Select(x => new { x.BasketId, x.BarketDate, x.BasketTotalPrice }).ToList();

            if (api != null)
            {
                if (BasketList != null)
                {
                    return Json(new { sonuc = "1", baskets = BasketList });
                }
                else
                {
                    return Json(new { sonuc = "-2", mesaj = "Sepette Hiçbir ürün bulunmamaktadır." });
                }

            }
            else
            {
                return Json(new { sonuc = "-1", mesaj = "Yanlış Api-key veya TC no" });
            }
        }

    }
}