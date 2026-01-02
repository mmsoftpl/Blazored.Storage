using System;
using System.Diagnostics.CodeAnalysis;
using Blazored.SessionStorage;
using Blazored.Storage.JsonConverters;
using Blazored.Storage.Serialization;
using Blazored.SessionStorage.TestExtensions;
using Microsoft.Extensions.DependencyInjection;
using Blazored.Storage;

namespace Bunit
{
    [ExcludeFromCodeCoverage]
    public static class BUnitSessionStorageTestExtensions

    {
        public static ISessionStorageService AddBlazoredSessionStorage(this TestContextBase context)
            => AddBlazoredSessionStorage(context, null);

        public static ISessionStorageService AddBlazoredSessionStorage(this TestContextBase context, Action<StorageOptions> configure)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            var StorageOptions = new StorageOptions();
            configure?.Invoke(StorageOptions);
            StorageOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());

            var localStorageService = new SessionStorageService(new InMemoryStorageProvider(), new SystemTextJsonSerializer(StorageOptions));
            context.Services.AddSingleton<ISessionStorageService>(localStorageService);

            return localStorageService;
        }
    }
}
