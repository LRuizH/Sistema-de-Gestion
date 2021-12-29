using DatoSSINE.BLL.SurveySolution;
using DatoSSINE.VO.SurveySolution.General;
using DatoSSINE.VO.SurveySolution.Usuario;
using Framework.BLL.Utilidades.Seguridad;
using Framework.BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Framework.BOL.Enumeracion;

namespace Framework.BLL.Integracion
{
   public class UsuariosSS
    {
        AppSettings _appSettings = new AppSettings();
        public  Resultado CrearUsuarios(GesUsuarioBOL modelo, Role rol, string rutSupervisor = null)
        {
            Resultado response = new Resultado();
            try
            {
                var parametros = new ParametrosBLL().ListParametros(1);
                var urlSurvey = parametros.Where(x => x.Descripcion == "UrlApi").Select(x => x.Valor).FirstOrDefault();
       
                var request = new UsuarioBLL().CrearUsuarioAsync(new CreateUsuarioEnt
                {
                    Email = string.IsNullOrEmpty(modelo.Usu_email) ? parametros.Where(x => x.Descripcion == "EmailUserPaso").Select(x => x.Valor).FirstOrDefault() : modelo.Usu_email,
                    FullName = string.IsNullOrEmpty(modelo.Usu_nombre) ? "User " + modelo.Usu_rut.Replace("-", "") : modelo.Usu_nombre,
                    Password = string.IsNullOrEmpty(modelo.Usu_contrasena) ? parametros.Where(x => x.Descripcion == "PasswordUserPaso").Select(x => x.Valor).FirstOrDefault() : modelo.Usu_contrasena,
                    PhoneNumber = string.IsNullOrEmpty(modelo.Usu_telefono) ? parametros.Where(x => x.Descripcion == "TelefonoUserPaso").Select(x => x.Valor).FirstOrDefault() : modelo.Usu_telefono,
                    Role = (int)rol,
                    Supervisor = rol == Role.SUPERVISOR ? string.Empty : rutSupervisor.Replace("-", ""),
                    URLSurveySS = urlSurvey,
                    UserName = modelo.Usu_rut.Replace("-", ""),
                    WorkSpaceSS = parametros.Where(x => x.Descripcion == "WorkSpaceSS").Select(x => x.Valor).FirstOrDefault(),
                    userAPI = parametros.Where(x => x.Descripcion == "UsrAPI").Select(x => x.Valor).FirstOrDefault(),
                    PassAPI = parametros.Where(x => x.Descripcion == "PassApi").Select(x => x.Valor).FirstOrDefault()
                }, string.Empty);

                response = request.Result;
            }
            catch (Exception ex)
            {
                Cabecera cabecera = new Cabecera();
                cabecera.CodigoRespuesta = 501;
                cabecera.Mensaje = "Error de servicio";
                response.Cabecera = cabecera;

                Error error = new Error();
                error.CodigoError = 501;
                error.Mensaje = ex.Message;
                response.Error = error;

            }

            return response;
        }

        public Resultado ModificarSupervisor(ModificarSupervisorEnt modelo)
        {

            Resultado response = new Resultado();
            try
            {
                var parametros = new ParametrosBLL().ListParametros(1);
                var urlSurvey = parametros.Where(x => x.Descripcion == "UrlApi").Select(x => x.Valor).FirstOrDefault();// _appSettings.UrlApi;
                modelo.URLSURVEY = urlSurvey;
                modelo.userAPI = parametros.Where(x => x.Descripcion == "UsrAPI").Select(x => x.Valor).FirstOrDefault();
                modelo.PassAPI = parametros.Where(x => x.Descripcion == "PassApi").Select(x => x.Valor).FirstOrDefault();
                modelo.Conexion = parametros.Where(x => x.Descripcion == "ServidorConexion").Select(x => x.Valor).FirstOrDefault();
                var request = new UsuarioBLL().ModificarTeamsAsync(modelo, string.Empty);
                request.Wait();
                response = request.Result;
            }
            catch (Exception ex)
            {
                Cabecera cabecera = new Cabecera();
                cabecera.CodigoRespuesta = 501;
                cabecera.Mensaje = "Error de servicio";
                response.Cabecera = cabecera;

                Error error = new Error();
                error.CodigoError = 501;
                error.Mensaje = ex.Message;
                response.Error = error;

            }

            return response;
        }
    }
}
