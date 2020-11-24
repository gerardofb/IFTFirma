using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Security;
using System.IO;

namespace IFTApiFirmaDocumentos.DTO
{
    public class Certificate
    {
        public byte[] CertificateBytes { get; set; }
        public System.Security.Cryptography.X509Certificates.X509Certificate2 Certificado { get; set; }
        public Org.BouncyCastle.Crypto.AsymmetricKeyParameter PubliceyParameter { get; set; }
        public string PublicKeyRSA { get; set; }
        public DatosCert Datos { get; set; }

        private string CERT_STORE_2012 = "-----BEGIN CERTIFICATE-----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-----END CERTIFICATE-----";
        private string CERT_STORE_2013 = "-----BEGIN CERTIFICATE-----MIIHHTCCBamgAwIBAgIUMDAwMDAwMDAwMDAwMDAwMDEwNzAwDQYJKoZIhvcNAQEFBQAwgbcxCzAJBgNVBAYTAk1YMRkwFwYDVQQIDBBEaXN0cml0byBGZWRlcmFsMRMwEQYDVQQHDApDdWF1aHRlbW9jMRgwFgYDVQQKDA9CYW5jbyBkZSBNZXhpY28xJTAjBgNVBAMMHEFnZW5jaWEgUmVnaXN0cmFkb3JhIENlbnRyYWwxNzA1BgkqhkiG9w0BCQIMKFJlc3BvbnNhYmxlIEpvc2UgQW50b25pbyBIZXJuYW5kZXogQXl1c28wHhcNMTMwNDI5MTY0MTU2WhcNMjEwNDI5MTY0MTU2WjCCAYoxODA2BgNVBAMML0EuQy4gZGVsIFNlcnZpY2lvIGRlIEFkbWluaXN0cmFjacOzbiBUcmlidXRhcmlhMS8wLQYDVQQKDCZTZXJ2aWNpbyBkZSBBZG1pbmlzdHJhY2nDs24gVHJpYnV0YXJpYTE4MDYGA1UECwwvQWRtaW5pc3RyYWNpw7NuIGRlIFNlZ3VyaWRhZCBkZSBsYSBJbmZvcm1hY2nDs24xHzAdBgkqhkiG9w0BCQEWEGFjb2RzQHNhdC5nb2IubXgxJjAkBgNVBAkMHUF2LiBIaWRhbGdvIDc3LCBDb2wuIEd1ZXJyZXJvMQ4wDAYDVQQRDAUwNjMwMDELMAkGA1UEBhMCTVgxGTAXBgNVBAgMEERpc3RyaXRvIEZlZGVyYWwxFDASBgNVBAcMC0N1YXVodMOpbW9jMRUwEwYDVQQtEwxTQVQ5NzA3MDFOTjMxNTAzBgkqhkiG9w0BCQIMJlJlc3BvbnNhYmxlOiBDbGF1ZGlhIENvdmFycnViaWFzIE9jaG9hMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA4Vwy/gl8pY/dyJJLPa6U3f0rqGyHtb6eG1AvI/R6nB4qXuGrcXB9lGpJ21aBSD1RyvEN/cS5GvDQUM+Gzkv1+og3TZthFs/FfInW/GuqFexStJXMd/NsypgOdBJOJxj68WrbWwyhT9yl271bx8GPipuuB3dA4c0rSip51btH2fIBRFWeDA1cDudawIhgy3Z90qFF1P/rWNno7+LJ35LzB7+SZg5kPE4RFs8a4NdWs9TI2Eei/JhAS6rz3g5BDIkJEGLdYnfF67hJr2RO1BQz/Yl4aEOAyEafKSEgkzBJqT6NeZR43VKPMTyRHHbaybVYCIVQkHzJKKk58aZPiYa8NwIDAQABo4IB7TCCAekwHQYDVR0OBBYEFLwp6I5rvjE2Z4XGN6+cAvQh0kzMMIH3BgNVHSMEge8wgeyAFFVTm6DD4wZ+0VZAg6F/fvdfRFl3oYG9pIG6MIG3MQswCQYDVQQGEwJNWDEZMBcGA1UECAwQRGlzdHJpdG8gRmVkZXJhbDETMBEGA1UEBwwKQ3VhdWh0ZW1vYzEYMBYGA1UECgwPQmFuY28gZGUgTWV4aWNvMSUwIwYDVQQDDBxBZ2VuY2lhIFJlZ2lzdHJhZG9yYSBDZW50cmFsMTcwNQYJKoZIhvcNAQkCDChSZXNwb25zYWJsZSBKb3NlIEFudG9uaW8gSGVybmFuZGV6IEF5dXNvghQwMDAwMDAwMDAwMDAwMDAwMDAwMjAPBgNVHRMECDAGAQH/AgEAMAsGA1UdDwQEAwIB/jA0BgNVHSUELTArBggrBgEFBQcDAgYIKwYBBQUHAwgGCWCGSAGG+EIEAQYKKwYBBAGCNwoDAzA7BggrBgEFBQcBAQQvMC0wKwYIKwYBBQUHMAGGH2h0dHBzOi8vY2ZkaS5zYXQuZ29iLm14L2Vkb2ZpZWwwKgYDVR0fBCMwITAfoB2gG4YZaHR0cDovL3d3dy5zYXQuZ29iLm14L2NybDARBglghkgBhvhCAQEEBAMCAQYwDQYJKoZIhvcNAQEFBQADggFdAERVuw31vCU+8hGGhcg705M+jdfnJMcf456xSG/ysoF9AJ1Vt5EZPtt4kgEgC6I9wJQmdP/9MO8j9OZJRuXgvIWiE6AFuxqFQWCMLSAntXYe9iMdjbGRZZWRi1Jjvjs3u5wKYSLty5OIOM72k52FkSvrZAEQzJ95oCRFnQO5ArUfbqkX7eqG7E70ouXVc62YD6bsxnyxsfXYWEt+m6kcRZQK0mrtykcyW50CaRdVKREeruhgK4rzsbqGu+8I/xOBhJ03zSqsfqLGOA1WGH4ZK2oguaIauNCVQOwmSrDAbB5DUg07ibhr4Br2qCtbfBv0UaiHy2ug66f8z+c8pzGS1RdixmUQ4exVTewrchVSZUchyeEaBN/mOyLWuiJ1MEDUV1bPglZUHP2NY4fyOtfhtSyvpjHEHBv4rpjG7KgsCI1o2G8SwbBMXjO3AuAVgzgpFWzXNnzQrtr9Rlq5aw==-----END CERTIFICATE-----";
        private string CERT_STORE_2015 = "-----BEGIN CERTIFICATE-----MIIHmTCCBiWgAwIBAgIUMDAwMDAwMDAwMDAwMDAwMDEwODMwDQYJKoZIhvcNAQELBQAwgbcxCzAJBgNVBAYTAk1YMRkwFwYDVQQIDBBEaXN0cml0byBGZWRlcmFsMRMwEQYDVQQHDApDdWF1aHRlbW9jMRgwFgYDVQQKDA9CYW5jbyBkZSBNZXhpY28xJTAjBgNVBAMMHEFnZW5jaWEgUmVnaXN0cmFkb3JhIENlbnRyYWwxNzA1BgkqhkiG9w0BCQIMKFJlc3BvbnNhYmxlIEpvc2UgQW50b25pbyBIZXJuYW5kZXogQXl1c28wHhcNMTUwNTI1MTgwNDIwWhcNMjMwNTI1MTgwNDIwWjCCAbIxODA2BgNVBAMML0EuQy4gZGVsIFNlcnZpY2lvIGRlIEFkbWluaXN0cmFjacOzbiBUcmlidXRhcmlhMS8wLQYDVQQKDCZTZXJ2aWNpbyBkZSBBZG1pbmlzdHJhY2nDs24gVHJpYnV0YXJpYTE4MDYGA1UECwwvQWRtaW5pc3RyYWNpw7NuIGRlIFNlZ3VyaWRhZCBkZSBsYSBJbmZvcm1hY2nDs24xHzAdBgkqhkiG9w0BCQEWEGFjb2RzQHNhdC5nb2IubXgxJjAkBgNVBAkMHUF2LiBIaWRhbGdvIDc3LCBDb2wuIEd1ZXJyZXJvMQ4wDAYDVQQRDAUwNjMwMDELMAkGA1UEBhMCTVgxGTAXBgNVBAgMEERpc3RyaXRvIEZlZGVyYWwxFDASBgNVBAcMC0N1YXVodMOpbW9jMRUwEwYDVQQtEwxTQVQ5NzA3MDFOTjMxXTBbBgkqhkiG9w0BCQIMTlJlc3BvbnNhYmxlOiBBZG1pbmlzdHJhY2nDs24gQ2VudHJhbCBkZSBTZXJ2aWNpb3MgVHJpYnV0YXJpb3MgYWwgQ29udHJpYnV5ZW50ZTCCAiIwDQYJKoZIhvcNAQEBBQADggIPADCCAgoCggIBALBvvsaH/V0hXumPYAH5nGemzzASC5nQI1rT0ib8m3MzJlW1fA7YzxpCSp84FhWXaSfPN2UVhShzgdLSLbQ1l1wQcJrEk2Ta2hb76plZkxPsluMx8wwU8zT008E5FvzWPD3IqI3NvAB0IrDj7YhMs0OR53Fpj/5zTAltWln8Fge4zTrk6UVg/DHE0/mJoYsHfrbwXM7xBBLtMqdCnb2+CTvzdVgw7a72WBziayOkRZJ6/ih44858c0j92mE/LbD+8bmFG4+aMIC1ysaMhFgWBR60AAhe7WniWF/aEgH8pY4N8dvt/6LNaX3WFTqtKh/qe8/ZSrvyeoI12mNwlSwfXkLVomqRBs5pSGC4KPku9FZ9gVxETvse5NU8dJMJa4w048fJbh0Q40x4+qFRYTLOV8nr+OW6wQAdB4gPCGi4TKBQizWb/A45iTe58qaP4241V9RAhx8lj7L9vCgFaWdeyNnTKPJI15bENPiX2y8MjVck7icWixDPR518otJUtUi6P813zorXoncEJfxFInZjK7m52MaAmSF5DodHzt+ZEY5uovVKQgxfUFNEtbkkpcIgHJ1NRAb5XXyCi3/5Uy9SpXdqDFdqSvFgWcx0uaqpcaTFhbPr9SyQ8Nk/eG4tHXLdt98I6C3tGoa0UF1eDSDOpHULSaHVDrxW61Fab/BxPJezAgMBAAGjggFBMIIBPTCB9wYDVR0jBIHvMIHsgBRVU5ugw+MGftFWQIOhf373X0RZd6GBvaSBujCBtzELMAkGA1UEBhMCTVgxGTAXBgNVBAgMEERpc3RyaXRvIEZlZGVyYWwxEzARBgNVBAcMCkN1YXVodGVtb2MxGDAWBgNVBAoMD0JhbmNvIGRlIE1leGljbzElMCMGA1UEAwwcQWdlbmNpYSBSZWdpc3RyYWRvcmEgQ2VudHJhbDE3MDUGCSqGSIb3DQEJAgwoUmVzcG9uc2FibGUgSm9zZSBBbnRvbmlvIEhlcm5hbmRleiBBeXVzb4IUMDAwMDAwMDAwMDAwMDAwMDAwMDIwHQYDVR0OBBYEFGoxvNX1YF3n4Y6PKMuhncG6LUxNMBIGA1UdEwEB/wQIMAYBAf8CAQAwDgYDVR0PAQH/BAQDAgH2MA0GCSqGSIb3DQEBCwUAA4IBXQASnHCKFtMxifL/FV9SqsASeuJXoO9rd77tcv5mBI6FnurANDs95PrkUYIXo9xS0Z9OdQxozmXPkh/bBZoyRcgiNr1CoWsbiAzxIfvS4ydc98OZF3G1BcKDuYc+zO3HEyDzPYNMlqF64+9tcpptQY4jreXDC3lqrKc0hZ6wL4QKLEvTE7AOnLOBQauUl9ZnPm6uEN9AWkIqu5ZIEW1FL6cRrS9HEeBNz3viaT4Kzd3lwJmMK0/yGAKjRWXwnruncIpJX7LLBw+tzdUVCljW8jLdwiM1W4gXk3CJ6ODjnuLzvQSd7Dr2e+8quwjSWjFory4I6cF0EJKSXp9p5szLUob5VI+HlRmoBwdxkLgtoLlPtV6sZ0Jd6JEop4DZ7Rz3nMJ2HqdgS1vC4SQUrMDxK0jOhB1A1MhsEWdzUXaFIjXNFyE/PR8QKiSnBkoYdqE7mEB6UMHPE/NH2PjsTvI=-----END CERTIFICATE-----";
        private string CERT_STORE_2011 = "-----BEGIN CERTIFICATE-----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-----END CERTIFICATE-----";
        private string CERT_STORE_2017 = "-----BEGIN CERTIFICATE-----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-----END CERTIFICATE-----";
        private string CERT_STORE_2019 = "-----BEGIN CERTIFICATE-----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-----END CERTIFICATE-----";

