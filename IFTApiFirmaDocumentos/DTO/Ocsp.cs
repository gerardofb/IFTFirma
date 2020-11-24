using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.X509;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace IFTApiFirmaDocumentos.DTO
{
    public class Ocsp
    {
        public byte[] PostData(string url, byte[] data, string contentType, string accept)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = contentType;
            request.ContentLength = data.Length;
            request.Accept = accept;
            Stream stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream respStream = response.GetResponseStream();
            byte[] resp = Utilerias.ToByteArray(respStream);
            respStream.Close();
            return resp;
        }
        public CertificateStatus ValidaOscp(X509Certificate eeCert, X509Certificate issuerCert)
        {
            string url = "https://cfdi.sat.gob.mx/edofiel";
            OcspReq req = GenerateOcspRequest(issuerCert, eeCert.SerialNumber);
            byte[] binaryResp = PostData(url, req.GetEncoded(), "application/ocsp-request", "application/ocsp-response");
            return ProcessOcspResponse(eeCert, issuerCert, binaryResp);
        }
        private CertificateStatus ProcessOcspResponse(X509Certificate eeCert, X509Certificate issuerCert, byte[] binaryResp)
        {
            OcspResp r = new OcspResp(binaryResp);
            CertificateStatus cStatus = null;

            switch (r.Status)
            {
                case OcspRespStatus.Successful:
                    BasicOcspResp or = (BasicOcspResp)r.GetResponseObject();

                    if (or.Responses.Length == 1)
                    {
                        SingleResp resp = or.Responses[0];

                        ValidateCertificateId(issuerCert, eeCert, resp.GetCertID());
                        Object certificateStatus = resp.GetCertStatus();
                        if (certificateStatus == Org.BouncyCastle.Ocsp.CertificateStatus.Good)
                        {
                            cStatus = CertificateStatus.Good;
                        }
                        else if (certificateStatus is Org.BouncyCastle.Ocsp.RevokedStatus)
                        {
                            throw new Exception(Resource.ErrorOscpRevocado);
                        }
                        else if (certificateStatus is Org.BouncyCastle.Ocsp.UnknownStatus)
                        {
                            throw new Exception(Resource.ErrorOscpDesconocido);
                        }
                    }
                    break;
                default:
                    throw new Exception(Resource.ErrorOscpDesconocido);
            }

            return cStatus;
        }
        private void ValidateCertificateId(X509Certificate issuerCert, X509Certificate eeCert, CertificateID certificateId)
        {
            CertificateID expectedId = new CertificateID(CertificateID.HashSha1, issuerCert, eeCert.SerialNumber);

            if (!expectedId.SerialNumber.Equals(certificateId.SerialNumber))
            {
                throw new Exception("Invalid certificate ID in response");
            }

            if (!Org.BouncyCastle.Utilities.Arrays.AreEqual(expectedId.GetIssuerNameHash(), certificateId.GetIssuerNameHash()))
            {
                throw new Exception("Invalid certificate Issuer in response");
            }

        }
        private OcspReq GenerateOcspRequest(X509Certificate issuerCert, BigInteger serialNumber)
        {
            CertificateID id = new CertificateID(CertificateID.HashSha1, issuerCert, serialNumber);
            return GenerateOcspRequest(id);
        }
        private OcspReq GenerateOcspRequest(CertificateID id)
        {
            OcspReqGenerator ocspRequestGenerator = new OcspReqGenerator();

            ocspRequestGenerator.AddRequest(id);

            BigInteger nonce = BigInteger.ValueOf(new DateTime().Ticks);

            ArrayList oids = new ArrayList();
            Hashtable values = new Hashtable();

            oids.Add(OcspObjectIdentifiers.PkixOcsp);

            Asn1OctetString asn1 = new DerOctetString(new DerOctetString(new byte[] { 1, 3, 6, 1, 5, 5, 7, 48, 1, 1 }));

            values.Add(OcspObjectIdentifiers.PkixOcsp, new X509Extension(false, asn1));
            ocspRequestGenerator.SetRequestExtensions(new X509Extensions(oids, values));

            return ocspRequestGenerator.Generate();
        }
    }
}