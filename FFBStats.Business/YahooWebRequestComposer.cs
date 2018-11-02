using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace FFBStats.Business
{
    public class YahooWebRequestComposer : IYahooWebRequestComposer
    {
        public async Task<string> GetAsync(string uri, string token)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Headers.Add("Authorization", $"Bearer {token}");

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}