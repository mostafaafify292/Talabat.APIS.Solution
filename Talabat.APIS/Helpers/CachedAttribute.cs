using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services.Contract;
using Talabat.Service;

namespace Talabat.APIS.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _expireTimeInSeconds;

        public CachedAttribute(int ExpireTimeInSeconds)
        {
            _expireTimeInSeconds = ExpireTimeInSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var casheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCasheService>();
            var CasheKey = GenerateCasheKeyFromRequest(context.HttpContext.Request);
            var casheResponse =  await casheService.GetCashedResponse(CasheKey);

            if (!string.IsNullOrEmpty(casheResponse))
            {
                var contentResult = new ContentResult()
                {
                    Content = casheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }
            var ExcutedEndPointContext = await next.Invoke(); //Excute EndPoint
            if (ExcutedEndPointContext.Result is OkObjectResult result) 
            {
             await casheService.CasheResponseAsync(CasheKey , result.Value , TimeSpan.FromSeconds(_expireTimeInSeconds));
            }
        }

        private string GenerateCasheKeyFromRequest(HttpRequest request)
        {
            var KeyBuilder = new StringBuilder();
            KeyBuilder.Append(request.Path);        //Api/Products
            foreach (var (Key,Value) in request.Query.OrderBy(q=>q.Key))
            {
                //sort = Name
                //PageIndex = 1
                //PageSize = 5
                KeyBuilder.Append($"|{Key}-{Value}");
                ////Api/Products|sort-Name|PageIndex-1|PageSize-5
            }
            return KeyBuilder.ToString();
        }
    }
}
