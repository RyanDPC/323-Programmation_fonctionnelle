using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Swapi
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();
            var response = await client.GetAsync("https://swapi.dev/api/films/");
            var json = await response.Content.ReadAsStringAsync();

            var filmsData = JsonSerializer.Deserialize<FilmsResponse>(json);

            // 1.Quel est le film Star Wars dont le titre est le plus long 
            var longestTitleFilm = filmsData.Results
                                            .OrderByDescending(f => f.Title.Length)
                                            .First();
            Console.WriteLine("1. Quel est le film Star Wars dont le titre est le plus long ?");
            Console.WriteLine($"{longestTitleFilm.Title} ({longestTitleFilm.Title.Length} lettres)\n");

            // 2. Quel est le personnage qui est présent dans le plus de films 
            var allCharacters = filmsData.Results
            .Aggregate((acc, item) => acc.Characters.Count > item.Characters.Count ? acc : item)
            .Characters
            .First();

            var characterResponse = await client.GetAsync(allCharacters);
            var characterJson = await characterResponse.Content.ReadAsStringAsync();
            var character = JsonSerializer.Deserialize<Character>(characterJson);

            Console.WriteLine("2. Quel est le personnage qui est présent dans le plus de films ?");
            Console.WriteLine($"{character.Name} (présent dans {character.Films.Count()} films)\n");

            // 3.Quelle est la planète la plus peuplée 
            var planetTasks = filmsData.Results
                .SelectMany(f => f.Planets)
                .Distinct()
                .Select(async url =>
                {
                    var json = await (await client.GetAsync(url)).Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Planet>(json);
                })
                .ToList();

            var planets = await Task.WhenAll(planetTasks);

            var mostPopulated = planets
                .Where(p => long.TryParse(p.Population, out _))
                .OrderByDescending(p => long.Parse(p.Population))
                .First();

            Console.WriteLine("3.Quelle est la planète la plus peuplée ?");
            Console.WriteLine($"{mostPopulated.Name} {mostPopulated.Population}");

            Console.ReadLine();
        }
    }

    public class FilmsResponse
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("results")]
        public List<Film> Results { get; set; }
    }


    public class Film
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("characters")]
        public List<string> Characters { get; set; }
        [JsonPropertyName("planets")]
        public List<string> Planets { get; set; }
    }
    public class Planet
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("population")]
        public string Population { get; set; }
    }

    public class Character
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("films")]
        public List<string> Films { get; set; }
    }

}
