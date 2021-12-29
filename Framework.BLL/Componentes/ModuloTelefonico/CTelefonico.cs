using Framework.BLL.Utilidades.Seguridad;
using Framework.BLL.Utilidades.Ajax;
using System;
using System.Collections.Generic;
using System.Text;
using Framework.BOL;
using Framework.DAL;
using Framework.BLL.Utilidades.Encriptacion;
using System.Data;

namespace Framework.BLL.Componentes.ModuloTelefonico
{
    public class CTelefonico
    {
        AppSettings _appSettings = new AppSettings();

        // Obtengo identificación del registro
        TelefonicoBOL _telefonicoBOL = new TelefonicoBOL();
        TelefonicoDAL _telefonicoDAL = new TelefonicoDAL();

        public string ObtieneIdentificacionInformante(string token, int paso)
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Carga opciones de respuesta
            GesFormPreguntasOpcionesBOL _gesFormPreguntasOpcionesBOL = new GesFormPreguntasOpcionesBOL();
            GesFormPreguntasOpcionesDAL _gesFormPreguntasOpcionesDAL = new GesFormPreguntasOpcionesDAL();
            List<GesFormPreguntasOpcionesBOL> listaOpcionesPregunta = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesPreguntaPorGrupos<GesFormPreguntasOpcionesBOL>("'999') and fpo_numero in ('15101','2101','5103','13106','13111','14108','14101','5101','13132') union all (select pk_form_preguntas = 999, gpf_codigo_pregunta = 'otra', fpo_numero = 10, fpo_glosa_primaria = 'OTRA COMUNA', fpo_orden = '9999'");

            List<GesFormPreguntasOpcionesBOL> listaOpcionesOtrasComunas = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesPreguntaPorGrupos<GesFormPreguntasOpcionesBOL>("'999') and fpo_numero not in ('15101','2101','5103','13106','13111','14108','14101','5101','13132'");

            // Obtengo opciones de respuesta
            StringBuilder sbcOMUNA = new StringBuilder();
            StringBuilder sbcOMUNA_OTRA = new StringBuilder();

            // Obtengo identificación del registro
            string _strToken = "";
            Encrypt _encrypt = new Encrypt();
            _strToken = _encrypt.DecryptString(token);
            string[] _strArrayToken = _strToken.Split(new[] { "&" }, StringSplitOptions.None);

            _telefonicoBOL.ID = _strArrayToken[1];
            _telefonicoBOL.IDUSUARIO = _strArrayToken[0];
            List<TelefonicoBOL> listaTelefonico = _telefonicoDAL.ListarDatosInformante<TelefonicoBOL>(_telefonicoBOL);

            if (listaTelefonico.Count > 0)
            {
                _telefonicoBOL = listaTelefonico[0];

                foreach (var item in listaOpcionesPregunta)
                {
                    if (item.Fpo_numero.ToString() == _telefonicoBOL.COMUNA.ToString())
                    {
                        sbcOMUNA.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                    }
                    else
                    {
                        sbcOMUNA.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                    }
                }

                foreach (var item in listaOpcionesOtrasComunas)
                {
                    if (item.Fpo_numero.ToString() == _telefonicoBOL.COMUNA_OTRA.ToString())
                    {
                        sbcOMUNA_OTRA.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                    }
                    else
                    {
                        sbcOMUNA_OTRA.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                    }
                }
            }
            else
            {

                _telefonicoBOL.FECHA = DateTime.Now.ToString();

                foreach (var item in listaOpcionesPregunta)
                {
                    sbcOMUNA.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                }

                foreach (var item in listaOpcionesOtrasComunas)
                {
                    sbcOMUNA_OTRA.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                }
            }

            // Submit del formulario
            _postJSON.P_form = "formulario-telefonico";
            _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
            _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/telefonico/ingresar-datos-informante";
            _postJSON.P_data_dinamica = true;
            _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioTelefonico(" + (paso + 1) + ",'" + token + "'); }";

            // Inicio Definición del Formulario Persona. 
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
            sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
            sb.Append("<input id=\"paso_formulario\" name=\"paso_formulario\" type=\"hidden\" value=\"" + paso + "\"/>");
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");

            sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
            sb.Append("<p style=\"margin-bottom:-2px;\">&nbsp;&nbsp;<strong>IDENTIFICACIÓN DATOS DEL USUARIO/INFORMANTE</strong></p>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<br>");
            sb.Append("</div>");

            // Inicio Linea 1 ()
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-4 col-md-4\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>FECHA</strong></p>");
            sb.Append("<input id=\"FECHA\" name=\"FECHA\" type=\"text\" class=\"form-control\" placeholder=\"FECHA\" value=\"" + _telefonicoBOL.FECHA + "\" disabled/>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-4 col-md-4\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>NOMBRE</strong></p>");
            sb.Append("<input id=\"NOMBRE\" name=\"NOMBRE\" type=\"text\" class=\"form-control\" placeholder=\"NOMBRE\" value=\"" + _telefonicoBOL.NOMBRE + "\" required/>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-4 col-md-4\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>APELLIDO</strong></p>");
            sb.Append("<input id=\"APELLIDO\" name=\"APELLIDO\" type=\"text\" class=\"form-control\" placeholder=\"APELLIDO\" value=\"" + _telefonicoBOL.APELLIDO + "\" required/>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-4 col-md-4\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>TELÉFONO</strong></p>");
            sb.Append("<input id=\"TELEFONO\" name=\"TELEFONO\" type=\"text\" class=\"form-control\" onKeyPress=\"if (this.value.length == 9) return false; return event.charCode >= 48 && event.charCode <= 57;\"  placeholder=\"TELÉFONO\" value=\"" + _telefonicoBOL.TELEFONO + "\" required/>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-4 col-md-4\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>CORREO ELECTRÓNICO</strong></p>");
            sb.Append("<input id=\"EMAIL\" name=\"EMAIL\" type=\"email\" class=\"form-control\" placeholder=\"CORREO ELECTRÓNICO\" value=\"" + _telefonicoBOL.EMAIL + "\" required/>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>COMUNA</strong></p>");
            sb.Append("<select id=\"COMUNA\" name=\"COMUNA\" class=\"form-control\" data-width=\"100%\" required>");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbcOMUNA.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 COMUNA_otro\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>ESPECIFICAR OTRA COMUNA</strong></p>");
            sb.Append("<select id=\"COMUNA_OTRA\" name=\"COMUNA_OTRA\" class=\"form-control\" data-width=\"100%\" required>");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbcOMUNA_OTRA.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 1 ()

            sb.Append("</div>");

            sb.Append("</div>");

