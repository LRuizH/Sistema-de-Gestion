using Framework.BLL.Utilidades.Ajax;
using Framework.BLL.Utilidades.Encriptacion;
using Framework.BLL.Utilidades.Html;
using Framework.BLL.Utilidades.Seguridad;
using Framework.BOL;
using Framework.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.BLL.Componentes.ModuloGestion
{
    public class CReasignaciones
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

        public string GetListaSupervisores(int sistema_id, int areacensal, int perfil)
        {
            string str = "";
            GesReasignacionesDAL _gesReasignacionesDAL = new GesReasignacionesDAL();
            List<CodeValue> lista = _gesReasignacionesDAL.ObtieneUsuariosPerfil<CodeValue>(sistema_id, areacensal, perfil);

            CSelect cSelect = new CSelect
            {
                select_code = "codigo",
                select_value = "valor",
                select_id = "cboSupervision",
                select_data = lista,
                select_start = "Seleccione",
                select_class = "form-control"
            };
            //  cSelect.select_selectedValue = selected;
            str = cSelect.getHTMLSelect(cSelect);
            return str;
        }

        public string GetListaUsuarioOrigen(int sistema_id, int areacensal, int perfil, string sup)
        {
            string str = "";
            GesReasignacionesDAL _gesReasignacionesDAL = new GesReasignacionesDAL();
            List<CodeValue> lista = _gesReasignacionesDAL.ObtieneUsuariosOrigen<CodeValue>(sistema_id, areacensal, perfil, sup);

            CSelect cSelect = new CSelect
            {
                select_code = "codigo",
                select_value = "valor",
                select_id = "cboUsuarioOrigen",
                select_data = lista,
                select_start = "Seleccione",
                select_class = "form-control usuOrig"
            };
            //  cSelect.select_selectedValue = selected;
            str = cSelect.getHTMLSelect(cSelect);
            return str;
        }

        public string GetListaUsuarioDestino(int sistema_id, int areacensal, int perfil, string rut, string sup)
        {
            string str = "";
            GesReasignacionesDAL _gesReasignacionesDAL = new GesReasignacionesDAL();
            List<CodeValue> lista = _gesReasignacionesDAL.ObtieneUsuariosDestino<CodeValue>(sistema_id, areacensal, perfil, rut, sup);

            CSelect cSelect = new CSelect
            {
                select_code = "codigo",
                select_value = "valor",
                select_id = "cboUsuarioDestino",
                select_data = lista,
                select_start = "Seleccione",
                select_class = "form-control usuDest"
            };
            //  cSelect.select_selectedValue = selected;
            str = cSelect.getHTMLSelect(cSelect);
            return str;
        }

        /// <summary>
        /// Obtiene componente html para realizar reasignaciones
        /// </summary>
        public string ObtieneCargaTrabajo(int sistema_id, int tipo_asig, int areacensal, string usu, int geo, int perfil_id, string sup)
        {
            GesAsignacionesBLL _gesAsignacionesBLL = new GesAsignacionesBLL();

            string _strHtml = "";
            string dondeAsignar = "";
            if (usu == null)
            {
                usu = "1";
            }

            if (tipo_asig == 1)
            {
                dondeAsignar = areacensal.ToString();
            }
            else if (tipo_asig == 2 || tipo_asig == 3 || tipo_asig == 4 || tipo_asig == 5 || tipo_asig == 6)
            {
                dondeAsignar = usu.ToString();
            }

            StringBuilder sb = new StringBuilder();

            // Genero metodo submit del formulario
            CallMethod _methodCallLoad = new CallMethod
            {
                Mc_contenido = "$.getScript('" + _appSettings.ServidorWeb + "Framework/assets/js/plugins/dataTables/datatables.min.js', function () {" +
                                        "$.getScript('" + _appSettings.ServidorWeb + "Framework/assets/js/plugins/dataTables/dataTables.bootstrap4.min.js', function () {" +
                                            "setTimeout(function () { $('.tabla-Asig').DataTable({ 'pageLength': 90000000, paging: false}); }, 100);" +
                                        "});" +
                                        "$.getScript('" + _appSettings.ServidorWeb + "Framework/assets/js/plugins/iCheck/icheck.min.js', function () {" +
                                                            "$('.i-checksA').iCheck({ " +
                                                                "checkboxClass: 'icheckbox_square-green'," +
                                                                "radioClass: 'iradio_square-green'," +
                                                            "});" +
                                                        "});" +
                                   "});" +
                                   "$('#filtro_select_usuarioOrigen').empty().html('" + GetListaUsuarioOrigen(sistema_id, areacensal, 7, sup) + "');" +
                                   "$('#filtro_select_usuarioDestino').empty().html('" + GetListaUsuarioDestino(sistema_id, areacensal, 7, "0", sup) + "');" +
                                   "$('.usuDest').attr('disabled','disabled');" +
                                   "$('.usuOrig').on('change', function(){" +
                                       "var selectedOrigen = $(this).val();" +
                                       "var selectedSup = $('#cboSupervision').val();" +
                                       "if(selectedOrigen == -1){" +
                                            "$('.usuDest').attr('disabled','disabled'); $('.usuDest').val(-1);" +
                                            "$('.contenedor-Origen').empty();" +
                                            "$('.contenedor-Destino').empty();" +                                            
                                       "}else{ " +
                                            "$('.usuDest').removeAttr('disabled');" +
                                            "MuestraUsuariosDestino(" + sistema_id + "," + areacensal + ", 7, selectedOrigen, selectedSup); " +
                                            "muestraCargaTrabajoUsuario(" + sistema_id + ", 11, " + areacensal + ", selectedOrigen, " + geo + ", 0, 7, 1);" +
                                            "$('.contenedor-Destino').empty();" +
                                       "}" +
                                   "});"
            };

            // Genero funcion para mostrar usuarios disponibles
            GetJSON _getJSONMuestraUsuariosDestino = new GetJSON();
            {
                _getJSONMuestraUsuariosDestino.G_url_servicio = _appSettings.ServidorWeb + "api/reasignaciones/muestra-usuario-destino";
                _getJSONMuestraUsuariosDestino.G_parametros = "{ sistema_id: sistema_id, areacensal: areacensal, perfil: perfil, usu: usu, sup: sup}";
                _getJSONMuestraUsuariosDestino.G_respuesta_servicio = "$('#filtro_select_usuarioDestino').empty().html(respuesta[0].elemento_html);" +
                                                                           "$('.usuDest').on('change', function(){" +
                                                                               "var selectedDestino = $(this).val();" +
                                                                               "if(selectedDestino == -1){" +
                                                                                    "$('.contenedor-Destino').empty();" +
                                                                               "}else{ " +
                                                                                    "muestraCargaTrabajoUsuario(" + sistema_id + ", 11, " + areacensal + ", selectedDestino, " + geo + ", 0, 7, 2);" +                                                                             
                                                                               "}" +
                                                                           "});";
            }

            CallMethod _methodCallMuestraUsuariosDestino = new CallMethod
            {
                Mc_nombre = "MuestraUsuariosDestino(sistema_id, areacensal, perfil, usu, sup)",
                Mc_contenido = _getJSONMuestraUsuariosDestino.GetJSONCall()
            };

            // Genero funcion obtiene Carga Usuario
            GetJSON _getJSONCargaTrabajoUsuario = new GetJSON();
            {
                _getJSONCargaTrabajoUsuario.G_url_servicio = _appSettings.ServidorWeb + "api/reasignaciones/muestra-carga-usuario";
                _getJSONCargaTrabajoUsuario.G_parametros = "{ sistema_id: sistema_id, tipo_asig: tipo_asig, areacensal: areacensal, usu: usu, geo: geo, disponibles: disponibles, perfil_id: perfil_id, tipo: tipo}";
                _getJSONCargaTrabajoUsuario.G_respuesta_servicio =  "if(tipo == 1){" +
                                                                        "$('.contenedor-Origen').html(respuesta[0].elemento_html);" +
                                                                    "}else{" +
                                                                        "$('.contenedor-Destino').html(respuesta[0].elemento_html);" +
                                                                    "}";
            }

            CallMethod _methodCallMuestraCargaTrabajoUsuario = new CallMethod
            {
                Mc_nombre = "muestraCargaTrabajoUsuario(sistema_id, tipo_asig, areacensal, usu, geo, disponibles, perfil_id, tipo)",
                Mc_contenido = _getJSONCargaTrabajoUsuario.GetJSONCall()
            };

            // Genero funcion para insertar y eliminar asignaciones
            PostJSON _getJSONInsertaAsignacion = new PostJSON();
            {
                _getJSONInsertaAsignacion.P_url_servicio = _appSettings.ServidorWeb + "api/reasignaciones/inserta-reasignaciones";
                _getJSONInsertaAsignacion.P_data = "{ tipo_asig: tipo_asig, sistema_id: sistema_id, sector: sector, usuarioOrigen: usuarioOrigen, usuarioDestino: usuarioDestino}";
                _getJSONInsertaAsignacion.P_respuesta_servicio = "$('.conasignacion').html(respuesta[0].elemento_html);"; //+
                                                                 //"if(tipo == 1){" +
                                                                 //   "muestraCargaTrabajoUsuario(" + sistema_id + ", 3, " + areacensal + ", usuarioOrigen, " + geo + ", 0, 7, 1);" +
                                                                 //"}else{" +
                                                                 //   "muestraCargaTrabajoUsuario(" + sistema_id + ", 3, " + areacensal + ", usuarioDestino, " + geo + ", 0, 7, 2);" +
                                                                 //"}";
            }

            CallMethod _methodCallInsertaAsignaciones = new CallMethod
            {
                Mc_nombre = "insertaReAsignaciones(tipo_asig, sistema_id, sector, usuarioOrigen, usuarioDestino)",
                Mc_contenido = _getJSONInsertaAsignacion.PostJSONCall()
            };

            //Genero función para recorrer seleccionados

            CallMethod _methodCallRecorrer = new CallMethod
            {
                Mc_nombre = "Recorrer(tipoAsignacion)",
                Mc_contenido = "if($('#cboUsuarioOrigen').val() == -1 || $('#cboUsuarioDestino').val() == -1){" +
                                    "$('.mensaje').show();" +
                                "}else{" +
                                    "$('.mensaje').hide();" +
                                    "$('.asignados').each(function () { " +
                                        "if ($('#' + this.id).is(':checked')) { " +
                                             "str = this.id;" +
                                             "var sector = str.split(\"_\")[1];" +
                                             "var selectedOrigen = $('#cboUsuarioOrigen').val();" +
                                             "var selectedDestino = $('#cboUsuarioDestino').val();" +
                                             "if(tipoAsignacion == 1 || tipoAsignacion == 2 || tipoAsignacion == 3 || tipoAsignacion == 4 || tipoAsignacion == 5 || tipoAsignacion == 6 || tipoAsignacion == 11){" +
                                                    "insertaReAsignaciones(tipoAsignacion, " + sistema_id + ", sector, selectedOrigen, selectedDestino);" +
                                                "}else{" +
                                                    "alert('no');" +
                                                "}" +
                                        " }" +
                                    "});" +
                                    "setTimeout(function() { muestraCargaTrabajoUsuario(" + sistema_id + ", 11, " + areacensal + ", $('#cboUsuarioOrigen').val(), " + geo + ", 0, 7, 1);}, 100);" +
                                    "setTimeout(function() { muestraCargaTrabajoUsuario(" + sistema_id + ", 11, " + areacensal + ", $('#cboUsuarioDestino').val(), " + geo + ", 0, 7, 2);}, 100);" +
                                "}"
            //setTimeout(function () { muestraCargaTrabajo(" + sistema_id + ", " + tipo_asig + ", " + areacensal + ", " + usu + ", " + geo + ", " + perfil_id + "); }, 100);
            //"muestraCargaTrabajo(" + sistema_id + ", " + tipo_asig + ", " + areacensal + ", " + usu + ", " + geo + ", " + perfil_id + ");"
        };

            //Genero función para seleccionar todo disponible

            CallMethod _methodCallMarcarTodo = new CallMethod
            {
                Mc_nombre = "MarcarTodo()",
                Mc_contenido = "$('.Asig').iCheck('check');"
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
                Mc_contenido = "$('.Asig').iCheck('uncheck');"
            };

            //Genero función para desmarcar todo Asignado

            CallMethod _methodCallDesmarcarTodoAsignado = new CallMethod
            {
                Mc_nombre = "DesmarcarTodoAsignado()",
                Mc_contenido = "$('.Asig').iCheck('uncheck');"
            };

            GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL();
            GesUsuarioBLL _gesUsuarioBLL = new GesUsuarioBLL();
            _gesUsuarioBOL = _gesUsuarioBLL.ObtieneUsuarioConectado(_appSettings.ObtieneCookie());

            //sb.Append("<form id=\"" + _postJSON.P_form + "\" class=\"md-float-material\" method=\"post\">");            
            sb.Append("<div class=\"ibox-title\">");
            sb.Append("<h5>Asignaciones</h5><br>");
            //sb.Append("<span>Asigne o quite  </span>");
            //sb.Append("<span>Seleccione <code>Empadronadores</code> para asignar a comuna</span>");
            sb.Append("</div>");
            sb.Append("<div class=\"ibox-content table-border-style\" >");
            sb.Append("<div class=\"row col-lg-12\">");
            sb.Append("<div class=\"col-lg-5 alert alert-warning\">");
            sb.Append("<h4 class=\"text-center\">USUARIO A QUITAR ASIGNACIONES</h4>");
            sb.Append("<div class=\"table-responsive\">");
            sb.Append("<div class=\"row col-lg-12 form-group\">");
            sb.Append("<div class=\"col-lg-6\">");
            sb.Append("<button type=\"submit\" onclick=\"MarcarTodo();\" class=\"btn btn-sm btn-block btn-primary\"><i class=\"fa fa-check-square-o\"></i> Marcar todo</button>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6\">");
            sb.Append("<button type=\"submit\" onclick=\"DesmarcarTodo();\" class=\"btn btn-sm btn-block btn-info\"><i class=\"fa fa-square-o\"></i> Desmarcar todo</button>");
            sb.Append("</div>");
            sb.Append("</div>");

            // div seleccionar filtro Usuario origen
            sb.Append("<div id=\"filtro_usuarioOrigen\" style=\"\" class=\"row col-lg-12 form-group\">");
            sb.Append("<label class=\"col-sm-2 col-form-label\"> Seleccione </label>");
            sb.Append("<div id=\"filtro_select_usuarioOrigen\" class=\"col-sm-10\">");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"contenedor-Origen\"></div>");
            //sb.Append(_gesAsignacionesBLL.ObtieneDisponibles(sistema_id, tipo_asig, areacensal, dondeAsignar, geo, 1, perfil_id)); //_gesCargaTrabajoBLL.ObtieneMenuPorPerfil(_gesCargaTrabajoBOL)
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-2 text-center\" style=\"padding-top: 20px; padding-bottom: 20px\">");
            sb.Append("<button type=\"submit\" onclick=\"Recorrer(11); \" class=\"btn btn-success dim btn-large-dim text-center m-b-20\"><i class=\"fa fa-arrows-h\"></i></button>");
            //sb.Append("<button type=\"submit\" onclick=\"Recorrer(" + tipo_asig + "); setTimeout(function () { muestraCargaTrabajo(" + sistema_id + ", " + tipo_asig + ", " + areacensal + ", '" + dondeAsignar + "', " + geo + ", " + perfil_id + "); }, 100);\" class=\"btn btn-success dim btn-large-dim text-center m-b-20\"><i class=\"fa fa-arrows-h\"></i></button>");
            //sb.Append("<button type=\"submit\" onclick=\"Recorrer(1);\" class=\"btn btn-danger btn-md btn-block waves-effect text-center m-b-20\"><i class=\"icofont icofont-arrow-left\"></i>  QUITAR</button>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-5 alert alert-success\">");
            sb.Append("<h4 class=\"text-center\">USUARIO A ASIGNAR</h4>");
            sb.Append("<div class=\"table-responsive\">");
            sb.Append("<div class=\"row col-lg-12 form-group\">");
            sb.Append("<div class=\"col-lg-6\">");
            //sb.Append("<button type=\"submit\" onclick=\"MarcarTodoAsignado();\" class=\"btn btn-sm btn-block btn-primary\"><i class=\"fa fa-check-square-o\"></i> Marcar todo</button>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-lg-6\">");
            //sb.Append("<button type=\"submit\" onclick=\"DesmarcarTodoAsignado();\" class=\"btn btn-sm btn-block btn-info\"><i class=\"fa fa-square-o\"></i> Desmarcar todo</button>");
            sb.Append("</div>");
            sb.Append("</div>");

            // div seleccionar filtro Usuario destino
            sb.Append("<div id=\"filtro_usuarioDestino\" style=\"\" class=\"row col-lg-12 form-group\">");
            sb.Append("<label class=\"col-sm-2 col-form-label\"> Seleccione </label>");
            sb.Append("<div id=\"filtro_select_usuarioDestino\" class=\"col-sm-10\">");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("<div class=\"contenedor-Destino\"></div>");
            //sb.Append(_gesAsignacionesBLL.ObtieneUsuariosAsignados(sistema_id, tipo_asig, areacensal, dondeAsignar, geo, 0, perfil_id)); //_gesCargaTrabajoBLL.ObtieneMenuPorPerfil(_gesCargaTrabajoBOL)
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
            sb.Append(_methodCallMuestraUsuariosDestino.CreaJQueryFunction());
            sb.Append(_methodCallMuestraCargaTrabajoUsuario.CreaJQueryFunction());

            _strHtml = sb.ToString();

            return _strHtml;
        }
    }
}
