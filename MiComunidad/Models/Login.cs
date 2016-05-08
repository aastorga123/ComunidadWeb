using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiComunidad.Models
{
    public class Login
    {
        [Key]
        public int loginID { get; set; }
        [Display(Name = "Correo")]
        [Required(ErrorMessage = "Debe ingresar {0}")]
        [DataType(DataType.EmailAddress, ErrorMessage ="El formato {0} es invalido")]
        [StringLength(30, ErrorMessage = "El campo {0} debe ser entre {1} and {2} caracteres", MinimumLength = 3)]
        public string EmailUsuario { get; set; }
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Debe ingresar {0}")]
        [DataType(DataType.Password)]
        [StringLength(30, ErrorMessage = "El campo {0} debe ser entre {1} and {2} caracteres", MinimumLength = 3)]
        public string PassUsuario { get; set; }

        public virtual ICollection<UsuarioLogin> UsuarioLogins { get; set; }
    }
}