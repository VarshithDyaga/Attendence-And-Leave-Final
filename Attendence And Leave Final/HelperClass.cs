﻿using Azure;
using Newtonsoft.Json;
using System.Text;
using Azure.Messaging.EventGrid;
using Azure.Storage.Blobs;
using Attendence_And_Leave_Final.Model;


namespace Attendence_And_Leave_Final

{

    public class Helper

    {

        public static async Task<bool> UploadBlob(

         IConfiguration config, Leaves leavesdata)

        {

            string blobConnString = config.GetConnectionString("StorAccConnString");

            BlobServiceClient client = new BlobServiceClient(blobConnString);

            string container = config.GetValue<string>("Container");

            var containerClient = client.GetBlobContainerClient(container);


            string fileName = "ems.Department." + Guid.NewGuid().ToString() + ".json";

            // Get a reference to a blob

            BlobClient blobClient = containerClient.GetBlobClient(fileName);


            //memorystream

            using (var stream = new MemoryStream())

            {

                var serializer = JsonSerializer.Create(new JsonSerializerSettings());


                // Use the 'leave open' option to keep the memory stream open after the stream writer is disposed

                using (var writer = new StreamWriter(stream, Encoding.UTF8, 1024, true))

                {

                    // Serialize the job to the StreamWriter

                    serializer.Serialize(writer, leavesdata);

                }


                // Rewind the stream to the beginning

                stream.Position = 0;


                // Upload the job via the stream

                await blobClient.UploadAsync(stream, overwrite: true);

            }


            await PublishToEventGrid(config, leavesdata);

            return true;

        }


        private static async Task PublishToEventGrid(

            IConfiguration config, Leaves leaves)

        {

            var endpoint = config.GetValue<string>("EventGridTopicEndpoint");

            var accessKey = config.GetValue<string>("EventGridAccessKey");

            EventGridPublisherClient client = new EventGridPublisherClient(

                new Uri(endpoint),

                new AzureKeyCredential(accessKey));

            var event1 = new EventGridEvent(

                    "EMS",

                    "EMS.DepartmentEvent",

                    "1.0",

                    JsonConvert.SerializeObject(leaves));

            event1.Id = new Guid().ToString();

            event1.EventTime = DateTime.Now;

            event1.Topic = config.GetValue<string>("EventGridTopic");

            List<EventGridEvent> eventsList = new List<EventGridEvent>

            {

                event1

            };

            // Send the events

            await client.SendEventsAsync(eventsList);

        }

    }

}