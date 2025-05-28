using HouseReservation.Contracts.Models.ViewModels;
using HouseReservation.Core.Models;
using HouseReservation.Core.Services.Interfaces;
using HouseReservation.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HouseReservation.Test
{
    public class HouseManagementControllerTests
    {
        public static readonly TheoryData<HouseEditViewModel> InvalidHouses = HouseTestData.InvalidHouses;

        private readonly Mock<IHouseService> _mockHouseService;
        private readonly HouseManagementController _controller;
        private readonly string _testUserId = "123";

        public HouseManagementControllerTests()
        {
            _mockHouseService = new Mock<IHouseService>();
            _controller = new HouseManagementController(_mockHouseService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, _testUserId)
                ], 
                "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Theory]
        [MemberData(nameof(InvalidHouses))]
        public async Task Create_InvalidModel_ReturnsViewWithModel(HouseEditViewModel model)
        {
            _controller.ModelState.AddModelError("AnyKey", "Invalid");

            var result = await _controller.Create(model);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(model, viewResult.Model);
        }

        [Fact]
        public async Task Create_ValidInput_CallsServiceAndRedirects()
        {
            var model = HouseTestData.ValidHouse;
            var result = await _controller.Create(model);

            _mockHouseService.Verify(s => s.AddHouseAsync(model, int.Parse(_testUserId)), Times.Once);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("MyHouses", redirectResult.ActionName);
        }

        [Fact]
        public async Task CreateAndRetrieveHouse_Success()
        {
            var testUserId = int.Parse(_testUserId);
            var vm = HouseTestData.ValidHouse;

            var expectedList = new List<HouseListItemViewModel>
            {
                new() {
                    Id = 1,
                    Title = vm.Title,
                    ImageUrl = vm.ImageUrl,
                    PricePerNight = vm.PricePerNight,
                    City = vm.City
                }
            };

            _mockHouseService.Setup(s => s.AddHouseAsync(It.IsAny<HouseEditViewModel>(), testUserId))
                       .Returns(Task.CompletedTask);

            _mockHouseService.Setup(s => s.GetMyHousesAsync(testUserId))
                       .ReturnsAsync(expectedList);

            var result = await _controller.Create(vm);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_controller.MyHouses), redirect.ActionName);

            var myHousesResult = await _controller.MyHouses();

            var viewResult = Assert.IsType<ViewResult>(myHousesResult);
            var model = Assert.IsType<MyHousesViewModel>(viewResult.Model);

            Assert.Single(model.Houses);
            var returnedHouse = model.Houses.ToList()[0];

            Assert.Equal(vm.Title, returnedHouse.Title);
            Assert.Equal(vm.ImageUrl, returnedHouse.ImageUrl);
            Assert.Equal(vm.PricePerNight, returnedHouse.PricePerNight);
            Assert.Equal(vm.City, returnedHouse.City);
        }
    }
}
