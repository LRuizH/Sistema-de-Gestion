using System;
using System.Collections.Generic;
using System.Text;
using Framework.BOL;
using Framework.BLL.Utilidades.Ajax;
using Framework.BLL.Utilidades.Seguridad;
using Framework.DAL;
using System.Data;

namespace Framework.BLL.Componentes.ModuloCuestionario
{
    public class CVivienda
    {
        AppSettings _appSettings = new AppSettings();

        /// <summary>
        /// Obtiene formulario Sección Información Direccion
        /// </summary>
        public string ObtieneViviendaInformacionDireccion(string token, int paso, string cuestionario = "")
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Vivienda
            ViviendaBOL _viviendaBOL = new ViviendaBOL();
            ViviendaDAL _viviendaDAL = new ViviendaDAL();

            _viviendaBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            List<ViviendaBOL> listaVivienda = _viviendaDAL.Listar<ViviendaBOL>(_viviendaBOL);
            if (listaVivienda.Count > 0)
            {
                _viviendaBOL = listaVivienda[0];
            }

            // Obtengo información de Dirección
            DataSet dsViviendaDireccion = new DataSet();
            dsViviendaDireccion = _viviendaDAL.ListarDireccion(_viviendaBOL.PK_VIVIENDA);

            string _region = "";
            string _comuna = "";
            string _direccion = "";
            string _nDepto = "";
            if (dsViviendaDireccion.Tables[0].Rows.Count > 0)
            {
                _region = dsViviendaDireccion.Tables[0].Rows[0]["Region"].ToString();
                _comuna = dsViviendaDireccion.Tables[0].Rows[0]["Comuna"].ToString();
                _direccion = dsViviendaDireccion.Tables[0].Rows[0]["DescripcionDirSec"].ToString();
                _nDepto = "NULL";
                if (_nDepto == "NULL")
                {
                    _nDepto = "No aplica";
                }
            }

            if (_region != "nok")
            {
                // Submit del formulario
                _postJSON.P_form = "formulario-vivienda";
                _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
                _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/vivienda/ingresar-datos-direccion";
                _postJSON.P_data_dinamica = true;
                _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioWeb(" + (paso + 1) + ",'" + token + "'); }";

                // Inicio Definición del Formulario
                sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
                sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
                sb.Append("<input id=\"paso_formulario\" name=\"paso_formulario\" type=\"hidden\" value=\"" + paso + "\"/>");
                sb.Append("<div class=\"row\">");

                sb.Append("<div class=\"col-lg-12\">");

                // Inicio Linea 1
                sb.Append("<div class=\"row\">");

                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
                sb.Append("<p style=\"margin-bottom:-2px;\"><strong>INFORMACIÓN DE LA VIVIENDA</strong></p>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<br>");
                sb.Append("</div>");

                sb.Append("<div class=\"col-lg-12 col-md-12\">");
                sb.Append("<div class=\"alert alert-warning\">");
                sb.Append("<br>");
                sb.Append("<div class=\"col-lg-12 col-md-12\">");
                sb.Append("<div class=\"form-group\">");
                sb.Append("<p>1) REGIÓN: <strong>" + _region + "</strong></p>");
                sb.Append("</div>");
                sb.Append("</div>");

                sb.Append("<div class=\"col-lg-12 col-md-12\">");
                sb.Append("<div class=\"form-group\">");
                sb.Append("<p>2) COMUNA: <strong>" + _comuna + "</strong></p>");
                sb.Append("</div>");
                sb.Append("</div>");

                sb.Append("<div class=\"col-lg-12 col-md-12\">");
                sb.Append("<div class=\"form-group\">");
                sb.Append("<p>3) DIRECCIÓN: <strong>" + _direccion + "</strong></p>");
                sb.Append("</div>");
                sb.Append("</div>");

                sb.Append("<div class=\"col-lg-12 col-md-12\">");
                sb.Append("<div class=\"form-group\">");
                sb.Append("<p>4) N° DE DEPARTAMENTO: <strong>" + _nDepto + "</strong></p>");
                sb.Append("</div>");
                sb.Append("</div>");

                sb.Append("</div>");
                sb.Append("</div>");

                sb.Append("<div class=\"col-lg-12 col-md-12\">");
                sb.Append("<div class=\"form-group\">");
                sb.Append("<p><strong>¿LA DIRECCIÓN PRECARGADA CORRESPONDE A LA DE SU VIVIENDA?</strong></p>");

