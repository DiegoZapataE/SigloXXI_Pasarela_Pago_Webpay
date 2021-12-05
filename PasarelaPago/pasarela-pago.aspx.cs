using Modelo;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Transbank.Webpay;

namespace PasarelaPago
{
    public partial class pasarela_pago : Page
    {
        //Mensaje
        private string message;

        //Código de tarjeta de prueba
        /*  
            Tarjeta: VISA
            Número: 4051885600446623
            Fecha de Expiración: Cualquiera
            CVV: 123
            RUT autenticación con emisor: 11.111.111-1
            Contraseña autenticación con emisor: 123
        */

        //Diccionario con los datos de la solicitud
        private Dictionary<string, string> request = new Dictionary<string, string>();
        protected void Page_Load()
        {
            var configuration = Configuration.ForTestingWebpayPlusNormal();
            var transaction = new Webpay(configuration);
            string data1 = "";
            string data2 = "";
            //Información del host
            var httpHost = HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString();
            var selfURL = HttpContext.Current.Request.ServerVariables["URL"].ToString();
            string action = !String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["action"]) ? HttpContext.Current.Request.QueryString["action"] : "init";

            //Url de inicio pasa pasar boleta y rut
            string sample_baseurl = "http://" + httpHost + selfURL + "?boleta=" + data1 + "&rut=" + data2;

            //Url de la aplicación
            string result_url = "http://" + httpHost + selfURL;

            //Diccionario con descripción
            var description = new Dictionary<string, string>
            {
                { "VD", "Venta Débito" },
                { "VN", "Venta Normal" },
                { "VC", "Venta en Cuotas" },
                { "SI", "Cuotas sin Interés" },
                { "S2", "2 cuotas sin Interés" },
                { "NC", "N cuotas sin Interés" }
            };

            /** Crea Dictionary con codigos de resultado */
            var codes = new Dictionary<string, string>
            {
                { "0", "Transacción aprobada" },
                { "-1", "Rechazo de transacción" },
                { "-2", "Transacción debe reintentarse" },
                { "-3", "Error en transacción" },
                { "-4", "Rechazo de transacción" },
                { "-5", "Rechazo por error de tasa" },
                { "-6", "Excede cupo máximo mensual" },
                { "-7", "Excede límite diario por transacción" },
                { "-8", "Rubro no autorizado" }
            };
            
            string buyOrder;
            int boleta;
            string rut;

