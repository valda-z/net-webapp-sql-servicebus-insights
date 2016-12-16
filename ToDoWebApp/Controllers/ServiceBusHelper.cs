using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using ToDoWebApp.Models;
using ToDoWebApp.Properties;

namespace ToDoWebApp.Controllers
{
    public static class ServiceBusHelper
    {
        public static void SendToAll(ToDo itm)
        {
            try
            {
                TopicClient Client = TopicClient.CreateFromConnectionString(
                    ConfigurationManager.AppSettings["SERVICEBUS_URI"]
                    , "todotopic");
                var msg = new BrokeredMessage(
                     JsonConvert.SerializeObject(itm, Formatting.Indented));
                msg.Properties.Add("Category", itm.Category);
                Client.Send(msg);
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}