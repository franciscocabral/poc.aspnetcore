using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using poc.middlewares.Handler;
using System.Collections.Generic;
using System.Globalization;

namespace poc.middlewares {

  public static class IApplicationBuilderExtension {

    public static IApplicationBuilder UseLocalizationHandler(this IApplicationBuilder app,
        string defaultCulture, List<string> supportedCultures) {
      List<CultureInfo> cultures = new List<CultureInfo>();
      supportedCultures.ForEach(c => cultures.Add(new CultureInfo(c)));

      app.UseRequestLocalization(new RequestLocalizationOptions() {
        DefaultRequestCulture = new RequestCulture(new CultureInfo(defaultCulture)),
        SupportedCultures = cultures,
        SupportedUICultures = cultures
      });

      app.UseMiddleware<LocalizationHandler>(defaultCulture);

      return app;
    }

    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app) {
      app.UseExceptionHandler(config => config.UseMiddleware<ExceptionHandler>());
      return app;
    }
  }
}
