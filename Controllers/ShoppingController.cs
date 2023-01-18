using Microsoft.AspNetCore.Mvc;
using prjMvcCoreDemo.Models;
using prjMvcCoreDemo.ViewModels;
using System.Text.Json;

namespace prjMvcCoreDemo.Controllers
{
    public class ShoppingController : Controller
    {

        public IActionResult CartView()
        {

            if (!HttpContext.Session.Keys.Contains(CDictionary.Sk_PURCHASED_PRODUCTS_LIST))
                return RedirectToAction("List");
            string json = HttpContext.Session.GetString(CDictionary.Sk_PURCHASED_PRODUCTS_LIST);
            List <CShoppingCartItem> cart = JsonSerializer.Deserialize<List<CShoppingCartItem>>(json);
            if (cart == null)

                return RedirectToAction("List");
            return View(cart);


        }

        public IActionResult List()
        {
            dbDemoContext db = new dbDemoContext();
            var datas = from t in db.TProducts
                        select t;
            return View(datas);

        }


        public IActionResult AddToCart(int? id)
        {

            ViewBag.fid = id;
            return View();

        }

        [HttpPost]
        public IActionResult AddToCart(CAddToCartViewModel vm)
        {
            dbDemoContext db = new dbDemoContext();
            TProduct p = db.TProducts.FirstOrDefault(t => t.FId == vm.txtFid);
            if (p == null)
                return RedirectToAction("List");

            List<CShoppingCartItem> cart = null;
            string json = "";
            if (HttpContext.Session.Keys.Contains(CDictionary.Sk_PURCHASED_PRODUCTS_LIST))
            {
                json = HttpContext.Session.GetString(CDictionary.Sk_PURCHASED_PRODUCTS_LIST);
                cart = JsonSerializer.Deserialize<List<CShoppingCartItem>>(json);
            }
               
            else
                cart=new List<CShoppingCartItem>();
            
            CShoppingCartItem item = new CShoppingCartItem();
            item.price = (decimal)p.FPrice;
            item.productId = vm.txtFid;
            item.count = vm.txtCount;
            item.product = p;
            cart.Add(item);
            json = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString(CDictionary.Sk_PURCHASED_PRODUCTS_LIST, json);
            return RedirectToAction("List");

        }



    }
}
