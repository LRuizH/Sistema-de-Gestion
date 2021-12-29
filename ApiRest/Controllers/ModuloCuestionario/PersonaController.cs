using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.BLL;
using Framework.BLL.Componentes.ModuloCuestionario;
using Framework.BLL.Utilidades.Html;
using Framework.BOL;
using Framework.DAL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiRest.Controllers.ModuloCuestionario
{
    [Route("api/[controller]")]
    public class PersonaController : Controller
    {       

        /// <summary>
        /// Ingresa datos del formulario de emigración persona
        /// </summary>
        [HttpPost]
        [Route("ingresar-datos-emigracion")]
        public List<ElementoHtml> IngresoHogarMigracionPersona(string formData)
        {
            PersonaExtBLL _personaExtBLL = new PersonaExtBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _personaExtBLL.IngresoHogarMigracionPersona(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa datos del formulario personas que componen el hogar
        /// </summary>
        [HttpPost]
        [Route("ingresar-datos-componen-hogar")]
        public List<ElementoHtml> IngresoFormularioPersonaxHogar(string formData)
        {
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = "ok";

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }
        
        /// <summary>
        /// Ingresa datos del formulario de persona
        /// </summary>
        [HttpPost]
        [Route("ingresar-datos")]
        public List<ElementoHtml> IngresoFormularioPersona(string formData)
        {
            PersonaBLL _personaBLL = new PersonaBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _personaBLL.IngresoListadoPersonas(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa datos del formulario de persona migracion
        /// </summary>
        [HttpPost]
        [Route("ingresar-datos-migracion")]
        public List<ElementoHtml> IngresoFormularioPersonaMigracion(string formData)
        {
            PersonaBLL _personaBLL = new PersonaBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _personaBLL.IngresoPersonaMigracion(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa datos del formulario de persona nacionalidad
        /// </summary>
        [HttpPost]
        [Route("ingresar-datos-nac")]
        public List<ElementoHtml> IngresoFormularioPersonaNacionalidad(string formData)
        {
            PersonaBLL _personaBLL = new PersonaBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _personaBLL.IngresoPersonaNacionalidad(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa datos del formulario de persona educacion
        /// </summary>
        [HttpPost]
        [Route("ingresar-datos-educacion")]
        public List<ElementoHtml> IngresoFormularioPersonaEducacion(string formData)
        {
            PersonaBLL _personaBLL = new PersonaBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _personaBLL.IngresoPersonaEducacion(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa datos del formulario de persona discapacidad
        /// </summary>
        [HttpPost]
        [Route("ingresar-datos-discapacidad")]
        public List<ElementoHtml> IngresoFormularioPersonaDiscapacidad(string formData)
        {
            PersonaBLL _personaBLL = new PersonaBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _personaBLL.IngresoPersonaDiscapacidad(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa datos del formulario de persona nucleos familiares
        /// </summary>
        [HttpPost]
        [Route("ingresar-datos-nucleos")]
        public List<ElementoHtml> IngresoFormularioPersonaNucleosFamiliares(string formData)
        {
            PersonaBLL _personaBLL = new PersonaBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _personaBLL.IngresoPersonaNucleosFamiliares(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa datos del formulario de persona caracteristicas laborales
        /// </summary>
        [HttpPost]
        [Route("ingresar-datos-laborales")]
        public List<ElementoHtml> IngresoFormularioPersonaLaborales(string formData)
        {
            PersonaBLL _personaBLL = new PersonaBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _personaBLL.IngresoPersonaLaborales(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa datos del formulario de persona religion o credo
        /// </summary>
        [HttpPost]
        [Route("ingresar-datos-religion")]
        public List<ElementoHtml> IngresoFormularioPersonaReligion(string formData)
        {
            PersonaBLL _personaBLL = new PersonaBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _personaBLL.IngresoPersonaReligionCredo(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa datos del formulario de persona fecundidad
        /// </summary>
        [HttpPost]
        [Route("ingresar-datos-fecundidad")]
        public List<ElementoHtml> IngresoFormularioPersonaFecundidad(string formData)
        {
            PersonaBLL _personaBLL = new PersonaBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _personaBLL.IngresoPersonaFecundidad(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Ingresa datos del formulario de persona diversidad
        /// </summary>
        [HttpPost]
        [Route("ingresar-datos-diversidad")]
        public List<ElementoHtml> IngresoFormularioPersonaDiversidad(string formData)
        {
            PersonaBLL _personaBLL = new PersonaBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            _elementoHtml.Elemento_html = _personaBLL.IngresoPersonaDiversidad(formData);

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }        

        /// <summary>
        /// Método que carga select 
        /// </summary>
        [HttpGet]
        [Route("muestra-comuna-segun-region")]
        public List<ElementoHtml> MuestraComunaSegunRegion(int num, int codigo_id, string clase_control, string id_campo)
        {
            GesGeografiaBOL _gesGeografiaBOL = new GesGeografiaBOL();
            CPersona _cPersona = new CPersona();
            _gesGeografiaBOL.Geografia_id = num;

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _cPersona.GetListaComuna(_gesGeografiaBOL, codigo_id, clase_control, id_campo)
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
