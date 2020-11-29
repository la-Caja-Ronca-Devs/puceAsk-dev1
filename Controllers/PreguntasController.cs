using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
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
        public ActionResult Index(int pagina =1)
        {
            var cantidadRegistrosPorPagina = 10;
            var preguntas = (from p in db.Pregunta select p)
                .OrderByDescending(s => s.PreguntaId)
                .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                .Take(cantidadRegistrosPorPagina);
            var contar = (from p in db.Pregunta select p).Count();
            var totalRegistros = contar;
            var totalpaginas = (int)Math.Ceiling((double)totalRegistros / cantidadRegistrosPorPagina);
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalRegistros = totalRegistros;
            ViewBag.TotalPaginas = totalpaginas;
            ViewBag.RegistrosPorPagina = cantidadRegistrosPorPagina;

            return View(preguntas);
        }

        [Authorize(Roles = "admin,user")]
        public ActionResult PreguntasManager(int? id, int? respuestaID)
        {
            var viewModel = new PreguntasManager();
            viewModel.preguntas = db.Pregunta
                .Include(i => i.Categoria);
            return View(viewModel);
        }

        [AllowAnonymous]
        public ActionResult Inicio(string categoria, string buscar, string ordenar, int pagina =1)
        {
            ViewBag.NameSortParam = String.IsNullOrEmpty(ordenar);

            var viewModel = new PreguntasManager();
            var pregBase = (from c in db.Pregunta
                                  .Include(i => i.Categoria)
                                  .Include(i => i.Usuario)
                                  .Include(i => i.Respuestas)
                            select c).ToList();

            if (categoria != null )//Si escoge categoría
            {
                var cantidadRegistrosPorPagina = 10;
                var totalRegistros = db.Pregunta.Count();
                var NombreCategoria = categoria;           
                var registros = (from c in pregBase                                   
                                 where c.Categoria.NombreCategoria == categoria
                                 select c).ToList();
                var registrosCategoria = registros.Count();
                viewModel.RegistrosCategoria = registrosCategoria;
                if (registrosCategoria > cantidadRegistrosPorPagina)//mas de los registros para una página
                {
                    viewModel.preguntas = registros.OrderBy(x => x.Fechapregunta)
                                     .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                                     .Take(cantidadRegistrosPorPagina);
                    
                    var totalpaginas = (int)Math.Ceiling((double)totalRegistros / cantidadRegistrosPorPagina);
                    viewModel.PaginaActual = pagina;
                    viewModel.TotalRegistros = totalRegistros;
                    viewModel.TotalPaginas = totalpaginas;
                    viewModel.RegistrosPorPagina = cantidadRegistrosPorPagina;
                }
                else//Menos registros por pagina
                {
                    viewModel.preguntas = registros;

                }
                ViewData["categoria"] = categoria;
            }
            else//Inicio todas las categorias
            {
                viewModel.preguntas = pregBase.OrderBy(x => Guid.NewGuid()).Take(20);
                ViewData["categoria"] = "Todas";                
            }
            viewModel.categorias = db.Categoria.ToList();
            //ViewBag.categorias = (from c in db.Categoria select c).ToList();

            switch (ordenar)
            {
                case "categoria":
                    viewModel.preguntas = viewModel.preguntas.OrderBy(s => s.Categoria.NombreCategoria);
                    break;
                case "antiguos":
                    viewModel.preguntas = viewModel.preguntas.OrderBy(s => s.Fechapregunta);
                    break;
                case "Date":
                    viewModel.preguntas = viewModel.preguntas.OrderByDescending(s => s.Fechapregunta);
                    break;
                case "titulo":
                    viewModel.preguntas = viewModel.preguntas.OrderBy(s => s.TituloPregunta);
                    break;
                default:
                    viewModel.preguntas = viewModel.preguntas;

                    break;
            }

            using (db)
            {

                if (!string.IsNullOrEmpty(buscar) )
                {
                    if(categoria != null)
                    foreach (var item in buscar.Split(new char[] { ' ' },
                             StringSplitOptions.RemoveEmptyEntries))
                    {
                        viewModel.preguntas = viewModel.preguntas.Where(x => x.Categoria.NombreCategoria.ToString() == categoria && x.TituloPregunta.ToLower().Contains(item.ToLower()) ||
                                                      x.DescPregunta.ToLower().Contains(item.ToLower())).ToList();

                    }
                    else //if (!string.IsNullOrEmpty(buscar) )
                    {
                        foreach (var item in buscar.Split(new char[] { ' ' },
                                 StringSplitOptions.RemoveEmptyEntries))
                        {
                            viewModel.preguntas = viewModel.preguntas.Where(x => x.TituloPregunta.ToLower().Contains(item.ToLower()) ||
                                                          x.DescPregunta.ToLower().Contains(item.ToLower()) ||
                                                          x.Categoria.NombreCategoria.ToLower().Contains(item.ToLower()))
                                                          .ToList();

                        }
                    }
                }
               
                return View(viewModel);
            }

        }
        // GET: Preguntas/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id, int pagina = 1)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var pregunta = (from p in db.Pregunta
                .Include(i => i.Categoria)
                .Include(i => i.Respuestas.Select(c => c.Usuario))
                .Include(i => i.Usuario)
                            where p.PreguntaId == id
                            select p).First();

            if (pregunta.MejorUsuarioRespuestaId != null)
            {
                var mejorRespuestas = (from p in pregunta.Respuestas
                                       where p.UsuarioId == pregunta.MejorUsuarioRespuestaId
                                       select p).First();

                var respuestas = (from p in pregunta.Respuestas
                                  where p.UsuarioId != pregunta.MejorUsuarioRespuestaId
                                  orderby p.FechaPublicacion
                                  select p).ToList();
                respuestas.Insert(0, mejorRespuestas);
                pregunta.Respuestas = respuestas;
            }
            else {
                pregunta.Respuestas = pregunta.Respuestas.OrderBy(m => m.FechaPublicacion).ToList();
            }

            TempData["idPregunta"] = pregunta;
            if (User.Identity.IsAuthenticated) { 
                //Control de acceso a responder
                ViewData["Rusuario"] = true;
                var usuario = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);            
                var respuestas1 = (from r in pregunta.Respuestas
                                   where r.UsuarioId == usuario.Id
                                   select r).ToList();
                if (respuestas1.Count() > 0)
                    //No tiene boton de responder
                    ViewData["Rusuario"] = false;
            }

            var cantidadRegistrosPorPagina = 30;
             var totalRegistros = (from p in pregunta.Respuestas
                                  select p).Count();

            

            var totalpaginas = (int)Math.Ceiling((double)totalRegistros / cantidadRegistrosPorPagina);
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalRegistros = totalRegistros;
            ViewBag.TotalPaginas = totalpaginas;
            ViewBag.RegistrosPorPagina = cantidadRegistrosPorPagina;         
            return View(pregunta);
        }

        // GET: Preguntas/Create
        [Authorize(Roles = "user")]
        public ActionResult Create()
        {
            ViewBag.categorias = (from c in db.Categoria select c).ToList();
            return View();
        }

        // POST: Preguntas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "user")]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include = "TituloPregunta,DescPregunta,CategoriaId")] Pregunta pregunta)
        {
            if (ModelState.IsValid)
            {            
                var usuario = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                var cuenta = (from c in db.Users where c.Id == usuario.Id select c).First();
                pregunta.UsuarioId = cuenta.Id;
                pregunta.Fechapregunta = DateTime.Now;

                db.Pregunta.Add(pregunta);
                db.SaveChanges();
                int preg = db.Pregunta.Max(item => item.PreguntaId);
                return RedirectToAction("Details","Preguntas",new { id = preg});
            }
            pregunta = pregunta = (from p in db.Pregunta
                .Include(i => i.Categoria)
                .Include(i => i.Respuestas.Select(c => c.Usuario))
                .Include(i => i.Usuario)
                where p.PreguntaId == pregunta.PreguntaId
                select p).First();
            return View(pregunta);
        }


        // GET: Preguntas/Edit/5
        [Authorize(Roles = "user")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pregunta = (from p in db.Pregunta
               .Include(i => i.Categoria)
               .Include(i => i.Respuestas.Select(c => c.Usuario))
               .Include(i => i.Usuario)
                            where p.PreguntaId == id
                            orderby p.Fechapregunta
                            select p).First();
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
        [Authorize(Roles = "user")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PreguntaId,TituloPregunta,DescPregunta,MejorUsuarioRespuestaId")] Pregunta pregunta)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Pregunta pr = db.Pregunta.Find(pregunta.PreguntaId);
                    pr.DescPregunta = pregunta.DescPregunta;
                    pr.MejorUsuarioRespuestaId = pregunta.MejorUsuarioRespuestaId;
                    //db.Entry(pregunta).State = EntityState.Modified;
                    db.SaveChanges();

                }
            }
            catch
            {
                ModelState.AddModelError("", "No se pudo actualizar, intentalo de nuevo.");
            }
            return RedirectToAction("Details", "Preguntas", new { id = pregunta.PreguntaId }); ;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult EditAdmin(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pregunta = (from p in db.Pregunta
               .Include(i => i.Categoria)
               .Include(i => i.Respuestas.Select(c => c.Usuario))
               .Include(i => i.Usuario)
                            where p.PreguntaId == id
                            orderby p.Fechapregunta
                            select p).First();
            if (pregunta == null)
            {
                return HttpNotFound();
            }
            ViewBag.categorias = (from c in db.Categoria select c).ToList();
            return View(pregunta);

        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult EditAdmin([Bind(Include = "PreguntaId,TituloPregunta,DescPregunta,CategoriaId")] Pregunta pregunta)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Pregunta pr = db.Pregunta.Find(pregunta.PreguntaId);
                    pr.TituloPregunta = pregunta.TituloPregunta;
                    pr.DescPregunta = pregunta.DescPregunta;
                    pr.CategoriaId = pregunta.CategoriaId;
                    //db.Entry(pregunta).State = EntityState.Modified;
                    db.SaveChanges();

                }
            }
            catch
            {
                ModelState.AddModelError("", "No se pudo actualizar, intentalo de nuevo.");
            }
            return RedirectToAction("Details", "Preguntas", new { id = pregunta.PreguntaId }); ;
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
        
        [Authorize(Roles = "user")]
        public ActionResult PreguntasRealizadas(int pagina =1, int filtro = 0)
        {
            var cantidadRegistrosPorPagina = 4;
            var usuario = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
           
            var preguntas = (from p in db.Pregunta
                .Include(i => i.Categoria)
                .Include(i => i.Respuestas.Select(c => c.Usuario))
                .Include(i => i.Usuario)
                             where p.UsuarioId == usuario.Id && (filtro == 1?(p.MejorUsuarioRespuestaId != null):(filtro == 2 ? (p.MejorUsuarioRespuestaId == null) :true))
                             select p);
            preguntas = preguntas.OrderByDescending(s => s.PreguntaId)
                .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                .Take(cantidadRegistrosPorPagina);
            var contar = (from p in preguntas
                          where p.UsuarioId == usuario.Id
                          select p).Count();
            var totalRegistros = contar;
            var totalpaginas = (int)Math.Ceiling((double)totalRegistros / cantidadRegistrosPorPagina);
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalRegistros = totalRegistros;
            ViewBag.TotalPaginas = totalpaginas;
            ViewBag.RegistrosPorPagina = cantidadRegistrosPorPagina;
           
            return View(preguntas);
        }

    }
}
