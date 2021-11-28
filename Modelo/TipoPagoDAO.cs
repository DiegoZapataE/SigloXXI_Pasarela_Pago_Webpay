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
    public class TipoPagoDAO
    {
        Conexion c = new Conexion();

        public List<TipoPago> ListarTiposDePago()
        {
            List<TipoPago> lista = new List<TipoPago>();
            try
            {
                using (OracleConnection con = new OracleConnection(c.qcon))
                {
                    OracleCommand cmd = new OracleCommand();
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = "PKG_TIPO_PAGO.BUSCAR_TODO";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("v_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    OracleDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        TipoPago tp = new TipoPago();
                        tp.Id_Tipo_Pago = reader.GetInt32(0);
                        tp.Nombre_Tipo_Pago = reader.GetString(1);
                        lista.Add(tp);
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

        public TipoPago TraerTipoPago(int id)
        {
            TipoPago u = new TipoPago();
            try
            {
                using (OracleConnection con = new OracleConnection(c.qcon))
                {
                    OracleCommand cmd = new OracleCommand();
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = "PKG_TIPO_PAGO.BUSCAR";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("v_id_tipo_pago", OracleDbType.Int32).Value = id;
                    cmd.Parameters.Add("v_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    OracleDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        u.Id_Tipo_Pago = reader.GetInt32(0);
                        u.Nombre_Tipo_Pago = reader.GetString(1);
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
