using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB_CRUD.Clases;
using WEB_CRUD.Models;

namespace WEB_CRUD.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Login ologin)
        {
            if(!ValidadorDatos.EsEmail(ologin.Correo))
            {
                ViewData["Message"] = "El email no tiene el formato correcto";
                return View();
            }

            if(ologin.Correo.Equals(Configuraciones.ObtieneUsuarioLogin()) &&
                ologin.Contraseña.Equals(Configuraciones.ObtieneContraseñaLogin()))
            {
                //Session["username"] = ologin.Correo;
                return RedirectToAction("Index","PersonasFisicas");
            }
            else
            {
                ViewData["Message"] = "Datos Incorrectos";
                return View();
            }

            return View();
        }
    }
}