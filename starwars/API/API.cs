using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApiData{

        public interface IApiDataReader{
             string BaseAddress { get; init;}
             Task<string> Read(string requestUri);
        }
        public class ApiDataReader : IApiDataReader{
        public string BaseAddress { get; init;}

        public ApiDataReader(string baseAddress){
            BaseAddress = baseAddress;
        }
        public async Task<string> Read(string requestUri){

using var handler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
};
            using var client = new HttpClient(handler);
            HttpResponseMessage response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        
    }

    public   static  class EncodeHander {
        public static string JSONSerialize(Root list){
            return JsonSerializer.Serialize(list);
        }
        public static Root JSONDeserialize(string data){
            return JsonSerializer.Deserialize<Root>(data)?? new Root();
        }     
    }

    public class StarWarsData{
        public  StarWarsRow[] TableData {get; init;}
        public StarWarsData( Root dataList){
            TableData = dataList.results
            .Select( item => {
                bool _;
                _ = int.TryParse(item.rotation_period, out int rotation);
                _ = int.TryParse(item.diameter, out int diam);
                _ = int.TryParse(item.surface_water, out int water);
                _ = int.TryParse(item.population, out int pop);
                return new StarWarsRow{
                 name  = item.name,
                 rotation_period = rotation,
                 diameter  =  diam,
                 surface_water = water,
                 population = pop
                };
            })
            .ToArray();
        }
    }

    public readonly record struct StarWarsRow(string name, int rotation_period,  int diameter,  int surface_water, int population);


    public record Result(
        [property: JsonPropertyName("name")] string name,
        [property: JsonPropertyName("rotation_period")] string rotation_period,
        [property: JsonPropertyName("orbital_period")] string orbital_period,
        [property: JsonPropertyName("diameter")] string diameter,
        [property: JsonPropertyName("climate")] string climate,
        [property: JsonPropertyName("gravity")] string gravity,
        [property: JsonPropertyName("terrain")] string terrain,
        [property: JsonPropertyName("surface_water")] string surface_water,
        [property: JsonPropertyName("population")] string population,
        [property: JsonPropertyName("residents")] IReadOnlyList<string> residents,
        [property: JsonPropertyName("films")] IReadOnlyList<string> films,
        [property: JsonPropertyName("created")] DateTime created,
        [property: JsonPropertyName("edited")] DateTime edited,
        [property: JsonPropertyName("url")] string url
    );

    public record Root(
        [property: JsonPropertyName("count")] int count,
        [property: JsonPropertyName("next")] string next,
        [property: JsonPropertyName("previous")] object previous,
        [property: JsonPropertyName("results")] IReadOnlyList<Result> results
    ){
    // Parameterless constructor initializing fields to default values
    public Root() : this(default, string.Empty, default, Array.Empty<Result>()) { }
};



}