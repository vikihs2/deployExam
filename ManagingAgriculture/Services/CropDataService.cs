using System;
using System.Collections.Generic;
using System.Linq;

namespace ManagingAgriculture.Services
{
    public class CropDataService
    {
        public static readonly Dictionary<string, List<string>> CropCategories = new Dictionary<string, List<string>>
        {
            { "🌾 Grains & Cereals", new List<string> { "Wheat", "Corn (Maize)", "Rice", "Barley", "Oats", "Rye", "Sorghum", "Millet" } },
            { "🥔 Root & Tuber Crops", new List<string> { "Potato", "Sweet Potato", "Carrot", "Beetroot", "Turnip", "Radish", "Cassava" } },
            { "🍅 Vegetables (Very Common)", new List<string> { "Tomato", "Cucumber", "Bell Pepper", "Chili Pepper", "Eggplant", "Onion", "Garlic", "Lettuce", "Spinach", "Cabbage", "Broccoli", "Cauliflower", "Zucchini" } },
            { "🌱 Legumes", new List<string> { "Beans (Green Beans)", "Peas", "Lentils", "Chickpeas", "Soybean" } },
            { "🍓 Fruits (Field / Greenhouse)", new List<string> { "Strawberry", "Watermelon", "Melon", "Pumpkin", "Squash" } },
            { "🌿 Industrial / Oil Crops", new List<string> { "Sunflower", "Rapeseed (Canola)", "Cotton", "Sugar Beet", "Sugarcane" } },
            { "🌿 Herbs (Common & Useful)", new List<string> { "Basil", "Parsley", "Dill", "Mint", "Oregano", "Thyme" } },
            { "🌲 Perennial / Special", new List<string> { "Alfalfa", "Clover", "Tobacco" } }
        };

        public static List<string> GetAllCrops()
        {
            return CropCategories.Values.SelectMany(x => x).OrderBy(x => x).ToList();
        }

        // Returns a suitability score (0-100) and a message based on simple rules
        public (int Score, string Message) CalculateGrowthSuitability(string cropType, string soilType, decimal? temp, bool isIndoor, int? waterFreq, string sunExposure)
        {
            int score = 100;
            List<string> issues = new List<string>();

            // 1. Temperature Check (Generic logic for demo)
            // Most crops like 15-30 C.
            if (temp.HasValue)
            {
                if (temp < 0) { score -= 40; issues.Add("Too cold/freezing!"); }
                else if (temp < 10) { score -= 20; issues.Add("Temperature is low for growth."); }
                else if (temp > 35) { score -= 20; issues.Add("Temperature is too high."); }
            }

            // 2. Soil Check
            // Root crops prefer Sandy/Loamy.
            // Vegetables prefer Loamy.
            if (!string.IsNullOrEmpty(soilType) && !string.IsNullOrEmpty(cropType))
            {
                var lowerCrop = cropType.ToLower();
                var lowerSoil = soilType.ToLower();

                if (lowerCrop.Contains("carrot") || lowerCrop.Contains("potato") || lowerCrop.Contains("radish"))
                {
                   if (lowerSoil.Contains("clay")) { score -= 30; issues.Add("Clay soil restricts root growth for this crop."); }
                }
            }

            // 3. Sunlight
            if (!string.IsNullOrEmpty(sunExposure))
            {
                if (sunExposure.Contains("Shade") && (cropType.Contains("Tomato") || cropType.Contains("Pepper") || cropType.Contains("Corn")))
                {
                    score -= 30; issues.Add("This crop needs more sun.");
                }
            }

            // 4. Watering
            if (waterFreq.HasValue)
            {
                if (waterFreq > 14) { score -= 10; issues.Add("Watering frequency seems too low."); }
                if (waterFreq == 0) { score -= 10; issues.Add("Need to water at least once."); }
            }

             // Cap score
            if (score < 0) score = 0;

            string msg = issues.Count > 0 ? string.Join(" ", issues) : "Conditions look excellent.";
            return (score, msg);
        }
        
        public int CalculateTimeProgress(DateTime planted, DateTime? harvest)
        {
            if (harvest == null) return 0;
            if (planted > DateTime.Now) return 0; // Future planting
            
            double totalDays = (harvest.Value - planted).TotalDays;
            double elapsed = (DateTime.Now - planted).TotalDays;

            if (totalDays <= 0) return 100; // instant? or error.
            
            double percent = (elapsed / totalDays) * 100.0;
            
            if (percent < 0) return 0;
            if (percent > 100) return 100;
            
            return (int)percent;
        }
    }
}
