using Newtonsoft.Json;
using SyslogNet.Client;
using SyslogNet.Client.Serialization;
using SyslogNet.Client.Transport;
using System;

namespace ElkSample
{
    class Program
    {
        //TODO: Update your ELK instance IP addresses and ports here
        private const string syslogAddress = "159.89.5.228";
        private const int syslogPort = 3514;
        private const string statsdAddress = "159.89.5.228";
        private const int statsdPort = 6000;

        static void Main(string[] args)
        {            
            UsingSyslog();
            //UsingStatsd();
        }

        private static void UsingSyslog()
        {
            var payload = new LogEntity
            {
                IntField = 7,
                LongField = 1200400,
                StringField = "Connection error with endpoint",
            };
            
            ISyslogMessageSender sender = new SyslogUdpSender(syslogAddress, syslogPort);
            ISyslogMessageSerializer serializer = new SyslogRfc5424MessageSerializer();

            var msg = new SyslogMessage(
                DateTimeOffset.Now, //DatetimeOffset
                Facility.UserLevelMessages, //Facility
                Severity.Debug, //Severity
                Environment.MachineName, //HostName
                "Open", //Appname
                "0", //ProcId
                "0", //MsgId
                JsonConvert.SerializeObject(payload) //Message
                );

            for (int counter = 0; counter < 1; counter++)
            {
                sender.Send(msg, new SyslogRfc5424MessageSerializer());
            }
        }
    }
}
