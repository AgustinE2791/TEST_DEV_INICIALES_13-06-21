using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using API_CRUD.Clases;
using API_CRUD.Models;
using System.Data.Entity.Core.Objects;

namespace API_CRUD.Controllers
{
    public class PersonasFisicasController : ApiController
    {
        private CRUDEntities db = new CRUDEntities();

        // GET: api/PersonasFisicas
        /// <summary>
        /// Metodo del cual obtienes una lista de personas fisicas
        /// </summary>
        public IQueryable<Tb_PersonasFisicas> Get_PersonasFisicas()
        {
            return db.Tb_PersonasFisicas;
        }

        // GET: api/PersonasFisicas/5
        /// <summary>
        /// Metodo del cual obtienes el registro de una persona fisica por Id
        /// </summary>
        [ResponseType(typeof(Tb_PersonasFisicas))]
        public IHttpActionResult Get_PersonasFisicas(int id)
        {
            #region Validaciones extras

            //ID
            if (!(id > 0))
            {
                return BadRequest("Favor de revisar el Id de la persona física a consultar");
            }
            #endregion

            Tb_PersonasFisicas tb_PersonasFisicas = db.Tb_PersonasFisicas.Find(id);

            if (tb_PersonasFisicas == null)
            {
                return NotFound();
            }

            return Ok(tb_PersonasFisicas);
        }

        // PUT: api/PersonasFisicas/5
        /// <summary>
        /// Metodo del cual actualiza parcial o totalmente los datos de una persona fisica
        /// </summary>
        [ResponseType(typeof(RespuestaAPI))]
        public IHttpActionResult Put_PersonasFisicas(int id, Tb_PersonasFisicas tb_PersonasFisicas)
        {
            RespuestaAPI oRespuestaAPI = new RespuestaAPI();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            #region Validaciones extras

            if (string.Empty == tb_PersonasFisicas.Nombre ||
            string.Empty == tb_PersonasFisicas.ApellidoPaterno ||
            string.Empty == tb_PersonasFisicas.ApellidoMaterno ||
            string.Empty == tb_PersonasFisicas.RFC)
            {
                return BadRequest("Favor de revisar los datos de la peticion");
            }

            if (tb_PersonasFisicas.FechaNacimiento == null)
            {
                return BadRequest("Favor de revisar los datos de la peticion");
            }

            if (tb_PersonasFisicas.UsuarioAgrega <= 0)
            {
                return BadRequest("Favor de revisar los datos de la peticion");
            }

            //ID
            if (!(id > 0))
            {
                return BadRequest("Favor de revisar el Id de la persona física a actualizar");
            }

            //RFC
            if (!ValidadorDatos.EsRFC(tb_PersonasFisicas.RFC))
            {
                return BadRequest("El RFC no tiene el formato correcto, favor de verificar");
            }
            #endregion

            //Ejecucion de procedimiento almacenado
            ObjectResult<sp_ActualizarPersonaFisica_Result> lSPResultado = null;
            sp_ActualizarPersonaFisica_Result oSPResultado = new sp_ActualizarPersonaFisica_Result();

            lSPResultado = db.sp_ActualizarPersonaFisica(id,
                tb_PersonasFisicas.Nombre,
                tb_PersonasFisicas.ApellidoPaterno,
                tb_PersonasFisicas.ApellidoMaterno,
                tb_PersonasFisicas.RFC,
                tb_PersonasFisicas.FechaNacimiento,
                tb_PersonasFisicas.UsuarioAgrega);

            oSPResultado = lSPResultado.First();
            db.SaveChanges();

            if (oSPResultado.ERROR < 0)
            {
                return BadRequest(string.Concat(oSPResultado.ERROR, " | ", oSPResultado.MENSAJEERROR));
            }
            else
            {
                oRespuestaAPI.Error = oSPResultado.ERROR;
                oRespuestaAPI.Mensaje = oSPResultado.MENSAJEERROR;
                return Ok(oRespuestaAPI);
            }
        }

