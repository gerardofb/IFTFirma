using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IFTApiFirmaDocumentos.DTO
{
    public static class Resource
    {
        public static string ErrorPassword = "No se ha podido validar la fiel. Razón: Contraseña inválida";
        public static string ErrorOscpRevocado =  "No se ha podido validar la fiel. Razón: Certificado revocado";
        public static string ErrorOscpDesconocido = "No se ha podido validar la fiel. Razón: Certifiado desconocido";
        public static string ErrorCertificado= "No se ha podido validar la fiel. Razón: Certificado inválido";
        public static string ErrorFiel = "No se ha podido validar la fiel. Razón: Fiel inválida";
        public static string ErrorExpiracion = "No se ha podido validar la fiel. Razón: Certificado expirado";
        public static string ErrorParidad;
    }
}