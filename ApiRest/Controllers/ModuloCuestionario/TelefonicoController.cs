using Framework.BLL;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace ApiRest.Controllers.ModuloCuestionario
{
    [Route("api/[controller]")]
    public class TelefonicoController: Controller
    {

        /// <summary>
        /// Ingresa datos del informante
        /// </summary>
        [HttpPost]
        [Route("ingresar-datos-informante")]
        public List<ElementoHtml> IngresoDatosInformante(string formData)
        {
            TelefonicoBLL _telefonicoBLL = new TelefonicoBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _telefonicoBLL.IngresoDatosInformante(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa motivo llamada del informante
        /// </summary>
        [HttpPost]
        [Route("ingresar-motivo-llamada")]
        public List<ElementoHtml> IngresoMotivoLLamada(string formData)
        {
            TelefonicoBLL _telefonicoBLL = new TelefonicoBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _telefonicoBLL.IngresoMotivoLLamada(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa motivo llamada del informante
        /// </summary>
        [HttpPost]
        [Route("ingresar-categoria-motivo-llamada")]
        public List<ElementoHtml> IngresoCategoriaMotivoLLamada(string formData)
        {
            TelefonicoBLL _telefonicoBLL = new TelefonicoBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _telefonicoBLL.IngresoCategoriaMotivoLLamada1(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa cierre gestión llamada del informante
        /// </summary>
        [HttpPost]
        [Route("ingresar-cierre-gestion")]
        public List<ElementoHtml> IngresoCierreGestionLLamada(string formData)
        {
            TelefonicoBLL _telefonicoBLL = new TelefonicoBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _telefonicoBLL.IngresoCierreGestionLLamada(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa validación de código de llamada del informante
        /// </summary>
        [HttpPost]
        [Route("ingresar-validacion-codigo")]
        public List<ElementoHtml> IngresoValidadionCodigo(string formData)
        {
            TelefonicoBLL _telefonicoBLL = new TelefonicoBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _telefonicoBLL.IngresoValidadionCodigo(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa direccion del informante para validar si es correcta
        /// </summary>
        [HttpPost]
        [Route("ingresar-validacion-direccion")]
        public List<ElementoHtml> IngresoValidadionDireccion(string formData)
        {
            TelefonicoBLL _telefonicoBLL = new TelefonicoBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _telefonicoBLL.IngresoValidadionDireccion(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa hoja de ruta
        /// </summary>
        [HttpPost]
        [Route("ingresar-hoja-ruta-cati")]
        public List<ElementoHtml> IngresoHojaRutaCati(string formData)
        {
            TelefonicoBLL _telefonicoBLL = new TelefonicoBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _telefonicoBLL.IngresoHojaRutaCati(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }


        /// <summary>
        /// Ingresa cierre cuestionario completos de la hoja de ruta
        /// </summary>
        [HttpPost]
        [Route("ingresar-cierre_cuestionario-completos-cati")]
        public List<ElementoHtml> IngresoCierreCuestionarioCati(string formData)
        {
            TelefonicoBLL _telefonicoBLL = new TelefonicoBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _telefonicoBLL.IngresoCierreCuestionarioCati(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa resultado de recontacto de la hoja de ruta
        /// </summary>
        [HttpPost]
        [Route("ingresar-resultado-recontacto-cati")]
        public List<ElementoHtml> IngresoResultadoRecontactoCati(string formData)
        {
            TelefonicoBLL _telefonicoBLL = new TelefonicoBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _telefonicoBLL.IngresoResultadoRecontactoCati(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

    }
}
