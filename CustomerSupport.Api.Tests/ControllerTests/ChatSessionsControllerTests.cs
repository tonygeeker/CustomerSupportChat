using CustomerSupportChatApi.BusinessLayer.Dtos;
using CustomerSupportChatApi.BusinessLayer.Interfaces;
using CustomerSupportChatApi.Controllers;
using CustomerSupportChatApi.DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CustomerSupport.Api.Tests.ControllerTests
{
    [TestFixture]
    public class ChatSessionsControllerTests
    {
        private ChatSessionsController controller;
        private Mock<IChatSessionsService> chatSessionServiceMock;
        private List<ChatSession> groupItems;
        private int id;

        [SetUp]
        public void Setup()
        {
            chatSessionServiceMock = new Mock<IChatSessionsService>();
            
        }

        private CreateChatSessionRequestDto GetCHatSessionCreationRequest()
        {
            return new CreateChatSessionRequestDto
            {
                CustomerEmail = "abc@def.com",
                CustomerName = "Test Name",
                Message = "Test Message",
                Subject = "Test Subject",
                TimeSent = DateTime.Now,
            };
        }

        [Test]
        public async Task WhenPostIsCalled_GivenCorrectRequest_ShouldReturnOkResult()
        {
            // arrange
            var request = GetCHatSessionCreationRequest();
            chatSessionServiceMock.Setup(x => x.CreateChatSession(request)).ReturnsAsync(true);
            controller = new ChatSessionsController(chatSessionServiceMock.Object);

            // act
            var actionResult = await controller.Post(request);

            // assert
            var result = actionResult as OkResult;
            Assert.That(actionResult.GetType() == typeof(OkResult));
            Assert.That(result.StatusCode == 200);
        }

        [Test]
        public async Task WhenPostIsCalled_GivenIncorrectRequest_ShouldReturnBadRequest()
        {
            // arrange
            var request = GetCHatSessionCreationRequest();
            request.CustomerName = string.Empty;
            request.Message = string.Empty;

            chatSessionServiceMock.Setup(x => x.CreateChatSession(request)).ReturnsAsync(true);
            controller = new ChatSessionsController(chatSessionServiceMock.Object);

            // act
            var actionResult = await controller.Post(request);

            // assert
            var result = actionResult as BadRequestObjectResult;
            Assert.That(actionResult.GetType() == typeof(BadRequestObjectResult));
            Assert.That(result.StatusCode == 400);
        }

        [Test]
        public async Task WhenPostIsCalled_GivenCorrectRequestAndServiceReturnsFalse_ShouldReturnBadRequest()
        {
            // arrange
            var request = GetCHatSessionCreationRequest();

            chatSessionServiceMock.Setup(x => x.CreateChatSession(request)).ReturnsAsync(false);
            controller = new ChatSessionsController(chatSessionServiceMock.Object);

            // act
            var actionResult = await controller.Post(request);

            // assert
            var result = actionResult as BadRequestObjectResult;
            Assert.That(actionResult.GetType() == typeof(BadRequestObjectResult));
            Assert.That(result.StatusCode == 400);
            Assert.That(result.Value == "Could not initiate chat at this time, please try again later!");
        }
    }
}
