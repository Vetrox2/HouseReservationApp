using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Firefox;
using HouseReservation.Contracts.Models.ViewModels;

namespace HouseReservation.Test
{
    public class UITest : IDisposable
    {
        public static readonly TheoryData<HouseEditViewModel> InvalidHouses = HouseTestData.InvalidHouses;

        private readonly IWebDriver driver;
        private readonly string rootUrl = "https://localhost:7207/";

        public UITest()
        {
            var options = new FirefoxOptions();
            options.AddArgument("--headless");
            driver = new FirefoxDriver(options);
        }

        [Fact]
        public void Login_WithCorrectCredentials_ShouldSucceed()
        {
            driver.Navigate().GoToUrl(rootUrl);
            driver.FindElement(By.Id("login")).Click();

            Assert.Contains("/Identity/Account/Login", driver.Url);

            driver.FindElement(By.Id("Input_Email")).SendKeys("user@1.pl");
            driver.FindElement(By.Id("Input_Password")).SendKeys("Aa1234!");
            driver.FindElement(By.Id("login-submit")).Click();

            Assert.DoesNotContain("/Account/Login", driver.Url);
            Assert.Equal(rootUrl, driver.Url);
        }

        [Fact]
        public void Login_WithIncorrectCredentials_ShouldFail()
        {
            driver.Navigate().GoToUrl(rootUrl);
            driver.FindElement(By.Id("login")).Click();
            Assert.Contains("/Account/Login", driver.Url);

            driver.FindElement(By.Id("Input_Email")).SendKeys("user@1.pl");
            driver.FindElement(By.Id("Input_Password")).SendKeys("WrongPassword");
            driver.FindElement(By.Id("login-submit")).Click();

            Assert.Contains("/Account/Login", driver.Url);
            Assert.Contains("Invalid login attempt", driver.PageSource);
        }

        [Fact]
        public void AddHouse_WithCorrectData_ShouldSucceed()
        {
            LoginAsAdmin();

            driver.FindElement(By.Id("my-houses")).Click();
            Assert.Contains("/HouseManagement/MyHouses", driver.Url);

            driver.FindElement(By.Id("house-create")).Click();
            Assert.Contains("/HouseManagement/Create", driver.Url);

            driver.FindElement(By.Id("Title")).SendKeys("Test House");
            driver.FindElement(By.Id("ImageUrl")).SendKeys("https://example.com/image.jpg");
            driver.FindElement(By.Id("Description")).SendKeys("A beautiful test house for testing.");
            driver.FindElement(By.Id("PricePerNight")).SendKeys("150");
            driver.FindElement(By.Id("SizeM2")).SendKeys("120");
            driver.FindElement(By.Id("Bedrooms")).SendKeys("1");
            driver.FindElement(By.Id("Bathrooms")).SendKeys("2");
            driver.FindElement(By.Id("Country")).SendKeys("Poland");
            driver.FindElement(By.Id("City")).SendKeys("Warsaw");
            driver.FindElement(By.Id("State")).SendKeys("Mazowieckie");
            driver.FindElement(By.Id("StreetName")).SendKeys("Testowa");
            driver.FindElement(By.Id("StreetNumber")).SendKeys("42");
            driver.FindElement(By.Id("PostalCode")).SendKeys("00-001");
            driver.FindElement(By.Id("submit")).Click();

            Assert.Contains("/HouseManagement/MyHouses", driver.Url);
            Assert.Contains("Test House", driver.PageSource);

            driver.Navigate().GoToUrl(rootUrl);
        }

        [Theory]
        [MemberData(nameof(InvalidHouses))]
        public void AddHouse_WithIncorrectData_ShouldFail(HouseEditViewModel vm)
        {
            LoginAsAdmin();

            driver.FindElement(By.Id("my-houses")).Click();
            Assert.Contains("/HouseManagement/MyHouses", driver.Url);

            driver.FindElement(By.Id("house-create")).Click();
            Assert.Contains("/HouseManagement/Create", driver.Url);

            if (vm.Title != null)
                driver.FindElement(By.Id("Title")).SendKeys(vm.Title);

            if (vm.ImageUrl != null)
                driver.FindElement(By.Id("ImageUrl")).SendKeys(vm.ImageUrl);

            if (vm.Description != null)
                driver.FindElement(By.Id("Description")).SendKeys(vm.Description);

            if (vm.PricePerNight != null)
                driver.FindElement(By.Id("PricePerNight")).SendKeys(vm.PricePerNight.ToString());

            if (vm.SizeM2 != null)
                driver.FindElement(By.Id("SizeM2")).SendKeys(vm.SizeM2.ToString());

            if (vm.Bedrooms != null)
                driver.FindElement(By.Id("Bedrooms")).SendKeys(vm.Bedrooms.ToString());

            if (vm.Bathrooms != null)
                driver.FindElement(By.Id("Bathrooms")).SendKeys(vm.Bathrooms.ToString());

            if (vm.Country != null)
                driver.FindElement(By.Id("Country")).SendKeys(vm.Country);

            if (vm.City != null)
                driver.FindElement(By.Id("City")).SendKeys(vm.City);

            if (vm.State != null)
                driver.FindElement(By.Id("State")).SendKeys(vm.State);

            if (vm.StreetName != null)
                driver.FindElement(By.Id("StreetName")).SendKeys(vm.StreetName);

            if (vm.StreetNumber != null)
                driver.FindElement(By.Id("StreetNumber")).SendKeys(vm.StreetNumber);

            if (vm.PostalCode != null)
                driver.FindElement(By.Id("PostalCode")).SendKeys(vm.PostalCode);

            driver.FindElement(By.Id("submit")).Click();

            Assert.Contains("/HouseManagement/Create", driver.Url);

            driver.Navigate().GoToUrl(rootUrl);
        }
        public void Dispose()
        {
            driver.Quit();
            driver.Dispose();
        }

        private void LoginAsAdmin()
        {
            driver.Navigate().GoToUrl(rootUrl);

            var loginLink = driver.FindElement(By.Id("login"));
            loginLink.Click();

            driver.FindElement(By.Id("Input_Email")).SendKeys("user@1.pl");
            driver.FindElement(By.Id("Input_Password")).SendKeys("Aa1234!");
            driver.FindElement(By.Id("login-submit")).Click();
        }
    }
}
