using puceAsk_dev1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace puceAsk_dev1.Controllers
{
    public class UsuariosController : InfoBaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Usuarios

        [Authorize(Roles ="admin")]
        public ActionResult Index()
        {
            var cantidadRegistrosPorPagina = 10;
            var totalRegistros = db.Users.Count();
            var usuarios = (from c in db.Users
                            select c).OrderBy(x => x.Id)
                                     .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                                     .Take(cantidadRegistrosPorPagina);

            var totalpaginas = (int)Math.Ceiling((double)totalRegistros / cantidadRegistrosPorPagina);
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalRegistros = totalRegistros;
            ViewBag.TotalPaginas = totalpaginas;
            ViewBag.RegistrosPorPagina = cantidadRegistrosPorPagina;
            var mensajes = db.Mensajes.Include(m => m.Emisor).Include(m => m.Receptor);
          
            return View(usuarios);
        }

        // GET: Usuarios/Details/5
        [Authorize (Roles ="admin")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //ApplicationUser usuario = db.Users.Find(id);
            ApplicationUser usuario = (from u in db.Users
                                       .Include(s => s.Respuestas)
                                       .Include(s => s.Preguntas)
                                       where u.Id == id
                                       select u).First(); 
            if (usuario == null)
            {
                return HttpNotFound();
            }

            return View(usuario);
        }

        [Authorize (Roles ="admin")]
        // GET: Usuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuarios/Edit/5
        [Authorize(Roles = "user, admin")]
        public ActionResult EditIndividual(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var usuario = (from u in db.Users
                            where u.Id == id
                            select u).First();
            if (usuario == null)
            {
                return HttpNotFound();
            }
            string imgDataURL = null;
            if (usuario.Foto != null)
            {
                byte[] foto = usuario.Foto;
                string imreBase64Data = Convert.ToBase64String(foto);
                imgDataURL = string.Format("data:image/png;base64,{0}", imreBase64Data);
            }
            else
            {
                imgDataURL = "https://img.icons8.com/color/user";
            }
            ViewBag.fotografia = imgDataURL;
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [Authorize(Roles = "user, admin")]
        public ActionResult EditIndividual([Bind(Exclude = "img", Include = "Id,FechaNacimiento,Nombre,Apellido,Sexo, Email, SaldoCuenta, EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    byte[] imageData = null;
                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFileBase poImgFile = Request.Files["Foto"];

                        if (poImgFile.ContentLength != 0)
                        {
                            using (var binary = new BinaryReader(poImgFile.InputStream))
                            {
                                imageData = binary.ReadBytes(poImgFile.ContentLength);
                            }
                        }
                        else
                        {
                            var imagen = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                            
                            imageData = imagen.Foto;
                        }
                    }
                    var usuario2 = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                    usuario2.Foto = imageData;
                    //db.Entry(usuario).State = EntityState.Modified;
                    usuario2.Email = usuario.Email;
                    db.SaveChanges();
                    return RedirectToAction("Inicio", "Preguntas");
                }

                return View(usuario);

            }
            catch
            {
                return View("Inicio", "Preguntas");
            }
        }

        // GET: Usuarios/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var usuario = (from u in db.Users
                           where u.Id == id
                           select u).First();
            if (usuario == null)
            {
                return HttpNotFound();
            }
            string imgDataURL = null;
            if (usuario.Foto != null)
            {
                byte[] foto = usuario.Foto;
                string imreBase64Data = Convert.ToBase64String(foto);
                imgDataURL = string.Format("data:image/png;base64,{0}", imreBase64Data);
            }
            else
            {
                imgDataURL = "https://img.icons8.com/color/user";
            }
            ViewBag.fotografia = imgDataURL;
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Exclude = "img", Include = "Id,FechaNacimiento,Nombre,Apellido,Sexo, Email, SaldoCuenta, EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    byte[] imageData = null;
                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFileBase poImgFile = Request.Files["Foto"];

                        if (poImgFile.ContentLength != 0)
                        {
                            using (var binary = new BinaryReader(poImgFile.InputStream))
                            {
                                imageData = binary.ReadBytes(poImgFile.ContentLength);
                            }
                        }
                        else
                        {
                            var imagen = db.Users.Find(usuario.Id);

                            imageData = imagen.Foto;
                        }
                    }
                    var usuario2 = db.Users.Find(usuario.Id);
                    usuario2.Foto = imageData;
                    //db.Entry(usuario).State = EntityState.Modified;
                    usuario2.Email = usuario.Email;
                    usuario2.FechaNacimiento = usuario.FechaNacimiento;
                    usuario2.Nombre = usuario.Nombre;
                    usuario2.Apellido = usuario.Apellido;
                    usuario2.Sexo = usuario.Sexo;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Usuarios");
                }

                return View(usuario);

            }
            catch
            {
                return View("Inicio", "Preguntas");
            }
        }

        // GET: Usuarios/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var usuario = (from u in db.Users
                           where u.Id == id
                           select u).First();
            if (usuario == null)
            {
                return HttpNotFound();
            }
            string imgDataURL = null;
            if (usuario.Foto != null)
            {
                byte[] foto = usuario.Foto;
                string imreBase64Data = Convert.ToBase64String(foto);
                imgDataURL = string.Format("data:image/png;base64,{0}", imreBase64Data);
            }
            else
            {
                imgDataURL = "https://img.icons8.com/color/user";
            }
            ViewBag.fotografia = imgDataURL;
            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser usuario = db.Users.Find(id);
            db.Users.Remove(usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
