using EventFlux;
using HsmServer.UnitTest.Events;
using HsmServer.UnitTest.Models;
using HsmServer.UnitTest.Validators;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace HsmServer.UnitTest
{
    public class ServerTest
    {
        private IServiceProvider _serviceProvider;
        private ServiceCollection serviceCollection;

        public ServerTest()
        {
            serviceCollection = new ServiceCollection();
        }

        [Fact]
        public void Validate_ShouldReturnError_WhenNameIsLessThanTwoCharacters()
        {
            User user = new()
            {
                Age = 18,
                Emails = ["kadir@gmail.com"],
                Name = "ka",
                UserBaskets = null,
                UserCustomer = new()
                {
                    Email = "kadirrr@gmail.com",
                    PhoneNumber = "07776665544"
                }
            };

            var mockValidator = new Mock<UserValidator>();

            var vResult = mockValidator.Object.Validate(user);

            Assert.Equal("Name must be big than 2 character", vResult.Errors.First());
            Assert.False(vResult.IsValid);
        }

        [Fact]
        public void Validate_ShouldReturnError_WhenEmailFormatIsWrong()
        {
            User user = new()
            {
                Age = 18,
                Emails = ["kadirgmail.com"],
                Name = "kadir",
                UserBaskets = null,
                UserCustomer = null
            };

            var mockValidator = new Mock<UserValidator>();

            var vResult = mockValidator.Object.Validate(user);

            Assert.Equal("Email format is wrong", vResult.Errors.First());
            Assert.False(vResult.IsValid);
        }

        [Fact]
        public void CustomValidate_ShouldReturnError_WhenCustomValidateIsWrong()
        {
            User user = new()
            {
                Age = 24,
                Emails = ["kadir@gmail.com"],
                Name = "suleyman",
                UserBaskets = null,
                UserCustomer = null
            };

            var mockValidator = new Mock<UserValidator>();

            var vResult = mockValidator.Object.Validate(user);

            Assert.Equal("User name must be contains 'k' character", vResult.Errors.First());
            Assert.Equal("User name length at least be than 10", vResult.Errors.Last());
            Assert.False(vResult.IsValid);
        }

        [Fact]
        public void NestedValidate_ShouldReturnError_WhenNestedClassValidateISWrong()
        {
            User user = new()
            {
                Age = 24,
                Emails = ["kadir@gmail.com"],
                Name = "kadirasdfgh",
                UserBaskets = null,
                UserCustomer = new()
                {
                    Email = "",
                    PhoneNumber = "055544422"
                }
            };

            var mockValidator = new Mock<UserValidator>();

            var vResult = mockValidator.Object.Validate(user);

            Assert.Equal("ProductCustomer Email is null", vResult.Errors.First());
            Assert.Equal("ProductCustomer not Email format", vResult.Errors[1]);
            Assert.Equal("ProductCustomer not PhoneNumber format", vResult.Errors.Last());
            Assert.False(vResult.IsValid);
        }

        private void AddSendInjection()
        {
            var mockEventBus = new Mock<EventBus>();

            mockEventBus.Setup(bus => bus.SendAsync(It.IsAny<ExampleEventRequest>()))
                .ReturnsAsync(new ExampleEventResponse { Res = "123" });

            serviceCollection.AddSingleton(sp => mockEventBus.Object);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public async Task Send_ShouldReturnResponse_WhenEventBusSendEvent()
        {
            ExampleEventRequest req = new() { Num = 123 };
            ExampleEventResponse res = new();

            AddSendInjection();

            var eventBus = _serviceProvider.GetRequiredService<EventBus>();

            res = await eventBus.SendAsync(req);

            Assert.Equal("123", res.Res);
        }

        [Fact]
        public async Task Publish_ShouldNotReturn_WhenPublishEvent()
        {
            PublishEventRequest req = new() { Data = "test data" };
            var mockEventBus = new Mock<EventBus>();

            mockEventBus
                .Setup(bus => bus.PublishAsync(It.IsAny<PublishEventRequest>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(sp => mockEventBus.Object);
            _serviceProvider = serviceCollection.BuildServiceProvider();

            var eventBus = _serviceProvider.GetRequiredService<EventBus>();

            await eventBus.PublishAsync(req);

            mockEventBus.Verify(bus => bus.PublishAsync(It.Is<PublishEventRequest>(r => r.Data == "test data")), Times.Once);
        }

    }
}