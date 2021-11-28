using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class EstadoBoleta
    {
        public int Id_Estado_Boleta { get; set; }
        public string Estado_Boleta { get; set; }

        public EstadoBoleta()
        {
            this.Init();
        }

        private void Init()
        {
            Id_Estado_Boleta = 0;
            Estado_Boleta = string.Empty;
        }

    }
}
