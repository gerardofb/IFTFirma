using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IFTApiFirmaDocumentos.DTO
{
    public class RequestHashArchivo
    {
        public string archivo { get; set; }
        public string claveprivada { get; set; }
        public string password { get; set; }
    }
}