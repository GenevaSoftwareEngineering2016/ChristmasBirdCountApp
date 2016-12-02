// (c) 2016 Geneva College Senior Software Project Team
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content.Res;
using Android.Widget;
using Environment = Android.OS.Environment;

namespace ChristmasBirdCountApp
{
    public static class BirdListFile
    {
        public static string Directory { get; set; }
        public static string FilePath { get; set; }

        public static void CreateWorkingBirdListFile(List<BirdCount> masterBirdList, List<BirdCount> workingBirdList)
        {
            StringBuilder csvTextBuilder = new StringBuilder();
            string delimiter = ",";

            // Compare birdList with "Master" and use "Master" below.
            // The 'finalBirdList' uses 'masterBirdList' to get all known birds (including 0 counts),
            // And updates counts for birds where user added sightings in the app.
            // This 'finalBirdList' is used for saving to disk and reporting in the email.
            List<BirdCount> finalBirdList = UpdateWorkingBirdListFromMaster(masterBirdList, workingBirdList);

            string[][] csvOutput = new string[finalBirdList.Count][];

            for (int i = 0; i < finalBirdList.Count; i++)
            {
                csvOutput[i] = new string[] { finalBirdList[i].Name, finalBirdList[i].Count.ToString() };
            }

            int listLength = csvOutput.Length;

            for (int index = 0; index < listLength; index++)
            {
                csvTextBuilder.AppendLine(string.Join(delimiter, csvOutput[index]));
            }

            SaveWorkingBirdListToFile(csvTextBuilder); // Save the bird list for later loading data into app or access for attachment when sending email
            // NOTE: Bird count list is saved in .csv file in same order as displayed in app.  Will be read back into app "backwards" (unless a List<T>.Reverse() is used).
        }

        private static void SaveWorkingBirdListToFile(StringBuilder csvText)
        {
            Directory = Environment.ExternalStorageDirectory.ToString();
            FilePath = Path.Combine(Directory, "Bird Count Results.csv");

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

        public static List<BirdCount> LoadWorkingBirdListFromFile()
        {
            List<BirdCount> loadedBirdList = new List<BirdCount>();
            Directory = Environment.ExternalStorageDirectory.ToString();
            FilePath = Path.Combine(Directory, "Bird Count Results.csv");

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
                                // Only add birds to list if their count is > 0 because we do not want to display all birds on screen
                                // We only want to display birds on screen that have been added (i.e. have a count > 0)
                                if (Convert.ToInt32(birdCountItem[1]) > 0)
                                {
                                    loadedBirdList.Insert(0, new BirdCount() { Name = birdCountItem[0], Count = Convert.ToInt32(birdCountItem[1]) });
                                }
                                else { /* Skip over this bird, becuase the user had not "added" it to the list (i.e. count = 0) */ }
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

            // Invert the Bird List to Correct for Reading the .csv File "Backwards"
            loadedBirdList.Reverse();

            return loadedBirdList;
        }

        public static List<BirdCount> LoadMasterBirdList(Android.Content.Context appContext)
        {
            List<BirdCount> loadedMasterBirdList = new List<BirdCount>();
            AssetManager appAssets = appContext.Assets;     // Master bird list is included as an asset in the project

            try
            {
                using (Stream fileStream = appAssets.Open("Simplified ABA Checklist.csv"))
                {
                    using (StreamReader fileReader = new StreamReader(fileStream))
                    {
                        while (!fileReader.EndOfStream)
                        {
                            var line = fileReader.ReadLine();
                            var birdCountItem = line.Split(',');
                            if (birdCountItem[0] != "" && birdCountItem[0] != null)
                            {
                                loadedMasterBirdList.Insert(0, new BirdCount() { Name = birdCountItem[0], Count = Convert.ToInt32(birdCountItem[1]) });
                            }
                            else { }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                Toast.MakeText(Application.Context, "Could not find master bird list!", ToastLength.Long).Show();
            }

            loadedMasterBirdList.Reverse(); // Reverse list so that all items appear in correct order

            return loadedMasterBirdList;
        }

        public static List<BirdCount> UpdateWorkingBirdListFromMaster(List<BirdCount> masterBirdList, List<BirdCount> workingBirdList)
        {
            // Update the "working" bird list from the "Master" bird list.
            // The "Master" bird list is only ever changed when a new list is included with the app in an app update.

            List<BirdCount> updatedWorkingBirdList = masterBirdList;

            foreach (BirdCount updatedBird in updatedWorkingBirdList)
            {
                // See where items in "workingBirdList" match the "updatedWorkingBirdList," and fill in those items
                foreach (BirdCount workingBird in workingBirdList)
                {
                    if (workingBird.Name == updatedBird.Name)
                    {
                        updatedBird.Count = workingBird.Count;
                        break;
                    }
                    else { /* Have not yet found a matching bird in the "updatedWorkingBirdList" */ }
                }
            }

            return updatedWorkingBirdList;
        }
    }
}