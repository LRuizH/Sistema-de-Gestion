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
    public class CAsignaciones
    {
        AppSettings _appSettings = new AppSettings();
        Encrypt _encrypt = new Encrypt();

        public string GetListaAreaCensal(GesGeografiaBOL _gesGeografiaBOL, int tipo_asig, string id_usuario, int perfil_usuario)
        {
            string str = "";
            GesGeografiaDAL _gesGeografiaDAL = new GesGeografiaDAL();
            List<CodeValue> lista = _gesGeografiaDAL.BuscarAreaCensalSegunRegion<CodeValue>(_gesGeografiaBOL, tipo_asig, id_usuario, perfil_usuario);

            CSelect cSelect = new CSelect
            {
                select_code = "codigo",
                select_value = "valor",
                select_id = "AreaCensal",
                select_data = lista,
                select_start = "Seleccione",
                select_class = "form-control acensal"
            };
            //  cSelect.select_selectedValue = selected;
            str = cSelect.getHTMLSelect(cSelect);
            return str;
        }

        public string GetListaAreaTipoAsig(GesGeografiaBOL _gesGeografiaBOL, int tipo_asig)
        {
            string str = "";
            GesGeografiaDAL _gesGeografiaDAL = new GesGeografiaDAL();
            List<CodeValue> lista = _gesGeografiaDAL.BuscarTipoAsigSegunArea<CodeValue>(_gesGeografiaBOL,tipo_asig);

            CSelect cSelect = new CSelect
            {
                select_code = "codigo",
                select_value = "valor",
                select_id = "TipoAsigArea",
                select_data = lista,
                select_start = "Seleccione",
                select_class = "form-control tasigarea"
            };
            //  cSelect.select_selectedValue = selected;
            str = cSelect.getHTMLSelect(cSelect);
            return str;
        }

        public string GetListaSectorCensal(GesGeografiaBOL _gesGeografiaBOL, int tipo_asig, string id_usuario, int perfil_usuario)
        {
            string str = "";
            GesGeografiaDAL _gesGeografiaDAL = new GesGeografiaDAL();
            List<CodeValue> lista = _gesGeografiaDAL.BuscarSectorSegunArea<CodeValue>(_gesGeografiaBOL, tipo_asig, id_usuario, perfil_usuario);

            CSelect cSelect = new CSelect
            {
                select_code = "codigo",
                select_value = "valor",
                select_id = "SectorCensal",
                select_data = lista,
                select_start = "Seleccione",
                select_class = "form-control sectcensal"
            };
            //  cSelect.select_selectedValue = selected;
            str = cSelect.getHTMLSelect(cSelect);
            return str;
        }

        public string GetListaLocalCensal(GesGeografiaBOL _gesGeografiaBOL, int tipo_asig, string id_usuario, int perfil_usuario)
        {
            string str = "";
            GesGeografiaDAL _gesGeografiaDAL = new GesGeografiaDAL();
            List<CodeValue> lista = _gesGeografiaDAL.BuscarLocalSegunArea<CodeValue>(_gesGeografiaBOL, tipo_asig, id_usuario, perfil_usuario);

            CSelect cSelect = new CSelect
            {
                select_code = "codigo",
                select_value = "valor",
                select_id = "LocalCensal",
                select_data = lista,
                select_start = "Seleccione",
                select_class = "form-control loccensal"
            };
            //  cSelect.select_selectedValue = selected;
            str = cSelect.getHTMLSelect(cSelect);
            return str;
        }

        public string GetListaALCLocal(string local_id, string id_usuario, int perfil_usuario)
        {
            string str = "";
            GesAsignacionesDAL _gesAsignacionesDAL = new GesAsignacionesDAL();
            List<CodeValue> lista = _gesAsignacionesDAL.ListaDatosALCLocal<CodeValue>(local_id, id_usuario, perfil_usuario);

            CSelect cSelect = new CSelect
            {
                select_code = "codigo",
                select_value = "valor",
                select_id = "FiltroALC",
                select_data = lista,
                select_start = "Seleccione...",
                select_class = "form-control cboAlc"
            };
            //  cSelect.select_selectedValue = selected;
            str = cSelect.getHTMLSelect(cSelect);
            return str;
        }

        /// <summary>
        /// Obtiene componente html para carga de trabajo
        /// </summary>
        public string ObtieneTablaPerfil(int sistema_id, int tipo_asig, int areacensal, string usu, int geo, int perfil_id)
        {
            GesAsignacionesBLL _gesAsignacionesBLL = new GesAsignacionesBLL();
            string _strHtml = "";

            StringBuilder sb = new StringBuilder();

            // Genero metodo submit del formulario
            CallMethod _methodCallLoad = new CallMethod
            {
                Mc_contenido = "setTimeout(function () { $('.tabla-Perfil').DataTable({ 'pageLength': 10, paging: true}); }, 100);" +
                               "$.getScript('" + _appSettings.ServidorWeb + "Framework/assets/js/plugins/iCheck/icheck.min.js', function () {" +
                                    "$('.i-checksA').iCheck({ " +
                                        "checkboxClass: 'icheckbox_square-green'," +
                                        "radioClass: 'iradio_square-green'," +
                                    "});" +
                               "});"
            };

            string textoPerfil = "";

            if (tipo_asig == 10) {
                textoPerfil = "Seleccione a que Coordinador de Grupo (CGC) desea asignar Censistas";
            } else if (tipo_asig == 11) {
                textoPerfil = "Seleccione Coordinador de Grupo (CGC) para visualizar a Censistas asignados";
            }

            sb.Append("<div class=\"ibox-title\">");
            sb.Append("<h5>" + textoPerfil + "</h5><br>"); //Seleccione a quien desea asignar
            //sb.Append("<span>Seleccione <code>Tipo de asignación</code> a trabajar</span>");
            sb.Append("</div>");
            sb.Append("<div class=\"ibox-content table-border-style\" >");
            sb.Append("<div class=\"row\">");
            sb.Append("<div class=\"table-responsive\">");
            sb.Append(_gesAsignacionesBLL.ListaSegunPerfil(sistema_id, tipo_asig, areacensal, usu, geo, perfil_id)); //_gesCargaTrabajoBLL.ObtieneMenuPorPerfil(_gesCargaTrabajoBOL)
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append(_methodCallLoad.CreaJQueryDocumentReady());

            _strHtml = sb.ToString();

            return _strHtml;
        }


        /// <summary>
        /// Obtiene componente html para carga de trabajo
        /// </summary>
        public string ObtieneTablaRecSector(int sistema_id, int tipo_asig, int areacensal, string usu, int geo, int perfil_id)
        {
            GesAsignacionesBLL _gesAsignacionesBLL = new GesAsignacionesBLL();
            string _strHtml = "";

            StringBuilder sb = new StringBuilder();

            // Genero metodo submit del formulario
            CallMethod _methodCallLoad = new CallMethod
            {
                Mc_contenido = "setTimeout(function () { $('.tabla-RecSec').DataTable({ 'pageLength': 10, paging: true}); }, 100);" +
                               "$.getScript('" + _appSettings.ServidorWeb + "Framework/assets/js/plugins/iCheck/icheck.min.js', function () {" +
                                    "$('.i-checksA').iCheck({ " +
                                        "checkboxClass: 'icheckbox_square-green'," +
                                        "radioClass: 'iradio_square-green'," +
                                    "});" +
                               "});"
            };

            string textoPerfil = "";

            if (tipo_asig == 11) {
                textoPerfil = "Seleccione a que Censista desea asignar carga";
            } 

            //if (tipo_asig != 6)
            //{
            //    textoPerfil = "Seleccione a que recolector desea asignar sectores";
            //}
            //else
            //{
            //    textoPerfil = "Seleccione a que analista desea asignar empresas";
            //}



            sb.Append("<div class=\"ibox-title\">");
            sb.Append("<h5>" + textoPerfil + "</h5><br>"); //Seleccione a quien desea asignar
            //sb.Append("<span>Seleccione <code>Tipo de asignación</code> a trabajar</span>");
            sb.Append("</div>");
            sb.Append("<div class=\"ibox-content table-border-style\" >");
            sb.Append("<div class=\"row\">");
            sb.Append("<div class=\"table-responsive\">");
            sb.Append(_gesAsignacionesBLL.ListaSegunRecSector(sistema_id, tipo_asig, areacensal, usu, geo, perfil_id)); //_gesCargaTrabajoBLL.ObtieneMenuPorPerfil(_gesCargaTrabajoBOL)
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append(_methodCallLoad.CreaJQueryDocumentReady());

            _strHtml = sb.ToString();

            return _strHtml;
        }


        /// <summary>
        /// Obtiene componente html para carga de trabajo
        /// </summary>
        public string ObtieneCargaTrabajo(int sistema_id, int tipo_asig, int areacensal, string usu, int geo, int perfil_id, int nivel)
        {
            GesAsignacionesBLL _gesAsignacionesBLL = new GesAsignacionesBLL();
            //_gesAsignacionesBOL.geografia_id = 13201;
            string _strHtml = "";
            string dondeAsignar = "";
            if (usu == null)
            {
                usu = "1";
            }

            //if (tipo_asig == 1)
            if (tipo_asig == 3 || tipo_asig == 4 || tipo_asig == 5)
            {
                dondeAsignar = areacensal.ToString();
            }
            //else if (tipo_asig == 2 || tipo_asig == 3 || tipo_asig == 4 || tipo_asig == 5 || tipo_asig == 6 || tipo_asig == 7 || tipo_asig == 8)
            else if (tipo_asig == 10 || tipo_asig == 11)
            {
                dondeAsignar = usu.ToString();
            }

            StringBuilder sb = new StringBuilder();
            string lenguaje_dt = "\"language\": {\"lengthMenu\": \"Mostrar _MENU_ filas\",\"zeroRecords\": \"No se encontraron registros\",\"info\": \"Mostrando _TOTAL_ resultados\",\"infoEmpty\": \"Sin registros disponibles\",\"infoFiltered\": \"(filtrados de un total de _MAX_ registros)\",\"sSearch\": \"Buscar:\",\"oPaginate\": {\"sNext\": \"Siguiente\",\"sPrevious\": \"Anterior\"}}";

            // Genero metodo submit del formulario
            CallMethod _methodCallLoad = new CallMethod
            {
                Mc_contenido = "setTimeout(function () { $('.tabla-Asig').DataTable({ 'pageLength': 20, paging: true, " + lenguaje_dt + "}); }, 100);" +
                               "$.getScript('" + _appSettings.ServidorWeb + "Framework/assets/js/plugins/iCheck/icheck.min.js', function () {" +
                                    "$('.i-checksA').iCheck({ " +
                                        "checkboxClass: 'icheckbox_square-green'," +
                                        "radioClass: 'iradio_square-green'," +
                                    "});" +
                               "});"
            };

            // Genero funcion para insertar y eliminar asignaciones
            PostJSON _getJSONInsertaAsignacion = new PostJSON();
            {
                _getJSONInsertaAsignacion.P_url_servicio = _appSettings.ServidorWeb + "api/asignaciones/inserta-asignaciones";
                _getJSONInsertaAsignacion.P_data = "{ tipo_asig: tipo_asig, sistema_id: sistema_id, usu: usu, areacensal: areacensal, nivelasig: nivelasig, perfilasig: perfilasig}";
                _getJSONInsertaAsignacion.P_respuesta_servicio = "$('.conasignacion').html(respuesta[0].elemento_html);";
            }

            CallMethod _methodCallInsertaAsignaciones = new CallMethod
            {
                Mc_nombre = "insertaAsignaciones(tipo_asig, sistema_id, usu, areacensal, nivelasig, perfilasig)",
                Mc_contenido = _getJSONInsertaAsignacion.PostJSONCall()
            };

            //Genero función para recorrer seleccionados

            CallMethod _methodCallRecorrer = new CallMethod
            {
                Mc_nombre = "Recorrer(tipoAsignacion)",
                Mc_contenido = "$('.asignados').each(function () { " +
                                    "if ($('#' + this.id).is(':checked')) { " +
                                         "str = this.id;" +
                                         "var usuarioid = str.split(\"_\")[1];" +
                                         "var tipoAreaAsig = $('#TipoAsigArea').val();" +
                                         "var perfilAsig = $('#cboPerfiles').val();" +
                                         //"if(tipoAsignacion == 1 || tipoAsignacion == 2 || tipoAsignacion == 3 || tipoAsignacion == 4 || tipoAsignacion == 5 || tipoAsignacion == 6 || tipoAsignacion == 7 || tipoAsignacion == 8){" +
                                         "if(tipoAsignacion == 3 || tipoAsignacion == 4 || tipoAsignacion == 5 || tipoAsignacion == 10 || tipoAsignacion == 11){" +
                                                "insertaAsignaciones(tipoAsignacion, " + sistema_id + ", usuarioid, '" + dondeAsignar + "', tipoAreaAsig, perfilAsig);" +
                                            "}else{" +
                                                "alert('no');" +
                                            "}" +
                                    //"insertaAsignaciones(tipoAsignacion, 1, usuarioid, " + areacensal + ");" +
                                    " }" +
                                "});"
                //setTimeout(function () { muestraCargaTrabajo(" + sistema_id + ", " + tipo_asig + ", " + areacensal + ", " + usu + ", " + geo + ", " + perfil_id + "); }, 100);
                //"muestraCargaTrabajo(" + sistema_id + ", " + tipo_asig + ", " + areacensal + ", " + usu + ", " + geo + ", " + perfil_id + ");"
            };

            //Genero función para seleccionar todo disponible

            CallMethod _methodCallMarcarTodo = new CallMethod
            {
                Mc_nombre = "MarcarTodo()",
                Mc_contenido = "$('.disp').iCheck('check');"
            };

            //Genero función para seleccionar todo Asignado

            CallMethod _methodCallMarcarTodoAsignado = new CallMethod
            {
                Mc_nombre = "MarcarTodoAsignado()",
                Mc_contenido = "$('.Asig').iCheck('check');"
            };

            //Genero función para desmarcar todo disponible

            CallMethod _methodCallDesmarcarTodo = new CallMethod
            {
                Mc_nombre = "DesmarcarTodo()",
                Mc_contenido = "$('.disp').iCheck('uncheck');"
            };

            //Genero función para desmarcar todo Asignado

            CallMethod _methodCallDesmarcarTodoAsignado = new CallMethod
            {
                Mc_nombre = "DesmarcarTodoAsignado()",
                Mc_contenido = "$('.Asig').iCheck('uncheck');"
            };

            GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL();
            _gesUsuarioBOL.Usu_id = _encrypt.DecryptString(_appSettings.ObtieneCookie());

            GesAsignacionesDAL gesAsignacionesDAL = new GesAsignacionesDAL();
            DataSet DsDatosUsu = new DataSet();
            DsDatosUsu = gesAsignacionesDAL.ListaDatosUsuario(_gesUsuarioBOL);

            //var id_usuario = DsDatosUsu.Tables[0].Rows[0]["usu_id"].ToString();
            var perfil_usuario = DsDatosUsu.Tables[0].Rows[0]["perfil_id"].ToString();
            //var geo_usuario = DsDatosUsu.Tables[0].Rows[0]["geografia_id"].ToString();
            //var area_usuario = DsDatosUsu.Tables[0].Rows[0]["areacensal"].ToString();


            //sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"md-float-material\" method=\"post\">");            
            sb.Append("<div class=\"ibox-title\">");
            sb.Append("<h5>Asignaciones</h5><br>");
            //sb.Append("<span>Asigne o quite  </span>");
            //sb.Append("<span>Seleccione <code>Empadronadores</code> para asignar a comuna</span>");
            sb.Append("</div>");
            sb.Append("<div class=\"ibox-content table-border-style\" >");
            sb.Append("<div class=\"row col-lg-12\">");
            sb.Append("<div class=\"col-lg-5 alert alert-warning\">");
            sb.Append("<h4 class=\"text-center\">ASIGNAR</h4>");
            sb.Append("<div class=\"table-responsive\">");
            sb.Append("<div class=\"row col-lg-12 form-group\">");
            sb.Append("<div class=\"col-lg-6\">");
            sb.Append("<button type=\"submit\" onclick=\"MarcarTodo();\" class=\"btn btn-sm btn-block btn-primary\"><i class=\"fa fa-check-square-o\"></i> Marcar todo</button>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6\">");
            sb.Append("<button type=\"submit\" onclick=\"DesmarcarTodo();\" class=\"btn btn-sm btn-block btn-info\"><i class=\"fa fa-square-o\"></i> Desmarcar todo</button>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append(_gesAsignacionesBLL.ObtieneDisponibles(sistema_id, tipo_asig, areacensal, dondeAsignar, geo, 1, perfil_id, nivel)); //_gesCargaTrabajoBLL.ObtieneMenuPorPerfil(_gesCargaTrabajoBOL)
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-2 text-center\" style=\"padding-top: 20px; padding-bottom: 20px\">");
            if (perfil_usuario == "6")
            {
                sb.Append("");
            }
            else
            {
                //"muestraCargaTrabajo(1, 1, selectedLC, '', geomostrar, selectedPerf,selectedTAA); "
                sb.Append("<button type=\"submit\" onclick=\"Recorrer(" + tipo_asig + "); setTimeout(function () { muestraCargaTrabajo(" + sistema_id + ", " + tipo_asig + ", " + areacensal + ", '" + dondeAsignar + "', " + geo + ", " + perfil_id + ", " + nivel + "); }, 100);\" class=\"btn btn-success dim btn-large-dim text-center m-b-20\"><i class=\"fa fa-arrows-h\"></i></button>");
            }
            //sb.Append("<button type=\"submit\" onclick=\"Recorrer(1);\" class=\"btn btn-danger btn-md btn-block waves-effect text-center m-b-20\"><i class=\"icofont icofont-arrow-left\"></i>  QUITAR</button>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-5 alert alert-success\">");
            sb.Append("<h4 class=\"text-center\">ASIGNADOS</h4>");
            sb.Append("<div class=\"table-responsive\">");
            sb.Append("<div class=\"row col-lg-12 form-group\">");
            sb.Append("<div class=\"col-lg-6\">");
            sb.Append("<button type=\"submit\" onclick=\"MarcarTodoAsignado();\" class=\"btn btn-sm btn-block btn-primary\"><i class=\"fa fa-check-square-o\"></i> Marcar todo</button>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6\">");
            sb.Append("<button type=\"submit\" onclick=\"DesmarcarTodoAsignado();\" class=\"btn btn-sm btn-block btn-info\"><i class=\"fa fa-square-o\"></i> Desmarcar todo</button>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append(_gesAsignacionesBLL.ObtieneUsuariosAsignados(sistema_id, tipo_asig, areacensal, dondeAsignar, geo, 0, perfil_id, nivel)); //_gesCargaTrabajoBLL.ObtieneMenuPorPerfil(_gesCargaTrabajoBOL)
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            //sb.Append("</form>");
            sb.Append(_methodCallLoad.CreaJQueryDocumentReady());
            sb.Append(_methodCallInsertaAsignaciones.CreaJQueryFunction());
            sb.Append(_methodCallRecorrer.CreaJQueryFunction());
            sb.Append(_methodCallMarcarTodo.CreaJQueryFunction());
            sb.Append(_methodCallDesmarcarTodo.CreaJQueryFunction());
            sb.Append(_methodCallMarcarTodoAsignado.CreaJQueryFunction());
            sb.Append(_methodCallDesmarcarTodoAsignado.CreaJQueryFunction());

            _strHtml = sb.ToString();

            return _strHtml;
        }

        /// <summary>
        /// Obtiene componente HTML con lista de resumen de conformacion de equipos
        /// </summary>
        /// <returns></returns>
        public string ObtieneTablaListadoResumenAsig(string usuario, int geo, int perfil_id)
        {
            GesAsignacionesBLL _gesAsignaciones = new GesAsignacionesBLL();
            //GesSupervisionBLL _gesSupervisionBLL = new GesSupervisionBLL();
            string _strHtml = "";

            StringBuilder sb = new StringBuilder();
            string lenguaje_dt = "\"language\": {\"lengthMenu\": \"Mostrar _MENU_ filas\",\"zeroRecords\": \"No se encontraron registros\",\"info\": \"Mostrando _TOTAL_ resultados\",\"infoEmpty\": \"Sin registros disponibles\",\"infoFiltered\": \"(filtrados de un total de _MAX_ registros)\",\"sSearch\": \"Buscar:\",\"oPaginate\": {\"sNext\": \"Siguiente\",\"sPrevious\": \"Anterior\"}}";


            // Genero metodo submit del formulario
            CallMethod _methodCallLoad = new CallMethod
            {
                Mc_contenido = "setTimeout(function () { $('.tabla-Estados').DataTable({ retrieve: true, \"lengthChange\": false, 'pageLength': 10, paging: true}); }, 100);" +
                               "setTimeout(function () { $('.tabla-Informantes').DataTable({ retrieve: true, \"lengthChange\": false, 'pageLength': 10, paging: true, " + lenguaje_dt + "}); }, 100);" +
                               "setTimeout(function () { $('.tabla-Estadosweb').DataTable({ retrieve: true, \"lengthChange\": false, 'pageLength': 10, paging: true}); }, 100);"
            };

            sb.Append("<div class=\"row\">");
            sb.Append("<div class=\"table-responsive\">");
            sb.Append(_gesAsignaciones.ListaResumenAsignacion(usuario, geo, perfil_id));
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append(_methodCallLoad.CreaJQueryDocumentReady());

            _strHtml = sb.ToString();

            return _strHtml;
        }


        /// <summary>
        /// Obtiene componente html de listado de equipos
        /// </summary>
        public string ObtieneTablaListadoEquipos(string usuario, int geo, int perfil_id, string local_id)
        {
            //GesSupervisionBLL _gesSupervisionBLL = new GesSupervisionBLL();
            GesAsignacionesBLL _gesAsignaciones = new GesAsignacionesBLL();
            string _strHtml = "";

            StringBuilder sb = new StringBuilder();
            string lenguaje_dt = "\"language\": {\"lengthMenu\": \"Mostrar _MENU_ filas\",\"zeroRecords\": \"No se encontraron registros\",\"info\": \"Mostrando _TOTAL_ resultados\",\"infoEmpty\": \"Sin registros disponibles\",\"infoFiltered\": \"(filtrados de un total de _MAX_ registros)\",\"sSearch\": \"Buscar:\",\"oPaginate\": {\"sNext\": \"Siguiente\",\"sPrevious\": \"Anterior\"}}";


            // Genero metodo submit del formulario
            CallMethod _methodCallLoad = new CallMethod
            {
                Mc_contenido = "setTimeout(function () { $('.tabla-Equipos').DataTable({ 'pageLength': 10, paging: true, 'searching': true, 'ordering': true, 'info': false, " + lenguaje_dt + "}); }, 100);"
            };

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<h2 class=\"text-center\">Listado de equipos censales por local</h2>");
            sb.Append("<div class=\"table-responsive\">");
            //sb.Append(_gesSupervisionBLL.DibujaTablaFormEmpresas(rut));
            sb.Append(_gesAsignaciones.ListaResumenAsignacionEquipos(usuario, geo, perfil_id,local_id));
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append(_methodCallLoad.CreaJQueryDocumentReady());

            _strHtml = sb.ToString();

            return _strHtml;
        }


        /// <summary>
        /// Obtiene componente html de listado de direcciones según ALC seleccionada
        /// </summary>
        public string ObtieneTablaListadoDirecciones(string alc_id, string nombre_alc)
        {
            //GesSupervisionBLL _gesSupervisionBLL = new GesSupervisionBLL();
            GesAsignacionesBLL _gesAsignaciones = new GesAsignacionesBLL();
            string _strHtml = "";

            StringBuilder sb = new StringBuilder();
            string lenguaje_dt = "\"language\": {\"lengthMenu\": \"Mostrar _MENU_ filas\",\"zeroRecords\": \"No se encontraron registros\",\"info\": \"Mostrando _TOTAL_ resultados\",\"infoEmpty\": \"Sin registros disponibles\",\"infoFiltered\": \"(filtrados de un total de _MAX_ registros)\",\"sSearch\": \"Buscar:\",\"oPaginate\": {\"sNext\": \"Siguiente\",\"sPrevious\": \"Anterior\"}}";


            // Genero metodo submit del formulario
            CallMethod _methodCallLoad = new CallMethod
            {
                Mc_contenido = "setTimeout(function () { $('.tabla-Direcciones').DataTable({ 'pageLength': 25, paging: true, 'searching': true, 'ordering': true, 'info': true, " + lenguaje_dt + "}); }, 100);"
            };

            sb.Append("<div class=\"col-lg-12\">");
            sb.Append("<h2 class=\"text-center\">Direcciones asociadas a: " + nombre_alc + "</h2>");
            sb.Append("<div class=\"table-responsive\">");
            //sb.Append(_gesSupervisionBLL.DibujaTablaFormEmpresas(rut));
            sb.Append(_gesAsignaciones.ListaDireccionesALC(alc_id));
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append(_methodCallLoad.CreaJQueryDocumentReady());

            _strHtml = sb.ToString();

            return _strHtml;
        }

    }
}
