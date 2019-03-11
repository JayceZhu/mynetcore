using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PubService.ActiveMq
{
    public class ActiveMQMessagePusher
    {
        static ActiveMQMessagePusher()
        {
            lock (myLock)
            {
                session = ActiveMQConnection.Connection.CreateSession() as Session;
                session.Start();
            }
        }

        private static object myLock = new object();
        private static Session session = null;

        /// <summary>
        /// 发送消息
        /// </summary>
        public static void Push(string queue, IDictionary<string, string> properties, object messageObj)
        {
            try
            {
                if (!session.Started)
                {
                    lock (myLock)
                    {
                        session = ActiveMQConnection.Connection.CreateSession() as Session;
                        session.Start();
                    }
                }
                IMessageProducer producer = session.CreateProducer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(queue));
                ITextMessage message = producer.CreateTextMessage();
                message.Text = JsonConvert.SerializeObject(messageObj);
                foreach (var key in properties.Keys)
                {
                    message.Properties[key] = properties[key];
                }
                producer.Send(message, MsgDeliveryMode.Persistent, MsgPriority.Normal, TimeSpan.MinValue);

            }
            catch (Exception ex)
            {
            }
        }
    }
}
