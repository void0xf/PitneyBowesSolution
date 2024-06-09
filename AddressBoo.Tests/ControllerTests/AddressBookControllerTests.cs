using AddressBook.API.Controllers;
using AddressBook.API.Models;
using AddressBook.API.Result;
using AddressBook.API.Services;
using AddressBook.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AddressBook.API.Tests.Controllers
{
    public class AddressBookControllerTests
    {
        private readonly Mock<IAddressBookService> _serviceMock;
        private readonly AddressBookController _controller;

        public AddressBookControllerTests()
        {
            _serviceMock = new Mock<IAddressBookService>();
            _controller = new AddressBookController(_serviceMock.Object);
        }

        [Fact]
        public void CreateAddressBook_ShouldReturnCreatedAtAction_WhenValidRequest()
        {
            var request = new AddressBookCreate
            {
                AddressLine1 = "123 Main St",
                AddressLine2 = "Apt 1",
                City = "Anytown",
                ZipCode = "12345",
                Country = "USA",
                State = "CA",
                Phone = "+1234567890"
            };

            var addressBook = new AddressBookModel
            {
                Id = Guid.NewGuid(),
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                City = request.City,
                ZipCode = request.ZipCode,
                Country = request.Country,
                State = request.State,
                Phone = request.Phone,
                CreationDate = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };

            _serviceMock
                .Setup(s => s.AddAddressBook(It.IsAny<AddressBookModel>()))
                .Returns(Result<AddressBookModel>.Success(addressBook));

            var result = _controller.CreateAddressBook(request);

            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<AddressBookModel>(actionResult.Value);
            Assert.Equal(addressBook, returnValue);
            Assert.Equal(nameof(_controller.GetByCity), actionResult.ActionName);
        }

        [Fact]
        public void CreateAddressBook_ShouldReturnBadRequest_WhenRequestIsNull()
        {
            var result = _controller.CreateAddressBook(null);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("AddressBookCreate request is null.", badRequestResult.Value);
        }

        [Fact]
        public void CreateAddressBook_ShouldReturnProblemDetails_WhenServiceReturnsError()
        {
            var request = new AddressBookCreate
            {
                AddressLine1 = "123 Main St",
                AddressLine2 = "Apt 1",
                City = "Anytown",
                ZipCode = "12345",
                Country = "USA",
                State = "CA",
                Phone = "+1234567890"
            };

            _serviceMock
                .Setup(s => s.AddAddressBook(It.IsAny<AddressBookModel>()))
                .Returns(
                    Result<AddressBookModel>.Failure(
                        new List<string> { "Validation error" },
                        "Validation error",
                        400
                    )
                );

            var result = _controller.CreateAddressBook(request);

            var problemResult = Assert.IsType<ObjectResult>(result);
            var problemDetails = Assert.IsType<ProblemDetails>(problemResult.Value);
            Assert.Equal("Validation error", problemDetails.Detail);
            Assert.Equal(400, problemDetails.Status);
        }

        [Fact]
        public void GetAddressBook_ShouldReturnOk_WhenAddressBookExists()
        {
            var addressBookId = Guid.NewGuid();
            var addressBook = new AddressBookModel
            {
                Id = addressBookId,
                AddressLine1 = "123 Main St",
                AddressLine2 = "Apt 1",
                City = "Anytown",
                ZipCode = "12345",
                Country = "USA",
                State = "CA",
                Phone = "555-1234"
            };

            _serviceMock
                .Setup(s => s.GetAddressBook(addressBookId))
                .Returns(Result<AddressBookModel>.Success(addressBook));

            var result = _controller.GetById(addressBookId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<AddressBookModel>(okResult.Value);
            Assert.Equal(addressBook, returnValue);
        }

        [Fact]
        public void GetAddressBook_ShouldReturnProblemDetails_WhenAddressBookDoesNotExist()
        {
            var addressBookId = Guid.NewGuid();

            _serviceMock
                .Setup(s => s.GetAddressBook(addressBookId))
                .Returns(
                    Result<AddressBookModel>.Failure(
                        new List<string> { "Address book not found" },
                        "Address book not found",
                        404
                    )
                );

            var result = _controller.GetById(addressBookId);

            var problemResult = Assert.IsType<ObjectResult>(result);
            var problemDetails = Assert.IsType<ProblemDetails>(problemResult.Value);
            Assert.Equal("Address book entry not found", problemDetails.Title);
            Assert.Equal(
                $"Address book entry with ID {addressBookId} not found.",
                problemDetails.Detail
            );
            Assert.Equal(404, problemDetails.Status);
        }

        [Fact]
        public void DeleteAddressBook_ShouldReturnNoContent_WhenAddressBookExists()
        {
            var addressBookId = Guid.NewGuid();

            _serviceMock
                .Setup(s => s.RemoveAddressBook(addressBookId))
                .Returns(Result<bool>.Success(true));

            var result = _controller.DeleteById(addressBookId);

            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }
    }
}
