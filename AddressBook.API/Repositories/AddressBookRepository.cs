using AddressBook.API.Models;
using AddressBook.API.Result;
using System.Collections.ObjectModel;

namespace AddressBook.API.Repositories;

public class AddressBookRepository : IAddressBookRepository
{
    private readonly Collection<AddressBookModel> _addressBooks = new();

    public Result<AddressBookModel> AddAddressBook(AddressBookModel addressBook)
    {
        if (addressBook == null)
        {
            return Result<AddressBookModel>.Failure(
                new List<string> { "Address book is null" },
                "Provided address book is null",
                400
            );
        }

        addressBook.CreationDate = DateTime.UtcNow;
        addressBook.LastModified = DateTime.UtcNow;

        _addressBooks.Add(addressBook);
        return Result<AddressBookModel>.Success(addressBook);
    }

    public Result<IEnumerable<AddressBookModel>> GetAllAddressBooks()
    {
        return Result<IEnumerable<AddressBookModel>>.Success(_addressBooks);
    }

    public Result<AddressBookModel> GetAddressBook(Guid id)
    {
        var addressBook = _addressBooks.FirstOrDefault(ab => ab.Id == id);
        if (addressBook == null)
        {
            return Result<AddressBookModel>.Failure(
                new List<string> { "Address book not found" },
                $"Address book with ID {id} not found",
                404
            );
        }

        return Result<AddressBookModel>.Success(addressBook);
    }

    public Result<bool> RemoveAddressBook(Guid id)
    {
        var addressBook = _addressBooks.FirstOrDefault(ab => ab.Id == id);
        if (addressBook == null)
        {
            return Result<bool>.Failure(
                new List<string> { "Address book not found" },
                $"Address book with ID {id} not found",
                404
            );
        }

        _addressBooks.Remove(addressBook);
        return Result<bool>.Success(true);
    }

    public Result<bool> UpdateAddressBook(Guid id, AddressBookModel updatedAddressBook)
    {
        var existingAddressBook = _addressBooks.FirstOrDefault(ab => ab.Id == id);
        if (existingAddressBook == null)
        {
            return Result<bool>.Failure(
                new List<string> { "Address book not found" },
                $"Address book with ID {id} not found",
                404
            );
        }

        existingAddressBook.AddressLine1 = updatedAddressBook.AddressLine1;
        existingAddressBook.AddressLine2 = updatedAddressBook.AddressLine2;
        existingAddressBook.City = updatedAddressBook.City;
        existingAddressBook.ZipCode = updatedAddressBook.ZipCode;
        existingAddressBook.Country = updatedAddressBook.Country;
        existingAddressBook.State = updatedAddressBook.State;
        existingAddressBook.Phone = updatedAddressBook.Phone;
        existingAddressBook.LastModified = DateTime.UtcNow;

        return Result<bool>.Success(true);
    }
}
