using Framework.BOL;
using Framework.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.BLL
{
    public class GesMenuBLL
    {
        string _strHtml = "";

        /// <summary>
        /// Permite listar el menu segun perfil
        /// </summary>
        public string ObtieneMenuPorPerfil(GesMenuBOL _gesMenuBOL)
        {
            string _script = "";
            try
            {
                GesMenuDAL gesMenuDAL = new GesMenuDAL();
                List<GesMenuBOL> listMenu = gesMenuDAL.ObtieneMenuPorPerfil<GesMenuBOL>(_gesMenuBOL);

                //Obtengo listado Menu Nivel 1
                var listMenuNivelUno = from m in listMenu
                                       where m.Menu_nivel == 1
                                       orderby m.Menu_posicion
                                       select m;

                //Recorro Nivel 1
                foreach (var itemNivelUno in listMenuNivelUno)
                {
                    //Genero script de carga de primer elemento en pantalla
                    if (itemNivelUno.Menu_posicion == 1)
                    {
                        _script = "<script>setTimeout(function() { " + itemNivelUno.Menu_accion + "}, 1000);</script>";
                    }
                    //Obtengo listado Menu Nivel 2
                    var listMenuNivelDos = from m in listMenu
                                           where m.Menu_padre == itemNivelUno.Menu_id
                                           orderby m.Menu_posicion
                                           select m;

                    //AgregarItemMenu(itemNivelUno, 1);

                    if (listMenuNivelDos.Count() > 0)
                    {
                        AgregarItemMenu(itemNivelUno, 1);
                        _strHtml = _strHtml + "<ul class=\"nav nav-second-level\">";

                        //Recorro Nivel 2
                        foreach (var itemNivelDos in listMenuNivelDos)
                        {
                            //AgregarItemMenu(itemNivelDos, 1);
                            //Obtengo listado Menu Nivel 3
                            var listMenuNivelTres = from m in listMenu
                                                    where m.Menu_padre == itemNivelDos.Menu_id
                                                    orderby m.Menu_posicion
                                                    select m;

                            if (listMenuNivelTres.Count() > 0)
                            {
                                AgregarItemMenu(itemNivelDos, 1);
                                _strHtml = _strHtml + "<ul class=\"nav nav-third-level\">";
                                //Recorro Nivel 3
                                foreach (var itemNivelTres in listMenuNivelTres)
                                {
                                    AgregarItemMenu(itemNivelTres, 1);
                                }
                                _strHtml = _strHtml + "</ul></li>";
                            }
                            else
                            {
                                AgregarItemMenu(itemNivelDos, 2);
                            }
                        }
                        _strHtml = _strHtml + "</ul>";
                    }
                    else
                    {
                        AgregarItemMenu(itemNivelUno, 2);
                    }
                    _strHtml = _strHtml + "</li>";
                }
                return _strHtml + _script;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AgregarItemMenu(GesMenuBOL _menuItem, int _tieneHijos)
        {
            switch (_menuItem.Menu_nivel)
            {
                case 1:
                    {
                        string act = "";
                        string acc2 = "";
                        if (_tieneHijos == 1)
                        {
                            if (_menuItem.Menu_seleccionado == true) { act = "'active'"; } else { act = "''"; }
                            acc2 = "<span class=\"fa arrow\"></span>";
                        }
                        else
                        {
                            if (_menuItem.Menu_seleccionado == true) { act = "'active'"; } else { act = "''"; }
                            acc2 = "";
                        }

                        string acc = "";
                        if (_menuItem.Menu_accion == "#") { acc = "#"; } else { acc = _menuItem.Menu_accion; }

                        _strHtml = _strHtml + "<li class=" + act + ">" + "<a href=\"javascript:void(0);\" onclick=\"" + acc + "\">" + "<i class=\"" + _menuItem.Menu_icono + "\"></i>" + "<span class=\"nav-label\">" + _menuItem.Menu_titulo + "</span>" + acc2 + "" + "</a>";
                        break;
                    }
                case 2:
                    {
                        string act = "";
                        string acc2 = "";
                        if (_tieneHijos == 1)
                        {
                            if (_menuItem.Menu_seleccionado == true) { act = "class=\"active\""; } else { act = ""; }
                            acc2 = "<span class=\"fa arrow\"></span>";
                        }
                        else
                        {
                            if (_menuItem.Menu_seleccionado == true) { act = "class=\"active\""; } else { act = ""; }
                            acc2 = "";
                        }
                        string acc = "";
                        if (_menuItem.Menu_accion == "#") { acc = "#"; } else { acc = _menuItem.Menu_accion; }

                        _strHtml = _strHtml + "<li" + act + ">" + "<a href=\"javascript:void(0);\" onclick=\"" + acc + "\"><i class=\"" + _menuItem.Menu_icono + "\"></i>" + _menuItem.Menu_titulo + "<span>" + acc2 + "</span>" + "</a>";
                        if (_tieneHijos != 1) { _strHtml = _strHtml + "</li>"; }
                        break;
                    }
                case 3:
                    {
                        string act = "";
                        if (_menuItem.Menu_seleccionado == true) { act = "class=\"active\""; } else { act = ""; }
                        string acc = "";
                        if (_menuItem.Menu_accion == "#") { acc = "#"; } else { acc = _menuItem.Menu_accion; }
                        _strHtml = _strHtml + "<li" + act + ">" + "<a href=\"javascript:void(0);\" onclick=\"" + acc + "\">" + _menuItem.Menu_titulo + "</a></li>";
                        break;
                    }
            }
        }
    }
}
