using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace IFTApiFirmaDocumentos.DTO
{
    public class FirmaElectronica
    {
        #region Atributos

        public PrivateKey privatekey { get; set; }
        public Certificate certificate { get; set; }
        //public Tramite tramite { get; set; }

        #endregion

        #region Constructores
        public FirmaElectronica()
        {
            privatekey = new PrivateKey();
            certificate = new Certificate();
        }
        #endregion

        #region MetodosPublicos
        public bool ValidaDatos()
        {
            return privatekey.KeyBytes != null && certificate.CertificateBytes != null && !string.IsNullOrEmpty(privatekey.Password) ? true : false;
        }

        public void Paridad()
        {
            string cadenafirmar = "Instituto Mexicano de la Propiedad Industrial";
            string cadenafirmada = privatekey.Firma(cadenafirmar);
            bool validar = VerificarParidad(cadenafirmar, cadenafirmada);

            if (!validar)
            {
                throw new Exception(Resource.ErrorParidad);
            }

        }

        public bool KeyUsageHasUsage(X509Certificate2 cert, X509KeyUsageFlags flag)
        {
            if (cert.Version < 3) { return true; }
            List<X509KeyUsageExtension> extensions = cert.Extensions.OfType<X509KeyUsageExtension>().ToList();
            if (!extensions.Any())
            {
                return flag != X509KeyUsageFlags.CrlSign && flag != X509KeyUsageFlags.KeyCertSign;
            }
            return (extensions[0].KeyUsages & flag) > 0;
        }

        #endregion

        #region MetodosPrivados
        private bool VerificarParidad(string data, string expectedSignature)
        {
            ISigner signer = SignerUtilities.GetSigner("SHA256withRSA");
            signer.Init(false, certificate.PubliceyParameter);
            var expectedSig = Convert.FromBase64String(expectedSignature);
            var msgBytes = Encoding.UTF8.GetBytes(data);
            signer.BlockUpdate(msgBytes, 0, msgBytes.Length);
            return signer.VerifySignature(expectedSig);
        }
        #endregion

    }
}