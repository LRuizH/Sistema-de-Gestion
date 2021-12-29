using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.BLL;
using Framework.BLL.Utilidades.Encriptacion;
using Framework.BLL.Utilidades.Seguridad;
using Framework.BLL.Vistas.ModuloCuestionario;
using Framework.BLL.Vistas.ModuloTelefonico;
using Framework.BOL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiRest.Controllers.ModuloCuestionario
{
    [Route("api/[controller]")]
    public class CuestionarioController : Controller
    {
        AppSettings _appSettings = new AppSettings();

        /// <summary>
        /// Método que carga pantalla de Cuestionario
        /// </summary>
        [HttpGet]
        [Route("muestra-cuestionario")]
        public List<ElementoHtml> MuestraCuestionarioUsuario(int paso, string token)
        {
            VCuestionario _vCuestionario = new VCuestionario();
            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _vCuestionario.ObtieneCuestionarioPorSeccion(paso, token)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }
        /// <summary>
        /// Método que genera Cookie del Identificador de la llamada
        /// </summary>
        [HttpGet]
        [Route("genera-cookie-llamada-cati")]
        public List<ElementoHtml> GeneraCookieLlamadaCati(string idLlamada)
        {
            // Genero Cookie con Id de la Llamada
            Encrypt _encrypt = new Encrypt();
            var Cookie = new CookieOptions();
            Cookie.Expires = DateTime.Now.AddDays(1);
            Cookie.IsEssential = true;
            Cookie.HttpOnly = true;
            HttpContext.Response.Cookies.Append("IdLlamadaCATI", idLlamada, Cookie);

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = "ok"
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Método que carga pantalla de Cuestionario
        /// </summary>
        [HttpGet]
        [Route("muestra-cuestionario-cati")]
        public List<ElementoHtml> MuestraCuestionarioUsuarioCati(int paso, string token)
        {
            VCuestionario _vCuestionario = new VCuestionario();
            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _vCuestionario.ObtieneCuestionarioPorSeccion(paso, token, "cati")
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Método que carga pantalla de Cuestionario telefonico -- VMasterPage.cs
        /// </summary>
        [HttpGet]
        [Route("muestra-cuestionario-telefonico")]
        public List<ElementoHtml> MuestraCuestionarioTelefonico(int paso, string token)
        {
            if (paso == 0)
            {
                var Cookie = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(-1),
                    IsEssential = true
                };
                HttpContext.Response.Cookies.Append("IdLlamadaCATI", "null", Cookie);
            }

            VTelefonico _vTelefonica = new VTelefonico();
            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _vTelefonica.ObtienePreguntasTelefonicas(paso, token)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Método que carga pantalla de Hoja de Ruta
        /// </summary>
        [HttpGet]
        [Route("muestra-hoja-ruta-cati")]
        public List<ElementoHtml> MuestraHojaRutaCati(int paso, string token)
        {
            VTelefonico _vTelefonica = new VTelefonico();
            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _vTelefonica.ObtienePreguntasHojaRuta(paso, token)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }
    }
}
