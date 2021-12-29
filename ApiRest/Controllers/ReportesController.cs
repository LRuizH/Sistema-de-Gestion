using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Framework.BLL;
using Framework.BLL.Vistas;
using Framework.BOL;
using Framework.DAL;
using Framework.BLL.Utilidades;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Framework.BLL.Utilidades.Seguridad;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiRest.Controllers
{
    [Route("api/[controller]")]
    public class ReportesController : Controller
    {

        /// <summary>
        /// Obtiene componente html para vista de reportes de asignaciones
        /// </summary>
        [HttpGet]
        [Route("muestra-reportes-asignacion")]
        public List<ElementoHtml> MuestraReportesAsignacion()
        {
            VReportes _vReportes = new VReportes();

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _vReportes.VSeleccionReportesAsignacion()
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }

        [HttpPost]
        [Route("genera-reportes")]
        public List<ElementoHtml> GeneraReportes()
        {
            GesReportesDAL _gesReportesDAL = new GesReportesDAL();


            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _gesReportesDAL.GeneraInsertReportes()
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };
            return lista;
        }

        /// <summary>
        /// Genera reportes en excel
        /// </summary>
        [HttpGet]
        [Route("export")]
        public List<ElementoHtml> Export(string usu, string llamada, [FromServices] IHostingEnvironment environment)
        {
            AppSettings _appSettings = new AppSettings();
            DataTable dt = new DataTable();

            GesReportesDAL gesReportesDAL = new GesReportesDAL();
            GesReportesBOL gesReportesBOL = new GesReportesBOL();
            DataSet DsReportes = new DataSet();

            // Obtengo usuario de sesión
            GesUsuarioBOL _gesUsuarioBOL = new GesUsuarioBOL();
            GesUsuarioBLL _gesUsuarioBLL = new GesUsuarioBLL();
            _gesUsuarioBOL = _gesUsuarioBLL.ObtieneUsuarioConectado(_appSettings.ObtieneCookie());

            gesReportesBOL.Usu_id = _gesUsuarioBOL.Usu_id;
            gesReportesBOL.LLamada = llamada;
            DsReportes = gesReportesDAL.GeneraReporte(gesReportesBOL);

            dt = DsReportes.Tables[0];

            string nombreReporte = "";
            switch (llamada)
            {
                case "Pa_Rpt_Asignacion":
                    nombreReporte = "REPORTE ASIGNACIÓN";
                    break;
                case "Pa_RPT_Indagacion":
                    nombreReporte = "REPORTE INDAGACIÓN";
                    break;                
            }

            string fechahora = DateTime.Now.ToString();
            string sWebRootFolder = environment.WebRootPath + @"\Documentos\";
            string sFileName = @"" + nombreReporte + ".xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                //ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("" + dt.Rows[1][0] + "");
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Hoja1");
                //First add the headers

                if (dt.Rows.Count > 0)
                {
                    worksheet.Cells["A1"].LoadFromDataTable(dt, true);

                    //Set the row "A" height to 50
                    double rowHeight = 50;
                    worksheet.Row(1).Height = rowHeight;
                    //primera fila en negrita
                    worksheet.Row(1).Style.Font.Bold = true;
                    //centrar valores vertical y horizontal
                    worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Row(1).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //color de primera fila
                    worksheet.Row(1).Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#DDEBF7"));
                    //bordes a encabezados
                    worksheet.Row(1).Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Row(1).Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    worksheet.Row(1).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    worksheet.Row(1).Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    //make all text fit the cells
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                }
                else
                {
                    worksheet.Cells[1, 1].Value = "Resultado";
                    worksheet.Cells["A2"].Value = "No se encontraron registros";
                    worksheet.Cells[1, 1, 1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 1, 1, 1].Style.Font.Size = 16;
                }

                package.Save(); //Save the workbook.
            }

            ElementoHtml _elementoHtml = new ElementoHtml
            {
                Elemento_html = _appSettings.ServidorWeb + "Documentos/" + sFileName
            };

            // Retorno objeto
            List<ElementoHtml> lista = new List<ElementoHtml>
            {
                _elementoHtml
            };

            return lista;
        }
    }
}

