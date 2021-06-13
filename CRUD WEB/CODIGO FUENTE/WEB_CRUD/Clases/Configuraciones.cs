using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace WEB_CRUD.Clases
{
    public static class Configuraciones
    {

        public static int ObtienePaginasReporte()
        {
            return Convert.ToInt32(WebConfigurationManager.AppSettings["PaginasReporte"]);
        }

        public static string ObtieneUsuarioApi()
        {
            return WebConfigurationManager.AppSettings["UsuarioApi"];
        }

        public static string ObtieneContraseñaApi()
        {
            return WebConfigurationManager.AppSettings["PasswordApi"];
        }

        public static string ObtieneUrlToken()
        {
            return WebConfigurationManager.AppSettings["UrlToken"];
        }

        public static string ObtieneUrlReporte()
        {
            return WebConfigurationManager.AppSettings["UrlReporte"];
        }

        public static string ObtieneUsuarioLogin()
        {
            return WebConfigurationManager.AppSettings["UsuarioLogin"];

        }
        public static string ObtieneContraseñaLogin()
        {
            return WebConfigurationManager.AppSettings["PasswordLogin"];
        }
    }
}