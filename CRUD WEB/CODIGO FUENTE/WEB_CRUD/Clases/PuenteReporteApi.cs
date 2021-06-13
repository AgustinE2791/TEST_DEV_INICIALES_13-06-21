
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace WEB_CRUD.Clases
{
    public static class PuenteReporteApi
    {
        /// <summary>
        /// Obtener token autenticacion JWT
        /// </summary>
        /// <returns></returns>
        public static string ObtenerToken()
        {            
            RespuestasAPI.TokenData oTokenData = new RespuestasAPI.TokenData();

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(Configuraciones.ObtieneUrlToken());
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"Username\":\"" + Configuraciones.ObtieneUsuarioApi() + "\"," +
                              "\"Password\":\"" + Configuraciones.ObtieneContraseñaApi() + "\"}";

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                string resultado = streamReader.ReadToEnd();                               
                oTokenData = JsonConvert.DeserializeObject<RespuestasAPI.TokenData>(resultado);
            }

            return oTokenData.Data;
        }

        /// <summary>
        /// Obtener informacion del metodo de reportes de clientes
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static RespuestasAPI.ReporteData ObtenerReporte(string token)
        {
            RespuestasAPI.ReporteData oReporteData = new RespuestasAPI.ReporteData();

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(Configuraciones.ObtieneUrlReporte());
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + token);           

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                string resultado = streamReader.ReadToEnd();
                oReporteData = JsonConvert.DeserializeObject<RespuestasAPI.ReporteData>(resultado);
            }

            return oReporteData;
        }
    }
}