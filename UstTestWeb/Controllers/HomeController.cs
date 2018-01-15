using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using UstTestWeb.App_Start;

namespace UstTestWeb.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Queue()
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

            return View(result);
        }
        public async Task<ActionResult> RSSFeed()
        {
            var objRss = new Rss();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                try
                {
                     var response  = await client.GetAsync("https://platform.boomi.com/account/trainingvishnurajnr-48CRQA/container/50c3bbff-261c-404a-9635-eb32fac2f12f/feed/rss-2.0");

                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        var buffer = Encoding.UTF8.GetBytes(content);
                        using (var stream = new MemoryStream(buffer))
                        {
                            var serializer = new XmlSerializer(typeof(Rss));
                            objRss = (Rss)serializer.Deserialize(stream);                           
                        }                      
                    }
                    else
                    {
                        
                    }
                }
                catch(Exception e)
                {
                  
                }
            }          

            return View(objRss);
        }        
    }

    [XmlRoot(ElementName = "guid")]
    public class Guid
    {
        [XmlAttribute(AttributeName = "isPermaLink")]
        public string IsPermaLink { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "item")]
    public class Item
    {
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }
        [XmlElement(ElementName = "link")]
        public string Link { get; set; }
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "category")]
        public List<string> Category { get; set; }
        [XmlElement(ElementName = "pubDate")]
        public string PubDate { get; set; }
        [XmlElement(ElementName = "guid")]
        public Guid Guid { get; set; }
        [XmlElement(ElementName = "creator", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Creator { get; set; }
        [XmlElement(ElementName = "date", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Date { get; set; }
    }

    [XmlRoot(ElementName = "channel")]
    public class Channel
    {
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }
        [XmlElement(ElementName = "link")]
        public string Link { get; set; }
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "pubDate")]
        public string PubDate { get; set; }
        [XmlElement(ElementName = "creator", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Creator { get; set; }
        [XmlElement(ElementName = "date", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Date { get; set; }
        [XmlElement(ElementName = "item")]
        public List<Item> Item { get; set; }
    }

    [XmlRoot(ElementName = "rss")]
    public class Rss
    {
        [XmlElement(ElementName = "channel")]
        public Channel Channel { get; set; }
        [XmlAttribute(AttributeName = "content", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Content { get; set; }
        [XmlAttribute(AttributeName = "rdf", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Rdf { get; set; }
        [XmlAttribute(AttributeName = "dc", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Dc { get; set; }
        [XmlAttribute(AttributeName = "taxo", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Taxo { get; set; }
        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }
    }


    [DataContract]
    [XmlRoot(ElementName = "Account")]
    public class Account
    {
        [XmlElement(ElementName = "Id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "IsDeleted")]
        public string IsDeleted { get; set; }
        [XmlElement(ElementName = "MasterRecordId")]
        public string MasterRecordId { get; set; }
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "RecordTypeId")]
        public string RecordTypeId { get; set; }
        [XmlElement(ElementName = "ParentId")]
        public string ParentId { get; set; }
        [XmlElement(ElementName = "BillingStreet")]
        public string BillingStreet { get; set; }
        [XmlElement(ElementName = "BillingCity")]
        public string BillingCity { get; set; }
        [XmlElement(ElementName = "BillingState")]
        public string BillingState { get; set; }
        [XmlElement(ElementName = "BillingPostalCode")]
        public string BillingPostalCode { get; set; }
        [XmlElement(ElementName = "BillingCountry")]
        public string BillingCountry { get; set; }
        [XmlElement(ElementName = "BillingLatitude")]
        public string BillingLatitude { get; set; }
        [XmlElement(ElementName = "BillingLongitude")]
        public string BillingLongitude { get; set; }
        [XmlElement(ElementName = "BillingGeocodeAccuracy")]
        public string BillingGeocodeAccuracy { get; set; }
        [XmlElement(ElementName = "BillingAddress")]
        public string BillingAddress { get; set; }
        [XmlElement(ElementName = "ShippingStreet")]
        public string ShippingStreet { get; set; }
        [XmlElement(ElementName = "ShippingCity")]
        public string ShippingCity { get; set; }
        [XmlElement(ElementName = "ShippingState")]
        public string ShippingState { get; set; }
        [XmlElement(ElementName = "ShippingPostalCode")]
        public string ShippingPostalCode { get; set; }
        [XmlElement(ElementName = "ShippingCountry")]
        public string ShippingCountry { get; set; }
        [XmlElement(ElementName = "ShippingLatitude")]
        public string ShippingLatitude { get; set; }
        [XmlElement(ElementName = "ShippingLongitude")]
        public string ShippingLongitude { get; set; }
        [XmlElement(ElementName = "ShippingGeocodeAccuracy")]
        public string ShippingGeocodeAccuracy { get; set; }
        [XmlElement(ElementName = "ShippingAddress")]
        public string ShippingAddress { get; set; }
        [XmlElement(ElementName = "Phone")]
        public string Phone { get; set; }
        [XmlElement(ElementName = "Website")]
        public string Website { get; set; }
        [XmlElement(ElementName = "PhotoUrl")]
        public string PhotoUrl { get; set; }
        [XmlElement(ElementName = "Industry")]
        public string Industry { get; set; }
        [XmlElement(ElementName = "NumberOfEmployees")]
        public string NumberOfEmployees { get; set; }
        [XmlElement(ElementName = "Description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "OwnerId")]
        public string OwnerId { get; set; }
        [XmlElement(ElementName = "CreatedDate")]
        public string CreatedDate { get; set; }
        [XmlElement(ElementName = "CreatedById")]
        public string CreatedById { get; set; }
        [XmlElement(ElementName = "LastModifiedDate")]
        public string LastModifiedDate { get; set; }
        [XmlElement(ElementName = "LastModifiedById")]
        public string LastModifiedById { get; set; }
        [XmlElement(ElementName = "SystemModstamp")]
        public string SystemModstamp { get; set; }
        [XmlElement(ElementName = "LastActivityDate")]
        public string LastActivityDate { get; set; }
        [XmlElement(ElementName = "LastViewedDate")]
        public string LastViewedDate { get; set; }
        [XmlElement(ElementName = "LastReferencedDate")]
        public string LastReferencedDate { get; set; }
        [XmlElement(ElementName = "Jigsaw")]
        public string Jigsaw { get; set; }
        [XmlElement(ElementName = "JigsawCompanyId")]
        public string JigsawCompanyId { get; set; }
        [XmlElement(ElementName = "AccountSource")]
        public string AccountSource { get; set; }
        [XmlElement(ElementName = "SicDesc")]
        public string SicDesc { get; set; }
        [XmlElement(ElementName = "Reference_Number__c")]
        public string Reference_Number__c { get; set; }
        [XmlElement(ElementName = "Fax_Number__c")]
        public string Fax_Number__c { get; set; }
        [XmlElement(ElementName = "CGO_Email__c")]
        public string CGO_Email__c { get; set; }
        [XmlElement(ElementName = "CGO_Contact__c")]
        public string CGO_Contact__c { get; set; }
        [XmlElement(ElementName = "Store_Type_Desc__c")]
        public string Store_Type_Desc__c { get; set; }
        [XmlElement(ElementName = "Is_Migrated__c")]
        public string Is_Migrated__c { get; set; }
        [XmlElement(ElementName = "Format_Desc__c")]
        public string Format_Desc__c { get; set; }
        [XmlElement(ElementName = "Opening_Hours_Main__c")]
        public string Opening_Hours_Main__c { get; set; }
        [XmlElement(ElementName = "Store_Manager__c")]
        public string Store_Manager__c { get; set; }
        [XmlElement(ElementName = "Store_Manager_Contact__c")]
        public string Store_Manager_Contact__c { get; set; }
        [XmlElement(ElementName = "Store_Manager_Email__c")]
        public string Store_Manager_Email__c { get; set; }
        [XmlElement(ElementName = "Area_Manager_Contact__c")]
        public string Area_Manager_Contact__c { get; set; }
        [XmlElement(ElementName = "Area_Manager_Email__c")]
        public string Area_Manager_Email__c { get; set; }
        [XmlElement(ElementName = "Area_Name__c")]
        public string Area_Name__c { get; set; }
    }

}