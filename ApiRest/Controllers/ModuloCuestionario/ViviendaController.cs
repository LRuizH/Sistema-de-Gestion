using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.BLL;
using Framework.BLL.Vistas.ModuloCuestionario;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiRest.Controllers.ModuloCuestionario
{
    [Route("api/[controller]")]
    public class ViviendaController : Controller
    {
        /// <summary>
        /// Ingresa datos dirección
        /// </summary>
        [HttpPost]
        [Route("ingresar-datos-direccion")]
        public List<ElementoHtml> IngresoViviendaDireccion(string formData)
        {
            ViviendaBLL _viviendaBLL = new ViviendaBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _viviendaBLL.IngresoViviendaDireccion(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }
        
        /// <summary>
         /// Ingresa datos del tipo de vivienda
         /// </summary>
        [HttpPost]
        [Route("ingresar-datos-informante")]
        public List<ElementoHtml> IngresoViviendaInformante(string formData)
        {
            ViviendaBLL _viviendaBLL = new ViviendaBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _viviendaBLL.IngresoViviendaInformante(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa datos del materialidad de vivienda
        /// </summary>
        [HttpPost]
        [Route("ingresar-materialidad-vivienda")]
        public List<ElementoHtml> IngresoViviendaMaterialidadInfraestructura(string formData)
        {
            ViviendaBLL _viviendaBLL = new ViviendaBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _viviendaBLL.IngresoViviendaMaterialidadInfraestructura(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa datos del servicios basicos de vivienda
        /// </summary>
        [HttpPost]
        [Route("ingresar-servicios-vivienda")]
        public List<ElementoHtml> IngresoViviendaServiciosBasicos(string formData)
        {
            ViviendaBLL _viviendaBLL = new ViviendaBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _viviendaBLL.IngresoViviendaServiciosBasicos(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa datos de identificación de hogares
        /// </summary>
        [HttpPost]
        [Route("ingresar-identificacion-hogar")]
        public List<ElementoHtml> IngresoViviendaIdentificacionHogares(string formData)
        {
            ViviendaBLL _viviendaBLL = new ViviendaBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _viviendaBLL.IngresoViviendaIdentificacionHogares(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }
    }
}
