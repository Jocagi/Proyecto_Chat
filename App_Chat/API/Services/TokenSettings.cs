using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class TokenSettings : ITokenSettings
    {
        public string SecretKey { get; set; }
    }

    public interface ITokenSettings
    {
        string SecretKey { get; set; }
    }
}
