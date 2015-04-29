using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GroceryList.Models;

namespace GroceryList.Controllers
{
    public class ListDetailsController : Controller
    {
        private GroceryEntities db = new GroceryEntities();

        // GET: ListDetails
        public ActionResult Index()
        {
            var listDetails = db.ListDetails.Include(l => l.List).Include(l => l.Product).Include(l => l.Store);
            return View(listDetails.ToList());
        }

        // GET: ListDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListDetail listDetail = db.ListDetails.Find(id);
            if (listDetail == null)
            {
                return HttpNotFound();
            }
            return View(listDetail);
        }

        public JsonResult GetProduct(string term)
        {
            var result =
                from p in db.Products
                where p.ProductName.Contains(term)
                select new { ProductId= p.ProductId, ProductName = p.ProductName.ToString() };
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProductList()
        {
            var list = db.Products.ToList();
            return View(list);
        }

        // GET: ListDetails/Create
        public ActionResult Create()

        {
            ViewBag.GroceryList = db.ListDetails.ToList();
            ViewBag.ListId = new SelectList(db.Lists, "ListId", "ListId");
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName");
            ViewBag.StoreId = new SelectList(db.Stores, "StoreId", "StoreName");
            return View();
        }

        // POST: ListDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ListId,ProductId,StoreId,ListDetailsId,StoreNameTwo")] ListDetail listDetail)
        {
            if (ModelState.IsValid)
            {
                db.ListDetails.Add(listDetail);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            ViewBag.ListId = new SelectList(db.Lists, "ListId", "ListId", listDetail.ListId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", listDetail.ProductId);
            ViewBag.StoreId = new SelectList(db.Stores, "StoreId", "StoreName", listDetail.StoreId);
            return View(listDetail);
        }

        // GET: ListDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListDetail listDetail = db.ListDetails.Find(id);
            if (listDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListId = new SelectList(db.Lists, "ListId", "ListId", listDetail.ListId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", listDetail.ProductId);
            ViewBag.StoreId = new SelectList(db.Stores, "StoreId", "StoreName", listDetail.StoreId);
            return View(listDetail);
        }

        // POST: ListDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ListId,ProductId,StoreId,ListDetailsId")] ListDetail listDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(listDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create");
            }
            ViewBag.ListId = new SelectList(db.Lists, "ListId", "ListId", listDetail.ListId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", listDetail.ProductId);
            ViewBag.StoreId = new SelectList(db.Stores, "StoreId", "StoreName", listDetail.StoreId);
            return View(listDetail);
        }

        // GET: ListDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListDetail listDetail = db.ListDetails.Find(id);
            if (listDetail == null)
            {
                return HttpNotFound();
            }
            return View(listDetail);
        }

        // POST: ListDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ListDetail listDetail = db.ListDetails.Find(id);
            db.ListDetails.Remove(listDetail);
            db.SaveChanges();
            return RedirectToAction("Create");
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
