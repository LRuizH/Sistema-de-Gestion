using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Framework.BLL;
using Framework.BLL.Utilidades.Seguridad;
using Framework.BLL.Vistas;
using Framework.BOL;
using Framework.DAL;
using Microsoft.AspNetCore.Mvc;

namespace ApiRest.Controllers
{
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
        AppSettings _appSettings = new AppSettings();

        /// <summary> 
        /// Obtiene componente para la vista de la información de la cuenta
        /// </summary>
        [HttpGet]
        [Route("cuenta")]
        public List<ElementoHtml> ObtieneCuenta()
        {
            VCuenta _vCuenta = new VCuenta();
            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _vCuenta.ObtieneCuenta()
            };

            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }

        /// <summary>
        /// Cambio de perfil
        /// </summary>
        [HttpPost]
        [Route("cambio-perfil")]
        public List<ElementoHtml> CambiarPerfil(int sistema_id, string usu_id, int perfil_id)
        {
            GesUsuarioBLL _gesUsuarioBLL = new GesUsuarioBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            if (_gesUsuarioBLL.ObtieneUsuarioConectado(_appSettings.ObtieneCookie()).Usu_id != "null")
            {
                _elementoHtml.Elemento_html = _gesUsuarioBLL.CambioPerfil(sistema_id, usu_id, perfil_id);
            }
            else
            {
                _elementoHtml.Elemento_html = "¡Ups! Acceso no autorizado.";
            }

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }

        /// <summary>
        /// Obtiene componente para la vista del mantenedor de usuarios
        /// </summary>
        [HttpGet]
        [Route("mantenedor-usuarios")]
        public List<ElementoHtml> ObtieneMantenedorUsuario()
        {
            AppSettings _appSettings = new AppSettings();
            //VAdminUsuario _vAdminUsuario = new VAdminUsuario();
            GesUsuarioBLL _gesUsuarioBLL = new GesUsuarioBLL();
            GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL();

            _gesUsuarioBOL = _gesUsuarioBLL.ObtieneUsuarioConectado(_appSettings.ObtieneCookie());

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                //Elemento_html = _vAdminUsuario.VMantenedorUsuarios(_gesUsuarioBOL)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }

        [HttpGet]
        [Route("lista_tbUsuarios")]
        public List<ElementoHtml> ListaTablaUsuarios()
        {
            AppSettings _appSettings = new AppSettings();
            //CAdminUsuario _cAdminUsuario = new CAdminUsuario();
            GesUsuarioBLL _gesUsuarioBLL = new GesUsuarioBLL();
            GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL();

            _gesUsuarioBOL = _gesUsuarioBLL.ObtieneUsuarioConectado(_appSettings.ObtieneCookie());

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                //Elemento_html = _cAdminUsuario.LlenaTablaUsuarios(_gesUsuarioBOL)
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }

        //[HttpGet]
        //[Route("buscaUsuarioBD")]
        //public List<ElementoHtml> BuscaUsuarioBD(string usu_id)
        //{           
        //    GesUsuarioBLL gesUsuarioBLL = new GesUsuarioBLL();
        //    GesUsuarioBOL gesUsuarioBOL = new GesUsuarioBOL();
        //    gesUsuarioBOL.Usu_id = usu_id;
        //    ElementoHtml _elementoHtml = new ElementoHtml
        //    {
        //        Elemento_html = gesUsuarioBLL.FBuscarUsuarioBD(gesUsuarioBOL)
        //    };
        //    // Retorno objeto
        //    List<ElementoHtml> lista = new List<ElementoHtml>();
        //    lista.Add(_elementoHtml);
        //    return lista;
        //}

        /// <summary>
        /// Obtiene componente para la vista del edición de usuarios
        /// </summary>       
        [HttpGet]
        [Route("Veditar")]
        public List<string> VEditarUsuario(string rut)
        {
            AppSettings _appSettings = new AppSettings();
           // CAdminUsuario cAdminUsuario = new CAdminUsuario();
            GesUsuarioDAL gesUsuarioDAL = new GesUsuarioDAL();
            GesUsuarioBLL _gesUsuarioBLL = new GesUsuarioBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();
            List<string> lista = new List<string>();
            var datosUsuarioConectado = _gesUsuarioBLL.ObtieneUsuarioConectado(_appSettings.ObtieneCookie());

            GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL
            {
                Usu_id = rut
            };
            try
            {
                //muestra vista formulario editar usuario
                var listaUsuario = gesUsuarioDAL.ListaUsuarios(_gesUsuarioBOL, "1");
                //gesUsuarioDAL.Buscar<GesUsuarioBOL>(_gesUsuarioBOL, "Usu_id");
                if (listaUsuario.Rows.Count > 0)
                {
                    foreach (DataRow dt in listaUsuario.Rows)
                    {
                        _gesUsuarioBOL.Usu_id = dt[0].ToString();
                        _gesUsuarioBOL.Usu_rut = dt[1].ToString();
                        _gesUsuarioBOL.Usu_nombre = dt[2].ToString();
                        _gesUsuarioBOL.Usu_email = dt[3].ToString();
                        _gesUsuarioBOL.Usu_telefono = dt[4].ToString();
                        _gesUsuarioBOL.Usu_activo = Convert.ToBoolean(dt[5].ToString());
                        _gesUsuarioBOL.Usu_fecha_contratacion = dt[6].ToString();
                        _gesUsuarioBOL.Usu_tipo = dt[7].ToString();
                        _gesUsuarioBOL.Usu_perfil = Convert.ToInt32(dt[8].ToString());
                    }
                    //lista.Add(cAdminUsuario.VistaFormularioAdmin(_gesUsuarioBOL, 2, datosUsuarioConectado));
                    lista.Add("OK");
                }
                else
                {
                    //lista.Add(cAdminUsuario.VistaFormularioAdmin(_gesUsuarioBOL, 1, datosUsuarioConectado));
                    lista.Add("NOK");
                }
            }
            catch (Exception ex)
            {
                //lista.Add(cAdminUsuario.VistaFormularioAdmin(_gesUsuarioBOL, 1, datosUsuarioConectado));
                lista.Add("NOK");
            }
            return lista;
        }

        [HttpPost]
        [Route("ingreso-usuario")]
        public List<ElementoHtml> IngresoUsuario(string formData)
        {
            GesUsuarioBLL _gesUsuarioBLL = new GesUsuarioBLL();
            ElementoHtml _elementoHtml = new ElementoHtml();

            GesUsuarioDAL gesUsuarioDAL = new GesUsuarioDAL();
            GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL();

            if (_gesUsuarioBLL.ObtieneUsuarioConectado(_appSettings.ObtieneCookie()).Usu_id != "null")
            {
                var datosUsuarioConectado = _gesUsuarioBLL.ObtieneUsuarioConectado(_appSettings.ObtieneCookie());
                _elementoHtml.Elemento_html = _gesUsuarioBLL.GrabarUsuario(formData, datosUsuarioConectado.Usu_id);

            }
            else
            {
                _elementoHtml.Elemento_html = "¡Ups! Acceso no autorizado.";
            }

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }
    }
}
