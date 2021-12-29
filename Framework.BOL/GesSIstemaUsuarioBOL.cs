using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.BOL
{
    public class GesSistemaUsuarioBOL
    {
        private int _sistema_id;
        private string _usu_id;

        /// <summary>
        /// Obtiene o establece identificador del sistema
        /// </summary>
        public int Sistema_id
        {
            get { return _sistema_id; }
            set { _sistema_id = value; }
        }

        /// <summary>
        /// Obtiene o establece identificador del usuario
        /// </summary>
        public string Usu_id
        {
            get { return _usu_id; }
            set { _usu_id = value; }
        }
    }
}
