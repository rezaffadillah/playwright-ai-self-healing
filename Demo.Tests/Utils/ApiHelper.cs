using System.Net.Http;
using Newtonsoft.Json;

namespace SauceDemo.Tests.Utils;

public class ApiHelper
{
    public static async Task<bool> ValidateUser(string username)
    {
        var client = new HttpClient();

        var response = await client.GetAsync($"http://localhost:5292/api/user/{username}");

        if (!response.IsSuccessStatusCode)
            return false;

        var responseBody = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"API Response: {responseBody}");

        dynamic? json = JsonConvert.DeserializeObject(responseBody);

        if (json == null)
            return false;

        return json.status == "active";
    }
}