using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingKernels
{
    public class CustomHttpMessageHandler : HttpClientHandler
    {
        string LocalModelUrl;
        public CustomHttpMessageHandler(string LocalModelUrl)
        {
            this.LocalModelUrl = LocalModelUrl;
        }
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {

            request.RequestUri = new Uri(this.LocalModelUrl);
            // Custom logic before the request
            Console.WriteLine($"Sending request to: {request.RequestUri}");
            // Call base handler
            var response = await base.SendAsync(request, cancellationToken);

            // Custom logic after the request
            Console.WriteLine($"Response status: {response.StatusCode}");

            return response;
        }
    }
}
