using Microsoft.AspNetCore.Http;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests
{
    public class TestHttpClient : HttpClient
    {
        public readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;

        public TestHttpClient(Uri baseAddress)
            : this(new Mock<HttpMessageHandler>(), baseAddress)
        {
        }

        private TestHttpClient(Mock<HttpMessageHandler> mockHttpMessageHandler, Uri baseAddress) : base(mockHttpMessageHandler.Object)
        {
            _mockHttpMessageHandler = mockHttpMessageHandler;
            BaseAddress = baseAddress;
        }

        public void VerifyPutAsAsync<T>(string relativePath, T value, Times times)
        {
            _mockHttpMessageHandler
             .Protected()
             .Verify("SendAsync", times,
             ItExpr.Is<HttpRequestMessage>(r =>
             r.Method == HttpMethod.Put &&
             r.RequestUri.AbsoluteUri == $"{BaseAddress.AbsoluteUri}{relativePath}" &&
             r.Content.ReadAsStringAsync().Result == JsonConvert.SerializeObject(value)
             ), ItExpr.IsAny<CancellationToken>());
        }

        public void SetUpPutAsAsync(HttpStatusCode statusCode)
        {
            _mockHttpMessageHandler
                  .Protected()
                  .Setup<Task<HttpResponseMessage>>("SendAsync",
                      ItExpr.IsAny<HttpRequestMessage>(),
                      ItExpr.IsAny<CancellationToken>())
                  .ReturnsAsync(new HttpResponseMessage(statusCode)).Verifiable("");
        }

        public void SetUpPutAsAsync<T>(T value, HttpStatusCode statusCode)
        {
            _mockHttpMessageHandler
                  .Protected()
                  .Setup<Task<HttpResponseMessage>>("SendAsync",
                      ItExpr.Is<HttpRequestMessage>(r => r.Content.ReadAsStringAsync().Result == JsonConvert.SerializeObject(value)),
                      ItExpr.IsAny<CancellationToken>())
                  .ReturnsAsync(new HttpResponseMessage(statusCode)).Verifiable("");
        }
        public static HttpRequest CreateHttpRequest(object body)
        {
            var mockRequest = new Mock<HttpRequest>();

            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);

            var json = JsonConvert.SerializeObject(body);

            sw.Write(json);
            sw.Flush();

            ms.Position = 0;

            mockRequest.Setup(x => x.Body).Returns(ms);

            return mockRequest.Object;
        }
    }
}
