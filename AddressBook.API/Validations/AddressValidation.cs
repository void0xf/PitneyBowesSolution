using AddressBook.API.Models;
using AddressBook.API.Result;

namespace AddressBook.API.Validations;

public static class AddressValidation
{
    public static Result<bool> ValidateAddress(AddressBookModel addressBook)
    {
        var errorMessages = new List<string>();

        if (string.IsNullOrEmpty(addressBook.AddressLine1))
        {
            errorMessages.Add("Address Line 1 cannot be empty.");
        }

        if (string.IsNullOrEmpty(addressBook.City))
        {
            errorMessages.Add("City cannot be empty.");
        }

        if (string.IsNullOrEmpty(addressBook.ZipCode))
        {
            errorMessages.Add("Zip Code cannot be empty.");
        }

        if (string.IsNullOrEmpty(addressBook.Country))
        {
            errorMessages.Add("Country cannot be empty.");
        }

        if (string.IsNullOrEmpty(addressBook.State))
        {
            errorMessages.Add("State cannot be empty.");
        }

        if (errorMessages.Count > 0)
        {
            return Result<bool>.Failure(errorMessages, "Address validation failed", 400);
        }

        return Result<bool>.Success(true);
    }
}
