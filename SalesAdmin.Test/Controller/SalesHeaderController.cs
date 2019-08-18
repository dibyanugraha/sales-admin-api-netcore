namespace SalesAdmin.Test
{
    using AutoMapper;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using SalesAdmin.Controllers;
    using SalesAdmin.Data;
    using SalesAdmin.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class SalesHeaderControllerTest : IClassFixture<TestWebApiFactory<SalesAdmin.Startup>>
    {
        private readonly IMapper _mapper;

        public TestWebApiFactory<SalesAdmin.Startup> _factory;
        public SalesHeaderControllerTest(TestWebApiFactory<SalesAdmin.Startup> factory)
        {
            _factory = factory;

            _mapper = new MapperConfiguration(b =>
            {
                b.CreateMap<SalesHeader, SalesHeaderResponse>();

            }).CreateMapper();
        }

        [Fact]
        public async Task Get_UserById_Are_Valid()
        {
            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
                //var repo = scope.ServiceProvider.GetRequiredService<ISalesHeaderRepository>();
                //var dbTest = scope.ServiceProvider.GetRequiredService<SalesHeaderController>();

                // Arrange
                var mockRepo = new Mock<ISalesHeaderRepository>();
                mockRepo.Setup(a => a.GetSalesHeadersAsync(1, 10, new SalesHeader { })).ReturnsAsync(GetTestSession());
                var controller = new SalesHeaderController(mockRepo.Object, _mapper);

                // Act
                var result = await controller.List(1, 10);

                // Assert
                var resultType = Assert.IsType<SalesHeader>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<SalesHeader>>(
                    resultType);
                Assert.Equal(2, model.Count());
            }
        }

        private List<SalesHeader> GetTestSession()
        {
            var salesHeaders = new List<SalesHeader>();

            salesHeaders.Add(new SalesHeader
            {
                Id = 1,
                No = "123",
                CreatedDateTime = DateTime.Today.Date
            });
            salesHeaders.Add(new SalesHeader
            {
                Id = 2,
                No = "234",
                CreatedDateTime = DateTime.Today.Date.AddDays(-1)
            });
            return salesHeaders;
        }
    }
}
