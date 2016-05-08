using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiComunidad.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioID { get; set; }
        [Display(Name = "Nombre Usuario")]
        [Required(ErrorMessage = "Debe ingresar {0}")]
        [StringLength(30, ErrorMessage = "El campo {0} debe ser entre {1} and {2} caracteres", MinimumLength = 3)]
        public string NombreUsuario { get; set; }
        
        [Display(Name = "tipo")]
        public TipoUsuario TipoUsuario { get; set; }

        [Display(Name = "Correo")]
        [Required(ErrorMessage = "Debe ingresar {0}")]
        [StringLength(30, ErrorMessage = "El campo {0} debe ser entre {1} and {2} caracteres", MinimumLength = 3)]
        [DataType(DataType.EmailAddress)]
        public string EmailUsuario { get; set; }
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Debe ingresar {0}")]
        [DataType(DataType.Password)]
        [StringLength(30, ErrorMessage = "El campo {0} debe ser entre {1} and {2} caracteres", MinimumLength = 3)]
        public string PassUsuario { get; set; }

        public int ClienteID { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<UsuarioLogin> UsuarioLogins { get; set; }

    }
}