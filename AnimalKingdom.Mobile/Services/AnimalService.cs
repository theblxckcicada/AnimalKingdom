using System.Net.Http.Headers;
using System.Net.Http.Json;
using AnimalKingdom.Shared.Models;
using Microsoft.Extensions.Configuration;

namespace AnimalKingdom.Mobile.Services
{
    public class AnimalService
    {
        private readonly string url;
        public event Action? AnimalsUpdated;

        public HttpClient httpClient;

        public HttpClient HttpClient { get; }

        public AnimalService(HttpClient httpClient, IConfiguration configuration)
        {
            // Only bypass SSL in debug mode
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            this.httpClient = new HttpClient(handler);
            HttpClient = httpClient;
            url = configuration.GetSection("ApiUrl").Value ?? "";

            //SetAccessToken(PublicClientSingleton.Instance.MSALClientHelper.AuthResult.AccessToken);
        }

        public void SetAccessToken(string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<Animal>> GetAnimals()
        {
            return await SendRequestAsync<List<Animal>>(() => httpClient.GetAsync(url)) ?? new List<Animal>();
        }

        public async Task<Animal?> GetAnimal(Guid id)
        {
            return await SendRequestAsync<Animal>(() => httpClient.GetAsync($"{url}/{id}"));
        }

        public async Task AddAnimal(Animal animal)
        {
            var result = await SendRequestAsync<Animal>(() => httpClient.PostAsJsonAsync(url, animal));
            AnimalsUpdated?.Invoke();
        }


        public async Task UpdateAnimal(Animal animal)
        {
            var result = await SendRequestAsync<Animal>(() => httpClient.PutAsJsonAsync($"{url}/{animal.Id}", animal));
            AnimalsUpdated?.Invoke();
        }

        public async Task DeleteAnimal(Guid id)
        {
            var result = await SendRequestAsync<Animal>(() => httpClient.DeleteAsync($"{url}/{id}"));
            AnimalsUpdated?.Invoke();
        }

        public async Task<List<Animal>?> Search(string searchValue)
        {
            // build an entity query
            EntityQuery query = new()
            {
                PageIndex = 0,
                PageSize = 10,
                Filter = searchValue,
                Sort = nameof(Animal.Name),
                SortDirection = "asc"
            };
            return (List<Animal>)(await SendRequestAsync<QueryEntitiesResponse<Animal>>(() => httpClient.PostAsJsonAsync($"{url}/query", query))).Entities;

        }

        private static async Task<T?> SendRequestAsync<T>(Func<Task<HttpResponseMessage>> requestFunc)
        {
            try
            {
                var response = await requestFunc();
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<T>();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return default;
            }
        }

    }
}
