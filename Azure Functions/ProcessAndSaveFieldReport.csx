#load "SendNewFieldReportEmail.csx"
#load "BirdCount.cs"
#load "FieldFormAnswers.cs"
#r "Microsoft.WindowsAzure.Storage"
#r "Newtonsoft.Json"
using System;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Text;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");

    // Get request body
    dynamic fieldCountData = await req.Content.ReadAsAsync<object>();

    // BLOB STORAGE SETUP
    log.Info("Creating a Blob Container and Blob.");
    var storageAccountConnectionString = "DefaultEndpointsProtocol=https;AccountName=genevabirding;AccountKey=KsBQ8Cpjjs7d7nIwoSodefI7REqLuMOigBX8adukBISaGjSuy24AxidGzLNnFdKssaSq7ORbXO1yqcj4yTQirw==;EndpointSuffix=core.windows.net";

    // Save fieldCountData (JSON) to Blob on Azure Storage
    var azureStorageAccount = CloudStorageAccount.Parse(storageAccountConnectionString); // Storage account on Azure where the Blob will be stored
    
    CloudBlobClient blobClient = azureStorageAccount.CreateCloudBlobClient();
    CloudBlobContainer container = blobClient.GetContainerReference("field-counts-container");

    try
    {
        container.CreateIfNotExists();      // Creates a new Blob Container if the specified container ("field-counts-container") does not exist.
        
        // Set wide-open "Container"-level permissions for the new blob container
        container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Container });
    }
    catch (Exception e)
    {
        log.Error(e.Message);
    }

    // Get Current Date and Time, and Format the String (To Be Used As Blob Name)
    string currentDateTime = DateTime.Now.ToString();
    currentDateTime = currentDateTime.Replace('/', '.');
    currentDateTime = currentDateTime.Replace(' ', '-');

    CloudBlockBlob blob = container.GetBlockBlobReference("fieldcount-" + currentDateTime + ".txt");

    try
    {
        blob.DeleteIfExists();
    }
    catch (Exception e)
    {
        log.Error(e.Message);
    }

    log.Info("Blob Container and Blob setup complete.");
    // END BLOB STORAGE SETUP

    // Store Count Data in Blob as JSON String
    log.Info("Saving Count Data JSON to Blob.");
    //using (var stream = new MemoryStream(Encoding.Default.GetBytes(fieldCountData), false))
    //{
    //    blob.UploadFromStream(stream, null, null);
    //}
    blob.UploadText(fieldCountData.ToString());
    log.Info("Count Data Saved to Blob as JSON.");


    // Deserialization from JSON
    // Help from: http://www.c-sharpcorner.com/article/json-serialization-and-deserialization-in-c-sharp/
    log.Info("Json String Contents: " + fieldCountData.ToString());

    // Deserialize Field Count Form Answers
    FieldFormAnswers fieldFormAnswers = new FieldFormAnswers();

    try
    {
        fieldFormAnswers = JsonConvert.DeserializeObject<FieldFormAnswers>(fieldCountData[0].formAnswers.ToString());
    }
    catch(Exception ex)
    {
        log.Error("Could not deserialize Json for FIELD FORM ANSWERS.  Details: " + ex.ToString());
    }

    // Deserialize List of Bird Counts
    List<BirdCount> birdCountList = new List<BirdCount>();

    try
    {
        birdCountList = JsonConvert.DeserializeObject<List<BirdCount>>(fieldCountData[1].birdCounts.ToString());
    }
    catch(Exception ex)
    {
        log.Error("Could not deserialize Json for BIRD COUNTS.  Details: " + ex.ToString());
    }


    // Send Email To Compiler, Notifying Him/Her That New Report Was Submitted
    try
    {
        SendNewFieldReportEmail(fieldFormAnswers, log);
        log.Info("New Report Notification Email Sent.");
    }
    catch (Exception ex)
    {
        log.Error("Could not send new report notification email.  Details: " + ex.ToString());
    }


    return fieldCountData == null
        ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass count data in the request body")
        : req.CreateResponse(HttpStatusCode.OK, "Deserialized JSON");
}