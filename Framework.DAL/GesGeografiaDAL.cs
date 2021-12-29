using Framework.BOL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.DAL
{
    public class GesGeografiaDAL
    {
        string _strSql = "";

        /// <summary>
        /// Lista región
        /// </summary>
        public List<T> ListarRegion<T>()
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT Geografia_codigo = CONVERT(varchar(10),idgeografia), Geografia_nombre = Nombre " +
                        "FROM dbo.GLO_GEOGRAFIA " +
                        "WHERE IdGeografiaNivel = 1 order by posicion";

                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista comunas por región
        /// </summary>
        public List<T> ListarComunas<T>()
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT Geografia_codigo = G3.Codigo, Geografia_nombre = G3.Nombre " +
                        "FROM dbo.GLO_GEOGRAFIA G1 " +
                            "INNER JOIN dbo.GLO_GEOGRAFIA G2 ON G1.IdGeografia = G2.Padre AND G2.IdGeografiaNivel = 2 " +
                            "INNER JOIN dbo.GLO_GEOGRAFIA G3 ON G2.IdGeografia = G3.Padre AND G3.IdGeografiaNivel = 3 " +
                        "WHERE G1.IdGeografiaNivel = 1 " +
                            "ORDER BY Geografia_nombre ";

                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista comunas por región
        /// </summary>
        public List<T> ListarComunas<T>(int geografia_id)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT Geografia_id = G3.IdGeografia, Geografia_nombre = G3.Nombre " +
                        "FROM dbo.GLO_GEOGRAFIA G1 " +
                            "INNER JOIN dbo.GLO_GEOGRAFIA G2 ON G1.IdGeografia = G2.Padre AND G2.IdGeografiaNivel = 2 " +
                            "INNER JOIN dbo.GLO_GEOGRAFIA G3 ON G2.IdGeografia = G3.Padre AND G3.IdGeografiaNivel = 3 " +
                        "WHERE G1.IdGeografiaNivel = 1 " +
                            "AND G2.Padre = @IdGeografia order by G3.nombre";
                _exQuery.AgregarParametro("IdGeografia", geografia_id.ToString());

                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista comunas por región
        /// </summary>
        public List<T> ListarComunasPorRegion<T>(GesGeografiaBOL gesGeografiaBOL)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT codigo = CONVERT(VARCHAR,G3.Codigo), valor = G3.Nombre " +
                        "FROM dbo.GLO_GEOGRAFIA G1 " +
                            "INNER JOIN dbo.GLO_GEOGRAFIA G2 ON G1.IdGeografia = G2.Padre AND G2.IdGeografiaNivel = 2 " +
                            "INNER JOIN dbo.GLO_GEOGRAFIA G3 ON G2.IdGeografia = G3.Padre AND G3.IdGeografiaNivel = 3 " +
                        "WHERE G1.IdGeografiaNivel = 1 " +
                            "AND G2.Padre = @IdGeografia order by G3.nombre";
                _exQuery.AgregarParametro("IdGeografia", gesGeografiaBOL.Geografia_id.ToString());

                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }  

        /// <summary>
        /// Lista datos de comuna
        /// </summary>
        public List<T> ListarComuna<T>(int geografia_id)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT Sistema_id = IdSistema, Geografia_id = IdGeografia, Geografia_nivel_id = IdGeografiaNivel, Geografia_codigo = Codigo, Geografia_padre = Padre, Geografia_posicion = Posicion, Geografia_nombre = Nombre " +
                          "FROM dbo.GLO_GEOGRAFIA " +
                          "WHERE IdGeografiaNivel = 3 AND IdGeografia = @IdGeografia ";
                _exQuery.AgregarParametro("IdGeografia", geografia_id.ToString());

                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista geografía por nivel
        /// </summary>
        public List<T> Buscar<T>(int nivel)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT Geografia_id = IdGeografia, Geografia_codigo = Codigo, Geografia_padre = Padre, Geografia_nivel_id = IdGeografiaNivel, Geografia_nombre = Nombre, Geografia_posicion = Posicion " +
                          "FROM dbo.GLO_GEOGRAFIA " +
                          "WHERE IdGeografiaNivel <= @IdGeografiaNivel AND IdGeografiaNivel > 0 ";
                _exQuery.AgregarParametro("IdGeografiaNivel", nivel.ToString());

                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista region asignada a usuario
        /// </summary>
        public List<GesGeografiaBOL> BuscarGeografiaUsuario(GesUsuarioBOL gesUsuarioBOL)
        {
            StoredProcedure sp = new StoredProcedure();
            try
            {
                sp.AgregarParametro("IdUsuario", gesUsuarioBOL.Usu_id);
                _strSql = "SELECT Geografia_id = GU.IdGeografia, Geografia_nombre = G.Nombre " +
                        "FROM dbo.GES_GEOUSUARIO GU " +
                            "INNER JOIN dbo.GLO_GEOGRAFIA G ON G.IdGeografia = GU.IdGeografia " +
                        "WHERE GU.IdUsuario = @IdUsuario ";
                var data = sp.EjecutarConsulta<GesGeografiaBOL>(_strSql);
                return data;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Lista area censal según nivel (PROBAR)
        /// </summary>
        public List<T> BuscarAreaCensalSegunNivel<T>(GesGeografiaBOL gesGeografiaBOL)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT CONVERT(VARCHAR,IdAreaLevantamiento) AS codigo, Nombre AS valor " +
                            "FROM dbo.GES_AREA_LEVANTAMIENTO " +
                            "WHERE IdGeografia = @IdGeografia AND Nivel = @Nivel AND IdAreaLevantamiento <> 0 ";
                _exQuery.AgregarParametro("IdGeografia", gesGeografiaBOL.Geografia_id.ToString());
                _exQuery.AgregarParametro("Nivel", gesGeografiaBOL.Geografia_nivel_id.ToString());
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista area censal según region
        /// </summary>
        public List<T> BuscarAreaCensalSegunRegion<T>(GesGeografiaBOL gesGeografiaBOL, int tipoAsig, string idUsuario, int perfilUsuario)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                //_strSql = "select CONVERT(VARCHAR,area_levantamiento_id) codigo, area_levantamiento_nombre valor from ges_area_levantamiento where geografia_id = @geografia_id and area_levantamiento_nivel = 1 and area_levantamiento_id <> 0";
                if (perfilUsuario == 1 || perfilUsuario == 2)
                {
                    _strSql = "SELECT CONVERT(VARCHAR,IdAreaLevantamiento) AS codigo, Nombre AS valor " +
                                "FROM dbo.GES_AREA_LEVANTAMIENTO " +
                                "WHERE IdGeografia = @IdGeografia AND Nivel = 1 AND IdAreaLevantamiento <> 0 ";
                } else {
                    _strSql = "SELECT CONVERT(VARCHAR,AL.IdAreaLevantamiento) AS codigo, AL.Nombre AS valor " +
                                "FROM dbo.GES_AREA_LEVANTAMIENTO AL " +
                                    "INNER JOIN dbo.GES_ASIGNACIONES A ON A.IdAsignacionA = AL.IdAreaLevantamiento " +
                                "WHERE AL.IdGeografia = @IdGeografia AND AL.Nivel = 1 AND AL.IdAreaLevantamiento <> 0 " +
                                    "AND A.IdAsignacionDe = @IdUsuario AND A.IdTipoAsignacion = 3 ";
                                    //"AND A.IdAsignacionDe = @IdUsuario AND A.IdTipoAsignacion = @IdTipoAsig";
                    _exQuery.AgregarParametro("IdUsuario", idUsuario);
                    //_exQuery.AgregarParametro("IdTipoAsig", tipoAsig);
                }

                _exQuery.AgregarParametro("IdGeografia", gesGeografiaBOL.Geografia_id.ToString());
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista tipos de asignacion segun area
        /// </summary>
        public List<T> BuscarTipoAsigSegunArea<T>(GesGeografiaBOL gesGeografiaBOL, int tipo_asig)
        {
            try
            {
                string condicion = "";
                if (tipo_asig == 1) {
                    condicion = "3, 4, 5";
                } else if (tipo_asig == 2) {
                    condicion = "4, 5";
                } else if (tipo_asig == 3) {
                    condicion = "5";
                } else if (tipo_asig == 10) {
                    condicion = "10";
                } else if (tipo_asig == 11) {
                    condicion = "11";
                }

                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT CONVERT(VARCHAR,IdTipoAsignacion) AS codigo, 'Asignar a ' + Nombre AS valor " +
                            "FROM dbo.GLO_TIPO_ASIGNACION " +
                            "WHERE IdTipoAsignacion IN (" + condicion + ") ";
                _exQuery.AgregarParametro("IdGeografia", gesGeografiaBOL.Geografia_id.ToString());
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista sectores censales
        /// </summary>
        public List<T> BuscarSectorSegunArea<T>(GesGeografiaBOL gesGeografiaBOL, int tipoAsig, string idUsuario, int perfilUsuario)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                //_strSql = "select CONVERT(VARCHAR,area_levantamiento_id) codigo, area_levantamiento_nombre valor from ges_area_levantamiento where geografia_id = @geografia_id and area_levantamiento_nivel = 1 and area_levantamiento_id <> 0";
                //_strSql = "SELECT CONVERT(VARCHAR,IdAreaLevantamiento) AS codigo, Nombre AS valor " +
                //            "FROM dbo.GES_AREA_LEVANTAMIENTO " +
                //            "WHERE Padre = @IdGeografia AND Nivel = 2 AND IdAreaLevantamiento <> 0 ";

                if (perfilUsuario == 1 || perfilUsuario == 2 || perfilUsuario == 3)
                {
                    _strSql = "SELECT CONVERT(VARCHAR,IdAreaLevantamiento) AS codigo, Nombre AS valor " +
                                "FROM dbo.GES_AREA_LEVANTAMIENTO " +
                                "WHERE Padre = @IdGeografia AND Nivel = 2 AND IdAreaLevantamiento <> 0 ";
                }
                else
                {
                    _strSql = "SELECT CONVERT(VARCHAR,AL.IdAreaLevantamiento) AS codigo, AL.Nombre AS valor " +
                                "FROM dbo.GES_AREA_LEVANTAMIENTO AL " +
                                    "INNER JOIN dbo.GES_ASIGNACIONES A ON A.IdAsignacionA = AL.IdAreaLevantamiento " +
                                "WHERE AL.Padre = @IdGeografia AND AL.Nivel = 2 AND AL.IdAreaLevantamiento <> 0 " +
                                    "AND A.IdAsignacionDe = @IdUsuario AND A.IdTipoAsignacion = 4";
                                    //"AND A.IdAsignacionDe = @IdUsuario AND A.IdTipoAsignacion = @IdTipoAsig";
                    _exQuery.AgregarParametro("IdUsuario", idUsuario);
                    //_exQuery.AgregarParametro("IdTipoAsig", tipoAsig);
                }

                _exQuery.AgregarParametro("IdGeografia", gesGeografiaBOL.Geografia_id.ToString());
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista locales censales
        /// </summary>
        public List<T> BuscarLocalSegunArea<T>(GesGeografiaBOL gesGeografiaBOL, int tipoAsig, string idUsuario, int perfilUsuario)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                if (perfilUsuario == 1 || perfilUsuario == 2 || perfilUsuario == 3 || perfilUsuario == 4)
                {
                    _strSql = "SELECT CONVERT(VARCHAR,IdAreaLevantamiento) AS codigo, Nombre AS valor " +
                                "FROM dbo.GES_AREA_LEVANTAMIENTO " +
                                "WHERE Padre = @IdGeografia AND Nivel = 3 AND IdAreaLevantamiento <> 0 ";
                }
                else
                {
                    _strSql = "SELECT CONVERT(VARCHAR,AL.IdAreaLevantamiento) AS codigo, AL.Nombre AS valor " +
                                "FROM dbo.GES_AREA_LEVANTAMIENTO AL " +
                                    "INNER JOIN dbo.GES_ASIGNACIONES A ON A.IdAsignacionA = AL.IdAreaLevantamiento " +
                                "WHERE AL.Padre = @IdGeografia AND AL.Nivel = 3 AND AL.IdAreaLevantamiento <> 0 " +
                                    "AND A.IdAsignacionDe = @IdUsuario AND A.IdTipoAsignacion = 5";
                                    //"AND A.IdAsignacionDe = @IdUsuario AND A.IdTipoAsignacion = @IdTipoAsig";
                    _exQuery.AgregarParametro("IdUsuario", idUsuario);
                    //_exQuery.AgregarParametro("IdTipoAsig", tipoAsig);
                }

                _exQuery.AgregarParametro("IdGeografia", gesGeografiaBOL.Geografia_id.ToString());
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
