using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using WEB_CRUD.Clases;

namespace WEB_CRUD.Controllers
{
    public class ReporteController : Controller
    {
        private CRUDEntities db = new CRUDEntities();

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            RespuestasAPI.ReporteData oReporteData = new RespuestasAPI.ReporteData();
            //Invocar Metodo API
            string token = PuenteReporteApi.ObtenerToken();
            oReporteData = PuenteReporteApi.ObtenerReporte(token);

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Nombre" : "";
            //ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var registros = from s in oReporteData.Data
                            select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                registros = registros.Where(s => s.Nombre.Contains(searchString)
                                       || s.Paterno.Contains(searchString)
                                       || s.Materno.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Nombre":
                    registros = registros.OrderByDescending(s => s.Nombre);
                    break;
                case "Paterno":
                    registros = registros.OrderBy(s => s.Paterno);
                    break;
                case "Materno":
                    registros = registros.OrderByDescending(s => s.Materno);
                    break;
                default:  // Name ascending 
                    registros = registros.OrderBy(s => s.Nombre);
                    break;
            }

            int pageSize = Configuraciones.ObtienePaginasReporte();
            int pageNumber = (page ?? 1);

            return View(registros.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ExportToExcel()
        {
            RespuestasAPI.ReporteData oReporteData = new RespuestasAPI.ReporteData();
            string token = PuenteReporteApi.ObtenerToken();
            oReporteData = PuenteReporteApi.ObtenerReporte(token);

            DataTable dtClientes = new DataTable();
            DataRow drClientes = null;         
                  
            dtClientes.Columns.Add("IdCliente", typeof(string));
            dtClientes.Columns.Add("FechaRegistroEmpresa", typeof(string));
            dtClientes.Columns.Add("RazonSocial", typeof(string));
            dtClientes.Columns.Add("RFC", typeof(string));
            dtClientes.Columns.Add("Sucursal", typeof(string));
            dtClientes.Columns.Add("IdEmpleado", typeof(string));
            dtClientes.Columns.Add("Nombre", typeof(string));
            dtClientes.Columns.Add("Paterno", typeof(string));
            dtClientes.Columns.Add("Materno", typeof(string));
            dtClientes.Columns.Add("IdViaje", typeof(string));


            foreach (Models.Reporte oReporteItem in oReporteData.Data)
            {
                drClientes = dtClientes.NewRow();
                drClientes["IdCliente"] = oReporteItem.IdCliente;
                drClientes["FechaRegistroEmpresa"] = oReporteItem.FechaRegistroEmpresa;
                drClientes["RazonSocial"] = oReporteItem.RazonSocial;
                drClientes["RFC"] = oReporteItem.RFC;
                drClientes["Sucursal"] = oReporteItem.Sucursal;
                drClientes["IdEmpleado"] = oReporteItem.IdEmpleado;
                drClientes["Nombre"] = oReporteItem.Nombre;
                drClientes["Paterno"] = oReporteItem.Paterno;
                drClientes["Materno"] = oReporteItem.Materno;
                drClientes["IdViaje"] = oReporteItem.IdViaje;
                dtClientes.Rows.Add(drClientes);
            }

            var gv = new GridView();
            gv.DataSource = dtClientes;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Reporte_Clientes.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return null;
        }
        private DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

    }
}



