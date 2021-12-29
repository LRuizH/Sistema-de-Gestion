using Framework.BOL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Framework.DAL
{
    public class GesReasignacionesDAL
    {
        string _strSql = "";

        /// <summary>
        /// Permite listar usuarios segun perfil
        /// </summary>
        public List<T> ObtieneUsuariosPerfil<T>(int sistema_id, int areacensal, int perfil)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT convert(VARCHAR, usu.IdUsuario) as codigo, usu.nombre as valor from dbo.ges_usuario AS usu " +
                          "inner join ges_atributo_usuario as atr " +
                          "on usu.IdUsuario = atr.IdUsuario " +
                          "where atr.IdSistema = @sistema_id and " +
                          "usu.IdUsuario in (select IdAsignacionDE from dbo.ges_asignaciones where IdSistema = @sistema_id and IdTipoAsignacion = 5 and IdAsignacionA = @areacensal) and " +
                          "atr.IdPerfil = @perfil ";
                _exQuery.AgregarParametro("sistema_id", sistema_id);
                _exQuery.AgregarParametro("areacensal", areacensal);
                _exQuery.AgregarParametro("perfil", perfil);
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Permite listar usuarios segun perfil
        /// </summary>
        public List<T> ObtieneUsuariosOrigen<T>(int sistema_id, int areacensal, int perfil, string sup)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT convert(VARCHAR, usu.IdUsuario) as codigo, usu.nombre as valor from dbo.ges_usuario AS usu " +
                          "inner join ges_atributo_usuario as atr " +
                          "on usu.IdUsuario = atr.IdUsuario " +
                          "where atr.IdSistema = @sistema_id and " +
                          "usu.IdUsuario in (select IdAsignacionDE from dbo.ges_asignaciones where IdSistema = @sistema_id and IdTipoAsignacion = 5 and IdAsignacionA = @areacensal) and " +
                          "usu.IdUsuario in (select IdAsignacionDE from dbo.ges_asignaciones where IdSistema = @sistema_id and IdTipoAsignacion = 10 and IdAsignacionA = @sup) and " +
                          "atr.IdPerfil = @perfil ";
                _exQuery.AgregarParametro("sistema_id", sistema_id);
                _exQuery.AgregarParametro("areacensal", areacensal);
                _exQuery.AgregarParametro("sup", sup);
                _exQuery.AgregarParametro("perfil", perfil);
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Permite listar usuarios sin asignar
        /// </summary>
        public List<T> ObtieneUsuariosDestino<T>(int sistema_id, int areacensal, int perfil, string rut, string sup)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "IF(@rut = '0') BEGIN SELECT '0' as codigo, 'Seleccione' as valor " +
                          "END " +
                          "ELSE BEGIN " +
                              "SELECT convert(VARCHAR, usu.IdUsuario) as codigo, usu.nombre as valor from dbo.ges_usuario AS usu " +
                              "inner join ges_atributo_usuario as atr " +
                              "on usu.IdUsuario = atr.IdUsuario " +
                              "where atr.IdSistema = @sistema_id and " +
                              "usu.IdUsuario in (select IdAsignacionDE from dbo.ges_asignaciones where IdSistema = @sistema_id and IdTipoAsignacion = 5 and IdAsignacionA = @areacensal) and " +
                              "usu.IdUsuario in (select IdAsignacionDE from dbo.ges_asignaciones where IdSistema = @sistema_id and IdTipoAsignacion = 10 and IdAsignacionA = @sup) and " +
                              "usu.IdUsuario not in (@rut) and " +
                              "atr.IdPerfil = @perfil " +
                          "END ";
                _exQuery.AgregarParametro("sistema_id", sistema_id);
                _exQuery.AgregarParametro("areacensal", areacensal);
                _exQuery.AgregarParametro("sup", sup);
                _exQuery.AgregarParametro("perfil", perfil);
                _exQuery.AgregarParametro("rut", rut);
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// inserta reasignaciones
        /// </summary>
        public DataSet InsertarReAsignaciones(int Tipo_asignacion_id, int Sistema_id, string sector, string usuarioOrigen, string usuarioDestino)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure("Pa_ges_reasignacion_insertar");
                _exQuery.AgregarParametro("tipo_asignacion_id", Tipo_asignacion_id);
                _exQuery.AgregarParametro("sistema_id", Sistema_id);
                _exQuery.AgregarParametro("sector", sector);
                _exQuery.AgregarParametro("usuarioOrigen", usuarioOrigen);
                _exQuery.AgregarParametro("usuarioDestino", usuarioDestino);
                return _exQuery.EjecutarProcedimiento();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Lista Asignacion ya realizadas en SuSo
        /// </summary>
        public DataSet ListaAsignaciones(int alc)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT A.Rut, A.IdAsignacionSuso,A.ALC " +
                          "FROM dbo.GES_ASIGNACION_VIVNUEVA A " +
                          "WHERE A.ALC = @Alc";
                _exQuery.AgregarParametro("Alc", alc);
                // _exQuery.AgregarParametro("Rut", rutResponsable);
                return _exQuery.EjecutarConsulta(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
