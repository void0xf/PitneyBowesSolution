using AddressBook.API.Models;
using AddressBook.API.Result;

namespace AddressBook.API.Repositories;

public interface IAddressBookRepository
{
    Result<IEnumerable<AddressBookModel>> GetAllAddressBooks();
    Result<AddressBookModel> GetAddressBook(Guid id);
    Result<bool> UpdateAddressBook(Guid id, AddressBookModel addressBook);
    Result<AddressBookModel> AddAddressBook(AddressBookModel addressBook);
    Result<bool> RemoveAddressBook(Guid id);
}
