using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Serialization;
using UstTestWeb.App_Start;
using UstTestWeb.Models;

namespace UstTestWeb.Controllers
{
    public class QueueController : ApiController
    {
        /// <summary>
        /// Gets the list of contacts
        /// </summary>
        /// <returns>The contacts</returns>
        [HttpGet]
        [Route("~/read")]
        public async Task<IEnumerable<Account>> Get()
        {
            return await GetQueue();
        }

        public async Task<List<Account>> GetQueue()
        {
            var result = new List<Account>();

            try
            {
                TokenProvider tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(DashboardConfig.keyName, DashboardConfig.accessKey);
                MessagingFactory messagingFactory = MessagingFactory.Create(DashboardConfig.baseAddress, tokenProvider);
                var receiver = await messagingFactory.CreateMessageReceiverAsync(DashboardConfig.queueName, ReceiveMode.ReceiveAndDelete);

                while (true)
                {
                    var msg = await receiver.ReceiveAsync(TimeSpan.Zero);
                    if (msg != null)
                    {
                        var serializer = new XmlSerializer(typeof(Account));
                        var objresult = (Account)serializer.Deserialize(new StreamReader(msg.GetBody<Stream>()));
                        result.Add(objresult);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }

            return result;
        }
    }
}
