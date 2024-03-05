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
        public static List<POS> GetPOSsFromJSON()
        {
            try
            {
                string fileName = "..\\..\\..\\StoreConfig.json";

                string jsonString = File.ReadAllText(fileName);
                var POSList = System.Text.Json.JsonSerializer.Deserialize<List<POS>>(jsonString);
                
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
    }
}
