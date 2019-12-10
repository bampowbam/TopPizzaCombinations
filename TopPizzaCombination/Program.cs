using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TopPizzaCombination
{
    class Program
    {
        static void Main(string[] args)
        {
            DownloadFileFromNetwork("http://files.olo.com/pizzas.json");
            List<Pizza> _pizzas = ParseJsonFile("input.json");
            Dictionary<String, int> result = GetToppingCombinationRank(_pizzas);
            Console.WriteLine("___________________________________________________________");
            Console.WriteLine("Toppings                         |Frequency               |");
            if (result != null)
            {
                foreach (var item in result.OrderByDescending(x => x.Value).Take(20))
                {
                    Console.WriteLine($"{item.Key.Replace('|', ',').PadRight("_________________________________".Length, ' ')}|{item.Value.ToString().PadLeft("_________________________________".Length, ' ')}|");
                }
                Console.WriteLine("__________________________________________________________");
            }
            Console.Read();
        }

        private static Dictionary<string, int> GetToppingCombinationRank(List<Pizza> pizzas)
        {
            Dictionary<String, int> _result = new Dictionary<string, int>();

            foreach (Pizza item in pizzas)
            {
                if (_result.ContainsKey(String.Join("|", item.Toppings)))
                {
                    _result[String.Join("|", item.Toppings)]++;
                }
                else
                {
                    _result.Add(String.Join("|", item.Toppings), 1);
                }
            }

            return _result;
        }

        private static List<Pizza> ParseJsonFile(string file)
        {
            JsonSerializer serializer = new JsonSerializer();
            try
            {
                using (FileStream s = File.Open(file, FileMode.Open))
                using (StreamReader sr = new StreamReader(s))
                {
                    using (JsonReader reader = new JsonTextReader(sr))
                    {
                        return serializer.Deserialize<List<Pizza>>(reader);
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                return null;
            }
        }

        static void DownloadFileFromNetwork(string Url)
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(Url, "input.json");
            }
        }
    }
}
