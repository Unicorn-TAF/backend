![Nuget](https://img.shields.io/nuget/v/Unicorn.Backend?style=plastic) ![Nuget](https://img.shields.io/nuget/dt/Unicorn.Backend?style=plastic)

# Unicorn.Backend

Implementation of interaction with web services based on `HttpClient`.

* Clients for REST and SOAP services
* Responses matchers collection

Base custom REST API client example
```csharp
// Implementation of some dummy api client (should inherit RestClient)
public class DummyApiClient : RestClient
{
    // Initializing an instance of DummyApiClient calling base constructor with api base url.
    public DummyApiClient() : base("https://reqres.in")
    {
    }

    // Call get on "/api/users/{id}" endpoint and receive a response
    public RestResponse GetUser(int id) =>
        SendRequest(HttpMethod.Get, $"/api/users/{id}");
}
```

Perform assertions on call result
```csharp
// Make a call to get user info
RestResponse userResponse = new DummyApiClient().GetUser(2);

// Perform assertions on the result using build-in matchers 
Assert.That(userResponse, Response.HasStatusCode(HttpStatusCode.OK));
Assert.That(userResponse, Response.Rest.HasTokenWithValue("$.data.id", 2));
```

Work with sessions
```csharp
// Custom session should implement IServiceSession
public class CustomSession : IServiceSession
{
    private readonly string _token;
    
    // let's consider the token was already retrieved by some way
    public CustomSession(string token)
    {
        _token = token;
    }

    // This method from IServiceSession should be implemented 
    // here based on specifics of target service authorization 
    // initial HttpRequestMessage should be populated with session data.
    public void UpdateRequestWithSessionData(HttpRequestMessage request)
    {
        request.Headers.Add("Authorization", $"Api-Token {_token}");
    }
}

public class CustomApiClient : RestClient
{
    // Just need to init the client with the session and to set Session property
    public CustomApiClient(CustomSession customSession) : base("https://some-url")
    {
        Session = customSession;
    }
}
```

Files download
```csharp
RestClient client = new RestClient("https://some-url");
client.DownloadFile("/api/some-endpoint", "destination_directory_");
```

SOAP client usage example
```csharp
SoapClient client = new SoapClient("https://www.dataaccess.com");
SoapResponse response = client.Get("/webservicesserver/NumberConversion.wso/NumberToWords?ubiNum=234");
Assert.That(response, Response.HasStatusCode(HttpStatusCode.OK));
Assert.That(response, Response.ContentContains("<string>two hundred and thirty four </string>"));
```