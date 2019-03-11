using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PubService
{
    public class ConfigurationUtil
    {
        private static IConfiguration _configuration = null;

        public static IConfigurationSection GetSection(string key)
        {
            return  _configuration.GetSection(key);;
        }

        public static void SetConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

    }
}
