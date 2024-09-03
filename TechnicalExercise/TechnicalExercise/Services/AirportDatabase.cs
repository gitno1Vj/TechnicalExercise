using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalExercise.Models;

namespace TechnicalExercise.Services
{
    internal class AirportDatabase : IAirportDatabase
    {
        public async Task<IList<AirportModel>> GetByCountry(string countryCode)
        {
            var queryPart = $"country={countryCode}";
            var raw = await GetRawContent(queryPart);
            return JsonConvert.DeserializeObject<List<AirportModel>>(raw ?? "");
        }

        public IList<AirportModel> LondonAirports
        {
            get
            {
                var queryPart = "v1/airports?country=GB&region=England&city=London";
                var raw = GetRawContent(queryPart);
                return JsonConvert.DeserializeObject<List<AirportModel>>(raw.Result ?? "");
            }
        }

        private static async Task<string> GetRawContent(string queryPart)
        {
            var client = new RestClient("https://airports-by-api-ninjas.p.rapidapi.com/");
            var request = new RestRequest($"v1/airports?{queryPart}");
            request.AddHeader("x-rapidapi-key", "368669ef32msh88075ec3406582fp1bc8c3jsn2585f16f8d20");
            request.AddHeader("x-rapidapi-host", "airports-by-api-ninjas.p.rapidapi.com");
            RestResponse response = await client.ExecuteAsync(request);
            return response.Content;
        }
    }
}
