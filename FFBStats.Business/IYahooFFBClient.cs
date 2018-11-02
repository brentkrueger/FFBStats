using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FFBStats.Business
{
    public interface IYahooFFBClient
    {
        Task<string> GetLeagues(string token);
        Task<string> GetGameIds(string token);
    }
}