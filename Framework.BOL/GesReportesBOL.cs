using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.BOL
{
    public class GesReportesBOL
    {
        private string _usu_id;
        private string _llamada;

        /// <summary>
        /// Obtiene o establece identificador del usuario
        /// </summary>
        public string Usu_id
        {
            get { return _usu_id; }
            set { _usu_id = value; }
        }

        /// <summary>
        /// Obtiene o establece procedimiento almacenado
        /// </summary>
        public string LLamada
        {
            get { return _llamada; }
            set { _llamada = value; }
        }
    }
}
