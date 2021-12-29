using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.BLL;
using Framework.BLL.Componentes.ModuloCuestionario;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiRest.Controllers.ModuloCuestionario
{
    [Route("api/[controller]")]
    public class HogarController : Controller
    {
        /// <summary>
        /// Obtiene eliminación de hogar
        /// </summary>
        [HttpGet]
        [Route("obtiene-eliminacion-hogar")]
        public List<ElementoHtml> ObtieneEliminacionHogar(string token)
        {
            CHogar _cHogar = new CHogar();
            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _cHogar.ObtieneEliminacionHogar(token)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Elimina el Hogar
        /// </summary>
        [HttpPost]
        [Route("eliminar-hogar")]
        public List<ElementoHtml> EliminarHogar(string token)
        {
            HogarBLL _hogarBLL = new HogarBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _hogarBLL.EliminarHogar(token);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa datos de Tenencia del Hogar
        /// </summary>
        [HttpPost]
        [Route("ingresar-hogar-tenencia")]
        public List<ElementoHtml> IngresoHogarTenencia(string formData)
        {
            HogarBLL _hogarBLL = new HogarBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _hogarBLL.IngresoHogarTenencia(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa datos de Fuente de energía del hogar
        /// </summary>
        [HttpPost]
        [Route("ingresar-hogar-fuente-energia")]
        public List<ElementoHtml> IngresoHogarFuenteEnergia(string formData)
        {
            HogarBLL _hogarBLL = new HogarBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _hogarBLL.IngresoHogarFuenteEnergia(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa datos de Gestión de residuos del hogar
        /// </summary>
        [HttpPost]
        [Route("ingresar-hogar-residuos")]
        public List<ElementoHtml> IngresoHogarGestionResiduos(string formData)
        {
            HogarBLL _hogarBLL = new HogarBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _hogarBLL.IngresoHogarGestionResiduos(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa datos de migraciones del hogar
        /// </summary>
        [HttpPost]
        [Route("ingresar-hogar-migraciones")]
        public List<ElementoHtml> IngresoHogarMigraciones(string formData)
        {
            HogarBLL _hogarBLL = new HogarBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _hogarBLL.IngresoHogarMigraciones(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa datos de miembros del hogar
        /// </summary>
        [HttpPost]
        [Route("ingresar-hogar-personas")]
        public List<ElementoHtml> IngresoHogarPersonas(string formData)
        {
            HogarBLL _hogarBLL = new HogarBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _hogarBLL.IngresoHogarPersonas(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Obtiene eliminación de personas
        /// </summary>
        [HttpGet]
        [Route("obtiene-eliminacion-persona")]
        public List<ElementoHtml> ObtieneEliminacionPersona(string token, string persona_id)
        {
            CHogar _cHogar = new CHogar();
            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _cHogar.ObtieneEliminacionPersona(token, persona_id)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Elimina persona
        /// </summary>
        [HttpPost]
        [Route("eliminar-persona")]
        public List<ElementoHtml> EliminarPersona(string token, string persona_id)
        {
            HogarBLL _hogarBLL = new HogarBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _hogarBLL.EliminarPersona(token, persona_id);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Obtiene edición de personas
        /// </summary>
        [HttpGet]
        [Route("obtiene-edicion-persona")]
        public List<ElementoHtml> ObtieneEdicionHogarPersona(string token, string persona_id)
        {
            CHogar _cHogar = new CHogar();
            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _cHogar.ObtieneHogarEdicionRegistroPersonas(token, persona_id)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa datos de miembros del hogar
        /// </summary>
        [HttpPost]
        [Route("editar-hogar-personas")]
        public List<ElementoHtml> EditarHogarPersonas(string formData)
        {
            HogarBLL _hogarBLL = new HogarBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _hogarBLL.EditarHogarPersonas(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa datos de miembros del hogar
        /// </summary>
        [HttpPost]
        [Route("ingresar-hogar-total-personas")]
        public List<ElementoHtml> IngresoHogarTotalPersonas(string formData)
        {
            HogarBLL _hogarBLL = new HogarBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _hogarBLL.IngresoHogarTotalPersonas(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }
    }
}
