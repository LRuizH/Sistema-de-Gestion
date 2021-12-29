using Framework.BLL;
using Framework.BLL.Utilidades.Seguridad;
using Framework.BLL.Vistas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ApiRest.Controllers
{

    [Route("api/[controller]")]
    public class AccesoController : Controller
    {
        AppSettings _appSettings = new AppSettings();

        /// <summary>
        /// Obtiene componente html para vista de acceso de usuarios
        /// </summary>
        [HttpGet]
        public List<ElementoHtml> ObtieneVistaAcceso(int sistema_id, string sistema_token)
        {
            string cookie = _appSettings.ObtieneCookie();
            ElementoHtml _elementoHtml = new ElementoHtml();

            try
            {
                if (cookie == "null")
                {
                    VAcceso _vAcceso = new VAcceso();
                    _elementoHtml.Elemento_html = _vAcceso.ObtieneAcceso(sistema_id, sistema_token, cookie);
                }
                else
                {
                    VMasterPage _vMasterPage = new VMasterPage();
                    _elementoHtml.Elemento_html = _vMasterPage.ObtieneMasterPage(sistema_id, sistema_token, cookie);
                }
            }
            catch
            {
                var Cookie = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(-1),
                    IsEssential = true
                };
                HttpContext.Response.Cookies.Append("CuentaFramework", "null", Cookie);
            }

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Obtiene componente html para vista de acceso de usuarios a cuestionario
        /// </summary>
        [HttpGet]
        [Route("obtiene-acceso-cuestionario")]
        public List<ElementoHtml> ObtieneVistaAccesoCuestionario(int sistema_id, string sistema_token)
        {
            string cookie = _appSettings.ObtieneCookie();
            ElementoHtml _elementoHtml = new ElementoHtml();

            try
            {
                if (cookie == "null")
                {
                    VAcceso _vAcceso = new VAcceso();
                    _elementoHtml.Elemento_html = _vAcceso.ObtieneAccesoPorCuestionario(sistema_id, sistema_token, cookie);
                }
                else
                {
                    VMasterPage _vMasterPage = new VMasterPage();
                    _elementoHtml.Elemento_html = _vMasterPage.ObtieneMasterPage(sistema_id, sistema_token, cookie);
                }
            }
            catch
            {
                var Cookie = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(-1),
                    IsEssential = true
                };
                HttpContext.Response.Cookies.Append("CuentaFramework", "null", Cookie);
            }

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Obtiene componente html para vista de acceso de usuarios a cuestionario  -- cuestionario-telefonico.html
        /// </summary>
        [HttpGet]
        [Route("obtiene-acceso-cuestionario-telefonico")]
        public List<ElementoHtml> ObtieneVistaAccesoTelefonico(int sistema_id, string sistema_token)
        {
            string cookie = _appSettings.ObtieneCookie();
            ElementoHtml _elementoHtml = new ElementoHtml();

            try
            {
                if (cookie == "null")
                {
                    VAcceso _vAcceso = new VAcceso();
                    _elementoHtml.Elemento_html = _vAcceso.ObtieneAccesoTelefonico(sistema_id, sistema_token, cookie);
                }
                else
                {
                    VMasterPage _vMasterPage = new VMasterPage();
                    _elementoHtml.Elemento_html = _vMasterPage.ObtieneMasterPage(sistema_id, sistema_token, cookie);
                }
            }
            catch
            {
                var Cookie = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(-1),
                    IsEssential = true
                };
                HttpContext.Response.Cookies.Append("CuentaFramework", "null", Cookie);
            }

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }
        /// <summary> 
        /// Obtiene componente para detectar el estado de la sesión
        /// </summary>
        [HttpGet]
        [Route("verifica-sesion")]
        public List<ElementoHtml> VerificaEstadoConexionUsuario()
        {
            string cookie = _appSettings.ObtieneCookie();
            GesUsuarioBLL _gesUsuarioBLL = new GesUsuarioBLL();

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _gesUsuarioBLL.ObtieneUsuarioConectado(cookie).Usu_id
            };

            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }
    }
}
