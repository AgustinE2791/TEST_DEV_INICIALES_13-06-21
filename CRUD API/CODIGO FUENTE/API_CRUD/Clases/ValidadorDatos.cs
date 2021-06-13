using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace API_CRUD.Clases
{
    public static class ValidadorDatos
    {
        /// <summary>
        /// Valida si la cadena de entrada tiene el formato correcto de RFC
        /// </summary>
        /// <param name="RFC"></param>
        /// <returns></returns>
        public static bool EsRFC(string RFC)
        {
            Regex oRgx = new Regex(@"^([A-ZÑ&]{3,4}) ?(?:- ?)?(\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])) ?(?:- ?)?([A-Z\d]{2})([A\d])$");
            if (oRgx.IsMatch(RFC))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
      
    }
}