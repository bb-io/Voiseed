using Apps.Voiseed.Constants;
using Apps.Voiseed.Models;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Voiseed.Api;

public class VoiseedClient : BlackBirdRestClient
{
    public VoiseedClient(IEnumerable<AuthenticationCredentialsProvider> creds) : base(new()
    {
        BaseUrl = new Uri("https://api.revoiceit.com/"),
        MaxTimeout = 300000
    })
    {
        var token = GetToken(creds.First(x=>x.KeyName == CredsNames.ClientId).Value, 
            creds.First(x => x.KeyName == CredsNames.ClientSecret).Value).GetAwaiter().GetResult();
        this.AddDefaultHeader("Authorization", token);
    }

    public override async Task<T> ExecuteWithErrorHandling<T>(RestRequest request)
    {
        string content = (await ExecuteWithErrorHandling(request)).Content;
        T val = JsonConvert.DeserializeObject<T>(content, JsonSettings);
        if (val == null)
        {
            throw new Exception($"Could not parse {content} to {typeof(T)}");
        }

        return val;
    }

    public override async Task<RestResponse> ExecuteWithErrorHandling(RestRequest request)
    {
        RestResponse restResponse = await ExecuteAsync(request);
        if (!restResponse.IsSuccessStatusCode)
        {
            throw ConfigureErrorException(restResponse);
        }

        return restResponse;
    }

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        throw new PluginApplicationException(response.Content);
    }

    private async Task<string> GetToken(string clientId, string clientSecret)
    {
        var request = new RestRequest("/auth/token", Method.Post);
        request.AddBody(new { clientId= clientId, clientSecret= clientSecret });


        var response = await ExecuteWithErrorHandling<TokenDto>(request);

        return response.AccessToken;
    } 

}