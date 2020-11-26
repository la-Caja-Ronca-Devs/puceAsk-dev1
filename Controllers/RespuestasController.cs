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


        public ActionResult RespuestasRealizadas() {
            var usuario = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
            var respuestas = (from p in db.Respuesta
                .Include(i => i.Usuario)
                .Include(i => i.Pregunta.Categoria)
                .Include(i => i.Pregunta.Usuario)
                              where p.UsuarioId == usuario.Id
                             select p).ToList();

            return View(respuestas);
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
            var pr = (from c in db.Pregunta.Include(i => i.Usuario) where c.PreguntaId == id select c).First();
            Respuesta respuesta = new Respuesta { Pregunta = pr };
            return View(respuesta);
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
                respuesta.UsuarioId = usuario.Id;
                respuesta.PreguntaId = id.PreguntaId;
                respuesta.FechaPublicacion = DateTime.Now;
                db.Respuesta.Add(respuesta);
                db.SaveChanges();
                return RedirectToAction("Details", "Preguntas", new { id = id.PreguntaId });
            }
            else
            {
                respuesta.DescRespuesta = "No pudo registrar su respuesta. Intente nuevamente.";
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
            var usuario = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
            Respuesta respuesta = (from r in db.Respuesta
                                   .Include(i => i.Usuario)
                                   .Include(i => i.Pregunta)
                                   where r.PreguntaId == id && r.UsuarioId == usuario.Id select r).First();
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
        public ActionResult Edit([Bind(Include = "PreguntaId,usuarioId,DescRespuesta")] Respuesta respuesta)
        {
            if (ModelState.IsValid)
            {
                var usuario = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                Respuesta pr = (from r in db.Respuesta where r.PreguntaId == respuesta.PreguntaId && r.UsuarioId == usuario.Id select r).First();
                pr.DescRespuesta = respuesta.DescRespuesta;
                pr.FechaPublicacion = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Details", "Preguntas", new { id = respuesta.PreguntaId });
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
