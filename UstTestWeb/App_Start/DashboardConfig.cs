using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace UstTestWeb.App_Start
{
    public static class DashboardConfig
    {
        public static string keyName = string.Empty;
        public static string accessKey = string.Empty;
        public static string baseAddress = string.Empty;
        public static string queueName = string.Empty;       
        public static void Config()
        {
            keyName = ConfigurationManager.AppSettings["keyName"];
            accessKey = ConfigurationManager.AppSettings["accessKey"];
            baseAddress = ConfigurationManager.AppSettings["baseAddress"];
            queueName = ConfigurationManager.AppSettings["queueName"];
          
        }
    }
}