        public void ReadPublic()
        {
            try
            {
                System.IO.StringWriter stWrite = new System.IO.StringWriter();
                Certificado = new X509Certificate2(CertificateBytes);
                Org.BouncyCastle.X509.X509Certificate cert = DotNetUtilities.FromX509Certificate(Certificado);
                PubliceyParameter = cert.GetPublicKey();
                MemoryStream ms = new MemoryStream();
                TextWriter writer = new StreamWriter(ms);
                Org.BouncyCastle.OpenSsl.PemWriter pmw = new Org.BouncyCastle.OpenSsl.PemWriter(stWrite);
                pmw.WriteObject(PubliceyParameter);
                stWrite.Close();
                PublicKeyRSA = stWrite.ToString();
                Datos = DatosCert.GetDatosCert(CertificateBytes);
            }
            catch (Exception exe)
            {
                throw new Exception(Resource.ErrorCertificado);
            }
        }
        public void ValidaOscp()
        {
            // LAS LINEAS COMENTADAS SON DEBIDO A QUE SE UTILIZARÁ UN SOLO CERTIFICADO PARA  VALIDAR
            string CERSAT = GetStore();
            Ocsp oscp = new Ocsp();
            Org.BouncyCastle.X509.X509Certificate cert = DotNetUtilities.FromX509Certificate(Certificado);
            byte[] certBuffer = GetBytesFromPEM(CERSAT, "CERTIFICATE");
            var certificate = new X509Certificate2(certBuffer);
            Org.BouncyCastle.X509.X509Certificate certsat = DotNetUtilities.FromX509Certificate(certificate);
            oscp.ValidaOscp(cert,certsat);
        }
        public void ValidaFiel()
        {
            if (!IsFiel())
            {
                throw new Exception(Resource.ErrorFiel);
            }
        }
        public bool IsFiel()
        {
            const string IssuerName = "SAT970701NN3";
            X509Certificate2 cert = new X509Certificate2(CertificateBytes);
            bool nonRepudio = false;
            bool digitalSignature = false;
            bool result = false;
            string issuer = cert.Issuer;
            string[] arreglo = issuer.Split(',');
            string res = string.Empty;
            foreach (var cadena in arreglo)
            {
                if (cadena.Contains("OID.2"))
                {
                    char[] arraychar = cadena.ToCharArray();
                    for (int i = 14; i < arraychar.Length; i++)
                    {
                        res += arraychar[i].ToString();
                    }
                }
            }
            try
            {
                if (cert != null)
                {
                    nonRepudio = KeyUsageHasUsage(cert, X509KeyUsageFlags.NonRepudiation);
                    digitalSignature = KeyUsageHasUsage(cert, X509KeyUsageFlags.DigitalSignature);
                    if (nonRepudio && digitalSignature/* && IssuerName == res*/)
                        result = true;
                    else
                        result = false;
                }
                return result;
            }
            catch (Exception)
            {
                return false;
                throw;
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
        private string GetStore()
        {
            // Valida contra OCSP
            string Store = string.Empty;
            DateTime expiracion = DateTime.Parse(Certificado.GetEffectiveDateString());
            int añoGenerado = expiracion.Year;
            int mesGenerado = expiracion.Month + 1;
            int diaGenerado = expiracion.Day;

            if (añoGenerado >= 2012)
            {
                if (expiracion > DateTime.Parse("25/05/2019"))
                {
                    Store = CERT_STORE_2019;
                }
                else if (expiracion > DateTime.Parse("18/07/2015"))
                {
                    //if (expiracion < DateTime.Parse("01/01/2017"))
                    Store = CERT_STORE_2015;
                    //else Store = CERT_STORE_2017;
                }
                else if (añoGenerado >= 2014)
                {
                    Store = CERT_STORE_2013;
                }
                else if (añoGenerado == 2013 && mesGenerado >= 5)
                {
                    Store = CERT_STORE_2013;
                }
                else
                {
                    Store = CERT_STORE_2012;
                }
            }
            else
            {
                Store = CERT_STORE_2011;
            }
            return Store;
        }
        public void ValidaFechaExpiracion()
        {
            DateTime FechaExpiracion = DateTime.Parse(Certificado.GetExpirationDateString());
            if (FechaExpiracion < DateTime.Now)
            {
                throw new Exception(Resource.ErrorExpiracion);
            }
        }
        public byte[] GetBytesFromPEM(string pemString, string section)
        {
            var header = String.Format("-----BEGIN {0}-----", section);
            var footer = String.Format("-----END {0}-----", section);

            var start = pemString.IndexOf(header, StringComparison.Ordinal);
            if (start < 0)
                return null;

            start += header.Length;
            var end = pemString.IndexOf(footer, start, StringComparison.Ordinal) - start;

            if (end < 0)
                return null;

            return Convert.FromBase64String(pemString.Substring(start, end));
        }

    }
}