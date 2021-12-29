using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.BOL
{
    public class GesAsignacionesBOL
    {
        private int _sistema_id;
        private DateTime _asignaciones_fecha;
        private int _tipo_asignacion_id;
        private string _asignaciones_codigo_de;
        private string _asignaciones_codigo_a;

        private string _usu_nombre;

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
        public DateTime asignaciones_fecha
        {
            get { return _asignaciones_fecha; }
            set { _asignaciones_fecha = value; }
        }

        /// <summary>
        /// Obtiene o establece tipo de asignacion
        /// </summary>
        public int Tipo_asignacion_id
        {
            get { return _tipo_asignacion_id; }
            set { _tipo_asignacion_id = value; }
        }

        /// <summary>
        /// Obtiene o establece codigo de asignaciones
        /// </summary>
        public string asignaciones_codigo_de
        {
            get { return _asignaciones_codigo_de; }
            set { _asignaciones_codigo_de = value; }
        }

        /// <summary>
        /// Obtiene o establece codigo de asignaciones
        /// </summary>
        public string asignaciones_codigo_a
        {
            get { return _asignaciones_codigo_a; }
            set { _asignaciones_codigo_a = value; }
        }

        /// <summary>
        /// Obtiene o establece nombre del usuario
        /// </summary>
        public string Usu_nombre
        {
            get { return _usu_nombre; }
            set { _usu_nombre = value; }
        }
    }
}
