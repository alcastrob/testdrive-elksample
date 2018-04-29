using Newtonsoft.Json;
using StatsdClient;
using SyslogNet.Client;
using SyslogNet.Client.Serialization;
using SyslogNet.Client.Transport;
using System;

namespace ElkSample
{
    class Program
    {
        //TODO: Update your ELK instance IP addresses and ports here
        private const string syslogAddress = "192.168.0.160";
        private const int syslogPort = 3514;
        private const string statsdAddress = "192.168.0.160";
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
                StringField = "string sample",
            };
            
            ISyslogMessageSender sender = new SyslogUdpSender(syslogAddress, syslogPort);
            ISyslogMessageSerializer serializer = new SyslogRfc5424MessageSerializer();

            var msg = new SyslogMessage(
                DateTimeOffset.Now, //DatetimeOffset
                Facility.UserLevelMessages, //Facility
                Severity.Debug, //Severity
                Environment.MachineName, //HostName
                "BPTest", //Appname
                "0", //ProcId
                "0", //MsgId
                JsonConvert.SerializeObject(payload) //Message
                );

            for (int counter = 0; counter < 100; counter++)
            {
                sender.Send(msg, new SyslogRfc5424MessageSerializer());
            }
        }

        private static void UsingStatsd()
        {
            Metrics.Configure(new MetricsConfig
            {
                StatsdServerName = statsdAddress,                
                StatsdServerPort = statsdPort,
                Prefix = "PrefixSample"
            });

            var msgPayload = new
            {
                origin = "Origin Sample",
                renderTime = DateTime.UtcNow
            };

            for (int counter = 0; counter < 100; counter++)
            {
                Metrics.Set("Bulkdata", //Name of the statds containers
                    JsonConvert.SerializeObject(msgPayload)); //Object to send
            }
        }
    }
}
