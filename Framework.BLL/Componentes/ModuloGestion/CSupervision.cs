using Framework.BLL.Utilidades.Ajax;
using Framework.BLL.Utilidades.Encriptacion;
using Framework.BLL.Utilidades.Html;
using Framework.BLL.Utilidades.Seguridad;
using Framework.BOL;
using Framework.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Framework.BLL.Componentes.ModuloGestion
{
    public class CSupervision
    {
        AppSettings _appSettings = new AppSettings();
        Encrypt _encrypt = new Encrypt();

        //Supervision general

        public string GetListaEstadosSupervisionGral(int num)
        {
            string str = "";
            List<CodeValue> lista = new List<CodeValue>
            {
                //new CodeValue() { valor = "Pendiente", codigo = "1" },
                new CodeValue() { valor = "Aceptada", codigo = "2" },
                new CodeValue() { valor = "No Aceptada", codigo = "3" }
            };

            CSelect cSelect = new CSelect
            {
                select_code = "codigo",
                select_value = "valor",
                select_id = "dato_col" + num + "",
                select_data = lista,
                select_start = "Seleccione...",
                select_class = "form-control supgral bloqSup"
            };
            //  cSelect.select_selectedValue = selected;
            str = cSelect.getHTMLSelect(cSelect);
            return str;
        }

        //fin supervision general

        public string GetListaNotas(int num)
        {
            string str = "";
            List<CodeValue> lista = new List<CodeValue>
            {
                new CodeValue() { valor = "Si", codigo = "1" },
                new CodeValue() { valor = "No", codigo = "2" },
                new CodeValue() { valor = "No aplica", codigo = "3" }
            };

            CSelect cSelect = new CSelect
            {
                select_code = "codigo",
                select_value = "valor",
                select_id = "sup_col" + num + "",
                select_data = lista,
                select_start = "Seleccione...",
                select_class = "form-control listaNotas"
            };
            //  cSelect.select_selectedValue = selected;
            str = cSelect.getHTMLSelect(cSelect);
            return str;
        }

        public string GetListaNotasEsp(int num)
        {
            string str = "";
            List<CodeValue> lista = new List<CodeValue>
            {
                new CodeValue() { valor = "Bien", codigo = "1" },
                new CodeValue() { valor = "Regular", codigo = "2" },
                new CodeValue() { valor = "Deficiente", codigo = "3" }
            };

            CSelect cSelect = new CSelect
            {
                select_code = "codigo",
                select_value = "valor",
                select_id = "sup_col" + num + "",
                select_data = lista,
                select_start = "Seleccione...",
                select_class = "form-control listaNotasEsp"
            };
            //  cSelect.select_selectedValue = selected;
            str = cSelect.getHTMLSelect(cSelect);
            return str;
        }

        public string GetListaTipoLevantamiento(string tipo_sup)
        {
            string str = "";
            GesSupervisionDAL _gesSupervisionDAL = new GesSupervisionDAL();
            List<CodeValue> lista = _gesSupervisionDAL.ListaDatosTipoLevantamiento<CodeValue>(tipo_sup);

            CSelect cSelect = new CSelect
            {
                select_code = "codigo",
                select_value = "valor",
                select_id = "FiltroTipoLev",
                select_data = lista,
                select_start = "Seleccione...",
                select_class = "form-control cboLev"
            };
            //  cSelect.select_selectedValue = selected;
            str = cSelect.getHTMLSelect(cSelect);
            return str;
        }

        /// <summary>
        /// Obtiene componente html para supervision movil
        /// </summary>
        public string ObtieneTablaListadoMovil(int alc, int tipo, int lev)
        {
            GesSupervisionBLL _gesSupervisionBLL = new GesSupervisionBLL();
            string _strHtml = "";

            GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL();
            GesUsuarioBLL _gesUsuarioBLL = new GesUsuarioBLL();
            _gesUsuarioBOL = _gesUsuarioBLL.ObtieneUsuarioConectado(_appSettings.ObtieneCookie());

            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=\"ibox-content table-border-style\" >");
            sb.Append("<div class=\"row\">");
            sb.Append("<div class=\"table-responsive\">");
            //Si es supervision retroalimentacion entra acá(tipo 1)
            //if(tipo == 1)
            //{
                sb.Append(_gesSupervisionBLL.ListaCuestionariosMovil(alc, tipo, lev, _gesUsuarioBOL.Usu_id));
            //}
            //else if (tipo == 2) //aca se deberia llamar a supervision censal
            //{
            //    //sb.Append(_gesSupervisionBLL.ListaCuestionariosMovilSC(estado, area_censal, tipo, _gesUsuarioBOL.Usu_id));
            //    sb.Append(_gesSupervisionBLL.ListaCuestionariosMovil(alc, tipo, lev, _gesUsuarioBOL.Usu_id));
            //}          
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            //sb.Append(_methodCallLoad.CreaJQueryDocumentReady());

            _strHtml = sb.ToString();

            return _strHtml;
        }

        /// <summary>
        /// Obtiene componente html para supervision movil
        /// </summary>
        public string ObtieneTablaAsignacionManual(int alc, int tipo)
        {
            GesSupervisionBLL _gesSupervisionBLL = new GesSupervisionBLL();
            string _strHtml = "";

            GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL();
            GesUsuarioBLL _gesUsuarioBLL = new GesUsuarioBLL();
            _gesUsuarioBOL = _gesUsuarioBLL.ObtieneUsuarioConectado(_appSettings.ObtieneCookie());

            StringBuilder sb = new StringBuilder();

            // Genero metodo submit del formulario
            CallMethod _methodCallLoad = new CallMethod
            {
                Mc_contenido = "setTimeout(function () { $('.tabla-Manual').DataTable({ 'pageLength': 10, paging: true}); }, 100);"
            };

            sb.Append("<div class=\"ibox-title\">");
            sb.Append("<h5>Seleccione qué indagación o cuestionario desea agregar a Supervisión Indirecta</h5><br>"); //Seleccione a quien desea asignar
            sb.Append("</div>");
            sb.Append("<div class=\"ibox-content table-border-style\" >");            
            sb.Append("<div class=\"row\">");            
            sb.Append("<div class=\"table-responsive\">");

            sb.Append(_gesSupervisionBLL.ListaCuestionariosAsigManual(alc, tipo, _gesUsuarioBOL.Usu_id));

            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append(_methodCallLoad.CreaJQueryDocumentReady());

            _strHtml = sb.ToString();

            return _strHtml;
        }                         
             
        /// <summary>
        /// Obtiene formularios
        /// </summary>
        public string ObtieneFormulariosSegunEstado(string guid, int tipo, string id_supervisor, string id_censista, string tipo_lev)
        {
            GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL();
            GesUsuarioBLL _gesUsuarioBLL = new GesUsuarioBLL();
            _gesUsuarioBOL = _gesUsuarioBLL.ObtieneUsuarioConectado(_appSettings.ObtieneCookie());

            GesAsignacionesDAL gesAsignacionesDAL = new GesAsignacionesDAL();
            DataSet DsDatosUsu = new DataSet();
            DsDatosUsu = gesAsignacionesDAL.ListaDatosUsuario(_gesUsuarioBOL);

            var perfil_usuario = DsDatosUsu.Tables[0].Rows[0]["perfil_id"].ToString();

            GesSupervisionBLL _gesSupervisionBLL = new GesSupervisionBLL();
            string _strHtml = "";
            string contenido = "";

            StringBuilder sb = new StringBuilder();


            if (tipo == 1) // SUP RETROALIMENTACION
            {
                if (tipo_lev.Equals("1")) { // CAPI

                    GesSupervisionDAL gesSupervisionDAL = new GesSupervisionDAL();
                    DataSet DsSupervision = new DataSet();
                    DsSupervision = gesSupervisionDAL.ListaSupervisionPorGuid(guid, 1);
                    string sup = "";
                    string col_sp = "";
                    string chkb = "";

                    if (DsSupervision.Tables[0].Rows.Count > 0) { sup = "1"; } else { sup = "0"; }

                    for (int i = 1; i < 57; i++)
                    {

                        if ((i >= 1 && i <= 10) || (i >= 13 && i <= 22) || (i >= 26 && i <= 30) || (i >= 33 && i <= 37) || (i >= 48 && i <= 52)) {
                            // CHECKBOX
                            if (sup.Equals("1")) {
                                col_sp = "sup_col" + (i + 2);
                                if (DsSupervision.Tables[0].Rows[0][col_sp].ToString().Equals("1")) {
                                    chkb = "checked";
                                } else {
                                    chkb = "";
                                }
                            } else {
                                chkb = "";
                            }
                            // sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=\"1\">");
                            contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + "<input type=\"checkbox\" " + chkb + " id=\"sup_col" + (i + 2) + "\" name=\"sup_col" + (i + 2) + "\" class=\"i-checks chk_perfil\" value=" + 1 + " >" + "');";
                        } else if (i == 11 || i == 31 || i == 39 || i == 43 || i == 47 || i == 54 || i == 56) {
                            // OBSERVACIONES
                            contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + "<input id=\"sup_col" + (i + 2) + "\" name=\"sup_col" + (i + 2) + "\" type=\"text\" class=\"form-control listaNotas\" maxlength=\"950\" placeholder=\"Ingrese su observación\" >" + "');";
                        } else if (i == 12 || i == 32 || i == 38) {
                            // SI-NO CON SALTOS
                            contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + "<select id=\"sup_col" + (i + 2) + "\" name=\"sup_col" + (i + 2) + "\" onchange=\"saltoCuestionario(" + (i + 2) + ")\" class=\"form-control listaNotasEsp\" data-width=\"100%\"><option value=\"-1\">Seleccione...</option><option value=\"1\">Si</option><option value=\"2\">No</option></select>" + "');";
                        } else if(i == 44 || i == 45){
                            // SI-NO-NO APLICA CON SALTOS
                            contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + "<select id=\"sup_col" + (i + 2) + "\" name=\"sup_col" + (i + 2) + "\" onchange=\"saltoCuestionario(" + (i + 2) + ")\" class=\"form-control listaNotasEsp\" data-width=\"100%\"><option value=\"-1\">Seleccione...</option><option value=\"1\">Si</option><option value=\"2\">No</option><option value=\"3\">No Aplica</option></select>" + "');";
                        } else {
                            // SI-NO-NO APLICA SIN SALTOS
                            contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + GetListaNotas(i + 2) + "');";
                        }
                    }
                    sb.Append(_gesSupervisionBLL.MuestraFormSupervisionDirecta(guid));

                } else if (tipo_lev.Equals("3")) { // CATI

                    for (int i = 1; i < 28; i++)
                    {
                        //if (i == 11) {
                        //    contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + "<select id=\"sup_col" + (i + 2) + "\" name=\"sup_col" + (i + 2) + "\" onchange=\"saltoCuestionario(" + (i + 2) + ")\" class=\"form-control listaNotasEsp\" data-width=\"100%\"><option value=\"-1\">Seleccione...</option><option value=\"1\">Si</option><option value=\"2\">No</option></select>" + "');";
                        //} else if (i == 4) {
                        //    contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + "<select id=\"sup_col" + (i + 2) + "\" name=\"sup_col" + (i + 2) + "\" class=\"form-control listaNotasEsp\" data-width=\"100%\"><option value=\"-1\">Seleccione...</option><option value=\"1\">Si</option><option value=\"2\">No</option></select>" + "');";
                        //} else if (i == 8) {
                        //    contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + "<select id=\"sup_col" + (i + 2) + "\" name=\"sup_col" + (i + 2) + "\" onchange=\"saltoCuestionario(" + (i + 2) + ")\" class=\"form-control listaNotasEsp\" data-width=\"100%\"><option value=\"-1\">Seleccione...</option><option value=\"1\">Si</option><option value=\"2\">No</option><option value=\"3\">No Aplica</option></select>" + "');";
                        //} else if (i == 22) {
                        //    contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + "<input id=\"sup_col" + (i + 2) + "\" name=\"sup_col" + (i + 2) + "\" type=\"text\" class=\"form-control listaNotas\" maxlength=\"950\" placeholder=\"Ingrese su observación\" >" + "');";
                        //} else {
                        //    contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + GetListaNotas(i + 2) + "');";
                        //}

                        if ((i >= 1 && i <= 5) || (i >= 19 && i <= 23)) {
                            contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + "<input type=\"checkbox\" id=\"sup_col" + (i + 2) + "\" name=\"sup_col" + (i + 2) + "\" class=\"i-checks chk_perfil\" value=" + 1 + " >" + "');";
                        } else if ((i >= 8 && i <= 10) || i == 6 || i == 14 || i == 26) {
                            // SI-NO SIN SALTOS
                            contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + "<select id=\"sup_col" + (i + 2) + "\" name=\"sup_col" + (i + 2) + "\" class=\"form-control listaNotasEsp\" data-width=\"100%\"><option value=\"-1\">Seleccione...</option><option value=\"1\">Si</option><option value=\"2\">No</option></select>" + "');";
                        } else if (i == 7 || i == 13 || i == 15 || i == 18 || i == 25 || i == 27) {
                            // OBSERVACIONES
                            contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + "<input id=\"sup_col" + (i + 2) + "\" name=\"sup_col" + (i + 2) + "\" type=\"text\" class=\"form-control listaNotas\" maxlength=\"950\" placeholder=\"Ingrese su observación\" >" + "');";
                        } else if (i == 11 || i == 12) {
                            // SI-NO CON SALTOS
                            contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + "<select id=\"sup_col" + (i + 2) + "\" name=\"sup_col" + (i + 2) + "\" onchange=\"saltoCuestionario(" + (i + 2) + ")\" class=\"form-control listaNotasEsp\" data-width=\"100%\"><option value=\"-1\">Seleccione...</option><option value=\"1\">Si</option><option value=\"2\">No</option></select>" + "');";
                        } else  {
                            contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + GetListaNotas(i + 2) + "');";
                        }
                    }
                    sb.Append(_gesSupervisionBLL.MuestraFormSupervisionDirectaTel(guid));

                }

            }
            else if (tipo == 2) // SUP CENSAL
            {
                if (tipo_lev.Equals("1")) {  // CAPI

                    for (int i = 1; i < 16; i++)
                    {
                        if (i == 1 || i == 10 || i == 11 || i == 14) {
                            contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + GetListaNotasEsp(i + 2) + "');";
                        } else if (i == 2 || i == 3) {
                            contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + "<select id=\"sup_col" + (i + 2) + "\" name=\"sup_col" + (i + 2) + "\" onchange=\"saltoCuestionario(" + (i + 2) + ")\" class=\"form-control listaNotasEsp\" data-width=\"100%\"><option value=\"-1\">Seleccione...</option><option value=\"1\">Si</option><option value=\"2\">No</option></select>" + "');";
                        } else if (i == 9) {
                            contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + "<select id=\"sup_col" + (i + 2) + "\" name=\"sup_col" + (i + 2) + "\" class=\"form-control listaNotasEsp\" data-width=\"100%\"><option value=\"-1\">Seleccione...</option><option value=\"1\">Si</option><option value=\"2\">No</option></select>" + "');";
                        } else if (i == 12) {
                            contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + "<select id=\"sup_col" + (i + 2) + "\" name=\"sup_col" + (i + 2) + "\" onchange=\"saltoCuestionario(" + (i + 2) + ")\" class=\"form-control listaNotasEsp\" data-width=\"100%\"><option value=\"-1\">Seleccione...</option><option value=\"1\">Si</option><option value=\"2\">No</option><option value=\"3\">No Aplica</option></select>" + "');";
                        } else if (i == 4 || i == 5 || i == 6 || i == 7 || i == 8 || i == 13 || i == 15) {
                            contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + "<input id=\"sup_col" + (i + 2) + "\" name=\"sup_col" + (i + 2) + "\" type=\"text\" class=\"form-control listaNotas\" maxlength=\"950\" placeholder=\"Ingrese su observación\" >" + "');";
                        }
                    }

                    sb.Append(_gesSupervisionBLL.MuestraFormSupervisionIndirectaMovil(guid));
                }
                else if (tipo_lev.Equals("2")) {  // CAWI

                    for (int i = 1; i < 11; i++)
                    {
                        if (i == 1 || i == 9) {
                            contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + GetListaNotasEsp(i + 2) + "');";
                        } else if (i == 2 || i == 3) {
                            contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + "<select id=\"sup_col" + (i + 2) + "\" name=\"sup_col" + (i + 2) + "\" onchange=\"saltoCuestionario(" + (i + 2) + ")\" class=\"form-control listaNotasEsp\" data-width=\"100%\"><option value=\"-1\">Seleccione...</option><option value=\"1\">Si</option><option value=\"2\">No</option></select>" + "');";
                        } else if (i == 4 || i == 5 || i == 6 || i == 7 || i == 8 || i == 10) {
                            contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + "<input id=\"sup_col" + (i + 2) + "\" name=\"sup_col" + (i + 2) + "\" type=\"text\" class=\"form-control listaNotas\" maxlength=\"950\" placeholder=\"Ingrese su observación\" >" + "');";
                        }
                    }
                    sb.Append(_gesSupervisionBLL.MuestraFormSupervisionIndirectaWeb(guid));

                } else if (tipo_lev.Equals("3")) {  // CATI

                    for (int i = 1; i < 14; i++)
                    {
                        if (i == 1 || i == 10 || i == 11 || i == 12) {
                            contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + GetListaNotasEsp(i + 2) + "');";
                        } else if (i == 2 || i == 3) {
                            contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + "<select id=\"sup_col" + (i + 2) + "\" name=\"sup_col" + (i + 2) + "\" onchange=\"saltoCuestionario(" + (i + 2) + ")\" class=\"form-control listaNotasEsp\" data-width=\"100%\"><option value=\"-1\">Seleccione...</option><option value=\"1\">Si</option><option value=\"2\">No</option></select>" + "');";
                        } else if (i == 9) {
                            contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + "<select id=\"sup_col" + (i + 2) + "\" name=\"sup_col" + (i + 2) + "\" class=\"form-control\" data-width=\"100%\"><option value=\"-1\">Seleccione...</option><option value=\"1\">Si</option><option value=\"2\">No</option></select>" + "');";
                        } else if (i == 4 || i == 5 || i == 6 || i == 7 || i == 8 || i == 13) {
                            contenido = contenido + "$('#filtro_Notas_" + i + "').empty().html('" + "<input id=\"sup_col" + (i + 2) + "\" name=\"sup_col" + (i + 2) + "\" type=\"text\" class=\"form-control listaNotas\" maxlength=\"950\" placeholder=\"Ingrese su observación\" >" + "');";
                        }
                    }

                    sb.Append(_gesSupervisionBLL.MuestraFormSupervisionIndirectaTel(guid));
                }



            } else if (tipo == 3) {  // SUP ESPECIAL - VIVIENDA DESOCUPADA

                contenido = contenido + "$('#filtro_Notas_1').empty().html('" + "<select id=\"sup_col3\" name=\"sup_col3\" class=\"form-control\" data-width=\"100%\"><option value=\"-1\">Seleccione...</option><option value=\"1\">Si</option><option value=\"2\">No</option></select>" + "');";
                contenido = contenido + "$('#filtro_Notas_2').empty().html('" + "<input id=\"sup_col4\" name=\"sup_col4\" type=\"text\" class=\"form-control listaNotas\" maxlength=\"950\" placeholder=\"Ingrese su observación\" >" + "');";

                sb.Append(_gesSupervisionBLL.MuestraFormSupervisionEspOcupacion(guid));

            } else if (tipo == 4) { // SUP ESPECIAL - VIVIENDA COLECTIVA

                contenido = contenido + "$('#filtro_Notas_1').empty().html('" + "<select id=\"sup_col3\" name=\"sup_col3\" class=\"form-control\" data-width=\"100%\"><option value=\"-1\">Seleccione...</option><option value=\"1\">Si</option><option value=\"2\">No</option></select>" + "');";
                contenido = contenido + "$('#filtro_Notas_2').empty().html('" + "<input id=\"sup_col4\" name=\"sup_col4\" type=\"text\" class=\"form-control listaNotas\" maxlength=\"950\" placeholder=\"Ingrese su observación\" >" + "');";
                // MuestraFormSupervisionEspTipoViv
                sb.Append(_gesSupervisionBLL.MuestraFormSupervisionEspTipoViv(guid));

            }

            // Genero metodo submit del formulario
            CallMethod _methodCallLoad = new CallMethod
            {
                Mc_contenido = contenido
                //Supervision retroalimentacion
                //"$('#filtro_Notas_1').empty().html('" + GetListaNotas(2) + "');" +
                //"$('#filtro_Notas_2').empty().html('" + GetListaNotas(3) + "');" +

            };

            sb.Append(_methodCallLoad.CreaJQueryDocumentReady());

            _strHtml = sb.ToString();

            return _strHtml;
        }        
    }
}
