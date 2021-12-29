using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using Framework.BOL;
using Framework.DAL;
using Framework.BLL.Utilidades.Seguridad;
using Framework.BLL.Utilidades.Ajax;
using DatoSSINE.VO.SurveySolution.Asignaciones;
using System.Threading.Tasks;
using Framework.BLL.Integracion;
using Framework.BOL.Integracion;

namespace Framework.BLL
{
    public class GesReasignacionesBLL
    {
        AppSettings _appSettings = new AppSettings();
        string _strHtml = "";
        DataSet DS;
      
        /// <summary>
        /// Permite listar usuarios con asignacion
        /// </summary>
        public string ObtieneUsuariosAsignados(int sistema_id, int tipo_asig, int areacensal, string usu, int geo, int disponibles, int perfil_id, int tipo)
        {
            try
            {
                // Genero metodo submit del formulario
                CallMethod _methodCallLoad = new CallMethod
                {
                    Mc_contenido = "$.getScript('" + _appSettings.ServidorWeb + "Framework/assets/js/plugins/dataTables/datatables.min.js', function () {" +
                                            "$.getScript('" + _appSettings.ServidorWeb + "Framework/assets/js/plugins/dataTables/dataTables.bootstrap4.min.js', function () {" +
                                                "setTimeout(function () { $('.tabla-Asig').DataTable({ 'pageLength': 90000000, paging: false}); }, 100);" +
                                                "setTimeout(function () { $('.tabla-Asig2').DataTable({ 'pageLength': 90000000, paging: false}); }, 100);" +
                                            "});" +
                                            "$.getScript('" + _appSettings.ServidorWeb + "Framework/assets/js/plugins/iCheck/icheck.min.js', function () {" +
                                                                "$('.i-checksA').iCheck({ " +
                                                                    "checkboxClass: 'icheckbox_square-green'," +
                                                                    "radioClass: 'iradio_square-green'," +
                                                                "});" +
                                                            "});" +
                                       "});"                                       
                };

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
                    if (tipo != 1)
                    {
                        sb.Append("<table class=\"table table-hover table-striped text-center\">");
                    }
                    else
                    {
                        sb.Append("<table class=\"table table-hover table-striped text-center\">");
                    }
                        
                    sb.Append("<thead><tr>");
                    sb.Append("<th class=\"small\">#</th>");
                    if (tipo_asig == 4 || tipo_asig == 6)
                    {
                        sb.Append("<th class=\"small\">Rut</th>");
                        sb.Append("<th class=\"small\">Nombre Productor</th>");
                        sb.Append("<th class=\"small\">Superficie</th>");
                    }
                    else
                    {
                        sb.Append("<th class=\"small\">Sector</th>");
                    }
                    sb.Append("</tr></thead>");
                    sb.Append("<tbody>");
                    for (int i = 0; i <= DtAsigSelect.Rows.Count - 1; i++)
                    {
                        sb.Append("<tr>");
                        if (tipo != 1)
                        {
                            sb.Append("<td></td>");
                        }
                        else
                        {
                            sb.Append("<td class=\"small\"><div class=\"icheckbox_square-green hover\" style=\"position: relative;\">");
                            //sb.Append("<label><input type=\"checkbox\" value=\"\"><span class=\"cr\"><i class=\"cr-icon icofont icofont-ui-check txt-primary\"></i></span></label>";
                            sb.Append("<input id=\"chk_" + DtAsigSelect.Rows[i]["Usu_id"] + "\" name=\"\" class=\"i-checksA asignados Asig\" style=\"position: absolute; opacity: 0;\" type=\"checkbox\">");
                            sb.Append("</div></td>");
                        }
                        
                        if (tipo_asig == 4 || tipo_asig == 6)
                        {
                            sb.Append("<td class=\"small\">&nbsp" + DtAsigSelect.Rows[i]["Usu_id"] + "</td>");
                            sb.Append("<td class=\"small\">&nbsp" + DtAsigSelect.Rows[i]["Usu_nombre"] + "</td>");
                            sb.Append("<td class=\"small\">&nbsp" + DtAsigSelect.Rows[i]["directorio_superficie"] + "</td>");
                        }
                        else
                        {
                            sb.Append("<td class=\"small\">&nbsp" + DtAsigSelect.Rows[i]["Usu_nombre"] + "</td>");

                        }
                        sb.Append("</tr>");
                    }
                    sb.Append("</tbody></table>");
                    sb.Append(_methodCallLoad.CreaJQueryDocumentReady());
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
        /// Permite asignar
        /// </summary>
        public string InsertarReAsignaciones(int tipo_asig, int sistema_id, string sector, string usuarioOrigen, string usuarioDestino)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                _strHtml = "";
                string retorno = "";

                GesReasignacionesDAL gesReAsignacionesDAL = new GesReasignacionesDAL();

                DataSet DsAsigSelect = new DataSet();
                DsAsigSelect = gesReAsignacionesDAL.InsertarReAsignaciones(tipo_asig, sistema_id, sector, usuarioOrigen, usuarioDestino);

                retorno = DsAsigSelect.Tables[0].Rows[0][0].ToString();

                //Recorro Nivel 1
                if (retorno == "1")
                {
                    sb.Append("<div class=\"ibox-content table-border-style\">");
                    sb.Append("<div class=\"row col-lg-12 text-center\">");
                    sb.Append("<div class=\"col-lg-12 alert alert-danger alert-dismissable\">");
                    sb.Append("<button aria-hidden=\"true\" data-dismiss=\"alert\" onclick=\"$('.conasignacion').empty();\" class=\"close\" type=\"button\">×</button>");
                    sb.Append("Para realizar la acción no debe tener asignaciones de ningún tipo.");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    _strHtml = sb.ToString();
                }
                else if (retorno == "3")
                {
                    sb.Append("<div class=\"ibox-content table-border-style\">");
                    sb.Append("<div class=\"row col-lg-12 text-center\">");
                    sb.Append("<div class=\"col-lg-12 alert alert-danger alert-dismissable\">");
                    sb.Append("<button aria-hidden=\"true\" data-dismiss=\"alert\" onclick=\"$('.conasignacion').empty();\" class=\"close\" type=\"button\">×</button>");
                    sb.Append("Solo puede existir un coordinador de área censal por área censal.");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    _strHtml = sb.ToString();
                }
                else if (retorno == "0" )
                {
                    int alc = Convert.ToInt32(sector);
                    string rutResposable = usuarioDestino;
                    Boolean respuestaError = false;

                    string Mensaje = string.Empty;
                    List<GesReasignacionSS> Reasignacion = new List<GesReasignacionSS>();
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
                                    Usuario_Responsable = rutResposable.Replace("-", "")
                                };
                                reaSig.Reasignacion = datos;
                                Reasignacion.Add(reaSig);
                            }

                        });
                        Task.WaitAll(task1);

