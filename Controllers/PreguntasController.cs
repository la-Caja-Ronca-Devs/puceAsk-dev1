using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

        public ActionResult Inicio(string categoria, string buscar, string ordenar, int pagina =1)
        {
            ViewBag.NameSortParam = String.IsNullOrEmpty(ordenar);
           
            var viewModel = new PreguntasManager();
          
            if (categoria != null )//Si escoge categoría
            {
                var cantidadRegistrosPorPagina = 2;//Cambiar a 20
                var totalRegistros = db.Pregunta.Count();
                var NombreCategoria = categoria;
                var registrosCategoria = (from c in db.Pregunta
                                    .Include(i => i.Categoria)
                                    .Include(i => i.Respuestas.Select(c => c.Usuario))
                                    .Include(i => i.Usuario)
                                          where c.Categoria.NombreCategoria == categoria
                                          select c).Count();
                viewModel.RegistrosCategoria = registrosCategoria;
                if (registrosCategoria > cantidadRegistrosPorPagina)//mas de los registros para una página
                {
                    viewModel.preguntas = (from c in db.Pregunta
                                    .Include(i => i.Categoria)
                                    .Include(i => i.Respuestas.Select(c => c.Usuario))
                                    .Include(i => i.Usuario)
                                           where c.Categoria.NombreCategoria == categoria
                                           select c).OrderBy(x => x.Fechapregunta)
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
                    viewModel.preguntas = (from c in db.Pregunta
                                    .Include(i => i.Categoria)
                                    .Include(i => i.Respuestas.Select(c => c.Usuario))
                                    .Include(i => i.Usuario)
                                           where c.Categoria.NombreCategoria == categoria
                                           select c);
                    
                }
                ViewData["categoria"] = categoria;
            }
            else//Inicio todas las categorias
            {

                /*if ( buscar==null)///caundo busca
                {
                        viewModel.preguntas = db.Pregunta.OrderBy(x => Guid.NewGuid())
                            .Include(i => i.Categoria)
                            .Include(i => i.Respuestas.Select(c => c.Usuario))
                            .Include(i => i.Usuario).Take(3);//Cambiar a 20
                    
                }
               
                if (buscar != null)
                {
                    viewModel.preguntas = db.Pregunta
                    .Include(i => i.Categoria)
                    .Include(i => i.Respuestas.Select(c => c.Usuario))
                    .Include(i => i.Usuario);//Cambiar a 20
                    
                
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

                    if (!string.IsNullOrEmpty(buscar))
                    {
                        foreach (var item in buscar.Split(new char[] { ' ' },
                                     StringSplitOptions.RemoveEmptyEntries))
                        {
                            viewModel.preguntas = viewModel.preguntas.Where(x => x.TituloPregunta.ToLower().Contains(item.ToLower()) ||
                                                x.DescPregunta.ToLower().Contains(item.ToLower()) ||
                                                x.Categoria.NombreCategoria.ToLower().Contains(item.ToLower()))
                                                .ToList();

                            //viewModel.preguntas = from p in db.Pregunta where p.TituloPregunta == like%buscar 
                        }
                    }
                    return View(viewModel);

                }
                ViewData["categoria"] = "Todas";
                viewModel.categorias = db.Categoria;
                ViewBag.categorias = (from c in db.Categoria select c).ToList();
            }
            return View(viewModel);
        }
        */
                viewModel.preguntas = db.Pregunta.OrderBy(x => Guid.NewGuid())
                                    .Include(i => i.Categoria)
                                    .Include(i => i.Respuestas.Select(c => c.Usuario))
                                    .Include(i => i.Usuario).Take(3);//Cambiar a 20
                ViewData["categoria"] = "Todas";
                /*if (buscar != null)///caundo busca
                {

                }*/
            }
            viewModel.categorias = db.Categoria;
            ViewBag.categorias = (from c in db.Categoria select c).ToList();

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

            using (db = new ApplicationDbContext())
            {

                if (!string.IsNullOrEmpty(buscar))
                {
                    foreach (var item in buscar.Split(new char[] { ' ' },
                             StringSplitOptions.RemoveEmptyEntries))
                    {
                        viewModel.preguntas = viewModel.preguntas.Where(x => x.TituloPregunta.ToLower().Contains(item.ToLower()) ||
                                                      x.DescPregunta.ToLower().Contains(item.ToLower()) ||
                                                      x.Categoria.NombreCategoria.ToLower().Contains(item.ToLower()))
                                                      .ToList();

                        //viewModel.preguntas = from p in db.Pregunta where p.TituloPregunta == like%buscar 
                    }
                }
                return View(viewModel);
            }

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
                .Include(i => i.Respuestas.Select(c => c.Usuario))
                .Include(i=> i.Usuario)

                where p.PreguntaId == id 
                select p).First();

            TempData["idPregunta"] = pregunta;
            return View(pregunta);
        }

        // GET: Preguntas/Create
        [Authorize(Roles = "user")]
        public ActionResult Create()
        {
            ViewBag.categorias = (from c in db.Categoria select c).ToList();
            return View(new PreguntasManager());
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
            var pregunta = (from p in db.Pregunta
               .Include(i => i.Categoria)
               .Include(i => i.Respuestas.Select(c => c.Usuario))
               .Include(i => i.Usuario)
                            where p.PreguntaId == id
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
        [Authorize(Roles = "user, admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PreguntaId,DescPregunta,MejorRespuesta")] Pregunta pregunta)
        {
            Pregunta per = db.Pregunta.Single(m => m.PreguntaId == pregunta.PreguntaId);
                Respuesta res = (from c in db.Respuesta
                                 .Include(i => i.Pregunta)
                                 .Include(i => i.Usuario)
                                 where c.Usuario.Id == pregunta.MejorUsuarioRespuestaId && c.PreguntaId == pregunta.PreguntaId select c).First();
            //per.MejorRespuesta = res.RespuestaId;
            per.DescPregunta = pregunta.DescPregunta;
            db.SaveChanges();
                return RedirectToAction("Details", "Preguntas", new { id = pregunta.PreguntaId });
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
        public ActionResult PreguntasRealizadas()
        {

            var usuario = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
            var cuenta = (from c in db.Users where c.Id == usuario.Id select c).First();
            var preguntas = (from p in db.Pregunta
                .Include(i => i.Categoria)
                .Include(i => i.Respuestas.Select(c => c.Usuario))
                .Include(i => i.Usuario)
                            where p.UsuarioId == cuenta.Id
                            select p).ToList();

            return View(preguntas);
        }

    }
}
