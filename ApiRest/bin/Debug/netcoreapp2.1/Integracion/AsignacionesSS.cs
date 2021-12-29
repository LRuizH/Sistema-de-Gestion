using DatoSSINE.VO.SurveySolution.Asignaciones;
using DatoSSINE.VO.SurveySolution.General;
using Framework.BLL.Utilidades.Seguridad;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Framework.BOL.Enumeracion;
using Framework.DAL;
using Framework.BOL.Integracion;
using System.Linq;

namespace Framework.BLL.Integracion
{
   public class AsignacionesSS
    {
        AppSettings _appSettings = new AppSettings();
        public  Resultado NuevaAsignacion(List<GesNuevaAsignacionSS> modelo, string rutResposable, int alc,long CodAlc, int TipoAsignacion)
        {
            Resultado response = new Resultado();
            try
            {
                var parametros = new ParametrosBLL().ListParametros(1);
                Parallel.ForEach(modelo, async list =>
                {
                    list.Asignacion.URLSURVEY = parametros.Where(x => x.Descripcion == "UrlApi").Select(x => x.Valor).FirstOrDefault();
                    list.Asignacion.userAPI = parametros.Where(x => x.Descripcion == "UsrAPI").Select(x => x.Valor).FirstOrDefault();
                    list.Asignacion.PassAPI = parametros.Where(x => x.Descripcion == "PassApi").Select(x => x.Valor).FirstOrDefault();
                    list.Asignacion.WorkSpace = parametros.Where(x => x.Descripcion == "WorkSpaceSS").Select(x => x.Valor).FirstOrDefault();
                    var request = await new DatoSSINE.BLL.SurveySolution.AsignacionesBLL().NuevaAsignacion(list.Asignacion, string.Empty);
                    response.Cabecera = request.Cabecera;
                    if (response.Cabecera.CodigoRespuesta == 200)
                    {
                        //Agragar datos de la asignación en base de gestion
                        var IdAsignacion = request.Asignacion.NewAssignmentsSal.Assignment.Id;
                        var InsertaAsignacion = new GesUsuarioDAL().InsertarAsignacionUserSG(rutResposable, IdAsignacion, (int)EstadosAsignacion.ASIGNADO, TipoAsignacion, alc, CodAlc, list.Asignacion.NewAssignmentsEnt.Quantity);
                        Cabecera cabecera = new Cabecera();
                        cabecera.CodigoRespuesta = 200;
                        response.Cabecera = cabecera;
                    }
                    else
                    {
                        //var InsertaAsignacion = new GesUsuarioDAL().InsertarAsignacionUserSG(rutResposable, null, (int)EstadosAsignacion.SINASIGNAR, null, alc, list.Direccion);
                        Cabecera cabecera = new Cabecera();
                        cabecera.CodigoRespuesta = 501;
                        cabecera.Mensaje = "Error de servicio";
                        response.Cabecera = cabecera;

                        DatoSSINE.VO.SurveySolution.General.Error error = new DatoSSINE.VO.SurveySolution.General.Error();
                        error.CodigoError = 501;
                        error.Mensaje = response.Error.Mensaje;
                        response.Error = error;
                    }

                });
                
            }
            catch (Exception ex)
            {
                Cabecera cabecera = new Cabecera();
                cabecera.CodigoRespuesta = 501;
                cabecera.Mensaje = "Error de servicio";
                response.Cabecera = cabecera;

                DatoSSINE.VO.SurveySolution.General.Error error = new DatoSSINE.VO.SurveySolution.General.Error();
                error.CodigoError = 501;
                error.Mensaje = ex.Message;
                response.Error = error;
            }


            return response;
        }

        public  Resultado ReAsignacion(List<GesReasignacionSS> ListaAsignaciones, string RutResponsable)
        {
            Resultado response = new Resultado();
            try
            {
                var parametros = new ParametrosBLL().ListParametros(1);
                Parallel.ForEach(ListaAsignaciones, async list =>
                {
                    list.Reasignacion.URLSURVEY = parametros.Where(x => x.Descripcion == "UrlApi").Select(x => x.Valor).FirstOrDefault();
                    list.Reasignacion.userAPI = parametros.Where(x => x.Descripcion == "UsrAPI").Select(x => x.Valor).FirstOrDefault();
                    list.Reasignacion.PassAPI = parametros.Where(x => x.Descripcion == "PassApi").Select(x => x.Valor).FirstOrDefault();
                    list.Reasignacion.WorkSpace = parametros.Where(x => x.Descripcion == "WorkSpaceSS").Select(x => x.Valor).FirstOrDefault();
                    var request = await new DatoSSINE.BLL.SurveySolution.AsignacionesBLL().ReAsignacion(list.Reasignacion, string.Empty);
                    response.Cabecera = request.Cabecera;
                    if (response.Cabecera.CodigoRespuesta == 200)
                    {
                        ////Agregar datos de la asignación en base de gestion
                        var InsertaAsignacion = new GesUsuarioDAL().InsertarAsignacionUserSG(RutResponsable, list.Reasignacion.Id_Asignacion.ToString(), (int)EstadosAsignacion.REASIGNADO);
                        Cabecera cabecera = new Cabecera();
                        cabecera.CodigoRespuesta = 200;
                        response.Cabecera = cabecera;
                    }
                    else
                    {
                        
                        Cabecera cabecera = new Cabecera();
                        cabecera.CodigoRespuesta = 501;
                        cabecera.Mensaje = "Error de servicio";
                        response.Cabecera = cabecera;

                        DatoSSINE.VO.SurveySolution.General.Error error = new DatoSSINE.VO.SurveySolution.General.Error();
                        error.CodigoError = 501;
                        error.Mensaje = response.Error.Mensaje;
                        response.Error = error;
                    }

                });
                
            }
            catch (Exception ex)
            {
                Cabecera cabecera = new Cabecera();
                cabecera.CodigoRespuesta = 501;
                cabecera.Mensaje = "Error de servicio";
                response.Cabecera = cabecera;

                DatoSSINE.VO.SurveySolution.General.Error error = new DatoSSINE.VO.SurveySolution.General.Error();
                error.CodigoError = 501;
                error.Mensaje = ex.Message;
                response.Error = error;
            }


            return response;
        }
    }
}
