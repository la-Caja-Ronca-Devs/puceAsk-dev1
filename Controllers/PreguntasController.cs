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
    public class PreguntasController : InfoBaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Preguntas
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View(db.Pregunta.ToList());
        }

        [Authorize(Roles = "admin,user")]
        public ActionResult PreguntasManager(int? id, int? respuestaID)
        {
            var viewModel = new PreguntasManager();
            viewModel.preguntas = db.Pregunta
                .Include(i => i.Categoria);
            return View(viewModel);
        }

        public ActionResult Inicio(string categoria, int pagina =1)
        {
            var viewModel = new PreguntasManager();
            if (categoria != null)
            {
                var cantidadRegistrosPorPagina = 2;
                var consulta = db.Pregunta.OrderBy(x => x.Fechapregunta)
                    .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                    .Take(cantidadRegistrosPorPagina).ToList();
                var totalRegistros = db.Pregunta.Count();
                viewModel.preguntas = consulta;
                viewModel.PaginaActual = pagina;
                viewModel.TotalRegistro = totalRegistros;
                viewModel.RegistroPorPagina = cantidadRegistrosPorPagina;

                var NombreCategoria = categoria;
                
                viewModel.preguntas = (from c in db.Pregunta
                                .Include(i => i.Categoria)
                                .Include(i => i.Respuestas.Select(c => c.Cuenta))
                                .Include(i => i.Cuenta.Usuario)
                                       where c.Categoria.NombreCategoria == categoria
                                       select c).Include("?"+pagina);
                
                
                
                ViewData["categoria"] = categoria;
            }
            else
            {
                viewModel.preguntas = db.Pregunta
                    .Include(i => i.Categoria)
                    .Include(i => i.Respuestas.Select(c => c.Cuenta))
                    .Include(i => i.Cuenta.Usuario);
                ViewData["categoria"] = "Todas";
            }
            viewModel.categorias = db.Categoria;
            return View(viewModel);
        }

        

        // GET: Preguntas/Details/5
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

                       
            var pregunta = (from p in db.Pregunta
                .Include(i => i.Categoria)
                .Include(i => i.Respuestas.Select(c => c.Cuenta))
                .Include(i=> i.Cuenta)
                where p.PreguntaId == id 
                select p).First();

            return View(pregunta);
        }

        // GET: Preguntas/Create
        [Authorize(Roles = "user")]
        public ActionResult Create()
        {
            return View(new PreguntasManager());
        }

        // POST: Preguntas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "user")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PreguntaId,RowVersion,TituloPregunta,DescPregunta,Fechapregunta, CategoriaId")] Pregunta pregunta)
        {
            if (ModelState.IsValid)
            {
                var usuario = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                var cuenta = (from c in db.Cuentas where c.Usuario.Id == usuario.Id select c).First();
                pregunta.CuentaId = cuenta.CuentaId;
                db.Pregunta.Add(pregunta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pregunta);
        }

        // GET: Preguntas/Edit/5
        [Authorize(Roles = "user, admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pregunta pregunta = db.Pregunta.Find(id);
            if (pregunta == null)
            {
                return HttpNotFound();
            }
            return View(pregunta);
        }

        // POST: Preguntas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "user, admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PreguntaId,RowVersion,TituloPregunta,DescPregunta,Fechapregunta")] Pregunta pregunta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pregunta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pregunta);
        }

        // GET: Preguntas/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pregunta pregunta = db.Pregunta.Find(id);
            if (pregunta == null)
            {
                return HttpNotFound();
            }
            return View(pregunta);
        }

        // POST: Preguntas/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pregunta pregunta = db.Pregunta.Find(id);
            db.Pregunta.Remove(pregunta);
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
