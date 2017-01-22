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
        public string NombreUsuario { get; set; }
        [Display(Name = "Contraseña Actual")]
        [DataType(DataType.Password)]
        public string PasswordActual { get; set; }
        [Display(Name = "Contraseña Nueva")]
        [DataType(DataType.Password)]
        [Required]
        public string PasswordNuevo { get; set; }
        [Display(Name = "Confirmar Contraseña")]
        [DataType(DataType.Password)]
        [Required]
        public string PasswordConfirmacion { get; set; }
        public string Mensaje { get; set; }
    }
}