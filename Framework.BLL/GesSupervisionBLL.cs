using Framework.BLL.Utilidades.Ajax;
using Framework.BLL.Utilidades.Encriptacion;
using Framework.BLL.Utilidades.Html;
using Framework.BLL.Utilidades.Seguridad;
using Framework.BOL;
using Framework.DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Framework.BLL
{
    public class GesSupervisionBLL
    {
        AppSettings _appSettings = new AppSettings();
        Encrypt _encrypt = new Encrypt();

        string _strHtml = "";

        #region "Supervisión Móvil"              

        /// <summary>
        /// Permite mostrar seccion en formulario movil
        /// </summary>
        public string DibujaSeccionFormularioMovil(string guid, int codigo)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                _strHtml = "";

                GesSupervisionDAL gesSupervisionDAL = new GesSupervisionDAL();

                DataTable Dtmovil = new DataTable();
                DataSet Dsmovil = new DataSet();
                Dsmovil = gesSupervisionDAL.ObtieneFormulariosMovil(guid, codigo);

                if (Dsmovil.Tables[0].Rows.Count > 0)
                {
                    Dtmovil = Dsmovil.Tables[0];

                    sb.Append("<table class=\"tabla-Estados table table-hover text-center\">"); //table-striped                                  
                    sb.Append("<thead><tr>");

                    for (int i = 0; i <= Dtmovil.Columns.Count - 1; i++)
                    {
                        sb.Append("<th class=\"small\">" + Dtmovil.Columns[i] + "</th>");
                    }
                    sb.Append("</tr></thead>");
                    sb.Append("<tbody>");
                    for (int i = 0; i <= Dtmovil.Rows.Count - 1; i++)
                    {
                        string dato = "";
                        sb.Append("<tr id=\"\" class=\"\">");

                        for (int e = 0; e <= Dtmovil.Columns.Count - 1; e++)
                        {
                            if (Dtmovil.Rows[i][e].ToString() == "-77")
                            {
                                dato = "";
                            }
                            else
                            {
                                dato = Dtmovil.Rows[i][e].ToString();
                            }
                            sb.Append("<td class=\"small\">&nbsp" + dato + "</td>");
                        }

                        sb.Append("</tr>");
                    }

                    sb.Append("</tbody></table>");
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
        /// Permite listar cuestionarios movil para supervision retroalimentacion(tipo 1) e censal(tipo 2)
        /// </summary>
        public string ListaCuestionariosMovil(int alc, int tipo, int lev, string usu)
        {
            GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL();
            GesUsuarioBLL _gesUsuarioBLL = new GesUsuarioBLL();
            _gesUsuarioBOL = _gesUsuarioBLL.ObtieneUsuarioConectado(_appSettings.ObtieneCookie());

            GesAsignacionesDAL gesAsignacionesDAL = new GesAsignacionesDAL();
            DataSet DsDatosUsu = new DataSet();
            DsDatosUsu = gesAsignacionesDAL.ListaDatosUsuario(_gesUsuarioBOL);

            var perfil_usuario = DsDatosUsu.Tables[0].Rows[0]["perfil_id"].ToString();

            //Genero función para recorrer seleccionados
            CallMethod _methodCallMarcaSeleccion = new CallMethod
            {
                Mc_nombre = "MarcaSeleccion(identificador)",
                Mc_contenido = "$('.Ver').show();" +
                                "$('.filasPerfil').removeClass('alert alert-success');" +
                               "$('.filasPerfilNone').hide();" +
                               "$('#trNone_' + identificador).show();" +
                               "$('#tr_' + identificador).addClass('alert alert-success');" +
                                "$('.Ocultar').hide();"
            };
            
            // Genero metodo submit del formulario
            CallMethod _methodCallLoad = new CallMethod
            {
                Mc_contenido = "setTimeout(function () { $('.tabla-Estados').DataTable({ 'pageLength': 50, paging: true}); }, 100);" +
                               "$.getScript('" + _appSettings.ServidorWeb + "Framework/assets/js/plugins/iCheck/icheck.min.js', function () {" +
                                    "$('.i-checks').iCheck({ " +
                                        "checkboxClass: 'icheckbox_square-green'," +
                                        "radioClass: 'iradio_square-green'," +
                                    "});" +
                                    "$('.i-checks').on('ifChanged', function(event) {" +
                                        "if ($(this).is(':checked')){" +
                                            "str = this.id;" +
                                            "var num = str.split('_')[1]; " +
                                            "generaAvisoRecolector(num, 1);" +
                                        //"alert($(.divPadre '#chk_' + num).val())" +
                                        "}" +
                                        "if ($(this).is(':unchecked')){" +
                                            "str = this.id;" +
                                            "var num = str.split('_')[1]; " +
                                            "generaAvisoRecolector(num, 0);" +
                                        "}" +
                                    "});" +
                               "});"
            };

            try
            {
                StringBuilder sb = new StringBuilder();
                _strHtml = "";

                GesSupervisionDAL gesSupervisionDAL = new GesSupervisionDAL();

                string _estado;
                string aviso;
                string bloqueo;
                //string tipo_lev = "3";
                DataTable Dtmovil = new DataTable();
                DataSet Dsmovil = new DataSet();
                Dsmovil = gesSupervisionDAL.ListaCuestionariosSegunEstado(alc, tipo, usu, lev);

                if (Dsmovil.Tables[0].Rows.Count > 0)
                {
                    Dtmovil = Dsmovil.Tables[0];

                    sb.Append("<table class=\"tabla-Estados table table-hover text-center\" cellspacing=\"0\">"); //table-striped                                  
                    sb.Append("<thead><tr>");
                    //sb.Append("<th rowspan=\"2\" style=\"position: relative;\">Identificador</th>");
                    //sb.Append("<th colspan=\"6\">Información</th>");
                    //sb.Append("<th colspan=\"3\">Revisión</th>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr>");
                    sb.Append("<th class=\"small\">Identificador</th>");
                    sb.Append("<th class=\"small\">Nombre Censista</th>");
                    sb.Append("<th class=\"small\">Nombre Coordinador de Grupo</th>");
                    if (tipo == 1) {
                        sb.Append("<th class=\"small\">Porcentaje Obtenido</th>");
                        sb.Append("<th class=\"small\">Valoración</th>");
                    } else {
                        sb.Append("<th class=\"small\">Estado Levantamiento</th>");
                    }
                    
                    sb.Append("<th class=\"small\">Estado Observación</th>");      
                    sb.Append("<th class=\"small\"></th>");
                    
                    sb.Append("</tr></thead>");
                    sb.Append("<tbody>");
                    for (int i = 0; i <= Dtmovil.Rows.Count - 1; i++)
                    {                       
                        sb.Append("<tr id=\"tr_" + Dtmovil.Rows[i]["IdDireccionSecundaria"] + "\" class=\"filasPerfil\">");
                        sb.Append("<td class=\"small\">&nbsp" + Dtmovil.Rows[i]["IdDireccionSecundaria"] + "</td>");
                        sb.Append("<td class=\"small\">&nbsp" + Dtmovil.Rows[i]["NombreCensista"] + "</td>");
                        sb.Append("<td class=\"small\">&nbsp" + Dtmovil.Rows[i]["NombreSupervisor"] + "</td>");
                        if (tipo == 1) {
                            string pje = Dtmovil.Rows[i]["PorcentajePuntaje"].ToString().Replace(',','.');
                            string glosaPje = "";
                            if (!string.IsNullOrEmpty(pje)) {
                                int punto = pje.IndexOf(".");
                                int pje100 = 0;
                                if (punto > 0) {
                                    pje100 = Convert.ToInt32(pje.Substring(0, punto));
                                }
                                if (pje100 > 80) {
                                    glosaPje = "Bueno";
                                } else if (pje100 > 50 && pje100 <= 80) {
                                    glosaPje = "Regular";
                                } else if (pje100 <= 50) {
                                    glosaPje = "Insuficiente";
                                }
                                sb.Append("<td class=\"small\">&nbsp;" + pje + "</td>");
                                sb.Append("<td class=\"small\">&nbsp;" + glosaPje + "</td>");
                            } else {
                                sb.Append("<td class=\"small\">&nbsp; Sin información</td>");
                                sb.Append("<td class=\"small\">&nbsp; Sin información</td>");
                            }
                            //sb.Append("<td class=\"small\">&nbsp" + Dtmovil.Rows[i]["PorcentajePuntaje"] + "</td>");
                        }
                        else {
                            sb.Append("<td class=\"small\"></td>");
                        }
                        
                        //sb.Append("<td class=\"small\">" + Dtmovil.Rows[i]["Supervisado"] + "</td>");
                        //sb.Append("<td class=\"small\">" + Dtmovil.Rows[i]["NombreSupervisor"] + "</td>");

                        if (Convert.ToInt32(Dtmovil.Rows[i]["supervisado"]) == 0)
                        {
                            sb.Append("<td class=\"small\">No realizada</td>");
                            sb.Append("<td class=\"small\"><button id=\"btnMostrar_" + Dtmovil.Rows[i]["IdDireccionSecundaria"] + "\" onclick=\"muestraFormEstado('" + Dtmovil.Rows[i]["IdDireccionSecundaria"] + "', " + tipo + ",'" + Dtmovil.Rows[i]["RutSupervisor"] + "','" + Dtmovil.Rows[i]["RutCensista"] + "', '" + lev + "');\" type=\"button\" data-toggle=\"modal\" data-target=\"#modal-master\" class=\"btn btn-block btn-success\"><i class=\"fa fa-search\"></i> Supervisar</button></td>");
                        }
                        else
                        {
                            sb.Append("<td class=\"small\">Finalizada</td>");
                            sb.Append("<td class=\"small\"><button id=\"btnMostrar_" + Dtmovil.Rows[i]["IdDireccionSecundaria"] + "\" onclick=\"muestraFormEstado('" + Dtmovil.Rows[i]["IdDireccionSecundaria"] + "', " + tipo + ",'" + Dtmovil.Rows[i]["RutSupervisor"] + "','" + Dtmovil.Rows[i]["RutCensista"] + "', '" + lev + "');\" type=\"button\" data-toggle=\"modal\" data-target=\"#modal-master\" class=\"btn btn-block btn-info\"><i class=\"fa fa-search\"></i> Ver</button></td>");
                        }
                        sb.Append("</tr>");
                    }

                    sb.Append("</tbody></table>");
                    sb.Append(_methodCallMarcaSeleccion.CreaJQueryFunction());
                    sb.Append(_methodCallLoad.CreaJQueryDocumentReady());
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
        /// Permite insertar supervision indirecta movil
        /// </summary>
        public string InsertarSupervisionIndirectaMovil(string formData)
        {
            try
            {
                // Recibo objeto y lo convierto en tabla
                string objeto = "[" + JObject.Parse(formData).ToString() + "]";

                // UnPivote Datatable
                DataTable DtDatos = (DataTable)JsonConvert.DeserializeObject(objeto, (typeof(DataTable)));

                StringBuilder sb = new StringBuilder();
                _strHtml = "";

                GesSupervisionBOL _gesSupervisionBOL = new GesSupervisionBOL();
                _gesSupervisionBOL.IdTipoSupervision = Convert.ToInt32(DtDatos.Rows[0]["IdTipoSupervision"].ToString());
                _gesSupervisionBOL.IdSupervision = DtDatos.Rows[0]["IdSupervision"].ToString();
                _gesSupervisionBOL.IdTipoLevantamiento = Convert.ToInt32(DtDatos.Rows[0]["IdTipoLevantamiento"].ToString());
                _gesSupervisionBOL.sup_col1 = DtDatos.Rows[0]["sup_col1"].ToString();
                _gesSupervisionBOL.sup_col2 = DtDatos.Rows[0]["sup_col2"].ToString();  // Directa (999)
                _gesSupervisionBOL.sup_col3 = DtDatos.Rows[0]["sup_col3"].ToString();
                _gesSupervisionBOL.sup_col4 = DtDatos.Rows[0]["sup_col4"].ToString();
                _gesSupervisionBOL.sup_col5 = DtDatos.Rows[0]["sup_col5"].ToString();
                _gesSupervisionBOL.sup_col6 = DtDatos.Rows[0]["sup_col6"].ToString();
                _gesSupervisionBOL.sup_col7 = DtDatos.Rows[0]["sup_col7"].ToString();
                _gesSupervisionBOL.sup_col8 = DtDatos.Rows[0]["sup_col8"].ToString();
                _gesSupervisionBOL.sup_col9 = DtDatos.Rows[0]["sup_col9"].ToString();
                _gesSupervisionBOL.sup_col10 = DtDatos.Rows[0]["sup_col10"].ToString();
                _gesSupervisionBOL.sup_col11 = DtDatos.Rows[0]["sup_col11"].ToString();
                _gesSupervisionBOL.sup_col12 = DtDatos.Rows[0]["sup_col12"].ToString();
                _gesSupervisionBOL.sup_col13 = DtDatos.Rows[0]["sup_col13"].ToString();
                _gesSupervisionBOL.sup_col14 = DtDatos.Rows[0]["sup_col14"].ToString();
                _gesSupervisionBOL.sup_col15 = DtDatos.Rows[0]["sup_col15"].ToString();
                _gesSupervisionBOL.sup_col16 = DtDatos.Rows[0]["sup_col16"].ToString();
                _gesSupervisionBOL.sup_col17 = DtDatos.Rows[0]["sup_col17"].ToString();

                GesSupervisionDAL gesSupervisionDAL = new GesSupervisionDAL();

                int error = 0;
                for (int i = 2; i <= 18; i++)
                {
                    if (DtDatos.Rows[0][i].ToString() == "-1" || DtDatos.Rows[0][i].ToString() == "")
                    {
                        if (i >= 2 && i <= 5) {
                            error = error + 1;
                        } else if (i == 6 && DtDatos.Rows[0]["sup_col4"].ToString().Equals("1")) {
                            error = error + 1;
                        } else if (i > 6 && DtDatos.Rows[0]["sup_col5"].ToString().Equals("1")) {
                            error = error + 1;
                        } else if (i == 15 && DtDatos.Rows[0]["sup_col5"].ToString().Equals("1")) {
                            error = error + 1;
                        } 
                        
                        //error = error + 1;
                    }
                }

                DataSet DsSupervisionInsert = new DataSet();
                string retorno = "";
                string clase1 = "";
                string clase2 = "";
                string mensaje = ""; 

                if (error == 0)
                {
                    DsSupervisionInsert = gesSupervisionDAL.InsertarSupervision(_gesSupervisionBOL);
                    retorno = DsSupervisionInsert.Tables[0].Rows[0][0].ToString();

                    if (retorno.Equals("0")) {
                        clase1 = "correcto";
                        clase2 = "success";
                        mensaje = "¡Supervisión realizada con éxito!";
                    } else if (retorno.Equals("1")) {
                        clase1 = "error";
                        clase2 = "danger";
                        mensaje = "La supervisión ya fue realizada y no puede ser modificada.";
                    }
                } else {
                    clase1 = "error";
                    clase2 = "danger";
                    mensaje = "Favor completar todos los campos.";
                }

                sb.Append("<div class=\"text-center " + clase1 + "\" style=\"\">");
                sb.Append("<div class=\"alert alert-" + clase2 + " alert-dismissable\">");
                sb.Append("<button aria-hidden=\"true\" data-dismiss=\"alert\"  class=\"close\" type=\"button\">×</button>");
                sb.Append(mensaje);
                sb.Append("</div>");
                sb.Append("</div>");

                _strHtml = sb.ToString();

                return _strHtml;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Permite insertar supervision indirecta web
        /// </summary>
        public string InsertarSupervisionIndirectaWeb(string formData)
        {
            try
            {
                // Recibo objeto y lo convierto en tabla
                string objeto = "[" + JObject.Parse(formData).ToString() + "]";

                // UnPivote Datatable
                DataTable DtDatos = (DataTable)JsonConvert.DeserializeObject(objeto, (typeof(DataTable)));

                StringBuilder sb = new StringBuilder();
                _strHtml = "";

                GesSupervisionBOL _gesSupervisionBOL = new GesSupervisionBOL();
                _gesSupervisionBOL.IdTipoSupervision = Convert.ToInt32(DtDatos.Rows[0]["IdTipoSupervision"].ToString());
                _gesSupervisionBOL.IdSupervision = DtDatos.Rows[0]["IdSupervision"].ToString();
                _gesSupervisionBOL.IdTipoLevantamiento = Convert.ToInt32(DtDatos.Rows[0]["IdTipoLevantamiento"].ToString());
                _gesSupervisionBOL.sup_col1 = DtDatos.Rows[0]["sup_col1"].ToString();
                _gesSupervisionBOL.sup_col2 = DtDatos.Rows[0]["sup_col2"].ToString();  // Directa (999)
                _gesSupervisionBOL.sup_col3 = DtDatos.Rows[0]["sup_col3"].ToString();
                _gesSupervisionBOL.sup_col4 = DtDatos.Rows[0]["sup_col4"].ToString();
                _gesSupervisionBOL.sup_col5 = DtDatos.Rows[0]["sup_col5"].ToString();
                _gesSupervisionBOL.sup_col6 = DtDatos.Rows[0]["sup_col6"].ToString();
                _gesSupervisionBOL.sup_col7 = DtDatos.Rows[0]["sup_col7"].ToString();
                _gesSupervisionBOL.sup_col8 = DtDatos.Rows[0]["sup_col8"].ToString();
                _gesSupervisionBOL.sup_col9 = DtDatos.Rows[0]["sup_col9"].ToString();
                _gesSupervisionBOL.sup_col10 = DtDatos.Rows[0]["sup_col10"].ToString();
                _gesSupervisionBOL.sup_col11 = DtDatos.Rows[0]["sup_col11"].ToString();
                _gesSupervisionBOL.sup_col12 = DtDatos.Rows[0]["sup_col12"].ToString();

                GesSupervisionDAL gesSupervisionDAL = new GesSupervisionDAL();

                int error = 0;
                for (int i = 2; i <= 13; i++)
                {
                    if (DtDatos.Rows[0][i].ToString() == "-1" || DtDatos.Rows[0][i].ToString() == "")
                    {
                        if ((i >= 2 && i <= 5)) {
                            error = error + 1;
                        } else if (i == 6 && DtDatos.Rows[0]["sup_col4"].ToString().Equals("1")) {
                            error = error + 1;
                        } else if (i > 6 && DtDatos.Rows[0]["sup_col5"].ToString().Equals("1")) {
                            error = error + 1;
                        } 
                        
                    }
                }

                DataSet DsSupervisionInsert = new DataSet();
                string retorno = "";
                string clase1 = "";
                string clase2 = "";
                string mensaje = ""; 

                if (error == 0)
                {
                    DsSupervisionInsert = gesSupervisionDAL.InsertarSupervision(_gesSupervisionBOL);
                    retorno = DsSupervisionInsert.Tables[0].Rows[0][0].ToString();

                    if (retorno.Equals("0")) {
                        clase1 = "correcto";
                        clase2 = "success";
                        mensaje = "¡Supervisión realizada con éxito!";
                    } else if (retorno.Equals("1")) {
                        clase1 = "error";
                        clase2 = "danger";
                        mensaje = "La supervisión ya fue realizada y no puede ser modificada.";
                    }
                } else {
                    clase1 = "error";
                    clase2 = "danger";
                    mensaje = "Favor completar todos los campos.";
                }

                sb.Append("<div class=\"text-center " + clase1 + "\" style=\"\">");
                sb.Append("<div class=\"alert alert-" + clase2 + " alert-dismissable\">");
                sb.Append("<button aria-hidden=\"true\" data-dismiss=\"alert\"  class=\"close\" type=\"button\">×</button>");
                sb.Append(mensaje);
                sb.Append("</div>");
                sb.Append("</div>");

                _strHtml = sb.ToString();

                return _strHtml;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Permite insertar supervision indirecta telefonica
        /// </summary>
        public string InsertarSupervisionIndirectaTel(string formData)
        {
            try
            {
                // Recibo objeto y lo convierto en tabla
                string objeto = "[" + JObject.Parse(formData).ToString() + "]";

                // UnPivote Datatable
                DataTable DtDatos = (DataTable)JsonConvert.DeserializeObject(objeto, (typeof(DataTable)));

                StringBuilder sb = new StringBuilder();
                _strHtml = "";

                GesSupervisionBOL _gesSupervisionBOL = new GesSupervisionBOL();
                _gesSupervisionBOL.IdTipoSupervision = Convert.ToInt32(DtDatos.Rows[0]["IdTipoSupervision"].ToString());
                _gesSupervisionBOL.IdSupervision = DtDatos.Rows[0]["IdSupervision"].ToString();
                _gesSupervisionBOL.IdTipoLevantamiento = Convert.ToInt32(DtDatos.Rows[0]["IdTipoLevantamiento"].ToString());
                _gesSupervisionBOL.sup_col1 = DtDatos.Rows[0]["sup_col1"].ToString();
                _gesSupervisionBOL.sup_col2 = DtDatos.Rows[0]["sup_col2"].ToString(); 
                _gesSupervisionBOL.sup_col3 = DtDatos.Rows[0]["sup_col3"].ToString();
                _gesSupervisionBOL.sup_col4 = DtDatos.Rows[0]["sup_col4"].ToString();
                _gesSupervisionBOL.sup_col5 = DtDatos.Rows[0]["sup_col5"].ToString();
                _gesSupervisionBOL.sup_col6 = DtDatos.Rows[0]["sup_col6"].ToString();
                _gesSupervisionBOL.sup_col7 = DtDatos.Rows[0]["sup_col7"].ToString();
                _gesSupervisionBOL.sup_col8 = DtDatos.Rows[0]["sup_col8"].ToString();
                _gesSupervisionBOL.sup_col9 = DtDatos.Rows[0]["sup_col9"].ToString();
                _gesSupervisionBOL.sup_col10 = DtDatos.Rows[0]["sup_col10"].ToString();
                _gesSupervisionBOL.sup_col11 = DtDatos.Rows[0]["sup_col11"].ToString();
                _gesSupervisionBOL.sup_col12 = DtDatos.Rows[0]["sup_col12"].ToString();
                _gesSupervisionBOL.sup_col13 = DtDatos.Rows[0]["sup_col13"].ToString();
                _gesSupervisionBOL.sup_col14 = DtDatos.Rows[0]["sup_col14"].ToString();
                _gesSupervisionBOL.sup_col15 = DtDatos.Rows[0]["sup_col15"].ToString();

                GesSupervisionDAL gesSupervisionDAL = new GesSupervisionDAL();

                int error = 0;
                for (int i = 2; i <= 16; i++)
                {
                    if (DtDatos.Rows[0][i].ToString() == "-1" || DtDatos.Rows[0][i].ToString() == "")
                    {
                        if ((i >= 2 && i <= 5)) {
                            error = error + 1;
                        } else if (i == 6 && DtDatos.Rows[0]["sup_col4"].ToString().Equals("1")) {
                            error = error + 1;
                        } else if (i > 6 && DtDatos.Rows[0]["sup_col5"].ToString().Equals("1")) {
                            error = error + 1;
                        }

                    }
                }

                DataSet DsSupervisionInsert = new DataSet();
                string retorno = "";
                string clase1 = "";
                string clase2 = "";
                string mensaje = "";

                if (error == 0)
                {
                    DsSupervisionInsert = gesSupervisionDAL.InsertarSupervision(_gesSupervisionBOL);
                    retorno = DsSupervisionInsert.Tables[0].Rows[0][0].ToString();

                    if (retorno.Equals("0"))
                    {
                        clase1 = "correcto";
                        clase2 = "success";
                        mensaje = "¡Supervisión realizada con éxito!";
                    }
                    else if (retorno.Equals("1"))
                    {
                        clase1 = "error";
                        clase2 = "danger";
                        mensaje = "La supervisión ya fue realizada y no puede ser modificada.";
                    }
                }
                else
                {
                    clase1 = "error";
                    clase2 = "danger";
                    mensaje = "Favor completar todos los campos.";
                }

                sb.Append("<div class=\"text-center " + clase1 + "\" style=\"\">");
                sb.Append("<div class=\"alert alert-" + clase2 + " alert-dismissable\">");
                sb.Append("<button aria-hidden=\"true\" data-dismiss=\"alert\"  class=\"close\" type=\"button\">×</button>");
                sb.Append(mensaje);
                sb.Append("</div>");
                sb.Append("</div>");

                _strHtml = sb.ToString();

                return _strHtml;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Permite insertar supervision especial
        /// </summary>
        public string InsertarSupervisionEspOcupacion(string formData)
        {
            try
            {
                // Recibo objeto y lo convierto en tabla
                string objeto = "[" + JObject.Parse(formData).ToString() + "]";

                // UnPivote Datatable
                DataTable DtDatos = (DataTable)JsonConvert.DeserializeObject(objeto, (typeof(DataTable)));

                StringBuilder sb = new StringBuilder();
                _strHtml = "";

                GesSupervisionBOL _gesSupervisionBOL = new GesSupervisionBOL();
                _gesSupervisionBOL.IdTipoSupervision = Convert.ToInt32(DtDatos.Rows[0]["IdTipoSupervision"].ToString());
                _gesSupervisionBOL.IdSupervision = DtDatos.Rows[0]["IdSupervision"].ToString();
                _gesSupervisionBOL.IdTipoLevantamiento = Convert.ToInt32(DtDatos.Rows[0]["IdTipoLevantamiento"].ToString());
                _gesSupervisionBOL.sup_col1 = DtDatos.Rows[0]["sup_col1"].ToString();
                _gesSupervisionBOL.sup_col2 = DtDatos.Rows[0]["sup_col2"].ToString(); 
                _gesSupervisionBOL.sup_col3 = DtDatos.Rows[0]["sup_col3"].ToString();
                _gesSupervisionBOL.sup_col4 = DtDatos.Rows[0]["sup_col4"].ToString();

                GesSupervisionDAL gesSupervisionDAL = new GesSupervisionDAL();

                int error = 0;
                for (int i = 2; i <= 5; i++)
                {
                    if (DtDatos.Rows[0][i].ToString() == "-1" || DtDatos.Rows[0][i].ToString() == "")
                    {
                        error = error + 1;

                    }
                }

                DataSet DsSupervisionInsert = new DataSet();
                string retorno = "";
                string clase1 = "";
                string clase2 = "";
                string mensaje = "";

                if (error == 0)
                {
                    DsSupervisionInsert = gesSupervisionDAL.InsertarSupervision(_gesSupervisionBOL);
                    retorno = DsSupervisionInsert.Tables[0].Rows[0][0].ToString();

                    if (retorno.Equals("0"))
                    {
                        clase1 = "correcto";
                        clase2 = "success";
                        mensaje = "¡Supervisión realizada con éxito!";
                    }
                    else if (retorno.Equals("1"))
                    {
                        clase1 = "error";
                        clase2 = "danger";
                        mensaje = "La supervisión ya fue realizada y no puede ser modificada.";
                    }
                }
                else
                {
                    clase1 = "error";
                    clase2 = "danger";
                    mensaje = "Favor completar todos los campos.";
                }

                sb.Append("<div class=\"text-center " + clase1 + "\" style=\"\">");
                sb.Append("<div class=\"alert alert-" + clase2 + " alert-dismissable\">");
                sb.Append("<button aria-hidden=\"true\" data-dismiss=\"alert\"  class=\"close\" type=\"button\">×</button>");
                sb.Append(mensaje);
                sb.Append("</div>");
                sb.Append("</div>");

                _strHtml = sb.ToString();

                return _strHtml;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Permite insertar supervision especial
        /// </summary>
        public string InsertarSupervisionEspTipoVivienda(string formData)
        {
            try
            {
                // Recibo objeto y lo convierto en tabla
                string objeto = "[" + JObject.Parse(formData).ToString() + "]";

                // UnPivote Datatable
                DataTable DtDatos = (DataTable)JsonConvert.DeserializeObject(objeto, (typeof(DataTable)));

                StringBuilder sb = new StringBuilder();
                _strHtml = "";

                GesSupervisionBOL _gesSupervisionBOL = new GesSupervisionBOL();
                _gesSupervisionBOL.IdTipoSupervision = Convert.ToInt32(DtDatos.Rows[0]["IdTipoSupervision"].ToString());
                _gesSupervisionBOL.IdSupervision = DtDatos.Rows[0]["IdSupervision"].ToString();
                _gesSupervisionBOL.IdTipoLevantamiento = Convert.ToInt32(DtDatos.Rows[0]["IdTipoLevantamiento"].ToString());
                _gesSupervisionBOL.sup_col1 = DtDatos.Rows[0]["sup_col1"].ToString();
                _gesSupervisionBOL.sup_col2 = DtDatos.Rows[0]["sup_col2"].ToString();
                _gesSupervisionBOL.sup_col3 = DtDatos.Rows[0]["sup_col3"].ToString();
                _gesSupervisionBOL.sup_col4 = DtDatos.Rows[0]["sup_col4"].ToString();

                GesSupervisionDAL gesSupervisionDAL = new GesSupervisionDAL();

                int error = 0;
                for (int i = 2; i <= 5; i++)
                {
                    if (DtDatos.Rows[0][i].ToString() == "-1" || DtDatos.Rows[0][i].ToString() == "")
                    {
                        error = error + 1;

                    }
                }

                DataSet DsSupervisionInsert = new DataSet();
                string retorno = "";
                string clase1 = "";
                string clase2 = "";
                string mensaje = "";

                if (error == 0)
                {
                    DsSupervisionInsert = gesSupervisionDAL.InsertarSupervision(_gesSupervisionBOL);
                    retorno = DsSupervisionInsert.Tables[0].Rows[0][0].ToString();

                    if (retorno.Equals("0"))
                    {
                        clase1 = "correcto";
                        clase2 = "success";
                        mensaje = "¡Supervisión realizada con éxito!";
                    }
                    else if (retorno.Equals("1"))
                    {
                        clase1 = "error";
                        clase2 = "danger";
                        mensaje = "La supervisión ya fue realizada y no puede ser modificada.";
                    }
                }
                else
                {
                    clase1 = "error";
                    clase2 = "danger";
                    mensaje = "Favor completar todos los campos.";
                }

                sb.Append("<div class=\"text-center " + clase1 + "\" style=\"\">");
                sb.Append("<div class=\"alert alert-" + clase2 + " alert-dismissable\">");
                sb.Append("<button aria-hidden=\"true\" data-dismiss=\"alert\"  class=\"close\" type=\"button\">×</button>");
                sb.Append(mensaje);
                sb.Append("</div>");
                sb.Append("</div>");

                _strHtml = sb.ToString();

                return _strHtml;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Permite listar cuestionarios para asignacion manual
        /// </summary>
        public string ListaCuestionariosAsigManual(int alc, int tipo, string usu)
        {
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

                GesSupervisionDAL gesSupervisionDAL = new GesSupervisionDAL();

                string _estado;
                DataTable Dtmovil = new DataTable();
                DataSet Dsmovil = new DataSet();
                Dsmovil = gesSupervisionDAL.ListaCuestionariosSegunEstado(alc, 3, usu,0);

                if (Dsmovil.Tables[0].Rows.Count > 0)
                {
                    Dtmovil = Dsmovil.Tables[0];

                    sb.Append("<table class=\"tabla-Manual table table-hover text-center\">"); //table-striped                                  
                    sb.Append("<thead><tr>");
                    sb.Append("<th class=\"small\">Tipo Levantamiento</th>");
                    sb.Append("<th class=\"small\">Identificador</th>");
                    sb.Append("<th class=\"small\">Fecha última visita</th>");
                    sb.Append("<th class=\"small\">Nombre Recolector</th>");
                    sb.Append("<th class=\"small\">Nombre Supervisor</th>");
                    sb.Append("<th></th>");
                    sb.Append("</tr></thead>");
                    sb.Append("<tbody>");
                    for (int i = 0; i <= Dtmovil.Rows.Count - 1; i++)
                    {
                        _estado = Dtmovil.Rows[i]["esmo_id"].ToString();

                        sb.Append("<tr id=\"tr_" + Dtmovil.Rows[i]["remo_guid"] + "\" class=\"filasPerfil\">");
                        if (Convert.ToInt32(_estado) > 0 && Convert.ToInt32(_estado) < 14)
                        {
                            sb.Append("<td class=\"small\"><div id=\"\">Indagación</div></td>");
                        }
                        else
                        {
                            sb.Append("<td class=\"small\"><div id=\"\">Cuestionario</div></td>");
                        }
                        sb.Append("<td class=\"small\">&nbsp" + Dtmovil.Rows[i]["Folio"] + "</td>");
                        sb.Append("<td class=\"small\">&nbsp" + Dtmovil.Rows[i]["remo_fecha_visita"] + "</td>");
                        sb.Append("<td class=\"small\">" + Dtmovil.Rows[i]["remo_nom_rec"] + "</td>");
                        sb.Append("<td class=\"small\">" + Dtmovil.Rows[i]["remo_nom_sup"] + "</td>");
                        sb.Append("<td class=\"small\"><button id=\"btnAsigManual_" + Dtmovil.Rows[i]["remo_guid"] + "\" onclick=\"generaAsignacionIndirectaManual('" + Dtmovil.Rows[i]["remo_guid"] + "', " + _estado + ");\" type=\"button\" class=\"btn btn-block btn-success\"><i class=\"fa fa-plus\"></i> Agregar</button></td>");
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
        /// Permite insertar supervision directa
        /// </summary>
        public string InsertarSupervisionDirecta(string formData)
        {
            try
            {
                // Recibo objeto y lo convierto en tabla
                string objeto = "[" + JObject.Parse(formData).ToString() + "]";

                // UnPivote Datatable
                DataTable DtDatos = (DataTable)JsonConvert.DeserializeObject(objeto, (typeof(DataTable)));

                StringBuilder sb = new StringBuilder();
                string tipoLev = DtDatos.Rows[0]["IdTipoLevantamiento"].ToString(); ; 
                _strHtml = "";

                GesSupervisionBOL _gesSupervisionBOL = new GesSupervisionBOL();
                _gesSupervisionBOL.IdTipoSupervision = 1;
                _gesSupervisionBOL.IdTipoLevantamiento = Convert.ToInt32(DtDatos.Rows[0]["IdTipoLevantamiento"].ToString());

                if (tipoLev.Equals("1"))
                {
                    _gesSupervisionBOL.IdSupervision = DtDatos.Rows[0]["IdSupervision"].ToString();
                    _gesSupervisionBOL.sup_col1 = DtDatos.Rows[0]["sup_col1"].ToString();
                    _gesSupervisionBOL.sup_col2 = DtDatos.Rows[0]["sup_col2"].ToString();
                    _gesSupervisionBOL.sup_col3 = DtDatos.Rows[0]["sup_col3"].ToString();
                    _gesSupervisionBOL.sup_col4 = DtDatos.Rows[0]["sup_col4"].ToString();
                    _gesSupervisionBOL.sup_col5 = DtDatos.Rows[0]["sup_col5"].ToString();
                    _gesSupervisionBOL.sup_col6 = DtDatos.Rows[0]["sup_col6"].ToString();
                    _gesSupervisionBOL.sup_col7 = DtDatos.Rows[0]["sup_col7"].ToString();
                    _gesSupervisionBOL.sup_col8 = DtDatos.Rows[0]["sup_col8"].ToString();
                    _gesSupervisionBOL.sup_col9 = DtDatos.Rows[0]["sup_col9"].ToString();
                    _gesSupervisionBOL.sup_col10 = DtDatos.Rows[0]["sup_col10"].ToString();
                    _gesSupervisionBOL.sup_col11 = DtDatos.Rows[0]["sup_col11"].ToString();
                    _gesSupervisionBOL.sup_col12 = DtDatos.Rows[0]["sup_col12"].ToString();
                    _gesSupervisionBOL.sup_col13 = DtDatos.Rows[0]["sup_col13"].ToString();
                    _gesSupervisionBOL.sup_col14 = DtDatos.Rows[0]["sup_col14"].ToString();
                    _gesSupervisionBOL.sup_col15 = DtDatos.Rows[0]["sup_col15"].ToString();
                    _gesSupervisionBOL.sup_col16 = DtDatos.Rows[0]["sup_col16"].ToString();
                    _gesSupervisionBOL.sup_col17 = DtDatos.Rows[0]["sup_col17"].ToString();
                    _gesSupervisionBOL.sup_col18 = DtDatos.Rows[0]["sup_col18"].ToString();
                    _gesSupervisionBOL.sup_col19 = DtDatos.Rows[0]["sup_col19"].ToString();
                    _gesSupervisionBOL.sup_col20 = DtDatos.Rows[0]["sup_col20"].ToString();
                    _gesSupervisionBOL.sup_col21 = DtDatos.Rows[0]["sup_col21"].ToString();
                    _gesSupervisionBOL.sup_col22 = DtDatos.Rows[0]["sup_col22"].ToString();
                    _gesSupervisionBOL.sup_col23 = DtDatos.Rows[0]["sup_col23"].ToString();
                    _gesSupervisionBOL.sup_col24 = DtDatos.Rows[0]["sup_col24"].ToString();
                    _gesSupervisionBOL.sup_col25 = DtDatos.Rows[0]["sup_col25"].ToString();
                    _gesSupervisionBOL.sup_col26 = DtDatos.Rows[0]["sup_col26"].ToString();
                    _gesSupervisionBOL.sup_col27 = DtDatos.Rows[0]["sup_col27"].ToString();
                    _gesSupervisionBOL.sup_col28 = DtDatos.Rows[0]["sup_col28"].ToString();
                    _gesSupervisionBOL.sup_col29 = DtDatos.Rows[0]["sup_col29"].ToString();
                    _gesSupervisionBOL.sup_col30 = DtDatos.Rows[0]["sup_col30"].ToString();
                    _gesSupervisionBOL.sup_col31 = DtDatos.Rows[0]["sup_col31"].ToString();
                    _gesSupervisionBOL.sup_col32 = DtDatos.Rows[0]["sup_col32"].ToString();
                    _gesSupervisionBOL.sup_col33 = DtDatos.Rows[0]["sup_col33"].ToString();
                    _gesSupervisionBOL.sup_col34 = DtDatos.Rows[0]["sup_col34"].ToString();
                    _gesSupervisionBOL.sup_col35 = DtDatos.Rows[0]["sup_col35"].ToString();
                    _gesSupervisionBOL.sup_col36 = DtDatos.Rows[0]["sup_col36"].ToString();
                    _gesSupervisionBOL.sup_col37 = DtDatos.Rows[0]["sup_col37"].ToString();
                    _gesSupervisionBOL.sup_col38 = DtDatos.Rows[0]["sup_col38"].ToString();
                    _gesSupervisionBOL.sup_col39 = DtDatos.Rows[0]["sup_col39"].ToString();
                    _gesSupervisionBOL.sup_col40 = DtDatos.Rows[0]["sup_col40"].ToString();
                    _gesSupervisionBOL.sup_col41 = DtDatos.Rows[0]["sup_col41"].ToString();
                    _gesSupervisionBOL.sup_col42 = DtDatos.Rows[0]["sup_col42"].ToString();
                    _gesSupervisionBOL.sup_col43 = DtDatos.Rows[0]["sup_col43"].ToString();
                    _gesSupervisionBOL.sup_col44 = DtDatos.Rows[0]["sup_col44"].ToString();
                    _gesSupervisionBOL.sup_col45 = DtDatos.Rows[0]["sup_col45"].ToString();
                    _gesSupervisionBOL.sup_col46 = DtDatos.Rows[0]["sup_col46"].ToString();
                    _gesSupervisionBOL.sup_col47 = DtDatos.Rows[0]["sup_col47"].ToString();
                    _gesSupervisionBOL.sup_col48 = DtDatos.Rows[0]["sup_col48"].ToString();
                    _gesSupervisionBOL.sup_col49 = DtDatos.Rows[0]["sup_col49"].ToString();
                    _gesSupervisionBOL.sup_col50 = DtDatos.Rows[0]["sup_col50"].ToString();
                    _gesSupervisionBOL.sup_col51 = DtDatos.Rows[0]["sup_col51"].ToString();
                    _gesSupervisionBOL.sup_col52 = DtDatos.Rows[0]["sup_col52"].ToString();
                    _gesSupervisionBOL.sup_col53 = DtDatos.Rows[0]["sup_col53"].ToString();
                    _gesSupervisionBOL.sup_col54 = DtDatos.Rows[0]["sup_col54"].ToString();
                    _gesSupervisionBOL.sup_col55 = DtDatos.Rows[0]["sup_col55"].ToString();
                    _gesSupervisionBOL.sup_col56 = DtDatos.Rows[0]["sup_col56"].ToString();
                    _gesSupervisionBOL.sup_col57 = DtDatos.Rows[0]["sup_col57"].ToString();
                    _gesSupervisionBOL.sup_col58 = DtDatos.Rows[0]["sup_col58"].ToString();
                    _gesSupervisionBOL.sup_col59 = DtDatos.Rows[0]["CodCuestionario"].ToString();
                }
                else if (tipoLev.Equals("2")){
                    _gesSupervisionBOL.IdSupervision = DtDatos.Rows[0]["IdSupervision"].ToString();
                    _gesSupervisionBOL.sup_col1 = DtDatos.Rows[0]["sup_col1"].ToString();
                    _gesSupervisionBOL.sup_col2 = DtDatos.Rows[0]["sup_col2"].ToString();
                    _gesSupervisionBOL.sup_col3 = DtDatos.Rows[0]["sup_col3"].ToString();
                    _gesSupervisionBOL.sup_col4 = DtDatos.Rows[0]["sup_col4"].ToString();
                    _gesSupervisionBOL.sup_col5 = DtDatos.Rows[0]["sup_col5"].ToString();
                    _gesSupervisionBOL.sup_col6 = DtDatos.Rows[0]["sup_col6"].ToString();
                    _gesSupervisionBOL.sup_col7 = DtDatos.Rows[0]["sup_col7"].ToString();
                    _gesSupervisionBOL.sup_col8 = DtDatos.Rows[0]["sup_col8"].ToString();
                    _gesSupervisionBOL.sup_col9 = DtDatos.Rows[0]["sup_col9"].ToString();
                    _gesSupervisionBOL.sup_col10 = DtDatos.Rows[0]["sup_col10"].ToString();
                    _gesSupervisionBOL.sup_col11 = DtDatos.Rows[0]["sup_col11"].ToString();
                    _gesSupervisionBOL.sup_col12 = DtDatos.Rows[0]["sup_col12"].ToString();
                    _gesSupervisionBOL.sup_col13 = DtDatos.Rows[0]["sup_col13"].ToString();
                    _gesSupervisionBOL.sup_col14 = DtDatos.Rows[0]["sup_col14"].ToString();
                    _gesSupervisionBOL.sup_col15 = DtDatos.Rows[0]["sup_col15"].ToString();
                    _gesSupervisionBOL.sup_col16 = DtDatos.Rows[0]["sup_col16"].ToString();
                    _gesSupervisionBOL.sup_col17 = DtDatos.Rows[0]["sup_col17"].ToString();
                    _gesSupervisionBOL.sup_col18 = DtDatos.Rows[0]["sup_col18"].ToString();
                    _gesSupervisionBOL.sup_col19 = DtDatos.Rows[0]["sup_col19"].ToString();
                    _gesSupervisionBOL.sup_col20 = DtDatos.Rows[0]["sup_col20"].ToString();
                    _gesSupervisionBOL.sup_col21 = DtDatos.Rows[0]["sup_col21"].ToString();
                    _gesSupervisionBOL.sup_col22 = DtDatos.Rows[0]["sup_col22"].ToString();
                    _gesSupervisionBOL.sup_col23 = DtDatos.Rows[0]["sup_col23"].ToString();
                    _gesSupervisionBOL.sup_col24 = DtDatos.Rows[0]["sup_col24"].ToString();
                    _gesSupervisionBOL.sup_col25 = DtDatos.Rows[0]["sup_col25"].ToString();
                    _gesSupervisionBOL.sup_col26 = DtDatos.Rows[0]["sup_col26"].ToString();
                    _gesSupervisionBOL.sup_col27 = DtDatos.Rows[0]["sup_col27"].ToString();
                    _gesSupervisionBOL.sup_col28 = DtDatos.Rows[0]["sup_col28"].ToString();
                    _gesSupervisionBOL.sup_col29 = DtDatos.Rows[0]["sup_col29"].ToString();
                    _gesSupervisionBOL.sup_col30 = DtDatos.Rows[0]["CodCuestionario"].ToString();
                }

                

                GesSupervisionDAL gesSupervisionDAL = new GesSupervisionDAL();

                int error = 0;
                if (tipoLev.Equals("1")) { 
                    for (int i = 2; i <= 59; i++)
                    {
                        if (DtDatos.Rows[0][i].ToString() == "-1" || DtDatos.Rows[0][i].ToString() == "")
                        {
                            if (i == 14 || i == 15 || i == 34 || i == 35) {
                                error = error + 1;
                            } else if ((i >= 26 && i <= 28) && DtDatos.Rows[0]["sup_col14"].ToString().Equals("1")) {
                                error = error + 1;
                            } else if (i == 41 && DtDatos.Rows[0]["sup_col34"].ToString().Equals("1")) {
                                error = error + 1;
                            } else if (i == 42 && DtDatos.Rows[0]["sup_col40"].ToString().Equals("2")) {
                                error = error + 1;
                            } else if ((i >= 43 && i <= 46) && DtDatos.Rows[0]["sup_col34"].ToString().Equals("2")) {
                                error = error + 1;
                            } else if (i == 47 && DtDatos.Rows[0]["sup_col40"].ToString().Equals("2")) {
                                error = error + 1;
                            } else if (i == 48 && (DtDatos.Rows[0]["sup_col46"].ToString().Equals("2") || DtDatos.Rows[0]["sup_col46"].ToString().Equals("3"))) {
                                error = error + 1;
                            } else if (i == 49 && DtDatos.Rows[0]["sup_col47"].ToString().Equals("1")) {
                                error = error + 1;
                            } else if ((i >= 56 && i <= 59) && DtDatos.Rows[0]["sup_col40"].ToString().Equals("1")) {
                                error = error + 1;
                            }
                            //error = error + 1;
                        }
                    }
                } else if (tipoLev.Equals("2")) { 

                    for (int i = 2; i <= 30; i++)
                    {
                        if (DtDatos.Rows[0][i].ToString() == "-1" || DtDatos.Rows[0][i].ToString() == "")
                        {
                            if (i >= 9 && i <= 14) {
                                error = error + 1;
                            } else if (i == 15 && DtDatos.Rows[0]["sup_col13"].ToString().Equals("1")) {
                                error = error + 1;
                            } else if (i == 16 && DtDatos.Rows[0]["sup_col14"].ToString().Equals("2")) {
                                error = error + 1;
                            } else if ((i == 17 || i == 18) && DtDatos.Rows[0]["sup_col13"].ToString().Equals("2")) {
                                error = error + 1;
                            } else if ((i >= 19 && i <= 21) && DtDatos.Rows[0]["sup_col14"].ToString().Equals("2") ) {
                                error = error + 1;
                            } else if ((i >= 27 && i <= 30) && DtDatos.Rows[0]["sup_col14"].ToString().Equals("1") ) {
                                error = error + 1;
                            }
                            //error = error + 1;
                        }
                    }
                }
                
                DataSet DsAsigSelect = new DataSet();
                if (error == 0)
                {
                    DsAsigSelect = gesSupervisionDAL.InsertarSupervision(_gesSupervisionBOL);
                    sb.Append("<div class=\"text-center correcto\" style=\"\">");
                    sb.Append("<div class=\"alert alert-success alert-dismissable\">");
                    sb.Append("<button aria-hidden=\"true\" data-dismiss=\"alert\"  class=\"close\" type=\"button\">×</button>");
                    sb.Append("¡Supervisión realizada con éxito!");
                    sb.Append("</div>");
                    sb.Append("</div>");
                }
                else
                {
                    sb.Append("<div class=\"text-center error\" style=\"\">");
                    sb.Append("<div class=\"alert alert-danger alert-dismissable\">");
                    sb.Append("<button aria-hidden=\"true\" data-dismiss=\"alert\"  class=\"close\" type=\"button\">×</button>");
                    sb.Append("Favor completar todos los campos.");
                    sb.Append("</div>");
                    sb.Append("</div>");
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
        /// Permite asignar supervisiones indirectas manualmente
        /// </summary>
        public string InsertarAsignacionIndirectaManual(string guid, int estado_id)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                _strHtml = "";
                string retorno = "";

                GesSupervisionDAL gesSupervisionDAL = new GesSupervisionDAL();
                gesSupervisionDAL.InsertarAsignacionIndirectaManual(guid, estado_id);

                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        /// <summary>
        /// Permite mostrar formulario de supervision directa
        /// </summary>
        public string MuestraFormSupervisionDirecta(string guid)
        {
            try
            {
                GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL();
                GesUsuarioBLL _gesUsuarioBLL = new GesUsuarioBLL();
                _gesUsuarioBOL = _gesUsuarioBLL.ObtieneUsuarioConectado(_appSettings.ObtieneCookie());


                GesAsignacionesDAL gesAsignacionesDAL = new GesAsignacionesDAL();
                DataSet DsDatosUsu = new DataSet();
                DsDatosUsu = gesAsignacionesDAL.ListaDatosUsuario(_gesUsuarioBOL);

                var perfil_usuario = DsDatosUsu.Tables[0].Rows[0]["perfil_id"].ToString();

                StringBuilder sb = new StringBuilder();
                _strHtml = "";

                GesSupervisionDAL gesSupervisionDAL = new GesSupervisionDAL();

                DataTable DtGrupo = new DataTable();
                DataSet DsGrupo = new DataSet();
                DsGrupo = gesSupervisionDAL.ObtieneDatosPrecargados(guid,1,1);

                // Obtengo información de la supervisión
                DataTable DtSupervision = new DataTable();
                DataSet DsSupervision = new DataSet();
                DsSupervision = gesSupervisionDAL.ListaSupervisionPorGuid(guid, 1);

                string sup_col = "";
                string sup_col1 = "";
                string sup_col2 = "";
                string codCuest = "";


                string contenido = "";
                string checks = "";
                string campos = "";


                if (DsGrupo.Tables[0].Rows.Count > 0)
                {
                    DtGrupo = DsGrupo.Tables[0];


                    string nomSupervisor = DtGrupo.Rows[0]["NombreSupervisor"].ToString();
                    string nomCensista = DtGrupo.Rows[0]["NombreCensista"].ToString();
                    string rutCensista = DtGrupo.Rows[0]["RutCensista"].ToString();
                    string nomComuna = DtGrupo.Rows[0]["NombreComuna"].ToString();
                    string numDistrito = DtGrupo.Rows[0]["Distrito"].ToString();
                    string numArea = DtGrupo.Rows[0]["Area"].ToString();
                    string idALC = DtGrupo.Rows[0]["ALC_id"].ToString();
                    string nomArea = "";
                    string estadoCuest = "0";



                    if (numArea.Equals("1")) { nomArea = "Urbana"; } else { nomArea = "Rural"; }
                    if (string.IsNullOrEmpty(idALC)) { idALC = "0"; }

                    // Genero funcion para insertar y eliminar asignaciones
                    PostJSON _postJSONInsertaSupervision = new PostJSON();
                    {
                        _postJSONInsertaSupervision.P_form = "supervision-directa";
                        _postJSONInsertaSupervision.P_url_servicio = _appSettings.ServidorWeb + "api/supervision/insertar-supervision-directa";
                        _postJSONInsertaSupervision.P_data_adicional = "parametro: 0";
                        _postJSONInsertaSupervision.P_data_dinamica = true;
                        _postJSONInsertaSupervision.P_respuesta_servicio = "$('.mensaje').html(respuesta[0].elemento_html);" +
                                                                           "setTimeout(function () { muestraListaMovil(" + idALC + ", 1,1); }, 1000);";
                    }

                    if (DsSupervision.Tables[0].Rows.Count > 0) {

                        sup_col1 = DsSupervision.Tables[0].Rows[0]["sup_col1"].ToString();
                        sup_col2 = DsSupervision.Tables[0].Rows[0]["sup_col2"].ToString();
                        codCuest = DsSupervision.Tables[0].Rows[0]["sup_col59"].ToString();
                        for (int i = 3; i < 59; i++)
                        {
                            sup_col = "sup_col" + i;
                            
                            if ((i >= 3 && i <= 12) || (i >= 15 && i <= 24) || (i >= 28 && i <= 32) || (i >= 35 && i <= 39) || (i >= 50 && i <= 54)) {
                                //if (DsSupervision.Tables[0].Rows[0][sup_col].ToString().Equals("1")) {
                                //    contenido = contenido + "$('#" + sup_col + "').prop('checked',true);";
                                //}
                            } else {
                                contenido = contenido + "$('#" + sup_col + "').val('" + DsSupervision.Tables[0].Rows[0][sup_col].ToString() + "'); ";
                            }
                            
                        }

                        string col_14 = DsSupervision.Tables[0].Rows[0]["sup_col14"].ToString();
                        string col_34 = DsSupervision.Tables[0].Rows[0]["sup_col34"].ToString();
                        string col_40 = DsSupervision.Tables[0].Rows[0]["sup_col40"].ToString();
                        string col_46 = DsSupervision.Tables[0].Rows[0]["sup_col46"].ToString();
                        string col_47 = DsSupervision.Tables[0].Rows[0]["sup_col47"].ToString();
                        
                        estadoCuest = DsSupervision.Tables[0].Rows[0]["IdEstadoSupervision"].ToString();

                        if (col_14.Equals("1")) {
                            campos = campos + "$('.ubic_rural').hide();" + "$('.ubic_xy').show();" + "$('.obs_manz').show();" + "$('.obs_topo').show();" + "$('.obs_cant').show();" + "$('.edit_xy').show();" + "$('.obs_iden').show();";
                        } else if (col_14.Equals("2")){
                            campos = campos + "$('.ubic_xy').hide();" + "$('.obs_manz').hide();" + "$('.obs_topo').hide();" + "$('.obs_cant').hide();" + "$('.ubic_rural').show();" + "$('.edit_xy').show();" + "$('.obs_iden').show();";
                        }

                        if (col_34.Equals("1")) {
                            campos = campos + "$('.sec_no_cont').hide();" + "$('.ver_oc').hide();" + "$('.inf_adic').hide();" + "$('.inf_cont').hide();" + "$('.obs_nocon').hide();";
                            campos = campos + "$('.prot_pres').show();" + "$('.coop_inf').show();";
                        } else if (col_34.Equals("2")) {
                            campos = campos + "$('.prot_pres').hide();" + "$('.coop_inf').hide();" + "$('.obs_conta').hide();";
                            campos = campos + "$('.sec_no_cont').show();" + "$('.ver_oc').show();" + "$('.inf_adic').show();" + "$('.inf_cont').show();" + "$('.obs_nocon').show();";
                        }

                        if (col_40.Equals("1")) {
                            campos = campos + "$('.sec_agenda').hide();" + "$('.con_cita').hide();" + "$('.mod_rem').hide();" + "$('.reg_cont').hide();" + "$('.obs_agen').hide();"
                                            + "$('.sec_no_cont').hide();" + "$('.ver_oc').hide();" + "$('.inf_adic').hide();" + "$('.inf_cont').hide();" + "$('.obs_nocon').hide();";
                            campos = campos + "$('.sec_cuest').show();" + "$('.prot_cues').show();" +
                                                "$('.lee_enun').show(); $('.obs_cuest').show();" +
                                                "$('.sec_fin').show();" + "$('.inf_sup').show();" + "$('.obs_sup').show();";
                        } else if (col_40.Equals("2")) {
                            campos = campos + "$('.sec_cuest').hide();" + "$('.prot_cues').hide();" +
                                                "$('.lee_enun').hide(); $('.obs_cuest').hide(); " +
                                                 "$('.sec_fin').hide();" + "$('.inf_sup').hide();" + "$('.obs_sup').hide();";
                            campos = campos + "$('.obs_conta').show();" + "$('.sec_agenda').show();" + "$('.con_cita').show();";
                        }

                        if (col_46.Equals("1")) {
                            campos = campos + "$('.mod_rem').hide();" + "$('.reg_cont').show();" + "$('.obs_agen').show();";
                            campos = campos + "$('.sec_cuest').hide();" + "$('.prot_cues').hide();" +
                                                "$('.lee_enun').hide(); $('.obs_cuest').hide(); " +
                                                 "$('.sec_fin').hide();" + "$('.inf_sup').hide();" + "$('.obs_sup').hide();";
                        } else if (col_46.Equals("2") || col_46.Equals("3")) {
                            campos = campos + "$('.reg_cont').hide();" + "$('.mod_rem').show();";
                            campos = campos + "$('.sec_cuest').hide();" + "$('.prot_cues').hide();" +
                                                "$('.lee_enun').hide(); $('.obs_cuest').hide(); " +
                                                 "$('.sec_fin').hide();" + "$('.inf_sup').hide();" + "$('.obs_sup').hide();";
                        }

                        if (col_47.Equals("1")) {
                            campos = campos + "$('.reg_cont').show();" + "$('.obs_agen').show();" +
                                            "$('.sec_cuest').hide();" + "$('.prot_cues').hide();" +
                                                "$('.lee_enun').hide(); $('.obs_cuest').hide(); " +
                                                 "$('.sec_fin').hide();" + "$('.inf_sup').hide();" + "$('.obs_sup').hide();";
                        } else if (col_47.Equals("2") || col_47.Equals("3")) {
                            campos = campos + "$('.reg_cont').hide();" + "$('.obs_agen').show();" +
                                        "$('.sec_cuest').hide();" + "$('.prot_cues').hide();" +
                                                "$('.lee_enun').hide(); $('.obs_cuest').hide(); " +
                                                 "$('.sec_fin').hide();" + "$('.inf_sup').hide();" + "$('.obs_sup').hide();";
                        }

                    } else {
                        sup_col1 = DateTime.Now.ToString("yyyy-MM-dd");
                        sup_col2 = DateTime.Now.ToString("HH:mm");
                    }


                    // Genero metodo submit del formulario
                    CallMethod _methodCallLoad = new CallMethod
                    {
                        Mc_contenido =
                                        "$.getScript('" + _appSettings.ServidorWeb + "Framework/assets/js/plugins/iCheck/icheck.min.js', function () {" +
                                            "$('.i-checks').iCheck({ " +
                                                "checkboxClass: 'icheckbox_square-green'," +
                                                "radioClass: 'iradio_square-green'," +
                                            "});" +
                                        "}); " +
                                        _postJSONInsertaSupervision.PostJSONCall() +
                                        "setTimeout(function () { " +
                                            contenido +
                                            //"$('#sup_col2').val(" + sup_col2 + "); " +
                                            //"$('#sup_col3').val(" + sup_col3 + "); " +
                                            //"$('#sup_col4').val(" + sup_col4 + "); " +
                                        "}, 500);" +

                                        campos
                    };

                    CallMethod _methodCallSaltoCuestionario = new CallMethod
                    {
                        Mc_nombre = "saltoCuestionario(sup_col)",
                        Mc_contenido =  "console.log('sup_col: ' + sup_col);" +
                                        "if(sup_col == 14){" +
                                            "var col_14 = $('#sup_col14').val();" +
                                            "console.log('valor de col_14: ' + col_14);" +
                                            "if(col_14 == 1){" +
                                                "$('.ubic_rural').hide();" +
                                                "$('.ubic_xy').show();" +
                                                "$('.obs_manz').show();" +
                                                "$('.obs_topo').show();" +
                                                "$('.obs_cant').show();" +
                                                "$('.edit_xy').show();" +
                                                "$('.obs_iden').show();" +
                                            // LAS OTRAS SECCIONES DEPENDEN DE CONT_INF ASÍ QUE NO HAY QUE MODIFICAR
                                            "}else if(col_14 == 2){" +
                                                "$('.ubic_xy').hide();" +
                                                "$('.obs_manz').hide();" +
                                                "$('.obs_topo').hide();" +
                                                "$('.obs_cant').hide();" +
                                                "$('.ubic_rural').show();" +
                                                "$('.edit_xy').show();" +
                                                "$('.obs_iden').show();" +
                                            "}else{" +
                                                "$('.ubic_xy').hide();" +
                                                "$('.obs_manz').hide();" +
                                                "$('.obs_topo').hide();" +
                                                "$('.obs_cant').hide();" +
                                                "$('.ubic_rural').hide();" +
                                                "$('.obs_reg').hide();" +
                                                "$('.edit_xy').hide();" +
                                                "$('.obs_iden').show();" +
                                            "}" +
                                        "}else if(sup_col == 34){" +
                                            "var col_34 = $('#sup_col34').val();" +
                                            "console.log('valor de col_34: ' + col_34);" +
                                            "if(col_34 == 1){" +
                                                // SE ESCONDE SECCIÓN DE NO CONTACTO Y SE MUESTRA PREGUNTA DE COOPERACIÓN
                                                "$('.sec_no_cont').hide();" +
                                                "$('.ver_oc').hide();" +
                                                "$('.inf_adic').hide();" +
                                                "$('.inf_cont').hide();" +
                                                "$('.obs_conta').hide();" +
                                                // SE ESCONDE TAMBIÉN SECCIÓN DE AGENDAMIENTO
                                                "$('.sec_agenda').hide();" +
                                                "$('.con_cita').hide();" +
                                                "$('.mod_rem').hide();" +
                                                "$('.reg_cont').hide();" +
                                                "$('.obs_agen').hide();" +
                                                // SE ESCONDE SECCIÓN DE APLICACIÓN CUESTIONARIO 
                                                "$('.sec_cuest').hide();" +
                                                "$('.prot_cues').hide();" +
                                                "$('.lee_enun').hide(); " +
                                                "$('.obs_cuest').hide();" +
                                                // SE ESCONDE TAMBIÉN SECCION CIERRE ENTREVISTA
                                                "$('.sec_fin').hide();" +
                                                "$('.inf_sup').hide();" +
                                                "$('.obs_sup').hide();" +
                                                // SE MUESTRAN LAS OTRAS PREGUNTAS EN LA SECCIÓN
                                                "$('.prot_pres').show();" +
                                                "$('.coop_inf').show();" +
                                                "$('.obs_conta').hide();" +
                                            "}else if(col_34 == 2){" +
                                                // SE ESCONDEN LAS PREGUNTAS DE LA SECCIÓN
                                                "$('.prot_pres').hide();" +
                                                "$('.coop_inf').hide();" +
                                                "$('.obs_conta').hide();" +
                                                // SE ESCONDE SECCIÓN DE APLICACIÓN CUESTIONARIO 
                                                "$('.sec_cuest').hide();" +
                                                "$('.prot_cues').hide();" +
                                                "$('.lee_enun').hide(); " +
                                                "$('.obs_cuest').hide();" +
                                                // SE ESCONDE TAMBIÉN SECCION CIERRE ENTREVISTA
                                                "$('.sec_fin').hide();" +
                                                "$('.inf_sup').hide();" +
                                                "$('.obs_sup').hide();" +
                                                // SE MUESTRA SECCIÓN DE NO CONTACTO Y PRIMERA PREGUNTA DE SECCIÓN AGENDA
                                                "$('.sec_no_cont').show();" +
                                                "$('.ver_oc').show();" +
                                                "$('.inf_adic').show();" +
                                                "$('.inf_cont').show();" +
                                                "$('.con_cita').show();" +
                                                "$('.obs_nocon').show();" +
                                                "$('.sec_agenda').show();" +
                                                "$('.con_cita').show();" +
                                            "}else{" +
                                                // SE ESCONDE TODO
                                                "$('.sec_no_cont').hide();" +
                                                "$('.ver_oc').hide();" +
                                                "$('.inf_adic').hide();" +
                                                "$('.inf_cont').hide();" +
                                                "$('.obs_conta').hide();" +
                                                "$('.sec_agenda').hide();" +
                                                "$('.con_cita').hide();" +
                                                "$('.mod_rem').hide();" +
                                                "$('.reg_cont').hide();" +
                                                "$('.obs_agen').hide();" +
                                                "$('.sec_cuest').hide();" +
                                                "$('.prot_cues').hide();" +
                                                "$('.lee_enun').hide(); " +
                                                "$('.obs_cuest').hide();" +
                                                "$('.sec_fin').hide();" +
                                                "$('.inf_sup').hide();" +
                                                "$('.obs_sup').hide();" +
                                                "$('.prot_pres').hide();" +
                                                "$('.coop_inf').hide();" +
                                                "$('.obs_conta').hide();" +
                                            "}" +
                                        "}else if(sup_col == 40){" +
                                            "var col_40 = $('#sup_col40').val();" +
                                            "console.log('valor de col_40: ' + col_40);" +
                                            "if(col_40 == 1){" +
                                                "$('.obs_conta').hide();" +
                                                // SE ESCONDEN SECCIONES NO CONTACTO Y AGENDAMIENTO  
                                                "$('.sec_no_cont').hide();" +
                                                "$('.ver_oc').hide();" +
                                                "$('.inf_adic').hide();" +
                                                "$('.inf_cont').hide();" +
                                                "$('.obs_nocon').hide();" +
                                                "$('.sec_agenda').hide();" +
                                                "$('.con_cita').hide();" +
                                                "$('.mod_rem').hide();" +
                                                "$('.reg_cont').hide();" +
                                                "$('.obs_agen').hide();" +
                                                // SE MUESTRA SECCIÓN DE APLICACIÓN CUESTIONARIO + SECCIÓN FINAL
                                                "$('.sec_cuest').show();" +
                                                "$('.prot_cues').show();" +
                                                "$('.lee_enun').show();" +
                                                "$('.obs_cuest').show();" +
                                                "$('.sec_fin').show();" +
                                                "$('.inf_sup').show();" +
                                                "$('.obs_sup').show();" +
                                            "}else if(col_40 == 2){" +
                                                "$('.obs_conta').show();" +
                                                // SE ESCONDE SECCIÓN DE NO CONTACTO Y APLICACIÓN 
                                                "$('.sec_no_cont').hide();" +
                                                "$('.ver_oc').hide();" +
                                                "$('.inf_adic').hide();" +
                                                "$('.inf_cont').hide();" +
                                                "$('.obs_nocon').hide();" +
                                                "$('.sec_cuest').hide();" +
                                                "$('.prot_cues').hide();" +
                                                "$('.lee_enun').hide();" +
                                                "$('.obs_cuest').hide();" +
                                                // SE ESCONDE TAMBIÉN SECCION CIERRE ENTREVISTA
                                                "$('.sec_fin').hide();" +
                                                "$('.inf_sup').hide();" +
                                                "$('.obs_sup').hide();" +
                                                // SE MUESTRA AGENDAMIENTO
                                                "$('.sec_agenda').show();" +
                                                "$('.con_cita').show();" +
                                                "$('.mod_rem').hide();" +
                                                "$('.reg_cont').hide();" +
                                                "$('.obs_agen').hide();" +
                                            "}else{" +
                                                "$('.obs_conta').hide();" +
                                                "$('.sec_no_cont').hide();" +
                                                "$('.ver_oc').hide();" +
                                                "$('.inf_adic').hide();" +
                                                "$('.inf_cont').hide();" +
                                                "$('.obs_nocon').hide();" +
                                                "$('.sec_cuest').hide();" +
                                                "$('.prot_cues').hide();" +
                                                "$('.lee_enun').hide();" +
                                                "$('.obs_cuest').hide();" +
                                                "$('.sec_fin').hide();" +
                                                "$('.inf_sup').hide();" +
                                                "$('.obs_sup').hide();" +
                                                "$('.sec_agenda').hide();" +
                                                "$('.con_cita').hide();" +
                                                "$('.mod_rem').hide();" +
                                                "$('.reg_cont').hide();" +
                                                "$('.obs_agen').hide();" +
                                            "}" +
                                        "}else if(sup_col == 46){" +
                                            "var col_46 = $('#sup_col46').val();" +
                                            "console.log('valor de col_46: ' + col_46);" +
                                            "if(col_46 == 1){" +
                                                // SE ESCONDE APLICACION DE CUESTIONARIO Y CIERRE
                                                "$('.sec_cuest').hide();" +
                                                "$('.prot_cues').hide();" +
                                                "$('.lee_enun').hide();" +
                                                "$('.obs_cuest').hide();" +
                                                "$('.sec_fin').hide();" +
                                                "$('.inf_sup').hide();" +
                                                "$('.obs_sup').hide();" +
                                                // SE SIGUE EN LA SECCIÓN
                                                "$('.mod_rem').hide();" +
                                                "$('.reg_cont').show();" +
                                                "$('.obs_agen').show();" +
                                            "}else if(col_46 == 2 || col_46 == 3){" +
                                                // SE ESCONDE APLICACION DE CUESTIONARIO Y CIERRE
                                                "$('.sec_cuest').hide();" +
                                                "$('.prot_cues').hide();" +
                                                "$('.lee_enun').hide();" +
                                                "$('.obs_cuest').hide();" +
                                                "$('.sec_fin').hide();" +
                                                "$('.inf_sup').hide();" +
                                                "$('.obs_sup').hide();" +
                                                // SE SIGUE EN LA SECCIÓN
                                                "$('.reg_cont').hide();" +
                                                "$('.mod_rem').show();" +
                                                "$('.obs_agen').show();" +
                                            "}" +
                                        "}else if(sup_col == 47){" +
                                            "var col_47 = $('#sup_col47').val();" +
                                            "console.log('valor de col_47: ' + col_47);" +
                                            "if(col_47 == 1){" +
                                                // SE ESCONDE APLICACION DE CUESTIONARIO Y CIERRE
                                                "$('.sec_cuest').hide();" +
                                                "$('.prot_cues').hide();" +
                                                "$('.lee_enun').hide();" +
                                                "$('.obs_cuest').hide();" +
                                                "$('.sec_fin').hide();" +
                                                "$('.inf_sup').hide();" +
                                                "$('.obs_sup').hide();" +
                                                //
                                                "$('.reg_cont').show();" +
                                                "$('.obs_agen').show();" +
                                            "}else if(col_47 == 2 || col_47 == 3){" +
                                                // SE ESCONDE APLICACION DE CUESTIONARIO Y CIERRE
                                                "$('.sec_cuest').hide();" +
                                                "$('.prot_cues').hide();" +
                                                "$('.lee_enun').hide();" +
                                                "$('.obs_cuest').hide();" +
                                                "$('.sec_fin').hide();" +
                                                "$('.inf_sup').hide();" +
                                                "$('.obs_sup').hide();" +
                                                //
                                                "$('.reg_cont').hide();" +
                                                "$('.obs_agen').show();" +
                                            "}" +
                                        "}"
                    };


                    sb.Append("<form id=\"" + _postJSONInsertaSupervision.P_form + "\" class=\"m-t\" method=\"post\">");
                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<h2 class=\"text-center\">OBSERVACIÓN DIRECTA - MÓVIL</h2>");
                    sb.Append("<hr />");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"alert alert-warning\">");
                    sb.Append("<h4 class=\"text-center\"><p>Coordinador/a de grupo: Recuerde que debe responder todas las preguntas del formulario de observación directa. Si tiene alguna duda sobre esto, revise en el Manual de Coordinador/a de Grupo el apartado sobre el proceso de observación directa, el que entrega una guía para el llenado del formulario según lo esperado por el proyecto. <br>" +
                                "Además, recuerde que los datos ingresados en el formulario <b>no podrán ser modificados</b> después de que guarde la información en el sistema de gestión. </p></h4>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\"><br></div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-4 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Comuna</strong></p>");
                    sb.Append("<input id=\"txt_supervisor\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomComuna + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Distrito</strong></p>");
                    sb.Append("<input id=\"10\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + numDistrito + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Área</strong></p>");
                    sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomArea + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    //sb.Append("<input id=\"IdTipoSupervision\" name=\"IdTipoSupervision\" type=\"hidden\" value=\"1\" />");
                    sb.Append("<input id=\"IdSupervision\" name=\"IdSupervision\" type=\"hidden\" value=\"" + DtGrupo.Rows[0]["IdDireccionSecundaria"] + "\" />");
                    //sb.Append("<input id=\"IdCensista\" name=\"IdCensista\" type=\"hidden\" value=\"" + rutCensista + "\" />");
                    sb.Append("<p><strong>Fecha <span style=\"color:red;\">*</span></strong></p>");
                    sb.Append("<input id=\"sup_col1\" name=\"sup_col1\" type=\"date\" class=\"form-control\" maxlength=\"8\" min=\"2021-01-01\" max=\"2023-12-31\" placeholder=\"\" value=\"" + sup_col1 + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Hora <span style=\"color:red;\">*</span></strong></p>");
                    sb.Append("<input id=\"sup_col2\" name=\"sup_col2\" type=\"text\" class=\"form-control\" maxlength=\"5\" pattern=\"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$\" title=\"Formato de hora es hh:mm. Ej: 16:25\" placeholder=\"hh:mm\" value=\"" + sup_col2 + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    
                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Nombre Censista</strong></p>");
                    sb.Append("<input id=\"txt_supervisor\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomCensista + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Nombre Coordinador de Grupo</strong></p>");
                    sb.Append("<input id=\"10\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomSupervisor + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    //sb.Append("<div class=\"col-lg-4 b-r\">");
                    //sb.Append("<div class=\"form-group\">");
                    //sb.Append("<p><strong>Abreviado</strong></p>");
                    //sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"\" disabled>");
                    //sb.Append("</div>");
                    //sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    //sb.Append("<div class=\"col-lg-6 b-r\">");
                    //sb.Append("<div class=\"form-group\">");
                    //sb.Append("<p><strong>GUID</strong></p>");
                    //sb.Append("<input id=\"9\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + DtGrupo.Rows[0]["IdDireccionSecundaria"] + "\" disabled>");
                    //sb.Append("</div>");
                    //sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-12 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Código Cuestionario</strong></p>");
                    sb.Append("<input type=\"text\" id=\"CodCuestionario\" name=\"CodCuestionario\" class=\"form-control\" maxlength=\"20\"  placeholder=\"Ingrese código\" value=\"" + codCuest + "\" required>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\"><hr></div>");

                    sb.Append("<div class=\"col-lg-12\"><br></div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    // SECCIÓN PREPARACIÓN DEL TRABAJO DE CAMPO
                    sb.Append("<div class=\"col-lg-12 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>PREPARACIÓN DEL TRABAJO DE CAMPO</strong></p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // REV_PREV
                    //sb.Append("<div class=\"col-lg-8 b-r\">");
                    //sb.Append("<div class=\"form-group\">");
                    //sb.Append("<p>¿LA PERSONA CENSISTA IDENTIFICA PREVIAMENTE SU ÁREA DE TRABAJO Y REVISA SUS MATERIALES ANTES DE SALIR A TERRENO?</p>");
                    //sb.Append("</div>");
                    //sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-7 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA IDENTIFICA PREVIAMENTE SU ÁREA DE TRABAJO Y REVISA SUS MATERIALES ANTES DE SALIR A TERRENO?</p>");
                    sb.Append("<div style=\"padding-top: 5px;padding-bottom: 5px;font-size: 12px;\"><span style=\"background-color:#0a3d62;color:#fff;padding:2px 3px 2px 3px;border-radius:2px;\">Importante:</span> Marcar sólo las opciones que la persona censista <b>sí</b> realizó.</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"\" class=\"filtro_Notas\">");
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_1\">");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("</span>");
                    sb.Append("<span class=\"label-text\"> 1) Ordena y revisa sus materiales antes de salir a terreno</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_2\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 2) Sincroniza el DMC</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_3\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 3) Revisa carga de DMC y cargador externo</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_4\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 4) Chequea carga listado de direcciones en el DMC</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_5\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 5) Revisa el área de levantamiento en el DMC y plano censal</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // UNI_CRED
                    sb.Append("<div class=\"col-lg-12 b-r\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-7 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA UTILIZA SU UNIFORME Y CREDENCIAL INSTITUCIONAL CORRECTAMENTE?</p>");
                    sb.Append("<div style=\"padding-top: 5px;padding-bottom: 5px;font-size: 12px;\"><span style=\"background-color:#0a3d62;color:#fff;padding:2px 3px 2px 3px;border-radius:2px;\">Importante:</span> Marcar sólo las opciones que la persona censista <b>sí</b> realizó.</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"\" class=\"filtro_Notas\">");
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_6\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 1) Utiliza la pechera institucional de forma visible</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_7\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 2) Utiliza la credencial institucional de forma visible</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_8\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 3) Utiliza el jockey institucional</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_9\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 4) Utiliza la mochila institucional</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_10\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 5) Utiliza los elementos de protección personal entregados por el proyecto (EPP)</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_PRE
                    sb.Append("<div class=\"col-lg-12 b-r\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-7 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>INGRESE OBSERVACIÓN </p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_11\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    // SECCIÓN IDENTIFICACIÓN Y OBSERVACIÓN
                    sb.Append("<div class=\"col-lg-12 b-r\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-12 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>IDENTIFICACIÓN Y OBSERVACIÓN</strong></p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // TIPO_AREA
                    sb.Append("<div class=\"col-lg-12 b-r tipo_area\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-7 b-r tipo_area\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA CARGA DE TRABAJO ASIGNADA CORRESPONDE AL ÁREA URBANA O RURAL CONCENTRADO?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r tipo_area\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_12\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // UBIC_XY
                    sb.Append("<div class=\"col-lg-7 b-r ubic_xy\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA IDENTIFICA Y SE UBICA EN EL PUNTO CORRECTO DE LA MANZANA PARA INICIAR EL RECORRIDO?</p>");
                    sb.Append("<div style=\"padding-top: 5px;padding-bottom: 5px;font-size: 12px;\"><span style=\"background-color:#0a3d62;color:#fff;padding:2px 3px 2px 3px;border-radius:2px;\">Importante:</span> Marcar sólo las opciones que la persona censista <b>sí</b> realizó.</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r ubic_xy\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"\" class=\"filtro_Notas\">");
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_13\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 1) Identifica la manzana que corresponde a su área de levantamiento</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_14\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 2) Identifica el punto norponiente de la manzana</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_15\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 3) Verifica el nombre de las calles de manzana en el DMC</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_16\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 4) Sigue su recorrido manteniéndo la línea de edificación con su hombro derecho</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_17\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 5) Verifica las numeraciones de las edificaciones en la manzana</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // UBIC_RURAL
                    sb.Append("<div class=\"col-lg-12 b-r ubic_rural\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-7 b-r ubic_rural\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA IDENTIFICA Y SE UBICA EN SU ÁREA DE LEVANTAMIENTO RURAL PARA INICIAR EL RECORRIDO?</p>");
                    sb.Append("<div style=\"padding-top: 5px;padding-bottom: 5px;font-size: 12px;\"><span style=\"background-color:#0a3d62;color:#fff;padding:2px 3px 2px 3px;border-radius:2px;\">Importante:</span> Marcar sólo las opciones que la persona censista <b>sí</b> realizó.</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r ubic_rural\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"\" class=\"filtro_Notas\">");
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_18\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 1) Identifica a la entidad que corresponde a su área de levantamiento</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_19\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 2) Identifica el punto de inicio </span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_20\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 3) Identifica el limite de la entidad </span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_21\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 4) Comienza el recorrido según el orden establecido por protocolo</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_22\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 5) Verifica que el punto XY este pre cargado correctamente</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_MANZ
                    sb.Append("<div class=\"col-lg-12 b-r obs_manz\" >&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-7 b-r obs_manz\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA IDENTIFICA Y AGREGA O ELIMINA NUEVAS CARAS DE MANZANA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r obs_manz\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_23\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_TOPO
                    sb.Append("<div class=\"col-lg-7 b-r obs_topo\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA IDENTIFICA Y ACTUALIZA CAMBIOS EN LA TOPONIMIA EN EL DMC?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r obs_topo\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_24\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_CANT
                    sb.Append("<div class=\"col-lg-7 b-r obs_cant\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA IDENTIFICA AUMENTOS O DISMINUCIONES EN LA CANTIDAD DE VIVIENDAS DE LA MANZANA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r obs_cant\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_25\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // EDIT_XY
                    sb.Append("<div class=\"col-lg-12 b-r edit_xy\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-7 b-r edit_xy\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA REGISTRA EL USO DE LA EDIFICACIÓN EN EL LISTADO DE DIRECCIONES Y SU GEORREFERENCIACIÓN  EL DMC?</p>");
                    sb.Append("<div style=\"padding-top: 5px;padding-bottom: 5px;font-size: 12px;\"><span style=\"background-color:#0a3d62;color:#fff;padding:2px 3px 2px 3px;border-radius:2px;\">Importante:</span> Marcar sólo las opciones que la persona censista <b>sí</b> realizó.</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r edit_xy\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"\" class=\"filtro_Notas\">");
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_26\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 1) Registra correctamente las coordenadas X/Y en el listado de direcciones</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_27\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 2) Identifica el uso de la edificación en el listado de direcciones</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_28\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 3) Identifica correctamente el destino de la edificación</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_29\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 4) Identifica correctamente el estado de ocupación de la vivienda</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_30\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 5) Indaga sobre la existencia de viviendas interiores</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_IDEN
                    sb.Append("<div class=\"col-lg-7 b-r obs_iden\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>INGRESE OBSERVACIÓN DE IDENTIFICACIÓN DE UNIDADES</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r obs_iden\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_31\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    // SECCIÓN CONTACTO Y COOPERACIÓN
                    sb.Append("<div class=\"col-lg-12 b-r sec_cont_coop\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-12 b-r sec_cont_coop\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>CONTACTO Y COOPERACIÓN</strong></p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // CONT_INF
                    sb.Append("<div class=\"col-lg-7 b-r cont_inf\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA LOGRA CONTACTARSE CON LOS RESIDENTES DE LA VIVIENDA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r cont_inf\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_32\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // PROT_PRES
                    sb.Append("<div class=\"col-lg-7 b-r prot_pres\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA APLICA EL PROTOCOLO DE PRESENTACIÓN CON LOS RESIDENTES DE LA VIVIENDA? </p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r prot_pres\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"\" class=\"filtro_Notas\">");
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_33\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 1) Se presenta como funcionario INE</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_34\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 2) Indica su nombre</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_35\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 3) Explica el contexto de la visita y el proyecto Censo</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_36\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 4) Informa sobre la confidencialidad de los datos y su uso</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_37\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 5) Se expresa de forma respetuosa</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // COOP_INF
                    sb.Append("<div class=\"col-lg-7 b-r coop_inf\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿EL CENSISTA LOGRA LA COOPERACIÓN DE LOS RESIDENTES DE LA VIVIENDA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r coop_inf\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_38\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_CONTA
                    sb.Append("<div class=\"col-lg-7 b-r obs_conta\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>INGRESE OBSERVACIÓN DE CONTACTO Y COOPERACIÓN</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r obs_conta\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_39\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    // SECCIÓN NO CONTACTO CON INFORMANTES
                    sb.Append("<div class=\"col-lg-12 b-r sec_no_cont\" >&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-12 b-r sec_no_cont\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>NO CONTACTO CON INFORMANTES</strong></p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // VER_OC
                    sb.Append("<div class=\"col-lg-7 b-r ver_oc\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA SOLICITA INFORMACIÓN A TERCEROS SOBRE EL HORARIO EN QUE PUEDEN ENCONTRAR A LOS RESIDENTES DE LA VIVIENDA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r ver_oc\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_40\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // INF_ADIC
                    sb.Append("<div class=\"col-lg-7 b-r inf_adic\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA REGISTRA LA INFORMACIÓN QUE ENTREGA TERCEROS EN EL DMC?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r inf_adic\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_41\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // INF_CONT
                    sb.Append("<div class=\"col-lg-7 b-r inf_cont\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA DEJA CARTA INFORMATIVA EN LA VIVIENDA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r inf_cont\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_42\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_NOCON
                    sb.Append("<div class=\"col-lg-7 b-r obs_nocon\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>INGRESE OBSERVACIÓN DE NO CONTACTO CON INFORMANTES</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r obs_nocon\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_43\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    // SECCIÓN AGENDAMIENTO Y MODALIDAD REMOTA
                    sb.Append("<div class=\"col-lg-12 b-r sec_agenda\" >&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-12 b-r sec_agenda\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>AGENDAMIENTO Y MODALIDAD REMOTA</strong></p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // CON_CITA
                    sb.Append("<div class=\"col-lg-7 b-r con_cita\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA  OFRECE REALIZAR LA ENTREVISTA EN OTRO MOMENTO E  INTENTA CONCERTA CITA CON EL INFORMANTE?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r con_cita\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_44\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // MOD_REM
                    sb.Append("<div class=\"col-lg-7 b-r mod_rem\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>EN CASO DE RECHAZO, ¿LA PERSONA CENSISTA EXPLICA Y OFRECE LA CARTA DE ACCESO A LAS MODALIDADES REMOTAS?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r mod_rem\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_45\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // REG_CONT
                    sb.Append("<div class=\"col-lg-7 b-r reg_cont\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA  SOLICITA Y REGISTRA INFORMACIÓN DE CONTACTO DEL INFORMANTE?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r reg_cont\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_46\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_AGEN
                    sb.Append("<div class=\"col-lg-7 b-r obs_agen\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>INGRESE OBSERVACIÓN DE AGENDAMIENTO MODALIDAD REMOTA</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r obs_agen\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_47\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    // SECCIÓN APLICACIÓN DEL CUESTIONARIO
                    sb.Append("<div class=\"col-lg-12 b-r sec_cuest\" >&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-12 b-r sec_cuest\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>APLICACIÓN DEL CUESTIONARIO</strong></p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // PROT_CUES
                    sb.Append("<div class=\"col-lg-7 b-r prot_cues\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA CUMPLE LOS PROTOCOLOS DE APLICACIÓN DE CUESTIONARIO CENSAL?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r prot_cues\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"\" class=\"filtro_Notas\">");
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_48\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 1) Identifica correctamente a los residentes habituales de la vivienda</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_49\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 2) Realiza todas las preguntas del cuestionario censal en el orden correspondiente y sin parafrasear</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_50\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 3) Lee todas las respuestas del cuestionario censal</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_51\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 4) Muestra una actitud amable durante el trascurso de la entrevista</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_52\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 5) Cumple con los protocolos sanitarios vigentes</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // LEE_ENUN
                    sb.Append("<div class=\"col-lg-7 b-r lee_enun\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA RESUELVE CORRECTAMENTE DUDAS Y/O CONSULTAS DE LOS INFORMANTES?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r lee_enun\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_53\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_CUEST
                    sb.Append("<div class=\"col-lg-7 b-r obs_cuest\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>INGRESE OBSERVACIÓN DE APLICACIÓN DE CUESTIONARIO</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r obs_cuest\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_54\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    // SECCIÓN CIERRE DE ENTREVISTA
                    sb.Append("<div class=\"col-lg-12 b-r sec_fin\" style=\"display:none;\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-12 b-r sec_fin\" style=\"display:none;\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>CIERRE DE ENTREVISTA</strong></p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // INF_SUP
                    sb.Append("<div class=\"col-lg-7 b-r inf_sup\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA SE DESPIDE DE LOS INFORMANTES CON AMABILIDAD Y AGRADECE EL TIEMPO ENTREGADO?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r inf_sup\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_55\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_SUP
                    sb.Append("<div class=\"col-lg-7 b-r obs_sup\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>INGRESE OBSERVACIONES SOBRE DIÁGNOSTICO</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r obs_sup\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_56\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<input id=\"IdTipoSupervision\" name=\"IdTipoSupervision\" type=\"hidden\" value=\"1\" />");
                    sb.Append("<input id=\"IdTipoLevantamiento\" name=\"IdTipoLevantamiento\" type=\"hidden\" value=\"1\" />");
                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-12 text-center mensaje\" style=\"\">");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\"><br></div>");

                    if ((perfil_usuario == "1" || perfil_usuario == "6") && !estadoCuest.Equals("1"))
                    {
                        sb.Append("<div class=\"col-lg-12 b-r\">");
                        sb.Append("<div class=\"form-group\">");
                        sb.Append("<button type=\"submit\" class=\"btn btn-primary btn-rounded btn-block\"><i class=\"fa fa-check\"></i> Guardar</button>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                    }
                    else
                    {
                        sb.Append("<div class=\"col-lg-12\"></div>");
                    }
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</form>");
                    sb.Append(_methodCallLoad.CreaJQueryDocumentReady());
                    sb.Append(_methodCallSaltoCuestionario.CreaJQueryFunction());
                    
                    _strHtml = sb.ToString();
                }
                else
                {
                    sb.Append("<div class=\"text-center\">No hay datos para mostrar.</div>");
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
        /// Permite mostrar formulario de supervision directa (CATI)
        /// </summary>
        public string MuestraFormSupervisionDirectaTel(string guid)
        {
            try
            {
                GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL();
                GesUsuarioBLL _gesUsuarioBLL = new GesUsuarioBLL();
                _gesUsuarioBOL = _gesUsuarioBLL.ObtieneUsuarioConectado(_appSettings.ObtieneCookie());

                GesAsignacionesDAL gesAsignacionesDAL = new GesAsignacionesDAL();
                DataSet DsDatosUsu = new DataSet();
                DsDatosUsu = gesAsignacionesDAL.ListaDatosUsuario(_gesUsuarioBOL);

                var perfil_usuario = DsDatosUsu.Tables[0].Rows[0]["perfil_id"].ToString();

                StringBuilder sb = new StringBuilder();
                _strHtml = "";

                GesSupervisionDAL gesSupervisionDAL = new GesSupervisionDAL();

                DataTable DtGrupo = new DataTable();
                DataSet DsGrupo = new DataSet();
                DsGrupo = gesSupervisionDAL.ObtieneDatosPrecargados(guid,1,3);

                // Obtengo información de la supervisión
                DataTable DtSupervision = new DataTable();
                DataSet DsSupervision = new DataSet();
                DsSupervision = gesSupervisionDAL.ListaSupervisionPorGuid(guid, 1);

                string sup_col = "";
                string sup_col1 = "";
                string sup_col2 = "";
                string codCuest = "";

                string contenido = "";
                string campos = "";


                if (DsGrupo.Tables[0].Rows.Count > 0)
                {
                    DtGrupo = DsGrupo.Tables[0];


                    string nomSupervisor = DtGrupo.Rows[0]["NombreSupervisor"].ToString();
                    string nomCensista = DtGrupo.Rows[0]["NombreCensista"].ToString();
                    string rutCensista = DtGrupo.Rows[0]["RutCensista"].ToString();
                    string nomComuna = DtGrupo.Rows[0]["NombreComuna"].ToString();
                    string numDistrito = DtGrupo.Rows[0]["Distrito"].ToString();
                    string numArea = DtGrupo.Rows[0]["Area"].ToString();
                    string idALC = DtGrupo.Rows[0]["ALC_id"].ToString();
                    string nomArea = "";
                    string estadoCuest = "0";



                    if (numArea.Equals("1")) { nomArea = "Urbana"; } else { nomArea = "Rural"; }
                    if (string.IsNullOrEmpty(idALC)) { idALC = "0"; }

                    // Genero funcion para insertar y eliminar asignaciones
                    PostJSON _postJSONInsertaSupervision = new PostJSON();
                    {
                        _postJSONInsertaSupervision.P_form = "supervision-directa";
                        _postJSONInsertaSupervision.P_url_servicio = _appSettings.ServidorWeb + "api/supervision/insertar-supervision-directa-tel";
                        _postJSONInsertaSupervision.P_data_adicional = "parametro: 0";
                        _postJSONInsertaSupervision.P_data_dinamica = true;
                        _postJSONInsertaSupervision.P_respuesta_servicio = "$('.mensaje').html(respuesta[0].elemento_html);" +
                                                                           "setTimeout(function () { muestraListaMovil(" + idALC + ", 1,3); }, 1000);";
                    }

                    if (DsSupervision.Tables[0].Rows.Count > 0) {

                        sup_col1 = DsSupervision.Tables[0].Rows[0]["sup_col1"].ToString();
                        sup_col2 = DsSupervision.Tables[0].Rows[0]["sup_col2"].ToString();
                        codCuest = DsSupervision.Tables[0].Rows[0]["sup_col30"].ToString();
                        for (int i = 3; i < 25; i++)
                        {
                            sup_col = "sup_col" + i;
                            //contenido = contenido + "$('#" + sup_col + "').val('" + DsSupervision.Tables[0].Rows[0][sup_col].ToString() + "'); ";
                            if ((i >= 3 && i <= 7) || (i >= 21 && i <= 25)) {
                                //if (DsSupervision.Tables[0].Rows[0][sup_col].ToString().Equals("1")) {
                                //    contenido = contenido + "$('#" + sup_col + "').prop('checked',true);";
                                //}
                            } else {
                                contenido = contenido + "$('#" + sup_col + "').val('" + DsSupervision.Tables[0].Rows[0][sup_col].ToString() + "'); ";
                            }
                        }

                        string col_13 = DsSupervision.Tables[0].Rows[0]["sup_col13"].ToString();
                        string col_14 = DsSupervision.Tables[0].Rows[0]["sup_col14"].ToString();
                        estadoCuest = DsSupervision.Tables[0].Rows[0]["IdEstadoSupervision"].ToString();

                        if (col_13.Equals("1")) {
                            campos = campos + "$('.coop_inf').show();" +
                                                "$('.inf_prot').hide();";
                        } else if (col_13.Equals("2")) {
                            campos = campos + "$('.coop_inf').hide();" +
                                                "$('.obs_ind').hide();" +
                                                "$('.inf_prot').show();" +
                                                "$('.obs_sin').show();" +
                                                "$('.sec_coop_inf').hide();" +
                                                "$('.pers_inf').hide();" +
                                                "$('.con_cita').hide();" +
                                                "$('.obs_noc').hide();" +
                                                "$('.sec_flujo_op').hide();" +
                                                "$('.apli_cuest').hide();" +
                                                "$('.cons_dud').hide();" +
                                                "$('.obs_cuest').hide();" +
                                                "$('.sec_fin').hide();" +
                                                "$('.desp_agra').hide();" +
                                                "$('.obs_sup').hide();";
                        }

                        if (col_14.Equals("1")) {
                            campos = campos + "$('.obs_ind').hide();" +
                                                "$('.sec_sin_acc').hide();" +
                                                "$('.inf_prot').hide();" +
                                                "$('.obs_sin').hide();" +
                                                "$('.sec_coop_inf').hide();" +
                                                "$('.pers_inf').hide();" +
                                                "$('.con_cita').hide();" +
                                                "$('.obs_noc').hide();" +
                                                "$('.apli_cuest').show();" +
                                                "$('.cons_dud').show();" +
                                                "$('.obs_cuest').show();" +
                                                "$('.sec_fin').show();" +
                                                "$('.desp_agra').show();" +
                                                "$('.obs_sup').show();";
                        } else if (col_14.Equals("2")) {
                            campos = campos + "$('.obs_ind').show();" +
                                                "$('.sec_sin_acc').hide();" +
                                                "$('.inf_prot').hide();" +
                                                "$('.obs_sin').hide();" +
                                                "$('.sec_coop_inf').show();" +
                                                "$('.pers_inf').show();" +
                                                "$('.con_cita').show();" +
                                                "$('.obs_noc').show();" +

                                                "$('.sec_flujo_op').hide();" +
                                                "$('.apli_cuest').hide();" +
                                                "$('.cons_dud').hide();" +
                                                "$('.obs_cuest').hide();" +
                                                "$('.sec_fin').hide();" +
                                                "$('.desp_agra').hide();" +
                                                "$('.obs_sup').hide();";
                        }


                    }
                    else {
                        sup_col1 = DateTime.Now.ToString("yyyy-MM-dd");
                        sup_col2 = DateTime.Now.ToString("HH:mm");
                    }


                    // Genero metodo submit del formulario
                    CallMethod _methodCallLoad = new CallMethod
                    {
                        Mc_contenido =
                                        "$.getScript('" + _appSettings.ServidorWeb + "Framework/assets/js/plugins/iCheck/icheck.min.js', function () {" +
                                            "$('.i-checks').iCheck({ " +
                                                "checkboxClass: 'icheckbox_square-green'," +
                                                "radioClass: 'iradio_square-green'," +
                                            "});" +
                                        "}); " +
                                        _postJSONInsertaSupervision.PostJSONCall() +
                                        "setTimeout(function () { " +
                                            contenido +
                                        "}, 500);" +
                                        campos
                    };

                    CallMethod _methodCallSaltoCuestionario = new CallMethod
                    {
                        Mc_nombre = "saltoCuestionario(sup_col)",
                        Mc_contenido = "console.log('sup_col: ' + sup_col);" +
                                        "if(sup_col == 13){" +
                                            "var col_13 = $('#sup_col13').val();" +
                                            "console.log('valor de col_11: ' + col_13);" +
                                            "if(col_13 == 1){" +
                                                "$('.coop_inf').show();" +
                                                //"$('.reso_dud').show();" +
                                                //"$('.coop_inf').show();" +
                                                //"$('.inf_prot').hide();" +
                                            "}else if(col_13 == 2){" +
                                                "$('.coop_inf').hide();" +
                                                "$('.obs_ind').hide();" +

                                                "$('.inf_prot').show();" +
                                                "$('.obs_sin').show();" +
                                                // SE TERMINA LA GESTIÓN POR LO QUE SE ESCONDE TODO EL RESTO
                                                "$('.sec_coop_inf').hide();" +
                                                "$('.pers_inf').hide();" +
                                                "$('.con_cita').hide();" +
                                                "$('.obs_noc').hide();" +
                                                "$('.sec_flujo_op').hide();" +
                                                "$('.apli_cuest').hide();" +
                                                "$('.cons_dud').hide();" +
                                                "$('.obs_cuest').hide();" +
                                                "$('.sec_fin').hide();" +
                                                "$('.desp_agra').hide();" +
                                                "$('.obs_sup').hide();" +
                                            "}else{" +
                                                "$('.vinc_rem').hide();" +
                                                "$('.reso_dud').hide();" +
                                                "$('.coop_inf').hide();" +
                                                "$('.inf_prot').hide();" +
                                                "$('.pers_inf').hide();" +
                                                "$('.con_cita').hide();" +
                                                "$('.lee_enun').hide();" +
                                                "$('.uso_sist').hide();" +
                                                "$('.prot_ent').hide();" +
                                                "$('.prot_cord').hide();" +
                                                "$('.ap_preg').hide();" +
                                                "$('.inf_sup').hide();" +
                                                "$('.desp_agra').hide();" +
                                                "$('.obs_sup').hide();" +
                                            "}" +
                                        "}else if(sup_col == 14){" +
                                            "var col_14 = $('#sup_col14').val();" +
                                            "console.log('valor de col_14: ' + col_14);" +
                                            "if(col_14 == 1){" +
                                                // SE ESCONDE SECCIÓN DE SIN ACCESO Y NO COOPERACION
                                                "$('.sec_sin_acc').hide();" +
                                                "$('.inf_prot').hide();" +
                                                "$('.obs_sin').hide();" +
                                                "$('.sec_coop_inf').hide();" +
                                                "$('.pers_inf').hide();" +
                                                "$('.con_cita').hide();" +
                                                "$('.obs_noc').hide();" +
                                                // SE MUESTRAN TODAS LAS OTRAS SECCIONES
                                                "$('.sec_flujo_op').show();" +
                                                "$('.apli_cuest').show();" +
                                                "$('.cons_dud').show();" +
                                                "$('.obs_cuest').show();" +
                                                "$('.sec_fin').show();" +
                                                "$('.desp_agra').show();" +
                                                "$('.obs_sup').show();" +
                                            "}else if(col_14 == 2){" +
                                                "$('.obs_ind').show();" +
                                                // SE ESCONDE SECCIÓN SIN ACCESO / FLUJO / CIERRE
                                                "$('.sec_sin_acc').hide();" +
                                                "$('.inf_prot').hide();" +
                                                "$('.obs_sin').hide();" +
                                                "$('.sec_flujo_op').hide();" +
                                                "$('.apli_cuest').hide();" +
                                                "$('.cons_dud').hide();" +
                                                "$('.obs_cuest').hide();" +
                                                "$('.sec_fin').hide();" +
                                                "$('.desp_agra').hide();" +
                                                "$('.obs_sup').hide();" +

                                                // SE MUESTRA SECCIÓN DE NO COOPERACION
                                                "$('.sec_coop_inf').show();" +
                                                "$('.pers_inf').show();" +
                                                "$('.con_cita').show();" +
                                                "$('.obs_noc').show();" +
                                            "}else{" +
                                                "$('.obs_ind').hide();" +
                                                "$('.sec_sin_acc').hide();" +
                                                "$('.inf_prot').hide();" +
                                                "$('.obs_sin').hide();" +
                                                "$('.sec_flujo_op').hide();" +
                                                "$('.apli_cuest').hide();" +
                                                "$('.cons_dud').hide();" +
                                                "$('.obs_cuest').hide();" +
                                                "$('.sec_fin').hide();" +
                                                "$('.desp_agra').hide();" +
                                                "$('.obs_sup').hide();" +
                                                "$('.sec_coop_inf').hide();" +
                                                "$('.pers_inf').hide();" +
                                                "$('.con_cita').hide();" +
                                                "$('.obs_noc').hide();" +
                                            "}" +
                                        "}"
                    };


                    sb.Append("<form id=\"" + _postJSONInsertaSupervision.P_form + "\" class=\"m-t\" method=\"post\">");
                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<h2 class=\"text-center\">OBSERVACIÓN DIRECTA - TELEFÓNICA</h2>");
                    sb.Append("<hr />");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"alert alert-warning\">");
                    sb.Append("<h4 class=\"text-center\"><p>Coordinador/a de grupo: Recuerde que debe responder todas las preguntas del formulario de observación directa. Si tiene alguna duda sobre esto, revise en el Manual de Coordinador/a de Grupo el apartado sobre el proceso de observación directa, el que entrega una guía para el llenado del formulario según lo esperado por el proyecto. <br>" +
                                "Además, recuerde que los datos ingresados en el formulario <b>no podrán ser modificados</b> después de que guarde la información en el sistema de gestión. </p></h4>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\"><br></div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-4 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Comuna</strong></p>");
                    sb.Append("<input id=\"txt_supervisor\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomComuna + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Distrito</strong></p>");
                    sb.Append("<input id=\"10\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + numDistrito + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Área</strong></p>");
                    sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomArea + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    //sb.Append("<input id=\"IdTipoSupervision\" name=\"IdTipoSupervision\" type=\"hidden\" value=\"1\" />");
                    
                    sb.Append("<input id=\"IdSupervision\" name=\"IdSupervision\" type=\"hidden\" value=\"" + DtGrupo.Rows[0]["IdDireccionSecundaria"] + "\" />");
                    //sb.Append("<input id=\"IdCensista\" name=\"IdCensista\" type=\"hidden\" value=\"" + rutCensista + "\" />");
                    sb.Append("<p><strong>Fecha <span style=\"color:red;\">*</span></strong></p>");
                    sb.Append("<input id=\"sup_col1\" name=\"sup_col1\" type=\"date\" class=\"form-control\" maxlength=\"8\" min=\"2021-01-01\" max=\"2023-12-31\" placeholder=\"\" value=\"" + sup_col1 + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Hora <span style=\"color:red;\">*</span></strong></p>");
                    sb.Append("<input id=\"sup_col2\" name=\"sup_col2\" type=\"text\" class=\"form-control\" maxlength=\"5\" pattern=\"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$\" title=\"Formato de hora es hh:mm. Ej: 16:25\" placeholder=\"hh:mm\" value=\"" + sup_col2 + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Nombre Censista</strong></p>");
                    sb.Append("<input id=\"txt_supervisor\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomCensista + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Nombre Coordinador de Grupo</strong></p>");
                    sb.Append("<input id=\"10\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomSupervisor + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    //sb.Append("<div class=\"col-lg-4 b-r\">");
                    //sb.Append("<div class=\"form-group\">");
                    //sb.Append("<p><strong>Abreviado</strong></p>");
                    //sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"\" disabled>");
                    //sb.Append("</div>");
                    //sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    //sb.Append("<div class=\"col-lg-6 b-r\">");
                    //sb.Append("<div class=\"form-group\">");
                    //sb.Append("<p><strong>GUID</strong></p>");
                    //sb.Append("<input id=\"9\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + DtGrupo.Rows[0]["IdDireccionSecundaria"] + "\" disabled>");
                    //sb.Append("</div>");
                    //sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-12 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Código Cuestionario</strong></p>");
                    sb.Append("<input type=\"text\" id=\"CodCuestionario\" name=\"CodCuestionario\" class=\"form-control\" maxlength=\"20\"  placeholder=\"Ingrese código\" value=\"" + codCuest + "\" required>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\"><hr></div>");

                    sb.Append("<div class=\"col-lg-12\"><br></div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    // SECCIÓN CONTACTO CON INFORMANTES
                    sb.Append("<div class=\"col-lg-12 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>CONTACTO CON INFORMANTES</strong></p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // CONT_INF
                    sb.Append("<div class=\"col-lg-7 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿EL O LA OPERADOR/A TELEFÓNICO APLICA CORRECTAMENTE EL PROTOCOLO DE PRESENTACIÓN?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"\" class=\"filtro_Notas\">");
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_1\">");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("</span>");
                    sb.Append("<span class=\"label-text\"> 1) Se presenta como funcionario INE</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_2\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 2) Indica su nombre</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_3\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 3) Explica el contexto  el proyecto Censo</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_4\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 4) Informa sobre la confidencialidad de los datos y su uso</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_5\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 5) Se expresa de forma respetuosa</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // REG_INF
                    sb.Append("<div class=\"col-lg-12 b-r\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-7 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿EL O LA OPERADOR/A TELEFÓNICO/A REGISTRA LA INFORMACIÓN DE CONTACTO DEL INFORMANTE EN EL SISTEMA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_6\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_PRE
                    sb.Append("<div class=\"col-lg-7 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>INGRESE OBSERVACIÓN CONTACTO CON LOS INFORMANTES</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_7\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // SECCIÓN INDAGACIÓN
                    sb.Append("<div class=\"col-lg-12 b-r sec_indag\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-12 b-r sec_indag\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>INDAGACIÓN</strong></p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // CENS_TERR
                    sb.Append("<div class=\"col-lg-12 b-r\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-7 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿EL O LA OPERADOR/A TELEFÓNICO/A PREGUNTA SI SU VIVIENDA FUE VISITADA POR UN CENSISTA EN TERRENO?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_8\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // VINC_REM
                    sb.Append("<div class=\"col-lg-7 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿EL O LA OPERADOR/A TELEFÓNICO/A  SOLICITA EL FOLIO DE LA CARTA INFORMATIVA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_9\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // RES_HAB
                    sb.Append("<div class=\"col-lg-7 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿El O LA OPERADOR/A TELEFÓNICO/A PREGUNTA AL INFORMANTE SI ES RESIDENTE HABITUAL DE LA VIVIENDA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_10\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // RES_VIV
                    sb.Append("<div class=\"col-lg-7 b-r \" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿EL INFORMANTE CUMPLE EL CRITERIO DE RESIDENTE HABITUAL?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r ubic_xy\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_11\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // COOP_INF
                    sb.Append("<div class=\"col-lg-7 b-r coop_inf\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿El O LA OPERADOR/A TELEFÓNICO/A LOGRA LA COOPERACIÓN DEL RESIDENTE  PARA REALIZAR LA ENTREVISTA CENSAL?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r coop_inf\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_12\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_IND
                    sb.Append("<div class=\"col-lg-7 b-r obs_ind\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿EL OPERADOR TELEFÓNICO LOGRA LA COOPERACIÓN DEL RESIDENTE  PARA REALIZAR LA ENTREVISTA CENSAL?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r obs_ind\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_13\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    // SECCIÓN SIN ACCESO A LA VINCULACIÓN REMOTA
                    sb.Append("<div class=\"col-lg-12 b-r sec_sin_acc\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-12 b-r sec_sin_acc\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>SIN ACCESO A LA VINCULACIÓN REMOTA</strong></p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // INF_PROT
                    sb.Append("<div class=\"col-lg-7 b-r inf_prot\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>EN CASO DE NO PODER VINCULAR LA VIVIENDA CON EL CUESTIONARIO CENSAL, ¿El O LA OPERADOR/A TELEFÓNICO/A INFORMA Y EXPLICA  CORRECTAMENTE AL INFORMANTE QUE DEBE HACER SEGÚN EL PROTOCOLO?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r inf_prot\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_14\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_SIN
                    sb.Append("<div class=\"col-lg-7 b-r obs_sin\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿EL OPERADOR TELEFÓNICO LOGRA LA COOPERACIÓN DEL RESIDENTE  PARA REALIZAR LA ENTREVISTA CENSAL?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r obs_sin\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_15\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    // SECCIÓN NO COOPERACIÓN DEL INFORMANTE
                    sb.Append("<div class=\"col-lg-12 b-r sec_coop_inf\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-12 b-r sec_coop_inf\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>NO COOPERACIÓN DEL INFORMANTE</strong></p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // PERS_INF
                    sb.Append("<div class=\"col-lg-7 b-r pers_inf\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿EL O LA OPERADOR/A TELEFÓNICO/A INTENTA PERSUADIR AL INFORMANTE DE MANERA RESPETUOSA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r pers_inf\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_16\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // CON_CITA
                    sb.Append("<div class=\"col-lg-7 b-r con_cita\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿EL O LA OPERADOR/A TELEFÓNICO/A OFRECE REALIZAR LA ENTREVISTA EN OTRO MOMENTO Y CONCERTA UNA CITA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r con_cita\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_17\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_NOC
                    sb.Append("<div class=\"col-lg-7 b-r obs_noc\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>INGRESE OBSERVACIÓN NO COOPERACIÓN CON EL INFORMANTE</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r obs_noc\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_18\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    // SECCIÓN FLUJO OPERATIVO TELEFÓNICO
                    sb.Append("<div class=\"col-lg-12 b-r sec_flujo_op\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-12 b-r sec_flujo_op\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>FLUJO OPERATIVO TELEFÓNICO</strong></p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // APLI_CUEST
                    sb.Append("<div class=\"col-lg-7 b-r apli_cuest\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿EL O LA OPERADOR/A TELEFÓNICO/A CUMPLE LOS PROTOCOLOS DE APLICACIÓN DE CUESTIONARIO CENSAL?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r apli_cuest\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"\" class=\"filtro_Notas\">");
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_19\">");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("</span>");
                    sb.Append("<span class=\"label-text\"> 1) Demuestra dominio en el uso del sistema de llamados y del cuestionario en la plataforma web</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_20\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 2) Demuestra dominio en el uso del cuestionario en la plataforma web</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_21\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 3) Realiza todas las preguntas del cuestionario censal en el orden correspondiente y sin parafrasear</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_22\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 4) Lee todas las respuestas del cuestionario censal</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    // check
                    sb.Append("<div class=\"form-check\">");
                    sb.Append("<label>");
                    sb.Append("<span id=\"filtro_Notas_23\"></span>");
                    //sb.Append("<input type=\"checkbox\" name='chk_perfil[]' id='chk_perfil_id' class='i-checks chk_perfil' value=" + 1 + ">");
                    sb.Append("<span class=\"label-text\"> 5) Mantiene cordialidad y disposición en la entrevista censal</span>");
                    sb.Append("</label>");
                    sb.Append("</div>");
                    // /check
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // CONS_DUD
                    sb.Append("<div class=\"col-lg-7 b-r cons_dud\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿EL O LA OPERADOR/A TELEFÓNICO/A  RESUELVE CORRECTAMENTE DUDAS Y/O CONSULTAS DE LOS INFORMANTES?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r cons_dud\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_24\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_CUEST
                    sb.Append("<div class=\"col-lg-7 b-r obs_cuest\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>INGRESE OBSERVACIÓN APLICACIÓN DEL CUESTIONARIO</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r obs_cuest\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_25\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    // SECCIÓN CIERRE DE ENTREVISTA
                    sb.Append("<div class=\"col-lg-12 b-r sec_fin\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-12 b-r sec_fin\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>CIERRE DE ENTREVISTA</strong></p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // DESP_AGRA
                    sb.Append("<div class=\"col-lg-7 b-r desp_agra\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿EL O LA OPERADOR/A TELEFÓNICO/A  SE DESPIDE DEL INFORMANTE  CON AMABILIDAD Y AGRADECE EL TIEMPO DISPUESTO EN LA ENTREVISTA CENSAL?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r desp_agra\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_26\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_SUP
                    sb.Append("<div class=\"col-lg-7 b-r obs_sup\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>INGRESE OBSERVACIONES SOBRE DIÁGNOSTICO</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-5 b-r obs_sup\" >");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_27\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<input id=\"IdTipoSupervision\" name=\"IdTipoSupervision\" type=\"hidden\" value=\"1\" />");
                    sb.Append("<input id=\"IdTipoLevantamiento\" name=\"IdTipoLevantamiento\" type=\"hidden\" value=\"2\" />");
                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-12 text-center mensaje\" style=\"\">");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\"><br></div>");

                    if ((perfil_usuario == "1" || perfil_usuario == "6") && !estadoCuest.Equals("1"))
                    {
                        sb.Append("<div class=\"col-lg-12 b-r\">");
                        sb.Append("<div class=\"form-group\">");
                        sb.Append("<button type=\"submit\" class=\"btn btn-primary btn-rounded btn-block\"><i class=\"fa fa-check\"></i> Guardar</button>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                    }
                    else
                    {
                        sb.Append("<div class=\"col-lg-12\"></div>");
                    }
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</form>");
                    sb.Append(_methodCallLoad.CreaJQueryDocumentReady());
                    sb.Append(_methodCallSaltoCuestionario.CreaJQueryFunction());

                    _strHtml = sb.ToString();
                }
                else
                {
                    sb.Append("<div class=\"text-center\">No hay datos para mostrar.</div>");
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
        /// Permite mostrar formulario de supervision indirecta
        /// </summary>
        public string MuestraFormSupervisionIndirectaMovil(string guid)
        {
            try
            {
                GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL();
                GesUsuarioBLL _gesUsuarioBLL = new GesUsuarioBLL();
                _gesUsuarioBOL = _gesUsuarioBLL.ObtieneUsuarioConectado(_appSettings.ObtieneCookie());

                GesAsignacionesDAL gesAsignacionesDAL = new GesAsignacionesDAL();
                DataSet DsDatosUsu = new DataSet();
                DsDatosUsu = gesAsignacionesDAL.ListaDatosUsuario(_gesUsuarioBOL);

                var perfil_usuario = DsDatosUsu.Tables[0].Rows[0]["perfil_id"].ToString();

                StringBuilder sb = new StringBuilder();
                _strHtml = "";

                GesSupervisionDAL gesSupervisionDAL = new GesSupervisionDAL();

                DataTable DtGrupo = new DataTable();
                DataSet DsGrupo = new DataSet();
                DsGrupo = gesSupervisionDAL.ObtieneDatosPrecargados(guid,2,1);

                // Obtengo información de la supervisión
                DataTable DtSupervision = new DataTable();
                DataSet DsSupervision = new DataSet();
                DsSupervision = gesSupervisionDAL.ListaSupervisionPorGuid(guid, 2);

                string sup_col = "";
                string sup_col1 = "";
                string sup_col2 = "";
                string gastos = "";
                

                string contenido = "";
                string campos = "";


                if (DsGrupo.Tables[0].Rows.Count > 0)
                {
                    DtGrupo = DsGrupo.Tables[0];


                    string nomSupervisor = DtGrupo.Rows[0]["NombreSupervisor"].ToString();
                    string nomCensista = DtGrupo.Rows[0]["NombreCensista"].ToString();
                    string rutCensista = DtGrupo.Rows[0]["RutCensista"].ToString();
                    string nomComuna = DtGrupo.Rows[0]["NombreComuna"].ToString();
                    string numDistrito = DtGrupo.Rows[0]["Distrito"].ToString();
                    string numArea = DtGrupo.Rows[0]["Area"].ToString();
                    string idALC = DtGrupo.Rows[0]["ALC_id"].ToString();
                    string informante = DtGrupo.Rows[0]["Reslev_nombre_informante"].ToString();
                    string cant_per = DtGrupo.Rows[0]["Reslev_total_per"].ToString();
                    string cant_hog = DtGrupo.Rows[0]["Reslev_cant_hog"].ToString();
                    //string datos_inf = DtGrupo.Rows[0]["datos"].ToString();
                    string nomArea = "";
                    string estadoCuest = "0";
                    string tabla_sexo = "<table class=\"table table-hover table-striped\"><tbody>";
                    string tabla_edad = "<table class=\"table table-hover table-striped\"><tbody>";
                    string tabla_parent = "<table class=\"table table-hover table-striped\"><tbody>";

                    if (!string.IsNullOrEmpty(cant_hog)) {
                        if (Convert.ToInt32(cant_hog) > 1) {
                            gastos = "No";
                        }
                        else {
                            gastos = "Sí";
                        }
                    } else {
                        gastos = "Sin información";
                    }

                    if (DsGrupo.Tables[1].Rows.Count > 0) {

                        foreach (DataRow row in DsGrupo.Tables[1].Rows)
                        {
                            
                            tabla_sexo = tabla_sexo + "<tr><td>" + row["PER_NOMBRE"] + "</td><td>" + row["SEXO_STR"] + "</td></tr>";
                            tabla_edad = tabla_edad + "<tr><td>" + row["PER_NOMBRE"] + "</td><td>" + row["EDAD"] + "</td></tr>";
                            tabla_parent = tabla_parent + "<tr><td>" + row["PER_NOMBRE"] + "</td><td>" + row["PARENTESCO_STR"] + "</td></tr>";
                        }

                        tabla_sexo = tabla_sexo + "</table>";
                        tabla_edad = tabla_edad + "</table>";
                        tabla_parent = tabla_parent + "</table>";

                    } else {
                        tabla_sexo = tabla_sexo + "<tr><td>Sin información</td></tr></table>";
                        tabla_edad = tabla_edad + "<tr><td>Sin información</td></tr></table>";
                        tabla_parent = tabla_parent + "<tr><td>Sin información</td></tr></table>";
                    }



                    if (numArea.Equals("1")) { nomArea = "Urbana"; } else { nomArea = "Rural"; }
                    if (string.IsNullOrEmpty(idALC)) { idALC = "0"; }
                    if (string.IsNullOrEmpty(informante)) { informante = "[INFORMANTE]"; }
                    //if (string.IsNullOrEmpty(datos_inf)) { 
                    //    informante = "[INFORMANTE]"; 
                    //}

                    // Genero funcion para insertar y eliminar asignaciones
                    PostJSON _postJSONInsertaSupervision = new PostJSON();
                    {
                        _postJSONInsertaSupervision.P_form = "supervision-indirecta-movil";
                        _postJSONInsertaSupervision.P_url_servicio = _appSettings.ServidorWeb + "api/supervision/insertar-supervision-indirecta-movil";
                        _postJSONInsertaSupervision.P_data_adicional = "parametro: 0";
                        _postJSONInsertaSupervision.P_data_dinamica = true;
                        _postJSONInsertaSupervision.P_respuesta_servicio = "$('.mensaje').html(respuesta[0].elemento_html);" +
                                                                           "setTimeout(function () { muestraListaMovil(" + idALC + ", 2,1); }, 1000);";
                    }

                    if (DsSupervision.Tables[0].Rows.Count > 0)
                    {

                        sup_col1 = DsSupervision.Tables[0].Rows[0]["sup_col1"].ToString();
                        sup_col2 = DsSupervision.Tables[0].Rows[0]["sup_col2"].ToString();
                        for (int i = 3; i < 14; i++)
                        {
                            sup_col = "sup_col" + i;
                            contenido = contenido + "$('#" + sup_col + "').val('" + DsSupervision.Tables[0].Rows[0][sup_col].ToString() + "'); ";
                        }

                        string col_4 = DsSupervision.Tables[0].Rows[0]["sup_col4"].ToString();
                        string col_5 = DsSupervision.Tables[0].Rows[0]["sup_col5"].ToString();
                        string col_14 = DsSupervision.Tables[0].Rows[0]["sup_col10"].ToString();
                        estadoCuest = DsSupervision.Tables[0].Rows[0]["IdEstadoSupervision"].ToString();

                        if (col_4.Equals("2")) {
                            campos = campos + "$('.res_viv').hide();" + "$('.tot_per').hide();" + "$('.pres_alim').hide();" + "$('.nac_sexo').hide();" + "$('.edad_act').hide();" + "$('.par_jefe').hide();" +
                                    "$('.cens_con').hide();" + "$('.dud_cuest').hide();" + "$('.comp_correc').hide();" + "$('.cump_prot').hide();" +
                                    "$('.obs_prot').hide();" + "$('.eval_cen').hide();" + "$('.obs_sup1').hide();";
                        }

                        if (col_5.Equals("1")) {
                            campos = campos + "$('.tot_per').show();" + "$('.pres_alim').show();" + "$('.nac_sexo').show();" + "$('.edad_act').show();" + "$('.par_jefe').show();" + "$('.obs_rph').show();" +
                                    "$('.cens_con').show();" + "$('.dud_cuest').show();" + "$('.comp_correc').show();" + "$('.cump_prot').show();";
                        } else {
                            campos = campos + "$('.tot_per').hide();" + "$('.pres_alim').hide();" + "$('.nac_sexo').hide();" + "$('.edad_act').hide();" + "$('.par_jefe').hide();" +
                                    "$('.cens_con').hide();" + "$('.dud_cuest').hide();" + "$('.comp_correc').hide();" + "$('.cump_prot').hide();" +
                                    "$('.obs_prot').hide();" + "$('.eval_cen').hide();" + "$('.obs_sup1').show();";
                        }

                        if (col_14.Equals("1") || col_14.Equals("3")) {
                            campos = campos + "$('.obs_prot').hide();" + "$('.eval_cen').show();" + "$('.obs_sup1').show();";
                        } else if (col_14.Equals("2")) {
                            campos = campos + "$('.obs_prot').show();" + "$('.eval_cen').show();" + "$('.obs_sup1').show();";
                        }

                    } else {
                        sup_col1 = DateTime.Now.ToString("yyyy-MM-dd");
                        sup_col2 = DateTime.Now.ToString("HH:mm");
                    }


                    // Genero metodo submit del formulario
                    CallMethod _methodCallLoad = new CallMethod
                    {
                        Mc_contenido = _postJSONInsertaSupervision.PostJSONCall() +
                                       "setTimeout(function () { " +
                                                contenido +
                                       //"$('#sup_col2').val(" + sup_col2 + "); " +
                                       //"$('#sup_col3').val(" + sup_col3 + "); " +
                                       //"$('#sup_col4').val(" + sup_col4 + "); " +
                                       "}, 500);" +
                                       campos
                    };

                    CallMethod _methodCallSaltoCuestionario = new CallMethod
                    {
                        Mc_nombre = "saltoCuestionario(sup_col)",
                        Mc_contenido = "console.log('sup_col: ' + sup_col);" +
                                        "if(sup_col == 4){" +
                                            "var col_4 = $('#sup_col4').val();" +
                                            "console.log('valor de col_4: ' + col_4);" +
                                            "if(col_4 == 1){" +
                                                "$('.res_viv').show();" +
                                                "$('.tot_per').hide();" +
                                                "$('.nac_sexo').hide();" +
                                                "$('.edad_act').hide();" +
                                                "$('.par_jefe').hide();" +
                                                "$('.cens_con').hide();" +
                                                "$('.dud_cuest').hide();" +
                                                "$('.comp_correc').hide();" +
                                                "$('.cump_prot').hide();" +
                                                "$('.obs_prot').hide();" +
                                                "$('.eval_cen').hide();" +
                                                "$('.obs_sup1').hide();" +
                                            "}else{" +
                                                "$('.res_viv').hide();" +
                                                "$('.tot_per').hide();" +
                                                "$('.nac_sexo').hide();" +
                                                "$('.edad_act').hide();" +
                                                "$('.par_jefe').hide();" +
                                                "$('.cens_con').hide();" +
                                                "$('.dud_cuest').hide();" +
                                                "$('.comp_correc').hide();" +
                                                "$('.cump_prot').hide();" +
                                                "$('.obs_prot').hide();" +
                                                "$('.eval_cen').hide();" +
                                                "$('.obs_sup1').hide();" +
                                            "}" +
                                        "}else if(sup_col == 5){" +
                                            "var col_5 = $('#sup_col5').val();" +
                                            "console.log('valor de col_5: ' + col_5);" +
                                            "if(col_5 == 1){" +
                                                // MOSTRAR INPUTS CON INFO PRECARGADA
                                                "$('.tot_per').show();" +
                                                "$('.nac_sexo').show();" +
                                                "$('.edad_act').show();" +
                                                "$('.par_jefe').show();" +
                                                "$('.obs_rph').show();" +
                                                "$('.cens_con').show();" +
                                                "$('.dud_cuest').show();" +
                                                "$('.comp_correc').show();" +
                                                "$('.cump_prot').show();" +

                                                "$('.obs_prot').hide();" +
                                                "$('.eval_cen').hide();" +
                                                "$('.obs_sup1').hide();" +
                                            "}else if(col_5 == 2){" +
                                                "$('.tot_per').hide();" +
                                                "$('.nac_sexo').hide();" +
                                                "$('.edad_act').hide();" +
                                                "$('.par_jefe').hide();" +
                                                "$('.cens_con').hide();" +
                                                "$('.dud_cuest').hide();" +
                                                "$('.comp_correc').hide();" +
                                                "$('.cump_prot').hide();" +
                                                "$('.obs_prot').hide();" +
                                                "$('.eval_cen').hide();" +
                                                "$('.obs_sup1').show();" +
                                            "}else{" +
                                                "$('.tot_per').hide();" +
                                                "$('.nac_sexo').hide();" +
                                                "$('.edad_act').hide();" +
                                                "$('.par_jefe').hide();" +
                                                "$('.obs_rph').hide();" +
                                                "$('.cens_con').hide();" +
                                                "$('.dud_cuest').hide();" +
                                                "$('.comp_correc').hide();" +
                                                "$('.cump_prot').hide();" +
                                                "$('.obs_prot').hide();" +
                                                "$('.eval_cen').hide();" +
                                                "$('.obs_sup1').hide();" +
                                            "}" +
                                        "}else if(sup_col == 14){" +
                                            "var col_14 = $('#sup_col14').val();" +
                                            "console.log('valor de col_14: ' + col_14);" +
                                            "if(col_14 == 1 || col_10 == 3){" +
                                                // SE MUESTRA SECCIÓN DE APLICACIÓN CUESTIONARIO + SECCIÓN FINAL Y SE ESCONDEN SECCIONES NO CONTACTO Y AGENDAMIENTO
                                                "$('.obs_prot').hide();" +
                                                "$('.eval_cen').show();" +
                                                "$('.obs_sup1').show();" +
                                            "}else if(col_14 == 2){" +
                                                "$('.eval_cen').show();" +
                                                "$('.obs_prot').show();" +
                                                "$('.obs_sup1').show();" +
                                            "}else{" +
                                                "$('.eval_cen').hide();" +
                                                "$('.obs_prot').hide();" +
                                                "$('.obs_sup1').hide();" +
                                            "}" +
                                        "}"
                    };


                    sb.Append("<form id=\"" + _postJSONInsertaSupervision.P_form + "\" class=\"m-t\" method=\"post\" >");
                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<h2 class=\"text-center\">OBSERVACIÓN INDIRECTA - MÓVIL</h2>");
                    sb.Append("<hr />");
                    sb.Append("</div>");

                    //sb.Append("<div class=\"col-lg-12\">");
                    //sb.Append("<div class=\"alert alert-warning\">");
                    //sb.Append("<h4 class=\"text-center\"><p>Supervisor: Recuerde que debe registrar un puntaje para cada aspecto señalado. Para esto es importante que revise el manual de <br>supervisión, el que entrega una guía para cada registro, según lo esperado por el proyecto. <br>" +
                    //                                                    "Recuerde, además, que los datos de esta pauta deben ser ingresados al sistema de gestión. </p></h4>");
                    //sb.Append("</div>");
                    //sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\"><br></div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-4 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Comuna</strong></p>");
                    sb.Append("<input id=\"txt_supervisor\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomComuna + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Distrito</strong></p>");
                    sb.Append("<input id=\"10\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + numDistrito + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Área</strong></p>");
                    sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomArea + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<input id=\"IdTipoSupervision\" name=\"IdTipoSupervision\" type=\"hidden\" value=\"2\" />");
                    
                    sb.Append("<input id=\"IdSupervision\" name=\"IdSupervision\" type=\"hidden\" value=\"" + DtGrupo.Rows[0]["IdDireccionSecundaria"] + "\" />");
                    //sb.Append("<input id=\"IdCensista\" name=\"IdCensista\" type=\"hidden\" value=\"" + rutCensista + "\" />");
                    sb.Append("<p><strong>Fecha <span style=\"color:red;\">*</span></strong></p>");
                    sb.Append("<input id=\"sup_col1\" name=\"sup_col1\" type=\"date\" class=\"form-control\" maxlength=\"8\" min=\"2021-01-01\" max=\"2023-12-31\"  value=\"" + sup_col1 + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Hora <span style=\"color:red;\">*</span></strong></p>");
                    sb.Append("<input id=\"sup_col2\" name=\"sup_col2\" type=\"text\" class=\"form-control\" maxlength=\"5\"  value=\"" + sup_col2 + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Nombre Censista</strong></p>");
                    sb.Append("<input id=\"txt_supervisor\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomCensista + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Nombre Coordinador de Grupo</strong></p>");
                    sb.Append("<input id=\"10\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomSupervisor + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    //sb.Append("<div class=\"col-lg-4 b-r\">");
                    //sb.Append("<div class=\"form-group\">");
                    //sb.Append("<p><strong>Abreviado</strong></p>");
                    //sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"\" disabled>");
                    //sb.Append("</div>");
                    //sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    //sb.Append("<div class=\"col-lg-6 b-r\">");
                    //sb.Append("<div class=\"form-group\">");
                    //sb.Append("<p><strong>GUID</strong></p>");
                    //sb.Append("<input id=\"9\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + DtGrupo.Rows[0]["IdDireccionSecundaria"] + "\" disabled>");
                    //sb.Append("</div>");
                    //sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-12 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Código Cuestionario</strong></p>");
                    sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\"><hr></div>");

                    sb.Append("<div class=\"col-lg-12\"><br></div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    // SECCIÓN PREPARACIÓN DEL TRABAJO DE CAMPO
                    //sb.Append("<div class=\"col-lg-12 b-r\">");
                    //sb.Append("<div class=\"form-group\">");
                    //sb.Append("<p><strong>SECCIÓN PREPARACIÓN DEL TRABAJO DE CAMPO</strong></p>");
                    //sb.Append("</div>");
                    //sb.Append("</div>");

                    // UBIC_XY1
                    sb.Append("<div class=\"col-lg-12 b-r\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-8 b-r ubic_xy1\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA REGISTRÓ CORRECTAMENTE EL USO DE LA EDIFICACIÓN EN EL LISTADO DE DIRECCIONES Y SU GEORREFERENCIACIÓN  EL DMC?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r ubic_xy1\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_1\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // VIV_CENS
                    sb.Append("<div class=\"col-lg-8 b-r viv_cens\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA VIVIENDA FUE CENSADA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r viv_cens\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_2\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // RES_VIV
                    sb.Append("<div class=\"col-lg-8 b-r res_viv\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿<b>" + informante + "</b> ES RESIDENTE DE LA VIVIENDA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r res_viv\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_3\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // TOT_PER
                    //sb.Append("<div class=\"col-lg-12 b-r\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-8 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿ME PODRÍA INDICAR CUANTAS PERSONAS RESIDEN EN LA VIVIENDA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div class=\"filtro_Notas\">");
                    sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + cant_per + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_PER
                    sb.Append("<div class=\"col-lg-8 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>OBSERVACIÓN CANTIDAD PERSONAS RESIDENTES DE LA VIVIENDA</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_4\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // PRES_ALIM
                    sb.Append("<div class=\"col-lg-8 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>DE LAS PERSONAS QUE RESIDEN EN LA VIVIENDA, ¿TODAS COMPARTEN PRESUPUESTO DE ALIMENTACIÓN?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div class=\"filtro_Notas\">");
                    sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + gastos + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_HOG
                    sb.Append("<div class=\"col-lg-8 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>OBSERVACIÓN DE LAS PERSONAS QUE COMPARTEN PRESUPUESTO DE ALIMENTACIÓN</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_5\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // NAC_SEXO
                    sb.Append("<div class=\"col-lg-8 b-r nac_sexo\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>PARA CADA RESIDENTE DE LA VIVIENDA: AL NACER, ¿CUÁL FUE SU SEXO? </p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r nac_sexo\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div  class=\"filtro_Notas\">");
                    sb.Append(tabla_sexo);
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_SE
                    sb.Append("<div class=\"col-lg-8 b-r nac_sexo\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>OBSERVACIÓN SEXO DE LOS RESIDENTES DE LA VIVIENDA</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r nac_sexo\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_6\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // EDAD_ACT
                    sb.Append("<div class=\"col-lg-8 b-r edad_act\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿CUÁNTOS AÑOS CUMPLIDOS TIENE RESIDENTE DE LA VIVIENDA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r edad_act\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div  class=\"filtro_Notas\">");
                    sb.Append(tabla_edad);
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // INF_ED
                    sb.Append("<div class=\"col-lg-8 b-r edad_act\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>OBSERVACIÓN AÑOS CUMPLIDOS RESIDENTES DE LA VIVIENDA</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r edad_act\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_7\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // PAR_RES
                    sb.Append("<div class=\"col-lg-8 b-r par_jefe\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿QUÉ PARENTESCO TIENE CADA RESIDENTE DE LA VIVIENDA CON EL JEFE O JEFA DE HOGAR?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r par_jefe\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div  class=\"filtro_Notas\">");
                    sb.Append(tabla_parent);
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_PAR
                    sb.Append("<div class=\"col-lg-8 b-r par_jefe\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>OBSERVACIÓN DE  PARENTESCO CON EL JEFE O JEFA DE HOGAR</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r par_jefe\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_8\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // CENS_CON
                    sb.Append("<div class=\"col-lg-8 b-r cens_con\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA LE EXPLICÓ EL CONTEXTO DEL PROYECTO CENSO?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r cens_con\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_9\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // DUD_CUEST
                    sb.Append("<div class=\"col-lg-8 b-r dud_cuest\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA REALIZÓ LAS PREGUNTAS Y RESOLVIÓ SUS DUDAS DE MANERA CORRECTA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r dud_cuest\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_10\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // COMP_CORREC
                    sb.Append("<div class=\"col-lg-8 b-r comp_correc\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA  SE COMPORTÓ DE FORMA CORRECTA DURANTE LA ENTREVISTA CENSAL?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r comp_correc\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_11\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // CUMP_PROT
                    sb.Append("<div class=\"col-lg-8 b-r cump_prot\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA CUMPLIÓ LOS PROTOCOLOS SANITARIOS VIGENTES? </p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r cump_prot\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_12\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_PROT
                    sb.Append("<div class=\"col-lg-8 b-r obs_prot\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>INDIQUE OBSERVACIONES DE LOS PROTOCOLOS</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r obs_prot\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_13\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // EVAL_CEN
                    sb.Append("<div class=\"col-lg-12 b-r eval_cen\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-8 b-r eval_cen\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿CÓMO EVALUARÍA EL TRABAJO DE LA PERSONA CENSISTA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r eval_cen\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_14\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_SUP1
                    sb.Append("<div class=\"col-lg-8 b-r obs_sup1\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>INGRESE OBSERVACIONES SOBRE SUPERVISIÓN</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r obs_sup1\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_15\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");


                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<input id=\"IdTipoLevantamiento\" name=\"IdTipoLevantamiento\" type=\"hidden\" value=\"1\" />");
                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-12 text-center mensaje\" style=\"\">");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\"><br></div>");

                    if ((perfil_usuario == "1" || perfil_usuario == "6") && !estadoCuest.Equals("1"))
                    {
                        sb.Append("<div class=\"col-lg-12 b-r\">");
                        sb.Append("<div class=\"form-group\">");
                        sb.Append("<button type=\"submit\" class=\"btn btn-primary btn-rounded btn-block\"><i class=\"fa fa-check\"></i> Guardar</button>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                    }
                    else
                    {
                        sb.Append("<div class=\"col-lg-12\"></div>");
                    }
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</form>");
                    sb.Append(_methodCallLoad.CreaJQueryDocumentReady());
                    sb.Append(_methodCallSaltoCuestionario.CreaJQueryFunction());

                    _strHtml = sb.ToString();
                }
                else
                {
                    sb.Append("<div class=\"text-center\">No hay datos para mostrar.</div>");
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
        /// Permite mostrar formulario de supervision indirecta
        /// </summary>
        public string MuestraFormSupervisionIndirectaWeb(string guid)
        {
            try
            {
                GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL();
                GesUsuarioBLL _gesUsuarioBLL = new GesUsuarioBLL();
                _gesUsuarioBOL = _gesUsuarioBLL.ObtieneUsuarioConectado(_appSettings.ObtieneCookie());

                GesAsignacionesDAL gesAsignacionesDAL = new GesAsignacionesDAL();
                DataSet DsDatosUsu = new DataSet();
                DsDatosUsu = gesAsignacionesDAL.ListaDatosUsuario(_gesUsuarioBOL);

                var perfil_usuario = DsDatosUsu.Tables[0].Rows[0]["perfil_id"].ToString();

                StringBuilder sb = new StringBuilder();
                _strHtml = "";

                GesSupervisionDAL gesSupervisionDAL = new GesSupervisionDAL();

                DataTable DtGrupo = new DataTable();
                DataSet DsGrupo = new DataSet();
                DsGrupo = gesSupervisionDAL.ObtieneDatosPrecargados(guid,2,2);

                // Obtengo información de la supervisión
                DataTable DtSupervision = new DataTable();
                DataSet DsSupervision = new DataSet();
                DsSupervision = gesSupervisionDAL.ListaSupervisionPorGuid(guid, 2);

                string sup_col = "";
                string sup_col1 = "";
                string sup_col2 = "";

                string contenido = "";
                string campos = "";


                if (DsGrupo.Tables[0].Rows.Count > 0)
                {
                    DtGrupo = DsGrupo.Tables[0];


                    string nomSupervisor = DtGrupo.Rows[0]["NombreSupervisor"].ToString();
                    string nomCensista = DtGrupo.Rows[0]["NombreCensista"].ToString();
                    string rutCensista = DtGrupo.Rows[0]["RutCensista"].ToString();
                    string nomComuna = DtGrupo.Rows[0]["NombreComuna"].ToString();
                    string numDistrito = DtGrupo.Rows[0]["Distrito"].ToString();
                    string numArea = DtGrupo.Rows[0]["Area"].ToString();
                    string idALC = DtGrupo.Rows[0]["ALC_id"].ToString();
                    string informante = DtGrupo.Rows[0]["Reslev_nombre_informante"].ToString();
                    string cant_per = DtGrupo.Rows[0]["Reslev_total_per"].ToString();
                    string cant_hog = DtGrupo.Rows[0]["Reslev_cant_hog"].ToString();
                    string nomArea = "";
                    string estadoCuest = "0";
                    string tabla_sexo = "<table class=\"table table-hover table-striped\"><tbody>";
                    string tabla_edad = "<table class=\"table table-hover table-striped\"><tbody>";
                    string tabla_parent = "<table class=\"table table-hover table-striped\"><tbody>";

                    if (DsGrupo.Tables[1].Rows.Count > 0) {

                        foreach (DataRow row in DsGrupo.Tables[1].Rows)
                        {
                            tabla_sexo = tabla_sexo + "<tr><td>" + row["PER_NOMBRE"] + "</td><td>" + row["SEXO_STR"] + "</td></tr>";
                            tabla_edad = tabla_edad + "<tr><td>" + row["PER_NOMBRE"] + "</td><td>" + row["EDAD"] + "</td></tr>";
                            tabla_parent = tabla_parent + "<tr><td>" + row["PER_NOMBRE"] + "</td><td>" + row["PARENTESCO_STR"] + "</td></tr>";
                        }

                        tabla_sexo = tabla_sexo + "</table>";
                        tabla_edad = tabla_edad + "</table>";
                        tabla_parent = tabla_parent + "</table>";

                    } else {
                        tabla_sexo = tabla_sexo + "<tr><td>Sin información</td></tr></table>";
                        tabla_edad = tabla_edad + "<tr><td>Sin información</td></tr></table>";
                        tabla_parent = tabla_parent + "<tr><td>Sin información</td></tr></table>";
                    }


                    if (numArea.Equals("1")) { nomArea = "Urbana"; } else { nomArea = "Rural"; }
                    if (string.IsNullOrEmpty(idALC)) { idALC = "0"; }

                    // Genero funcion para insertar y eliminar asignaciones
                    PostJSON _postJSONInsertaSupervision = new PostJSON();
                    {
                        _postJSONInsertaSupervision.P_form = "supervision-indirecta-web";
                        _postJSONInsertaSupervision.P_url_servicio = _appSettings.ServidorWeb + "api/supervision/insertar-supervision-indirecta-web";
                        _postJSONInsertaSupervision.P_data_adicional = "parametro: 0";
                        _postJSONInsertaSupervision.P_data_dinamica = true;
                        _postJSONInsertaSupervision.P_respuesta_servicio = "$('.mensaje').html(respuesta[0].elemento_html);" +
                                                                           "setTimeout(function () { muestraListaMovil(" + idALC + ", 2,2); }, 1000);";
                    }

                    if (DsSupervision.Tables[0].Rows.Count > 0)
                    {

                        sup_col1 = DsSupervision.Tables[0].Rows[0]["sup_col1"].ToString();
                        sup_col2 = DsSupervision.Tables[0].Rows[0]["sup_col2"].ToString();
                        for (int i = 3; i < 14; i++)
                        {
                            sup_col = "sup_col" + i;
                            contenido = contenido + "$('#" + sup_col + "').val('" + DsSupervision.Tables[0].Rows[0][sup_col].ToString() + "'); ";
                        }

                        string col_4 = DsSupervision.Tables[0].Rows[0]["sup_col4"].ToString();
                        string col_5 = DsSupervision.Tables[0].Rows[0]["sup_col5"].ToString();
                        estadoCuest = DsSupervision.Tables[0].Rows[0]["IdEstadoSupervision"].ToString();

                        if (col_4.Equals("2")) {
                            campos = campos + "$('.res_viv').hide();" + "$('.tot_per').hide();" + "$('.pres_alim').hide();" + "$('.nac_sexo').hide();" + "$('.edad_act').hide();" + "$('.par_jefe').hide();" +
                                     "$('.eval_plat').hide();" + "$('.obs_sup1').hide();";
                        }

                        if (col_5.Equals("1")) {
                            campos = campos + "$('.tot_per').show();" + "$('.nac_sexo').show();" + "$('.edad_act').show();" + "$('.par_jefe').show();" + "$('.pres_alim').show();" +
                                    "$('.eval_plat').show();" + "$('.obs_sup1').show();";
                        } else {
                            campos = campos + "$('.tot_per').hide();" + "$('.nac_sexo').hide();" + "$('.edad_act').hide();" + "$('.par_jefe').hide();" + "$('.obs_rph').hide();" +
                                    "$('.pres_alim').hide();" + "$('.eval_plat').hide();" + "$('.obs_sup1').show();";
                        }


                    }
                    else
                    {
                        sup_col1 = DateTime.Now.ToString("yyyy-MM-dd");
                        sup_col2 = DateTime.Now.ToString("HH:mm");
                    }


                    // Genero metodo submit del formulario
                    CallMethod _methodCallLoad = new CallMethod
                    {
                        Mc_contenido = _postJSONInsertaSupervision.PostJSONCall() +
                                       "setTimeout(function () { " +
                                                contenido +
                                       //"$('#sup_col2').val(" + sup_col2 + "); " +
                                       //"$('#sup_col3').val(" + sup_col3 + "); " +
                                       //"$('#sup_col4').val(" + sup_col4 + "); " +
                                       "}, 500);" +
                                       campos
                    };

                    CallMethod _methodCallSaltoCuestionario = new CallMethod
                    {
                        Mc_nombre = "saltoCuestionario(sup_col)",
                        Mc_contenido = "console.log('sup_col: ' + sup_col);" +
                                        "if(sup_col == 4){" +
                                            "var col_4 = $('#sup_col4').val();" +
                                            "console.log('valor de col_4: ' + col_4);" +
                                            "if(col_4 == 1){" +
                                                "$('.res_viv').show();" +
                                                "$('.tot_per').hide();" +
                                                "$('.nac_sexo').hide();" +
                                                "$('.edad_act').hide();" +
                                                "$('.par_jefe').hide();" +
                                                "$('.obs_rph').hide();" +
                                                "$('.eval_plat').hide();" +
                                                "$('.obs_sup1').hide();" +
                                            "}else{" +
                                                "$('.res_viv').hide();" +
                                                "$('.tot_per').hide();" +
                                                "$('.nac_sexo').hide();" +
                                                "$('.edad_act').hide();" +
                                                "$('.par_jefe').hide();" +
                                                "$('.obs_rph').hide();" +
                                                "$('.eval_plat').hide();" +
                                                "$('.obs_sup1').hide();" +
                                            "}" +
                                        "}else if(sup_col == 5){" +
                                            "var col_5 = $('#sup_col5').val();" +
                                            "console.log('valor de col_5: ' + col_5);" +
                                            "if(col_5 == 1){" +
                                                // MOSTRAR INPUTS CON INFO PRECARGADA
                                                "$('.tot_per').show();" +
                                                "$('.nac_sexo').show();" +
                                                "$('.edad_act').show();" +
                                                "$('.par_jefe').show();" +
                                                "$('.obs_rph').show();" +
                                                "$('.eval_plat').show();" +
                                                "$('.obs_sup1').show();" +
                                            "}else if(col_5 == 2){" +
                                                "$('.tot_per').hide();" +
                                                "$('.nac_sexo').hide();" +
                                                "$('.edad_act').hide();" +
                                                "$('.par_jefe').hide();" +
                                                "$('.obs_rph').show();" +
                                                "$('.eval_plat').show();" +
                                                "$('.obs_sup1').show();" +
                                            "}else{" +
                                                "$('.tot_per').hide();" +
                                                "$('.nac_sexo').hide();" +
                                                "$('.edad_act').hide();" +
                                                "$('.par_jefe').hide();" +
                                                "$('.obs_rph').hide();" +
                                                "$('.cens_con').hide();" +
                                                "$('.dud_cuest').hide();" +
                                                "$('.comp_correc').hide();" +
                                                "$('.cump_prot').hide();" +
                                                "$('.obs_prot').hide();" +
                                                "$('.eval_cen').hide();" +
                                                "$('.obs_sup1').hide();" +
                                            "}" +
                                        "}"
                    };


                    sb.Append("<form id=\"" + _postJSONInsertaSupervision.P_form + "\" class=\"m-t\" method=\"post\" >");
                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<h2 class=\"text-center\">OBSERVACIÓN INDIRECTA - WEB</h2>");
                    sb.Append("<hr />");
                    sb.Append("</div>");

                    //sb.Append("<div class=\"col-lg-12\">");
                    //sb.Append("<div class=\"alert alert-warning\">");
                    //sb.Append("<h4 class=\"text-center\"><p>Supervisor: Recuerde que debe registrar un puntaje para cada aspecto señalado. Para esto es importante que revise el manual de <br>supervisión, el que entrega una guía para cada registro, según lo esperado por el proyecto. <br>" +
                    //                                                    "Recuerde, además, que los datos de esta pauta deben ser ingresados al sistema de gestión. </p></h4>");
                    //sb.Append("</div>");
                    //sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\"><br></div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-4 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Comuna</strong></p>");
                    sb.Append("<input id=\"txt_supervisor\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomComuna + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Distrito</strong></p>");
                    sb.Append("<input id=\"10\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + numDistrito + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Área</strong></p>");
                    sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomArea + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<input id=\"IdTipoSupervision\" name=\"IdTipoSupervision\" type=\"hidden\" value=\"2\" />");

                    sb.Append("<input id=\"IdSupervision\" name=\"IdSupervision\" type=\"hidden\" value=\"" + DtGrupo.Rows[0]["IdDireccionSecundaria"] + "\" />");
                    //sb.Append("<input id=\"IdCensista\" name=\"IdCensista\" type=\"hidden\" value=\"" + rutCensista + "\" />");
                    sb.Append("<p><strong>Fecha <span style=\"color:red;\">*</span></strong></p>");
                    sb.Append("<input id=\"sup_col1\" name=\"sup_col1\" type=\"date\" class=\"form-control\" maxlength=\"8\" min=\"2021-01-01\" max=\"2023-12-31\"  value=\"" + sup_col1 + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Hora <span style=\"color:red;\">*</span></strong></p>");
                    sb.Append("<input id=\"sup_col2\" name=\"sup_col2\" type=\"text\" class=\"form-control\" maxlength=\"5\"  value=\"" + sup_col2 + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Nombre Censista</strong></p>");
                    sb.Append("<input id=\"txt_supervisor\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomCensista + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Nombre Coordinador de Grupo</strong></p>");
                    sb.Append("<input id=\"10\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomSupervisor + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    //sb.Append("<div class=\"col-lg-4 b-r\">");
                    //sb.Append("<div class=\"form-group\">");
                    //sb.Append("<p><strong>Abreviado</strong></p>");
                    //sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"\" disabled>");
                    //sb.Append("</div>");
                    //sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    //sb.Append("<div class=\"col-lg-6 b-r\">");
                    //sb.Append("<div class=\"form-group\">");
                    //sb.Append("<p><strong>GUID</strong></p>");
                    //sb.Append("<input id=\"9\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + DtGrupo.Rows[0]["IdDireccionSecundaria"] + "\" disabled>");
                    //sb.Append("</div>");
                    //sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-12 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Código Cuestionario</strong></p>");
                    sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\"><hr></div>");

                    sb.Append("<div class=\"col-lg-12\"><br></div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    // SECCIÓN PREPARACIÓN DEL TRABAJO DE CAMPO
                    //sb.Append("<div class=\"col-lg-12 b-r\">");
                    //sb.Append("<div class=\"form-group\">");
                    //sb.Append("<p><strong>SECCIÓN PREPARACIÓN DEL TRABAJO DE CAMPO</strong></p>");
                    //sb.Append("</div>");
                    //sb.Append("</div>");

                    // UBIC_XY1
                    sb.Append("<div class=\"col-lg-12 b-r\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-8 b-r ubic_xy1\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA REGISTRÓ CORRECTAMENTE EL USO DE LA EDIFICACIÓN EN EL LISTADO DE DIRECCIONES Y SU GEORREFERENCIACIÓN EN EL DMC?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r ubic_xy1\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_1\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // VIV_CENS
                    sb.Append("<div class=\"col-lg-8 b-r viv_cens\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA VIVIENDA FUE CENSADA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r viv_cens\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_2\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // RES_VIV
                    sb.Append("<div class=\"col-lg-8 b-r res_viv\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿<b>" + informante + "</b> ES RESIDENTE DE LA VIVIENDA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r res_viv\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_3\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // TOT_PER
                    //sb.Append("<div class=\"col-lg-12 b-r\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-8 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿ME PODRÍA INDICAR CUANTAS PERSONAS RESIDEN EN LA VIVIENDA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div class=\"filtro_Notas\">");
                    sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + cant_per + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_PER
                    sb.Append("<div class=\"col-lg-8 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>OBSERVACIÓN CANTIDAD PERSONAS RESIDENTES DE LA VIVIENDA</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_4\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // PRES_ALIM
                    sb.Append("<div class=\"col-lg-8 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>DE LAS PERSONAS QUE RESIDEN EN LA VIVIENDA, ¿TODAS COMPARTEN PRESUPUESTO DE ALIMENTACIÓN?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div class=\"filtro_Notas\">");
                    sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_HOG
                    sb.Append("<div class=\"col-lg-8 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>OBSERVACIÓN DE LAS PERSONAS QUE COMPARTEN PRESUPUESTO DE ALIMENTACIÓN</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_5\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // NAC_SEXO
                    sb.Append("<div class=\"col-lg-8 b-r nac_sexo\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>PARA CADA RESIDENTE DE LA VIVIENDA: AL NACER, ¿CUÁL FUE SU SEXO? </p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r nac_sexo\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div  class=\"filtro_Notas\">");
                    sb.Append(tabla_sexo);
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_SE
                    sb.Append("<div class=\"col-lg-8 b-r nac_sexo\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>OBSERVACIÓN SEXO DE LOS RESIDENTE DE LA VIVIENDA</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r nac_sexo\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_6\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // EDAD_ACT
                    sb.Append("<div class=\"col-lg-8 b-r edad_act\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿CUÁNTOS AÑOS CUMPLIDOS TIENE CADA RESIDENTE DE LA VIVIENDA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r edad_act\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div  class=\"filtro_Notas\">");
                    sb.Append(tabla_edad);
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // INF_ED
                    sb.Append("<div class=\"col-lg-8 b-r edad_act\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>OBSERVACIÓN AÑOS CUMPLIDOS RESIDENTES DE LA VIVIENDA</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r edad_act\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_7\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // PAR_RES
                    sb.Append("<div class=\"col-lg-8 b-r par_jefe\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿QUÉ PARENTESCO TIENE CADA RESIDENTE DE LA VIVIENDA CON EL JEFE O JEFA DE HOGAR?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r par_jefe\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div  class=\"filtro_Notas\">");
                    sb.Append(tabla_parent);
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_PAR
                    sb.Append("<div class=\"col-lg-8 b-r par_jefe\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>OBSERVACIÓN DE  PARENTESCO CON EL JEFE O JEFA DE HOGAR</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r par_jefe\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_8\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // EVAL_CEN
                    sb.Append("<div class=\"col-lg-12 b-r eval_cen\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-8 b-r eval_cen\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿CÓMO EVALUARÍA EL TRABAJO DE LA PERSONA CENSISTA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r eval_cen\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_9\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_SUP1
                    sb.Append("<div class=\"col-lg-8 b-r obs_sup1\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>INGRESE OBSERVACIONES SOBRE SUPERVISIÓN</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r obs_sup1\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_10\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");


                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<input id=\"IdTipoLevantamiento\" name=\"IdTipoLevantamiento\" type=\"hidden\" value=\"1\" />");
                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-12 text-center mensaje\" style=\"\">");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\"><br></div>");

                    if ((perfil_usuario == "1" || perfil_usuario == "6") && !estadoCuest.Equals("1"))
                    {
                        sb.Append("<div class=\"col-lg-12 b-r\">");
                        sb.Append("<div class=\"form-group\">");
                        sb.Append("<button type=\"submit\" class=\"btn btn-primary btn-rounded btn-block\"><i class=\"fa fa-check\"></i> Guardar</button>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                    }
                    else
                    {
                        sb.Append("<div class=\"col-lg-12\"></div>");
                    }
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</form>");
                    sb.Append(_methodCallLoad.CreaJQueryDocumentReady());
                    sb.Append(_methodCallSaltoCuestionario.CreaJQueryFunction());

                    _strHtml = sb.ToString();
                }
                else
                {
                    sb.Append("<div class=\"text-center\">No hay datos para mostrar.</div>");
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
        /// Permite mostrar formulario de supervision indirecta
        /// </summary>
        public string MuestraFormSupervisionIndirectaTel(string guid)
        {
            try
            {
                GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL();
                GesUsuarioBLL _gesUsuarioBLL = new GesUsuarioBLL();
                _gesUsuarioBOL = _gesUsuarioBLL.ObtieneUsuarioConectado(_appSettings.ObtieneCookie());

                GesAsignacionesDAL gesAsignacionesDAL = new GesAsignacionesDAL();
                DataSet DsDatosUsu = new DataSet();
                DsDatosUsu = gesAsignacionesDAL.ListaDatosUsuario(_gesUsuarioBOL);

                var perfil_usuario = DsDatosUsu.Tables[0].Rows[0]["perfil_id"].ToString();

                StringBuilder sb = new StringBuilder();
                _strHtml = "";

                GesSupervisionDAL gesSupervisionDAL = new GesSupervisionDAL();

                DataTable DtGrupo = new DataTable();
                DataSet DsGrupo = new DataSet();
                DsGrupo = gesSupervisionDAL.ObtieneDatosPrecargados(guid,2,3);

                // Obtengo información de la supervisión
                DataTable DtSupervision = new DataTable();
                DataSet DsSupervision = new DataSet();
                DsSupervision = gesSupervisionDAL.ListaSupervisionPorGuid(guid, 2);

                string sup_col = "";
                string sup_col1 = "";
                string sup_col2 = "";

                string contenido = "";
                string campos = "";


                if (DsGrupo.Tables[0].Rows.Count > 0)
                {
                    DtGrupo = DsGrupo.Tables[0];


                    string nomSupervisor = DtGrupo.Rows[0]["NombreSupervisor"].ToString();
                    string nomCensista = DtGrupo.Rows[0]["NombreCensista"].ToString();
                    string rutCensista = DtGrupo.Rows[0]["RutCensista"].ToString();
                    string nomComuna = DtGrupo.Rows[0]["NombreComuna"].ToString();
                    string numDistrito = DtGrupo.Rows[0]["Distrito"].ToString();
                    string numArea = DtGrupo.Rows[0]["Area"].ToString();
                    string idALC = DtGrupo.Rows[0]["ALC_id"].ToString();
                    string informante = DtGrupo.Rows[0]["Reslev_nombre_informante"].ToString();
                    string cant_per = DtGrupo.Rows[0]["Reslev_total_per"].ToString();
                    string cant_hog = DtGrupo.Rows[0]["Reslev_cant_hog"].ToString();
                    string nomArea = "";
                    string estadoCuest = "0";
                    string tabla_sexo = "<table class=\"table table-hover table-striped\"><tbody>";
                    string tabla_edad = "<table class=\"table table-hover table-striped\"><tbody>";
                    string tabla_parent = "<table class=\"table table-hover table-striped\"><tbody>";

                    if (DsGrupo.Tables[1].Rows.Count > 0) {

                        foreach (DataRow row in DsGrupo.Tables[1].Rows)
                        {
                            tabla_sexo = tabla_sexo + "<tr><td>" + row["PER_NOMBRE"] + "</td><td>" + row["SEXO_STR"] + "</td></tr>";
                            tabla_edad = tabla_edad + "<tr><td>" + row["PER_NOMBRE"] + "</td><td>" + row["EDAD"] + "</td></tr>";
                            tabla_parent = tabla_parent + "<tr><td>" + row["PER_NOMBRE"] + "</td><td>" + row["PARENTESCO_STR"] + "</td></tr>";
                        }

                        tabla_sexo = tabla_sexo + "</table>";
                        tabla_edad = tabla_edad + "</table>";
                        tabla_parent = tabla_parent + "</table>";

                    } else {
                        tabla_sexo = tabla_sexo + "<tr><td>Sin información</td></tr></table>";
                        tabla_edad = tabla_edad + "<tr><td>Sin información</td></tr></table>";
                        tabla_parent = tabla_parent + "<tr><td>Sin información</td></tr></table>";
                    }


                    if (numArea.Equals("1")) { nomArea = "Urbana"; } else { nomArea = "Rural"; }
                    if (string.IsNullOrEmpty(idALC)) { idALC = "0"; }

                    // Genero funcion para insertar y eliminar asignaciones
                    PostJSON _postJSONInsertaSupervision = new PostJSON();
                    {
                        _postJSONInsertaSupervision.P_form = "supervision-indirecta-tel";
                        _postJSONInsertaSupervision.P_url_servicio = _appSettings.ServidorWeb + "api/supervision/insertar-supervision-indirecta-tel";
                        _postJSONInsertaSupervision.P_data_adicional = "parametro: 0";
                        _postJSONInsertaSupervision.P_data_dinamica = true;
                        _postJSONInsertaSupervision.P_respuesta_servicio = "$('.mensaje').html(respuesta[0].elemento_html);" +
                                                                           "setTimeout(function () { muestraListaMovil(" + idALC + ", 2,3); }, 1000);";
                    }

                    if (DsSupervision.Tables[0].Rows.Count > 0)
                    {

                        sup_col1 = DsSupervision.Tables[0].Rows[0]["sup_col1"].ToString();
                        sup_col2 = DsSupervision.Tables[0].Rows[0]["sup_col2"].ToString();
                        for (int i = 3; i < 14; i++)
                        {
                            sup_col = "sup_col" + i;
                            contenido = contenido + "$('#" + sup_col + "').val('" + DsSupervision.Tables[0].Rows[0][sup_col].ToString() + "'); ";
                        }

                        string col_4 = DsSupervision.Tables[0].Rows[0]["sup_col4"].ToString();
                        string col_5 = DsSupervision.Tables[0].Rows[0]["sup_col5"].ToString();
                        estadoCuest = DsSupervision.Tables[0].Rows[0]["IdEstadoSupervision"].ToString();

                        if (col_4.Equals("2")) {
                            campos = campos + "$('.res_viv').hide();" + "$('.tot_per').hide();" + "$('.pres_alim').hide();" + "$('.nac_sexo').hide();" + "$('.edad_act').hide();" + "$('.par_jefe').hide();" +
                                    "$('.cont_cens').hide();" + "$('.dud_cuest').hide();" + "$('.comp_correc').hide();" +
                                    "$('.eval_ope').hide();" + "$('.obs_sup1').hide();";
                        }

                        if (col_5.Equals("1")) {
                            campos = campos + "$('.tot_per').show();" + "$('.nac_sexo').show();" + "$('.edad_act').show();" + "$('.par_jefe').show();" +
                                    "$('.cont_cens').show();" + "$('.dud_cuest').show();" + "$('.comp_correc').show();" + "$('.eval_ope').show();" + "$('.obs_sup1').show();";
                        } else {
                            campos = campos + "$('.tot_per').hide();" + "$('.nac_sexo').hide();" + "$('.edad_act').hide();" + "$('.par_jefe').hide();" + "$('.obs_rph').show();" +
                                    "$('.cont_cens').show();" + "$('.dud_cuest').show();" + "$('.comp_correc').show();" + "$('.eval_ope').show();" + "$('.obs_sup1').show();";
                        }

                    } else {
                        sup_col1 = DateTime.Now.ToString("yyyy-MM-dd");
                        sup_col2 = DateTime.Now.ToString("HH:mm");
                    }


                    // Genero metodo submit del formulario
                    CallMethod _methodCallLoad = new CallMethod
                    {
                        Mc_contenido = _postJSONInsertaSupervision.PostJSONCall() +
                                       "setTimeout(function () { " +
                                                contenido +
                                       //"$('#sup_col2').val(" + sup_col2 + "); " +
                                       //"$('#sup_col3').val(" + sup_col3 + "); " +
                                       //"$('#sup_col4').val(" + sup_col4 + "); " +
                                       "}, 500);" +
                                       campos
                    };

                    CallMethod _methodCallSaltoCuestionario = new CallMethod
                    {
                        Mc_nombre = "saltoCuestionario(sup_col)",
                        Mc_contenido = "console.log('sup_col: ' + sup_col);" +
                                        "if(sup_col == 4){" +
                                            "var col_4 = $('#sup_col4').val();" +
                                            "console.log('valor de col_4: ' + col_4);" +
                                            "if(col_4 == 1){" +
                                                "$('.res_viv').show();" +
                                                "$('.tot_per').hide();" +
                                                "$('.nac_sexo').hide();" +
                                                "$('.edad_act').hide();" +
                                                "$('.par_jefe').hide();" +
                                                "$('.obs_rph').hide();" +
                                                "$('.cont_cens').hide();" +
                                                "$('.dud_cuest').hide();" +
                                                "$('.comp_correc').hide();" +
                                                "$('.eval_ope').hide();" +
                                                "$('.obs_sup1').hide();" +
                                            "}else{" +
                                                "$('.res_viv').hide();" +
                                                "$('.tot_per').hide();" +
                                                "$('.nac_sexo').hide();" +
                                                "$('.edad_act').hide();" +
                                                "$('.par_jefe').hide();" +
                                                "$('.obs_rph').show();" +
                                                "$('.cont_cens').hide();" +
                                                "$('.dud_cuest').hide();" +
                                                "$('.comp_correc').hide();" +
                                                "$('.eval_ope').hide();" +
                                                "$('.obs_sup1').hide();" +
                                            "}" +
                                        "}else if(sup_col == 5){" +
                                            "var col_5 = $('#sup_col5').val();" +
                                            "console.log('valor de col_5: ' + col_5);" +
                                            "if(col_5 == 1){" +
                                                // MOSTRAR INPUTS CON INFO PRECARGADA
                                                "$('.tot_per').show();" +
                                                "$('.nac_sexo').show();" +
                                                "$('.edad_act').show();" +
                                                "$('.par_jefe').show();" +
                                                //"$('.obs_rph').show();" +
                                                "$('.cont_cens').show();" +
                                                "$('.dud_cuest').show();" +
                                                "$('.comp_correc').show();" +
                                                "$('.eval_ope').show();" +
                                                "$('.obs_sup1').show();" +
                                            "}else if(col_5 == 2){" +
                                                "$('.tot_per').hide();" +
                                                "$('.nac_sexo').hide();" +
                                                "$('.edad_act').hide();" +
                                                "$('.par_jefe').hide();" +
                                                //"$('.obs_rph').hide();" +
                                                "$('.cont_cens').hide();" +
                                                "$('.dud_cuest').hide();" +
                                                "$('.comp_correc').hide();" +
                                                "$('.eval_ope').hide();" +
                                                "$('.obs_sup1').show();" +
                                            "}else{" +
                                                "$('.tot_per').hide();" +
                                                "$('.nac_sexo').hide();" +
                                                "$('.edad_act').hide();" +
                                                "$('.par_jefe').hide();" +
                                                "$('.obs_rph').hide();" +
                                                "$('.cont_cens').hide();" +
                                                "$('.dud_cuest').hide();" +
                                                "$('.comp_correc').hide();" +
                                                "$('.eval_ope').hide();" +
                                                "$('.obs_sup1').hide();" +
                                            "}" +   
                                        "}"
                    };


                    sb.Append("<form id=\"" + _postJSONInsertaSupervision.P_form + "\" class=\"m-t\" method=\"post\" >");
                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<h2 class=\"text-center\">OBSERVACIÓN INDIRECTA - TELEFÓNICA</h2>");
                    sb.Append("<hr />");
                    sb.Append("</div>");

                    //sb.Append("<div class=\"col-lg-12\">");
                    //sb.Append("<div class=\"alert alert-warning\">");
                    //sb.Append("<h4 class=\"text-center\"><p>Supervisor: Recuerde que debe registrar un puntaje para cada aspecto señalado. Para esto es importante que revise el manual de <br>supervisión, el que entrega una guía para cada registro, según lo esperado por el proyecto. <br>" +
                    //                                                    "Recuerde, además, que los datos de esta pauta deben ser ingresados al sistema de gestión. </p></h4>");
                    //sb.Append("</div>");
                    //sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\"><br></div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-4 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Comuna</strong></p>");
                    sb.Append("<input id=\"txt_supervisor\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomComuna + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Distrito</strong></p>");
                    sb.Append("<input id=\"10\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + numDistrito + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Área</strong></p>");
                    sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomArea + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<input id=\"IdTipoSupervision\" name=\"IdTipoSupervision\" type=\"hidden\" value=\"2\" />");
                    
                    sb.Append("<input id=\"IdSupervision\" name=\"IdSupervision\" type=\"hidden\" value=\"" + DtGrupo.Rows[0]["IdDireccionSecundaria"] + "\" />");
                    //sb.Append("<input id=\"IdCensista\" name=\"IdCensista\" type=\"hidden\" value=\"" + rutCensista + "\" />");
                    sb.Append("<p><strong>Fecha <span style=\"color:red;\">*</span></strong></p>");
                    sb.Append("<input id=\"sup_col1\" name=\"sup_col1\" type=\"date\" class=\"form-control\" maxlength=\"8\" min=\"2021-01-01\" max=\"2023-12-31\"  value=\"" + sup_col1 + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Hora <span style=\"color:red;\">*</span></strong></p>");
                    sb.Append("<input id=\"sup_col2\" name=\"sup_col2\" type=\"text\" class=\"form-control\" maxlength=\"5\"  value=\"" + sup_col2 + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Nombre Censista</strong></p>");
                    sb.Append("<input id=\"txt_supervisor\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomCensista + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Nombre Coordinador de Grupo</strong></p>");
                    sb.Append("<input id=\"10\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomSupervisor + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    //sb.Append("<div class=\"col-lg-4 b-r\">");
                    //sb.Append("<div class=\"form-group\">");
                    //sb.Append("<p><strong>Abreviado</strong></p>");
                    //sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"\" disabled>");
                    //sb.Append("</div>");
                    //sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    //sb.Append("<div class=\"col-lg-6 b-r\">");
                    //sb.Append("<div class=\"form-group\">");
                    //sb.Append("<p><strong>GUID</strong></p>");
                    //sb.Append("<input id=\"9\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + DtGrupo.Rows[0]["IdDireccionSecundaria"] + "\" disabled>");
                    //sb.Append("</div>");
                    //sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-12 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Código Cuestionario</strong></p>");
                    sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\"><hr></div>");

                    sb.Append("<div class=\"col-lg-12\"><br></div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    // SECCIÓN PREPARACIÓN DEL TRABAJO DE CAMPO
                    //sb.Append("<div class=\"col-lg-12 b-r\">");
                    //sb.Append("<div class=\"form-group\">");
                    //sb.Append("<p><strong>SECCIÓN PREPARACIÓN DEL TRABAJO DE CAMPO</strong></p>");
                    //sb.Append("</div>");
                    //sb.Append("</div>");

                    // UBIC_XY1
                    sb.Append("<div class=\"col-lg-12 b-r\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-8 b-r ubic_xy1\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA REGISTRÓ CORRECTAMENTE EL USO DE LA EDIFICACIÓN EN EL LISTADO DE DIRECCIONES Y SU GEORREFERENCIACIÓN  EL DMC?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r ubic_xy1\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_1\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // VIV_CENS
                    sb.Append("<div class=\"col-lg-8 b-r viv_cens\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA VIVIENDA FUE CENSADA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r viv_cens\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_2\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // RES_VIV
                    sb.Append("<div class=\"col-lg-8 b-r res_viv\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿<b>" + informante + "</b> ES RESIDENTE DE LA VIVIENDA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r res_viv\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_3\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // TOT_PER
                    //sb.Append("<div class=\"col-lg-12 b-r\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-8 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿ME PODRÍA INDICAR CUANTAS PERSONAS RESIDEN EN LA VIVIENDA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div class=\"filtro_Notas\">");
                    sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + cant_per + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_PER
                    sb.Append("<div class=\"col-lg-8 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>OBSERVACIÓN CANTIDAD PERSONAS RESIDENTES DE LA VIVIENDA</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_4\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // PRES_ALIM
                    sb.Append("<div class=\"col-lg-8 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>DE LAS PERSONAS QUE RESIDEN EN LA VIVIENDA, ¿TODAS COMPARTEN PRESUPUESTO DE ALIMENTACIÓN?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div class=\"filtro_Notas\">");
                    sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_HOG
                    sb.Append("<div class=\"col-lg-8 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>OBSERVACIÓN DE LAS PERSONAS QUE COMPARTEN PRESUPUESTO DE ALIMENTACIÓN</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_5\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // NAC_SEXO
                    sb.Append("<div class=\"col-lg-8 b-r nac_sexo\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>PARA CADA RESIDENTE DE LA VIVIENDA: AL NACER, ¿CUÁL FUE SU SEXO? </p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r nac_sexo\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div  class=\"filtro_Notas\">");
                    sb.Append(tabla_sexo);
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_SE
                    sb.Append("<div class=\"col-lg-8 b-r nac_sexo\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>OBSERVACIÓN SEXO DE LOS RESIDENTE DE LA VIVIENDA</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r nac_sexo\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_6\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // EDAD_ACT
                    sb.Append("<div class=\"col-lg-8 b-r edad_act\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿CUÁNTOS AÑOS CUMPLIDOS TIENE CADA RESIDENTE DE LA VIVIENDA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r edad_act\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div  class=\"filtro_Notas\">");
                    sb.Append(tabla_edad);
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // INF_ED
                    sb.Append("<div class=\"col-lg-8 b-r edad_act\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>OBSERVACIÓN AÑOS CUMPLIDOS RESIDENTES DE LA VIVIENDA</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r edad_act\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_7\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // PAR_RES
                    sb.Append("<div class=\"col-lg-8 b-r par_jefe\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿QUÉ PARENTESCO TIENE CADA RESIDENTE DE LA VIVIENDA CON EL JEFE O JEFA DE HOGAR?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r par_jefe\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div  class=\"filtro_Notas\">");
                    sb.Append(tabla_parent);
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_PAR
                    sb.Append("<div class=\"col-lg-8 b-r par_jefe\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>OBSERVACIÓN DE  PARENTESCO CON EL JEFE O JEFA DE HOGAR</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r par_jefe\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_8\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // CENS_CON
                    sb.Append("<div class=\"col-lg-8 b-r cens_con\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA LE EXPLICÓ EL CONTEXTO DEL PROYECTO CENSO?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r cens_con\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_9\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // DUD_CUEST
                    sb.Append("<div class=\"col-lg-8 b-r dud_cuest\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA REALIZÓ LAS PREGUNTAS Y RESOLVIÓ SUS DUDAS DE MANERA CORRECTA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r dud_cuest\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_10\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // COMP_CORREC
                    sb.Append("<div class=\"col-lg-8 b-r comp_correc\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA PERSONA CENSISTA  SE COMPORTÓ DE FORMA CORRECTA DURANTE LA ENTREVISTA CENSAL?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r comp_correc\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_11\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // EVAL_OPE
                    sb.Append("<div class=\"col-lg-12 b-r eval_ope\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-8 b-r eval_cen\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿CÓMO EVALUARÍA EL TRABAJO DE LA PERSONA CENSISTA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r eval_ope\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_12\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // OBS_SUP1
                    sb.Append("<div class=\"col-lg-8 b-r obs_sup1\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>INGRESE OBSERVACIONES SOBRE SUPERVISIÓN</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r obs_sup1\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_13\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");


                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<input id=\"IdTipoLevantamiento\" name=\"IdTipoLevantamiento\" type=\"hidden\" value=\"1\" />");
                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-12 text-center mensaje\" style=\"\">");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\"><br></div>");

                    if ((perfil_usuario == "1" || perfil_usuario == "6") && !estadoCuest.Equals("1"))
                    {
                        sb.Append("<div class=\"col-lg-12 b-r\">");
                        sb.Append("<div class=\"form-group\">");
                        sb.Append("<button type=\"submit\" class=\"btn btn-primary btn-rounded btn-block\"><i class=\"fa fa-check\"></i> Guardar</button>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                    }
                    else
                    {
                        sb.Append("<div class=\"col-lg-12\"></div>");
                    }
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</form>");
                    sb.Append(_methodCallLoad.CreaJQueryDocumentReady());
                    sb.Append(_methodCallSaltoCuestionario.CreaJQueryFunction());

                    _strHtml = sb.ToString();
                }
                else
                {
                    sb.Append("<div class=\"text-center\">No hay datos para mostrar.</div>");
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
        /// Permite mostrar formulario de supervision indirecta
        /// </summary>
        public string MuestraFormSupervisionEspOcupacion(string guid)
        {
            try
            {
                GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL();
                GesUsuarioBLL _gesUsuarioBLL = new GesUsuarioBLL();
                _gesUsuarioBOL = _gesUsuarioBLL.ObtieneUsuarioConectado(_appSettings.ObtieneCookie());

                GesAsignacionesDAL gesAsignacionesDAL = new GesAsignacionesDAL();
                DataSet DsDatosUsu = new DataSet();
                DsDatosUsu = gesAsignacionesDAL.ListaDatosUsuario(_gesUsuarioBOL);

                var perfil_usuario = DsDatosUsu.Tables[0].Rows[0]["perfil_id"].ToString();

                StringBuilder sb = new StringBuilder();
                _strHtml = "";

                GesSupervisionDAL gesSupervisionDAL = new GesSupervisionDAL();

                DataTable DtGrupo = new DataTable();
                DataSet DsGrupo = new DataSet();
                DsGrupo = gesSupervisionDAL.ObtieneDatosPrecargados(guid,3,0);

                // Obtengo información de la supervisión
                DataTable DtSupervision = new DataTable();
                DataSet DsSupervision = new DataSet();
                DsSupervision = gesSupervisionDAL.ListaSupervisionPorGuid(guid, 3);

                string sup_col = "";
                string sup_col1 = "";
                string sup_col2 = "";

                string contenido = "";
                string campos = "";


                if (DsGrupo.Tables[0].Rows.Count > 0)
                {
                    DtGrupo = DsGrupo.Tables[0];


                    string nomSupervisor = DtGrupo.Rows[0]["NombreSupervisor"].ToString();
                    string nomCensista = DtGrupo.Rows[0]["NombreCensista"].ToString();
                    string rutCensista = DtGrupo.Rows[0]["RutCensista"].ToString();
                    string nomComuna = DtGrupo.Rows[0]["NombreComuna"].ToString();
                    string numDistrito = DtGrupo.Rows[0]["Distrito"].ToString();
                    string numArea = DtGrupo.Rows[0]["Area"].ToString();
                    string idALC = DtGrupo.Rows[0]["ALC_id"].ToString();
                    string nomArea = "";
                    string estadoCuest = "0";



                    if (numArea.Equals("1")) { nomArea = "Urbana"; } else { nomArea = "Rural"; }
                    if (string.IsNullOrEmpty(idALC)) { idALC = "0"; }

                    // Genero funcion para insertar y eliminar asignaciones
                    PostJSON _postJSONInsertaSupervision = new PostJSON();
                    {
                        _postJSONInsertaSupervision.P_form = "supervision-esp-ocupacion";
                        _postJSONInsertaSupervision.P_url_servicio = _appSettings.ServidorWeb + "api/supervision/insertar-supervision-esp-ocupacion";
                        _postJSONInsertaSupervision.P_data_adicional = "parametro: 0";
                        _postJSONInsertaSupervision.P_data_dinamica = true;
                        _postJSONInsertaSupervision.P_respuesta_servicio = "$('.mensaje').html(respuesta[0].elemento_html);" +
                                                                           "setTimeout(function () { muestraListaMovil(" + idALC + ", 3,0); }, 1000);";
                    }

                    if (DsSupervision.Tables[0].Rows.Count > 0)
                    {

                        sup_col1 = DsSupervision.Tables[0].Rows[0]["sup_col1"].ToString();
                        sup_col2 = DsSupervision.Tables[0].Rows[0]["sup_col2"].ToString();
                        for (int i = 3; i < 5; i++)
                        {
                            sup_col = "sup_col" + i;
                            contenido = contenido + "$('#" + sup_col + "').val('" + DsSupervision.Tables[0].Rows[0][sup_col].ToString() + "'); ";
                        }

                        estadoCuest = DsSupervision.Tables[0].Rows[0]["IdEstadoSupervision"].ToString();

                        

                    } else {
                        sup_col1 = DateTime.Now.ToString("yyyy-MM-dd");
                        sup_col2 = DateTime.Now.ToString("HH:mm");
                    }


                    // Genero metodo submit del formulario
                    CallMethod _methodCallLoad = new CallMethod
                    {
                        Mc_contenido = _postJSONInsertaSupervision.PostJSONCall() +
                                       "setTimeout(function () { " +
                                                contenido +
                                       //"$('#sup_col2').val(" + sup_col2 + "); " +
                                       //"$('#sup_col3').val(" + sup_col3 + "); " +
                                       //"$('#sup_col4').val(" + sup_col4 + "); " +
                                       "}, 500);" +
                                       campos
                    };

                    CallMethod _methodCallSaltoCuestionario = new CallMethod
                    {
                        Mc_nombre = "saltoCuestionario(sup_col)",
                        Mc_contenido = "console.log('sup_col: ' + sup_col);"
                                        
                    };


                    sb.Append("<form id=\"" + _postJSONInsertaSupervision.P_form + "\" class=\"m-t\" method=\"post\" >");
                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<h2 class=\"text-center\">VERIFICACIÓN DE ESTADO DE OCUPACIÓN</h2>");
                    sb.Append("<hr />");
                    sb.Append("</div>");

                    //sb.Append("<div class=\"col-lg-12\">");
                    //sb.Append("<div class=\"alert alert-warning\">");
                    //sb.Append("<h4 class=\"text-center\"><p>Supervisor: Recuerde que debe registrar un puntaje para cada aspecto señalado. Para esto es importante que revise el manual de <br>supervisión, el que entrega una guía para cada registro, según lo esperado por el proyecto. <br>" +
                    //                                                    "Recuerde, además, que los datos de esta pauta deben ser ingresados al sistema de gestión. </p></h4>");
                    //sb.Append("</div>");
                    //sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\"><br></div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-4 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Comuna</strong></p>");
                    sb.Append("<input id=\"txt_supervisor\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomComuna + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Distrito</strong></p>");
                    sb.Append("<input id=\"10\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + numDistrito + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Área</strong></p>");
                    sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomArea + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<input id=\"IdTipoSupervision\" name=\"IdTipoSupervision\" type=\"hidden\" value=\"3\" />");
                    
                    sb.Append("<input id=\"IdSupervision\" name=\"IdSupervision\" type=\"hidden\" value=\"" + DtGrupo.Rows[0]["IdDireccionSecundaria"] + "\" />");
                    //sb.Append("<input id=\"IdCensista\" name=\"IdCensista\" type=\"hidden\" value=\"" + rutCensista + "\" />");
                    sb.Append("<p><strong>Fecha <span style=\"color:red;\">*</span></strong></p>");
                    sb.Append("<input id=\"sup_col1\" name=\"sup_col1\" type=\"date\" class=\"form-control\" maxlength=\"8\" min=\"2021-01-01\" max=\"2023-12-31\"  value=\"" + sup_col1 + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Hora <span style=\"color:red;\">*</span></strong></p>");
                    sb.Append("<input id=\"sup_col2\" name=\"sup_col2\" type=\"text\" class=\"form-control\" maxlength=\"5\"  value=\"" + sup_col2 + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Nombre Censista</strong></p>");
                    sb.Append("<input id=\"txt_supervisor\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomCensista + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Nombre Coordinador de Grupo</strong></p>");
                    sb.Append("<input id=\"10\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomSupervisor + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    //sb.Append("<div class=\"col-lg-4 b-r\">");
                    //sb.Append("<div class=\"form-group\">");
                    //sb.Append("<p><strong>Abreviado</strong></p>");
                    //sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"\" disabled>");
                    //sb.Append("</div>");
                    //sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    //sb.Append("<div class=\"col-lg-6 b-r\">");
                    //sb.Append("<div class=\"form-group\">");
                    //sb.Append("<p><strong>GUID</strong></p>");
                    //sb.Append("<input id=\"9\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + DtGrupo.Rows[0]["IdDireccionSecundaria"] + "\" disabled>");
                    //sb.Append("</div>");
                    //sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-12 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Código Cuestionario</strong></p>");
                    sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"\" >");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\"><hr></div>");

                    sb.Append("<div class=\"col-lg-12\"><br></div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    // SECCIÓN PREPARACIÓN DEL TRABAJO DE CAMPO
                    //sb.Append("<div class=\"col-lg-12 b-r\">");
                    //sb.Append("<div class=\"form-group\">");
                    //sb.Append("<p><strong>SECCIÓN PREPARACIÓN DEL TRABAJO DE CAMPO</strong></p>");
                    //sb.Append("</div>");
                    //sb.Append("</div>");

                    // 
                    sb.Append("<div class=\"col-lg-12 b-r\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-8 b-r ubic_xy1\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿CONFIRMA LA DESOCUPACIÓN DE LA VIVIENDA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r ubic_xy1\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_1\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // 
                    sb.Append("<div class=\"col-lg-8 b-r viv_cens\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>INGRESE OBSERVACIONES SOBRE SUPERVISIÓN REALIZADA</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r viv_cens\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_2\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");



                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<input id=\"IdTipoLevantamiento\" name=\"IdTipoLevantamiento\" type=\"hidden\" value=\"0\" />");
                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-12 text-center mensaje\" style=\"\">");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\"><br></div>");

                    if ((perfil_usuario == "1" || perfil_usuario == "6") && !estadoCuest.Equals("1"))
                    {
                        sb.Append("<div class=\"col-lg-12 b-r\">");
                        sb.Append("<div class=\"form-group\">");
                        sb.Append("<button type=\"submit\" class=\"btn btn-primary btn-rounded btn-block\"><i class=\"fa fa-check\"></i> Guardar</button>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                    }
                    else
                    {
                        sb.Append("<div class=\"col-lg-12\"></div>");
                    }
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</form>");
                    sb.Append(_methodCallLoad.CreaJQueryDocumentReady());
                    sb.Append(_methodCallSaltoCuestionario.CreaJQueryFunction());

                    _strHtml = sb.ToString();
                }
                else
                {
                    sb.Append("<div class=\"text-center\">No hay datos para mostrar.</div>");
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
        /// Permite mostrar formulario de supervision indirecta
        /// </summary>
        public string MuestraFormSupervisionEspTipoViv(string guid)
        {
            try
            {
                GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL();
                GesUsuarioBLL _gesUsuarioBLL = new GesUsuarioBLL();
                _gesUsuarioBOL = _gesUsuarioBLL.ObtieneUsuarioConectado(_appSettings.ObtieneCookie());

                GesAsignacionesDAL gesAsignacionesDAL = new GesAsignacionesDAL();
                DataSet DsDatosUsu = new DataSet();
                DsDatosUsu = gesAsignacionesDAL.ListaDatosUsuario(_gesUsuarioBOL);

                var perfil_usuario = DsDatosUsu.Tables[0].Rows[0]["perfil_id"].ToString();

                StringBuilder sb = new StringBuilder();
                _strHtml = "";

                GesSupervisionDAL gesSupervisionDAL = new GesSupervisionDAL();

                DataTable DtGrupo = new DataTable();
                DataSet DsGrupo = new DataSet();
                DsGrupo = gesSupervisionDAL.ObtieneDatosPrecargados(guid,4,0);

                // Obtengo información de la supervisión
                DataTable DtSupervision = new DataTable();
                DataSet DsSupervision = new DataSet();
                DsSupervision = gesSupervisionDAL.ListaSupervisionPorGuid(guid, 4);

                string sup_col = "";
                string sup_col1 = "";
                string sup_col2 = "";

                string contenido = "";
                string campos = "";


                if (DsGrupo.Tables[0].Rows.Count > 0)
                {
                    DtGrupo = DsGrupo.Tables[0];


                    string nomSupervisor = DtGrupo.Rows[0]["NombreSupervisor"].ToString();
                    string nomCensista = DtGrupo.Rows[0]["NombreCensista"].ToString();
                    string rutCensista = DtGrupo.Rows[0]["RutCensista"].ToString();
                    string nomComuna = DtGrupo.Rows[0]["NombreComuna"].ToString();
                    string numDistrito = DtGrupo.Rows[0]["Distrito"].ToString();
                    string numArea = DtGrupo.Rows[0]["Area"].ToString();
                    string idALC = DtGrupo.Rows[0]["ALC_id"].ToString();
                    string cant_hog = DtGrupo.Rows[0]["Reslev_cant_hog"].ToString(); ;
                    string cant_per = DtGrupo.Rows[0]["Reslev_total_per"].ToString(); ;
                    string nomArea = "";
                    string estadoCuest = "0";



                    if (numArea.Equals("1")) { nomArea = "Urbana"; } else { nomArea = "Rural"; }
                    if (string.IsNullOrEmpty(idALC)) { idALC = "0"; }

                    // Genero funcion para insertar y eliminar asignaciones
                    PostJSON _postJSONInsertaSupervision = new PostJSON();
                    {
                        _postJSONInsertaSupervision.P_form = "supervision-esp-tipoviv";
                        _postJSONInsertaSupervision.P_url_servicio = _appSettings.ServidorWeb + "api/supervision/insertar-supervision-esp-tipoviv";
                        _postJSONInsertaSupervision.P_data_adicional = "parametro: 0";
                        _postJSONInsertaSupervision.P_data_dinamica = true;
                        _postJSONInsertaSupervision.P_respuesta_servicio = "$('.mensaje').html(respuesta[0].elemento_html);" +
                                                                           "setTimeout(function () { muestraListaMovil(" + idALC + ", 4,0); }, 1000);";
                    }

                    if (DsSupervision.Tables[0].Rows.Count > 0)
                    {

                        sup_col1 = DsSupervision.Tables[0].Rows[0]["sup_col1"].ToString();
                        sup_col2 = DsSupervision.Tables[0].Rows[0]["sup_col2"].ToString();
                        //cant_hog = DsSupervision.Tables[0].Rows[0]["Reslev_cant_hog"].ToString();
                        //cant_per = DsSupervision.Tables[0].Rows[0]["Reslev_total_per"].ToString();
                        for (int i = 3; i < 5; i++)
                        {
                            sup_col = "sup_col" + i;
                            contenido = contenido + "$('#" + sup_col + "').val('" + DsSupervision.Tables[0].Rows[0][sup_col].ToString() + "'); ";
                        }

                        estadoCuest = DsSupervision.Tables[0].Rows[0]["IdEstadoSupervision"].ToString();


                    } else {
                        sup_col1 = DateTime.Now.ToString("yyyy-MM-dd");
                        sup_col2 = DateTime.Now.ToString("HH:mm");
                    }


                    // Genero metodo submit del formulario
                    CallMethod _methodCallLoad = new CallMethod
                    {
                        Mc_contenido = _postJSONInsertaSupervision.PostJSONCall() +
                                       "setTimeout(function () { " +
                                                contenido +
                                       //"$('#sup_col2').val(" + sup_col2 + "); " +
                                       //"$('#sup_col3').val(" + sup_col3 + "); " +
                                       //"$('#sup_col4').val(" + sup_col4 + "); " +
                                       "}, 500);" +
                                       campos
                    };

                    CallMethod _methodCallSaltoCuestionario = new CallMethod
                    {
                        Mc_nombre = "saltoCuestionario(sup_col)",
                        Mc_contenido = "console.log('sup_col: ' + sup_col);" 
                                       
                    };


                    sb.Append("<form id=\"" + _postJSONInsertaSupervision.P_form + "\" class=\"m-t\" method=\"post\" >");
                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<h2 class=\"text-center\">VERIFICACIÓN DE VIVIENDAS PARTICULARES</h2>");
                    sb.Append("<hr />");
                    sb.Append("</div>");

                    //sb.Append("<div class=\"col-lg-12\">");
                    //sb.Append("<div class=\"alert alert-warning\">");
                    //sb.Append("<h4 class=\"text-center\"><p>Supervisor: Recuerde que debe registrar un puntaje para cada aspecto señalado. Para esto es importante que revise el manual de <br>supervisión, el que entrega una guía para cada registro, según lo esperado por el proyecto. <br>" +
                    //                                                    "Recuerde, además, que los datos de esta pauta deben ser ingresados al sistema de gestión. </p></h4>");
                    //sb.Append("</div>");
                    //sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\"><br></div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-4 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Comuna</strong></p>");
                    sb.Append("<input id=\"txt_supervisor\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomComuna + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Distrito</strong></p>");
                    sb.Append("<input id=\"10\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + numDistrito + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Área</strong></p>");
                    sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomArea + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<input id=\"IdTipoSupervision\" name=\"IdTipoSupervision\" type=\"hidden\" value=\"4\" />");
                    
                    sb.Append("<input id=\"IdSupervision\" name=\"IdSupervision\" type=\"hidden\" value=\"" + DtGrupo.Rows[0]["IdDireccionSecundaria"] + "\" />");
                    //sb.Append("<input id=\"IdCensista\" name=\"IdCensista\" type=\"hidden\" value=\"" + rutCensista + "\" />");
                    sb.Append("<p><strong>Fecha <span style=\"color:red;\">*</span></strong></p>");
                    sb.Append("<input id=\"sup_col1\" name=\"sup_col1\" type=\"date\" class=\"form-control\" maxlength=\"8\" min=\"2021-01-01\" max=\"2023-12-31\"  value=\"" + sup_col1 + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Hora <span style=\"color:red;\">*</span></strong></p>");
                    sb.Append("<input id=\"sup_col2\" name=\"sup_col2\" type=\"text\" class=\"form-control\" maxlength=\"5\"  value=\"" + sup_col2 + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Nombre Censista</strong></p>");
                    sb.Append("<input id=\"txt_supervisor\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomCensista + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-6 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Nombre Coordinador de Grupo</strong></p>");
                    sb.Append("<input id=\"10\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + nomSupervisor + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    //sb.Append("<div class=\"col-lg-4 b-r\">");
                    //sb.Append("<div class=\"form-group\">");
                    //sb.Append("<p><strong>Abreviado</strong></p>");
                    //sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"\" disabled>");
                    //sb.Append("</div>");
                    //sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    //sb.Append("<div class=\"col-lg-6 b-r\">");
                    //sb.Append("<div class=\"form-group\">");
                    //sb.Append("<p><strong>GUID</strong></p>");
                    //sb.Append("<input id=\"9\" type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + DtGrupo.Rows[0]["IdDireccionSecundaria"] + "\" disabled>");
                    //sb.Append("</div>");
                    //sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-12 b-r\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p><strong>Código Cuestionario</strong></p>");
                    sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"\" >");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\"><hr></div>");

                    sb.Append("<div class=\"col-lg-12\"><br></div>");

                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-12 b-r\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-8 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>TOTAL DE HOGARES REGISTRADOS PARA LA VIVIENDA</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r tot_per\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div  class=\"filtro_Notas\">");
                    sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + cant_hog + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // 
                    sb.Append("<div class=\"col-lg-8 b-r nac_sexo\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>TOTAL DE PERSONAS REGISTRADAS PARA LA VIVIENDA </p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r nac_sexo\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div  class=\"filtro_Notas\">");
                    sb.Append("<input type=\"text\" class=\"form-control\" maxlength=\"100\" placeholder=\"\" value=\"" + cant_per + "\" disabled>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    // 
                    sb.Append("<div class=\"col-lg-12 b-r\">&nbsp;</div>");
                    sb.Append("<div class=\"col-lg-8 b-r ubic_xy1\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>¿LA INFORMACIÓN INGRESADA SOBRE LA VIVIENDA ES CORRECTA?</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r ubic_xy1\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_1\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    // 
                    sb.Append("<div class=\"col-lg-8 b-r viv_cens\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<p>INGRESE SUS OBSERVACIONES</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-lg-4 b-r viv_cens\">");
                    sb.Append("<div class=\"form-group\">");
                    sb.Append("<div id=\"filtro_Notas_2\" class=\"filtro_Notas\">");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    
                    


                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<input id=\"IdTipoLevantamiento\" name=\"IdTipoLevantamiento\" type=\"hidden\" value=\"0\" />");
                    sb.Append("<div class=\"col-lg-12\">");
                    sb.Append("<div class=\"row\">");

                    sb.Append("<div class=\"col-lg-12 text-center mensaje\" style=\"\">");
                    sb.Append("</div>");

                    sb.Append("<div class=\"col-lg-12\"><br></div>");

                    if ((perfil_usuario == "1" || perfil_usuario == "6") && !estadoCuest.Equals("1"))
                    {
                        sb.Append("<div class=\"col-lg-12 b-r\">");
                        sb.Append("<div class=\"form-group\">");
                        sb.Append("<button type=\"submit\" class=\"btn btn-primary btn-rounded btn-block\"><i class=\"fa fa-check\"></i> Guardar</button>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                    }
                    else
                    {
                        sb.Append("<div class=\"col-lg-12\"></div>");
                    }
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</form>");
                    sb.Append(_methodCallLoad.CreaJQueryDocumentReady());
                    sb.Append(_methodCallSaltoCuestionario.CreaJQueryFunction());

                    _strHtml = sb.ToString();
                }
                else
                {
                    sb.Append("<div class=\"text-center\">No hay datos para mostrar.</div>");
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
    #endregion
}