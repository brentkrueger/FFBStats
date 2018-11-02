using System.Threading.Tasks;

namespace FFBStats.Business
{
    public interface IYahooWebRequestComposer
    {
        Task<string> GetAsync(string url, string token);
    }
}