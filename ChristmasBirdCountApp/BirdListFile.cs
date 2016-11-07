// (c) 2016 Geneva College Senior Software Project Team
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Android.App;
using Android.Widget;
using Environment = Android.OS.Environment;

namespace ChristmasBirdCountApp
{
    public static class BirdListFile
    {
        public static string Directory { get; set; }
        public static string FilePath { get; set; }

        public static void CreateBirdListFile(List<BirdCount> birdList)
        {
            StringBuilder csvTextBuilder = new StringBuilder();
            string delimiter = ",";

            string[][] csvOutput = new string[birdList.Count][];

            for (int i = 0; i < birdList.Count; i++)
            {
                csvOutput[i] = new string[] { birdList[i].Name, birdList[i].Count.ToString() };
            }

            int listLength = csvOutput.Length;

            for (int index = 0; index < listLength; index++)
            {
                csvTextBuilder.AppendLine(string.Join(delimiter, csvOutput[index]));
            }

            SaveBirdListToFile(csvTextBuilder); // Save the attachment for later access when sending the email
        }

        public static void SaveBirdListToFile(StringBuilder csvText)
        {
            Directory = Environment.ExternalStorageDirectory.ToString();
            FilePath = Path.Combine(Directory, "BirdCountResults.csv");

            try
            {
                using (var streamWriter = new StreamWriter(FilePath, true))
                {
                    streamWriter.WriteLine(csvText.ToString());
                }
            }
            catch (Exception)
            {
                Toast.MakeText(Application.Context, "Could not create file!", ToastLength.Long).Show();
            }
        }
    }
}