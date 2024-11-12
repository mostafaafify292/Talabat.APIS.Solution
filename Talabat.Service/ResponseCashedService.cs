using Microsoft.AspNetCore.ResponseCaching;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Services.Contract;

namespace Talabat.Service
{
    public class ResponseCashedService : IResponseCasheService
    {
        private readonly IDatabase _database;
        public ResponseCashedService(IConnectionMultiplexer Redis)
        {
            _database = Redis.GetDatabase();
        }
        public async Task CasheResponseAsync(string CasheKey, object Response, TimeSpan ExpireTime)
        {
            if (Response is null) return;
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };    
            var SerialezedResponse = JsonSerializer.Serialize(Response , options);
            await _database.StringSetAsync(CasheKey , SerialezedResponse , ExpireTime);
        }

        public async Task<string?> GetCashedResponse(string CasheKey)
        {
            
            var CasheResponse =await _database.StringGetAsync(CasheKey);
            if (CasheResponse.IsNullOrEmpty) return null;
            return CasheResponse;
        }
    }
}
