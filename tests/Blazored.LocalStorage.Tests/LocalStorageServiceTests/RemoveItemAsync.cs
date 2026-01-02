using System;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.Storage.JsonConverters;
using Blazored.Storage.Serialization;
using Blazored.Storage;
using Blazored.LocalStorage.TestExtensions;
using Blazored.LocalStorage.Tests.TestAssets;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Blazored.LocalStorage.Tests.LocalStorageServiceTests
{
    public class RemoveItemAsync
    {
        private readonly LocalStorageService _sut;
        private readonly IStorageProvider _storageProvider;

        private const string Key = "testKey";

        public RemoveItemAsync()
        {
            var mockOptions = new Mock<IOptions<StorageOptions>>();
            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new TimespanJsonConverter());
            mockOptions.Setup(u => u.Value).Returns(new StorageOptions());
            IJsonSerializer serializer = new SystemTextJsonSerializer(mockOptions.Object);
            _storageProvider = new InMemoryStorageProvider();
            _sut = new LocalStorageService(_storageProvider, serializer);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public Task ThrowsArgumentNullException_When_KeyIsInvalid(string key)
        {
            // arrange / act
            var action = new Func<Task>(async () => await _sut.RemoveItemAsync(key));

            // assert
            return Assert.ThrowsAsync<ArgumentNullException>(action);
        }

        [Fact]
        public async Task RemovesItemFromStoreIfExists()
        {
            // Arrange
            var data = new TestObject(2, "Jane Smith");
            await _sut.SetItemAsync(Key, data);

            // Act
            await _sut.RemoveItemAsync(Key);

            // Assert
            Assert.Equal(0, await _storageProvider.LengthAsync());
        }
        
        [Fact]
        public async Task DoesNothingWhenItemDoesNotExistInStore()
        {
            // Act
            await _sut.RemoveItemAsync(Key);

            // Assert
            Assert.Equal(0, await _storageProvider.LengthAsync());
        }
    }
}
