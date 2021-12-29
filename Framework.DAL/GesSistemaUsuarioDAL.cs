using System;
using System.Collections.Generic;
using Framework.BOL;

namespace Framework.DAL
{
    public class GesSistemaUsuarioDAL
    {
        string _strSql = "";

        /// <summary>
        /// Lista acceso al sistema por id de sistema y usuario
        /// </summary>
        public List<T> ListarPorUsuario<T>(GesSistemaUsuarioBOL _gesSistemaUsuarioBOL)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT Sistema_id = IdSistema, Usu_id = IdUsuario FROM dbo.ges_sistema_usuario WHERE IdUsuario = @IdUsuario";
                _exQuery.AgregarParametro("IdUsuario", _gesSistemaUsuarioBOL.Usu_id);
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista acceso al sistema por id de sistema y usuario
        /// </summary>
        public List<T> ListarPorSistemaUsuario<T>(GesSistemaUsuarioBOL _gesSistemaUsuarioBOL)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT Sistema_id = IdSistema, Usu_id = IdUsuario FROM dbo.ges_sistema_usuario WHERE IdSistema = @IdSistema AND IdUsuario = @IdUsuario";
                _exQuery.AgregarParametro("IdSistema", _gesSistemaUsuarioBOL.Sistema_id.ToString());
                _exQuery.AgregarParametro("IdUsuario", _gesSistemaUsuarioBOL.Usu_id);
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
