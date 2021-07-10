using System.Net;
using System.Net.Http;
using Application.DTO.Request;
using Application.DTO.Response;
using FluentAssertions;
using Infrastructure.Data.Configuration;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tests.Integration.Base;
using Xbehave;

namespace Tests.Integration.Scenarios
{
    public class Signup : BaseScenario
    {
        [Scenario]
        public void Signup_Should_Add_A_User()
        {
            IHost host = null;
            HttpResponseMessage response = null;
            var signupRequest = new SignupRequest
            {
                Login = "foo",
                Password = "bar1234569009"
            };

            "Given the host".x(async stepContext => host = await GetHost());
            "And given there is no user initially".x(async stepContext =>
            {
                using var scope = host.Services.CreateScope();
                var dbContext =
                    (AuthDbContext)(scope.ServiceProvider.GetService(typeof(AuthDbContext)));

                (await dbContext.Users
                    .AsNoTracking()
                    .CountAsync()).Should().Be(0);
            });
            "When a POST is made".x(async stepContext =>
            {
                response = await host.GetTestClient().PostAsync("signup", GetStringContent(signupRequest));
            });
            "Then the status OK is returned".x(stepContext =>
            {
                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            });
            "And the token is returned as well".x(async stepContext =>
            {
                var signupResponse = await DeserializeResponseContent<SignupResponse>(response);
                signupResponse.Success.Should().BeTrue();
                signupResponse.Message.Should().BeNull();
                signupResponse.Token.Should().NotBeNullOrEmpty();
            });
            "And the user was added".x(async stepContext =>
            {
                using var scope = host.Services.CreateScope();
                var dbContext =
                    (AuthDbContext)(scope.ServiceProvider.GetService(typeof(AuthDbContext)));

                (await dbContext.Users
                    .AsNoTracking()
                    .CountAsync(u => u.Login == signupRequest.Login)).Should().Be(1);
            });
        }

        [Scenario]
        public void Login_And_Password_Must_Be_Present()
        {
            IHost host = null;
            var signupRequest = new SignupRequest
            {
                Login = "",
                Password = "bar1234569009"
            };

            void SomethingIsNotPresent(SignupRequest request)
            {
                HttpResponseMessage response = null;

                "And given there is no user initially".x(async stepContext =>
                {
                    using var scope = host.Services.CreateScope();
                    var dbContext =
                        (AuthDbContext)(scope.ServiceProvider.GetService(typeof(AuthDbContext)));

                    (await dbContext.Users
                        .AsNoTracking()
                        .CountAsync()).Should().Be(0);
                });
                "When a POST is made".x(async stepContext =>
                {
                    response = await host.GetTestClient().PostAsync("signup", GetStringContent(request));
                });
                "Then the status OK is returned".x(stepContext =>
                {
                    response.Should().NotBeNull();
                    response.StatusCode.Should().Be(HttpStatusCode.OK);
                });
                "And the token is not returned".x(async stepContext =>
                {
                    var signupResponse = await DeserializeResponseContent<SignupResponse>(response);
                    signupResponse.Success.Should().BeFalse();
                    signupResponse.Message.Should().Be("Login and password must be present");
                    signupResponse.Token.Should().BeNull();
                });
                "And the user was not added".x(async stepContext =>
                {
                    using var scope = host.Services.CreateScope();
                    var dbContext =
                        (AuthDbContext)(scope.ServiceProvider.GetService(typeof(AuthDbContext)));

                    (await dbContext.Users
                        .AsNoTracking()
                        .CountAsync()).Should().Be(0);
                });
            }

            "Given the host".x(async stepContext => host = await GetHost());
            SomethingIsNotPresent(signupRequest);

            signupRequest.Login = "foo";
            signupRequest.Password = null;
            SomethingIsNotPresent(signupRequest);
        }

        [Scenario]
        public void Max_Login_Length_Is_10()
        {
            IHost host = null;
            HttpResponseMessage response = null;
            var signupRequest = new SignupRequest
            {
                Login = "1234567890-",
                Password = "bar1234569009"
            };

            "Given the host".x(async stepContext => host = await GetHost());
            "And given there is no user initially".x(async stepContext =>
            {
                using var scope = host.Services.CreateScope();
                var dbContext =
                    (AuthDbContext)(scope.ServiceProvider.GetService(typeof(AuthDbContext)));

                (await dbContext.Users
                    .AsNoTracking()
                    .CountAsync()).Should().Be(0);
            });
            "When a POST is made".x(async stepContext =>
            {
                response = await host.GetTestClient().PostAsync("signup", GetStringContent(signupRequest));
            });
            "Then the status OK is returned".x(stepContext =>
            {
                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            });
            "And the token is not returned".x(async stepContext =>
            {
                var signupResponse = await DeserializeResponseContent<SignupResponse>(response);
                signupResponse.Success.Should().BeFalse();
                signupResponse.Message.Should().Be("Max login length is 10");
                signupResponse.Token.Should().BeNull();
            });
            "And the user was not added".x(async stepContext =>
            {
                using var scope = host.Services.CreateScope();
                var dbContext =
                    (AuthDbContext)(scope.ServiceProvider.GetService(typeof(AuthDbContext)));

                (await dbContext.Users
                    .AsNoTracking()
                    .CountAsync()).Should().Be(0);
            });
        }



        [Scenario]
        public void Login_Can_Not_Be_Repeated()
        {
            IHost host = null;
            HttpResponseMessage response = null;
            var signupRequest = new SignupRequest
            {
                Login = "foo",
                Password = "bar123456789"
            };

            "Given the host".x(async stepContext => host = await GetHost());
            "And given there is no user initially, add a user".x(async stepContext =>
            {
                using var scope = host.Services.CreateScope();
                var dbContext =
                    (AuthDbContext)(scope.ServiceProvider.GetService(typeof(AuthDbContext)));

                (await dbContext.Users
                    .AsNoTracking()
                    .CountAsync()).Should().Be(0);

                await host.GetTestClient().PostAsync("signup", GetStringContent(signupRequest));
            });
            "When a POST is made".x(async stepContext =>
            {
                response = await host.GetTestClient().PostAsync("signup", GetStringContent(signupRequest));
            });
            "Then the status OK is returned".x(stepContext =>
            {
                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            });
            "And the token is not returned".x(async stepContext =>
            {
                var signupResponse = await DeserializeResponseContent<SignupResponse>(response);
                signupResponse.Success.Should().BeFalse();
                signupResponse.Message.Should().Be("The login is taken");
                signupResponse.Token.Should().BeNull();
            });
            "And the user was not added".x(async stepContext =>
            {
                using var scope = host.Services.CreateScope();
                var dbContext =
                    (AuthDbContext)(scope.ServiceProvider.GetService(typeof(AuthDbContext)));

                (await dbContext.Users
                    .AsNoTracking()
                    .CountAsync()).Should().Be(1);
            });
        }
    }
}