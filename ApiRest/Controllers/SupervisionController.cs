using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Framework.DAL;
using Framework.BLL;
using Framework.BLL.Componentes.ModuloGestion;
using Framework.BLL.Vistas;
using Framework.BOL;
using Microsoft.AspNetCore.Mvc;
using Framework.BLL.Utilidades.Html;

namespace ApiRest.Controllers
{
    [Route("api/[controller]")]
    public class SupervisionController : Controller
    {
        
        [HttpGet]
        [Route("muestra-supervision")]
        public List<ElementoHtml> MuestraSupervision()
        {
            VSupervision _vSupervision = new VSupervision();

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _vSupervision.VSeleccionSupervision()
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }

        /// <summary>
        /// Método que carga select con tipos de levantamiento según supervision
        /// </summary>
        [HttpGet]
        [Route("muestra-tipo-lev")]
        public List<ElementoHtml> MuestraTipoLev(string tipo_sup)
        {
            GesGeografiaBOL _gesGeografiaBOL = new GesGeografiaBOL();
            CSupervision _cSupervision = new CSupervision();


            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _cSupervision.GetListaTipoLevantamiento(tipo_sup)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }

        [HttpGet]
        [Route("muestra-movil")]
        public List<ElementoHtml> MuestraMovil(int alc, int tipo, int lev)
        {
            GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL();
            CSupervision _cSupervision = new CSupervision();

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _cSupervision.ObtieneTablaListadoMovil(alc, tipo, lev)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>();
            lista.Add(_elementoHtml);
            return lista;
        }

        [HttpGet]
        [Route("muestra-movil-formularios")]
        public List<ElementoHtml> MuestraMovilFormularios(string guid, int tipo, string id_supervisor, string id_censista, string tipo_lev)
        {
            GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL();
            CSupervision _cSupervision = new CSupervision();

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _cSupervision.ObtieneFormulariosSegunEstado(guid, tipo, id_supervisor, id_censista, tipo_lev)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>();
            lista.Add(_elementoHtml);
            return lista;
        }

        [HttpPost] 
        [Route("insertar-supervision-directa")]
        public List<ElementoHtml> IngresoSupervisionDirecta(string formData, string parametro)
        {
            GesSupervisionBLL _gesSupervisionBLL = new GesSupervisionBLL();
            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _gesSupervisionBLL.InsertarSupervisionDirecta(formData)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }

        [HttpPost] 
        [Route("insertar-supervision-directa-tel")]
        public List<ElementoHtml> IngresoSupervisionDirectaTel(string formData, string parametro)
        {
            GesSupervisionBLL _gesSupervisionBLL = new GesSupervisionBLL();
            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _gesSupervisionBLL.InsertarSupervisionDirecta(formData)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }

        [HttpPost]
        [Route("insertar-supervision-indirecta-movil")]
        public List<ElementoHtml> IngresoSupervisionIndirectaMovil(string formData, string parametro)
        {
            GesSupervisionBLL _gesSupervisionBLL = new GesSupervisionBLL();
            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _gesSupervisionBLL.InsertarSupervisionIndirectaMovil(formData)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }

        [HttpPost]
        [Route("insertar-supervision-indirecta-web")]
        public List<ElementoHtml> IngresoSupervisionIndirectaWeb(string formData, string parametro)
        {
            GesSupervisionBLL _gesSupervisionBLL = new GesSupervisionBLL();
            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _gesSupervisionBLL.InsertarSupervisionIndirectaWeb(formData)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }


        [HttpPost]
        [Route("insertar-supervision-indirecta-tel")]
        public List<ElementoHtml> IngresoSupervisionIndirectaTel(string formData, string parametro)
        {
            GesSupervisionBLL _gesSupervisionBLL = new GesSupervisionBLL();
            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _gesSupervisionBLL.InsertarSupervisionIndirectaTel(formData)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }

        [HttpPost]
        [Route("insertar-supervision-esp-ocupacion")]
        public List<ElementoHtml> IngresoSupervisionEspOcupacion(string formData, string parametro)
        {
            GesSupervisionBLL _gesSupervisionBLL = new GesSupervisionBLL();
            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _gesSupervisionBLL.InsertarSupervisionEspOcupacion(formData)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }

        [HttpPost]
        [Route("insertar-supervision-esp-tipoviv")]
        public List<ElementoHtml> IngresoSupervisionEspTipoVivienda(string formData, string parametro)
        {
            GesSupervisionBLL _gesSupervisionBLL = new GesSupervisionBLL();
            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _gesSupervisionBLL.InsertarSupervisionEspTipoVivienda(formData)
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