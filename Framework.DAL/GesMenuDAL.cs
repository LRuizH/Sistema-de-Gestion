using Framework.BOL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.DAL
{
    public class GesMenuDAL
    {

        string _strSql = "";

        /// <summary>
        /// Permite listar datos del menu segun perfil
        /// </summary>
        public List<T> ObtieneMenuPorPerfil<T>(GesMenuBOL _gesMenuBOL)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT Menu_id = gm.IdMenu, Menu_titulo = gm.Titulo, Menu_icono = gm.Icono, Menu_padre = gm.Padre, Menu_nivel = gm.Nivel, Menu_accion = gm.Accion, " +
                    " Menu_activo = gm.Activo, Menu_posicion = gm.Posicion FROM dbo.ges_menu AS gm " +
                    "INNER JOIN dbo.ges_privilegio AS gp ON gp.IdMenu = gm.IdMenu AND gp.IdSistema = gm.IdSistema WHERE gp.IdSistema = @IdSistema AND gp.IdPerfil = @IdPerfil AND gm.activo = 1";
                _exQuery.AgregarParametro("IdSistema", _gesMenuBOL.Sistema_id.ToString());
                _exQuery.AgregarParametro("IdPerfil", _gesMenuBOL.Perfil_id.ToString());
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
