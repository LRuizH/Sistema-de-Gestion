using Framework.BOL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Framework.DAL
{
    public class GesAsignacionesDAL
    {
        string _strSql = "";

        /// <summary>
        /// Permite listar usuarios sin asignar
        /// </summary>
        public List<T> ObtieneUsuariosSinAsignacion<T>()
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT Usu_id = U.IdUsuario, Usu_nombre = U.Nombre, Usu_email = U.Email  " +
                        " FROM dbo.GES_USUARIO U  " +
                        " WHERE U.IdUsuario NOT IN (SELECT IdUsuario from dbo.GES_ASIGNACIONES) ";
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Permite listar usuarios asignados
        /// </summary>
        public List<T> ObtieneUsuariosAsignados<T>()
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT Usu_id = U.IdUsuario, Usu_nombre = U.Nombre, Usu_email = U.Email  " +
                        " FROM dbo.GES_USUARIO U  " +
                        " WHERE U.IdUsuario IN (SELECT IdUsuario from dbo.GES_ASIGNACIONES) ";
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Permite listar usuarios asignados
        /// </summary>
        public List<T> ListaSegunPerfil<T>(int sistema_id, int tipo_asig, int areacensal, string usu, int geo, int perfil_id)    //int _perfil
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                //_strSql = "SELECT Usu_id = usu.usu_id,Usu_nombre = usu.usu_nombre,Usu_rut = usu.usu_rut,Usu_email = usu.usu_email,Usu_telefono = usu.usu_telefono from dbo.ges_usuario AS usu inner join ges_atributo_usuario as au on usu.usu_id = au.usu_id where perfil_id = @perfil_id";
                _strSql = "IF @tipo_asig = 10 BEGIN " +
                            "SELECT Usu_id = U.IdUsuario,Usu_nombre = U.Nombre,Usu_rut = U.Rut,Usu_email = U.Email,Usu_telefono = U.Telefono " +
                            "FROM dbo.GES_USUARIO U " +
                                "INNER JOIN dbo.GES_GEOUSUARIO GU ON GU.IdUsuario = U.IdUsuario " +
                                "INNER JOIN dbo.GES_ATRIBUTO_USUARIO AU ON AU.IdUsuario = U.IdUsuario AND AU.IdSistema = GU.IdSistema " +
                            "WHERE GU.IdSistema = @sistema_id AND U.Activo = 1 AND AU.IdPerfil = 6 " +
                                "AND GU.IdGeografia = @geo " +
                                "AND U.IdUsuario IN (SELECT A.IdAsignacionDe FROM dbo.GES_ASIGNACIONES A WHERE A.IdSistema = @sistema_id AND A.IdTipoAsignacion = 5 AND A.IdAsignacionA = @areacensal) " +
                          "END " +
                          "IF @tipo_asig = 11 BEGIN " +
                            "IF @perfil_id = 6 BEGIN " +
                                "SELECT Usu_id = U.IdUsuario,Usu_nombre = U.Nombre,Usu_rut = U.Rut,Usu_email = U.Email,Usu_telefono = U.Telefono " +
                                "FROM dbo.GES_USUARIO U " +
                                    "INNER JOIN dbo.GES_GEOUSUARIO GU ON GU.IdUsuario = U.IdUsuario " +
                                    "INNER JOIN dbo.GES_ATRIBUTO_USUARIO AU ON  AU.IdUsuario = U.IdUsuario AND AU.IdSistema = GU.IdSistema " +
                                "WHERE GU.IdSistema = @sistema_id AND U.Activo = 1 " +
                                    "AND AU.IdPerfil = @perfil_id AND GU.IdGeografia = @geo " +
                                    "AND U.IdUsuario IN (SELECT IdAsignacionDe FROM dbo.GES_ASIGNACIONES A WHERE A.IdSistema = @sistema_id AND A.IdTipoAsignacion = 5 AND A.IdAsignacionA = @areacensal) " +
                            "END " +
                            "ELSE BEGIN " +
                                "SELECT Usu_id = U.IdUsuario,Usu_nombre = U.Nombre,Usu_rut = U.Rut,Usu_email = U.Email,Usu_telefono = U.Telefono " +
                                "FROM dbo.GES_USUARIO U " +
                                    "INNER JOIN dbo.GES_GEOUSUARIO GU ON GU.IdUsuario = U.IdUsuario " +
                                    "INNER JOIN dbo.GES_ATRIBUTO_USUARIO AU ON  AU.IdUsuario = U.IdUsuario AND AU.IdSistema = GU.IdSistema " +
                                "WHERE GU.IdSistema = @sistema_id AND U.Activo = 1 " +
                                    "AND AU.IdPerfil = @perfil_id AND GU.IdGeografia = @geo " +
                                    "AND U.IdUsuario IN (SELECT IdAsignacionDe FROM dbo.GES_ASIGNACIONES A WHERE A.IdSistema = @sistema_id AND A.IdTipoAsignacion = 5 AND A.IdAsignacionA = @areacensal) " +
                                    "AND U.IdUsuario IN (SELECT IdAsignacionDe FROM dbo.GES_ASIGNACIONES A WHERE A.IdSistema = @sistema_id AND A.IdTipoAsignacion = 10 AND A.IdAsignacionA = @usu) " +
                            "END " +
                          "END ";
                        //"if @tipo_asig in (1,2,7,8) begin " +
                        //    "if @perfil_id = 6 begin " +
                        //        //muestra coordinadores que esten asignados a area censal seleccionada
                        //        "SELECT Usu_id = usu.usu_id,Usu_nombre = usu.usu_nombre,Usu_rut = usu.usu_rut,Usu_email = usu.usu_email,Usu_telefono = usu.usu_telefono " +
                        //        "from dbo.ges_usuario AS usu " +
                        //        "inner join ges_geousuario as geou " +
                        //        "on usu.usu_id = geou.usu_id " +
                        //        "inner join ges_atributo_usuario as atr " +
                        //        "on usu.usu_id = atr.usu_id and atr.sistema_id = geou.sistema_id " +
                        //        "where geou.sistema_id = @sistema_id and usu_activo = 1 and perfil_id = 5 and geografia_id = @geo and " +
                        //        "usu.usu_id in (select asignaciones_codigo_de from dbo.ges_asignaciones where sistema_id = @sistema_id and tipo_asignacion_id = 1 and asignaciones_codigo_a = @areacensal )  " +
                        //    "end " +
                        //    "else if  @perfil_id = 7 begin " +
                        //        // muestra supervisores que esten asignados a area censal seleccionada
                        //        "SELECT Usu_id = usu.usu_id,Usu_nombre = usu.usu_nombre,Usu_rut = usu.usu_rut,Usu_email = usu.usu_email,Usu_telefono = usu.usu_telefono " +
                        //        "from dbo.ges_usuario AS usu " +
                        //        "inner join ges_geousuario as geou " +
                        //        "on usu.usu_id = geou.usu_id " +
                        //        "inner join ges_atributo_usuario as atr " +
                        //        "on usu.usu_id = atr.usu_id and atr.sistema_id = geou.sistema_id " +
                        //        "where geou.sistema_id = @sistema_id and usu_activo = 1 and perfil_id = 6 and geografia_id = @geo and " +
                        //              "usu.usu_id in (select asignaciones_codigo_de from dbo.ges_asignaciones where sistema_id = @sistema_id and tipo_asignacion_id = 1 and asignaciones_codigo_a = @areacensal ) and " +
                        //              "usu.usu_id in (select asignaciones_codigo_de from dbo.ges_asignaciones where sistema_id = @sistema_id and tipo_asignacion_id = 7) " +
                        //    "end " +
                        //    "else begin " +
                        //        "SELECT Usu_id = usu.usu_id,Usu_nombre = usu.usu_nombre,Usu_rut = usu.usu_rut,Usu_email = usu.usu_email,Usu_telefono = usu.usu_telefono " +
                        //        "from dbo.ges_usuario AS usu " +
                        //        "inner join ges_geousuario as geou " +
                        //        "on usu.usu_id = geou.usu_id " +
                        //        "inner join ges_atributo_usuario as atr " +
                        //        "on usu.usu_id = atr.usu_id and atr.sistema_id = geou.sistema_id " +
                        //        "where geou.sistema_id = @sistema_id and usu_activo = 1 and perfil_id = 6 and geografia_id = @geo and " +
                        //              "usu.usu_id in (select asignaciones_codigo_de from dbo.ges_asignaciones where sistema_id = @sistema_id and tipo_asignacion_id = 1 and asignaciones_codigo_a = @areacensal ) and " +
                        //              "usu.usu_id in (select asignaciones_codigo_de from dbo.ges_asignaciones where sistema_id = @sistema_id and tipo_asignacion_id = @tipo_asig) " +
                        //    "end " +
                        //"end " +
                        //"else if @tipo_asig in (3) begin " +
                        //    // muestra supervisores que esten asignados a area censal seleccionada
                        //    "if @perfil_id = 6 begin " +
                        //        "SELECT Usu_id = usu.usu_id,Usu_nombre = usu.usu_nombre,Usu_rut = usu.usu_rut,Usu_email = usu.usu_email,Usu_telefono = usu.usu_telefono " +
                        //        "from dbo.ges_usuario AS usu " +
                        //        "inner join ges_geousuario as geou " +
                        //        "on usu.usu_id = geou.usu_id " +
                        //        "inner join ges_atributo_usuario as atr " +
                        //        "on usu.usu_id = atr.usu_id and atr.sistema_id = geou.sistema_id " +
                        //        "where geou.sistema_id = @sistema_id and usu_activo = 1 and perfil_id = @perfil_id and geografia_id = @geo and " +
                        //                "usu.usu_id in (select asignaciones_codigo_de from dbo.ges_asignaciones where sistema_id = @sistema_id and tipo_asignacion_id = 1 and asignaciones_codigo_a = @areacensal ) " +
                        //    "end " +
                        //    "else begin " +
                        //        "SELECT Usu_id = usu.usu_id,Usu_nombre = usu.usu_nombre,Usu_rut = usu.usu_rut,Usu_email = usu.usu_email,Usu_telefono = usu.usu_telefono " +
                        //        "from dbo.ges_usuario AS usu " +
                        //        "inner join ges_geousuario as geou " +
                        //        "on usu.usu_id = geou.usu_id " +
                        //        "inner join ges_atributo_usuario as atr " +
                        //        "on usu.usu_id = atr.usu_id and atr.sistema_id = geou.sistema_id " +
                        //        "where geou.sistema_id = @sistema_id and usu_activo = 1 and perfil_id = @perfil_id and geografia_id = @geo and " +
                        //                "usu.usu_id in (select asignaciones_codigo_de from dbo.ges_asignaciones where sistema_id = @sistema_id and tipo_asignacion_id = 1 and asignaciones_codigo_a = @areacensal ) and " +
                        //                "usu.usu_id in (select asignaciones_codigo_de from dbo.ges_asignaciones where sistema_id = @sistema_id and tipo_asignacion_id = 8 and asignaciones_codigo_a =  @usu ) " +
                        //    "end " +
                        //"end " +
                        //"else if @tipo_asig in (4,5) begin " +
                        //        // muestra supervisores o analistas
                        //        //"if @perfil_id = 6 begin " +
                        //        "SELECT Usu_id = usu.usu_id,Usu_nombre = usu.usu_nombre,Usu_rut = usu.usu_rut,Usu_email = usu.usu_email,Usu_telefono = usu.usu_telefono " +
                        //        "from dbo.ges_usuario AS usu " +
                        //        "inner join ges_geousuario as geou " +
                        //        "on usu.usu_id = geou.usu_id " +
                        //        "inner join ges_atributo_usuario as atr " +
                        //        "on usu.usu_id = atr.usu_id and atr.sistema_id = geou.sistema_id " +
                        //        "where geou.sistema_id = @sistema_id and usu_activo = 1 and perfil_id = @perfil_id " +
                        ////"end "
                        //"end " +
                        //"else if @tipo_asig in (6) begin " +
                        //    "if @perfil_id = 17 begin " +
                        //        "SELECT Usu_id = usu.usu_id,Usu_nombre = usu.usu_nombre,Usu_rut = usu.usu_rut,Usu_email = usu.usu_email,Usu_telefono = usu.usu_telefono " +
                        //        "from dbo.ges_usuario AS usu " +
                        //        "inner join ges_geousuario as geou " +
                        //        "on usu.usu_id = geou.usu_id " +
                        //        "inner join ges_atributo_usuario as atr " +
                        //        "on usu.usu_id = atr.usu_id and atr.sistema_id = geou.sistema_id " +
                        //        "where geou.sistema_id = @sistema_id and usu_activo = 1 and perfil_id = @perfil_id " +
                        //    "end " +
                        //    "else begin " +
                        //        "SELECT Usu_id = usu.usu_id,Usu_nombre = usu.usu_nombre,Usu_rut = usu.usu_rut,Usu_email = usu.usu_email,Usu_telefono = usu.usu_telefono " +
                        //        "from dbo.ges_usuario AS usu " +
                        //        "inner join ges_atributo_usuario as atr " +
                        //        "on usu.usu_id = atr.usu_id " +
                        //        "where usu_activo = 1 and perfil_id = @perfil_id and " +
                        //                "usu.usu_id in (select asignaciones_codigo_de from dbo.ges_asignaciones where sistema_id = @sistema_id and tipo_asignacion_id = 5 and asignaciones_codigo_a =  @usu ) " +
                        //    "end " +
                        //"end ";
                _exQuery.AgregarParametro("sistema_id", sistema_id);
                _exQuery.AgregarParametro("perfil_id", perfil_id);
                _exQuery.AgregarParametro("geo", geo);
                _exQuery.AgregarParametro("tipo_asig", tipo_asig);
                _exQuery.AgregarParametro("areacensal", areacensal);
                _exQuery.AgregarParametro("usu", usu);
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista asignaciones segun tipo
        /// </summary>
        public DataSet ListarAsignaciones(int sistema_id, int tipo_asig, int parametroIndicador, string usu, int geo, int disponibles, int perfil_id)
        {
            try
            {
                DataSet dstest = new DataSet();
                StoredProcedure _exQuery = new StoredProcedure("Pa_GesAsignaciones_Listas");
                _exQuery.AgregarParametro("IdSistema", sistema_id);
                _exQuery.AgregarParametro("IdTipoAsignacion", tipo_asig);
                _exQuery.AgregarParametro("ParametroIndicador", parametroIndicador);
                _exQuery.AgregarParametro("IdUsuario", usu);
                _exQuery.AgregarParametro("IdGeografia", geo);
                _exQuery.AgregarParametro("Disponibles", disponibles);
                _exQuery.AgregarParametro("IdPerfil", perfil_id);
                dstest = _exQuery.EjecutarProcedimiento();
                return dstest;//_exQuery.EjecutarProcedimiento();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Inserta asignaciones
        /// </summary>
        public DataSet InsertarAsignaciones(int Tipo_asignacion_id, int Sistema_id, string asignaciones_codigo_de, string asignaciones_codigo_a, int nivel_asig, int perfil_asig)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure("Pa_GesAsignaciones_Insertar");
                _exQuery.AgregarParametro("IdTipoAsignacion", Tipo_asignacion_id);
                _exQuery.AgregarParametro("IdSistema", Sistema_id);
                _exQuery.AgregarParametro("IdAsignacionDe", asignaciones_codigo_de);
                _exQuery.AgregarParametro("IdAsignacionA", asignaciones_codigo_a);
                _exQuery.AgregarParametro("NivelAsignacion", nivel_asig);
                _exQuery.AgregarParametro("PerfilAsignacion", perfil_asig);
                //return _exQuery.EjecutarProcedimiento().Tables[0].Rows[0][0].ToString();
                return _exQuery.EjecutarProcedimiento();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Permite listar datos de usuario
        /// </summary>
        public DataSet ListaDatosUsuario(GesUsuarioBOL gesUsuarioBOL)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "DECLARE @tipo_asig INT = (SELECT MAX(A.IdTipoAsignacion) FROM dbo.GES_ASIGNACIONES A WHERE A.IdAsignacionDe = @IdUsuario AND A.IdTipoAsignacion < 10); " +
                          "SELECT usu_id = U.IdUsuario, usu_nombre = U.Nombre, usu_email = U.Email, geografia_id = GU.IdGeografia, perfil_id = AU.IdPerfil, " +
                                "(SELECT A.IdAsignacionA FROM dbo.GES_ASIGNACIONES A WHERE A.IdAsignacionDe = @IdUsuario AND A.IdTipoAsignacion = @tipo_asig) AS areacensal " +
                          "FROM dbo.GES_USUARIO U " +
                                "INNER JOIN dbo.GES_GEOUSUARIO GU ON GU.IdUsuario = U.IdUsuario " +
                                "INNER JOIN dbo.GES_ATRIBUTO_USUARIO AU ON AU.IdUsuario = U.IdUsuario AND GU.IdSistema = AU.IdSistema " +
                          "WHERE U.IdUsuario = @IdUsuario AND AU.AtributoUsuarioPerfilActivo = 1 ";
                _exQuery.AgregarParametro("IdUsuario", gesUsuarioBOL.Usu_id);
                return _exQuery.EjecutarConsulta(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Permite listar datos de asignaciones de una persona a area censal
        /// </summary>
        public DataSet ListaDatosAsigAreaCensal(GesUsuarioBOL gesUsuarioBOL)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT " +
                                "(SELECT A.IdAsignacionA FROM dbo.GES_ASIGNACIONES A WHERE A.IdAsignacionDe = @IdUsuario AND A.IdTipoAsignacion = 3) AS IdMacrosector, " +
                                "(SELECT A.IdAsignacionA FROM dbo.GES_ASIGNACIONES A WHERE A.IdAsignacionDe = @IdUsuario AND A.IdTipoAsignacion = 4) AS IdSector, " +
                                "(SELECT A.IdAsignacionA FROM dbo.GES_ASIGNACIONES A WHERE A.IdAsignacionDe = @IdUsuario AND A.IdTipoAsignacion = 5) AS IdLocalCensal ";
                _exQuery.AgregarParametro("IdUsuario", gesUsuarioBOL.Usu_id);
                return _exQuery.EjecutarConsulta(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Permite listar datos de asignaciones de ALC asociadas a locales 
        /// </summary>
        public List<T> ListaDatosALCLocal<T>(string id_local, string id_usuario, int perfil_usuario)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT CONVERT(VARCHAR,ALC.IdALC) AS codigo, 'ALC ' + ALC.NumeroALC + ' - ' + ALC.CodigoALC + ' | ' +  U.Nombre AS valor " +
                          "FROM dbo.GES_AREA_LEVANTAMIENTO AL " +
                                "INNER JOIN dbo.GES_ALC ALC ON ALC.CodLocalCensal = AL.IdUnicoLocalCensalGeo " +
                                "INNER JOIN dbo.GES_ASIGNACIONES A ON A.IdTipoAsignacion = 11 AND A.IdAsignacionDe = CONVERT(VARCHAR,ALC.IdALC) " +
                                "INNER JOIN dbo.GES_USUARIO U ON U.IdUsuario = A.IdAsignacionA " +
                          "WHERE AL.IdAreaLevantamiento = CONVERT(INT,@IdLocal) ";

                if (perfil_usuario == 6) {
                    _strSql = _strSql + "AND A.IdAsignacionA IN (SELECT A2.IdAsignacionDe FROM dbo.GES_ASIGNACIONES A2 WHERE A2.IdTipoAsignacion = 10 AND A2.IdAsignacionA = @IdUsuario) ";
                    _exQuery.AgregarParametro("IdUsuario", id_usuario);
                }

                _exQuery.AgregarParametro("IdLocal", id_local);
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Lista empresas para supervision
        /// </summary>
        public DataSet ListaResumenAsignacionEquipos(string usuario, int geo, int perfil_id, string id_area, int parametro)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure("Pa_GesAsignaciones_Resumenes");
                _exQuery.AgregarParametro("IdSistema", 1);
                _exQuery.AgregarParametro("IdUsuario", usuario);
                _exQuery.AgregarParametro("IdGeografia", geo);
                _exQuery.AgregarParametro("IdPerfil", perfil_id);
                _exQuery.AgregarParametro("IdArea", id_area);
                _exQuery.AgregarParametro("Parametro", parametro);
                return _exQuery.EjecutarProcedimiento();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public DataSet ListaDireccionesALC(string alc_id)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "DECLARE @CodALC VARCHAR(20) = (SELECT CodigoALC FROM dbo.GES_ALC WHERE IdALC = @IdALC);" +
                          "SELECT D.IdDireccion, D.CodALC, D.MANZENT, D.UbicacionGeografica,D.Direccion, " +
                                "CASE WHEN D.TipoVia = 'NULL' THEN NULL ELSE D.TipoVia END AS TipoVia, " +
                                "CASE WHEN D.NombreVia = 'NULL' THEN NULL ELSE D.NombreVia END AS NombreVia, " +
                                "CASE WHEN D.NumeroDomiciliario = 'NULL' THEN NULL ELSE D.NumeroDomiciliario END AS NumeroDomiciliario, " +
                                "CASE WHEN D.NumeroTerreno = 'NULL' THEN NULL ELSE D.NumeroTerreno END AS NumeroTerreno, " +
                                "CASE WHEN D.NumeroCasa = 'NULL' THEN NULL ELSE D.NumeroCasa END AS NumeroCasa, " +
                                "CASE WHEN D.NumeroPiso = 'NULL' THEN NULL ELSE D.NumeroPiso END AS NumeroPiso, " +
                                "CASE WHEN D.NumeroDepto = 'NULL' THEN NULL ELSE D.NumeroDepto END AS NumeroDepto, " +
                                "CASE WHEN D.UsoDestino = 'NULL' THEN NULL ELSE D.UsoDestino END AS UsoDestino, " +
                                "CASE WHEN D.Descripcion = 'NULL' THEN NULL ELSE D.Descripcion END AS Descripcion, " +
                                "CASE WHEN D.TipoDireccion = 'NULL' THEN NULL ELSE D.TipoDireccion END AS TipoDireccion, " +
                                "(SELECT G.Nombre FROM dbo.GLO_GEOGRAFIA G  WHERE G.IdGeografiaNivel= 3 AND G.Codigo= D.Cut) as Comuna, " +
                                " D.ManzanaEntidad " +
                          "FROM dbo.GES_DIRECCION D " +
                          "WHERE CodALC = @CodALC ";
                _exQuery.AgregarParametro("IdALC", alc_id);
                return _exQuery.EjecutarConsulta(_strSql);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Permite listar datos de usuario para envío de notificaciones
        /// </summary>
        public DataSet ListaDatosUsuarioNotificacion(int tipo_asig, string asig_de, string asig_a, int perfil)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();

                if (tipo_asig == 3 || tipo_asig == 4 || tipo_asig <= 5)
                {
                    _strSql = "SELECT A.IdAsignacionDe, U1.Nombre, U1.Email, A.IdAsignacionA, AL.Nombre AS NombreAreaCensal, S.Token " +
                              "FROM GES_ASIGNACIONES A  " +
                                  "INNER JOIN GES_USUARIO U1 ON U1.IdUsuario = A.IdAsignacionDe  AND U1.Email IS NOT NULL " +
                                  "INNER JOIN GES_AREA_LEVANTAMIENTO AL ON AL.IdAreaLevantamiento = CONVERT(INT,@IdAsignacionA) " +
                                  "INNER JOIN GES_SISTEMA S ON S.IdSistema = 1 " +
                              "WHERE A.IdSistema = 1 AND A.IdAsignacionDe = @IdAsignacionDe AND A.IdAsignacionA = @IdAsignacionA " +
                                  "AND A.IdTipoAsignacion = @IdTipoAsignacion ";
                }
                else if (tipo_asig == 10)
                {
                    _strSql = "SELECT A.IdAsignacionDe, U1.Nombre, U1.Email, A.IdAsignacionA, U2.Nombre AS NombreUsuario, S.Token " +
                              "FROM GES_ASIGNACIONES A  " +
                                  "INNER JOIN GES_USUARIO U1 ON U1.IdUsuario = A.IdAsignacionDe  AND U1.Email IS NOT NULL " +
                                  "INNER JOIN GES_USUARIO U2 ON U2.IdUsuario = A.IdAsignacionA " +
                                  "INNER JOIN GES_SISTEMA S ON S.IdSistema = 1 " +
                              "WHERE A.IdSistema = 1 AND A.IdAsignacionDe = @IdAsignacionDe AND A.IdAsignacionA = @IdAsignacionA " +
                                  "AND A.IdTipoAsignacion = @IdTipoAsignacion ";
                }
                else if (tipo_asig == 11)
                {
                    _strSql = "SELECT A.IdAsignacionDe, 'ALC ' + ALC.NumeroALC + ' - ' + ALC.CodigoALC AS NombreALC, A.IdAsignacionA, U1.Nombre, U1.Email, S.Token " +
                              "FROM GES_ASIGNACIONES A  " +
                                  "INNER JOIN GES_USUARIO U1 ON U1.IdUsuario = A.IdAsignacionA AND U1.Email IS NOT NULL " +
                                  "INNER JOIN GES_ALC ALC ON ALC.IdALC = CONVERT(INT,A.IdAsignacionDe) " +
                                  "INNER JOIN GES_SISTEMA S ON S.IdSistema = 1 " +
                              "WHERE A.IdSistema = 1 AND A.IdAsignacionDe = @IdAsignacionDe AND A.IdAsignacionA = @IdAsignacionA " +
                                  "AND A.IdTipoAsignacion = @IdTipoAsignacion ";
                }

                _exQuery.AgregarParametro("IdAsignacionDe", asig_de);
                _exQuery.AgregarParametro("IdAsignacionA", asig_a);
                _exQuery.AgregarParametro("IdTipoAsignacion", tipo_asig);
                return _exQuery.EjecutarConsulta(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Obtiene datos de usuario y datos SuSo
        /// </summary>
        public List<T> ObtieneDatosUsuarios<T>(string usu, int alc = 0)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();

                _strSql = "SELECT Usu_id = U.IdUsuario,Usu_nombre = U.Nombre,Usu_rut = U.Rut,Usu_email = U.Email,Usu_telefono = U.Telefono," +
                           "(SELECT TOP 1 US.UserId FROM dbo.GES_USUARIO_SUSO US WHERE US.Rut = U.Rut) AS Usu_idSuso, " +
                          "(SELECT DISTINCT 1 FROM  dbo.GES_ASIGNACION_VIVNUEVA A WHERE  A.ALC = @alc) AS Existe_asignacion " +
                           "FROM dbo.GES_USUARIO U " +
                             "LEFT JOIN dbo.GES_GEOUSUARIO GU ON GU.IdUsuario = U.IdUsuario " +
                             "LEFT JOIN dbo.GES_ATRIBUTO_USUARIO AU ON AU.IdUsuario = U.IdUsuario AND AU.IdSistema = GU.IdSistema " +
                            "WHERE U.Activo = 1 " +
                                "AND U.Rut = @usu";
                // _exQuery.AgregarParametro("sistema_id", sistema_id);
                _exQuery.AgregarParametro("usu", usu);
                _exQuery.AgregarParametro("alc", alc);
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<T> ObtieneDireccionesGeo<T>(string codAlc)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure("Pa_GesObtiene_DireccionesGeo");
                _exQuery.AgregarParametro("CodAlc", codAlc);
               
                //return _exQuery.EjecutarConsulta<T>(_strSql);
                return _exQuery.EjecutarProcedimiento<T>();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public List<T> ObtieneDetalleDireccion<T>(string codAlc, string IdDireccionPrincipal)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure("Pa_GesObtiene_Detalle_Direcciones");
                _exQuery.AgregarParametro("CodAlc", codAlc);
                _exQuery.AgregarParametro("dDireccionPrincipal", IdDireccionPrincipal);

                //return _exQuery.EjecutarConsulta<T>(_strSql);
                return _exQuery.EjecutarProcedimiento<T>();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
