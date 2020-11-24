using IFTApiFirmaDocumentos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IFTApiFirmaDocumentos.Controllers
{
    public class FirmaController : ApiController
    {
        [Route("Firma")]
        [HttpPost]
        public IHttpActionResult GetCadenaOriginal([FromBody]RequestCadenaOriginal request)
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
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(CadenaOriginal);
        }
    }
}
