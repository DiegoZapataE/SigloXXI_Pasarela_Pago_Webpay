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
    public class ClienteDAO
    {
        Conexion c = new Conexion();
        public Cliente TraerCliente(int rut)
        {
            Cliente cli = new Cliente();
            try
            {
                using (OracleConnection con = new OracleConnection(c.qcon))
                {
                    OracleCommand cmd = new OracleCommand();
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = "PKG_CLIENTES.BUSCAR";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("v_rut_cliente", OracleDbType.Int32).Value = rut;
                    cmd.Parameters.Add("v_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    OracleDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        cli.Rut_Cliente = reader.GetInt32(0);
                        cli.Dv_Cliente = reader.GetString(1);
                        cli.Clave_Cliente = reader.GetString(2);
                        cli.Nombre_Cliente = reader.GetString(3);
                        cli.PApellido_Cliente = reader.GetString(4);
                        cli.SApellido_Cliente = reader.GetString(5);
                        cli.Telefono_Cliente = reader.GetInt32(6);
                        cli.Direccion_Cliente = reader.GetString(7);
                        cli.Email_Cliente = reader.GetString(8);
                    }
                    con.Close();
                    reader.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            return cli;
        }
    }
}