            // Inicio Botones del Cuestionario
            sb.Append("<div class=\"row text-center\">");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<hr />");
            sb.Append("<div class=\"mensaje text-center\"></div>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
            sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioTelefonico(999);\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
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

        public string ObtieneCategoriaMotivoLLamada1(string token, int paso)
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Carga opciones de respuesta
            GesFormPreguntasOpcionesBOL _gesFormPreguntasOpcionesBOL = new GesFormPreguntasOpcionesBOL();
            GesFormPreguntasOpcionesDAL _gesFormPreguntasOpcionesDAL = new GesFormPreguntasOpcionesDAL();
            List<GesFormPreguntasOpcionesBOL> listaOpcionesPregunta = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesMotivoLlamada<GesFormPreguntasOpcionesBOL>();
            List<GesFormPreguntasOpcionesBOL> listaOpcionesCatPregunta = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesCategoriaMotivoLlamada<GesFormPreguntasOpcionesBOL>();
            List<GesFormPreguntasOpcionesBOL> listaOpcionesComuna;

            // Obtengo identificación del registro
            string _strToken = "";
            Encrypt _encrypt = new Encrypt();
            _strToken = _encrypt.DecryptString(token);
            string[] _strArrayToken = _strToken.Split(new[] { "&" }, StringSplitOptions.None);

            _telefonicoBOL.ID = _strArrayToken[1];
            _telefonicoBOL.IDUSUARIO = _strArrayToken[0];
            List<TelefonicoBOL> listaTelefonico = _telefonicoDAL.ListarDatosInformante<TelefonicoBOL>(_telefonicoBOL);

            if (listaTelefonico.Count > 0)
            {
                _telefonicoBOL = listaTelefonico[0];
            }

            // Obtengo el nombre la comuna             
            string _NombreComuna = "";

            if (_telefonicoBOL.COMUNA == "10")
            {
                listaOpcionesComuna = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesPreguntaPorGrupos<GesFormPreguntasOpcionesBOL>("'999') and fpo_numero in ('" + _telefonicoBOL.COMUNA_OTRA + "'");
            }
            else
            {
                listaOpcionesComuna = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesPreguntaPorGrupos<GesFormPreguntasOpcionesBOL>("'999') and fpo_numero in ('" + _telefonicoBOL.COMUNA + "'");
            }

            foreach (var item in listaOpcionesComuna)
            {
                _NombreComuna = item.Fpo_glosa_primaria.ToString();
            }

            // Obtengo opciones de respuesta
            StringBuilder sbMOT = new StringBuilder();
            StringBuilder sbAT = new StringBuilder();
            StringBuilder sbCATI = new StringBuilder();
            StringBuilder sbPI = new StringBuilder();

            foreach (var item in listaOpcionesPregunta)
            {
                switch (item.Gpf_codigo_pregunta)
                {
                    case "MOT":
                        if (item.Fpo_numero.ToString() == _telefonicoBOL.IDMOTIVOLLAMADA.ToString())
                        {
                            sbMOT.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbMOT.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                }
            }

            foreach (var item in listaOpcionesCatPregunta)
            {
                switch (item.Gpf_codigo_pregunta)
                {
                    case "CAT1":
                        if (item.Fpo_numero.ToString() == _telefonicoBOL.MOTIVO1.ToString())
                        {
                            sbAT.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbAT.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case "CAT2":
                        if (item.Fpo_numero.ToString() == _telefonicoBOL.MOTIVO2.ToString())
                        {
                            sbCATI.Append("<option value=\"" + item.Fpo_numero.ToString() + "\"selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbCATI.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                }
            }

            // Submit del formulario
            _postJSON.P_form = "formulario-telefonico";
            _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
            _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/telefonico/ingresar-motivo-llamada";
            _postJSON.P_data_dinamica = true;
            _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioTelefonico(" + (paso + 1) + ",'" + token + "'); }";

            // Inicio Definición del Formulario Persona. 
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
            sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
            sb.Append("<input id=\"paso_formulario\" name=\"paso_formulario\" type=\"hidden\" value=\"" + paso + "\"/>");
            sb.Append("<div class=\"row\">");
            sb.Append("<div class=\"col-lg-12\">");

            // Inicio informacion basica informante
            sb.Append("<div class=\"alert alert-success\">");
            sb.Append("<p>Nombre Informante: <strong>" + _telefonicoBOL.NOMBRE + " " + _telefonicoBOL.APELLIDO + "</strong></p>");
            sb.Append("<p>Telefono: <strong>" + _telefonicoBOL.TELEFONO + "</strong></p>");
            sb.Append("<p>Correo Electrónico: <strong>" + _telefonicoBOL.EMAIL + "</strong></p>");
            sb.Append("</div>");
            // Fin informacion basica informante

            sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
            sb.Append("<p style=\"margin-bottom:-2px;\">&nbsp;&nbsp;<strong>IDENTIFICACIÓN MOTIVO DE LA LLAMADA</strong></p>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<br>");
            sb.Append("</div>");

            // Inicio Linea 1 (Pregunta 1 y 1.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>¿CUÁL ES EL MOTIVO DE LA LLAMADA?</strong></p>");
            sb.Append("<select id=\"MOTIVO\" name=\"MOTIVO\" class=\"form-control\" data-width=\"100%\" required>");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbMOT.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 CAT_llamada1 \" >");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>1. ATENCIÓN GENERAL </strong></p>");
            sb.Append("<select id=\"MOTIVO1\" name=\"MOTIVO1\" class=\"form-control\" data-width=\"100%\" required>");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbAT.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 CAT_llamada2\" >");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>ENTREVISTA TELEFÓNICA (CATI) </strong></p>");
            sb.Append("<select id=\"MOTIVO2\" name=\"MOTIVO2\" class=\"form-control\" data-width=\"100%\" required>");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbCATI.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 1 ()

            sb.Append("</div>");

            sb.Append("</div>");

            // Inicio Botones del Cuestionario
            sb.Append("<div class=\"row text-center\">");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<hr />");
            sb.Append("<div class=\"mensaje text-center\"></div>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
            sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioTelefonico(" + (paso - 1) + ",'" + token + "');\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
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

        public string ObtieneCategoriaMotivoLLamada2(string token, int paso)
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Carga opciones de respuesta
            GesFormPreguntasOpcionesBOL _gesFormPreguntasOpcionesBOL = new GesFormPreguntasOpcionesBOL();
            GesFormPreguntasOpcionesDAL _gesFormPreguntasOpcionesDAL = new GesFormPreguntasOpcionesDAL();
            List<GesFormPreguntasOpcionesBOL> listaOpcionesPregunta = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesInformacionGeneral<GesFormPreguntasOpcionesBOL>();
            List<GesFormPreguntasOpcionesBOL> listaOpcionesComuna;

            // Obtengo identificación del registro
            string _strToken = "";
            Encrypt _encrypt = new Encrypt();
            _strToken = _encrypt.DecryptString(token);
            string[] _strArrayToken = _strToken.Split(new[] { "&" }, StringSplitOptions.None);

            _telefonicoBOL.ID = _strArrayToken[1];
            _telefonicoBOL.IDUSUARIO = _strArrayToken[0];
            List<TelefonicoBOL> listaTelefonico = _telefonicoDAL.ListarDatosInformante<TelefonicoBOL>(_telefonicoBOL);

            if (listaTelefonico.Count > 0)
            {
                _telefonicoBOL = listaTelefonico[0];

            }

            // Obtengo el nombre la comuna             
            string _NombreComuna = "";

            if (_telefonicoBOL.COMUNA == "10")
            {
                listaOpcionesComuna = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesPreguntaPorGrupos<GesFormPreguntasOpcionesBOL>("'999') and fpo_numero in ('" + _telefonicoBOL.COMUNA_OTRA + "'");
            }
            else
            {
                listaOpcionesComuna = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesPreguntaPorGrupos<GesFormPreguntasOpcionesBOL>("'999') and fpo_numero in ('" + _telefonicoBOL.COMUNA + "'");
            }

            foreach (var item in listaOpcionesComuna)
            {
                _NombreComuna = item.Fpo_glosa_primaria.ToString();
            }

            // Obtengo opciones de respuesta
            StringBuilder sbInfoGral = new StringBuilder();
            StringBuilder sbAutoCawi = new StringBuilder();

            int i = 1;
            int j = 1;
            foreach (var item in listaOpcionesPregunta)
            {
                switch (item.Gpf_codigo_pregunta)
                {
                    case "CAT1":
                        if (i == 1)
                        {
                            if (_telefonicoBOL.INFO_GEN1 == "1")
                            {
                                sbInfoGral.Append("<input id=\"INFOGRAL_" + i + "\"  name=\"INFOGRAL_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbInfoGral.Append("<input id=\"INFOGRAL_" + i + "\"  name=\"INFOGRAL_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        if (i == 2)
                        {
                            if (_telefonicoBOL.INFO_GEN2 == "1")
                            {
                                sbInfoGral.Append("<input id=\"INFOGRAL_" + i + "\"  name=\"INFOGRAL_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbInfoGral.Append("<input id=\"INFOGRAL_" + i + "\"  name=\"INFOGRAL_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        if (i == 3)
                        {
                            if (_telefonicoBOL.INFO_GEN3 == "1")
                            {
                                sbInfoGral.Append("<input id=\"INFOGRAL_" + i + "\"  name=\"INFOGRAL_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbInfoGral.Append("<input id=\"INFOGRAL_" + i + "\"  name=\"INFOGRAL_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        if (i == 4)
                        {
                            if (_telefonicoBOL.INFO_GEN4 == "1")
                            {
                                sbInfoGral.Append("<input id=\"INFOGRAL_" + i + "\"  name=\"INFOGRAL_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbInfoGral.Append("<input id=\"INFOGRAL_" + i + "\"  name=\"INFOGRAL_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        if (i == 5)
                        {
                            if (_telefonicoBOL.INFO_GEN5 == "1")
                            {
                                sbInfoGral.Append("<input id=\"INFOGRAL_" + i + "\"  name=\"INFOGRAL_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbInfoGral.Append("<input id=\"INFOGRAL_" + i + "\"  name=\"INFOGRAL_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        if (i == 6)
                        {
                            if (_telefonicoBOL.INFO_GEN6 == "1")
                            {
                                sbInfoGral.Append("<input id=\"INFOGRAL_" + i + "\"  name=\"INFOGRAL_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbInfoGral.Append("<input id=\"INFOGRAL_" + i + "\"  name=\"INFOGRAL_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        if (i == 7)
                        {
                            if (_telefonicoBOL.INFO_GEN7 == "1")
                            {
                                sbInfoGral.Append("<input id=\"INFOGRAL_" + i + "\"  name=\"INFOGRAL_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbInfoGral.Append("<input id=\"INFOGRAL_" + i + "\"  name=\"INFOGRAL_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        if (i == 8)
                        {
                            if (_telefonicoBOL.INFO_GEN8 == "1")
                            {
                                sbInfoGral.Append("<input id=\"INFOGRAL_" + i + "\"  name=\"INFOGRAL_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbInfoGral.Append("<input id=\"INFOGRAL_" + i + "\"  name=\"INFOGRAL_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        if (i == 9)
                        {
                            if (_telefonicoBOL.INFO_GEN9 == "1")
                            {
                                sbInfoGral.Append("<input id=\"INFOGRAL_" + i + "\"  name=\"INFOGRAL_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbInfoGral.Append("<input id=\"INFOGRAL_" + i + "\"  name=\"INFOGRAL_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        if (i == 10)
                        {
                            if (_telefonicoBOL.INFO_GEN10 == "1")
                            {
                                sbInfoGral.Append("<input id=\"INFOGRAL_" + i + "\"  name=\"INFOGRAL_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbInfoGral.Append("<input id=\"INFOGRAL_" + i + "\"  name=\"INFOGRAL_" + i + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        sbInfoGral.Append("<span class=\"text-inverse\"> " + item.Fpo_glosa_primaria + "</span><br />");
                        i++;
                        break;
                    case "CAT2":
                        if (j == 1)
                        {
                            if (_telefonicoBOL.AUTO_CAWI1 == "1")
                            {
                                sbAutoCawi.Append("<input id=\"AUTOCAWI_" + j + "\"  name=\"AUTOCAWI_" + j + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbAutoCawi.Append("<input id=\"AUTOCAWI_" + j + "\"  name=\"AUTOCAWI_" + j + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        if (j == 2)
                        {
                            if (_telefonicoBOL.AUTO_CAWI2 == "1")
                            {
                                sbAutoCawi.Append("<input id=\"AUTOCAWI_" + j + "\"  name=\"AUTOCAWI_" + j + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbAutoCawi.Append("<input id=\"AUTOCAWI_" + j + "\"  name=\"AUTOCAWI_" + j + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        if (j == 3)
                        {
                            if (_telefonicoBOL.AUTO_CAWI3 == "1")
                            {
                                sbAutoCawi.Append("<input id=\"AUTOCAWI_" + j + "\"  name=\"AUTOCAWI_" + j + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbAutoCawi.Append("<input id=\"AUTOCAWI_" + j + "\"  name=\"AUTOCAWI_" + j + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        if (j == 4)
                        {
                            if (_telefonicoBOL.AUTO_CAWI4 == "1")
                            {
                                sbAutoCawi.Append("<input id=\"AUTOCAWI_" + j + "\"  name=\"AUTOCAWI_" + j + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbAutoCawi.Append("<input id=\"AUTOCAWI_" + j + "\"  name=\"AUTOCAWI_" + j + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        if (j == 5)
                        {
                            if (_telefonicoBOL.AUTO_CAWI5 == "1")
                            {
                                sbAutoCawi.Append("<input id=\"AUTOCAWI_" + j + "\"  name=\"AUTOCAWI_" + j + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbAutoCawi.Append("<input id=\"AUTOCAWI_" + j + "\"  name=\"AUTOCAWI_" + j + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        if (j == 6)
                        {
                            if (_telefonicoBOL.AUTO_CAWI6 == "1")
                            {
                                sbAutoCawi.Append("<input id=\"AUTOCAWI_" + j + "\"  name=\"AUTOCAWI_" + j + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbAutoCawi.Append("<input id=\"AUTOCAWI_" + j + "\"  name=\"AUTOCAWI_" + j + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        if (j == 7)
                        {
                            if (_telefonicoBOL.AUTO_CAWI7 == "1")
                            {
                                sbAutoCawi.Append("<input id=\"AUTOCAWI_" + j + "\"  name=\"AUTOCAWI_" + j + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbAutoCawi.Append("<input id=\"AUTOCAWI_" + j + "\"  name=\"AUTOCAWI_" + j + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        if (j == 8)
                        {
                            if (_telefonicoBOL.AUTO_CAWI8 == "1")
                            {
                                sbAutoCawi.Append("<input id=\"AUTOCAWI_" + j + "\"  name=\"AUTOCAWI_" + j + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbAutoCawi.Append("<input id=\"AUTOCAWI_" + j + "\"  name=\"AUTOCAWI_" + j + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        if (j == 9)
                        {
                            if (_telefonicoBOL.AUTO_CAWI9 == "1")
                            {
                                sbAutoCawi.Append("<input id=\"AUTOCAWI_" + j + "\"  name=\"AUTOCAWI_" + j + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbAutoCawi.Append("<input id=\"AUTOCAWI_" + j + "\"  name=\"AUTOCAWI_" + j + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        if (j == 10)
                        {
                            if (_telefonicoBOL.AUTO_CAWI10 == "1")
                            {
                                sbAutoCawi.Append("<input id=\"AUTOCAWI_" + j + "\"  name=\"AUTOCAWI_" + j + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\" checked>");
                            }
                            else
                            {
                                sbAutoCawi.Append("<input id=\"AUTOCAWI_" + j + "\"  name=\"AUTOCAWI_" + j + "\" class=\"magic-radio\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            }
                        }
                        sbAutoCawi.Append("<span class=\"text-inverse\"> " + item.Fpo_glosa_primaria + "</span><br />");
                        j++;
                        break;
                }
            }

            foreach (var item in listaOpcionesComuna)
            {
                _NombreComuna = item.Fpo_glosa_primaria.ToString();
            }

            switch (_telefonicoBOL.IDMOTIVOLLAMADA)
            {
                case "1":
                    // Submit del formulario
                    _postJSON.P_form = "formulario-telefonico";
                    _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
                    _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/telefonico/ingresar-categoria-motivo-llamada";
                    _postJSON.P_data_dinamica = true;
                    _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioTelefonico(" + (paso + 1) + ",'" + token + "'); }";

                    // Inicio Definición del Formulario Persona. 
                    sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
                    sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
                    sb.Append("<input id=\"paso_formulario\" name=\"paso_formulario\" type=\"hidden\" value=\"" + paso + "\"/>");
                    sb.Append("<input id=\"IDMOTIVOLLAMADA\" name=\"IDMOTIVOLLAMADA\" type=\"hidden\" value=\"" + _telefonicoBOL.IDMOTIVOLLAMADA + "\" >");
                    sb.Append("<input id=\"MOTIVO1\" name=\"MOTIVO1\" type=\"hidden\" value=\"" + _telefonicoBOL.MOTIVO1 + "\" >");
                    sb.Append("<div class=\"row\">");
                    sb.Append("<div class=\"col-lg-12\">");

                    // Inicio informacion basica informante
                    sb.Append("<div class=\"alert alert-success\">");
                    sb.Append("<p>Nombre Informante: <strong>" + _telefonicoBOL.NOMBRE + " " + _telefonicoBOL.APELLIDO + "</strong></p>");
                    sb.Append("<p>Telefono: <strong>" + _telefonicoBOL.TELEFONO + "</strong></p>");
                    sb.Append("<p>Correo Electrónico: <strong>" + _telefonicoBOL.EMAIL + "</strong></p>");
                    sb.Append("</div>");
                    // Fin informacion basica informante

                    sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
                    sb.Append("<p style=\"margin-bottom:-2px;\">&nbsp;&nbsp;<strong>IDENTIFICACIÓN MOTIVO DE LA LLAMADA</strong></p>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<br>");
                    sb.Append("</div>");

                    // Inicio Linea 1 
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-12 col-md-12 INFOGRAL\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>INFORMACIÓN GENERAL SOBRE EL PROCESO CENSAL</strong></p>");
                    sb.Append(sbInfoGral.ToString());
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12 INFOGRAL_Otro\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>ESPECIFIQUE OTRO TIPO DE CONSULTA SOBRE EL PROCESO CENSAL</strong></p>");
                    sb.Append("<textarea id=\"OTRO_TIP_CONS\" name=\"OTRO_TIP_CONS\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OTRO\" value=\"" + _telefonicoBOL.OTRO_TIP_CONS + "\" />");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    // Fin Linea 1 ()

                    // Inicio Linea 2 
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-12 col-md-12 AUTOCAWI\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>AUTOEMPADRONAMIENTO CAWI</strong></p>");
                    sb.Append(sbAutoCawi.ToString());
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12 AUTOCAWI_Otro\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>ESPECIFIQUE OTRO TIPO DE CONSULTA CAWI</strong></p>");
                    sb.Append("<textarea id=\"OTRO_CON_CAWI\" name=\"OTRO_CON_CAWI\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OTRO\" value=\"" + _telefonicoBOL.OTRO_CON_CAWI + "\" />");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    // Fin Linea 2 

                    sb.Append("</div>");

                    sb.Append("</div>");

                    // Inicio Botones del Cuestionario
                    sb.Append("<div class=\"row text-center\">");
                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<hr />");
                    sb.Append("<div class=\"mensaje text-center\"></div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
                    sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioTelefonico(" + (paso - 1) + ",'" + token + "');\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
                    sb.Append("<button type =\"submit\" class=\"btn btn-success btn-md btn-block\"><i class=\"fa fa-floppy-o\"></i> Guardar y continuar</button>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // Fin Botones del Cuestionario

                    sb.Append("</form>");
                    // Fin Definición del Formulario
                    break;
                case "2":
                    sb.Append("<script type=\"text/javascript\">obtieneCuestionarioTelefonico(4,'" + token + "');</script>");
                    break;
                case "3":
                    sb.Append("<script type=\"text/javascript\">obtieneCuestionarioTelefonico(3,'" + token + "');</script>");
                    break;
            }

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

        public string ObtieneCierraGestion(string token, int paso)
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Obtengo identificación del registro
            TelefonicoDAL _telefonicoDAL = new TelefonicoDAL();
            TelefonicoBOL _telefonicoBOL = new TelefonicoBOL();

            // Carga opciones de respuesta
            GesFormPreguntasOpcionesBOL _gesFormPreguntasOpcionesBOL = new GesFormPreguntasOpcionesBOL();
            GesFormPreguntasOpcionesDAL _gesFormPreguntasOpcionesDAL = new GesFormPreguntasOpcionesDAL();
            List<GesFormPreguntasOpcionesBOL> listaOpcionesPregunta = _gesFormPreguntasOpcionesDAL.ObtieneResultadoConsulta<GesFormPreguntasOpcionesBOL>();
            //List<GesFormPreguntasOpcionesBOL> listaOpcionesComuna;

            // Obtengo identificación del registro
            string _strToken = "";
            Encrypt _encrypt = new Encrypt();
            _strToken = _encrypt.DecryptString(token);
            string[] _strArrayToken = _strToken.Split(new[] { "&" }, StringSplitOptions.None);

            _telefonicoBOL.ID = _strArrayToken[1];
            _telefonicoBOL.IDUSUARIO = _strArrayToken[0];
            List<TelefonicoBOL> listaTelefonico = _telefonicoDAL.ListarDatosInformante<TelefonicoBOL>(_telefonicoBOL);

            if (listaTelefonico.Count > 0)
            {
                _telefonicoBOL = listaTelefonico[0];
            }

            // Obtengo opciones de respuesta
            StringBuilder sbCiGes = new StringBuilder();

            foreach (var item in listaOpcionesPregunta)
            {
                switch (item.Gpf_codigo_pregunta)
                {
                    case "RES":
                        if (item.Fpo_numero.ToString() == _telefonicoBOL.RESOL_CONS.ToString())
                        {
                            sbCiGes.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbCiGes.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                }
            }

            // Submit del formulario
            _postJSON.P_form = "formulario-telefonico";
            _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
            _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/telefonico/ingresar-cierre-gestion";
            _postJSON.P_data_dinamica = true;
            _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioTelefonico(999); }";

            // Inicio Definición del Formulario Persona. 
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
            sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
            sb.Append("<input id=\"paso_formulario\" name=\"paso_formulario\" type=\"hidden\" value=\"" + paso + "\"/>");
            sb.Append("<input id=\"IDMOTIVOLLAMADA\" name=\"IDMOTIVOLLAMADA\" type=\"hidden\" value=\"" + _telefonicoBOL.IDMOTIVOLLAMADA + "\" >");
            sb.Append("<input id=\"MOTIVO1\" name=\"MOTIVO1\" type=\"hidden\" value=\"" + _telefonicoBOL.MOTIVO1 + "\" >");
            sb.Append("<div class=\"row\">");
            sb.Append("<div class=\"col-lg-12\">");

            // Inicio informacion basica informante
            sb.Append("<div class=\"alert alert-success\">");
            sb.Append("<p>Nombre Informante: <strong>" + _telefonicoBOL.NOMBRE + " " + _telefonicoBOL.APELLIDO + "</strong></p>");
            sb.Append("<p>Telefono: <strong>" + _telefonicoBOL.TELEFONO + "</strong></p>");
            sb.Append("<p>Correo Electrónico: <strong>" + _telefonicoBOL.EMAIL + "</strong></p>");
            sb.Append("</div>");
            // Fin informacion basica informante

            sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
            sb.Append("<p style=\"margin-bottom:-2px;\">&nbsp;&nbsp;<strong>CIERRE DE GESTIÓN</strong></p>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<br>");
            sb.Append("</div>");

            // Inicio Linea 1 (Pregunta 1 y 1.1)
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 col-md-12 \">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>¿SE RESOLVIÓ LA CONSULTA DURANTE EL LLAMADO?</strong></p>");
            sb.Append("<select id=\"RESOL_CONS\" name=\"RESOL_CONS\" class=\"form-control\" data-width=\"100%\" required>");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbCiGes.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 RESOL_Otro\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>ESPECIFIQUE OTRO</strong></p>");
            sb.Append("<textarea id=\"OTRA_RESOL\" name=\"OTRA_RESOL\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OTRO...\" value=\"" + _telefonicoBOL.OTRA_RESOL + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 \">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>OBSERVACIONES GENERALES DE LA LLAMADA</strong></p>");
            sb.Append("<textarea id=\"OBS_CONSUL\" name=\"OBS_CONSUL\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OBSERVACIONES...\" value=\"" + _telefonicoBOL.OBS_CONSUL + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 1 ()

            sb.Append("</div>");
            sb.Append("</div>");

            string _strVolver = "";
            switch (_telefonicoBOL.IDMOTIVOLLAMADA)
            {
                case "1":
                    _strVolver = "<button type =\"button\" onclick=\"obtieneCuestionarioTelefonico(" + (paso - 1) + ",'" + token + "');\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>";
                    break;
                case "2":
                    _strVolver = "<button type =\"button\" onclick=\"obtieneCuestionarioTelefonico(4,'" + token + "');\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>";
                    break;
                case "3":
                    _strVolver = "<button type =\"button\" onclick=\"obtieneCuestionarioTelefonico(1,'" + token + "');\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>";
                    break;
            }

            // Inicio Botones del Cuestionario
            sb.Append("<div class=\"row text-center\">");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<hr />");
            sb.Append("<div class=\"mensaje text-center\"></div>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
            sb.Append(_strVolver);
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
            sb.Append("<button type =\"submit\" class=\"btn btn-success btn-md btn-block\"><i class=\"fa fa-floppy-o\"></i> Cierre Gestión </button>");
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

        public string ObtieneValidacionCodigo(string token, int paso)
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Obtengo identificación del registro
            TelefonicoDAL _telefonicoDAL = new TelefonicoDAL();
            TelefonicoBOL _telefonicoBOL = new TelefonicoBOL();

            // Carga opciones de respuesta
            GesFormPreguntasOpcionesBOL _gesFormPreguntasOpcionesBOL = new GesFormPreguntasOpcionesBOL();
            GesFormPreguntasOpcionesDAL _gesFormPreguntasOpcionesDAL = new GesFormPreguntasOpcionesDAL();
            List<GesFormPreguntasOpcionesBOL> listaOpcionesComuna;

            // Obtengo identificación del registro
            string _strToken = "";
            Encrypt _encrypt = new Encrypt();
            _strToken = _encrypt.DecryptString(token);
            string[] _strArrayToken = _strToken.Split(new[] { "&" }, StringSplitOptions.None);

            _telefonicoBOL.ID = _strArrayToken[1];
            _telefonicoBOL.IDUSUARIO = _strArrayToken[0];
            List<TelefonicoBOL> listaTelefonico = _telefonicoDAL.ListarDatosInformante<TelefonicoBOL>(_telefonicoBOL);

            if (listaTelefonico.Count > 0)
            {
                _telefonicoBOL = listaTelefonico[0];

            }

            // Obtengo el nombre la comuna             
            string _NombreComuna = "";

            if (_telefonicoBOL.COMUNA == "10")
            {
                listaOpcionesComuna = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesPreguntaPorGrupos<GesFormPreguntasOpcionesBOL>("'999') and fpo_numero in ('" + _telefonicoBOL.COMUNA_OTRA + "'");
            }
            else
            {
                listaOpcionesComuna = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesPreguntaPorGrupos<GesFormPreguntasOpcionesBOL>("'999') and fpo_numero in ('" + _telefonicoBOL.COMUNA + "'");
            }

            foreach (var item in listaOpcionesComuna)
            {
                _NombreComuna = item.Fpo_glosa_primaria.ToString();
            }

            string _strCheckValP1Si = "";
            string _strCheckValP1No = "";

            string _strCheckValP2Si = "";
            string _strCheckValP2No = "";

            string _strCheckValP3Si = "";
            string _strCheckValP3No = "";

            if (listaTelefonico.Count > 0)
            {
                _telefonicoBOL = listaTelefonico[0];
                if (_telefonicoBOL.VALP1 == "1") { _strCheckValP1Si = "checked=\"checked\""; }
                if (_telefonicoBOL.VALP1 == "0") { _strCheckValP1No = "checked=\"checked\""; }

                if (_telefonicoBOL.VALP2 == "1") { _strCheckValP2Si = "checked=\"checked\""; }
                if (_telefonicoBOL.VALP2 == "0") { _strCheckValP2No = "checked=\"checked\""; }

                if (_telefonicoBOL.VALP3 == "1") { _strCheckValP3Si = "checked=\"checked\""; }
                if (_telefonicoBOL.VALP3 == "0") { _strCheckValP3No = "checked=\"checked\""; }
            }

            // Submit del formulario
            _postJSON.P_form = "formulario-validacion";
            _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
            _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/telefonico/ingresar-validacion-codigo";
            _postJSON.P_data_dinamica = true;
            _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioTelefonico(" + (paso + 1) + ",'" + token + "'); }";

            // Inicio Definición del Formulario Persona. 
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
            sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
            sb.Append("<input id=\"paso_formulario\" name=\"paso_formulario\" type=\"hidden\" value=\"" + paso + "\"/>");
            sb.Append("<div class=\"row\">");
            sb.Append("<div class=\"col-lg-12\">");

            // Inicio informacion basica informante
            sb.Append("<div class=\"alert alert-success\">");
            sb.Append("<p>Nombre Informante: <strong>" + _telefonicoBOL.NOMBRE + " " + _telefonicoBOL.APELLIDO + "</strong></p>");
            sb.Append("<p>Telefono: <strong>" + _telefonicoBOL.TELEFONO + "</strong></p>");
            sb.Append("<p>Correo Electrónico: <strong>" + _telefonicoBOL.EMAIL + "</strong></p>");
            sb.Append("</div>");
            // Fin informacion basica informante

            sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
            sb.Append("<p style=\"margin-bottom:-2px;\">&nbsp;&nbsp;<strong>VALIDACIÓN PREVIA A CUESTIONARIO CENSAL</strong></p>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<br>");
            sb.Append("</div>");

            // Inicio Pregunta 1
            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>¿TIENE 18 O MÁS AÑOS DE EDAD?</strong></p>");

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<input id=\"rbt_optVALP1_1\" class=\"magic-radio\" type=\"radio\" name=\"VALP1\" value=\"1\" " + _strCheckValP1Si + " required> ");
            sb.Append("<label for=\"rbt_optVALP1_1\" style=\"display: inline;\">&nbsp;Si</label>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<input id=\"rbt_optVALP1_2\" class=\"magic-radio\" type=\"radio\" name=\"VALP1\" value=\"0\" " + _strCheckValP1No + " required> ");
            sb.Append("<label for=\"rbt_optVALP1_2\" style=\"display: inline;\">&nbsp;No</label>");
            sb.Append("</div>");

            sb.Append("</div>");
            sb.Append("</div>");
            // Fin pregunta 1

            // Inicio Pregunta 2
            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>¿FUE VISITADO POR UN CENSISTA?</strong></p>");

            sb.Append("<div class=\"clase_control_VALP2\">");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<input id=\"rbt_optVALP2_1\" class=\"magic-radio\" type=\"radio\" name=\"VALP2\" value=\"1\" " + _strCheckValP2Si + " required> ");
            sb.Append("<label for=\"rbt_optVALP2_1\" style=\"display: inline;\">&nbsp;Si</label>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<input id=\"rbt_optVALP2_2\" class=\"magic-radio\" type=\"radio\" name=\"VALP2\" value=\"0\" " + _strCheckValP2No + " required> ");
            sb.Append("<label for=\"rbt_optVALP2_2\" style=\"display: inline;\">&nbsp;No</label>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            sb.Append("</div>");
            // Fin pregunta 2

            // Inicio Pregunta 3
            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>¿RECIBIÓ LA CARTA INFORMATIVA CON EL CÓDIGO DE ACCESO?</strong></p>");

            sb.Append("<div class=\"clase_control_VALP3\">");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<input id=\"rbt_optVALP3_1\" class=\"magic-radio\" type=\"radio\" name=\"VALP3\" value=\"1\" " + _strCheckValP3Si + " required> ");
            sb.Append("<label for=\"rbt_optVALP3_1\" style=\"display: inline;\">&nbsp;Si</label>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<input id=\"rbt_optVALP3_2\" class=\"magic-radio\" type=\"radio\" name=\"VALP3\" value=\"0\" " + _strCheckValP3No + " required> ");
            sb.Append("<label for=\"rbt_optVALP3_2\" style=\"display: inline;\">&nbsp;No</label>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            sb.Append("</div>");
            // Fin pregunta 3

            // Incio Pregunta 4
            sb.Append("<div class=\"col-lg-12 col-md-12\">");
            sb.Append("<div class=\"alert alert-warning text-center\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>INGRESE NÚMERO DE FOLIO:</strong></p>");
            sb.Append("<input id=\"VALP4\" name=\"VALP4\" type=\"text\" class=\"form-control\" placeholder=\"NÚMERO DE FOLIO\" value=\"" + _telefonicoBOL.VALP4 + "\" required/>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            // Fin Pregunta 4

            sb.Append("</div>");
            sb.Append("</div>");

            // Inicio Botones del Cuestionario
            sb.Append("<div class=\"row text-center\">");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<hr />");
            sb.Append("<div class=\"mensaje text-center\"></div>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
            sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioTelefonico(1,'" + token + "');\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
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

        public string ObtieneValidacionDireccion(string token, int paso)
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Obtengo identificación del registro
            TelefonicoDAL _telefonicoDAL = new TelefonicoDAL();
            TelefonicoBOL _telefonicoBOL = new TelefonicoBOL();

            // Carga opciones de respuesta
            GesFormPreguntasOpcionesBOL _gesFormPreguntasOpcionesBOL = new GesFormPreguntasOpcionesBOL();
            GesFormPreguntasOpcionesDAL _gesFormPreguntasOpcionesDAL = new GesFormPreguntasOpcionesDAL();
            List<GesFormPreguntasOpcionesBOL> listaOpcionesComuna;

            // Obtengo identificación del registro
            string _strToken = "";
            Encrypt _encrypt = new Encrypt();
            _strToken = _encrypt.DecryptString(token);
            string[] _strArrayToken = _strToken.Split(new[] { "&" }, StringSplitOptions.None);

            _telefonicoBOL.ID = _strArrayToken[1];
            _telefonicoBOL.IDUSUARIO = _strArrayToken[0];
            List<TelefonicoBOL> listaTelefonico = _telefonicoDAL.ListarDatosInformante<TelefonicoBOL>(_telefonicoBOL);

            if (listaTelefonico.Count > 0)
            {
                _telefonicoBOL = listaTelefonico[0];

            }

            // Obtengo el nombre la comuna             
            string _NombreComuna = "";

            if (_telefonicoBOL.COMUNA == "10")
            {
                listaOpcionesComuna = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesPreguntaPorGrupos<GesFormPreguntasOpcionesBOL>("'999') and fpo_numero in ('" + _telefonicoBOL.COMUNA_OTRA + "'");
            }
            else
            {
                listaOpcionesComuna = _gesFormPreguntasOpcionesDAL.ObtieneOpcionesPreguntaPorGrupos<GesFormPreguntasOpcionesBOL>("'999') and fpo_numero in ('" + _telefonicoBOL.COMUNA + "'");
            }

            foreach (var item in listaOpcionesComuna)
            {
                _NombreComuna = item.Fpo_glosa_primaria.ToString();
            }

            string _strCheckValP5Si = "";
            string _strCheckValP5No = "";

            string _strCheckValP6Si = "";
            string _strCheckValP6No = "";

            string _strCheckValP7Si = "";
            string _strCheckValP7No = "";

            string _strCheckValP9Si = "";
            string _strCheckValP9No = "";

            string _strCheckValP10Si = "";
            string _strCheckValP10No = "";

            string _region = "";
            string _comuna = "";
            string _direccion = "";
            string _nDepto = "";
            bool _existeDireccion = false;

            if (listaTelefonico.Count > 0)
            {
                _telefonicoBOL = listaTelefonico[0];

                // Obtengo información de Dirección
                DataSet dsViviendaDireccion = new DataSet();
                ViviendaDAL _viviendaDAL = new ViviendaDAL();

                dsViviendaDireccion = _viviendaDAL.ListarDireccionTelefonico(_telefonicoBOL.VALP4);

                if (dsViviendaDireccion.Tables[0].Rows.Count > 0)
                {
                    _existeDireccion = true;
                    _region = dsViviendaDireccion.Tables[0].Rows[0]["Region"].ToString();
                    _comuna = dsViviendaDireccion.Tables[0].Rows[0]["Comuna"].ToString();
                    _direccion = dsViviendaDireccion.Tables[0].Rows[0]["DescripcionDirSec"].ToString();
                    _nDepto = "NULL";
                    if (_nDepto == "NULL")
                    {
                        _nDepto = "No aplica";
                    }
                    _telefonicoBOL.VALP5 = "1";
                    _telefonicoBOL.VALP6 = "1";
                }
                else
                {
                    _telefonicoBOL.VALP5 = "0";
                    _telefonicoBOL.VALP6 = "0";
                    _telefonicoBOL.VALP7 = "0";
                }

                if (_telefonicoBOL.VALP5 == "1") { _strCheckValP5Si = "checked=\"checked\""; }
                if (_telefonicoBOL.VALP5 == "0") { _strCheckValP5No = "checked=\"checked\""; }

                if (_telefonicoBOL.VALP6 == "1") { _strCheckValP6Si = "checked=\"checked\""; }
                if (_telefonicoBOL.VALP6 == "0") { _strCheckValP6No = "checked=\"checked\""; }

                if (_telefonicoBOL.VALP7 == "1") { _strCheckValP7Si = "checked=\"checked\""; }
                if (_telefonicoBOL.VALP7 == "0") { _strCheckValP7No = "checked=\"checked\""; }

                if (_telefonicoBOL.VALP9 == "1") { _strCheckValP9Si = "checked=\"checked\""; }
                if (_telefonicoBOL.VALP9 == "0") { _strCheckValP9No = "checked=\"checked\""; }

                if (_telefonicoBOL.VALP10 == "1") { _strCheckValP10Si = "checked=\"checked\""; }
                if (_telefonicoBOL.VALP10 == "0") { _strCheckValP10No = "checked=\"checked\""; }
            }

            if (_telefonicoBOL.VALP1 == "1" && _telefonicoBOL.VALP3 == "1")
            {
                // Submit del formulario
                _postJSON.P_form = "formulario-validacion";
                _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
                _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/telefonico/ingresar-validacion-direccion";
                _postJSON.P_data_dinamica = true;
                _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioTelefonico(" + (paso + 1) + ",'" + token + "'); }";

                // Inicio Definición del Formulario Persona. 
                sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
                sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
                sb.Append("<input id=\"paso_formulario\" name=\"paso_formulario\" type=\"hidden\" value=\"" + paso + "\"/>");
                sb.Append("<div class=\"row\">");
                sb.Append("<div class=\"col-lg-12\">");

                // Inicio informacion basica informante
                sb.Append("<div class=\"alert alert-success\">");
                sb.Append("<p>Nombre Informante: <strong>" + _telefonicoBOL.NOMBRE + " " + _telefonicoBOL.APELLIDO + "</strong></p>");
                sb.Append("<p>Telefono: <strong>" + _telefonicoBOL.TELEFONO + "</strong></p>");
                sb.Append("<p>Correo Electrónico: <strong>" + _telefonicoBOL.EMAIL + "</strong></p>");
                sb.Append("</div>");
                // Fin informacion basica informante

                sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
                sb.Append("<p style=\"margin-bottom:-2px;\">&nbsp;&nbsp;<strong>VALIDACIÓN PREVIA A CUESTIONARIO CENSAL</strong></p>");
                sb.Append("</div>");
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<br>");
                sb.Append("</div>");

                // Inicio Información Dirección
                if (_existeDireccion == true)
                {
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
                }
                else
                {
                    sb.Append("<div class=\"alert alert-danger text-center\">");
                    sb.Append("<h2>No existe información para este código de Vivienda</h2>");
                    sb.Append("</div>");
                }
                // Fin Información Dirección

                // Inicio Pregunta 1
                sb.Append("<div class=\"col-lg-12 col-md-12\">");
                sb.Append("<div class=\"form-group\">");
                sb.Append("<p><strong>¿EL CÓDIGO EXISTE?</strong></p>");

                sb.Append("<div class=\"clase_control_VALP5\">");
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_optVALP5_1\" class=\"magic-radio\" type=\"radio\" name=\"VALP5\" value=\"1\" " + _strCheckValP5Si + " required> ");
                sb.Append("<label for=\"rbt_optVALP5_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_optVALP5_2\" class=\"magic-radio\" type=\"radio\" name=\"VALP5\" value=\"0\" " + _strCheckValP5No + " required> ");
                sb.Append("<label for=\"rbt_optVALP5_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
                sb.Append("</div>");

                sb.Append("</div>");
                sb.Append("</div>");
                // Fin pregunta 1

                // Inicio Pregunta 2
                sb.Append("<div class=\"col-lg-12 col-md-12\">");
                sb.Append("<div class=\"form-group\">");
                sb.Append("<p><strong>¿EL CÓDIGO SE ENCUENTRA VINCULADO A UNA DIRECCIÓN?</strong></p>");

                sb.Append("<div class=\"clase_control_VALP6\">");
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_optVALP6_1\" class=\"magic-radio\" type=\"radio\" name=\"VALP6\" value=\"1\" " + _strCheckValP6Si + " required> ");
                sb.Append("<label for=\"rbt_optVALP6_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_optVALP6_2\" class=\"magic-radio\" type=\"radio\" name=\"VALP6\" value=\"0\" " + _strCheckValP6No + " required> ");
                sb.Append("<label for=\"rbt_optVALP6_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
                sb.Append("</div>");

                sb.Append("</div>");
                sb.Append("</div>");
                // Fin pregunta 2

                // Inicio Pregunta 3
                sb.Append("<div class=\"col-lg-12 col-md-12\">");
                sb.Append("<div class=\"form-group\">");
                sb.Append("<p><strong>¿CORRESPONDE LA DIRECCIÓN A SU VIVIENDA?</strong></p>");

                sb.Append("<div class=\"clase_control_VALP7\">");
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_optVALP7_1\" class=\"magic-radio\" type=\"radio\" name=\"VALP7\" value=\"1\" " + _strCheckValP7Si + " required> ");
                sb.Append("<label for=\"rbt_optVALP7_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_optVALP7_2\" class=\"magic-radio\" type=\"radio\" name=\"VALP7\" value=\"0\" " + _strCheckValP7No + " required> ");
                sb.Append("<label for=\"rbt_optVALP7_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
                sb.Append("</div>");

                sb.Append("</div>");
                sb.Append("</div>");
                // Fin pregunta 3

                // Incio Pregunta 4
                sb.Append("<div class=\"col-lg-12 col-md-12\">");
                sb.Append("<div class=\"form-group\">");
                sb.Append("<p><strong>INGRESE DIRECCIÓN DE LA VIVIENDA:</strong></p>");
                sb.Append("<input id=\"VALP8\" name=\"VALP8\" type=\"text\" class=\"form-control\" placeholder=\"DIRECCIÓN DE LA VIVIENDA\" value=\"" + _telefonicoBOL.VALP8 + "\" required disabled/>");
                sb.Append("</div>");
                sb.Append("</div>");
                // Fin Pregunta 4

                // Inicio Pregunta 5
                sb.Append("<div class=\"col-lg-12 col-md-12\">");
                sb.Append("<div class=\"form-group\">");
                sb.Append("<p><strong>¿HAY PERSONAS QUE RESIDAN HABITUALMENTE EN LA DIRECCIÓN?</strong></p>");

                sb.Append("<div class=\"clase_control_VALP9\">");
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_optVALP9_1\" class=\"magic-radio\" type=\"radio\" name=\"VALP9\" value=\"1\" " + _strCheckValP9Si + " required> ");
                sb.Append("<label for=\"rbt_optVALP9_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_optVALP9_2\" class=\"magic-radio\" type=\"radio\" name=\"VALP9\" value=\"0\" " + _strCheckValP9No + " required> ");
                sb.Append("<label for=\"rbt_optVALP9_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
                sb.Append("</div>");

                sb.Append("</div>");
                sb.Append("</div>");
                // Fin pregunta 5

                // Inicio Pregunta 6
                sb.Append("<div class=\"col-lg-12 col-md-12\">");
                sb.Append("<div class=\"form-group\">");
                sb.Append("<p><strong>¿USTED ES RESIDENTE HABITUAL DE LA VIVIENDA?</strong></p>");

                sb.Append("<div class=\"clase_control_VALP10\">");
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_optVALP10_1\" class=\"magic-radio\" type=\"radio\" name=\"VALP10\" value=\"1\" " + _strCheckValP10Si + " required> ");
                sb.Append("<label for=\"rbt_optVALP10_1\" style=\"display: inline;\">&nbsp;Si</label>");
                sb.Append("</div>");
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<input id=\"rbt_optVALP10_2\" class=\"magic-radio\" type=\"radio\" name=\"VALP10\" value=\"0\" " + _strCheckValP10No + " required> ");
                sb.Append("<label for=\"rbt_optVALP10_2\" style=\"display: inline;\">&nbsp;No</label>");
                sb.Append("</div>");
                sb.Append("</div>");

                sb.Append("</div>");
                sb.Append("</div>");
                // Fin pregunta 6

                sb.Append("</div>");
                sb.Append("</div>");

                // Inicio Botones del Cuestionario
                sb.Append("<div class=\"row text-center\">");
                sb.Append("<div class=\"col-lg-12\">");
                sb.Append("<hr />");
                sb.Append("<div class=\"mensaje text-center\"></div>");
                sb.Append("</div>");
                sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
                sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioTelefonico(" + (paso - 1) + ",'" + token + "');\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
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
                sb.Append("<script type=\"text/javascript\">obtieneCuestionarioTelefonico(3,'" + token + "');</script>");
            }

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

        public string ObtieneCuestionarioCATI(string token, int paso)
        {
            StringBuilder sb = new StringBuilder();

            // Obtengo identificación del registro
            TelefonicoDAL _telefonicoDAL = new TelefonicoDAL();
            TelefonicoBOL _telefonicoBOL = new TelefonicoBOL();

            // Obtengo identificación del registro
            string _strToken = "";
            Encrypt _encrypt = new Encrypt();
            _strToken = _encrypt.DecryptString(token);
            string[] _strArrayToken = _strToken.Split(new[] { "&" }, StringSplitOptions.None);

            _telefonicoBOL.ID = _strArrayToken[1];
            _telefonicoBOL.IDUSUARIO = _strArrayToken[0];
            List<TelefonicoBOL> listaTelefonico = _telefonicoDAL.ListarDatosInformante<TelefonicoBOL>(_telefonicoBOL);

            if (listaTelefonico.Count > 0)
            {
                _telefonicoBOL = listaTelefonico[0];
                if (_telefonicoBOL.VALP6 == "1" && _telefonicoBOL.VALP10 == "1")
                {
                    // Obtengo información de Dirección
                    DataSet dsViviendaDireccion = new DataSet();
                    ViviendaDAL _viviendaDAL = new ViviendaDAL();

                    dsViviendaDireccion = _viviendaDAL.ListarDireccionTelefonico(_telefonicoBOL.VALP4);

                    // Genero token de cuestionario
                    string _idFormulario = "";
                    _idFormulario = _encrypt.EncryptString(dsViviendaDireccion.Tables[0].Rows[0]["IdQRSuso"].ToString() + "&" + "0&0");

                    sb.Append("<script type=\"text/javascript\">obtieneCookieLlamadaWebCATI('" + _idFormulario + "','" + _telefonicoBOL.ID + "');</script>");
                }
                else
                {
                    sb.Append("<script type=\"text/javascript\">alert('No es posible aperturar cuestionario. NO EXISTE CÓDIGO VINCULADO A DIRECCIÓN O USTED NO ES RESIDENTE HABITUAL DE LA VIVIENDA. Debe cerrar gestión u obtener un código de vivienda correcto.'); obtieneCuestionarioTelefonico(3,'" + token + "');</script>");
                }
            }

            return sb.ToString();
        }

        public string HojaRutaCATI(string token, int paso)
        {
            StringBuilder sb = new StringBuilder();
            PostJSON _postJSON = new PostJSON();

            // Obtengo identificación del registro
            TelefonicoDAL _telefonicoDAL = new TelefonicoDAL();
            TelefonicoBOL _telefonicoBOL = new TelefonicoBOL();

            // Carga opciones de respuesta
            GesFormPreguntasOpcionesBOL _gesFormPreguntasOpcionesBOL = new GesFormPreguntasOpcionesBOL();
            GesFormPreguntasOpcionesDAL _gesFormPreguntasOpcionesDAL = new GesFormPreguntasOpcionesDAL();
            List<GesFormPreguntasOpcionesBOL> listaOpcionesPregunta = _gesFormPreguntasOpcionesDAL.ObtieneSiNo<GesFormPreguntasOpcionesBOL>();
            List<GesFormPreguntasOpcionesBOL> listaOpcionesPreguntas = _gesFormPreguntasOpcionesDAL.ObtienePreguntasCierreCuestionario<GesFormPreguntasOpcionesBOL>();

            // Obtengo identificación del registro
            string _strToken = "";
            Encrypt _encrypt = new Encrypt();
            _strToken = _encrypt.DecryptString(token);
            string[] _strArrayToken = _strToken.Split(new[] { "&" }, StringSplitOptions.None);

            _telefonicoBOL.ID = _strArrayToken[1];
            _telefonicoBOL.IDUSUARIO = _strArrayToken[0];
            List<TelefonicoBOL> listaTelefonico = _telefonicoDAL.ListarDatosInformante<TelefonicoBOL>(_telefonicoBOL);

            if (listaTelefonico.Count > 0)
            {
                _telefonicoBOL = listaTelefonico[0];
            }

            //if (_telefonicoBOL.CPC4 == "") { _telefonicoBOL.CPC4 = _telefonicoBOL.NOMBRE + " " + _telefonicoBOL.APELLIDO; }
            //if (_telefonicoBOL.CPC5 == "") { _telefonicoBOL.CPC4 = _telefonicoBOL.TELEFONO; }
            //if (_telefonicoBOL.CPC6 == "") { _telefonicoBOL.CPC4 = _telefonicoBOL.EMAIL; }
            //if (_telefonicoBOL.CPC15 == "") { _telefonicoBOL.CPC4 = _telefonicoBOL.NOMBRE + " " + _telefonicoBOL.APELLIDO; }
            //if (_telefonicoBOL.CPC16 == "") { _telefonicoBOL.CPC4 = _telefonicoBOL.TELEFONO; }
            //if (_telefonicoBOL.CPC17 == "") { _telefonicoBOL.CPC4 = _telefonicoBOL.EMAIL; }


            // Obtengo opciones de respuesta
            StringBuilder sbHR1 = new StringBuilder();
            StringBuilder sbHR2 = new StringBuilder();
            StringBuilder sbCPC1 = new StringBuilder();
            StringBuilder sbCPC3 = new StringBuilder();
            StringBuilder sbCPC10 = new StringBuilder();
            StringBuilder sbCPC12 = new StringBuilder();
            StringBuilder sbCPC14 = new StringBuilder();

            foreach (var item in listaOpcionesPregunta)
            {
                switch (item.Gpf_codigo_pregunta)
                {
                    case "SINO":
                        if (item.Fpo_numero.ToString() == _telefonicoBOL.HR1.ToString())
                        {
                            sbHR1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbHR1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        if (item.Fpo_numero.ToString() == _telefonicoBOL.HR2.ToString())
                        {
                            sbHR2.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbHR2.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        if (item.Fpo_numero.ToString() == _telefonicoBOL.CPC3.ToString())
                        {
                            sbCPC3.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbCPC3.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        if (item.Fpo_numero.ToString() == _telefonicoBOL.CPC14.ToString())
                        {
                            sbCPC14.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbCPC14.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                }
            }

            foreach (var item in listaOpcionesPreguntas)
            {
                switch (item.Gpf_codigo_pregunta)
                {
                    case "RAZ_ENT":
                        if (item.Fpo_numero.ToString() == _telefonicoBOL.CPC1.ToString())
                        {
                            sbCPC1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbCPC1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case "SIN_ENTR":
                        if (item.Fpo_numero.ToString() == _telefonicoBOL.CPC10.ToString())
                        {
                            sbCPC10.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbCPC10.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                    case "RAZ_REC":
                        if (item.Fpo_numero.ToString() == _telefonicoBOL.CPC12.ToString())
                        {
                            sbCPC12.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
                        }
                        else
                        {
                            sbCPC12.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
                        }
                        break;
                }
            }

            // Submit del formulario
            _postJSON.P_form = "formulario-telefonico";
            _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
            _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/telefonico/ingresar-hoja-ruta-cati";
            _postJSON.P_data_dinamica = true;
            _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioTelefonico(999); }";

            // Inicio Definición del Formulario. 
            sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
            sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
            sb.Append("<input id=\"paso_formulario\" name=\"paso_formulario\" type=\"hidden\" value=\"" + paso + "\"/>");
            sb.Append("<input id=\"Nombre_CPC4\" type=\"hidden\" value=\"" + _telefonicoBOL.NOMBRE + " " + _telefonicoBOL.APELLIDO + "\"/>");
            sb.Append("<input id=\"Telefono_CPC5\" type=\"hidden\" value=\"" + _telefonicoBOL.TELEFONO + "\"/>");
            sb.Append("<input id=\"Email_CPC6\" type=\"hidden\" value=\"" + _telefonicoBOL.EMAIL + "\"/>");
            sb.Append("<input id=\"Nombre_CPC15\" type=\"hidden\" value=\"" + _telefonicoBOL.NOMBRE + " " + _telefonicoBOL.APELLIDO + "\"/>");
            sb.Append("<input id=\"Telefono_CPC16\" type=\"hidden\" value=\"" + _telefonicoBOL.TELEFONO + "\"/>");
            sb.Append("<input id=\"Email_CPC17\" type=\"hidden\" value=\"" + _telefonicoBOL.EMAIL + "\"/>");
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12\">");

            // Inicio informacion basica informante
            sb.Append("<div class=\"alert alert-success\">");
            sb.Append("<p>Nombre Informante: <strong>" + _telefonicoBOL.NOMBRE + " " + _telefonicoBOL.APELLIDO + "</strong></p>");
            sb.Append("<p>Telefono: <strong>" + _telefonicoBOL.TELEFONO + "</strong></p>");
            sb.Append("<p>Correo Electrónico: <strong>" + _telefonicoBOL.EMAIL + "</strong></p>");
            sb.Append("</div>");
            // Fin informacion basica informante

            sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
            sb.Append("<p style=\"margin-bottom:-2px;\">&nbsp;&nbsp;<strong>RESULTADO DE LA ENTREVISTA (HdR CATI)</strong></p>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<br>");
            sb.Append("</div>");

            // Inicio Linea 1 
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-6 col-md-6\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>¿SE PUDO REALIZAR LA ENTREVISTA?</strong></p>");
            sb.Append("<select id=\"REA_ENT\" name=\"HR1\" class=\"form-control\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbHR1.ToString());
            sb.Append("</select>");
            sb.Append("</div>");    
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-6 col-md-6 COM_PRE_C\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>¿COMPLETÓ TODAS LAS PREGUNTAS DEL CUESTIONARIO?</strong></p>");
            sb.Append("<select id=\"COM_PRE\" name=\"HR2\" class=\"form-control\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbHR2.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 1 

            // Inicio Linea 1.1
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 RAZ_ENT_\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>RAZONES ENTREVISTA PARCIAL</strong></p>");
            sb.Append("<select id=\"RAZ_ENT\" name=\"CPC1\" class=\"form-control\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbCPC1.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 RAZENT_Otro\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>ESPECIFIQUE OTRA RAZÓN DE ENTREVISTA PARCIAL</strong></p>");
            sb.Append("<input id=\"OTRO_RAZ_ENT\" name=\"CPC2\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OTRO\" value=\"" + _telefonicoBOL.CPC2 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-4 col-md-4 CITA_TEL_\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>¿HA PODIDO CONCERTAR CITA?</strong></p>");
            sb.Append("<select id=\"CITA_TEL\" name=\"CPC3\" class=\"form-control\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbCPC3.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-4 col-md-4 NOMBRE_\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>NOMBRE DE LA PERSONA QUE SE CONTACTA</strong></p>");
            sb.Append("<input id=\"NOMBRE\" name=\"CPC4\" type=\"text\" class=\"form-control\" placeholder=\"NOMBRE\" value=\"" + _telefonicoBOL.CPC4 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-4 col-md-4 TELEFONO_\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>TELÉFONO DE CONTACTO DEL INFORMANTE</strong></p>");
            sb.Append("<input id=\"TELEFONO\" name=\"CPC5\" type=\"text\" class=\"form-control\" onKeyPress=\"if (this.value.length == 9) return false; return event.charCode >= 48 && event.charCode <= 57;\"  placeholder=\"TELÉFONO\" value=\"" + _telefonicoBOL.CPC5 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-4 col-md-4 EMAIL_\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>CORREO ELECTRÓNICO</strong></p>");
            sb.Append("<input id=\"EMAIL\" name=\"CPC6\" type=\"email\" class=\"form-control\" placeholder=\"CORREO ELECTRÓNICO\" value=\"" + _telefonicoBOL.CPC6 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-4 col-md-4 FECHA_\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>FECHA DE LA CITA</strong></p>");
            sb.Append("<input id=\"FECHA\" name=\"CPC7\" type=\"date\" class=\"form-control\" placeholder=\"FECHA\" value=\"" + _telefonicoBOL.CPC7 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-4 col-md-4 HORA_\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>HORA DE LA CITA</strong></p>");
            sb.Append("<input id=\"HORA\" name=\"CPC8\" type=\"text\" class=\"form-control\" placeholder=\"HORA\" value=\"" + _telefonicoBOL.CPC8 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 OBS_CITA_\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>OBSERVACIÓN DE LA CITA</strong></p>");
            sb.Append("<textarea id=\"OBS_CITA\" name=\"CPC9\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OBSERVACIÓN\" value=\"" + _telefonicoBOL.CPC9 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 1.1

            // Inicio Linea 2
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-12 SIN_ENTR_\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>UNIDADES CONTACTADAS PERO SIN ENTREVISTA</strong></p>");
            sb.Append("<select id=\"SIN_ENTR\" name=\"CPC10\" class=\"form-control\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbCPC10.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 SINENTR_Otro\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>ESPECIFIQUE OTRA RAZÓN DE NO ENTREVISTA  cpc11</strong></p>");
            sb.Append("<input id=\"OTRO_SIN_ENTR\" name=\"CPC11\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OTRO\" value=\"" + _telefonicoBOL.CPC11 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 RAZ_REC_\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>ANOTE LA RAZÓN DEL RECHAZO cpc12</strong></p>");
            sb.Append("<select id=\"RAZ_REC\" name=\"CPC12\" class=\"form-control\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbCPC12.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 RAZREC_Otro\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>ESPECIFIQUE OTRA RAZÓN DE RECHAZO cpc13</strong></p>");
            sb.Append("<input id=\"OTRO_RAZ_REC\" name=\"CPC13\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OTRO\" value=\"" + _telefonicoBOL.CPC13 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-4 col-md-4 CON_CITA_\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>¿HA PODIDO CONCERTAR CITA? cpc14</strong></p>");
            sb.Append("<select id=\"CON_CITA\" name=\"CPC14\" class=\"form-control\" data-width=\"100%\" >");
            sb.Append("<option value=\"\">Seleccione opción...</option>");
            sb.Append(sbCPC14.ToString());
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 2

           
            // Inicio Linea 3
            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-lg-4 col-md-4 NOM_CONT_\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>NOMBRE DE LA PERSONA QUE SE CONTACTA</strong></p>");
            sb.Append("<input id=\"NOM_CONT\" name=\"CPC15\" type=\"text\" class=\"form-control\" placeholder=\"NOMBRE\" value=\"" + _telefonicoBOL.CPC15 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-4 col-md-4 TEL_CONT_\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>TELÉFONO DE CONTACTO DEL INFORMANTE</strong></p>");
            sb.Append("<input id=\"TEL_CONT\" name=\"CPC16\" type=\"text\" class=\"form-control\" onKeyPress=\"if (this.value.length == 9) return false; return event.charCode >= 48 && event.charCode <= 57;\"  placeholder=\"TELÉFONO\" value=\"" + _telefonicoBOL.CPC16 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-4 col-md-4 EMAIL_CONT_\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>CORREO ELECTRÓNICO DEL INFORMANTE</strong></p>");
            sb.Append("<input id=\"EMAIL_CONT\" name=\"CPC17\" type=\"email\" class=\"form-control\" placeholder=\"CORREO ELECTRÓNICO\" value=\"" + _telefonicoBOL.CPC17 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-4 col-md-4 FECHA_CITA_\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>FECHA DE LA CITA</strong></p>");
            sb.Append("<input id=\"FECHA_CITA\" name=\"CPC18\" type=\"text\" class=\"form-control\" placeholder=\"FECHA\" value=\"" + _telefonicoBOL.CPC18 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-4 col-md-4 HORA_CITA_\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>HORA DE LA CITA</strong></p>");
            sb.Append("<input id=\"HORA_CITA\" name=\"CPC19\" type=\"text\" class=\"form-control\" placeholder=\"HORA\" value=\"" + _telefonicoBOL.CPC19 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"col-lg-12 OBSE_CITA_\">");
            sb.Append("<div class=\"form-group\">");
            sb.Append("<p><strong>OBSERVACIÓN DE LA CITA</strong></p>");
            sb.Append("<textarea id=\"OBSE_CITA\" name=\"CPC20\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OBSERVACIÓN\" value=\"" + _telefonicoBOL.CPC20 + "\" />");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            // Fin Linea 3

            sb.Append("</div>");

            sb.Append("</div>");

            // Inicio Botones del Cuestionario
            sb.Append("<div class=\"row text-center\">");
            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<hr />");
            sb.Append("<div class=\"mensaje text-center\"></div>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
            sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioTelefonico(5,'" + token + "');\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
            sb.Append("<button type =\"submit\" class=\"btn btn-success btn-md btn-block\"><i class=\"fa fa-floppy-o\"></i> Guardar </button>");
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

        //public string CierreProcesoCuestionario(string token, int paso)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    PostJSON _postJSON = new PostJSON();

        //    // Obtengo identificación del registro
        //    TelefonicoDAL _telefonicoDAL = new TelefonicoDAL();
        //    TelefonicoBOL _telefonicoBOL = new TelefonicoBOL();

        //    // Carga opciones de respuesta
        //    GesFormPreguntasOpcionesBOL _gesFormPreguntasOpcionesBOL = new GesFormPreguntasOpcionesBOL();
        //    GesFormPreguntasOpcionesDAL _gesFormPreguntasOpcionesDAL = new GesFormPreguntasOpcionesDAL();
        //    List<GesFormPreguntasOpcionesBOL> listaOpcionesPregunta = _gesFormPreguntasOpcionesDAL.ObtienePreguntasCierreCuestionario<GesFormPreguntasOpcionesBOL>();

        //    // Obtengo identificación del registro
        //    string _strToken = "";
        //    Encrypt _encrypt = new Encrypt();
        //    _strToken = _encrypt.DecryptString(token);
        //    string[] _strArrayToken = _strToken.Split(new[] { "&" }, StringSplitOptions.None);

        //    _telefonicoBOL.ID = _strArrayToken[1];
        //    _telefonicoBOL.IDUSUARIO = _strArrayToken[0];
        //    List<TelefonicoBOL> listaTelefonico = _telefonicoDAL.ListarDatosInformante<TelefonicoBOL>(_telefonicoBOL);

        //    // Obtengo opciones de respuesta
        //    StringBuilder sbCPC1 = new StringBuilder();
        //    StringBuilder sbCPC3 = new StringBuilder();
        //    StringBuilder sbCPC10 = new StringBuilder();
        //    StringBuilder sbCPC12 = new StringBuilder();
        //    StringBuilder sbCPC14 = new StringBuilder();

        //    if (listaTelefonico.Count > 0)
        //    {
        //        _telefonicoBOL = listaTelefonico[0];
        //    }

        //    // Solo si No completo el cuestionario
        //    if (_telefonicoBOL.HR2 == "1")
        //    {
        //        foreach (var item in listaOpcionesPregunta)
        //        {
        //            switch (item.Gpf_codigo_pregunta)
        //            {
        //                case "RAZ_ENT":
        //                    if (item.Fpo_numero.ToString() == _telefonicoBOL.CPC1.ToString())
        //                    {
        //                        sbCPC1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
        //                    }
        //                    else
        //                    {
        //                        sbCPC1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
        //                    }
        //                    break;
        //                case "SINO":
        //                    if (item.Fpo_numero.ToString() == _telefonicoBOL.CPC3.ToString())
        //                    {
        //                        sbCPC3.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
        //                    }
        //                    else
        //                    {
        //                        sbCPC3.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
        //                    }
        //                    if (item.Fpo_numero.ToString() == _telefonicoBOL.CPC14.ToString())
        //                    {
        //                        sbCPC14.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
        //                    }
        //                    else
        //                    {
        //                        sbCPC14.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
        //                    }
        //                    break;
        //                case "SIN_ENTR":
        //                    if (item.Fpo_numero.ToString() == _telefonicoBOL.CPC10.ToString())
        //                    {
        //                        sbCPC10.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
        //                    }
        //                    else
        //                    {
        //                        sbCPC10.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
        //                    }
        //                    break;
        //                case "RAZ_REC":
        //                    if (item.Fpo_numero.ToString() == _telefonicoBOL.CPC12.ToString())
        //                    {
        //                        sbCPC12.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
        //                    }
        //                    else
        //                    {
        //                        sbCPC12.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
        //                    }
        //                    break;
        //            }
        //        }

        //        // Submit del formulario
        //        _postJSON.P_form = "formulario-telefonico";
        //        _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
        //        _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/telefonico/ingresar-cierre_cuestionario-completos-cati";
        //        _postJSON.P_data_dinamica = true;
        //        _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioTelefonico(999); }";

        //        // Inicio Definición del Formulario. 
        //        sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
        //        sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
        //        sb.Append("<input id=\"paso_formulario\" name=\"paso_formulario\" type=\"hidden\" value=\"" + paso + "\"/>");
        //        sb.Append("<div class=\"row\">");

        //        sb.Append("<div class=\"col-lg-12\">");

        //        // Inicio informacion basica informante
        //        sb.Append("<div class=\"alert alert-success\">");
        //        sb.Append("<p>Nombre Informante: <strong>" + _telefonicoBOL.NOMBRE + " " + _telefonicoBOL.APELLIDO + "</strong></p>");
        //        sb.Append("<p>Telefono: <strong>" + _telefonicoBOL.TELEFONO + "</strong></p>");
        //        sb.Append("<p>Correo Electrónico: <strong>" + _telefonicoBOL.EMAIL + "</strong></p>");
        //        sb.Append("</div>");
        //        // Fin informacion basica informante

        //        sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
        //        sb.Append("<p style=\"margin-bottom:-2px;\">&nbsp;&nbsp;<strong>CIERRA EL PROCESO EN CUESTIONARIOS COMPLETOS</strong></p>");
        //        sb.Append("</div>");
        //        sb.Append("<div class=\"col-lg-12\">");
        //        sb.Append("<br>");
        //        sb.Append("</div>");

        //        // Inicio Linea 1 
        //        sb.Append("<div class=\"row\">");

        //        sb.Append("<div class=\"col-lg-12\">");
        //        sb.Append("<div class=\"form-group\">");
        //        sb.Append("<p><strong>RAZONES ENTREVISTA PARCIAL</strong></p>");
        //        sb.Append("<select id=\"RAZ_ENT\" name=\"CPC1\" class=\"form-control\" data-width=\"100%\" >");
        //        sb.Append("<option value=\"\">Seleccione opción...</option>");
        //        sb.Append(sbCPC1.ToString());
        //        sb.Append("</select>");
        //        sb.Append("</div>");
        //        sb.Append("</div>");

        //        sb.Append("<div class=\"col-lg-12 RAZENT_Otro\">");
        //        sb.Append("<div class=\"form-group\">");
        //        sb.Append("<p><strong>ESPECIFIQUE OTRA RAZÓN DE ENTREVISTA PARCIAL</strong></p>");
        //        sb.Append("<input id=\"OTRO_RAZ_ENT\" name=\"CPC2\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OTRO\" value=\"" + _telefonicoBOL.CPC2 + "\" />");
        //        sb.Append("</div>");
        //        sb.Append("</div>");

        //        sb.Append("<div class=\"col-lg-4 col-md-4\">");
        //        sb.Append("<div class=\"form-group\">");
        //        sb.Append("<p><strong>¿HA PODIDO CONCERTAR CITA?</strong></p>");
        //        sb.Append("<select id=\"CITA_TEL\" name=\"CPC3\" class=\"form-control\" data-width=\"100%\" >");
        //        sb.Append("<option value=\"\">Seleccione opción...</option>");
        //        sb.Append(sbCPC3.ToString());
        //        sb.Append("</select>");
        //        sb.Append("</div>");
        //        sb.Append("</div>");

        //        sb.Append("<div class=\"col-lg-4 col-md-4\">");
        //        sb.Append("<div class=\"form-group\">");
        //        sb.Append("<p><strong>NOMBRE DE LA PERSONA QUE SE CONTACTA</strong></p>");
        //        sb.Append("<input id=\"NOMBRE\" name=\"CPC4\" type=\"text\" class=\"form-control\" placeholder=\"NOMBRE\" value=\"" + _telefonicoBOL.CPC4 + "\" />");
        //        sb.Append("</div>");
        //        sb.Append("</div>");

        //        sb.Append("<div class=\"col-lg-4 col-md-4\">");
        //        sb.Append("<div class=\"form-group\">");
        //        sb.Append("<p><strong>TELÉFONO DE CONTACTO DEL INFORMANTE</strong></p>");
        //        sb.Append("<input id=\"TELEFONO\" name=\"CPC5\" type=\"text\" class=\"form-control\" onKeyPress=\"if (this.value.length == 9) return false; return event.charCode >= 48 && event.charCode <= 57;\"  placeholder=\"TELÉFONO\" value=\"" + _telefonicoBOL.CPC5 + "\" />");
        //        sb.Append("</div>");
        //        sb.Append("</div>");

        //        sb.Append("<div class=\"col-lg-4 col-md-4\">");
        //        sb.Append("<div class=\"form-group\">");
        //        sb.Append("<p><strong>CORREO ELECTRÓNICO</strong></p>");
        //        sb.Append("<input id=\"EMAIL\" name=\"CPC6\" type=\"email\" class=\"form-control\" placeholder=\"CORREO ELECTRÓNICO\" value=\"" + _telefonicoBOL.CPC6 + "\" />");
        //        sb.Append("</div>");
        //        sb.Append("</div>");

        //        sb.Append("<div class=\"col-lg-4 col-md-4\">");
        //        sb.Append("<div class=\"form-group\">");
        //        sb.Append("<p><strong>FECHA DE LA CITA</strong></p>");
        //        sb.Append("<input id=\"FECHA\" name=\"CPC7\" type=\"date\" class=\"form-control\" placeholder=\"FECHA\" value=\"" + _telefonicoBOL.CPC7 + "\" />");
        //        sb.Append("</div>");
        //        sb.Append("</div>");

        //        sb.Append("<div class=\"col-lg-4 col-md-4\">");
        //        sb.Append("<div class=\"form-group\">");
        //        sb.Append("<p><strong>HORA DE LA CITA</strong></p>");
        //        sb.Append("<input id=\"HORA\" name=\"CPC8\" type=\"text\" class=\"form-control\" placeholder=\"HORA\" value=\"" + _telefonicoBOL.CPC8 + "\" />");
        //        sb.Append("</div>");
        //        sb.Append("</div>");

        //        sb.Append("<div class=\"col-lg-12 RAZENT_Otro\">");
        //        sb.Append("<div class=\"form-group\">");
        //        sb.Append("<p><strong>OBSERVACIÓN DE LA CITA</strong></p>");
        //        sb.Append("<input id=\"OBS_CITA\" name=\"CPC9\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OBSERVACIOÓN\" value=\"" + _telefonicoBOL.CPC9 + "\" />");
        //        sb.Append("</div>");
        //        sb.Append("</div>");

        //        sb.Append("<div class=\"col-lg-12\">");
        //        sb.Append("<div class=\"form-group\">");
        //        sb.Append("<p><strong>UNIDADES CONTACTADAS PERO SIN ENTREVISTA</strong></p>");
        //        sb.Append("<select id=\"SIN_ENTR\" name=\"CPC10\" class=\"form-control\" data-width=\"100%\" >");
        //        sb.Append("<option value=\"\">Seleccione opción...</option>");
        //        sb.Append(sbCPC10.ToString());
        //        sb.Append("</select>");
        //        sb.Append("</div>");
        //        sb.Append("</div>");

        //        sb.Append("<div class=\"col-lg-12 SINENTR_Otro\">");
        //        sb.Append("<div class=\"form-group\">");
        //        sb.Append("<p><strong>ESPECIFIQUE OTRA RAZÓN DE RECHAZO</strong></p>");
        //        sb.Append("<input id=\"OTRO_SIN_ENTR\" name=\"CPC11\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OTRO\" value=\"" + _telefonicoBOL.CPC11 + "\" />");
        //        sb.Append("</div>");
        //        sb.Append("</div>");

        //        sb.Append("<div class=\"col-lg-12\">");
        //        sb.Append("<div class=\"form-group\">");
        //        sb.Append("<p><strong>ANOTE LA RAZÓN DEL RECHAZO</strong></p>");
        //        sb.Append("<select id=\"RAZ_REC\" name=\"CPC12\" class=\"form-control\" data-width=\"100%\" >");
        //        sb.Append("<option value=\"\">Seleccione opción...</option>");
        //        sb.Append(sbCPC12.ToString());
        //        sb.Append("</select>");
        //        sb.Append("</div>");
        //        sb.Append("</div>");

        //        sb.Append("<div class=\"col-lg-12 RAZREC_Otro\">");
        //        sb.Append("<div class=\"form-group\">");
        //        sb.Append("<p><strong>ESPECIFIQUE OTRA RAZÓN DE RECHAZO</strong></p>");
        //        sb.Append("<input id=\"OTRO_RAZ_REC\" name=\"CPC13\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OTRO\" value=\"" + _telefonicoBOL.CPC13 + "\" />");
        //        sb.Append("</div>");
        //        sb.Append("</div>");

        //        sb.Append("<div class=\"col-lg-4 col-md-4\">");
        //        sb.Append("<div class=\"form-group\">");
        //        sb.Append("<p><strong>¿HA PODIDO CONCERTAR CITA?</strong></p>");
        //        sb.Append("<select id=\"CON_CITA\" name=\"CPC14\" class=\"form-control\" data-width=\"100%\" >");
        //        sb.Append("<option value=\"\">Seleccione opción...</option>");
        //        sb.Append(sbCPC14.ToString());
        //        sb.Append("</select>");
        //        sb.Append("</div>");
        //        sb.Append("</div>");

        //        sb.Append("<div class=\"col-lg-4 col-md-4\">");
        //        sb.Append("<div class=\"form-group\">");
        //        sb.Append("<p><strong>NOMBRE DE LA PERSONA QUE SE CONTACTA</strong></p>");
        //        sb.Append("<input id=\"NOMBRE\" name=\"CPC15\" type=\"text\" class=\"form-control\" placeholder=\"NOMBRE\" value=\"" + _telefonicoBOL.CPC15 + "\" />");
        //        sb.Append("</div>");
        //        sb.Append("</div>");

        //        sb.Append("<div class=\"col-lg-4 col-md-4\">");
        //        sb.Append("<div class=\"form-group\">");
        //        sb.Append("<p><strong>TELÉFONO DE CONTACTO DEL INFORMANTE</strong></p>");
        //        sb.Append("<input id=\"TELEFONO\" name=\"CPC16\" type=\"text\" class=\"form-control\" onKeyPress=\"if (this.value.length == 9) return false; return event.charCode >= 48 && event.charCode <= 57;\"  placeholder=\"TELÉFONO\" value=\"" + _telefonicoBOL.CPC16 + "\" />");
        //        sb.Append("</div>");
        //        sb.Append("</div>");

        //        sb.Append("<div class=\"col-lg-4 col-md-4\">");
        //        sb.Append("<div class=\"form-group\">");
        //        sb.Append("<p><strong>CORREO ELECTRÓNICO DEL INFORMANTE</strong></p>");
        //        sb.Append("<input id=\"EMAIL\" name=\"CPC17\" type=\"email\" class=\"form-control\" placeholder=\"CORREO ELECTRÓNICO\" value=\"" + _telefonicoBOL.CPC17 + "\" />");
        //        sb.Append("</div>");
        //        sb.Append("</div>");

        //        sb.Append("<div class=\"col-lg-4 col-md-4\">");
        //        sb.Append("<div class=\"form-group\">");
        //        sb.Append("<p><strong>FECHA DE LA CITA</strong></p>");
        //        sb.Append("<input id=\"FECHA\" name=\"CPC18\" type=\"text\" class=\"form-control\" placeholder=\"FECHA\" value=\"" + _telefonicoBOL.CPC18 + "\" />");
        //        sb.Append("</div>");
        //        sb.Append("</div>");

        //        sb.Append("<div class=\"col-lg-4 col-md-4\">");
        //        sb.Append("<div class=\"form-group\">");
        //        sb.Append("<p><strong>HORA DE LA CITA</strong></p>");
        //        sb.Append("<input id=\"HORA\" name=\"CPC19\" type=\"text\" class=\"form-control\" placeholder=\"HORA\" value=\"" + _telefonicoBOL.CPC19 + "\" />");
        //        sb.Append("</div>");
        //        sb.Append("</div>");

        //        sb.Append("<div class=\"col-lg-12 RAZENT_Otro\">");
        //        sb.Append("<div class=\"form-group\">");
        //        sb.Append("<p><strong>OBSERVACIÓN DE LA CITA</strong></p>");
        //        sb.Append("<input id=\"OBS_CITA\" name=\"CPC20\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OBSERVACIOÓN\" value=\"" + _telefonicoBOL.CPC20 + "\" />");
        //        sb.Append("</div>");
        //        sb.Append("</div>");

        //        sb.Append("</div>");
        //        // Fin Linea 1 

        //        sb.Append("</div>");

        //        sb.Append("</div>");

        //        // Inicio Botones del Cuestionario
        //        sb.Append("<div class=\"row text-center\">");
        //        sb.Append("<div class=\"col-lg-12\">");
        //        sb.Append("<hr />");
        //        sb.Append("<div class=\"mensaje text-center\"></div>");
        //        sb.Append("</div>");
        //        sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
        //        sb.Append("<button type =\"button\" onclick=\"obtieneHojaRutaCATI(1,'" + token + "');\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
        //        sb.Append("</div>");
        //        sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
        //        sb.Append("<button type =\"submit\" class=\"btn btn-success btn-md btn-block\"><i class=\"fa fa-floppy-o\"></i> Guardar</button>");
        //        sb.Append("</div>");
        //        sb.Append("</div>");
        //        // Fin Botones del Cuestionario

        //        sb.Append("</form>");
        //        // Fin Definición del Formulario
        //    } else
        //    {
        //        sb.Append("<script type=\"text/javascript\">obtieneCuestionarioTelefonico(999);</script>");
        //    }

        //    // Genero load del formulario
        //    CallMethod _methodCallLoad = new CallMethod
        //    {
        //        Mc_contenido = _postJSON.PostJSONCall() +
        //                       "$('.selectpicker').selectpicker();" +
        //                       "$('.magic-radio').iCheck({" +
        //                            "checkboxClass: 'icheckbox_square-green'," +
        //                            "radioClass: 'iradio_square-green'," +
        //                            "increaseArea: '20%'" +
        //                       "});"
        //    };

        //    return sb.ToString() + _methodCallLoad.CreaJQueryDocumentReady();

        //}

        //public string ResultadoRecontacto(string token, int paso)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    PostJSON _postJSON = new PostJSON();

        //    // Obtengo identificación del registro
        //    TelefonicoDAL _telefonicoDAL = new TelefonicoDAL();
        //    TelefonicoBOL _telefonicoBOL = new TelefonicoBOL();

        //    // Carga opciones de respuesta
        //    GesFormPreguntasOpcionesBOL _gesFormPreguntasOpcionesBOL = new GesFormPreguntasOpcionesBOL();
        //    GesFormPreguntasOpcionesDAL _gesFormPreguntasOpcionesDAL = new GesFormPreguntasOpcionesDAL();
        //    List<GesFormPreguntasOpcionesBOL> listaOpcionesPregunta = _gesFormPreguntasOpcionesDAL.ObtienePreguntasCierreCuestionario<GesFormPreguntasOpcionesBOL>();

        //    // Obtengo identificación del registro
        //    string _strToken = "";
        //    Encrypt _encrypt = new Encrypt();
        //    _strToken = _encrypt.DecryptString(token);
        //    string[] _strArrayToken = _strToken.Split(new[] { "&" }, StringSplitOptions.None);

        //    _telefonicoBOL.ID = _strArrayToken[1];
        //    _telefonicoBOL.IDUSUARIO = _strArrayToken[0];
        //    List<TelefonicoBOL> listaTelefonico = _telefonicoDAL.ListarDatosInformante<TelefonicoBOL>(_telefonicoBOL);

        //    // Obtengo opciones de respuesta
        //    StringBuilder sbRRC1 = new StringBuilder();
        //    StringBuilder sbRRC2 = new StringBuilder();
        //    StringBuilder sbRRC3 = new StringBuilder();
        //    StringBuilder sbRRC4 = new StringBuilder();
        //    StringBuilder sbRRC6 = new StringBuilder();
        //    StringBuilder sbRRC8 = new StringBuilder();
        //    StringBuilder sbRRC10 = new StringBuilder();
        //    StringBuilder sbRRC17 = new StringBuilder();

        //    if (listaTelefonico.Count > 0)
        //    {
        //        _telefonicoBOL = listaTelefonico[0];
        //    }

        //    foreach (var item in listaOpcionesPregunta)
        //    {
        //        switch (item.Gpf_codigo_pregunta)
        //        {
        //            case "RAZ_CUES":
        //                if (item.Fpo_numero.ToString() == _telefonicoBOL.RRC4.ToString())
        //                {
        //                    sbRRC4.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
        //                }
        //                else
        //                {
        //                    sbRRC4.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
        //                }
        //                break;
        //            case "SINO":
        //                if (item.Fpo_numero.ToString() == _telefonicoBOL.RRC1.ToString())
        //                {
        //                    sbRRC1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
        //                }
        //                else
        //                {
        //                    sbRRC1.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
        //                }
        //                if (item.Fpo_numero.ToString() == _telefonicoBOL.RRC2.ToString())
        //                {
        //                    sbRRC2.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
        //                }
        //                else
        //                {
        //                    sbRRC2.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
        //                }
        //                if (item.Fpo_numero.ToString() == _telefonicoBOL.RRC3.ToString())
        //                {
        //                    sbRRC3.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
        //                }
        //                else
        //                {
        //                    sbRRC3.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
        //                }
        //                if (item.Fpo_numero.ToString() == _telefonicoBOL.RRC10.ToString())
        //                {
        //                    sbRRC10.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
        //                }
        //                else
        //                {
        //                    sbRRC10.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
        //                }
        //                break;
        //            case "SIN_ENTR":
        //                if (item.Fpo_numero.ToString() == _telefonicoBOL.RRC6.ToString())
        //                {
        //                    sbRRC6.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
        //                }
        //                else
        //                {
        //                    sbRRC6.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
        //                }
        //                break;
        //            case "RAZ_REC":
        //                if (item.Fpo_numero.ToString() == _telefonicoBOL.RRC8.ToString())
        //                {
        //                    sbRRC8.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
        //                }
        //                else
        //                {
        //                    sbRRC8.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
        //                }
        //                break;
        //            case "NO_CONT":
        //                if (item.Fpo_numero.ToString() == _telefonicoBOL.RRC17.ToString())
        //                {
        //                    sbRRC17.Append("<option value=\"" + item.Fpo_numero.ToString() + "\" selected>" + item.Fpo_glosa_primaria + "</option>");
        //                }
        //                else
        //                {
        //                    sbRRC17.Append("<option value=\"" + item.Fpo_numero.ToString() + "\">" + item.Fpo_glosa_primaria + "</option>");
        //                }
        //                break;
        //        }
        //    }

        //    // Submit del formulario
        //    _postJSON.P_form = "formulario-telefonico";
        //    _postJSON.P_load = "$('.contenedor-Framework').html('<div class=\"row\"><div class=\"col-lg-4\"></div><div class=\"col-lg-4 text-center\"><img src=\"" + _appSettings.ServidorWeb + "/Framework/assets/images/wait_progress.gif?=v1\" /></div></div>');";
        //    _postJSON.P_url_servicio = _appSettings.ServidorWeb + "api/telefonico/ingresar-resultado-recontacto-cati";
        //    _postJSON.P_data_dinamica = true;
        //    _postJSON.P_respuesta_servicio = "if (respuesta[0].elemento_html == 'ok') { obtieneCuestionarioTelefonico(999); }";

        //    // Inicio Definición del Formulario. 
        //    sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"m-t\" method=\"post\" disabled>");
        //    sb.Append("<input id=\"idFormulario\" name=\"idFormulario\" type=\"hidden\" value=\"" + token + "\"/>");
        //    sb.Append("<input id=\"paso_formulario\" name=\"paso_formulario\" type=\"hidden\" value=\"" + paso + "\"/>");
        //    sb.Append("<div class=\"row\">");

        //    sb.Append("<div class=\"col-lg-12\">");

        //    // Inicio informacion basica informante
        //    sb.Append("<div class=\"alert alert-success\">");
        //    sb.Append("<p>Nombre Informante: <strong>" + _telefonicoBOL.NOMBRE + " " + _telefonicoBOL.APELLIDO + "</strong></p>");
        //    sb.Append("<p>Telefono: <strong>" + _telefonicoBOL.TELEFONO + "</strong></p>");
        //    sb.Append("<p>Correo Electrónico: <strong>" + _telefonicoBOL.EMAIL + "</strong></p>");
        //    sb.Append("</div>");
        //    // Fin informacion basica informante

        //    sb.Append("<div class=\"p-xs bg-muted col-lg-12 text-center\">");
        //    sb.Append("<p style=\"margin-bottom:-2px;\">&nbsp;&nbsp;<strong>RESULTADO DE RECONTACTO</strong></p>");
        //    sb.Append("</div>");
        //    sb.Append("<div class=\"col-lg-12\">");
        //    sb.Append("<br>");
        //    sb.Append("</div>");

        //    // Inicio Linea 1 
        //    sb.Append("<div class=\"row\">");

        //    sb.Append("<div class=\"col-lg-6 col-md-6\">");
        //    sb.Append("<div class=\"form-group\">");
        //    sb.Append("<p><strong>¿LOGRO CONTACTARSE CON EL INFORMANTE?</strong></p>");
        //    sb.Append("<select id=\"CONT_INFO\" name=\"RRC1\" class=\"form-control\" data-width=\"100%\" >");
        //    sb.Append("<option value=\"\">Seleccione opción...</option>");
        //    sb.Append(sbRRC1.ToString());
        //    sb.Append("</select>");
        //    sb.Append("</div>");
        //    sb.Append("</div>");

        //    sb.Append("<div class=\"col-lg-6 col-md-6\">");
        //    sb.Append("<div class=\"form-group\">");
        //    sb.Append("<p><strong>¿SE PUDO REALIZAR LA ENTREVISTA?</strong></p>");
        //    sb.Append("<select id=\"REAL_ENT\" name=\"RRC2\" class=\"form-control\" data-width=\"100%\" >");
        //    sb.Append("<option value=\"\">Seleccione opción...</option>");
        //    sb.Append(sbRRC2.ToString());
        //    sb.Append("</select>");
        //    sb.Append("</div>");
        //    sb.Append("</div>");

        //    sb.Append("<div class=\"col-lg-6 col-md-6\">");
        //    sb.Append("<div class=\"form-group\">");
        //    sb.Append("<p><strong>¿COMPLETO TODAS LAS PREGUNTAS DEL CUESTIONARIO?</strong></p>");
        //    sb.Append("<select id=\"COM_CUES\" name=\"RRC3\" class=\"form-control\" data-width=\"100%\" >");
        //    sb.Append("<option value=\"\">Seleccione opción...</option>");
        //    sb.Append(sbRRC3.ToString());
        //    sb.Append("</select>");
        //    sb.Append("</div>");
        //    sb.Append("</div>");

        //    sb.Append("<div class=\"col-lg-12\">");
        //    sb.Append("<div class=\"form-group\">");
        //    sb.Append("<p><strong>RAZONES DE CUESTIONARIO PARCIAL (LLAMADA DESDE ATENCIÓN CIUDADANA/OPERADOR TELEFÓNICO)</strong></p>");
        //    sb.Append("<select id=\"RAZ_CUES\" name=\"RRC4\" class=\"form-control\" data-width=\"100%\" >");
        //    sb.Append("<option value=\"\">Seleccione opción...</option>");
        //    sb.Append(sbRRC4.ToString());
        //    sb.Append("</select>");
        //    sb.Append("</div>");
        //    sb.Append("</div>");

        //    sb.Append("<div class=\"col-lg-12 RAZCUES_Otro\">");
        //    sb.Append("<div class=\"form-group\">");
        //    sb.Append("<p><strong>ESPECIFIQUE OTRA RAZÓN DE CUESTIONARIO PARCIAL</strong></p>");
        //    sb.Append("<input id=\"OTRO_RAZ_CUES\" name=\"RRC5\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OTRO\" value=\"" + _telefonicoBOL.RRC5 + "\" />");
        //    sb.Append("</div>");
        //    sb.Append("</div>");

        //    sb.Append("<div class=\"col-lg-12\">");
        //    sb.Append("<div class=\"form-group\">");
        //    sb.Append("<p><strong>UNIDADES CONTACTADAS PERO SIN ENTREVISTA</strong></p>");
        //    sb.Append("<select id=\"SIN_ENTR\" name=\"RRC6\" class=\"form-control\" data-width=\"100%\" >");
        //    sb.Append("<option value=\"\">Seleccione opción...</option>");
        //    sb.Append(sbRRC6.ToString());
        //    sb.Append("</select>");
        //    sb.Append("</div>");
        //    sb.Append("</div>");

        //    sb.Append("<div class=\"col-lg-12 RAZNOENRT_Otro\">");
        //    sb.Append("<div class=\"form-group\">");
        //    sb.Append("<p><strong>ESPECIFIQUE OTRA RAZÓN DE NO ENTREVISTA</strong></p>");
        //    sb.Append("<input id=\"OTRO_NO_ENTR\" name=\"RRC7\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OTRO\" value=\"" + _telefonicoBOL.RRC7 + "\" />");
        //    sb.Append("</div>");
        //    sb.Append("</div>");

        //    sb.Append("<div class=\"col-lg-12\">");
        //    sb.Append("<div class=\"form-group\">");
        //    sb.Append("<p><strong>ANOTE LA RAZÓN DEL RECHAZO</strong></p>");
        //    sb.Append("<select id=\"RAZ_REC\" name=\"RRC8\" class=\"form-control\" data-width=\"100%\" >");
        //    sb.Append("<option value=\"\">Seleccione opción...</option>");
        //    sb.Append(sbRRC8.ToString());
        //    sb.Append("</select>");
        //    sb.Append("</div>");
        //    sb.Append("</div>");

        //    sb.Append("<div class=\"col-lg-12 RAZREC_Otro\">");
        //    sb.Append("<div class=\"form-group\">");
        //    sb.Append("<p><strong>ESPECIFIQUE OTRA RAZÓN DE RECHAZO</strong></p>");
        //    sb.Append("<input id=\"OTRO_RAZ_REC\" name=\"RRC9\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OTRO\" value=\"" + _telefonicoBOL.RRC9 + "\" />");
        //    sb.Append("</div>");
        //    sb.Append("</div>");

        //    sb.Append("<div class=\"col-lg-6 col-md-6\">");
        //    sb.Append("<div class=\"form-group\">");
        //    sb.Append("<p><strong>¿HA PODIDO CONCERTAR CITA?</strong></p>");
        //    sb.Append("<select id=\"CON_CITA\" name=\"RRC10\" class=\"form-control\" data-width=\"100%\" >");
        //    sb.Append("<option value=\"\">Seleccione opción...</option>");
        //    sb.Append(sbRRC10.ToString());
        //    sb.Append("</select>");
        //    sb.Append("</div>");
        //    sb.Append("</div>");

        //    sb.Append("<div class=\"col-lg-6 col-md-6\">");
        //    sb.Append("<div class=\"form-group\">");
        //    sb.Append("<p><strong>NOMBRE DE LA PERSONA QUE SE CONTACTA</strong></p>");
        //    sb.Append("<input id=\"NOMBRE\" name=\"RRC11\" type=\"text\" class=\"form-control\" placeholder=\"NOMBRE\" value=\"" + _telefonicoBOL.RRC11 + "\" />");
        //    sb.Append("</div>");
        //    sb.Append("</div>");

        //    sb.Append("<div class=\"col-lg-6 col-md-6\">");
        //    sb.Append("<div class=\"form-group\">");
        //    sb.Append("<p><strong>TELÉFONO DE CONTACTO DEL INFORMANTE</strong></p>");
        //    sb.Append("<input id=\"TELEFONO\" name=\"RRC12\" type=\"text\" class=\"form-control\" onKeyPress=\"if (this.value.length == 9) return false; return event.charCode >= 48 && event.charCode <= 57;\"  placeholder=\"TELÉFONO\" value=\"" + _telefonicoBOL.RRC12 + "\" />");
        //    sb.Append("</div>");
        //    sb.Append("</div>");

        //    sb.Append("<div class=\"col-lg-6 col-md-6\">");
        //    sb.Append("<div class=\"form-group\">");
        //    sb.Append("<p><strong>CORREO ELECTRÓNICO DEL INFORMANTE</strong></p>");
        //    sb.Append("<input id=\"EMAIL\" name=\"RRC13\" type=\"email\" class=\"form-control\" placeholder=\"CORREO ELECTRÓNICO\" value=\"" + _telefonicoBOL.RRC13 + "\" />");
        //    sb.Append("</div>");
        //    sb.Append("</div>");

        //    sb.Append("<div class=\"col-lg-6 col-md-6\">");
        //    sb.Append("<div class=\"form-group\">");
        //    sb.Append("<p><strong>FECHA DE LA CITA</strong></p>");
        //    sb.Append("<input id=\"FECHA\" name=\"RRC14\" type=\"text\" class=\"form-control\" placeholder=\"FECHA\" value=\"" + _telefonicoBOL.RRC14 + "\" />");
        //    sb.Append("</div>");
        //    sb.Append("</div>");

        //    sb.Append("<div class=\"col-lg-6 col-md-6\">");
        //    sb.Append("<div class=\"form-group\">");
        //    sb.Append("<p><strong>HORA DE LA CITA</strong></p>");
        //    sb.Append("<input id=\"HORA\" name=\"RRC15\" type=\"text\" class=\"form-control\" placeholder=\"HORA\" value=\"" + _telefonicoBOL.RRC15 + "\" />");
        //    sb.Append("</div>");
        //    sb.Append("</div>");

        //    sb.Append("<div class=\"col-lg-12 RAZENT_Otro\">");
        //    sb.Append("<div class=\"form-group\">");
        //    sb.Append("<p><strong>OBSERVACIÓN DE LA CITA</strong></p>");
        //    sb.Append("<input id=\"OBS_CITA\" name=\"RRC16\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OBSERVACIOÓN\" value=\"" + _telefonicoBOL.RRC16 + "\" />");
        //    sb.Append("</div>");
        //    sb.Append("</div>");

        //    sb.Append("<div class=\"col-lg-12\">");
        //    sb.Append("<div class=\"form-group\">");
        //    sb.Append("<p><strong>CÓDIGOS DE RESULTADO PARA UNIDADES NO CONTACTADAS Y SIN ENTREVISTA</strong></p>");
        //    sb.Append("<select id=\"NO_CONT\" name=\"RRC17\" class=\"form-control\" data-width=\"100%\" >");
        //    sb.Append("<option value=\"\">Seleccione opción...</option>");
        //    sb.Append(sbRRC17.ToString());
        //    sb.Append("</select>");
        //    sb.Append("</div>");
        //    sb.Append("</div>");

        //    sb.Append("<div class=\"col-lg-12 NOCONT_Otro\">");
        //    sb.Append("<div class=\"form-group\">");
        //    sb.Append("<p><strong>ESPECIFIQUE OTRA RAZÓN DE NO CONTACTO</strong></p>");
        //    sb.Append("<input id=\"OTRO_NO_CONT\" name=\"RRC18\" type=\"text\" class=\"form-control\" onkeypress=\"SoloLetras();\" placeholder=\"OTRO\" value=\"" + _telefonicoBOL.RRC18 + "\" />");
        //    sb.Append("</div>");
        //    sb.Append("</div>");

        //    sb.Append("</div>");
        //    // Fin Linea 1 

        //    sb.Append("</div>");

        //    sb.Append("</div>");

        //    // Inicio Botones del Cuestionario
        //    sb.Append("<div class=\"row text-center\">");
        //    sb.Append("<div class=\"col-lg-12\">");
        //    sb.Append("<hr />");
        //    sb.Append("<div class=\"mensaje text-center\"></div>");
        //    sb.Append("</div>");
        //    sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
        //    sb.Append("<button type =\"button\" onclick=\"obtieneCuestionarioTelefonico(999);\" class=\"btn btn-warning btn-md btn-block\"><i class=\"fa fa-chevron-left\"></i> Volver</button>");
        //    sb.Append("</div>");
        //    sb.Append("<div class=\"col-lg-6 col-md-12 col-sm-12 col-xs-12\">");
        //    sb.Append("<button type =\"submit\" class=\"btn btn-success btn-md btn-block\"><i class=\"fa fa-floppy-o\"></i> Guardar </button>");
        //    sb.Append("</div>");
        //    sb.Append("</div>");
        //    // Fin Botones del Cuestionario

        //    sb.Append("</form>");
        //    // Fin Definición del Formulario

        //    // Genero load del formulario
        //    CallMethod _methodCallLoad = new CallMethod
        //    {
        //        Mc_contenido = _postJSON.PostJSONCall() +
        //                       "$('.selectpicker').selectpicker();" +
        //                       "$('.magic-radio').iCheck({" +
        //                            "checkboxClass: 'icheckbox_square-green'," +
        //                            "radioClass: 'iradio_square-green'," +
        //                            "increaseArea: '20%'" +
        //                       "});"
        //    };

        //    return sb.ToString() + _methodCallLoad.CreaJQueryDocumentReady();

        //}
    }
}
