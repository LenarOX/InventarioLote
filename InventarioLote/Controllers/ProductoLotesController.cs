using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventarioLote;
using Newtonsoft.Json;

namespace InventarioLote.Controllers
{
    public class ProductoLotesController : Controller
    {
        private InventarioLotesEntities db = new InventarioLotesEntities();

        public ActionResult ListaLotes()
        {
            ViewBag.ProductoId = new SelectList(db.Productos, "ProductoId", "Producto");
            var productoLote = db.ProductoLote.Include(p => p.Productos);
            return View(productoLote.ToList());
        }

        public ActionResult BuscarLotes(string ProductoId)
        {
            if(ProductoId != "")
            {
                ViewBag.ProductoId = new SelectList(db.Productos, "ProductoId", "Producto");
                return View("~/Views/ProductoLotes/ListaLotes.cshtml", db.ProductoLote.Where(p => p.ProductoId == ProductoId));
            }

            ViewBag.ProductoId = new SelectList(db.Productos, "ProductoId", "Producto");
            var productoLote = db.ProductoLote.Include(p => p.Productos);
            return View(productoLote.ToList());
        }

        [HttpPost]
        public ActionResult GetCodigoLote(string ProductoId)
        {
            //var ProductoLote = db.ProductoLote.Where(p => p.ProductoId == ProductoId).ToList();
            var ProductoLote = db.ProductoLote.Where(p => p.ProductoId == ProductoId).Select(p => new {ProductoLoteId = p.ProductoLoteId, Costo = p.Costo, Cantidad = p.Cantidad}).ToList();
            /*var list = JsonConvert.SerializeObject(ProductoLote,
            Formatting.None,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });*/
            return this.Json(ProductoLote, JsonRequestBehavior.AllowGet);
            //return this.Json("hola", JsonRequestBehavior.AllowGet);
        }
        // GET: ProductoLotes
        public ActionResult Index()
        {
            var productoLote = db.ProductoLote.Include(p => p.Productos);
            return View(productoLote.ToList());
        }

        // GET: ProductoLotes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductoLote productoLote = db.ProductoLote.Find(id);
            if (productoLote == null)
            {
                return HttpNotFound();
            }
            return View(productoLote);
        }

        // GET: ProductoLotes/Create
        public ActionResult Create()
        {
            ViewBag.ProductoId = new SelectList(db.Productos, "ProductoId", "Producto");
            return View();
        }

        // POST: ProductoLotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductoLoteId,ProductoId,Cantidad,Costo,Fecha")] ProductoLote productoLote)
        {
            if (ModelState.IsValid)
            {
                db.ProductoLote.Add(productoLote);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductoId = new SelectList(db.Productos, "ProductoId", "Producto", productoLote.ProductoId);
            return View(productoLote);
        }

        // GET: ProductoLotes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductoLote productoLote = db.ProductoLote.Find(id);
            if (productoLote == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductoId = new SelectList(db.Productos, "ProductoId", "Producto", productoLote.ProductoId);
            return View(productoLote);
        }

        // POST: ProductoLotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductoLoteId,ProductoId,Cantidad,Costo,Fecha")] ProductoLote productoLote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productoLote).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductoId = new SelectList(db.Productos, "ProductoId", "Producto", productoLote.ProductoId);
            return View(productoLote);
        }

        // GET: ProductoLotes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductoLote productoLote = db.ProductoLote.Find(id);
            if (productoLote == null)
            {
                return HttpNotFound();
            }
            return View(productoLote);
        }

        // POST: ProductoLotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductoLote productoLote = db.ProductoLote.Find(id);
            db.ProductoLote.Remove(productoLote);
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
