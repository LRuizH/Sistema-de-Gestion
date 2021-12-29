using System;
using System.Collections.Generic;
using System.Text;
using Framework.BOL;

namespace Framework.DAL
{
    public class GesSistemaDAL
    {
        string _strSql = "";

        /// <summary>
        /// Lista información del sistema
        /// </summary>
        public List<T> Listar<T>(GesSistemaBOL _gesSistemaBOL)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT Sistema_id = s.IdSistema, Sistema_token = s.Token, Sistema_url = s.[Url] , Sistema_sigla = s.Sigla, Sistema_nombre = s.Nombre, Proyecto_id = p.IdProyecto , Proyecto_nombre = p.Nombre " +
                   "FROM dbo.ges_sistema AS s INNER JOIN dbo.ges_proyecto AS p ON p.IdProyecto = s.IdProyecto WHERE s.IdSistema = @IdSistema AND s.Token = @Token";
                _exQuery.AgregarParametro("IdSistema", _gesSistemaBOL.Sistema_id.ToString());
                _exQuery.AgregarParametro("Token", _gesSistemaBOL.Sistema_token);
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
