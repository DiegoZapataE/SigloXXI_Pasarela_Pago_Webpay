using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class Mesa
    {
        public int Id_Mesa { get; set; }
        public int Estado_Mesa_Id_Estado_Mesa { get; set; }
        public int Clientes_Rut_Cliente { get; set; }


        public Mesa()
        {
            this.Init();
        }

        private void Init()
        {
            Id_Mesa = 0;
            Estado_Mesa_Id_Estado_Mesa = 0;
            Clientes_Rut_Cliente = 0;
        }
    }
}