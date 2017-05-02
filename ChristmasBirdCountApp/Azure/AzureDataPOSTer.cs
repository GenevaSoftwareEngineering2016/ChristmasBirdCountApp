// (c) 2017 Geneva College Senior Software Project Team
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Android.Content;
using Android.Widget;
using ChristmasBirdCountApp.Forms;
using Newtonsoft.Json;

namespace ChristmasBirdCountApp.Azure
{
    class AzureDataPOSTer
    {
        public Context CurrentAppContext { get; private set; }

        public string CountFormType { get; set; }

        public FieldFormAnswers FieldFormAnswers { get; set; }
        public FeederFormAnswers FeederFormAnswers { get; set; }
        public CountWeekFormAnswers CountWeekFormAnswers { get; set; }

        public String CountDataJson { get; set; }

        public AzureDataPOSTer(Context appContext, string countFormType, FieldFormAnswers fieldFormAnswers = null, FeederFormAnswers feederFormAnswers = null, CountWeekFormAnswers countWeekFormAnswers = null)
        {
            CurrentAppContext = appContext;
            CountFormType = countFormType.ToLower();
            FieldFormAnswers = fieldFormAnswers;
            FeederFormAnswers = feederFormAnswers;
            CountWeekFormAnswers = countWeekFormAnswers;
            CountDataJson = "";
        }

        public bool PerformPostAgainstAzureFunctionApi()
        {
            try
            {
                CreateJsonFromCsvAndFormFields();
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
                    var url = "";

                    if (CountFormType == "field")
                    {
                        // Use Field Report API on Azure
                        url = "https://genevabirding.azurewebsites.net/api/ProcessAndSaveFieldReport?code=gvzl/Rk1IAWacyeQa7QHaDTfF8AupWK3RpnZkzjc3QIrQnFkikb4SA==";
                    }
                    else if (CountFormType == "feeder")
                    {
                        // Use Feeder Report API on Azure
                        url = "https://genevabirding.azurewebsites.net/api/ProcessAndSaveFeederReport?code=jFcwkNu9yXMxGy6JXVS6psmRUaYWMQddQojgqbn1Zlbs5/h2JHwjpw==";
                    }
                    else if (CountFormType == "countweek")
                    {
                        // Use Count Week Report API on Azure
                        url = "https://genevabirding.azurewebsites.net/api/ProcessAndSaveCountWeekReport?code=nF1RBAiYbPyQacxyp2UXfZ4vPbzy3CcleecX9V7su1KD9zmD4c9gKQ==";
                    }
                    else
                    {
                        url = "";   // The Count Form Type Was INVALID
                    }

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

        private void CreateJsonFromCsvAndFormFields()
        {
            // Serialize the Form Fields as JSON
            CountDataJson += "[{'formAnswers':";

            if (CountFormType == "field")
            {
                // Use Field Form Answers
                CountDataJson += JsonConvert.SerializeObject(FieldFormAnswers);
            }
            else if (CountFormType == "feeder")
            {
                // Use Feeder Form Answers
                CountDataJson += JsonConvert.SerializeObject(FeederFormAnswers);
            }
            else if (CountFormType == "countweek")
            {
                // Use Count Week Form Answers
                CountDataJson += JsonConvert.SerializeObject(CountWeekFormAnswers);
            }
            else
            {
                CountDataJson += "Invalid Form Answers";   // The Count Form Type Was INVALID
            }

            CountDataJson += "},";

            // Serialize Bird Count List Contents as JSON
            // Get the latest bird list stored in the CSV
            List<BirdCount> csvContents = new List<BirdCount>();
            csvContents = BirdListFile.ReadFinalBirdCountsFromCSV();

            // Help from: http://www.c-sharpcorner.com/article/json-serialization-and-deserialization-in-c-sharp/
            // Apply Starting Formatting for JSON Array
            CountDataJson += "{'birdCounts':[";

            foreach (BirdCount bird in csvContents)
            {
                CountDataJson += JsonConvert.SerializeObject(bird);
                CountDataJson += ",";
            }

            // Apply Ending Formatting for JSON Array
            CountDataJson.Remove(CountDataJson.LastIndexOf(","), 1);        // Remove the last comma that was not needed
            CountDataJson += "]}]";
        }
    }
}