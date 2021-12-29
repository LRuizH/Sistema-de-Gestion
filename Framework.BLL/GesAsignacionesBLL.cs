using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using Framework.BOL;
using Framework.DAL;
using Framework.BLL.Utilidades.Seguridad;
using Framework.BLL.Utilidades.Ajax;
using Framework.BLL.Utilidades.Email;
using DatoSSINE.VO.SurveySolution.General;
using Framework.BLL.Integracion;
using DatoSSINE.VO.SurveySolution.Usuario;
using static Framework.BOL.Enumeracion;
using DatoSSINE.VO.SurveySolution.Asignaciones;
using System.Threading.Tasks;
using Framework.BOL.Integracion;

namespace Framework.BLL
{
    public class GesAsignacionesBLL
    {
        AppSettings _appSettings = new AppSettings();
        string _strHtml = "";
        DataSet DS;

        public static List<ParametrosBOL> ListParametros ()
        {
            var parametros = new List<ParametrosBOL>();
             return parametros = new ParametrosBLL().ListParametros(1);
        }
        /// <summary>
        /// Permite listar usuarios asignados
        /// </summary>
        /// 
        public string ListaSegunPerfil(int sistema_id, int tipo_asig, int areacensal, string usu, int geo, int perfil_id)
        {
            //Genero función para recorrer seleccionados
            CallMethod _methodCallMarcaSeleccion = new CallMethod
            {
                Mc_nombre = "MarcaSeleccion(identificador)",
                Mc_contenido = "$('.filasPerfil').removeClass('alert alert-success');" +
                               "$('#tr_' + identificador).addClass('alert alert-success');"
            };

            try
            {
                StringBuilder sb = new StringBuilder();
                _strHtml = "";

                if (usu == null)
                {
                    usu = "1";
                }

                GesAsignacionesDAL gesAsignacionesDAL = new GesAsignacionesDAL();
                List<GesUsuarioBOL> listCarga = gesAsignacionesDAL.ListaSegunPerfil<GesUsuarioBOL>(sistema_id, tipo_asig, areacensal, usu, geo, perfil_id);

                //Obtengo listado Usuarios
                var listUsuPer = from m in listCarga
                                 orderby m.Usu_id
                                 select m;

                if (listUsuPer.Count() > 0)
                {

                    sb.Append("<table class=\"tabla-Perfil table table-hover text-center\">"); //table-striped                                  
                    sb.Append("<thead><tr>");
                    sb.Append("<th>Nombre</th>");
                    sb.Append("<th>Rut</th>");
                    sb.Append("<th>E-mail</th>");
                    sb.Append("<th>Teléfono</th>");
                    sb.Append("<th></th>");
                    sb.Append("</tr></thead>");
                    sb.Append("<tbody>");
                    foreach (var itemCarga in listUsuPer)
                    {
                        sb.Append("<tr id=\"tr_" + itemCarga.Usu_id.ToString() + "\" class=\"filasPerfil\">");
                        sb.Append("<td>&nbsp" + itemCarga.Usu_nombre + "</td>");
                        sb.Append("<td>&nbsp" + itemCarga.Usu_rut + "</td>");
                        sb.Append("<td>&nbsp" + itemCarga.Usu_email + "</td>");
                        sb.Append("<td>&nbsp" + itemCarga.Usu_telefono + "</td>");
                        if (tipo_asig == 10) 
                        {
                            sb.Append("<td><button id=\"btnVer_" + itemCarga.Usu_id.ToString() + "\" onclick=\"MarcaSeleccion('" + itemCarga.Usu_id.ToString() + "'); muestraCargaTrabajo(" + sistema_id + ", " + tipo_asig + ", " + areacensal + ", '" + itemCarga.Usu_id + "', " + geo + ", 7, 3);\" class=\"btn btn-sm btn-block btn-primary \"><i class=\"fa fa-edit\"></i> Ver</button></td>");
                        }
                        else if (tipo_asig == 11)
                        {
                            sb.Append("<td><button id=\"btnVer_" + itemCarga.Usu_id.ToString() + "\" onclick=\"MarcaSeleccion('" + itemCarga.Usu_id.ToString() + "'); $('.contenedor-Asignaciones').empty(); muestraListaRecSector(" + sistema_id + ", " + tipo_asig + ", " + areacensal + ", '" + itemCarga.Usu_id + "', " + geo + ", 7);\" class=\"btn btn-sm btn-block btn-primary \"><i class=\"fa fa-edit\"></i> Ver</button></td>");
                        }
                        //else if (tipo_asig == 1 || tipo_asig == 2 || tipo_asig == 7 || tipo_asig == 8)
                        //{
                        //    sb.Append("<td><button id=\"btnVer_" + itemCarga.Usu_id.ToString() + "\" onclick=\"MarcaSeleccion('" + itemCarga.Usu_id.ToString() + "'); muestraCargaTrabajo(" + sistema_id + ", " + tipo_asig + ", " + areacensal + ", '" + itemCarga.Usu_id + "', " + geo + ", " + perfil_id + ");\" class=\"btn btn-sm btn-block btn-primary \"><i class=\"fa fa-edit\"></i> Ver</button></td>");
                        //}
                        //else if (tipo_asig == 4)
                        //{
                        //    sb.Append("<td><button id=\"btnVer_" + itemCarga.Usu_id.ToString() + "\" onclick=\"MarcaSeleccion('" + itemCarga.Usu_id.ToString() + "'); muestraListaRubros('" + itemCarga.Usu_id + "', " + tipo_asig + "); \" class=\"btn btn-sm btn-block btn-primary \"><i class=\"fa fa-edit\"></i> Ver</button></td>");
                        //}
                        //else if (tipo_asig == 5)
                        //{
                        //    sb.Append("<td><button id=\"btnVer_" + itemCarga.Usu_id.ToString() + "\" onclick=\"MarcaSeleccion('" + itemCarga.Usu_id.ToString() + "'); muestraCargaTrabajo(" + sistema_id + ", " + tipo_asig + ", " + areacensal + ", '" + itemCarga.Usu_id + "', " + geo + ", 18); \" class=\"btn btn-sm btn-block btn-primary \"><i class=\"fa fa-edit\"></i> Ver</button></td>");
                        //}
                        //else if (tipo_asig == 6)
                        //{
                        //    sb.Append("<td><button id=\"btnVer_" + itemCarga.Usu_id.ToString() + "\" onclick=\"MarcaSeleccion('" + itemCarga.Usu_id.ToString() + "'); muestraListaRecSector(" + sistema_id + ", " + tipo_asig + ", " + areacensal + ", '" + itemCarga.Usu_id + "', " + geo + ", 18); \" class=\"btn btn-sm btn-block btn-primary \"><i class=\"fa fa-edit\"></i> Ver</button></td>");
                        //}
                        sb.Append("</tr>");
                    }
                    sb.Append("</tbody></table>");
                    sb.Append(_methodCallMarcaSeleccion.CreaJQueryFunction());
                    _strHtml = sb.ToString();
                }
                else
                {
                    sb.Append("<div class=\"text-center\">No hay información para mostrar.</div>");
                    _strHtml = sb.ToString();
                }

                return _strHtml;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Permite listar usuarios asignados
        /// </summary>
        /// 
        public string ListaSegunRecSector(int sistema_id, int tipo_asig, int areacensal, string usu, int geo, int perfil_id)
        {
            //Genero función para recorrer seleccionados
            CallMethod _methodCallMarcaSeleccion = new CallMethod
            {
                Mc_nombre = "MarcaSeleccionRec(identificador)",
                Mc_contenido = "$('.filasRecSector').removeClass('alert alert-success');" +
                               "$('#tr_' + identificador).addClass('alert alert-success');"
            };

            try
            {
                StringBuilder sb = new StringBuilder();
                _strHtml = "";

                if (usu == null)
                {
                    usu = "1";
                }

                GesAsignacionesDAL gesAsignacionesDAL = new GesAsignacionesDAL();
                List<GesUsuarioBOL> listCarga = gesAsignacionesDAL.ListaSegunPerfil<GesUsuarioBOL>(sistema_id, tipo_asig, areacensal, usu, geo, perfil_id);

                //Obtengo listado Usuarios
                var listUsuPer = from m in listCarga
                                 orderby m.Usu_id
                                 select m;

                if (listUsuPer.Count() > 0)
                {

                    sb.Append("<table class=\"tabla-RecSec table table-hover text-center\">"); //table-striped
                    sb.Append("<thead><tr>");
                    sb.Append("<th>Nombre</th>");
                    sb.Append("<th>Rut</th>");
                    sb.Append("<th>E-mail</th>");
                    sb.Append("<th>Teléfono</th>");
                    sb.Append("<th></th>");
                    //sb.Append("<th></th>");
                    sb.Append("</tr></thead>");
                    sb.Append("<tbody>");
                    foreach (var itemCarga in listUsuPer)
                    {
                        sb.Append("<tr id=\"tr_" + itemCarga.Usu_id.ToString() + "\" class=\"filasRecSector\">");
                        sb.Append("<td>&nbsp" + itemCarga.Usu_nombre + "</td>");
                        sb.Append("<td>&nbsp" + itemCarga.Usu_rut + "</td>");
                        sb.Append("<td>&nbsp" + itemCarga.Usu_email + "</td>");
                        sb.Append("<td>&nbsp" + itemCarga.Usu_telefono + "</td>");
                        //if (tipo_asig != 6)
                        //{
                            sb.Append("<td><button id=\"btnVer_" + itemCarga.Usu_id.ToString() + "\" onclick=\"MarcaSeleccionRec('" + itemCarga.Usu_id.ToString() + "'); muestraCargaTrabajo(" + sistema_id + ", " + tipo_asig + ", " + areacensal + ", '" + itemCarga.Usu_id + "', " + geo + ", " + perfil_id + ");\" class=\"btn btn-sm btn-block btn-primary \"><i class=\"fa fa-edit\"></i> Ver</button></td>");
                            //sb.Append("<td><button id=\"btnDescargar_" + itemCarga.Usu_id.ToString() + "\" onclick=\"muestraGeneraDescarga('" + itemCarga.Usu_id.ToString() + "', 1);\" class=\"btn btn-block btn-success \"><i class=\"fa fa fa-download\"></i></button></td>");
                        //}
                        //else
                        //{
                        //    sb.Append("<td><button id=\"btnVer_" + itemCarga.Usu_id.ToString() + "\" onclick=\"MarcaSeleccionRec('" + itemCarga.Usu_id.ToString() + "'); muestraListaRubros('" + itemCarga.Usu_id + "', " + tipo_asig + ");\" class=\"btn btn-sm btn-block btn-primary \"><i class=\"fa fa-edit\"></i> Ver</button></td>");
                        //    sb.Append("<td><button id=\"btnDescargar_" + itemCarga.Usu_id.ToString() + "\" onclick=\"muestraGeneraDescarga('" + itemCarga.Usu_id.ToString() + "', 3);\" class=\"btn btn-block btn-success \"><i class=\"fa fa fa-download\"></i></button></td>");
                        //}
                        sb.Append("</tr>");
                    }
                    sb.Append("</tbody></table>");
                    sb.Append(_methodCallMarcaSeleccion.CreaJQueryFunction());
                    _strHtml = sb.ToString();
                }
                else
                {
                    sb.Append("<div class=\"text-center\">No hay información para mostrar.</div>");
                    _strHtml = sb.ToString();
                }

                return _strHtml;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Permite listar disponibles para asignacion
        /// </summary>
        public string ObtieneDisponibles(int sistema_id, int tipo_asig, int areacensal, string usu, int geo, int disponibles, int perfil_id, int nivel)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                _strHtml = "";

                GesAsignacionesDAL gesAsignacionesDAL = new GesAsignacionesDAL();

                DataTable DtAsigDisp = new DataTable();
                DataSet DsAsigDisp = new DataSet();
                DsAsigDisp = gesAsignacionesDAL.ListarAsignaciones(sistema_id, tipo_asig, areacensal, usu, geo, disponibles, perfil_id);

                //Recorro Nivel 1
                if (DsAsigDisp.Tables[0].Rows.Count > 0)
                {
                    DtAsigDisp = DsAsigDisp.Tables[0];
                    sb.Append("<table class=\"tabla-Asig table table-hover table-striped text-center\">");
                    sb.Append("<thead><tr>");
                    sb.Append("<th class=\"small\">#</th>");
                    //if (tipo_asig == 4 || tipo_asig == 6)
                    //{
                    //    sb.Append("<th class=\"small\">Rut</th>");
                    //    sb.Append("<th class=\"small\">Nombre Productor</th>");
                    //    sb.Append("<th class=\"small\">Superficie</th>");
                    //}
                    //else
                    //{
                    //    sb.Append("<th class=\"small\">Nombre</th>");
                    //}
                    sb.Append("<th class=\"small\">Nombre</th>");
                    if (tipo_asig == 11) {
                        sb.Append("<th class=\"small\"></th>");
                    }
                    sb.Append("</tr></thead>");
                    sb.Append("<tbody>");
                    for (int i = 0; i <= DtAsigDisp.Rows.Count - 1; i++)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td class=\"small\"><div class=\"icheckbox_square-green hover\" style=\"position: relative;\">");
                        //sb.Append("<label><input type=\"checkbox\" value=\"\"><span class=\"cr\"><i class=\"cr-icon icofont icofont-ui-check txt-primary\"></i></span></label>";
                        sb.Append("<input id=\"chk_" + DtAsigDisp.Rows[i]["Usu_id"] + "\" name=\"\" class=\"i-checksA asignados disp\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                        sb.Append("</div></td>");
                        //if (tipo_asig == 4 || tipo_asig == 6)
                        //{
                        //    sb.Append("<td class=\"small\">&nbsp" + DtAsigDisp.Rows[i]["Usu_id"] + "</td>");
                        //    sb.Append("<td class=\"small\">&nbsp" + DtAsigDisp.Rows[i]["Usu_nombre"] + "</td>");
                        //    sb.Append("<td class=\"small\">&nbsp" + DtAsigDisp.Rows[i]["directorio_superficie"] + "</td>");
                        //}
                        //else
                        //{
                        //    sb.Append("<td class=\"small\">&nbsp" + DtAsigDisp.Rows[i]["Usu_nombre"] + "</td>");
                        //}
                        sb.Append("<td class=\"small\">&nbsp" + DtAsigDisp.Rows[i]["Usu_nombre"] + "</td>");
                        if (tipo_asig == 11) {
                            sb.Append("<td class=\"small\"><button id=\"btnVer_" + DtAsigDisp.Rows[i]["Usu_id"] + "\" onclick=\"MarcaSeleccion('" + DtAsigDisp.Rows[i]["Usu_id"] + "'); muestraListaDireccionesALC('" + DtAsigDisp.Rows[i]["Usu_id"] + "','" + DtAsigDisp.Rows[i]["Usu_nombre"] + "');\" class=\"btn btn-sm btn-block btn-warning\" data-toggle=\"modal\" data-target=\"#modal-master\" ><i class=\"fa fa-search\"></i></button> </td>");
                        }
                        sb.Append("</tr>");
                    }
                    sb.Append("</tbody></table>");
                }
                else
                {
                    sb.Append("<div class=\"text-center\">No hay información para mostrar.</div>");
                }
                _strHtml = sb.ToString();
                return _strHtml;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Permite listar usuarios con asignacion
        /// </summary>
        public string ObtieneUsuariosAsignados(int sistema_id, int tipo_asig, int areacensal, string usu, int geo, int disponibles, int perfil_id, int nivel)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                _strHtml = "";

                GesAsignacionesDAL gesAsignacionesDAL = new GesAsignacionesDAL();
                //List<GesAsignacionesBOL> listCarga = gesAsignacionesDAL.ObtieneUsuariosAsignados<GesAsignacionesBOL>();

                DataTable DtAsigSelect = new DataTable();
                DataSet DsAsigSelect = new DataSet();
                DsAsigSelect = gesAsignacionesDAL.ListarAsignaciones(sistema_id, tipo_asig, areacensal, usu, geo, disponibles, perfil_id);

                //Recorro Nivel 1
                if (DsAsigSelect.Tables[0].Rows.Count > 0)
                {
                    DtAsigSelect = DsAsigSelect.Tables[0];
                    sb.Append("<table class=\"tabla-Asig table table-hover table-striped text-center\">");
                    sb.Append("<thead><tr>");
                    sb.Append("<th class=\"small\">#</th>");
                    //if (tipo_asig == 4 || tipo_asig == 6)
                    //{
                    //    sb.Append("<th class=\"small\">Rut</th>");
                    //    sb.Append("<th class=\"small\">Nombre Productor</th>");
                    //    sb.Append("<th class=\"small\">Superficie</th>");
                    //}
                    //else
                    //{
                    //    sb.Append("<th class=\"small\">Nombre</th>");
                    //}
                    sb.Append("<th class=\"small\">Nombre</th>");
                    if (tipo_asig == 11) {
                        sb.Append("<th class=\"small\"></th>");
                    }
                    sb.Append("</tr></thead>");
                    sb.Append("<tbody>");
                    for (int i = 0; i <= DtAsigSelect.Rows.Count - 1; i++)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td class=\"small\"><div class=\"icheckbox_square-green hover\" style=\"position: relative;\">");
                        //sb.Append("<label><input type=\"checkbox\" value=\"\"><span class=\"cr\"><i class=\"cr-icon icofont icofont-ui-check txt-primary\"></i></span></label>";
                        sb.Append("<input id=\"chk_" + DtAsigSelect.Rows[i]["Usu_id"] + "\" name=\"\" class=\"i-checksA asignados Asig\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                        sb.Append("</div></td>");
                        //if (tipo_asig == 4 || tipo_asig == 6)
                        //{
                        //    sb.Append("<td class=\"small\">&nbsp" + DtAsigSelect.Rows[i]["Usu_id"] + "</td>");
                        //    sb.Append("<td class=\"small\">&nbsp" + DtAsigSelect.Rows[i]["Usu_nombre"] + "</td>");
                        //    sb.Append("<td class=\"small\">&nbsp" + DtAsigSelect.Rows[i]["directorio_superficie"] + "</td>");
                        //}
                        //else
                        //{
                        //    sb.Append("<td class=\"small\">&nbsp" + DtAsigSelect.Rows[i]["Usu_nombre"] + "</td>");
                        //}
                        sb.Append("<td class=\"small\">&nbsp" + DtAsigSelect.Rows[i]["Usu_nombre"] + "</td>");
                        if (tipo_asig == 11) {
                            sb.Append("<td class=\"small\"><button id=\"btnVer_" + DtAsigSelect.Rows[i]["Usu_id"] + "\" onclick=\"MarcaSeleccion('" + DtAsigSelect.Rows[i]["Usu_id"] + "'); muestraListaDireccionesALC('" + DtAsigSelect.Rows[i]["Usu_id"] + "','" + DtAsigSelect.Rows[i]["Usu_nombre"] + "');\" class=\"btn btn-sm btn-block btn-primary\" data-toggle=\"modal\" data-target=\"#modal-master\" ><i class=\"fa fa-search\"></i></button> </td>");
                        }
                        sb.Append("</tr>");
                    }
                    sb.Append("</tbody></table>");
                }
                else
                {
                    sb.Append("<div class=\"text-center\">No hay información para mostrar.</div>");
                }
                _strHtml = sb.ToString();
                return _strHtml;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
   
        /// <summary>
        /// Registra asignaciones realizadas
        /// </summary>
        public string InsertarAsignaciones(int tipo_asig, int sistema_id, string usu, string areacensal, int nivelasig, int perfilasig)
        {
            GesAsignacionesDAL gesAsignacionesDAL = new GesAsignacionesDAL();
            string retorno = "";
            try
            {
                StringBuilder sb = new StringBuilder();
                _strHtml = "";
              
                string mensaje = "";
                string clase = "";

                DataSet DsAsigSelect = new DataSet();
                DsAsigSelect = gesAsignacionesDAL.InsertarAsignaciones(tipo_asig, sistema_id, usu, areacensal, nivelasig, perfilasig);

                retorno = DsAsigSelect.Tables[0].Rows[0][0].ToString();
                bool respuestaError = false;

                //Recorro Nivel 1
                if (retorno == "1")
                {
                    mensaje = "El usuario que se intenta asignar ya tiene otra asignación registrada.";
                    clase = "alert-danger";
                }
                else if (retorno == "2")
                {
                    mensaje = "No es posible realizar el proceso. Asegúrese que todos los niveles necesarios se encuentran asignados al usuario (macrosector, sector o local).";
                    clase = "alert-danger";
                }
                else if (retorno == "3")
                {
                    mensaje = "Solo puede existir un Coordinador para la clasificación geográfica seleccionada.";
                    clase = "alert-danger";
                }
                else if (retorno == "4")
                {
                    mensaje = "No es posible realizar proceso. Asegúrese que no existan otras asignaciones asociadas.";
                    clase = "alert-danger";
                }
                else if (retorno == "0" || retorno == "5")
                {
                    string mensajeError = string.Empty;
                    var parametros = ListParametros();

                    if(parametros.Count > 0)
                    {
                        switch (tipo_asig)
                        {
                            case 10:
                                bool On_Off_CreacionGrupos = Convert.ToBoolean(parametros.Where(x => x.Descripcion == "On_Off_CreacionGrupos").Select(x => x.Valor).FirstOrDefault());
                                if(On_Off_CreacionGrupos)
                                {
                                    string rutSupervisor = areacensal;
                                    string rutEntrevistador = usu;
                                    if (retorno == Convert.ToString((int)RetornoAsignacion.ASIGNADO))
                                    {
                                        var crearGrupo = CrearGrupoSuso(rutSupervisor, rutEntrevistador, sistema_id);

                                        respuestaError = crearGrupo.Item1;
                                        mensajeError = crearGrupo.Item2;
                                    }
                                    else
                                    {
                                        string RutUserDPaso = parametros.Where(x => x.Descripcion == "UserPasoAsignaciones").Select(x => x.Valor).FirstOrDefault();
                                        CrearUsuarioDePaso(sistema_id, RutUserDPaso);
                                        GesUsuarioBOL datosSupervisor = gesAsignacionesDAL.ObtieneDatosUsuarios<GesUsuarioBOL>(RutUserDPaso).FirstOrDefault();
                                        GesUsuarioBOL datosEntrevistador = gesAsignacionesDAL.ObtieneDatosUsuarios<GesUsuarioBOL>(rutEntrevistador).FirstOrDefault();

                                        var EditarCoordinador = ModificarCoordinadorGrupo(RutUserDPaso, rutEntrevistador);
                                        if (EditarCoordinador.Cabecera.CodigoRespuesta != 200)
                                        {
                                            respuestaError = true;
                                            mensajeError = "Problemas con la Asignación:" + EditarCoordinador.Cabecera.Mensaje;
                                        }
                                        var InsertaUsuarioSuso = new GesUsuarioDAL().InsertarUserSG(rutEntrevistador, datosEntrevistador.Usu_idSuso, Enumeracion.TipoUsuarios.Entrevistador.ToString(), datosSupervisor.Usu_idSuso);
                                    }
                                }
                                break;
                            case 11:
                                // carga de trabajo a censistas
                                bool On_Off_Asignaciones = Convert.ToBoolean(parametros.Where(x => x.Descripcion == "On_Off_Asignaciones").Select(x => x.Valor).FirstOrDefault());
                                if(On_Off_Asignaciones)
                                {
                                    int alc = Convert.ToInt32(usu);
                                    var CrearAsignacion = CrearAsignacionSuso(areacensal, alc, retorno);
                                    if (CrearAsignacion.Item1)
                                    {
                                        respuestaError = CrearAsignacion.Item1;
                                        mensajeError = CrearAsignacion.Item2;
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        respuestaError = true;
                    }
                   

                   
                    if (retorno == "0" && !respuestaError) {

                        mensaje = "Asignación realizada satisfactoriamente.";
                        clase = "alert-success";

                        if (tipo_asig == 3 || tipo_asig == 4 || tipo_asig == 5) {
                            if (tipo_asig == perfilasig) {
                                EnviarNotificacion(tipo_asig, usu, areacensal, perfilasig);
                            } else if (tipo_asig == 5 && perfilasig > 5 ) {
                                EnviarNotificacion(tipo_asig, usu, areacensal, perfilasig);
                            }
                        } else if (tipo_asig == 10 || tipo_asig == 11) {
                            EnviarNotificacion(tipo_asig, usu, areacensal, perfilasig);
                        }
                    }
                    else
                    {
                        if (respuestaError == true)
                        {
                            // si ocurre un problema con la generación supervisor o entrevistadores se devuelve al en sistema de gestion la asigfnación
                            DsAsigSelect = gesAsignacionesDAL.InsertarAsignaciones(tipo_asig, sistema_id, usu, areacensal, nivelasig, perfilasig);
                            mensaje = mensajeError;
                            clase = "alert-danger";
                        }
                        else
                        {
                            if (retorno == Convert.ToString((int)RetornoAsignacion.SINASIGNAR))
                            {
                                mensaje = "Asignación realizada satisfactoriamente.";
                                clase = "alert-success";
                            }

                        }
                    }
                }
                else 
                {
                    mensaje = "No fue posible realizar proceso debido a un error indeterminado.";
                    clase = "alert-danger";
                }

                sb.Append("<div class=\"ibox-content table-border-style\">");
                sb.Append("<div class=\"row col-lg-12 text-center\">");
                sb.Append("<div class=\"col-lg-12 alert " + clase + " alert-dismissable\">");
                sb.Append("<button aria-hidden=\"true\" data-dismiss=\"alert\" onclick=\"$('.conasignacion').empty();\" class=\"close\" type=\"button\">×</button>");
                sb.Append(mensaje);
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</div>");

                _strHtml = sb.ToString();

                return _strHtml;
            }
            catch (Exception ex)
            {
                if(retorno == "0" || retorno == "5")
                {
                    var dts = gesAsignacionesDAL.InsertarAsignaciones(tipo_asig, sistema_id, usu, areacensal, nivelasig, perfilasig);
                }
              
                throw new Exception(ex.Message);
            }
        }

        public void EnviarNotificacion(int tipo_asig, string asig_de, string asig_a, int perfil) {
            
            Email _email = new Email();
            GesAsignacionesDAL _gesAsignacionesDAL = new GesAsignacionesDAL();
            string rut_user = "";
            string mensaje = "";

            DataSet dsNotif = new DataSet();
            DataTable dtNotif = new DataTable();

            try
            {
                dsNotif = _gesAsignacionesDAL.ListaDatosUsuarioNotificacion(tipo_asig, asig_de, asig_a, perfil);

                if (dsNotif.Tables[0].Rows.Count > 0)
                {

                    dtNotif = dsNotif.Tables[0];

                    for (int i = 0; i <= dtNotif.Rows.Count - 1; i++)
                    {

                        GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL();
                        GesUsuarioBLL _gesUsuarioBLL = new GesUsuarioBLL();
                        GesSistemaBOL _gesSistemaBOL = new GesSistemaBOL
                        {
                            Sistema_id = 1,
                            Sistema_token = dtNotif.Rows[i]["Token"].ToString()
                        };

                        if (tipo_asig == 11)
                        {
                            rut_user = dtNotif.Rows[i]["IdAsignacionA"].ToString();
                            mensaje = "Área de Levantamiento Censal <b>" + dtNotif.Rows[i]["NombreALC"].ToString() + "</b> fue asignada a su usuario ";
                        }
                        else
                        {
                            rut_user = dtNotif.Rows[i]["IdAsignacionDe"].ToString();
                            if (tipo_asig == 3 || tipo_asig == 4 || tipo_asig == 5) {
                                mensaje = "<b>" + dtNotif.Rows[i]["NombreAreaCensal"].ToString() + "</b> fue asignado a su usuario.";
                            } else if (tipo_asig == 10) {
                                mensaje = "<b>" + dtNotif.Rows[i]["NombreUsuario"].ToString() + "</b> fue asignado como su Coordinador de Grupo Censal."; ;
                            }
                        }

                        _gesUsuarioBOL.Usu_rut = rut_user;
                        _gesUsuarioBOL = _gesUsuarioBLL.BuscarUsuario(_gesUsuarioBOL, "Usu_rut");

                        _email.NombreRemitente = "Censo 2023 - Sistema Gestión"; //_appSettings.ServidorWeb;
                        _email.From = _appSettings.CorreoSoporte;
                        _email.AgregarCorreoDestino(_gesUsuarioBOL.Usu_email);
                        _email.Asunto = "Notificación de Asignación";
                        _email.Html = "<p style=\"font-family: calibri; margin: 0px; color: #333;\">" +
                                            "Por medio de este correo se notifica de una nueva asignación realizada a su usuario en <strong>{proyecto-nombre}</strong>. " +
                                            "Los detalles son los siguientes:" +
                                            "<div style=\"width: auto; background-color: #ddd; margin-top: 20px; border-radius: 5px; font-family: calibri;  padding: 10px; text-align: center;\">" +
                                            mensaje +
                                            "</div>" +
                                        "</p>" ;
                        _email.Cuerpo = _email.EmailTemplate(_gesSistemaBOL, _gesUsuarioBOL);
                        _email.EstablecerFormatoDeCuerpoHtml();
                        _email.Prioridad = "High";
                        _email.EnviarCorreoAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Permite listar areas de levantamiento y coordinadores registrados para cada una
        /// </summary>
        public string ListaResumenAsignacion(string usuario, int geo, int perfil_id)
        {

            CallMethod _methodCallLoad = new CallMethod
            {
                Mc_contenido = "$('.estadoweb').on('change', function(){" +
                                    "var selected = $(this).val();" +
                                    "$('.contenedor-supervision').empty().html(muestraListaEmpresas('" + usuario + "', selected, selected));" +
                               "});"
            };

            //Genero función para recorrer seleccionados
            CallMethod _methodCallMarcaSeleccion = new CallMethod
            {
                Mc_nombre = "MarcaSeleccion(identificador)",
                Mc_contenido = "$('.filasPerfil').removeClass('alert alert-success');" +
                               "$('.filasPerfilNone').hide();" +
                               "$('#trNone_' + identificador).show();" +
                               "$('#tr_' + identificador).addClass('alert alert-success');"
            };

            try
            {
                StringBuilder sb = new StringBuilder();
                _strHtml = "";
                string _estado;
                string _nomCOM;
                string _nomCOS;
                string _nomCLC;
                string _nomCOR;
                //GesSupervisionDAL gesSupervisionDAL = new GesSupervisionDAL();
                GesAsignacionesDAL _gesAsignacionesDAL = new GesAsignacionesDAL();

                DataTable Dtweb = new DataTable();
                DataSet Dsweb = new DataSet();
                //Dsweb = gesSupervisionDAL.ListaEmpresas(usuario, estado, tipo);
                Dsweb = _gesAsignacionesDAL.ListaResumenAsignacionEquipos(usuario, geo, perfil_id,"0",1);


                if (Dsweb.Tables[0].Rows.Count > 0)
                {
                    Dtweb = Dsweb.Tables[0];

                    
                    if (!string.IsNullOrEmpty(Dtweb.Rows[0]["NombreCOR"].ToString())) {
                        _nomCOR = Dtweb.Rows[0]["NombreCOR"].ToString();
                    } else {
                        _nomCOR = "<span style=\"color:#e74c3c\"><i class=\"fa fa-exclamation-circle\"></i> No registrado</span>";
                    }

                    sb.Append("<div class=\"row col-lg-12 form group\" style=\"margin-top:20px!important;\"><h3>Coordinador Operativo Regional: " + _nomCOR + "</h3></div>");
                    sb.Append("<table class=\"tabla-Informantes table table-hover text-center\">"); //table-striped                                  
                    sb.Append("<thead><tr>");
                    sb.Append("<th class=\"\" style=\"color:#95a5a6\">MACROSECTORES</th>");
                    sb.Append("<th class=\"\" style=\"color:#95a5a6\">SECTORES</th>");
                    sb.Append("<th class=\"\" style=\"color:#95a5a6\">LOCALES CENSALES</th>");
                    sb.Append("<th class=\"\" style=\"color:#95a5a6\">EQUIPOS</th>");
                    sb.Append("<th></th>");
                    sb.Append("</tr></thead>");
                    sb.Append("<tbody>");
                    for (int i = 0; i <= Dtweb.Rows.Count - 1; i++)
                    {
                        //_estado = Dtweb.Rows[i]["estado_id"].ToString();
                        if (!string.IsNullOrEmpty(Dtweb.Rows[i]["NombreCOM"].ToString())) {
                            _nomCOM = Dtweb.Rows[i]["NombreCOM"].ToString();
                        } else {
                            _nomCOM = "<span style=\"color:#e74c3c\"><i class=\"fa fa-exclamation-circle\"></i> No registrado</span>";
                        }

                        if (!string.IsNullOrEmpty(Dtweb.Rows[i]["NombreCOS"].ToString())) {
                            _nomCOS = Dtweb.Rows[i]["NombreCOS"].ToString();
                        } else {
                            _nomCOS = "<span style=\"color:#e74c3c\"><i class=\"fa fa-exclamation-circle\"></i> No registrado</span>";
                        }

                        if (!string.IsNullOrEmpty(Dtweb.Rows[i]["NombreCLC"].ToString())) {
                            _nomCLC = Dtweb.Rows[i]["NombreCLC"].ToString();
                        } else {
                            _nomCLC = "<span style=\"color:#e74c3c\"><i class=\"fa fa-exclamation-circle\"></i> No registrado</span>";
                        }

                        string _cod = Dtweb.Rows[i]["IdMacrosector"].ToString() + Dtweb.Rows[i]["IdSector"].ToString() + Dtweb.Rows[i]["IdLocal"].ToString();

                        sb.Append("<tr id=\"tr_" + _cod + "\" class=\"filasPerfil\">");
                        sb.Append("<td class=\"\">&nbsp;<b>" + Dtweb.Rows[i]["NombreMacrosector"] + "</b><br />Coordinador(a): " + _nomCOM + "</td>");
                        sb.Append("<td class=\"\">&nbsp;<b>" + Dtweb.Rows[i]["NombreSector"] + "</b><br />Coordinador(a): " + _nomCOS + "</td>");
                        sb.Append("<td class=\"\">&nbsp;<b>" + Dtweb.Rows[i]["NombreLocal"] + "</b><br />Coordinador(a): " + _nomCLC + "</td>");
                        sb.Append("<td class=\"\">&nbsp;<b>" + Dtweb.Rows[i]["TotalCGC"] + "</b></td>");
                        sb.Append("<td class=\"\"><button id=\"btnVer_" + _cod + "\" onclick=\"MarcaSeleccion('" + _cod + "'); muestraListaResumenAsigEquipos('" + usuario + "'," + geo + "," + perfil_id + ",'" + Dtweb.Rows[i]["IdLocal"].ToString() + "');\" class=\"btn btn-sm btn-block btn-primary\" data-toggle=\"modal\" data-target=\"#modal-master\" ><i class=\"fa fa-edit\"></i> Ver Equipos</button></td>");
                        //sb.Append("<td class=\"\"><button id=\"btnVer_" + Dtweb.Rows[i]["IdMacrosector"] + "\" onclick=\"MarcaSeleccion('" + Dtweb.Rows[i]["IdMacrosector"] + "'); muestraWebForm('" + Dtweb.Rows[i]["IdMacrosector"] + "');\" class=\"btn btn-sm btn-block btn-primary\" data-toggle=\"modal\" data-target=\"#modal-master\" ><i class=\"fa fa-edit\"></i> Ver Equipos</button></td>");
                        //if (_estado == "3")
                        //    sb.Append("<td class=\"small\"><button id=\"btnMostrar_" + Dtweb.Rows[i]["Rut"] + "\" onclick=\"muestraFormEstado('" + Dtweb.Rows[i]["Rut"] + "', 99,99);\" type=\"button\" data-toggle=\"modal\" data-target=\"#modal-master\" class=\"btn btn-block btn-success\"><i class=\"fa fa-search\"></i> Abrir Cuestionarios</button></td>");
                        //}else if (_estado == "4")
                        //    sb.Append("<td class=\"small\"><button id=\"btnMostrar_" + Dtweb.Rows[i]["Rut"] + "\" onclick=\"muestraFormEstado('" + Dtweb.Rows[i]["Rut"] + "', 99,99);\" type=\"button\" data-toggle=\"modal\" data-target=\"#modal-master\" class=\"btn btn-block btn-warning\"><i class=\"fa fa-edit\"></i> Editar</button></td>");
                        //else if (_estado == "5")
                        //sb.Append("<td class=\"small\"><button id=\"btnMostrar_" + Dtweb.Rows[i]["IdMacrosector"] + "\" onclick=\"muestraFormEstado('" + Dtweb.Rows[i]["IdMacrosector"] + "', 100,100);\" type=\"button\" data-toggle=\"modal\" data-target=\"#modal-master\" class=\"btn btn-block btn-danger\"><i class=\"fa fa-edit\"></i> Editar Cuestionarios</button></td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("</tbody></table>");
                    sb.Append(_methodCallLoad.CreaJQueryDocumentReady());
                    sb.Append(_methodCallMarcaSeleccion.CreaJQueryFunction());
                    _strHtml = sb.ToString();
                }
                else
                {
                    sb.Append("<div class=\"text-center\">No hay información para mostrar.</div>");
                    _strHtml = sb.ToString();
                }

                return _strHtml;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Permite listar areas de levantamiento y coordinadores registrados para cada una
        /// </summary>
        public string ListaResumenAsignacionEquipos(string usuario, int geo, int perfil_id, string local_id)
        {

            CallMethod _methodCallLoad = new CallMethod
            {
                Mc_contenido = "$('.estadoweb').on('change', function(){" +
                                    "var selected = $(this).val();" +
                                    "$('.contenedor-supervision').empty().html(muestraListaEmpresas('" + usuario + "', selected, selected));" +
                               "});"
            };

            ////Genero función para recorrer seleccionados
            //CallMethod _methodCallMarcaSeleccion = new CallMethod
            //{
            //    Mc_nombre = "MarcaSeleccion(identificador)",
            //    Mc_contenido = "$('.filasPerfil').removeClass('alert alert-success');" +
            //                   "$('.filasPerfilNone').hide();" +
            //                   "$('#trNone_' + identificador).show();" +
            //                   "$('#tr_' + identificador).addClass('alert alert-success');"
            //};

            try
            {
                StringBuilder sb = new StringBuilder();
                _strHtml = "";
                //GesSupervisionDAL gesSupervisionDAL = new GesSupervisionDAL();
                GesAsignacionesDAL _gesAsignacionesDAL = new GesAsignacionesDAL();

                DataTable Dtequipo = new DataTable();
                DataSet Dsequipo = new DataSet();
                //Dsweb = gesSupervisionDAL.ListaEmpresas(usuario, estado, tipo);
                Dsequipo = _gesAsignacionesDAL.ListaResumenAsignacionEquipos(usuario, geo, perfil_id,local_id,2);


                if (Dsequipo.Tables[0].Rows.Count > 0)
                {
                    Dtequipo = Dsequipo.Tables[0];

                    string sin_censistas = "<span style=\"color:#e74c3c\"><i class=\"fa fa-exclamation-circle\"></i> Sin censistas asignados</span>";

                    sb.Append("<table class=\"tabla-Equipos table table-hover text-center\">"); //table-striped                                  
                    sb.Append("<thead><tr>");
                    sb.Append("<th class=\"\" style=\"color:#95a5a6\">COORDINADOR GRUPO CENSAL</th>");
                    sb.Append("<th class=\"\" style=\"color:#95a5a6\">CENSISTA</th>");
                    //sb.Append("<th class=\"\" style=\"color:#95a5a6\">LOCALES CENSALES</th>");
                    //sb.Append("<th class=\"\" style=\"color:#95a5a6\">EQUIPOS</th>");
                    //sb.Append("<th></th>");
                    sb.Append("</tr></thead>");
                    sb.Append("<tbody>");
                    for (int i = 0; i <= Dtequipo.Rows.Count - 1; i++)
                    {
                        //_estado = Dtweb.Rows[i]["estado_id"].ToString();

                        sb.Append("<tr class=\"filasPerfil\">");
                        sb.Append("<td class=\"\">&nbsp;" + Dtequipo.Rows[i]["NombreSupervisor"] + "</td>");
                        if (Dtequipo.Rows[i]["NombreCensista"].ToString().Equals("SIN_CENSISTAS")) {
                            sb.Append("<td class=\"\">&nbsp;" + sin_censistas + "</td>");
                        } else {
                            sb.Append("<td class=\"\">&nbsp;" + Dtequipo.Rows[i]["NombreCensista"] + "</td>");
                        }
                        //sb.Append("<td class=\"\">&nbsp;<b>" + Dtequipo.Rows[i]["NombreLocal"] + "</b><br />Coordinador(a): " + _nomCLC + "</td>");
                        //sb.Append("<td class=\"\">&nbsp;<b>" + Dtequipo.Rows[i]["TotalCGC"] + "</b></td>");
                        //sb.Append("<td class=\"\"><button id=\"btnVer_" + _cod + "\" onclick=\"MarcaSeleccion('" + _cod + "'); muestraListaResumenAsigEquipos('" + usuario + "'," + geo + "," + perfil_id + ",'" + Dtweb.Rows[i]["IdLocal"].ToString() + "');\" class=\"btn btn-sm btn-block btn-primary\" data-toggle=\"modal\" data-target=\"#modal-master\" ><i class=\"fa fa-edit\"></i> Ver Equipos</button></td>");
                        //sb.Append("<td class=\"\"><button id=\"btnVer_" + Dtweb.Rows[i]["IdMacrosector"] + "\" onclick=\"MarcaSeleccion('" + Dtweb.Rows[i]["IdMacrosector"] + "'); muestraWebForm('" + Dtweb.Rows[i]["IdMacrosector"] + "');\" class=\"btn btn-sm btn-block btn-primary\" data-toggle=\"modal\" data-target=\"#modal-master\" ><i class=\"fa fa-edit\"></i> Ver Equipos</button></td>");
                        //if (_estado == "3")
                        //    sb.Append("<td class=\"small\"><button id=\"btnMostrar_" + Dtweb.Rows[i]["Rut"] + "\" onclick=\"muestraFormEstado('" + Dtweb.Rows[i]["Rut"] + "', 99,99);\" type=\"button\" data-toggle=\"modal\" data-target=\"#modal-master\" class=\"btn btn-block btn-success\"><i class=\"fa fa-search\"></i> Abrir Cuestionarios</button></td>");
                        //}else if (_estado == "4")
                        //    sb.Append("<td class=\"small\"><button id=\"btnMostrar_" + Dtweb.Rows[i]["Rut"] + "\" onclick=\"muestraFormEstado('" + Dtweb.Rows[i]["Rut"] + "', 99,99);\" type=\"button\" data-toggle=\"modal\" data-target=\"#modal-master\" class=\"btn btn-block btn-warning\"><i class=\"fa fa-edit\"></i> Editar</button></td>");
                        //else if (_estado == "5")
                        //sb.Append("<td class=\"small\"><button id=\"btnMostrar_" + Dtweb.Rows[i]["IdMacrosector"] + "\" onclick=\"muestraFormEstado('" + Dtweb.Rows[i]["IdMacrosector"] + "', 100,100);\" type=\"button\" data-toggle=\"modal\" data-target=\"#modal-master\" class=\"btn btn-block btn-danger\"><i class=\"fa fa-edit\"></i> Editar Cuestionarios</button></td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("</tbody></table>");
                    sb.Append(_methodCallLoad.CreaJQueryDocumentReady());
                    //sb.Append(_methodCallMarcaSeleccion.CreaJQueryFunction());
                    _strHtml = sb.ToString();
                }
                else
                {
                    sb.Append("<div class=\"text-center\">No hay información para mostrar.</div>");
                    _strHtml = sb.ToString();
                }

                return _strHtml;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Permite listar areas de levantamiento y coordinadores registrados para cada una
        /// </summary>
        public string ListaDireccionesALC(string alc_id)
        {

            CallMethod _methodCallLoad = new CallMethod
            {
                Mc_contenido = "$('.estadoweb').on('change', function(){" +
                                    "var selected = $(this).val();" +
                                    "$('.contenedor-supervision').empty().html(muestraListaEmpresas('1', selected, selected));" +
                               "});"
            };

            try
            {
                StringBuilder sb = new StringBuilder();
                _strHtml = "";
                //GesSupervisionDAL gesSupervisionDAL = new GesSupervisionDAL();
                GesAsignacionesDAL _gesAsignacionesDAL = new GesAsignacionesDAL();

                DataTable Dtdirecciones = new DataTable();
                DataSet Dsdirecciones = new DataSet();
                //Dsweb = gesSupervisionDAL.ListaEmpresas(usuario, estado, tipo);
                Dsdirecciones = _gesAsignacionesDAL.ListaDireccionesALC(alc_id);


                if (Dsdirecciones.Tables[0].Rows.Count > 0)
                {
                    Dtdirecciones = Dsdirecciones.Tables[0];

                    sb.Append("<table class=\"tabla-Direcciones table table-hover text-center\">"); //table-striped                                  
                    sb.Append("<thead><tr>");
                    //sb.Append("<th class=\"\" style=\"color:#95a5a6\">MANZENT</th>");
                    sb.Append("<th class=\"small\" style=\"color:#95a5a6\">UBICACIÓN</th>");
                    sb.Append("<th class=\"small\" style=\"color:#95a5a6\">TIPO VÍA</th>");
                    sb.Append("<th class=\"small\" style=\"color:#95a5a6\">NOMBRE VÍA</th>");
                    sb.Append("<th class=\"small\" style=\"color:#95a5a6\">NÚMERO DOMICILIARIO</th>");
                    sb.Append("<th class=\"small\" style=\"color:#95a5a6\">NÚM. SITIO, PARCELA, PREDIO, LOTEO Y/O HIJUELA</th>");
                    sb.Append("<th class=\"small\" style=\"color:#95a5a6\">NÚM. O LETRA CASA, BLOCK, TORRE U OTRO</th>");
                    sb.Append("<th class=\"small\" style=\"color:#95a5a6\">NÚMERO PISO</th>");
                    sb.Append("<th class=\"small\" style=\"color:#95a5a6\">NÚM. O LETRA DEPTO.</th>");
                    sb.Append("<th class=\"small\" style=\"color:#95a5a6\">USO O DESTINO EDIFICACIÓN</th>");
                    sb.Append("<th class=\"small\" style=\"color:#95a5a6\">DESC. Y/O ANOTACIONES GENERALES</th>");
                    sb.Append("<th class=\"small\" style=\"color:#95a5a6\">TIPO DIRECCIÓN</th>");
                    //sb.Append("<th></th>");
                    sb.Append("</tr></thead>");
                    sb.Append("<tbody>");
                    for (int i = 0; i <= Dtdirecciones.Rows.Count - 1; i++)
                    {
                        //_estado = Dtweb.Rows[i]["estado_id"].ToString();

                        sb.Append("<tr class=\"filasPerfil\">");
                        //sb.Append("<td class=\"\">&nbsp;" + Dtdirecciones.Rows[i]["MANZENT"] + "</td>");
                        sb.Append("<td class=\"small\">&nbsp;" + Dtdirecciones.Rows[i]["UbicacionGeografica"] + "</td>");
                        sb.Append("<td class=\"small\">&nbsp;" + Dtdirecciones.Rows[i]["TipoVia"] + "</td>");
                        sb.Append("<td class=\"small\">&nbsp;" + Dtdirecciones.Rows[i]["NombreVia"] + "</td>");
                        sb.Append("<td class=\"small\">&nbsp;" + Dtdirecciones.Rows[i]["NumeroDomiciliario"] + "</td>");
                        sb.Append("<td class=\"small\">&nbsp;" + Dtdirecciones.Rows[i]["NumeroTerreno"] + "</td>");
                        sb.Append("<td class=\"small\">&nbsp;" + Dtdirecciones.Rows[i]["NumeroCasa"] + "</td>");
                        sb.Append("<td class=\"small\">&nbsp;" + Dtdirecciones.Rows[i]["NumeroPiso"] + "</td>");
                        sb.Append("<td class=\"small\">&nbsp;" + Dtdirecciones.Rows[i]["NumeroDepto"] + "</td>");
                        sb.Append("<td class=\"small\">&nbsp;" + Dtdirecciones.Rows[i]["UsoDestino"] + "</td>");
                        sb.Append("<td class=\"small\">&nbsp;" + Dtdirecciones.Rows[i]["Descripcion"] + "</td>");
                        sb.Append("<td class=\"small\">&nbsp;" + Dtdirecciones.Rows[i]["TipoDireccion"] + "</td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("</tbody></table>");
                    sb.Append(_methodCallLoad.CreaJQueryDocumentReady());
                    //sb.Append(_methodCallMarcaSeleccion.CreaJQueryFunction());
                    _strHtml = sb.ToString();
                }
                else
                {
                    sb.Append("<div class=\"text-center\">No hay información para mostrar.</div>");
                    _strHtml = sb.ToString();
                }

                return _strHtml;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Crear grupo de trabajo
        /// </summary>
        public Tuple<bool, string> CrearGrupoSuso(string rutSupervisor, string rutEntrevistador, int sistema_id)
        {
            GesAsignacionesDAL gesAsignacionesDAL = new GesAsignacionesDAL();
            bool flag = false;
            bool respuestaError = false;
            string mensajeError = string.Empty;

            try
            {
                string SupervisorId = string.Empty;
      
                GesUsuarioBOL datosSupervisor = gesAsignacionesDAL.ObtieneDatosUsuarios<GesUsuarioBOL>(rutSupervisor).FirstOrDefault();
                GesUsuarioBOL datosEntrevistador = gesAsignacionesDAL.ObtieneDatosUsuarios<GesUsuarioBOL>(rutEntrevistador).FirstOrDefault();

                //crea Supervisor SuSo
                if (datosSupervisor != null && string.IsNullOrEmpty(datosSupervisor.Usu_idSuso))
                {
                    var crearSupervisor = CrearUsuarioSuso(datosSupervisor, Role.SUPERVISOR);
                    if (crearSupervisor.Cabecera.CodigoRespuesta == 200)
                    {
                        var UserId = crearSupervisor.Usuario.PostCrearUsuario.UserId;
                        SupervisorId = UserId;
                        var InsertaUsuarioSuso = new GesUsuarioDAL().InsertarUserSG(rutSupervisor, UserId, Enumeracion.TipoUsuarios.Supervisor.ToString(), null);
                    }
                    else
                    {
                        flag = true;
                    }
                }

                GesUsuarioBOL idSuSo = gesAsignacionesDAL.ObtieneDatosUsuarios<GesUsuarioBOL>(rutSupervisor).FirstOrDefault();
                SupervisorId = idSuSo.Usu_idSuso;

                if (flag && string.IsNullOrEmpty(idSuSo.Usu_idSuso))
                {
                    respuestaError = true;
                    mensajeError = "Problemas al crear supervisor en Survey Solutions.";
                }

                SupervisorId = string.IsNullOrEmpty(SupervisorId) == true ? datosSupervisor.Usu_idSuso : SupervisorId;

                //crea entrevistador SuSo
                if (datosEntrevistador != null && string.IsNullOrEmpty(datosEntrevistador.Usu_idSuso) && !respuestaError)
                {

                    var crearEntrevistador = CrearUsuarioSuso(datosEntrevistador, Role.INTERVIEWER, rutSupervisor);
                    if (crearEntrevistador.Cabecera.CodigoRespuesta == 200)
                    {
                        var UserId = crearEntrevistador.Usuario.PostCrearUsuario.UserId;
                        // EntrevistadorId = UserId;
                        var InsertaUsuarioSuso = new GesUsuarioDAL().InsertarUserSG(rutEntrevistador, UserId, Enumeracion.TipoUsuarios.Entrevistador.ToString(), SupervisorId);
                    }
                    else
                    {
                        respuestaError = true;
                        mensajeError = "Problemas al crear entrevistador en Survey Solutions.";
                    }

                }
               
                if (!respuestaError && !string.IsNullOrEmpty(SupervisorId) && !string.IsNullOrEmpty(datosEntrevistador.Usu_idSuso))
                {
                    //reasigna usuario
                    var EditarCoordinador = ModificarCoordinadorGrupo(rutSupervisor, rutEntrevistador);
                    if (EditarCoordinador.Cabecera.CodigoRespuesta == 200)
                    {
                        var ModificarSupervisorGestion = new GesUsuarioDAL().InsertarUserSG(rutEntrevistador, datosEntrevistador.Usu_idSuso, Enumeracion.TipoUsuarios.Entrevistador.ToString(), SupervisorId);
                    }
                    else
                    {
                        respuestaError = true;
                        mensajeError = "Problemas al modificar Coordinador de Grupo:" + EditarCoordinador.Error.Mensaje;
                    }
                }
                return new Tuple<bool, string>(respuestaError, mensajeError);
            }
            catch (Exception ex)
            {
                respuestaError = true;
                mensajeError = "Problemas con Survey Solutions:" + ex.Message;
                return new Tuple<bool, string>(respuestaError, mensajeError);
            }

        }
        /// <summary>
        /// Modifica coordinador de grupo
        /// </summary>
        public Resultado ModificarCoordinadorGrupo(string rutSupervisor, string rutEntrevistador)
        {
            Resultado response = new Resultado();
            try
            {
                response = new UsuariosSS().ModificarSupervisor(new ModificarSupervisorEnt { UserNameSupervisor = rutSupervisor.Replace("-", ""), UserNameEntrevistador = rutEntrevistador.Replace("-", "") });

                return response;
            }
            catch (Exception ex)
            {
                Cabecera cabecera = new Cabecera();
                cabecera.CodigoRespuesta = 501;
                cabecera.Mensaje = "Error de servicio";
                response.Cabecera = cabecera;

                DatoSSINE.VO.SurveySolution.General.Error error = new DatoSSINE.VO.SurveySolution.General.Error();
                error.CodigoError = 501;
                error.Mensaje = ex.Message;
                response.Error = error;
                return response;
            }
        }
        /// <summary>
        /// Crea usuario Supervidor y entrevistador
        /// </summary>
        /// <returns></returns>
        public Resultado CrearUsuarioSuso(GesUsuarioBOL datosUsuario, Role role, string rutSupervisor = null)
        {
            Resultado response = new Resultado();
            try
            {
                response = new UsuariosSS().CrearUsuarios(datosUsuario, role, rutSupervisor);

                return response;
            }
            catch (Exception ex)
            {
                Cabecera cabecera = new Cabecera();
                cabecera.CodigoRespuesta = 501;
                cabecera.Mensaje = "Error de servicio";
                response.Cabecera = cabecera;

                DatoSSINE.VO.SurveySolution.General.Error error = new DatoSSINE.VO.SurveySolution.General.Error();
                error.CodigoError = 501;
                error.Mensaje = ex.Message;
                response.Error = error;
                return response;
            }
        }
        /// <summary>
        /// Crea Usuario de Paso
        /// </summary>
        public void CrearUsuarioDePaso(int sistema_id, string RutUserDPaso)
        {

            try
            {
                var parametros = ListParametros();
                var UsuarioDePaso = new GesUsuarioDAL().GetUsuarioSuSo(RutUserDPaso);
                if (UsuarioDePaso.Tables[0].Rows.Count == 0)
                {
                    //crea Supervisor SuSo
                    GesUsuarioBOL usr = new GesUsuarioBOL();
                    usr.Usu_nombre = parametros.Where(x => x.Descripcion == "NombreUsuarioPaso").Select(x => x.Valor).FirstOrDefault();
                    usr.Usu_rut = RutUserDPaso.Replace("-", "");
                    usr.Usu_email = parametros.Where(x => x.Descripcion == "EmailUserPaso").Select(x => x.Valor).FirstOrDefault();
                    usr.Usu_contrasena = parametros.Where(x => x.Descripcion == "PasswordUserPaso").Select(x => x.Valor).FirstOrDefault();
                    usr.Usu_telefono = parametros.Where(x => x.Descripcion == "TelefonoUserPaso").Select(x => x.Valor).FirstOrDefault();
                    var CreacionUsuario = CrearUsuarioSuso(usr, Role.SUPERVISOR);

                    if (CreacionUsuario.Cabecera.CodigoRespuesta == 200)
                    {
                        var UserId = CreacionUsuario.Usuario.PostCrearUsuario.UserId;
                        var InsertaUsuarioSuso = new GesUsuarioDAL().InsertarUserSG(RutUserDPaso, UserId, Enumeracion.TipoUsuarios.Supervisor.ToString(), null);
                    }

                }
            }
            catch (Exception ex)
            {
            
            }
        }
        /// <summary>
        /// Reasigna
        /// <returns></returns>
        public Tuple<bool, string> Reasignar(string rutResponsable, int alc)
        {
            List<GesReasignacionSS> Reasignacion = new List<GesReasignacionSS>();
            GesReasignacionesDAL gesReAsignacionesDAL = new GesReasignacionesDAL();

            try
            {
                DataSet listAsignaciones = gesReAsignacionesDAL.ListaAsignaciones(alc);
                if (listAsignaciones.Tables[0].Rows.Count > 0)
                {
                    Task task1 = Task.Factory.StartNew(() =>
                    {
                        foreach (DataRow dr in listAsignaciones.Tables[0].Rows)
                        {
                            GesReasignacionSS reaSig = new GesReasignacionSS();
                            GetAssignmentsEnt datos = new GetAssignmentsEnt()
                            {
                                Id_Asignacion = Convert.ToInt32(dr["IdAsignacionSuso"].ToString()),
                                Usuario_Responsable = rutResponsable.Replace("-", "")

                            };
                            reaSig.Reasignacion = datos;
                            //reaSig.ALC = Convert.ToInt32(dr["ALC"].ToString());
                            //reaSig.Direccion = 0;// Convert.ToInt32(dr["IdDireccion"].ToString());
                            Reasignacion.Add(reaSig);
                        }

                    });
                    Task.WaitAll(task1);

                    var FReasignacion = Reasignacion.Where(x => x.Reasignacion.Id_Asignacion != null).ToList();
                    var reasignar = new AsignacionesSS().ReAsignacion(FReasignacion, rutResponsable);
                    if (reasignar.Cabecera.CodigoRespuesta != 200)
                    {
                        return new Tuple<bool, string>(true, "Problemas con la carga de trabajo en Survey Solutions.");
                    }
                }
                return new Tuple<bool, string>(false, "");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(true, "Problemas con la carga de trabajo en Survey Solutions:" + ex.Message);
            }
        }
        public Tuple<bool, string> CrearAsignacionSuso(string rutResponsable, int alc,string CodigoRetorno)
        {
            GesAsignacionesDAL gesAsignacionesDAL = new GesAsignacionesDAL();
            try
            {
                DataSet Dsdirecciones = gesAsignacionesDAL.ListaDireccionesALC(alc.ToString());

                if (CodigoRetorno == Convert.ToString((int)RetornoAsignacion.ASIGNADO))
                {
                    GesUsuarioBOL InfoEntrevistador = gesAsignacionesDAL.ObtieneDatosUsuarios<GesUsuarioBOL>(rutResponsable, alc).FirstOrDefault();
                    if (InfoEntrevistador.Existe_asignacion == 0)
                    {
                        var ConformacionCuestionario = DatosPrecargadosVivConocida(Dsdirecciones.Tables[0].Rows[0].ItemArray[1].ToString(), rutResponsable);
                        var ConformacionCuestionarioVivNueva = DatosPrecargadosVivNueva(Dsdirecciones.Tables[0].Rows[0].ItemArray[1].ToString(), rutResponsable);
                        var ConFormacionCuestionarioArea = DatosPrecargadosArea(Dsdirecciones.Tables[0].Rows[0].ItemArray[1].ToString(), rutResponsable);

                        var AsignacionVivConocida = new AsignacionesSS().NuevaAsignacion(ConformacionCuestionario, rutResponsable, alc, Convert.ToInt64(Dsdirecciones.Tables[0].Rows[0].ItemArray[1].ToString()),1);
                        var AsignacionVivNuevaa = new AsignacionesSS().NuevaAsignacion(ConformacionCuestionarioVivNueva, rutResponsable, alc, Convert.ToInt64(Dsdirecciones.Tables[0].Rows[0].ItemArray[1].ToString()),2);
                        var AsignacionArea = new AsignacionesSS().NuevaAsignacion(ConFormacionCuestionarioArea, rutResponsable, alc, Convert.ToInt64(Dsdirecciones.Tables[0].Rows[0].ItemArray[1].ToString()),3);

                        if (AsignacionVivConocida.Cabecera.CodigoRespuesta != 200 )
                        {                            
                            return new Tuple<bool, string>(true, "Problemas con la carga de trabajo en Survey Solutions.");
                        }
                        if (AsignacionVivNuevaa.Cabecera.CodigoRespuesta != 200)
                        {
                            //reasignar Usuario Default
                            return new Tuple<bool, string>(true, "Problemas con la carga de trabajo en Survey Solutions.");
                        }
                        if (AsignacionArea.Cabecera.CodigoRespuesta != 200)
                        {
                            //reasignar Usuario Default
                            return new Tuple<bool, string>(true, "Problemas con la carga de trabajo en Survey Solutions.");
                        }
                    }
                    else
                    {
                        CrearUsuarioDePaso(1, rutResponsable);

                        var ResultReasignar = Reasignar(rutResponsable, alc);

                        if (ResultReasignar.Item1)
                        {
                            return new Tuple<bool, string>(ResultReasignar.Item1, ResultReasignar.Item2);
                        }
                    }
                }
                else
                {
                    //Se asignan las cargas al usuario Default de asignaciones
                    var parametros = ListParametros();
                    string RutUserDPaso = parametros.Where(x => x.Descripcion == "UserPasoAsignaciones").Select(x => x.Valor).FirstOrDefault();
                    CrearUsuarioDePaso(1, RutUserDPaso);

                    var ResultReasignar = Reasignar(RutUserDPaso, alc);

                    if (ResultReasignar.Item1)
                    {
                        return new Tuple<bool, string>(ResultReasignar.Item1, ResultReasignar.Item2);
                    }
                }

             return new Tuple<bool, string>(false, "");
            }
            catch(Exception ex)
            {
                return new Tuple<bool, string>(true, ex.Message);
            }
        }
        public Tuple<IList<IdentifyingData>> DatosPortadaVivConocida(List<GesDireccionGeo> Direcciones)
        {
            IList<IdentifyingData> IdentifyingData = new List<IdentifyingData>();
            try
            {
                
                IdentifyingData.Add(new IdentifyingData { Identity = string.Empty, Variable = "port_df_manzana", Answer = Direcciones.First().port_df_manzana });
                IdentifyingData.Add(new IdentifyingData { Identity = string.Empty, Variable = "port_df_dir_princ", Answer = Direcciones.First().calle + " " + Direcciones.First().altura });
                IdentifyingData.Add(new IdentifyingData { Identity = string.Empty, Variable = "cod_alc", Answer = Direcciones.First().cod_alc });
                IdentifyingData.Add(new IdentifyingData { Identity = string.Empty, Variable = "idg_comuna", Answer = Direcciones.First().idg_comuna });
                IdentifyingData.Add(new IdentifyingData { Identity = string.Empty, Variable = "df_id_dirprin", Answer = Direcciones.First().df_id_dirprin.ToString() });
                IdentifyingData.Add(new IdentifyingData { Identity = string.Empty, Variable = "df_manzana", Answer = Direcciones.First().df_manzana.ToString() });
                IdentifyingData.Add(new IdentifyingData { Identity = string.Empty, Variable = "calle", Answer = Direcciones.First().calle });
                IdentifyingData.Add(new IdentifyingData { Identity = string.Empty, Variable = "altura", Answer = Direcciones.First().altura });
                IdentifyingData.Add(new IdentifyingData { Identity = string.Empty, Variable = "indice_dirpri", Answer = Direcciones.Where(x=> x.indice_dirpri == 0).First().indice_dirpri.ToString()});
                return new Tuple<IList<IdentifyingData>>(IdentifyingData);
            }
            catch (Exception ex)
            {
              return new Tuple<IList<IdentifyingData>>(new List<IdentifyingData>());
            }
        }
        public List<GesNuevaAsignacionSS> DatosPrecargadosVivConocida(string codAlc,string rutResponsable)
        {
            GesAsignacionesDAL gesAsignacionesDAL = new GesAsignacionesDAL();
            List<GesNuevaAsignacionSS> DatosAsignacion = new List<GesNuevaAsignacionSS>();
            IList<IdentifyingData> IdentifyingData = new List<IdentifyingData>();
            try
            {
                var parametros = ListParametros();
                List<GesDireccionGeo> Direcciones = gesAsignacionesDAL.ObtieneDireccionesGeo<GesDireccionGeo>(codAlc);
             
                if (Direcciones.Count == 0) { return DatosAsignacion; }

                IdentifyingData = DatosPortadaVivConocida(Direcciones).Item1;
                var cantManzana = (from x in Direcciones select x.port_df_manzana).Distinct().ToList();
                IdentifyingData.Add(new IdentifyingData { Identity = string.Empty, Variable = "dirs_cant_manzanas", Answer = cantManzana.Count().ToString() });
                int index = 0;

                foreach (var manz in cantManzana)
                {
                    IdentifyingData.Add(new IdentifyingData { Identity = "ab64760775c248ef8e49af82c9ad2cdd_" + index, Variable = "dirs_manzana_nom", Answer = manz });
                    IdentifyingData.Add(new IdentifyingData { Identity = "ed189a6afb3e4bc4a809789e02cb495a_" + index, Variable = "id_manzana", Answer = index.ToString() });
                    
                    var calleManzana = Direcciones.Where(x => x.port_df_manzana == manz).ToList();
                    var cantCalles = (from x in calleManzana select x.calle).Distinct().ToList();
                    IdentifyingData.Add(new IdentifyingData { Identity = string.Empty, Variable = "dirs_cant_calles", Answer = cantCalles.Count().ToString() });
                    int IdxCalle = 0;
                    foreach (var Calles in cantCalles)
                    {
                        IdentifyingData.Add(new IdentifyingData { Identity = "5210d0d2953247b88c734cdba61e0e85_" + IdxCalle, Variable = "dirs_calle_nom", Answer = Calles });
                        IdentifyingData.Add(new IdentifyingData { Identity = "a5b32b9c75a7485592aa75c83197a4c4_" + IdxCalle, Variable = "dirs_manzana_idx", Answer = index.ToString() });
                        IdentifyingData.Add(new IdentifyingData { Identity = "aee587dd48dd49ae9fcfba1b37623a07_" + IdxCalle, Variable = "dirs_calleid", Answer = IdxCalle.ToString() });
                       
                        var Alturas = Direcciones.Where(x => x.calle == Calles).ToList();
                        var CountDirPrin = Alturas.Count();
                        IdentifyingData.Add(new IdentifyingData { Identity = string.Empty, Variable = "dirs_cant_dirprinc", Answer = CountDirPrin.ToString() });
                        int IdxDirPrin = 0;
                        int indexSec = 0;
                        foreach (var dirPrin in Alturas)
                        {
                            IdentifyingData.Add(new IdentifyingData { Identity = "803dc7389a8f4c338a13d2bb421ac290_" + IdxDirPrin, Variable = "dirs_dirprinc_txt", Answer = dirPrin.altura });
                            IdentifyingData.Add(new IdentifyingData { Identity = "615c75cf6f8149ff9761686444d17bd9_" + IdxDirPrin, Variable = "dirs_dirprinc_calle_idx", Answer = IdxCalle.ToString() });
                            IdentifyingData.Add(new IdentifyingData { Identity = "7ac931ab0196403192dec31b291212c7_" + IdxDirPrin, Variable = "dirs_dirprinc_id", Answer = IdxDirPrin.ToString() });

                            IdentifyingData.Add(new IdentifyingData { Identity = string.Empty, Variable = "dirs_cant_dirsecu", Answer = Direcciones.Sum(x => x.Quantity).ToString() });
                            List<GesDetalleDireccion> listDetalleSec = gesAsignacionesDAL.ObtieneDetalleDireccion<GesDetalleDireccion>(dirPrin.cod_alc, dirPrin.IdDireccionPrincipal);
 
                            foreach (var listSec in listDetalleSec)
                            {     
                                IdentifyingData.Add(new IdentifyingData { Identity = "10049fe4efc4475bbc44bc8024661ef9_" + indexSec, Variable = "dirs_dirsecu_txt", Answer = listSec.NroDomiciliarioCompleto.ToString() });
                                IdentifyingData.Add(new IdentifyingData { Identity = "92b5a723a83149838c968c9954920ef2_" + indexSec, Variable = "dirs_dirsecu_princ_idx", Answer = IdxDirPrin.ToString() });
                                IdentifyingData.Add(new IdentifyingData { Identity = "97774ee4352a44eabec245572bdda68f_" + indexSec, Variable = "dirs_dirsecu_id", Answer = indexSec.ToString() });
                                indexSec++;    
                            }
                           
                            IdxDirPrin++;
                        }
                        IdxCalle++;
                    }    
                    index++;
                }
               
                NuevaAsignacionEnt asigss = new NuevaAsignacionEnt();
                GesNuevaAsignacionSS newAsig = new GesNuevaAsignacionSS();
                var DtAsignacion = new NewAssignmentsEnt
                {
                    Comments = string.Empty,
                    Email = string.Empty,
                    IsAudioRecordingEnabled = false,
                    Password = string.Empty,
                    Quantity = Direcciones.Sum(x => x.Quantity),
                    QuestionnaireId = parametros.Where(x => x.Descripcion == "QuestionnaireVivUrbanoConoc").Select(x => x.Valor).FirstOrDefault(),
                    Responsible = rutResponsable.Replace("-", ""),
                    WebMode = false, 
                    IdentifyingData = IdentifyingData
                };
             
                newAsig.Asignacion = new NuevaAsignacionEnt() { NewAssignmentsEnt = DtAsignacion };
                DatosAsignacion.Add(newAsig);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return DatosAsignacion;
        }
        public Tuple<IList<IdentifyingData>> DatosPortadaVivNueva(List<GesDireccionGeo> Direcciones)
        {
            IList<IdentifyingData> IdentifyingData = new List<IdentifyingData>();
            try
            {
                IdentifyingData.Add(new IdentifyingData { Identity = string.Empty, Variable = "cod_alc", Answer = Direcciones.First().cod_alc });
                IdentifyingData.Add(new IdentifyingData { Identity = string.Empty, Variable = "idg_comuna", Answer = Direcciones.First().idg_comuna });
                IdentifyingData.Add(new IdentifyingData { Identity = string.Empty, Variable = "df_id_dirprin", Answer = string.Empty });
               
                return new Tuple<IList<IdentifyingData>>(IdentifyingData);
            }
            catch (Exception ex)
            {
                return new Tuple<IList<IdentifyingData>>(new List<IdentifyingData>());
            }
        }
        public List<GesNuevaAsignacionSS> DatosPrecargadosVivNueva(string codAlc, string rutResponsable)
        {
            GesAsignacionesDAL gesAsignacionesDAL = new GesAsignacionesDAL();
            List<GesNuevaAsignacionSS> DatosAsignacion = new List<GesNuevaAsignacionSS>();
            IList<IdentifyingData> IdentifyingData = new List<IdentifyingData>();
            try
            {
                var parametros = ListParametros();
                List<GesDireccionGeo> Direcciones = gesAsignacionesDAL.ObtieneDireccionesGeo<GesDireccionGeo>(codAlc);

                if (Direcciones.Count == 0) { return DatosAsignacion; }

                IdentifyingData = DatosPortadaVivNueva(Direcciones).Item1;
                var cantManzana = (from x in Direcciones select x.port_df_manzana).Distinct().ToList();
                IdentifyingData.Add(new IdentifyingData { Identity = string.Empty, Variable = "dirs_cant_manzanas", Answer = cantManzana.Count().ToString() });
                int index = 0;

                foreach (var manz in cantManzana)
                {
                    IdentifyingData.Add(new IdentifyingData { Identity = "425b8aab3de34ad1a044211f277fd801_" + index, Variable = "dirs_manzana_nom", Answer = manz });
                    IdentifyingData.Add(new IdentifyingData { Identity = "c3c9c59a63a34028bbcc2defc91d8279_" + index, Variable = "id_manzana", Answer = index.ToString() });

                    var calleManzana = Direcciones.Where(x => x.port_df_manzana == manz).ToList();
                    var cantCalles = (from x in calleManzana select x.calle).Distinct().ToList();
                    IdentifyingData.Add(new IdentifyingData { Identity = string.Empty, Variable = "dirs_cant_calles", Answer = cantCalles.Count().ToString() });
                    int IdxCalle = 0;
                    foreach (var Calles in cantCalles)
                    {
                        IdentifyingData.Add(new IdentifyingData { Identity = "4539a9ccd0504a519192abd775e8f9c8_" + IdxCalle, Variable = "dirs_calle_nom", Answer = Calles });
                        IdentifyingData.Add(new IdentifyingData { Identity = "fe941f76bda34ea6bb8d6f2bd54390a3_" + IdxCalle, Variable = "dirs_manzana_idx", Answer = index.ToString() });
                        IdentifyingData.Add(new IdentifyingData { Identity = "5868236506254c08a931bf9262c76ea4_" + IdxCalle, Variable = "dirs_calleid", Answer = IdxCalle.ToString() });

                        var Alturas = Direcciones.Where(x => x.calle == Calles).ToList();
                        var CountDirPrin = Alturas.Count();
                        IdentifyingData.Add(new IdentifyingData { Identity = string.Empty, Variable = "dirs_cant_dirprinc", Answer = CountDirPrin.ToString() });
                        int IdxDirPrin = 0;
                        int indexSec = 0;
                        foreach (var dirPrin in Alturas)
                        {
                            IdentifyingData.Add(new IdentifyingData { Identity = "e48a16914c4440eeac8e64aee6644a9f_" + IdxDirPrin, Variable = "dirs_dirprinc_txt", Answer = dirPrin.altura });
                            IdentifyingData.Add(new IdentifyingData { Identity = "d67a6bbec7774dd996b257ab0db6a730_" + IdxDirPrin, Variable = "dirs_dirprinc_calle_idx", Answer = IdxCalle.ToString() });
                            IdentifyingData.Add(new IdentifyingData { Identity = "69c69f2048304704b41ca1d31f3cb63f_" + IdxDirPrin, Variable = "dirs_dirprinc_id", Answer = IdxDirPrin.ToString() });

                            IdentifyingData.Add(new IdentifyingData { Identity = string.Empty, Variable = "dirs_cant_dirsecu", Answer = Direcciones.Sum(x => x.Quantity).ToString() });
                            List<GesDetalleDireccion> listDetalleSec = gesAsignacionesDAL.ObtieneDetalleDireccion<GesDetalleDireccion>(dirPrin.cod_alc, dirPrin.IdDireccionPrincipal);


                            foreach (var listSec in listDetalleSec)
                            {

                                IdentifyingData.Add(new IdentifyingData { Identity = "032571483131445aab2eda82f52df3a8_" + indexSec, Variable = "dirs_dirsecu_txt", Answer = listSec.NroDomiciliarioCompleto.ToString() });
                                IdentifyingData.Add(new IdentifyingData { Identity = "514d0dc18d414dd5b76b59fe21de2cd6_" + indexSec, Variable = "dirs_dirsecu_princ_idx", Answer = IdxDirPrin.ToString() });
                                IdentifyingData.Add(new IdentifyingData { Identity = "d8df6350705c4159b45da555c5afd874_" + indexSec, Variable = "dirs_dirsecu_id", Answer = indexSec.ToString() });
                                indexSec++;

                            }

                            IdxDirPrin++;
                        }
                        IdxCalle++;
                    }
                    index++;
                }

                NuevaAsignacionEnt asigss = new NuevaAsignacionEnt();
                GesNuevaAsignacionSS newAsig = new GesNuevaAsignacionSS();
                var DtAsignacion = new NewAssignmentsEnt
                {
                    Comments = string.Empty,
                    Email = string.Empty,
                    IsAudioRecordingEnabled = false,
                    Password = string.Empty,
                    Quantity = 30,
                    QuestionnaireId = parametros.Where(x => x.Descripcion == "CPV_CAPI_VivNva_Urb_PMM").Select(x => x.Valor).FirstOrDefault(),
                    Responsible = rutResponsable.Replace("-", ""),
                    WebMode = false,
                    IdentifyingData = IdentifyingData
                };
              
                newAsig.Asignacion = new NuevaAsignacionEnt() { NewAssignmentsEnt = DtAsignacion };
                DatosAsignacion.Add(newAsig);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return DatosAsignacion;
        }
        public Tuple<IList<IdentifyingData>> DatosPortadaVivArea(List<GesDireccionGeo> Direcciones)
        {
            IList<IdentifyingData> IdentifyingData = new List<IdentifyingData>();
            try
            {
                IdentifyingData.Add(new IdentifyingData { Identity = string.Empty, Variable = "cod_alc", Answer = Direcciones.First().cod_alc });
                IdentifyingData.Add(new IdentifyingData { Identity = string.Empty, Variable = "idg_comuna", Answer = Direcciones.First().idg_comuna });
 
                return new Tuple<IList<IdentifyingData>>(IdentifyingData);
            }
            catch (Exception ex)
            {
                return new Tuple<IList<IdentifyingData>>(new List<IdentifyingData>());
            }
        }
        public List<GesNuevaAsignacionSS> DatosPrecargadosArea(string codAlc, string rutResponsable)
        {
            GesAsignacionesDAL gesAsignacionesDAL = new GesAsignacionesDAL();
            List<GesNuevaAsignacionSS> DatosAsignacion = new List<GesNuevaAsignacionSS>();
            IList<IdentifyingData> IdentifyingData = new List<IdentifyingData>();
            try
            {
                var parametros = ListParametros();
                List<GesDireccionGeo> Direcciones = gesAsignacionesDAL.ObtieneDireccionesGeo<GesDireccionGeo>(codAlc);

                if (Direcciones.Count == 0) { return DatosAsignacion; }

                IdentifyingData = DatosPortadaVivArea(Direcciones).Item1;
                var cantManzana = (from x in Direcciones select x.port_df_manzana).Distinct().ToList();
                IdentifyingData.Add(new IdentifyingData { Identity = string.Empty, Variable = "numero_manzanas", Answer = cantManzana.Count().ToString() });
                int index = 0;

                foreach (var manz in cantManzana)
                {
                    IdentifyingData.Add(new IdentifyingData { Identity = "e5d21a5d25a3449b8c39fdc4e2befdea_" + index, Variable = "cod_dc_zc_mz", Answer = manz });
                    IdentifyingData.Add(new IdentifyingData { Identity = "92b1867b11364149933c1abf680c254c_" + index, Variable = "tipo_asignacion", Answer = "2" });

                    var NumeroCaras = Direcciones.Where(x => x.port_df_manzana == manz).ToList();
                    var ContCaras = (from x in NumeroCaras select x.calle ).Distinct().ToList();
                    IdentifyingData.Add(new IdentifyingData { Identity = "965595995a3b4f6f95fc61b4da5bc488_" + index, Variable = "numero_caras", Answer = ContCaras.Count().ToString() });
                    
                    
                    int IdxCaras = 0;
                    foreach (var Caras in ContCaras)
                    {
                        var IdTipoVia = Direcciones.Where(x => x.calle == Caras).ToList().First().IdTipoVia;
                        IdentifyingData.Add(new IdentifyingData { Identity = "b619dfd5f0bb4f3686f45e6a95ed2648_" + IdxCaras, Variable = "tipo_via_cara", Answer = IdTipoVia.ToString() });
                        IdentifyingData.Add(new IdentifyingData { Identity = "ad2f4b10c6094fa68dd4f764c50a4f3c_" + IdxCaras, Variable = "nombre_via_cara", Answer = Caras });

                        var Alturas = Direcciones.Where(x => x.calle == Caras).ToList();
                        var CountNumDomi = Alturas.Count();
                        IdentifyingData.Add(new IdentifyingData { Identity = "8cf84d41d0634d7d8a01147bfd989fb7_" + IdxCaras, Variable = "cant_nros_domicilios", Answer = CountNumDomi.ToString() });
                  
                        int indexNumDomi = 0;
                        foreach (var domicilio in Alturas)
                        {
                            IdentifyingData.Add(new IdentifyingData { Identity = "a940311d82d943628bf0531f75124b71_" + indexNumDomi, Variable = "nro_domiciliario", Answer = domicilio.altura });
                            indexNumDomi++;
                        }
                    }
                    index++;
                }

                NuevaAsignacionEnt asigss = new NuevaAsignacionEnt();
                GesNuevaAsignacionSS newAsig = new GesNuevaAsignacionSS();
                var DtAsignacion = new NewAssignmentsEnt
                {
                    Comments = string.Empty,
                    Email = string.Empty,
                    IsAudioRecordingEnabled = false,
                    Password = string.Empty,
                    Quantity = 1,
                    QuestionnaireId = parametros.Where(x => x.Descripcion == "QuestionnaireAreaUrbano").Select(x => x.Valor).FirstOrDefault(),
                    Responsible = rutResponsable.Replace("-", ""),
                    WebMode = false,
                    IdentifyingData = IdentifyingData
                };
              
                newAsig.Asignacion = new NuevaAsignacionEnt() { NewAssignmentsEnt = DtAsignacion };
                DatosAsignacion.Add(newAsig);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return DatosAsignacion;
        }
    }
}
