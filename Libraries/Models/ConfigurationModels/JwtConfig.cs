using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ConfigurationModels
{
    public class JwtConfig
    {
        public required string Issuer { get; set; }
        public required string Audince { get; set; }
    }
}
