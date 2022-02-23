//using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

//using Microsoft.Azure.WebJobs;
//using Microsoft.Azure.WebJobs.Host;
//using Microsoft.Azure.EventHubs;
//using System.Text;
//using System.Net.Http;
//using Microsoft.Extensions.Logging;

//namespace Dotdev.Api
//{
//    public class IotHubToTwins
//    {
//        private static HttpClient client = new HttpClient();
        
//        [FunctionName("IotHubToTwins")]
//        public void Run([IoTHubTrigger("messages/events", Connection = "IOT_HUB_CONNSTR")]EventData message, ILogger log)
//        {
//            log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");
//        }
//    }
//}