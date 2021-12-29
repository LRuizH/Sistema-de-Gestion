using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Framework.BLL;
using Framework.BLL.Componentes.ModuloGestion;
using Framework.BLL.Vistas;
using Framework.BOL;
using Framework.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;


namespace ApiRest.Controllers
{
    [Route("api/[controller]")]
    public class AsignacionesController : Controller
    {
        /// <summary>
        /// Método que carga pantalla de asignaciones
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("muestra-asignacion")]
        public List<ElementoHtml> MuestraAsignacion()
        {
            VAsignaciones _vAsignaciones = new VAsignaciones();

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _vAsignaciones.VSeleccionAsignaciones()
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }

        /// <summary>
        /// Método que carga pantalla con resumen de conformación de equipos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("muestra-resumen-asignacion")]
        public List<ElementoHtml> MuestraResumenAsignacion()
        {
            VAsignaciones _vAsignaciones = new VAsignaciones();

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _vAsignaciones.VResumenAsignaciones()
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }


        [HttpGet]
        [Route("muestra-carga")]
        public List<ElementoHtml> MuestraCarga(int sistema_id, int tipo_asig, int areacensal, string usu, int geo, int perfil_id, int nivel)
        {
            GesAsignacionesBOL _gesAsignacionesBOL = new GesAsignacionesBOL();
            CAsignaciones _cAsignaciones = new CAsignaciones();

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _cAsignaciones.ObtieneCargaTrabajo(sistema_id, tipo_asig, areacensal, usu, geo, perfil_id, nivel)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }

        [HttpGet]
        [Route("muestra-segun-perfil")]
        public List<ElementoHtml> MuestraSegunPerfil(int sistema_id, int tipo_asig, int areacensal, string usu, int geo, int perfil_id)
        {
            GesAsignacionesBOL _gesAsignacionesBOL = new GesAsignacionesBOL();
            CAsignaciones _cAsignaciones = new CAsignaciones();

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _cAsignaciones.ObtieneTablaPerfil(sistema_id, tipo_asig, areacensal, usu, geo, perfil_id)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }

        /// <summary>
        /// Método que carga lista de censistas por local para carga de trabajo
        /// </summary>
        [HttpGet]
        [Route("muestra-segun-rec-sector")]
        public List<ElementoHtml> MuestraSegunRecSector(int sistema_id, int tipo_asig, int areacensal, string usu, int geo, int perfil_id)
        {
            GesAsignacionesBOL _gesAsignacionesBOL = new GesAsignacionesBOL();
            CAsignaciones _cAsignaciones = new CAsignaciones();

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _cAsignaciones.ObtieneTablaRecSector(sistema_id, tipo_asig, areacensal, usu, geo, perfil_id)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }


        /// <summary>
        /// Método que carga select con macrosectores
        /// </summary>
        [HttpGet]
        [Route("muestra-area-censal")]
        public List<ElementoHtml> MuestraAreaCensal(int num, int tipo_asig, string id_usuario, int perfil_usuario)
        {
            GesGeografiaBOL _gesGeografiaBOL = new GesGeografiaBOL();
            CAsignaciones _cAsignaciones = new CAsignaciones();
            _gesGeografiaBOL.Geografia_id = num;

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _cAsignaciones.GetListaAreaCensal(_gesGeografiaBOL, tipo_asig, id_usuario, perfil_usuario)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }

        /// <summary>
        /// Método que carga select con areas censales
        /// </summary>
        [HttpGet]
        [Route("muestra-tipo-area-asig")]
        public List<ElementoHtml> MuestraTipoAreaAsig(int num, int tipo_asig)
        {
            GesGeografiaBOL _gesGeografiaBOL = new GesGeografiaBOL();
            CAsignaciones _cAsignaciones = new CAsignaciones();
            _gesGeografiaBOL.Geografia_id = num;

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _cAsignaciones.GetListaAreaTipoAsig(_gesGeografiaBOL, tipo_asig)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }

