
using Azure;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProductManagement.Functions;

public class Function1
{
    private readonly ILogger<Function1> _logger;
    private readonly HttpClient _httpClient;

    public Function1(
        ILogger<Function1> logger,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient(); //This creates an HTTP client that we use to send requests.
    }

    [Function("GetProducts")]
    public async Task<IActionResult> Run( //asyn-- allows the method to perform asynchronous operations without blocking its current thread while waiting for an operation to complete
        [HttpTrigger(AuthorizationLevel.Function, "get")]
        HttpRequest req)
    {
        int categoryId = 2;

        string apiUrl =
            $"https://localhost:7113/api/categories/{categoryId}/products";

        var response = await _httpClient.GetAsync(apiUrl);

        var result = await response.Content.ReadAsStringAsync();

        return new ContentResult
        {
            Content = result,
            ContentType = "application/json", //This tells the caller what type of content we're returning.JSON data
            StatusCode = (int)response.StatusCode
        };
    }
}
//IactionResult -represents the HTTP response that your method will return.For example, your method might return: HTTP 200 – Success HTTP 404 – Not FoundHTTP 500 – Server Error
//Task represents an operation that may complete in the future.
//Anonymous	Anyone who can access the URL can call it; no function key required.Function Caller must provide a valid function key.Admin   Requires the host-level master key.
//Because we want to protect our Azure Function from being called by everyone without authorization.
//HttpRequest represents the incoming HTTP request received by your Azure Function.
//the $ symbol is called string interpolation.
//getasyn->Because we want to send a GET request to your existing ProductManagement API.
//await	Waits for the HTTP operation to finish without blocking the thread
//response Stores the HTTP response received from the API
/*response.Conten ->Remember, response contains the complete HTTP response from your API.
/But we only want the response body, response.Content accesses that body.*/
//ReadAsStringAsync()->This reads the response body and converts it into a string.

    