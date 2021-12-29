using System;
using System.Collections.Generic;
using System.Text;
using Framework.BOL;
using Framework.BLL.Utilidades.Encriptacion;
using Framework.DAL;
using Microsoft.AspNetCore.Http;
using System.DirectoryServices;
using System.Collections;
using System.Linq;
using Newtonsoft.Json;
using Framework.BLL.Utilidades.Html;
using Framework.BLL.Utilidades.Seguridad;
using Framework.BLL.Utilidades;
using Framework.BLL.Componentes;
using System.Data;
using Newtonsoft.Json.Linq;

namespace Framework.BLL
{
    public class GesUsuarioBLL
    {
        /// <summary>
        /// Obtiene información de usuario autenticado
        /// </summary>
        public GesUsuarioBOL ObtieneUsuarioConectado(string cookie)
        {
            GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL();
            try
            {
                //Busco usuario
                Encrypt _encrypt = new Encrypt();
                _gesUsuarioBOL.Usu_id = _encrypt.DecryptString(cookie);

                GesUsuarioDAL _gesUsuarioDAL = new GesUsuarioDAL();
                List<GesUsuarioBOL> listaUsuario = _gesUsuarioDAL.Buscar<GesUsuarioBOL>(_gesUsuarioBOL, "Usu_id");
                if (listaUsuario.Count > 0)
                {
                    _gesUsuarioBOL = listaUsuario[0];
                    return _gesUsuarioBOL;
                }
                else
                {
                    _gesUsuarioBOL.Usu_id = "null";
                    return _gesUsuarioBOL;
                }
            }
            catch (Exception ex)
            {
                _gesUsuarioBOL.Usu_id = "null";
                return _gesUsuarioBOL;
            }
        }

        /// <summary>
        /// Autentica un usuario
        /// </summary>
        public GesUsuarioBOL AutenticaUsuario(GesUsuarioBOL _gesUsuarioBOL, bool recuerda_contrasena)
        {
            //Verifico usuario en Active Directory
            ActiveDirectory _activeDirectory = new ActiveDirectory
            {
                Cuenta_id = _gesUsuarioBOL.Usu_id,
                Cuenta_contrasena = _gesUsuarioBOL.Usu_contrasena
            };

            //Retorno objeto
            _gesUsuarioBOL = _activeDirectory.AutenticaUsuarioActiveDirectory(_gesUsuarioBOL);

            //Si usuario es nulo busco en tabla
            if (_gesUsuarioBOL.Usu_respuesta == 0) // No encontrado en Active Directory
            {
                GesUsuarioDAL gesUsuarioDAL = new GesUsuarioDAL();
                List<GesUsuarioBOL> listaUsuario = gesUsuarioDAL.Autenticar<GesUsuarioBOL>(_gesUsuarioBOL);
                if (listaUsuario.Count > 0)
                {
                    _gesUsuarioBOL.Usu_respuesta = 2;
                    _gesUsuarioBOL = listaUsuario[0];
                }
            }

            if (_gesUsuarioBOL.Usu_respuesta == 1) //Encontrado en Active Directory
            {
                GesUsuarioDAL gesUsuarioDAL = new GesUsuarioDAL();
                List<GesUsuarioBOL> listaUsuario = gesUsuarioDAL.AutenticarAD<GesUsuarioBOL>(_gesUsuarioBOL);
                if (listaUsuario.Count > 0)
                {
                    _gesUsuarioBOL.Usu_respuesta = 2;
                    _gesUsuarioBOL = listaUsuario[0];
                }
                else
                {
                    _gesUsuarioBOL.Usu_respuesta = 1;
                }
            }
            return _gesUsuarioBOL;
        }

        /// <summary>
        /// Autentica un usuario para ingreso directo de cuestionario
        /// </summary>
        public GesUsuarioBOL AutenticaUsuarioPorCuestionario(GesUsuarioBOL _gesUsuarioBOL, int sistema_id)
        {
            GesUsuarioDAL gesUsuarioDAL = new GesUsuarioDAL();
            List<GesUsuarioBOL> listaUsuario = gesUsuarioDAL.AutenticarPorCuestionario<GesUsuarioBOL>(_gesUsuarioBOL, sistema_id);
            if (listaUsuario.Count > 0)
            {
                _gesUsuarioBOL.Usu_respuesta = 1;
                _gesUsuarioBOL = listaUsuario[0];
            }

            return _gesUsuarioBOL;
        }

