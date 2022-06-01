using Demo.SR.PolyProject.API.Models;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace Demo.SR.PolyProject.API.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<WeatherApiConfig> _weatherApiSettings;

        public WeatherService(HttpClient httpClient, IOptions<WeatherApiConfig> weatherApiSettings)
        {
            _httpClient=httpClient;
            _weatherApiSettings=weatherApiSettings;
        }
        public async Task<string> GetWeatherDetailsByCityName(string cityName)
        {
            var apiKey = _weatherApiSettings.Value.ApiKey;
            var url = $"?key={apiKey}&q={cityName}";
            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode==true?  await response.Content.ReadAsStringAsync(): null;
        }
    }
}
