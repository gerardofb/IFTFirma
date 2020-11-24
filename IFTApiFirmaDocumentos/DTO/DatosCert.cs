using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace IFTApiFirmaDocumentos.DTO
{
    public class DatosCert
    {

        private string sujeto_id;
        private string sujeto_nombre;
        private string emisor_nombre;
        private string emisor_id;
        public string Sujeto_Id
        {
            get { return sujeto_id; }
            set
            {
                List<string> Resultados = value.Split(',').ToList();
                List<string> SeparacionRFC = Resultados.Where(u => u.Contains("OID.2.5.4.45")).ToList();
                if (SeparacionRFC.Count > 0)
                {
                    sujeto_id = SeparacionRFC[0].Split('=').ToList()[1];
                }
                else
                {
                    sujeto_id = string.Empty;
                }
            }
        }
        public string Emisor_Id
        {
            get
            {
                return emisor_id;
            }
            set
            {
                List<string> Resultados = value.Split(',').ToList();
                List<string> Separacionautoridad = Resultados.Where(u => u.Contains("2.5.4.45=")).ToList();
                if (Separacionautoridad.Count > 0)
                {
                    emisor_id = Separacionautoridad[0].Split('=').ToList()[1];
                }
                else
                {
                    emisor_id = string.Empty;
                }
            }
        }
        public string Emisor_Nombre
        {
            get
            {
                return emisor_nombre;
            }
            set
            {
                List<string> Resultados = value.Split(',').ToList();
                List<string> Separacionautoridad = Resultados.Where(u => u.Contains("O=")).ToList();
                if (Separacionautoridad.Count > 0)
                {
                    emisor_nombre = Separacionautoridad[0].Split('=').ToList()[1];
                }
                else
                {
                    emisor_nombre = string.Empty;
                }
            }
        }
        public string Sujeto_Nombre
        {
            get { return sujeto_nombre; }
            set
            {
                List<string> Resultados = value.Split(',').ToList();
                List<string> SeparacionNombre = Resultados.Where(u => u.Contains("2.5.4.41=")).ToList();
                if (SeparacionNombre.Count > 0)
                {
                    sujeto_nombre = SeparacionNombre[0].Split('=').ToList()[1];
                }
                else
                {
                    sujeto_nombre = string.Empty;
                }
            }
        }
        public string Serie { get; set; }
        public static DatosCert GetDatosCert(byte[] cer)
        {

            if (cer != null)
            {
                try
                {
                    X509Certificate cert = new X509Certificate();
                    cert.Import(cer);
                    string resultsTrue = cert.ToString(true);
                    List<string> separacion = new List<string>();
                    separacion = resultsTrue.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.None).ToList();
                    DatosCert datos = new DatosCert();
                    datos.Sujeto_Nombre = separacion[0];
                    datos.Sujeto_Id = separacion[0];
                    datos.Emisor_Nombre = separacion[1];
                    datos.Emisor_Id = separacion[1];
                    datos.Serie = Utilerias.ConvertHex(cert.GetSerialNumberString());
                    return datos;
                }
                catch (Exception exe)
                {
                    throw exe;
                }
            }
            else
                throw new Exception("El archivo no se encontro");
        }


    }
}