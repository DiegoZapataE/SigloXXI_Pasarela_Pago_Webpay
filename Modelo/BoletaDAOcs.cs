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
    public class BoletaDAO
    {
        Conexion c = new Conexion();

        public Boleta TraerBoleta(int id)
        {
            Boleta o = new Boleta();
            try
            {
                using (OracleConnection con = new OracleConnection(c.qcon))
                {
                    OracleCommand cmd = new OracleCommand();
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = "PKG_BOLETAS.BUSCAR";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("v_id_boleta", OracleDbType.Int32).Value = id;
                    cmd.Parameters.Add("v_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    OracleDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        o.Id_Boleta = reader.GetInt32(0);
                        o.Detalle_Boleta = reader.GetString(1);
                        o.Valor_Boleta = reader.GetInt32(2);
                        o.Fecha_Boleta = reader.GetDateTime(3);
                        o.Clientes_Rut_Cliente = reader.GetInt32(4);
                        o.Tipo_Pago_Id_Tipo_Pago = reader.GetInt32(5);
                        o.Estado_Boleta_Id_Estado_Boleta = reader.GetInt32(6);
                    }
                    con.Close();
                    reader.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            return o;
        }

        public bool ActualizarEstadoBoleta(int id, int estado)
        {
            bool resultado = false;
            int verificar = 0;
            try
            {
                using (OracleConnection con = new OracleConnection(c.qcon))
                {
                    OracleCommand cmd = new OracleCommand();
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = "PKG_BOLETAS.ACTUALIZAR_ESTADO_BOLETA";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("v_id_boleta", OracleDbType.Int32).Value = id;
                    cmd.Parameters.Add("v_estado_boleta_id_estado_boleta", OracleDbType.Int32).Value = estado;

                    verificar = cmd.ExecuteNonQuery();

                    if (verificar != 0)
                    {
                        resultado = true;
                    }
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            return resultado;
        }
        public Boleta TraerUltimaBoleta()
        {
            Boleta o = new Boleta();
            try
            {
                using (OracleConnection con = new OracleConnection(c.qcon))
                {
                    OracleCommand cmd = new OracleCommand();
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = "PKG_BOLETAS.BUSCAR_ULTIMA_BOLETA";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("v_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    OracleDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        o.Id_Boleta = reader.GetInt32(0);
                    }
                    con.Close();
                    reader.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            return o;
        }
        public int TraerValorTotalPedidosProveedores()
        {
            int total = 0;
            try
            {
                using (OracleConnection con = new OracleConnection(c.qcon))
                {
                    OracleCommand cmd = new OracleCommand();
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = "PKG_PEDIDO_PROVEEDOR.VALOR_TOTAL";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("v_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    OracleDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        total = reader.GetInt32(0);
                    }
                    con.Close();
                    reader.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            return total;
        }
    }
}
