namespace Catalog.API.Extensions
{
    public static class WebApplicationExtension
    {
        public static IApplicationBuilder UseExceptionHandling(this WebApplication app)
            => app.Environment.IsDevelopment() ? app.UseDeveloperExceptionPage() : app;

        public static IApplicationBuilder UseApiCors(this WebApplication app)
            => app.UseCors(c =>
            {
                c.WithMethods("GET");
                c.AllowAnyOrigin();
                c.AllowAnyHeader();
            });

        public static IApplicationBuilder UseApiSwagger(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
