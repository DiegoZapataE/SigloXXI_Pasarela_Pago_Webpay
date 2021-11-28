using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class Boleta
    {
        public int Id_Boleta { get; set; }
        public string Detalle_Boleta { get; set; }
        public int Valor_Boleta { get; set; }
        public DateTime Fecha_Boleta { get; set; }
        public int Clientes_Rut_Cliente { get; set; }
        public int Tipo_Pago_Id_Tipo_Pago { get; set; }
        public int Estado_Boleta_Id_Estado_Boleta { get; set; }

        public Boleta()
        {
            this.Init();
        }

        private void Init()
        {
            Id_Boleta = 0;
            Detalle_Boleta = string.Empty;
            Valor_Boleta = 0;
            Tipo_Pago_Id_Tipo_Pago = 0;
            Estado_Boleta_Id_Estado_Boleta = 0;
        }
    }
}
