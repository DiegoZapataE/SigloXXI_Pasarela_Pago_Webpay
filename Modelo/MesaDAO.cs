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
    public class MesaDAO
    {
        Conexion c = new Conexion();
        public Mesa TraerMesa(int rut)
        {
            Mesa o = new Mesa();
            try
            {
                using (OracleConnection con = new OracleConnection(c.qcon))
                {
                    OracleCommand cmd = new OracleCommand();
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = "PKG_MESAS_WEB.BUSCAR_POR_RUT_PASARELA";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("v_clientes_rut_cliente", OracleDbType.Int32).Value = rut;
                    cmd.Parameters.Add("v_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    OracleDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        o.Id_Mesa = reader.GetInt32(0);
                        o.Estado_Mesa_Id_Estado_Mesa = reader.GetInt32(1);
                        o.Clientes_Rut_Cliente = reader.GetInt32(2);
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
        public bool ActualizarMesa(int id)
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
                    cmd.CommandText = "PKG_MESAS_WEB.DEJAR_MESA_LIBRE";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("v_id_mesa", OracleDbType.Int32).Value = id;
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
    }
}
