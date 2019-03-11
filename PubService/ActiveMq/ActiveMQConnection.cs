using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace PubService.ActiveMq
{
    public class ActiveMQConnection
    {
        private static Lazy<IConnection> lazyConnection = new Lazy<IConnection>(() => CreateConnection());

        private static IConnection CreateConnection()
        {
            try
            {
                var connectionString = ConfigurationUtil.GetSection("ActiveMQConfig")["ActiveMQConnection"];
                StringDictionary config = new StringDictionary();
                var data = connectionString.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in data)
                {
                    var kvPair = item.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                    config[kvPair[0]] = kvPair[1];
                }
                var result = new ConnectionFactory(config["brokerUri"]).CreateConnection() as Connection;
                result.UserName = config["username"];
                result.Password = config["password"];
                result.Start();
                return result;
            }
            catch
            {
                return null;
            }
        }
        // public static string ConnectionString { get; set; }

        public static IConnection Connection
        {
            get
            {
                if (lazyConnection.Value == null || !lazyConnection.Value.IsStarted)
                {
                    lazyConnection = new Lazy<IConnection>(() => CreateConnection());
                }
                return lazyConnection.Value;
            }
        }

    }
}
