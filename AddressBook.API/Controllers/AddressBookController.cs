using AddressBook.API.Models;
using AddressBook.API.Services;
using AddressBook.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressBookController : ControllerBase
{
    private readonly IAddressBookService _addressBookService;

    public AddressBookController(IAddressBookService addressBookService)
    {
        _addressBookService = addressBookService;
    }

    [HttpGet]
    public IActionResult GetLastAdded()
    {
        var result = _addressBookService.GetLastAdded();
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return NotFound();
    }

    [HttpGet("{city}")]
    public IActionResult GetByCity(string city)
    {
        var result = _addressBookService.GetByCity(city);
        if (result.Data.Any())
        {
            return Ok(result.Data);
        }
        return NotFound();
    }

    [HttpPost]
    public IActionResult CreateAddressBook([FromBody] AddressBookCreate addressBookCreate)
    {
        if (addressBookCreate == null)
        {
            return BadRequest("AddressBookCreate request is null.");
        }

        var addressBookModel = new AddressBookModel
        {
            Id = Guid.NewGuid(),
            AddressLine1 = addressBookCreate.AddressLine1,
            AddressLine2 = addressBookCreate.AddressLine2,
            City = addressBookCreate.City,
            ZipCode = addressBookCreate.ZipCode,
            Country = addressBookCreate.Country,
            State = addressBookCreate.State,
            Phone = addressBookCreate.Phone,
            CreationDate = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };

        var result = _addressBookService.AddAddressBook(addressBookModel);

        if (result.IsSuccess)
        {
            return CreatedAtAction(
                nameof(GetByCity),
                new { city = addressBookModel.City },
                result.Data
            );
        }

        return Problem(
            detail: result.ProblemDetails.Detail,
            statusCode: result.ProblemDetails.Status
        );
    }
}
