using MiComunidad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiComunidad.ViewModels
{
    public class UsuarioView
    {
        public Usuario Usuario { get; set; }
        public List<Usuario> Usuarios { get; set; }

        public PagedList.IPagedList<Usuario> PageUsuario;
        public int PageCount;
        public int PageNumber;

        public RolUsuario RolUsuario { get; set; }
        public List<RolUsuario> RolUsuarios { get; set; }
    }
}