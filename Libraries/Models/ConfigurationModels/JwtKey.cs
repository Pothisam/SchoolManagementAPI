using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ConfigurationModels
{
    public class JwtKey
    {
        private const string _key = "SVuZbh2ICMYjZydFkDBjjgh9P0C52oJto/xuqMvATvwr/g3lVfl7dWZdOQcv6IIg";
        private JwtKey()
        {
        }
        public const string AuthKey = _key;
    }
}
