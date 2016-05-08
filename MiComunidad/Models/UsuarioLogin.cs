using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiComunidad.Models
{
    public class UsuarioLogin
    {
        [Key]
        public int UsuarioLoginID { get; set; }
        public int UsuarioID { get; set; }
        public int loginID { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual Login Login { get; set; }
    }
}