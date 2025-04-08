using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Prog6_Assessment_CodyBoelens.Controllers;
using Prog6_Assessment_CodyBoelens.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeestjeOpJeFeestjeUnitTests
{
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _loggerMock;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _loggerMock = new Mock<ILogger<HomeController>>();

            _controller = new HomeController(_loggerMock.Object);

            // Setup HttpContext with Session
            var httpContext = new DefaultHttpContext
            {
                Session = new MockHttpSession()
            };
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public void Index_ReturnsViewWithEventDateFromSession()
        {
            // Arrange
            var eventDate = "2025-04-09";
            _controller.HttpContext.Session.SetString("selectedEventDate", eventDate);

            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eventDate, result.ViewData["EventDate"]);
        }

        [Fact]
        public void Index_NoEventDateInSession_ReturnsViewWithNullEventDate()
        {
            // Arrange
            // No session data set, so ViewBag.EventDate should be null

            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.ViewData["EventDate"]);
        }

        [Fact]
        public void Privacy_ReturnsView()
        {
            // Arrange
            // No specific setup needed

            // Act
            var result = _controller.Privacy() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Error_ReturnsViewWithErrorViewModel_WhenActivityIdExists()
        {
            // Arrange
            var activity = new Activity("TestOperation").Start(); // Start creates an Id
            var expectedId = activity.Id; // Get the generated Id

            // Act
            var result = _controller.Error() as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ErrorViewModel>(result.Model);
            Assert.Equal(expectedId, model.RequestId);

            // Cleanup
            activity.Stop();
            Activity.Current = null; // Clear current activity
        }

        [Fact]
        public void Error_ReturnsViewWithTraceIdentifier_WhenNoActivityId()
        {
            // Arrange
            Activity.Current = null; // Ensure no current activity
            var traceIdentifier = "test-trace-id";
            _controller.HttpContext.TraceIdentifier = traceIdentifier;

            // Act
            var result = _controller.Error() as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ErrorViewModel>(result.Model);
            Assert.Equal(traceIdentifier, model.RequestId);
        }

        // MockHttpSession implementation
        public class MockHttpSession : ISession
        {
            private readonly Dictionary<string, byte[]> _sessionStorage = new();

            public bool IsAvailable => true;
            public string Id => "test-session-id";
            public IEnumerable<string> Keys => _sessionStorage.Keys;

            public void Clear() => _sessionStorage.Clear();
            public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
            public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
            public void Remove(string key) => _sessionStorage.Remove(key);
            public void Set(string key, byte[] value) => _sessionStorage[key] = value;
            public bool TryGetValue(string key, out byte[] value) => _sessionStorage.TryGetValue(key, out value);
            public string GetString(string key) => TryGetValue(key, out byte[] value) ? System.Text.Encoding.UTF8.GetString(value) : null;
        }
    }
}
