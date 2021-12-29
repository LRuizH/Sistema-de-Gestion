using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Framework.BOL;
using Framework.DAL;

namespace Framework.BLL
{
    public class HogarBLL
    {
        /// <summary>
        /// Elimina hogar
        /// </summary>
        public string EliminarHogar(string token)
        {
            string _strHtml = "";
            StringBuilder sb = new StringBuilder();
            try
            {
                // Obtengo identificación del registro
                IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
                _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

                // Ingreso datos de la tabla en clase de HogarBOL
                HogarBOL _hogarBOL = new HogarBOL();
                HogarDAL _hogarDAL = new HogarDAL();

                _hogarBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
                _hogarBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;

                _strHtml = _hogarDAL.EliminarHogar(_hogarBOL);
            }
            catch (Exception ex)
            {
                _strHtml = ex.Message;
            }

            return _strHtml;
        }

        /// <summary>
        /// Inserta o actualiza información de Hogar Tenencia
        /// </summary>
        public string IngresoHogarTenencia(string formData)
        {
            string _strHtml = "";
            StringBuilder sb = new StringBuilder();
            try
            {
                // Recibo objeto y lo convierto en tabla
                string objeto = "[" + JObject.Parse(formData).ToString() + "]";

                // UnPivote Datatable
                DataTable DtDatos = (DataTable)JsonConvert.DeserializeObject(objeto, (typeof(DataTable)));

                // Obtengo identificación del registro
                IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
                _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(DtDatos.Rows[0]["idFormulario"].ToString());

                // Ingreso datos de la tabla en clase de HogarBOL
                HogarBOL _hogarBOL = new HogarBOL();
                HogarDAL _hogarDAL = new HogarDAL();

                _hogarBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
                _hogarBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;
                _hogarBOL.TEN1 = DtDatos.Rows[0]["TEN1"].ToString();
                _hogarBOL.TEN2 = DtDatos.Rows[0]["TEN2"].ToString();
                _hogarBOL.TEN3 = DtDatos.Rows[0]["TEN3"].ToString();

                _strHtml = _hogarDAL.IngresoHogarTenencia(_hogarBOL);
            }
            catch (Exception ex)
            {
                _strHtml = ex.Message;
            }

            return _strHtml;
        }

        /// <summary>
        /// Ingresa datos de Fuente de energía del hogar
        /// </summary>
        public string IngresoHogarFuenteEnergia(string formData)
        {
            string _strHtml = "";
            StringBuilder sb = new StringBuilder();
            try
            {
                // Recibo objeto y lo convierto en tabla
                string objeto = "[" + JObject.Parse(formData).ToString() + "]";

                // UnPivote Datatable
                DataTable DtDatos = (DataTable)JsonConvert.DeserializeObject(objeto, (typeof(DataTable)));

                // Obtengo identificación del registro
                IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
                _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(DtDatos.Rows[0]["idFormulario"].ToString());

                // Ingreso datos de la tabla en clase de HogarBOL
                HogarBOL _hogarBOL = new HogarBOL();
                HogarDAL _hogarDAL = new HogarDAL();

                _hogarBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
                _hogarBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;
                _hogarBOL.ENE1 = DtDatos.Rows[0]["ENE1"].ToString();
                _hogarBOL.ENE2 = DtDatos.Rows[0]["ENE2"].ToString();
                _hogarBOL.ENE3 = DtDatos.Rows[0]["ENE3"].ToString();
                _hogarBOL.ENE4 = DtDatos.Rows[0]["ENE4"].ToString();
                _hogarBOL.ENE5 = DtDatos.Rows[0]["ENE5"].ToString();
                _hogarBOL.ENE6 = DtDatos.Rows[0]["ENE6"].ToString();
                _hogarBOL.ENE7 = DtDatos.Rows[0]["ENE7"].ToString();
                _hogarBOL.ENE8 = DtDatos.Rows[0]["ENE8"].ToString();
                _hogarBOL.ENE9 = DtDatos.Rows[0]["ENE9"].ToString();
                _hogarBOL.ENE10 = DtDatos.Rows[0]["ENE10"].ToString();
                _hogarBOL.ENE11 = DtDatos.Rows[0]["ENE11"].ToString();
                _hogarBOL.ENE12 = DtDatos.Rows[0]["ENE12"].ToString();

                _strHtml = _hogarDAL.IngresoHogarFuenteEnergia(_hogarBOL);
            }
            catch (Exception ex)
            {
                _strHtml = ex.Message;
            }

            return _strHtml;
        }

