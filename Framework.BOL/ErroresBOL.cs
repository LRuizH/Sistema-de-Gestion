using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.BOL
{
    public class ErroresBOL
    {
        private int _error_id;
        private int _error_tipo;
        private string _error_mensage;
        private int _error_seccion;

        /// <summary>
        /// Obtiene o establece Error_id
        /// </summary>
        public int Error_id
        {
            get { return _error_id; }
            set { _error_id = value; }
        }

        /// <summary>
        /// Obtiene o establece Error_tipo
        /// </summary>
        public int Error_tipo
        {
            get { return _error_tipo; }
            set { _error_tipo = value; }
        }

        /// <summary>
        /// Obtiene o establece Error_mensaje
        /// </summary>
        public string Error_mensaje
        {
            get { return _error_mensage; }
            set { _error_mensage = value; }
        }

        /// <summary>
        /// Obtiene o establece Error_seccion
        /// </summary>
        public int Error_seccion
        {
            get { return _error_seccion; }
            set { _error_seccion = value; }
        }
    }
}
