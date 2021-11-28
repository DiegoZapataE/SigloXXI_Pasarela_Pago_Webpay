using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class TipoPago
    {
        public int Id_Tipo_Pago { get; set; }
        public string Nombre_Tipo_Pago { get; set; }

        public TipoPago()
        {
            this.Init();
        }

        private void Init()
        {
            Id_Tipo_Pago = 0;
            Nombre_Tipo_Pago = string.Empty;
        }
    }
}
