using System.Globalization;
using ErSoftDev.Framework.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Hosting;

namespace ErSoftDev.Framework.Configuration
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseHstsNotInDevelopment(this IApplicationBuilder applicationBuilder, IWebHostEnvironment hostingEnvironment)
        {
            if (!hostingEnvironment.IsDevelopment())
                applicationBuilder.UseHsts();
        }

        public static void UseCustomRequestLocalization(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("fa-IR"),
                SupportedCultures = new[]
                {
                    new CultureInfo("fa-IR"),
                    new CultureInfo("en-US")
                },

            });
        }
        public static void UseCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }

        public static void UseCustomStringLocalizer(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomStringLocalizedMiddleware>();
        }

        public static void UseRateLimitationMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<RateLimitationMiddleware>();
        }

        public static void UseCustomStaticFile(this IApplicationBuilder app)
        {
            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".log"] = "text/html";
            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
            });
        }
    }
}
