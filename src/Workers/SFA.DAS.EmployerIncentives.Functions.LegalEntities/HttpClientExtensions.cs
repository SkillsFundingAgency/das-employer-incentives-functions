using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    internal static class HttpClientExtensions
    {
        internal static StringContent GetStringContent(this object obj) => new StringContent(JsonConvert.SerializeObject(obj), Encoding.Default, "application/json");
    }
}
