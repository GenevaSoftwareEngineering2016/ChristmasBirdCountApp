using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using Android.Content;
using Android.Widget;

namespace ChristmasBirdCountApp.Azure
{
    class AzureDataPOSTer
    {
        public Context CurrentAppContext { get; private set; }
        public String CountDataJson { get; set; }

        public AzureDataPOSTer(Context appContext)
        {
            CurrentAppContext = appContext;
        }

        public bool PerformPostAgainstAzureFunctionApi()
        {
            try
            {
                CreateJsonFromCsv();
            }
            catch (Exception ex)
            {
                Toast.MakeText(CurrentAppContext, "Could not convert report to JSON.  Details: " + ex.ToString(), ToastLength.Long).Show();
                return false;
            }

            try
            {
                // HTTP POST to Azure Function
                using (var client = new HttpClient())
                {
                    var url = "https://genevabirding.azurewebsites.net/api/SendCountResultsEmail?code=QhLJtph6QlC4GICWFAJPlcX37WzDLauDGJo2TzFKLr4gbS8hh9d1lg==";

                    var postBody = new StringContent(CountDataJson, Encoding.UTF8, "application/json");

                    client.PostAsync(new Uri(url), postBody).Result.EnsureSuccessStatusCode();  // Send the HTTP POST to Azure Functions.
                }

                Toast.MakeText(CurrentAppContext, "Report sent.", ToastLength.Long).Show();
                return true;
            }
            catch (Exception ex)
            {
                Toast.MakeText(CurrentAppContext, "Could not send report.  Details: " + ex.ToString(), ToastLength.Long).Show();
                return false;
            }
        }

        private void CreateJsonFromCsv()
        {
            List<BirdCount> csvContents = new List<BirdCount>();
            csvContents = BirdListFile.LoadWorkingBirdListFromFile();   // Get the latest bird list stored in the CSV

            // Serialize Bird Count List Contents as JSON
            // Help from: http://www.c-sharpcorner.com/article/json-serialization-and-deserialization-in-c-sharp/
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(BirdCount));
            using (MemoryStream streamObject = new MemoryStream())
            {
                foreach (BirdCount bird in csvContents)
                {
                    jsonSerializer.WriteObject(streamObject, bird);
                }

                streamObject.Position = 0;

                using (StreamReader reader = new StreamReader(streamObject))
                {
                    CountDataJson = reader.ReadToEnd();
                }
            }
        }
    }
}