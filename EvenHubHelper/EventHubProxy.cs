using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.IO;
namespace EventHubHelper
{


    public class EventHubProxy
    {
        private static EventHubClient eventHubClient;
        private const string EhConnectionString = "Endpoint=sb://cntkmtc.servicebus.chinacloudapi.cn/;SharedAccessKeyName=all;SharedAccessKey=qi2FOjvgb8kRRRW5zpUFNDt5P6G8HSxtEZgW+JZ0ysM=;EntityPath=ingest";
        private const string EhEntityPath = "ingest";
        private const string PBIApiString = @"https://api.powerbi.com/beta/c603cbb7-6c19-47de-ac9d-98f4ac9e7f47/datasets/904f4f8c-5f65-4c97-81eb-464d8c053b72/rows?key=Q7ItaNcVC4YjqPxcA6kClYxm5tOu7JQOsIfflPmxMa7mgLlUkgGc2YqYyeAENBszieV3Uukv6I93bmqWwUlDHA%3D%3D";


        public static async Task WriteDataAsync(MetricEvent info)
        {
            // Creates an EventHubsConnectionStringBuilder object from the connection string, and sets the EntityPath.
            // Typically, the connection string should have the entity path in it, but for the sake of this simple scenario
            // we are using the connection string from the namespace.
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(EhConnectionString)
            {
                TransportType = Microsoft.Azure.EventHubs.TransportType.AmqpWebSockets
            };

            if (connectionStringBuilder.EntityPath == null)
            {
                connectionStringBuilder.EntityPath = EhEntityPath;
            }


            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
            try { 
            await SendMessagesToEventHub(info);
            }
            catch (Exception exception)
            {
                Debug.Write($"{DateTime.Now} > Exception: {exception.Message}");

            }
            finally
            {
                Debug.Write($"message sent.");
                await eventHubClient.CloseAsync();
            }
        }

        //Creates an event hub client and sends 100 messages to the event hub.
        public static async Task Test100MessagesToEventHub(int numberOfDevices)
        {
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(EhConnectionString)
            {
                TransportType = Microsoft.Azure.EventHubs.TransportType.AmqpWebSockets
            };

            if (connectionStringBuilder.EntityPath == null)
            {
                connectionStringBuilder.EntityPath = EhEntityPath;
            }


            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            try
            {
                Random random = new Random();
                for (var i = 0; i < numberOfDevices; i++)
                {
                    MetricEvent info = new MetricEvent() {
                        DeviceId = random.Next(numberOfDevices),                        
                        MakeTime = DateTime.Now,
                        Purity = random.Next(numberOfDevices),
                        Shortage = random.Next(numberOfDevices)
                    };

                    var serializedString = Serialize(info, Formatting.Indented);
                    Debug.Write($"Sending message: {serializedString.ToString()}");
                    await SendMessagesToEventHub(info);

                }
            }
            catch (Exception exception)
            {
                Debug.Write($"{DateTime.Now} > Exception: {exception.Message}");

            }
            finally
            {
                Debug.Write($"{numberOfDevices} messages sent.");
                await eventHubClient.CloseAsync();
            }
        }
        private static async Task SendMessagesToEventHub(MetricEvent info)
        {
            string uniqueEventId = Guid.NewGuid().ToString();
            try
            {

                    var serializedString = Serialize(info, Formatting.Indented);
                    EventData eventData = new EventData(Encoding.UTF8.GetBytes(serializedString));
                    Debug.Write($"Sending message: {serializedString.ToString()}");
                    eventData.Properties["EventId"] = uniqueEventId;
                    await eventHubClient.SendAsync(eventData);

            }
            catch (Exception exception)
            {
                Debug.Write($"{DateTime.Now} > Exception: {exception.Message}");

            }
            finally
            {
                Debug.Write($"message {uniqueEventId} sent.");
                //await eventHubClient.CloseAsync();
            }
        }

        public static async Task SendMessagesToPBI(MetricEvent info)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(PBIApiString);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.KeepAlive = true;
            //httpWebRequest.ContentLength = 0;
            //var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            HttpWebResponse httpResponse;

            //int numberOfDevices = 100;

            try
            {

                //for (var i = 0; i < numberOfDevices; i++)
                {
                    var jsonString = "["+Serialize(info, Formatting.Indented)+"]";
                    //var jsonString =  Serialize(info, Formatting.Indented) ;

                    Debug.Write($"Sending message: {jsonString.ToString()}");
                   using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        
                          await Task.Run(() => streamWriter.Write(jsonString));
                        await Task.Run(() => streamWriter.Flush());
                        await Task.Run(() => streamWriter.Close());

                        httpResponse = await Task.Run(() => (HttpWebResponse)httpWebRequest.GetResponse());                       

                    }

                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = await Task.Run(() => streamReader.ReadToEnd());
                        Debug.Write(result);
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.Write($"{DateTime.Now} > Exception: {exception.Message}");

            }
            finally
            {
                Debug.Write($"messages sent.");
            }

        }

        private static string Serialize(object item, Formatting formatting = default(Formatting))
        {
            if (item == null)
            {
                throw new ArgumentException("The item argument cannot be null.");
            }
            var json = JsonConvert.SerializeObject(item, formatting);
            return json;
        }

    }
}
