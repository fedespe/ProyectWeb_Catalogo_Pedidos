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
        public int Id { get; set; }
        [Display(Name = "Nombre Usuario")]
        public String NombreUsuario { get; set; }

        public Administrador Administrador { get; set; }

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
