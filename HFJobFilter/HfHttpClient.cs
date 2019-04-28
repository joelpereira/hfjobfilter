using System.Net.Http;

namespace HFJobFilter
{
    public class HfHttpClient
    {
        public HttpClient Client { get; set; }

        public HfHttpClient(HttpClient httpClient)
        {
            Client = httpClient;
        }
    }
}