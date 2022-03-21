using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ReadDataFromAppendBlobType {
    class Program {
        static void Main(string[] args)
        {
            string data = GetBlob("datalake", "TxtOutput/simple.txt");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        public static string GetBlob(string containerName, string fileName)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=pemsbidevstorage;AccountKey=8Dpv45Iu+RWKvbSTCSR/uCDAwgYotzY7nckjhxHSYYoGAgIKxBKiw1i2kemWdsJYs+OZIV1xixgoIvxYNwrgPw==;EndpointSuffix=core.windows.net;";
            // Setup the connection to the storage account
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            // Connect to the blob storage
            CloudBlobClient serviceClient = storageAccount.CreateCloudBlobClient();
            // Connect to the blob container
            CloudBlobContainer container = serviceClient.GetContainerReference($"{containerName}");
            // Connect to the blob file
            CloudAppendBlob appendBlob = container.GetAppendBlobReference($"{fileName}");
            // Get the blob file as text
            // string contents = blob.DownloadTextAsync().Result;
            long streamStart = 0;
            while (true)
            {
                appendBlob.FetchAttributesAsync();
                var availableLength = appendBlob.Properties.Length;
                if (streamStart < availableLength)
                {
                    var memoryStream = new MemoryStream();
                    appendBlob.DownloadRangeToStreamAsync(memoryStream, streamStart, null);
                    var length = memoryStream.Length;
                    memoryStream.Position = 0;
                    using (var reader = new StreamReader(memoryStream))
                    {
                        Console.Write(reader.ReadToEnd());
                    }
                    streamStart = streamStart + length;
                }
                Thread.Sleep(TimeSpan.FromSeconds(2));
            }
        }
    }
}