                sb.Append("<div class=\"clase_control_ID0\">");
                if (_viviendaBOL.ID0 == "1")
                {
                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<input id=\"rbt_opt0_1\" class=\"magic-radio\" type=\"radio\" name=\"ID0\" value=\"1\" checked=\"checked\" required>");
                    sb.Append("<label for=\"rbt_opt0_1\" style=\"display: inline;\">&nbsp;Si</label>");
                    sb.Append("</div>");
                }
                else
                {
                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<input id=\"rbt_opt0_1\" class=\"magic-radio\" type=\"radio\" name=\"ID0\" value=\"1\" required>");
                    sb.Append("<label for=\"rbt_opt0_1\" style=\"display: inline;\">&nbsp;Si</label>");
                    sb.Append("</div>");
                }

                if (_viviendaBOL.ID0 == "0")
                {
                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<input id=\"rbt_opt0_2\" class=\"magic-radio\" type=\"radio\" name=\"ID0\" value=\"0\" checked=\"checked\">");
                    sb.Append("<label for=\"rbt_opt0_2\" style=\"display: inline;\">&nbsp;No</label>");
                    sb.Append("</div>");
                }
                else
                {
                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<input id=\"rbt_opt0_2\" class=\"magic-radio\" type=\"radio\" name=\"ID0\" value=\"0\">");
                    sb.Append("<label for=\"rbt_opt0_2\" style=\"display: inline;\">&nbsp;No</label>");
                    sb.Append("</div>");
                }
                sb.Append("</div>");

                sb.Append("</div>");
                sb.Append("</div>");

                sb.Append("</div>");
                // Fin Linea 1

                sb.Append("</div>");
                sb.Append("</div>");

                // Inicio Botones del Cuestionario
                sb.Append("<div class=\"row text-center\">");
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<hr />");
                sb.Append("<div class=\"mensaje text-center\"></div>");
                sb.Append("</div>");
                sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
                sb.Append("</div>");
                sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
                sb.Append("<button type =\"submit\" class=\"btn btn-success btn-md btn-block\"><i class=\"fa fa-floppy-o\"></i> Guardar y continuar</button>");
                sb.Append("</div>");
                sb.Append("</div>");
                // Fin Botones del Cuestionario

                sb.Append("</form>");
                // Fin Definición del Formulario
            }
            else
            {
                sb.Append("<div class=\"alert alert-danger text-center\">");
                sb.Append("<h2>Estimado Usuario, intente nuevamente en unos minutos, muchas gracias.</h2>");
                sb.Append("</div>");
            }

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
        /// Obtiene formulario Sección Información Informante
        /// </summary>
        public string ObtieneViviendaInformacionInformante(string token, int paso, string cuestionario = "")
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Vivienda
            ViviendaBOL _viviendaBOL = new ViviendaBOL();
            ViviendaDAL _viviendaDAL = new ViviendaDAL();

            _viviendaBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            List<ViviendaBOL> listaVivienda = _viviendaDAL.Listar<ViviendaBOL>(_viviendaBOL);
            if (listaVivienda.Count > 0)
            {
                _viviendaBOL = listaVivienda[0];
            }

            CallMethod _methodCallLoad = new CallMethod();

            // Obtengo opciones de respuesta
            StringBuilder sbV1 = new StringBuilder();
            StringBuilder sbV1Select = new StringBuilder();

            if (_viviendaBOL.ID0 == "1")
            {
                // Carga opciones de respuesta
                GesFormPreguntasOpcionesBOL _gesFormPreguntasOpcionesBOL = new GesFormPreguntasOpcionesBOL();
                GesFormPreguntasOpcionesDAL _gesFormPreguntasOpcionesDAL = new GesFormPreguntasOpcionesDAL();
                List<GesFormPreguntasOpcionesBOL> listaOpcionesPregunta = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesPreguntaPorGrupos<GesFormPreguntasOpcionesBOL>("'1'");

                foreach (var item in listaOpcionesPregunta)
                {
                    switch (item.Gpf_codigo_pregunta)
                    {
                        case "V1":
                            if (item.Fpo_numero.ToString() == _viviendaBOL.V1.ToString())
                            {
                                sbV1Select.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                            }
                            else
                            {
                                sbV1Select.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                            }
                            break;
                    }
                }

                // Submit del formulario
                _postJSON.P_form = "formulario-vivienda";
                _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
                _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/vivienda/ingresar-datos-informante";
                _postJSON.P_data_dinamica = true;
                _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioWeb(" + (paso + 1) + ",'" + token + "'); }";

                // Inicio Definición del Formulario
                sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
                sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
                sb.Append("<input id=\"paso_formulario\" name=\"paso_formulario\" type=\"hidden\" value=\"" + paso + "\"/>");
                sb.Append("<div class=\"row\">");

                sb.Append("<div class=\"col-lg-12\">");

                // Inicio Linea 1 (Pregunta 1 y 1.1)
                sb.Append("<div class=\"row\">");

                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
                sb.Append("<p style=\"margin-bottom:-2px;\"><strong>DATOS DEL USUARIO</strong></p>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<br>");
                sb.Append("</div>");

                sb.Append("<div class=\"col-lg-4 col-md-4\">");
                sb.Append("<div class=\"form-group\">");
                sb.Append("<p><strong>NOMBRE COMPLETO</strong></p>");
                sb.Append("<input id=\"ID1\" name=\"ID1\" type=\"text\" class=\"form-control\" placeholder=\"NOMBRE\" value=\"" + _viviendaBOL.ID1 + "\" required />");
                sb.Append("</div>");
                sb.Append("</div>");

                sb.Append("<div class=\"col-lg-4 col-md-4\">");
                sb.Append("<div class=\"form-group\">");
                sb.Append("<p><strong>TELÉFONO</strong></p>");
                sb.Append("<input id=\"ID2\" name=\"ID2\" type=\"text\" class=\"form-control\" onKeyPress=\"if (this.value.length == 9) return false; return event.charCode >= 48 && event.charCode <= 57;\" placeholder=\"TELÉFONO\" value=\"" + _viviendaBOL.ID2 + "\" required />");
                sb.Append("</div>");
                sb.Append("</div>");

                sb.Append("<div class=\"col-lg-4 col-md-4\">");
                sb.Append("<div class=\"form-group\">");
                sb.Append("<p><strong>CORREO ELECTRÓNICO</strong></p>");
                sb.Append("<input id=\"ID3\" name=\"ID3\" type=\"email\" class=\"form-control\" placeholder=\"CORREO ELECTRÓNICO\" value=\"" + _viviendaBOL.ID3 + "\" required />");
                sb.Append("</div>");
                sb.Append("</div>");

                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<div class=\"form-group\">");
                sb.Append("<p><strong>INDIQUE EL TIPO DE VIVIENDA PARTICULAR</strong></p>");
                sb.Append("<select id=\"V1\" name=\"V1\" class=\"form-control\" data-width=\"100%\" required>");
                sb.Append("<option value=\"\">Seleccione opción...</option>");
                sb.Append(sbV1Select.ToString());
                sb.Append("</select>");
                sb.Append("</div>");
                sb.Append("</div>");

                sb.Append("<div class=\"col-lg-12 V1_Otro\">");
                sb.Append("<div class=\"form-group\">");
                sb.Append("<p><strong>ESPECIFIQUE EL OTRO TIPO DE VIVIENDA PARTICULAR</strong></p>");
                sb.Append("<input id=\"V1_OTRO\" name=\"V1_OTRO\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OTRO\" value=\"" + _viviendaBOL.V1_OTRO + "\" required />");
                sb.Append("</div>");
                sb.Append("</div>");

                sb.Append("</div>");
                // Fin Linea 1 (Pregunta 1 y 1.1)

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
                _methodCallLoad.Mc_contenido = _postJSON.PostJSONCall() +
                                               "$('.magic-radio').iCheck({" +
                                                    "checkboxClass: 'icheckbox_square-green'," +
                                                    "radioClass: 'iradio_square-green'," +
                                                    "increaseArea: '20%'" +
                                               "});";
            }
            else
            {
                sb.Append("<div class=\"row\">");

                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<div class=\"alert alert-info\">");
                sb.Append("<h2 class=\"text-center\">Como la informaciónde la dirección de tu vivienda no coincide.<br />Contáctanos a nuestra mesa de ayuda.</h2>");
                sb.Append("</div>");
                sb.Append("</div>");

                sb.Append("<div class=\"col-lg-4 col-md-4 col-sm-4 col-xs-12\">");
                sb.Append("</div>");
                sb.Append("<div class=\"col-lg-4 col-md-4 col-sm-4 col-xs-12\">");
                sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso - 1) + ",'" + token + "');\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
                sb.Append("</div>");

                sb.Append("</div>");
            }


            return sb.ToString() + _methodCallLoad.CreaJQueryDocumentReady();
        }

        /// <summary>
        /// Obtiene formulario Sección Vivienda Tipo de Vivienda
        /// </summary>
        public string ObtieneViviendaMaterialidadInfraestructuraVivienda(string token, int paso, string cuestionario = "")
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Vivienda
            ViviendaBOL _viviendaBOL = new ViviendaBOL();
            ViviendaDAL _viviendaDAL = new ViviendaDAL();

            _viviendaBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            List<ViviendaBOL> listaVivienda = _viviendaDAL.Listar<ViviendaBOL>(_viviendaBOL);
            if (listaVivienda.Count > 0)
            {
                _viviendaBOL = listaVivienda[0];
            }

            // Carga opciones de respuesta
            GesFormPreguntasOpcionesBOL _gesFormPreguntasOpcionesBOL = new GesFormPreguntasOpcionesBOL();
            GesFormPreguntasOpcionesDAL _gesFormPreguntasOpcionesDAL = new GesFormPreguntasOpcionesDAL();
            List<GesFormPreguntasOpcionesBOL> listaOpcionesPregunta = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesPreguntaPorGrupos<GesFormPreguntasOpcionesBOL>("'3','5','7','9','11','13'");

            // Obtengo opciones de respuesta
            StringBuilder sbMAT1 = new StringBuilder();
            StringBuilder sbMAT3 = new StringBuilder();
            StringBuilder sbMAT5 = new StringBuilder();
            StringBuilder sbMAT7 = new StringBuilder();
            StringBuilder sbMAT9 = new StringBuilder();
            StringBuilder sbMAT11 = new StringBuilder();

            int i = 1;
            foreach (var item in listaOpcionesPregunta)
            {
                switch (item.Gpf_codigo_pregunta)
                {
                    case "MAT1":
                        if (item.Fpo_numero.ToString() == _viviendaBOL.MAT1.ToString())
                        {
                            sbMAT1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbMAT1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case "MAT3":
                        if (item.Fpo_numero.ToString() == _viviendaBOL.MAT3.ToString())
                        {
                            sbMAT3.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbMAT3.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case "MAT5":
                        if (item.Fpo_numero.ToString() == _viviendaBOL.MAT5.ToString())
                        {
                            sbMAT5.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbMAT5.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case "MAT7":
                        if (item.Fpo_numero.ToString() == _viviendaBOL.MAT7.ToString())
                        {
                            sbMAT7.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbMAT7.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case "MAT9":
                        if (item.Fpo_numero.ToString() == _viviendaBOL.MAT9.ToString())
                        {
                            sbMAT9.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbMAT9.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case "MAT11":
                        if (i == 1)
                        {
                            if (_viviendaBOL.MAT11 == "1")
                            {
                                sbMAT11.Append("<input id=\"MAT11_" + i + "\"  name=\"MAT11_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbMAT11.Append("<input id=\"MAT11_" + i + "\"  name=\"MAT11_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        if (i == 2)
                        {
                            if (_viviendaBOL.MAT12 == "1")
                            {
                                sbMAT11.Append("<input id=\"MAT11_" + i + "\"  name=\"MAT11_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbMAT11.Append("<input id=\"MAT11_" + i + "\"  name=\"MAT11_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        if (i == 3)
                        {
                            if (_viviendaBOL.MAT13 == "1")
                            {
                                sbMAT11.Append("<input id=\"MAT11_" + i + "\"  name=\"MAT11_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbMAT11.Append("<input id=\"MAT11_" + i + "\"  name=\"MAT11_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        if (i == 4)
                        {
                            if (_viviendaBOL.MAT14 == "1")
                            {
                                sbMAT11.Append("<input id=\"MAT11_" + i + "\"  name=\"MAT11_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbMAT11.Append("<input id=\"MAT11_" + i + "\"  name=\"MAT11_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        if (i == 5)
                        {
                            if (_viviendaBOL.MAT15 == "1")
                            {
                                sbMAT11.Append("<input id=\"MAT11_" + i + "\"  name=\"MAT11_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbMAT11.Append("<input id=\"MAT11_" + i + "\"  name=\"MAT11_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        if (i == 6)
                        {
                            if (_viviendaBOL.MAT16 == "1")
                            {
                                sbMAT11.Append("<input id=\"MAT11_" + i + "\"  name=\"MAT11_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbMAT11.Append("<input id=\"MAT11_" + i + "\"  name=\"MAT11_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        if (i == 7)
                        {
                            if (_viviendaBOL.MAT17 == "1")
                            {
                                sbMAT11.Append("<input id=\"MAT11_" + i + "\"  name=\"MAT11_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbMAT11.Append("<input id=\"MAT11_" + i + "\"  name=\"MAT11_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        sbMAT11.Append("<span class=\"text-inverse\"> " + item.Fpo_glosa_primaria + "</span><br />");
                        i++;
                        break;
                }
            }

            // Submit del formulario
            _postJSON.P_form = "formulario-vivienda";
            _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
            _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/vivienda/ingresar-materialidad-vivienda";
            _postJSON.P_data_dinamica = true;
            _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioWeb(" + (paso + 1) + ",'" + token + "'); }";

            // Inicio Definición del Formulario
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
            sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
            sb.Append("<input id=\"paso_formulario\" name=\"paso_formulario\" type=\"hidden\" value=\"" + paso + "\"/>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");

            // Inicio Linea 1 (Pregunta 1 y 1.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
            sb.Append("<p style=\"margin-bottom:-2px;\">&nbsp;&nbsp;<strong>MATERIALIDAD E INFRAESTRUCTURA DE LA VIVIENDA</strong></p>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<br>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>1.- ¿CUÁL ES EL PRINCIPAL MATERIAL UTILIZADO EN EL REVESTIMIENTO O FORRO DE LAS PAREDES HACIA EL EXTERIOR?</strong><br />Considere el material predominante o que se encuentre en mayor proporción en el revestimiento hacia el exterior de la vivienda.</p>");
            sb.Append("<select id=\"MAT1\" name=\"MAT1\" class=\"form-control\" data-width=\"100%\">");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbMAT1.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 MAT2_Otro\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>1.1.- ESPECIFIQUE EL OTRO TIPO DE MATERIAL UTILIZADO EN EL REVISTIMIENTO O FORRO DE LAS PAREDES HACIA EL EXTERIOR</strong></p>");
            sb.Append("<input id=\"MAT2\" name=\"MAT2\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OTRO\" value=\"" + _viviendaBOL.MAT2 + "\" disabled />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 1 (Pregunta 1 y 1.1)

            // Inicio Linea 2 (Pregunta 2 y 2.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>2.- ¿CUÁL ES EL PRINCIPAL MATERIAL UTILIZADO EN EL REVESTIMIENTO O FORRO DE LAS PAREDES HACIA EL INTERIOR?</strong><br />Considere el material predominante o que se encuentre en mayor proporción en el revestimiento hacia el interior de la vivienda.</p>");
            sb.Append("<select id=\"MAT3\" name=\"MAT3\" class=\"form-control\" data-width=\"100%\">");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbMAT3.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 MAT4_Otro\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>2.1.- ESPECIFIQUE EL OTRO TIPO DE MATERIAL UTILIZADO EN EL REVISTIMIENTO O FORRO DE LAS PAREDES HACIA EL INTERIOR</strong></p>");
            sb.Append("<input id=\"MAT4\" name=\"MAT4\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OTRO\" value=\"" + _viviendaBOL.MAT4 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 2 (Pregunta 2 y 2.1)

            // Inicio Linea 3 (Pregunta 3 y 3.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>3.- ¿CUÁL ES EL PRINCIPAL MATERIAL UTILIZADO EN LA ESTRUCTURA DE LAS PAREDES?</strong><br />Refiere al material que soporta la estructura de la vivienda. Considere el material predominante o que se encuentre en mayor proporción.</p>");
            sb.Append("<select id=\"MAT5\" name=\"MAT5\" class=\"form-control\" data-width=\"100%\">");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbMAT5.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 MAT6_Otro\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>3.1.- ESPECIFIQUE EL OTRO MATERIAL UTILIZADO EN LA ESTRUCTURA DE LAS PAREDES</strong></p>");
            sb.Append("<input id=\"MAT6\" name=\"MAT6\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OTRO\" value=\"" + _viviendaBOL.MAT6 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 3 (Pregunta 3 y 3.1)

            // Inicio Linea 4 (Pregunta 4 y 4.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>4.- ¿CUÁL ES EL PRINCIPAL MATERIAL DE CONSTRUCCIÓN EN LA CUBIERTA DEL TECHO?</strong><br />Considere el material predominante o que se encuentra en mayor proporción en el techo. En departamentos, por “cubierta del techo” se considera el material que reviste el nivel más alto del edificio.</p>");
            sb.Append("<select id=\"MAT7\" name=\"MAT7\" class=\"form-control\" data-width=\"100%\">");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbMAT7.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 MAT8_Otro\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>4.1.- ESPECIFIQUE EL OTRO MATERIAL DE CONSTRUCCIÓN EN LA CUBIERTA DEL TECHO</strong></p>");
            sb.Append("<input id=\"MAT8\" name=\"MAT8\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OTRO\" value=\"" + _viviendaBOL.MAT8 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 4 (Pregunta 4 y 4.1)

            // Inicio Linea 5 (Pregunta 5 y 5.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>5.- ¿CUÁL ES EL PRINCIPAL MATERIAL DE CONSTRUCCIÓN EN EL PISO?</strong><br />Considere el material predominante o que se encuentra en mayor proporción en el piso.</p>");
            sb.Append("<select id=\"MAT9\" name=\"MAT9\" class=\"form-control\" data-width=\"100%\">");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbMAT9.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 MAT10_Otro\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>5.1.- ESPECIFIQUE EL OTRO MATERIAL DE CONSTRUCCIÓN EN EL PISO</strong></p>");
            sb.Append("<input id=\"MAT10\" name=\"MAT10\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OTRO\" value=\"" + _viviendaBOL.MAT10 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 5 (Pregunta 5 y 5.1)

            // Inicio Linea 6 (Pregunta 6)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>6.- EN SU VIVIENDA, ¿SE PRESENTA ALGUNAS DE LAS SIGUIENTES SITUACIONES?</strong></p>");
            sb.Append(sbMAT11.ToString());
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 6 (Pregunta 6)

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
        /// Obtiene formulario Sección Servicios Basicos
        /// </summary>
        public string ObtieneViviendaServiciosBasicos(string token, int paso, string cuestionario = "")
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Vivienda
            ViviendaBOL _viviendaBOL = new ViviendaBOL();
            ViviendaDAL _viviendaDAL = new ViviendaDAL();

            _viviendaBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            List<ViviendaBOL> listaVivienda = _viviendaDAL.Listar<ViviendaBOL>(_viviendaBOL);
            if (listaVivienda.Count > 0)
            {
                _viviendaBOL = listaVivienda[0];
            }

            // Carga opciones de respuesta
            GesFormPreguntasOpcionesBOL _gesFormPreguntasOpcionesBOL = new GesFormPreguntasOpcionesBOL();
            GesFormPreguntasOpcionesDAL _gesFormPreguntasOpcionesDAL = new GesFormPreguntasOpcionesDAL();
            List<GesFormPreguntasOpcionesBOL> listaOpcionesPregunta = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesPreguntaPorGrupos<GesFormPreguntasOpcionesBOL>("'14','15','16','17','19','20','21','22'");

            // Obtengo opciones de respuesta
            StringBuilder sbSER1 = new StringBuilder();
            StringBuilder sbSER2 = new StringBuilder();
            StringBuilder sbSER3 = new StringBuilder();
            StringBuilder sbSER4 = new StringBuilder();
            StringBuilder sbSER6 = new StringBuilder();
            StringBuilder sbSER7 = new StringBuilder();
            StringBuilder sbSER8 = new StringBuilder();
            StringBuilder sbSER9 = new StringBuilder();

            foreach (var item in listaOpcionesPregunta)
            {
                switch (item.Gpf_codigo_pregunta)
                {
                    case "SER1":
                        if (item.Fpo_numero.ToString() == _viviendaBOL.SER1.ToString())
                        {
                            sbSER1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbSER1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case "SER2":
                        if (item.Fpo_numero.ToString() == _viviendaBOL.SER2.ToString())
                        {
                            sbSER2.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbSER2.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case "SER3":
                        if (item.Fpo_numero.ToString() == _viviendaBOL.SER3.ToString())
                        {
                            sbSER3.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbSER3.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case "SER4":
                        if (item.Fpo_numero.ToString() == _viviendaBOL.SER4.ToString())
                        {
                            sbSER4.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbSER4.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case "SER6":
                        if (item.Fpo_numero.ToString() == _viviendaBOL.SER6.ToString())
                        {
                            sbSER6.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbSER6.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case "SER7":
                        if (item.Fpo_numero.ToString() == _viviendaBOL.SER7.ToString())
                        {
                            sbSER7.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbSER7.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case "SER8":
                        if (item.Fpo_numero.ToString() == _viviendaBOL.SER8.ToString())
                        {
                            sbSER8.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbSER8.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case "SER9":
                        if (item.Fpo_numero.ToString() == _viviendaBOL.SER9.ToString())
                        {
                            sbSER9.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbSER9.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                }
            }

            // Submit del formulario
            _postJSON.P_form = "formulario-vivienda";
            _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
            _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/vivienda/ingresar-servicios-vivienda";
            _postJSON.P_data_dinamica = true;
            _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioWeb(" + (paso + 1) + ",'" + token + "'); }";

            // Inicio Definición del Formulario 
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
            sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
            sb.Append("<input id=\"paso_formulario\" name=\"paso_formulario\" type=\"hidden\" value=\"" + paso + "\"/>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");

            // Inicio Linea 1 (Pregunta 7)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
            sb.Append("<p style=\"margin-bottom:-2px;\"><strong>SERVICIOS BÁSICOS DE LA VIVIENDA</strong></p>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<br>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>7.- ¿CUÁNTAS PIEZAS SE USAN EXCLUSIVAMENTE COMO DORMITORIO?</strong><br />No considere aquellas piezas que también se usan como cocina, comedor o living.<br />En el caso de loft, estudios, monoambientes o mediaguas sin divisiones, debe seleccionar categoría \"0\".</p>");
            sb.Append("<select id=\"SER1\" name=\"SER1\" class=\"form-control\" data-width=\"100%\">");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbSER1.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 1 (Pregunta 8)

            // Inicio Linea 2 (Pregunta 8 y 9)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>8- ¿DE DÓNDE PROVIENE PRINCIPALMENTE EL AGUA QUE USA EN SU VIVIENDA?</strong><br />Considere el agua para uso doméstico de la vivienda.</p>");
            sb.Append("<select id=\"SER2\" name=\"SER2\" class=\"form-control\" data-width=\"100%\">");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbSER2.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>9.- El AGUA A SU VIVIENDA LLEGA PRINCIPALMENTE</strong></p>");
            sb.Append("<select id=\"SER3\" name=\"SER3\" class=\"form-control\" data-width=\"100%\">");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbSER3.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 2 (Pregunta 8 y 9)

            // Inicio Linea 3 (Pregunta 10 y 10.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>10.- ¿DE DÓNDE PROVIENE PRINCIPALMENTE LA ELECTRINICAD QUE USA EN SU VIVIENDA?</strong><br />Considere la fuente que constituye la base del sistema o que aporta la mayor cantidad de electricidad a la vivienda.</p>");
            sb.Append("<select id=\"SER4\" name=\"SER4\" class=\"form-control\" data-width=\"100%\">");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbSER4.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 SER5_Otro\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>10.1- ESPECIFIQUE DE DÓNDE PROVIENE LA ELECTRICIDAD QUE USA EN SU VIVIENDA</strong></p>");
            sb.Append("<input id=\"SER5\" name=\"SER5\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OTRO\" value=\"" + _viviendaBOL.SER5 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 3 (Pregunta 10 y 10.1)

            // Inicio Linea 4 (Pregunta 11 y 12)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>11.- ¿CUÁNTOS INODOROS O WC TIENE SU VIVIENDA?</strong><br />Se deben contabilizar letrinas aun cuando no se encuentren conectadas a alcantarilla.<br />Considere tanto los inodoros que están dentro y fuera de la vivienda, siempre y cuando sean de uso exclusivo para los residentes de la vivienda.</p>");
            sb.Append("<select id=\"SER6\" name=\"SER6\" class=\"form-control\" data-width=\"100%\">");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbSER6.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>12.- EL INODORO O WC SE ENCUENTRA</strong><br />Si el servicio higiénico se encuentra fuera de la vivienda, pero dentro del sitio, indique que la vivienda sí cuenta con servicios higiénicos y registre la alternativa correspondiente a su conexión.</p>");
            sb.Append("<select id=\"SER7\" name=\"SER7\" class=\"form-control\" data-width=\"100%\">");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbSER7.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 4 (Pregunta 11 y 12)

            // Inicio Linea 5 (Pregunta 13)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>13.- ¿CUÁNTAS DUCHAS TIENE SU VIVIENDA</strong><br />Considere todos los sistemas de caída de agua, aun cuando sean tinas. Cuente sólo las duchas que sean de uso exclusivo para los residentes de la vivienda.</p>");
            sb.Append("<select id=\"SER8\" name=\"SER8\" class=\"form-control\" data-width=\"100%\">");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbSER8.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 5 (Pregunta 13)

            // Inicio Linea 6 (Pregunta 14 Y 15)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>14.- ¿CUÁL ES EL PRINCIPAL MEDIO DE ELIMINACIÓN DE BASURA?</strong><br />En el caso de basureros comunes o uso de “shafts” en edificios, seleccione la primera categoría.</p>");
            sb.Append("<select id=\"SER9\" name=\"SER9\" class=\"form-control\" data-width=\"100%\">");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbSER9.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 SER10_Otro\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>14.1.- ESPECIFIQUE EL OTRO MEDIO DE ELIMINACIÓN DE BASURA</strong></p>");
            sb.Append("<input id=\"SER10\" name=\"SER10\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OTRO\" value=\"" + _viviendaBOL.SER10 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 12 (Pregunta 15 Y 15.1)

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
        /// Obtiene formulario Sección Servicios Identificación Hogares
        /// </summary>
        public string ObtieneViviendaIdentificacionHogares(string token, int paso, string cuestionario = "")
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Vivienda
            ViviendaBOL _viviendaBOL = new ViviendaBOL();
            ViviendaDAL _viviendaDAL = new ViviendaDAL();

            _viviendaBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            List<ViviendaBOL> listaVivienda = _viviendaDAL.Listar<ViviendaBOL>(_viviendaBOL);
            if (listaVivienda.Count > 0)
            {
                _viviendaBOL = listaVivienda[0];
            }

            // Submit del formulario
            _postJSON.P_form = "formulario-vivienda";
            _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
            _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/vivienda/ingresar-identificacion-hogar";
            _postJSON.P_data_dinamica = true;
            _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioWeb(" + (paso + 1) + ",'" + token + "'); }";

            // Inicio Definición del Formulario 
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
            sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
            sb.Append("<input id=\"paso_formulario\" name=\"paso_formulario\" type=\"hidden\" value=\"" + paso + "\"/>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");

            // Inicio Linea 1 (Pregunta 15)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
            sb.Append("<p style=\"margin-bottom:-2px;\"><strong>IDENTIFICACIÓN DE HOGARES</strong></p>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<br>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>15.- ¿CUÁNTAS PERSONAS RESIDEN HABITUALMENTE EN SU VIVIENDA?</strong><br />Considere a todas las personas: niñas y niños pequeños, personas mayores y personas postradas. Incluya a quienes han vivido al menos seis meses en el último año o, llevando menos tiempo, tienen intención de permanecer los próximos seis meses en la vivienda.</p>");
            sb.Append("<input id=\"IHOG1\" name=\"IHOG1\" type=\"number\" class=\"form-control\" min=\"1\" max =\"99\" onKeyPress=\"if (this.value.length == 2) return false; return event.charCode >= 48 && event.charCode <= 57;\" placeholder=\"N°\" value=\"" + _viviendaBOL.IHOG1 + "\" required />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 1 (Pregunta 14)

            // Inicio Linea 2 (Pregunta 15.1 y 15.2)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>15.1 DE LAS PERSONAS QUE RESIDEN HABITUALMENTE EN SU VIVIENDA, ¿TODAS COMPARTEN LOS GASTOS DE ALIMENTACIÓN?</strong><br />Compartir gastos implica que todas las personas se benefician de un mismo presupuesto para alimentos, independiente de si aportan ingresos.</p>");

            sb.Append("<div class=\"clase_control_IHOG2\">");
            if (_viviendaBOL.IHOG2 == "1")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt15_1\" class=\"magic-radio\" type=\"radio\" name=\"IHOG2\" value=\"1\" checked=\"checked\">");
                sb.Append("<label for=\"rbt_opt15_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt15_1\" class=\"magic-radio\" type=\"radio\" name=\"IHOG2\" value=\"1\">");
                sb.Append("<label for=\"rbt_opt15_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }

            if (_viviendaBOL.IHOG2 == "0")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt15_2\" class=\"magic-radio\" type=\"radio\" name=\"IHOG2\" value=\"0\" checked=\"checked\">");
                sb.Append("<label for=\"rbt_opt15_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt15_2\" class=\"magic-radio\" type=\"radio\" name=\"IHOG2\" value=\"0\">");
                sb.Append("<label for=\"rbt_opt15_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }
            sb.Append("</div>");

            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>15.2 ENTONCES, CONTANDO EL DE USTED, ¿CUÁNTOS GRUPOS TIENEN GASTOS SEPARADOS DE ALIMENTACIÓN?</strong></p>");
            sb.Append("<input id=\"IHOG3\" name=\"IHOG3\" type=\"number\" class=\"form-control\" min=\"1\" max =\"99\" onKeyPress=\"if (this.value.length == 2) return false; return event.charCode >= 48 && event.charCode <= 57;\" placeholder=\"N°\" value=\"" + _viviendaBOL.IHOG3 + "\" required />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 2 (Pregunta 15.1 y 15.2)

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
    }
}
