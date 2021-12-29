using System;
using System.Collections.Generic;
using System.Data;
using Framework.BOL;


namespace Framework.DAL
{
    public class GesUsuarioDAL
    {
        string _strSql = "";

        /// <summary>
        /// Autentica un usuario
        /// </summary>
        public List<T> Autenticar<T>(GesUsuarioBOL _gesUsuarioBOL)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT Usu_id = IdUsuario, Usu_nombre = Nombre, Usu_email = Email, Usu_telefono = Telefono, Usu_respuesta = 2 FROM dbo.ges_usuario WHERE IdUsuario=@IdUsuario AND Contrasenia=@Contrasenia and Activo = 1";
                _exQuery.AgregarParametro("IdUsuario", _gesUsuarioBOL.Usu_id);
                _exQuery.AgregarParametro("Contrasenia", _gesUsuarioBOL.Usu_contrasena);
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Autentica un usuario por active directory
        /// </summary>
        public List<T> AutenticarAD<T>(GesUsuarioBOL _gesUsuarioBOL)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT Usu_id,Usu_nombre,Usu_email,Usu_telefono,Usu_respuesta = 2 FROM dbo.ges_usuario WHERE Usu_id=@Usu_id";
                _exQuery.AgregarParametro("Usu_id", _gesUsuarioBOL.Usu_id);
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Autentica un usuario para ingreso directo de cuestionario
        /// </summary>
        public List<T> AutenticarPorCuestionario<T>(GesUsuarioBOL _gesUsuarioBOL, int sistema_id)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "DECLARE @IdQRSuso INT " +
                          "SET @IdQRSuso = (SELECT IdQRSuso FROM dbo.GES_INTEGRACION_QR_SUSO WHERE CodigoAcceso = @IdUsuario) " + 
                          "DECLARE @IdUnicoVivienda INT " +
                          "SET @IdUnicoVivienda = (SELECT COUNT(*) FROM dbo.GES_INTEGRACION_QR_SUSO WHERE IdQRSuso = @IdQRSuso) " +
                          "DECLARE @ExisteUsuario INT " +
                          "SET @ExisteUsuario = (SELECT COUNT(*) FROM dbo.GES_USUARIO WHERE IdUsuario = CONVERT(NVARCHAR,@IdQRSuso)) " +
                          "IF (@IdUnicoVivienda = 1) BEGIN " +
                                "IF (@ExisteUsuario = 1) BEGIN " +
                                    "SELECT Usu_id = IdUsuario, Usu_nombre = Nombre, Usu_respuesta = 1 FROM dbo.GES_USUARIO WHERE IdUsuario=CONVERT(VARCHAR,@IdQRSuso) AND  Activo = 1 " +
                                "END " +
                                "ELSE BEGIN " +
                                    "INSERT INTO dbo.GES_USUARIO (IdUsuario,Nombre,FechaRegistro,Activo) VALUES (@IdQRSuso,@Nombre,GETDATE(),1) " +
                                    "INSERT INTO dbo.GES_SISTEMA_USUARIO VALUES (@IdSistema,@IdQRSuso) " +
                                    "INSERT INTO dbo.GES_ATRIBUTO_USUARIO VALUES (@IdSistema,@IdQRSuso,8,2,1,1,0) " +
                                    "SELECT Usu_id = IdUsuario, Usu_nombre = Nombre, Usu_respuesta = 1 FROM dbo.GES_USUARIO WHERE IdUsuario=CONVERT(VARCHAR,@IdQRSuso) AND Activo = 1 " +
                                "END " +
                          "END " +
                          "ELSE BEGIN " + 
                                "SELECT 0 " +
                          "END";
                _exQuery.AgregarParametro("IdUsuario", _gesUsuarioBOL.Usu_id.ToString());
                _exQuery.AgregarParametro("Nombre", _gesUsuarioBOL.Usu_nombre);
                _exQuery.AgregarParametro("IdSistema", sistema_id);
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Cambia de perfil en sistema
        /// </summary>
        public string CambioPerfil(int sistema_id, string usu_id, int perfil_id)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "UPDATE dbo.ges_atributo_usuario SET AtributoUsuarioPerfilActivo = 0 WHERE IdSistema = @sistema_id AND IdUsuario = @usu_id " +
                          "UPDATE dbo.ges_atributo_usuario SET AtributoUsuarioPerfilActivo = 1 WHERE IdSistema = @sistema_id AND IdUsuario = @usu_id AND IdPerfil = @perfil_id " +
                          "SELECT 'ok'";
                _exQuery.AgregarParametro("sistema_id", sistema_id);
                _exQuery.AgregarParametro("usu_id", usu_id);
                _exQuery.AgregarParametro("perfil_id", perfil_id);
                return _exQuery.EjecutarConsulta(_strSql).Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        /// <summary>
        /// Busca un usuario
        /// </summary>
        public List<T> Buscar<T>(GesUsuarioBOL _gesUsuarioBOL, string _columna)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                switch (_columna)
                {
                    case "Usu_id":
                        _strSql = "SELECT Usu_id = IdUsuario, Usu_rut = Rut, Usu_nombre = Nombre, Usu_email = Email, Usu_telefono = Telefono, Usu_activo = Activo, Usu_cambio_contrasena = ISNULL(CambioContrasenia,0) FROM dbo.ges_usuario WHERE idUsuario = @IdUsuario";
                        _exQuery.AgregarParametro("IdUsuario", _gesUsuarioBOL.Usu_id);
                        break;
                    case "Usu_email":
                        _strSql = "SELECT Usu_id = IdUsuario, Usu_nombre =Nombre, Usu_email = Email, Usu_telefono = Telefono FROM dbo.ges_usuario WHERE Email = @Email";
                        _exQuery.AgregarParametro("Email", _gesUsuarioBOL.Usu_email);
                        break;
                    case "Usu_rut":
                        _strSql = "SELECT Usu_id =IdUsuario, Usu_rut = Rut, Usu_nombre =Nombre, Usu_email = Email, Usu_telefono = Telefono, Usu_activo = Activo FROM dbo.ges_usuario WHERE Rut = @Rut";
                        _exQuery.AgregarParametro("Rut", _gesUsuarioBOL.Usu_rut);
                        break;
                    case "Usu_recuerda":
                        _strSql = "SELECT Usu_id =IdUsuario, Usu_rut = Rut, Usu_nombre =Nombre, Usu_email = Email, Usu_telefono = Telefono, Usu_activo = Activo FROM dbo.ges_usuario WHERE IdUsuario = @Rut OR IdUsuario IN (SELECT IdUsuario FROM dbo.ges_atributo_usuario WHERE IdSistema = 2)";
                        _exQuery.AgregarParametro("Rut", _gesUsuarioBOL.Usu_rut);
                        break;
                }
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
               
