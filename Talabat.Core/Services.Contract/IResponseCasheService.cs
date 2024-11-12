using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Services.Contract
{
    public interface IResponseCasheService
    {
        //Cashe Data
        public Task CasheResponseAsync(string CasheKey, object Response, TimeSpan ExpireTime);
        //Get Cashed Data
        public Task<string?> GetCashedResponse(string CasheKey);
    }
}
