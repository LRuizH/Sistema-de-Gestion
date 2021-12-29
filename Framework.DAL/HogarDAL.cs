using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Framework.BOL;

namespace Framework.DAL
{
    public class HogarDAL
    {
        string _strSql = "";

        /// <summary>
        /// Eliminar Hogar
        /// </summary>
        public string EliminarHogar(HogarBOL _hogarBOL)
        {
            try
            {
                StoredProcedure sp = new StoredProcedure("Pa_Cawi_GesHogar_Eliminar");
                sp.AgregarParametro("PK_VIVIENDA", _hogarBOL.PK_VIVIENDA);
                sp.AgregarParametro("PK_HOGAR", _hogarBOL.PK_HOGAR);
                return sp.EjecutarProcedimiento().Tables[0].Rows[0][0].ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Ingresa datos de Tenencia del Hogar
        /// </summary>
        public string IngresoHogarTenencia(HogarBOL _hogarBOL)
        {
            try
            {
                StoredProcedure sp = new StoredProcedure("Pa_Cawi_GesHogarTenencia_Insertar");
                sp.AgregarParametro("PK_VIVIENDA", _hogarBOL.PK_VIVIENDA);
                sp.AgregarParametro("PK_HOGAR", _hogarBOL.PK_HOGAR);
                sp.AgregarParametro("TEN1", _hogarBOL.TEN1);
                sp.AgregarParametro("TEN2", _hogarBOL.TEN2);
                sp.AgregarParametro("TEN3", _hogarBOL.TEN3);

                return sp.EjecutarProcedimiento().Tables[0].Rows[0][0].ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Ingresa datos de Fuente de energía del hogar
        /// </summary>
        public string IngresoHogarFuenteEnergia(HogarBOL _hogarBOL)
        {
            try
            {
                StoredProcedure sp = new StoredProcedure("Pa_Cawi_GesHogarFuenteEnergia_Insertar");
                sp.AgregarParametro("PK_VIVIENDA", _hogarBOL.PK_VIVIENDA);
                sp.AgregarParametro("PK_HOGAR", _hogarBOL.PK_HOGAR);
                sp.AgregarParametro("ENE1", _hogarBOL.ENE1);
                sp.AgregarParametro("ENE2", _hogarBOL.ENE2);
                sp.AgregarParametro("ENE3", _hogarBOL.ENE3);
                sp.AgregarParametro("ENE4", _hogarBOL.ENE4);
                sp.AgregarParametro("ENE5", _hogarBOL.ENE5);
                sp.AgregarParametro("ENE6", _hogarBOL.ENE6);
                sp.AgregarParametro("ENE7", _hogarBOL.ENE7);
                sp.AgregarParametro("ENE8", _hogarBOL.ENE8);
                sp.AgregarParametro("ENE9", _hogarBOL.ENE9);
                sp.AgregarParametro("ENE10", _hogarBOL.ENE10);
                sp.AgregarParametro("ENE11", _hogarBOL.ENE11);
                sp.AgregarParametro("ENE12", _hogarBOL.ENE12);

                return sp.EjecutarProcedimiento().Tables[0].Rows[0][0].ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Ingresa datos de Gestión de residuos del hogar
        /// </summary>
        public string IngresoHogarGestionResiduos(HogarBOL _hogarBOL)
        {
            try
            {
                StoredProcedure sp = new StoredProcedure("Pa_Cawi_GesHogarGestionResiduos_Insertar");
                sp.AgregarParametro("PK_VIVIENDA", _hogarBOL.PK_VIVIENDA);
                sp.AgregarParametro("PK_HOGAR", _hogarBOL.PK_HOGAR);
                sp.AgregarParametro("RES1", _hogarBOL.RES1);
                sp.AgregarParametro("RES2", _hogarBOL.RES2);
                return sp.EjecutarProcedimiento().Tables[0].Rows[0][0].ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Ingresa datos de migraciones del hogar
        /// </summary>
        public string IngresoHogarMigraciones(HogarBOL _hogarBOL)
        {
            try
            {
                StoredProcedure sp = new StoredProcedure("Pa_Cawi_GesHogarMigraciones_Insertar");
                sp.AgregarParametro("PK_VIVIENDA", _hogarBOL.PK_VIVIENDA);
                sp.AgregarParametro("PK_HOGAR", _hogarBOL.PK_HOGAR);
                sp.AgregarParametro("EMI1", _hogarBOL.EMI1);
                sp.AgregarParametro("EMI2", _hogarBOL.EMI2);
                return sp.EjecutarProcedimiento().Tables[0].Rows[0][0].ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Ingresa datos de personas del hogar
        /// </summary>
        public string IngresoHogarPersonas(HogarBOL _hogarBOL)
        {
            try
            {
                StoredProcedure sp = new StoredProcedure("Pa_Cawi_GesHogarPersonas_Insertar");
                sp.AgregarParametro("PK_VIVIENDA", _hogarBOL.PK_VIVIENDA);
                sp.AgregarParametro("PK_HOGAR", _hogarBOL.PK_HOGAR);
                sp.AgregarParametro("HOG_NOM_PERS", _hogarBOL.HOG_NOM_PERS);
                return sp.EjecutarProcedimiento().Tables[0].Rows[0][0].ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Actualiza datos de personas del hogar
        /// </summary>
        public string EditarHogarPersonas(HogarBOL _hogarBOL, string persona_id)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "UPDATE [CAWICPV_2023].[dbo].[GES_PERSONAS] SET PER_NOMBRE = UPPER(@HOG_NOM_PERS) WHERE [PK_VIVIENDA] = @PK_VIVIENDA AND PK_HOGAR = @PK_HOGAR AND PK_PERSONAS = @PK_PERSONAS " +
                          "SELECT 'ok' ";
                _exQuery.AgregarParametro("PK_VIVIENDA", _hogarBOL.PK_VIVIENDA);
                _exQuery.AgregarParametro("PK_HOGAR", _hogarBOL.PK_HOGAR);
                _exQuery.AgregarParametro("PK_PERSONAS", persona_id);
                _exQuery.AgregarParametro("HOG_NOM_PERS", _hogarBOL.HOG_NOM_PERS);
                return _exQuery.EjecutarConsulta(_strSql).Tables[0].Rows[0][0].ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Ingresa datos de total de personas del hogar
        /// </summary>
        public string IngresoHogarTotalPersonas(HogarBOL _hogarBOL, DataTable dt)
        {
            try
            {
                StoredProcedure sp = new StoredProcedure("Pa_Cawi_GesHogarTotalPersonas_Insertar");
                sp.AgregarParametro("PK_VIVIENDA", _hogarBOL.PK_VIVIENDA);
                sp.AgregarParametro("PK_HOGAR", _hogarBOL.PK_HOGAR);
                sp.AgregarParametro("HOG_TOT_PERS", _hogarBOL.HOG_TOT_PERS);
                sp.AgregarParametro("HOG_TOT_H", _hogarBOL.HOG_TOT_H);
                sp.AgregarParametro("HOG_TOT_M", _hogarBOL.HOG_TOT_M);
                sp.AgregarParametro("HOG_JEFE_HOG", _hogarBOL.HOG_JEFE_HOG);
                sp.AgregarParametro("respuesta", dt);
                return sp.EjecutarProcedimiento().Tables[0].Rows[0][0].ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Lista información del Hogar
        /// </summary>
        public List<T> Listar<T>(HogarBOL _hogarBOL)
        {
            try
            {
                StoredProcedure sp = new StoredProcedure("Pa_Cawi_GesHogar_Listar");
                sp.AgregarParametro("PK_VIVIENDA", _hogarBOL.PK_VIVIENDA);
                sp.AgregarParametro("PK_HOGAR", _hogarBOL.PK_HOGAR);
                return sp.EjecutarProcedimiento<T>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista Total de hogares de la vivienda
        /// </summary>
        public List<T> ListarPorVivienda<T>(HogarBOL _hogarBOL)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT " +
                                "PK_VIVIENDA, " +
                                "PK_HOGAR, " +
                                "H_NHOG, " +
                                "H_NHOG_VISUAL = CONVERT(INT, ROW_NUMBER() OVER (ORDER BY H_NHOG)), " +
                                "HOG_ESTADO = (SELECT CASE (SELECT CASE CONVERT(VARCHAR,COUNT(*)) WHEN '0' THEN '1' ELSE CONVERT(VARCHAR,COUNT(*)) END FROM [CAWICPV_2023].[dbo].[GES_PERSONAS] WHERE PK_VIVIENDA = a.PK_VIVIENDA AND PK_HOGAR = a.PK_HOGAR) WHEN (SELECT CONVERT(VARCHAR,COUNT(*)) FROM [CAWICPV_2023].[dbo].[GES_PERSONAS] WHERE PK_VIVIENDA = a.PK_VIVIENDA AND PK_HOGAR = a.PK_HOGAR AND PER10 = '1') THEN '1' ELSE '0' END) " +
                          " FROM [CAWICPV_2023].[dbo].[GES_HOGAR] AS a WHERE [PK_VIVIENDA] = @PK_VIVIENDA";
                _exQuery.AgregarParametro("PK_VIVIENDA", _hogarBOL.PK_VIVIENDA);
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista personas del hogar
        /// </summary>
        public DataSet ListarPersonasPorHogar(HogarBOL _hogarBOL, bool residentes_habituales)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                if (residentes_habituales == true)
                {
                    _strSql = "SELECT PK_VIVIENDA, PK_HOGAR, PK_PERSONAS, NPER, PER_NOMBRE, PER8 = ISNULL(PER8,'-1'), PER9 = ISNULL(PER9,'0'), PER10 = ISNULL(PER10,'0') FROM [CAWICPV_2023].[dbo].[GES_PERSONAS] WHERE [PK_VIVIENDA] = @PK_VIVIENDA AND [PK_HOGAR] = @PK_HOGAR AND PER8 = 1 ORDER BY PK_PERSONAS DESC";
                } else
                {
                    _strSql = "SELECT PK_VIVIENDA, PK_HOGAR, PK_PERSONAS, NPER, PER_NOMBRE, PER8 = ISNULL(PER8,'-1') FROM [CAWICPV_2023].[dbo].[GES_PERSONAS] WHERE [PK_VIVIENDA] = @PK_VIVIENDA AND [PK_HOGAR] = @PK_HOGAR ORDER BY PK_PERSONAS DESC";
                }              
                _exQuery.AgregarParametro("PK_VIVIENDA", _hogarBOL.PK_VIVIENDA);
                _exQuery.AgregarParametro("PK_HOGAR", _hogarBOL.PK_HOGAR);
                return _exQuery.EjecutarConsulta(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista migraciones del hogar
        /// </summary>
        public List<T> ListarMigracionesPorHogar<T>(PersonaExtBOL _personaExtBOL)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT PK_VIVIENDA, PK_HOGAR, PK_VPE, VPE_NPER, VPE1 = ISNULL(VPE1,''), VPE_COMPLETO = (SELECT COUNT(*) FROM [CAWICPV_2023].[dbo].[GES_VIV_PER_EXT] WHERE PK_VIVIENDA = a.PK_VIVIENDA AND PK_HOGAR = a.PK_HOGAR AND PK_VPE = a.PK_VPE AND (VPE1 IS NULL OR VPE2 IS NULL OR VPE3 IS NULL OR VPE4 IS NULL OR VPE5 IS NULL)) FROM [CAWICPV_2023].[dbo].[GES_VIV_PER_EXT] AS a WHERE [PK_VIVIENDA] = @PK_VIVIENDA AND PK_HOGAR = @PK_HOGAR";
                _exQuery.AgregarParametro("PK_VIVIENDA", _personaExtBOL.PK_VIVIENDA);
                _exQuery.AgregarParametro("PK_HOGAR", _personaExtBOL.PK_HOGAR);
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
