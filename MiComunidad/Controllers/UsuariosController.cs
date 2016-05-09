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
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;

namespace MiComunidad.Controllers
{
    public class UsuariosController : Controller
    {
        private MiComunidadContext db = new MiComunidadContext();

        // GET: Usuarios
        public ActionResult Index(string Sorting_Order, string q, int page = 1, int pageSize = 3)
        {
           
            if (Session["Login"] == null)
            {
                if (Session["rut"] != null)
                {
                    string rut = Session["rut"] as string;
                    return RedirectToAction("Login", "Usuarios", new {rut=rut });
                }
                return RedirectToAction("Login", "Usuarios");
            }

            var UsuarioView = new ViewModels.UsuarioView();
            UsuarioView.Usuario = new Models.Usuario();
            UsuarioView.Usuarios = new List<Models.Usuario>();
            var usuarios = db.Usuarios.Include(u => u.Cliente);
            UsuarioView.Usuarios = usuarios.ToList();

            ViewBag.SortingNombreUsuario = string.IsNullOrEmpty(Sorting_Order) ? "NombreUsuario_Description" : "";
            ViewBag.CurrentSort = Sorting_Order;


            usuarios = from pro in db.Usuarios select pro;
            switch (Sorting_Order)
            {
                case "NombreUsuario_Description":
                    usuarios = usuarios.OrderByDescending(u => u.NombreUsuario);
                    break;
                default:
                    usuarios = usuarios.OrderBy(u => u.NombreUsuario);
                    break;
            }

            var list = db.Clientes.ToList();
            list.Add(new Cliente { ClienteID = 0, Rut = "[Seleccione un cliente...]" });
            list = list.OrderBy(c => c.Nombre).ToList();
            ViewBag.ClienteID = new SelectList(list, "ClienteID", "Rut");


            UsuarioView.PageNumber = page;
            UsuarioView.PageCount = pageSize;

            UsuarioView.Usuarios = usuarios.ToPagedList(UsuarioView.PageNumber, UsuarioView.PageCount).ToList();

            UsuarioView.PageUsuario = usuarios.ToPagedList(UsuarioView.PageNumber, UsuarioView.PageCount);

            Session["UsuarioView"] = UsuarioView;
            return View(UsuarioView);
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        public ActionResult RolCrear()
        {
            var UsuarioView = new ViewModels.UsuarioView();
            UsuarioView.Usuario = new Models.Usuario();
            UsuarioView.Usuarios = new List<Models.Usuario>();
            var usuarios = db.Usuarios.Include(u => u.Cliente);
            UsuarioView.Usuarios = usuarios.ToList();

            UsuarioView.RolUsuario = new RolUsuario();
            UsuarioView.RolUsuarios = new List<RolUsuario>();
            UsuarioView.RolUsuarios = db.RolUsuarios.ToList();

            return View(UsuarioView);
        }
        // GET: Usuarios/Create
        public ActionResult Create(int? id)
        {
            var UsuarioView = new ViewModels.UsuarioView();
            UsuarioView.Usuario = new Models.Usuario();
            UsuarioView.Usuarios = new List<Models.Usuario>();
            var usuarios = db.Usuarios.Include(u => u.Cliente);
            UsuarioView.Usuarios = usuarios.ToList();

            var customerID = int.Parse(Request["ClienteID"]);
            string rut = Request["rut"].ToString();
            var list = db.Clientes.ToList();
            list.Add(new Cliente { ClienteID = 0, Nombre = "[Seleccione un cliente...]" });
            list = list.OrderBy(c => c.Nombre).ToList();
            ViewBag.ClienteID = new SelectList(list, "ClienteID", "Rut");


            return View("Index", UsuarioView);
        }

        // POST: Usuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UsuarioID,NombreUsuario,EmailUsuario,PassUsuario,TipoUsuario,ClienteID")] Usuario usuario)
        {
            var UsuarioView = new ViewModels.UsuarioView();
            UsuarioView.Usuario = usuario;
            UsuarioView.Usuarios = new List<Models.Usuario>();
            var usuarios = db.Usuarios.Include(u => u.Cliente);
            UsuarioView.Usuarios = usuarios.ToList();
            Login login = new Models.Login();
            UsuarioLogin usuariologin = new UsuarioLogin();
            //string rut = Request["rut"].ToString();
            //var usuarioView = Session["UsuarioView"] as ViewModels.UsuarioView;
            int customerID = int.Parse(Request["ClienteID"]);
            UsuarioView.Usuario.ClienteID = customerID;
            usuario.ClienteID = customerID;


            if (ModelState.IsValid)
            {
                if (Session["IdUsuario"] != null)
                {
                    int iduser = (int)Session["IdUsuario"];
                    Usuario BuscarusuarioDB = db.Usuarios.Find(iduser);
                    if (BuscarusuarioDB != null)
                    {
                        if (BuscarusuarioDB.UsuarioID == iduser)
                        {
                            Login BuscarLoginDB = db.logins.FirstOrDefault(l => l.EmailUsuario == BuscarusuarioDB.EmailUsuario && l.PassUsuario == BuscarusuarioDB.PassUsuario);
                            if(BuscarLoginDB!=null)
                            {
                                BuscarLoginDB.EmailUsuario = usuario.EmailUsuario;
                                BuscarLoginDB.PassUsuario = usuario.PassUsuario;

                                db.Entry(BuscarLoginDB).State = EntityState.Modified;
                                db.SaveChanges();

                                BuscarusuarioDB.NombreUsuario = usuario.NombreUsuario;
                                BuscarusuarioDB.PassUsuario = usuario.PassUsuario;
                                BuscarusuarioDB.EmailUsuario = usuario.EmailUsuario;
                                BuscarusuarioDB.TipoUsuario = usuario.TipoUsuario;

                                db.Entry(BuscarusuarioDB).State = EntityState.Modified;
                                db.SaveChanges();

                                UsuarioLogin BuscarUsuarioLoginDB = db.UsuarioLogins.FirstOrDefault(l => l.UsuarioID == BuscarusuarioDB.UsuarioID && l.loginID == BuscarLoginDB.loginID);
                                if (BuscarUsuarioLoginDB != null)
                                {
                                    BuscarUsuarioLoginDB.Login = BuscarLoginDB;
                                    BuscarUsuarioLoginDB.Usuario = BuscarusuarioDB;

                                    db.Entry(BuscarUsuarioLoginDB).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                    

                            Session["IdUsuario"] = null;
                            return RedirectToAction("Index");
                        }
                    }
                }
                db.Usuarios.Add(usuario);
                db.Entry(usuario).State = EntityState.Added;
                db.SaveChanges();

                
                login.EmailUsuario = usuario.EmailUsuario;
                login.PassUsuario = usuario.PassUsuario;
                db.logins.Add(login);
                db.Entry(login).State = EntityState.Added;
                db.SaveChanges();

                usuariologin.Login = login;
                usuariologin.Usuario = usuario;
                db.UsuarioLogins.Add(usuariologin);
                db.Entry(usuariologin).State = EntityState.Added;
                db.SaveChanges();

                FormatearCorreo(usuario);
                
                return RedirectToAction("Index");
            }

            //ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Rut", usuario.ClienteID);
            return View(usuario);
        }


        public void FormatearCorreo(Usuario usuario)
        {
            Cliente cliente = db.Clientes.Find(usuario.ClienteID);//db.Clientes.Where(c => c.ClienteID == usuario.ClienteID);
            String cuerpoMensaje = System.IO.File.OpenText(Server.MapPath("../Views/Usuarios/MailTemplateRegistrado.html")).ReadToEnd();
            
            String[] args = new String[4];
            args[0] = usuario.EmailUsuario;
            args[1] = usuario.PassUsuario;
            args[2] = "https://localhost/Usuarios/Login?" + "rut=" + cliente.Rut;
            args[3] = ""; //Server.MapPath("~/Views/Clientes/" + cliente.Rut + "/" + cliente.Logo );//"../Clientes/Logo/" + cliente.Rut + "/" + cliente.Logo;
            //args[3] = "<img src='" +  + "' />";
            string carpetaBase = AppDomain.CurrentDomain.BaseDirectory + "\\Views\\Clientes\\Logo\\";
            //"C:\Users\Admin\Documents\Visual Studio 2015\Projects\MiComunidad\MiComunidad\Captura.JPG"
            string carpetaarchivo = carpetaBase + cliente.Rut + "\\" + cliente.Logo;
            cuerpoMensaje = String.Format(cuerpoMensaje, args);
            EnviarCorreoController.EnviarCorreoRegistroUsuario("Mi Comunidad - Registro Usuario", cuerpoMensaje, usuario, carpetaarchivo);
        }



        // GET: Usuarios/Edit/5
        public ActionResult Edit(int? id, string Sorting_Order, string q, int page = 1, int pageSize = 3)
        {
            var UsuarioView = new ViewModels.UsuarioView();
            UsuarioView.Usuario = new Models.Usuario();
            UsuarioView.Usuarios = new List<Models.Usuario>();
            var usuarios = db.Usuarios.Include(u => u.Cliente);
            UsuarioView.Usuarios = usuarios.ToList();

            ViewBag.SortingNombreUsuario = string.IsNullOrEmpty(Sorting_Order) ? "NombreUsuario_Description" : "";
            ViewBag.CurrentSort = Sorting_Order;


            usuarios = from pro in db.Usuarios select pro;
            switch (Sorting_Order)
            {
                case "NombreUsuario_Description":
                    usuarios = usuarios.OrderByDescending(u => u.NombreUsuario);
                    break;
                default:
                    usuarios = usuarios.OrderBy(u => u.NombreUsuario);
                    break;
            }

            //var list = db.Clientes.ToList();
            //list.Add(new Cliente { ClienteID = 0, Rut = "[Seleccione un cliente...]" });
            //list = list.OrderBy(c => c.Nombre).ToList();
            //ViewBag.ClienteID = new SelectList(list, "ClienteID", "Rut");

            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Rut", id);


            UsuarioView.PageNumber = page;
            UsuarioView.PageCount = pageSize;

            UsuarioView.Usuarios = usuarios.ToPagedList(UsuarioView.PageNumber, UsuarioView.PageCount).ToList();

            UsuarioView.PageUsuario = usuarios.ToPagedList(UsuarioView.PageNumber, UsuarioView.PageCount);
            UsuarioView.Usuario = db.Usuarios.Find(id);
            if (UsuarioView.Usuario == null)
            {
                return HttpNotFound();
            }

            Session["IdUsuario"] = id;
            return View("Index", UsuarioView);
        }

        // POST: Usuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UsuarioID,NombreUsuario,EmailUsuario,PassUsuario,TipoUsuario,ClienteID")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Rut", usuario.ClienteID);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            

            Login Login = db.logins.FirstOrDefault(l => l.EmailUsuario == usuario.EmailUsuario && l.PassUsuario == usuario.PassUsuario);
            if (Login != null)
            {
               
                UsuarioLogin usuarioLogin = db.UsuarioLogins.FirstOrDefault(l => l.UsuarioID == id && l.loginID == Login.loginID);
                if (usuarioLogin != null)
                {
                    db.UsuarioLogins.Remove(usuarioLogin);
                    db.Entry(usuarioLogin).State = EntityState.Deleted;
                    db.SaveChanges();

                    db.logins.Remove(Login);
                    db.Entry(Login).State = EntityState.Deleted;
                    db.SaveChanges();

                    db.Usuarios.Remove(usuario);
                    db.Entry(usuario).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");

        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuario);
            db.Entry(usuario).State = EntityState.Deleted;
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

        public void ImagenLogo(string rut)
        {
            if (!string.IsNullOrEmpty(rut))
            {
                
                Cliente cliente = db.Clientes.FirstOrDefault(c => c.Rut == rut);
                if (cliente != null)
                {
                    string path = Server.MapPath("~/Views/Clientes/Logo/" + rut + "/" + cliente.Logo);
                    byte[] imageByteData = System.IO.File.ReadAllBytes(path);
                    string imageBase64Data = Convert.ToBase64String(imageByteData);
                    string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                    ViewBag.Logo = imageDataURL;
                }
            }
            else
            {
                ViewBag.Logo = "";
            }
        }
        public  ActionResult Login()
        {
            if(Session["ErrorSesion"]!=null)
            {
                Session["ErrorSesion"] = null;
            }
            if(Request["rut"]!=null)
            {
                string rut = Request["rut"].ToString();
                Session["rut"] = rut;
                ImagenLogo(rut);
            }
            
            return View();

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login Login, string rut)
        {
            if (!ModelState.IsValid)
            {
                ImagenLogo(rut);
                return View();
            }
            else
            {

                Login validaLogin = db.logins.FirstOrDefault(u => u.EmailUsuario == Login.EmailUsuario && u.PassUsuario == Login.PassUsuario);
                if (validaLogin != null)
                {
                    if (validaLogin.EmailUsuario == Login.EmailUsuario && validaLogin.PassUsuario == Login.PassUsuario)
                    {
                        if (string.IsNullOrEmpty(rut))
                        {
                            Usuario buscarusuarioBD = db.Usuarios.FirstOrDefault(u => u.EmailUsuario == Login.EmailUsuario && u.PassUsuario == Login.PassUsuario);
                            if (buscarusuarioBD != null)
                            {
                                Cliente buscarclienteBD = db.Clientes.Find(buscarusuarioBD.ClienteID);
                                if (buscarclienteBD != null)
                                {
                                    rut = buscarclienteBD.Rut;
                                }
                                
                            }
                           
                        }
                        Session["rut"] = rut;
                        Session["Login"] = validaLogin;
                        return RedirectToAction("Inicio", "Usuarios");
                    }

                }
                else
                {
                    Session["rut"] = null;
                    Session["Login"] = null;
                    ViewBag.Error = "Su correo y/o nombre de usuario son inválidos";
                }


                //return RedirectToAction("~/Views/Shared/_Layout.cshtml");
            }
            return RedirectToAction("Login","Usuarios");
        }
        public ActionResult Inicio()
        {
            if (Session["rut"] != null)
            {
                return View();
            }
            else
            {
                if (Session["Login"]!=null)
                {
                    return View();
                }
                return RedirectToAction("Login", "Usuarios");
            }

        }

        public ActionResult Logo()
        {
            if (Session["rut"] != null)
            {
                string rut = Session["rut"] as string;
                if (!string.IsNullOrEmpty(rut))
                {
                    ImagenLogo(rut);
                    return PartialView();
                }
            }
            if (Session["Login"] != null)
            {
                ImagenLogo(null);
                return PartialView();
            }
            return RedirectToAction("Login", "Usuarios");
            
            
        }

        public ActionResult CerrarSesion()
        {
            Session["Login"] = null;

            if (Session["Login"] == null)
            {
                if (Session["rut"] != null)
                {
                    string rut = Session["rut"] as string;
                    return RedirectToAction("Login", "Usuarios", new { rut = rut });
                }
                return RedirectToAction("Login", "Usuarios");
            }

            Session["rut"] = null;
            
            return RedirectToAction("Login", "Usuarios");
        }


    }
}