        /// <summary>
        /// Método que carga select con sectores censales
        /// </summary>
        [HttpGet]
        [Route("muestra-sector-censal")]
        public List<ElementoHtml> MuestraSectorCensal(int num, int tipo_asig, string id_usuario, int perfil_usuario)
        {
            GesGeografiaBOL _gesGeografiaBOL = new GesGeografiaBOL();
            CAsignaciones _cAsignaciones = new CAsignaciones();
            _gesGeografiaBOL.Geografia_id = num;

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _cAsignaciones.GetListaSectorCensal(_gesGeografiaBOL, tipo_asig, id_usuario, perfil_usuario)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }

        /// <summary>
        /// Método que carga select con locales censales
        /// </summary>
        [HttpGet]
        [Route("muestra-local-censal")]
        public List<ElementoHtml> MuestraLocalCensal(int num, int tipo_asig, string id_usuario, int perfil_usuario)
        {
            GesGeografiaBOL _gesGeografiaBOL = new GesGeografiaBOL();
            CAsignaciones _cAsignaciones = new CAsignaciones();
            _gesGeografiaBOL.Geografia_id = num;

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _cAsignaciones.GetListaLocalCensal(_gesGeografiaBOL, tipo_asig, id_usuario, perfil_usuario)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }

        

        [HttpGet]
        [Route("muestra-lista-resumen")]
        public List<ElementoHtml> MuestraListaResumenAsig(string usuario, int geo, int perfil_id)
        {
            GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL();
            CAsignaciones _cAsignaciones = new CAsignaciones();

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _cAsignaciones.ObtieneTablaListadoResumenAsig(usuario, geo, perfil_id)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }


        [HttpGet]
        [Route("muestra-lista-resumen-equipos")]
        public List<ElementoHtml> MuestraListaResumenAsigEquipos(string usuario, int geo, int perfil_id, string local_id)
        {
            GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL();
            CAsignaciones _cAsignaciones = new CAsignaciones();
            //CSupervision _cSupervision = new CSupervision();

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _cAsignaciones.ObtieneTablaListadoEquipos(usuario, geo, perfil_id, local_id)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>();
            lista.Add(_elementoHtml);
            return lista;
        }


        [HttpGet]
        [Route("muestra-lista-dir-alc")]
        public List<ElementoHtml> MuestraListaDireccionesALC(string alc_id, string nombre_alc)
        {
            GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL();
            CAsignaciones _cAsignaciones = new CAsignaciones();
            //CSupervision _cSupervision = new CSupervision();

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _cAsignaciones.ObtieneTablaListadoDirecciones(alc_id,nombre_alc)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>();
            lista.Add(_elementoHtml);
            return lista;
        }


        /// <summary>
        /// Método que carga select con sectores censales
        /// </summary>
        [HttpGet]
        [Route("muestra-alc-local")]
        public List<ElementoHtml> MuestraALCLocal(string id_local, string id_usuario, int perfil_usuario)
        {
            GesGeografiaBOL _gesGeografiaBOL = new GesGeografiaBOL();
            CAsignaciones _cAsignaciones = new CAsignaciones();
            

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _cAsignaciones.GetListaALCLocal(id_local, id_usuario, perfil_usuario)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }



        [HttpPost]
        [Route("inserta-asignaciones")]
        public List<ElementoHtml> IngresoAsignaciones(int tipo_asig, int sistema_id, string usu, string areacensal, int nivelasig, int perfilasig)
        {
            GesAsignacionesBLL _gesAsignacionesBLL = new GesAsignacionesBLL();
            GesAsignacionesBOL _gesAsignacionesBOL = new GesAsignacionesBOL();

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _gesAsignacionesBLL.InsertarAsignaciones(tipo_asig, sistema_id, usu, areacensal, nivelasig, perfilasig)
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
