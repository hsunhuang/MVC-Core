using Microsoft.AspNetCore.Mvc;
using prjMvcCoreDemo.Models;
using prjMvcCoreDemo.ViewModels;

namespace prjMvcCoreDemo.Controllers
{
    public class ProductController : Controller
    {
        IWebHostEnvironment _environment;
        public ProductController(IWebHostEnvironment p)
        {
            _environment = p;
        }



        //查詢
        public IActionResult List(CKeywordViewModel vm)
        {
            IEnumerable<TProduct> datas = null;
            dbDemoContext db = new dbDemoContext();
            if (string.IsNullOrEmpty(vm.txtKeyword))
                datas = from t in db.TProducts
                        select t;
            else
                datas = db.TProducts.Where(t => t.FName.Contains(vm.txtKeyword) );

            return View(datas);
        }
        //刪除
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                dbDemoContext db = new dbDemoContext();
                TProduct delCustomer = db.TProducts.FirstOrDefault(t => t.FId == id);
                if (delCustomer != null)
                {
                    db.TProducts.Remove(delCustomer);
                    db.SaveChangesAsync();
                }
            }
            return RedirectToAction("List");
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(TProduct p)
        {
            dbDemoContext db = new dbDemoContext(); //紅框
            db.TProducts.Add(p);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpPost]
        //存入資料庫
        public ActionResult Edit(CProductViewModel p)  //使用CProductViewModel
        {
            dbDemoContext db = new dbDemoContext();
            TProduct x = db.TProducts.FirstOrDefault(t => t.FId == p.FId);
            if (x != null)
            {
                if (p.photo != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    string path = _environment.WebRootPath + "/images/" + photoName;
                    x.FImagePath = photoName;
                    p.photo.CopyTo(new FileStream(path,FileMode.Create));
                }

                x.FName = p.FName;
                x.FQty = p.FQty;
                x.FCost = p.FCost;
                x.FPrice = p.FPrice;
                db.SaveChangesAsync();
            }
            return RedirectToAction("List");
        }

        //第一步找資料
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                dbDemoContext db = new dbDemoContext();
                TProduct x = db.TProducts.FirstOrDefault(t => t.FId == id);
                if (x != null)
                    return View(x);

            }
            return RedirectToAction("List"); //刪除後回傳給List
        }


    }
}
