using Enoch.CrossCutting.WebApi;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CrossCutting.Tests.WebApiExtension
{
    public class WebApiExtensionTest
    {
        private readonly Mock<HttpMessageHandler> httpMessageHandler = new Mock<HttpMessageHandler>();

        [Fact(DisplayName = "WebApi GET must return Success Status Code - (200)")]
        [Trait("CrossCutting", "WebApi")]
        public void CrossCutting_Get_MustReturnSuccess()
        {
            //Arrange
            var server = "https://apiteste.com.br";
            var route = "api";
            var token = "token";
            var parameters = "";

            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(string.Empty)
                });

            var httpClient = new HttpClient(httpMessageHandler.Object);

            var webApi = new WebApi(httpClient);

            //Act
            var response = webApi.Get(server, route, parameters, token);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "WebApi GET must return BadRequest Status Code - (400)")]
        [Trait("CrossCutting", "WebApi")]
        public void CrossCutting_Get_MustReturnBadRequest()
        {
            //Arrange
            var server = "https://apiteste.com.br";
            var route = "api";
            var token = "token";
            var parameters = "";

            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(string.Empty)
                });

            var httpClient = new HttpClient(httpMessageHandler.Object);

            var webApi = new WebApi(httpClient);

            //Act
            var response = webApi.Get(server, route, parameters, token);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact(DisplayName = "WebApi GET must return Unauthorized Status Code - (401)")]
        [Trait("CrossCutting", "WebApi")]
        public void CrossCutting_Get_MustReturnUnauthorized()
        {
            //Arrange
            var server = "https://apiteste.com.br";
            var route = "api";
            var token = "token";
            var parameters = "";

            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Content = new StringContent(string.Empty)
                });

            var httpClient = new HttpClient(httpMessageHandler.Object);

            var webApi = new WebApi(httpClient);

            //Act
            var response = webApi.Get(server, route, parameters, token);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact(DisplayName = "WebApi POST must return Success Status Code - (200)")]
        [Trait("CrossCutting", "WebApi")]
        public void CrossCutting_POST_MustReturnSuccess()
        {
            //Arrange
            var server = "https://apiteste.com.br";
            var route = "api";
            var token = "token";
            var parameters = "";

            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(string.Empty)
                });

            var httpClient = new HttpClient(httpMessageHandler.Object);

            var webApi = new WebApi(httpClient);

            //Act
            var response = webApi.Post(server, route, token, parameters, new { });

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "WebApi POST must return BadRequest Status Code - (400)")]
        [Trait("CrossCutting", "WebApi")]
        public void CrossCutting_POST_MustReturnBadRequest()
        {
            //Arrange
            var server = "https://apiteste.com.br";
            var route = "api";
            var token = "token";
            var parameters = "";

            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(string.Empty)
                });

            var httpClient = new HttpClient(httpMessageHandler.Object);

            var webApi = new WebApi(httpClient);

            //Act
            var response = webApi.Post(server, route, token, parameters, new { });

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact(DisplayName = "WebApi POST must return Unauthorized Status Code - (401)")]
        [Trait("CrossCutting", "WebApi")]
        public void CrossCutting_POST_MustReturnUnauthorized()
        {
            //Arrange
            var server = "https://apiteste.com.br";
            var route = "api";
            var token = "token";
            var parameters = "";

            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Content = new StringContent(string.Empty)
                });

            var httpClient = new HttpClient(httpMessageHandler.Object);

            var webApi = new WebApi(httpClient);

            //Act
            var response = webApi.Post(server, route, token, parameters, new { });

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact(DisplayName = "WebApi PUT must return Success Status Code - (200)")]
        [Trait("CrossCutting", "WebApi")]
        public void CrossCutting_PUT_MustReturnSuccess()
        {
            //Arrange
            var server = "https://apiteste.com.br";
            var route = "api";
            var token = "token";

            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(string.Empty)
                });

            var httpClient = new HttpClient(httpMessageHandler.Object);

            var webApi = new WebApi(httpClient);

            //Act
            var response = webApi.Put(server, route, token, new { });

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "WebApi PUT must return BadRequest Status Code - (400)")]
        [Trait("CrossCutting", "WebApi")]
        public void CrossCutting_PUT_MustReturnBadRequest()
        {
            //Arrange
            var server = "https://apiteste.com.br";
            var route = "api";
            var token = "token";

            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(string.Empty)
                });

            var httpClient = new HttpClient(httpMessageHandler.Object);

            var webApi = new WebApi(httpClient);

            //Act
            var response = webApi.Put(server, route, token, new { });

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact(DisplayName = "WebApi PUT must return Unauthorized Status Code - (401)")]
        [Trait("CrossCutting", "WebApi")]
        public void CrossCutting_PUT_MustReturnUnauthorized()
        {
            //Arrange
            var server = "https://apiteste.com.br";
            var route = "api";
            var token = "token";

            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Content = new StringContent(string.Empty)
                });

            var httpClient = new HttpClient(httpMessageHandler.Object);

            var webApi = new WebApi(httpClient);

            //Act
            var response = webApi.Put(server, route, token, new { });

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact(DisplayName = "WebApi Patch must return Success Status Code - (200)")]
        [Trait("CrossCutting", "WebApi")]
        public void CrossCutting_Patch_MustReturnSuccess()
        {
            //Arrange
            var server = "https://apiteste.com.br";
            var route = "api";
            var token = "token";

            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(string.Empty)
                });

            var httpClient = new HttpClient(httpMessageHandler.Object);

            var webApi = new WebApi(httpClient);

            //Act
            var response = webApi.Patch(server, route, token, new { });

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "WebApi Patch must return BadRequest Status Code - (400)")]
        [Trait("CrossCutting", "WebApi")]
        public void CrossCutting_Patch_MustReturnBadRequest()
        {
            //Arrange
            var server = "https://apiteste.com.br";
            var route = "api";
            var token = "token";

            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(string.Empty)
                });

            var httpClient = new HttpClient(httpMessageHandler.Object);

            var webApi = new WebApi(httpClient);

            //Act
            var response = webApi.Put(server, route, token, new { });

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact(DisplayName = "WebApi Patch must return Unauthorized Status Code - (401)")]
        [Trait("CrossCutting", "WebApi")]
        public void CrossCutting_Patch_MustReturnUnauthorized()
        {
            //Arrange
            var server = "https://apiteste.com.br";
            var route = "api";
            var token = "token";

            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Content = new StringContent(string.Empty)
                });

            var httpClient = new HttpClient(httpMessageHandler.Object);

            var webApi = new WebApi(httpClient);

            //Act
            var response = webApi.Patch(server, route, token, new { });

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
