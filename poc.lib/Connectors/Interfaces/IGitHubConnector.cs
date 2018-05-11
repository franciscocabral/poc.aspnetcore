using poc.lib.Connectors.Contracts;

namespace poc.lib.Connectors.Interfaces {

  public interface IGitHubConnector {

    GitHubUser GetUser(string userName);
  }
}
