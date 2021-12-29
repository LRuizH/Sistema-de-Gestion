using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.BOL
{
    public class GesFormPreguntasOpcionesBOL
    {
        private int _pk_form_preguntas;
        private string _gpf_codigo_pregunta;
        private int _fpo_numero;
        private string _fpo_glosa_primaria;

        /// <summary>
        /// Obtiene o establece identificador de pregunta
        /// </summary>
        public int Pk_form_preguntas
        {
            get { return _pk_form_preguntas; }
            set { _pk_form_preguntas = value; }
        }

        /// <summary>
        /// Obtiene o establece glosa de la opción de pregunta
        /// </summary>
        public string Gpf_codigo_pregunta
        {
            get { return _gpf_codigo_pregunta; }
            set { _gpf_codigo_pregunta = value; }
        }

        /// <summary>
        /// Obtiene o establece valor de la opción de pregunta
        /// </summary>
        public int Fpo_numero
        {
            get { return _fpo_numero; }
            set { _fpo_numero = value; }
        }

        /// <summary>
        /// Obtiene o establece glosa de la opción de pregunta
        /// </summary>
        public string Fpo_glosa_primaria
        {
            get { return _fpo_glosa_primaria; }
            set { _fpo_glosa_primaria = value; }
        }
    }
}
