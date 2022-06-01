using System.Threading.Tasks;

namespace Demo.SR.PolyProject.API.Services
{
    public interface IWeatherService
    {
        Task<string> GetWeatherDetailsByCityName(string cityName);
    }
}