        /// <summary>
        /// Cambia de perfil en sistema
        /// </summary>
        public string CambioPerfil(int sistema_id, string usu_id, int perfil_id)
        {
            string _strRespuesta = "";
            GesUsuarioDAL gesUsuarioDAL = new GesUsuarioDAL();
            _strRespuesta = gesUsuarioDAL.CambioPerfil(sistema_id, usu_id, perfil_id);

            if (_strRespuesta != "ok")
            {
                _strRespuesta = "<div class=\"alert alert-danger text-center\">Ocurrio un error al realizar el cambio de perfil. Intentelo nuevamente. Si vuele a tener inconvenientes contactese con soporte TI.</div>";
            }

            return _strRespuesta;
        }

        /// <summary>
        /// Busca un usuario
        /// </summary>
        public GesUsuarioBOL BuscarUsuario(GesUsuarioBOL _gesUsuarioBOL, string columna_busqueda)
        {
            GesUsuarioDAL gesUsuarioDAL = new GesUsuarioDAL();
            List<GesUsuarioBOL> listaUsuario = gesUsuarioDAL.Buscar<GesUsuarioBOL>(_gesUsuarioBOL, columna_busqueda);
            if (listaUsuario.Count > 0)
            {
                _gesUsuarioBOL = listaUsuario[0];
            }
            else
            {
                _gesUsuarioBOL.Usu_id = "null";
            }
            return _gesUsuarioBOL;
        }

        /// <summary>
        /// Obtiene la lista de perfil
        /// </summary>
        /// <returns></returns>
        public string ObtienePerfil()
        {
            CSelect select = new CSelect();
            string str = "";
            GesUsuarioDAL gesUsuarioDAL = new GesUsuarioDAL();
            CodeValue codeValue = new CodeValue();
            select.select_data = gesUsuarioDAL.ListarPerfiles<CodeValue>();
            select.select_start = "Seleccione";
            select.select_search = true;
            select.select_multiple = true;
            select.select_id = "perfil_id";
            select.select_class = "selectpicker obligatorio";
            str = select.getHTMLSelect(select);
            return str;
        }

