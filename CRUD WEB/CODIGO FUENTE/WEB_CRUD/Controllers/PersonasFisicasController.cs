using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WEB_CRUD;
using System.Data.Entity.Core.Objects;
using PagedList;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using WEB_CRUD.Clases;

namespace WEB_CRUD.Controllers
{
    public class PersonasFisicasController : Controller
    {
        private CRUDEntities db = new CRUDEntities();

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
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

            var registros = from s in db.Tb_PersonasFisicas
                            select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                registros = registros.Where(s => s.Nombre.Contains(searchString)
                                       || s.ApellidoMaterno.Contains(searchString)
                                       || s.ApellidoPaterno.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Nombre":
                    registros = registros.OrderByDescending(s => s.Nombre);
                    break;
                case "FechaNacimiento":
                    registros = registros.OrderBy(s => s.FechaNacimiento);
                    break;
                case "FechaRegistro":
                    registros = registros.OrderByDescending(s => s.FechaRegistro);
                    break;
                default:  // Name ascending 
                    registros = registros.OrderBy(s => s.ApellidoPaterno);
                    break;
            }

            int pageSize = Configuraciones.ObtienePaginasReporte();
            int pageNumber = (page ?? 1);
            return View(registros.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ExportToExcel()
        {
            var gv = new GridView();
            gv.DataSource = db.Tb_PersonasFisicas.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Reporte_PersonaFisica.xls");
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

        // GET: Tb_PersonasFisicas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tb_PersonasFisicas tb_PersonasFisicas = db.Tb_PersonasFisicas.Find(id);
            if (tb_PersonasFisicas == null)
            {
                return HttpNotFound();
            }
            return View(tb_PersonasFisicas);
        }

        // GET: Tb_PersonasFisicas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tb_PersonasFisicas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPersonaFisica,FechaRegistro,FechaActualizacion,Nombre,ApellidoPaterno,ApellidoMaterno,RFC,FechaNacimiento,UsuarioAgrega,Activo")] Tb_PersonasFisicas tb_PersonasFisicas)
        {
            if (ModelState.IsValid)
            {
                //Ejecucion de procedimiento almacenado
                ObjectResult<sp_AgregarPersonaFisica_Result> lSPResultado = null;
                sp_AgregarPersonaFisica_Result oSPResultado = new sp_AgregarPersonaFisica_Result();


                if (string.Empty == tb_PersonasFisicas.Nombre ||
                string.Empty == tb_PersonasFisicas.ApellidoPaterno ||
                string.Empty == tb_PersonasFisicas.ApellidoMaterno ||
                string.Empty == tb_PersonasFisicas.RFC)
                {
                    ViewData["Message"] ="Favor de revisar los datos";
                    return View();
                }

                if (tb_PersonasFisicas.FechaNacimiento == null)
                {
                    ViewData["Message"] = "Favor de revisar los datos";
                    return View();
                }

                if (tb_PersonasFisicas.UsuarioAgrega <= 0)
                {
                    ViewData["Message"] = "Favor de revisar los datos";
                    return View();
                }

                //RFC
                if (!ValidadorDatos.EsRFC(tb_PersonasFisicas.RFC))
                {
                    ViewData["Message"] = "El RFC no tiene el formato correcto";
                    return View();
                }

                lSPResultado = db.sp_AgregarPersonaFisica(tb_PersonasFisicas.Nombre,
                    tb_PersonasFisicas.ApellidoPaterno,
                    tb_PersonasFisicas.ApellidoMaterno,
                    tb_PersonasFisicas.RFC,
                    tb_PersonasFisicas.FechaNacimiento,
                    tb_PersonasFisicas.UsuarioAgrega);
                db.SaveChanges();

                oSPResultado = lSPResultado.First();

                if (oSPResultado.ERROR < 0)
                {
                    ViewData["Message"] = string.Concat(oSPResultado.ERROR, " | ", oSPResultado.MENSAJEERROR);
                    return View();
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            return View(tb_PersonasFisicas);
        }

        // GET: Tb_PersonasFisicas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tb_PersonasFisicas tb_PersonasFisicas = db.Tb_PersonasFisicas.Find(id);
            if (tb_PersonasFisicas == null)
            {
                return HttpNotFound();
            }
            return View(tb_PersonasFisicas);
        }

        // POST: Tb_PersonasFisicas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPersonaFisica,FechaRegistro,FechaActualizacion,Nombre,ApellidoPaterno,ApellidoMaterno,RFC,FechaNacimiento,UsuarioAgrega,Activo")] Tb_PersonasFisicas tb_PersonasFisicas)
        {
            if (ModelState.IsValid)
            {
                //Ejecucion de procedimiento almacenado
                ObjectResult<sp_ActualizarPersonaFisica_Result> lSPResultado = null;
                sp_ActualizarPersonaFisica_Result oSPResultado = new sp_ActualizarPersonaFisica_Result();

                if (string.Empty == tb_PersonasFisicas.Nombre ||
               string.Empty == tb_PersonasFisicas.ApellidoPaterno ||
               string.Empty == tb_PersonasFisicas.ApellidoMaterno ||
               string.Empty == tb_PersonasFisicas.RFC)
                {
                    ViewData["Message"] = "Favor de revisar los datos";
                    return View();
                }

                if (tb_PersonasFisicas.FechaNacimiento == null)
                {
                    ViewData["Message"] = "Favor de revisar los datos";
                    return View();
                }

                if (tb_PersonasFisicas.UsuarioAgrega <= 0)
                {
                    ViewData["Message"] = "Favor de revisar los datos";
                    return View();
                }

                //RFC
                if (!ValidadorDatos.EsRFC(tb_PersonasFisicas.RFC))
                {
                    ViewData["Message"] = "El RFC no tiene el formato correcto";
                    return View();
                }

                lSPResultado = db.sp_ActualizarPersonaFisica(tb_PersonasFisicas.IdPersonaFisica,
                    tb_PersonasFisicas.Nombre,
                    tb_PersonasFisicas.ApellidoPaterno,
                    tb_PersonasFisicas.ApellidoMaterno,
                    tb_PersonasFisicas.RFC,
                    tb_PersonasFisicas.FechaNacimiento,
                    tb_PersonasFisicas.UsuarioAgrega);
                db.SaveChanges();

                oSPResultado = lSPResultado.First();

                if (oSPResultado.ERROR < 0)
                {
                    ViewData["Message"] = string.Concat(oSPResultado.ERROR, " | ", oSPResultado.MENSAJEERROR);
                    return View();
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }

            return View(tb_PersonasFisicas);
        }

        // GET: Tb_PersonasFisicas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Tb_PersonasFisicas tb_PersonasFisicas = db.Tb_PersonasFisicas.Find(id);

            if (tb_PersonasFisicas == null)
            {
                return HttpNotFound();
            }
            return View(tb_PersonasFisicas);
        }

        // POST: Tb_PersonasFisicas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Ejecucion de procedimiento almacenado
            ObjectResult<sp_EliminarPersonaFisica_Result> lSPResultado = null;
            sp_EliminarPersonaFisica_Result oSPResultado = new sp_EliminarPersonaFisica_Result();

            lSPResultado = db.sp_EliminarPersonaFisica(id);
            db.SaveChanges();

            oSPResultado = lSPResultado.First();

            if (oSPResultado.ERROR < 0)
            {
                ViewData["Message"] = string.Concat(oSPResultado.ERROR, " | ", oSPResultado.MENSAJEERROR);
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
