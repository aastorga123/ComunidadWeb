using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MiComunidad.Models;
using System.Collections.Generic;

namespace MiComunidad.Controllers
{
    public class ClientesController : Controller
    {
        private MiComunidadContext db = new MiComunidadContext();

        // GET: Clientes

        public ActionResult AddCliente()
        {
            var ClienteView = new ViewModels.ClienteView();
            ClienteView.Cliente = new Models.Cliente();
            ClienteView.Clientes = new List<Models.Cliente>();
            ClienteView.Clientes = db.Clientes.ToList();
            return View(ClienteView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCliente(Cliente cliente)
        {
            var ClienteView = new ViewModels.ClienteView();
            ClienteView.Cliente = new Models.Cliente();
            ClienteView.Clientes = new List<Models.Cliente>();
            ClienteView.Clientes = db.Clientes.ToList();
           
            if (ModelState.IsValid)
            {
                db.Clientes.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("AddCliente");

            }

            return View(ClienteView);
        }

        [HttpPost]
        public ActionResult Accion(string submitButton, int? id)
        {
           
            switch (submitButton)
            {
                case "Eliminar":
                    // delegate sending to another controller action
                    return (Eliminar(id));
                case "Cancelar":
                    // call another action to perform the cancellation
                    return (Cancelar());
                default:
                    // If they've submitted the form without a submitButton, 
                    // just return the view again.
                    return (View());
            }
        }

        private ActionResult Cancelar()
        {
            return RedirectToAction("AddCliente");

            //var ClienteView = new ViewModels.ClienteView();
            //ClienteView.Cliente = new Models.Cliente();
            //ClienteView.Clientes = new List<Models.Cliente>();
            //ClienteView.Clientes = db.Clientes.ToList();

            //return (View("AddCliente", ClienteView));
        }

        private ActionResult Eliminar(int? id)
        {
             var ClienteView = new ViewModels.ClienteView();
            //ClienteView.Cliente = new Models.Cliente();
            ClienteView.Clientes = new List<Models.Cliente>();
            ClienteView.Clientes = db.Clientes.ToList();

            return View("AddCliente", ClienteView);

            //if (id != null)
            //{
            //    Cliente cliente = db.Clientes.Find(id);
            //    if (cliente != null)
            //    {
            //        db.Clientes.Remove(cliente);
            //        db.SaveChanges();
            //        return RedirectToAction("AddCliente");

            //    }
            //    else { return HttpNotFound(); }
            //}
            //else
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
        }

        //public ActionResult Index()
        //{
        //    return View(db.Clientes.ToList());
        //}

        // GET: Clientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClienteID,Rut,Nombre,Ubicacion,Url,Email")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Clientes.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            var ClienteView = new ViewModels.ClienteView();
            ClienteView.Cliente = new Models.Cliente();
            ClienteView.Clientes = new List<Models.Cliente>();
            ClienteView.Clientes = db.Clientes.ToList();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            ClienteView.Cliente = cliente;
            return View("AddCliente", ClienteView);
        }

        // POST: Clientes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClienteID,Rut,Nombre,Ubicacion,Url,Email")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int? id)
        {
            var ClienteView = new ViewModels.ClienteView();
            ClienteView.Cliente = new Models.Cliente();
            ClienteView.Clientes = new List<Models.Cliente>();
            ClienteView.Clientes = db.Clientes.ToList();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClienteView.Cliente = db.Clientes.Find(id);
            if (ClienteView.Cliente == null)
            {
                return HttpNotFound();
            }
            ClienteView.Cliente = db.Clientes.Find(id);
            db.Clientes.Remove(ClienteView.Cliente);
            db.SaveChanges();
            return RedirectToAction("AddCliente");
            //return View("AddCliente", ClienteView);
        }

       /* public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View("", cliente);
        }*/

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = db.Clientes.Find(id);
            db.Clientes.Remove(cliente);
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
