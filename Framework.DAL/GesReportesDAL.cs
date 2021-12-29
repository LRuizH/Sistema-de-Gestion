using Framework.BOL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Framework.DAL
{
    public class GesReportesDAL
    {
        string _strSql = "";

        /// <summary>
        /// inserta asignaciones
        /// </summary>
        public DataSet GeneraReporte(GesReportesBOL gesReportesBOL)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure(gesReportesBOL.LLamada);
                _exQuery.AgregarParametro("Usu_id", gesReportesBOL.Usu_id);
                //_exQuery.AgregarParametro("sistema_id", Sistema_id);
                return _exQuery.EjecutarProcedimientoReportes();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Lista cuestionarios para supervision movil segun estado
        /// </summary>
        //public string GeneraInsertReportes()
        //{
        //    try
        //    {
        //        StoredProcedure _exQuery = new StoredProcedure("[CAF_PROCESAMIENTO].[dbo].[PA_PROCESAMIENTO_REPORTES]");
        //        return _exQuery.EjecutarProcedimiento().ToString();
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message);
        //    }
        //}

        /// <summary>
        /// genera insert para reportes
        /// </summary>
        public string GeneraInsertReportes()
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "exec [CAF_PROCESAMIENTO].[dbo].[PA_PROCESAMIENTO_REPORTES] ";
                return _exQuery.EjecutarConsulta(_strSql).ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
