using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ET;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace ProyectoWeb.ViewModel.ClienteViewModel
{
    public class LoginViewModel
    {
        [Display(Name = "Nombre Usuario")]
        public String NombreUsuario { get; set; }
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public String Password { get; set; }
        public String Mensaje { get; set; }
    }
}