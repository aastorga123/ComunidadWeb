using MiComunidad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiComunidad.ViewModels
{
    public class ClienteView
    {
        public Cliente Cliente { get; set; }
        public List<Cliente> Clientes { get; set; }
    }
}