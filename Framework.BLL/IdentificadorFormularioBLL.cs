using System;
using System.Collections.Generic;
using System.Text;
using Framework.BLL.Utilidades.Encriptacion;

namespace Framework.BLL
{
    public class IdentificadorCuestionario
    {
        private string _idVivienda;
        private string _idHogar;
        private string _idPersona;

        /// <summary>
        /// Obtiene o establece identificador de vivienda
        /// </summary>
        public string IdVivienda
        {
            get { return _idVivienda; }
            set { _idVivienda = value; }
        }

        /// <summary>
        /// Obtiene o establece identificador de hogar
        /// </summary>
        public string IdHogar
        {
            get { return _idHogar; }
            set { _idHogar = value; }
        }

        /// <summary>
        /// Obtiene o establece identificador de persona
        /// </summary>
        public string IdPersona
        {
            get { return _idPersona; }
            set { _idPersona = value; }
        }

        /// <summary>
        /// Obtiene identificación de la vivienda, hogar y persona
        /// </summary>
        public IdentificadorCuestionario ObtieneIdentificacionVHP(string token)
        {
            string _strToken = "";
            Encrypt _encrypt = new Encrypt();
            _strToken = _encrypt.DecryptString(token);

            string[] _strArrayToken = _strToken.Split(new[] { "&" }, StringSplitOptions.None);

            IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();

            _identificadorCuestionario.IdVivienda = _strArrayToken[0];
            _identificadorCuestionario.IdHogar = _strArrayToken[1];
            _identificadorCuestionario.IdPersona = _strArrayToken[2];

            return _identificadorCuestionario;
        }
    }
}
