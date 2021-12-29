using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.BOL
{
    public class GesGeografiaBOL
    {
        private int _sistema_id;
        private int _geografia_id;
        private string _geografia_codigo;
        private int _geografia_padre;
        private int _geografia_posicion;
        private int _geografia_nivel_id;
        private string _geografia_nombre;

        /// <summary>
        /// Obtiene o establece identificador de la geografia
        /// </summary>
        public int Sistema_id
        {
            get { return _sistema_id; }
            set { _sistema_id = value; }
        }

        /// <summary>
        /// Obtiene o establece identificador de la geografia
        /// </summary>
        public int Geografia_id
        {
            get { return _geografia_id; }
            set { _geografia_id = value; }
        }

        /// <summary>
        /// Obtiene o establece codigo de la geografia
        /// </summary>
        public string Geografia_codigo
        {
            get { return _geografia_codigo; }
            set { _geografia_codigo = value; }
        }

        /// <summary>
        /// Obtiene o establece padre de la geografia
        /// </summary>
        public int Geografia_padre
        {
            get { return _geografia_padre; }
            set { _geografia_padre = value; }
        }

        /// <summary>
        /// Obtiene o establece padre de la geografia
        /// </summary>
        public int Geografia_posicion
        {
            get { return _geografia_posicion; }
            set { _geografia_posicion = value; }
        }

        /// <summary>
        /// Obtiene o establece padre de la geografia
        /// </summary>
        public int Geografia_nivel_id
        {
            get { return _geografia_nivel_id; }
            set { _geografia_nivel_id = value; }
        }

        /// <summary>
        /// Obtiene o establece nombre de la geografia
        /// </summary>
        public string Geografia_nombre
        {
            get { return _geografia_nombre; }
            set { _geografia_nombre = value; }
        }
    }
}
