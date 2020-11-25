using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWebApiFirma.DTO
{
    public class RequestHashArchivo
    {
        public string archivo { get; set; }
        public string claveprivada { get; set; }
        public string password { get; set; }
    }
}
