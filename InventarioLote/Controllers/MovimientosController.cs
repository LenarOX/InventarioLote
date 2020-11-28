using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventarioLote;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace InventarioLote.Controllers
{
    public class MovimientosController : Controller
    {
        private InventarioLotesEntities db = new InventarioLotesEntities();




        public ActionResult Cancelar(int id)
        {
            Movimientos movimiento = db.Movimientos.Where(m => m.MovimientoId == id).First();
            if (movimiento != null)
            {
                ProductoLote productoLote = db.ProductoLote.Find(movimiento.ProductoLoteId);
                
                db.Movimientos.Remove(movimiento);
                productoLote.Cantidad = productoLote.Cantidad + movimiento.cantidad;
                db.Entry(productoLote).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.ProductoId = new SelectList(db.Productos, "ProductoId", "Producto");
                return View("~/Views/Movimientos/ListaMovimientos.cshtml", db.Movimientos.ToList());
            }
            var movimientos = db.Movimientos.Include(m => m.ProductoLote);
            ViewBag.ProductoId = new SelectList(db.Productos, "ProductoId", "Producto");
            return View("~/Views/Movimientos/ListaMovimientos.cshtml", movimientos.ToList());
        }

        // GET: Movimientos
        public ActionResult ListaMovimientos()
        {
            var movimientos = db.Movimientos.Include(m => m.ProductoLote);
            ViewBag.ProductoId = new SelectList(db.Productos, "ProductoId", "Producto");
            return View(movimientos.ToList());
        }

        public ActionResult BuscarMovimientos(string ProductoId, DateTime? FechaInicial, DateTime? FechaFinal)
        {
            var movimientos = db.Movimientos.Include(m => m.ProductoLote).Where(m => m.ProductoLote.ProductoId == ProductoId).Where(m => m.Fecha >= FechaInicial).Where(m => m.Fecha <= FechaFinal);
            ViewBag.ProductoId = new SelectList(db.Productos, "ProductoId", "Producto");
            return View("~/Views/Movimientos/ListaMovimientos.cshtml", movimientos.ToList());
        }

        [HttpPost]
        public ActionResult GuardarMovimiento([Bind(Include = "MovimientoId,Fecha,ProductoLoteId,cantidad,Total")] Movimientos movimientos)
        {
            string mensaje = "";
            if (ModelState.IsValid)
            {
                try
                {
                    ProductoLote ProductoLotes = db.ProductoLote.Find(movimientos.ProductoLoteId);
                    movimientos.Fecha = DateTime.Now;
                    db.Movimientos.Add(movimientos);
                    //db.SaveChanges();
                    ProductoLotes.Cantidad = ProductoLotes.Cantidad - movimientos.cantidad;
                    db.Entry(ProductoLotes).State = EntityState.Modified;
                    db.SaveChanges();
                    mensaje = "guardado";
                    return Json(mensaje, JsonRequestBehavior.AllowGet); ;
                }
                catch (DbEntityValidationException dbEx)
                {
                    string[] ErrorMessages = new string[dbEx.EntityValidationErrors.Count() + 1];
                    var x = 0;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}",
                                validationError.PropertyName,
                                validationError.ErrorMessage);

                            ErrorMessages[x] = string.Format("Property: {0} Error: {1}$", validationError.PropertyName, validationError.ErrorMessage);
                            x++;
                        }
                    }
                    string errores = "";
                    foreach (var error in ErrorMessages)
                    {
                        errores += error;
                    }
                    TempData["ErrorMessages"] = errores;
                    return RedirectToAction("DBSaveError", "Messages");
                }

                
            }
            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }

        // GET: Movimientos
        public ActionResult Index()
        {
            var movimientos = db.Movimientos.Include(m => m.ProductoLote);
            return View(movimientos.ToList());
        }

        // GET: Movimientos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movimientos movimientos = db.Movimientos.Find(id);
            if (movimientos == null)
            {
                return HttpNotFound();
            }
            return View(movimientos);
        }

        // GET: Movimientos/Create
        public ActionResult Create()
        {
            ViewBag.ProductoLoteId = new SelectList(db.ProductoLote, "ProductoLoteId", "ProductoId");
            return View();
        }

        // POST: Movimientos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MovimientoId,Fecha,ProductoLoteId,cantidad,Total")] Movimientos movimientos)
        {
            if (ModelState.IsValid)
            {

                db.Movimientos.Add(movimientos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductoLoteId = new SelectList(db.ProductoLote, "ProductoLoteId", "ProductoId", movimientos.ProductoLoteId);
            return View(movimientos);
        }

        // GET: Movimientos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movimientos movimientos = db.Movimientos.Find(id);
            if (movimientos == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductoLoteId = new SelectList(db.ProductoLote, "ProductoLoteId", "ProductoId", movimientos.ProductoLoteId);
            return View(movimientos);
        }

        // POST: Movimientos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MovimientoId,Fecha,ProductoLoteId,cantidad,Total")] Movimientos movimientos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movimientos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductoLoteId = new SelectList(db.ProductoLote, "ProductoLoteId", "ProductoId", movimientos.ProductoLoteId);
            return View(movimientos);
        }

        // GET: Movimientos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movimientos movimientos = db.Movimientos.Find(id);
            if (movimientos == null)
            {
                return HttpNotFound();
            }
            return View(movimientos);
        }

        // POST: Movimientos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movimientos movimientos = db.Movimientos.Find(id);
            db.Movimientos.Remove(movimientos);
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
