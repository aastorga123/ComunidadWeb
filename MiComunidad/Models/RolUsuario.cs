using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiComunidad.Models
{
    public class RolUsuario
    {
        [Key]
        public int RolUsuarioID { get; set; }
        [Display(Name = "Rol")]
        [StringLength(50, ErrorMessage = "El campo {0} debe ser entre {1} and {2} caracteres", MinimumLength = 0)]
        public string NombreRol { get; set; }
        [Display(Name = "Descripción")]
        [DataType(DataType.MultilineText)]
        [StringLength(200, ErrorMessage = "El campo {0} debe ser entre {1} and {2} caracteres", MinimumLength = 0)]
        public string DescripcionRol { get; set; }

        public int UsuarioID { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}