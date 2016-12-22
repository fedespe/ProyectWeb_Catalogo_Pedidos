using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWeb.ViewModel.AccountViewModel
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
