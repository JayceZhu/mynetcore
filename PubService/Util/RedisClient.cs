using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PubService
{
    public class RedisClient : IDisposable
    {
        private ConcurrentDictionary<string, ConnectionMultiplexer> _connections;

        public RedisClient(IConfigurationRoot config)
        {
        
            //_connections = new ConcurrentDictionary<string, ConnectionMultiplexer>();
        }

        private static ConnectionMultiplexer CreateConnection()
        {
            return ConnectionMultiplexer.Connect(ConfigurationUtil.GetSection("RedisConfig").GetSection("Redis_Default")["Connection"]);

        }

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() => CreateConnection());

        public static ConnectionMultiplexer Connnection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        public static IDatabase GetDatabase(int? db = null)
        {
            var defaultdb = 0;
            if (db != null)
            {
                defaultdb = (int)db;
            }
            return Connnection.GetDatabase(defaultdb);
        }

        public void Dispose()
        {
            if (Connnection != null)
            {

                Connnection.Close();
            }
        }
    }
}
