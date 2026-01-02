using System;
using System.Diagnostics.CodeAnalysis;
using Blazored.Storage;
using Blazored.Storage.JsonConverters;
using Blazored.Storage.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Blazored.SessionStorage;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBlazoredSessionStorage(this IServiceCollection services)
        => AddBlazoredSessionStorage(services, null);

    public static IServiceCollection AddBlazoredSessionStorage(this IServiceCollection services, Action<StorageOptions> configure)
    {
        return services
            .AddScoped<IJsonSerializer, SystemTextJsonSerializer>()
            .AddScoped<IStorageProvider, BrowserStorageProvider>()
            .AddScoped<ISessionStorageService, SessionStorageService>()
            .AddScoped<ISyncSessionStorageService, SessionStorageService>()
            .Configure<StorageOptions>(configureOptions =>
            {
                configure?.Invoke(configureOptions);
                configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
            });
    }
    
    /// <summary>
    /// Registers the Blazored SessionStorage services as singletons. This should only be used in Blazor WebAssembly applications.
    /// Using this in Blazor Server applications will cause unexpected and potentially dangerous behaviour. 
    /// </summary>
    /// <returns></returns>
    public static IServiceCollection AddBlazoredSessionStorageAsSingleton(this IServiceCollection services)
        => AddBlazoredSessionStorageAsSingleton(services, null);
        
    /// <summary>
    /// Registers the Blazored SessionStorage services as singletons. This should only be used in Blazor WebAssembly applications.
    /// Using this in Blazor Server applications will cause unexpected and potentially dangerous behaviour. 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IServiceCollection AddBlazoredSessionStorageAsSingleton(this IServiceCollection services, Action<StorageOptions> configure)
    {
        return services
            .AddSingleton<IJsonSerializer, SystemTextJsonSerializer>()
            .AddSingleton<IStorageProvider, BrowserStorageProvider>()
            .AddSingleton<ISessionStorageService, SessionStorageService>()
            .AddSingleton<ISyncSessionStorageService, SessionStorageService>()
            .Configure<StorageOptions>(configureOptions =>
            {
                configure?.Invoke(configureOptions);
                configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
            });
    }
}
