using Controlador;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class Cliente
    {
        public int Rut_Cliente { get; set; }
        public string Dv_Cliente { get; set; }
        public string Clave_Cliente { get; set; }
        public string Nombre_Cliente { get; set; }
        public string PApellido_Cliente { get; set; }
        public string SApellido_Cliente { get; set; }
        public int Telefono_Cliente { get; set; }
        public string Direccion_Cliente { get; set; }
        public string Email_Cliente { get; set; }
        public int Roles_Id_Rol { get; set; }
        public string Rol { get; set; }

        public Cliente()
        {
            this.Init();
        }

        private void Init()
        {
            Rut_Cliente = 0;
            Dv_Cliente = string.Empty;
            Clave_Cliente = string.Empty;
            Nombre_Cliente = string.Empty;
            PApellido_Cliente = string.Empty;
            SApellido_Cliente = string.Empty;
            Telefono_Cliente = 0;
            Direccion_Cliente = string.Empty;
            Email_Cliente = string.Empty;
            Roles_Id_Rol = -1;
            Rol = string.Empty;
        }
    }
}
