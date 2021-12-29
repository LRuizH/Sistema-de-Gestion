using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.BOL
{
    public class GesSistemaBOL
    {
        private int _sistema_id;
        private string _sistema_token;
        private string _sistema_url;
        private string _sistema_sigla;
        private string _sistema_nombre;
        private string _sistema_logo;
        private string _sistema_descripcion;
        private int _proyecto_id;
        private string _proyecto_nombre;
        private string _proyecto_ayuda_registro;

        /// <summary>
        /// Obtiene o establece identificador del sistema
        /// </summary>
        public int Sistema_id
        {
            get { return _sistema_id; }
            set { _sistema_id = value; }
        }

        /// <summary>
        /// Obtiene o establece token del sistema
        /// </summary>
        public string Sistema_token
        {
            get { return _sistema_token; }
            set { _sistema_token = value; }
        }

        /// <summary>
        /// Obtiene o establece url del sistema
        /// </summary>
        public string Sistema_url
        {
            get { return _sistema_url; }
            set { _sistema_url = value; }
        }

        /// <summary>
        /// Obtiene o establece sigla del sistema
        /// </summary>
        public string Sistema_sigla
        {
            get { return _sistema_sigla; }
            set { _sistema_sigla = value; }
        }

        /// <summary>
        /// Obtiene o establece nombre del sistema
        /// </summary>
        public string Sistema_nombre
        {
            get { return _sistema_nombre; }
            set { _sistema_nombre = value; }
        }

        /// <summary>
        /// Obtiene o establece logo del sistema
        /// </summary>
        public string Sistema_logo
        {
            get { return _sistema_logo; }
            set { _sistema_logo = value; }
        }

        /// <summary>
        /// Obtiene o establece descripción del sistema
        /// </summary>
        public string Sistema_descripcion
        {
            get { return _sistema_descripcion; }
            set { _sistema_descripcion = value; }
        }

        /// <summary>
        /// Obtiene o establece identificador del proyecto
        /// </summary>
        public int Proyecto_id
        {
            get { return _proyecto_id; }
            set { _proyecto_id = value; }
        }

        /// <summary>
        /// Obtiene o establece nombre del proyecto
        /// </summary>
        public string Proyecto_nombre
        {
            get { return _proyecto_nombre; }
            set { _proyecto_nombre = value; }
        }

        /// <summary>
        /// Obtiene o establece descripción de ayuda en el registro como usuario del sistema y el proyecto
        /// </summary>
        public string Proyecto_ayuda_registro
        {
            get { return _proyecto_ayuda_registro; }
            set { _proyecto_ayuda_registro = value; }
        }

        /// <summary>
        /// Constructor de clase GesSistemaBOL
        /// </summary>
        public GesSistemaBOL()
        {
            try
            {
                Sistema_id = 0;
                Sistema_token = "null";
                Sistema_url = "null";
                Sistema_nombre = "null";
                Sistema_logo = "logo.png";
                Proyecto_id = 0;
                Proyecto_ayuda_registro = "";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
