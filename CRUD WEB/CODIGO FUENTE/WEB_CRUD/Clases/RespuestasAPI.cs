using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WEB_CRUD.Models;

namespace WEB_CRUD.Clases
{
    public class RespuestasAPI
    {
        public class TokenData
        {
            public string Data;
        }

        public class ReporteData
        {
            public List<Reporte> Data;
        }
    }
}