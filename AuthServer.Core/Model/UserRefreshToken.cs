using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Model
{
    public class UserRefreshToken
    {
        public string UserId { get; set; }
        public string COde { get; set; }
        public DateTime Expiration { get; set; }
    }
}
