using Framework.BOL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Framework.DAL
{
    public class GesSupervisionDAL
    {
        string _strSql = "";

        /// <summary>
        /// Lista empresas para creación de cuentas
        /// </summary>
        public DataSet ListaEmpresas(string usuario, string estado, int tipo)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure("Pa_supervision_web_listarEmpresas");
                _exQuery.AgregarParametro("usuario", usuario);
                _exQuery.AgregarParametro("estado", estado);
                _exQuery.AgregarParametro("tipo", tipo);
                return _exQuery.EjecutarProcedimiento();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Lista empresas para supervision
        /// </summary>
        public DataSet ListaEmpresasSupervisionWeb(string usuario, string estado, int tipo)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure("Pa_supervision_web_listarEmpresasSupervision");
                _exQuery.AgregarParametro("usu_id", usuario);
                _exQuery.AgregarParametro("estado_id", estado);
                _exQuery.AgregarParametro("tipo", tipo);
                return _exQuery.EjecutarProcedimiento();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Lista interrupciones por productor web
        /// </summary>
        public DataSet ListaInterrupcionesProductorWeb(int geografia_id, int _area_cesal, int tipo)
        {
            try
            {
                string _filtro = "";
                if (_area_cesal == -1 && tipo == -1)
                {
                    _filtro = "WHERE rm.esmo_id IN (1,14) ";
                } else
                {
                    if (_area_cesal != -1 && tipo == -1)
                    {
                        _filtro = "WHERE rm.esmo_id IN (1,14) AND rm.remo_area_censal IN (" + _area_cesal + ")";
                    }
                    if (_area_cesal == -1 && tipo != -1)
                    {
                        _filtro = "WHERE rm.esmo_id IN (" + tipo + ")";
                    }
                    if (_area_cesal != -1 && tipo != -1)
                    {
                        _filtro = "WHERE rm.esmo_id IN (" + tipo + ") AND rm.remo_area_censal IN (" + _area_cesal + ")";
                    }
                }
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT " +
                                "remo_guid," +
                                "rm.esmo_id," +
                                "[Área censal] = (SELECT area_levantamiento_nombre FROM dbo.ges_area_levantamiento WHERE area_levantamiento_id = rm.remo_area_censal)," +
                                "Folio," +
                                "Estado = (SELECT esmo_glosa FROM [CAF_PROCESAMIENTO].[dbo].[glo_estado_movil] WHERE esmo_id = rm.esmo_id)," +
                                "Estado_id = ISNULL((SELECT TOP 1 sup_col4 FROM dbo.ges_dato WHERE catalogo_id = 75 AND sup_col1 = rm.remo_guid),0)," +
                                "Marca_Web = ISNULL(Marca_Web,0)," +
                                "[Glosa revisión] = ISNULL((SELECT TOP 1 CASE sup_col4 WHEN 2 THEN '<p class=\"text-info\"><i class=\"fa fa-info\"></i> Recuperar Web</p>' END FROM dbo.ges_dato WHERE catalogo_id = 75 AND sup_col1 = rm.remo_guid),'<p class=\"text-warning\"><i class=\"fa fa-eye\"></i> Pendiente</p>') " +
                          "FROM [CAF_PROCESAMIENTO].[dbo].[ges_resumen_movil] AS rm " +
                          _filtro + " AND ISNULL(Marca_Web,0) = 0 AND rm.remo_area_censal IN (SELECT area_levantamiento_id FROM dbo.ges_area_levantamiento WHERE geografia_id IN (" + geografia_id + "))";
                return _exQuery.EjecutarConsulta(_strSql);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Lista resumen indagación interrupciones por productor web
        /// </summary>
        public DataSet ListaResumenIndagacionProductorWeb(string guid)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT " +
                                "Roles = y.roles," +
                                "Superficie = y.superficie," +
                                "Suelo = y.suelo," +
                                "Observación = y.observacion " +
                          "FROM[CAF_PROCESAMIENTO].dbo.Ges_IndagacionOperativa_Detalle AS z " +
                          "INNER JOIN[CAF_PROCESAMIENTO].dbo.Ges_IndagacionOperativa_Detalle_Roles AS y ON z.id = y.ghrdguid " +
                          "WHERE z.ghrguid = @guid";
                _exQuery.AgregarParametro("guid", guid);
                return _exQuery.EjecutarConsulta(_strSql);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Lista resumen indagación interrupciones por productor web nueva información
        /// </summary>
        public DataSet ListaResumenRolesProductorWeb(string guid)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "select " +
                                "a.guid, " +
	                            "a.Roles as Rol," +
	                            "a.Superficie as SuperfHA," +
	                            "case when a.Parte_Rol = 1 then 'SI' else 'NO'end as ParteRol," +
	                            "case when a.Verificacion_Rol = 1 then 'Registros oficiales(Contribuciones, SII, etc.)' " +
	                            "else case when a.Verificacion_Rol = 2 then 'Registros propios(Computador, cuadernos, recuerda mentalmente)' " +
	                            "else case when a.verificacion_Rol = 3 then 'Tablet del recolector' " +
	                            "else case when a.verificacion_Rol = 4 then 'No se Puede verificar' end end end end as Fuente_verificacion, " +
	                            "a.Observacion " +
                            "from [CAF_PROCESAMIENTO].dbo.Enc_Explotacion_RolSuperficie as a " +
                            "inner join(select guid, MAX(PkMaster) as PK_Master from [CAF_PROCESAMIENTO].dbo.Enc_Explotacion_RolSuperficie group by guid) as b on " +
                            "a.guid = b.guid and a.PkMaster = b.PK_Master " +
                            "where a.guid = @guid";
                _exQuery.AgregarParametro("guid", guid);
                return _exQuery.EjecutarConsulta(_strSql);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Lista resumen cuestionarios interrupciones por productor web
        /// </summary>
        public DataSet ListaResumenCuestionarioProductorWeb(string guid, string query)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                if (query == "encabezado")
                {
                    _strSql = "select a.remo_guid, a.folio, a.remo_usu_id, " +
                                "c.usu_nombre,ex.pk_explotacion," +
                                "(case when p1 in(1, 2, 3) then 'Persona Natural' else case when p1 >= 4 then 'Persona Juridica' else '' end end) as Tipo_Productor," +
                                "(case when p1 in(1, 2, 3) then p5_1+' ' + p5_3 else case when p1 >= 4 then p16             end end) as Productor/*Productor*/," +
                                "(case when p1 in(1, 2, 3) then p6_1+'-' + p6_2 else case when p1 >= 4 then p17_1+'-' + p17_2 end end) as Rut_Productor /**Rut_Productor*/," +
                                "(case when p1 in(1, 2, 3) then p14_1         else case when p1 >= 4 then p22_1           end end) as FonoUno /*Fono_Productor*/," +
                                "(case when p1 in(1, 2, 3) then p14_2			else case when p1 >= 4 then p22_2           end end) as TipoFonoUno /**Tipo_Fono*/," +
                                "(case when p1 in(1, 2, 3) then p14_3         else case when p1 >= 4 then p22_3           end end) as FonoDos /*Fono_Productor*/," +
                                "(case when p1 in(1, 2, 3) then p14_4			else case when p1 >= 4 then p22_4           end end) as TipoFonoDos /**Tipo_Fono*/," +
                                "a.remo_observaciones " +
                                "from CAF_PROCESAMIENTO.dbo.enc_explotacion as ex " +
                                "inner join CAF_PROCESAMIENTO.dbo.ges_resumen_movil as a on a.remo_guid = ex.guid " +
                                "inner join CAF_PROCESAMIENTO.dbo.glo_estado_movil as b  on a.esmo_id = b.esmo_id " +
                                "inner join GESTION_CA.dbo.ges_usuario as c on c.usu_id = a.remo_usu_id " +
                                "where b.esmo_id in (14) AND a.remo_guid = @guid " +
                                "order by 1,2 asc";
                } else
                {
                    _strSql = "select " +
                                "a.remo_guid,a.folio, a.remo_usu_id," +
                                "etb.p31 as Establecimiento," +
                                "etb.p34_1 + ' - ' + etb.p34_3 as Informante," +
                                "etb.p36_1 as FonoInformante," +
                                "etb.p36_2 as TipofonoInformante," +
                                "etb.p38_1 + ' - ' + etb.p38_3 as Administrador," +
                                "etb.p41_1 as FonoAdmin," +
                                "etb.p41_2 as TipofonoAdmin," +
                                "pre.p49 as Predio," +
                                "rol.p52_1 + ' - ' + rol.p52_2 as Rol," +
                                "rol.p54 as Superficie," +
                                "rol.p55 as Region," +
                                "gr.Geografia_nombre as Glo_Region," +
                                "rol.p56 as Comuna," +
                                "gc.Geografia_nombre as Glo_Comuna " +
                                "from CAF_PROCESAMIENTO.dbo.ges_resumen_movil as a " +
                                "inner join CAF_PROCESAMIENTO.dbo.glo_estado_movil as b on a.esmo_id = b.esmo_id " +
                                "left join CAF_PROCESAMIENTO.dbo.Enc_Explotacion as ex on a.remo_guid = ex.guid " +
                                "left join CAF_PROCESAMIENTO.dbo.Enc_Establecimiento as etb on ex.guid = etb.guid and ex.pk_explotacion = etb.pk_explotacion " +
                                "left join CAF_PROCESAMIENTO.dbo.Enc_Predio as pre on etb.guid = pre.guid and etb.PK_Establecimiento = pre.PK_Establecimiento " +
                                "left join CAF_PROCESAMIENTO.dbo.Enc_Rol as rol on pre.guid = rol.guid and pre.pk_predio = rol.pk_predio " +
                                "left join GESTION_CA.dbo.glo_geografia as gc on rol.p56 = gc.geografia_codigo and gc.geografia_nivel_id = 3 " +
                                "left join GESTION_CA.dbo.glo_geografia as gr on rol.p55 = gr.geografia_codigo and gr.geografia_nivel_id = 1 " +
                                "where a.remo_guid = @guid";
                }
                _exQuery.AgregarParametro("guid", guid);
                return _exQuery.EjecutarConsulta(_strSql);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Lista cuentas de informantes web a crear
        /// </summary>
        public DataSet ListaCuentasInformante(string rut, int accion)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure("Pa_supervision_web_listar");
                _exQuery.AgregarParametro("rut", rut);
                _exQuery.AgregarParametro("accion", accion);
                return _exQuery.EjecutarProcedimiento();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Lista grupos segun formulario para crear secciones
        /// </summary>
        public DataSet ListaGruposFormulario(int proceso_id, int proyecto_id, int servicio_id)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT DISTINCT s.seccion_id, seccion_nombre FROM ges_formulario_grupo AS fg INNER JOIN dbo.glo_seccion AS s ON fg.seccion_id = s.seccion_id WHERE proceso_id = @proceso_id AND proyecto_id = @proyecto_id AND servicio_id = @servicio_id ORDER BY s.seccion_id ASC ";
                _exQuery.AgregarParametro("proceso_id", proceso_id);
                _exQuery.AgregarParametro("proyecto_id", proyecto_id);
                _exQuery.AgregarParametro("servicio_id", servicio_id);
                return _exQuery.EjecutarConsulta(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista cuestionarios para supervision movil segun estado
        /// </summary>
        public DataSet ListaCuestionariosSegunEstado(int alc, int tipo, string usu, int lev)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure("Pa_ges_resumen_listar");
                _exQuery.AgregarParametro("alc", alc);
                _exQuery.AgregarParametro("tipo", tipo);
                _exQuery.AgregarParametro("usuario", usu);
                _exQuery.AgregarParametro("tipo_lev", lev);
                return _exQuery.EjecutarProcedimiento();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Lista errores según cuestionario
        /// </summary>
        public DataSet ListaErroresCuestionario(string guid)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure("Pa_supervision_muestraErrores");
                _exQuery.AgregarParametro("guid", guid);
                return _exQuery.EjecutarProcedimiento();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista errores según cuestionario
        /// </summary>
        public DataSet ListaObservacionesErrores(string guid)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure("Pa_supervision_muestraObsErrores");
                _exQuery.AgregarParametro("guid", guid);
                return _exQuery.EjecutarProcedimiento();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista errores VALIDACIONES DE TERCER NIVEL SEGUN GUID
        /// </summary>
        public DataSet ListaErroresCuestionarioVal3(string guid)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure("Pa_supervision_muestraErroresVal3");
                _exQuery.AgregarParametro("guid", guid);
                return _exQuery.EjecutarProcedimiento();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista errores VALIDACIONES WEB DE TERCER NIVEL SEGUN RUT Y ESTABLECIMIENTO
        /// </summary>
        public DataSet ListaErroresCuestionarioWebVal3(int rut, int establecimiento_id)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure("Pa_supervision_muestraErroresWebVal3");
                _exQuery.AgregarParametro("rut", rut);
                _exQuery.AgregarParametro("establecimiento_id", establecimiento_id);
                return _exQuery.EjecutarProcedimiento();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista errores según cuestionario
        /// </summary>
        public DataSet ListaJustificacionErrores(string guid, string idError)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure("Pa_supervision_muestraJustificacion");
                _exQuery.AgregarParametro("guid", guid);
                _exQuery.AgregarParametro("idError", idError);
                return _exQuery.EjecutarProcedimiento();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista supervisión directa  del cuestionario
        /// </summary>
        public DataSet ListaSupervisionPorGuid(string guid, int IdTipoSupervision)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();

                if (IdTipoSupervision == 3) {
                    _strSql = "SELECT RL.Reslev_guid, RL.Reslev_tipo_levantamiento, RL.Reslev_total_per, RL.Reslev_cant_hog, RL.Reslev_nombre_informante, RL.Reslev_viv_ocupada, RL.Reslev_alc " +
                                  ",SD.IdTipoSupervision, SD.IdSupervision, SD.IdEstadoSupervision, SD.sup_col1, SD.sup_col2, SD.sup_col3, SD.sup_col4 " +
                              "FROM [dbo].[GES_SUPERVISION_DETALLE] SD " +
                                    "INNER JOIN CAWICPV_2023..GES_RESUMEN_LEVANTAMIENTO RL ON RL.Reslev_guid = SD.IdSupervision " +
                              "WHERE SD.IdTipoSupervision = @IdTipoSupervision AND SD.IdSupervision = @guid " +
                                    "AND RL.Reslev_viv_ocupada = 1  ";
                } else if (IdTipoSupervision == 4) {
                    _strSql = "SELECT RL.Reslev_guid, RL.Reslev_tipo_levantamiento, RL.Reslev_total_per, RL.Reslev_cant_hog, RL.Reslev_nombre_informante, RL.Reslev_viv_ocupada, RL.Reslev_alc " +
                                  ",SD.IdTipoSupervision, SD.IdSupervision, SD.IdEstadoSupervision, SD.sup_col1, SD.sup_col2, SD.sup_col3, SD.sup_col4 " +
                              "FROM [dbo].[GES_SUPERVISION_DETALLE] SD " +
                                    "INNER JOIN CAWICPV_2023..GES_RESUMEN_LEVANTAMIENTO RL ON RL.Reslev_guid = SD.IdSupervision " +
                              "WHERE SD.IdTipoSupervision = @IdTipoSupervision AND SD.IdSupervision = @guid " +
                                    "AND RL.Reslev_cant_hog > 1 OR RL.Reslev_total_per > 10  ";
                } else {
                    _strSql = "SELECT TOP 1 * FROM [dbo].[GES_SUPERVISION_DETALLE] WHERE IdTipoSupervision = @IdTipoSupervision AND IdSupervision = @guid";
                }
                
                _exQuery.AgregarParametro("guid", guid);
                _exQuery.AgregarParametro("IdTipoSupervision", IdTipoSupervision);
                return _exQuery.EjecutarConsulta(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// lista datos para precarga de datos en supervision indirecta indagacion
        /// </summary>
        public DataSet ListaPrecargaSupIndirectaPorGuid(string guid)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "select c.ghrGUID, b.Superficie, d.esmo_id, e.esmo_glosa, count(b.Roles) CantidadRoles " +
                        "from [CAF_PROCESAMIENTO].[dbo].[Ges_IndagacionOperativa] as c " +
                        "inner join [CAF_PROCESAMIENTO].[dbo].[Ges_IndagacionOperativa_Detalle] as a on c.ghrGUID = a.ghrGUID " +
                        "inner join [CAF_PROCESAMIENTO].[dbo].[Ges_IndagacionOperativa_Detalle_Roles] as b on a.iD = b.ghrdGUID " +
                        "inner join [CAF_PROCESAMIENTO].[dbo].[ges_resumen_movil] as d on c.ghrGUID = d.remo_guid " +
                        "inner join [CAF_PROCESAMIENTO].[dbo].[glo_estado_movil] as e on e.esmo_id = d.esmo_id " +
                        "where d.esmo_id in (11,12) and c.ghrGUID = @guid " +
                        "group by c.ghrGUID, c.Folio, b.Superficie, d.esmo_id, e.esmo_glosa, b.Suelo"; 
                _exQuery.AgregarParametro("guid", guid);
                return _exQuery.EjecutarConsulta(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista errores según cuestionario e identificador de la regla
        /// </summary>
        public DataSet ListaErrorCuestionarioPorGuidRegla(string guid, string error_id)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT * FROM [dbo].[ges_dato] WHERE catalogo_id = 56 AND sup_col1 = @guid AND sup_col3 = @error_id";
                _exQuery.AgregarParametro("guid", guid);
                _exQuery.AgregarParametro("error_id", error_id);
                return _exQuery.EjecutarConsulta(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista errores según cuestionario e identificador de la regla
        /// </summary>
        public DataSet ListaErrorCuestionarioPorGuidReglaVal3(string guid, string error_id)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT * FROM [dbo].[ges_dato] WHERE catalogo_id = 70 AND sup_col1 = @guid AND sup_col3 = @error_id";
                _exQuery.AgregarParametro("guid", guid);
                _exQuery.AgregarParametro("error_id", error_id);
                return _exQuery.EjecutarConsulta(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista datos precargados
        /// </summary>
        public DataSet ObtieneDatosPrecargados(string guid, int tipo_sup, int tipo_lev)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure("Pa_supervision_muestraFormPrecarga");
                _exQuery.AgregarParametro("guid", guid);
                _exQuery.AgregarParametro("tipo_sup", tipo_sup);
                _exQuery.AgregarParametro("tipo_lev", tipo_lev);
                return _exQuery.EjecutarProcedimiento();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Lista precargas de uso de suelo para supervision indirecta indagacion y cuestionario
        /// </summary>
        public DataSet ObtienePrecargaUnidadDatosSupervisar(string guid, int tipo)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure("Pa_Precarga_UnidadDatosSupervisar");
                _exQuery.AgregarParametro("guid", guid);
                _exQuery.AgregarParametro("tipo", tipo);
                return _exQuery.EjecutarProcedimiento();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }        

        /// <summary>
        /// Lista cuestionarios para supervision movil segun estado
        /// </summary>
        public DataSet ObtieneFormulariosMovil(string guid, int codigo)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure("[CAF_PROCESAMIENTO].[dbo].[PA_GES_FORMULARIO_LISTAR]");
                _exQuery.AgregarParametro("guid", guid);
                _exQuery.AgregarParametro("codigo", codigo);
                return _exQuery.EjecutarProcedimiento();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Permite listar datos de asignaciones de ALC asociadas a locales 
        /// </summary>
        public List<T> ListaDatosTipoLevantamiento<T>(string tipo_sup)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT CONVERT(VARCHAR,IdTipoLevantamiento) as codigo, Nombre as valor " +
                          "FROM dbo.GLO_TIPO_LEVANTAMIENTO ";

                if (tipo_sup.Equals("1")) {
                    _strSql = _strSql + "WHERE IdTipoLevantamiento IN (1,3); ";
                }

                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// inserta formularios supervision segun estado
        /// </summary>
        public DataSet InsertarSupervision(GesSupervisionBOL _gesSupervisionBOL)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure("Pa_supervision_insertar");
                _exQuery.AgregarParametro("IdTipoSupervision", _gesSupervisionBOL.IdTipoSupervision);
                _exQuery.AgregarParametro("IdSupervision", _gesSupervisionBOL.IdSupervision);
                _exQuery.AgregarParametro("IdTipoLevantamiento", _gesSupervisionBOL.IdTipoLevantamiento);
                _exQuery.AgregarParametro("sup_col1", _gesSupervisionBOL.sup_col1);
                _exQuery.AgregarParametro("sup_col2", _gesSupervisionBOL.sup_col2);
                _exQuery.AgregarParametro("sup_col3", _gesSupervisionBOL.sup_col3);
                _exQuery.AgregarParametro("sup_col4", _gesSupervisionBOL.sup_col4);
                _exQuery.AgregarParametro("sup_col5", _gesSupervisionBOL.sup_col5);
                _exQuery.AgregarParametro("sup_col6", _gesSupervisionBOL.sup_col6);
                _exQuery.AgregarParametro("sup_col7", _gesSupervisionBOL.sup_col7);
                _exQuery.AgregarParametro("sup_col8", _gesSupervisionBOL.sup_col8);
                _exQuery.AgregarParametro("sup_col9", _gesSupervisionBOL.sup_col9);
                _exQuery.AgregarParametro("sup_col10", _gesSupervisionBOL.sup_col10);
                _exQuery.AgregarParametro("sup_col11", _gesSupervisionBOL.sup_col11);
                _exQuery.AgregarParametro("sup_col12", _gesSupervisionBOL.sup_col12);
                _exQuery.AgregarParametro("sup_col13", _gesSupervisionBOL.sup_col13);
                _exQuery.AgregarParametro("sup_col14", _gesSupervisionBOL.sup_col14);
                _exQuery.AgregarParametro("sup_col15", _gesSupervisionBOL.sup_col15);
                _exQuery.AgregarParametro("sup_col16", _gesSupervisionBOL.sup_col16);
                _exQuery.AgregarParametro("sup_col17", _gesSupervisionBOL.sup_col17);
                _exQuery.AgregarParametro("sup_col18", _gesSupervisionBOL.sup_col18);
                _exQuery.AgregarParametro("sup_col19", _gesSupervisionBOL.sup_col19);
                _exQuery.AgregarParametro("sup_col20", _gesSupervisionBOL.sup_col20);
                _exQuery.AgregarParametro("sup_col21", _gesSupervisionBOL.sup_col21);
                _exQuery.AgregarParametro("sup_col22", _gesSupervisionBOL.sup_col22);
                _exQuery.AgregarParametro("sup_col23", _gesSupervisionBOL.sup_col23);
                _exQuery.AgregarParametro("sup_col24", _gesSupervisionBOL.sup_col24);
                _exQuery.AgregarParametro("sup_col25", _gesSupervisionBOL.sup_col25);
                _exQuery.AgregarParametro("sup_col26", _gesSupervisionBOL.sup_col26);
                _exQuery.AgregarParametro("sup_col27", _gesSupervisionBOL.sup_col27);
                _exQuery.AgregarParametro("sup_col28", _gesSupervisionBOL.sup_col28);
                _exQuery.AgregarParametro("sup_col29", _gesSupervisionBOL.sup_col29);
                _exQuery.AgregarParametro("sup_col30", _gesSupervisionBOL.sup_col30);
                _exQuery.AgregarParametro("sup_col31", _gesSupervisionBOL.sup_col31);
                _exQuery.AgregarParametro("sup_col32", _gesSupervisionBOL.sup_col32);
                _exQuery.AgregarParametro("sup_col33", _gesSupervisionBOL.sup_col33);
                _exQuery.AgregarParametro("sup_col34", _gesSupervisionBOL.sup_col34);
                _exQuery.AgregarParametro("sup_col35", _gesSupervisionBOL.sup_col35);
                _exQuery.AgregarParametro("sup_col36", _gesSupervisionBOL.sup_col36);
                _exQuery.AgregarParametro("sup_col37", _gesSupervisionBOL.sup_col37);
                _exQuery.AgregarParametro("sup_col38", _gesSupervisionBOL.sup_col38);
                _exQuery.AgregarParametro("sup_col39", _gesSupervisionBOL.sup_col39);
                _exQuery.AgregarParametro("sup_col40", _gesSupervisionBOL.sup_col40);
                _exQuery.AgregarParametro("sup_col41", _gesSupervisionBOL.sup_col41);
                _exQuery.AgregarParametro("sup_col42", _gesSupervisionBOL.sup_col42);
                _exQuery.AgregarParametro("sup_col43", _gesSupervisionBOL.sup_col43);
                _exQuery.AgregarParametro("sup_col44", _gesSupervisionBOL.sup_col44);
                _exQuery.AgregarParametro("sup_col45", _gesSupervisionBOL.sup_col45);
                _exQuery.AgregarParametro("sup_col46", _gesSupervisionBOL.sup_col46);
                _exQuery.AgregarParametro("sup_col47", _gesSupervisionBOL.sup_col47);
                _exQuery.AgregarParametro("sup_col48", _gesSupervisionBOL.sup_col48);
                _exQuery.AgregarParametro("sup_col49", _gesSupervisionBOL.sup_col49);
                _exQuery.AgregarParametro("sup_col50", _gesSupervisionBOL.sup_col50);
                _exQuery.AgregarParametro("sup_col51", _gesSupervisionBOL.sup_col51);
                _exQuery.AgregarParametro("sup_col52", _gesSupervisionBOL.sup_col52);
                _exQuery.AgregarParametro("sup_col53", _gesSupervisionBOL.sup_col53);
                _exQuery.AgregarParametro("sup_col54", _gesSupervisionBOL.sup_col54);
                _exQuery.AgregarParametro("sup_col55", _gesSupervisionBOL.sup_col55);
                _exQuery.AgregarParametro("sup_col56", _gesSupervisionBOL.sup_col56);
                _exQuery.AgregarParametro("sup_col57", _gesSupervisionBOL.sup_col57);
                _exQuery.AgregarParametro("sup_col58", _gesSupervisionBOL.sup_col58);
                _exQuery.AgregarParametro("sup_col59", _gesSupervisionBOL.sup_col59);
                _exQuery.AgregarParametro("sup_col60", _gesSupervisionBOL.sup_col60);
                return _exQuery.EjecutarProcedimiento();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// inserta formularios supervision segun estado
        /// </summary>
        public DataSet InsertarSupervisionWeb(GesSupervisionBOL _gesSupervisionBOL)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure("Pa_supervision_insertarSegunEstadoWeb");
                _exQuery.AgregarParametro("sup_col1", _gesSupervisionBOL.sup_col1);
                _exQuery.AgregarParametro("sup_col2", _gesSupervisionBOL.sup_col2);
                _exQuery.AgregarParametro("sup_col3", _gesSupervisionBOL.sup_col3);
                _exQuery.AgregarParametro("sup_col4", _gesSupervisionBOL.sup_col4);
                _exQuery.AgregarParametro("sup_col5", _gesSupervisionBOL.sup_col5);
                _exQuery.AgregarParametro("sup_col6", _gesSupervisionBOL.sup_col6);
                _exQuery.AgregarParametro("sup_col7", _gesSupervisionBOL.sup_col7);
                _exQuery.AgregarParametro("sup_col8", _gesSupervisionBOL.sup_col8);
                _exQuery.AgregarParametro("sup_col9", _gesSupervisionBOL.sup_col9);
                _exQuery.AgregarParametro("sup_col10", _gesSupervisionBOL.sup_col10);
                return _exQuery.EjecutarProcedimiento();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Lista supervisión directa  del cuestionario
        /// </summary>
        public DataSet ListaSupervisionWebPorId(string sup_col1, int catalogo_id)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT * FROM [dbo].[ges_dato] WHERE catalogo_id = @catalogo_id AND sup_col1 = @sup_col1";
                _exQuery.AgregarParametro("sup_col1", sup_col1);
                _exQuery.AgregarParametro("catalogo_id", catalogo_id);
                return _exQuery.EjecutarConsulta(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Permite aperturar cuestionarios finalizados
        /// </summary>
        public DataSet AperturarCuestionariosWeb(string explotacion_rut, int resumen_web_establecimiento_id)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure("Pa_ges_establecimiento_apertura_cuestionario");
                _exQuery.AgregarParametro("explotacion_rut", explotacion_rut);
                _exQuery.AgregarParametro("resumen_web_establecimiento_id", resumen_web_establecimiento_id);
                return _exQuery.EjecutarProcedimiento();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Lista las areas censales encontradas
        /// </summary>
        public DataSet ObtieneAreaCensal()
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT distinct remo_area_censal FROM [CAF_PROCESAMIENTO].[dbo].[ges_resumen_movil] WHERE remo_area_censal IN (SELECT area_levantamiento_id FROM dbo.ges_area_levantamiento) ORDER BY remo_area_censal ";
                return _exQuery.EjecutarConsulta(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista las areas censales encontradas
        /// </summary>
        public DataSet ObtieneRecolector()
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT DISTINCT remo_usu_id FROM [CAF_PROCESAMIENTO].[dbo].[ges_resumen_movil] ";
                return _exQuery.EjecutarConsulta(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// inserta formularios supervision segun estado
        /// </summary>
        public DataSet InsertarSupervisionIndirecta(string recolector, int estado)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure("Pa_supervision_indirecta_insertar");
                _exQuery.AgregarParametro("recolector", recolector);
                _exQuery.AgregarParametro("estado", estado);
                return _exQuery.EjecutarProcedimiento();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// inserta formularios supervision segun estado
        /// </summary>
        public DataSet InsertarAsignacionIndirectaManual(string guid, int estado_id)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "INSERT INTO [CAF_PROCESAMIENTO].[dbo].[Ges_supervision_indirecta](suin_guid, suin_estado_supervision, suin_fecha) VALUES(@guid, 0, getdate())  ";
                _exQuery.AgregarParametro("guid", guid);
                _exQuery.AgregarParametro("estado_id", estado_id);
                return _exQuery.EjecutarConsulta(_strSql);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// marca aviso a recolector
        /// </summary>
        public DataSet InsertarAvisoRecolector(string guid, int accion)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "UPDATE [CAF_PROCESAMIENTO].[dbo].[ges_resumen_movil] SET Aviso = @accion WHERE remo_guid = @guid";
                _exQuery.AgregarParametro("guid", guid);
                _exQuery.AgregarParametro("accion", accion);
                return _exQuery.EjecutarConsulta(_strSql);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// traspasar info a supervisor
        /// </summary>
        public DataSet InsertarASupervisorVal3(string guid)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "UPDATE [CAF_PROCESAMIENTO].[dbo].[Val3_Rev] SET Accion = 1 WHERE [GUID] = @guid";
                _exQuery.AgregarParametro("guid", guid);
                return _exQuery.EjecutarConsulta(_strSql);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// traspasar info a supervisor
        /// </summary>
        public DataSet InsertarASupervisorProdWeb(string guid)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "UPDATE [CAF_PROCESAMIENTO].[dbo].[ges_resumen_movil] SET Marca_Web = 2 WHERE remo_guid = @guid";
                _exQuery.AgregarParametro("guid", guid);
                return _exQuery.EjecutarConsulta(_strSql);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Lista las areas censales encontradas
        /// </summary>
        public DataSet ObtieneEstadoSupervisionIndirecta(string guid)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT * FROM [CAF_PROCESAMIENTO].[dbo].[Ges_supervision_indirecta] AS a WHERE suin_guid = @guid ";
                _exQuery.AgregarParametro("guid", guid);
                return _exQuery.EjecutarConsulta(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// crear cuentas web
        /// </summary>
        public DataSet CrearCuenta(string rut, int accion)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure("Pa_supervision_web_listar");
                _exQuery.AgregarParametro("rut", rut);
                _exQuery.AgregarParametro("accion", accion);
                return _exQuery.EjecutarProcedimiento();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Lista mapa
        /// </summary>
        public List<T> ListaMapa<T>(string usu_id, string parametro_filtro)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure("Pa_supervision_muestra_mapa");
                _exQuery.AgregarParametro("usu_id", usu_id);
                _exQuery.AgregarParametro("parametro_filtro", parametro_filtro);
                return _exQuery.EjecutarProcedimiento<T>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}