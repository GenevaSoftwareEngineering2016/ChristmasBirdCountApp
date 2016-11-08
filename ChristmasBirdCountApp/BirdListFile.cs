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

            SaveBirdListToFile(csvTextBuilder); // Save the bird list for later loading data into app or access for attachment when sending email
            // NOTE: Bird count list is saved in .csv file in same order as displayed in app.  Will be read back into app "backwards" (unless a List<T>.Reverse() is used).
        }

        private static void SaveBirdListToFile(StringBuilder csvText)
        {
            Directory = Environment.ExternalStorageDirectory.ToString();
            FilePath = Path.Combine(Directory, "BirdCountResults.csv");

            try
            {
                using (StreamWriter streamWriter = new StreamWriter(FilePath, false))
                {
                    streamWriter.WriteLine(csvText.ToString());
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                Toast.MakeText(Application.Context, "Could not create file!", ToastLength.Long).Show();
            }
        }

        public static List<BirdCount> LoadBirdListFromFile()
        {
            List<BirdCount> loadedBirdList = new List<BirdCount>();
            Directory = Environment.ExternalStorageDirectory.ToString();
            FilePath = Path.Combine(Directory, "BirdCountResults.csv");

            try
            {
                if (File.Exists(FilePath))
                {
                    using (StreamReader fileReader = new StreamReader(File.OpenRead(FilePath)))
                    {
                        while (!fileReader.EndOfStream)
                        {
                            var line = fileReader.ReadLine();
                            var birdCountItem = line.Split(',');
                            if (birdCountItem[0] != "" && birdCountItem[0] != null)
                            {
                                loadedBirdList.Insert(0, new BirdCount() { Name = birdCountItem[0], Count = Convert.ToInt32(birdCountItem[1]) });
                            }
                            else { }
                        }
                    }
                }
                else
                {
                    // Create a new file, because one does not already exist.
                    using (StreamWriter streamWriter = new StreamWriter(File.Create(FilePath))){}
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                Toast.MakeText(Application.Context, "Could not load file or file does not exist!", ToastLength.Long).Show();
            }

            // HACK - ADC 11/07/2016
            // Invert the Bird List to Correct for Reading the .csv File "Backwards"
            loadedBirdList.Reverse();
            // END HACK - ADC 11/07/2016

            return loadedBirdList;
        }
    }
}