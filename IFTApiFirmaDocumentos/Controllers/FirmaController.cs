using IFTApiFirmaDocumentos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Security.Cryptography;
using System.Web.Http;

namespace IFTApiFirmaDocumentos.Controllers
{
    public class FirmaController : ApiController
    {
        [Route("Firma")]
        [HttpPost]
        public IHttpActionResult GetCadenaOriginal([FromBody] RequestCadenaOriginal request)
        {
            string CadenaOriginal = String.Empty;
            try
            {
                byte[] bytesCertificado = Convert.FromBase64String(request.cert);
                byte[] bytesLlave = Convert.FromBase64String(request.key);
                FirmaDigital Firma = new FirmaDigital(bytesCertificado, bytesLlave, request.password);
                CadenaOriginal = String.Format("{0}|{1}|{2}|{3}", Firma.GetDatosNombre(), Firma.GetDatosRfc(),
                    Firma.GetDatosSerie(), Firma.GetDatosAutoridad());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(CadenaOriginal);
        }
        [Route("ObtenerHash")]
        [HttpPost]
        public IHttpActionResult GetHashPorArchivo([FromBody] RequestHashArchivo request)
        {
            string sellodigital = String.Empty;
            byte[] bytesFirmados = null;
            try
            {
                byte[] ClavePrivada = Convert.FromBase64String(request.claveprivada);
                byte[] bArchivo = Convert.FromBase64String(request.archivo);

                SecureString lSecStr = new SecureString();
                SHA256Managed sham = new SHA256Managed();
                lSecStr.Clear();
                foreach (char c in request.password.ToCharArray())
                    lSecStr.AppendChar(c);

                RSACryptoServiceProvider lrsa = OpenSSLKey.DecodeEncryptedPrivateKeyInfo(ClavePrivada, lSecStr);
                bytesFirmados = lrsa.SignData(bArchivo, sham);

            }
            catch (Exception ex)
            {
                sellodigital = "Clave privada incorrecta";
                return BadRequest(sellodigital);
            }
            sellodigital = Convert.ToBase64String(bytesFirmados);
            return Ok(sellodigital);

        }
    }
}
