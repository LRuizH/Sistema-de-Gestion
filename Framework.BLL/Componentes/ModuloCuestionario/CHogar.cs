using Framework.BLL.Utilidades.Ajax;
using Framework.BLL.Utilidades.Encriptacion;
using Framework.BLL.Utilidades.Seguridad;
using Framework.BOL;
using Framework.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Framework.BLL.Componentes.ModuloCuestionario
{
    public class CHogar
    {
        AppSettings _appSettings = new AppSettings();

        /// <summary>
        /// Obtiene Resúmen Sección Hogar
        /// </summary>
        public string ObtieneResumenSeccionHogar(string token, int paso, string cuestionario = "")
        {
            string _idFormulario = "";
            StringBuilder sb = new StringBuilder();
            StringBuilder sbTabla = new StringBuilder();
            StringBuilder sbDetalle = new StringBuilder();

            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Hogar
            HogarBOL _hogarBOL = new HogarBOL();
            HogarDAL _hogarDAL = new HogarDAL();

            _hogarBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            List<HogarBOL> listaResumenHogar = _hogarDAL.ListarPorVivienda<HogarBOL>(_hogarBOL);

            if (listaResumenHogar.Count > 0)
            {
                Encrypt _encrypt = new Encrypt();

                // Obtengo listado de hogares
                foreach (var item in listaResumenHogar)
                {
                    _idFormulario = _encrypt.EncryptString(_hogarBOL.PK_VIVIENDA + "&" + "" + item.PK_HOGAR + "&0");

                    sbDetalle.Append("<tr>");
                    sbDetalle.Append("<td><button onclick=\"obtieneCuestionarioWeb(5,'" + _idFormulario + "'); localStorage.setItem('NumHogar', " + item.H_NHOG_VISUAL + ");\" class=\"btn btn-primary btn-block\">Hogar " + item.H_NHOG_VISUAL + "</button></td>");
                    if (item.HOG_ESTADO == "1")
                    {
                        sbDetalle.Append("<td><p style=\"color:#8bc34a\"><i class=\"fa fa-handshake-o\"></i> Completo</p></td>");
                    }
                    else
                    {
                        sbDetalle.Append("<td><p class=\"text-warning\"><i class=\"fa fa-info\"></i> Pendiente</p></td>");
                    }

                    sbDetalle.Append("<td><button type=\"button\" onclick=\"ObtieneEliminacionHogar('" + _idFormulario + "');\" class=\"btn btn-danger\" data-toggle=\"modal\" data-target=\"#modal-master\"><i class=\"fa fa-trash\"></i></button></td>");
                    sbDetalle.Append("</tr>");
                }

                sbTabla.Append("<div class=\"row\">");
                sbTabla.Append("<div class=\"col-lg-12 text-center\">");
                if (listaResumenHogar.Count == 1) {
                    sbTabla.Append("<h1>SELECCIONE E INGRESE LA INFORMACIÓN DEL HOGAR</h1>");
                } else {
                    sbTabla.Append("<h1>SELECCIONE E INGRESE LA INFORMACIÓN DE CADA UNO DE LOS HOGARES IDENTIFICADOS</h1>");
                }
                sbTabla.Append("<hr />");
                sbTabla.Append("</div>");
                sbTabla.Append("</div>");

                sbTabla.Append("<link rel=\"stylesheet\" href='" + _appSettings.ServidorWeb + "Framework/assets/css/plugins/dataTables/datatables.min.css'>");
                sbTabla.Append("<div class=\"row\">");
                sbTabla.Append("<div class=\"table-responsive\">");
                sbTabla.Append("<table class=\"tabla-informante table table-striped text-center\">");
                sbTabla.Append("<thead>");
                sbTabla.Append("<tr>");
                sbTabla.Append("<th>HOGAR</th>");
                sbTabla.Append("<th>ESTADO</th>");
                sbTabla.Append("<th>ELIMINAR</th>");
                sbTabla.Append("</tr>");
                sbTabla.Append("</thead>");
                sbTabla.Append("<tbody>");
                sbTabla.Append(sbDetalle.ToString());
                sbTabla.Append("</tbody>");
                sbTabla.Append("</table>");
                sbTabla.Append("</div>");
                sbTabla.Append("</div>");

                sbTabla.Append("<div class=\"row\">");
                sbTabla.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
                sbTabla.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso - 1) + ",'" + token + "');\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
                sbTabla.Append("</div>");
                sbTabla.Append("</div>");
            }
            else
            {
                sbTabla.Append("<div class=\"row\">");
                sb.Append("<h1>Error. La cantidad de hogares debe ser mayor a 0.</h1><button class=\"btn btn-default btn-md btn-block\" onclick=\"obtieneCuestionarioWeb(" + (paso - 1) + ",'" + token + "');\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
                sbTabla.Append("</div>");
            }

            sb.Append(sbTabla.ToString());


            // Genero función para el eliminación de hogar
            GetJSON _getJSONObtieneEliminacionHogar = new GetJSON
            {
                G_url_servicio = _appSettings.ServidorWeb + "api/hogar/obtiene-eliminacion-hogar",
                G_parametros = "{ token: '" + _idFormulario + "' }",
                G_respuesta_servicio = "$('.contenido-generico').html(respuesta[0].elemento_html);"
            };

            CallMethod _methodCallObtieneEliminacionHogar = new CallMethod
            {
                Mc_nombre = "ObtieneEliminacionHogar()",
                Mc_contenido = _getJSONObtieneEliminacionHogar.GetJSONCall()
            };

            return sb.ToString() + _methodCallObtieneEliminacionHogar.CreaJQueryFunction();
        }

        /// <summary>
        /// Obtiene componente para eliminación de hogar
        /// </summary>
        public string ObtieneEliminacionHogar(string token)
        {
            string _strHtml = "";
            StringBuilder sb = new StringBuilder();

            // Genero función de eliminación
            PostJSON _postEliminarJSON = new PostJSON
            {
                P_url_servicio = _appSettings.ServidorWeb + "api/hogar/eliminar-hogar",
                P_data = "{ token: '" + token + "' }",
                P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { alert('El hogar se elimino satisfactoriamente.'); $('#modal-master').modal('hide'); obtieneCuestionarioWeb(4,'" + token + "'); }"
            };

            CallMethod _callMethodEliminar = new CallMethod
            {
                Mc_nombre = "EliminarHogar()",
                Mc_contenido = _postEliminarJSON.PostJSONCall()
            };

            sb.Append("<div class=\"col-lg-12 text-center\">");
            sb.Append("<h2>¿Confirmas que deseas eliminar este registro?</h2>");
            sb.Append("<div class=\"row\">");
            sb.Append("<div class=\"col-lg-3 col-xs-3\"></div>");
            sb.Append("<div class=\"col-lg-6 col-xs-6\">");
            sb.Append("<br />");
            sb.Append("<button class=\"eliminar-formulario btn btn-lg btn-danger btn-block\" onclick=\"EliminarHogar();\" type=\"button\">Confirmar eliminación</button>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-3 col-xs-3\"></div>");
            sb.Append("<br />");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append(_callMethodEliminar.CreaJQueryFunction());

            _strHtml = sb.ToString();

            return _strHtml;
        }

        /// <summary>
        /// Obtiene Identificación Hogar
        /// </summary>
        public string ObtieneIdentificacionHogar(string token, HogarBOL _hogarBOL)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=\"row\">");
            sb.Append("<div class=\"col-lg-12  text-center\">");
            sb.Append("<div class=\"alert alert-info\">");
            sb.Append("<h3><strong>Información del Hogar <span class=\"NumHog\"></span></strong></h3>");
            sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(4,'" + token + "');\" class=\"btn btn-info btn-md\"><i class=\"fa fa-chevron-left\"></i> Volver al resumen de Hogares</button>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("<script type=\"text/javascript\">$('.NumHog').html(localStorage.getItem('NumHogar'));</script>");

            return sb.ToString();
        }

        /// <summary>
        /// Obtiene formulario Sección Hogar Tenencia
        /// </summary>
        public string ObtieneHogarTenencia(string token, int paso, string cuestionario = "")
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Hogar
            HogarBOL _hogarBOL = new HogarBOL();
            HogarDAL _hogarDAL = new HogarDAL();

            _hogarBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            _hogarBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;

            List<HogarBOL> listaHogar = _hogarDAL.Listar<HogarBOL>(_hogarBOL);
            string _strCheckP16Si = "";
            string _strCheckP16No = "";
            if (listaHogar.Count > 0)
            {
                _hogarBOL = listaHogar[0];

                if (_hogarBOL.TEN1 == "1")
                {
                    _strCheckP16Si = "checked=\"checked\"";
                }

                if (_hogarBOL.TEN1 == "0")
                {
                    _strCheckP16No = "checked=\"checked\"";
                }
            }

            // Submit del formulario
            _postJSON.P_form = "formulario-hogar";
            _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
            _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/hogar/ingresar-hogar-tenencia";
            _postJSON.P_data_dinamica = true;
            _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioWeb(" + (paso + 1) + ",'" + token + "'); }";

            // Identificación Vivienda-Hogar
            sb.Append(ObtieneIdentificacionHogar(token, _hogarBOL));

            // Inicio Definición del Formulario
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
            sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
            sb.Append("<input id=\"paso_formulario\" name=\"paso_formulario\" type=\"hidden\" value=\"" + paso + "\"/>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");

            // Inicio Linea 1 (Pregunta 16)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
            sb.Append("<p style=\"margin-bottom:-2px;\"><strong>TENENCIA DE LA VIVIENDA</strong></p>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<br />");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<p>Cada grupo identificado es un hogar para efectos del Censo.<br />Este módulo contiene preguntas que buscan caracterizar a él o los hogares que componen la vivienda en la que usted reside.</p>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>16.- ¿ALGÚN INTEGRANTE DE SU HOGAR ES EL PROPIETARIO DE LA VIVIENDA?</strong></p>");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<input id=\"rbt_opt16_1\" class=\"magic-radio\" type=\"radio\" name=\"TEN1\" value=\"1\" " + _strCheckP16Si + "> ");
            sb.Append("<label for=\"rbt_opt16_1\" style=\"display: inline;\">&nbsp;Si</label>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<input id=\"rbt_opt16_2\" class=\"magic-radio\" type=\"radio\" name=\"TEN1\" value=\"0\" " + _strCheckP16No + "> ");
            sb.Append("<label for=\"rbt_opt16_2\" style=\"display: inline;\">&nbsp;No</label>");
            sb.Append("</div>");

            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 1 (Pregunta 16)

            // Inicio Linea 2 (Pregunta 17 y 17.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>17.- LA VIVIENDA QUE OCUPA SU HOGAR ES:</strong></p>");
            sb.Append("<input id=\"TEN2_prec\" name=\"TEN2_prec\" type=\"hidden\" value=\"" + _hogarBOL.TEN2 + "\"/>");
            sb.Append("<select id=\"TEN2\" name=\"TEN2\" class=\"form-control\" data-width=\"100%\">");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12 TEN3_Otro\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>17.1.- ESPECIFIQUE OTRO TIPO DE OCUPACIÓN DE SU VIVIENDA</strong></p>");
            sb.Append("<input id=\"TEN3\" name=\"TEN3\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OTRO\" value=\"" + _hogarBOL.TEN3 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 4 (Pregunta 18 y 18.1)

            sb.Append("</div>");

            sb.Append("</div>");

            // Inicio Botones del Cuestionario
            sb.Append("<div class=\"row text-center\">");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<hr />");
            sb.Append("<div class=\"mensaje text-center\"></div>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
            sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso - 1) + ",'" + token + "');\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
            sb.Append("<button type =\"submit\" class=\"btn btn-success btn-md btn-block\"><i class=\"fa fa-floppy-o\"></i> Guardar y continuar</button>");
            sb.Append("</div>");
            sb.Append("</div>");
            // Fin Botones del Cuestionario

            sb.Append("</form>");
            // Fin Definición del Formulario

            // Genero load del formulario
            CallMethod _methodCallLoad = new CallMethod
            {
                Mc_contenido = _postJSON.PostJSONCall() +
                               "$('.magic-radio').iCheck({" +
                                    "checkboxClass: 'icheckbox_square-green'," +
                                    "radioClass: 'iradio_square-green'," +
                                    "increaseArea: '20%'" +
                               "});"
            };

            return sb.ToString() + _methodCallLoad.CreaJQueryDocumentReady();
        }

        /// <summary>
        /// Obtiene formulario Sección Hogar Fuente de Energia
        /// </summary>
        public string ObtieneHogarFuenteEnergia(string token, int paso, string cuestionario = "")
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Hogar
            HogarBOL _hogarBOL = new HogarBOL();
            HogarDAL _hogarDAL = new HogarDAL();

            _hogarBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            _hogarBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;

            List<HogarBOL> listaHogar = _hogarDAL.Listar<HogarBOL>(_hogarBOL);
            if (listaHogar.Count > 0)
            {
                _hogarBOL = listaHogar[0];
            }

            // Carga opciones de respuesta
            GesFormPreguntasOpcionesBOL _gesFormPreguntasOpcionesBOL = new GesFormPreguntasOpcionesBOL();
            GesFormPreguntasOpcionesDAL _gesFormPreguntasOpcionesDAL = new GesFormPreguntasOpcionesDAL();
            List<GesFormPreguntasOpcionesBOL> listaOpcionesPregunta = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesPreguntaPorGrupos<GesFormPreguntasOpcionesBOL>("'37','39','41'");

            // Obtengo opciones de respuesta
            StringBuilder sbENE1 = new StringBuilder();
            StringBuilder sbENE3 = new StringBuilder();
            StringBuilder sbENE5 = new StringBuilder();

            foreach (var item in listaOpcionesPregunta)
            {
                switch (item.Gpf_codigo_pregunta)
                {
                    case "ENE1":
                        if (item.Fpo_numero.ToString() == _hogarBOL.ENE1.ToString())
                        {
                            sbENE1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbENE1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case "ENE3":
                        if (item.Fpo_numero.ToString() == _hogarBOL.ENE3.ToString())
                        {
                            sbENE3.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbENE3.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case "ENE5":
                        if (item.Fpo_numero.ToString() == _hogarBOL.ENE5.ToString())
                        {
                            sbENE5.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbENE5.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                }
            }

            // Identificación Vivienda-Hogar
            sb.Append(ObtieneIdentificacionHogar(token, _hogarBOL));

            // Submit del formulario
            _postJSON.P_form = "formulario-hogar";
            _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
            _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/hogar/ingresar-hogar-fuente-energia";
            _postJSON.P_data_dinamica = true;
            _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioWeb(" + (paso + 1) + ",'" + token + "'); }";

            // Inicio Definición del Formulario
            sb.Append("<link rel=\"stylesheet\" href='" + _appSettings.ServidorWeb + "Framework/assets/css/plugins/dataTables/datatables.min.css'>");
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
            sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
            sb.Append("<input id=\"paso_formulario\" name=\"paso_formulario\" type=\"hidden\" value=\"" + paso + "\"/>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");

            // Inicio Linea 1 (Pregunta 18 y 18.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
            sb.Append("<p style=\"margin-bottom:-2px;\"><strong>FUENTE DE ENERGÍA O COMBUSTIBLES</strong></p>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<br>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>18.- ¿CUÁL ES LA PRINCIPAL FUENTE DE ENERGÍA O COMBUSTIBLE QUE UTILIZAN PARA COCINAR?</strong><br />Considere aquella energía o combustible utilizada en mayor proporción en el hogar. Si hay dos fuentes en iguales proporciones, registre aquella utilizada de forma habitual durante la mayor parte del año.</p>");
            sb.Append("<select id=\"ENE1\" name=\"ENE1\" class=\"form-control\" data-width=\"100%\">");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbENE1.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 ENE2_Otro\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>18.1.- ESPECIFIQUE LA OTRA FUENTE DE ENERGÍA O COMBUSTIBLE QUE UTILIZAN PARA COCINAR</strong></p>");
            sb.Append("<input id=\"ENE2\" name=\"ENE2\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OTRO\" value=\"" + _hogarBOL.ENE2 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 1 (Pregunta 18 y 18.1)

            // Inicio Linea 2 (Pregunta 19 y 19.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>19.- ¿CUÁL ES LA PRINCIPAL FUENTE DE ENERGÍA O COMBUSTIBLE QUE UTILIZAN PARA CALEFACCIONAR?</strong><br />Considere aquella energía o combustible utilizada en mayor proporción en el hogar. Si hay dos fuentes en iguales proporciones, registre aquella utilizada de forma habitual durante la mayor parte del año.</p>");
            sb.Append("<select id=\"ENE3\" name=\"ENE3\" class=\"form-control\" data-width=\"100%\">");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbENE3.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 ENE4_Otro\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>19.1.- ESPECIFIQUE LA OTRA FUENTE DE ENERGÍA O COMBUSTIBLE QUE UTILIZAN PARA CALEFACCIONAR</strong></p>");
            sb.Append("<input id=\"ENE4\" name=\"ENE4\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OTRO\" value=\"" + _hogarBOL.ENE4 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 2 (Pregunta 19 y 19.1)

            // Inicio Linea 3 (Pregunta 20 y 20.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>20.- ¿CUÁL ES LA PRINCIPAL FUENTE DE ENERGÍA O COMBUSTIBLE QUE UTILIZAN PARA CALENTAR EL AGUA?</strong><br />Considere aquella energía o combustible utilizada en mayor proporción en el hogar. Si hay dos fuentes en iguales proporciones, registre aquella utilizada de forma habitual durante la mayor parte del año.</p>");
            sb.Append("<select id=\"ENE5\" name=\"ENE5\" class=\"form-control\" data-width=\"100%\">");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbENE5.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 ENE6_Otro\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>20.1.- ESPECIFIQUE LA OTRA FUENTE DE ENERGÍA O COMBUSTIBLE QUE UTILIZAN PARA CALENTAR EL AGUA</strong></p>");
            sb.Append("<input id=\"ENE6\" name=\"ENE6\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OTRO\" value=\"" + _hogarBOL.ENE6 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>21.- ESTE HOGAR, ¿DISPONE DE ALGUNO DE LOS SIGUIENTES EQUIPOS O SERVICIOS?</strong></p>");

            string _strCheckSiENE7 = "";
            string _strCheckNoENE7 = "";
            if (_hogarBOL.ENE7 == "1") { _strCheckSiENE7 = "checked=\"checked\";"; } else { _strCheckSiENE7 = ""; }
            if (_hogarBOL.ENE7 == "0") { _strCheckNoENE7 = "checked=\"checked\";"; } else { _strCheckNoENE7 = ""; }

            string _strCheckSiENE8 = "";
            string _strCheckNoENE8 = "";
            if (_hogarBOL.ENE8 == "1") { _strCheckSiENE8 = "checked=\"checked\";"; } else { _strCheckSiENE8 = ""; }
            if (_hogarBOL.ENE8 == "0") { _strCheckNoENE8 = "checked=\"checked\";"; } else { _strCheckNoENE8 = ""; }

            string _strCheckSiENE9 = "";
            string _strCheckNoENE9 = "";
            if (_hogarBOL.ENE9 == "1") { _strCheckSiENE9 = "checked=\"checked\";"; } else { _strCheckSiENE9 = ""; }
            if (_hogarBOL.ENE9 == "0") { _strCheckNoENE9 = "checked=\"checked\";"; } else { _strCheckNoENE9 = ""; }

            string _strCheckSiENE10 = "";
            string _strCheckNoENE10 = "";
            if (_hogarBOL.ENE10 == "1") { _strCheckSiENE10 = "checked=\"checked\";"; } else { _strCheckSiENE10 = ""; }
            if (_hogarBOL.ENE10 == "0") { _strCheckNoENE10 = "checked=\"checked\";"; } else { _strCheckNoENE10 = ""; }

            string _strCheckSiENE11 = "";
            string _strCheckNoENE11 = "";
            if (_hogarBOL.ENE11 == "1") { _strCheckSiENE11 = "checked=\"checked\";"; } else { _strCheckSiENE11 = ""; }
            if (_hogarBOL.ENE11 == "0") { _strCheckNoENE11 = "checked=\"checked\";"; } else { _strCheckNoENE11 = ""; }

            string _strCheckSiENE12 = "";
            string _strCheckNoENE12 = "";
            if (_hogarBOL.ENE12 == "1") { _strCheckSiENE12 = "checked=\"checked\";"; } else { _strCheckSiENE12 = ""; }
            if (_hogarBOL.ENE12 == "0") { _strCheckNoENE12 = "checked=\"checked\";"; } else { _strCheckNoENE12 = ""; }

            sb.Append("<table class=\"table table-bordered\">");
            sb.Append("<tbody>");
            sb.Append("<tr>");
            sb.Append("<td class=\"col-lg-4 col-md-6 col-xs-8\">1) Teléfono fijo</td>");
            sb.Append("<td class=\"col-lg-4 col-md-6 col-xs-4\">");
            sb.Append("<input id=\"rbt_opt21_1\" class=\"magic-radio\" type=\"radio\" name=\"ENE7\" value=\"1\" " + _strCheckSiENE7 + ">");
            sb.Append("<label for=\"rbt_opt21_1\" style=\"display: inline;\">&nbsp;Si</label>&nbsp;");
            sb.Append("<input id=\"rbt_opt21_2\" class=\"magic-radio\" type=\"radio\" name=\"ENE7\" value=\"0\" " + _strCheckNoENE7 + ">");
            sb.Append("<label for=\"rbt_opt21_2\" style=\"display: inline;\">&nbsp;No</label>");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td>2) Teléfono móvil o smarphone</td>");
            sb.Append("<td>");
            sb.Append("<input id=\"rbt_opt21_3\" class=\"magic-radio\" type=\"radio\" name=\"ENE8\" value=\"1\" " + _strCheckSiENE8 + ">");
            sb.Append("<label for=\"rbt_opt21_3\" style=\"display: inline;\">&nbsp;Si</label>&nbsp;");
            sb.Append("<input id=\"rbt_opt21_4\" class=\"magic-radio\" type=\"radio\" name=\"ENE8\" value=\"0\" " + _strCheckNoENE8 + ">");
            sb.Append("<label for=\"rbt_opt21_4\" style=\"display: inline;\">&nbsp;No</label>");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td>3) Computador (escritorio, portátil o Tablet)</td>");
            sb.Append("<td>");
            sb.Append("<input id=\"rbt_opt21_5\" class=\"magic-radio\" type=\"radio\" name=\"ENE9\" value=\"1\" " + _strCheckSiENE9 + ">");
            sb.Append("<label for=\"rbt_opt21_5\" style=\"display: inline;\">&nbsp;Si</label>&nbsp;");
            sb.Append("<input id=\"rbt_opt21_6\" class=\"magic-radio\" type=\"radio\" name=\"ENE9\" value=\"0\" " + _strCheckNoENE9 + ">");
            sb.Append("<label for=\"rbt_opt21_6\" style=\"display: inline;\">&nbsp;No</label>");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td>4) Internet banda ancha fija</td>");
            sb.Append("<td>");
            sb.Append("<input id=\"rbt_opt21_7\" class=\"magic-radio\" type=\"radio\" name=\"ENE10\" value=\"1\" " + _strCheckSiENE10 + ">");
            sb.Append("<label for=\"rbt_opt21_7\" style=\"display: inline;\">&nbsp;Si</label>&nbsp;");
            sb.Append("<input id=\"rbt_opt21_8\" class=\"magic-radio\" type=\"radio\" name=\"ENE10\" value=\"0\" " + _strCheckNoENE10 + ">");
            sb.Append("<label for=\"rbt_opt21_8\" style=\"display: inline;\">&nbsp;No</label>");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td>5) Internet banda ancha móvil (modem Wifi, USB o BAM)</td>");
            sb.Append("<td>");
            sb.Append("<input id=\"rbt_opt21_9\" class=\"magic-radio\" type=\"radio\" name=\"ENE11\" value=\"1\" " + _strCheckSiENE11 + ">");
            sb.Append("<label for=\"rbt_opt21_9\" style=\"display: inline;\">&nbsp;Si</label>&nbsp;");
            sb.Append("<input id=\"rbt_opt21_10\" class=\"magic-radio\" type=\"radio\" name=\"ENE11\" value=\"0\" " + _strCheckNoENE11 + ">");
            sb.Append("<label for=\"rbt_opt21_10\" style=\"display: inline;\">&nbsp;No</label>");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td>6) Internet móvil desde un celular o smarphone</td>");
            sb.Append("<td>");
            sb.Append("<input id=\"rbt_opt21_11\" class=\"magic-radio\" type=\"radio\" name=\"ENE12\" value=\"1\" " + _strCheckSiENE12 + ">");
            sb.Append("<label for=\"rbt_opt21_11\" style=\"display: inline;\">&nbsp;Si</label>&nbsp;");
            sb.Append("<input id=\"rbt_opt21_12\" class=\"magic-radio\" type=\"radio\" name=\"ENE12\" value=\"0\" " + _strCheckNoENE12 + ">");
            sb.Append("<label for=\"rbt_opt21_12\" style=\"display: inline;\">&nbsp;No</label>");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("</tbody>");
            sb.Append("</table>");

            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 3 (Pregunta 20 y 20.1)

            sb.Append("</div>");

            sb.Append("</div>");

            // Inicio Botones del Cuestionario
            sb.Append("<div class=\"row text-center\">");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<hr />");
            sb.Append("<div class=\"mensaje text-center\"></div>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
            sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso - 1) + ",'" + token + "');\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
            sb.Append("<button type =\"submit\" class=\"btn btn-success btn-md btn-block\"><i class=\"fa fa-floppy-o\"></i> Guardar y continuar</button>");
            sb.Append("</div>");
            sb.Append("</div>");
            // Fin Botones del Cuestionario

            sb.Append("</form>");
            // Fin Definición del Formulario

            // Genero load del formulario
            CallMethod _methodCallLoad = new CallMethod
            {
                Mc_contenido = _postJSON.PostJSONCall() +
                               "$('.magic-radio').iCheck({" +
                                    "checkboxClass: 'icheckbox_square-green'," +
                                    "radioClass: 'iradio_square-green'," +
                                    "increaseArea: '20%'" +
                               "});"
            };

            return sb.ToString() + _methodCallLoad.CreaJQueryDocumentReady();
        }

        /// <summary>
        /// Obtiene formulario Sección Hogar Gestión de Residuos
        /// </summary>
        public string ObtieneHogarGestionResiduos(string token, int paso, string cuestionario = "")
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Hogar
            HogarBOL _hogarBOL = new HogarBOL();
            HogarDAL _hogarDAL = new HogarDAL();

            _hogarBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            _hogarBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;

            List<HogarBOL> listaHogar = _hogarDAL.Listar<HogarBOL>(_hogarBOL);

            string _strCheckRES1Si = "";
            string _strCheckRES1No = "";
            string _strCheckRES2Si = "";
            string _strCheckRES2No = "";

            if (listaHogar.Count > 0)
            {
                _hogarBOL = listaHogar[0];

                if (_hogarBOL.RES1 == "1")
                {
                    _strCheckRES1Si = "checked=\"checked\"";
                }

                if (_hogarBOL.RES1 == "0")
                {
                    _strCheckRES1No = "checked=\"checked\"";
                }

                if (_hogarBOL.RES2 == "1")
                {
                    _strCheckRES2Si = "checked=\"checked\"";
                }

                if (_hogarBOL.RES2 == "0")
                {
                    _strCheckRES2No = "checked=\"checked\"";
                }
            }

            // Identificación Vivienda-Hogar
            sb.Append(ObtieneIdentificacionHogar(token, _hogarBOL));

            // Submit del formulario
            _postJSON.P_form = "formulario-hogar";
            _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
            _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/hogar/ingresar-hogar-residuos";
            _postJSON.P_data_dinamica = true;
            _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioWeb(" + (paso + 1) + ",'" + token + "'); }";

            // Inicio Definición del Formulario
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
            sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
            sb.Append("<input id=\"paso_formulario\" name=\"paso_formulario\" type=\"hidden\" value=\"" + paso + "\"/>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");

            // Inicio Linea 1 (Pregunta 21 y 22)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
            sb.Append("<p style=\"margin-bottom:-2px;\"><strong>GESTIÓN DE RESIDUOS</strong></p>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<br>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>22.- ¿HABITUALMENTE SEPARA PLÁSTICO, VIDRIO, ALUMINIO, PAPEL U OTRO PARA RECICLAJE?</strong><br />Separación de residuos en una o más categorías (latas, vidrios, papeles, plásticos, otros) y que pueden ser recogidos en forma clasificada por los servicios de aseo o dejada por los miembros del hogar en contenedores especiales.</p>");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<input id=\"rbt_opt22_1\" class=\"magic-radio\" type=\"radio\" name=\"RES1\" value=\"1\" " + _strCheckRES1Si + "> ");
            sb.Append("<label for=\"rbt_opt22_1\" style=\"display: inline;\">&nbsp;Si</label>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<input id=\"rbt_opt22_2\" class=\"magic-radio\" type=\"radio\" name=\"RES1\" value=\"0\" " + _strCheckRES1No + ">");
            sb.Append("<label for=\"rbt_opt22_2\" style=\"display: inline;\">&nbsp;No</label>");
            sb.Append("</div>");

            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>23.- ¿SEPARAN LOS RESIDUOS ORGÁNICOS PARA HACER ABONO COMPOSTABLE?</strong><br />Degradación de residuos orgánicos para la transformación en abono natural.</p>");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<input id=\"rbt_opt23_1\" class=\"magic-radio\" type=\"radio\" name=\"RES2\" value=\"1\" " + _strCheckRES2Si + ">");
            sb.Append("<label for=\"rbt_opt23_1\" style=\"display: inline;\">&nbsp;Si</label>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<input id=\"rbt_opt23_2\" class=\"magic-radio\" type=\"radio\" name=\"RES2\" value=\"0\" " + _strCheckRES2No + ">");
            sb.Append("<label for=\"rbt_opt23_2\" style=\"display: inline;\">&nbsp;No</label>");
            sb.Append("</div>");

            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 2 (Pregunta 21 y 22)

            sb.Append("</div>");

            sb.Append("</div>");

            // Inicio Botones del Cuestionario
            sb.Append("<div class=\"row text-center\">");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<hr />");
            sb.Append("<div class=\"mensaje text-center\"></div>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
            sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso - 1) + ",'" + token + "');\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
            sb.Append("<button type =\"submit\" class=\"btn btn-success btn-md btn-block\"><i class=\"fa fa-floppy-o\"></i> Guardar y continuar</button>");
            sb.Append("</div>");
            sb.Append("</div>");
            // Fin Botones del Cuestionario

            sb.Append("</form>");
            // Fin Definición del Formulario

            // Genero load del formulario
            CallMethod _methodCallLoad = new CallMethod
            {
                Mc_contenido = _postJSON.PostJSONCall() +
                               "$('.magic-radio').iCheck({" +
                                    "checkboxClass: 'icheckbox_square-green'," +
                                    "radioClass: 'iradio_square-green'," +
                                    "increaseArea: '20%'" +
                               "});"
            };

            return sb.ToString() + _methodCallLoad.CreaJQueryDocumentReady();
        }

        /// <summary>
        /// Obtiene formulario Sección Hogar Migraciones
        /// </summary>
        public string ObtieneHogarMigraciones(string token, int paso, string cuestionario = "")
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Hogar
            HogarBOL _hogarBOL = new HogarBOL();
            HogarDAL _hogarDAL = new HogarDAL();

            _hogarBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            _hogarBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;

            List<HogarBOL> listaHogar = _hogarDAL.Listar<HogarBOL>(_hogarBOL);

            string _strCheckEMI1Si = "";
            string _strCheckEMI1No = "";

            if (listaHogar.Count > 0)
            {
                _hogarBOL = listaHogar[0];

                if (_hogarBOL.EMI1 == "1")
                {
                    _strCheckEMI1Si = "checked=\"checked\"";
                }

                if (_hogarBOL.EMI1 == "0")
                {
                    _strCheckEMI1No = "checked=\"checked\"";
                }
            }

            // Identificación Vivienda-Hogar
            sb.Append(ObtieneIdentificacionHogar(token, _hogarBOL));

            // Submit del formulario
            _postJSON.P_form = "formulario-hogar";
            _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
            _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/hogar/ingresar-hogar-migraciones";
            _postJSON.P_data_dinamica = true;
            _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioWeb(" + (paso + 1) + ",'" + token + "'); }";

            // Inicio Definición del Formulario
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
            sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
            sb.Append("<input id=\"paso_formulario\" name=\"paso_formulario\" type=\"hidden\" value=\"" + paso + "\"/>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");

            // Inicio Linea 1 (Pregunta 23 y 23.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
            sb.Append("<p style=\"margin-bottom:-2px;\"><strong>EMIGRACIÓN EN EL HOGAR</strong></p>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<br>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>24.- A PARTIR DE NOVIEMBRE DE 2016, ¿ALGUNA PERSONA QUE PERTENECÍA AL HOGAR VIVE ACTUALMENTE EN EL EXTRANJERO?</strong><br />Personas integrantes del hogar que cambiaron su residencia habitual hacia otro país en los últimos de 5 años.</p>");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<input id=\"rbt_opt24_1\" class=\"magic-radio\" type=\"radio\" name=\"EMI1\" value=\"1\" " + _strCheckEMI1Si + "> ");
            sb.Append("<label for=\"rbt_opt24_1\" style=\"display: inline;\">&nbsp;Si</label>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<input id=\"rbt_opt24_2\" class=\"magic-radio\" type=\"radio\" name=\"EMI1\" value=\"0\" " + _strCheckEMI1No + ">");
            sb.Append("<label for=\"rbt_opt24_2\" style=\"display: inline;\">&nbsp;No</label>");
            sb.Append("</div>");

            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group EMI2_Ocultar\">");
            sb.Append("<p><strong>24.1.- ¿CUÁNTAS?</strong></p>");
            sb.Append("<input id=\"EMI2\" name=\"EMI2\" type=\"number\" class=\"form-control\" min=\"1\" max =\"99\" onKeyPress=\"if (this.value.length == 2) return false; return event.charCode >= 48 && event.charCode <= 57;\" placeholder=\"N°\" value=\"" + _hogarBOL.EMI2 + "\" required />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 1 (Pregunta 23 y 23.1)

            sb.Append("</div>");

            sb.Append("</div>");

            // Inicio Botones del Cuestionario
            sb.Append("<div class=\"row text-center\">");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<hr />");
            sb.Append("<div class=\"mensaje text-center\"></div>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
            sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso - 1) + ",'" + token + "');\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
            sb.Append("<button type =\"submit\" class=\"btn btn-success btn-md btn-block\"><i class=\"fa fa-floppy-o\"></i> Guardar y continuar</button>");
            sb.Append("</div>");
            sb.Append("</div>");
            // Fin Botones del Cuestionario

            sb.Append("</form>");
            // Fin Definición del Formulario

            // Genero load del formulario
            CallMethod _methodCallLoad = new CallMethod
            {
                Mc_contenido = _postJSON.PostJSONCall() +
                               "$('.magic-radio').iCheck({" +
                                    "checkboxClass: 'icheckbox_square-green'," +
                                    "radioClass: 'iradio_square-green'," +
                                    "increaseArea: '20%'" +
                               "});"
            };

            return sb.ToString() + _methodCallLoad.CreaJQueryDocumentReady();
        }

        /// <summary>
        /// Obtiene Resúmen Sección Hogar Migraciones
        /// </summary>
        public string ObtieneResumenSeccionHogarMigraciones(string token, int paso, string cuestionario = "")
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbTabla = new StringBuilder();
            StringBuilder sbDetalle = new StringBuilder();

            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Hogar
            PersonaExtBOL _personaExtBOL = new PersonaExtBOL();
            HogarBOL _hogarBOL = new HogarBOL();
            HogarDAL _hogarDAL = new HogarDAL();

            _personaExtBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            _personaExtBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;

            _hogarBOL.PK_VIVIENDA = _personaExtBOL.PK_VIVIENDA;
            _hogarBOL.PK_HOGAR = _personaExtBOL.PK_VIVIENDA;

            List<PersonaExtBOL> listaResumenMigraciones = _hogarDAL.ListarMigracionesPorHogar<PersonaExtBOL>(_personaExtBOL);

            if (listaResumenMigraciones.Count > 0)
            {
                Encrypt _encrypt = new Encrypt();

                // Identificación Vivienda-Hogar
                sb.Append(ObtieneIdentificacionHogar(token, _hogarBOL));

                // Obtengo listado de hogares
                bool _migracionesPersonas = false;
                foreach (var item in listaResumenMigraciones)
                {
                    string _idFormulario = "";
                    _idFormulario = _encrypt.EncryptString(item.PK_VIVIENDA + "&" + "" + item.PK_HOGAR + "&" + item.PK_VPE + "");

                    sbDetalle.Append("<tr>");
                    if (item.VPE1 == "")
                    {
                        sbDetalle.Append("<td><button onclick=\"obtieneCuestionarioWeb(" + (paso + 1) + ",'" + _idFormulario + "');\" class=\"btn btn-primary btn-block\">Persona-" + item.PK_VPE + "</button></td>");
                    }
                    else
                    {
                        sbDetalle.Append("<td><button onclick=\"obtieneCuestionarioWeb(" + (paso + 1) + ",'" + _idFormulario + "');\" class=\"btn btn-primary btn-block\">" + item.VPE1 + "</button></td>");
                    }

                    if (item.VPE_COMPLETO == 0)
                    {
                        sbDetalle.Append("<td><p style=\"color:#8bc34a\"><i class=\"fa fa-handshake-o\"></i> Completo</p></td>");
                        _migracionesPersonas = true;
                    }
                    else
                    {
                        sbDetalle.Append("<td><p class=\"text-warning\"><i class=\"fa fa-info\"></i> Pendiente</p></td>");
                        _migracionesPersonas = false;
                    }
                    sbDetalle.Append("</tr>");
                }

                sbTabla.Append("<div class=\"row\">");
                sbTabla.Append("<div class=\"col-lg-12 text-center\">");
                sbTabla.Append("<h1>Personas que emigraron del hogar</h1>");
                sbTabla.Append("<hr />");
                sbTabla.Append("</div>");
                sbTabla.Append("</div>");

                sbTabla.Append("<link rel=\"stylesheet\" href='" + _appSettings.ServidorWeb + "Framework/assets/css/plugins/dataTables/datatables.min.css'>");
                sbTabla.Append("<div class=\"row\">");
                sbTabla.Append("<div class=\"table-responsive\">");
                sbTabla.Append("<table class=\"tabla-informante table table-striped text-center\">");
                sbTabla.Append("<thead>");
                sbTabla.Append("<tr>");
                sbTabla.Append("<th>PERSONA</th>");
                sbTabla.Append("<th>ESTADO</th>");
                sbTabla.Append("</tr>");
                sbTabla.Append("</thead>");
                sbTabla.Append("<tbody>");
                sbTabla.Append(sbDetalle.ToString());
                sbTabla.Append("</tbody>");
                sbTabla.Append("</table>");
                sbTabla.Append("</div>");
                sbTabla.Append("</div>");

                if (_migracionesPersonas == true)
                {
                    sbTabla.Append("<div class=\"row\">");

                    sbTabla.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
                    sbTabla.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso - 1) + ",'" + token + "');\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
                    sbTabla.Append("</div>");

                    sbTabla.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
                    sbTabla.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso + 2) + ",'" + token + "');\" class=\"btn btn-success btn-md btn-block\">Siguiente <i class=\"fa fa-chevron-right\"></i> </button>");
                    sbTabla.Append("</div>");

                    sbTabla.Append("</div>");
                }
                else
                {
                    sbTabla.Append("<div class=\"row\">");
                    sbTabla.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
                    sbTabla.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso - 1) + ",'" + token + "');\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
                    sbTabla.Append("</div>");
                }
            }
            else
            {
                sbTabla.Append("<script type=\"text/javascript\">obtieneCuestionarioWeb(" + (paso + 2) + ",'" + token + "');</script>");
            }

            sb.Append(sbTabla.ToString());

            return sb.ToString();
        }

        /// <summary>
        /// Obtiene Resúmen Sección Hogar Registro de Personas
        /// </summary>
        public string ObtieneHogarRegistroPersonas(string token, int paso, string cuestionario = "")
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Hogar
            HogarBOL _hogarBOL = new HogarBOL();
            HogarDAL _hogarDAL = new HogarDAL();

            _hogarBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            _hogarBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;

            List<HogarBOL> listaHogar = _hogarDAL.Listar<HogarBOL>(_hogarBOL);
            if (listaHogar.Count > 0)
            {
                _hogarBOL = listaHogar[0];
            }

            // Identificación Vivienda-Hogar
            sb.Append(ObtieneIdentificacionHogar(token, _hogarBOL));

            // Submit del formulario
            _postJSON.P_form = "formulario-hogar";
            _postJSON.P_load = "$('.mensaje-personas').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
            _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/hogar/ingresar-hogar-personas";
            _postJSON.P_data_dinamica = true;
            _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { alert('La persona se ingreso satisfactoriamente.'); obtieneCuestionarioWeb(" + paso + ",'" + token + "'); }";

            sb.Append("<div class=\"row\">");
            sb.Append("<div class=\"col-lg-12  text-center\">");
            sb.Append("<div class=\"alert alert-warning\">");
            sb.Append("<h3>REGISTRO DE RESIDENTES HABITUALES EN EL HOGAR</h3>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            // Inicio Definición del Formulario
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
            sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
            sb.Append("<input id=\"paso_formulario\" name=\"paso_formulario\" type=\"hidden\" value=\"" + paso + "\"/>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>25.- ESCRIBA EL NOMBRE Y APELLIDO DE TODAS LAS PERSONAS DEL HOGAR QUE RESIDEN HABITUALMENTE EN LA VIVIENDA, ESTÉN PRESENTES O NO AL MOMENTO DEL LLENADO DEL CUESTIONARIO.</strong></p>");
            sb.Append("<p>Considere a todas las personas: niñas y niños pequeños, personas mayores y personas postradas. Comience por la o el jefe de hogar y continue en el siguiente orden: 1) cónyuge o conviviente, 2) hijos e hijas de mayor a menor, 3) otros parientes, 4) no parientes.</p>");
            sb.Append("<input id=\"HOG_NOM_PERS\" name=\"HOG_NOM_PERS\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"NOMBRE COMPLETO\" value=\"\" required />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-3 col-md-3\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<button type =\"submit\" class=\"btn btn-success btn-md btn-block\"><i class=\"fa fa-plus\"></i> Agregar residente</button>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"alert alert-danger mensaje-personas-nok\" style=\"display: none;\"></div>");
            sb.Append("</div>");

            sb.Append("</form>");
            // Fin Definición del Formulario

            // Listado de Personas
            string _strResumenPersonas = ObtieneHogarResumenPersonas(token);
            sb.Append(_strResumenPersonas);

            // Inicio Botones del Cuestionario
            sb.Append("<div class=\"row text-center\">");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<hr />");
            sb.Append("<div class=\"mensaje text-center\"></div>");
            sb.Append("</div>");
            if (_hogarBOL.EMI2 != "")
            {
                sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
                sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso - 2) + ",'" + token + "');\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
                sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso - 3) + ",'" + token + "');\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
                sb.Append("</div>");
            }

            if (_strResumenPersonas != "")
            {
                sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
                sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso + 1) + ",'" + token + "');\" class=\"btn btn-success btn-md btn-block\"><i class=\"fa fa-floppy-o\"></i> Guardar y continuar</button>");
                sb.Append("</div>");
            }

            sb.Append("</div>");
            // Fin Botones del Cuestionario

            // Genero load del formulario
            CallMethod _methodCallLoad = new CallMethod
            {
                Mc_contenido = _postJSON.PostJSONCall() +
                               "$('.magic-radio').iCheck({" +
                                    "checkboxClass: 'icheckbox_square-green'," +
                                    "radioClass: 'iradio_square-green'," +
                                    "increaseArea: '20%'" +
                               "});"
            };

            return sb.ToString() + _methodCallLoad.CreaJQueryDocumentReady();
        }

        /// <summary>
        /// Obtiene Sección Hogar Registro de Personas, Modal de edición de datos
        /// </summary>
        public string ObtieneHogarEdicionRegistroPersonas(string token, string persona_id)
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Obtengo información de Persona
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            PersonaBOL _personaBOL = new PersonaBOL();
            PersonaDAL _personaDAL = new PersonaDAL();

            _personaBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            _personaBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;
            _personaBOL.PK_PERSONA = persona_id;

            List<PersonaBOL> listaPersona = _personaDAL.Listar<PersonaBOL>(_personaBOL);
            if (listaPersona.Count > 0)
            {
                _personaBOL = listaPersona[0];
            }

            // Submit del formulario
            _postJSON.P_form = "formulario-hogar-editar-persona";
            _postJSON.P_load = "$('.mensaje-personas-edicion').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
            _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/hogar/editar-hogar-personas";
            _postJSON.P_data_dinamica = true;
            _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { alert('La persona se actualizo satisfactoriamente.'); obtieneCuestionarioWeb(11,'" + token + "'); }";

            // Inicio Definición del Formulario
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<h2 class=\"text-center\">Actualización de Residentes Habituales</h2>");
            sb.Append("<hr />");
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
            sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
            sb.Append("<input id=\"persona_id\" name=\"persona_id\" type=\"hidden\" value=\"" + persona_id + "\"/>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>25.- NOMBRE COMPLETO</strong></p>");
            sb.Append("<input id=\"HOG_NOM_PERS\" name=\"HOG_NOM_PERS\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"NOMBRE COMPLETO\" value=\"" + _personaBOL.PER_NOMBRE + "\" required />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-3 col-md-3\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<button type =\"submit\" class=\"btn btn-success btn-md btn-block\"><i class=\"fa fa-check\"></i> Actualizar</button>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");

            sb.Append("</form>");
            sb.Append("</div>");
            // Fin Definición del Formulario

            // Genero load del formulario
            CallMethod _methodCallLoad = new CallMethod
            {
                Mc_contenido = _postJSON.PostJSONCall() +
                               "$('.magic-radio').iCheck({" +
                                    "checkboxClass: 'icheckbox_square-green'," +
                                    "radioClass: 'iradio_square-green'," +
                                    "increaseArea: '20%'" +
                               "});"
            };

            return sb.ToString() + _methodCallLoad.CreaJQueryDocumentReady();
        }

        /// <summary>
        /// Obtiene componente para eliminación de persona
        /// </summary>
        public string ObtieneEliminacionPersona(string token, string persona_id)
        {
            string _strHtml = "";
            StringBuilder sb = new StringBuilder();

            // Genero función de eliminación
            PostJSON _postEliminarJSON = new PostJSON
            {
                P_url_servicio = _appSettings.ServidorWeb + "api/hogar/eliminar-persona",
                P_data = "{ token: '" + token + "', persona_id: '" + persona_id + "' }",
                P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { alert('La persona se elimino satisfactoriamente.'); $('#modal-master').modal('hide'); obtieneCuestionarioWeb(11,'" + token + "'); }"
            };

            CallMethod _callMethodEliminar = new CallMethod
            {
                Mc_nombre = "EliminarPersona()",
                Mc_contenido = _postEliminarJSON.PostJSONCall()
            };

            sb.Append("<div class=\"col-lg-12 text-center\">");
            sb.Append("<h2>¿Confirmas que deseas eliminar este registro?</h2>");
            sb.Append("<div class=\"row\">");
            sb.Append("<div class=\"col-lg-3 col-xs-3\"></div>");
            sb.Append("<div class=\"col-lg-6 col-xs-6\">");
            sb.Append("<br />");
            sb.Append("<button class=\"eliminar-formulario btn btn-lg btn-danger btn-block\" onclick=\"EliminarPersona();\" type=\"button\">Confirmar eliminación</button>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-3 col-xs-3\"></div>");
            sb.Append("<br />");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append(_callMethodEliminar.CreaJQueryFunction());

            _strHtml = sb.ToString();

            return _strHtml;
        }

        /// <summary>
        /// Obtiene Resúmen Sección Hogar Total de Personas
        /// </summary>
        public string ObtieneHogarTotalPersonas(string token, int paso, string cuestionario = "")
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbPersonas = new StringBuilder();
            PostJSON _postJSON = new PostJSON();
            PostJSON _postPersonaJSON = new PostJSON();

            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Hogar
            HogarBOL _hogarBOL = new HogarBOL();
            HogarDAL _hogarDAL = new HogarDAL();

            _hogarBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            _hogarBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;

            List<HogarBOL> listaHogar = _hogarDAL.Listar<HogarBOL>(_hogarBOL);
          
            if (listaHogar.Count > 0)
            {
                _hogarBOL = listaHogar[0];
            }

            // Obtengo listado de personas
            DataSet DsPersonas = new DataSet();
            DsPersonas = _hogarDAL.ListarPersonasPorHogar(_hogarBOL, false);

            StringBuilder sbTabla = new StringBuilder();
            StringBuilder sbTablaDetalle = new StringBuilder();

            if (DsPersonas.Tables[0].Rows.Count > 0)
            {
                int i = 0;
                string _strCheckSiPER8 = "";
                string _strCheckNoPER8 = "";
                foreach (DataRow dr in DsPersonas.Tables[0].Rows)
                {
                    if (dr["PER8"].ToString() == "1") { _strCheckSiPER8 = "checked=\"checked\";"; } else { _strCheckSiPER8 = ""; }
                    if (dr["PER8"].ToString() == "0") { _strCheckNoPER8 = "checked=\"checked\";"; } else { _strCheckNoPER8 = ""; }

                    sbTablaDetalle.Append("<tr>");
                    sbTablaDetalle.Append("<td class=\"col-lg-4 col-md-6 col-xs-8\">" + dr["PER_NOMBRE"].ToString() + "</td>");
                    sbTablaDetalle.Append("<td class=\"col-lg-4 col-md-6 col-xs-4\">");
                    sbTablaDetalle.Append("<input id=\"rbt_opt26_1_" + i + "\" class=\"magic-radio\" type=\"radio\" name=\"" + dr["PK_PERSONAS"].ToString() + "\" value=\"1\" " + _strCheckSiPER8 + " required>");
                    sbTablaDetalle.Append("<label for=\"rbt_opt26_1_" + i + "\" style=\"display: inline;\">&nbsp;Si</label>&nbsp;");
                    sbTablaDetalle.Append("<input id=\"rbt_opt26_2_" + i + "\" class=\"magic-radio\" type=\"radio\" name=\"" + dr["PK_PERSONAS"].ToString() + "\" value=\"0\" " + _strCheckNoPER8 + " required>");
                    sbTablaDetalle.Append("<label for=\"rbt_opt26_2_" + i + "\" style=\"display: inline;\">&nbsp;No</label>");
                    sbTablaDetalle.Append("</td>");
                    sbTablaDetalle.Append("</tr>");

                    if (_hogarBOL.HOG_JEFE_HOG == dr["PK_PERSONAS"].ToString())
                    {
                        sbPersonas.Append("<option value=\"" + dr["PK_PERSONAS"].ToString() + "\" selected>" + dr["PER_NOMBRE"].ToString() + "</option>");
                    }
                    else
                    {
                        sbPersonas.Append("<option value=\"" + dr["PK_PERSONAS"].ToString() + "\">" + dr["PER_NOMBRE"].ToString() + "</option>");
                    }
                    i++;
                }

                sbTabla.Append("<table class=\"table table-bordered\">");
                sbTabla.Append("<tbody>");
                sbTabla.Append(sbTablaDetalle.ToString());
                sbTabla.Append("</tbody>");
                sbTabla.Append("</table>");
            }

            // Identificación Vivienda-Hogar
            sb.Append(ObtieneIdentificacionHogar(token, _hogarBOL));

            // Submit del formulario
            _postJSON.P_form = "formulario-hogar";
            _postJSON.P_load = "$('.mensaje-personas').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
            _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/hogar/ingresar-hogar-total-personas";
            _postJSON.P_data_dinamica = true;
            _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioWeb(" + (paso + 1) + ",'" + token + "'); }";

            // Inicio Definición del Formulario
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
            sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
            sb.Append("<input id=\"paso_formulario\" name=\"paso_formulario\" type=\"hidden\" value=\"" + paso + "\"/>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>26.- CONFIRMO QUE TODAS LAS PERSONAS LISTADAS SON INTEGRANTES DEL HOGAR, Y QUE POR TANTO, DEBEN SER CONTABILIZADOS EN ESTA VIVIENDA; Y QUE NO FALTA NINGUNA PERSONA QUE NO HAYA SIDO CONSIDERADA.</strong></p>");
            sb.Append("<br />");
            sb.Append(sbTabla.ToString());
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<hr />");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>27.- TOTAL DE PERSONAS EN EL HOGAR</strong></p>");
            sb.Append("<input id=\"HOG_TOT_PERS\" name=\"HOG_TOT_PERS\" type=\"text\" class=\"form-control\" onKeyPress=\"if (this.value.length == 2) return false; return event.charCode >= 48 && event.charCode <= 57;\" placeholder=\"N°\" value=\"" + _hogarBOL.HOG_TOT_PERS + "\" required />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>ENTONCES, DEL TOTAL DE PERSONAS DEL HOGAR ¿CUÁNTOS SON HOMBRES Y CUÁNTAS SON MUJERES?</strong></p>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>28.- TOTAL DE HOMBRES EN EL HOGAR</strong></p>");
            sb.Append("<input id=\"HOG_TOT_H\" name=\"HOG_TOT_H\" type=\"text\" class=\"form-control\" placeholder=\"N°\" onKeyPress=\"if (this.value.length == 2) return false; return event.charCode >= 48 && event.charCode <= 57;\" value=\"" + _hogarBOL.HOG_TOT_H + "\" required />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>29.- TOTAL DE MUJERES EN EL HOGAR</strong></p>");
            if (_hogarBOL.HOG_TOT_PERS == 0)
            {
                sb.Append("<input id=\"HOG_TOT_M\" name=\"HOG_TOT_M\" type=\"text\" class=\"form-control\" onKeyPress=\"if (this.value.length == 2) return false; return event.charCode >= 48 && event.charCode <= 57;\" placeholder=\"N°\" value=\"" + _hogarBOL.HOG_TOT_M + "\" required />");
            }
            else
            {
                sb.Append("<input id=\"HOG_TOT_M\" name=\"HOG_TOT_M\" type=\"text\" class=\"form-control\" onKeyPress=\"if (this.value.length == 2) return false; return event.charCode >= 48 && event.charCode <= 57;\" placeholder=\"N°\" value=\"" + _hogarBOL.HOG_TOT_M + "\" required />");
            }

            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<hr />");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-8 col-md-8\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>30.- DE LAS SIGUIENTES PERSONAS ¿ME PODRÍA INDICAR QUIÉN ES EL JEFE O JEFA DE HOGAR?</strong></p>");
            sb.Append("<select id=\"HOG_JEFE_HOG\" name=\"HOG_JEFE_HOG\" class=\"form-control\" data-width=\"100%\" required>");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPersonas.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");

            // Inicio Botones del Cuestionario
            sb.Append("<div class=\"row text-center\">");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<hr />");
            sb.Append("<div class=\"mensaje text-center\"></div>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
            sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso - 1) + ",'" + token + "');\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
            sb.Append("<button type =\"submit\" class=\"btn btn-success btn-md btn-block\"><i class=\"fa fa-floppy-o\"></i> Guardar y continuar</button>");
            sb.Append("</div>");
            sb.Append("</div>");
            // Fin Botones del Cuestionario

            sb.Append("</form>");
            // Fin Definición del Formulario

            // Genero load del formulario
            CallMethod _methodCallLoad = new CallMethod
            {
                Mc_contenido = _postJSON.PostJSONCall() +
                               "$('.magic-radio').iCheck({" +
                                    "checkboxClass: 'icheckbox_square-green'," +
                                    "radioClass: 'iradio_square-green'," +
                                    "increaseArea: '20%'" +
                               "});"
            };

            return sb.ToString() + _methodCallLoad.CreaJQueryDocumentReady();
        }

        /// <summary>
        /// Obtiene Resúmen Sección Hogar Resumen de Personas
        /// </summary>
        public string ObtieneHogarResumenPersonas(string token)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbTabla = new StringBuilder();
            StringBuilder sbDetalle = new StringBuilder();

            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Hogar
            HogarBOL _hogarBOL = new HogarBOL();
            HogarDAL _hogarDAL = new HogarDAL();

            DataSet DsPersonas = new DataSet();
            _hogarBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            _hogarBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;

            DsPersonas = _hogarDAL.ListarPersonasPorHogar(_hogarBOL, false);

            // Genero función para el edición de personas
            GetJSON _getJSONObtieneEdicionHogarPersona = new GetJSON
            {
                G_url_servicio = _appSettings.ServidorWeb + "api/hogar/obtiene-edicion-persona",
                G_parametros = "{ token: '" + token + "', persona_id: persona_id }",
                G_respuesta_servicio = "$('.contenido-generico').html(respuesta[0].elemento_html);"
            };

            CallMethod _methodCallObtieneEdicionHogarPersona = new CallMethod
            {
                Mc_nombre = "ObtieneEdicionHogarPersona(persona_id)",
                Mc_contenido = _getJSONObtieneEdicionHogarPersona.GetJSONCall()
            };

            // Genero función para el eliminación de personas
            GetJSON _getJSONObtieneEliminacionHogarPersona = new GetJSON
            {
                G_url_servicio = _appSettings.ServidorWeb + "api/hogar/obtiene-eliminacion-persona",
                G_parametros = "{ token: '" + token + "', persona_id: persona_id }",
                G_respuesta_servicio = "$('.contenido-generico').html(respuesta[0].elemento_html);"
            };

            CallMethod _methodCallObtieneEliminacionHogarPersona = new CallMethod
            {
                Mc_nombre = "ObtieneEliminacionHogarPersona(persona_id)",
                Mc_contenido = _getJSONObtieneEliminacionHogarPersona.GetJSONCall()
            };

            if (DsPersonas.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in DsPersonas.Tables[0].Rows)
                {
                    sbDetalle.Append("<tr>");
                    sbDetalle.Append("<td class=\"col-lg-8 col-md-8\"><h3>" + dr["PER_NOMBRE"].ToString() + "</h3></td>");
                    sbDetalle.Append("<td><button type=\"button\" onclick=\"ObtieneEdicionHogarPersona('" + dr["PK_PERSONAS"].ToString() + "');\" class=\"btn btn-warning\" data-toggle=\"modal\" data-target=\"#modal-master\"><i class=\"fa fa-edit\"></i></button></td>");
                    sbDetalle.Append("<td><button type=\"button\" onclick=\"ObtieneEliminacionHogarPersona('" + dr["PK_PERSONAS"].ToString() + "');\" class=\"btn btn-danger\" data-toggle=\"modal\" data-target=\"#modal-master\"><i class=\"fa fa-trash\"></i></button></td>");
                    sbDetalle.Append("</tr>");
                }

                sbTabla.Append("<link rel=\"stylesheet\" href='" + _appSettings.ServidorWeb + "Framework/assets/css/plugins/dataTables/datatables.min.css'>");
                sbTabla.Append("<div class=\"alert alert-success table-responsive\">");
                sbTabla.Append("<table class=\"tabla-informante table text-center\">");
                sbTabla.Append("<tbody>");
                sbTabla.Append("<thead>");
                sbTabla.Append("<tr>");
                sbTabla.Append("<th>PERSONA</th>");
                sbTabla.Append("<th>EDITAR</th>");
                sbTabla.Append("<th>ELIMINAR</th>");
                sbTabla.Append("</tr>");
                sbTabla.Append("</thead>");
                sbTabla.Append(sbDetalle.ToString());
                sbTabla.Append("</tbody>");
                sbTabla.Append("</table>");
                sbTabla.Append("</div>");
                sb.Append(sbTabla.ToString());
                sb.Append(_methodCallObtieneEdicionHogarPersona.CreaJQueryFunction());
                sb.Append(_methodCallObtieneEliminacionHogarPersona.CreaJQueryFunction());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Obtiene Resúmen Sección Hogar Resumen de Personas Listado (Paso 1 Personas)
        /// </summary>
        public string ObtieneHogarResumenPersonasListado(string token, int paso, string cuestionario = "")
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbTabla = new StringBuilder();
            StringBuilder sbDetalle = new StringBuilder();
            Encrypt _encrypt = new Encrypt();

            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Hogar
            HogarBOL _hogarBOL = new HogarBOL();
            HogarDAL _hogarDAL = new HogarDAL();

            DataSet DsPersonas = new DataSet();
            _hogarBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            _hogarBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;

            DsPersonas = _hogarDAL.ListarPersonasPorHogar(_hogarBOL, true);

            int _contPerComp = 0;
            bool _listadoPersonas = false;
            if (DsPersonas.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in DsPersonas.Tables[0].Rows)
                {
                    string _idFormulario = "";
                    _idFormulario = _encrypt.EncryptString(_hogarBOL.PK_VIVIENDA + "&" + "" + _hogarBOL.PK_HOGAR + "&" + dr["PK_PERSONAS"] + "");

                    sbDetalle.Append("<tr>");
                    sbDetalle.Append("<td class=\"col-lg-8 col-md-8\"><button type=\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso + 1) + ",'" + _idFormulario + "');\" class=\"btn btn-primary btn-block\">" + dr["PER_NOMBRE"].ToString() + "</button></td>");
                    if (dr["PER9"].ToString() == "1")
                    {
                        sbDetalle.Append("<td><p style=\"color:#8bc34a\"><i class=\"fa fa-handshake-o\"></i> Completo</p></td>");
                        _contPerComp++;
                    }
                    else
                    {
                        sbDetalle.Append("<td><p class=\"text-warning\"><i class=\"fa fa-info\"></i> Pendiente</p></td>");
                    }
                    sbDetalle.Append("</tr>");
                }

                if (_contPerComp == DsPersonas.Tables[0].Rows.Count)
                {
                    _listadoPersonas = true;
                }

                // Identificación Vivienda-Hogar
                sb.Append(ObtieneIdentificacionHogar(token, _hogarBOL));

                sb.Append("<div class=\"row\">");
                sb.Append("<div class=\"col-lg-12  text-center\">");
                sb.Append("<div class=\"alert alert-warning\">");
                sb.Append("<h3>RESIDENTES HABITUALES EN EL HOGAR</h3>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</div>");

                sbTabla.Append("<div class=\"row\">");
                sbTabla.Append("<div class=\"col-lg-12 text-center\">");
                sbTabla.Append("<h1>Seleccione persona para continuar</h1>");
                sbTabla.Append("<hr />");
                sbTabla.Append("</div>");
                sbTabla.Append("</div>");

                sbTabla.Append("<link rel=\"stylesheet\" href='" + _appSettings.ServidorWeb + "Framework/assets/css/plugins/dataTables/datatables.min.css'>");
                sbTabla.Append("<div class=\"alert alert-success table-responsive\">");
                sbTabla.Append("<table class=\"tabla-informante table text-center\">");
                sbTabla.Append("<tbody>");
                sbTabla.Append("<thead>");
                sbTabla.Append("<tr>");
                sbTabla.Append("<th>PERSONA</th>");
                sbTabla.Append("<th>ESTADO</th>");
                sbTabla.Append("</tr>");
                sbTabla.Append("</thead>");
                sbTabla.Append(sbDetalle.ToString());
                sbTabla.Append("</tbody>");
                sbTabla.Append("</table>");

                if (_listadoPersonas == true)
                {
                    sbTabla.Append("<div class=\"row\">");

                    sbTabla.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
                    sbTabla.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso - 1) + ",'" + token + "');\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
                    sbTabla.Append("</div>");

                    sbTabla.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
                    sbTabla.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso + 2) + ",'" + token + "');\" class=\"btn btn-success btn-md btn-block\">Siguiente <i class=\"fa fa-chevron-right\"></i> </button>");
                    sbTabla.Append("</div>");

                    sbTabla.Append("</div>");
                }
                else
                {
                    sbTabla.Append("<div class=\"row\">");
                    sbTabla.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
                    sbTabla.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso - 1) + ",'" + token + "');\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
                    sbTabla.Append("</div>");
                    sbTabla.Append("</div>");
                }
            } else
            {
                sb.Append("<div class=\"row\">");
                sb.Append("<div class=\"col-lg-12  text-center\">");
                sb.Append("<div class=\"alert alert-danger\">");
                sb.Append("<h3>NO HAY REGISTROS DE RESIDENTES HABITUALES EN EL HOGAR</h3>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</div>");

                sb.Append("<div class=\"row\">");
                sbTabla.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
                sbTabla.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso - 1) + ",'" + token + "');\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
                sbTabla.Append("</div>");
                sb.Append("</div>");
            }

            sb.Append(sbTabla.ToString());

            return sb.ToString();
        }

        /// <summary>
        /// Obtiene Resúmen Sección Hogar Resumen de Personas
        /// </summary>
        public string ObtieneHogarResumenPersonas(string token, int paso, string cuestionario = "")
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbTabla = new StringBuilder();
            StringBuilder sbDetalle = new StringBuilder();
            Encrypt _encrypt = new Encrypt();

            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Hogar
            HogarBOL _hogarBOL = new HogarBOL();
            HogarDAL _hogarDAL = new HogarDAL();

            DataSet DsPersonas = new DataSet();
            _hogarBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            _hogarBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;

            DsPersonas = _hogarDAL.ListarPersonasPorHogar(_hogarBOL, true);

            if (DsPersonas.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in DsPersonas.Tables[0].Rows)
                {
                    string _idFormulario = "";
                    _idFormulario = _encrypt.EncryptString(_hogarBOL.PK_VIVIENDA + "&" + "" + _hogarBOL.PK_HOGAR + "&" + dr["PK_PERSONAS"] + "");

                    sbDetalle.Append("<tr>");
                    sbDetalle.Append("<td class=\"col-lg-8 col-md-8\"><button type=\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso + 1) + ",'" + _idFormulario + "');\" class=\"btn btn-primary btn-block\">" + dr["PER_NOMBRE"].ToString() + "</button></td>");
                    if (dr["PER10"].ToString() == "1")
                    {
                        sbDetalle.Append("<td><p style=\"color:#8bc34a\"><i class=\"fa fa-handshake-o\"></i> Completo</p></td>");
                    }
                    else
                    {
                        sbDetalle.Append("<td><p class=\"text-warning\"><i class=\"fa fa-info\"></i> Pendiente</p></td>");
                    }
                    sbDetalle.Append("</tr>");
                }

                // Identificación Vivienda-Hogar
                sb.Append(ObtieneIdentificacionHogar(token, _hogarBOL));

                sb.Append("<div class=\"row\">");
                sb.Append("<div class=\"col-lg-12  text-center\">");
                sb.Append("<div class=\"alert alert-warning\">");
                sb.Append("<h3>RESIDENTES HABITUALES EN EL HOGAR</h3>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</div>");

                sbTabla.Append("<div class=\"row\">");
                sbTabla.Append("<div class=\"col-lg-12 text-center\">");
                sbTabla.Append("<h1>Seleccione persona para continuar</h1>");
                sbTabla.Append("<hr />");
                sbTabla.Append("</div>");
                sbTabla.Append("</div>");

                sbTabla.Append("<link rel=\"stylesheet\" href='" + _appSettings.ServidorWeb + "Framework/assets/css/plugins/dataTables/datatables.min.css'>");
                sbTabla.Append("<div class=\"alert alert-success table-responsive\">");
                sbTabla.Append("<table class=\"tabla-informante table text-center\">");
                sbTabla.Append("<tbody>");
                sbTabla.Append("<thead>");
                sbTabla.Append("<tr>");
                sbTabla.Append("<th>PERSONA</th>");
                sbTabla.Append("<th>ESTADO</th>");
                sbTabla.Append("</tr>");
                sbTabla.Append("</thead>");
                sbTabla.Append(sbDetalle.ToString());
                sbTabla.Append("</tbody>");
                sbTabla.Append("</table>");

                sbTabla.Append("<div class=\"row\">");
                sbTabla.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
                sbTabla.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso - 2) + ",'" + token + "');\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
                sbTabla.Append("</div>");
                sbTabla.Append("</div>");

                sbTabla.Append("</div>");
            }

            sb.Append(sbTabla.ToString());

            return sb.ToString();
        }
    }
}
