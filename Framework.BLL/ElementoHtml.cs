using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.BLL
{
    public class ElementoHtml
    {
        private string _elemento_html;

        /// <summary>
        /// Propiedad encargada de almacenar html del elemento
        /// </summary>
        public string Elemento_html
        {
            get
            {
                return _elemento_html;
            }
            set
            {
                _elemento_html = value;
            }
        }
    }
}
