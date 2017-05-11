#r "Microsoft.WindowsAzure.Storage"
using System;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");

    // Parse query parameter (GET)
    string countType = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "countType", true) == 0)
        .Value;

    // BLOB STORAGE SETUP
    log.Info("Accessing Storage Account and Blob Container.");
    var storageAccountConnectionString = "DefaultEndpointsProtocol=https;AccountName=genevabirding;AccountKey=KsBQ8Cpjjs7d7nIwoSodefI7REqLuMOigBX8adukBISaGjSuy24AxidGzLNnFdKssaSq7ORbXO1yqcj4yTQirw==;EndpointSuffix=core.windows.net";
    var azureStorageAccount = CloudStorageAccount.Parse(storageAccountConnectionString); // Storage account on Azure where the Blobs are stored 
    CloudBlobClient blobClient = azureStorageAccount.CreateCloudBlobClient();
    
    CloudBlobContainer container;

    if (countType == "field")
    {
        container = blobClient.GetContainerReference("field-counts-container");
    }
    else if (countType == "feeder")
    {
        container = blobClient.GetContainerReference("feeder-counts-container");
    }
    else if (countType == "countweek")
    {
        container = blobClient.GetContainerReference("countweek-counts-container");
    }
    else
    {
        container = blobClient.GetContainerReference("");
        log.Error("Invalid count type.");
    }

    // Find all Blobs in the Container, and list the names (URIs) of the Blobs
    var blobs = container.ListBlobs();

    foreach (var blobItem in blobs)
    {
        // Get Names of Blobs by Creating a Substring (Removing Unwanted Parts of Blob URI)
        // Referenced: http://stackoverflow.com/questions/9505400/extract-part-of-a-string-between-point-a-and-b
        string blobName = "";
        int startPos = blobItem.Uri.ToString().LastIndexOf("-counts-container") + "-counts-container".Length + 1;
        int length = blobItem.Uri.ToString().Length - startPos;
        blobName = blobItem.Uri.ToString().Substring(startPos, length);

        log.Info(blobItem.Uri.ToString());
        log.Info(blobName);
    }

    // SEND TO THE WEB-BROWSER
    return blobs == null
        ? req.CreateResponse(HttpStatusCode.BadRequest, "Could not access blobs.")
        : req.CreateResponse(HttpStatusCode.OK, "Accessed blobs.");
}