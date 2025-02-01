using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingKernels
{
    public class CustomHttpMessageHandler : HttpClientHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // Custom logic before the request
            Console.WriteLine($"Sending request to: {request.RequestUri}");

            request.RequestUri = new Uri("http://localhost:1234/v1/chat/completions");

            // Call base handler
            var response = await base.SendAsync(request, cancellationToken);

            // Custom logic after the request
            Console.WriteLine($"Response status: {response.StatusCode}");

            return response;
        }
    }
}
