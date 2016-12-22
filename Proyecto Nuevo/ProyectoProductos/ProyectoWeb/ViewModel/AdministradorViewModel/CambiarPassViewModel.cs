using ET;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWeb.ViewModel.AdministradorViewModel
{
    public class CambiarPassViewModel
    {
        [Display(Name = "Nombre Usuario")]
        public String NombreUsuario { get; set; }

        public List<Administrador> administradores { get; set; }

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
