using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Threading.Tasks;

namespace poc.middlewares.Handler {

  public class LocalizationHandler {
    private readonly RequestDelegate _next;

    private CultureInfo _culture;

    public LocalizationHandler(RequestDelegate next, string defaultCulture) {
      _culture = new CultureInfo(defaultCulture);
      _next = next;
    }

    public Task InvokeAsync(HttpContext context) {
      // Call the next delegate/middleware in the pipeline
      return this._next(context);
    }
  }
}