        /// <summary>
        /// Inserta o actualiza un nuevo usuario
        /// </summary>
        /// <param name="_gesUsuarioBOL"></param>
        /// <returns></returns>
        public string GrabarUsuario(string formData, string usu_id_conectado)
        {
            //evento=1:ingreso, evento=2:edicion
            //usu_tipo=1:usuario interno, usu_tipo=2:usuario externo
            string _strHTML = "";
            StringBuilder sb = new StringBuilder();
            GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL();
            GesUsuarioDAL gesUsuarioDAL = new GesUsuarioDAL();
            dynamic data = JsonConvert.DeserializeObject(formData);
            try
            {
                if (data["evento"] != "")
                {
                    string evento = data["evento"];
                    _gesUsuarioBOL.Usu_tipo = data["tipo_usuario"];

                    if (evento == "1")
                    {
                        if (data["usuario_ine"] == "")
                        {
                            _gesUsuarioBOL.Usu_id = data["rut"];
                            List<GesUsuarioBOL> usuRegistrado = gesUsuarioDAL.Buscar<GesUsuarioBOL>(_gesUsuarioBOL, "Usu_id");
                            if (usuRegistrado.Count > 0)
                                return "Existe_Rut";
                            else
                            {
                                _gesUsuarioBOL.Usu_rut = data["rut"];
                                List<GesUsuarioBOL> rutRegistrado = gesUsuarioDAL.Buscar<GesUsuarioBOL>(_gesUsuarioBOL, "Usu_rut");
                                if (rutRegistrado.Count > 0) return "Existe_Rut";
                            }
                        }
                        else
                        {
                            _gesUsuarioBOL.Usu_id = data["usuario_ine"];
                            List<GesUsuarioBOL> usuRegistrado = gesUsuarioDAL.Buscar<GesUsuarioBOL>(_gesUsuarioBOL, "Usu_id");
                            if (usuRegistrado.Count > 0)
                                return "Existe_Usuario";
                            else
                            {
                                _gesUsuarioBOL.Usu_rut = data["rut"];
                                List<GesUsuarioBOL> rutRegistrado = gesUsuarioDAL.Buscar<GesUsuarioBOL>(_gesUsuarioBOL, "Usu_rut");
                                if (rutRegistrado.Count > 0) return "Existe_Rut";
                            }

                        }
                    }
                    else
                    {
                        _gesUsuarioBOL.Usu_id = data["usuario_ine"] != "" ? data["usuario_ine"] : data["rut"];
                        //    var usu_carga_trabajo = gesUsuarioDAL.ListaUsuarios(_gesUsuarioBOL, "1");
                        //    if (usu_carga_trabajo.Rows.Count > 0)
                        //    {
                        //        foreach (DataRow dt in usu_carga_trabajo.Rows)
                        //        {
                        //            if (Convert.ToInt32(dt[7].ToString()) == 3) return "Existe_Carga";
                        //        }
                        //    }
                    }

                    _gesUsuarioBOL.Usu_rut = data["rut"];
                    _gesUsuarioBOL.Usu_nombre = data["nombre"];
                    _gesUsuarioBOL.Usu_email = data["email"];
                    _gesUsuarioBOL.Usu_telefono = data["telefono"];
                    _gesUsuarioBOL.Usu_activo = (data["usu_activo"] == 1) ? true : false;
                    _gesUsuarioBOL.Usu_estado = data["proceso_usu"];
                    _gesUsuarioBOL.Usu_proceso = data["usu_proceso"];
                    _gesUsuarioBOL.Usu_fecha_contratacion = data["fecha_contratacion"];

                    string _geo_usu = data["geo_usu"];
                    string _perfil_usu = data["perfil_usu"];
                    string[] geografia_usu = _geo_usu.Split(",");
                    string[] perfil_usu = _perfil_usu.Split(",");

                    DataTable dtGeografia = new DataTable("geografia");
                    DataTable dtPerfil = new DataTable("perfil");
                    dtGeografia.Columns.Add("geografia_id", typeof(int));
                    dtGeografia.Columns.Add("geografia_name", typeof(string));
                    dtPerfil.Columns.Add("perfil_id", typeof(int));
                    dtPerfil.Columns.Add("perfil_name", typeof(string));

                    foreach (var gu in geografia_usu)
                    {
                        dtGeografia.Rows.Add(Convert.ToInt32(gu), _gesUsuarioBOL.Usu_id);
                    }
                    foreach (var pu in perfil_usu)
                    {
                        dtPerfil.Rows.Add(Convert.ToInt32(pu), _gesUsuarioBOL.Usu_id);
                    }

                    GesUsuarioDAL _gesUsuarioDAL = new GesUsuarioDAL();

                    _strHTML = _gesUsuarioDAL.Insertar(_gesUsuarioBOL, dtPerfil, dtGeografia, usu_id_conectado);

                }
            }
            catch (Exception)
            {
                _strHTML = "Lo sentimos! ocurrió un error  no se pudo ingresar el usuario; contáctese con el administrador del usuario";
            }
            return _strHTML;
        }

        /// <summary>
        /// Autentica un usuario para ingreso telefonico
        /// </summary>
        public GesUsuarioBOL AutenticaUsuarioTelefonico(GesUsuarioBOL _gesUsuarioBOL, int sistema_id)
        {
            GesUsuarioDAL gesUsuarioDAL = new GesUsuarioDAL();
            List<GesUsuarioBOL> listaUsuario = gesUsuarioDAL.AutenticarPorRegistroTelefonico<GesUsuarioBOL>(_gesUsuarioBOL, sistema_id);
            if (listaUsuario.Count > 0)
            {
                _gesUsuarioBOL.Usu_respuesta = 1;
                _gesUsuarioBOL = listaUsuario[0];
            }

            return _gesUsuarioBOL;
        }
    }
}