        /// <summary>
        /// Ingresa datos de Gestión de residuos del hogar
        /// </summary>
        public string IngresoHogarGestionResiduos(string formData)
        {
            string _strHtml = "";
            StringBuilder sb = new StringBuilder();
            try
            {
                // Recibo objeto y lo convierto en tabla
                string objeto = "[" + JObject.Parse(formData).ToString() + "]";

                // UnPivote Datatable
                DataTable DtDatos = (DataTable)JsonConvert.DeserializeObject(objeto, (typeof(DataTable)));

                // Obtengo identificación del registro
                IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
                _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(DtDatos.Rows[0]["idFormulario"].ToString());

                // Ingreso datos de la tabla en clase de HogarBOL
                HogarBOL _hogarBOL = new HogarBOL();
                HogarDAL _hogarDAL = new HogarDAL();

                _hogarBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
                _hogarBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;
                _hogarBOL.RES1 = DtDatos.Rows[0]["RES1"].ToString();
                _hogarBOL.RES2 = DtDatos.Rows[0]["RES2"].ToString();

                _strHtml = _hogarDAL.IngresoHogarGestionResiduos(_hogarBOL);
            }
            catch (Exception ex)
            {
                _strHtml = ex.Message;
            }

            return _strHtml;
        }

        /// <summary>
        /// Ingresa datos de migraciones del hogar
        /// </summary>
        public string IngresoHogarMigraciones(string formData)
        {
            string _strHtml = "";
            StringBuilder sb = new StringBuilder();
            try
            {
                // Recibo objeto y lo convierto en tabla
                string objeto = "[" + JObject.Parse(formData).ToString() + "]";

                // UnPivote Datatable
                DataTable DtDatos = (DataTable)JsonConvert.DeserializeObject(objeto, (typeof(DataTable)));

                // Obtengo identificación del registro
                IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
                _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(DtDatos.Rows[0]["idFormulario"].ToString());

                // Ingreso datos de la tabla en clase de HogarBOL
                HogarBOL _hogarBOL = new HogarBOL();
                HogarDAL _hogarDAL = new HogarDAL();

                _hogarBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
                _hogarBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;
                _hogarBOL.EMI1 = DtDatos.Rows[0]["EMI1"].ToString();
                _hogarBOL.EMI2 = DtDatos.Rows[0]["EMI2"].ToString();

                _strHtml = _hogarDAL.IngresoHogarMigraciones(_hogarBOL);
            }
            catch (Exception ex)
            {
                _strHtml = ex.Message;
            }

            return _strHtml;
        }

        /// <summary>
        /// Ingresa datos de personas del hogar
        /// </summary>
        public string IngresoHogarPersonas(string formData)
        {
            string _strHtml = "";
            StringBuilder sb = new StringBuilder();
            try
            {
                // Recibo objeto y lo convierto en tabla
                string objeto = "[" + JObject.Parse(formData).ToString() + "]";

                // UnPivote Datatable
                DataTable DtDatos = (DataTable)JsonConvert.DeserializeObject(objeto, (typeof(DataTable)));

                // Obtengo identificación del registro
                IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
                _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(DtDatos.Rows[0]["idFormulario"].ToString());

                // Ingreso datos de la tabla en clase de HogarBOL
                HogarBOL _hogarBOL = new HogarBOL();
                HogarDAL _hogarDAL = new HogarDAL();

                _hogarBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
                _hogarBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;
                _hogarBOL.HOG_NOM_PERS = DtDatos.Rows[0]["HOG_NOM_PERS"].ToString();

                _strHtml = _hogarDAL.IngresoHogarPersonas(_hogarBOL);
            }
            catch (Exception ex)
            {
                _strHtml = ex.Message;
            }

            return _strHtml;
        }

