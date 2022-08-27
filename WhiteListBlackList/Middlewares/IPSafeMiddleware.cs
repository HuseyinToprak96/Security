using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WhiteListBlackList.Middlewares
{
    public class IPSafeMiddleware
    {
        private readonly RequestDelegate _next;//gelen isteği yakalar
        private readonly IPList _iPList;

        public IPSafeMiddleware(IOptions<IPList> iPList, RequestDelegate next)
        {
            _iPList = iPList.Value;
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext)//middleware de olmazsa olmaz metot ismidir.
        {
            var requestIPAdress = httpContext.Connection.RemoteIpAddress;
            var isWhiteList = _iPList.WhiteList.Where(x => IPAddress.Parse(x) == requestIPAdress).Any();
            if (!isWhiteList)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;//Enumdur. Yasaaklı giriş yapılamaz anlamındadır.
                return; //metotdan çıkarır.
            }
            await _next(httpContext);//eğer varsa sonraki adıma geçer.
        }
    }
}
