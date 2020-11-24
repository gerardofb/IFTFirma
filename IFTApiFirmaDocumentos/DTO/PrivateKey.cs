using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace IFTApiFirmaDocumentos.DTO
{
    public class PrivateKey
    {
        public byte[] KeyBytes { get; set; }
        public string PrivateKeyRSA { get; set; }
        public AsymmetricKeyParameter PrivateKeyParameter { get; set; }
        public string Password { get; set; }

        public void ReadPrivate()
        {
            try
            {
                System.IO.StringWriter stWrite = new System.IO.StringWriter();
                PrivateKeyParameter = PrivateKeyFactory.DecryptKey(Password.ToCharArray(), KeyBytes);
                MemoryStream ms = new MemoryStream();
                TextWriter writer = new StreamWriter(ms);
                Org.BouncyCastle.OpenSsl.PemWriter pmw = new Org.BouncyCastle.OpenSsl.PemWriter(stWrite);
                pmw.WriteObject(PrivateKeyParameter);
                stWrite.Close();
                PrivateKeyRSA = stWrite.ToString();
            }
            catch (Exception exe)
            {
                throw new Exception(Resource.ErrorPassword);
            }
        }
        public string Firma(string cadena)
        {
            ISigner sig = SignerUtilities.GetSigner("SHA256withRSA");
            sig.Init(true, (RsaKeyParameters)PrivateKeyParameter);
            var bytes = Encoding.UTF8.GetBytes(cadena);
            sig.BlockUpdate(bytes, 0, bytes.Length);
            byte[] signature = sig.GenerateSignature();
            var signedString = Convert.ToBase64String(signature);
            return signedString;
        }
        

    }
}