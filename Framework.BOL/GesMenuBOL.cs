using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.BOL
{
    public class GesMenuBOL
    {
        private int _sistema_id; 
        private int _menu_id;
        private int _perfil_id;
        private string _menu_titulo;
        private string _menu_icono;
        private int _menu_padre;
        private int _menu_nivel;
        private string _menu_accion;
        private int _menu_posicion;
        private bool _menu_activo;
        private bool _menu_seleccionado;

        /// <summary>
        /// Obtiene o establece identificador del sistema
        /// </summary>
        public int Sistema_id
        {
            get { return _sistema_id; }
            set { _sistema_id = value; }
        }

        /// <summary>
        /// Obtiene o establece identificador del menu
        /// </summary>
        public int Menu_id
        {
            get { return _menu_id; }
            set { _menu_id = value; }
        }

        /// <summary>
        /// Obtiene o establece identificador del perfil
        /// </summary>
        public int Perfil_id
        {
            get { return _perfil_id; }
            set { _perfil_id = value; }
        }

        /// <summary>
        /// Obtiene o establece titulo del menu
        /// </summary>
        public string Menu_titulo
        {
            get { return _menu_titulo; }
            set { _menu_titulo = value; }
        }

        /// <summary>
        /// Obtiene o establece icono del menu
        /// </summary>
        public string Menu_icono
        {
            get { return _menu_icono; }
            set { _menu_icono = value; }
        }

        /// <summary>
        /// Obtiene o establece nivel padre en menu
        /// </summary>
        public int Menu_padre
        {
            get { return _menu_padre; }
            set { _menu_padre = value; }
        }

        /// <summary>
        /// Obtiene o establece nivel dentro del menu
        /// </summary>
        public int Menu_nivel
        {
            get { return _menu_nivel; }
            set { _menu_nivel = value; }
        }

        /// <summary>
        /// Obtiene o establece menu seleccionado
        /// </summary>
        public bool Menu_seleccionado
        {
            get { return _menu_seleccionado; }
            set { _menu_seleccionado = value; }
        }

        /// <summary>
        /// Obtiene o establece accion del menu
        /// </summary>
        public string Menu_accion 
        {
            get { return _menu_accion; }
            set { _menu_accion = value; }
        }

        /// <summary>
        /// Obtiene o establece posicion dentro del menu
        /// </summary>
        public int Menu_posicion
        {
            get { return _menu_posicion; }
            set { _menu_posicion = value; }
        }

        /// <summary>
        /// Obtiene o establece opcion activa en el menu
        /// </summary>
        public bool Menu_activo 
        {
            get { return _menu_activo; }
            set { _menu_activo = value; }
        }
    }
}
