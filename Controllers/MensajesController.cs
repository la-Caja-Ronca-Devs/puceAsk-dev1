using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using puceAsk_dev1.Models;

namespace puceAsk_dev1.Controllers
{
    public class MensajesController : InfoBaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Mensajes
        public ActionResult Index()
        {
            var mensajes = db.Mensajes.Include(m => m.Emisor).Include(m => m.Receptor);
            return View(mensajes.ToList());
        }

        [Authorize(Roles = "user")]
        public ActionResult MensajesRecibidos()
        {
            var usuario = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
            var mensajes = from m in db.Mensajes
                           .Include(m => m.Emisor)
                           .Include(m => m.Receptor)
                           where m.Receptor.Id == usuario.Id select m;
            mensajes = mensajes.OrderByDescending(s => s.FechaMensaje);
            return View(mensajes.ToList());
        }

        // GET: Mensajes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mensaje mensaje = db.Mensajes.Find(id);
            if (mensaje == null)
            {
                return HttpNotFound();
            }
            return View(mensaje);
        }

        // GET: Mensajes/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.Receptores = (from r in db.Users
                                  select r).ToList();
            ViewBag.ReceptorId = new SelectList(db.Users, "Id", "UserName");
            return View();
        }

        // POST: Mensajes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create([Bind(Include = "MensajeId,ReceptorId,EmisorId,MensajeDesc,FechaMensaje")] Mensaje mensaje)
        {
            var usuario = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
            mensaje.EmisorId = usuario.Id;
            mensaje.FechaMensaje = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Mensajes.Add(mensaje);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.EmisorId = new SelectList(db.Cuentas, "CuentaId", "CuentaId", mensaje.EmisorId);
            //ViewBag.ReceptorId = new SelectList(db.Cuentas, "CuentaId", "CuentaId", mensaje.ReceptorId);
            return View(mensaje);
        }

        // GET: Mensajes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mensaje mensaje = db.Mensajes.Find(id);
            if (mensaje == null)
            {
                return HttpNotFound();
            }
            //ViewBag.EmisorId = new SelectList(db.Cuentas, "CuentaId", "CuentaId", mensaje.EmisorId);
            //ViewBag.ReceptorId = new SelectList(db.Cuentas, "CuentaId", "CuentaId", mensaje.ReceptorId);
            return View(mensaje);
        }

        // POST: Mensajes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MensajeId,ReceptorId,EmisorId,MensajeDesc,FechaMensaje")] Mensaje mensaje)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mensaje).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.ReceptorId = new SelectList(db.Cuentas, "CuentaId", "CuentaId", mensaje.ReceptorId);
            return View(mensaje);
        }

        // GET: Mensajes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mensaje mensaje = db.Mensajes.Find(id);
            if (mensaje == null)
            {
                return HttpNotFound();
            }
            return View(mensaje);
        }

        // POST: Mensajes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mensaje mensaje = db.Mensajes.Find(id);
            db.Mensajes.Remove(mensaje);
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
