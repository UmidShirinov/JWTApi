using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Configuration
{
    public class Client
    {
        public int ClientId{ get; set; }
        public string Secret { get; set; }
        public List<string> Audience { get; set; } // hansi APi lere girise bilecek
    }
}
