using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WEB_CRUD.Models
{
    public class Login
    {
        [Required (ErrorMessage = "Por favor ingresa tu correo")]
        [Display (Name ="Correo: ")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "Por favor ingresa tu contraseña")]
        [Display(Name = "Contraseña: ")]
        public string Contraseña { get; set; }
    }
}