using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MiComunidad.Models;
using System.Collections.Generic;
using PagedList;
using System.Web;
using System;

namespace MiComunidad.Controllers
{
    public class ClientesController : Controller
    {
        private MiComunidadContext db = new MiComunidadContext();

        // GET: Clientes

        public ActionResult Index(string Sorting_Order, string q, int page = 1, int pageSize = 3)
        {
            if (Session["Login"] == null)
            {
                if (Session["rut"] != null)
                {
                    string rut = Session["rut"] as string;
                    return RedirectToAction("Login", "Usuarios", new { rut = rut });
                }
                return RedirectToAction("Login", "Usuarios");
            } 

            var ClienteView = new ViewModels.ClienteView();
            ClienteView.Clientes = new List<Models.Cliente>();
            ClienteView.Cliente = new Models.Cliente();

            ViewBag.SortingRut = string.IsNullOrEmpty(Sorting_Order) ? "Rut_Description" : "";
            ViewBag.SortingNombre = string.IsNullOrEmpty(Sorting_Order) ? "Nombre_Description" : "";
            ViewBag.SortingUbicacion = string.IsNullOrEmpty(Sorting_Order) ? "Ubicacion_Description" : "";
            ViewBag.CurrentSort = Sorting_Order;

            
            var clientes = from cli in db.Clientes select cli;
            switch (Sorting_Order)
            {
                case "Rut_Description":
                    clientes = clientes.OrderByDescending(cli => cli.Rut);
                    break;
                case "Nombre_Description":
                    clientes = clientes.OrderByDescending(cli => cli.Nombre);
                    break;
                case "Ubicacion_Description":
                    clientes = clientes.OrderByDescending(cli => cli.Ubicacion);
                    break;
                default:
                    clientes = clientes.OrderBy(cli => cli.Nombre);
                    break;
            }
            
            ClienteView.PageNumber = page;
            ClienteView.PageCount = pageSize;

            ClienteView.Clientes = clientes.ToPagedList(ClienteView.PageNumber, ClienteView.PageCount).ToList(); 

            ClienteView.PageCliente = clientes.ToPagedList(ClienteView.PageNumber, ClienteView.PageCount);

            //ClienteView.PageCliente = PagedList.PagedListExtensions.ToPagedList(clientes,
            //ClienteView.PageNumber, ClienteView.PageCount);


            return View(ClienteView);

        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Cliente cliente, int? id, HttpPostedFileBase file)
        {
            string NombreCarpeta = "Logo";
            string logo = string.Empty;
            string strCarpeta = "\\" + NombreCarpeta + "\\";
            string carpetaBase = AppDomain.CurrentDomain.BaseDirectory + "\\Views\\Clientes\\";

            var ClienteView = new ViewModels.ClienteView();
            ClienteView.Cliente = cliente;
            ClienteView.Clientes = new List<Models.Cliente>();
            string rut = cliente.Rut;
            ClienteView.Cliente.ClienteID=(int)id;

            ClienteView.Clientes = db.Clientes.ToList();
           
            if (ModelState.IsValid)
            {
                //var rutExiste = ClienteView.Clientes.SingleOrDefault(c => c.Rut == rut);
                //var g = db.Clientes.Where(c => c.Rut == rut).ToList();
                Cliente BuscarclienteDB = db.Clientes.Find(id);
                if (BuscarclienteDB != null)
                {
                    if (BuscarclienteDB.Rut == rut)
                    {
                        BuscarclienteDB.Rut = cliente.Rut;
                        BuscarclienteDB.Nombre = cliente.Nombre;
                        BuscarclienteDB.Ubicacion = cliente.Ubicacion;
                        BuscarclienteDB.Url = cliente.Url;
                        //si el archivo es nuevo modifica
                        if (file == null)
                        {
                            logo = BuscarclienteDB.Logo;
                        }
                        else
                        {
                            
                            if (!System.IO.Directory.Exists(carpetaBase + strCarpeta))
                            {
                                //1.-primera ves crea carpeta
                                System.IO.Directory.CreateDirectory(carpetaBase + strCarpeta);
                                string subcarpeta = carpetaBase + strCarpeta + cliente.Rut;
                                if (!System.IO.Directory.Exists(subcarpeta))
                                {
                                    //1.-primera ves crea subcarpeta
                                    System.IO.Directory.CreateDirectory(subcarpeta);
                                }
                                logo = (DateTime.Now.ToString("yyyyMMdd") + "-" + file.FileName).ToLower();
                                System.IO.File.Delete(Server.MapPath("~/Views/Clientes" + "/" + NombreCarpeta + "/" + cliente.Rut + "/" + BuscarclienteDB.Logo));
                                file.SaveAs(Server.MapPath("~/Views/Clientes" + "/" + NombreCarpeta + "/" + cliente.Rut + "/" + logo));
                            }
                            else
                            {
                                string subcarpeta = carpetaBase + strCarpeta + cliente.Rut;
                                if (!System.IO.Directory.Exists(subcarpeta))
                                {
                                    //1.-primera ves crea subcarpeta
                                    System.IO.Directory.CreateDirectory(subcarpeta);
                                }
                                
                                logo = (DateTime.Now.ToString("yyyyMMdd") + "-" + file.FileName).ToLower();
                                System.IO.File.Delete(Server.MapPath("~/Views/Clientes" + "/" + NombreCarpeta + "/" + cliente.Rut + "/" + BuscarclienteDB.Logo));
                                file.SaveAs(Server.MapPath("~/Views/Clientes" + "/" + NombreCarpeta + "/" + cliente.Rut + "/" + logo));
                            }
                            
                        }
                        BuscarclienteDB.Logo = logo;
                        db.Entry(BuscarclienteDB).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");                    

                    }
                }
                else
                {
                    ClienteView.Cliente = cliente;
                   if (!System.IO.Directory.Exists(carpetaBase + strCarpeta))
                    {
                        //1.-primera ves crea carpeta
                        System.IO.Directory.CreateDirectory(carpetaBase + strCarpeta);
                        string subcarpeta = carpetaBase + strCarpeta + cliente.Rut;
                        if (!System.IO.Directory.Exists(subcarpeta))
                        {
                            //1.-primera ves crea subcarpeta
                            System.IO.Directory.CreateDirectory(subcarpeta);
                        }
                        logo = (DateTime.Now.ToString("yyyyMMdd") + "-" + file.FileName).ToLower();
                        file.SaveAs(Server.MapPath("~/Views/Clientes" + "/" + NombreCarpeta + "/" + cliente.Rut + "/" + logo));
                    }
                    else
                    {
                        string subcarpeta = carpetaBase + strCarpeta + cliente.Rut;
                        if (!System.IO.Directory.Exists(subcarpeta))
                        {
                            //1.-primera ves crea subcarpeta
                            System.IO.Directory.CreateDirectory(subcarpeta);
                        }
                        logo = (DateTime.Now.ToString("yyyyMMdd") + "-" + file.FileName).ToLower();
                        file.SaveAs(Server.MapPath("~/Views/Clientes" + "/" + NombreCarpeta + "/" + cliente.Rut +  "/" + logo));
                    }

                    ClienteView.Cliente.Logo = logo;
                    db.Clientes.Add(ClienteView.Cliente);
                    db.Entry(ClienteView.Cliente).State = EntityState.Added;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
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
            return RedirectToAction("Index");

            //var ClienteView = new ViewModels.ClienteView();
            //ClienteView.Cliente = new Models.Cliente();
            //ClienteView.Clientes = new List<Models.Cliente>();
            //ClienteView.Clientes = db.Clientes.ToList();

            //return (View("Index", ClienteView));
        }

        private ActionResult Eliminar(int? id)
        {
             var ClienteView = new ViewModels.ClienteView();
            //ClienteView.Cliente = new Models.Cliente();
            ClienteView.Clientes = new List<Models.Cliente>();
            ClienteView.Clientes = db.Clientes.ToList();

            return View("Index", ClienteView);

        
        }

     
        // GET: Clientes/Details/5
        public ActionResult Details(int? id, string Sorting_Order, string q, int page = 1, int pageSize = 3)
        {
            var ClienteView = new ViewModels.ClienteView();
            ClienteView.Cliente = new Models.Cliente();
            ClienteView.Clientes = new List<Models.Cliente>();
            ClienteView.Clientes = db.Clientes.ToList();

            ViewBag.SortingRut = string.IsNullOrEmpty(Sorting_Order) ? "Rut_Description" : "";
            ViewBag.SortingNombre = string.IsNullOrEmpty(Sorting_Order) ? "Nombre_Description" : "";
            ViewBag.SortingUbicacion = string.IsNullOrEmpty(Sorting_Order) ? "Ubicacion_Description" : "";
            ViewBag.CurrentSort = Sorting_Order;


            var clientes = from cli in db.Clientes select cli;
            switch (Sorting_Order)
            {
                case "Rut_Description":
                    clientes = clientes.OrderByDescending(cli => cli.Rut);
                    break;
                case "Nombre_Description":
                    clientes = clientes.OrderByDescending(cli => cli.Nombre);
                    break;
                case "Ubicacion_Description":
                    clientes = clientes.OrderByDescending(cli => cli.Ubicacion);
                    break;
                default:
                    clientes = clientes.OrderBy(cli => cli.Nombre);
                    break;
            }

            ClienteView.PageNumber = page;
            ClienteView.PageCount = pageSize;

            ClienteView.Clientes = clientes.ToPagedList(ClienteView.PageNumber, ClienteView.PageCount).ToList();

            ClienteView.PageCliente = clientes.ToPagedList(ClienteView.PageNumber, ClienteView.PageCount);

            
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                //return HttpNotFound();
            }
            if (id == null)
            {
                //return View("Index", ClienteView);
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClienteView.Cliente = cliente;
            //return View("Index", ClienteView);
            //return View(ClienteView.Cliente);

            return AddNote("Details", ClienteView.Cliente);
        }

        public ActionResult AddNote(string vista, object modelo)
        {
            return PartialView(vista,modelo);
        }

        [HttpPost]
        public ActionResult AddNote(Cliente cliente)
        {
            return RedirectToAction("Index");
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
        public ActionResult Edit(int? id, string Sorting_Order, string q, int page = 1, int pageSize = 3)
        {
            var ClienteView = new ViewModels.ClienteView();
            ClienteView.Cliente = new Models.Cliente();
            ClienteView.Clientes = new List<Models.Cliente>();
            ClienteView.Clientes = db.Clientes.ToList();

            ViewBag.SortingRut = string.IsNullOrEmpty(Sorting_Order) ? "Rut_Description" : "";
            ViewBag.SortingNombre = string.IsNullOrEmpty(Sorting_Order) ? "Nombre_Description" : "";
            ViewBag.SortingUbicacion = string.IsNullOrEmpty(Sorting_Order) ? "Ubicacion_Description" : "";
            ViewBag.CurrentSort = Sorting_Order;


            var clientes = from cli in db.Clientes select cli;
            switch (Sorting_Order)
            {
                case "Rut_Description":
                    clientes = clientes.OrderByDescending(cli => cli.Rut);
                    break;
                case "Nombre_Description":
                    clientes = clientes.OrderByDescending(cli => cli.Nombre);
                    break;
                case "Ubicacion_Description":
                    clientes = clientes.OrderByDescending(cli => cli.Ubicacion);
                    break;
                default:
                    clientes = clientes.OrderBy(cli => cli.Nombre);
                    break;
            }

            ClienteView.PageNumber = page;
            ClienteView.PageCount = pageSize;

            ClienteView.Clientes = clientes.ToPagedList(ClienteView.PageNumber, ClienteView.PageCount).ToList();

            ClienteView.PageCliente = clientes.ToPagedList(ClienteView.PageNumber, ClienteView.PageCount);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            ViewBag.Logo = cliente.Logo;
            ClienteView.Cliente = cliente;
            return View("Index", ClienteView);
        }

        // POST: Clientes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cliente cliente, int? id)
        {
            //[Bind(Include = "ClienteID,Rut,Nombre,Ubicacion,Url,Email")] Cliente cliente
            var ClienteView = new ViewModels.ClienteView();
            ClienteView.Cliente = cliente;//new Models.Cliente();
            ClienteView.Clientes = new List<Models.Cliente>();
            ClienteView.Clientes = db.Clientes.ToList();
            
            string rut = cliente.Rut;

            if (ModelState.IsValid)
            {
                // var rutExiste = ClienteView.Clientes.SingleOrDefault(c => c.Rut == rut);
                Cliente BuscarBlienteDB = db.Clientes.Find(id);
                if (BuscarBlienteDB != null)
                {
                    if (ClienteView.Cliente.Rut == rut)
                    {
                        foreach (var c in ClienteView.Clientes)
                        {
                            db.Entry(c).State = EntityState.Modified;
                        }

                        ClienteView.Cliente.ClienteID = (int)id;
                        cliente.ClienteID = (int)id;
                        db.Entry(cliente).State = EntityState.Added;
                        db.SaveChanges();
                        


                    }
                }
               else
                {
                    return RedirectToAction("Index");
                }
            }
            return View("Index", ClienteView);
            //return View(ClienteView);
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
            db.Entry(ClienteView.Cliente).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
            //return View("Index", ClienteView);
        }

      

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = db.Clientes.Find(id);
            db.Clientes.Remove(cliente);
            db.Entry(cliente).State = EntityState.Deleted;
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
