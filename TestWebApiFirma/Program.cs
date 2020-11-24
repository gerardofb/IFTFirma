using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Configuration;
using Newtonsoft.Json;
using System.Runtime.Remoting;
using TestWebApiFirma.DTO;

namespace TestWebApiFirma
{
    class Program
    {
        static void Main(string[] args)
        {
            string CadenaOriginal = String.Empty;
            string rutaCer = String.Empty;
            string rutaKey = String.Empty;
            string password = String.Empty;
            Console.WriteLine("Escriba la ruta del archivo .cer");
            rutaCer = Console.ReadLine().Trim();
            Console.WriteLine("Escriba la ruta del archivo .key");
            rutaKey = Console.ReadLine().Trim();
            Console.WriteLine("Escriba la contraseña de la fiel");
            password = Console.ReadLine().Trim();

            try
            {
                CadenaOriginal = GetCadenaOriginal(rutaCer, rutaKey, password).Result;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ocurrió el siguiente error " + ex.Message);
            }
            Console.WriteLine(String.Format("La cadena original del certificado es {0}",CadenaOriginal));
            Console.ReadKey();
        }
        public static async Task<string> GetCadenaOriginal(string rutaCer, string rutaKey,  string Password)
        {
            string CadenaOriginal = String.Empty;
            byte[] datosCer = null;
            byte[] datosKey = null;
            using(FileStream cer = File.Open(rutaCer,FileMode.Open))
            {
                datosCer = new byte[cer.Length];
                cer.Read(datosCer, 0, datosCer.Length);
            }
            using (FileStream key = File.Open(rutaKey, FileMode.Open))
            {
                datosKey = new byte[key.Length];
                key.Read(datosKey, 0, datosKey.Length);
            }
            using(HttpClient Cliente = new HttpClient())
            {
                Cliente.BaseAddress = new Uri(ConfigurationManager.AppSettings["RutaWebApi"]);
                RequestCadenaOriginal peticion = new RequestCadenaOriginal
                {
                    cert = Convert.ToBase64String(datosCer),
                    key = Convert.ToBase64String(datosKey),
                    password = Password
                };
                string ruta = Cliente.BaseAddress + "Firma";
                HttpResponseMessage request = await Cliente.PostAsync(ruta, new StringContent(JsonConvert.SerializeObject(peticion),Encoding.UTF8, "application/json"));
                if (!request.IsSuccessStatusCode)
                {
                    string razon = await request.Content.ReadAsStringAsync();
                    CadenaOriginal = String.Format("{0} {1}", request.ReasonPhrase, razon);
                }
                else
                {
                    CadenaOriginal = await request.Content.ReadAsStringAsync();
                }
            }
            return CadenaOriginal;
        }
    }
}
