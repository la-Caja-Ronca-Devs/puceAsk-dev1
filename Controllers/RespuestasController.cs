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
    public class RespuestasController : InfoBaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();       
        // GET: Respuestas
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View(db.Respuesta.ToList());
        }

        // GET: Respuestas/Details/5
        [Authorize(Roles = "admin,user")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Respuesta respuesta = db.Respuesta.Find(id);            
            if (respuesta == null)
            {
                return HttpNotFound();
            }
            return View(respuesta);
        }

        // GET: Respuestas/Create
        [Authorize(Roles = "user")]
        public ActionResult Create(int id)
        {
            return View();
        }

        // POST: Respuestas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize(Roles = "user")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DescRespuesta")] Respuesta respuesta)
        {           
            if (ModelState.IsValid)
            {
                Pregunta id = (Pregunta)TempData["idPregunta"];
                var usuario = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                var cuenta = (from c in db.Cuentas where c.Usuario.Id == usuario.Id select c).First();
                respuesta.CuentaId = cuenta.CuentaId;
                respuesta.PreguntaId = id.PreguntaId;
                respuesta.FechaPublicacion = DateTime.Now;
                db.Respuesta.Add(respuesta);
                db.SaveChanges();
                return RedirectToAction("Details", "Preguntas", new { id = id.PreguntaId });
            }
            else
            {
                respuesta.DescRespuesta = "No se ingreso";
                return View(respuesta);
            }
            
        }

        // GET: Respuestas/Edit/5
        [Authorize(Roles = "user, admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Respuesta respuesta = db.Respuesta.Find(id);
            if (respuesta == null)
            {
                return HttpNotFound();
            }
            return View(respuesta);
        }

        // POST: Respuestas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "user, admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RespuestaId,RowVersion,FechaPublicacion,DescRespuesta")] Respuesta respuesta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(respuesta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(respuesta);
        }

        // GET: Respuestas/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Respuesta respuesta = db.Respuesta.Find(id);
            if (respuesta == null)
            {
                return HttpNotFound();
            }
            return View(respuesta);
        }

        // POST: Respuestas/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "user")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Respuesta respuesta = db.Respuesta.Find(id);
            db.Respuesta.Remove(respuesta);
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
