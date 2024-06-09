using AddressBook.API.Models;
using AddressBook.API.Repositories;
using AddressBook.API.Result;
using AddressBook.API.Validations;

namespace AddressBook.API.Services;

public class AddressBookService : IAddressBookService
{
    private readonly IAddressBookRepository _repository;

    public AddressBookService(IAddressBookRepository repository)
    {
        _repository = repository;
    }

    public Result<AddressBookModel> AddAddressBook(AddressBookModel addressBook)
    {
        var validation = AddressValidation.ValidateAddress(addressBook);
        if (!validation.IsSuccess)
        {
            return Result<AddressBookModel>.Failure(
                validation.ErrorMessages,
                validation.ProblemDetails.Detail,
                400
            );
        }

        return _repository.AddAddressBook(addressBook);
    }

    public Result<IEnumerable<AddressBookModel>> GetAllAddressBooks()
    {
        return _repository.GetAllAddressBooks();
    }

    public Result<AddressBookModel> GetAddressBook(Guid id)
    {
        return _repository.GetAddressBook(id);
    }

    public Result<bool> RemoveAddressBook(Guid id)
    {
        return _repository.RemoveAddressBook(id);
    }

    public Result<bool> UpdateAddressBook(Guid id, AddressBookModel addressBook)
    {
        var validation = AddressValidation.ValidateAddress(addressBook);
        if (!validation.IsSuccess)
        {
            return Result<bool>.Failure(
                validation.ErrorMessages,
                validation.ProblemDetails.Detail,
                400
            );
        }

        return _repository.UpdateAddressBook(id, addressBook);
    }
}
