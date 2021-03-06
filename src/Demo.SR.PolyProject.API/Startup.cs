using Demo.SR.PolyProject.API.Middlewares;
using Demo.SR.PolyProject.API.Models;
using Demo.SR.PolyProject.API.Services;
using Demo.SR.PolyProject.API.Utilitities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Polly;
using System;

namespace Demo.SR.PolyProject.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           services.AddControllers();

            services.AddHttpContextAccessor();
            services.AddScoped<ICorrelationIdAccessor, CorrelationIdAccessor>();
            services.Configure<WeatherApiConfig>(Configuration.GetSection("WeatherApi"));

            services.AddTransient<DefaultRequestIdMessageHandler>();

            services.AddHttpClient<IWeatherService, WeatherService>(c =>
             {
                 var baseAddress = Configuration.GetSection("WeatherApi:BaseUrl")?.Value;

                 c.BaseAddress = new Uri(baseAddress);
             }).AddHttpMessageHandler<DefaultRequestIdMessageHandler>()
                .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,retryAttempt))))
              .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(4, TimeSpan.FromSeconds(4)));
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo.SR.PolyProject.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo.SR.PolyProject.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<CorrelationIdHeaderEnricherMiddleware>();

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
