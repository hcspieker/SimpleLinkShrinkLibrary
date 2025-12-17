using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SimpleLinkShrinkLibrary.Core.Application.Services;
using SimpleLinkShrinkLibrary.Core.Domain.Entities;
using SimpleLinkShrinkLibrary.Core.Domain.Exceptions;
using SimpleLinkShrinkLibrary.Web.SharedRazorClassLibrary.Controllers;
using SimpleLinkShrinkLibrary.Web.SharedRazorClassLibrary.Models;
using SimpleLinkShrinkLibrary.Web.SharedRazorClassLibrary.UnitTests.Exceptions;

namespace SimpleLinkShrinkLibrary.Web.SharedRazorClassLibrary.UnitTests.Controllers
{
    public class ManageShortlinksControllerTests
    {
        private readonly Mock<IShortlinkService> _serviceMock;
        private readonly HttpContext _httpContext;
        private readonly ManageShortlinksController _controller;

        public ManageShortlinksControllerTests()
        {
            _serviceMock = new Mock<IShortlinkService>();

            _httpContext = new DefaultHttpContext();
            _httpContext.Request.Scheme = "https";
            _httpContext.Request.Host = new HostString("localhost");
            _httpContext.Request.Path = "/ManageShortlinks";

            _controller = new ManageShortlinksController(_serviceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _httpContext
                }
            };
        }

        [Fact]
        [Trait("Category", "ManageShortlinksController_Index")]
        public async Task Index_Default_ReturnsIndex()
        {
            // Arrange & Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        [Trait("Category", "ManageShortlinksController_CreateUrl")]
        public async Task CreateUrl_InvalidModelState_ReturnsIndex()
        {
            // Arrange
            _controller.ModelState.AddModelError("TargetUrl", "Required");

            var model = new ShortlinkCreateViewModel();

            // Act
            var result = await _controller.CreateUrl(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", viewResult.ViewName);
            Assert.Equal(model, viewResult.Model);
        }

        [Fact]
        [Trait("Category", "ManageShortlinksController_CreateUrl")]
        public async Task CreateUrl_CreatingEntry_UsesService()
        {
            // Arrange
            var targetUrl = "https://example.com";

            var model = new ShortlinkCreateViewModel { TargetUrl = targetUrl };
            var createdShortlink = new Shortlink
            {
                Id = 123,
                Alias = targetUrl,
                TargetUrl = model.TargetUrl,
                ExpirationDate = DateTime.UtcNow.AddDays(7)
            };

            _serviceMock.Setup(x => x.Create(It.Is<string>(y => y == targetUrl)))
                .ReturnsAsync(createdShortlink);

            // Act
            var result = await _controller.CreateUrl(model);

            // Assert
            _serviceMock.Verify(x => x.Create(It.Is<string>(y => y == targetUrl)), Times.Once);
        }

        [Fact]
        [Trait("Category", "ManageShortlinksController_CreateUrl")]
        public async Task CreateUrl_ErrorCallingService_AddModelError()
        {
            // Arrange
            var targetUrl = "https://example.com";

            var model = new ShortlinkCreateViewModel { TargetUrl = targetUrl };
            var createdShortlink = new Shortlink
            {
                Id = 123,
                Alias = targetUrl,
                TargetUrl = model.TargetUrl,
                ExpirationDate = DateTime.UtcNow.AddDays(7)
            };

            _serviceMock.Setup(x => x.Create(It.Is<string>(y => y == targetUrl)))
                .ThrowsAsync(new CreateShortlinkException());

            // Act
            var result = await _controller.CreateUrl(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", viewResult.ViewName);
            Assert.Equal(model, viewResult.Model);
            Assert.False(_controller.ModelState.IsValid);

            var errors = _controller.ModelState[""]?.Errors;
            Assert.NotNull(errors);
            Assert.Contains("There was a problem creating the shortlink. Please try again.", errors!.Select(e => e.ErrorMessage));
        }

        [Fact]
        [Trait("Category", "ManageShortlinksController_CreateUrl")]
        public async Task CreateUrl_CreatingEntry_RedirectsToState()
        {
            var targetUrl = "https://example.com";

            var model = new ShortlinkCreateViewModel { TargetUrl = targetUrl };
            var createdShortlink = new Shortlink
            {
                Id = 123,
                Alias = targetUrl,
                TargetUrl = model.TargetUrl,
                ExpirationDate = DateTime.UtcNow.AddDays(7)
            };

            _serviceMock.Setup(x => x.Create(It.Is<string>(y => y == targetUrl)))
                .ReturnsAsync(createdShortlink);

            // Act
            var result = await _controller.CreateUrl(model);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("State", redirectResult.ActionName);
            Assert.Equal(createdShortlink.Alias, redirectResult.RouteValues!["id"]);
        }

        [Fact]
        [Trait("Category", "ManageShortlinksController_State")]
        public async Task State_ExistingEntry_CallsGetByAlias()
        {
            var alias = "short123";

            var existingShortlink = new Shortlink
            {
                Id = 123,
                Alias = alias,
                TargetUrl = "https://example.com",
                ExpirationDate = DateTime.UtcNow.AddDays(7)
            };

            _serviceMock.Setup(x => x.GetByAlias(It.Is<string>(y => y == alias)))
                .ReturnsAsync(existingShortlink);

            // Act
            var result = await _controller.State(alias);

            // Assert
            _serviceMock.Verify(x => x.GetByAlias(It.Is<string>(y => y == alias)), Times.Once);
        }

        [Fact]
        [Trait("Category", "ManageShortlinksController_State")]
        public async Task State_ExistingEntry_ReturnsState()
        {
            var alias = "short123";

            var existingShortlink = new Shortlink
            {
                Id = 123,
                Alias = alias,
                TargetUrl = "https://example.com",
                ExpirationDate = DateTime.UtcNow.AddDays(7)
            };

            _serviceMock.Setup(x => x.GetByAlias(It.Is<string>(y => y == alias)))
                .ReturnsAsync(existingShortlink);

            // Act
            var result = await _controller.State(alias);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);

            var model = Assert.IsType<ShortlinkDetailViewModel>(viewResult.Model);
            Assert.Equal(existingShortlink.Id, model.ShortlinkId);
            Assert.Equal(existingShortlink.TargetUrl, model.TargetUrl);
            Assert.Equal(existingShortlink.ExpirationDate, model.ExpirationDate);
        }

        [Fact]
        [Trait("Category", "ManageShortlinksController_State")]
        public async Task State_ExistingEntry_ReturnsCorrectShortlinkUrl()
        {
            var alias = "short123";

            var existingShortlink = new Shortlink
            {
                Id = 123,
                Alias = alias,
                TargetUrl = "https://example.com",
                ExpirationDate = DateTime.UtcNow.AddDays(7)
            };

            _serviceMock.Setup(x => x.GetByAlias(It.Is<string>(y => y == alias)))
                .ReturnsAsync(existingShortlink);

            // Act
            var result = await _controller.State(alias);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);

            var model = Assert.IsType<ShortlinkDetailViewModel>(viewResult.Model);

            var scheme = _httpContext.Request.Scheme;
            var host = _httpContext.Request.Host;

            var expectedUrl = $"{scheme}://{host}/s/{existingShortlink.Alias}";

            Assert.Equal(expectedUrl, model.ShortlinkUrl);
        }

        [Fact]
        [Trait("Category", "ManageShortlinksController_State")]
        public async Task State_ExistingEntry_ReturnsCorrectStatusUrl()
        {
            var alias = "short123";
            _httpContext.Request.Path = $"/ManageShortlinks/State/{alias}";

            var existingShortlink = new Shortlink
            {
                Id = 123,
                Alias = alias,
                TargetUrl = "https://example.com",
                ExpirationDate = DateTime.UtcNow.AddDays(7)
            };

            _serviceMock.Setup(x => x.GetByAlias(It.Is<string>(y => y == alias)))
                .ReturnsAsync(existingShortlink);

            // Act
            var result = await _controller.State(alias);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);

            var model = Assert.IsType<ShortlinkDetailViewModel>(viewResult.Model);

            var scheme = _httpContext.Request.Scheme;
            var host = _httpContext.Request.Host;
            var controllerName = nameof(ManageShortlinksController).Replace("Controller", "");
            var actionName = nameof(ManageShortlinksController.State);

            var expectedUrl = $"{scheme}://{host}/{controllerName}/{actionName}/{existingShortlink.Alias}";

            Assert.Equal(expectedUrl, model.StatusUrl);
        }

