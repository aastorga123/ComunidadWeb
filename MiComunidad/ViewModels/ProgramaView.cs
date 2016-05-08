using MiComunidad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiComunidad.ViewModels
{
    public class ProgramaView
    {
        public Programa Programa { get; set; }
        public List<Programa> Programas { get; set; }

        public PagedList.IPagedList<Programa> PageCliente;
        public int PageCount;
        public int PageNumber;
    }
}