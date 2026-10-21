using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace ApiFetcher
{
    class Program
    {
        // Step 2: Use appropriate libraries (HttpClient is built into C#)
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            Console.WriteLine("Fetching a joke from the API...\n");

            // Call the method to fetch and display the data
            await FetchAndDisplayJokeAsync();
        }

        static async Task FetchAndDisplayJokeAsync()
        {
            // Step 1: Select a public API (Official Joke API)
            string apiUrl = "https://official-joke-api.appspot.com/random_joke";

            try
            {
                // Fetch the data from the API
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                // Ensure the request was successful (Status 200 OK)
                response.EnsureSuccessStatusCode();

                // Read the response content as a string
                string responseBody = await response.Content.ReadAsStringAsync();

                // Step 3: Parse the fetched data to extract required information
                // We use JsonSerializer to map the JSON to a C# class
                Console.WriteLine("RAW JSON: " + responseBody);
                JokeResponse jokeData = JsonSerializer.Deserialize<JokeResponse>(responseBody);

                // Step 4: Display the extracted data in a clear, user-friendly format
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("Here is your joke:");
                Console.WriteLine($"❓ Setup: {jokeData.Setup}");
                Console.WriteLine($"🎉 Punchline: {jokeData.Punchline}");
                Console.WriteLine("--------------------------------------------------");
            }
            catch (HttpRequestException e)
            {
                // Step 5: Test the program / Handle errors gracefully
                Console.WriteLine("\nError fetching data from the API.");
                Console.WriteLine($"Message: {e.Message}");
            }
            catch (JsonException e)
            {
                // Handle cases where the API returns data in an unexpected format
                Console.WriteLine("\nError parsing the API response.");
                Console.WriteLine($"Message: {e.Message}");
            }
        }
    }

    // This class defines the structure of the JSON response we expect from the API.
    // The property names must match the JSON keys (case-insensitive by default in System.Text.Json).
    public class JokeResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("setup")]
        public string Setup { get; set; }

        [JsonPropertyName("punchline")]
        public string Punchline { get; set; }
    }
}

