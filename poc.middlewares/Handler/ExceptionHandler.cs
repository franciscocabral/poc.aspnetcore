using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using poc.resources;
using System.Net;
using System.Threading.Tasks;

namespace poc.middlewares.Handler {

  public class ExceptionHandler {
    private readonly IStringLocalizer<ErrorMessages> _errorMessages;
    private readonly RequestDelegate _next;

    public ExceptionHandler(RequestDelegate next, IStringLocalizer<ErrorMessages> errorMessages) {
      _next = next;
      _errorMessages = errorMessages;
    }

    public async Task InvokeAsync(HttpContext context) {
      IExceptionHandlerFeature ex = context.Features.Get<IExceptionHandlerFeature>();

      if (ex != null) {
        //Log.Exception(ex);
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.Headers.Add("Content-Type", new[] { "application/json" });

        ErrorResponse err = new ErrorResponse() {
          Message = _errorMessages["InternalServerError"],
          Success = false,
        };

        await context.Response.WriteAsync(JsonConvert.SerializeObject(err));
        return;
      }

      await _next(context);
    }
  }

  public class ErrorResponse {
    public bool Success;
    public string Message;
  }
}
