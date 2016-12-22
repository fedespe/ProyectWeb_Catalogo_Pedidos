using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWeb.ViewModel.AdministradorViewModel
{
    public class EditarViewModel
    {
        public ET.Administrador administrador { get; set; }

        [Required]
        [Display(Name = "Nombre Usuario")]
        public string NombreUsuario { get; set; }

        public EditarViewModel()
        {
            this.administrador = new ET.Administrador();
        }
        public void completarEditarVM()
        {
            NombreUsuario = administrador.NombreUsuario;
        }

        public void completarAdministrador()
        {
            administrador.NombreUsuario = NombreUsuario;
        }
    }
}
