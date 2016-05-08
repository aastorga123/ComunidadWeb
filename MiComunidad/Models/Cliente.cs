using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MiComunidad.Models
{
    public class Cliente
    {
        [Key]
        public int ClienteID { get; set; }
        [Display(Name = "Rut")]
        [RegularExpression(@"\b\d{1,8}\-[K|k|0-9]", ErrorMessage = "Formato {0} no es válido. (Ej: 13657654-K)")]
        [Required(ErrorMessage = "Debe ingresar {0}")]
        [StringLength(15, ErrorMessage = "El campo {0} debe ser entre {1} and {2} caracteres", MinimumLength = 3)]
        public string Rut { get; set; }
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Debe ingresar {0}")]
        [StringLength(30, ErrorMessage = "El campo {0} debe ser entre {1} and {2} caracteres", MinimumLength = 3)]
        public string Nombre { get; set; }
        [Display(Name = "Ubicacion")]
        [Required(ErrorMessage = "Debe ingresar {0}")]
        [StringLength(50, ErrorMessage = "El campo {0} debe ser entre {1} and {2} caracteres", MinimumLength = 3)]

        public string Ubicacion { get; set; }
        [Display(Name = "Url")]
        [Url]
        [Required(ErrorMessage = "Debe ingresar {0}")]
        [StringLength(100, ErrorMessage = "El campo {0} debe ser entre {1} and {2} caracteres", MinimumLength = 3)]
        public string Url { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Debe ingresar {0}")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessage = "El campo {0} debe ser entre {1} and {2} caracteres", MinimumLength = 3)]
        public string Email { get; set; }
        [Display(Name = "Logo")]
        public string Logo { get; set; }
        [NotMapped]
        public string Acciones { get { return Acciones; } }

        public virtual ICollection<Usuario> Usuarios { get; set; }

    }
}