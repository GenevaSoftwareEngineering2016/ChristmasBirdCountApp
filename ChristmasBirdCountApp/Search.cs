// (c) 2016 Geneva College Senior Software Project Team
using System.Collections.Generic;
using System.Linq;

namespace ChristmasBirdCountApp
{
    public static class Search
    {
        public static List<BirdCount> FilterBirdCountList(string filterTerm, List<BirdCount> workingBirdList)
        {
            List<BirdCount> narrowedBirdList;

            if (string.IsNullOrEmpty(filterTerm))
            {
                // If the user enters nothing in the "filter" (search) box, return entire 'working' bird list.
                narrowedBirdList = workingBirdList;
            }
            else
            {
                IEnumerable<BirdCount> narrowedBirds = workingBirdList.Where(birdName => birdName.Name.ToLower().Contains(filterTerm.ToLower()));

                narrowedBirdList = narrowedBirds.ToList();
            }

            return narrowedBirdList;
        }
    }
}