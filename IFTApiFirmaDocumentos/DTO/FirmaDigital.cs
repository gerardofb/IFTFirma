using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IFTApiFirmaDocumentos.DTO
{
    public class FirmaDigital
    {
        public DTO.FirmaElectronica firma { get; set; }
        public FirmaDigital(byte[] cert, byte[] key, string pass)
        {
            try
            {
                firma = new DTO.FirmaElectronica();
                this.firma.certificate.CertificateBytes = cert;
                this.firma.privatekey.KeyBytes = key;
                this.firma.privatekey.Password = pass;
                GeneraFirma();
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }
        private void GeneraFirma()
        {
            firma.certificate.ReadPublic();
            firma.certificate.ValidaFiel();
            firma.certificate.ValidaFechaExpiracion();
            // PARA EJECUTAR ESTA SECCIÓN SE REQUIERE UN CERTIFICADO ESPECÍFICO DEL INSTITUTO FEDERAL DE TELECOMUNICACIONES
            //firma.certificate.ValidaOscp();
            firma.privatekey.ReadPrivate();
            // PARA EJECUTAR ESTA SECCIÓN SE REQUIERE UN CERTIFICADO ESPECÍFICO DEL INSTITUTO FEDERAL DE TELECOMUNICACIONES
            firma.Paridad();
        }
        public string Firma(string cadena)
        {
            return firma.privatekey.Firma(cadena);
        }

        public string GetDatosNombre()
        {
            return firma.certificate.Datos.Sujeto_Nombre;
        }

        public string GetDatosRfc()
        {
            return firma.certificate.Datos.Sujeto_Id;
        }
        public string GetDatosSerie()
        {
            return firma.certificate.Datos.Serie;
        }
        public string GetDatosAutoridad()
        {
            return firma.certificate.Datos.Emisor_Nombre;
        }
    }
}