            switch (action)
            {
                default:
                    try
                    {
                        var random = new Random();
                        BoletaDAO bdao = new BoletaDAO();

                        boleta = int.Parse(Request.QueryString["boleta"]);
                        rut = Request.QueryString["rut"];

                        decimal amount = Convert.ToDecimal(bdao.TraerBoleta(boleta).Valor_Boleta);
                        buyOrder = boleta.ToString();
                        string sessionId = random.Next(0, 1000).ToString();
                        string urlReturn = result_url + "?boleta="+ boleta + "&rut="+ rut + "&action=result";
                        string urlFinal = result_url + "?action=end";

                        request.Add("amount", amount.ToString());
                        request.Add("buyOrder", buyOrder.ToString());
                        request.Add("sessionId", sessionId.ToString());
                        request.Add("urlReturn", urlReturn.ToString());
                        request.Add("urlFinal", urlFinal.ToString());

                        var result = transaction.NormalTransaction.initTransaction(amount, buyOrder, sessionId, urlReturn, urlFinal);

                        if (result.token != null && result.token != "")
                        {
                            message = "Sesión iniciada con éxito en Webpay.";
                        }
                        else
                        {
                            message = "Plataforma Webpay no disponible.";
                        }
                        HttpContext.Current.Response.Write("<p hidden style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p hidden style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result) + "</p>");
                        HttpContext.Current.Response.Write("<div class='principal'>");
                        HttpContext.Current.Response.Write("<div class='login-box'>");
                        HttpContext.Current.Response.Write("<div class='login-logo'>");
                        HttpContext.Current.Response.Write("<div class='login-box-body'>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%;'><strong>Número de orden: </strong>" + boleta.ToString() + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%;'><strong>Monto: </strong>" + amount.ToString() + "</p>");
                        HttpContext.Current.Response.Write("" + message + "</br></br>");
                        HttpContext.Current.Response.Write("<form action=" + result.url + " method='post'><input type='hidden' name='token_ws' value=" + result.token + "><input type='submit' value='Pagar'></form> <a href='http://localhost:8080/SigloXXI_Web_Carro/menu_comprar.jsp'><strong>Volver al inicio</strong></a>");
                        HttpContext.Current.Response.Write("</div>");
                        HttpContext.Current.Response.Write("</div>");
                        HttpContext.Current.Response.Write("</div>");
                        HttpContext.Current.Response.Write("</div>");

                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Response.Write("<p hidden style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%;'><strong>Error:</strong></br><strong>No se encontró una boleta pendiente de pago. Por favor comunicarlo a recepción. </strong>"+ ex.Message + "</p>  <a href='http://localhost:8080/SigloXXI_Web_Carro/menu_comprar.jsp'><strong>Volver al inicio</strong></a>");
                    }
                    break;

                case "result":
                    try
                    {
                        //Obtiene la llave a través del método POST
                        string[] keysPost = Request.Form.AllKeys;
                        boleta = int.Parse(Request.QueryString["boleta"]);
                        rut = Request.QueryString["rut"];
                        //Token
                        string token = Request.Form["token_ws"];
                        request.Add("token", token.ToString());
                        var result = transaction.NormalTransaction.getTransactionResult(token);

                        HttpContext.Current.Response.Write("<p hidden style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br> " + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p hidden style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br> " + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result) + "</p>");

                        if (result.detailOutput[0].responseCode == 0)
                        {
                            message = "Pago aceptado por Webpay. \n" +
                                      "Presiona continuar para visualizar los datos.";
                            HttpContext.Current.Response.Write("<script>localStorage.setItem('authorizationCode', " + result.detailOutput[0].authorizationCode + ")</script>");
                            HttpContext.Current.Response.Write("<script>localStorage.setItem('commercecode', " + result.detailOutput[0].commerceCode + ")</script>");
                            HttpContext.Current.Response.Write("<script>localStorage.setItem('amount', " + result.detailOutput[0].amount + ")</script>");
                            HttpContext.Current.Response.Write("<script>localStorage.setItem('buyOrder', " + result.detailOutput[0].buyOrder + ")</script>");

                            //Llamamos a las clases
                            BoletaDAO bodao = new BoletaDAO();
                            MesaDAO mDAO = new MesaDAO();
                            ClienteDAO cDAO = new ClienteDAO();
                            TipoPagoDAO tDAO = new TipoPagoDAO();
                            EstadoBoletaDAO eDAO = new EstadoBoletaDAO();
                            EmailDAO emailDAO = new EmailDAO();
                            
                            //Actualizamos los datos de la boleta y la mesa
                            int nro_boleta = boleta;
                            int rut_boleta = bodao.TraerBoleta(nro_boleta).Clientes_Rut_Cliente;
                            bodao.ActualizarEstadoBoleta(nro_boleta, 2);
                            bodao.ActualizarTipoPago(nro_boleta);
                            int id_mesa = mDAO.TraerMesa(rut_boleta).Id_Mesa;
                            mDAO.ActualizarMesa(id_mesa);

                            //Se empieza a generar la data para el correo
                            Boleta b = new Boleta();
                            Cliente cl = new Cliente();
                            TipoPago t = new TipoPago();
                            EstadoBoleta e = new EstadoBoleta();

                            //Boleta
                            b.Id_Boleta = nro_boleta;
                            b.Detalle_Boleta = bodao.TraerBoleta(nro_boleta).Detalle_Boleta;
                            b.Valor_Boleta = bodao.TraerBoleta(nro_boleta).Valor_Boleta;
                            b.Fecha_Boleta = bodao.TraerBoleta(nro_boleta).Fecha_Boleta;
                            b.Clientes_Rut_Cliente = bodao.TraerBoleta(nro_boleta).Clientes_Rut_Cliente;
                            b.Tipo_Pago_Id_Tipo_Pago = bodao.TraerBoleta(nro_boleta).Tipo_Pago_Id_Tipo_Pago;
                            b.Estado_Boleta_Id_Estado_Boleta = bodao.TraerBoleta(nro_boleta).Estado_Boleta_Id_Estado_Boleta;

                            //Tipo de pago
                            t.Id_Tipo_Pago = bodao.TraerBoleta(nro_boleta).Tipo_Pago_Id_Tipo_Pago;
                            t.Nombre_Tipo_Pago = tDAO.TraerTipoPago(t.Id_Tipo_Pago).Nombre_Tipo_Pago;

                            //Estado boleta
                            e.Id_Estado_Boleta = bodao.TraerBoleta(nro_boleta).Estado_Boleta_Id_Estado_Boleta;
                            e.Estado_Boleta = eDAO.TraerEstadoBoleta(e.Id_Estado_Boleta).Estado_Boleta;

                            //Cliente
                            cl.Email_Cliente = cDAO.TraerCliente(rut_boleta).Email_Cliente;

                            //Enviamos el correo
                            emailDAO.EmailPagoVerificado(cl, b, e, t);

                            HttpContext.Current.Response.Write("<div class='principal'>");
                            HttpContext.Current.Response.Write("<div class='login-box'>");
                            HttpContext.Current.Response.Write("<div class='login-logo'>");
                            HttpContext.Current.Response.Write("<div class='login-box-body'>");
                            HttpContext.Current.Response.Write(message + "</br></br>");
                            HttpContext.Current.Response.Write("<form action=" + result.urlRedirection + " method='post'><input type='hidden' name='token_ws' value=" + token + "><input type='submit' value='Continuar'></form>");
                            HttpContext.Current.Response.Write("</div>");
                            HttpContext.Current.Response.Write("</div>");
                            HttpContext.Current.Response.Write("</div>");
                            HttpContext.Current.Response.Write("</div>");
                        }
                        else
                        {
                            message = "Pago rechazado por Webpay.";
                            HttpContext.Current.Response.Write("<div class='principal'>");
                            HttpContext.Current.Response.Write("<div class='login-box'>");
                            HttpContext.Current.Response.Write("<div class='login-logo'>");
                            HttpContext.Current.Response.Write("<div class='login-box-body'>");
                            HttpContext.Current.Response.Write(message + "</br></br>");
                            HttpContext.Current.Response.Write("<form action=" + result.urlRedirection + " method='post'><input type='hidden' name='token_ws' value=" + token + "><input type='submit' value='Continuar'></form>  <a href='http://localhost:8080/SigloXXI_Web_Carro/menu_comprar.jsp'><strong>Volver al inicio</strong></a>");
                            HttpContext.Current.Response.Write("</div>");
                            HttpContext.Current.Response.Write("</div>");
                            HttpContext.Current.Response.Write("</div>");
                            HttpContext.Current.Response.Write("</div>");
                        }
                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Response.Write("<p hidden style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>Resultado:</strong></br> Ocurrió un error en la transacción (Validar correcta configuración de parámetros). " + ex.Message + "</p>  <a href='http://localhost:8080/SigloXXI_Web_Carro/menu_comprar.jsp'><strong>Volver al inicio</strong></a>");
                    }
                    break;

                case "end":

                    try
                    {
                        HttpContext.Current.Response.Write("<p hidden style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        if (Request.Form["token_ws"] != null)
                        {
                            request.Add("", "");
                            HttpContext.Current.Response.Write("<p hidden style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br>[token_ws] = " + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(Request.Form["token_ws"]) + "</p>");
                            message = "Transacción finalizada con éxito.";
                            
                            HttpContext.Current.Response.Write("<script>var commercecode = localStorage.getItem('commercecode');document.getElementById('commercecode').value = commercecode;</script>");
                            HttpContext.Current.Response.Write("<script>var authorizationCode = localStorage.getItem('authorizationCode');document.getElementById('authorizationCode').value = authorizationCode;</script>");
                            HttpContext.Current.Response.Write("<script>var amount = localStorage.getItem('amount');document.getElementById('amount').value = amount;</script>");
                            HttpContext.Current.Response.Write("<script>var buyOrder = localStorage.getItem('buyOrder');document.getElementById('buyOrder').value = buyOrder;</script>");
                        }
                        else if (Request.Form["TBK_TOKEN"] != null)
                        {
                            HttpContext.Current.Response.Write("<p hidden style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br>");
                            foreach (string key in Request.Form.AllKeys)
                            {
                                HttpContext.Current.Response.Write("[" + key + "] = " + Request.Form[key] + "<br>");
                            }
                            HttpContext.Current.Response.Write("</p>");
                            message = "Transacción Abortada. Pulsa el botón volver para regresar a la ventana anterior.";
                        }
                        HttpContext.Current.Response.Write("<div class='principal'>");
                        HttpContext.Current.Response.Write("<div class='login-box'>");
                        HttpContext.Current.Response.Write("<div class='login-logo'>");
                        HttpContext.Current.Response.Write("<div class='login-box-body'>");
                        HttpContext.Current.Response.Write(message + "</br></br> <a href='http://localhost:8080/SigloXXI_Web_Carro/menu_comprar.jsp'><strong>Volver al inicio</strong></a>");
                        HttpContext.Current.Response.Write("</div>");
                        HttpContext.Current.Response.Write("</div>");
                        HttpContext.Current.Response.Write("</div>");
                        HttpContext.Current.Response.Write("</div>");
                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Response.Write("<p hidden style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>Resultado:</strong></br> Ocurrió un error en la transacción (Validar correcta configuración de parámetros). " + ex.Message + "</p>  <a href='http://localhost:8080/SigloXXI_Web_Carro/menu_comprar.jsp'><strong>Volver al inicio</strong></a>");
                    }
                    break;
            }
        }
    }
}
