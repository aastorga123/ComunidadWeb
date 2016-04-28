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
        //db.CustomerLogs.Where(p => p.CustomerID == CustomerId).OrderByDescending(p => p.DateCreated)
        public PagedList.IPagedList<Cliente> PageCliente;// { get { return PagedList.PagedListExtensions.ToPagedList(db.Clientes.Where(c => c.ClienteID == Cliente.ClienteID).OrderByDescending(p => p.Nombre), PageNumber,PageCount); } }
        public int PageCount; 
        public int PageNumber;
    }
        //@model PagedList.IPagedList<MiComunidad.ViewModels.ClienteView>
    
}