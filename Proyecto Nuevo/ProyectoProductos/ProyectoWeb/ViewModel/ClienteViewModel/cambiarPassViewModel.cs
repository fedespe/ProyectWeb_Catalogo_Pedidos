using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ET;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace ProyectoWeb.ViewModel.ClienteViewModel
{
    public class CambiarPassViewModel
    {
        public int Id { get; set; }
        public Cliente Cliente { get; set; }
        [Display(Name = "Nombre Usuario")]
        public String NombreUsuario { get; set; }
        [Display(Name = "Contraseña Actual")]
        [DataType(DataType.Password)]
        [Required]
        public String PasswordActual { get; set; }
        [Display(Name = "Contraseña Nueva")]
        [DataType(DataType.Password)]
        [Required]
        public String PasswordNuevo { get; set; }
        [Display(Name = "Confirmar Contraseña")]
        [DataType(DataType.Password)]
        [Required]
        public String PasswordConfirmacion { get; set; }
        public String Mensaje { get; set; }
    }
}