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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiRest.Controllers
{
    [Route("api/[controller]")]
    public class ReasignacionesController : Controller
    {
        [HttpGet]
        [Route("muestra-reasignacion")]
        public List<ElementoHtml> MuestraReasignacion()
        {
            VReasignaciones _vReasignaciones = new VReasignaciones();

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _vReasignaciones.VSeleccionReasignaciones()
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
        public List<ElementoHtml> MuestraCarga(int sistema_id, int tipo_asig, int areacensal, string usu, int geo, int perfil_id, string sup)
        {
            GesAsignacionesBOL _gesAsignacionesBOL = new GesAsignacionesBOL();
            CReasignaciones _cReasignaciones = new CReasignaciones();

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _cReasignaciones.ObtieneCargaTrabajo(sistema_id, tipo_asig, areacensal, usu, geo, perfil_id, sup)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }

        [HttpGet]
        [Route("muestra-usuario-perfil")]
        public List<ElementoHtml> MuestraUsuarioSegunPerfil(int sistema_id, int areacensal, int perfil)
        {
            GesAsignacionesBOL _gesAsignacionesBOL = new GesAsignacionesBOL();
            CReasignaciones _cReasignaciones = new CReasignaciones();

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _cReasignaciones.GetListaSupervisores(sistema_id, areacensal, perfil)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }

        [HttpGet]
        [Route("muestra-usuario-destino")]
        public List<ElementoHtml> MuestraUsuarioDestino(int sistema_id, int areacensal, int perfil, string usu, string sup)
        {
            GesAsignacionesBOL _gesAsignacionesBOL = new GesAsignacionesBOL();
            CReasignaciones _cReasignaciones = new CReasignaciones();

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _cReasignaciones.GetListaUsuarioDestino(sistema_id, areacensal, perfil, usu, sup)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }

        [HttpGet]
        [Route("muestra-carga-usuario")]
        public List<ElementoHtml> MuestraCargaUsuario(int sistema_id, int tipo_asig, int areacensal, string usu, int geo, int disponibles, int perfil_id, int tipo)
        {
            GesReasignacionesBLL _gesReasignacionesBLL = new GesReasignacionesBLL();
            CReasignaciones _cReasignaciones = new CReasignaciones();

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _gesReasignacionesBLL.ObtieneUsuariosAsignados(sistema_id, tipo_asig, areacensal, usu, geo, disponibles, perfil_id, tipo)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }

        [HttpPost]
        [Route("inserta-reasignaciones")]
        public List<ElementoHtml> IngresoAsignaciones(int tipo_asig, int sistema_id, string sector, string usuarioOrigen, string usuarioDestino)
        {
            GesReasignacionesBLL _gesReasignacionesBLL = new GesReasignacionesBLL();

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _gesReasignacionesBLL.InsertarReAsignaciones(tipo_asig, sistema_id, sector, usuarioOrigen, usuarioDestino)
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
