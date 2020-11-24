using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IFTApiFirmaDocumentos.DTO
{
    public class RequestCadenaOriginal
    {
        public string cert { get; set; }
        public string key { get; set; }
        public string password { get; set; }
    }
}