        // POST: api/PersonasFisicas
        /// <summary>
        /// Metodo del cual se crea un registro de una persona fisica
        /// </summary>
        [ResponseType(typeof(RespuestaAPI))]
        public IHttpActionResult Post_PersonasFisicas(Tb_PersonasFisicas tb_PersonasFisicas)
        {
            RespuestaAPI oRespuestaAPI = new RespuestaAPI();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            #region Validaciones extras    

            if (string.Empty == tb_PersonasFisicas.Nombre ||
           string.Empty == tb_PersonasFisicas.ApellidoPaterno ||
           string.Empty == tb_PersonasFisicas.ApellidoMaterno ||
           string.Empty == tb_PersonasFisicas.RFC)
            {
                return BadRequest("Favor de revisar los datos de la peticion");
            }

            if (tb_PersonasFisicas.FechaNacimiento == null)
            {
                return BadRequest("Favor de revisar los datos de la peticion");
            }

            if (tb_PersonasFisicas.UsuarioAgrega <= 0)
            {
                return BadRequest("Favor de revisar los datos de la peticion");
            }

            //RFC
            if (!ValidadorDatos.EsRFC(tb_PersonasFisicas.RFC))
            {
                return BadRequest("El RFC no tiene el formato correcto, favor de verificar");
            }
            #endregion

            //Ejecucion de procedimiento almacenado
            ObjectResult<sp_AgregarPersonaFisica_Result> lSPResultado = null;
            sp_AgregarPersonaFisica_Result oSPResultado = new sp_AgregarPersonaFisica_Result();

            lSPResultado = db.sp_AgregarPersonaFisica(tb_PersonasFisicas.Nombre,
                tb_PersonasFisicas.ApellidoPaterno,
                tb_PersonasFisicas.ApellidoMaterno,
                tb_PersonasFisicas.RFC,
                tb_PersonasFisicas.FechaNacimiento,
                tb_PersonasFisicas.UsuarioAgrega);

            oSPResultado = lSPResultado.First();
            db.SaveChanges();

            if (oSPResultado.ERROR < 0)
            {
                return BadRequest(string.Concat(oSPResultado.ERROR, " | ", oSPResultado.MENSAJEERROR));
            }
            else
            {
                oRespuestaAPI.Error = oSPResultado.ERROR;
                oRespuestaAPI.Mensaje = oSPResultado.MENSAJEERROR;
                return Ok(oRespuestaAPI);
            }
        }

        // DELETE: api/PersonasFisicas/5
        /// <summary>
        /// Metodo del cual se elimina un registro de una persona fisica
        /// </summary>
        [ResponseType(typeof(RespuestaAPI))]
        public IHttpActionResult Delete_PersonasFisicas(int id)
        {
            RespuestaAPI oRespuestaAPI = new RespuestaAPI();

            #region Validaciones extras
            //ID
            if (!(id > 0))
            {
                return BadRequest("Favor de revisar el Id de la persona física a eliminar");
            }
            #endregion

            Tb_PersonasFisicas tb_PersonasFisicas = db.Tb_PersonasFisicas.Find(id);

            if (tb_PersonasFisicas == null)
            {
                return NotFound();
            }

            //Ejecucion de procedimiento almacenado
            ObjectResult<sp_EliminarPersonaFisica_Result> lSPResultado = null;
            sp_EliminarPersonaFisica_Result oSPResultado = new sp_EliminarPersonaFisica_Result();

            lSPResultado = db.sp_EliminarPersonaFisica(tb_PersonasFisicas.IdPersonaFisica);
            db.SaveChanges();

            oSPResultado = lSPResultado.First();

            if (oSPResultado.ERROR < 0)
            {
                return BadRequest(string.Concat(oSPResultado.ERROR, " | ", oSPResultado.MENSAJEERROR));
            }
            else
            {
                oRespuestaAPI.Error = oSPResultado.ERROR;
                oRespuestaAPI.Mensaje = oSPResultado.MENSAJEERROR;
                return Ok(oRespuestaAPI);
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

        private bool PersonasFisicasExists(int id)
        {
            return db.Tb_PersonasFisicas.Count(e => e.IdPersonaFisica == id) > 0;
        }
    }
}