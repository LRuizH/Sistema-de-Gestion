using System;
using System.Collections.Generic;
using System.Text;
using Framework.BOL;

namespace Framework.DAL
{
    public class GesFormPreguntasOpcionesDAL
    {
        string _strSql = "";

        /// <summary>
        /// Permite listar opciones de preguntas
        /// </summary>
        public List<T> ObtieneOpcionesPreguntaPorGrupos<T>(string _preguntas)
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "select * from(SELECT Pk_form_preguntas = o.PK_FORM_PREGUNTAS, Gpf_codigo_pregunta = GFP_CODIGO_PREGUNTA, Fpo_numero = FPO_NUMERO, Fpo_glosa_primaria = FPO_GLOSA_PRIMARIA,FPO_ORDEN FROM [CAWICPV_2023].dbo.GES_FORM_PREGUNTAS_OPCIONES AS o INNER JOIN  [CAWICPV_2023].dbo.GES_FORM_PREGUNTAS AS p ON p.PK_FORM_PREGUNTAS = o.PK_FORM_PREGUNTAS  " +
                            "union all " +
                            "select " +
                            "[Pk_form_preguntas] = 999, " +
                            "[Gpf_codigo_pregunta] = 'COM', " +
                            "[Fpo_numero] = Codigo, " +
                            "[Fpo_glosa_primaria] = Nombre, " +
                            "posicion " +
                            "from[dbo].[GLO_GEOGRAFIA] where IdGeografiaNivel = 3 " +
                            "union all " +
                            "select " +
                            "[Pk_form_preguntas] = 888, " +
                            "[Gpf_codigo_pregunta] = 'PAIS', " +
                            "[Fpo_numero] = Codigo, " +
                            "[Fpo_glosa_primaria] = Nombre, " +
                            "posicion " +
                            "from[dbo].[GLO_PAIS] " +
                            ") as listas " +    
                            " WHERE CONVERT(VARCHAR,PK_FORM_PREGUNTAS) IN (" + _preguntas + ") ORDER BY PK_FORM_PREGUNTAS,FPO_ORDEN ASC";
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Permite listar opciones de preguntas
        /// </summary>
        public List<T> ObtieneOpcionesMotivoLlamada<T>()
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "select Pk_form_preguntas = 0, Gpf_codigo_pregunta = 'MOT', Fpo_numero = IdMotivoLlamada, Fpo_glosa_primaria = Nombre "+
                          "from GLO_MOTIVO_LLAMADA_CATI ORDER BY 1";
                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<T> ObtieneOpcionesCategoriaMotivoLlamada<T>()
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT   DISTINCT IdMotivoLLamada AS Pk_form_preguntas, " +
                          "(SELECT  CASE WHEN IdMotivoLLamada = 1 THEN 'CAT1' END) AS Gpf_codigo_pregunta, " +
                          "idcategoria as Fpo_numero, Categoria AS Fpo_glosa_primaria " +
                          "from     GLO_SUB_CAT_MOTIVO_LLAMADA " +
                          "where IdMotivoLLamada in(1) " +
                          "UNION ALL " +
                          "SELECT   DISTINCT IdMotivoLLamada AS Pk_form_preguntas, " +
                          "(SELECT	CASE WHEN IdMotivoLLamada = '2' THEN 'CAT2' END) AS Gpf_codigo_pregunta, " +
                          "IdSubCategoria as Fpo_numero, SubCategoria AS Fpo_glosa_primaria " +
                          "from     GLO_SUB_CAT_MOTIVO_LLAMADA " +
                          "where IdMotivoLLamada in(2) " +
                          "ORDER BY 1,3  ";

                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<T> ObtieneOpcionesInformacionGeneral<T>()
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT   DISTINCT IdMotivoLLamada AS Pk_form_preguntas, " +
                          "(SELECT	CASE WHEN IdCategoria = '1' THEN 'CAT1' WHEN IdCategoria = '2' THEN 'CAT2' END) AS Gpf_codigo_pregunta, " +
                          "IdSubCategoria as Fpo_numero, SubCategoria AS Fpo_glosa_primaria " +
                          "from     GLO_SUB_CAT_MOTIVO_LLAMADA " +
                          "where IdMotivoLLamada in(1) " +
                          "ORDER BY 1,3  ";

                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {   
                throw new Exception(ex.Message);
            }
        }

        public List<T> ObtieneResultadoConsulta<T>()
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "SELECT   DISTINCT IdMotivoLLamada AS Pk_form_preguntas, " +
                          "(SELECT	CASE WHEN IdCategoria = '1' THEN 'RES' END) AS Gpf_codigo_pregunta, " +
                          "IdSubCategoria as Fpo_numero, SubCategoria AS Fpo_glosa_primaria " +
                          "from     GLO_SUB_CAT_MOTIVO_LLAMADA " +
                          "where IdMotivoLLamada in(4) " +
                          "ORDER BY 1,3  ";

                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<T> ObtieneSiNo<T>()
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "select Pk_form_preguntas = 0, Gpf_codigo_pregunta = 'SINO', Fpo_numero = Orden, Fpo_glosa_primaria = Glosa " +
                          "from glo_hoja_ruta " +
                          "where Id in (6) " +
                          "ORDER BY Orden ";

                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<T> ObtienePreguntasCierreCuestionario<T>()
        {
            try
            {
                StoredProcedure _exQuery = new StoredProcedure();
                _strSql = "select Pk_form_preguntas = id, " +
                          " (SELECT	CASE WHEN Id = '1' THEN 'RAZ_ENT' WHEN Id = 2 then 'SIN_ENTR' when id = 3 then 'RAZ_REC' when id = 4 then 'RAZ_CUES' when id = 5 then 'NO_CONT' when id = 6 then 'SINO' END) AS Gpf_codigo_pregunta, " +
                          "Fpo_numero = Orden, Fpo_glosa_primaria = Glosa " +
                          "from glo_hoja_ruta " +
                          "ORDER BY id, Orden ";

                return _exQuery.EjecutarConsulta<T>(_strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
