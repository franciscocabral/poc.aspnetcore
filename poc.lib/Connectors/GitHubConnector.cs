using Microsoft.Extensions.Configuration;
using poc.lib.Connectors.Contracts;
using poc.lib.Connectors.Interfaces;
using RestSharp;
using System;

namespace poc.lib.Connectors {

  public class GitHubConnector : IGitHubConnector {
    private readonly IRestClient _client;

    public GitHubConnector(IConfiguration settings) {
      this._client = new RestClient(new Uri(settings.GetSection("Settings")["GitHub"]));
    }

    public GitHubUser GetUser(string userName) {
      RestRequest request = new RestRequest("users/{user}", Method.GET);
      request.AddUrlSegment("user", userName);

      request.AddHeader("accept", "application/json");
      request.AddHeader("accept-language", "en-Us");

      IRestResponse<GitHubUser> response = _client.Execute<GitHubUser>(request);

      if (!response.IsSuccessful) {
        return null;
      }

      GitHubUser user = response.Data;
      return user;
    }
  }
}
