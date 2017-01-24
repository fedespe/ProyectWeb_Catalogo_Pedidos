using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWeb.ViewModel.AdministradorViewModel
{
    public class CrearViewModel
    {
        public ET.Administrador administrador { get; set; }

        [Required]
        [Display(Name = "Nombre Usuario")]
        public string NombreUsuario { get; set; }

        [Required]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public CrearViewModel()
        {
            this.administrador = new ET.Administrador();
        }

        public void completarAdministrador()
        {
            administrador.NombreUsuario = NombreUsuario;
            administrador.Password = Password;
        }
    }
}
