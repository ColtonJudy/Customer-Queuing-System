using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace CustomerQueuingSystem
{
    internal static class Config
    {
        static readonly string fileName = "..\\..\\..\\StoreConfig.json";

        public static List<POS> GetPOSsFromJSON()
        {
            try
            {
                string jsonString = File.ReadAllText(fileName);
                var jobject = JsonNode.Parse(jsonString);
                var POSSection = jobject?["POSs"];
                var POSList = System.Text.Json.JsonSerializer.Deserialize<List<POS>>(POSSection);
                
                if(POSList != null)
                {
                    return POSList;

                }
                else
                {
                    return new List<POS>();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error reading or deserializing JSON: " + ex.Message);

                return new List<POS>();
            }
        }

        public static void SetShowStartScreen(bool willShow)
        {
            try
            {
                string jsonString = File.ReadAllText(fileName);
                var jobject = JsonNode.Parse(jsonString);
                jobject["ShowStartScreen"] = willShow;
                File.WriteAllText(fileName, jobject.ToString());
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error updating JSON: " + ex.Message);
            }
        }

        public static bool GetShowStartScreen()
        {
            try
            {
                string jsonString = File.ReadAllText(fileName);
                var jobject = JsonNode.Parse(jsonString);

                var showStartScreenSection = jobject?["ShowStartScreen"];
                bool showStartScreen = System.Text.Json.JsonSerializer.Deserialize<bool>(showStartScreenSection);
                
                return showStartScreen;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error reading or deserializing JSON: " + ex.Message);

                return true;
            }
        }
    }
}
