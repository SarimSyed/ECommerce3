using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Polly;
using System;
using System.IO;
using System.Reflection;

namespace ECommerce.Api.Search
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
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<ISearchService, SearchService>();

            services.AddScoped<IOrdersService, OrdersService>();
            services.AddScoped<ICustomersService, CustomersService>();
            services.AddHttpClient("ProductsService", config =>
            {
                config.BaseAddress = new Uri(Configuration["Services:Products"]);
            }).AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(5, _ => TimeSpan.FromMilliseconds(500)));
            services.AddHttpClient("OrdersService", config =>
            {
                config.BaseAddress = new Uri(Configuration["Services:Orders"]);
            });
            services.AddHttpClient("CustomersService", config =>
            {
                config.BaseAddress = new Uri(Configuration["Services:Customers"]);
            });
            services.AddControllers();
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My Search API", Version = "v1" });

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                c.IncludeXmlComments(xmlCommentsFullPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "My Search API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapSwagger();

            });
        }
    }
}