        /// <summary>
        /// Inserta y actualiza un usuario
        /// </summary>
        /// <param name="_gesUsuarioBOL"></param>
        /// <returns></returns>
        public string Insertar(GesUsuarioBOL _gesUsuarioBOL, DataTable dt_perfil, DataTable dt_geografia,string usu_id_conectado)
        { 
            try
            {               
                StoredProcedure sp = new StoredProcedure("Pa_GesUsuario_Insertar");               
                int sistema_id = 1;
                sp.AgregarParametro("IdUsuario_conectado", usu_id_conectado);
                sp.AgregarParametro("IdUsuario", _gesUsuarioBOL.Usu_id);
                sp.AgregarParametro("Rut", _gesUsuarioBOL.Usu_rut);
                sp.AgregarParametro("Nombre", _gesUsuarioBOL.Usu_nombre);
                sp.AgregarParametro("Email", _gesUsuarioBOL.Usu_email);
                sp.AgregarParametro("Telefono", _gesUsuarioBOL.Usu_telefono);           
                sp.AgregarParametro("IdTipoUsuario", _gesUsuarioBOL.Usu_tipo);
                sp.AgregarParametro("Activo", _gesUsuarioBOL.Usu_activo);
                sp.AgregarParametro("FechaContratacion", _gesUsuarioBOL.Usu_fecha_contratacion);
                sp.AgregarParametro("Idestado", Convert.ToInt32(_gesUsuarioBOL.Usu_estado));
                sp.AgregarParametro("IdProceso", _gesUsuarioBOL.Usu_proceso);
                sp.AgregarParametro("IdSistema", sistema_id);
                sp.AgregarParametro("Perfil", dt_perfil);
                sp.AgregarParametro("Geografia", dt_geografia);               
                return sp.EjecutarProcedimiento().Tables[0].Rows[0][0].ToString();              
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Lista todos los usuarios del Sistema
        /// </summary>
        public DataTable ListaUsuarios(GesUsuarioBOL gesUsuarioBOL, string codigo)
        {
            try
            {
                StoredProcedure sp = new StoredProcedure("Pa_GesUsuario_Listas");
                sp.AgregarParametro("usu_id", gesUsuarioBOL.Usu_id);
                sp.AgregarParametro("codigo", codigo);                           
                var dataTable=sp.EjecutarProcedimiento();
                return dataTable.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Consulta por el perfil activo del usuario en el Sistema
        /// </summary>
        public List<T> PerfilUsuarioDAL<T>(GesUsuarioBOL gesUsuarioBOL)
        {
            try
            {
                _strSql = "SELECT CONVERT(VARCHAR,gau.IdPerfil ) AS codigo FROM dbo.ges_atributo_usuario gau WHERE gau.IdUsuario = @IdUsuario AND atributoUsuarioPerfilActivo = 1";
                StoredProcedure sp = new StoredProcedure();
                sp.AgregarParametro("IdUsuario", gesUsuarioBOL.Usu_id);
                return sp.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Consulta por el/los perfiles del usuario en el Sistema
        /// </summary>
        public List<T> PerfilesUsuarioDAL<T>(GesUsuarioBOL gesUsuarioBOL)
        {
            try
            {
                _strSql = "SELECT CONVERT(VARCHAR,gau.IdPerfil) AS codigo, p.Nombre AS valor FROM dbo.ges_atributo_usuario gau INNER JOIN dbo.glo_perfil AS p ON p.IdPerfil = gau.IdPerfil WHERE gau.IdUsuario = @IdUsuario";
                StoredProcedure sp = new StoredProcedure();
                sp.AgregarParametro("IdUsuario", gesUsuarioBOL.Usu_id);
                return sp.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista todos los perfiles del Sistema
        /// </summary>
        public List<T> ListarPerfiles<T>()
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT CONVERT(VARCHAR, IdPerfil) AS codigo, Nombre AS valor FROM dbo.GLO_PERFIL WHERE Activo = 1";
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<T> ListarPerfilesAsignacion<T>(string perfil_usuario, string nivel)
        {
            try
            {
                string condicion = "";
                if (perfil_usuario.Equals("1") || perfil_usuario.Equals("2")) {
                    condicion = " AND IdPerfil > 2 AND IdGeografiaNivel >= " + nivel;
                } else {
                    condicion = " AND IdPerfil > " + perfil_usuario + " AND IdGeografiaNivel >= " + nivel;
                }

                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT CONVERT(VARCHAR, IdPerfil) AS codigo, Nombre AS valor FROM dbo.GLO_PERFIL WHERE Activo = 1 " + condicion;
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista todos los perfiles para asignacion
        /// </summary>
        //public List<T> ListarPerfilesAsignacion<T>()
        //{
        //    try
        //    {
        //        StoredProcedure _exQuery = new StoredProcedure();
        //        _strSql = "SELECT IdPerfil AS codigo, Nombre AS valor FROM dbo.GLO_PERFIL WHERE IdPerfil > 1 AND Activo = 1";
        //        return _exQuery.EjecutarConsulta<T>(_strSql);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        /// <summary>
        /// Lista perfiles segun el perfil del usuario activo
        /// </summary>
        public List<T> ListaPerfilesSegunPerfil<T>(GesUsuarioBOL gesUsuarioBOL)
        {
            try
            {                
                StoredProcedure sp = new StoredProcedure("Pa_GloPerfil_Listas");
                sp.AgregarParametro("usu_id", gesUsuarioBOL.Usu_id);
                return sp.EjecutarProcedimiento<T>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// devuelve el proceso del usuario (sirve modulo reclutamiento y capacitacion)
        /// </summary>      
        public List<T> GetProcesoUsuario<T>(string usu_id)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT top 1 IdProceso Usu_proceso FROM dbo.ges_atributo_usuario where IdSistema=1 and IdUsuario=@usu_id";
                _exQuery.AgregarParametro("usu_id", usu_id);
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<T> GetEstadoUsuario<T>(string usu_id)
        {
            try
            {
                StoredProcedure stored = new StoredProcedure();
                _strSql = "SELECT top 1 ga.IdEstadoUsuario Usu_proceso FROM dbo.ges_usuario u INNER JOIN ges_atributo_usuario ga on ga.IdUsuario = u.IdUsuario WHERE u.IdUsuario=@usu_id ";
                stored.AgregarParametro("usu_id", usu_id);
                return stored.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Permite listar datos de usuario
        /// </summary>
        public DataSet ListaDatosUsuario(GesUsuarioBOL gesUsuarioBOL)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT usu_id = U.IdUsuario, usu_nombre = U.Nombre, usu_email = U.Email, geografia_id = GU.IdGeografia, perfil_id = AU.IdPerfil, " +
                                "(SELECT A.IdAsignacionA FROM dbo.GES_ASIGNACIONES A WHERE A.IdAsignacionDe = @IdUsuario AND A.IdTipoAsignacion = 1) AS areacensal " +
                          "FROM dbo.GES_USUARIO U " +
                                "INNER JOIN dbo.GES_GEOUSUARIO GU ON GU.IdUsuario = U.IdUsuario " +
                                "INNER JOIN dbo.GES_ATRIBUTO_USUARIO AU ON AU.IdUsuario = U.IdUsuario AND GU.IdSistema = AU.IdSistema " +
                          "WHERE U.IdUsuario = @IdUsuario AND AU.AtributoUsuarioPerfilActivo = 1 ";
                _exQuery.AgregarParametro("IdUsuario", gesUsuarioBOL.Usu_id);
                return _exQuery.EjecutarConsulta(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Inserta Usuario SuSo en sistema de gestion
        /// </summary>
        public string InsertarUserSG(string rut, string userId, string TipoUsuario, string SupervisorId)
        {
            try
            {
                StoredProcedure sp = new StoredProcedure("Pa_GesUsuarioSuso_Insertar");

                sp.AgregarParametro("Rut_param", rut);
                sp.AgregarParametro("UserId_param", userId);
                sp.AgregarParametro("TipoUsuario_param", TipoUsuario);
                sp.AgregarParametro("SupervisorId_param", SupervisorId);

                return sp.EjecutarProcedimiento().Tables[0].Rows[0][0].ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// Valida Si existe Usuario SuSo
        /// </summary>
        public DataSet GetUsuarioSuSo(string rut)
        {
            try
            {
                StoredProcedure stored = new StoredProcedure();
                _strSql = "SELECT top 1 US.Rut,US.UserId FROM dbo.GES_USUARIO_SUSO US WHERE US.Rut = @usu_rut ";
                stored.AgregarParametro("usu_rut", rut);
                return stored.EjecutarConsulta(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Inserta Asignaciones SuSo en Sistema de gestión
        /// </summary>
        public string InsertarAsignacionUserSG(string rut, string idAsignacion, int estado, int? TipoAsignacion = null, int? alc = null, long? CodAlc = null ,int? cantAsignaciones = null)
        {
            try
            {
                StoredProcedure sp = new StoredProcedure("Pa_GesAsignacionSuso_Insertar");

                sp.AgregarParametro("Rut", rut);
                sp.AgregarParametro("IdAsignacionSuso", idAsignacion);
                sp.AgregarParametro("ALC", alc);
                sp.AgregarParametro("CantidadAsignaciones", cantAsignaciones);
                sp.AgregarParametro("CodAlc", CodAlc);
                sp.AgregarParametro("TipoAsignacion", TipoAsignacion);
                sp.AgregarParametro("IdEstadoAsignacion", estado);
 

                return sp.EjecutarProcedimiento().Tables[0].Rows[0][0].ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Autentica un usuario para ingreso directo de registro telefonico
        /// </summary>
        public List<T> AutenticarPorRegistroTelefonico<T>(GesUsuarioBOL _gesUsuarioBOL, int sistema_id)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "DECLARE @ExisteUsuario INT " +
                          "SET @ExisteUsuario = (SELECT COUNT(*) FROM dbo.GES_USUARIO WHERE IdUsuario = @IdUsuario) " +
                          "BEGIN IF (@ExisteUsuario = 1) BEGIN " +
                                    "SELECT Usu_id = IdUsuario, Usu_nombre = Nombre, Usu_respuesta = 1 FROM dbo.GES_USUARIO WHERE IdUsuario=@IdUsuario AND  Activo = 1 " +
                                "END " +
                                "ELSE BEGIN " +
                                 "SELECT 0 " +
                                 "END " +
                          "END";
                _exQuery.AgregarParametro("IdUsuario", _gesUsuarioBOL.Usu_id.ToString());
                //_exQuery.AgregarParametro("Nombre", _gesUsuarioBOL.Usu_nombre);
                //_exQuery.AgregarParametro("IdSistema", sistema_id);
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
