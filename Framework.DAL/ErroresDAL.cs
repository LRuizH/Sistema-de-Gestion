using System;
using System.Collections.Generic;
using System.Text;
using Framework.BOL;

namespace Framework.DAL
{
    public class ErroresDAL
    {
        /// <summary>
        /// Lista Errores Vivienda Hogar
        /// </summary>
        public List<T> ListarErroresViviendaHogar<T>(HogarBOL _hogarBOL, int paso)
        {
            try
            {
                StoredProcedure sp = new StoredProcedure("Pa_Cawi_GesValidacionesViviendaHogar");
                sp.AgregarParametro("PK_VIVIENDA", _hogarBOL.PK_VIVIENDA);
                sp.AgregarParametro("PK_HOGAR", _hogarBOL.PK_HOGAR);
                sp.AgregarParametro("PASO", paso);
                return sp.EjecutarProcedimiento<T>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista Errores Persona
        /// </summary>
        public List<T> ListarErroresPersona<T>(PersonaBOL _personaBOL, int paso)
        {
            try
            {
                StoredProcedure sp = new StoredProcedure("Pa_Cawi_GesValidacionesPersona");
                sp.AgregarParametro("PK_VIVIENDA", _personaBOL.PK_VIVIENDA);
                sp.AgregarParametro("PK_HOGAR", _personaBOL.PK_HOGAR);
                sp.AgregarParametro("PK_PERSONA", _personaBOL.PK_PERSONA);
                sp.AgregarParametro("PASO", paso);
                return sp.EjecutarProcedimiento<T>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
