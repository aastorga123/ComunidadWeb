using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiComunidad.Models
{
    public class Programa
    {
        [key]
        public int ProgramaID { get; set; }
        [Display(Name = "Programa")]
        [Required(ErrorMessage = "Debe ingresar {0}")]
        [StringLength(30, ErrorMessage = "El campo {0} debe ser entre {1} and {2} caracteres", MinimumLength = 3)]
        public string NombrePrograma { get; set; }
        [Display(Name = "Descripción")]
        [DataType(DataType.MultilineText)]
        [StringLength(200, ErrorMessage = "El campo {0} debe ser entre {1} and {2} caracteres", MinimumLength = 0)]
        public string DescripcionPrograma { get; set; }

    }
}