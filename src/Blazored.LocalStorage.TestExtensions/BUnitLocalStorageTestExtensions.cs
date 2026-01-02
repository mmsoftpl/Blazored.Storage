using System;
using System.Diagnostics.CodeAnalysis;
using Blazored.LocalStorage;
using Blazored.Storage.JsonConverters;
using Blazored.Storage.Serialization;
using Blazored.LocalStorage.TestExtensions;
using Microsoft.Extensions.DependencyInjection;
using Blazored.Storage;

namespace Bunit
{
    [ExcludeFromCodeCoverage]
    public static class BUnitLocalStorageTestExtensions
    {
        public static ILocalStorageService AddBlazoredLocalStorage(this TestContextBase context)
            => AddBlazoredLocalStorage(context, null);

        public static ILocalStorageService AddBlazoredLocalStorage(this TestContextBase context, Action<StorageOptions>? configure)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            var StorageOptions = new StorageOptions();
            configure?.Invoke(StorageOptions);
            StorageOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());

            var localStorageService = new LocalStorageService(new InMemoryStorageProvider(), new SystemTextJsonSerializer(StorageOptions));
            context.Services.AddSingleton<ILocalStorageService>(localStorageService);

            return localStorageService;
        }
    }
}
