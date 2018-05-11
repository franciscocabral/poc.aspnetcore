using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace poc.lib {

  public static class IServiceCollectionExtension {

    public static IServiceCollection AddPocLibConnectors(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Singleton) {
      Assembly assembly = typeof(IServiceCollectionExtension).Assembly;
      services.AddBySufix(lifetime, assembly, "Connector");

      return services;
    }

    private static IServiceCollection AddBySufix(this IServiceCollection services, ServiceLifetime lifetime, Assembly assembly, string sufix) {
      List<Type> apiInterfaces = assembly.ExportedTypes.Where(x => x.IsInterface && x.Name.EndsWith(sufix)).ToList();
      List<Type> apiImplementations = assembly.ExportedTypes.Where(x => !x.IsInterface && !x.IsAbstract && x.Name.EndsWith(sufix)).ToList();

      foreach (Type @interface in apiInterfaces) {
        Type implementation = apiImplementations.FirstOrDefault(x => @interface.IsAssignableFrom(x));
        if (implementation == null) {
          continue;
        }

        switch (lifetime) {
          case ServiceLifetime.Singleton:
            services.AddSingleton(@interface, implementation);
            break;

          case ServiceLifetime.Scoped:
            services.AddScoped(@interface, implementation);
            break;

          case ServiceLifetime.Transient:
            services.AddTransient(@interface, implementation);
            break;

          default:
            continue;
        }
      }

      return services;
    }
  }
}
