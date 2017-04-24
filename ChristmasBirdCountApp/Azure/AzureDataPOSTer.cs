using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Android.Content;
using Android.Widget;
using Newtonsoft.Json;

namespace ChristmasBirdCountApp.Azure
{
    class AzureDataPOSTer
    {
        public Context CurrentAppContext { get; private set; }
        public String CountDataJson { get; set; }

        public AzureDataPOSTer(Context appContext)
        {
            CurrentAppContext = appContext;
            CountDataJson = "";
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
            csvContents = BirdListFile.ReadFinalBirdCountsFromCSV();   // Get the latest bird list stored in the CSV

            // Serialize Bird Count List Contents as JSON
            // Help from: http://www.c-sharpcorner.com/article/json-serialization-and-deserialization-in-c-sharp/
            // Apply Starting Formatting for JSON Array
            CountDataJson += "]";

            foreach (BirdCount bird in csvContents)
            {
                CountDataJson += JsonConvert.SerializeObject(bird);
            }

            // Apply Ending Formatting for JSON Array
            CountDataJson += "]";
        }
    }
}