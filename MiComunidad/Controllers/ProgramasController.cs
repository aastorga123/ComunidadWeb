using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MiComunidad.Models;
using PagedList;

namespace MiComunidad.Controllers
{
    public class ProgramasController : Controller
    {
        private MiComunidadContext db = new MiComunidadContext();

        // GET: Programas
        public ActionResult Index(string Sorting_Order, string q, int page = 1, int pageSize = 3)
        {
            var Programaview = new ViewModels.ProgramaView();
            Programaview.Programa = new Models.Programa();
            Programaview.Programas = new List<Models.Programa>();
            Programaview.Programas = db.Programas.ToList();

            ViewBag.SortingNombrePrograma = string.IsNullOrEmpty(Sorting_Order) ? "NombrePrograma_Description" : "";
            ViewBag.CurrentSort = Sorting_Order;


            var programas = from pro in db.Programas select pro;
            switch (Sorting_Order)
            {
                case "NombrePrograma_Description":
                    programas = programas.OrderByDescending(p => p.NombrePrograma);
                    break;
                default:
                    programas = programas.OrderBy(p => p.NombrePrograma);
                    break;
            }

            Programaview.PageNumber = page;
            Programaview.PageCount = pageSize;

            Programaview.Programas = programas.ToPagedList(Programaview.PageNumber, Programaview.PageCount).ToList();

            Programaview.PageCliente = programas.ToPagedList(Programaview.PageNumber, Programaview.PageCount);

            return View(Programaview);
        }

        // GET: Programas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Programa programa = db.Programas.Find(id);
            if (programa == null)
            {
                return HttpNotFound();
            }
            return View(programa);
        }

        // GET: Programas/Create
        public ActionResult Create()
        {
            var Programaview = new ViewModels.ProgramaView();
            Programaview.Programa = new Models.Programa();
            Programaview.Programas = new List<Models.Programa>();
            Programaview.Programas = db.Programas.ToList();

            return View(Programaview);
        }

        // POST: Programas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? id,[Bind(Include = "ProgramaID,NombrePrograma,DescripcionPrograma")] Programa programa)
        {
            var Programaview = new ViewModels.ProgramaView();
            Programaview.Programa = programa;
            Programaview.Programas = new List<Models.Programa>();
            Programaview.Programas = db.Programas.ToList();
            int idpr = programa.ProgramaID;
            if (ModelState.IsValid)
            {
                Programa BuscarProgramaDB = db.Programas.Find(idpr);
                if (BuscarProgramaDB != null)
                {
                    if (BuscarProgramaDB.NombrePrograma == programa.NombrePrograma)
                    {
                        db.Programas.Remove(BuscarProgramaDB);
                        db.Entry(BuscarProgramaDB).State = EntityState.Deleted;
                        db.SaveChanges();

                        db.Programas.Add(Programaview.Programa);
                        db.Entry(Programaview.Programa).State = EntityState.Added;
                        db.SaveChanges();

                        db.Entry(Programaview.Programa).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }

                db.Programas.Add(programa);
                db.Entry(programa).State = EntityState.Added;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            else
            {
                return View("Index", Programaview);
            }

            return RedirectToAction("Index"); //View(Programaview);
        }

        // GET: Programas/Edit/5
        public ActionResult Edit(int? id, string Sorting_Order, string q, int page = 1, int pageSize = 3)
        {
            var Programaview = new ViewModels.ProgramaView();
            Programaview.Programa = new Models.Programa();
            Programaview.Programas = new List<Models.Programa>();
            Programaview.Programas = db.Programas.ToList();

            ViewBag.SortingNombrePrograma = string.IsNullOrEmpty(Sorting_Order) ? "NombrePrograma_Description" : "";
            ViewBag.CurrentSort = Sorting_Order;


            var programas = from pro in db.Programas select pro;
            switch (Sorting_Order)
            {
                case "NombrePrograma_Description":
                    programas = programas.OrderByDescending(p => p.NombrePrograma);
                    break;
                default:
                    programas = programas.OrderBy(p => p.NombrePrograma);
                    break;
            }

            Programaview.PageNumber = page;
            Programaview.PageCount = pageSize;

            Programaview.Programas = programas.ToPagedList(Programaview.PageNumber, Programaview.PageCount).ToList();

            Programaview.PageCliente = programas.ToPagedList(Programaview.PageNumber, Programaview.PageCount);
            Programaview.Programa = db.Programas.Find(id);
            if (Programaview.Programa == null)
            {
                return HttpNotFound();
            }
            return View("Index", Programaview);
           // return View(programa);
        }

        // POST: Programas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProgramaID,NombrePrograma,DescripcionPrograma")] Programa programa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(programa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(programa);
        }

        // GET: Programas/Delete/5
        public ActionResult Delete(int? id)
        {
            var Programaview = new ViewModels.ProgramaView();
            Programaview.Programa = new Models.Programa();
            Programaview.Programas = new List<Models.Programa>();
            Programaview.Programas = db.Programas.ToList();


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Programaview.Programa = db.Programas.Find(id);
            if (Programaview.Programa == null)
            {
                return HttpNotFound();
            }
            db.Programas.Remove(Programaview.Programa);
            db.Entry(Programaview.Programa).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");

            //return View(Programaview);
        }

        // POST: Programas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Programa programa = db.Programas.Find(id);
            db.Programas.Remove(programa);
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
