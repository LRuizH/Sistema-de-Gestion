using Framework.BLL;
using Framework.BLL.Componentes;
using Framework.BLL.Utilidades.Ajax;
using Framework.BLL.Utilidades.Email;
using Framework.BLL.Utilidades.Encriptacion;
using Framework.BLL.Utilidades.Seguridad;
using Framework.BLL.Vistas;
using Framework.BOL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ApiRest.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        /// <summary>
        /// Autentica usuario
        /// </summary>
        [HttpPost]
        [Route("autenticar")]
        public List<GesUsuarioBOL> Autenticacion(int sistema_id, string sistema_token, string usu_id, string usu_contrasena, bool recuerda_contrasena)
        {
            string _strRespuesta = "";
            GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL
            {
                Usu_id = usu_id,
                Usu_contrasena = usu_contrasena
            };

            GesUsuarioBLL _gesUsuarioBLL = new GesUsuarioBLL();
            VMasterPage vMasterPage = new VMasterPage();
            _gesUsuarioBOL = _gesUsuarioBLL.AutenticaUsuario(_gesUsuarioBOL, recuerda_contrasena);

            // Usuario interno sin acceso
            if (_gesUsuarioBOL.Usu_respuesta == 1)
            {
                _gesUsuarioBOL.Usu_pagina = "interno";
            }

            // Con Acceso
            if (_gesUsuarioBOL.Usu_respuesta == 2)
            {
                Encrypt _encrypt = new Encrypt();
                var Cookie = new CookieOptions();

                if (recuerda_contrasena == true)
                {
                    Cookie.Expires = DateTime.Now.AddDays(1);
                    Cookie.IsEssential = true;
                    Cookie.HttpOnly = true;
                    HttpContext.Response.Cookies.Append("CuentaFramework", _encrypt.EncryptString(_gesUsuarioBOL.Usu_id), Cookie);
                }
                else
                {
                    Cookie.Expires = DateTime.Now.AddHours(1);
                    Cookie.IsEssential = true;
                    Cookie.HttpOnly = true;
                    HttpContext.Response.Cookies.Append("CuentaFramework", _encrypt.EncryptString(_gesUsuarioBOL.Usu_id), Cookie);
                }

                _gesUsuarioBOL.Usu_contrasena = "null";
                _gesUsuarioBOL.Usu_pagina = vMasterPage.ObtieneMasterPage(sistema_id, sistema_token, _encrypt.EncryptString(_gesUsuarioBOL.Usu_id));

                // Registro Log
                Logs _logs = new Logs
                {
                    Categoria_id = 4,
                    Catalogo_id = 20,
                    Proyecto_id = 1,
                    Cubo_id = 1,
                    Logs_columnas = "'" + _gesUsuarioBOL.Usu_id + "','" + HttpContext.Connection.RemoteIpAddress + "','true','" + DateTime.Now + "'"
                };
                _strRespuesta = _logs.IngresaLog();
            }
            else
            {
                // Registro Log
                Logs _logs = new Logs
                {
                    Categoria_id = 4,
                    Catalogo_id = 20,
                    Proyecto_id = 1,
                    Cubo_id = 1,
                    Logs_columnas = "'" + _gesUsuarioBOL.Usu_id + "','" + HttpContext.Connection.RemoteIpAddress + "','false','" + DateTime.Now + "'"
                };
                _strRespuesta = _logs.IngresaLog();
            }

            List<GesUsuarioBOL> lista = new List<GesUsuarioBOL>
            {
                _gesUsuarioBOL
            };
            return lista;
        }

        /// <summary>
        /// Autentica usuario para ingreso directo de cuestionario
        /// </summary>
        [HttpPost]
        [Route("autenticar-por-cuestionario")]
        public List<GesUsuarioBOL> AutenticacionPorCuestionario(int sistema_id, string sistema_token, string usu_id)
        {
            string _strRespuesta = "";
            GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL
            {
                Usu_id = usu_id,
                Usu_nombre = "Vivienda: " + usu_id
            };

            GesUsuarioBLL _gesUsuarioBLL = new GesUsuarioBLL(); 
            VMasterPage vMasterPage = new VMasterPage();
            _gesUsuarioBOL = _gesUsuarioBLL.AutenticaUsuarioPorCuestionario(_gesUsuarioBOL, sistema_id);

            // Con Acceso
            if (_gesUsuarioBOL.Usu_respuesta == 1)
            {
                Encrypt _encrypt = new Encrypt();
                var Cookie = new CookieOptions();

                Cookie.Expires = DateTime.Now.AddDays(7);
                Cookie.IsEssential = true;
                Cookie.HttpOnly = true;
                HttpContext.Response.Cookies.Append("CuentaFramework", _encrypt.EncryptString(_gesUsuarioBOL.Usu_id), Cookie);

                _gesUsuarioBOL.Usu_contrasena = "null";
                _gesUsuarioBOL.Usu_pagina = vMasterPage.ObtieneMasterPage(sistema_id, sistema_token, _encrypt.EncryptString(_gesUsuarioBOL.Usu_id));

                // Registro Log
                Logs _logs = new Logs
                {
                    Categoria_id = 4,
                    Catalogo_id = 20,
                    Proyecto_id = 1,
                    Cubo_id = 1,
                    Logs_columnas = "'" + _gesUsuarioBOL.Usu_id + "','" + HttpContext.Connection.RemoteIpAddress + "','true','" + DateTime.Now + "'"
                };
                _strRespuesta = _logs.IngresaLog();
            }
            else
            {
                // Registro Log
                Logs _logs = new Logs
                {
                    Categoria_id = 4,
                    Catalogo_id = 20,
                    Proyecto_id = 1,
                    Cubo_id = 1,
                    Logs_columnas = "'" + _gesUsuarioBOL.Usu_id + "','" + HttpContext.Connection.RemoteIpAddress + "','false','" + DateTime.Now + "'"
                };
                _strRespuesta = _logs.IngresaLog();
            }

            List<GesUsuarioBOL> lista = new List<GesUsuarioBOL>
            {
                _gesUsuarioBOL
            };
            return lista;
        }

        /// <summary>
        /// Cierra sesión del usuario
        /// </summary>
        [HttpGet]
        [Route("cerrar-sesion")]
        public List<ElementoHtml> CerrarSesion(int sistema_id, string sistema_token)
        {
            var Cookie = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1),
                IsEssential = true
            };
            HttpContext.Response.Cookies.Append("CuentaFramework", "null", Cookie);
            HttpContext.Response.Cookies.Append("IdLlamadaCATI", "null", Cookie);

            // Retorno objeto
            VAcceso _vAcceso = new VAcceso();
            ElementoHtml _elementoHtml = new ElementoHtml();
            if (sistema_id == 1)
            {
                _elementoHtml.Elemento_html = _vAcceso.ObtieneAcceso(sistema_id, sistema_token, "null");
            }
            else
            {
                _elementoHtml.Elemento_html = _vAcceso.ObtieneAccesoPorCuestionario(sistema_id, sistema_token, "null");
            }
   
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Autentica usuario para ingreso directo de cuestionario
        /// </summary>
        [HttpPost]
        [Route("autenticar-por-id-telefonico")]
        public List<GesUsuarioBOL> AutenticacionPorIdTelefonico(int sistema_id, string sistema_token, string usu_id)
        {
            string _strRespuesta = "";
            GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL
            {
                Usu_id = usu_id,
                Usu_nombre = "Vivienda: " + usu_id
            };

            GesUsuarioBLL _gesUsuarioBLL = new GesUsuarioBLL();
            VMasterPage vMasterPage = new VMasterPage();
            _gesUsuarioBOL = _gesUsuarioBLL.AutenticaUsuarioTelefonico(_gesUsuarioBOL, sistema_id);

            // Con Acceso
            if (_gesUsuarioBOL.Usu_respuesta == 1)
            {
                Encrypt _encrypt = new Encrypt();
                var Cookie = new CookieOptions();

                Cookie.Expires = DateTime.Now.AddDays(7);
                Cookie.IsEssential = true;
                Cookie.HttpOnly = true;
                HttpContext.Response.Cookies.Append("CuentaFramework", _encrypt.EncryptString(_gesUsuarioBOL.Usu_id), Cookie);

                _gesUsuarioBOL.Usu_contrasena = "null";
                _gesUsuarioBOL.Usu_pagina = vMasterPage.ObtieneMasterPage(sistema_id, sistema_token, _encrypt.EncryptString(_gesUsuarioBOL.Usu_id));

                // Registro Log
                Logs _logs = new Logs
                {
                    Categoria_id = 4,
                    Catalogo_id = 20,
                    Proyecto_id = 1,
                    Cubo_id = 1,
                    Logs_columnas = "'" + _gesUsuarioBOL.Usu_id + "','" + HttpContext.Connection.RemoteIpAddress + "','true','" + DateTime.Now + "'"
                };
                _strRespuesta = _logs.IngresaLog();
            }
            else
            {
                // Registro Log
                Logs _logs = new Logs
                {
                    Categoria_id = 4,
                    Catalogo_id = 20,
                    Proyecto_id = 1,
                    Cubo_id = 1,
                    Logs_columnas = "'" + _gesUsuarioBOL.Usu_id + "','" + HttpContext.Connection.RemoteIpAddress + "','false','" + DateTime.Now + "'"
                };
                _strRespuesta = _logs.IngresaLog();
            }

            List<GesUsuarioBOL> lista = new List<GesUsuarioBOL>
            {
                _gesUsuarioBOL
            };
            return lista;
        }
    }
}