        [Fact]
        [Trait("Category", "ManageShortlinksController_State")]
        public async Task State_EntryNotFound_ReturnsPageNotFound()
        {
            var alias = "short123";

            _serviceMock.Setup(x => x.GetByAlias(It.Is<string>(y => y == alias)))
                .ThrowsAsync(new RetrieveShortlinkException());

            // Act
            var result = await _controller.State(alias);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("NotFound", viewResult.ViewName);
            Assert.Equal(404, _controller.Response.StatusCode);
        }

        [Fact]
        [Trait("Category", "ManageShortlinksController_Delete")]
        public async Task Delete_DeletingEntry_WorksAsExpected()
        {
            // Arrange
            var shortlinkId = 123;

            _serviceMock.Setup(x => x.Delete(shortlinkId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(shortlinkId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        [Trait("Category", "ManageShortlinksController_Delete")]
        public async Task Delete_EntryNotFound_ReturnsDeleted()
        {
            // Arrange
            var shortlinkId = 123;

            _serviceMock.Setup(x => x.Delete(shortlinkId))
                .ThrowsAsync(new EntryNotFoundException("Shortlink not found"));

            // Act
            var result = await _controller.Delete(shortlinkId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        [Trait("Category", "ManageShortlinksController_Delete")]
        public async Task Delete_GeneralException_ReturnsErrorPage()
        {
            // Arrange
            var shortlinkId = 123;

            _serviceMock.Setup(x => x.Delete(shortlinkId))
                .ThrowsAsync(new DummyException("Unexpected error"));

            // Act
            var result = await _controller.Delete(shortlinkId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Error", viewResult.ViewName);
            Assert.Equal(500, _controller.Response.StatusCode);
        }

        [Fact]
        [Trait("Category", "ManageShortlinksController_PageNotFound")]
        public async Task PageNotFound_Default_ReturnsNotFound()
        {
            // Arrange & Act
            var result = _controller.PageNotFound();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("NotFound", viewResult.ViewName);
            Assert.Equal(404, _controller.Response.StatusCode);
        }
    }
}