        /// <summary>
        /// Edita datos de personas del hogar
        /// </summary>
        public string EditarHogarPersonas(string formData)
        {
            string _strHtml = "";
            StringBuilder sb = new StringBuilder();
            try
            {
                // Recibo objeto y lo convierto en tabla
                string objeto = "[" + JObject.Parse(formData).ToString() + "]";

                // UnPivote Datatable
                DataTable DtDatos = (DataTable)JsonConvert.DeserializeObject(objeto, (typeof(DataTable)));

                // Obtengo identificación del registro
                IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
                _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(DtDatos.Rows[0]["idFormulario"].ToString());

                // Ingreso datos de la tabla en clase de HogarBOL
                HogarBOL _hogarBOL = new HogarBOL();
                HogarDAL _hogarDAL = new HogarDAL();

                _hogarBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
                _hogarBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;
                _hogarBOL.HOG_NOM_PERS = DtDatos.Rows[0]["HOG_NOM_PERS"].ToString();

                _strHtml = _hogarDAL.EditarHogarPersonas(_hogarBOL, DtDatos.Rows[0]["persona_id"].ToString());
            }
            catch (Exception ex)
            {
                _strHtml = ex.Message;
            }

            return _strHtml;
        }

        /// <summary>
        /// Elimina persona
        /// </summary>
        public string EliminarPersona(string token, string persona_id)
        {
            string _strHtml = "";
            StringBuilder sb = new StringBuilder();
            try
            {
                // Obtengo identificación del registro
                IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
                _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(token);

                // Ingreso datos de la tabla en clase de HogarBOL
                PersonaBOL _personaBOL = new PersonaBOL();
                PersonaDAL _personaDAL = new PersonaDAL();

                _personaBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
                _personaBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;
                _personaBOL.PK_PERSONA = persona_id;

                _strHtml = _personaDAL.EliminarPersona(_personaBOL);
            }
            catch (Exception ex)
            {
                _strHtml = ex.Message;
            }

            return _strHtml;
        }

        /// <summary>
        /// Ingresa datos de total de personas del hogar
        /// </summary>
        public string IngresoHogarTotalPersonas(string formData)
        {
            string _strHtml = "";
            StringBuilder sb = new StringBuilder();
            try
            {
                // Recibo objeto y lo convierto en tabla
                string objeto = "[" + JObject.Parse(formData).ToString() + "]";

                // UnPivote Datatable
                DataTable DtDatos = (DataTable)JsonConvert.DeserializeObject(objeto, (typeof(DataTable)));

                // Obtengo en formato hacia abajo
                DataTable DtFormulario = new DataTable();
                for (int i = 0; i <= DtDatos.Rows.Count; i++)
                {
                    DtFormulario.Columns.Add();
                }
                for (int i = 0; i < DtDatos.Columns.Count; i++)
                {
                    DtFormulario.Rows.Add();
                    DtFormulario.Rows[i][0] = DtDatos.Columns[i].ColumnName;
                }
                for (int i = 0; i < DtDatos.Columns.Count; i++)
                {
                    for (int j = 0; j < DtDatos.Rows.Count; j++)
                    {
                        DtFormulario.Rows[i][j + 1] = DtDatos.Rows[j][i];
                    }
                }

                // Obtengo identificación del registro
                IdentificadorCuestionario _identificadorCuestionario = new IdentificadorCuestionario();
                _identificadorCuestionario = _identificadorCuestionario.ObtieneIdentificacionVHP(DtDatos.Rows[0]["idFormulario"].ToString());

                // Ingreso datos de la tabla en clase de HogarBOL
                HogarBOL _hogarBOL = new HogarBOL();
                HogarDAL _hogarDAL = new HogarDAL();

                _hogarBOL.PK_VIVIENDA = _identificadorCuestionario.IdVivienda;
                _hogarBOL.PK_HOGAR = _identificadorCuestionario.IdHogar;
                _hogarBOL.HOG_TOT_PERS = int.Parse(DtDatos.Rows[0]["HOG_TOT_PERS"].ToString());
                _hogarBOL.HOG_TOT_H = int.Parse(DtDatos.Rows[0]["HOG_TOT_H"].ToString());
                _hogarBOL.HOG_TOT_M = int.Parse(DtDatos.Rows[0]["HOG_TOT_M"].ToString());
                _hogarBOL.HOG_JEFE_HOG = DtDatos.Rows[0]["HOG_JEFE_HOG"].ToString();

                _strHtml = _hogarDAL.IngresoHogarTotalPersonas(_hogarBOL, DtFormulario);
            }
            catch (Exception ex)
            {
                _strHtml = ex.Message;
            }

            return _strHtml;
        }
    }
}
