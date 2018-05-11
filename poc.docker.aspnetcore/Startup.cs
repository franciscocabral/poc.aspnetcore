using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using poc.lib;
using poc.middlewares;
using System.Collections.Generic;

namespace poc.aspnetcore {

  public class Startup {
    public IConfiguration Configuration { get; }

    public Startup(IHostingEnvironment env, IConfiguration configuration) {
      Configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services) {
      services.AddLocalization(options => { options.ResourcesPath = ""; });

      //By default, Allow all CORS
      services.AddCors(options => {
        options.AddPolicy("AllowAll", builder => {
          builder
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin()
              .AllowCredentials();
        });
      });

      services.AddMvc(options => {
        IStringLocalizerFactory localizer = services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
        IStringLocalizer dictionary = localizer.Create("ModelBindingMessages", "poc.resources");

        options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor((x) =>
            dictionary["missingBindRequired", x]);
        options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) =>
            dictionary["attemptedValueIsInvalid", x, y]);
      })
          .AddDataAnnotationsLocalization()
          .AddJsonOptions(options => {
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            options.SerializerSettings.ContractResolver = new DefaultContractResolver {
              //NamingStrategy = new CamelCaseNamingStrategy(),
              NamingStrategy = new SnakeCaseNamingStrategy(),
            };
          });

      //Dependency injection from Poc.Lib as singliton
      services.AddPocLibConnectors(ServiceLifetime.Singleton);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      }

      app.UseLocalizationHandler("en-US", new List<string> { "en-US", "pt-BR" });
      app.UseCustomExceptionHandler();

      app.UseCors("AllowAll");
      app.UseMvc();
    }
  }
}
