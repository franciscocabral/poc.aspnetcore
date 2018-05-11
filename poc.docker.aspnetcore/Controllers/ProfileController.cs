using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using poc.lib.Connectors.Contracts;
using poc.lib.Connectors.Interfaces;
using poc.resources;
using System;

namespace poc.aspnetcore.Controllers {

  [Route("api/profile")]
  public class ProfileController : Controller {
    private readonly IGitHubConnector _gitHubConnector;
    private readonly IStringLocalizer<ValidationMessages> _validationMessages;

    public ProfileController(IGitHubConnector gitHubConnector, IStringLocalizer<ValidationMessages> validationMessages) {
      this._gitHubConnector = gitHubConnector;
      _validationMessages = validationMessages;
    }

    [HttpGet("{username}")]
    public IActionResult GetUser([FromRoute] string username) {
      GitHubUser user = this._gitHubConnector.GetUser(username);

      if (user != null) {
        return this.Ok(user);
      }

      return this.NotFound(new { Message = _validationMessages["InvalidUsername"].Value });
    }

    [HttpPost("{username}")]
    public IActionResult PostUser([FromRoute] string username) {
      throw new NotImplementedException();
    }
  }
}
