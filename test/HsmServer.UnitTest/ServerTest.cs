using EfCore.Repository;
using EfCore.Repository.Abstractions;
using EfCore.Repository.Concretes;
using EventFlux;
using HsmServer.UnitTest.Context;
using HsmServer.UnitTest.Events;
using HsmServer.UnitTest.Models;
using HsmServer.UnitTest.Validators;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

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

        [Fact]
        public async Task UnitOfWork_GetListAsync_ShouldReturnNonNullList_WhenDataExists()
        {
            var fakePersons = new List<Person>
            {
                new Person { Id = 1, Name = "Alice", Age = 30 },
                new Person { Id = 2, Name = "Bob", Age = 25 }
            };


            var mockReadRepo = new Mock<IReadRepository<Person>>();
            mockReadRepo.Setup(repo => repo.GetListAsync(It.IsAny<CancellationToken>())).ReturnsAsync(fakePersons);

            var mockUnitOfWork = new Mock<IUnitOfWork<Person>>();
            mockUnitOfWork.Setup(uof => uof.GetReadRepository()).Returns(mockReadRepo.Object);

            var readRepo = mockUnitOfWork.Object.GetReadRepository();
            var result = await readRepo.GetListAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(fakePersons.Count, result.Count);
        }

        [Fact]
        public async Task UnitOfWork_AddAsync_ShouldReturnTrue_WhenDataAdd()
        {
            var fakePersons = new List<Person>
            {
                new Person { Id = 1, Name = "Alice", Age = 30 },
                new Person { Id = 2, Name = "Bob", Age = 25 }
            };

            var mockWriteRepo = new Mock<IWriteRepository<Person>>();
            mockWriteRepo.Setup(repo => repo.AddAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            mockWriteRepo.Setup(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var mockUnitOfWork = new Mock<IUnitOfWork<Person>>();
            mockUnitOfWork.Setup(uof => uof.GetWriteRepository()).Returns(mockWriteRepo.Object);

            var writeRepo = mockUnitOfWork.Object.GetWriteRepository();
            await writeRepo.AddAsync(new Person() { Age = 25, Id = 5, Name = "example" });
            bool response = await writeRepo.SaveChangesAsync();

            Assert.True(response);
        }

        [Fact]
        public async Task GetListAsync_ShouldReturnNonNullList_WhenSpecificationIsApplied()
        {
            var fakePersons = new List<Person>
            {
                new Person { Id = 1, Name = "Alice", Age = 24 },
                new Person { Id = 2, Name = "Oliver", Age = 25 },
                new Person { Id = 3, Name = "Jack", Age = 26 },
                new Person { Id = 4, Name = "Amelia", Age = 27 },
                new Person { Id = 5, Name = "Olivia", Age = 28 },
                new Person { Id = 6, Name = "Emily", Age = 29 },
                new Person { Id = 7, Name = "Thomas", Age = 30 },
            };

            Specification<Person> specification = new()
            {
                AsNoTracking = false,
                Skip = 0,
                Take = 4
            };
            specification.Conditions.Add(p => p.Age <= 26);

            Mock<DemoDbContext> mockDemoContext = new();
            mockDemoContext.Setup(context => context.Set<Person>()).ReturnsDbSet(fakePersons);

            Mock<ReadRepository<Person>> mockReadRepo = new(mockDemoContext.Object);
            var personList = await mockReadRepo.Object.GetListAsync(specification, CancellationToken.None);

            Assert.True(3 == personList.Count());
        }

        [Fact]
        public async Task GetListAsync_ShouldReturnNonNullList_WhenMapFilterIsApplied()
        {
            var fakePersons = new List<Person>
            {
                new Person { Id = 1, Name = "Alice", Age = 24 },
                new Person { Id = 2, Name = "Oliver", Age = 25 },
                new Person { Id = 3, Name = "Jack", Age = 26 },
                new Person { Id = 4, Name = "Amelia", Age = 27 },
                new Person { Id = 5, Name = "Olivia", Age = 28 },
                new Person { Id = 6, Name = "Emily", Age = 29 },
                new Person { Id = 7, Name = "Thomas", Age = 30 },
            };

            Expression<Func<Person, PersonDto>> selectExpression = person => new()
            {
                Name = person.Name,
                Age = person.Age,
                Id = person.Id
            };

            Mock<DemoDbContext> mockDemoContext = new();
            mockDemoContext.Setup(context => context.Set<Person>()).ReturnsDbSet(fakePersons);

            Mock<ReadRepository<Person>> mockReadRepository = new(mockDemoContext.Object);

            Mock<IUnitOfWork<Person>> mockUnitOfWork = new();
            mockUnitOfWork.Setup(uof => uof.GetReadRepository()).Returns(mockReadRepository.Object);

            var readRepo = mockUnitOfWork.Object.GetReadRepository();
            var personDtoList = await readRepo.GetListAsync(selectExpression, CancellationToken.None);

            Assert.IsType<List<PersonDto>>(personDtoList);
            Assert.True(fakePersons.Count() == personDtoList.Count());
        }

        [Fact]
        public async Task GetListAsync_ShouldReturnPaginatedList_WhenMapFilterIsApplied()
        {
            var fakePersons = new List<Person>
            {
                new Person { Id = 1, Name = "Alice", Age = 24 },
                new Person { Id = 2, Name = "Oliver", Age = 25 },
                new Person { Id = 3, Name = "Jack", Age = 26 },

                new Person { Id = 4, Name = "Amelia", Age = 27 },
                new Person { Id = 5, Name = "Olivia", Age = 28 },
                new Person { Id = 6, Name = "Emily", Age = 29 },

                new Person { Id = 6, Name = "Emily", Age = 29 },
                new Person { Id = 7, Name = "Taylor", Age = 30 },
                new Person { Id = 8, Name = "Wilson", Age = 30 },

                new Person { Id = 9, Name = "Roberts", Age = 30 },
                new Person { Id = 10, Name = "Martin", Age = 30 },
                new Person { Id = 11, Name = "Roy", Age = 30 },
                new Person { Id = 12, Name = "Li", Age = 30 },
            };

            PaginationSpecification<Person> paginationSpecification = new()
            {
                AsNoTracking = false,
                PageIndex = 3,
                PageSize = 3
            };

            Mock<DemoDbContext> mockDemoContext = new();
            mockDemoContext.Setup(context => context.Set<Person>()).ReturnsDbSet(fakePersons);

            Mock<ReadRepository<Person>> mockReadRepository = new(mockDemoContext.Object);

            Mock<IUnitOfWork<Person>> mockUnitOfWork = new();
            mockUnitOfWork.Setup(uof => uof.GetReadRepository()).Returns(mockReadRepository.Object);

            var readRepo = mockUnitOfWork.Object.GetReadRepository();
            var personList = await readRepo.GetListAsync(paginationSpecification, CancellationToken.None);

            Assert.True("Wilson" == personList.Items.Last().Name);
            Assert.True(personList.PageIndex == paginationSpecification.PageIndex);
            Assert.True(personList.TotalItems == fakePersons.Count());
        }
    }

    public class PersonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}