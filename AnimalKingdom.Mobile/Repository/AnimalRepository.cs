using System.Net.Http.Json;
using AnimalKingdom.Shared.Models;

namespace AnimalKingdom.Mobile.Repository
{
    public class AnimalRepository
    {
        private readonly string url = "https://10.0.2.2:7148/api/animal";
        public event Action? AnimalsUpdated;

        public HttpClient httpClient;
        public AnimalRepository(HttpClient httpClient)
        {
#if DEBUG
            // Only bypass SSL in debug mode
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            this.httpClient = new HttpClient(handler);
#else
        _httpClient = httpClient;
#endif
        }

        public async Task<List<Animal>> GetAnimals()
        {
            // query the animals from the api endpoint 
            try
            {

                HttpResponseMessage response = await httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadFromJsonAsync<List<Animal>>();
                return responseBody!;

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
            return [];
        }

        public async Task<Animal?> GetAnimal(Guid id)
        {
            // query the animals from the api endpoint 
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"{url}/{id}");

                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadFromJsonAsync<Animal>();
                return responseBody;

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
            return null;
        }

        public async Task AddAnimal(Animal animal)
        {
            // query the animals from the api endpoint 
            try
            {
                HttpResponseMessage response = await httpClient.PostAsJsonAsync(url, animal);

                response.EnsureSuccessStatusCode();

                _ = await response.Content.ReadFromJsonAsync<Animal>();


            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }

            AnimalsUpdated?.Invoke();
        }

        public async Task UpdateAnimal(Animal animal)
        {
            // find the animal 
            try
            {
                HttpResponseMessage response = await httpClient.PutAsJsonAsync(url, animal);

                response.EnsureSuccessStatusCode();

                _ = await response.Content.ReadFromJsonAsync<Animal>();

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }

        }

        public async Task DeleteAnimal(Guid id)
        {
            try
            {
                HttpResponseMessage response = await httpClient.DeleteAsync($"{url}/{id}");

                response.EnsureSuccessStatusCode();

                _ = await response.Content.ReadFromJsonAsync<Animal>();


            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }

        }

        public async Task<List<Animal>> Search(string searchValue)
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

            // query the api 
            try
            {

                HttpResponseMessage response = await httpClient.PostAsJsonAsync(url, query);

                response.EnsureSuccessStatusCode();

                var queryResponse = await response.Content.ReadFromJsonAsync<QueryEntitiesResponse<Animal>>();
                return (List<Animal>)queryResponse!.Entities;

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
            return [];

        }
    }
}
