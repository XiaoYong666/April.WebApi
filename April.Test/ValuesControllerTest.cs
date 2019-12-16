﻿using April.Entity;
using April.Service.Interfaces;
using April.WebApi;
using April.WebApi.Controllers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace April.Test
{
    public class ValuesControllerTest
    {
        public HttpClient Client { get; }

        public ValuesControllerTest()
        {
            var server = new TestServer(WebHost.CreateDefaultBuilder().UseStartup<Startup>());
            Client = server.CreateClient();
        }

        [Fact]
        public async void TestGetV2()
        {
            var response = await Client.GetAsync("/api/Values");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void TestGet()
        {
            // Arrange
            var mockRepo = new Mock<IStudentService>();
            var controller = new ValuesController(mockRepo.Object);
            // Act
            var result = controller.Get();
            // Assert
            Assert.Equal(new string[] { "value1", "" }, result.Value);
        }

        [Theory]
        [InlineData(1)]
        public void TestGetByID(int id)
        {
            var mockRepo = new Mock<IStudentService>();
            mockRepo.Setup(repo => repo.GetList(s => s.ID == 38).ToList())
                .Returns(GetList());

            var controller = new ValuesController(mockRepo.Object);

            var result = controller.Get(id);

            Assert.NotNull(result);
            Assert.Contains("大洛阳", result.Value);
        }

        private List<StudentEntity> GetList()
        {
            List<StudentEntity> entities = new List<StudentEntity>();

            entities.Add(new StudentEntity()
            {
                ID = 1,
                Name = "小明",
                Number = "123456",
                Age = 19,
                Sex = 1,
                Address = "大洛阳"
            });
            entities.Add(new StudentEntity()
            {
                ID = 2,
                Name = "小红",
                Number = "456789",
                Age = 18,
                Sex = 0,
                Address = "大洛阳"
            });

            return entities;
        }
    }
}