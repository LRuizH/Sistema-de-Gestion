using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.BOL
{
    public class GesUsuarioBOL
    {
        private string _usu_id;
        private string _usu_contrasena;
        private string _usu_nombre;
        private string _usu_email;
        private string _usu_telefono;
        private int _usu_respuesta;
        private string _usu_pagina;
        private string _usu_tipo;
        private string _usu_estado;
        private bool _usu_activo;
        private bool _usu_cambio_contrasena;
        private int _usu_proceso;
        private string _usu_fecha_contratacion;
        public int Tipo_usuario_id { get; set; }
        public string Usu_rut { get; set; }
        public int Usu_perfil { get; set; }
        private string _usu_id_suso { get; set; } //LR 16-08-2021
        private int _Existe_Asignacion { get; set; }//LR 16-08-2021

        /// <summary>
        /// Obtiene o establece identificador del usuario
        /// </summary>
        public string Usu_id
        {
            get { return _usu_id; }
            set { _usu_id = value; }
        }

        /// <summary>
        /// Obtiene o establece contraseña del usuario
        /// </summary>
        public string Usu_contrasena
        {
            get { return _usu_contrasena; }
            set { _usu_contrasena = value; }
        }

        /// <summary>
        /// Obtiene o establece nombre del usuario
        /// </summary>
        public string Usu_nombre
        {
            get { return _usu_nombre; }
            set { _usu_nombre = value; }
        }

        /// <summary>
        /// Obtiene o establece email del usuario
        /// </summary>
        public string Usu_email
        {
            get { return _usu_email; }
            set { _usu_email = value; }
        }

        /// <summary>
        /// Obtiene o establece teléfono del usuario
        /// </summary>
        public string Usu_telefono
        {
            get { return _usu_telefono; }
            set { _usu_telefono = value; }
        }

        /// <summary>
        /// Obtiene o establece respuesta de la busqueda del usuario
        /// </summary>
        public int Usu_respuesta
        {
            get { return _usu_respuesta; }
            set { _usu_respuesta = value; }
        }

        /// <summary>
        /// Obtiene o establece página del usuario
        /// </summary>
        public string Usu_pagina
        {
            get { return _usu_pagina; }
            set { _usu_pagina = value; }
        }

        /// <summary>
        /// Obtiene o establece el tipo de usuario(interno/externo)
        /// </summary>
        public string Usu_tipo
        {
            get { return _usu_tipo; }
            set { _usu_tipo = value; }
        }
        public string Usu_estado
        {
            get { return _usu_estado; }
            set { _usu_estado = value; }
        }

        public bool Usu_activo
        {
            get { return _usu_activo; }
            set { _usu_activo = value; }
        }
        public bool Usu_cambio_contrasena
        {
            get { return _usu_cambio_contrasena; }
            set { _usu_cambio_contrasena = value; }
        }

        public int Usu_proceso
        {
            get { return _usu_proceso; }
            set { _usu_proceso = value; }
        }

        public string Usu_fecha_contratacion
        {
            get { return _usu_fecha_contratacion; }
            set { _usu_fecha_contratacion = value; }
        }

        //LR 16-08-2021
        public string Usu_idSuso
        {
            get { return _usu_id_suso; }
            set { _usu_id_suso = value; }
        }
        //LR 16-08-2021
        public int Existe_asignacion
        {
            get { return _Existe_Asignacion; }
            set { _Existe_Asignacion = value; }
        }
    }
}
