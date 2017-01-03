using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Usuario
    {
        public int Id { get; set; }

        //[Required]
        //[Display(Name = "Nombre Usuario")]
        public string NombreUsuario { get; set; }

        //[Required]
        //[Display(Name = "Password")]
        //[DataType(DataType.Password)]
        public string Password { get; set; }

        public int IdPedidoEnConstruccion { get; set; }
    }
}
