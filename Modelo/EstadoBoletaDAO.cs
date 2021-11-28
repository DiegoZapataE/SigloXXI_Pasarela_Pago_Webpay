using Controlador;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class EstadoBoletaDAO
    {
        Conexion c = new Conexion();

        public List<EstadoBoleta> ListarEstadoBoletas()
        {
            List<EstadoBoleta> lista = new List<EstadoBoleta>();
            try
            {
                using (OracleConnection con = new OracleConnection(c.qcon))
                {
                    OracleCommand cmd = new OracleCommand();
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = "PKG_ESTADO_BOLETA.BUSCAR";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("v_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    OracleDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        EstadoBoleta o = new EstadoBoleta();
                        o.Id_Estado_Boleta = reader.GetInt32(0);
                        o.Estado_Boleta = reader.GetString(1);
                        lista.Add(o);
                    }
                    con.Close();
                    reader.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            return lista;
        }
        public EstadoBoleta TraerEstadoBoleta(int id)
        {
            EstadoBoleta u = new EstadoBoleta();
            try
            {
                using (OracleConnection con = new OracleConnection(c.qcon))
                {
                    OracleCommand cmd = new OracleCommand();
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = "PKG_ESTADO_BOLETA.BUSCAR";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("v_id_estado_boleta", OracleDbType.Int32).Value = id;
                    cmd.Parameters.Add("v_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    OracleDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        u.Id_Estado_Boleta = reader.GetInt32(0);
                        u.Estado_Boleta = reader.GetString(1);
                    }
                    con.Close();
                    reader.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            return u;
        }
    }
}
