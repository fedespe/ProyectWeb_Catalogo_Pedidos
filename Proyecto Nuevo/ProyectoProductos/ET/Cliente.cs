﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Cliente : Usuario
    {
        public string NombreFantasia { get; set; }

        public string Rut { get; set; }

        public string RazonSocial { get; set; }

        public double Descuento { get; set; }

        //[Display(Name = "Días de pago")]
        public string DiasDePago { get; set; }

        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public string NombreDeContacto { get; set; }

        public string TelefonoDeContacto { get; set; }

        public string EmailDeContacto { get; set; }

        public string Foto { get; set; }

        public List<Pedido> Pedidos { get; set; }
    }
}
