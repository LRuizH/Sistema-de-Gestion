using Framework.BLL.Utilidades.Ajax;
using Framework.BLL.Utilidades.Html;
using Framework.BLL.Utilidades.Seguridad;
using Framework.BOL;
using Framework.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.BLL.Componentes.ModuloCuestionario
{
    public class CPersona
    {
        AppSettings _appSettings = new AppSettings();

        public string GetListaComuna(GesGeografiaBOL _gesGeografiaBOL, int codigo_id, string clase_control, string id_campo)
        {
            string str = "";
            GesGeografiaDAL _gesGeografiaDAL = new GesGeografiaDAL();
            List<CodeValue> lista = _gesGeografiaDAL.ListarComunasPorRegion<CodeValue>(_gesGeografiaBOL);

            CSelect cSelect = new CSelect
            {
                select_code = "codigo",
                select_value = "valor",
                select_id = "" + id_campo + "",
                select_data = lista,
                select_start = "Seleccione",
                select_class = "form-control bloqueo " + clase_control + ""
            };
            //  cSelect.select_selectedValue = selected;
            str = cSelect.getHTMLSelect(cSelect);
            return str;
        }

        /// <summary>
        /// Obtiene formulario Sección Emigración Personas
        /// </summary>
        public string ObtieneFormularioSeccionEmigracionPersona(string token, int paso, string cuestionario = "")
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Hogar
            PersonaExtBOL _personaExtBOL = new PersonaExtBOL();
            PersonaExtDAL _personaExtDAL = new PersonaExtDAL();

            _personaExtBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            _personaExtBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;
            _personaExtBOL.PK_VPE = int.Parse(_identificadorCuestionario.IdPersona.ToString());
            List<PersonaExtBOL> listaMigracionPersona = _personaExtDAL.ListarMigracionesPorHogarPersona<PersonaExtBOL>(_personaExtBOL);

            string _strCheckVPE2Si = "";
            string _strCheckVPE2No = "";

            if (listaMigracionPersona.Count > 0)
            {
                _personaExtBOL = listaMigracionPersona[0];

                if (_personaExtBOL.VPE2 == "1")
                {
                    _strCheckVPE2Si = "checked=\"checked\"";
                }

                if (_personaExtBOL.VPE2 == "0")
                {
                    _strCheckVPE2No = "checked=\"checked\"";
                }
            }

            // Carga opciones de respuesta
            GesFormPreguntasOpcionesBOL _gesFormPreguntasOpcionesBOL = new GesFormPreguntasOpcionesBOL();
            GesFormPreguntasOpcionesDAL _gesFormPreguntasOpcionesDAL = new GesFormPreguntasOpcionesDAL();
            List<GesFormPreguntasOpcionesBOL> listaOpcionesPregunta = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesPreguntaPorGrupos<GesFormPreguntasOpcionesBOL>("'31','33'");

            // Submit del formulario
            _postJSON.P_form = "formulario-persona-n";
            _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
            _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/persona/ingresar-datos-emigracion";
            _postJSON.P_data_dinamica = true;
            _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'retorno') { obtieneCuestionarioWeb(" + (paso - 1) + ",'" + token + "'); }" +
                                             "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioWeb(" + (paso + 1) + ",'" + token + "'); }";

            // Identificación Vivienda-Hogar
            sb.Append("<div class=\"row\">");
            sb.Append("<div class=\"col-lg-12  text-center\">");
            sb.Append("<div class=\"alert alert-info\">");
            sb.Append("<h3><strong>Información del " + _personaExtBOL.PK_HOGAR + ".<br /><small>Personas que emigraron del hogar</small></strong></h3>");
            sb.Append("<p><strong>Persona  " + _personaExtBOL.PK_VPE + "</strong></p>");
            sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(5,'" + token + "');\" class=\"btn btn-info btn-md\"><i class=\"fa fa-chevron-left\"></i> Volver al resumen de Hogares</button>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            // Inicio Definición del Formulario Persona. 
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
            sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
            sb.Append("<input id=\"paso_formulario\" name=\"paso_formulario\" type=\"hidden\" value=\"" + paso + "\"/>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");

            // Inicio Linea 1 (Pregunta 23.2)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
            sb.Append("<p style=\"margin-bottom:-2px;\"><strong>INFORMACIÓN DE EMIGRACIÓN</strong></p>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<br>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>24.2.- NOMBRE Y APELLIDO</strong></p>");
            sb.Append("<input id=\"VPE1\" name=\"VPE1\" type=\"text\" class=\"form-control\" placeholder=\"NOMBRE Y APELLIDO\" value=\"" + _personaExtBOL.VPE1 + "\" required />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 1 (Pregunta 23.2)

            // Inicio Linea 2 (Pregunta 23.3 y 23.4)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            if (_personaExtBOL.VPE1 == "")
            {
                 sb.Append("<p><strong>24.3.- CUANDO <span class=\"nombre-persona\"></span> SE FUE A VIVIR EN EL EXTRANJERO, ¿VIVÍA CON USTEDES?</strong></p>");
            } else
            {
                sb.Append("<p><strong>24.3.- CUANDO <span class=\"nombre-persona\">" + _personaExtBOL.VPE1 + "</span> SE FUE A VIVIR EN EL EXTRANJERO, ¿VIVÍA CON USTEDES?</strong></p>");
            }
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<input id=\"rbt_opt24_3_1\" class=\"magic-radio\" type=\"radio\" name=\"VPE2\" value=\"1\" " + _strCheckVPE2Si + ">");
            sb.Append("<label for=\"rbt_opt24_3_1\" style=\"display: inline;\">&nbsp;Si</label>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<input id=\"rbt_opt24_3_2\" class=\"magic-radio\" type=\"radio\" name=\"VPE2\" value=\"0\" " + _strCheckVPE2No + ">");
            sb.Append("<label for=\"rbt_opt24_3_2\" style=\"display: inline;\">&nbsp;No</label>");
            sb.Append("</div>");

            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            if (_personaExtBOL.VPE1 == "")
            {
                sb.Append("<p><strong>23.4.- ¿CUÁL ES EL SEXO DE <span class=\"nombre-persona\"></span>?</strong></p>");
            } else
            {
                sb.Append("<p><strong>23.4.- ¿CUÁL ES EL SEXO DE <span class=\"nombre-persona\">" + _personaExtBOL.VPE1 + "</span>?</strong></p>");
            }
               
            sb.Append("<select id=\"VPE3\" name=\"VPE3\" class=\"form-control\" data-width=\"100%\">");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append("<option value=\"1\">Hombre</option>");
            sb.Append("<option value=\"2\">Mujer</option>");
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("<script>$('#VPE3').val('" + _personaExtBOL.VPE3 + "');</script>");

            sb.Append("</div>");
            // Fin Linea 2 (Pregunta 23.3 y 23.4)

            // Inicio Linea 3 (Pregunta 23.5 y 23.6)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            if (_personaExtBOL.VPE1 == "")
            {
                sb.Append("<p><strong>23.5.- CUANDO <span class=\"nombre-persona\"></span> EMIGRÓ DEL PAÍS, ¿CUÁNTOS AÑOS CUMPLIDOS TENÍA?</strong></p>");
            } else
            {
                sb.Append("<p><strong>23.5.- CUANDO <span class=\"nombre-persona\">" + _personaExtBOL.VPE1 + "</span> EMIGRÓ DEL PAÍS, ¿CUÁNTOS AÑOS CUMPLIDOS TENÍA?</strong></p>");
            }
             
            if (_personaExtBOL.VPE4 == "")
            {
                sb.Append("<input id=\"VPE4\" name=\"VPE4\" type=\"number\" class=\"form-control\" min=\"1\" max =\"130\" onKeyPress =\"if (this.value.length == 3) return false; return event.charCode >= 48 && event.charCode <= 57;\" placeholder=\"N°\" value=\"\" />");
            }
            else
            {
                sb.Append("<input id=\"VPE4\" name=\"VPE4\" type=\"number\" class=\"form-control\" min=\"1\" max =\"130\" onKeyPress=\"if (this.value.length == 3) return false; return event.charCode >= 48 && event.charCode <= 57;\" placeholder=\"N°\" value=\"" + _personaExtBOL.VPE4 + "\" />");
            }
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            if (_personaExtBOL.VPE1 == "")
            {
                sb.Append("<p><strong>23.6.- ¿EN QUÉ AÑO <span class=\"nombre-persona\"></span> SE FUE A VIVIR AL EXTRANJERO?</strong></p>");
            } else
            {
                sb.Append("<p><strong>23.6.- ¿EN QUÉ AÑO <span class=\"nombre-persona\">" + _personaExtBOL.VPE1 + "</span> SE FUE A VIVIR AL EXTRANJERO?</strong></p>");
            }
               
            sb.Append("<select id=\"VPE5\" name=\"VPE5\" class=\"form-control\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append("<option value=\"2016\">2016</option>");
            sb.Append("<option value=\"2017\">2017</option>");
            sb.Append("<option value=\"2018\">2018</option>");
            sb.Append("<option value=\"2019\">2019</option>");
            sb.Append("<option value=\"2020\">2020</option>");
            sb.Append("<option value=\"2021\">2021</option>");
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("<script>$('#VPE5').val('" + _personaExtBOL.VPE5 + "');</script>");

            sb.Append("</div>");
            // Fin Linea 3 (Pregunta 23.5 y 23.6)

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
                               "$('.selectpicker').selectpicker();" +
                               "$('.magic-radio').iCheck({" +
                                    "checkboxClass: 'icheckbox_square-green'," +
                                    "radioClass: 'iradio_square-green'," +
                                    "increaseArea: '20%'" +
                               "});"
            };

            return sb.ToString() + _methodCallLoad.CreaJQueryDocumentReady();
        }

        /// <summary>
        /// Obtiene Listado de personas
        /// </summary>
        public string ObtieneIdentificacionPersonas(string token, string nombre)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=\"row\">");
            sb.Append("<div class=\"col-lg-12  text-center\">");
            sb.Append("<div class=\"alert alert-info\">");
            sb.Append("<h3><strong>Información de " + nombre  + "</strong></h3>");
            sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(15,'" + token + "');\" class=\"btn btn-info btn-md\"><i class=\"fa fa-chevron-left\"></i> Volver al resumen de Personas</button>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        /// <summary>
        /// Obtiene formulario Sección Personas
        /// </summary>
        public string ObtieneFormularioListadoPersonas(string token, int paso)
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();
            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Persona
            PersonaBOL _personaBOL = new PersonaBOL();
            PersonaDAL _personaDAL = new PersonaDAL();

            _personaBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            _personaBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;
            _personaBOL.PK_PERSONA = _identificadorCuestionario.IdPersona;
            
            List<PersonaBOL> listaPersona = _personaDAL.Listar<PersonaBOL>(_personaBOL);
            if (listaPersona.Count > 0)
            {
                _personaBOL = listaPersona[0];
            }

            string NombrePersona = _personaBOL.PER_NOMBRE;
            string ParentescoPersona = _personaBOL.PER1;
            string dato_PER11 = _personaBOL.PER11.ToString();
            // Carga opciones de respuesta
            GesFormPreguntasOpcionesBOL _gesFormPreguntasOpcionesBOL = new GesFormPreguntasOpcionesBOL();
            GesFormPreguntasOpcionesDAL _gesFormPreguntasOpcionesDAL = new GesFormPreguntasOpcionesDAL();
            List<GesFormPreguntasOpcionesBOL> listaOpcionesPregunta = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesPreguntaPorGrupos<GesFormPreguntasOpcionesBOL>("'55','56','63','59','61'");

            // Obtengo opciones de respuesta
            StringBuilder sbPER1 = new StringBuilder();
            StringBuilder sbPER2 = new StringBuilder();
            StringBuilder sbPER5 = new StringBuilder();
            StringBuilder sbPER11 = new StringBuilder();
            StringBuilder sbP36 = new StringBuilder();

            foreach (var item in listaOpcionesPregunta)
            {
                switch (item.Pk_form_preguntas)
                {
                    case 55:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER1.ToString())
                        {
                            sbPER1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 56:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER2.ToString())
                        {
                            sbPER2.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER2.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 59:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER5.ToString())
                        {
                            sbPER5.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER5.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 61:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER11.ToString())
                        {
                            sbPER11.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER11.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                }
            }

            // Submit del formulario
            _postJSON.P_form = "formulario-persona";
            _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
            _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/persona/ingresar-datos";
            _postJSON.P_data_dinamica = true;
            _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioWeb(13,'" + token + "'); }";

            // Identificación Hogar-Persona
            sb.Append(ObtieneIdentificacionPersonas(token, NombrePersona));

            // Inicio Definición del Formulario Persona. 
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
            sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
            sb.Append("<input id=\"valParentesco\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val11\" type=\"hidden\" value=\"\"/>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");

            // Inicio Linea 1 (Pregunta 28)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
            sb.Append("<p style=\"margin-bottom:-2px;\"><strong>LISTADO DE PERSONAS DEL HOGAR</strong></p>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<br>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>31.- ¿QUÉ PARENTESCO TIENE " + NombrePersona + " CON (NOMBRE DE EL JEFE O JEFA DE HOGAR)? </strong><br>Se debe identificar la relación existente entre cada persona del hogar con quien se definió como jefe o jefa. Esa relación puede ser sanguinea, legal o de otro tipo.</p>");
            sb.Append("<select id=\"PER1\" name=\"PER1\" class=\"form-control cboParentesco\" data-width=\"100%\" required>");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER1.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 1 (Pregunta 28)

            // Inicio Linea 2 (Pregunta 29)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>32.- ¿CUÁL ES EL SEXO DE " + NombrePersona + "?   </strong></p>");
            sb.Append("<select id=\"PER2\" name=\"PER2\" class=\"form-control\" data-width=\"100%\" required>");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER2.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");            

            sb.Append("</div>");
            // Fin Linea 2 (Pregunta 29)

            // Inicio Linea 3 (Pregunta 36 y 36.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>33.- ¿CUÁNTOS AÑOS CUMPLIDOS TIENE " + NombrePersona + "? </strong><br>Anote 0 en niños menores de 1 año.</p>");
            sb.Append("<input id=\"PER3\" name=\"PER3\" type=\"number\" class=\"form-control\" onKeyPress=\"if (this.value.length == 3) return false; return event.charCode >= 48 && event.charCode <= 57;\" placeholder=\"¿Cuántos años cumplidos tiene?\" value=\"" + _personaBOL.PER3 + "\" required/>");
            sb.Append("</div>");
            sb.Append("</div>");          

            sb.Append("</div>");
            // Fin Linea 3 (Pregunta 36 y 36.1)

            // Inicio Linea 3 (Pregunta 36 y 36.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>34.- ¿EN QUÉ FECHA NACIÓ " + NombrePersona + "?  </strong></p>");
            //sb.Append("<input id=\"PER4\" name=\"PER4\" type=\"date\" class=\"form-control\" min=\"1900-01-01\" max=\"2023-12-31\" placeholder=\"\" value=\"" + _personaBOL.PER4 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 3 (Pregunta 36 y 36.1)

            // Inicio Linea 3 (Pregunta 36 y 36.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>DÍA DE NACIMIENTO  </strong></p>");
            sb.Append("<input id=\"PER4\" name=\"PER4\" type=\"number\" class=\"form-control\" min=\"1\" max=\"31\" onKeyPress=\"if (this.value.length == 2) return false; return event.charCode >= 48 && event.charCode <= 57;\" placeholder=\"\" value=\"" + _personaBOL.PER4 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>MES DE NACIMIENTO  </strong></p>");
            sb.Append("<select id=\"PER5\" name=\"PER5\" class=\"form-control\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER5.ToString());
            sb.Append("</select>");
            //sb.Append("<input id=\"PER5\" name=\"PER5\" type=\"number\" class=\"form-control\" min=\"1\" max=\"12\" onKeyPress=\"if (this.value.length == 2) return false; return event.charCode >= 48 && event.charCode <= 57;\" placeholder=\"\" value=\"" + _personaBOL.PER5 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>AÑO DE NACIMIENTO  </strong></p>");
            sb.Append("<input id=\"PER6\" name=\"PER6\" type=\"number\" class=\"form-control\" min=\"1889\" max=\"2023\" onKeyPress=\"if (this.value.length == 4) return false; return event.charCode >= 48 && event.charCode <= 57;\" placeholder=\"\" value=\"" + _personaBOL.PER6 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 3 (Pregunta 36 y 36.1)

            // Inicio Linea 4 (Pregunta 30 y 34)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>35.- ¿CUÁL ES EL NÚMERO DE CÉDULA DE IDENTIDAD O RUN DE " + NombrePersona + "?  </strong><br>Ingresar rut sin puntos y con guion.</p>");
            sb.Append("<select id=\"PER11\" name=\"PER11\" class=\"form-control cboRut\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER11.ToString());
            sb.Append("</select>");            
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            //sb.Append("<p><strong>35.- ¿CUÁL ES EL NÚMERO DE CÉDULA DE IDENTIDAD O RUN DE " + NombrePersona + "?  </strong><br>Ingresar rut sin puntos y con guion.</p>");
            sb.Append("<input id=\"PER7\" name=\"PER7\" type=\"text\" class=\"form-control cboNumRut\" onKeyPress=\"if (this.value.length == 10) return false; return (event.charCode >= 48 && event.charCode <= 57) || (event.charCode == 45) || (event.charCode == 75) || (event.charCode == 107);\" placeholder=\"Especifique\" value=\"" + _personaBOL.PER7 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            //sb.Append("<div class=\"col-lg-12 col-md-12\">");
            //sb.Append("<div class=\"form-group\">");
            //sb.Append("<p><strong>36.- Con cuál género se identifica " + NombrePersona + "?  </strong></p>");
            //sb.Append("<select id=\"p36\" name=\"p36\" class=\"form-control cboPadre\" data-width=\"100%\">");
            //sb.Append("<option value=\"\">Seleccione opción...</option>");
            //sb.Append(sbP36.ToString());
            //sb.Append("</select>");
            //sb.Append("</div>");
            //sb.Append("</div>");

            //sb.Append("<div class=\"col-lg-12 col-md-12\">");
            //sb.Append("<div class=\"form-group txtOtro\">");
            //sb.Append("<p><strong>36.1.- Especifique el otro género </strong></p>");
            //sb.Append("<input id=\"p36_1\" name=\"p36_1\" type=\"text\" class=\"form-control\" placeholder=\"Especifique\" value=\"\" />");
            //sb.Append("</div>");
            //sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 4 (Pregunta 30 y 34)

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
                               "$('.selectpicker').selectpicker();" +
                               "$('.magic-radio').iCheck({" +
                                    "checkboxClass: 'icheckbox_square-green'," +
                                    "radioClass: 'iradio_square-green'," +
                                    "increaseArea: '20%'" +
                               "});" +
                            //divs ocultos
                            //"$('.txtOtro').hide(); " 
                            //fin //divs ocultos
                            //funciones de seleccionables
                            "$('#" + _postJSON.P_form + " .cboRut').on('change', function() {" +
                                 "var cborut = $(this).val(); " +
                                 "if(cborut != '') {" +
                                     "$('.cboNumRut').val('').attr('disabled','disabled');" +
                                 "}else {" +
                                     "$('.cboNumRut').val('').removeAttr('disabled');" +
                                 "}" +
                             "});" +
                            //fin funciones seleccionables
                            // funciones de load en campos
                            "setTimeout(function () { " +
                                "$('#val11').val(" + dato_PER11 + "); " +
                                "$('#valParentesco').val(" + ParentescoPersona + ");" +

                                "if ($('#valParentesco').val() == '1' ) { " +
                                    "$('.cboParentesco').attr('disabled','disabled');" +
                                "}else{" +
                                    "$('.cboParentesco option[value=1]').remove();" +
                                "}" +

                                "if ($('#val11').val() == '' ) { " +
                                    "$('.cboNumRut').removeAttr('disabled');" +
                                "}else{" +
                                    "$('.cboNumRut').attr('disabled','disabled');" +
                                "}" +
                            "}, 500);"
            };

            return sb.ToString() + _methodCallLoad.CreaJQueryDocumentReady();
        }

        /// <summary>
        /// Obtiene formulario Sección Personas NUCLEOS FAMILIARES
        /// </summary>
        public string ObtieneFormularioSeccionPersonaPaso2(string token, int paso, string cuestionario = "")
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Persona
            PersonaBOL _personaBOL = new PersonaBOL();
            PersonaDAL _personaDAL = new PersonaDAL();

            _personaBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            _personaBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;
            _personaBOL.PK_PERSONA = _identificadorCuestionario.IdPersona;
            List<PersonaBOL> listaPersona = _personaDAL.Listar<PersonaBOL>(_personaBOL);
            if (listaPersona.Count > 0)
            {
                _personaBOL = listaPersona[0];
            }

            string NombrePersona = _personaBOL.PER_NOMBRE;
            string NPER = _personaBOL.NPER;
            string EdadPersona = _personaBOL.PER3.ToString();

            string dato_PER_FAM2 = _personaBOL.PER_FAM2.ToString();
            string dato_PER_FAM4 = _personaBOL.PER_FAM4.ToString();
            string dato_PER_FAM6 = _personaBOL.PER_FAM6.ToString();

            //carga personas del hogar
            List<PersonaBOL> listaPersonasHogar = _personaDAL.ListarPersonasSegunHogar<PersonaBOL>(_identificadorCuestionario.IdVivienda, _identificadorCuestionario.IdHogar, _identificadorCuestionario.IdPersona);
            List<PersonaBOL> listaPersonasHombres = _personaDAL.ListarPersonasHombres<PersonaBOL>(_identificadorCuestionario.IdVivienda, _identificadorCuestionario.IdHogar, _identificadorCuestionario.IdPersona);
            List<PersonaBOL> listaPersonasMujeres = _personaDAL.ListarPersonasMujeres<PersonaBOL>(_identificadorCuestionario.IdVivienda, _identificadorCuestionario.IdHogar, _identificadorCuestionario.IdPersona);
            StringBuilder sbPER_FAM3 = new StringBuilder();
            StringBuilder sbPER_FAM5 = new StringBuilder();
            StringBuilder sbPER_FAM7 = new StringBuilder();
            foreach (var items in listaPersonasHogar)
            {
                 if (items.NPER.ToString() == _personaBOL.PER_FAM3.ToString())
                {
                    sbPER_FAM3.Append("<option value=\"" + items.NPER.ToString() + "\" selected>" + items.PER_NOMBRE + "</option>");
                }
                else
                {
                    sbPER_FAM3.Append("<option value=\"" + items.NPER.ToString() + "\">" + items.PER_NOMBRE + "</option>");
                }         
            }

            foreach (var items in listaPersonasMujeres)
            {
                if (items.NPER.ToString() == _personaBOL.PER_FAM5.ToString())
                {
                    sbPER_FAM5.Append("<option value=\"" + items.NPER.ToString() + "\" selected>" + items.PER_NOMBRE + "</option>");
                }
                else
                {
                    sbPER_FAM5.Append("<option value=\"" + items.NPER.ToString() + "\">" + items.PER_NOMBRE + "</option>");
                }
            }

            foreach (var items in listaPersonasHombres)
            {
                if (items.NPER.ToString() == _personaBOL.PER_FAM7.ToString())
                {
                    sbPER_FAM7.Append("<option value=\"" + items.NPER.ToString() + "\" selected>" + items.PER_NOMBRE + "</option>");
                }
                else
                {
                    sbPER_FAM7.Append("<option value=\"" + items.NPER.ToString() + "\">" + items.PER_NOMBRE + "</option>");
                }
            }

            // Carga opciones de respuesta
            GesFormPreguntasOpcionesBOL _gesFormPreguntasOpcionesBOL = new GesFormPreguntasOpcionesBOL();
            GesFormPreguntasOpcionesDAL _gesFormPreguntasOpcionesDAL = new GesFormPreguntasOpcionesDAL();
            List<GesFormPreguntasOpcionesBOL> listaOpcionesPregunta = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesPreguntaPorGrupos<GesFormPreguntasOpcionesBOL>("'117','118'");

            // Obtengo opciones de respuesta
            StringBuilder sbPER_FAM1 = new StringBuilder();
            StringBuilder sbPER_FAM2 = new StringBuilder();

            foreach (var item in listaOpcionesPregunta)
            {
                switch (item.Pk_form_preguntas)
                {
                    case 117:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_FAM1.ToString())
                        {
                            sbPER_FAM1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_FAM1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 118:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_FAM2.ToString())
                        {
                            sbPER_FAM2.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_FAM2.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                }
            }

            // Submit del formulario
            _postJSON.P_form = "formulario-persona-nucleos";
            _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
            _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/persona/ingresar-datos-nucleos";
            _postJSON.P_data_dinamica = true;
            _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioWeb(" + (paso + 1) + ",'" + token + "'); }";

            // Identificación Hogar-Persona
            sb.Append(ObtieneIdentificacionPersonas(token, NombrePersona));

            // Inicio Definición del Formulario Persona. 
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
            sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
            sb.Append("<input id=\"NPER\" name=\"NPER\" type=\"hidden\" value=\"" + NPER + "\"/>");
            sb.Append("<input id=\"val2\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val4\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val6\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"valEdad\" type=\"hidden\" value=\"\"/>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");

            // Inicio Linea 30 (Pregunta 58 y 59)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
            sb.Append("<p style=\"margin-bottom:-2px;\"><strong>NÚCLEOS FAMILIARES</strong></p>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<br>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divmen15\">");
            sb.Append("<p><strong>36.- ¿CUÁL ES EL ESTADO CIVIL DE " + NombrePersona + "? </strong><br>Refiere al estado legal de " + NombrePersona + ", independiente de su situación actual de convivencia o no con una pareja.</p>");
            sb.Append("<select id=\"PER_FAM1\" name=\"PER_FAM1\" class=\"form-control cbomen15\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_FAM1.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 30 (Pregunta 58 y 59)

            // Inicio Linea 30 (Pregunta 37 y 38)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divmen15\">");
            sb.Append("<p><strong>37.- ¿CUÁL ES LA SITUACIÓN CONYUGAL ACTUAL DE " + NombrePersona + "?  </strong><br>Considerar solamente la situación actual de convivencia.</p>");
            sb.Append("<select id=\"PER_FAM2\" name=\"PER_FAM2\" class=\"form-control cboSituacionActual cbomen15\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_FAM2.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divmen15\">");
            sb.Append("<p><strong>38.- ¿QUIÉN ES LA PAREJA DE " + NombrePersona + "? </strong></p>");
            sb.Append("<select id=\"PER_FAM3\" name=\"PER_FAM3\" class=\"form-control bloqueo cboPareja cbomen15\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_FAM3.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 30 (Pregunta 37 y 38)

            // Inicio Linea 31 (Pregunta 61 y 61.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>39.- ACTUALMENTE, ¿VIVE LA MADRE DE " + NombrePersona + " EN EL HOGAR? </strong></p>");

            sb.Append("<div class=\"form-group-radio-FAM4\">");
            if (_personaBOL.PER_FAM4 == "1")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt62_1\" class=\"magic-radio\" type=\"radio\" name=\"PER_FAM4\" value=\"1\" checked=\"checked\" >");
                sb.Append("<label for=\"rbt_opt62_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt62_1\" class=\"magic-radio\" type=\"radio\" name=\"PER_FAM4\" value=\"1\" >");
                sb.Append("<label for=\"rbt_opt62_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }

            if (_personaBOL.PER_FAM4 == "0")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt62_2\" class=\"magic-radio\" type=\"radio\" name=\"PER_FAM4\" value=\"0\" checked=\"checked\" >");
                sb.Append("<label for=\"rbt_opt62_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt62_2\" class=\"magic-radio\" type=\"radio\" name=\"PER_FAM4\" value=\"0\" >");
                sb.Append("<label for=\"rbt_opt62_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }
            sb.Append("</div>");

            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboQuienMadre\">");
            sb.Append("<p><strong>39.1.- ¿QUIÉN ES LA MADRE DE " + NombrePersona + "? </strong></p>");
            sb.Append("<select id=\"PER_FAM5\" name=\"PER_FAM5\" class=\"form-control bloqueo cboQuienMadre\" data-width=\"100%\">");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_FAM5.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 31 (Pregunta 61 y 61.1)

            // Inicio Linea 32 (Pregunta 62 y 62.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>40.- ACTUALMENTE, ¿VIVE EL PADRE DE " + NombrePersona + " EN EL HOGAR? </strong></p>");

            if (_personaBOL.PER_FAM6 == "1")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt63_1\" class=\"magic-radio\" type=\"radio\" name=\"PER_FAM6\" value=\"1\" checked=\"checked\" >");
                sb.Append("<label for=\"rbt_opt63_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt63_1\" class=\"magic-radio\" type=\"radio\" name=\"PER_FAM6\" value=\"1\" >");
                sb.Append("<label for=\"rbt_opt63_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }

            if (_personaBOL.PER_FAM6 == "0")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt63_2\" class=\"magic-radio\" type=\"radio\" name=\"PER_FAM6\" value=\"0\" checked=\"checked\" >");
                sb.Append("<label for=\"rbt_opt63_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt63_2\" class=\"magic-radio\" type=\"radio\" name=\"PER_FAM6\" value=\"0\" >");
                sb.Append("<label for=\"rbt_opt63_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }

            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboQuienPadre\">");
            sb.Append("<p><strong>40.1.- ¿QUIÉN ES EL PADRE DE " + NombrePersona + "? </strong></p>");
            sb.Append("<select id=\"PER_FAM7\" name=\"PER_FAM7\" class=\"form-control bloqueo cboQuienPadre\" data-width=\"100%\">");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_FAM7.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 32 (Pregunta 62 y 62.1)

            sb.Append("</div>");

            sb.Append("</div>");

            // Inicio Botones del Cuestionario
            sb.Append("<div class=\"row text-center\">");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<hr />");
            sb.Append("<div class=\"mensaje text-center\"></div>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
            sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso - 1) + ",'" + token + "');\"  class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
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
                               "$('.selectpicker').selectpicker();" +
                               "$('.magic-radio').iCheck({" +
                                    "checkboxClass: 'icheckbox_square-green'," +
                                    "radioClass: 'iradio_square-green'," +
                                    "increaseArea: '20%'" +
                               "}).on('ifChecked', function(event) {" +
                                    "if ($(this).is(':checked')){" +
                                        "if(this.id == 'rbt_opt62_1'){" +
                                            "$('.cboQuienMadre').val('').removeAttr('disabled');" +
                                            "$('.divcboQuienMadre').show();" +
                                        "}" +
                                        "if(this.id == 'rbt_opt62_2'){" +
                                            "$('.cboQuienMadre').val('').attr('disabled','disabled');" +
                                            "$('.divcboQuienMadre').hide();" +
                                        "}" +
                                        "if(this.id == 'rbt_opt63_1'){" +
                                            "$('.cboQuienPadre').val('').removeAttr('disabled');" +
                                            "$('.divcboQuienPadre').show();" +
                                        "}" +
                                        "if(this.id == 'rbt_opt63_2'){" +
                                            "$('.cboQuienPadre').val('').attr('disabled','disabled');" +
                                            "$('.divcboQuienPadre').hide();" +
                                        "}" +
                                    "}" +
                                "});" +
                               ////divs ocultos
                               "$('#" + _postJSON.P_form + " .bloqueo').attr('disabled','disabled'); " +

                               "$('.divcboQuienMadre').hide();" +
                               "$('.divcboQuienPadre').hide();" +
                               //fin divs ocultos
                               //radiobutton
                               //"$('.form-group-radio-FAM4').css('background-color', '#e9ecef');" +
                               //"$('.form-group-radio-FAM5').css('background-color', '#e9ecef');" +
                               //fin
                               "$('#" + _postJSON.P_form + " .cboSituacionActual').on('change', function() {" +
                                     "var cbPar = $(this).val(); " +
                                     "if(cbPar == '1' || cbPar == '2') {" +
                                        "$('.cboPareja').val('').removeAttr('disabled');" +
                                     "}else {" +
                                        "$('.cboPareja').val('').attr('disabled','disabled');" +
                                     "}" +
                                 "});" +
                            // funciones de load en campos
                            "setTimeout(function () { " + 
                                "$('#valEdad').val(" + EdadPersona + ");" +
                                "$('#val2').val(" + dato_PER_FAM2 + "); " +
                                "$('#val4').val(" + dato_PER_FAM4 + "); " +
                                "$('#val6').val(" + dato_PER_FAM6 + "); " +

                                "if ($('#valEdad').val() > 14 ) { " +
                                    "$('.cbomen15').removeAttr('disabled');" +
                                    "$('.divmen15').show();" +
                                "}else{"+
                                    "$('.cbomen15').val('').attr('disabled','disabled');" +
                                    "$('.divmen15').hide();" +
                                "}" +

                                "if ($('#val2').val() == '1' || $('#val2').val() == '2') { " +
                                    "$('.cboPareja').removeAttr('disabled');" +
                                "}" +
                                "if ($('#val4').val() == '1' ) { " +
                                    "$('.cboQuienMadre').removeAttr('disabled');" +
                                    "$('.divcboQuienMadre').show();" +
                                "}" +
                                "if ($('#val6').val() == '1' ) { " +
                                    "$('.cboQuienPadre').removeAttr('disabled');" +
                                    "$('.divcboQuienPadre').show();" +
                                "}" +
                            "}, 500);"
            };

            return sb.ToString() + _methodCallLoad.CreaJQueryDocumentReady();
        }

        /// <summary>
        /// Obtiene formulario Sección Personas MIGRACIÓN
        /// </summary>
        public string ObtieneFormularioSeccionPersonaPaso3(string token, int paso, string cuestionario = "")
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Persona
            PersonaBOL _personaBOL = new PersonaBOL();
            PersonaDAL _personaDAL = new PersonaDAL();

            _personaBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            _personaBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;
            _personaBOL.PK_PERSONA = _identificadorCuestionario.IdPersona;
            
            List<PersonaBOL> listaPersona = _personaDAL.Listar<PersonaBOL>(_personaBOL);
            if (listaPersona.Count > 0)
            {
                _personaBOL = listaPersona[0];
            }
            string NombrePersona = _personaBOL.PER_NOMBRE;
            string NPER = _personaBOL.NPER;

            string dato_PER_MIG1 = _personaBOL.PER_MIG1.ToString();
            string dato_PER_MIG2 = _personaBOL.PER_MIG2.ToString();
            string dato_PER_MIG4 = _personaBOL.PER_MIG4.ToString();
            string dato_PER_MIG5 = _personaBOL.PER_MIG5.ToString();
            string dato_PER_MIG7 = _personaBOL.PER_MIG7.ToString();
            string dato_PER_MIG8 = _personaBOL.PER_MIG8.ToString();
            string dato_PER_MIG13 = _personaBOL.PER_MIG13.ToString();
            string dato_PER_MIG15 = _personaBOL.PER_MIG15.ToString();
            string dato_PER_MIG16 = _personaBOL.PER_MIG16.ToString();
            string dato_PER_MIG17 = _personaBOL.PER_MIG17.ToString();

            // Carga opciones de respuesta
            GesFormPreguntasOpcionesBOL _gesFormPreguntasOpcionesBOL = new GesFormPreguntasOpcionesBOL();
            GesFormPreguntasOpcionesDAL _gesFormPreguntasOpcionesDAL = new GesFormPreguntasOpcionesDAL();
            List<GesFormPreguntasOpcionesBOL> listaOpcionesPregunta = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesPreguntaPorGrupos<GesFormPreguntasOpcionesBOL>("'59','65','999','888','67','69','71','73','75','79','80'");

            // Obtengo opciones de respuesta
            StringBuilder sbPER_MIG1 = new StringBuilder();
            StringBuilder sbPER_MIG2 = new StringBuilder();
            StringBuilder sbPER_MIG3 = new StringBuilder();
            StringBuilder sbPER_MIG4 = new StringBuilder();
            StringBuilder sbPER_MIG5 = new StringBuilder();
            StringBuilder sbPER_MIG6 = new StringBuilder();
            StringBuilder sbPER_MIG7 = new StringBuilder();
            StringBuilder sbPER_MIG8 = new StringBuilder();
            StringBuilder sbPER_MIG9 = new StringBuilder();
            StringBuilder sbPER_MIG10 = new StringBuilder();
            StringBuilder sbPER_MIG12 = new StringBuilder();
            StringBuilder sbPER_MIG13 = new StringBuilder();
            StringBuilder sbPER_MIG14 = new StringBuilder();
            StringBuilder sbPER_MIG15 = new StringBuilder(); //región comuna PER_MIG2
            StringBuilder sbPER_MIG16 = new StringBuilder(); //región comuna PER_MIG5
            StringBuilder sbPER_MIG17 = new StringBuilder(); //región comuna PER_MIG8

            foreach (var item in listaOpcionesPregunta)
            {
                switch (item.Pk_form_preguntas)
                {
                    case 65:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_MIG1.ToString())
                        {
                            sbPER_MIG1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_MIG1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 999:
                        // Se carga abajo
                        break;
                    case 888:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_MIG3.ToString())
                        {
                            sbPER_MIG3.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_MIG3.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_MIG6.ToString())
                        {
                            sbPER_MIG6.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_MIG6.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_MIG9.ToString())
                        {
                            sbPER_MIG9.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_MIG9.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_MIG14.ToString())
                        {
                            sbPER_MIG14.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_MIG14.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 69:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_MIG4.ToString())
                        {
                            sbPER_MIG4.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_MIG4.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;                   
                    case 73:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_MIG7.ToString())
                        {
                            sbPER_MIG7.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_MIG7.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 79:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_MIG12.ToString())
                        {
                            sbPER_MIG12.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_MIG12.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 80:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_MIG13.ToString())
                        {
                            sbPER_MIG13.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_MIG13.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 59:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_MIG10.ToString())
                        {
                            sbPER_MIG10.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_MIG10.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                }
            }

            // Cargo Región
            GesGeografiaDAL _gesGeografiaDAL = new GesGeografiaDAL();
            List<GesGeografiaBOL> listaRegion = _gesGeografiaDAL.ListarRegion<GesGeografiaBOL>();

            foreach (var item in listaRegion)
            {
                if (item.Geografia_codigo.ToString() == _personaBOL.PER_MIG15.ToString())
                {
                    sbPER_MIG15.Append("<option value=\"" + item.Geografia_codigo.ToString() + "\" selected>" + item.Geografia_nombre + "</option>");
                }
                else
                {
                    sbPER_MIG15.Append("<option value=\"" + item.Geografia_codigo.ToString() + "\">" + item.Geografia_nombre + "</option>");
                }
                if (item.Geografia_codigo.ToString() == _personaBOL.PER_MIG16.ToString())
                {
                    sbPER_MIG16.Append("<option value=\"" + item.Geografia_codigo.ToString() + "\" selected>" + item.Geografia_nombre + "</option>");
                }
                else
                {
                    sbPER_MIG16.Append("<option value=\"" + item.Geografia_codigo.ToString() + "\">" + item.Geografia_nombre + "</option>");
                }
                if (item.Geografia_codigo.ToString() == _personaBOL.PER_MIG17.ToString())
                {
                    sbPER_MIG17.Append("<option value=\"" + item.Geografia_codigo.ToString() + "\" selected>" + item.Geografia_nombre + "</option>");
                }
                else
                {
                    sbPER_MIG17.Append("<option value=\"" + item.Geografia_codigo.ToString() + "\">" + item.Geografia_nombre + "</option>");
                }
            }

            // Cargo Comunas
            //List<GesGeografiaBOL> listaComunas = _gesGeografiaDAL.ListarComunas<GesGeografiaBOL>();

            //foreach (var item in listaComunas)
            //{
            //    //if (item.Geografia_codigo.ToString() == _personaBOL.PER_MIG2.ToString())
            //    //{
            //    //    sbPER_MIG2.Append("<option value=\"" + item.Geografia_codigo.ToString() + "\" selected>" + item.Geografia_nombre + "</option>");
            //    //}
            //    //else
            //    //{
            //    //    sbPER_MIG2.Append("<option value=\"" + item.Geografia_codigo.ToString() + "\">" + item.Geografia_nombre + "</option>");
            //    //}
            //    //if (item.Geografia_codigo.ToString() == _personaBOL.PER_MIG5.ToString())
            //    //{
            //    //    sbPER_MIG5.Append("<option value=\"" + item.Geografia_codigo.ToString() + "\" selected>" + item.Geografia_nombre + "</option>");
            //    //}
            //    //else
            //    //{
            //    //    sbPER_MIG5.Append("<option value=\"" + item.Geografia_codigo.ToString() + "\">" + item.Geografia_nombre + "</option>");
            //    //}
            //    if (item.Geografia_codigo.ToString() == _personaBOL.PER_MIG8.ToString())
            //    {
            //        sbPER_MIG8.Append("<option value=\"" + item.Geografia_codigo.ToString() + "\" selected>" + item.Geografia_nombre + "</option>");
            //    }
            //    else
            //    {
            //        sbPER_MIG8.Append("<option value=\"" + item.Geografia_codigo.ToString() + "\">" + item.Geografia_nombre + "</option>");
            //    }
            //}

            // Submit del formulario
            _postJSON.P_form = "formulario-persona-migracion";
            _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
            _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/persona/ingresar-datos-migracion";
            _postJSON.P_data_dinamica = true;
            _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioWeb(" + (paso + 1) + ",'" + token + "'); }";

            // Identificación Hogar-Persona
            sb.Append(ObtieneIdentificacionPersonas(token, NombrePersona));

            // Inicio Definición del Formulario Persona. 
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
            sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
            sb.Append("<input id=\"NPER\" name=\"NPER\" type=\"hidden\" value=\"" + NPER + "\"/>");
            sb.Append("<input id=\"val1\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val2\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val4\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val5\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val7\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val8\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val13\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val15\" type=\"hidden\" value=\"\"/>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");

            // Inicio Linea 4 (Pregunta 37 y 37.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
            sb.Append("<p style=\"margin-bottom:-2px;\"><strong>MIGRACIÓN</strong></p>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<br>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>41.- ¿EN QUÉ COMUNA O PAÍS VIVÍA " + NombrePersona + " EN NOVIEMBRE DE 2016?  </strong></p>");
            sb.Append("<select id=\"PER_MIG1\" name=\"PER_MIG1\" class=\"form-control cboDondeVivia\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_MIG1.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");            

            sb.Append("</div>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboDondeViviaCom\">");
            sb.Append("<p><strong>41.1.- ¿EN QUÉ REGIÓN Y COMUNA?     </strong></p>");
            //sb.Append("<select id=\"PER_MIG2\" name=\"PER_MIG2\" class=\"form-control bloqueoPick cboDondeViviaCom selectpicker\" data-live-search=\"true\" data-width=\"100%\" >");
            //sb.Append("<option value=\"\">Seleccione opción...</option>");
            //sb.Append(sbPER_MIG2.ToString());
            //sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboDondeViviaCom\">");
            sb.Append("<p><strong>REGIÓN  </strong></p>");
            sb.Append("<select id=\"PER_MIG15\" name=\"PER_MIG15\" class=\"form-control bloqueo cboDondeViviaReg\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_MIG15.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboDondeViviaCom\">");
            sb.Append("<p><strong>COMUNA  </strong></p>");
            sb.Append("<div id=\"filtro_select_com_2\" class=\"filtro_select_comuna_2\">");
            sb.Append("</div>");
            //sb.Append("<select id=\"PER_MIG2\" name=\"PER_MIG2\" class=\"form-control bloqueo cboDondeViviaCom\" data-width=\"100%\" >");
            //sb.Append("<option value=\"\">Seleccione opción...</option>");
            //sb.Append(sbPER_MIG2.ToString());
            //sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");

            // Fin Linea 4 (Pregunta 37 y 37.1)

            // Inicio Linea 5 (Pregunta 37.2 y 37.3)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboDondeViviaPais\">");
            sb.Append("<p><strong>41.1.- ¿EN QUÉ PAÍS?   </strong></p>");
            sb.Append("<select id=\"PER_MIG3\" name=\"PER_MIG3\" class=\"form-control bloqueoPick cboDondeViviaPais selectpicker\" data-live-search=\"true\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_MIG3.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            //sb.Append("<div class=\"col-lg-12 col-md-12\">");
            //sb.Append("<div class=\"form-group txtOtro37 dP37_3\">");
            //sb.Append("<p><strong>37.3.- Especifique el otro país  </strong></p>");
            //sb.Append("<input id=\"p37_3\" name=\"p37_3\" type=\"text\" class=\"form-control\" placeholder=\"Especifique el otro país\" value=\"\" />");
            //sb.Append("</div>");
            //sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 5 (Pregunta 37.2 y 37.3)

            // Inicio Linea 6 (Pregunta 38 y 38.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>42.- ¿EN QUÉ COMUNA O PAÍS VIVÍA " + NombrePersona + " EN MARZO DE 2020?  </strong></p>");
            sb.Append("<select id=\"PER_MIG4\" name=\"PER_MIG4\" class=\"form-control cboDondeVivia2020\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_MIG4.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboDondeViviaCom2020\">");
            sb.Append("<p><strong>42.1.- ¿EN QUÉ COMUNA?   </strong></p>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboDondeViviaCom2020\">");
            sb.Append("<p><strong>REGIÓN  </strong></p>");
            sb.Append("<select id=\"PER_MIG16\" name=\"PER_MIG16\" class=\"form-control bloqueo cboDondeViviaReg2020\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_MIG16.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboDondeViviaCom2020\">");
            sb.Append("<p><strong>COMUNA   </strong></p>");
            sb.Append("<div id=\"filtro_select_com_5\" class=\"filtro_select_comuna_5\">");
            sb.Append("</div>");
            //sb.Append("<select id=\"PER_MIG5\" name=\"PER_MIG5\" class=\"form-control cboDondeViviaCom2020 bloqueoPick selectpicker\" data-live-search=\"true\" data-width=\"100%\" >");
            //sb.Append("<option value=\"\">Seleccione opción...</option>");
            //sb.Append(sbPER_MIG5.ToString());
            //sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 6 (Pregunta 38 y 38.1)

            // Inicio Linea 7 (Pregunta 38.2 y 38.3)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboDondeViviaPais2020\">");
            sb.Append("<p><strong>42.1 ¿EN QUÉ PAÍS? </strong></p>");
            sb.Append("<select id=\"PER_MIG6\" name=\"PER_MIG6\" class=\"form-control cboDondeViviaPais2020 bloqueoPick selectpicker\" data-live-search=\"true\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_MIG6.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            //sb.Append("<div class=\"col-lg-12 col-md-12\">");
            //sb.Append("<div class=\"form-group txtOtro38 dP38_3\">");
            //sb.Append("<p><strong>38.3.- Especifique el otro país  </strong></p>");
            //sb.Append("<input id=\"p38_3\" name=\"p38_3\" type=\"text\" class=\"form-control\" placeholder=\"Especifique el otro país\" value=\"\" />");
            //sb.Append("</div>");
            //sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 7 (Pregunta 38.2 y 38.3)

            // Inicio Linea 8 (Pregunta 39 y 39.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>43.- CUANDO NACIÓ " + NombrePersona + ", ¿EN QUÉ COMUNA O PAÍS VIVÍA LA MADRE DE " + NombrePersona + "?   </strong></p>");
            sb.Append("<select id=\"PER_MIG7\" name=\"PER_MIG7\" class=\"form-control cboDondeViviaMadre\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_MIG7.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboDondeViviaMadreCom\">");
            sb.Append("<p><strong>43.1.- ¿EN QUÉ COMUNA?   </strong></p>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboDondeViviaMadreCom\">");
            sb.Append("<p><strong>REGIÓN  </strong></p>");
            sb.Append("<select id=\"PER_MIG17\" name=\"PER_MIG17\" class=\"form-control bloqueo cboDondeViviaMadreReg\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_MIG17.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboDondeViviaMadreCom\">");
            sb.Append("<p><strong>COMUNA   </strong></p>");
            sb.Append("<div id=\"filtro_select_com_8\" class=\"filtro_select_comuna_8\">");
            sb.Append("</div>");
            //sb.Append("<select id=\"PER_MIG8\" name=\"PER_MIG8\" class=\"form-control bloqueoPick cboDondeViviaMadreCom selectpicker\" data-live-search=\"true\" data-width=\"100%\" >");
            //sb.Append("<option value=\"\">Seleccione opción...</option>");
            //sb.Append(sbPER_MIG8.ToString());
            //sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 8 (Pregunta 39 y 39.1)

            // Inicio Linea 9 (Pregunta 39.2 y 39.3)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboDondeViviaMadrePais\">");
            sb.Append("<p><strong>43.1 ¿EN QUÉ PAÍS?</strong></p>");
            sb.Append("<select id=\"PER_MIG9\" name=\"PER_MIG9\" class=\"form-control bloqueoPick cboDondeViviaMadrePais selectpicker\" data-live-search=\"true\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_MIG9.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            //sb.Append("<div class=\"col-lg-12 col-md-12\">");
            //sb.Append("<div class=\"form-group txtOtro39 dP39_3\">");
            //sb.Append("<p><strong>39.3.- Especifique el otro país  </strong></p>");
            //sb.Append("<input id=\"p39_3\" name=\"p39_3\" type=\"text\" class=\"form-control\" placeholder=\"Especifique el otro país\" value=\"\" />");
            //sb.Append("</div>");
            //sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 9 (Pregunta 39.2 y 39.3)

            // Inicio Linea 10 (Pregunta 39.4 y 39.5)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>44.- ¿EN QUÉ MES Y AÑO LLEGÓ " + NombrePersona + " A VIVIR A CHILE?  </strong></p>");
            //sb.Append("<input id=\"PER_MIG10\" name=\"PER_MIG10\" type=\"month\" class=\"form-control bloqueo cboDondeLLego\" placeholder=\"\" value=\"" + _personaBOL.PER_MIG10 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 10 (Pregunta 39.4 y 39.5)

            // Inicio Linea 3 (Pregunta 36 y 36.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>MES </strong></p>");
            //sb.Append("<input id=\"PER_MIG10\" name=\"PER_MIG10\" type=\"number\" class=\"form-control bloqueo cboDondeMesLLego\" min=\"1\" max=\"12\" onKeyPress=\"if (this.value.length == 2) return false; return event.charCode >= 48 && event.charCode <= 57;\" placeholder=\"\" value=\"" + _personaBOL.PER_MIG10 + "\" />");
            sb.Append("<select id=\"PER_MIG10\" name=\"PER_MIG10\" class=\"form-control bloqueo cboDondeMesLLego\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_MIG10.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>AÑO </strong></p>");
            sb.Append("<input id=\"PER_MIG11\" name=\"PER_MIG11\" type=\"number\" class=\"form-control bloqueo cboDondeAnioLLego\" min=\"1889\" max=\"2023\" onKeyPress=\"if (this.value.length == 4) return false; return event.charCode >= 48 && event.charCode <= 57;\" placeholder=\"\" value=\"" + _personaBOL.PER_MIG11 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");

            // Inicio Linea 10 (Pregunta 39.4 y 39.5)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>44.1.- CONSIDERANDO QUE NO RECUERDA LA FECHA ESPECÍFICA, ¿ME PODRÍA DECIR EN QUÉ PERIODO LLEGÓ " + NombrePersona + " A CHILE? </strong></p>");
            sb.Append("<select id=\"PER_MIG12\" name=\"PER_MIG12\" class=\"form-control bloqueo cboDondePeriodoLLego\" data-width=\"100%\">");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_MIG12.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 10 (Pregunta 39.4 y 39.5)

            // Inicio Linea 11 (Pregunta 40)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>45.- ¿CUÁL ES LA NACIONALIDAD DE " + NombrePersona + "?   </strong></p>");
            sb.Append("<select id=\"PER_MIG13\" name=\"PER_MIG13\" class=\"form-control cboNac\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_MIG13.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 11 (Pregunta 40)

            // Inicio Linea 12 (Pregunta 40.1 y 40.2)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboPaisOtra\">");
            sb.Append("<p><strong>45.1.-¿CUÁL ES EL PAÍS DE LA OTRA NACIONALIDAD? </strong></p>");
            sb.Append("<select id=\"PER_MIG14\" name=\"PER_MIG14\" class=\"form-control cboPaisOtra bloqueoPick selectpicker\" data-live-search=\"true\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_MIG14.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            //sb.Append("<div class=\"col-lg-12 col-md-12\">");
            //sb.Append("<div class=\"form-group txtOtro40 dP40_2\">");
            //sb.Append("<p><strong>40.2.- Especifique el otro país   </strong></p>");
            //sb.Append("<input id=\"p40_2\" name=\"p40_2\" type=\"text\" class=\"form-control\" placeholder=\"Especifique el otro país\" value=\"\" />");
            //sb.Append("</div>");
            //sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 12 (Pregunta 40.1 y 40.2)

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
                               "$('.selectpicker').selectpicker({ style: 'btn-default' });" +
                               "$('.magic-radio').iCheck({" +
                                    "checkboxClass: 'icheckbox_square-green'," +
                                    "radioClass: 'iradio_square-green'," +
                                    "increaseArea: '20%'" +
                               "});"  +
                               //divs campos bloqueados
                               "$('#" + _postJSON.P_form + " .bloqueoPick').attr('disabled','disabled').selectpicker('refresh'); " +
                               "$('#" + _postJSON.P_form + " .bloqueo').attr('disabled','disabled'); " +
                               "$('#" + _postJSON.P_form + " .bloqueoPick option[value=152]').remove().selectpicker('refresh');" +

                               "$('.divcboDondeViviaCom').hide();" +
                               "$('.divcboDondeViviaPais').hide();" +
                               "$('.divcboDondeViviaCom2020').hide();" +
                               "$('.divcboDondeViviaPais2020').hide();" +
                               "$('.divcboDondeViviaMadreCom').hide();" +
                               "$('.divcboDondeViviaMadrePais').hide();" + 
                               "$('.divcboPaisOtra').hide();" +
                //fin campos bloqueados
                //funciones de seleccionables
                //combos preg. ¿EN QUÉ COMUNA O PAÍS VIVÍA " + NombrePersona + " EN AGOSTO DE 2016? 
                "$('#" + _postJSON.P_form + " .cboDondeVivia').on('change', function() {" +
                     "var cbotro = $(this).val(); " +
                     "if(cbotro == 3) {" +
                         "$('.cboDondeViviaCom').val('').removeAttr('disabled');" +
                         "$('.filtro_select_comuna_2').empty().html(muestraComunasSegunRegion('', 2, 'cboDondeViviaCom', 'PER_MIG2'));" +
                         "$('.cboDondeViviaReg').val('').removeAttr('disabled');" +
                         "$('.divcboDondeViviaCom').show();" +
                         "$('.cboDondeViviaPais').val('').attr('disabled','disabled').selectpicker('refresh');" +
                         "$('.divcboDondeViviaPais').hide();" +
                     "}" +
                     "else if(cbotro == 4) {" +
                             "$('.cboDondeViviaPais').val('').removeAttr('disabled').selectpicker('refresh');" +
                             "$('.divcboDondeViviaPais').show();" +
                             "$('.cboDondeViviaCom').val('').attr('disabled','disabled');" +
                             "$('.cboDondeViviaReg').val('').attr('disabled','disabled');" +
                             "$('.divcboDondeViviaCom').hide();" +
                     "}else{" +
                             "$('.cboDondeViviaCom').val('').attr('disabled','disabled');" +
                             "$('.cboDondeViviaReg').val('').attr('disabled','disabled');" +
                             "$('.divcboDondeViviaCom').hide();" +
                             "$('.cboDondeViviaPais').val('').attr('disabled','disabled').selectpicker('refresh');" +
                             "$('.divcboDondeViviaPais').hide();" +
                     "}" +
                 "});" +
                //cambio region
                "$('#" + _postJSON.P_form + " .cboDondeViviaReg').on('change', function() {" +
                    "var cboreg = $(this).val(); " +
                     //"muestraComunasSegunRegion(1, cboreg,true,2)" +
                     "$('.filtro_select_comuna_2').empty().html(muestraComunasSegunRegion(cboreg, 2, 'cboDondeViviaCom', 'PER_MIG2'));" +
                 "});" +
                //cambio region
                "$('#" + _postJSON.P_form + " .cboDondeViviaReg2020').on('change', function() {" +
                    "var cboreg2020 = $(this).val(); " +
                     //"muestraComunasSegunRegion(1, cboreg,true,2)" +
                     "$('.filtro_select_comuna_5').empty().html(muestraComunasSegunRegion(cboreg2020, 5, 'cboDondeViviaCom2020', 'PER_MIG5'));" +
                 "});" +
                //cambio region
                "$('#" + _postJSON.P_form + " .cboDondeViviaMadreReg').on('change', function() {" +
                    "var cboregMadre = $(this).val(); " +
                     //"muestraComunasSegunRegion(1, cboreg,true,2)" +
                     "$('.filtro_select_comuna_8').empty().html(muestraComunasSegunRegion(cboregMadre, 8, 'cboDondeViviaMadreCom', 'PER_MIG8'));" +
                 "});" +
                //combos preg. ¿EN QUÉ COMUNA O PAÍS VIVÍA " + NombrePersona + " EN MARZO DE 2020? 
                "$('#" + _postJSON.P_form + " .cboDondeVivia2020').on('change', function() {" +
                     "var cbotro2020 = $(this).val(); " +
                     "if(cbotro2020 == 3) {" +
                         "$('.cboDondeViviaCom2020').val('').removeAttr('disabled');" +
                         "$('.filtro_select_comuna_5').empty().html(muestraComunasSegunRegion('', 5, 'cboDondeViviaCom2020', 'PER_MIG5'));" +
                         "$('.cboDondeViviaReg2020').val('').removeAttr('disabled');" +
                         "$('.divcboDondeViviaCom2020').show();" +
                         "$('.cboDondeViviaPais2020').val('').attr('disabled','disabled').selectpicker('refresh');" +
                         "$('.divcboDondeViviaPais2020').hide();" +
                     "}" +
                     "else if(cbotro2020 == 4) {" +
                             "$('.cboDondeViviaPais2020').val('').removeAttr('disabled').selectpicker('refresh');" +
                             "$('.divcboDondeViviaPais2020').show();" +
                             "$('.cboDondeViviaCom2020').val('').attr('disabled','disabled');" +
                             "$('.cboDondeViviaReg2020').val('').attr('disabled','disabled');" +
                             "$('.divcboDondeViviaCom2020').hide();" +
                     "}else{" +
                             "$('.cboDondeViviaCom2020').val('').attr('disabled','disabled');" +
                             "$('.cboDondeViviaReg2020').val('').attr('disabled','disabled');" +
                             "$('.divcboDondeViviaCom2020').hide();" +
                             "$('.cboDondeViviaPais2020').val('').attr('disabled','disabled').selectpicker('refresh');" +
                             "$('.divcboDondeViviaPais2020').hide();" +
                     "}" +
                 "});" +
                //combos preg. CUANDO NACIÓ " + NombrePersona + ", ¿EN QUÉ COMUNA O PAÍS VIVÍA LA MADRE DE " + NombrePersona + "? 
                "$('#" + _postJSON.P_form + " .cboDondeViviaMadre').on('change', function() {" +
                     "var cbotroMadre = $(this).val(); " +
                     "if(cbotroMadre == 2) {" +
                         "$('.cboDondeViviaMadreCom').val('').removeAttr('disabled');" +
                         "$('.filtro_select_comuna_8').empty().html(muestraComunasSegunRegion('', 8, 'cboDondeViviaMadreCom', 'PER_MIG8'));" +
                         "$('.cboDondeViviaMadreReg').val('').removeAttr('disabled');" +
                         "$('.divcboDondeViviaMadreCom').show();" +
                         "$('.cboDondeViviaMadrePais').val('').attr('disabled','disabled').selectpicker('refresh');" +
                         "$('.divcboDondeViviaMadrePais').hide();" +
                         "$('.cboDondeLLego').val('').attr('disabled','disabled');" +
                         "$('.cboDondePeriodoLLego').val('').attr('disabled','disabled');" +
                         "$('.cboDondeMesLLego').val('').attr('disabled','disabled');" +
                         "$('.cboDondeAnioLLego').val('').attr('disabled','disabled');" +
                     "}" +
                     "else if(cbotroMadre == 3) {" +
                             "$('.cboDondeViviaMadrePais').val('').removeAttr('disabled').selectpicker('refresh');" +
                             "$('.divcboDondeViviaMadrePais').show();" +
                             "$('.cboDondeViviaMadreCom').val('').attr('disabled','disabled');" +
                             "$('.cboDondeViviaMadreReg').val('').attr('disabled','disabled');" +
                             "$('.divcboDondeViviaMadreCom').hide();" +
                             "$('.cboDondeLLego').val('').removeAttr('disabled');" +
                             "$('.cboDondePeriodoLLego').val('').removeAttr('disabled');" +
                             "$('.cboDondeMesLLego').val('').removeAttr('disabled');" +
                             "$('.cboDondeAnioLLego').val('').removeAttr('disabled');" +
                     "}else{" +
                             "$('.cboDondeViviaMadreCom').val('').attr('disabled','disabled');" +
                             "$('.cboDondeViviaMadreReg').val('').attr('disabled','disabled');" +
                             "$('.divcboDondeViviaMadreCom').hide();" +
                             "$('.cboDondeViviaMadrePais').val('').attr('disabled','disabled').selectpicker('refresh');" +
                             "$('.divcboDondeViviaMadrePais').hide();" +
                             "$('.cboDondeLLego').val('').attr('disabled','disabled');" +
                             "$('.cboDondePeriodoLLego').val('').attr('disabled','disabled');" +
                             "$('.cboDondeMesLLego').val('').attr('disabled','disabled');" +
                             "$('.cboDondeAnioLLego').val('').attr('disabled','disabled');" +
                     "}" +
                 "});" +
                 //combos preg.	¿CUÁL ES LA NACIONALIDAD DE (NOMBRE)? 
                 "$('#" + _postJSON.P_form + " .cboNac').on('change', function() {" +
                    "var cboNac = $(this).val(); " +
                    "if(cboNac == 2 || cboNac == 3) {" +
                        "$('.cboPaisOtra').val('').removeAttr('disabled').selectpicker('refresh');" +
                        "$('.divcboPaisOtra').show();" +
                    "}else{" +
                        "$('.cboPaisOtra').val('').attr('disabled','disabled').selectpicker('refresh');" +
                        "$('.divcboPaisOtra').hide();" +
                    "}" +
                "});" +
                //fin funciones seleccionables
                // funciones de load en campos
                "setTimeout(function () { " +
                    "$('#val1').val(" + dato_PER_MIG1 + "); " +
                    "$('#val2').val(" + dato_PER_MIG2 + "); " +
                    "$('#val4').val(" + dato_PER_MIG4 + "); " +
                    "$('#val7').val(" + dato_PER_MIG7 + "); " +
                    "$('#val13').val(" + dato_PER_MIG13 + "); " +
                    "$('#val15').val(" + dato_PER_MIG15 + "); " +
                    "$('.filtro_select_comuna_2').empty().html(muestraComunasSegunRegion('" + dato_PER_MIG15 + "', 2, 'cboDondeViviaCom', 'PER_MIG2'));" +
                    "$('.filtro_select_comuna_5').empty().html(muestraComunasSegunRegion('" + dato_PER_MIG16 + "', 5, 'cboDondeViviaCom2020', 'PER_MIG5'));" +
                    "$('.filtro_select_comuna_8').empty().html(muestraComunasSegunRegion('" + dato_PER_MIG17 + "', 8, 'cboDondeViviaMadreCom', 'PER_MIG8'));" +
                    "if ($('#val1').val() == '3') { " +
                        "$('.cboDondeViviaCom').removeAttr('disabled');" +
                        "$('.filtro_select_comuna_2').empty().html(muestraComunasSegunRegion('" + dato_PER_MIG15 + "', 2, 'cboDondeViviaCom', 'PER_MIG2'));" +
                        "setTimeout(function () { $('#PER_MIG2').val('" + dato_PER_MIG2 + "').change();}, 1500);" +
                        "$('.cboDondeViviaReg').removeAttr('disabled');" +
                        "$('.divcboDondeViviaCom').show();" +
                    "}else if($('#val1').val() == '4') { " +
                        "$('.cboDondeViviaPais').removeAttr('disabled').selectpicker('refresh');" +
                        "$('.divcboDondeViviaPais').show();" +
                    "}" +
                    "if ($('#val4').val() == '3') { " +
                        "$('.cboDondeViviaCom2020').removeAttr('disabled');" +
                        "$('.filtro_select_comuna_5').empty().html(muestraComunasSegunRegion('" + dato_PER_MIG16 + "', 5, 'cboDondeViviaCom2020', 'PER_MIG5'));" +
                        "setTimeout(function () { $('#PER_MIG5').val('" + dato_PER_MIG5 + "').change();}, 1500);" +
                        "$('.cboDondeViviaReg2020').removeAttr('disabled');" +
                        "$('.divcboDondeViviaCom2020').show();" +
                    "}else if($('#val4').val() == '4') { " +
                        "$('.cboDondeViviaPais2020').removeAttr('disabled').selectpicker('refresh');" +
                        "$('.divcboDondeViviaPais2020').show();" +
                    "}" +
                    "if ($('#val7').val() == '3') { " +
                        "$('.cboDondeViviaMadrePais').removeAttr('disabled').selectpicker('refresh');" +
                        "$('.divcboDondeViviaMadrePais').show();" +
                        "$('.cboDondeLLego').removeAttr('disabled');" +
                        "$('.cboDondePeriodoLLego').removeAttr('disabled');" +
                        "$('.cboDondeMesLLego').removeAttr('disabled');" +
                        "$('.cboDondeAnioLLego').removeAttr('disabled');" +
                    "}else if($('#val7').val() == '2') { " +
                        "$('.cboDondeViviaMadreCom').removeAttr('disabled');" +
                        "$('.filtro_select_comuna_8').empty().html(muestraComunasSegunRegion('" + dato_PER_MIG17 + "', 8, 'cboDondeViviaMadreCom', 'PER_MIG8'));" +
                        "setTimeout(function () { $('#PER_MIG8').val('" + dato_PER_MIG8 + "').change();}, 1500);" +
                        "$('.cboDondeViviaMadreReg').removeAttr('disabled');" +
                        "$('.divcboDondeViviaMadreCom').show();" +
                    "}" +
                    "if ($('#val13').val() == '2' || $('#val13').val() == '3') { " +
                        "$('.cboPaisOtra').removeAttr('disabled').selectpicker('refresh');" +
                        "$('.divcboPaisOtra').show();" +
                    "}" +
                "}, 500);"
            };

            // Genero funcion para Carga comunas()
            GetJSON _getJSONListaComunasSegunRegion = new GetJSON();
            {
                _getJSONListaComunasSegunRegion.G_url_servicio = _appSettings.ServidorWeb + "api/persona/muestra-comuna-segun-region";
                _getJSONListaComunasSegunRegion.G_parametros = "{ num: num, codigo_id: codigo_id, clase_control: clase_control, id_campo: id_campo}";
                _getJSONListaComunasSegunRegion.G_respuesta_servicio = "if(codigo_id == 2){" +
                                                                            "$('#filtro_select_com_2').html(respuesta[0].elemento_html);" +
                                                                       "}" +
                                                                       "if(codigo_id == 5){" +
                                                                            "$('#filtro_select_com_5').html(respuesta[0].elemento_html);" +
                                                                       "}" +
                                                                       "if(codigo_id == 8){" +
                                                                            "$('#filtro_select_com_8').html(respuesta[0].elemento_html);" +
                                                                       "}";
            }
            CallMethod _methodCallMuestraListaComunasSegunRegion = new CallMethod
            {
                Mc_nombre = "muestraComunasSegunRegion(num, codigo_id, clase_control, id_campo)",
                Mc_contenido = _getJSONListaComunasSegunRegion.GetJSONCall()
            };

            sb.Append(_methodCallMuestraListaComunasSegunRegion.CreaJQueryFunction());

            return sb.ToString() + _methodCallLoad.CreaJQueryDocumentReady();
        }

        /// <summary>
        /// Obtiene formulario Sección Personas NACIONALIDAD O INTERCULTURALIDAD
        /// </summary>
        public string ObtieneFormularioSeccionPersonaPaso4(string token, int paso, string cuestionario = "")
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Persona
            PersonaBOL _personaBOL = new PersonaBOL();
            PersonaDAL _personaDAL = new PersonaDAL();

            _personaBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            _personaBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;
            _personaBOL.PK_PERSONA = _identificadorCuestionario.IdPersona;            
            List<PersonaBOL> listaPersona = _personaDAL.Listar<PersonaBOL>(_personaBOL);
            if (listaPersona.Count > 0)
            {
                _personaBOL = listaPersona[0];
            }

            string NombrePersona = _personaBOL.PER_NOMBRE;
            string NPER = _personaBOL.NPER;
            string EdadPersona = _personaBOL.PER3.ToString();

            string dato_PER_NAC1 = _personaBOL.PER_NAC1.ToString();
            string dato_PER_NAC2 = _personaBOL.PER_NAC2.ToString();
            string dato_PER_NAC3 = _personaBOL.PER_NAC3.ToString();
            string dato_PER_NAC4 = _personaBOL.PER_NAC4.ToString();
            string dato_PER_NAC6 = _personaBOL.PER_NAC6.ToString();
            string dato_PER_NAC8 = _personaBOL.PER_NAC8.ToString();
            string dato_PER_NAC9 = _personaBOL.PER_NAC9.ToString();
            string dato_PER_NAC11 = _personaBOL.PER_NAC11.ToString();
            string dato_PER_NAC14 = _personaBOL.PER_NAC14.ToString();

            // Carga opciones de respuesta
            GesFormPreguntasOpcionesBOL _gesFormPreguntasOpcionesBOL = new GesFormPreguntasOpcionesBOL();
            GesFormPreguntasOpcionesDAL _gesFormPreguntasOpcionesDAL = new GesFormPreguntasOpcionesDAL();
            List<GesFormPreguntasOpcionesBOL> listaOpcionesPregunta = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesPreguntaPorGrupos<GesFormPreguntasOpcionesBOL>("'81','88','90','95','97','99','141'");

            // Obtengo opciones de respuesta
            StringBuilder sbPER_NAC1 = new StringBuilder();
            StringBuilder sbPER_NAC2 = new StringBuilder();
            StringBuilder sbPER_NAC4 = new StringBuilder();
            StringBuilder sbPER_NAC6 = new StringBuilder();
            StringBuilder sbPER_NAC9 = new StringBuilder();
            StringBuilder sbPER_NAC11 = new StringBuilder();
            StringBuilder sbPER_NAC13 = new StringBuilder();
            StringBuilder sbPER_NAC14 = new StringBuilder();

            foreach (var item in listaOpcionesPregunta)
            {
                switch (item.Pk_form_preguntas)
                {                    
                    case 88:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_NAC2.ToString())
                        {
                            sbPER_NAC2.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_NAC2.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 90:
                        if(item.Fpo_numero.ToString() == _personaBOL.PER_NAC4.ToString())
                        {
                            sbPER_NAC4.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_NAC4.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 95:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_NAC9.ToString())
                        {
                            sbPER_NAC9.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_NAC9.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 97:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_NAC11.ToString())
                        {
                            sbPER_NAC11.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_NAC11.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 99:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_NAC13.ToString())
                        {
                            sbPER_NAC13.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_NAC13.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 141:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_NAC14.ToString())
                        {
                            sbPER_NAC14.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_NAC14.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                }
            }

            // Submit del formulario
            _postJSON.P_form = "formulario-persona-nac";
            _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
            _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/persona/ingresar-datos-nac";
            _postJSON.P_data_dinamica = true;
            _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { " +
                                                 "if(" + EdadPersona + " > '4'){" +
                                                    "obtieneCuestionarioWeb(" + (paso + 1) + ",'" + token + "'); " +
                                                 "}else{" +
                                                    "obtieneCuestionarioWeb(" + (paso + 2) + ", '" + token + "'); " +
                                                 "}"+
                                             "}";

            // Identificación Hogar-Persona
            sb.Append(ObtieneIdentificacionPersonas(token, NombrePersona));

            // Inicio Definición del Formulario Persona. 
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
            sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
            sb.Append("<input id=\"NPER\" name=\"NPER\" type=\"hidden\" value=\"" + NPER + "\"/>");
            sb.Append("<input id=\"val1\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val2\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val3\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val4\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val6\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val8\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val9\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val11\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val14\" type=\"hidden\" value=\"\"/>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");

            // Inicio Linea 11 (Pregunta 40)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
            sb.Append("<p style=\"margin-bottom:-2px;\"><strong>INTERCULTURALIDAD </strong></p>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<br>");
            sb.Append("</div>");            

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>46.- ¿" + NombrePersona + " ES O SE CONSIDERA PERTENECIENTE A ALGÚN PUEBLO INDÍGENA U ORIGINARIO?  </strong></p>");

            if (_personaBOL.PER_NAC1 == "1")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt41_1\" class=\"magic-radio\" type=\"radio\" name=\"PER_NAC1\" value=\"1\" checked=\"checked\" >");
                sb.Append("<label for=\"rbt_opt41_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt41_1\" class=\"magic-radio\" type=\"radio\" name=\"PER_NAC1\" value=\"1\" >");
                sb.Append("<label for=\"rbt_opt41_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }

            if (_personaBOL.PER_NAC1 == "0")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt41_2\" class=\"magic-radio\" type=\"radio\" name=\"PER_NAC1\" value=\"0\" checked=\"checked\" >");
                sb.Append("<label for=\"rbt_opt41_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt41_2\" class=\"magic-radio\" type=\"radio\" name=\"PER_NAC1\" value=\"0\" >");
                sb.Append("<label for=\"rbt_opt41_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }            

            sb.Append("</div>");
            sb.Append("</div>");

            //sb.Append("<div class=\"col-lg-12 col-md-12\">");
            //sb.Append("<div class=\"form-group txtOtro40 dP40_3\">");
            //sb.Append("<p><strong>40.3.- Seleccionar el país de la otra nacionalidad  </strong></p>");
            //sb.Append("<select id=\"p40_3\" name=\"p40_3\" class=\"form-control cboPadre40_3\" data-width=\"100%\">");
            //sb.Append("<option value=\"\">Seleccione opción...</option>");
            //sb.Append(sbP40_3.ToString());
            //sb.Append("</select>");
            //sb.Append("</div>");
            //sb.Append("</div>");

            //sb.Append("<div class=\"col-lg-12 col-md-12\">");
            //sb.Append("<div class=\"form-group txtOtro40 dP40_4\">");
            //sb.Append("<p><strong>40.4.- Especifique el otro país  </strong></p>");
            //sb.Append("<input id=\"p40_4\" name=\"p40_4\" type=\"text\" class=\"form-control\" placeholder=\"Especifique el otro país\" value=\"\" />");
            //sb.Append("</div>");
            //sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 13 (Pregunta 40.3 y 40.4)

            // Inicio Linea 16 (Pregunta 41.1 y 41.2)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>46.1.- ¿CUÁL?  </strong></p>");
            sb.Append("<select id=\"PER_NAC2\" name=\"PER_NAC2\" class=\"form-control bloqueo cboCual\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_NAC2.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboCualEspecifique\">");
            sb.Append("<p><strong>46.2.-ESPECIFIQUE EL OTRO PUEBLO INDÍGENA U ORIGINARIO </strong></p>");
            sb.Append("<input id=\"PER_NAC3\" name=\"PER_NAC3\" type=\"text\" class=\"form-control cboCualEspecifique bloqueo\" onkeypress=\"SoloLetras()\" placeholder=\"Especifique\" value=\"" + _personaBOL.PER_NAC3 + "\" / >");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 16 (Pregunta 41.1 y 41.2)

            // Inicio Linea 17 (Pregunta 41.3 y 41.4)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>46.3.- ¿CÓMO SE CONSIDERA " + NombrePersona + "? </strong></p>");
            sb.Append("<select id=\"PER_NAC4\" name=\"PER_NAC4\" class=\"form-control bloqueo cboComoSeConsidera\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_NAC4.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboComoSeConsideraEspecifique\">");
            sb.Append("<p><strong>46.4.- ESPECIFIQUE LA OTRA IDENTIDAD </strong></p>");
            sb.Append("<input id=\"PER_NAC5\" name=\"PER_NAC5\" type=\"text\" class=\"form-control bloqueo cboComoSeConsideraEspecifique\" onkeypress=\"SoloLetras()\" placeholder=\"Especifique\" value=\"" + _personaBOL.PER_NAC5 + "\"  />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 17 (Pregunta 41.3 y 41.4)

            // Inicio Linea 18 (Pregunta 42 y 43)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>47.- ¿" + NombrePersona + " COMPARTE LA COSMOVISIÓN O ESPIRITUALIDAD DE SU PUEBLO? </strong></p>");

            sb.Append("<div class=\"form-group-radio-cboComparte\">");

            if (_personaBOL.PER_NAC6 == "1")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt42_1\" class=\"magic-radio bloqueo cboComparte\" type=\"radio\" name=\"PER_NAC6\" value=\"1\" checked=\"checked\" >");
                sb.Append("<label for=\"rbt_opt42_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt42_1\" class=\"magic-radio bloqueo cboComparte\" type=\"radio\" name=\"PER_NAC6\" value=\"1\" >");
                sb.Append("<label for=\"rbt_opt42_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }

            if (_personaBOL.PER_NAC6 == "0")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt42_2\" class=\"magic-radio bloqueo cboComparte\" type=\"radio\" name=\"PER_NAC6\" value=\"0\" checked=\"checked\" >");
                sb.Append("<label for=\"rbt_opt42_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt42_2\" class=\"magic-radio bloqueo cboComparte\" type=\"radio\" name=\"PER_NAC6\" value=\"0\" >");
                sb.Append("<label for=\"rbt_opt42_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }

            sb.Append("</div>");

            sb.Append("</div>");
            sb.Append("</div>");           

            sb.Append("</div>");
            // Fin Linea 18 (Pregunta 42 y 43)

            // Inicio Linea 18 (Pregunta 42 y 43)
            sb.Append("<div class=\"row\">");            

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>48.- ¿" + NombrePersona + " PRACTICA LAS COSTUMBRES, TRADICIONES, CEREMONIAS O RITUALES DEL PUEBLO INDÍGENA U ORIGINARIO AL CUAL PERTENECE?  </strong></p>");

            sb.Append("<div class=\"form-group-radio-cboPractica\">");

            if (_personaBOL.PER_NAC7 == "1")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt43_1\" class=\"magic-radio bloqueo cboPractica\" type=\"radio\" name=\"PER_NAC7\" value=\"1\" checked=\"checked\" >");
                sb.Append("<label for=\"rbt_opt43_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt43_1\" class=\"magic-radio bloqueo cboPractica\" type=\"radio\" name=\"PER_NAC7\" value=\"1\" >");
                sb.Append("<label for=\"rbt_opt43_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }

            if (_personaBOL.PER_NAC7 == "0")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt43_2\" class=\"magic-radio bloqueo cboPractica\" type=\"radio\" name=\"PER_NAC7\" value=\"0\" checked=\"checked\" >");
                sb.Append("<label for=\"rbt_opt43_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt43_2\" class=\"magic-radio bloqueo cboPractica\" type=\"radio\" name=\"PER_NAC7\" value=\"0\" >");
                sb.Append("<label for=\"rbt_opt43_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }

            sb.Append("</div>");

            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 18 (Pregunta 42 y 43)

            // Inicio Linea 19 (Pregunta 44)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>49.- ¿" + NombrePersona + " ES O SE CONSIDERA AFRODESCENDIENTE? </strong><br>Se entiende por afrodescendientes a todos los pueblos y las personas descendientes de la dispersión africana que debieron abandonar su lugar de procedencia de origen (África).</p>");

            if (_personaBOL.PER_NAC8 == "1")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt44_1\" class=\"magic-radio\" type=\"radio\" name=\"PER_NAC8\" value=\"1\" checked=\"checked\" >");
                sb.Append("<label for=\"rbt_opt44_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt44_1\" class=\"magic-radio\" type=\"radio\" name=\"PER_NAC8\" value=\"1\" >");
                sb.Append("<label for=\"rbt_opt44_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }

            if (_personaBOL.PER_NAC8 == "0")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt44_2\" class=\"magic-radio\" type=\"radio\" name=\"PER_NAC8\" value=\"0\" checked=\"checked\" >");
                sb.Append("<label for=\"rbt_opt44_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt44_2\" class=\"magic-radio\" type=\"radio\" name=\"PER_NAC8\" value=\"0\" >");
                sb.Append("<label for=\"rbt_opt44_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }

            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 19 (Pregunta 44)

            // Inicio Linea 17 (Pregunta 41.3 y 41.4)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>49.1.- ¿CON CUÁL DE LOS SIGUIENTES CONCEPTOS SE IDENTIFICA " + NombrePersona + "?  </strong></p>");
            sb.Append("<select id=\"PER_NAC9\" name=\"PER_NAC9\" class=\"form-control bloqueo cboConseptoAfro\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_NAC9.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboConseptoAfroEspecifique\">");
            sb.Append("<p><strong>49.2.- ESPECIFIQUE EL OTRO CONCEPTO </strong></p>");
            sb.Append("<input id=\"PER_NAC10\" name=\"PER_NAC10\" type=\"text\" class=\"form-control bloqueo cboConseptoAfroEspecifique\" onkeypress=\"SoloLetras()\" placeholder=\"Especifique\" value=\"" + _personaBOL.PER_NAC10 + "\"  />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 17 (Pregunta 41.3 y 41.4)

            // Inicio Linea 20 (Pregunta 45 y 45.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>50.- ¿" + NombrePersona + " HABLA O ENTIENDE UNA DE LAS SIGUIENTES LENGUAS INDÍGENAS U ORIGINARIAS </strong></p>");
            sb.Append("<select id=\"PER_NAC11\" name=\"PER_NAC11\" class=\"form-control cboHabla\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_NAC11.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboHablaEspecifique\">");
            sb.Append("<p><strong>50.1.- ESPECIFIQUE OTRA LENGUA INDÍGENA U ORIGINARIA </strong></p>");
            sb.Append("<input id=\"PER_NAC12\" name=\"PER_NAC12\" type=\"text\" class=\"form-control bloqueo cboHablaEspecifique\" onkeypress=\"SoloLetras()\" placeholder=\"Especifique\" value=\"" + _personaBOL.PER_NAC12 + "\"  />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 20 (Pregunta 45 y 45.1)

            // Inicio Linea 20 (Pregunta 45.2)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>50.2.- EN RELACIÓN A ESA LENGUA INDÍGENA U ORIGINARIA, " + NombrePersona + ":   </strong></p>");
            sb.Append("<select id=\"PER_NAC13\" name=\"PER_NAC13\" class=\"form-control bloqueo cboLengua\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_NAC13.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 20 (Pregunta 45.2)

            // Inicio Linea 43 (Pregunta 73 y 73.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>51.- ¿CUÁL ES LA RELIGIÓN O CREDO DE " + NombrePersona + "? </strong></p>");
            sb.Append("<select id=\"PER_NAC14\" name=\"PER_NAC14\" class=\"form-control cboReligion\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_NAC14.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboReligionEspecifique\">");
            sb.Append("<p><strong>51.1.- ESPECIFIQUE LA OTRA RELIGIÓN O CREDO</strong></p>");
            sb.Append("<input id=\"PER_NAC15\" name=\"PER_NAC15\" type=\"text\" class=\"form-control bloqueo cboReligionEspecifique\" onkeypress=\"SoloLetras()\" placeholder=\"Especifique\" value=\"" + _personaBOL.PER_NAC15 + "\"  />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 43 (Pregunta 73 y 73.1)

            // Inicio Linea 14 (Pregunta 40.5 y 40.6)
            sb.Append("<div class=\"row\">");

            //sb.Append("<div class=\"col-lg-12 col-md-12\">");
            //sb.Append("<div class=\"form-group txtOtro40 dP40_5\">");
            //sb.Append("<p><strong>40.5.- Primer país de la doble nacionalidad   </strong></p>");
            //sb.Append("<input id=\"p40_5\" name=\"p40_5\" type=\"text\" class=\"form-control\" placeholder=\"Especifique primer país\" value=\"\" />");
            //sb.Append("</div>");
            //sb.Append("</div>");

            //sb.Append("<div class=\"col-lg-12 col-md-12\">");
            //sb.Append("<div class=\"form-group txtOtro40 dP40_6\">");
            //sb.Append("<p><strong>40.6.- Segundo país de la doble nacionalidad  </strong></p>");
            //sb.Append("<input id=\"p40_6\" name=\"p40_6\" type=\"text\" class=\"form-control\" placeholder=\"Especifique segundo país\" value=\"\" />");
            //sb.Append("</div>");
            //sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 14 (Pregunta 40.5 y 40.6)

            sb.Append("</div>");

            sb.Append("</div>");

            // Inicio Botones del Cuestionario
            sb.Append("<div class=\"row text-center\">");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<hr />");
            sb.Append("<div class=\"mensaje text-center\"></div>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
            sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso - 1) + ",'" + token + "');\"  class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
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
                               "$('.selectpicker').selectpicker();" +
                               "$('.magic-radio').iCheck({" +
                                    "checkboxClass: 'icheckbox_square-green'," +
                                    "radioClass: 'iradio_square-green'," +
                                    "increaseArea: '20%'" +
                               "}).on('ifChecked', function(event) {" +
                                    "if ($(this).is(':checked')){" +
                                        "if(this.id == 'rbt_opt41_1'){" +
                                            "$('.cboCual').val('').removeAttr('disabled');" +
                                        "}" +
                                        "if(this.id == 'rbt_opt41_2'){" +
                                            "$('.cboCual').val('').attr('disabled','disabled');" +
                                            "$('.cboCualEspecifique').val('').attr('disabled','disabled');" +
                                            "$('.divcboCualEspecifique').hide();" +
                                            "$('.cboComoSeConsidera').val('').attr('disabled','disabled');" +
                                            "$('.cboComoSeConsideraEspecifique').val('').attr('disabled','disabled');" +
                                            "$('.divcboComoSeConsideraEspecifique').hide();" +
                                            "$('.cboComparte').attr('disabled','disabled');" +
                                            "$('.form-group-radio-cboComparte').css('background-color', '#e9ecef'); " +
                                            "$('.cboPractica').attr('disabled','disabled');" +
                                            "$('.form-group-radio-cboPractica').css('background-color', '#e9ecef');" +
                                            "$('input[name=PER_NAC6]').iCheck('uncheck');" +
                                            "$('input[name=PER_NAC7]').iCheck('uncheck');" +
                                            "$('input[name=PER_NAC9]').iCheck('uncheck');" +
                                        "}" +
                                        "if(this.id == 'rbt_opt44_1'){" +
                                            "$('.cboConseptoAfro').val('').removeAttr('disabled');" +
                                        "}" +
                                        "if(this.id == 'rbt_opt44_2'){" +
                                            "$('.cboConseptoAfro').val('').attr('disabled','disabled');" +
                                            "$('.cboConseptoAfroEspecifique').val('').attr('disabled','disabled');" +
                                            "$('.divcboConseptoAfroEspecifique').hide();" +
                                        "}" +
                                    "}" +
                                "});" +
                                //divs ocultos
                                "$('#" + _postJSON.P_form + " .bloqueoPick').attr('disabled','disabled').selectpicker('refresh'); " +
                                "$('#" + _postJSON.P_form + " .bloqueoPick option[value=152]').remove().selectpicker('refresh');" +
                                "$('#" + _postJSON.P_form + " .bloqueo').attr('disabled','disabled'); " +

                                "$('.divcboCualEspecifique').hide();" +
                                "$('.divcboComoSeConsideraEspecifique').hide();" +
                                "$('.divcboConseptoAfroEspecifique').hide();" +
                                "$('.divcboHablaEspecifique').hide();" +
                                "$('.divcboReligionEspecifique').hide();" +
                               //fin divs ocultos
                               "$('.form-group-radio-cboComparte').css('background-color', '#e9ecef'); " +
                               "$('.form-group-radio-cboPractica').css('background-color', '#e9ecef');" +
                                 //funciones de seleccionables                                
                                 "$('#" + _postJSON.P_form + " .cboCual').on('change', function() {" +
                                     "var cbCual = $(this).val(); " +
                                     "if(cbCual == 2 || cbCual == 3 || cbCual == 4 || cbCual == 5 || cbCual == 6 || cbCual == 7 || cbCual == 8 || cbCual == 9 || cbCual == 10) {" +
                                         "$('.cboComparte').removeAttr('disabled');" +
                                         "$('input[name=PER_NAC6]').iCheck('uncheck');" +
                                         "$('.form-group-radio-cboComparte').css('background-color', ''); " +
                                         "$('.cboCualEspecifique').val('').attr('disabled','disabled');" +
                                         "$('.divcboCualEspecifique').hide();" +
                                         "$('.cboComoSeConsidera').val('').attr('disabled','disabled');" +
                                         "$('.cboComoSeConsideraEspecifique').val('').attr('disabled','disabled');" +
                                         "$('.divcboComoSeConsideraEspecifique').hide();" +
                                     "}else if(cbCual == 1){" +
                                         "$('.cboComoSeConsidera').val('').removeAttr('disabled');" +
                                         "$('.cboCualEspecifique').val('').attr('disabled','disabled');" +
                                         "$('.divcboCualEspecifique').hide();" +
                                     "}else if(cbCual == 11){" +
                                         "$('.cboCualEspecifique').val('').removeAttr('disabled');" +
                                         "$('.divcboCualEspecifique').show();" +
                                     "}else {" +
                                        "$('.cboCualEspecifique').val('').attr('disabled','disabled');" +
                                        "$('.divcboCualEspecifique').hide();" +
                                        "$('.cboComoSeConsidera').val('').attr('disabled','disabled');" +
                                        "$('.cboComoSeConsideraEspecifique').val('').attr('disabled','disabled');" +
                                        "$('.divcboComoSeConsideraEspecifique').hide();" +
                                     "}" +
                                 "});" +
                                 "$('#" + _postJSON.P_form + " .cboComoSeConsidera').on('change', function() {" +
                                     "var cboComoSe = $(this).val(); " +
                                     "if(cboComoSe == 1 || cboComoSe == 2 || cboComoSe == 3 || cboComoSe == 4 || cboComoSe == 5) {" +
                                         "$('.cboComoSeConsideraEspecifique').val('').attr('disabled','disabled');" +
                                         "$('.divcboComoSeConsideraEspecifique').hide();" +
                                         "$('.cboComparte').removeAttr('disabled');" +
                                         "$('input[name=PER_NAC6]').iCheck('uncheck');" +
                                         "$('.form-group-radio-cboComparte').css('background-color', ''); " +
                                         "$('.cboPractica').removeAttr('disabled');" +
                                         "$('input[name=PER_NAC7]').iCheck('uncheck');" +
                                         "$('.form-group-radio-cboPractica').css('background-color', ''); " +
                                     "}else if(cboComoSe == 6){" +
                                         "$('.cboComoSeConsideraEspecifique').val('').removeAttr('disabled');" +
                                         "$('.divcboComoSeConsideraEspecifique').show();" +
                                         "$('.cboComparte').removeAttr('disabled');" +
                                         "$('input[name=PER_NAC6]').iCheck('uncheck');" +
                                         "$('.form-group-radio-cboComparte').css('background-color', '');" +
                                         "$('.cboPractica').removeAttr('disabled');" +
                                         "$('input[name=PER_NAC7]').iCheck('uncheck');" +
                                         "$('.form-group-radio-cboPractica').css('background-color', '');" +
                                     "}else{" +
                                         "$('.cboComoSeConsideraEspecifique').val('').attr('disabled','disabled');" +
                                         "$('.divcboComoSeConsideraEspecifique').hide();" +
                                         "$('.cboComparte').attr('disabled','disabled');" +
                                         "$('input[name=PER_NAC6]').iCheck('uncheck');" +
                                         "$('.form-group-radio-cboComparte').css('background-color', '#e9ecef');" +
                                         "$('.cboPractica').attr('disabled','disabled');" +
                                         "$('input[name=PER_NAC7]').iCheck('uncheck');" +
                                         "$('.form-group-radio-cboPractica').css('background-color', '#e9ecef');" +
                                     "}" +
                                 "});" +
                                 "$('#" + _postJSON.P_form + " .cboConseptoAfro').on('change', function() {" +
                                     "var cboConsep = $(this).val(); " +
                                     "if(cboConsep == 1 || cboConsep == 2 || cboConsep == 3 || cboConsep == 4 || cboConsep == 5 || cboConsep == 6) {" +
                                         "$('.cboComoSeConsideraEspecifique').val('').attr('disabled','disabled');" +
                                         "$('.divcboComoSeConsideraEspecifique').hide();" +
                                         "$('.cboConseptoAfroEspecifique').val('').attr('disabled','disabled');" +
                                         "$('.divcboConseptoAfroEspecifique').hide();" +
                                     "}else if(cboConsep == 7){" +
                                         "$('.cboConseptoAfroEspecifique').val('').removeAttr('disabled');" +
                                         "$('.divcboConseptoAfroEspecifique').show();" +
                                     "}else{" +
                                         "$('.cboComoSeConsideraEspecifique').val('').attr('disabled','disabled');" +
                                         "$('.divcboComoSeConsideraEspecifique').hide();" +
                                         "$('.cboConseptoAfroEspecifique').val('').attr('disabled','disabled');" +
                                         "$('.divcboConseptoAfroEspecifique').hide();" +
                                     "}" +
                                 "});" +
                                 "$('#" + _postJSON.P_form + " .cboHabla').on('change', function() {" +
                                     "var cboHbl = $(this).val(); " +
                                     "if(cboHbl == 1 || cboHbl == 2 || cboHbl == 3 || cboHbl == 4 || cboHbl == 5 || cboHbl == 6 || cboHbl == 7) {" +
                                         "$('.cboHablaEspecifique').val('').attr('disabled','disabled');" +
                                         "$('.divcboHablaEspecifique').hide();" +
                                         "$('.cboLengua').val('').removeAttr('disabled');" +
                                     "}else if(cboHbl == 8){" +
                                         "$('.cboHablaEspecifique').val('').removeAttr('disabled');" +
                                         "$('.divcboHablaEspecifique').show();" +
                                         "$('.cboLengua').val('').removeAttr('disabled');" +
                                     "}else{" +
                                         "$('.cboComoSeConsideraEspecifique').val('').attr('disabled','disabled');" +
                                         "$('.divcboComoSeConsideraEspecifique').hide();" +
                                         "$('.cboHablaEspecifique').val('').attr('disabled','disabled');" +
                                         "$('.divcboHablaEspecifique').hide();" +
                                         "$('.cboLengua').val('').attr('disabled','disabled');" +
                                     "}" +
                                 "});" +
                                 "$('#" + _postJSON.P_form + " .cboReligion').on('change', function() {" +
                                    "var cboRel = $(this).val(); " +
                                    "if(cboRel == 10) {" +
                                        "$('.cboReligionEspecifique').val('').removeAttr('disabled');" +
                                        "$('.divcboReligionEspecifique').show();" +
                                     "}else {" +
                                        "$('.cboReligionEspecifique').val('').attr('disabled','disabled');" +
                                        "$('.divcboReligionEspecifique').hide();" +
                                     "}" +
                                "});" +
                //fin funciones seleccionables
                // funciones de load en campos
                "setTimeout(function () { " +
                    "$('#val1').val(" + dato_PER_NAC1 + "); " +
                    "$('#val2').val(" + dato_PER_NAC2 + "); " +
                    //"$('#val3').val(" + dato_PER_NAC3 + "); " +
                    "$('#val4').val(" + dato_PER_NAC4 + "); " +
                    "$('#val6').val(" + dato_PER_NAC6 + "); " +
                    "$('#val8').val(" + dato_PER_NAC8 + "); " +
                    "$('#val9').val(" + dato_PER_NAC9 + "); " +
                    "$('#val11').val(" + dato_PER_NAC11 + "); " +
                    "$('#val14').val(" + dato_PER_NAC14 + "); " +
                    "if ($('#val1').val() == '1') { " +
                        "$('.cboCual').removeAttr('disabled');" +
                    "}" +
                    "if ($('#val2').val() == '1') { " +
                        "$('.cboComoSeConsidera').removeAttr('disabled');" +
                    "}else if(($('#val2').val() > 1) && ($('#val2').val() < 11)) { " +
                        "$('.cboComparte').removeAttr('disabled');" +
                        "$('.form-group-radio-cboComparte').css('background-color', '');" +
                    "}else if($('#val2').val() == '11') { " +
                        "$('.cboCualEspecifique').removeAttr('disabled');" +
                        "$('.divcboCualEspecifique').show();" +
                    "}" +

                    "if ($('#val4').val() == '6') { " +
                        "$('.cboComoSeConsideraEspecifique').removeAttr('disabled');" +
                        "$('.divcboComoSeConsideraEspecifique').show();" +
                    "}" +

                    "if(($('#val6').val() >= 1) && ($('#val6').val() <= 6)) { " +
                        "$('.cboComparte').removeAttr('disabled');" +
                        "$('.form-group-radio-cboComparte').css('background-color', '');" +
                        "$('.cboPractica').removeAttr('disabled');" +
                        "$('.form-group-radio-cboPractica').css('background-color', '');" +
                    "}" +
                    "if ($('#val8').val() == '1') { " +
                        "$('.cboConseptoAfro').removeAttr('disabled');" +
                    "}" +
                    "if ($('#val9').val() == '7') { " +
                        "$('.cboConseptoAfroEspecifique').removeAttr('disabled');" +
                        "$('.divcboConseptoAfroEspecifique').show();" +
                    "}" +
                    "if ($('#val11').val() == '8') { " +
                        "$('.cboHablaEspecifique').removeAttr('disabled');" +
                        "$('.divcboHablaEspecifique').show();" +
                        "$('.cboLengua').removeAttr('disabled');" +
                    "}else if(($('#val11').val() >= 1) && ($('#val11').val() <= 7)) { " +
                        "$('.cboLengua').removeAttr('disabled');" +
                    "}" +                    

                    "if ($('#val14').val() == '10') { " +
                        "$('.cboReligionEspecifique').removeAttr('disabled');" +
                        "$('.divcboReligionEspecifique').show();" +
                    "}" +

                    "if (" + EdadPersona + " < 5) { " +
                        "$('.cboReligion').val('').attr('disabled','disabled');" +
                    "}" +

                "}, 500);"
            };

            return sb.ToString() + _methodCallLoad.CreaJQueryDocumentReady();
        }

        /// <summary>
        /// Obtiene formulario Sección Personas DISCAPACIDAD
        /// </summary>
        public string ObtieneFormularioSeccionPersonaPaso5(string token, int paso, string cuestionario = "")
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Persona
            PersonaBOL _personaBOL = new PersonaBOL();
            PersonaDAL _personaDAL = new PersonaDAL();

            _personaBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            _personaBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;
            _personaBOL.PK_PERSONA = _identificadorCuestionario.IdPersona;

            List<PersonaBOL> listaPersona = _personaDAL.Listar<PersonaBOL>(_personaBOL);
            if (listaPersona.Count > 0)
            {
                _personaBOL = listaPersona[0];
            }

            string NombrePersona = _personaBOL.PER_NOMBRE;
            string NPER = _personaBOL.NPER;

            // Carga opciones de respuesta
            GesFormPreguntasOpcionesBOL _gesFormPreguntasOpcionesBOL = new GesFormPreguntasOpcionesBOL();
            GesFormPreguntasOpcionesDAL _gesFormPreguntasOpcionesDAL = new GesFormPreguntasOpcionesDAL();
            List<GesFormPreguntasOpcionesBOL> listaOpcionesPregunta = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesPreguntaPorGrupos<GesFormPreguntasOpcionesBOL>("'111','112','113','114','115','116'");

            // Obtengo opciones de respuesta
            StringBuilder sbPER_DIS1 = new StringBuilder();
            StringBuilder sbPER_DIS2 = new StringBuilder();
            StringBuilder sbPER_DIS3 = new StringBuilder();
            StringBuilder sbPER_DIS4 = new StringBuilder();
            StringBuilder sbPER_DIS5 = new StringBuilder();
            StringBuilder sbPER_DIS6 = new StringBuilder();

            foreach (var item in listaOpcionesPregunta)
            {
                switch (item.Pk_form_preguntas)
                {
                    case 111:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_DIS1.ToString())
                        {
                            sbPER_DIS1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_DIS1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 112:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_DIS2.ToString())
                        {
                            sbPER_DIS2.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_DIS2.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 113:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_DIS3.ToString())
                        {
                            sbPER_DIS3.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_DIS3.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 114:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_DIS4.ToString())
                        {
                            sbPER_DIS4.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_DIS4.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 115:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_DIS5.ToString())
                        {
                            sbPER_DIS5.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_DIS5.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 116:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_DIS6.ToString())
                        {
                            sbPER_DIS6.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_DIS6.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                }
            }

            // Submit del formulario
            _postJSON.P_form = "formulario-persona-discapacidad";
            _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
            _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/persona/ingresar-datos-discapacidad";
            _postJSON.P_data_dinamica = true;
            _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioWeb(" + (paso + 1) + ",'" + token + "'); }";

            // Identificación Hogar-Persona
            sb.Append(ObtieneIdentificacionPersonas(token, NombrePersona));

            // Inicio Definición del Formulario Persona. 
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
            sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
            sb.Append("<input id=\"NPER\" name=\"NPER\" type=\"hidden\" value=\"" + NPER + "\"/>");
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");

            // Inicio Linea 27 (Pregunta 52 y 53)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
            sb.Append("<p style=\"margin-bottom:-2px;\"><strong>DISCAPACIDAD </strong></p>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<br>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>52.- ¿TIENE " + NombrePersona + " DIFICULTAD PERMANENTE PARA CAMINAR O SUBIR PELDAÑOS? </strong></p>");
            sb.Append("<select id=\"PER_DIS1\" name=\"PER_DIS1\" class=\"form-control\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_DIS1.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>53.- ¿TIENE " + NombrePersona + " DIFICULTAD PERMANENTE PARA ENTENDER, RECORDAR COSAS O CONCENTRARSE? </strong></p>");
            sb.Append("<select id=\"PER_DIS2\" name=\"PER_DIS2\" class=\"form-control\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_DIS2.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 27 (Pregunta 52 y 53)

            // Inicio Linea 28 (Pregunta 54 y 55)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>54.- ¿TIENE " + NombrePersona + " DIFICULTAD PERMANENTE PARA COMPRENDER O SER ENTENDIDO UTILIZANDO SU LENGUAJE HABITUAL? </strong></p>");
            sb.Append("<select id=\"PER_DIS3\" name=\"PER_DIS3\" class=\"form-control\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_DIS3.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>55.- ¿TIENE " + NombrePersona + " DIFICULTAD PERMANENTE PARA VER, INCLUSO CUANDO USA ANTEOJOS ÓPTICOS O LENTES? </strong></p>");
            sb.Append("<select id=\"PER_DIS4\" name=\"PER_DIS4\" class=\"form-control\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_DIS4.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 28 (Pregunta 54 y 55)

            // Inicio Linea 29 (Pregunta 56 y 57)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>56.- ¿TIENE " + NombrePersona + " DIFICULTAD PERMANENTE PARA OÍR, INCLUSO CUANDO USA UN DISPOSITIVO DE AYUDA PARA LA AUDICIÓN O AUDÍFONOS? </strong></p>");
            sb.Append("<select id=\"PER_DIS5\" name=\"PER_DIS5\" class=\"form-control\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_DIS5.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>57.- ¿TIENE " + NombrePersona + " DIFICULTAD PERMANENTE PARA REALIZAR SU CUIDADO PERSONAL POR SÍ MISMO, POR EJEMPLO, PARA COMER, BAÑARSE O VESTIRSE SOLO/A? </strong></p>");
            sb.Append("<select id=\"PER_DIS6\" name=\"PER_DIS6\" class=\"form-control\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_DIS6.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 29 (Pregunta 56 y 57)

            sb.Append("</div>");

            sb.Append("</div>");

            // Inicio Botones del Cuestionario
            sb.Append("<div class=\"row text-center\">");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<hr />");
            sb.Append("<div class=\"mensaje text-center\"></div>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
            sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso - 1) + ",'" + token + "');\"  class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
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
                               "$('.selectpicker').selectpicker();" +
                               "$('.magic-radio').iCheck({" +
                                    "checkboxClass: 'icheckbox_square-green'," +
                                    "radioClass: 'iradio_square-green'," +
                                    "increaseArea: '20%'" +
                               "});"
            };

            return sb.ToString() + _methodCallLoad.CreaJQueryDocumentReady();
        }

        /// <summary>
        /// Obtiene formulario Sección Personas EDUCACIÓN
        /// </summary>
        public string ObtieneFormularioSeccionPersonaPaso6(string token, int paso, string cuestionario = "")
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Persona
            PersonaBOL _personaBOL = new PersonaBOL();
            PersonaDAL _personaDAL = new PersonaDAL();

            _personaBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            _personaBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;
            _personaBOL.PK_PERSONA = _identificadorCuestionario.IdPersona;
            
            List<PersonaBOL> listaPersona = _personaDAL.Listar<PersonaBOL>(_personaBOL);
            if (listaPersona.Count > 0)
            {
                _personaBOL = listaPersona[0];
            }

            string NombrePersona = _personaBOL.PER_NOMBRE;
            string NPER = _personaBOL.NPER;
            string EdadPersona = _personaBOL.PER3.ToString();

            //declara variables para load
            string dato_PER_EDU1 = _personaBOL.PER_EDU1.ToString();
            string dato_PER_EDU2 = _personaBOL.PER_EDU2.ToString();
            string dato_PER_EDU3 = _personaBOL.PER_EDU3.ToString();
            string dato_PER_EDU4 = _personaBOL.PER_EDU4.ToString();
            string dato_PER_EDU5 = _personaBOL.PER_EDU5.ToString();
            string dato_PER_EDU6 = _personaBOL.PER_EDU6.ToString();
            string dato_PER_EDU7 = _personaBOL.PER_EDU7.ToString();
            string dato_PER_EDU9 = _personaBOL.PER_EDU9.ToString();
            string dato_PER_EDU14 = _personaBOL.PER_EDU14.ToString();// region comuna edu7

            // Carga opciones de respuesta
            GesFormPreguntasOpcionesBOL _gesFormPreguntasOpcionesBOL = new GesFormPreguntasOpcionesBOL();
            GesFormPreguntasOpcionesDAL _gesFormPreguntasOpcionesDAL = new GesFormPreguntasOpcionesDAL();
            List<GesFormPreguntasOpcionesBOL> listaOpcionesPregunta = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesPreguntaPorGrupos<GesFormPreguntasOpcionesBOL>("'999','888','100','101','102','103','104','105','107','109'");

            // Obtengo opciones de respuesta
            StringBuilder sbPER_EDU1 = new StringBuilder();
            StringBuilder sbPER_EDU2 = new StringBuilder();
            StringBuilder sbPER_EDU4 = new StringBuilder();
            StringBuilder sbPER_EDU6 = new StringBuilder();
            StringBuilder sbPER_EDU7 = new StringBuilder();
            StringBuilder sbPER_EDU8 = new StringBuilder();
            StringBuilder sbPER_EDU9 = new StringBuilder();
            StringBuilder sbPER_EDU14 = new StringBuilder();

            foreach (var item in listaOpcionesPregunta)
            {
                switch (item.Pk_form_preguntas)
                {
                    case 100:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_EDU1.ToString())
                        {
                            sbPER_EDU1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_EDU1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 101:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_EDU2.ToString())
                        {
                            sbPER_EDU2.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_EDU2.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 103:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_EDU4.ToString())
                        {
                            sbPER_EDU4.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_EDU4.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 105:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_EDU6.ToString())
                        {
                            sbPER_EDU6.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_EDU6.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 999:
                        // Se carga abajo
                        break;
                    case 888:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_EDU8.ToString())
                        {
                            sbPER_EDU8.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_EDU8.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 109:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_EDU9.ToString())
                        {
                            sbPER_EDU9.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_EDU9.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                }
            }

            // Cargo Región
            GesGeografiaDAL _gesGeografiaDAL = new GesGeografiaDAL();
            List<GesGeografiaBOL> listaRegion = _gesGeografiaDAL.ListarRegion<GesGeografiaBOL>();

            foreach (var item in listaRegion)
            {
                if (item.Geografia_codigo.ToString() == _personaBOL.PER_EDU14.ToString())
                {
                    sbPER_EDU14.Append("<option value=\"" + item.Geografia_codigo.ToString() + "\" selected>" + item.Geografia_nombre + "</option>");
                }
                else
                {
                    sbPER_EDU14.Append("<option value=\"" + item.Geografia_codigo.ToString() + "\">" + item.Geografia_nombre + "</option>");
                }
            }

            // Cargo Comunas
            //List<GesGeografiaBOL> listaComunas = _gesGeografiaDAL.ListarComunas<GesGeografiaBOL>();

            //foreach (var item in listaComunas)
            //{
            //    if (item.Geografia_codigo.ToString() == _personaBOL.PER_EDU7.ToString())
            //    {
            //        sbPER_EDU7.Append("<option value=\"" + item.Geografia_codigo.ToString() + "\" selected>" + item.Geografia_nombre + "</option>");
            //    }
            //    else
            //    {
            //        sbPER_EDU7.Append("<option value=\"" + item.Geografia_codigo.ToString() + "\">" + item.Geografia_nombre + "</option>");
            //    }
            //}

            // Submit del formulario
            _postJSON.P_form = "formulario-persona-educacion";
            _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
            _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/persona/ingresar-datos-educacion";
            _postJSON.P_data_dinamica = true;
            _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { " +
                                                 "if(" + EdadPersona + " > '14'){" +
                                                    "obtieneCuestionarioWeb(" + (paso + 1) + ",'" + token + "'); " +
                                                 "}else{" +
                                                    "obtieneCuestionarioWeb(" + (paso + 2) + ", '" + token + "'); " +
                                                 "}" +
                                             "}";

            // Identificación Hogar-Persona
            sb.Append(ObtieneIdentificacionPersonas(token, NombrePersona));

            // Inicio Definición del Formulario Persona. 
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
            sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
            sb.Append("<input id=\"NPER\" name=\"NPER\" type=\"hidden\" value=\"" + NPER + "\"/>");
            sb.Append("<input id=\"val1\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val2\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val3\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val4\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val5\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val6\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val9\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val14\" type=\"hidden\" value=\"\"/>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");

            // Inicio Linea 21 (Pregunta 46)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
            sb.Append("<p style=\"margin-bottom:-2px;\"><strong>EDUCACIÓN</strong></p>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<br>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>58.- ENTENDIENDO NIVEL EDUCATIVO COMO EDUCACIÓN DIFERENCIAL, PREESCOLAR, BÁSICA, MEDIA, SUPERIOR O POSTGRADO, ¿CUÁL ES EL NIVEL EDUCATIVO MÁS ALTO AL QUE " + NombrePersona + " HA LLEGADO?</strong><br>Se entiende que alguien alcanza o llega a un nivel, una vez que finaliza por lo menos un curso o año del nivel correspondiente.</p>");
            sb.Append("<select id=\"PER_EDU1\" name=\"PER_EDU1\" class=\"form-control cboNvlEducativo\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_EDU1.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 21 (Pregunta 46)

            // Inicio Linea 21 (Pregunta 47)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>59.- Y DENTRO DEL NIVEL (DECLARADO EN PREGUNTA ANTERIOR), ¿CUÁL ES EL ÚLTIMO CURSO/AÑO QUE " + NombrePersona + " HA APROBADO? </strong><br>Un curso aprobado aplica cuando la persona lo terminó y cumplió todos los requisitos para aprobarlo. En términos coloquiales, lo pasó.</p>");
            sb.Append("<select id=\"PER_EDU2\" name=\"PER_EDU2\" class=\"form-control bloqueo cboDentroNivel\" data-width=\"100%\">");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_EDU2.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 21 (Pregunta 47)

            // Inicio Linea 22 (Pregunta 48 y 48.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>60.- " + NombrePersona + ", ¿TERMINÓ EL NIVEL (DECLARADO EN LA PREGUNTA ANTERIOR)?  </strong><br>Recuerde que un nivel completado es aquel donde se obtiene un certificado que valida la aprobación.</p>");

            sb.Append("<div class=\"form-group-radio-cboTerminoNvl\">");

            if (_personaBOL.PER_EDU3 == "1")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt48_1\" class=\"magic-radio bloqueo cboTerminoNvl\" type=\"radio\" name=\"PER_EDU3\" value=\"1\" checked=\"checked\" >");
                sb.Append("<label for=\"rbt_opt48_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt48_1\" class=\"magic-radio bloqueo cboTerminoNvl\" type=\"radio\" name=\"PER_EDU3\" value=\"1\" >");
                sb.Append("<label for=\"rbt_opt48_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }

            if (_personaBOL.PER_EDU3 == "0")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt48_2\" class=\"magic-radio bloqueo cboTerminoNvl\" type=\"radio\" name=\"PER_EDU3\" value=\"0\" checked=\"checked\" >");
                sb.Append("<label for=\"rbt_opt48_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt48_2\" class=\"magic-radio bloqueo cboTerminoNvl\" type=\"radio\" name=\"PER_EDU3\" value=\"0\" >");
                sb.Append("<label for=\"rbt_opt48_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }

            sb.Append("</div>");

            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>61.- SI " + NombrePersona + " NO HA TERMINADO EL NIVEL DECLARADO, ¿CUÁL ES EL NIVEL MÁS ALTO FINALIZADO? </strong><br>El nivel finalizado implica logro de certificaciones o grados reconocidos por la autoridad educativa.</p>");
            sb.Append("<select id=\"PER_EDU4\" name=\"PER_EDU4\" class=\"form-control bloqueo cbonvlAlto\" data-width=\"100%\" >"); 
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_EDU4.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 22 (Pregunta 48 y 48.1)

            // Inicio Linea 23 (Pregunta 49)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>62.- ¿" + NombrePersona + " SE ENCUENTRA ESTUDIANDO ACTUALMENTE EN UN ESTABLECIMIENTO EDUCACIONAL? </strong></p>");

            sb.Append("<div class=\"form-group-radio-cboEstudiaAct\">");

            if (_personaBOL.PER_EDU5 == "1")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt50_1\" class=\"magic-radio bloqueo cboEstudiaAct\" type=\"radio\" name=\"PER_EDU5\" value=\"1\" checked=\"checked\" >");
                sb.Append("<label for=\"rbt_opt50_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt50_1\" class=\"magic-radio bloqueo cboEstudiaAct\" type=\"radio\" name=\"PER_EDU5\" value=\"1\" >");
                sb.Append("<label for=\"rbt_opt50_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }

            if (_personaBOL.PER_EDU5 == "0")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt50_2\" class=\"magic-radio bloqueo cboEstudiaAct\" type=\"radio\" name=\"PER_EDU5\" value=\"0\" checked=\"checked\" >");
                sb.Append("<label for=\"rbt_opt50_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt50_2\" class=\"magic-radio bloqueo cboEstudiaAct\" type=\"radio\" name=\"PER_EDU5\" value=\"0\" >");
                sb.Append("<label for=\"rbt_opt50_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }

            sb.Append("</div>");

            sb.Append("</div>");
            sb.Append("</div>");


            sb.Append("</div>");
            // Fin Linea 23 (Pregunta 49)

            // Inicio Linea 24 (Pregunta 50 y 50.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>63.- ¿EN QUÉ COMUNA O PAÍS SE UBICA EL LUGAR DE ESTUDIO DE " + NombrePersona + "?  </strong></p>");
            sb.Append("<select id=\"PER_EDU6\" name=\"PER_EDU6\" class=\"form-control bloqueo cboLugarEstudio\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_EDU6.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboLugarCom\">");
            sb.Append("<p><strong>63.1.- ¿EN QUÉ COMUNA?  </strong></p>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboLugarCom\">");
            sb.Append("<p><strong>REGIÓN  </strong></p>");
            sb.Append("<select id=\"PER_EDU14\" name=\"PER_EDU14\" class=\"form-control bloqueo cboLugarReg\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_EDU14.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboLugarCom\">");
            sb.Append("<p><strong>COMUNA  </strong></p>");
            sb.Append("<div id=\"filtro_select_com_7\" class=\"filtro_select_comuna_7\">");
            sb.Append("</div>");
            //sb.Append("<select id=\"PER_EDU7\" name=\"PER_EDU7\" class=\"form-control bloqueoPick cboLugarCom selectpicker\" data-live-search=\"true\" data-width=\"100%\" >");
            //sb.Append("<option value=\"\">Seleccione opción...</option>");
            //sb.Append(sbPER_EDU7.ToString());
            //sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 24 (Pregunta 50 y 50.1)

            // Inicio Linea 25 (Pregunta 50.2 y 50.3)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboLugarPais\">");
            sb.Append("<p><strong>63.1.- ¿EN QUÉ PAÍS?   </strong></p>");
            sb.Append("<select id=\"PER_EDU8\" name=\"PER_EDU8\" class=\"form-control bloqueoPick cboLugarPais selectpicker\" data-live-search=\"true\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_EDU8.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            //sb.Append("<div class=\"col-lg-12 col-md-12\">");
            //sb.Append("<div class=\"form-group txtOtro46 dP50_3\">");
            //sb.Append("<p><strong>50.3.- ¿EN QUÉ PAÍS?    </strong></p>");
            //sb.Append("<input id=\"p50_3\" name=\"p50_3\" type=\"text\" class=\"form-control\" placeholder=\"Especifique\" value=\"\" />");
            //sb.Append("</div>");
            //sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 25 (Pregunta 50.3)

            // Inicio Linea 26 (Pregunta 51 y 51.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group \">");
            sb.Append("<p><strong>64.- ¿CUÁL ES EL MEDIO DE TRANSPORTE PRINCIPAL QUE UTILIZA " + NombrePersona + " PARA DIRIGIRSE A SU LUGAR DE ESTUDIO?  </strong></p>");
            sb.Append("<select id=\"PER_EDU9\" name=\"PER_EDU9\" class=\"form-control bloqueo cboTransporte\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_EDU9.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboTransporteEspecifique\">");
            sb.Append("<p><strong>64.1.- ESPECIFIQUE EL OTRO MEDIO DE TRANSPORTE QUE UTILIZA PARA DIRIGIRSE A SU LUGAR DE ESTUDIO </strong></p>");
            sb.Append("<input id=\"PER_EDU10\" name=\"PER_EDU10\" type=\"text\" class=\"form-control bloqueo cboTransporteEspecifique\" onkeypress=\"SoloLetras()\" placeholder=\"Especifique\" value=\"" + _personaBOL.PER_EDU10 + "\"  />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 26 (Pregunta 51 y 51.1)

            // Inicio Linea 26 (Pregunta 51 y 51.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>65.- EN LOS ÚLTIMOS 3 MESES, ¿" + NombrePersona + " HA UTILIZADO ALGUNO DE LOS SIGUIENTES EQUIPOS O SERVICIOS EN CUALQUIER LUGAR? </strong></p>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>1) Teléfono móvil o Smartphone  </strong></p>");

            sb.Append("<div class=\"form-group-radio-cbomovil\">");

            if (_personaBOL.PER_EDU11 == "1")
            {                
                sb.Append("<div class=\"col-lg-12 col-md-12\">");
                sb.Append("<input id=\"rbt_opt651_1\" class=\"magic-radio bloqueo cbomovil\" type=\"radio\" name=\"PER_EDU11\" value=\"1\" checked=\"checked\"  >");
                sb.Append("<label for=\"rbt_opt651_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12 col-md-12\">");
                sb.Append("<input id=\"rbt_opt651_1\" class=\"magic-radio bloqueo cbomovil\" type=\"radio\" name=\"PER_EDU11\" value=\"1\"  >");
                sb.Append("<label for=\"rbt_opt651_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }

            if (_personaBOL.PER_EDU11 == "0")
            {
                sb.Append("<div class=\"col-lg-12 col-md-12\">");
                sb.Append("<input id=\"rbt_opt651_2\" class=\"magic-radio bloqueo cbomovil\" type=\"radio\" name=\"PER_EDU11\" value=\"0\" checked=\"checked\"  >");
                sb.Append("<label for=\"rbt_opt651_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12 col-md-12\">");
                sb.Append("<input id=\"rbt_opt651_2\" class=\"magic-radio bloqueo cbomovil\" type=\"radio\" name=\"PER_EDU11\" value=\"0\"  >");
                sb.Append("<label for=\"rbt_opt651_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }

            sb.Append("</div>");

            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>2) Computador (escritorio, portátil o Tablet)  </strong></p>");

            sb.Append("<div class=\"form-group-radio-cbocompu\">");

            if (_personaBOL.PER_EDU12 == "1")
            {
                sb.Append("<div class=\"col-lg-12 col-md-12\">");
                sb.Append("<input id=\"rbt_opt652_1\" class=\"magic-radio bloqueo cbocompu\" type=\"radio\" name=\"PER_EDU12\" value=\"1\" checked=\"checked\"  >");
                sb.Append("<label for=\"rbt_opt652_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12 col-md-12\">");
                sb.Append("<input id=\"rbt_opt652_1\" class=\"magic-radio bloqueo cbocompu\" type=\"radio\" name=\"PER_EDU12\" value=\"1\"  >");
                sb.Append("<label for=\"rbt_opt652_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }

            if (_personaBOL.PER_EDU12 == "0")
            {
                sb.Append("<div class=\"col-lg-12 col-md-12\">");
                sb.Append("<input id=\"rbt_opt652_2\" class=\"magic-radio bloqueo cbocompu\" type=\"radio\" name=\"PER_EDU12\" value=\"0\" checked=\"checked\"  >");
                sb.Append("<label for=\"rbt_opt652_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12 col-md-12\">");
                sb.Append("<input id=\"rbt_opt652_2\" class=\"magic-radio bloqueo cbocompu\" type=\"radio\" name=\"PER_EDU12\" value=\"0\"  >");
                sb.Append("<label for=\"rbt_opt652_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }

            sb.Append("</div>");

            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>3) Internet fija o móvil  </strong></p>");

            sb.Append("<div class=\"form-group-radio-cbointernet\">");

            if (_personaBOL.PER_EDU13 == "1")
            {
                sb.Append("<div class=\"col-lg-12 col-md-12\">");
                sb.Append("<input id=\"rbt_opt653_1\" class=\"magic-radio bloqueo cbointernet\" type=\"radio\" name=\"PER_EDU13\" value=\"1\" checked=\"checked\"  >");
                sb.Append("<label for=\"rbt_opt653_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12 col-md-12\">");
                sb.Append("<input id=\"rbt_opt653_1\" class=\"magic-radio bloqueo cbointernet\" type=\"radio\" name=\"PER_EDU13\" value=\"1\"  >");
                sb.Append("<label for=\"rbt_opt653_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }

            if (_personaBOL.PER_EDU13 == "0")
            {
                sb.Append("<div class=\"col-lg-12 col-md-12\">");
                sb.Append("<input id=\"rbt_opt653_2\" class=\"magic-radio bloqueo cbointernet\" type=\"radio\" name=\"PER_EDU13\" value=\"0\" checked=\"checked\"  >");
                sb.Append("<label for=\"rbt_opt653_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12 col-md-12\">");
                sb.Append("<input id=\"rbt_opt653_2\" class=\"magic-radio bloqueo cbointernet\" type=\"radio\" name=\"PER_EDU13\" value=\"0\"  >");
                sb.Append("<label for=\"rbt_opt653_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }
            sb.Append("</div>");

            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 26 (Pregunta 51 y 51.1)

            sb.Append("</div>");

            sb.Append("</div>");

            // Inicio Botones del Cuestionario
            sb.Append("<div class=\"row text-center\">");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<hr />");
            sb.Append("<div class=\"mensaje text-center\"></div>");
            sb.Append("</div>");
            if (int.Parse(EdadPersona) > 4)
            {
                sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
                sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso - 1) + ",'" + token + "');\"  class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
                sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso - 2) + ",'" + token + "');\"  class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
                sb.Append("</div>");
            }            
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
                               "$('.selectpicker').selectpicker();" +
                               "$('.magic-radio').iCheck({" +
                                    "checkboxClass: 'icheckbox_square-green'," +
                                    "radioClass: 'iradio_square-green'," +
                                    "increaseArea: '20%'" +
                               "}).on('ifChecked', function(event) {" +
                                    "if ($(this).is(':checked')){" +
                                        "if(this.id == 'rbt_opt48_1'){" +
                                            "$('.cboEstudiaAct').removeAttr('disabled');" +
                                            "$('input[name=PER_EDU5]').iCheck('uncheck');" +
                                            "$('.form-group-radio-cboEstudiaAct').css('background-color', ''); " +
                                            "$('.cbonvlAlto').val('').attr('disabled','disabled');" +
                                        "}" +
                                        "if(this.id == 'rbt_opt48_2'){" +
                                            "$('.cbonvlAlto').val('').removeAttr('disabled');" +
                                        "}" +
                                        "if(this.id == 'rbt_opt50_1'){" +
                                            "$('.cboLugarEstudio').val('').removeAttr('disabled');" +
                                            "$('.cbomovil').attr('disabled', 'disabled'); " +
                                            "$('input[name=PER_EDU11]').iCheck('uncheck');" +
                                            "$('.form-group-radio-cbomovil').css('background-color', '#e9ecef'); " +
                                            "$('.cbocompu').attr('disabled','disabled');" +
                                            "$('input[name=PER_EDU12]').iCheck('uncheck');" +
                                            "$('.form-group-radio-cbocompu').css('background-color', '#e9ecef'); " +
                                            "$('.cbointernet').attr('disabled','disabled');" +
                                            "$('input[name=PER_EDU13]').iCheck('uncheck');" +
                                            "$('.form-group-radio-cbointernet').css('background-color', '#e9ecef'); " +
                                        "}" +
                                        "if(this.id == 'rbt_opt50_2'){" +
                                            "if(" + EdadPersona + " < 5){" +
                                                "$('.cboLugarEstudio').val('').attr('disabled','disabled');" +
                                                "$('.cboLugarCom').val('').attr('disabled','disabled');" +
                                                "$('.cboLugarReg').val('').attr('disabled','disabled');" +
                                                "$('.divcboLugarCom').hide();" +
                                                "$('.cboLugarPais').val('').attr('disabled','disabled').selectpicker('refresh');" +
                                                "$('.divcboLugarPais').hide();" +
                                                "$('.cboTransporte').val('').attr('disabled','disabled');" +
                                                "$('.cboTransporteEspecifique').val('').attr('disabled','disabled');" +
                                                "$('.divcboTransporteEspecifique').hide();" +
                                                "$('.cbomovil').attr('disabled','disabled');" +
                                                "$('input[name=PER_EDU11]').iCheck('uncheck');" +
                                                "$('.form-group-radio-cbomovil').css('background-color', '#e9ecef'); " +
                                                "$('.cbocompu').attr('disabled','disabled');" +
                                                "$('input[name=PER_EDU12]').iCheck('uncheck');" +
                                                "$('.form-group-radio-cbocompu').css('background-color', '#e9ecef'); " +
                                                "$('.cbointernet').attr('disabled','disabled');" +
                                                "$('input[name=PER_EDU13]').iCheck('uncheck');" +
                                                "$('.form-group-radio-cbointernet').css('background-color', '#e9ecef'); " +
                                            "}else{" +
                                                "$('.cboLugarEstudio').val('').attr('disabled','disabled');" +
                                                "$('.cboLugarCom').val('').attr('disabled','disabled');" +
                                                "$('.cboLugarReg').val('').attr('disabled','disabled');" +
                                                "$('.divcboLugarCom').hide();" +
                                                "$('.cboLugarPais').val('').attr('disabled','disabled').selectpicker('refresh');" +
                                                "$('.divcboLugarPais').hide();" +
                                                "$('.cboTransporte').val('').attr('disabled','disabled');" +
                                                "$('.cboTransporteEspecifique').val('').attr('disabled','disabled');" +
                                                "$('.divcboTransporteEspecifique').hide();" +
                                                "$('.cbomovil').removeAttr('disabled');" +
                                                "$('input[name=PER_EDU11]').iCheck('uncheck');" +
                                                "$('.form-group-radio-cbomovil').css('background-color', ''); " +
                                                "$('.cbocompu').removeAttr('disabled');" +
                                                "$('input[name=PER_EDU12]').iCheck('uncheck');" +
                                                "$('.form-group-radio-cbocompu').css('background-color', ''); " +
                                                "$('.cbointernet').removeAttr('disabled');" +
                                                "$('input[name=PER_EDU13]').iCheck('uncheck');" +
                                                "$('.form-group-radio-cbointernet').css('background-color', ''); " +
                                            "}" +
                                        "}" +
                                    "}" +
                                "});" +
                               //divs ocultos
                               "$('#" + _postJSON.P_form + " .bloqueo').attr('disabled','disabled'); " +
                               "$('#" + _postJSON.P_form + " .bloqueoPick').attr('disabled','disabled').selectpicker('refresh'); " +
                               "$('#" + _postJSON.P_form + " .bloqueoPick option[value=152]').remove().selectpicker('refresh');" +

                               "$('.divcboLugarCom').hide();" +
                               "$('.divcboLugarPais').hide();" +
                               "$('.divcboTransporteEspecifique').hide();" +
                                //fin divs ocultos
                                "$('.form-group-radio-cboTerminoNvl').css('background-color', '#e9ecef'); " +
                                "$('.form-group-radio-cboEstudiaAct').css('background-color', '#e9ecef'); " +
                                "$('.form-group-radio-cbomovil').css('background-color', '#e9ecef'); " +
                                "$('.form-group-radio-cbocompu').css('background-color', '#e9ecef'); " +
                                "$('.form-group-radio-cbointernet').css('background-color', '#e9ecef'); " +

                                //funciones de seleccionables
                                "var dato_PregNvlEducativo = '';" +
                                "$('#" + _postJSON.P_form + " .cboNvlEducativo').on('change', function() {" +
                                    "var cbonvl = $(this).val(); " +
                                    "dato_PregNvlEducativo = $(this).val();" +
                                    "if(cbonvl == '') {" +
                                        "$('.bloqueo').val('').attr('disabled','disabled'); " +
                                        "$('.bloqueoPick').val('').attr('disabled','disabled').selectpicker('refresh'); " +
                                        "$('input[name=PER_EDU3]').iCheck('uncheck');" +
                                        "$('.form-group-radio-cboTerminoNvl').css('background-color', '#e9ecef'); " +
                                        "$('input[name=PER_EDU5]').iCheck('uncheck');" +
                                        "$('.form-group-radio-cboEstudiaAct').css('background-color', '#e9ecef'); " +
                                    "} else if(cbonvl == '1' || cbonvl == '2' || cbonvl == '3' || cbonvl == '4' || cbonvl == '5' || cbonvl == '6'){" +
                                        "$('.cboDentroNivel').val('').attr('disabled','disabled');" +
                                        "$('input[name=PER_EDU3]').iCheck('uncheck');" +
                                         "$('.form-group-radio-cboTerminoNvl').css('background-color', '#e9ecef'); " +
                                         "$('input[name=PER_EDU5]').iCheck('uncheck');" +
                                         "$('.form-group-radio-cboEstudiaAct').css('background-color', ''); " +
                                         "$('.cboEstudiaAct').removeAttr('disabled');" +
                                         "$('.cbonvlAlto').val('').attr('disabled','disabled');" +
                                    "}else {" +
                                        "$('.cboDentroNivel').val('').removeAttr('disabled');" +
                                        "$('input[name=PER_EDU5]').iCheck('uncheck');" +
                                        "$('.form-group-radio-cboEstudiaAct').css('background-color', '#e9ecef'); " +
                                    "}" + 
                                "});" +
                                "$('#" + _postJSON.P_form + " .cboDentroNivel').on('change', function() {" +
                                     "var cbdnvl = dato_PregNvlEducativo; " +
                                     "if(cbdnvl == '14' || cbdnvl == '15' || cbdnvl == '16' || cbdnvl == '17' || cbdnvl == '18' || cbdnvl == '12' || cbdnvl == '13') {" +
                                         "$('.cboTerminoNvl').removeAttr('disabled');" +
                                         "$('input[name=PER_EDU3]').iCheck('uncheck');" +
                                         "$('.form-group-radio-cboTerminoNvl').css('background-color', ''); " +
                                     "}else if(cbdnvl == '1' || cbdnvl == '2' || cbdnvl == '3' || cbdnvl == '4' || cbdnvl == '5' || cbdnvl == '6' || cbdnvl == '7' || cbdnvl == '8' || cbdnvl == '9' || cbdnvl == '10' || cbdnvl == '11' ) {" +
                                        "$('.cboTerminoNvl').attr('disabled','disabled');" +
                                        "$('input[name=PER_EDU3]').iCheck('uncheck');" +
                                        "$('.form-group-radio-cboTerminoNvl').css('background-color', '#e9ecef'); " +
                                        "$('.cbonvlAlto').val('').attr('disabled','disabled');" +
                                        "$('.cboEstudiaAct').removeAttr('disabled');" +
                                        "$('input[name=PER_EDU5]').iCheck('uncheck');" +
                                        "$('.form-group-radio-cboEstudiaAct').css('background-color', ''); " +
                                     "}else {" +
                                        "$('.cboTerminoNvl').attr('disabled','disabled');" +
                                        "$('input[name=PER_EDU3]').iCheck('uncheck');" +
                                        "$('.form-group-radio-cboTerminoNvl').css('background-color', '#e9ecef'); " +
                                        "$('.cbonvlAlto').val('').attr('disabled','disabled');" +
                                        "$('.cboEstudiaAct').attr('disabled','disabled');" +
                                        "$('input[name=PER_EDU5]').iCheck('uncheck');" +
                                        "$('.form-group-radio-cboEstudiaAct').css('background-color', '#e9ecef'); " +
                                     "}" +
                                 "});" +
                                 "$('#" + _postJSON.P_form + " .cbonvlAlto').on('change', function() {" +
                                     "var cbdnvlAl = $(this).val(); " +
                                     "if(cbdnvlAl == '1' || cbdnvlAl == '2' || cbdnvlAl == '3' || cbdnvlAl == '4' || cbdnvlAl == '5' || cbdnvlAl == '6' || cbdnvlAl == '7' || cbdnvlAl == '8' || cbdnvlAl == '9' || cbdnvlAl == '10') {" +
                                        "$('.cboEstudiaAct').removeAttr('disabled');" +
                                        "$('input[name=PER_EDU5]').iCheck('uncheck');" +
                                        "$('.form-group-radio-cboEstudiaAct').css('background-color', ''); " +
                                     "}else {" +
                                        "$('.cboEstudiaAct').val('').attr('disabled','disabled');" +
                                        "$('input[name=PER_EDU5]').iCheck('uncheck');" +
                                        "$('.form-group-radio-cboEstudiaAct').css('background-color', '#e9ecef'); " +
                                     "}" +
                                 "});" +
                                //cambio region
                                "$('#" + _postJSON.P_form + " .cboLugarReg').on('change', function() {" +
                                    "var cboreg = $(this).val(); " +
                                     "$('.filtro_select_comuna_7').empty().html(muestraComunasSegunRegion(cboreg, 7, 'cboLugarCom', 'PER_EDU7'));" +
                                 "});" +
                                 "$('#" + _postJSON.P_form + " .cboLugarEstudio').on('change', function() {" +
                                     "var cbLugar = $(this).val(); " +
                                     "if(cbLugar == 1) {" +
                                         "$('.cboLugarCom').val('').attr('disabled','disabled');" +
                                         "$('.cboLugarReg').val('').attr('disabled','disabled');" +
                                         "$('.divcboLugarCom').hide();" +
                                         "$('.cboLugarPais').val('').attr('disabled','disabled').selectpicker('refresh');" +
                                         "$('.divcboLugarPais').hide();" +
                                         "$('.cboTransporte').val('').removeAttr('disabled');" +
                                     "}else if(cbLugar == 2) {" +
                                         "$('.cboLugarReg').val('').removeAttr('disabled');" +
                                         "$('.cboLugarCom').val('').removeAttr('disabled');" +
                                         "$('.filtro_select_comuna_2').empty().html(muestraComunasSegunRegion('', 7, 'cboLugarCom', 'PER_EDU7'));" +
                                         "$('.divcboLugarCom').show();" +
                                         "$('.cboLugarPais').val('').attr('disabled','disabled').selectpicker('refresh');" +
                                         "$('.divcboLugarPais').hide();" +
                                         "$('.cboTransporte').val('').removeAttr('disabled');" +
                                     "}else if(cbLugar == 3) {" +
                                         "$('.cboLugarPais').val('').removeAttr('disabled').selectpicker('refresh');" +
                                         "$('.divcboLugarPais').show();" +
                                         "$('.cboLugarCom').val('').attr('disabled','disabled');" +
                                         "$('.cboLugarReg').val('').attr('disabled','disabled');" +
                                         "$('.divcboLugarCom').hide();" +
                                         "$('.cboTransporte').val('').removeAttr('disabled');" +
                                     "}else if(cbLugar == 4) {" +
                                         "$('.cboLugarCom').val('').attr('disabled','disabled');" +
                                         "$('.cboLugarReg').val('').attr('disabled','disabled');" +
                                         "$('.divcboLugarCom').hide();" +
                                         "$('.cboLugarPais').val('').attr('disabled','disabled').selectpicker('refresh');" +
                                         "$('.divcboLugarPais').hide();" +
                                         "$('.cboTransporte').val('').attr('disabled','disabled');" +
                                         "$('.cboTransporteEspecifique').val('').attr('disabled','disabled');" +
                                         "$('.divcboTransporteEspecifique').hide();" +
                                         "if(" + EdadPersona + " < 5){" +
                                            "$('.cbomovil').attr('disabled', 'disabled'); " +
                                            "$('input[name=PER_EDU11]').iCheck('uncheck');" +
                                            "$('.form-group-radio-cbomovil').css('background-color', '#e9ecef'); " +
                                            "$('.cbocompu').attr('disabled','disabled');" +
                                            "$('input[name=PER_EDU12]').iCheck('uncheck');" +
                                            "$('.form-group-radio-cbocompu').css('background-color', '#e9ecef'); " +
                                            "$('.cbointernet').attr('disabled','disabled');" +
                                            "$('input[name=PER_EDU13]').iCheck('uncheck');" +
                                            "$('.form-group-radio-cbointernet').css('background-color', '#e9ecef'); " +
                                        "}else{" +
                                            "$('.cbomovil').removeAttr('disabled');" +
                                            "$('input[name=PER_EDU11]').iCheck('uncheck');" +
                                            "$('.form-group-radio-cbomovil').css('background-color', ''); " +
                                            "$('.cbocompu').removeAttr('disabled');" +
                                            "$('input[name=PER_EDU12]').iCheck('uncheck');" +
                                            "$('.form-group-radio-cbocompu').css('background-color', ''); " +
                                            "$('.cbointernet').removeAttr('disabled');" +
                                            "$('input[name=PER_EDU13]').iCheck('uncheck');" +
                                            "$('.form-group-radio-cbointernet').css('background-color', ''); " +
                                        "}" +
                                     "}else{" +
                                         "$('.cboLugarCom').val('').attr('disabled','disabled');" +
                                         "$('.cboLugarReg').val('').attr('disabled','disabled');" +
                                         "$('.divcboLugarCom').hide();" +
                                         "$('.cboLugarPais').val('').attr('disabled','disabled').selectpicker('refresh');" +
                                         "$('.divcboLugarPais').hide();" +
                                         "$('.cboTransporte').val('').attr('disabled','disabled');" +
                                         "$('.cboTransporteEspecifique').val('').attr('disabled','disabled');" +
                                         "$('.divcboTransporteEspecifique').hide();" +
                                     "}" +
                                 "});" +
                                 "$('#" + _postJSON.P_form + " .cboTransporte').on('change', function() {" +
                                     "var cbTranspor = $(this).val(); " +
                                     "if(cbTranspor == '11') {" +
                                        "$('.cboTransporteEspecifique').val('').removeAttr('disabled');" +
                                        "$('.divcboTransporteEspecifique').show();" +
                                     "}else {" +
                                        "$('.cboTransporteEspecifique').val('').attr('disabled','disabled');" +
                                        "$('.divcboTransporteEspecifique').hide();" +
                                     "}" +
                                     "if(" + EdadPersona + " < 5){" +
                                        "$('.cbomovil').attr('disabled', 'disabled'); " +
                                        "$('input[name=PER_EDU11]').iCheck('uncheck');" +
                                        "$('.form-group-radio-cbomovil').css('background-color', '#e9ecef'); " +
                                        "$('.cbocompu').attr('disabled','disabled');" +
                                        "$('input[name=PER_EDU12]').iCheck('uncheck');" +
                                        "$('.form-group-radio-cbocompu').css('background-color', '#e9ecef'); " +
                                        "$('.cbointernet').attr('disabled','disabled');" +
                                        "$('input[name=PER_EDU13]').iCheck('uncheck');" +
                                        "$('.form-group-radio-cbointernet').css('background-color', '#e9ecef'); " +
                                    "}else{" +
                                        "$('.cbomovil').removeAttr('disabled');" +
                                        "$('input[name=PER_EDU11]').iCheck('uncheck');" +
                                        "$('.form-group-radio-cbomovil').css('background-color', ''); " +
                                        "$('.cbocompu').removeAttr('disabled');" +
                                        "$('input[name=PER_EDU12]').iCheck('uncheck');" +
                                        "$('.form-group-radio-cbocompu').css('background-color', ''); " +
                                        "$('.cbointernet').removeAttr('disabled');" +
                                        "$('input[name=PER_EDU13]').iCheck('uncheck');" +
                                        "$('.form-group-radio-cbointernet').css('background-color', ''); " +
                                    "}" +
                                 "});" +
                //fin funciones seleccionables
                // funciones de load en campos
                "setTimeout(function () { " +
                    "$('#val1').val(" + dato_PER_EDU1 + "); " +
                    "$('#val2').val(" + dato_PER_EDU2 + "); " +
                    "$('#val3').val(" + dato_PER_EDU3 + "); " +
                    "$('#val4').val(" + dato_PER_EDU4 + "); " +
                    "$('#val5').val(" + dato_PER_EDU5 + "); " +
                    "$('#val6').val(" + dato_PER_EDU6 + "); " +
                    "$('#val9').val(" + dato_PER_EDU9 + "); " +
                    "$('.filtro_select_comuna_7').empty().html(muestraComunasSegunRegion('" + dato_PER_EDU14 + "', 7, 'cboLugarCom', 'PER_EDU7'));" +

                    "if ($('#val1').val() >= 7 && $('#val1').val() <= 18) { " +
                        "$('.cboDentroNivel').removeAttr('disabled');" +
                    "}else if($('#val1').val() >= 1 && $('#val1').val() <= 6) { " +
                        "$('.cboDentroNivel').attr('disabled','disabled');" +
                        "$('input[name=PER_EDU3]').iCheck('uncheck');" +
                        "$('.form-group-radio-cboTerminoNvl').css('background-color', '#e9ecef'); " +
                        "$('.cbonvlAlto').attr('disabled','disabled');" +
                        "$('.form-group-radio-cboEstudiaAct').css('background-color', ''); " +
                        "$('.cboEstudiaAct').removeAttr('disabled');" +
                    "}" + 
                    "if ($('#val2').val() >= 1 && $('#val2').val() <= 8) { " +
                        "if ($('#val1').val() >= 14 && $('#val1').val() <= 18) { " +
                            "$('.cboTerminoNvl').removeAttr('disabled');" +
                            "$('.form-group-radio-cboTerminoNvl').css('background-color', ''); " +
                        "}else if($('#val1').val() >= 1 && $('#val1').val() <= 13) { " +
                            "$('.cboEstudiaAct').removeAttr('disabled');" +
                            "$('.form-group-radio-cboEstudiaAct').css('background-color', ''); " +
                        "}" +
                    "}" +
                    "if ($('#val3').val() == '1') { " +
                        "$('.cboEstudiaAct').removeAttr('disabled');" +
                        "$('.form-group-radio-cboEstudiaAct').css('background-color', ''); " +
                    "}else if($('#val3').val() == '0') { " +
                        "$('.cbonvlAlto').removeAttr('disabled');" +
                    "}" +
                    "if ($('#val4').val() >= 1 && $('#val4').val() <= 10) { " +
                        "$('.cboEstudiaAct').removeAttr('disabled');" +
                        "$('.form-group-radio-cboEstudiaAct').css('background-color', ''); " +
                    "}" +
                    "if ($('#val5').val() == '1') { " +
                        "$('.cboLugarEstudio').removeAttr('disabled');" +
                    "}else if($('#val5').val() == '0') { " +
                        "if(" + EdadPersona + " > 4){" +
                            "$('.cbomovil').removeAttr('disabled');" +
                            "$('.form-group-radio-cbomovil').css('background-color', ''); " +
                            "$('.cbocompu').removeAttr('disabled');" +
                            "$('.form-group-radio-cbocompu').css('background-color', ''); " +
                            "$('.cbointernet').removeAttr('disabled');" +
                            "$('.form-group-radio-cbointernet').css('background-color', ''); " +
                        "}" +
                    "}" +
                    "if ($('#val6').val() == '1') { " +
                        "$('.cboTransporte').removeAttr('disabled');" +
                    "}else if($('#val6').val() == '2') { " +
                        "$('.cboLugarCom').removeAttr('disabled');" +
                        "$('.filtro_select_comuna_7').empty().html(muestraComunasSegunRegion('" + dato_PER_EDU14 + "', 7, 'cboLugarCom', 'PER_EDU7'));" +
                        "setTimeout(function () { $('#PER_EDU7').val('" + dato_PER_EDU7 + "').change();}, 1500);" +
                        "$('.cboLugarReg').removeAttr('disabled');" +
                        "$('.divcboLugarCom').show();" +
                        "$('.cboTransporte').removeAttr('disabled');" +
                    "}else if($('#val6').val() == '3') { " +
                        "$('.cboLugarPais').removeAttr('disabled').selectpicker('refresh');" +
                        "$('.divcboLugarPais').show();" +
                        "$('.cboTransporte').removeAttr('disabled');" +
                    "}else if($('#val6').val() == '4') { " +
                        "if(" + EdadPersona + " > 4){" +
                            "$('.cbomovil').removeAttr('disabled');" +
                            "$('.form-group-radio-cbomovil').css('background-color', ''); " +
                            "$('.cbocompu').removeAttr('disabled');" +
                            "$('.form-group-radio-cbocompu').css('background-color', ''); " +
                            "$('.cbointernet').removeAttr('disabled');" +
                            "$('.form-group-radio-cbointernet').css('background-color', ''); " +
                        "}" +
                    "}" + 
                    "if ($('#val9').val() == '11') { " +
                        "$('.cboTransporteEspecifique').removeAttr('disabled');" +
                        "$('.divcboTransporteEspecifique').show();" +
                        "$('.cbomovil').removeAttr('disabled');" +
                        "$('.form-group-radio-cbomovil').css('background-color', ''); " +
                        "$('.cbocompu').removeAttr('disabled');" +
                        "$('.form-group-radio-cbocompu').css('background-color', ''); " +
                        "$('.cbointernet').removeAttr('disabled');" +
                        "$('.form-group-radio-cbointernet').css('background-color', ''); " +
                    "}else if($('#val9').val() != ''){" +
                        "$('.cbomovil').removeAttr('disabled');" +
                        "$('.form-group-radio-cbomovil').css('background-color', ''); " +
                        "$('.cbocompu').removeAttr('disabled');" +
                        "$('.form-group-radio-cbocompu').css('background-color', ''); " +
                        "$('.cbointernet').removeAttr('disabled');" +
                        "$('.form-group-radio-cbointernet').css('background-color', ''); " +
                    "}" +
                    "if(" + EdadPersona + " < 5){" +
                        "$('.cbomovil').attr('disabled', 'disabled'); " +
                        "$('input[name=PER_EDU11]').iCheck('uncheck');" +
                        "$('.form-group-radio-cbomovil').css('background-color', '#e9ecef'); " +
                        "$('.cbocompu').attr('disabled','disabled');" +
                        "$('input[name=PER_EDU12]').iCheck('uncheck');" +
                        "$('.form-group-radio-cbocompu').css('background-color', '#e9ecef'); " +
                        "$('.cbointernet').attr('disabled','disabled');" +
                        "$('input[name=PER_EDU13]').iCheck('uncheck');" +
                        "$('.form-group-radio-cbointernet').css('background-color', '#e9ecef'); " +
                    "}" +
                "}, 500);"
            };

            // Genero funcion para Carga comunas()
            GetJSON _getJSONListaComunasSegunRegion = new GetJSON();
            {
                _getJSONListaComunasSegunRegion.G_url_servicio = _appSettings.ServidorWeb + "api/persona/muestra-comuna-segun-region";
                _getJSONListaComunasSegunRegion.G_parametros = "{ num: num, codigo_id: codigo_id, clase_control: clase_control, id_campo: id_campo}";
                _getJSONListaComunasSegunRegion.G_respuesta_servicio = "if(codigo_id == 7){" +
                                                                            "$('#filtro_select_com_7').html(respuesta[0].elemento_html);" +
                                                                       "}";
            }
            CallMethod _methodCallMuestraListaComunasSegunRegion = new CallMethod
            {
                Mc_nombre = "muestraComunasSegunRegion(num, codigo_id, clase_control, id_campo)",
                Mc_contenido = _getJSONListaComunasSegunRegion.GetJSONCall()
            };

            sb.Append(_methodCallMuestraListaComunasSegunRegion.CreaJQueryFunction());

            return sb.ToString() + _methodCallLoad.CreaJQueryDocumentReady();
        }

        /// <summary>
        /// Obtiene formulario Sección Personas CARACTERISTICAS LABORALES
        /// </summary>
        public string ObtieneFormularioSeccionPersonaPaso7(string token, int paso, string cuestionario = "")
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Persona
            PersonaBOL _personaBOL = new PersonaBOL();
            PersonaDAL _personaDAL = new PersonaDAL();

            _personaBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            _personaBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;
            _personaBOL.PK_PERSONA = _identificadorCuestionario.IdPersona;
            List<PersonaBOL> listaPersona = _personaDAL.Listar<PersonaBOL>(_personaBOL);
            if (listaPersona.Count > 0)
            {
                _personaBOL = listaPersona[0];
            }

            string NombrePersona = _personaBOL.PER_NOMBRE;
            string NPER = _personaBOL.NPER;

            string dato_PER_LAB1 = _personaBOL.PER_LAB1.ToString();
            string dato_PER_LAB2 = _personaBOL.PER_LAB2.ToString();
            string dato_PER_LAB3 = _personaBOL.PER_LAB3.ToString();
            string dato_PER_LAB4 = _personaBOL.PER_LAB4.ToString();
            string dato_PER_LAB5 = _personaBOL.PER_LAB5.ToString();
            string dato_PER_LAB9 = _personaBOL.PER_LAB9.ToString();
            string dato_PER_LAB12 = _personaBOL.PER_LAB12.ToString();
            string dato_PER_LAB13 = _personaBOL.PER_LAB13.ToString();
            string dato_PER_LAB15 = _personaBOL.PER_LAB15.ToString();
            string dato_PER_LAB17 = _personaBOL.PER_LAB17.ToString();

            // Carga opciones de respuesta
            GesFormPreguntasOpcionesBOL _gesFormPreguntasOpcionesBOL = new GesFormPreguntasOpcionesBOL();
            GesFormPreguntasOpcionesDAL _gesFormPreguntasOpcionesDAL = new GesFormPreguntasOpcionesDAL();
            List<GesFormPreguntasOpcionesBOL> listaOpcionesPregunta = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesPreguntaPorGrupos<GesFormPreguntasOpcionesBOL>("'125','128','132','135','137','139','888','999'");

            // Obtengo opciones de respuesta
            StringBuilder sbPER_LAB2 = new StringBuilder();
            StringBuilder sbPER_LAB5 = new StringBuilder();
            StringBuilder sbPER_LAB9 = new StringBuilder();
            StringBuilder sbPER_LAB12 = new StringBuilder();
            StringBuilder sbPER_LAB13 = new StringBuilder();
            StringBuilder sbPER_LAB14 = new StringBuilder();
            StringBuilder sbPER_LAB15 = new StringBuilder();
            StringBuilder sbPER_LAB17 = new StringBuilder();

            foreach (var item in listaOpcionesPregunta)
            {
                switch (item.Pk_form_preguntas)
                {
                    case 125:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_LAB2.ToString())
                        {
                            sbPER_LAB2.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_LAB2.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 128:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_LAB5.ToString())
                        {
                            sbPER_LAB5.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_LAB5.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 132:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_LAB9.ToString())
                        {
                            sbPER_LAB9.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_LAB9.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 135:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_LAB12.ToString())
                        {
                            sbPER_LAB12.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_LAB12.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 999:
                        // Se carga abajo
                        break;
                    case 888:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_LAB14.ToString())
                        {
                            sbPER_LAB14.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_LAB14.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 139:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_LAB15.ToString())
                        {
                            sbPER_LAB15.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_LAB15.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                }
            }
            // Cargo Región
            GesGeografiaDAL _gesGeografiaDAL = new GesGeografiaDAL();
            List<GesGeografiaBOL> listaRegion = _gesGeografiaDAL.ListarRegion<GesGeografiaBOL>();

            foreach (var item in listaRegion)
            {
                if (item.Geografia_codigo.ToString() == _personaBOL.PER_LAB17.ToString())
                {
                    sbPER_LAB17.Append("<option value=\"" + item.Geografia_codigo.ToString() + "\" selected>" + item.Geografia_nombre + "</option>");
                }
                else
                {
                    sbPER_LAB17.Append("<option value=\"" + item.Geografia_codigo.ToString() + "\">" + item.Geografia_nombre + "</option>");
                }
            }
            // Cargo Comunas
            //GesGeografiaDAL _gesGeografiaDAL = new GesGeografiaDAL();
            //List<GesGeografiaBOL> listaComunas = _gesGeografiaDAL.ListarComunas<GesGeografiaBOL>();

            //foreach (var item in listaComunas)
            //{
            //    if (item.Geografia_codigo.ToString() == _personaBOL.PER_LAB13.ToString())
            //    {
            //        sbPER_LAB13.Append("<option value=\"" + item.Geografia_codigo.ToString() + "\" selected>" + item.Geografia_nombre + "</option>");
            //    }
            //    else
            //    {
            //        sbPER_LAB13.Append("<option value=\"" + item.Geografia_codigo.ToString() + "\">" + item.Geografia_nombre + "</option>");
            //    }
            //}

            // Submit del formulario
            _postJSON.P_form = "formulario-persona-laborales";
            _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
            _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/persona/ingresar-datos-laborales";
            _postJSON.P_data_dinamica = true;
            _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioWeb(" + (paso + 1) + ",'" + token + "'); }";

            // Identificación Hogar-Persona
            sb.Append(ObtieneIdentificacionPersonas(token, NombrePersona));

            // Inicio Definición del Formulario Persona. 
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
            sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
            sb.Append("<input id=\"NPER\" name=\"NPER\" type=\"hidden\" value=\"" + NPER + "\"/>");
            sb.Append("<input id=\"val1\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val2\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val3\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val4\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val5\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val9\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val12\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val15\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val17\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"valUltCuatro\" type=\"hidden\" value=\"\"/>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");

            // Inicio Linea 33 (Pregunta 63)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
            sb.Append("<p style=\"margin-bottom:-2px;\"><strong>CARACTERÍSTICAS LABORALES</strong></p>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<br>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>66.- LA SEMANA PASADA, ES DECIR, DE LUNES A DOMINGO, " + NombrePersona + " ¿TRABAJÓ POR UN PAGO O INGRESO? </strong><br>Se considera que trabajó si efectuó alguna actividad económica por, al menos, una hora.</p>");

            if (_personaBOL.PER_LAB1 == "1")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt64_1\" class=\"magic-radio\" type=\"radio\" name=\"PER_LAB1\" value=\"1\" checked=\"checked\"  >");
                sb.Append("<label for=\"rbt_opt64_1\" style=\"display: inline;\">&nbsp;Si Trabajó</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt64_1\" class=\"magic-radio\" type=\"radio\" name=\"PER_LAB1\" value=\"1\"  >");
                sb.Append("<label for=\"rbt_opt64_1\" style=\"display: inline;\">&nbsp;Si Trabajó</label>");
                sb.Append("</div>");
            }

            if (_personaBOL.PER_LAB1 == "0")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt64_2\" class=\"magic-radio\" type=\"radio\" name=\"PER_LAB1\" value=\"0\" checked=\"checked\"  >");
                sb.Append("<label for=\"rbt_opt64_2\" style=\"display: inline;\">&nbsp;No Trabajó</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt64_2\" class=\"magic-radio\" type=\"radio\" name=\"PER_LAB1\" value=\"0\"  >");
                sb.Append("<label for=\"rbt_opt64_2\" style=\"display: inline;\">&nbsp;No Trabajó</label>");
                sb.Append("</div>");
            }

            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 33 (Pregunta 63)

            // Inicio Linea 34 (Pregunta 64 y 65)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>67.- INDEPENDIENTE DE LO ANTERIOR, LA SEMANA PASADA, " + NombrePersona + ": </strong></p>");
            sb.Append("<select id=\"PER_LAB2\" name=\"PER_LAB2\" class=\"form-control bloqueo cboIndAnterior\" data-width=\"100%\" >"); 
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_LAB2.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");           

            sb.Append("</div>");
            // Fin Linea 34 (Pregunta 64 y 65)

            // Inicio Linea 34 (Pregunta 64 y 65)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group txtOtro63 dP65\">");
            sb.Append("<p><strong>68.- EN LAS ÚLTIMAS CUATRO SEMANAS, ¿" + NombrePersona + " BUSCÓ UN TRABAJO REMUNERADO O HA HECHO GESTIONES PARA CREAR UNA EMPRESA O NEGOCIO? </strong></p>");

            sb.Append("<div class=\"form-group-radio-cboUltimascuatro\">");

            if (_personaBOL.PER_LAB3 == "1")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt66_1\" class=\"magic-radio bloqueo cboUltimascuatro\" type=\"radio\" name=\"PER_LAB3\" value=\"1\" checked=\"checked\" >");
                sb.Append("<label for=\"rbt_opt66_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt66_1\" class=\"magic-radio bloqueo cboUltimascuatro\" type=\"radio\" name=\"PER_LAB3\" value=\"1\" >");
                sb.Append("<label for=\"rbt_opt66_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }

            if (_personaBOL.PER_LAB3 == "0")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt66_2\" class=\"magic-radio bloqueo cboUltimascuatro\" type=\"radio\" name=\"PER_LAB3\" value=\"0\" checked=\"checked\" >");
                sb.Append("<label for=\"rbt_opt66_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt66_2\" class=\"magic-radio bloqueo cboUltimascuatro\" type=\"radio\" name=\"PER_LAB3\" value=\"0\" >");
                sb.Append("<label for=\"rbt_opt66_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }

            sb.Append("</div>");

            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 34 (Pregunta 64 y 65)

            // Inicio Linea 35 (Pregunta 66)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>69.- SI UN TRABAJO O UNA OPORTUNIDAD DE NEGOCIOS ESTUVIERA DISPONIBLE, ¿PODRÍA " + NombrePersona + " COMENZAR A TRABAJAR DENTRO DE LAS PRÓXIMAS DOS SEMANAS?</strong></p>");

            sb.Append("<div class=\"form-group-radio-cboEstDisponible\">");

            if (_personaBOL.PER_LAB4 == "1")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt67_1\" class=\"magic-radio bloqueo cboEstDisponible\" type=\"radio\" name=\"PER_LAB4\" value=\"1\" checked=\"checked\" >");
                sb.Append("<label for=\"rbt_opt67_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt67_1\" class=\"magic-radio bloqueo cboEstDisponible\" type=\"radio\" name=\"PER_LAB4\" value=\"1\" >");
                sb.Append("<label for=\"rbt_opt67_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }

            if (_personaBOL.PER_LAB4 == "0")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt67_2\" class=\"magic-radio bloqueo cboEstDisponible\" type=\"radio\" name=\"PER_LAB4\" value=\"0\" checked=\"checked\" >");
                sb.Append("<label for=\"rbt_opt67_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt67_2\" class=\"magic-radio bloqueo cboEstDisponible\" type=\"radio\" name=\"PER_LAB4\" value=\"0\" >");
                sb.Append("<label for=\"rbt_opt67_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }

            sb.Append("</div>");

            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 35 (Pregunta 66)

            // Inicio Linea 36 (Pregunta 67 y 67.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>70.- ¿CUÁL DE LAS SIGUIENTES RAZONES DESCRIBE MEJOR SU SITUACIÓN ACTUAL? </strong><br>Si hay más de una razón que describa la situación actual de " + NombrePersona + ", debe priorizar la que más la identifique.</p>");
            sb.Append("<select id=\"PER_LAB5\" name=\"PER_LAB5\" class=\"form-control bloqueo cboSituActual\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_LAB5.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboSituActualEspecifique\">");
            sb.Append("<p><strong>70.1.- ESPECIFIQUE LA OTRA SITUACIÓN QUE DESCRIBE MEJOR SU SITUACIÓN ACTUAL </strong></p>");
            sb.Append("<input id=\"PER_LAB6\" name=\"PER_LAB6\" type=\"text\" class=\"form-control bloqueo cboSituActualEspecifique\" onkeypress=\"SoloLetras()\" placeholder=\"Especifique\" value=\"" + _personaBOL.PER_LAB6 + "\"  />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 36 (Pregunta 67 y 67.1)

            // Inicio Linea 37 (Pregunta 68 y 68.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>71.- ¿CUÁL ES EL OFICIO, LABOR U OCUPACIÓN PRINCIPAL QUE REALIZÓ " + NombrePersona + " LA SEMANA PASADA? </strong><br>El oficio, labor u ocupación refiere al tipo de trabajo que realiza " + NombrePersona + ". Por ejemplo: minero, campesino, guardia, profesor de básica, analista, fabricante de dulces, vendedor de celulares, etc.</p>");
            sb.Append("<input id=\"PER_LAB7\" name=\"PER_LAB7\" type=\"text\" class=\"form-control bloqueo cboOficio\" onkeypress=\"SoloLetras()\" placeholder=\"Especifique\" value=\"" + _personaBOL.PER_LAB7 + "\"  />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group txtOtro63 dP68_1\">");
            sb.Append("<p><strong>71.1.- ¿QUÉ TAREAS REALIZÓ EN ESE TRABAJO? </strong><br>Detalla las actividades que " + NombrePersona + " realiza en función de lo contestado en la pregunta anterior. Por ejemplo: enseñar, criar animales, vender fruta, etc.</p>");
            sb.Append("<input id=\"PER_LAB8\" name=\"PER_LAB8\" type=\"text\" class=\"form-control bloqueo cboOficioTareas\" onkeypress=\"SoloLetras()\" placeholder=\"Especifique\" value=\"" + _personaBOL.PER_LAB8 + "\"  />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 37 (Pregunta 68 y 68.1)

            // Inicio Linea 38 (Pregunta 69 y 70.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>72.- ¿" + NombrePersona + " TRABAJA COMO…? </strong></p>");
            sb.Append("<select id=\"PER_LAB9\" name=\"PER_LAB9\" class=\"form-control bloqueo cboTrabajaComo\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_LAB9.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>73.1.- ¿A QUÉ SE DEDICA LA EMPRESA, NEGOCIO O INSTITUCIÓN DONDE TRABAJA " + NombrePersona + "? </strong><br>Refiere a la actividad de la empresa, negocio o institución y no lo que hace " + NombrePersona + " en ese lugar. Por ejemplo: venta de alimentos, fábrica de ropa, salud pública, etc.</p>");
            sb.Append("<input id=\"PER_LAB10\" name=\"PER_LAB10\" type=\"text\" class=\"form-control bloqueo cboSeDedica\" onkeypress=\"SoloLetras()\" placeholder=\"Especifique\" value=\"" + _personaBOL.PER_LAB10 + "\"  />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 38 (Pregunta 69 y 70.1)

            // Inicio Linea 39 (Pregunta 70.2)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>73.2.- ¿A QUÉ SE DEDICA " + NombrePersona + " COMO TRABAJADOR POR CUENTA PROPIA?</strong><br>Refiere a la actividad de la empresa o negocio, y no lo que hace " + NombrePersona + " en específico. Por ejemplo: venta de alimentos, fábrica de ropa, servicios de salud, servicios de limpieza, etc.</p>");
            sb.Append("<input id=\"PER_LAB11\" name=\"PER_LAB11\" type=\"text\" class=\"form-control bloqueo cboDedicaPropia\" onkeypress=\"SoloLetras()\" placeholder=\"Especifique\" value=\"" + _personaBOL.PER_LAB11 + "\"  />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 39 (Pregunta 70.2 )

            // Inicio Linea 40 (Pregunta 71 y 71.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>74.- ¿EN QUÉ COMUNA O PAÍS SE UBICA EL TRABAJO DE " + NombrePersona + "? </strong><br>Si trabaja en más de una comuna, registre aquella donde pase la mayor parte del tiempo.</p>");
            sb.Append("<select id=\"PER_LAB12\" name=\"PER_LAB12\" class=\"form-control bloqueo cboUbicaTrab\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_LAB12.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboUbicaCom\">");
            sb.Append("<p><strong>74.1.- ¿EN QUÉ COMUNA? </strong></p>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboUbicaCom\">");
            sb.Append("<p><strong>REGIÓN  </strong></p>");
            sb.Append("<select id=\"PER_LAB17\" name=\"PER_LAB17\" class=\"form-control bloqueo cboUbicaReg\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_LAB17.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboUbicaCom\">");
            sb.Append("<p><strong>COMUNA </strong></p>");
            sb.Append("<div id=\"filtro_select_com_13\" class=\"filtro_select_comuna_13\">");
            sb.Append("</div>");
            //sb.Append("<select id=\"PER_LAB13\" name=\"PER_LAB13\" class=\"form-control bloqueoPick cboUbicaCom selectpicker\" data-live-search=\"true\" data-width=\"100%\" >");
            //sb.Append("<option value=\"\">Seleccione opción...</option>");
            //sb.Append(sbPER_LAB13.ToString());
            //sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboUbicaPais\">");
            sb.Append("<p><strong>74.1 ¿EN QUÉ PAÍS?  </strong></p>");
            sb.Append("<select id=\"PER_LAB14\" name=\"PER_LAB14\" class=\"form-control bloqueoPick cboUbicaPais cboPadre71_2 selectpicker\" data-live-search=\"true\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_LAB14.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            //sb.Append("<div class=\"col-lg-12 col-md-12\">");
            //sb.Append("<div class=\"form-group txtOtro63 dP71_3\">");
            //sb.Append("<p><strong>71.3.- Especifique el otro país </strong></p>");
            //sb.Append("<input id=\"p71_3\" name=\"p71_3\" type=\"text\" class=\"form-control\" placeholder=\"Especifique\" value=\"\" />");
            //sb.Append("</div>");
            //sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 41 (Pregunta 71.2 y 71.3)

            // Inicio Linea 42 (Pregunta 72 y 2.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>75.- ¿CUÁL ES EL MEDIO DE TRANSPORTE PRINCIPAL QUE UTILIZA " + NombrePersona + " PARA DIRIGIRSE A SU TRABAJO? </strong></p>");
            sb.Append("<select id=\"PER_LAB15\" name=\"PER_LAB15\" class=\"form-control bloqueo cboTransporte\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_LAB15.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboTransporteEspecifique\">");
            sb.Append("<p><strong>75.1.- ESPECIFIQUE EL OTRO MEDIO DE TRANSPORTE</strong></p>");
            sb.Append("<input id=\"PER_LAB16\" name=\"PER_LAB16\" type=\"text\" class=\"form-control bloqueo cboTransporteEspecifique\" onkeypress=\"SoloLetras()\" placeholder=\"Especifique\" value=\"" + _personaBOL.PER_LAB16 + "\"  />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 42 (Pregunta 72 y 2.1)

            sb.Append("</div>");

            sb.Append("</div>");

            // Inicio Botones del Cuestionario
            sb.Append("<div class=\"row text-center\">");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<hr />");
            sb.Append("<div class=\"mensaje text-center\"></div>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
            sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso - 1) + ",'" + token + "');\"  class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
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
                               "$('.selectpicker').selectpicker();" +
                               "$('.magic-radio').iCheck({" +
                                    "checkboxClass: 'icheckbox_square-green'," +
                                    "radioClass: 'iradio_square-green'," +
                                    "increaseArea: '20%'" +
                                "}).on('ifChecked', function(event) {" +
                                    "if ($(this).is(':checked')){" +
                                        "if(this.id == 'rbt_opt64_1'){" +
                                            "$('.cboOficio').val('').removeAttr('disabled');" +
                                            "$('.cboOficioTareas').val('').removeAttr('disabled');" +
                                            "$('.cboTrabajaComo').val('').removeAttr('disabled');" +
                                            "$('.cboUbicaTrab').val('').removeAttr('disabled');" +
                                            "$('.cboIndAnterior').val('').attr('disabled','disabled');" +
                                            "$('.cboUltimascuatro').attr('disabled','disabled');" +
                                            "$('input[name=PER_LAB3]').iCheck('uncheck');" +
                                            "$('.form-group-radio-cboUltimascuatro').css('background-color', '#e9ecef'); " +
                                            "$('input[name=PER_LAB4]').iCheck('uncheck');" +
                                            "$('.cboEstDisponible').attr('disabled','disabled');" +
                                            "$('.form-group-radio-cboEstDisponible').css('background-color', '#e9ecef'); " +
                                            "$('.cboSituActual').val('').attr('disabled','disabled');" +
                                            "$('.cboSituActualEspecifique').val('').attr('disabled','disabled');" +
                                            "$('.divcboSituActualEspecifique').hide();" +
                                        "}" +
                                        "if(this.id == 'rbt_opt64_2'){" +
                                            "$('.cboIndAnterior').val('').removeAttr('disabled');" +
                                            // despues de preg 69
                                            "$('.cboOficio').val('').attr('disabled','disabled');" +
                                            "$('.cboOficioTareas').val('').attr('disabled','disabled');" +
                                            "$('.cboTrabajaComo').val('').attr('disabled','disabled');" +
                                            "$('.cboSeDedica').val('').attr('disabled','disabled');" +
                                            "$('.cboDedicaPropia').val('').attr('disabled','disabled');" +
                                            "$('.cboUbicaTrab').val('').attr('disabled','disabled');" +
                                            "$('.cboUbicaCom').val('').attr('disabled','disabled');" +
                                            "$('.cboUbicaReg').val('').attr('disabled','disabled');" +
                                            "$('.divcboUbicaCom').hide();" +
                                             "$('.cboUbicaPais').val('').attr('disabled','disabled').selectpicker('refresh');" +
                                             "$('.divcboUbicaPais').hide();" +
                                             "$('.cboTransporte').val('').attr('disabled','disabled');" +
                                             "$('.cboTransporteEspecifique').val('').attr('disabled','disabled');" +
                                             "$('.divcboTransporteEspecifique').hide();" +
                                        "}" +
                                        "if(this.id == 'rbt_opt66_1'){" +
                                            "$('.cboEstDisponible').removeAttr('disabled');" +
                                            "$('input[name=PER_LAB4]').iCheck('uncheck');" +
                                            "$('.form-group-radio-cboEstDisponible').css('background-color', ''); " +
                                            "$('#valUltCuatro').val(1); " +
                                        "}" +
                                        "if(this.id == 'rbt_opt66_2'){" +
                                            "$('.cboEstDisponible').removeAttr('disabled');" +
                                            "$('input[name=PER_LAB4]').iCheck('uncheck');" +
                                            "$('.form-group-radio-cboEstDisponible').css('background-color', ''); " +
                                            "$('#valUltCuatro').val(0); " +
                                        "}" +
                                        "if(this.id == 'rbt_opt67_1'){" +                                            
                                            "if($('#valUltCuatro').val() == 1) {" +
                                                "$('.cboSituActual').val('').attr('disabled','disabled');" +
                                                "$('.cboSituActualEspecifique').val('').attr('disabled','disabled');" +
                                                "$('.divcboSituActualEspecifique').hide();" +
                                                "$('.cboOficio').val('').attr('disabled','disabled');" +
                                                "$('.cboOficioTareas').val('').attr('disabled','disabled');" +
                                                "$('.cboTrabajaComo').val('').attr('disabled','disabled');" +
                                                "$('.cboSeDedica').val('').attr('disabled','disabled');" +
                                                "$('.cboDedicaPropia').val('').attr('disabled','disabled');" +
                                                "$('.cboUbicaTrab').val('').attr('disabled','disabled');" +
                                                "$('.cboUbicaCom').val('').attr('disabled','disabled');" +
                                                "$('.cboUbicaReg').val('').attr('disabled','disabled');" +
                                                "$('.divcboUbicaCom').hide();" +
                                                "$('.cboUbicaPais').val('').attr('disabled','disabled').selectpicker('refresh');" +
                                                "$('.divcboUbicaPais').hide();" +
                                                "$('.cboTransporte').val('').attr('disabled','disabled');" +
                                                "$('.cboTransporteEspecifique').val('').attr('disabled','disabled');" +
                                                "$('.divcboTransporteEspecifique').hide();" +
                                            "}else {" +
                                                "$('.cboSituActual').val('').removeAttr('disabled');" +
                                            "}" +                                            
                                        "}" +
                                        "if(this.id == 'rbt_opt67_2'){" +
                                            "$('.cboSituActual').val('').removeAttr('disabled');" +
                                        "}" +
                                    "}" +
                                "});" +
                               ////divs ocultos
                               "$('#" + _postJSON.P_form + " .bloqueo').attr('disabled','disabled'); " +
                               "$('#" + _postJSON.P_form + " .bloqueoPick').attr('disabled','disabled').selectpicker('refresh'); " +
                               "$('#" + _postJSON.P_form + " .bloqueoPick option[value=152]').remove().selectpicker('refresh');" +

                               "$('.divcboUbicaCom').hide();" +
                               "$('.divcboUbicaPais').hide();" +
                               "$('.divcboSituActualEspecifique').hide();" +
                               "$('.divcboTransporteEspecifique').hide();" +
                                 // //fin divs ocultos
                                 "$('.form-group-radio-cboUltimascuatro').css('background-color', '#e9ecef'); " +
                                 "$('.form-group-radio-cboEstDisponible').css('background-color', '#e9ecef'); " +
                                 // //funciones de seleccionables
                                 "$('#" + _postJSON.P_form + " .cboIndAnterior').on('change', function() {" +
                                    "var cboInd = $(this).val(); " +
                                    "if(cboInd == 1 || cboInd == 2 || cboInd == 3) {" +
                                        "$('.cboOficio').val('').removeAttr('disabled');" +
                                        "$('.cboOficioTareas').val('').removeAttr('disabled');" +
                                        "$('.cboTrabajaComo').val('').removeAttr('disabled');" +
                                        "$('.cboUbicaTrab').val('').removeAttr('disabled');" +
                                        "$('.cboUltimascuatro').attr('disabled','disabled');" +
                                        "$('input[name=PER_LAB3]').iCheck('uncheck');" +
                                        "$('.form-group-radio-cboUltimascuatro').css('background-color', '#e9ecef'); " +
                                        "$('.cboEstDisponible').attr('disabled','disabled');" +
                                        "$('input[name=PER_LAB4]').iCheck('uncheck');" +
                                        "$('.form-group-radio-cboEstDisponible').css('background-color', '#e9ecef'); " +
                                        "$('.cboSituActual').val('').attr('disabled','disabled');" +
                                        "$('.cboSituActualEspecifique').val('').attr('disabled','disabled');" +
                                        "$('.divcboSituActualEspecifique').hide();" +
                                    "} else if(cboInd == 4){" +
                                        "$('.cboOficio').val('').attr('disabled','disabled');" +
                                        "$('.cboOficioTareas').val('').attr('disabled','disabled');" +
                                        "$('.cboTrabajaComo').val('').attr('disabled','disabled');" +
                                        "$('.cboUbicaTrab').val('').attr('disabled','disabled');" +
                                        "$('.cboUltimascuatro').removeAttr('disabled');" +
                                        "$('input[name=PER_LAB3]').iCheck('uncheck');" +
                                        "$('.form-group-radio-cboUltimascuatro').css('background-color', ''); " +
                                    "}else {" +
                                        "$('.cboOficio').val('').attr('disabled','disabled');" +
                                        "$('.cboOficioTareas').val('').attr('disabled','disabled');" +
                                        "$('.cboTrabajaComo').val('').attr('disabled','disabled');" +
                                        "$('.cboUbicaTrab').val('').attr('disabled','disabled');" +
                                        "$('input[name=PER_LAB3]').iCheck('uncheck');" +
                                        "$('.cboUltimascuatro').attr('disabled','disabled');" +
                                        "$('input[name=PER_LAB3]').iCheck('uncheck');" +
                                        "$('.form-group-radio-cboUltimascuatro').css('background-color', '#e9ecef'); " +
                                        "$('.cboEstDisponible').attr('disabled','disabled');" +
                                        "$('input[name=PER_LAB4]').iCheck('uncheck');" +
                                        "$('.form-group-radio-cboEstDisponible').css('background-color', '#e9ecef'); " +
                                        "$('.cboSituActual').val('').attr('disabled','disabled');" +
                                        "$('.cboSituActualEspecifique').val('').attr('disabled','disabled');" +
                                        "$('.divcboSituActualEspecifique').hide();" +
                                    "}" +
                                "});" +
                                "$('#" + _postJSON.P_form + " .cboSituActual').on('change', function() {" +
                                    "var cbSitAct = $(this).val(); " +
                                    "if(cbSitAct == 7) {" +
                                        "$('.cboSituActualEspecifique').val('').removeAttr('disabled');" +
                                        "$('.divcboSituActualEspecifique').show();" +
                                    "}" +
                                    "else {" +
                                        "$('.cboSituActualEspecifique').val('').attr('disabled','disabled');" +
                                        "$('.divcboSituActualEspecifique').hide();" +
                                    "}" +
                                "});" +
                                "$('#" + _postJSON.P_form + " .cboTrabajaComo').on('change', function() {" +
                                    "var cbtbjComo = $(this).val(); " +
                                    "if(cbtbjComo == 1 || cbtbjComo == 2 || cbtbjComo == 4) {" +
                                        "$('.cboDedicaPropia').val('').removeAttr('disabled');" +
                                        "$('.cboSeDedica').val('').attr('disabled','disabled');" +
                                    "} else if(cbtbjComo == 3 || cbtbjComo == 5){" +
                                        "$('.cboSeDedica').val('').removeAttr('disabled');" +
                                        "$('.cboDedicaPropia').val('').attr('disabled','disabled');" +
                                    "}else {" +
                                        "$('.cboSeDedica').val('').attr('disabled','disabled');" +
                                        "$('.cboDedicaPropia').val('').attr('disabled','disabled');" +
                                    "}" +
                                "});" +
                                //cambio region
                                "$('#" + _postJSON.P_form + " .cboUbicaReg').on('change', function() {" +
                                    "var cboreg = $(this).val(); " +
                                     "$('.filtro_select_comuna_13').empty().html(muestraComunasSegunRegion(cboreg, 13, 'cboUbicaCom', 'PER_LAB13'));" +
                                 "});" +
                               "$('#" + _postJSON.P_form + " .cboUbicaTrab').on('change', function() {" +
                                     "var cbLugar = $(this).val(); " +
                                     "if(cbLugar == 2) {" +
                                         "$('.cboUbicaCom').val('').attr('disabled','disabled');" +
                                         "$('.cboUbicaReg').val('').attr('disabled','disabled');" +
                                         "$('.divcboUbicaCom').hide();" +
                                         "$('.cboUbicaPais').val('').attr('disabled','disabled').selectpicker('refresh');" +
                                         "$('.divcboUbicaPais').hide();" +
                                         "$('.cboTransporte').val('').removeAttr('disabled');" +
                                         "$('.cboTransporteEspecifique').val('').attr('disabled','disabled');" +
                                         "$('.divcboTransporteEspecifique').hide();" +
                                     "}else if(cbLugar == 3) {" +
                                         "$('.cboUbicaCom').val('').removeAttr('disabled');" +
                                         "$('.filtro_select_comuna_13').empty().html(muestraComunasSegunRegion('', 13, 'cboUbicaCom', 'PER_LAB13'));" +
                                         "$('.cboUbicaReg').val('').removeAttr('disabled');" +
                                         "$('.divcboUbicaCom').show();" +
                                         "$('.cboUbicaPais').val('').attr('disabled','disabled').selectpicker('refresh');" +
                                         "$('.divcboUbicaPais').hide();" +
                                         "$('.cboTransporte').val('').removeAttr('disabled');" +
                                         "$('.cboTransporteEspecifique').val('').attr('disabled','disabled');" +
                                         "$('.divcboTransporteEspecifique').hide();" +
                                     "}else if(cbLugar == 4) {" +
                                         "$('.cboUbicaPais').val('').removeAttr('disabled').selectpicker('refresh');" +
                                         "$('.divcboUbicaPais').show();" +
                                         "$('.cboUbicaCom').val('').attr('disabled','disabled');" +
                                         "$('.cboUbicaReg').val('').attr('disabled','disabled');" +
                                         "$('.divcboUbicaCom').hide();" +
                                         "$('.cboTransporte').val('').removeAttr('disabled');" +
                                         "$('.cboTransporteEspecifique').val('').attr('disabled','disabled');" +
                                         "$('.divcboTransporteEspecifique').hide();" +
                                     "}else if(cbLugar == 1) {" +
                                         "$('.cboUbicaCom').val('').attr('disabled','disabled');" +
                                         "$('.cboUbicaReg').val('').attr('disabled','disabled');" +
                                         "$('.divcboUbicaCom').hide();" +
                                         "$('.cboUbicaPais').val('').attr('disabled','disabled').selectpicker('refresh');" +
                                         "$('.divcboUbicaPais').hide();" +
                                         "$('.cboTransporte').val('').attr('disabled','disabled');" +
                                         "$('.cboTransporteEspecifique').val('').attr('disabled','disabled');" +
                                         "$('.divcboTransporteEspecifique').hide();" +
                                     "}else{" +
                                         "$('.cboUbicaCom').val('').attr('disabled','disabled');" +
                                         "$('.cboUbicaReg').val('').attr('disabled','disabled');" +
                                         "$('.divcboUbicaCom').hide();" +
                                         "$('.cboUbicaPais').val('').attr('disabled','disabled').selectpicker('refresh');" +
                                         "$('.divcboUbicaPais').hide();" +
                                         "$('.cboTransporte').val('').attr('disabled','disabled');" +
                                         "$('.cboTransporteEspecifique').val('').attr('disabled','disabled');" +
                                         "$('.divcboTransporteEspecifique').hide();" +
                                     "}" +
                                 "});" +
                                 "$('#" + _postJSON.P_form + " .cboTransporte').on('change', function() {" +
                                     "var cbTranspor = $(this).val(); " +
                                     "if(cbTranspor == 11) {" +
                                        "$('.cboTransporteEspecifique').val('').removeAttr('disabled');" +
                                        "$('.divcboTransporteEspecifique').show();" +
                                     "}else {" +
                                        "$('.cboTransporteEspecifique').val('').attr('disabled','disabled');" +
                                        "$('.divcboTransporteEspecifique').hide();" +
                                     "}" +
                                 "});" +
                //fin funciones seleccionables
                // funciones de load en campos
                "setTimeout(function () { " +
                    "$('#val1').val(" + dato_PER_LAB1 + "); " +
                    "$('#val2').val(" + dato_PER_LAB2 + "); " +
                    "$('#val3').val(" + dato_PER_LAB3 + "); " +
                    "$('#val4').val(" + dato_PER_LAB4 + "); " +
                    "$('#val5').val(" + dato_PER_LAB5 + "); " +
                    "$('#val9').val(" + dato_PER_LAB9 + "); " +
                    "$('#val12').val(" + dato_PER_LAB12 + "); " +
                    "$('#val15').val(" + dato_PER_LAB15 + "); " +
                    "$('#valUltCuatro').val(" + dato_PER_LAB3 + "); " +
                    "$('.filtro_select_comuna_13').empty().html(muestraComunasSegunRegion('" + dato_PER_LAB17 + "', 13, 'cboUbicaCom', 'PER_LAB13'));" +

                    "if ($('#val1').val() == '1') { " +
                        "$('.cboOficio').removeAttr('disabled');" +
                        "$('.cboOficioTareas').removeAttr('disabled');" +
                        "$('.cboTrabajaComo').removeAttr('disabled');" +
                        "$('.cboUbicaTrab').removeAttr('disabled');" +
                    "}else if($('#val1').val() == '0') { " +
                        "$('.cboIndAnterior').removeAttr('disabled');" +
                    "}" +
                    "if ($('#val2').val() == '4') { " +
                        "$('.cboUltimascuatro').removeAttr('disabled');" +
                        "$('.form-group-radio-cboUltimascuatro').css('background-color', ''); " +
                    "}else if(($('#val2').val() >= 1) && ($('#val2').val() <= 3)) { " +
                        "$('.cboOficio').removeAttr('disabled');" +
                        "$('.cboOficioTareas').removeAttr('disabled');" +
                        "$('.cboTrabajaComo').removeAttr('disabled');" +
                        "$('.cboUbicaTrab').removeAttr('disabled');" +
                    "}" +
                    "if ($('#val3').val() == '1' || $('#val3').val() == '0') { " +
                        "$('.cboEstDisponible').removeAttr('disabled');" +
                        "$('.form-group-radio-cboEstDisponible').css('background-color', ''); " +
                    "}" +
                    
                    "if($('#val4').val() == '1') {" +
                        "if($('#valUltCuatro').val() == 1) {" +
                            "$('.cboSituActual').attr('disabled','disabled');" +
                            "$('.cboSituActualEspecifique').attr('disabled','disabled');" +
                            "$('.divcboSituActualEspecifique').hide();" +
                            "$('.cboOficio').attr('disabled','disabled');" +
                            "$('.cboOficioTareas').attr('disabled','disabled');" +
                            "$('.cboTrabajaComo').attr('disabled','disabled');" +
                            "$('.cboSeDedica').attr('disabled','disabled');" +
                            "$('.cboDedicaPropia').attr('disabled','disabled');" +
                            "$('.cboUbicaTrab').attr('disabled','disabled');" +
                            "$('.cboUbicaCom').attr('disabled','disabled');" +
                            "$('.cboUbicaReg').attr('disabled','disabled');" +
                            "$('.divcboUbicaCom').hide();" +
                            "$('.cboUbicaPais').attr('disabled','disabled').selectpicker('refresh');" +
                            "$('.divcboUbicaPais').hide();" +
                            "$('.cboTransporte').attr('disabled','disabled');" +
                            "$('.cboTransporteEspecifique').attr('disabled','disabled');" +
                            "$('.divcboTransporteEspecifique').hide();" +
                        "}else {" +
                            "$('.cboSituActual').removeAttr('disabled');" +
                        "}" +
                    "}else if($('#val4').val() == '0'){" +
                        "$('.cboSituActual').removeAttr('disabled');" +
                    "}" +

                    "if ($('#val5').val() == '7') { " +
                        "$('.cboSituActualEspecifique').removeAttr('disabled');" +
                        "$('.divcboSituActualEspecifique').show();" +
                    "}" +
                    "if(($('#val9').val() == '1') || ($('#val9').val() == '2') || ($('#val9').val() == '4')) { " +
                        "$('.cboDedicaPropia').removeAttr('disabled');" +
                    "}else if(($('#val9').val() == '3') || ($('#val9').val() == '5')) { " +
                        "$('.cboSeDedica').removeAttr('disabled');" +
                    "}" +
                    "if ($('#val12').val() == '2') { " +
                        "$('.cboTransporte').removeAttr('disabled');" +
                    "}else if($('#val12').val() == '3') { " +
                        "$('.cboUbicaCom').removeAttr('disabled');" +
                        "$('.filtro_select_comuna_13').empty().html(muestraComunasSegunRegion('" + dato_PER_LAB17 + "', 13, 'cboUbicaCom', 'PER_LAB13'));" +
                        "setTimeout(function () { $('#PER_LAB13').val('" + dato_PER_LAB13 + "').change();}, 1500);" +
                        "$('.cboUbicaReg').removeAttr('disabled');" +
                        "$('.divcboUbicaCom').show();" +
                        "$('.cboTransporte').removeAttr('disabled');" +
                    "}else if($('#val12').val() == '4') { " +
                        "$('.cboUbicaPais').removeAttr('disabled').selectpicker('refresh');" +
                        "$('.divcboUbicaPais').show();" +
                        "$('.cboTransporte').removeAttr('disabled');" +
                    "}" +
                    "if ($('#val15').val() == '11') { " +
                        "$('.cboTransporteEspecifique').removeAttr('disabled');" +
                        "$('.divcboTransporteEspecifique').show();" +
                    "}" +
                    
                "}, 500);"
            };

            // Genero funcion para Carga comunas()
            GetJSON _getJSONListaComunasSegunRegion = new GetJSON();
            {
                _getJSONListaComunasSegunRegion.G_url_servicio = _appSettings.ServidorWeb + "api/persona/muestra-comuna-segun-region";
                _getJSONListaComunasSegunRegion.G_parametros = "{ num: num, codigo_id: codigo_id, clase_control: clase_control, id_campo: id_campo}";
                _getJSONListaComunasSegunRegion.G_respuesta_servicio = "if(codigo_id == 13){" +
                                                                            "$('#filtro_select_com_13').html(respuesta[0].elemento_html);" +
                                                                       "}";
            }
            CallMethod _methodCallMuestraListaComunasSegunRegion = new CallMethod
            {
                Mc_nombre = "muestraComunasSegunRegion(num, codigo_id, clase_control, id_campo)",
                Mc_contenido = _getJSONListaComunasSegunRegion.GetJSONCall()
            };

            sb.Append(_methodCallMuestraListaComunasSegunRegion.CreaJQueryFunction());

            return sb.ToString() + _methodCallLoad.CreaJQueryDocumentReady();
        }

        /// <summary>
        /// Obtiene formulario Sección Personas DIVERSIDAD DE GÉNERO Y SEXUAL 
        /// </summary>
        public string ObtieneFormularioSeccionPersonaPaso8(string token, int paso, string cuestionario = "")
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Persona
            PersonaBOL _personaBOL = new PersonaBOL();
            PersonaDAL _personaDAL = new PersonaDAL();

            _personaBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            _personaBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;
            _personaBOL.PK_PERSONA = _identificadorCuestionario.IdPersona;
            List<PersonaBOL> listaPersona = _personaDAL.Listar<PersonaBOL>(_personaBOL);
            if (listaPersona.Count > 0)
            {
                _personaBOL = listaPersona[0];
            }

            string NombrePersona = _personaBOL.PER_NOMBRE;
            string NPER = _personaBOL.NPER;
            string EdadPersona = _personaBOL.PER3.ToString();
            string SexoPersona = _personaBOL.PER2.ToString();

            string dato_PER_DIV2 = _personaBOL.PER_DIV2.ToString();
            string dato_PER_DIV5 = _personaBOL.PER_DIV5.ToString();

            // Carga opciones de respuesta
            GesFormPreguntasOpcionesBOL _gesFormPreguntasOpcionesBOL = new GesFormPreguntasOpcionesBOL();
            GesFormPreguntasOpcionesDAL _gesFormPreguntasOpcionesDAL = new GesFormPreguntasOpcionesDAL();
            List<GesFormPreguntasOpcionesBOL> listaOpcionesPregunta = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesPreguntaPorGrupos<GesFormPreguntasOpcionesBOL>("'153','154','157'");

            // Obtengo opciones de respuesta
            StringBuilder sbPER_DIV1 = new StringBuilder();
            StringBuilder sbPER_DIV2 = new StringBuilder();
            StringBuilder sbPER_DIV5 = new StringBuilder();

            foreach (var item in listaOpcionesPregunta)
            {
                switch (item.Pk_form_preguntas)
                {
                    case 153:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_DIV1.ToString())
                        {
                            sbPER_DIV1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_DIV1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 154:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_DIV2.ToString())
                        {
                            sbPER_DIV2.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_DIV2.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case 157:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_DIV5.ToString())
                        {
                            sbPER_DIV5.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_DIV5.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                }
            }

            // Submit del formulario
            _postJSON.P_form = "formulario-persona-otros";
            _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
            _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/persona/ingresar-datos-diversidad";
            _postJSON.P_data_dinamica = true;
            _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { " +
                                                 "if(" + EdadPersona + " > 14 && " + SexoPersona + " == '2'){" +
                                                    "obtieneCuestionarioWeb(" + (paso + 1) + ",'" + token + "'); " +
                                                 "}else{" +
                                                    "obtieneCuestionarioWeb(15, '" + token + "'); " +
                                                 "}" +
                                             "}";

            // Identificación Hogar-Persona
            sb.Append(ObtieneIdentificacionPersonas(token, NombrePersona));

            // Inicio Definición del Formulario Persona. 
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
            sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
            sb.Append("<input id=\"NPER\" name=\"NPER\" type=\"hidden\" value=\"" + NPER + "\"/>");
            sb.Append("<input id=\"val2\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val5\" type=\"hidden\" value=\"\"/>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");

            // Inicio Linea 48 (Pregunta 77 y 77.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
            sb.Append("<p style=\"margin-bottom:-2px;\"><strong>DIVERSIDAD DE GÉNERO Y SEXUAL </strong></p>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<br>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<div class=\"alert alert-warning\">");
            sb.Append("<h4 class=\"text-center\"><p>A continuación, se presentan preguntas relacionadas con la caracterización de la diversidad de género y sexual de la población. Toda la información que nos entregue es confidencial y está resguardada por el Secreto Estadístico. </p></h4>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>76.- CUANDO NACIÓ " + NombrePersona + ", ¿CUÁL FUE SU SEXO? </strong></p>");
            sb.Append("<select id=\"PER_DIV1\" name=\"PER_DIV1\" class=\"form-control\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_DIV1.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 49 (Pregunta 78 y 78.1)

            // Inicio Linea 50 (Pregunta 79)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>77.- ¿CON CUÁL GÉNERO SE IDENTIFICA " + NombrePersona + "? </strong></p>");
            sb.Append("<select id=\"PER_DIV2\" name=\"PER_DIV2\" class=\"form-control cboGenero\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_DIV2.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboGeneroEspecifique\">");
            sb.Append("<p><strong>77.1.- ESPECIFIQUE OTRO </strong></p>");
            sb.Append("<input id=\"PER_DIV3\" name=\"PER_DIV3\" type=\"text\" class=\"form-control bloqueo cboGeneroEspecifique\" onkeypress=\"SoloLetras()\" placeholder=\"Especifique\" value=\"" + _personaBOL.PER_DIV3 + "\"  />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 50 (Pregunta 79)

            // Inicio Linea 50 (Pregunta 79)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>78.- ¿LA INFORMACIÓN DE " + NombrePersona + " FUE ENTREGADA POR LA PROPIA PERSONA? </strong></p>");

            if (_personaBOL.PER_DIV4 == "1")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt79_1\" class=\"magic-radio\" type=\"radio\" name=\"PER_DIV4\" value=\"1\" checked=\"checked\" >");
                sb.Append("<label for=\"rbt_opt79_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt79_1\" class=\"magic-radio\" type=\"radio\" name=\"PER_DIV4\" value=\"1\" >");
                sb.Append("<label for=\"rbt_opt79_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
            }

            if (_personaBOL.PER_DIV4 == "0")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt79_2\" class=\"magic-radio\" type=\"radio\" name=\"PER_DIV4\" value=\"0\" checked=\"checked\" >");
                sb.Append("<label for=\"rbt_opt79_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_opt79_2\" class=\"magic-radio\" type=\"radio\" name=\"PER_DIV4\" value=\"0\" >");
                sb.Append("<label for=\"rbt_opt79_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
            }

            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 50 (Pregunta 79)
            if (cuestionario == "") { 
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<div class=\"alert alert-warning\">");
                sb.Append("<h4 class=\"text-center\"><p>Esta pregunta es <u><b>voluntaria</b></u>. Responda únicamente por sí mismo/a, si además de usted están presentes otras personas en el hogar al momento de contestar, permita que esta pregunta sea autoreportada por cada una de ellas.</p></h4>");
                sb.Append("</div>");
                sb.Append("</div>");
            }
            if (cuestionario == "cati")
            {
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<div class=\"alert alert-warning\">");
                sb.Append("<h4 class=\"text-center\"><p>La siguiente pregunta es <u><b>voluntaria</b></u>. y debe ser respondida únicamente por usted. Le voy a leer la pregunta dos veces, la segunda vez me dirá “Sí” cuando mencione el término que le identifica.</p></h4>");
                sb.Append("</div>");
                sb.Append("</div>");
            }
            // Inicio Linea 50 (Pregunta 79)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>79.- USTED ACTUALMENTE SE IDENTIFICA COMO </strong></p>");
            sb.Append("<select id=\"PER_DIV5\" name=\"PER_DIV5\" class=\"form-control cboGeneroActual\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_DIV5.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group divcboGeneroActualEspecifique\">");
            sb.Append("<p><strong>79.1.- ESPECIFIQUE CÓMO SE IDENTIFICA  </strong></p>");
            sb.Append("<input id=\"PER_DIV6\" name=\"PER_DIV6\" type=\"text\" class=\"form-control bloqueo cboGeneroActualEspecifique\" onkeypress=\"SoloLetras()\" placeholder=\"Especifique\" value=\"" + _personaBOL.PER_DIV6 + "\"  />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 50 (Pregunta 79)

            sb.Append("</div>");

            sb.Append("</div>");

            // Inicio Botones del Cuestionario
            sb.Append("<div class=\"row text-center\">");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<hr />");
            sb.Append("<div class=\"mensaje text-center\"></div>");
            sb.Append("</div>");
            if (int.Parse(EdadPersona) > 14)
            {
                sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
                sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso - 1) + ",'" + token + "');\"  class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
                sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso - 2) + ",'" + token + "');\"  class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
                sb.Append("</div>");
            }
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
                               "$('.selectpicker').selectpicker();" +
                               "$('.magic-radio').iCheck({" +
                                    "checkboxClass: 'icheckbox_square-green'," +
                                    "radioClass: 'iradio_square-green'," +
                                    "increaseArea: '20%'" +
                               "});" +
                               //divs ocultos
                               "$('#" + _postJSON.P_form + " .bloqueo').attr('disabled','disabled'); " +

                               "$('.divcboGeneroEspecifique').hide();" +
                               "$('.divcboGeneroActualEspecifique').hide();" +
                               ////fin divs ocultos
                               ////funciones de seleccionables
                               "$('#" + _postJSON.P_form + " .cboGenero').on('change', function() {" +
                                    "var cboGen = $(this).val(); " +
                                    "if(cboGen == 4) {" +
                                        "$('.cboGeneroEspecifique').val('').removeAttr('disabled');" +
                                        "$('.divcboGeneroEspecifique').show();" +
                                     "}else {" +
                                        "$('.cboGeneroEspecifique').val('').attr('disabled','disabled');" +
                                        "$('.divcboGeneroEspecifique').hide();" +
                                     "}" +
                                "});" +
                                "$('#" + _postJSON.P_form + " .cboGeneroActual').on('change', function() {" +
                                    "var cboGenAct = $(this).val(); " +
                                    "if(cboGenAct == 5) {" +
                                        "$('.cboGeneroActualEspecifique').val('').removeAttr('disabled');" +
                                        "$('.divcboGeneroActualEspecifique').show();" +
                                     "}else {" +
                                        "$('.cboGeneroActualEspecifique').val('').attr('disabled','disabled');" +
                                        "$('.divcboGeneroActualEspecifique').hide();" +
                                     "}" +
                                "});" +                                
                // funciones de load en campos
                "setTimeout(function () { " +
                    "$('#val2').val(" + dato_PER_DIV2 + "); " +
                    "$('#val5').val(" + dato_PER_DIV5 + "); " +

                    "if ($('#val2').val() == '4') { " +
                        "$('.cboGeneroEspecifique').removeAttr('disabled');" +
                        "$('.divcboGeneroEspecifique').show();" +
                    "}" +
                    "if ($('#val5').val() == '5') { " +
                        "$('.cboGeneroActualEspecifique').removeAttr('disabled');" +
                        "$('.divcboGeneroActualEspecifique').show();" +
                    "}" + //EdadPersona
                    "if (" + EdadPersona + " < 5) { " +
                        "$('.cboGenero').val('').attr('disabled','disabled');" +
                    "}" +
                    "if (" + EdadPersona + " < 15) { " +
                        "$('.cboGeneroActual').val('').attr('disabled','disabled');" +
                    "}" +
                "}, 500);"
            };

            return sb.ToString() + _methodCallLoad.CreaJQueryDocumentReady();
        }

        /// <summary>
        /// Obtiene formulario Sección Personas FECUNDIDAD
        /// </summary>
        public string ObtieneFormularioSeccionPersonaPaso9(string token, int paso, string cuestionario = "")
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Obtengo identificación del registro
            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
            _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

            // Obtengo información Persona
            PersonaBOL _personaBOL = new PersonaBOL();
            PersonaDAL _personaDAL = new PersonaDAL();

            _personaBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
            _personaBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;
            _personaBOL.PK_PERSONA = _identificadorCuestionario.IdPersona;
            List<PersonaBOL> listaPersona = _personaDAL.Listar<PersonaBOL>(_personaBOL);
            if (listaPersona.Count > 0)
            {
                _personaBOL = listaPersona[0];
            }

            string NombrePersona = _personaBOL.PER_NOMBRE;
            string NPER = _personaBOL.NPER;
            string dato_PER_FEC1 = _personaBOL.PER_FEC1.ToString();
            string dato_PER_FEC4 = _personaBOL.PER_FEC4.ToString();

            // Carga opciones de respuesta
            GesFormPreguntasOpcionesBOL _gesFormPreguntasOpcionesBOL = new GesFormPreguntasOpcionesBOL();
            GesFormPreguntasOpcionesDAL _gesFormPreguntasOpcionesDAL = new GesFormPreguntasOpcionesDAL();
            List<GesFormPreguntasOpcionesBOL> listaOpcionesPregunta = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesPreguntaPorGrupos<GesFormPreguntasOpcionesBOL>("'59'");

            // Obtengo opciones de respuesta
            StringBuilder sbPER_FEC8 = new StringBuilder();

            foreach (var item in listaOpcionesPregunta)
            {
                switch (item.Pk_form_preguntas)
                {                    
                    case 59:
                        if (item.Fpo_numero.ToString() == _personaBOL.PER_FEC8.ToString())
                        {
                            sbPER_FEC8.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbPER_FEC8.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                }
            }

            // Submit del formulario
            _postJSON.P_form = "formulario-persona-fecundidad";
            _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
            _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/persona/ingresar-datos-fecundidad";
            _postJSON.P_data_dinamica = true;
            _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioWeb(15,'" + token + "'); }";

            // Identificación Hogar-Persona
            sb.Append(ObtieneIdentificacionPersonas(token, NombrePersona));

            // Inicio Definición del Formulario Persona. 
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
            sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
            sb.Append("<input id=\"NPER\" name=\"NPER\" type=\"hidden\" value=\"" + NPER + "\"/>");
            sb.Append("<input id=\"val1\" type=\"hidden\" value=\"\"/>");
            sb.Append("<input id=\"val4\" type=\"hidden\" value=\"\"/>");
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");

            // Inicio Linea 44 (Pregunta 74 y 74.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
            sb.Append("<p style=\"margin-bottom:-2px;\"><strong>FECUNDIDAD</strong></p>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<br>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>80.- ¿CUÁNTAS HIJAS E HIJOS NACIDOS VIVOS HA TENIDO " + NombrePersona + " EN TOTAL?  </strong></p>");
            sb.Append("<input id=\"PER_FEC1\" name=\"PER_FEC1\" type=\"number\" class=\"form-control cboNacVivos\" min=\"0\" onKeyPress =\"if (this.value.length == 2) return false; return event.charCode >= 48 && event.charCode <= 57;\" placeholder=\"Especifique\" value=\"" + _personaBOL.PER_FEC1 + "\"  />");
            sb.Append("</div>");
            sb.Append("</div>");            

            sb.Append("</div>");
            // Fin Linea 44 (Pregunta 74 y 74.1)

            // Inicio Linea 45 (Pregunta 74.2 y 75)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>80.1.- NÚMERO DE HIJAS:</strong></p>");
            sb.Append("<input id=\"PER_FEC2\" name=\"PER_FEC2\" type=\"number\" class=\"form-control todoFec\" min=\"0\" onKeyPress=\"if (this.value.length == 2) return false; return event.charCode >= 48 && event.charCode <= 57;\" placeholder=\"Especifique\" value=\"" + _personaBOL.PER_FEC2 + "\"  />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>80.2.- NÚMERO DE HIJOS:</strong></p>");
            sb.Append("<input id=\"PER_FEC3\" name=\"PER_FEC3\" type=\"number\" class=\"form-control todoFec\" min=\"0\" onKeyPress=\"if (this.value.length == 2) return false; return event.charCode >= 48 && event.charCode <= 57;\" placeholder=\"Especifique\" value=\"" + _personaBOL.PER_FEC3 + "\"  />");
            sb.Append("</div>");
            sb.Append("</div>");            

            sb.Append("</div>");
            // Fin Linea 45 (Pregunta 74.2 y 75)

            // Inicio Linea 46 (Pregunta 75.1 y 75.2)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>81.- ¿CUÁNTAS HIJAS E HIJOS DE " + NombrePersona + " ESTÁN VIVOS ACTUALMENTE? </strong></p>");
            sb.Append("<input id=\"PER_FEC4\" name=\"PER_FEC4\" type=\"number\" class=\"form-control todoFec cbovivosActual\" min=\"0\" onKeyPress=\"if (this.value.length == 2) return false; return event.charCode >= 48 && event.charCode <= 57;\" placeholder=\"Especifique\" value=\"" + _personaBOL.PER_FEC4 + "\"  />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 46 (Pregunta 75.1 y 75.2)

            // Inicio Linea 46 (Pregunta 75.1 y 75.2)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>81.1.- NÚMERO DE HIJAS: </strong></p>");
            sb.Append("<input id=\"PER_FEC5\" name=\"PER_FEC5\" type=\"number\" class=\"form-control todoFec todoVivos\" min=\"0\" onKeyPress=\"if (this.value.length == 2) return false; return event.charCode >= 48 && event.charCode <= 57;\" placeholder=\"Especifique\" value=\"" + _personaBOL.PER_FEC5 + "\"  />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>81.2.- NÚMERO DE HIJOS: </strong></p>");
            sb.Append("<input id=\"PER_FEC6\" name=\"PER_FEC6\" type=\"number\" class=\"form-control todoFec todoVivos\" min=\"0\" onKeyPress=\"if (this.value.length == 2) return false; return event.charCode >= 48 && event.charCode <= 57;\" placeholder=\"Especifique\" value=\"" + _personaBOL.PER_FEC6 + "\"  />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 46 (Pregunta 75.1 y 75.2)

            // Inicio Linea 47 (Pregunta 76)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>82.- ¿EN QUÉ FECHA NACIÓ LA ÚLTIMA HIJA O HIJO NACIDO VIVO DE " + NombrePersona + "? DÍA DE NACIMIENTO </strong></p>");
            //sb.Append("<input id=\"PER_FEC7\" name=\"PER_FEC7\" type=\"date\" class=\"form-control\" min=\"1900-01-01\" max=\"2023-12-31\" placeholder=\"\" value=\"" + _personaBOL.PER_FEC7 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 47 (Pregunta 76)

            // Inicio Linea 3 (Pregunta 36 y 36.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>DÍA  </strong></p>");
            sb.Append("<input id=\"PER_FEC7\" name=\"PER_FEC7\" type=\"number\" class=\"form-control todoFec\" min=\"1\" max=\"31\" onKeyPress=\"if (this.value.length == 2) return false; return event.charCode >= 48 && event.charCode <= 57;\" placeholder=\"\" value=\"" + _personaBOL.PER_FEC7 + "\"  />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>MES  </strong></p>");
            //sb.Append("<input id=\"PER_FEC8\" name=\"PER_FEC8\" type=\"number\" class=\"form-control todoFec\" min=\"1\" max=\"12\" onKeyPress=\"if (this.value.length == 2) return false; return event.charCode >= 48 && event.charCode <= 57;\" placeholder=\"\" value=\"" + _personaBOL.PER_FEC8 + "\"  />");
            sb.Append("<select id=\"PER_FEC8\" name=\"PER_FEC8\" class=\"form-control todoFec\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbPER_FEC8.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>AÑO   </strong></p>");
            sb.Append("<input id=\"PER_FEC9\" name=\"PER_FEC9\" type=\"number\" class=\"form-control todoFec\" min=\"1889\" max=\"2023\" onKeyPress=\"if (this.value.length == 4) return false; return event.charCode >= 48 && event.charCode <= 57;\" placeholder=\"\" value=\"" + _personaBOL.PER_FEC9 + "\"  />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 3 (Pregunta 36 y 36.1)

            sb.Append("</div>");

            sb.Append("</div>");

            // Inicio Botones del Cuestionario
            sb.Append("<div class=\"row text-center\">");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<hr />");
            sb.Append("<div class=\"mensaje text-center\"></div>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
            sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioWeb(" + (paso - 1) + ",'" + token + "');\"  class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
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
                               "$('.selectpicker').selectpicker();" +
                               "$('.magic-radio').iCheck({" +
                                    "checkboxClass: 'icheckbox_square-green'," +
                                    "radioClass: 'iradio_square-green'," +
                                    "increaseArea: '20%'" +
                               "});" +
                               "$('#" + _postJSON.P_form + " .cboNacVivos').on('change', function() {" +
                                    "var cboNacViv = $(this).val(); " +
                                    "if(cboNacViv == '0') {" +
                                        "$('.todoFec').val('').attr('disabled','disabled');" +
                                     "}else {" +
                                        "$('.todoFec').val('').removeAttr('disabled');" +
                                     "}" +
                                "});" +
                                "$('#" + _postJSON.P_form + " .cbovivosActual').on('change', function() {" +
                                    "var cboVivAct = $(this).val(); " +
                                    "if(cboVivAct == '0') {" +
                                        "$('.todoVivos').val('').attr('disabled','disabled');" +
                                     "}else {" +
                                        "$('.todoVivos').val('').removeAttr('disabled');" +
                                     "}" +
                                "});" +
                // funciones de load en campos
                "setTimeout(function () { " +
                    "$('#val1').val(" + dato_PER_FEC1 + "); " +
                    "$('#val4').val(" + dato_PER_FEC4 + "); " +

                    "if ($('#val1').val() == '0') { " +
                        "$('.todoFec').val('').attr('disabled','disabled');" +
                    "}" +
                    "if ($('#val4').val() == '0') { " +
                        "$('.todoVivos').val('').attr('disabled','disabled');" +
                    "}" +
                "}, 500);"
            };

            return sb.ToString() + _methodCallLoad.CreaJQueryDocumentReady();
        }

    }
}

