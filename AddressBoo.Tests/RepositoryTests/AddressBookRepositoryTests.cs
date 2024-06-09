using AddressBook.API.Models;
using AddressBook.API.Repositories;

namespace AddressBoo.Tests.RepositoryTests;

public class AddressBookRepositoryTests
{
    private readonly AddressBookRepository _repository;

    public AddressBookRepositoryTests()
    {
        _repository = new AddressBookRepository();
    }

    [Fact]
    public void AddAdressBook_ShouldReturnSuccess_WhenValidAddressBook()
    {
        var addressBook = new AddressBookModel
        {
            Id = Guid.NewGuid(),
            AddressLine1 = "123 Main St",
            AddressLine2 = "Apt 1",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA",
            State = "CA",
            Phone = "555-1234"
        };

        var result = _repository.AddAddressBook(addressBook);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(addressBook, result.Data);
    }

    [Fact]
    public void GetAllAddressBooks_ShouldReturnAllAddressBooks()
    {
        var addressBook1 = new AddressBookModel
        {
            Id = Guid.NewGuid(),
            AddressLine1 = "123 Main St",
            AddressLine2 = "Apt 1",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA",
            State = "CA",
            Phone = "555-1234"
        };
        var addressBook2 = new AddressBookModel
        {
            Id = Guid.NewGuid(),
            AddressLine1 = "456 Elm St",
            AddressLine2 = "Apt 2",
            City = "Othertown",
            ZipCode = "67890",
            Country = "USA",
            State = "NY",
            Phone = "555-5678"
        };

        _repository.AddAddressBook(addressBook1);
        _repository.AddAddressBook(addressBook2);

        var result = _repository.GetAllAddressBooks();

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Count());
    }

    [Fact]
    public void GetAddressBook_ShouldReturnSuccess_WhenAddressBookExists()
    {
        var addressBook = new AddressBookModel
        {
            Id = Guid.NewGuid(),
            AddressLine1 = "123 Main St",
            AddressLine2 = "Apt 1",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA",
            State = "CA",
            Phone = "555-1234"
        };

        _repository.AddAddressBook(addressBook);

        var result = _repository.GetAddressBook(addressBook.Id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(addressBook, result.Data);
    }

    [Fact]
    public void GetAddressBook_ShouldReturnFailure_WhenAddressBookDoesNotExist()
    {
        var result = _repository.GetAddressBook(Guid.NewGuid());

        Assert.False(result.IsSuccess);
        Assert.Null(result.Data);
        Assert.Contains("Address book not found", result.ErrorMessages);
    }
}
