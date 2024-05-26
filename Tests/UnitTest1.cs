//using Newtonsoft.Json;
//using NUnitTestProject_UNI.Core.Models;
//using System.Net;
//using NunitTests.Core.Utils;
//using System.Text;
//using FluentAssertions;
//using System.Reflection.Metadata;

//namespace NUnitTestProject_UNI.Tests

//{
//    public class Tests
//    {
//        [TestFixture]
//        public class BaseTests
//        {
//            private readonly HttpClientHelper _handler;
//            private object createdUserId;

//            public BaseTests()
//            {
//                _handler = new HttpClientHelper();
//            }

//            [Test]
//            public async Task GetAllUsers()
//            {
//                var response = await _handler.GET("users");

//                Assert.That(response.StatusCode.ToString(), Is.EqualTo("OK"));
//                var content = await response.Content.ReadAsStringAsync();

//                Console.WriteLine($"Response Content: {content}");
//                TestContext.WriteLine($"Response Content: {content}");
//            }

//            [Test]
//            public async Task GetSpecificUser()
//            {
//                int userId = 6929797;
//                string endpoint = $"users/{userId}";

//                var response = await _handler.GET(endpoint);

//                Assert.That(response.StatusCode.ToString(), Is.EqualTo("OK"));
//                var content = await response.Content.ReadAsStringAsync();

//                Console.WriteLine($"Response Content: {content}");
//                TestContext.WriteLine($"Response Content: {content}");
//            }

//            [Test]
//            public async Task CreateUser()
//            {
//                var userName = Guid.NewGuid().ToString();
//                var requestBody = _handler.CreateUserRequestBody(userName);

//                var response = await _handler.POST("users", requestBody);

//                var responseBodyString = await response.Content.ReadAsStringAsync();
//                var responseBody = JsonConvert.DeserializeObject<UserResponse>(responseBodyString);

//                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

//                Assert.That(responseBody.Name, Is.EqualTo(userName));
//                Assert.IsNotNull(responseBody.Id);

//                createdUserId = responseBody.Id;

//                Console.WriteLine($"Response Body: {responseBodyString}");
//                Console.WriteLine($"User Name: {responseBody.Name}");
//                Console.WriteLine($"User ID: {responseBody.Id}");
//            }

//            [Test]
//            public async Task UpdateUser()
//            {
//                int userId = 6929837;
//                var originalUserResponse = await _handler.GET($"users/{userId}");
//                originalUserResponse.EnsureSuccessStatusCode();

//                var originalUserResponseString = await originalUserResponse.Content.ReadAsStringAsync();
//                var originalUser = JsonConvert.DeserializeObject<UserResponse>(originalUserResponseString);

//                var updatedUserName = Guid.NewGuid().ToString();
//                var requestBody = _handler.CreateUserRequestBody(updatedUserName);

//                var response = await _handler.PATCH($"users/{userId}", requestBody);
//                response.EnsureSuccessStatusCode();


//                var updatedUserResponseString = await response.Content.ReadAsStringAsync();
//                var updatedUserResponse = JsonConvert.DeserializeObject<UserResponse>(updatedUserResponseString);

//                Console.WriteLine($"Previous Name: {originalUser.Name}");
//                Console.WriteLine($"Updated Name: {updatedUserResponse.Name}");
//            }

//            [Test]
//            public async Task DeleteUser()
//            {
//                await CreateUser();

//                var deleteResponse = await _handler.DELETE($"users/{createdUserId}");

//                Assert.AreEqual(HttpStatusCode.NoContent, deleteResponse.StatusCode);

//                var getUserResponse = await _handler.GET($"users/{createdUserId}");

//                Assert.AreEqual(HttpStatusCode.NotFound, getUserResponse.StatusCode);

//                Console.WriteLine($"Deleted UserId: {createdUserId}");
//            }
//        }
//    }
//}

using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnitTestProject_UNI.Core.Models;
using NunitTests.Core.Utils;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NUnitTestProject_UNI.Tests
{
    public class Tests
    {
        [TestFixture]
        public class BaseTests
        {
            private readonly HttpClientHelper _handler;
            private object createdUserId;

            public BaseTests()
            {
                _handler = new HttpClientHelper();
            }

            [Test]
            public async Task GetAllUsers()
            {
                var response = await _handler.GET("users");

                response.StatusCode.Should().Be(HttpStatusCode.OK);
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response Content: {content}");
                TestContext.WriteLine($"Response Content: {content}");
            }

            [Test]
            public async Task GetSpecificUser()
            {
                int userId = 6929797;
                string endpoint = $"users/{userId}";

                var response = await _handler.GET(endpoint);

                response.StatusCode.Should().Be(HttpStatusCode.OK);
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response Content: {content}");
                TestContext.WriteLine($"Response Content: {content}");
            }

            [Test]
            public async Task CreateUser()
            {
                var userName = Guid.NewGuid().ToString();
                var requestBody = _handler.CreateUserRequestBody(userName);

                var response = await _handler.POST("users", requestBody);

                var responseBodyString = await response.Content.ReadAsStringAsync();
                var responseBody = JsonConvert.DeserializeObject<UserResponse>(responseBodyString);

                response.StatusCode.Should().Be(HttpStatusCode.Created);
                responseBody.Name.Should().Be(userName);
                responseBody.Id.Should().NotBeNull();

                createdUserId = responseBody.Id;

                Console.WriteLine($"Response Body: {responseBodyString}");
                Console.WriteLine($"User Name: {responseBody.Name}");
                Console.WriteLine($"User ID: {responseBody.Id}");
            }

            [Test]
            public async Task UpdateUser()
            {
                int userId = 6929837;
                var originalUserResponse = await _handler.GET($"users/{userId}");
                originalUserResponse.EnsureSuccessStatusCode();

                var originalUserResponseString = await originalUserResponse.Content.ReadAsStringAsync();
                var originalUser = JsonConvert.DeserializeObject<UserResponse>(originalUserResponseString);

                var updatedUserName = Guid.NewGuid().ToString();
                var requestBody = _handler.CreateUserRequestBody(updatedUserName);

                var response = await _handler.PATCH($"users/{userId}", requestBody);
                response.EnsureSuccessStatusCode();

                var updatedUserResponseString = await response.Content.ReadAsStringAsync();
                var updatedUserResponse = JsonConvert.DeserializeObject<UserResponse>(updatedUserResponseString);

                Console.WriteLine($"Previous Name: {originalUser.Name}");
                Console.WriteLine($"Updated Name: {updatedUserResponse.Name}");

                updatedUserResponse.Name.Should().Be(updatedUserName);
            }

            [Test]
            public async Task DeleteUser()
            {
                await CreateUser();

                var deleteResponse = await _handler.DELETE($"users/{createdUserId}");

                deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

                var getUserResponse = await _handler.GET($"users/{createdUserId}");

                getUserResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

                Console.WriteLine($"Deleted UserId: {createdUserId}");
            }
        }
    }
}
