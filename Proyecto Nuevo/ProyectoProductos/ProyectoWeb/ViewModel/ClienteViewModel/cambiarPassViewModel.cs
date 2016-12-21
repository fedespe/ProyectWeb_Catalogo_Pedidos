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
        [Display(Name = "Nombre Usuario")]
        public String NombreUsuario { get; set; }
        [Display(Name = "Contraseña Actual")]
        [DataType(DataType.Password)]
        public String PasswordActual { get; set; }
        [Display(Name = "Contraseña Nueva")]
        [DataType(DataType.Password)]
        public String PasswordNuevo { get; set; }
        [Display(Name = "Confirmar Contraseña")]
        [DataType(DataType.Password)]
        public String PasswordConfirmacion { get; set; }
        public String Mensaje { get; set; }
    }
}