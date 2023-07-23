using System.Globalization;
using ErSoftDev.Common.Contracts;
using ErSoftDev.Common.Utilities;
using ErSoftDev.DomainSeedWork;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;


namespace ErSoftDev.Framework.Middlewares
{
    public class CustomStringLocalizedMiddleware

    {
        //نحوه ی ایجاد یک میدل ویر به صورت زیر می باشد

        /// <summary>
        /// جهت دریافت خطای هندل نشده در میدل ویر بعدی
        /// </summary>
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomStringLocalizedMiddleware> _logger;
        private readonly IStringLocalizer<SharedTranslate> _stringLocalizer;


        /// <summary>
        /// سازنده 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        /// <param name="stringLocalizer"></param>
        public CustomStringLocalizedMiddleware(RequestDelegate next, ILogger<CustomStringLocalizedMiddleware> logger, IStringLocalizer<SharedTranslate> stringLocalizer)
        {
            _next = next;
            _logger = logger;
            _stringLocalizer = stringLocalizer;
        }

        /// <summary>
        /// برای پیاده سازی یک میدل ویر باید ساختار به این ترتیب باشد
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {

            httpContext.Request.Headers.TryGetValue("Culture", out var culture);
            if (!culture.ToString().HasValue())
                culture = "fa-IR";
            var numberInfo = CultureInfo.CreateSpecificCulture(culture).NumberFormat;
            var currentCulture = new CultureInfo(culture);
            //{
            //    NumberFormat = numberInfo,
            //    DateTimeFormat =
            //        {
            //            DateSeparator = "/",
            //            ShortDatePattern = "dd/MM/yyyy"
            //        }
            //};

            Thread.CurrentThread.CurrentUICulture = currentCulture;
            Thread.CurrentThread.CurrentCulture = currentCulture;

            //await _next(httpContext);

            #region TestMultiLanguage Middleware
            var originBody = httpContext.Response.Body;
            try
            {
                var memStream = new MemoryStream();
                httpContext.Response.Body = memStream;

                await _next(httpContext).ConfigureAwait(false);

                memStream.Position = 0;
                var responseBody = new StreamReader(memStream).ReadToEnd();


                if (responseBody.HasValue())
                {
                    var f = System.Text.Json.JsonSerializer.Deserialize<ApiResult>(responseBody);
                    httpContext.Response.ContentType = "Application/json";
                    //await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new ApiResult(f.Status,_stringLocalizer[f.Status.ToString()])));
                    //responseBody = JsonConvert.SerializeObject(new ApiResult(f.Status, _stringLocalizer[f.Status.ToString()]));
                }


                //Custom logic to modify response
                //                    responseBody = JsonConvert.SerializeObject(new ApiResult(f.Status, _stringLocalizer[f.Status.ToString()], f.ErrorCode, _stringLocalizer[f.ErrorCode.ToString()]));

                var memoryStreamModified = new MemoryStream();
                var sw = new StreamWriter(memoryStreamModified);
                sw.Write(responseBody);
                sw.Flush();
                memoryStreamModified.Position = 0;

                await memoryStreamModified.CopyToAsync(originBody).ConfigureAwait(false);

            }
            finally
            {
                httpContext.Response.Body = originBody;
            }
            #endregion


        }
        //private Stream ReplaceBody(HttpResponse response)
        //{
        //    var originBody = response.Body;
        //    response.Body = new MemoryStream();
        //    return originBody;
        //}

        //private void ReturnBody(HttpResponse response, Stream originBody)
        //{
        //    response.Body.Seek(0, SeekOrigin.Begin);
        //    response.Body.CopyTo(originBody);
        //    response.Body = originBody;
        //}
    }
}
