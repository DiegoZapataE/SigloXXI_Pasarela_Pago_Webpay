using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class Conexion
    {
        //Base de datos local
        public string qcon = "Data source =localhost:1521/xe; password = 1234; user id = SIGLOXXI; ";
        public OracleConnection con = new OracleConnection("DATA SOURCE = localhost:1521/xe ; PASSWORD = 1234 ; USER ID = SIGLOXXI");

        //Base de datos remota
        //public string qcon = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=34.176.185.52)(PORT=1521)))(CONNECT_DATA=(SID=ORA12C)));User ID=SIGLOXXI;Password=Duoc.1234";
        //public OracleConnection con = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=34.176.185.52)(PORT=1521)))(CONNECT_DATA=(SID=ORA12C)));User ID=SIGLOXXI;Password=Duoc.1234");
    }
}