                        var FReasignacion = Reasignacion.Where(x => x.Reasignacion.Id_Asignacion != null).ToList();
                        var reasignar = new AsignacionesSS().ReAsignacion(FReasignacion, rutResposable);
                        if (reasignar.Cabecera.CodigoRespuesta != 200)
                        {
                            respuestaError = true;
                            Mensaje = "Problemas al Reasignar en el sistema Survey Solutions.";
                        }
                    }
                    else
                    {
                        respuestaError = true;
                        var reasignarGes = gesReAsignacionesDAL.InsertarReAsignaciones(tipo_asig, sistema_id, sector, rutResposable, usuarioOrigen);
                        Mensaje = "No existen asignaciones en Survey Solutions.";
                    }

                    if (respuestaError)
                    {
                        sb.Append("<div class=\"ibox-content table-border-style\">");
                        sb.Append("<div class=\"row col-lg-12 text-center\">");
                        sb.Append("<div class=\"col-lg-12 alert alert-danger alert-dismissable\">");
                        sb.Append("<button aria-hidden=\"true\" data-dismiss=\"alert\" onclick=\"$('.conasignacion').empty();\" class=\"close\" type=\"button\">×</button>");
                        sb.Append(Mensaje);
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");

                        _strHtml = sb.ToString();
                        return _strHtml;
                    }

                    sb.Append("<div class=\"ibox-content table-border-style\">");
                    sb.Append("<div class=\"row col-lg-12 text-center\">");
                    sb.Append("<div class=\"col-lg-12 alert alert-success alert-dismissable\">");
                    sb.Append("<button aria-hidden=\"true\" data-dismiss=\"alert\" onclick=\"$('.conasignacion').empty();\" class=\"close\" type=\"button\">×</button>");
                    sb.Append("Asignación realizada satisfactoriamente.");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    _strHtml = sb.ToString();
                }

                return _strHtml;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
