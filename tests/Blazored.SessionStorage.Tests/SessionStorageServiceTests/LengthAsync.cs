using System.Text.Json;
using System.Threading.Tasks;
using Blazored.Storage.JsonConverters;
using Blazored.Storage;
using Blazored.SessionStorage.TestExtensions;
using Blazored.SessionStorage.Tests.TestAssets;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using Blazored.Storage.Serialization;

namespace Blazored.SessionStorage.Tests.SessionStorageServiceTests;

public class LengthAsync
{
    private readonly SessionStorageService _sut;

    public LengthAsync()
    {
        var mockOptions = new Mock<IOptions<StorageOptions>>();
        var jsonOptions = new JsonSerializerOptions();
        jsonOptions.Converters.Add(new TimespanJsonConverter());
        mockOptions.Setup(u => u.Value).Returns(new StorageOptions());
        IJsonSerializer serializer = new SystemTextJsonSerializer(mockOptions.Object);
        IStorageProvider storageProvider = new InMemoryStorageProvider();
        _sut = new SessionStorageService(storageProvider, serializer);
    }

    [Fact]
    public async Task ReturnsZeroWhenStoreIsEmpty()
    {
        // Act
        var itemCount = await _sut.LengthAsync();

        // Assert
        Assert.Equal(0, itemCount);
    }

    [Fact]
    public async Task ReturnsNumberOfItemsInStore()
    {
        // Arrange
        var item1 = new TestObject(1, "Jane Smith");
        var item2 = new TestObject(2, "John Smith");
            
        await _sut.SetItemAsync("Item1", item1);
        await _sut.SetItemAsync("Item2", item2);

        // Act
        var itemCount = await _sut.LengthAsync();

        // Assert
        Assert.Equal(2, itemCount);
    }
}
