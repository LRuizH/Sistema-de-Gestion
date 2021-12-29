
using System.Text;
using Framework.BOL;
using Framework.BLL.Utilidades.Seguridad;

namespace Framework.BLL.Utilidades.Ajax
{
    public class CLogin
    {
        AppSettings _appSettings = new AppSettings();

        /// <summary>
        /// Obtiene componente html para Login de usuarios
        /// </summary>
        public string ObtieneLogin(GesSistemaBOL _gesSistemaBOL)
        {
            string _strHtml = "";

            string _mensajeInterno = "<div class=\"alert alert-info\">" +
                                        "<h5>No tienes acceso a este sistema</h5>" +
                                        "<p>Debes contactarte con un administrador de este sistema para solicitar acceso.</p>" +
                                        "<a href=\"javascript:void(0);\" onclick=\"cierraMensaje();\">Cerrar</a>" +
                                     "</div>";

            string _mensajeError = "<div class=\"alert alert-danger\">" +
                                        "<h5>Error de autenticación</h5>" +
                                        "<p>Usuario o contraseña incorrecta.</p>" +
                                        "<a href=\"javascript:void(0);\" onclick=\"cierraMensaje();\">Cerrar</a>" +
                                   "</div>";

            // Genero funcion para autenticación de usuarios
            PostJSON _postJSON = new PostJSON();
            {
                _postJSON.P_form = "login-ine";
                _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/login/autenticar";
                _postJSON.P_respuesta_servicio = "if(respuesta[0].usu_respuesta != 0) { " +
                                                        "if (respuesta[0].usu_pagina === 'interno') { " +
                                                            "$('.mensaje').html('" + _mensajeInterno + "');" +
                                                        "} else { " +
                                                            "$('.ine-Framework').html(respuesta[0].usu_pagina);" +
                                                        "}" +
                                                 "} else { " +
                                                    "$('.mensaje').html('" + _mensajeError + "');" +
                                                 "}";
            }

            // Genero metodo submit del formulario
            CallMethod _methodCallLoad = new CallMethod
            {
                Mc_contenido = "$('#usu_id').focus();" + "$.getScript('" + _appSettings.ServidorWeb + "Framework/assets/js/plugins/iCheck/icheck.min.js', function () {" +
                                                            "$('.i-checks').iCheck({ " +
                                                                "checkboxClass: 'icheckbox_square-green'," +
                                                                "radioClass: 'iradio_square-green'," +
                                                            "});" +
                                                         "});" + _postJSON.PostJSONCall()
            };

            // Genero funcion obtieneLogin()
            GetJSON _obtieneLogin = new GetJSON();
            {
                _obtieneLogin.G_url_servicio = _appSettings.ServidorWeb + "api/acceso";
                _obtieneLogin.G_parametros = "{ sistema_id: " + _gesSistemaBOL.Sistema_id + ", sistema_token: '" + _gesSistemaBOL.Sistema_token + "' }";
                _obtieneLogin.G_respuesta_servicio = "$('.ine-Framework').html(respuesta[0].elemento_html);";
            }

            CallMethod _methodCallObtieneLogin = new CallMethod();
            _methodCallObtieneLogin.Mc_nombre = "obtieneLogin()";
            if (_gesSistemaBOL.Sistema_id == 1) // Opción temporal, se deberia tener solo un acceso (index.html)
            {
                _methodCallObtieneLogin.Mc_contenido = "history.pushState({}, '', '../index.html');" + _obtieneLogin.GetJSONCall();
            } else
            {
                _methodCallObtieneLogin.Mc_contenido = "history.pushState({}, '', '../index-empresas.html');" + _obtieneLogin.GetJSONCall();
            }

            // Genero funcion cierraMensaje()
            CallMethod _methodCallCierraMensaje = new CallMethod
            {
                Mc_nombre = "cierraMensaje()",
                Mc_contenido = "$('.mensaje').empty();"
            };

            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=\"ibox-content\">");
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\">");
            sb.Append("<div class=\"\" >");
            sb.Append("<h1 class=\"\"><small>" + _gesSistemaBOL.Proyecto_nombre + "</small></h1>");
            sb.Append("<h2 class=\"\">" + _gesSistemaBOL.Sistema_nombre + "</h2>");
            sb.Append("<input id=\"sistema_id\" name=\"sistema_id\" type=\"hidden\" value=\"" + _gesSistemaBOL.Sistema_id + "\" />");
            sb.Append("<input id=\"sistema_token\" name=\"sistema_token\" type=\"hidden\" value=\"" + _gesSistemaBOL.Sistema_token + "\" />");
            sb.Append("<div class=\"panel\">");
            sb.Append("<br />");
            sb.Append("<h3><small>Ingresa tus datos de acceso</small></h3>");
            sb.Append("<div class=\"hr-line-dashed\"></div>");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<input id=\"usu_id\" name=\"usu_id\" type=\"text\" class=\"form-control\" placeholder=\"Usuario\" required autofocus pattern=\"([0-9]{7,8}-[0-9Kk]{1})|(admin)\" title=\"RUT sin puntos y con guion. Ej: 12345678-0\" />");
            sb.Append("</div>");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<input id=\"usu_contrasena\" name=\"usu_contrasena\" type=\"password\" class=\"form-control\" placeholder=\"Contraseña\">");
            sb.Append("</div>");
            sb.Append("<div class=\"row text-left\">");
            sb.Append("<div class=\"col-sm-6 col-xs-12\">");
            sb.Append("<label>");
            sb.Append("<div class=\"\" style=\"\">");
            sb.Append("<input id=\"recuerda_contrasena\" name=\"recuerda_contrasena\" class=\"i-checks\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
            sb.Append("<span class=\"text-inverse\"> Recuérdame</span>");
            sb.Append("</div>");
            sb.Append("</label>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("<div class=\"row m-t-30\">");
            sb.Append("<div class=\"col-md-12 text-center\">");
            sb.Append("<button type=\"submit\" class=\"btn btn-primary block full-width m-b\"><i class=\"fa fa-sign-in\"></i> ENTRAR</button>");
            sb.Append("<div class=\"mensaje\"></div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</form>");
            sb.Append("</div>");
            sb.Append(_methodCallLoad.CreaJQueryDocumentReady());
            sb.Append(_methodCallObtieneLogin.CreaJQueryFunction());
            sb.Append(_methodCallCierraMensaje.CreaJQueryFunction());

            _strHtml = sb.ToString();

            return _strHtml;
        }

        /// <summary>
        /// Obtiene componente html para Login de acceso directo a cuestionario
        /// </summary>
        public string ObtieneLoginAccesoCuestionario(GesSistemaBOL _gesSistemaBOL)
        {
            string _strHtml = "";

            string _mensajeError = "<div class=\"alert alert-danger\">" +
                                        "<h5>Error de información</h5>" +
                                        "<p>No se ha encontrado un cuestionario para el dato ingresado.</p>" +
                                        "<a href=\"javascript:void(0);\" onclick=\"cierraMensaje();\">Cerrar</a>" +
                                   "</div>";

            // Genero funcion para autenticación de usuarios
            PostJSON _postJSON = new PostJSON();
            {
                _postJSON.P_form = "login-ine";
                _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/login/autenticar-por-cuestionario";
                _postJSON.P_respuesta_servicio = "if(respuesta[0].usu_respuesta != 0) { " +
                                                    "$('.ine-Framework').html(respuesta[0].usu_pagina);" +
                                                 "} else { " +
                                                    "$('.mensaje').html('" + _mensajeError + "');" +
                                                 "}";
            }

            // Genero metodo submit del formulario
            CallMethod _methodCallLoad = new CallMethod
            {
                Mc_contenido = "$('#usu_id').focus();" + _postJSON.PostJSONCall() + "AccesoDirecto();"
            };

            // Genero funcion cierraMensaje()
            CallMethod _methodCallCierraMensaje = new CallMethod
            {
                Mc_nombre = "cierraMensaje()",
                Mc_contenido = "$('.mensaje').empty();"
            };

            // Genero funcion AccesoDirecto()
            CallMethod _methodCallAccesoDirecto = new CallMethod
            {
                Mc_nombre = "AccesoDirecto()",
                Mc_contenido = "var token_valor = obtieneParametrosURL('id'); if (token_valor != '') { $('#usu_id').val(token_valor); $('#login-ine .btn-success').trigger('click'); }"
            };

            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=\"ibox-content\">");
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\">");
            sb.Append("<div class=\"\" >");
            sb.Append("<h1 class=\"\"><small>" + _gesSistemaBOL.Proyecto_nombre + "</small></h1>");
            sb.Append("<h2 class=\"\">" + _gesSistemaBOL.Sistema_nombre + "</h2>");
            sb.Append("<input id=\"sistema_id\" name=\"sistema_id\" type=\"hidden\" value=\"" + _gesSistemaBOL.Sistema_id + "\" />");
            sb.Append("<input id=\"sistema_token\" name=\"sistema_token\" type=\"hidden\" value=\"" + _gesSistemaBOL.Sistema_token + "\" />");
            sb.Append("<div class=\"panel\">");
            sb.Append("<br />");
            sb.Append("<h3>Ingresa el N° único de vivienda</h3>");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<input id=\"usu_id\" name=\"usu_id\" type=\"text\" class=\"form-control\" placeholder=\"N° único de vivienda\" required autofocus />");
            sb.Append("</div>");
            sb.Append("<div class=\"row m-t-30\">");
            sb.Append("<div class=\"col-md-12 text-center\">");
            sb.Append("<button type=\"submit\" class=\"btn btn-success block full-width m-b\"><i class=\"fa fa-search\"></i> BUSCAR CUESTIONARIO</button>");
            sb.Append("<div class=\"mensaje\"></div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</form>");
            sb.Append("</div>");
            sb.Append(_methodCallLoad.CreaJQueryDocumentReady());
            sb.Append(_methodCallAccesoDirecto.CreaJQueryFunction());
            sb.Append(_methodCallCierraMensaje.CreaJQueryFunction());

            _strHtml = sb.ToString();

            return _strHtml;
        }

        /// <summary>
        /// Obtiene componente html para Login de acceso directo a cuestionario telefonico
        /// </summary>
        public string ObtieneLoginAccesoCuestionarioTelefonico(GesSistemaBOL _gesSistemaBOL)
        {
            string _strHtml = "";

            string _mensajeError = "<div class=\"alert alert-danger\">" +
                                        "<h5>Error de información</h5>" +
                                        "<p>No se ha encontrado un registro para el dato ingresado.</p>" +
                                        "<a href=\"javascript:void(0);\" onclick=\"cierraMensaje();\">Cerrar</a>" +
                                   "</div>";

            // Genero funcion para autenticación de usuarios
            PostJSON _postJSON = new PostJSON();
            {
                _postJSON.P_form = "login-ine";
                _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/login/autenticar-por-id-telefonico";
                _postJSON.P_respuesta_servicio = "if(respuesta[0].usu_respuesta != 0) { " +
                                                    "$('.ine-Framework').html(respuesta[0].usu_pagina);" +
                                                 "} else { " +
                                                    "$('.mensaje').html('" + _mensajeError + "');" +
                                                 "}";
            }

            // Genero metodo submit del formulario
            CallMethod _methodCallLoad = new CallMethod
            {
                Mc_contenido = "$('#usu_id').focus();" + _postJSON.PostJSONCall()
            };

            // Genero funcion cierraMensaje()
            CallMethod _methodCallCierraMensaje = new CallMethod
            {
                Mc_nombre = "cierraMensaje()",
                Mc_contenido = "$('.mensaje').empty();"
            };

            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=\"ibox-content\">");
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\">");
            sb.Append("<div class=\"\" >");
            sb.Append("<h1 class=\"\"><small>" + _gesSistemaBOL.Proyecto_nombre + "</small></h1>");
            sb.Append("<h2 class=\"\">" + _gesSistemaBOL.Sistema_nombre + "</h2>");
            sb.Append("<input id=\"sistema_id\" name=\"sistema_id\" type=\"hidden\" value=\"" + _gesSistemaBOL.Sistema_id + "\" />");
            sb.Append("<input id=\"sistema_token\" name=\"sistema_token\" type=\"hidden\" value=\"" + _gesSistemaBOL.Sistema_token + "\" />");
            sb.Append("<div class=\"panel\">");
            sb.Append("<br />");
            sb.Append("<h3>Ingresa el ID de Registro</h3>");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<input id=\"usu_id\" name=\"usu_id\" type=\"text\" class=\"form-control\" placeholder=\"ID de Registro\" required autofocus />");
            sb.Append("</div>");
            sb.Append("<div class=\"row m-t-30\">");
            sb.Append("<div class=\"col-md-12 text-center\">");
            sb.Append("<button type=\"submit\" class=\"btn btn-success block full-width m-b\"><i class=\"fa fa-search\"></i> INGRESAR </button>");
            sb.Append("<div class=\"mensaje\"></div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</form>");
            sb.Append("</div>");
            sb.Append(_methodCallLoad.CreaJQueryDocumentReady());
            sb.Append(_methodCallCierraMensaje.CreaJQueryFunction());

            _strHtml = sb.ToString();

            return _strHtml;
        }
    }
}
